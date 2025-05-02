using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class DicePalaceBoozeLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000180 RID: 384 RVA: 0x00061BC4 File Offset: 0x0005FDC4
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceBooze.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000181 RID: 385 RVA: 0x00003DFD File Offset: 0x00001FFD
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceBooze;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000182 RID: 386 RVA: 0x00003E04 File Offset: 0x00002004
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceBooze;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000183 RID: 387 RVA: 0x00003E0B File Offset: 0x0000200B
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_booze;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000184 RID: 388 RVA: 0x00003E0F File Offset: 0x0000200F
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000185 RID: 389 RVA: 0x00003E17 File Offset: 0x00002017
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00061C5C File Offset: 0x0005FE5C
	public override void Start()
	{
		base.Start();
		this.decanter.LevelInit(this.properties);
		this.martini.LevelInit(this.properties);
		this.tumbler.LevelInit(this.properties);
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = this.properties.CurrentState.decanter.decanterHP + this.properties.CurrentState.martini.martiniHP + this.properties.CurrentState.tumbler.tumblerHP;
		foreach (Transform lamp in this.lamps)
		{
			base.StartCoroutine(this.lamps_cr(lamp));
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00003E1F File Offset: 0x0000201F
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceboozePattern_cr());
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00003E2E File Offset: 0x0000202E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00061D44 File Offset: 0x0005FF44
	public IEnumerator dicepalaceboozePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00061D60 File Offset: 0x0005FF60
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceBooze.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceBooze.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00061D7C File Offset: 0x0005FF7C
	public IEnumerator lamps_cr(Transform lamp)
	{
		float t = 0f;
		float maxSpeed = 0f;
		float speed = maxSpeed;
		for (;;)
		{
			t = 0f;
			maxSpeed = Random.Range(5f, 15f);
			speed = maxSpeed;
			while (!CupheadLevelCamera.Current.isShaking)
			{
				yield return null;
			}
			bool movingRight = Rand.Bool();
			while (speed > 0f)
			{
				t = ((!movingRight) ? (t - CupheadTime.Delta) : (t + CupheadTime.Delta));
				float phase = Mathf.Sin(t);
				lamp.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
				speed -= 0.05f;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400013B RID: 315
	public LevelProperties.DicePalaceBooze properties;

	// Token: 0x0400013C RID: 316
	[SerializeField]
	public Transform[] lamps;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	public DicePalaceBoozeLevelDecanter decanter;

	// Token: 0x0400013E RID: 318
	[SerializeField]
	public DicePalaceBoozeLevelMartini martini;

	// Token: 0x0400013F RID: 319
	[SerializeField]
	public DicePalaceBoozeLevelTumbler tumbler;

	// Token: 0x04000140 RID: 320
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	public string _bossQuote;
}
