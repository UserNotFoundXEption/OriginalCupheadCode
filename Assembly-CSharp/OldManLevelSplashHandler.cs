using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class OldManLevelSplashHandler : AbstractPausableComponent
{
	// Token: 0x06002145 RID: 8517 RVA: 0x000BA0D8 File Offset: 0x000B82D8
	public void SplashOut(float posX)
	{
		this.splashOut.Create(new Vector3(posX, base.transform.position.y));
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x000BA10C File Offset: 0x000B830C
	public void SplashIn(float posX)
	{
		this.splashIn.Create(new Vector3(posX, base.transform.position.y));
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000BA140 File Offset: 0x000B8340
	public void Update()
	{
		Dictionary<int, AbstractPlayerController>.ValueCollection allPlayers = PlayerManager.GetAllPlayers();
		for (int i = 0; i < 2; i++)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer((PlayerId)i);
			if (player == null || player.IsDead)
			{
				this.lastKnownPlayerPos[i] = Vector3.zero;
			}
			else
			{
				if (this.lastKnownPlayerPos[i].y < base.transform.position.y)
				{
					if (player.transform.position.y > base.transform.position.y)
					{
					}
				}
				else if (player.transform.position.y <= base.transform.position.y)
				{
					this.SplashIn(player.transform.position.x);
					this.SFX_PlayerSplashIn();
				}
				this.lastKnownPlayerPos[i] = player.transform.position;
			}
		}
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x0001C68B File Offset: 0x0001A88B
	public void SFX_PlayerSplashIn()
	{
		AudioManager.Play("sfx_dlc_omm_p3_stomachacid_splash");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_splash");
	}

	// Token: 0x04001B69 RID: 7017
	[SerializeField]
	public Effect splashIn;

	// Token: 0x04001B6A RID: 7018
	[SerializeField]
	public Effect splashOut;

	// Token: 0x04001B6B RID: 7019
	public Vector3[] lastKnownPlayerPos = new Vector3[]
	{
		Vector3.zero,
		Vector3.zero
	};
}
