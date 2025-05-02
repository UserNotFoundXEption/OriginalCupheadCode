using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class TestLevel : Level
{
	// Token: 0x0600042C RID: 1068 RVA: 0x000693CC File Offset: 0x000675CC
	public override void PartialInit()
	{
		this.properties = LevelProperties.Test.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600042D RID: 1069 RVA: 0x00005021 File Offset: 0x00003221
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Test;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x00005024 File Offset: 0x00003224
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_test;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x00005028 File Offset: 0x00003228
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x00005030 File Offset: 0x00003230
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00005038 File Offset: 0x00003238
	public override void Start()
	{
		base.Start();
		this.jared.LevelInit(this.properties);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00069464 File Offset: 0x00067664
	public override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(32))
		{
			LevelPlayerController player = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerOne);
			player.animationController.SetColorOverTime(Color.blue, 1f);
		}
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00005051 File Offset: 0x00003251
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.testPattern_cr());
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00005060 File Offset: 0x00003260
	public override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x000694A0 File Offset: 0x000676A0
	public IEnumerator testPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000694BC File Offset: 0x000676BC
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Test.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000315 RID: 789
	public LevelProperties.Test properties;

	// Token: 0x04000316 RID: 790
	[SerializeField]
	public TestLevelFlyingJared jared;

	// Token: 0x04000317 RID: 791
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000318 RID: 792
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x02000857 RID: 2135
	[Serializable]
	public class Prefabs
	{
	}
}
