using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class RobotLevel : Level
{
	// Token: 0x0600039B RID: 923 RVA: 0x00066EE4 File Offset: 0x000650E4
	public override void PartialInit()
	{
		this.properties = LevelProperties.Robot.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600039C RID: 924 RVA: 0x00004CD8 File Offset: 0x00002ED8
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Robot;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600039D RID: 925 RVA: 0x00004CDF File Offset: 0x00002EDF
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_robot;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600039E RID: 926 RVA: 0x00066F7C File Offset: 0x0006517C
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Robot.States.Main:
			case LevelProperties.Robot.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Robot.States.HeliHead:
				return this._bossPortraitHeliHead;
			case LevelProperties.Robot.States.Inventor:
				return this._bossPortraitInventor;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600039F RID: 927 RVA: 0x00066FFC File Offset: 0x000651FC
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Robot.States.Main:
			case LevelProperties.Robot.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Robot.States.HeliHead:
				return this._bossQuoteHeliHead;
			case LevelProperties.Robot.States.Inventor:
				return this._bossQuoteInventor;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0006707C File Offset: 0x0006527C
	public override void Start()
	{
		base.Start();
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		float[] array = new float[base.timeline.events.Count];
		for (int i = 0; i < base.timeline.events.Count; i++)
		{
			array[i] = base.timeline.events[i].percentage;
		}
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.hose.health;
		base.timeline.health += (float)this.properties.CurrentState.orb.chestHP;
		base.timeline.health += (float)this.properties.CurrentState.shotBot.hatchGateHealth;
		base.timeline.health += (float)this.properties.CurrentState.heart.heartHP;
		float num = base.timeline.health;
		if (Level.Current.mode != Level.Mode.Easy)
		{
			for (int j = 0; j < array.Length; j++)
			{
				float num2 = this.properties.TotalHealth * ((j >= array.Length - 1) ? array[j] : (array[j] - array[j + 1]));
				Level.Current.timeline.health += num2;
			}
			base.timeline.AddEvent(new Level.Timeline.Event(string.Empty, 1f - num / Level.Current.timeline.health));
			for (int k = 0; k < array.Length; k++)
			{
				num += this.properties.TotalHealth * ((k >= array.Length - 1) ? array[k] : (array[k] - array[k + 1]));
				if (k < array.Length - 1)
				{
					base.timeline.AddEvent(new Level.Timeline.Event(string.Empty, 1f - num / Level.Current.timeline.health));
				}
			}
		}
		this.robot.LevelInit(this.properties);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00004CE3 File Offset: 0x00002EE3
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.robotPattern_cr());
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000672E4 File Offset: 0x000654E4
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		LevelProperties.Robot.States stateName = this.properties.CurrentState.stateName;
		if (stateName != LevelProperties.Robot.States.HeliHead)
		{
			if (stateName == LevelProperties.Robot.States.Inventor)
			{
				this.heliHead.ChangeState();
			}
		}
		else
		{
			this.StopAllCoroutines();
			this.robot.TriggerPhaseTwo(new Action(this.OnHeliheadSpawn));
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00004CF2 File Offset: 0x00002EF2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitHeliHead = null;
		this._bossPortraitInventor = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00067354 File Offset: 0x00065554
	public IEnumerator robotPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00067370 File Offset: 0x00065570
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Robot.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Robot.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00004D0F File Offset: 0x00002F0F
	public void OnHeliheadSpawn()
	{
		base.StartCoroutine(this.spawnHeliHead_cr());
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0006738C File Offset: 0x0006558C
	public IEnumerator spawnHeliHead_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		this.robot.animator.SetTrigger("Phase2Transition");
		yield return this.robot.animator.WaitForAnimationToEnd(this, "Death Dance", true, true);
		this.heliHead.GetComponent<RobotLevelHelihead>().InitHeliHead(this.properties);
		yield break;
	}

	// Token: 0x04000292 RID: 658
	public LevelProperties.Robot properties;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	public RobotLevelRobot robot;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	public RobotLevelHelihead heliHead;

	// Token: 0x04000295 RID: 661
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	public Sprite _bossPortraitHeliHead;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	public Sprite _bossPortraitInventor;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	public string _bossQuoteHeliHead;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	public string _bossQuoteInventor;
}
