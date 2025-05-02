using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class HouseLevel : Level
{
	// Token: 0x060002F2 RID: 754 RVA: 0x00064A5C File Offset: 0x00062C5C
	public override void PartialInit()
	{
		this.properties = LevelProperties.House.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060002F3 RID: 755 RVA: 0x00004817 File Offset: 0x00002A17
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.House;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000481E File Offset: 0x00002A1E
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_house_elder_kettle;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060002F5 RID: 757 RVA: 0x00004822 File Offset: 0x00002A22
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000482A File Offset: 0x00002A2A
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00064AF4 File Offset: 0x00062CF4
	public override void Start()
	{
		base.Start();
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(new Levels[]
		{
			Levels.Devil
		}, Level.Mode.Hard))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 8f);
		}
		else if (PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard) > 0)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 7f);
		}
		else if (PlayerData.Data.IsHardModeAvailable)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 6f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 5f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 4f);
		}
		else if (PlayerData.Data.CountLevelsCompleted(Level.world1BossLevels) > 1)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 3f);
		}
		else if (PlayerData.Data.IsTutorialCompleted)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
		}
		else if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 0f)
		{
			this.tutorialGameObject.SetActive(false);
			base.Ending = true;
		}
		SceneLoader.OnLoaderCompleteEvent += this.SelectMusic;
		this.AddDialoguerEvents();
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00004832 File Offset: 0x00002A32
	public void SelectMusic()
	{
		if (PlayerData.Data.pianoAudioEnabled)
		{
			AudioManager.PlayBGMPlaylistManually(false);
		}
		else
		{
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00004853 File Offset: 0x00002A53
	public override void OnDestroy()
	{
		base.OnDestroy();
		SceneLoader.OnLoaderCompleteEvent -= this.SelectMusic;
		this.RemoveDialoguerEvents();
		this.playerTutorialEffects = null;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00064C9C File Offset: 0x00062E9C
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00004879 File Offset: 0x00002A79
	public void OnDialogueEndedHandler()
	{
		base.Ending = false;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00004882 File Offset: 0x00002A82
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00064CEC File Offset: 0x00062EEC
	public void StartTutorial()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerTutorialEffects[0].gameObject.SetActive(true);
		this.playerTutorialEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerTutorialEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerTutorialEffects[1].gameObject.SetActive(true);
			this.playerTutorialEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerTutorialEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00064DBC File Offset: 0x00062FBC
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "ElderKettleFirstWeapon")
		{
			this.tutorialGameObject.SetActive(true);
			base.StartCoroutine(this.power_up_cr());
		}
		if (message == "EndJoy")
		{
		}
		if (message == "Sleep")
		{
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00064E14 File Offset: 0x00063014
	public IEnumerator power_up_cr()
	{
		yield return new WaitForSeconds(0.15f);
		AudioManager.Play("sfx_potion_poof");
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.animator.Play("Power_Up");
			}
		}
		yield break;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0000489A File Offset: 0x00002A9A
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.housePattern_cr());
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00064E30 File Offset: 0x00063030
	public IEnumerator housePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 0f)
		{
			this.elderDialoguePoint.BeginDialogue();
		}
		yield break;
	}

	// Token: 0x040001FE RID: 510
	public LevelProperties.House properties;

	// Token: 0x040001FF RID: 511
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	public PlayerDeathEffect[] playerTutorialEffects;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	public HouseElderKettle elderDialoguePoint;

	// Token: 0x04000203 RID: 515
	[SerializeField]
	public GameObject tutorialGameObject;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	public int dialoguerVariableID;
}
