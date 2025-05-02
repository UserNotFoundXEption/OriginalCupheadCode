using System;
using System.Collections;

// Token: 0x02000496 RID: 1174
public class MapNPCGraveyardGhost : MapDialogueInteraction
{
	// Token: 0x0600312D RID: 12589 RVA: 0x000E8D74 File Offset: 0x000E6F74
	public override void Start()
	{
		base.Start();
		if (CharmCurse.IsMaxLevel(PlayerId.PlayerOne) || CharmCurse.IsMaxLevel(PlayerId.PlayerTwo))
		{
			Dialoguer.SetGlobalFloat(41, 2f);
		}
		else if (CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 || CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1)
		{
			Dialoguer.SetGlobalFloat(41, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(41, 0f);
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600312E RID: 12590 RVA: 0x00028F26 File Offset: 0x00027126
	public override bool ChangesDepth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x00028F29 File Offset: 0x00027129
	public void TalkAfterPlayerGotCharm()
	{
		base.StartCoroutine(this.got_charm_notification_cr());
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x000E8DE4 File Offset: 0x000E6FE4
	public IEnumerator got_charm_notification_cr()
	{
		Dialoguer.SetGlobalFloat(41, 1f);
		while (Map.Current.players[0].state == MapPlayerController.State.Stationary || (Map.Current.players[1] != null && Map.Current.players[1].state == MapPlayerController.State.Stationary))
		{
			yield return null;
		}
		this.StartSpeechBubble();
		while (this.currentlySpeaking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x000E8E00 File Offset: 0x000E7000
	public override void Update()
	{
		base.Update();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < this.idleNormalizedTime)
			{
				this.idleCycleCount++;
				if (this.idleCycleCount % 3 == 0)
				{
					base.animator.SetTrigger("Puff");
				}
				if (this.idleCycleCount % 7 == 3)
				{
					base.animator.SetTrigger("BlinkOnce");
				}
				if (this.idleCycleCount % 7 == 6)
				{
					base.animator.SetTrigger("BlinkTwice");
				}
			}
			this.idleNormalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		}
	}

	// Token: 0x04002893 RID: 10387
	public const int GRAVEYARD_GHOST_STATE_INDEX = 41;

	// Token: 0x04002894 RID: 10388
	public float idleNormalizedTime;

	// Token: 0x04002895 RID: 10389
	public int idleCycleCount;

	// Token: 0x04002896 RID: 10390
	public int nextPuffMultiplier = 4;
}
