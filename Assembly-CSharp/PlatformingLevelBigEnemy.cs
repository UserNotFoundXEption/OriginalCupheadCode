using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class PlatformingLevelBigEnemy : PlatformingLevelShootingEnemy
{
	// Token: 0x06002B55 RID: 11093 RVA: 0x000246A5 File Offset: 0x000228A5
	public override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.StartLockCheck), 1);
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000246C1 File Offset: 0x000228C1
	public void StartLockCheck()
	{
		base.StartCoroutine(this.camera_locking_cr());
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000D5B14 File Offset: 0x000D3D14
	public IEnumerator camera_locking_cr()
	{
		while (!this.isDead)
		{
			if (!this.bigEnemyCameraLock)
			{
				this.dist = PlayerManager.Center.x - base.transform.position.x;
				if (this.dist > -this.LockDistance)
				{
					this.bigEnemyCameraLock = true;
					CupheadLevelCamera.Current.LockCamera(true);
					this.OnLock();
				}
			}
			else if (this.bigEnemyCameraLock)
			{
				this.dist = PlayerManager.Center.x - this.passDistance.transform.position.x;
				if (this.dist > 0f)
				{
					this.bigEnemyCameraLock = false;
					CupheadLevelCamera.Current.LockCamera(false);
					this.OnPass();
					break;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000246D0 File Offset: 0x000228D0
	public virtual void OnPass()
	{
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x000246D2 File Offset: 0x000228D2
	public virtual void OnLock()
	{
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000246D4 File Offset: 0x000228D4
	public override void Die()
	{
		this.bigEnemyCameraLock = false;
		CupheadLevelCamera.Current.LockCamera(false);
		base.Die();
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000D5B30 File Offset: 0x000D3D30
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(this.passDistance.transform.position, new Vector3(this.passDistance.transform.position.x, this.passDistance.transform.position.y - 1000f));
	}

	// Token: 0x040023EC RID: 9196
	[SerializeField]
	public Transform passDistance;

	// Token: 0x040023ED RID: 9197
	public float LockDistance = 500f;

	// Token: 0x040023EE RID: 9198
	public bool bigEnemyCameraLock;

	// Token: 0x040023EF RID: 9199
	public bool isDead;

	// Token: 0x040023F0 RID: 9200
	public float dist;
}
