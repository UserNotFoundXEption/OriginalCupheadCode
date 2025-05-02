using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class ThreadedLoader
{
	// Token: 0x060006C0 RID: 1728 RVA: 0x00006CFE File Offset: 0x00004EFE
	public ThreadedLoader(MonoBehaviour coroutineParent)
	{
		this.coroutineParent = coroutineParent;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x000705EC File Offset: 0x0006E7EC
	public ThreadedLoader.LoadOperation LoadAssetBundle(string path)
	{
		ThreadedLoader.LoadOperation loadOperation = new ThreadedLoader.LoadOperation();
		loadOperation.path = path;
		if (this.busy)
		{
			this.operationQueue.Add(loadOperation);
			return loadOperation;
		}
		this.startLoad(loadOperation);
		return loadOperation;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00070628 File Offset: 0x0006E828
	public void startLoad(ThreadedLoader.LoadOperation operation)
	{
		this.busy = true;
		this.threadBusy = true;
		this.coroutineParent.StartCoroutine(this.threadWait_cr(operation));
		Thread thread = new Thread(delegate
		{
			this.loadData(operation);
		});
		thread.Start();
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00070688 File Offset: 0x0006E888
	public IEnumerator threadWait_cr(ThreadedLoader.LoadOperation operation)
	{
		while (this.threadBusy)
		{
			yield return null;
		}
		AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(operation.data);
		yield return request;
		operation.SetComplete(request.assetBundle);
		this.busy = false;
		if (this.operationQueue.Count > 0)
		{
			ThreadedLoader.LoadOperation operation2 = this.operationQueue[0];
			this.operationQueue.RemoveAt(0);
			this.startLoad(operation2);
		}
		yield break;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00006D18 File Offset: 0x00004F18
	public void loadData(ThreadedLoader.LoadOperation operation)
	{
		operation.data = File.ReadAllBytes(operation.path);
		this.threadBusy = false;
	}

	// Token: 0x040004FF RID: 1279
	public MonoBehaviour coroutineParent;

	// Token: 0x04000500 RID: 1280
	public List<ThreadedLoader.LoadOperation> operationQueue = new List<ThreadedLoader.LoadOperation>();

	// Token: 0x04000501 RID: 1281
	public bool busy;

	// Token: 0x04000502 RID: 1282
	public bool threadBusy;

	// Token: 0x020008D5 RID: 2261
	public class LoadOperation : DLCManager.AssetBundleLoadWaitInstruction
	{
		// Token: 0x06005289 RID: 21129 RVA: 0x0003F1B1 File Offset: 0x0003D3B1
		public void SetComplete(AssetBundle bundle)
		{
			this.complete = true;
			base.assetBundle = bundle;
		}

		// Token: 0x04004398 RID: 17304
		public string path;

		// Token: 0x04004399 RID: 17305
		public byte[] data;
	}
}
