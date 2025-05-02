using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class PlatformingLevel : Level
{
	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06002F71 RID: 12145 RVA: 0x00027827 File Offset: 0x00025A27
	// (set) Token: 0x06002F72 RID: 12146 RVA: 0x0002782E File Offset: 0x00025A2E
	public new static PlatformingLevel Current { get; set; }

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06002F73 RID: 12147 RVA: 0x00027836 File Offset: 0x00025A36
	public override Levels CurrentLevel
	{
		get
		{
			return this._currentLevel;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06002F74 RID: 12148 RVA: 0x0002783E File Offset: 0x00025A3E
	public override Scenes CurrentScene
	{
		get
		{
			return this._currentScene;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06002F75 RID: 12149 RVA: 0x00027846 File Offset: 0x00025A46
	public override Sprite BossPortrait
	{
		get
		{
			return (!this.useAltQuote) ? this._bossPortrait : this._bossPortraitAlt;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06002F76 RID: 12150 RVA: 0x00027864 File Offset: 0x00025A64
	public override string BossQuote
	{
		get
		{
			return (!this.useAltQuote) ? this._bossQuote : this._bossQuoteAlt;
		}
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x000E1C6C File Offset: 0x000DFE6C
	public override void Awake()
	{
		this._currentLevel = SceneLoader.CurrentLevel;
		this._currentScene = EnumUtils.Parse<Scenes>(LevelProperties.GetLevelScene(this._currentLevel));
		this.goalTimes = new Level.GoalTimes(this.goalTime, this.goalTime, this.goalTime);
		Level.OverrideDifficulty = true;
		base.mode = Level.Mode.Normal;
		base.Awake();
		PlatformingLevel.Current = this;
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x00027882 File Offset: 0x00025A82
	public override void Start()
	{
		base.Start();
		this.LevelCoinsIDs.Sort((CoinPositionAndID a, CoinPositionAndID b) => a.xPos.CompareTo(b.xPos));
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x000E1CD0 File Offset: 0x000DFED0
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		base.timeline = new Level.Timeline();
		base.timeline.health = 100f;
		base.StartCoroutine(this.checkPosition_cr());
		Level.ScoringData.pacifistRun = true;
		PlatformingLevelExit.OnWinStartEvent += this.OnWinStart;
		PlatformingLevelExit.OnWinCompleteEvent += this.OnWinComplete;
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x000278B2 File Offset: 0x00025AB2
	public void OnWinStart()
	{
		base.Ending = true;
		CupheadLevelCamera.Current.MoveRightCollider();
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x000278C5 File Offset: 0x00025AC5
	public void OnWinComplete()
	{
		LevelCoin.OnLevelComplete();
		Level.ScoringData.coinsCollected = PlayerData.Data.GetNumCoinsCollectedInLevel(this.CurrentLevel);
		Level.ScoringData.useCoinsInsteadOfSuperMeter = true;
		base.zHack_OnWin();
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x000278F7 File Offset: 0x00025AF7
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (PlatformingLevel.Current == this)
		{
			PlatformingLevel.Current = null;
		}
		this._bossPortrait = null;
		this._bossPortraitAlt = null;
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x000E1D38 File Offset: 0x000DFF38
	public override void Reset()
	{
		base.Reset();
		this.type = Level.Type.Platforming;
		this.bounds.bottom = 500;
		this.camera.moveX = true;
		this.camera.moveY = true;
		this.camera.mode = CupheadLevelCamera.Mode.Platforming;
		this.camera.colliders = true;
		this.camera.bounds.rightEnabled = false;
		this.camera.bounds.topEnabled = false;
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x000E1DB4 File Offset: 0x000DFFB4
	public IEnumerator checkPosition_cr()
	{
		for (;;)
		{
			foreach (AbstractPlayerController abstractPlayerController in this.players)
			{
				if (abstractPlayerController != null && !abstractPlayerController.IsDead && base.LevelType == Level.Type.Platforming && this.camera.mode == CupheadLevelCamera.Mode.Path)
				{
					float value = this.camera.path.GetClosestNormalizedPoint(abstractPlayerController.center, abstractPlayerController.center, true, true) * 100f;
					base.timeline.SetPlayerDamage(abstractPlayerController.id, value);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x00027923 File Offset: 0x00025B23
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x00027936 File Offset: 0x00025B36
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x00027949 File Offset: 0x00025B49
	public void DrawGizmos(float a)
	{
		if (this.camera.mode != CupheadLevelCamera.Mode.Path)
		{
			return;
		}
		this.camera.path.DrawGizmos(a, base.baseTransform.position);
	}

	// Token: 0x0400275B RID: 10075
	public const float TIMELINE_LENGTH = 100f;

	// Token: 0x0400275D RID: 10077
	public List<CoinPositionAndID> LevelCoinsIDs = new List<CoinPositionAndID>();

	// Token: 0x0400275E RID: 10078
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400275F RID: 10079
	[SerializeField]
	public Sprite _bossPortraitAlt;

	// Token: 0x04002760 RID: 10080
	[SerializeField]
	public string _bossQuote;

	// Token: 0x04002761 RID: 10081
	[SerializeField]
	public string _bossQuoteAlt;

	// Token: 0x04002762 RID: 10082
	[SerializeField]
	public float goalTime;

	// Token: 0x04002763 RID: 10083
	public Levels _currentLevel;

	// Token: 0x04002764 RID: 10084
	public Scenes _currentScene;

	// Token: 0x04002765 RID: 10085
	public bool useAltQuote;

	// Token: 0x020010D0 RID: 4304
	public enum Theme
	{
		// Token: 0x0400773D RID: 30525
		Forest
	}
}
