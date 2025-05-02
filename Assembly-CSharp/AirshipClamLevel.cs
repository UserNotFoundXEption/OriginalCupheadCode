using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class AirshipClamLevel : Level
{
	// Token: 0x06000038 RID: 56 RVA: 0x0005E2E4 File Offset: 0x0005C4E4
	public override void PartialInit()
	{
		this.properties = LevelProperties.AirshipClam.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000039 RID: 57 RVA: 0x000034A7 File Offset: 0x000016A7
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipClam;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600003A RID: 58 RVA: 0x000034AE File Offset: 0x000016AE
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_clam;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600003B RID: 59 RVA: 0x000034B2 File Offset: 0x000016B2
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600003C RID: 60 RVA: 0x000034BA File Offset: 0x000016BA
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000034C2 File Offset: 0x000016C2
	public override void Start()
	{
		base.Start();
		this.clam.LevelInit(this.properties);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000034DB File Offset: 0x000016DB
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipclamPattern_cr());
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0005E37C File Offset: 0x0005C57C
	public IEnumerator airshipclamPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0005E398 File Offset: 0x0005C598
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipClam.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.AirshipClam.Pattern.Spit)
		{
			if (p != LevelProperties.AirshipClam.Pattern.Barnacles)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				base.StartCoroutine(this.barnacles_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.spit_cr());
		}
		yield break;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0005E3B4 File Offset: 0x0005C5B4
	public IEnumerator spit_cr()
	{
		if (!this.attacking)
		{
			this.clam.OnSpitStart(new Action(this.EndAttack));
			this.attacking = true;
		}
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0005E3D0 File Offset: 0x0005C5D0
	public IEnumerator barnacles_cr()
	{
		if (!this.attacking)
		{
			this.clam.OnBarnaclesStart(new Action(this.EndAttack));
			this.attacking = true;
		}
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000034EA File Offset: 0x000016EA
	public void EndAttack()
	{
		this.attacking = false;
	}

	// Token: 0x04000070 RID: 112
	public LevelProperties.AirshipClam properties;

	// Token: 0x04000071 RID: 113
	[SerializeField]
	public AirshipClamLevelClam clam;

	// Token: 0x04000072 RID: 114
	public bool attacking;

	// Token: 0x04000073 RID: 115
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000074 RID: 116
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
