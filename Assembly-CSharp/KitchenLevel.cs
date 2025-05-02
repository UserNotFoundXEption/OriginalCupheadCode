using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class KitchenLevel : Level
{
	// Token: 0x06000303 RID: 771 RVA: 0x00064E4C File Offset: 0x0006304C
	public override void PartialInit()
	{
		this.properties = LevelProperties.Kitchen.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000304 RID: 772 RVA: 0x000048BC File Offset: 0x00002ABC
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Kitchen;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000305 RID: 773 RVA: 0x000048C3 File Offset: 0x00002AC3
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_kitchen;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000306 RID: 774 RVA: 0x000048C7 File Offset: 0x00002AC7
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000307 RID: 775 RVA: 0x000048CF File Offset: 0x00002ACF
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00064EE4 File Offset: 0x000630E4
	public override void Start()
	{
		base.Start();
		this.CheckIfBossesCompleted();
		base.StartCoroutine(this.check_camera_cr());
		base.StartCoroutine(this.cycle_sunbeams_cr());
		this.beforeGettingIngredients.SetActive(!this.trapDoorOpen);
		this.afterGettingIngredients.SetActive(this.trapDoorOpen);
		this.AddDialoguerEvents();
		PlayerManager.OnPlayerJoinedEvent += this.SetPlayerBasementMaterial;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x000048D7 File Offset: 0x00002AD7
	public override void OnDestroy()
	{
		this.RemoveDialoguerEvents();
		PlayerManager.OnPlayerJoinedEvent -= this.SetPlayerBasementMaterial;
		base.OnDestroy();
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00064F54 File Offset: 0x00063154
	public void SetPlayerBasementMaterial(PlayerId p)
	{
		if (!this.basementBG.activeInHierarchy)
		{
			return;
		}
		foreach (SpriteRenderer spriteRenderer in PlayerManager.GetPlayer(p).GetComponentsInChildren<SpriteRenderer>())
		{
			if (spriteRenderer.material.name == "Sprites-Default (Instance)" || (spriteRenderer.sharedMaterial.name == "ChaliceRecolor (Instance)" && spriteRenderer.sharedMaterial.GetFloat("_RecolorFactor") == 0f))
			{
				spriteRenderer.material = this.playerBasementMaterial;
				spriteRenderer.color = new Color(0.7137255f, 0.4862745f, 0.129411772f);
			}
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x000048F6 File Offset: 0x00002AF6
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000490E File Offset: 0x00002B0E
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00004926 File Offset: 0x00002B26
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MetSaltbaker")
		{
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000493D File Offset: 0x00002B3D
	public void CheckIfBossesCompleted()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
		{
			this.trapDoorOpen = true;
			base.StartCoroutine(this.check_trigger_cr());
		}
		else
		{
			this.trapDoorOpen = false;
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0006500C File Offset: 0x0006320C
	public override void OnLevelStart()
	{
		if (Dialoguer.GetGlobalFloat(23) == 1f)
		{
			AudioManager.Play("sfx_dlc_bakery_doorenter");
		}
		if (this.trapDoorOpen)
		{
			AudioManager.StartBGMAlternate(1);
		}
		else if (PlayerData.Data.pianoAudioEnabled)
		{
			AudioManager.StartBGMAlternate(2);
		}
		else
		{
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0006506C File Offset: 0x0006326C
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.triggerEndGame != null)
		{
			Vector2 vector;
			vector..ctor(this.triggerEndGame.position.x, this.triggerEndGame.position.y + 1000f);
			Vector2 vector2;
			vector2..ctor(this.triggerEndGame.position.x, this.triggerEndGame.position.y - 1000f);
			Gizmos.DrawLine(vector, vector2);
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0006510C File Offset: 0x0006330C
	public IEnumerator check_trigger_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		bool hasntPassed = true;
		while (hasntPassed)
		{
			if (player.transform.position.x >= this.triggerEndGame.position.x)
			{
				hasntPassed = false;
			}
			if (player2 != null && player2.transform.position.x >= this.triggerEndGame.position.x)
			{
				hasntPassed = false;
			}
			yield return null;
		}
		PlayerManager.playerWasChalice[0] = player.stats.isChalice;
		PlayerManager.playerWasChalice[1] = (player2 != null && player2.stats.isChalice);
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			Level.SetCurrentMode(Level.Mode.Normal);
		}
		Cutscene.Load(Scenes.scene_level_saltbaker, Scenes.scene_cutscene_dlc_saltbaker_prebattle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		yield break;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00065128 File Offset: 0x00063328
	public IEnumerator check_camera_cr()
	{
		this.camera.mode = CupheadLevelCamera.Mode.Relative;
		bool inPit = false;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		float lastP1YPos = 0f;
		float lastP2YPos = 0f;
		while (!inPit)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null && !player2.IsDead)
			{
				inPit = (player2.transform.position.y < -400f && player.transform.position.y < -400f);
				if (!inPit)
				{
					if (player.transform.position.y < -400f)
					{
						player.gameObject.SetActive(false);
					}
					if (player2.transform.position.y < -400f)
					{
						player2.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				inPit = (player.transform.position.y < -400f);
			}
			if (player != null && Mathf.Sign(player.transform.position.y + 208f) != Mathf.Sign(lastP1YPos + 208f))
			{
				foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.sortingLayerName = ((player.transform.position.y >= -208f) ? "Player" : "Enemies");
				}
				lastP1YPos = player.transform.position.y;
			}
			if (player2 != null && Mathf.Sign(player2.transform.position.y + 208f) != Mathf.Sign(lastP2YPos + 208f))
			{
				foreach (SpriteRenderer spriteRenderer2 in player2.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer2.sortingLayerName = ((player2.transform.position.y >= -208f) ? "Player" : "Enemies");
				}
				lastP2YPos = player.transform.position.y;
			}
			yield return null;
		}
		this.kitchenBG.SetActive(false);
		this.basementBG.SetActive(true);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.0001f, 0.0001f);
		AudioManager.PlayLoop("sfx_dlc_bakery_basementamb_loop");
		AudioManager.PlayLoop("sfx_dlc_bakery_basementtorch_loop");
		this.afterGettingIngredients.SetActive(false);
		CupheadLevelCamera.Current.ChangeHorizontalBounds(740, 3500);
		Level.Current.SetBounds(new int?(680), new int?(6860), null, null);
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Lerp);
		CupheadLevelCamera.Current.LERP_SPEED = 5f;
		CupheadLevelCamera.Current.SetPosition(new Vector3(-100f, 0f));
		player.transform.position = new Vector3(-500f, 800f);
		player.gameObject.SetActive(true);
		if (player2 != null && !player2.IsDead)
		{
			player2.transform.position = new Vector3(-400f, 800f);
			player2.gameObject.SetActive(true);
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			if (abstractPlayerController != null)
			{
				this.SetPlayerBasementMaterial(abstractPlayerController.id);
				foreach (SpriteRenderer spriteRenderer3 in abstractPlayerController.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer3.sortingLayerName = "Player";
				}
			}
		}
		AudioManager.StartBGMAlternate(0);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.5f, 1f);
		while (CupheadLevelCamera.Current.transform.position.x < 2320f)
		{
			this.HandleTorchSFX();
			yield return null;
		}
		this.saltbakerShadow.SetTrigger("Continue");
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Platforming);
		AudioManager.Play("sfx_dlc_saltbaker_evilbasementlaugh");
		while (CupheadLevelCamera.Current.transform.position.x < 2800f)
		{
			this.HandleTorchSFX();
			yield return null;
		}
		this.saltbakerShadow.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00065144 File Offset: 0x00063344
	public void HandleTorchSFX()
	{
		float num = float.MaxValue;
		float num2 = 0f;
		foreach (Transform transform in this.torchPositions)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				if (abstractPlayerController != null)
				{
					float num3 = Mathf.Abs(abstractPlayerController.center.x - transform.position.x);
					if (num3 < num)
					{
						num2 = Mathf.Sign(transform.position.x - abstractPlayerController.center.x);
						num = num3;
					}
				}
			}
		}
		float num4 = Mathf.InverseLerp(320f, 0f, num);
		float value = num2 * (1f - num4);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementtorch_loop", Mathf.Lerp(0.01f, 0.8f, num4), 0.0001f);
		AudioManager.Pan("sfx_dlc_bakery_basementtorch_loop", value);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0006527C File Offset: 0x0006347C
	public IEnumerator cycle_sunbeams_cr()
	{
		float t = 0f;
		for (;;)
		{
			for (int i = 0; i < 3; i++)
			{
				this.sunbeams[i].color = new Color(1f, 1f, 1f, (Mathf.Sin((float)i * 2.09439516f + t) + 1f) / 2f);
			}
			t += CupheadTime.Delta * this.sunbeamCycleSpeed;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000205 RID: 517
	public LevelProperties.Kitchen properties;

	// Token: 0x04000206 RID: 518
	public const int DIALOGUER_VAR_ID = 23;

	// Token: 0x04000207 RID: 519
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	public GameObject beforeGettingIngredients;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	public GameObject afterGettingIngredients;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	public SpriteRenderer[] sunbeams;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	public float sunbeamCycleSpeed = 2f;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	public Animator saltbakerShadow;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	public Transform triggerEndGame;

	// Token: 0x0400020F RID: 527
	public bool trapDoorOpen;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	public SpriteRenderer trapDoorOverlay;

	// Token: 0x04000211 RID: 529
	public bool forceUnlockSaltbakerBattle;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	public GameObject kitchenBG;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	public GameObject basementBG;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	public Material playerBasementMaterial;

	// Token: 0x04000215 RID: 533
	[SerializeField]
	public Transform[] torchPositions;
}
