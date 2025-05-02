using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class AirshipJellyLevel : Level
{
	// Token: 0x0600004F RID: 79 RVA: 0x0005E4BC File Offset: 0x0005C6BC
	public override void PartialInit()
	{
		this.properties = LevelProperties.AirshipJelly.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000050 RID: 80 RVA: 0x00003546 File Offset: 0x00001746
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipJelly;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000051 RID: 81 RVA: 0x00003549 File Offset: 0x00001749
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_jelly;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000052 RID: 82 RVA: 0x0000354D File Offset: 0x0000174D
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00003555 File Offset: 0x00001755
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0000355D File Offset: 0x0000175D
	public override void Start()
	{
		base.Start();
		this.jelly.LevelInit(this.properties);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003576 File Offset: 0x00001776
	public override void OnLevelStart()
	{
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0005E554 File Offset: 0x0005C754
	public IEnumerator airshipPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0005E570 File Offset: 0x0005C770
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipJelly.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000079 RID: 121
	public LevelProperties.AirshipJelly properties;

	// Token: 0x0400007A RID: 122
	[SerializeField]
	public AirshipJellyLevelJelly jelly;

	// Token: 0x0400007B RID: 123
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x02000725 RID: 1829
	[Serializable]
	public class Prefabs
	{
	}
}
