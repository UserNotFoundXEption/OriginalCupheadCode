using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000105 RID: 261
[RequireComponent(typeof(CanvasGroup))]
public class LevelReviveCardGUI : AbstractMonoBehaviour
{
	// Token: 0x06000C3F RID: 3135 RVA: 0x00082C44 File Offset: 0x00080E44
	public override void Awake()
	{
		base.Awake();
		LevelReviveCardGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		base.gameObject.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.helpCanvasGroup.alpha = 0f;
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
		this.state = LevelReviveCardGUI.State.Init;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0000ABC5 File Offset: 0x00008DC5
	public void OnDestroy()
	{
		LevelReviveCardGUI.Current = null;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0000ABCD File Offset: 0x00008DCD
	public void Update()
	{
		if (this.state != LevelReviveCardGUI.State.Ready)
		{
			return;
		}
		if (PlayerManager.GetPlayerInput(this.deadPlayer).GetButtonDown(13))
		{
			this.RevivePlayer();
			this.state = LevelReviveCardGUI.State.Exiting;
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00082CA8 File Offset: 0x00080EA8
	public void RevivePlayer()
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].HP = 3;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].BonusHP = 0;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].SuperCharge = 0f;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].tokenCount--;
		this.playerOneCard.SetTokenCount();
		this.playerTwoCard.SetTokenCount();
		SceneLoader.ContinueTowerOfPower();
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00082D24 File Offset: 0x00080F24
	public void In()
	{
		base.gameObject.SetActive(true);
		this.playerOneCard.Init(PlayerId.PlayerOne);
		this.playerTwoCard.Init(PlayerId.PlayerTwo);
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP == 0)
		{
			this.deadPlayer = PlayerId.PlayerOne;
		}
		else
		{
			this.deadPlayer = PlayerId.PlayerTwo;
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00082D88 File Offset: 0x00080F88
	public IEnumerator in_cr()
	{
		AudioManager.Play("level_menu_card_up");
		yield return base.TweenValue(0f, 1f, 0.05f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetAlpha));
		yield return new WaitForSeconds(1f);
		AudioManager.Play("player_die_vinylscratch");
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4f);
		if (!Level.IsChessBoss)
		{
			AudioManager.ChangeBGMPitch(0.7f, 6f);
		}
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValue));
		yield return null;
		this.state = LevelReviveCardGUI.State.Ready;
		yield break;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0000AC00 File Offset: 0x00008E00
	public void SetAlpha(float value)
	{
		this.canvasGroup.alpha = value;
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00082DA4 File Offset: 0x00080FA4
	public void SetCardValue(float value)
	{
		this.playerOneCard.SetAlpha(value);
		this.playerTwoCard.SetAlpha(value);
		this.helpCanvasGroup.alpha = value;
		this.playerOneCard.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(15f, 4f, value)));
		this.playerTwoCard.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(-15f, -4f, value)));
	}

	// Token: 0x040009BB RID: 2491
	public static LevelReviveCardGUI Current;

	// Token: 0x040009BC RID: 2492
	[Space(10f)]
	[SerializeField]
	public TowerOfPowerContinueCardGUI playerOneCard;

	// Token: 0x040009BD RID: 2493
	[Space(10f)]
	[SerializeField]
	public TowerOfPowerContinueCardGUI playerTwoCard;

	// Token: 0x040009BE RID: 2494
	[Space(10f)]
	[SerializeField]
	public CanvasGroup helpCanvasGroup;

	// Token: 0x040009BF RID: 2495
	public LevelReviveCardGUI.State state;

	// Token: 0x040009C0 RID: 2496
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040009C1 RID: 2497
	public CanvasGroup canvasGroup;

	// Token: 0x040009C2 RID: 2498
	public PlayerId deadPlayer;

	// Token: 0x02000986 RID: 2438
	public enum State
	{
		// Token: 0x0400472B RID: 18219
		Init,
		// Token: 0x0400472C RID: 18220
		Ready,
		// Token: 0x0400472D RID: 18221
		Exiting
	}
}
