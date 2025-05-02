using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class DicePalaceChipsLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000198 RID: 408 RVA: 0x00061E68 File Offset: 0x00060068
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceChips.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000199 RID: 409 RVA: 0x00003EBA File Offset: 0x000020BA
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceChips;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600019A RID: 410 RVA: 0x00003EC1 File Offset: 0x000020C1
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceChips;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600019B RID: 411 RVA: 0x00003EC8 File Offset: 0x000020C8
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_chips;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600019C RID: 412 RVA: 0x00003ECC File Offset: 0x000020CC
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600019D RID: 413 RVA: 0x00003ED4 File Offset: 0x000020D4
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00003EDC File Offset: 0x000020DC
	public override void Start()
	{
		base.Start();
		this.chips.LevelInit(this.properties);
		base.StartCoroutine(CupheadLevelCamera.Current.rotate_camera());
		base.StartCoroutine(this.rotate_background_cr());
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00003F13 File Offset: 0x00002113
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacechipsPattern_cr());
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00003F22 File Offset: 0x00002122
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00061F00 File Offset: 0x00060100
	public IEnumerator dicepalacechipsPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00061F1C File Offset: 0x0006011C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceChips.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceChips.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00061F38 File Offset: 0x00060138
	public IEnumerator rotate_background_cr()
	{
		float time = 1.5f;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			this.background.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * 1f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000147 RID: 327
	public LevelProperties.DicePalaceChips properties;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	public GameObject background;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	public DicePalaceChipsLevelChips chips;

	// Token: 0x0400014A RID: 330
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	public string _bossQuote;
}
