using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class DiceGateLevel : Level
{
	// Token: 0x06000172 RID: 370 RVA: 0x000617A8 File Offset: 0x0005F9A8
	public override void PartialInit()
	{
		this.properties = LevelProperties.DiceGate.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000173 RID: 371 RVA: 0x00003DB0 File Offset: 0x00001FB0
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DiceGate;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000174 RID: 372 RVA: 0x00003DB7 File Offset: 0x00001FB7
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_gate;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000175 RID: 373 RVA: 0x00003DBB File Offset: 0x00001FBB
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000176 RID: 374 RVA: 0x00003DC3 File Offset: 0x00001FC3
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00061840 File Offset: 0x0005FA40
	public override void Start()
	{
		base.Start();
		SceneLoader.OnLoaderCompleteEvent += this.SetMusic;
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			this.world1Background.SetActive(true);
			if (PlayerData.Data.CheckLevelCompleted(Levels.Veggies))
			{
				this.chalkboardCrosses[0].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Slime))
			{
				this.chalkboardCrosses[1].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Frogs))
			{
				this.chalkboardCrosses[2].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingBlimp))
			{
				this.chalkboardCrosses[3].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Flower))
			{
				this.chalkboardCrosses[4].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
			{
				this.dialogueInteractionPoint.animationTriggerOnEnd = this.completeLevelAnimationTrigger;
				this.OpenWay();
			}
			else
			{
				this.CloseWay();
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			this.world2Background.SetActive(true);
			this.toPrevWorld.dialogueProperties = this.world2PrevProperties;
			this.toNextWorld.dialogueProperties = this.world2NextProperties;
			if (PlayerData.Data.CheckLevelCompleted(Levels.Baroness))
			{
				this.chalkboardCrosses[0].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingGenie))
			{
				this.chalkboardCrosses[1].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Clown))
			{
				this.chalkboardCrosses[2].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingBird))
			{
				this.chalkboardCrosses[3].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Dragon))
			{
				this.chalkboardCrosses[4].SetActive(true);
			}
			this.dialogueInteractionPoint.dialogueInteraction = this.dialogueWorld2;
			if (PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
			{
				this.dialogueInteractionPoint.animationTriggerOnEnd = this.completeLevelAnimationTrigger;
				this.OpenWay();
			}
			else
			{
				this.CloseWay();
			}
		}
		else
		{
			Debug.LogError("SOMETHING BAD HAPPENED", null);
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00003DCB File Offset: 0x00001FCB
	public void SetMusic()
	{
		AudioManager.PlayBGMPlaylistManually(true);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00003DD3 File Offset: 0x00001FD3
	public override void OnLevelStart()
	{
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00003DD5 File Offset: 0x00001FD5
	public override void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.SetMusic;
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00061AC4 File Offset: 0x0005FCC4
	public void CloseWay()
	{
		this.toNextWorld.enabled = false;
		this.kingDice.SetActive(true);
		if (PlayerData.Data.CurrentMapData.hasVisitedDieHouse)
		{
			if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
			{
				Dialoguer.SetGlobalFloat(16, 1f);
			}
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00061B20 File Offset: 0x0005FD20
	public void OpenWay()
	{
		this.toNextWorld.enabled = true;
		if (PlayerData.Data.CurrentMapData.hasKingDiceDisappeared)
		{
			this.kingDice.SetActive(false);
		}
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			Dialoguer.SetGlobalFloat(16, 2f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(17, 1f);
		}
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00061B8C File Offset: 0x0005FD8C
	public IEnumerator dicegatePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00061BA8 File Offset: 0x0005FDA8
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DiceGate.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400012D RID: 301
	public LevelProperties.DiceGate properties;

	// Token: 0x0400012E RID: 302
	[SerializeField]
	public AbstractLevelInteractiveEntity toNextWorld;

	// Token: 0x0400012F RID: 303
	[SerializeField]
	public AbstractLevelInteractiveEntity toPrevWorld;

	// Token: 0x04000130 RID: 304
	public AbstractUIInteractionDialogue.Properties world2PrevProperties;

	// Token: 0x04000131 RID: 305
	public AbstractUIInteractionDialogue.Properties world2NextProperties;

	// Token: 0x04000132 RID: 306
	[SerializeField]
	public GameObject kingDice;

	// Token: 0x04000133 RID: 307
	[SerializeField]
	public List<GameObject> chalkboardCrosses;

	// Token: 0x04000134 RID: 308
	[SerializeField]
	public DialogueInteractionPoint dialogueInteractionPoint;

	// Token: 0x04000135 RID: 309
	[SerializeField]
	public DialoguerDialogues dialogueWorld2;

	// Token: 0x04000136 RID: 310
	[SerializeField]
	public string completeLevelAnimationTrigger;

	// Token: 0x04000137 RID: 311
	[SerializeField]
	public GameObject world1Background;

	// Token: 0x04000138 RID: 312
	[SerializeField]
	public GameObject world2Background;

	// Token: 0x04000139 RID: 313
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
