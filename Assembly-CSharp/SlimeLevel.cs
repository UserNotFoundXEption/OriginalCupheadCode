using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class SlimeLevel : Level
{
	// Token: 0x06000404 RID: 1028 RVA: 0x00068D38 File Offset: 0x00066F38
	public override void PartialInit()
	{
		this.properties = LevelProperties.Slime.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00004EE1 File Offset: 0x000030E1
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Slime;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00004EE8 File Offset: 0x000030E8
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_slime;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00068DD0 File Offset: 0x00066FD0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Slime.States.Main:
			case LevelProperties.Slime.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Slime.States.BigSlime:
				return this._bossPortraitBigSlime;
			case LevelProperties.Slime.States.Tombstone:
				return this._bossPortraitTombstone;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00068E50 File Offset: 0x00067050
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Slime.States.Main:
			case LevelProperties.Slime.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Slime.States.BigSlime:
				return this._bossQuoteBigSlime;
			case LevelProperties.Slime.States.Tombstone:
				return this._bossQuoteTombstone;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00004EEC File Offset: 0x000030EC
	public override void Start()
	{
		base.Start();
		this.smallSlime.LevelInit(this.properties);
		this.bigSlime.LevelInit(this.properties);
		this.tombStone.LevelInit(this.properties);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00004F27 File Offset: 0x00003127
	public override void OnLevelStart()
	{
		this.smallSlime.IntroContinue();
		base.StartCoroutine(this.slimePattern_cr());
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00068ED0 File Offset: 0x000670D0
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Slime.States.BigSlime)
		{
			this.reachedBigSlimeState = true;
			this.StopAllCoroutines();
			this.smallSlime.Transform();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Slime.States.Tombstone)
		{
			this.StopAllCoroutines();
			this.bigSlime.DeathTransform();
		}
		if (!this.reachedBigSlimeState)
		{
			this.smallSlime.CurrentPropertyState = this.properties.CurrentState;
		}
		this.bigSlime.CurrentPropertyState = this.properties.CurrentState;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00004F41 File Offset: 0x00003141
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBigSlime = null;
		this._bossPortraitMain = null;
		this._bossPortraitTombstone = null;
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00068F74 File Offset: 0x00067174
	public IEnumerator slimePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00068F90 File Offset: 0x00067190
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Slime.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Slime.Pattern.Jump)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040002F7 RID: 759
	public LevelProperties.Slime properties;

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	public SlimeLevelSlime smallSlime;

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	public SlimeLevelSlime bigSlime;

	// Token: 0x040002FA RID: 762
	[SerializeField]
	public SlimeLevelTombstone tombStone;

	// Token: 0x040002FB RID: 763
	public bool reachedBigSlimeState;

	// Token: 0x040002FC RID: 764
	public DamageDealer damageDealer;

	// Token: 0x040002FD RID: 765
	public DamageReceiver damageReceiver;

	// Token: 0x040002FE RID: 766
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	public Sprite _bossPortraitBigSlime;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	public Sprite _bossPortraitTombstone;

	// Token: 0x04000301 RID: 769
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	public string _bossQuoteBigSlime;

	// Token: 0x04000303 RID: 771
	[SerializeField]
	public string _bossQuoteTombstone;
}
