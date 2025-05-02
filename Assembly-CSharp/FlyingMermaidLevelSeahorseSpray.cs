using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class FlyingMermaidLevelSeahorseSpray : AbstractPausableComponent
{
	// Token: 0x06001DBF RID: 7615 RVA: 0x000B186C File Offset: 0x000AFA6C
	public void Update()
	{
		if (this.ended)
		{
			return;
		}
		foreach (PlanePlayerMotor planePlayerMotor in this.playerInfos.Keys)
		{
			if (!(planePlayerMotor == null))
			{
				if (Mathf.Abs(planePlayerMotor.transform.position.x - base.transform.position.x) < this.width / 2f && planePlayerMotor.player.center.y < this.topRoot.position.y)
				{
					this.playerInfos[planePlayerMotor].force.enabled = true;
					this.playerInfos[planePlayerMotor].timeSinceFx += CupheadTime.Delta;
					if (this.playerInfos[planePlayerMotor].timeSinceFx >= this.playerInfos[planePlayerMotor].fxWaitTime)
					{
						Effect effect = this.effectPrefab.Create(planePlayerMotor.player.center + new Vector3(0f, -40f));
						int num = (this.playerInfos[planePlayerMotor].lastFxVariant + Random.Range(0, 3)) % 3;
						effect.animator.SetInteger("Effect", num);
						this.playerInfos[planePlayerMotor].lastFxVariant = num;
						this.playerInfos[planePlayerMotor].fxWaitTime = Random.Range(0.125f, 0.17f);
						this.playerInfos[planePlayerMotor].timeSinceFx = 0f;
					}
				}
				else
				{
					this.playerInfos[planePlayerMotor].force.enabled = false;
					this.playerInfos[planePlayerMotor].fxWaitTime = 0f;
				}
			}
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000B1A90 File Offset: 0x000AFC90
	public void Init(LevelProperties.FlyingMermaid.Seahorse properties)
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				PlanePlayerMotor.Force force = new PlanePlayerMotor.Force(new Vector2(0f, properties.waterForce), false);
				planePlayerController.motor.AddForce(force);
				FlyingMermaidLevelSeahorseSpray.PlayerInfo playerInfo = new FlyingMermaidLevelSeahorseSpray.PlayerInfo();
				playerInfo.force = force;
				this.playerInfos[planePlayerController.motor] = playerInfo;
			}
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000B1B3C File Offset: 0x000AFD3C
	public void End()
	{
		this.ended = true;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				planePlayerController.motor.RemoveForce(this.playerInfos[planePlayerController.motor].force);
			}
		}
	}

	// Token: 0x04001863 RID: 6243
	public float width = 20f;

	// Token: 0x04001864 RID: 6244
	public Dictionary<PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo> playerInfos = new Dictionary<PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo>();

	// Token: 0x04001865 RID: 6245
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x04001866 RID: 6246
	[SerializeField]
	public Transform topRoot;

	// Token: 0x04001867 RID: 6247
	public bool ended;

	// Token: 0x02000D53 RID: 3411
	public class PlayerInfo
	{
		// Token: 0x04006091 RID: 24721
		public PlanePlayerMotor.Force force;

		// Token: 0x04006092 RID: 24722
		public float timeSinceFx;

		// Token: 0x04006093 RID: 24723
		public float fxWaitTime;

		// Token: 0x04006094 RID: 24724
		public int lastFxVariant = -1;
	}
}
