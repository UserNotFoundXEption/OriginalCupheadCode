using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class PlatformingLevelAutoscrollObject : AbstractCollidableObject
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06002F84 RID: 12164 RVA: 0x000279AC File Offset: 0x00025BAC
	// (set) Token: 0x06002F85 RID: 12165 RVA: 0x000279B4 File Offset: 0x00025BB4
	public bool isMoving { get; set; }

	// Token: 0x06002F86 RID: 12166 RVA: 0x000279BD File Offset: 0x00025BBD
	public override void Awake()
	{
		base.Awake();
		this.isMoving = false;
		this.isLocked = false;
	}

	// Token: 0x06002F87 RID: 12167 RVA: 0x000279D3 File Offset: 0x00025BD3
	public virtual void Start()
	{
		if (this.checkToLock)
		{
			base.StartCoroutine(this.check_to_lock_cr());
		}
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x000E1DD0 File Offset: 0x000DFFD0
	public virtual void Update()
	{
		if (this.isMoving && base.transform.position.x > this.endPosition.transform.position.x)
		{
			this.StartEndingAutoscroll();
			this.isMoving = false;
		}
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x000E1E28 File Offset: 0x000E0028
	public virtual IEnumerator check_to_lock_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		float dist = PlayerManager.Center.x - base.transform.position.x;
		while (dist < -this.lockDistance)
		{
			dist = PlayerManager.Center.x - base.transform.position.x;
			yield return null;
		}
		CupheadLevelCamera.Current.LockCamera(true);
		this.isLocked = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002F8A RID: 12170 RVA: 0x000279ED File Offset: 0x00025BED
	public virtual void StartAutoscroll()
	{
		this.isMoving = true;
		this.isLocked = false;
		CupheadLevelCamera.Current.LockCamera(false);
		CupheadLevelCamera.Current.SetAutoScroll(true);
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x00027A13 File Offset: 0x00025C13
	public virtual void StartEndingAutoscroll()
	{
		base.StartCoroutine(this.end_autoscroll());
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x00027A22 File Offset: 0x00025C22
	public virtual void EndAutoscroll()
	{
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x000E1E44 File Offset: 0x000E0044
	public IEnumerator end_autoscroll()
	{
		CupheadLevelCamera.Current.SetAutoScroll(false);
		this.EndAutoscroll();
		yield return null;
		yield break;
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x000E1E60 File Offset: 0x000E0060
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.endPosition.transform != null)
		{
			Gizmos.DrawLine(new Vector3(this.endPosition.transform.position.x, this.endPosition.transform.position.y + 1500f), new Vector3(this.endPosition.transform.position.x, this.endPosition.transform.position.y - 1500f));
		}
	}

	// Token: 0x04002767 RID: 10087
	[SerializeField]
	public Transform endPosition;

	// Token: 0x04002768 RID: 10088
	public float lockDistance = 600f;

	// Token: 0x04002769 RID: 10089
	public float endDelay = 1f;

	// Token: 0x0400276B RID: 10091
	public bool checkToLock;

	// Token: 0x0400276C RID: 10092
	public bool isLocked;
}
