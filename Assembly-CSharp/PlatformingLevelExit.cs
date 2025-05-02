using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class PlatformingLevelExit : AbstractCollidableObject
{
	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06002FA9 RID: 12201 RVA: 0x000E264C File Offset: 0x000E084C
	// (remove) Token: 0x06002FAA RID: 12202 RVA: 0x000E2680 File Offset: 0x000E0880
	public static event Action OnWinStartEvent;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06002FAB RID: 12203 RVA: 0x000E26B4 File Offset: 0x000E08B4
	// (remove) Token: 0x06002FAC RID: 12204 RVA: 0x000E26E8 File Offset: 0x000E08E8
	public static event Action OnWinCompleteEvent;

	// Token: 0x06002FAD RID: 12205 RVA: 0x000E271C File Offset: 0x000E091C
	public void FixedUpdate()
	{
		if (this._activated)
		{
			if (!this._exited)
			{
				for (int i = 0; i < 2; i++)
				{
					AbstractPlayerController player = PlayerManager.GetPlayer((i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne);
					if (!(player == null))
					{
						if (player.center.x > base.transform.position.x + this._exitDistance)
						{
							this._exited = true;
							if (PlatformingLevelExit.OnWinCompleteEvent != null)
							{
								PlatformingLevelExit.OnWinCompleteEvent();
								PlatformingLevelExit.OnWinCompleteEvent = null;
							}
							break;
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < 2; j++)
			{
				AbstractPlayerController player2 = PlayerManager.GetPlayer((j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne);
				if (!(player2 == null) && !player2.IsDead)
				{
					if (player2.center.x > base.transform.position.x)
					{
						this._activated = true;
						if (PlatformingLevelExit.OnWinStartEvent != null)
						{
							PlatformingLevelExit.OnWinStartEvent();
							PlatformingLevelExit.OnWinStartEvent = null;
						}
						PlatformingLevelEnd.Win();
						base.StartCoroutine(this.on_win_complete_cr());
						break;
					}
				}
			}
		}
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x00027B66 File Offset: 0x00025D66
	public override void OnDestroy()
	{
		base.OnDestroy();
		PlatformingLevelExit.OnWinStartEvent = null;
		PlatformingLevelExit.OnWinCompleteEvent = null;
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x000E2878 File Offset: 0x000E0A78
	public IEnumerator on_win_complete_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.onCompleteWaitTime);
		if (PlatformingLevelExit.OnWinCompleteEvent != null)
		{
			PlatformingLevelExit.OnWinCompleteEvent();
			PlatformingLevelExit.OnWinCompleteEvent = null;
		}
		yield break;
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x00027B7A File Offset: 0x00025D7A
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06002FB1 RID: 12209 RVA: 0x00027B8D File Offset: 0x00025D8D
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000E2894 File Offset: 0x000E0A94
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Vector3 vector = base.baseTransform.position + new Vector3(this._exitDistance, 0f, 0f);
		Gizmos.DrawLine(base.baseTransform.position, vector);
		Gizmos.DrawLine(vector + new Vector3(0f, -5000f, 0f), vector + new Vector3(0f, 5000f, 0f));
		Gizmos.color = Color.white;
	}

	// Token: 0x04002783 RID: 10115
	[SerializeField]
	[Range(200f, 1500f)]
	public float _exitDistance = 500f;

	// Token: 0x04002784 RID: 10116
	[SerializeField]
	public float onCompleteWaitTime = 2f;

	// Token: 0x04002785 RID: 10117
	public bool _activated;

	// Token: 0x04002786 RID: 10118
	public bool _exited;
}
