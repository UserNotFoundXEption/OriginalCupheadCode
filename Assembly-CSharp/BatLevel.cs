using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class BatLevel : Level
{
	// Token: 0x06000076 RID: 118 RVA: 0x0005EC58 File Offset: 0x0005CE58
	public override void PartialInit()
	{
		this.properties = LevelProperties.Bat.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000077 RID: 119 RVA: 0x0000368D File Offset: 0x0000188D
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Bat;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000078 RID: 120 RVA: 0x00003690 File Offset: 0x00001890
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_bat;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00003694 File Offset: 0x00001894
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600007A RID: 122 RVA: 0x0000369C File Offset: 0x0000189C
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000036A4 File Offset: 0x000018A4
	public override void Start()
	{
		base.Start();
		this.bat.LevelInit(this.properties);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000036BD File Offset: 0x000018BD
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		base.StartCoroutine(this.batPattern_cr());
		base.StartCoroutine(this.goblins_cr());
	}

	// Token: 0x0600007D RID: 125 RVA: 0x0005ECF0 File Offset: 0x0005CEF0
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Bat.States.Wolf)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_3_cr());
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x0005ED5C File Offset: 0x0005CF5C
	public IEnumerator batPattern_cr()
	{
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x0005ED78 File Offset: 0x0005CF78
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Bat.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Bat.Pattern.Bouncer)
		{
			if (p != LevelProperties.Bat.Pattern.Lightning)
			{
				yield return new WaitForSeconds(1f);
			}
			else
			{
				yield return base.StartCoroutine(this.lightning_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.bouncer_cr());
		}
		yield break;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0005ED94 File Offset: 0x0005CF94
	public IEnumerator bouncer_cr()
	{
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		this.bat.StartBouncer();
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0005EDB0 File Offset: 0x0005CFB0
	public IEnumerator lightning_cr()
	{
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		this.bat.StartLightning();
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0005EDCC File Offset: 0x0005CFCC
	public IEnumerator phase_2_cr()
	{
		this.bat.StartPhase2();
		yield return null;
		yield break;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0005EDE8 File Offset: 0x0005CFE8
	public IEnumerator phase_3_cr()
	{
		this.bat.StartPhase3();
		yield return null;
		yield break;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0005EE04 File Offset: 0x0005D004
	public IEnumerator goblins_cr()
	{
		if (!this.properties.CurrentState.goblins.Enabled)
		{
			yield return null;
		}
		else
		{
			this.bat.StartGoblin();
		}
		yield return null;
		yield break;
	}

	// Token: 0x04000091 RID: 145
	public LevelProperties.Bat properties;

	// Token: 0x04000092 RID: 146
	[Space(10f)]
	[SerializeField]
	public BatLevelBat bat;

	// Token: 0x04000093 RID: 147
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000094 RID: 148
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x0200072F RID: 1839
	[Serializable]
	public class Prefabs
	{
	}
}
