using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class BaronessLevel : Level
{
	// Token: 0x06000063 RID: 99 RVA: 0x0005E65C File Offset: 0x0005C85C
	public override void PartialInit()
	{
		this.properties = LevelProperties.Baroness.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000064 RID: 100 RVA: 0x000035CB File Offset: 0x000017CB
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Baroness;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000065 RID: 101 RVA: 0x000035D2 File Offset: 0x000017D2
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_baroness;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000066 RID: 102 RVA: 0x000035D6 File Offset: 0x000017D6
	// (set) Token: 0x06000067 RID: 103 RVA: 0x000035DD File Offset: 0x000017DD
	public static List<string> PICKED_BOSSES { get; set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000068 RID: 104 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Baroness.States.Main:
			case LevelProperties.Baroness.States.Generic:
				if (this.currentMiniBoss == null)
				{
					return this._bossPortraitChase;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Gumball)
				{
					return this._bossPortraitGumball;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Waffle)
				{
					return this._bossPortraitWaffle;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.CandyCorn)
				{
					return this._bossPortraitCandyCorn;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Cupcake)
				{
					return this._bossPortraitCupcake;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Jawbreaker)
				{
					return this._bossPortraitJawbreaker;
				}
				return this._bossPortraitChase;
			case LevelProperties.Baroness.States.Chase:
				return this._bossPortraitChase;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitChase;
			}
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000069 RID: 105 RVA: 0x0005E7F8 File Offset: 0x0005C9F8
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Baroness.States.Main:
			case LevelProperties.Baroness.States.Generic:
				if (this.currentMiniBoss == null)
				{
					return this._bossQuoteChase;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Gumball)
				{
					return this._bossQuoteGumball;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Waffle)
				{
					return this._bossQuoteWaffle;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.CandyCorn)
				{
					return this._bossQuoteCandyCorn;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Cupcake)
				{
					return this._bossQuoteCupcake;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Jawbreaker)
				{
					return this._bossQuoteJawbreaker;
				}
				return this._bossQuoteChase;
			case LevelProperties.Baroness.States.Chase:
				return this._bossQuoteChase;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteChase;
			}
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000035E5 File Offset: 0x000017E5
	public override void Start()
	{
		base.Start();
		this.castle.LevelInit(this.properties);
		this.PickMiniBosses();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003604 File Offset: 0x00001804
	public void PickMiniBosses()
	{
		base.StartCoroutine(this.pickminibosses_cr());
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0005E8FC File Offset: 0x0005CAFC
	public IEnumerator update_current_boss_cr()
	{
		while (this.properties.CurrentState.stateName != LevelProperties.Baroness.States.Chase)
		{
			while (BaronessLevelCastle.CURRENT_MINI_BOSS == this.currentMiniBoss && BaronessLevelCastle.CURRENT_MINI_BOSS != null)
			{
				yield return null;
			}
			this.currentMiniBoss = BaronessLevelCastle.CURRENT_MINI_BOSS;
			if (this.currentMiniBoss != null)
			{
				this.currentMiniBoss.OnDamageTakenEvent += base.timeline.DealDamage;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0005E918 File Offset: 0x0005CB18
	public IEnumerator pickminibosses_cr()
	{
		LevelProperties.Baroness.Open p = this.properties.CurrentState.open;
		string[] pattern = p.miniBossString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int randIndex = 0;
		List<string> tempList = new List<string>(pattern);
		BaronessLevel.PICKED_BOSSES = new List<string>();
		for (int i = 0; i < p.miniBossAmount; i++)
		{
			randIndex = Random.Range(0, tempList.ToArray().Length);
			BaronessLevel.PICKED_BOSSES.Add(tempList[randIndex]);
			tempList.Remove(tempList[randIndex]);
		}
		this.SetUpTimeline();
		yield return null;
		yield break;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0005E934 File Offset: 0x0005CB34
	public void SetUpTimeline()
	{
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		List<float> list = new List<float>();
		for (int i = 0; i < BaronessLevel.PICKED_BOSSES.Count; i++)
		{
			string text = BaronessLevel.PICKED_BOSSES[i];
			if (text != null)
			{
				if (!(text == "1"))
				{
					if (!(text == "2"))
					{
						if (!(text == "3"))
						{
							if (!(text == "4"))
							{
								if (text == "5")
								{
									base.timeline.health += (float)this.properties.CurrentState.jawbreaker.jawbreakerHomingHP;
									list.Add((float)this.properties.CurrentState.jawbreaker.jawbreakerHomingHP);
								}
							}
							else
							{
								base.timeline.health += (float)this.properties.CurrentState.cupcake.HP;
								list.Add((float)this.properties.CurrentState.cupcake.HP);
							}
						}
						else
						{
							base.timeline.health += (float)this.properties.CurrentState.candyCorn.HP;
							list.Add((float)this.properties.CurrentState.candyCorn.HP);
						}
					}
					else
					{
						base.timeline.health += (float)this.properties.CurrentState.waffle.HP;
						list.Add((float)this.properties.CurrentState.waffle.HP);
					}
				}
				else
				{
					base.timeline.health += (float)this.properties.CurrentState.gumball.HP;
					list.Add((float)this.properties.CurrentState.gumball.HP);
				}
			}
		}
		base.timeline.health += this.properties.CurrentHealth;
		for (int j = 0; j < BaronessLevel.PICKED_BOSSES.Count; j++)
		{
			base.timeline.AddEventAtHealth(BaronessLevel.PICKED_BOSSES[j], base.timeline.GetHealthOfLastEvent() + (int)list[j]);
		}
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		this.castle.StartIntro();
		base.StartCoroutine(this.update_current_boss_cr());
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003613 File Offset: 0x00001813
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Baroness.States.Chase)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.chase_cr());
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003644 File Offset: 0x00001844
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.baronessPattern_cr());
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00003653 File Offset: 0x00001853
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCandyCorn = null;
		this._bossPortraitChase = null;
		this._bossPortraitCupcake = null;
		this._bossPortraitGumball = null;
		this._bossPortraitJawbreaker = null;
		this._bossPortraitWaffle = null;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0005EC04 File Offset: 0x0005CE04
	public IEnumerator baronessPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0005EC20 File Offset: 0x0005CE20
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Baroness.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Baroness.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0005EC3C File Offset: 0x0005CE3C
	public IEnumerator chase_cr()
	{
		this.castle.StartChase();
		yield return null;
		yield break;
	}

	// Token: 0x04000081 RID: 129
	public LevelProperties.Baroness properties;

	// Token: 0x04000083 RID: 131
	[SerializeField]
	public BaronessLevelCastle castle;

	// Token: 0x04000084 RID: 132
	public BaronessLevelMiniBossBase currentMiniBoss;

	// Token: 0x04000085 RID: 133
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitGumball;

	// Token: 0x04000086 RID: 134
	[SerializeField]
	public Sprite _bossPortraitWaffle;

	// Token: 0x04000087 RID: 135
	[SerializeField]
	public Sprite _bossPortraitCandyCorn;

	// Token: 0x04000088 RID: 136
	[SerializeField]
	public Sprite _bossPortraitCupcake;

	// Token: 0x04000089 RID: 137
	[SerializeField]
	public Sprite _bossPortraitJawbreaker;

	// Token: 0x0400008A RID: 138
	[SerializeField]
	public Sprite _bossPortraitChase;

	// Token: 0x0400008B RID: 139
	[SerializeField]
	public string _bossQuoteGumball;

	// Token: 0x0400008C RID: 140
	[SerializeField]
	public string _bossQuoteWaffle;

	// Token: 0x0400008D RID: 141
	[SerializeField]
	public string _bossQuoteCandyCorn;

	// Token: 0x0400008E RID: 142
	[SerializeField]
	public string _bossQuoteCupcake;

	// Token: 0x0400008F RID: 143
	[SerializeField]
	public string _bossQuoteJawbreaker;

	// Token: 0x04000090 RID: 144
	[SerializeField]
	public string _bossQuoteChase;
}
