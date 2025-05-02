using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class ShmupTutorialLevel : Level
{
	// Token: 0x060003F9 RID: 1017 RVA: 0x00068C4C File Offset: 0x00066E4C
	public override void PartialInit()
	{
		this.properties = LevelProperties.ShmupTutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x00004E97 File Offset: 0x00003097
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ShmupTutorial;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x00004E9E File Offset: 0x0000309E
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_shmup_tutorial;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x00004EA2 File Offset: 0x000030A2
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00004EAA File Offset: 0x000030AA
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00004EB2 File Offset: 0x000030B2
	public override void Start()
	{
		base.Start();
		this.canvasAnimator.SetTrigger("StartAnimation");
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00004ECA File Offset: 0x000030CA
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.shmuptutorialPattern_cr());
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00068CE4 File Offset: 0x00066EE4
	public IEnumerator shmuptutorialPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00068D00 File Offset: 0x00066F00
	public IEnumerator shmupTutorialStartAnimation_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.waitForAnimationTime);
		this.canvasAnimator.SetTrigger("StartAnimation");
		yield break;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00068D1C File Offset: 0x00066F1C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ShmupTutorial.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.ShmupTutorial.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040002F2 RID: 754
	public LevelProperties.ShmupTutorial properties;

	// Token: 0x040002F3 RID: 755
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x040002F5 RID: 757
	public Animator canvasAnimator;

	// Token: 0x040002F6 RID: 758
	public float waitForAnimationTime;
}
