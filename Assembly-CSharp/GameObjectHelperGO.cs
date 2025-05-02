using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class GameObjectHelperGO : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000558 RID: 1368 RVA: 0x0006C740 File Offset: 0x0006A940
	// (remove) Token: 0x06000559 RID: 1369 RVA: 0x0006C778 File Offset: 0x0006A978
	public event GameObjectHelperGO.OnUpdateHandler onUpdate;

	// Token: 0x0600055A RID: 1370 RVA: 0x00005BD8 File Offset: 0x00003DD8
	public void Update()
	{
		if (this.onUpdate != null)
		{
			this.onUpdate();
		}
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x0600055B RID: 1371 RVA: 0x0006C7B0 File Offset: 0x0006A9B0
	// (remove) Token: 0x0600055C RID: 1372 RVA: 0x0006C7E8 File Offset: 0x0006A9E8
	public event GameObjectHelperGO.OnFixedUpdateHandler onFixedUpdate;

	// Token: 0x0600055D RID: 1373 RVA: 0x00005BF0 File Offset: 0x00003DF0
	public void FixedUpdate()
	{
		if (this.onFixedUpdate != null)
		{
			this.onFixedUpdate();
		}
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600055E RID: 1374 RVA: 0x0006C820 File Offset: 0x0006AA20
	// (remove) Token: 0x0600055F RID: 1375 RVA: 0x0006C858 File Offset: 0x0006AA58
	public event GameObjectHelperGO.OnLateUpdateHandler onLateUpdate;

	// Token: 0x06000560 RID: 1376 RVA: 0x00005C08 File Offset: 0x00003E08
	public void LateUpdate()
	{
		if (this.onLateUpdate != null)
		{
			this.onLateUpdate();
		}
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000561 RID: 1377 RVA: 0x0006C890 File Offset: 0x0006AA90
	// (remove) Token: 0x06000562 RID: 1378 RVA: 0x0006C8C8 File Offset: 0x0006AAC8
	public event GameObjectHelperGO.OnDrawGizmosHandler onDrawGizmos;

	// Token: 0x06000563 RID: 1379 RVA: 0x00005C20 File Offset: 0x00003E20
	public void OnDrawGizmos()
	{
		if (this.onDrawGizmos != null)
		{
			this.onDrawGizmos();
		}
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000564 RID: 1380 RVA: 0x0006C900 File Offset: 0x0006AB00
	// (remove) Token: 0x06000565 RID: 1381 RVA: 0x0006C938 File Offset: 0x0006AB38
	public event GameObjectHelperGO.OnDestroyHandler onDestroy;

	// Token: 0x06000566 RID: 1382 RVA: 0x00005C38 File Offset: 0x00003E38
	public void OnDestroy()
	{
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
		this.clear();
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00005C56 File Offset: 0x00003E56
	public void clear()
	{
		this.onUpdate = null;
		this.onFixedUpdate = null;
		this.onLateUpdate = null;
		this.onDrawGizmos = null;
	}

	// Token: 0x020008AF RID: 2223
	// (Invoke) Token: 0x0600521E RID: 21022
	public delegate void OnUpdateHandler();

	// Token: 0x020008B0 RID: 2224
	// (Invoke) Token: 0x06005222 RID: 21026
	public delegate void OnFixedUpdateHandler();

	// Token: 0x020008B1 RID: 2225
	// (Invoke) Token: 0x06005226 RID: 21030
	public delegate void OnLateUpdateHandler();

	// Token: 0x020008B2 RID: 2226
	// (Invoke) Token: 0x0600522A RID: 21034
	public delegate void OnDrawGizmosHandler();

	// Token: 0x020008B3 RID: 2227
	// (Invoke) Token: 0x0600522E RID: 21038
	public delegate void OnDestroyHandler();
}
