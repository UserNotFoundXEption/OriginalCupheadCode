using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020004AB RID: 1195
public class Map : AbstractMonoBehaviour
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06003192 RID: 12690 RVA: 0x0002936B File Offset: 0x0002756B
	// (set) Token: 0x06003193 RID: 12691 RVA: 0x00029372 File Offset: 0x00027572
	public static Map Current { get; set; }

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x0002937A File Offset: 0x0002757A
	// (set) Token: 0x06003195 RID: 12693 RVA: 0x00029382 File Offset: 0x00027582
	public Map.State CurrentState { get; set; }

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06003196 RID: 12694 RVA: 0x0002938B File Offset: 0x0002758B
	// (set) Token: 0x06003197 RID: 12695 RVA: 0x00029393 File Offset: 0x00027593
	public MapPlayerController[] players { get; set; }

	// Token: 0x06003198 RID: 12696 RVA: 0x000EA448 File Offset: 0x000E8648
	public override void Awake()
	{
		base.Awake();
		Map.Current = this;
		Cuphead.Init(false);
		Level.ResetBossesHub();
		Level.IsGraveyard = false;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		this.scene = EnumUtils.Parse<Scenes>(SceneManager.GetActiveScene().name);
		PlayerData.Data.CurrentMap = this.scene;
		this.CreateUI();
		this.CreatePlayers();
		this.ui.Init(this.players);
		this.cupheadMapCamera = Object.FindObjectOfType<CupheadMapCamera>();
		this.cupheadMapCamera.Init(this.cameraProperties);
		CupheadTime.SetAll(1f);
		SceneLoader.OnLoaderCompleteEvent += this.SelectMusic;
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x0002939C File Offset: 0x0002759C
	public void Start()
	{
		if (PlatformHelper.ManuallyRefreshDLCAvailability)
		{
			DLCManager.CheckInstallationStatusChanged();
		}
		AudioManager.PlayLoop(string.Empty);
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x000EA51C File Offset: 0x000E871C
	public void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.SelectMusic;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.MapResources = null;
		this.cameraProperties = null;
		this.firstNode = null;
		this.entryPoints = null;
		this.ui = null;
		this.cupheadMapCamera = null;
		this.players = null;
		if (Map.Current == this)
		{
			Map.Current = null;
		}
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x000EA5A8 File Offset: 0x000E87A8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.bottom));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.bottom));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.top));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.bottom), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.bottom));
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x000293C4 File Offset: 0x000275C4
	public void CreateUI()
	{
		this.ui = Object.FindObjectOfType<MapUI>();
		if (this.ui == null)
		{
			this.ui = MapUI.Create();
		}
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x000EA6F8 File Offset: 0x000E88F8
	public void CreatePlayers()
	{
		if (!PlayerData.Data.CurrentMapData.sessionStarted)
		{
			PlayerData.Data.CurrentMapData.sessionStarted = true;
			PlayerData.Data.CurrentMapData.playerOnePosition = this.firstNode.transform.position + this.firstNode.returnPositions.playerOne;
			PlayerData.Data.CurrentMapData.playerTwoPosition = this.firstNode.transform.position + this.firstNode.returnPositions.playerTwo;
			if (!PlayerManager.Multiplayer)
			{
				PlayerData.Data.CurrentMapData.playerOnePosition = this.firstNode.transform.position + this.firstNode.returnPositions.singlePlayer;
			}
		}
		else if (PlayerData.Data.CurrentMapData.enteringFrom != PlayerData.MapData.EntryMethod.None)
		{
			this.entryPoints[(int)PlayerData.Data.CurrentMapData.enteringFrom].SetPlayerReturnPos();
			PlayerData.Data.CurrentMapData.enteringFrom = PlayerData.MapData.EntryMethod.None;
		}
		PlayerData.SaveCurrentFile();
		MapPlayerPose pose = MapPlayerPose.Default;
		if (Level.Won && Level.PreviousLevel != Levels.Saltbaker)
		{
			pose = MapPlayerPose.Won;
		}
		this.players = new MapPlayerController[2];
		this.players[0] = MapPlayerController.Create(PlayerId.PlayerOne, new MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerOnePosition, pose));
		if (PlayerManager.Multiplayer)
		{
			this.players[1] = MapPlayerController.Create(PlayerId.PlayerTwo, new MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerTwoPosition, pose));
		}
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x000EA8BC File Offset: 0x000E8ABC
	public virtual void OnPlayerJoined(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			Vector3 position = this.players[0].transform.position;
			Vector3 vector = position + new Vector3(0.05f, 0.05f, 0f);
			LayerMask layerMask = -257;
			for (int i = 0; i < 10; i++)
			{
				float num = (float)(36 * -(float)i + 150);
				Vector2 vector2;
				vector2..ctor(Mathf.Cos(0.0174532924f * num), Mathf.Sin(0.0174532924f * num));
				if (!(Physics2D.CircleCast(position, 0.2f, vector2, 0.7f, layerMask.value).collider != null))
				{
					vector = position + vector2 * 0.7f;
					break;
				}
			}
			this.players[1] = MapPlayerController.Create(PlayerId.PlayerTwo, new MapPlayerController.InitObject(vector, MapPlayerPose.Joined));
			this.players[1].animationController.spriteRenderer.sortingOrder = this.players[0].animationController.spriteRenderer.sortingOrder;
			LevelNewPlayerGUI.Current.Init();
			this.SetRichPresence();
		}
		this.CheckMusic(true);
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x000293ED File Offset: 0x000275ED
	public virtual void OnPlayerLeave(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			this.players[1].OnLeave();
		}
		this.CheckMusic(true);
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x0002940A File Offset: 0x0002760A
	public virtual void SelectMusic()
	{
		this.currentMusic = -1;
		this.CheckMusic(false);
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x0002941A File Offset: 0x0002761A
	public void OnCloseEquipMenu()
	{
		this.CheckMusic(true);
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x00029423 File Offset: 0x00027623
	public void OnNPCChangeMusic()
	{
		this.CheckMusic(true);
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x000EAA00 File Offset: 0x000E8C00
	public virtual void CheckMusic(bool isRecheck)
	{
		int num = this.currentMusic;
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout2 = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo);
		if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
		{
			if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)))
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 2 : 4);
			}
			else
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 1 : 3);
			}
		}
		else
		{
			num = ((!PlayerData.Data.pianoAudioEnabled) ? -1 : 0);
		}
		if (!isRecheck || num != this.currentMusic)
		{
			this.currentMusic = num;
			if (this.currentMusic == -1)
			{
				AudioManager.PlayBGM();
			}
			else
			{
				AudioManager.StartBGMAlternate(this.currentMusic);
			}
		}
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x0002942C File Offset: 0x0002762C
	public void OnLoadLevel()
	{
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x0002942E File Offset: 0x0002762E
	public void OnLoadShop()
	{
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000EAB44 File Offset: 0x000E8D44
	public IEnumerator start_cr()
	{
		this.SetRichPresence();
		Level.ResetBossesHub();
		if (Level.Won && Level.PreviousLevel != Levels.Saltbaker)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
			bool longPlayerAnimation = true;
			bool cameraMoved = false;
			Vector3 cameraStartPos = this.cupheadMapCamera.transform.position;
			if (AbstractMapLevelDependentEntity.RegisteredEntities != null)
			{
				while (AbstractMapLevelDependentEntity.RegisteredEntities.Count > 0)
				{
					yield return null;
					this.CurrentState = Map.State.Event;
					AbstractMapLevelDependentEntity entity = AbstractMapLevelDependentEntity.RegisteredEntities[0];
					foreach (AbstractMapLevelDependentEntity abstractMapLevelDependentEntity in AbstractMapLevelDependentEntity.RegisteredEntities)
					{
						if (!abstractMapLevelDependentEntity.panCamera)
						{
							entity = abstractMapLevelDependentEntity;
							break;
						}
						if (!(abstractMapLevelDependentEntity == entity))
						{
							float num = Vector2.Distance(this.cupheadMapCamera.transform.position, abstractMapLevelDependentEntity.CameraPosition);
							if (num < Vector2.Distance(this.cupheadMapCamera.transform.position, entity.CameraPosition))
							{
								entity = abstractMapLevelDependentEntity;
							}
						}
					}
					AbstractMapLevelDependentEntity.RegisteredEntities.Remove(entity);
					if (entity.panCamera)
					{
						yield return this.cupheadMapCamera.MoveToPosition(entity.CameraPosition, 0.5f, 0.9f);
						cameraMoved = true;
					}
					entity.MapMeetCondition();
					while (entity.CurrentState != AbstractMapLevelDependentEntity.State.Complete)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, 0.25f);
					longPlayerAnimation = false;
				}
				if (cameraMoved)
				{
					this.cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75f, 1f);
				}
			}
			if (!PlayerManager.playerWasChalice[0] && (!PlayerManager.Multiplayer || !PlayerManager.playerWasChalice[1]))
			{
				yield return CupheadTime.WaitForSeconds(this, (!longPlayerAnimation) ? 1f : 2.5f);
				this.players[0].OnWinComplete();
				if (PlayerManager.Multiplayer)
				{
					this.players[1].OnWinComplete();
				}
			}
			else
			{
				if (PlayerManager.playerWasChalice[0])
				{
					this.players[0].OnWinComplete();
				}
				if (PlayerManager.Multiplayer && PlayerManager.playerWasChalice[1])
				{
					this.players[1].OnWinComplete();
				}
				yield return CupheadTime.WaitForSeconds(this, 1f);
				if (!PlayerManager.playerWasChalice[0])
				{
					this.players[0].OnWinComplete();
				}
				if (PlayerManager.Multiplayer && !PlayerManager.playerWasChalice[1])
				{
					this.players[1].OnWinComplete();
				}
			}
			if (!Level.PreviouslyWon || Level.PreviousDifficulty < Level.Mode.Normal || Level.PreviousLevel == Levels.Mausoleum)
			{
				if (!Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil && Level.PreviousLevel != Levels.DicePalaceMain && Level.PreviousLevel != Levels.Mausoleum && Level.PreviousLevelType == Level.Type.Battle)
				{
					if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
					{
						if (Level.Difficulty == Level.Mode.Easy && !PlayerData.Data.hasBeatenAnyDLCBossOnEasy && !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
						{
							MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.SimpleIngredient);
							PlayerData.Data.hasBeatenAnyDLCBossOnEasy = true;
							PlayerData.SaveCurrentFile();
						}
					}
					else if (Level.Difficulty == Level.Mode.Easy && !PlayerData.Data.hasBeatenAnyBossOnEasy && (!PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) || !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) || !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal)))
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.KingDice);
						PlayerData.Data.hasBeatenAnyBossOnEasy = true;
						PlayerData.SaveCurrentFile();
					}
					if (Level.Difficulty >= Level.Mode.Normal)
					{
						if (Level.PreviousLevel == Levels.Airplane)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.AirplaneIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.RumRunners)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.RumIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.OldMan)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.OldManIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.SnowCult)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SnowIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.FlyingCowboy)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.CowboyIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.Graveyard)
						{
							GameObject.Find("GhostDetective").GetComponent<MapNPCGraveyardGhost>().TalkAfterPlayerGotCharm();
						}
						else if (Level.PreviousLevel != Levels.Saltbaker)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SoulContract);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
					}
					if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) && Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.BackToKitchen);
					}
					int bossCounter = 0;
					for (int i = 0; i < Level.chaliceLevels.Length; i++)
					{
						if (PlayerData.Data.GetLevelData(Level.chaliceLevels[i]).completedAsChaliceP1)
						{
							bossCounter++;
						}
					}
				}
				else if (Level.SuperUnlocked)
				{
					MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Super);
					if (!PlayerData.Data.hasUnlockedFirstSuper)
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Mausoleum);
						PlayerData.Data.hasUnlockedFirstSuper = true;
						PlayerData.SaveCurrentFile();
					}
					longPlayerAnimation = false;
				}
			}
			while (MapEventNotification.Current && MapEventNotification.Current.showing)
			{
				yield return null;
			}
		}
		if (DLCManager.showAvailabilityPrompt)
		{
			yield return CupheadTime.WaitForSeconds(this, (!Level.Won) ? 1.5f : 0.5f);
			DLCManager.ResetAvailabilityPrompt();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable);
		}
		this.CurrentState = Map.State.Ready;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		InterruptingPrompt.SetCanInterrupt(true);
		Level.ResetPreviousLevelInfo();
		yield break;
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x00029430 File Offset: 0x00027630
	public void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "WorldMap", SceneLoader.SceneName);
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Exploring", true);
	}

	// Token: 0x040028D0 RID: 10448
	public MapResources MapResources;

	// Token: 0x040028D1 RID: 10449
	[SerializeField]
	public Map.Camera cameraProperties;

	// Token: 0x040028D2 RID: 10450
	[Space(10f)]
	[SerializeField]
	public AbstractMapInteractiveEntity firstNode;

	// Token: 0x040028D3 RID: 10451
	[SerializeField]
	public AbstractMapInteractiveEntity[] entryPoints;

	// Token: 0x040028D4 RID: 10452
	public MapUI ui;

	// Token: 0x040028D5 RID: 10453
	public Scenes scene;

	// Token: 0x040028D6 RID: 10454
	public CupheadMapCamera cupheadMapCamera;

	// Token: 0x040028D9 RID: 10457
	public Levels level;

	// Token: 0x040028DA RID: 10458
	public List<CoinPositionAndID> LevelCoinsIDs = new List<CoinPositionAndID>();

	// Token: 0x040028DB RID: 10459
	public int currentMusic;

	// Token: 0x02001109 RID: 4361
	public enum State
	{
		// Token: 0x04007874 RID: 30836
		Starting,
		// Token: 0x04007875 RID: 30837
		Ready,
		// Token: 0x04007876 RID: 30838
		Event,
		// Token: 0x04007877 RID: 30839
		Exiting,
		// Token: 0x04007878 RID: 30840
		Graveyard
	}

	// Token: 0x0200110A RID: 4362
	[Serializable]
	public class Camera
	{
		// Token: 0x04007879 RID: 30841
		public bool moveX = true;

		// Token: 0x0400787A RID: 30842
		public bool moveY = true;

		// Token: 0x0400787B RID: 30843
		public CupheadBounds bounds = new CupheadBounds(-6.4f, 6.4f, 3.6f, -3.6f);
	}
}
