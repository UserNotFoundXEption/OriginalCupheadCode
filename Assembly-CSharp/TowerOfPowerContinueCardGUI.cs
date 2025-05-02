using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
[RequireComponent(typeof(CanvasGroup))]
public class TowerOfPowerContinueCardGUI : AbstractMonoBehaviour
{
	// Token: 0x06000C50 RID: 3152 RVA: 0x00083048 File Offset: 0x00081248
	public override void Awake()
	{
		base.Awake();
		foreach (UIImageAnimationLoop uiimageAnimationLoop in this.CardMugmanAnimation)
		{
			uiimageAnimationLoop.gameObject.SetActive(false);
		}
		foreach (UIImageAnimationLoop uiimageAnimationLoop2 in this.CardCupheadAnimation)
		{
			uiimageAnimationLoop2.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00083100 File Offset: 0x00081300
	public void Init(PlayerId playerId)
	{
		this.canvas.alpha = 0f;
		this.SetPlayer(playerId);
		this.player = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId];
		this.SetTitlePlayersName();
		this.SetTokenCount();
		this.Continue.gameObject.SetActive(this.player.HP == 0);
		this.CountDown_text.gameObject.SetActive(this.player.HP == 0);
		this.SetAnimation();
		if (this.player.HP == 0)
		{
			base.StartCoroutine(this.update_countdown_cr());
		}
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0008319C File Offset: 0x0008139C
	public void SetAnimation()
	{
		if (this.yourPlayerIsMugman)
		{
			this.CardMugmanAnimation[(this.player.HP != 0) ? 0 : 1].gameObject.SetActive(true);
		}
		else
		{
			this.CardCupheadAnimation[(this.player.HP != 0) ? 0 : 1].gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0000ACBC File Offset: 0x00008EBC
	public void SetPlayer(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerOne)
		{
			this.yourPlayerIsMugman = PlayerManager.player1IsMugman;
		}
		else
		{
			this.yourPlayerIsMugman = !PlayerManager.player1IsMugman;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00083214 File Offset: 0x00081414
	public void SetTitlePlayersName()
	{
		if (this.yourPlayerIsMugman)
		{
		}
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00083240 File Offset: 0x00081440
	public void SetTokenCount()
	{
		int tokenCount = this.player.tokenCount;
		this.TokenLeft_text.text = "Token: " + tokenCount;
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00083274 File Offset: 0x00081474
	public IEnumerator update_countdown_cr()
	{
		while (this.countDown > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
			this.countDown--;
			this.UpdateCountDownText();
		}
		yield return null;
		SceneLoader.ContinueTowerOfPower();
		yield break;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0000ACE2 File Offset: 0x00008EE2
	public void UpdateCountDownText()
	{
		this.CountDown_text.text = this.countDown.ToString();
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0000AD00 File Offset: 0x00008F00
	public void OnDestroy()
	{
		this.Continue = null;
		this.CountDown_text = null;
		this.CardCupheadAnimation.Clear();
		this.CardMugmanAnimation.Clear();
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0000AD26 File Offset: 0x00008F26
	public void SetAlpha(float value)
	{
		this.canvas.alpha = value;
	}

	// Token: 0x040009CC RID: 2508
	[SerializeField]
	public SpriteRenderer PlayerName;

	// Token: 0x040009CD RID: 2509
	[SerializeField]
	public Sprite CupheadNameData;

	// Token: 0x040009CE RID: 2510
	[SerializeField]
	public Sprite CupmanNameData;

	// Token: 0x040009CF RID: 2511
	[Space(10f)]
	[SerializeField]
	public Text TokenLeft_text;

	// Token: 0x040009D0 RID: 2512
	[SerializeField]
	public Text Continue;

	// Token: 0x040009D1 RID: 2513
	[SerializeField]
	public Text CountDown_text;

	// Token: 0x040009D2 RID: 2514
	[SerializeField]
	public List<UIImageAnimationLoop> CardCupheadAnimation = new List<UIImageAnimationLoop>();

	// Token: 0x040009D3 RID: 2515
	[SerializeField]
	public List<UIImageAnimationLoop> CardMugmanAnimation = new List<UIImageAnimationLoop>();

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	public CanvasGroup canvas;

	// Token: 0x040009D5 RID: 2517
	public bool yourPlayerIsMugman;

	// Token: 0x040009D6 RID: 2518
	public int countDown = 10;

	// Token: 0x040009D7 RID: 2519
	public PlayersStatsBossesHub player;
}
