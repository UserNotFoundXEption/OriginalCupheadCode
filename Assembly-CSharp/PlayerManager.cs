using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200057D RID: 1405
public static class PlayerManager
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x06003AD8 RID: 15064 RVA: 0x00111358 File Offset: 0x0010F558
	// (remove) Token: 0x06003AD9 RID: 15065 RVA: 0x0011138C File Offset: 0x0010F58C
	public static event PlayerManager.PlayerChangedDelegate OnPlayerJoinedEvent;

	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06003ADA RID: 15066 RVA: 0x001113C0 File Offset: 0x0010F5C0
	// (remove) Token: 0x06003ADB RID: 15067 RVA: 0x001113F4 File Offset: 0x0010F5F4
	public static event PlayerManager.PlayerChangedDelegate OnPlayerLeaveEvent;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06003ADC RID: 15068 RVA: 0x00111428 File Offset: 0x0010F628
	// (remove) Token: 0x06003ADD RID: 15069 RVA: 0x0011145C File Offset: 0x0010F65C
	public static event Action OnControlsChanged;

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06003ADE RID: 15070 RVA: 0x0002FDA6 File Offset: 0x0002DFA6
	public static bool ShouldShowJoinPrompt
	{
		get
		{
			return PlayerManager.playerSlots[1].joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed;
		}
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x00111490 File Offset: 0x0010F690
	public static void Awake()
	{
		PlayerManager.Multiplayer = false;
		PlayerManager.players = new Dictionary<int, AbstractPlayerController>();
		PlayerManager.players.Add(0, null);
		PlayerManager.players.Add(1, null);
		PlayerManager.playerInputs = new Dictionary<int, Player>();
		PlayerManager.playerInputs.Add(0, ReInput.players.GetPlayer(0));
		PlayerManager.playerInputs.Add(1, ReInput.players.GetPlayer(1));
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x001114FC File Offset: 0x0010F6FC
	public static void Init()
	{
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache0 == null)
		{
			PlayerManager.<>f__mg$cache0 = new SignInEventHandler(PlayerManager.OnUserSignedIn);
		}
		@interface.OnUserSignedIn += PlayerManager.<>f__mg$cache0;
		OnlineInterface interface2 = OnlineManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache1 == null)
		{
			PlayerManager.<>f__mg$cache1 = new SignOutEventHandler(PlayerManager.OnUserSignedOut);
		}
		interface2.OnUserSignedOut += PlayerManager.<>f__mg$cache1;
		if (PlayerManager.<>f__mg$cache2 == null)
		{
			PlayerManager.<>f__mg$cache2 = new Action<ControllerStatusChangedEventArgs>(PlayerManager.OnControllerConnected);
		}
		ReInput.ControllerConnectedEvent += PlayerManager.<>f__mg$cache2;
		if (PlayerManager.<>f__mg$cache3 == null)
		{
			PlayerManager.<>f__mg$cache3 = new Action<ControllerStatusChangedEventArgs>(PlayerManager.OnControllerDisconnected);
		}
		ReInput.ControllerDisconnectedEvent += PlayerManager.<>f__mg$cache3;
		PlmInterface interface3 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache4 == null)
		{
			PlayerManager.<>f__mg$cache4 = new OnUnconstrainedHandler(PlayerManager.OnUnconstrained);
		}
		interface3.OnUnconstrained += PlayerManager.<>f__mg$cache4;
		PlmInterface interface4 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache5 == null)
		{
			PlayerManager.<>f__mg$cache5 = new OnResumeHandler(PlayerManager.OnResume);
		}
		interface4.OnResume += PlayerManager.<>f__mg$cache5;
		PlmInterface interface5 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache6 == null)
		{
			PlayerManager.<>f__mg$cache6 = new OnSuspendHandler(PlayerManager.OnSuspend);
		}
		interface5.OnSuspend += PlayerManager.<>f__mg$cache6;
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0011162C File Offset: 0x0010F82C
	public static void SetPlayerCanJoin(PlayerId player, bool canJoin, bool promptBeforeJoin)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.canJoin = canJoin;
		playerSlot.promptBeforeJoin = promptBeforeJoin;
		if (!canJoin && playerSlot.joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed)
		{
			playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
		}
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x0011167C File Offset: 0x0010F87C
	public static void ClearJoinPrompt()
	{
		for (int i = 0; i < 2; i++)
		{
			if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed)
			{
				PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
			}
		}
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x001116BC File Offset: 0x0010F8BC
	public static void SetPlayerCanSwitch(PlayerId player, bool canSwitch)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.canSwitch = canSwitch;
		playerSlot.requestedSwitch = false;
	}

	// Token: 0x06003AE4 RID: 15076 RVA: 0x001116F4 File Offset: 0x0010F8F4
	public static void PlayerLeave(PlayerId player)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.Leaving;
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x0002FDB7 File Offset: 0x0002DFB7
	public static void OnChaliceCharmUnequipped(PlayerId player)
	{
		PlayerManager.playerWasChalice[(int)player] = false;
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x0002FDC1 File Offset: 0x0002DFC1
	public static void OnControllerConnected(ControllerStatusChangedEventArgs args)
	{
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x00111724 File Offset: 0x0010F924
	public static void Update()
	{
		if (InterruptingPrompt.IsInterrupting())
		{
			for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
			{
				if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined && PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed)
				{
					PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					Joystick joystick = CupheadInput.CheckForUnconnectedControllerPress();
					Player playerInput = PlayerManager.GetPlayerInput(playerId);
					if (joystick != null)
					{
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController;
						PlayerManager.playerSlots[i].controllerId = joystick.id;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = false;
						PlayerManager.playerSlots[i].lastController = 2;
						playerInput.controllers.AddController(joystick, true);
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, 2, PlayerManager.playerSlots[i].controllerId);
						PlayerManager.ControlsChanged();
					}
					if (!PlatformHelper.IsConsole && playerInput.GetAnyButtonDown())
					{
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.NoController;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = false;
						PlayerManager.ControlsChanged();
						PlayerManager.playerSlots[i].lastController = 0;
					}
				}
			}
			return;
		}
		for (int j = 0; j < PlayerManager.playerSlots.Length; j++)
		{
			if (PlayerManager.playerSlots[j].canJoin && PlayerManager.playerSlots[j].joinState != PlayerManager.PlayerSlot.JoinState.Joined)
			{
				PlayerId playerId2 = (j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				bool flag = false;
				Joystick joystick2 = CupheadInput.CheckForUnconnectedControllerPress();
				Player playerInput2 = PlayerManager.GetPlayerInput(playerId2);
				if (joystick2 != null)
				{
					flag = true;
					PlayerManager.playerSlots[j].controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController;
					PlayerManager.playerSlots[j].controllerId = joystick2.id;
				}
				else if (!PlatformHelper.IsConsole && ((!(SceneManager.GetActiveScene().name == "scene_title")) ? playerInput2.GetAnyButtonDown() : (playerInput2.controllers.Keyboard.GetAnyButtonDown() && PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.NotJoining)))
				{
					flag = true;
					PlayerManager.playerSlots[j].controllerState = PlayerManager.PlayerSlot.ControllerState.NoController;
				}
				if (flag)
				{
					if (PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.NotJoining && PlayerManager.playerSlots[j].promptBeforeJoin)
					{
						PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed;
					}
					else
					{
						bool flag2 = false;
						PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.JoinRequested;
						if (OnlineManager.Instance.Interface.SupportsMultipleUsers)
						{
							ulong value = (ulong)joystick2.systemId.Value;
							OnlineUser userForController = OnlineManager.Instance.Interface.GetUserForController(value);
							if (userForController != null && ((j == 0 && !userForController.Equals(OnlineManager.Instance.Interface.SecondaryUser)) || (j == 1 && !userForController.Equals(OnlineManager.Instance.Interface.MainUser))))
							{
								OnlineManager.Instance.Interface.SetUser(playerId2, userForController);
								flag2 = true;
							}
							else
							{
								OnlineManager.Instance.Interface.SignInUser(false, playerId2, value);
							}
						}
						else if (OnlineManager.Instance.Interface.SupportsUserSignIn && playerId2 == PlayerId.PlayerOne)
						{
							OnlineManager.Instance.Interface.SignInUser(false, playerId2, 0UL);
						}
						else
						{
							flag2 = true;
						}
						if (flag2)
						{
							if (joystick2 != null)
							{
								playerInput2.controllers.AddController(joystick2, true);
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId2].id, 2, PlayerManager.playerSlots[j].controllerId);
							}
							PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.Joined;
							if (playerId2 == PlayerId.PlayerTwo)
							{
								PlayerManager.Multiplayer = true;
							}
							PlayerManager.OnPlayerJoinedEvent(playerId2);
							AudioManager.Play("player_spawn");
						}
					}
				}
			}
		}
		for (int k = 0; k < PlayerManager.playerSlots.Length; k++)
		{
			if (OnlineManager.Instance.Interface.SupportsUserSignIn && PlayerManager.playerSlots[k].canSwitch && PlayerManager.playerSlots[k].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				if (OnlineManager.Instance.Interface.SupportsMultipleUsers || k != 1)
				{
					PlayerId playerId3 = (k != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					Player playerInput3 = PlayerManager.GetPlayerInput(playerId3);
					if (playerInput3.GetButtonDown(11))
					{
						PlayerManager.playerSlots[k].requestedSwitch = true;
						PlayerManager.playerSlots[(k + 1) % 2].requestedSwitch = false;
						ulong controllerId = 0UL;
						if (playerInput3.controllers.joystickCount > 0)
						{
							controllerId = (ulong)playerInput3.controllers.Joysticks[0].systemId.Value;
						}
						OnlineManager.Instance.Interface.SwitchUser(playerId3, controllerId);
					}
				}
			}
		}
		for (int l = 0; l < PlayerManager.playerSlots.Length; l++)
		{
			if (SceneLoader.CurrentlyLoading)
			{
				break;
			}
			if (PlayerManager.playerSlots[l].joinState == PlayerManager.PlayerSlot.JoinState.Leaving)
			{
				PlayerId playerId4 = (l != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				Player playerInput4 = PlayerManager.GetPlayerInput(playerId4);
				playerInput4.controllers.ClearControllersOfType<Joystick>();
				PlayerManager.playerSlots[l].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
				if (playerId4 == PlayerId.PlayerTwo)
				{
					PlayerManager.Multiplayer = false;
				}
				OnlineManager.Instance.Interface.SetRichPresenceActive(playerId4, false);
				OnlineManager.Instance.Interface.SetUser(playerId4, null);
				if (playerId4 == PlayerId.PlayerOne)
				{
					PlayerManager.shouldGoToStartScreen = true;
				}
				else if (PlayerManager.OnPlayerLeaveEvent != null)
				{
					PlayerManager.OnPlayerLeaveEvent(playerId4);
					AudioManager.Play("player_despawn");
				}
			}
		}
		for (int m = 0; m < PlayerManager.playerSlots.Length; m++)
		{
			if (PlayerManager.playerSlots[m].shouldAssignController)
			{
				PlayerId playerId5 = (m != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				Player playerInput5 = PlayerManager.GetPlayerInput(playerId5);
				playerInput5.controllers.AddController<Joystick>(PlayerManager.playerSlots[m].controllerId, true);
				ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId5].id, 2, PlayerManager.playerSlots[m].controllerId);
				PlayerManager.playerSlots[m].shouldAssignController = false;
			}
		}
		if (ControllerDisconnectedPrompt.Instance != null && !ControllerDisconnectedPrompt.Instance.Visible && ControllerDisconnectedPrompt.Instance.allowedToShow)
		{
			for (int n = 0; n < 2; n++)
			{
				PlayerId playerId6 = (n != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (PlayerManager.IsControllerDisconnected(playerId6, false))
				{
					ControllerDisconnectedPrompt.Instance.Show(playerId6);
					break;
				}
			}
		}
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			if (InterruptingPrompt.CanInterrupt() && PauseManager.state != PauseManager.State.Paused)
			{
				PauseManager.Pause();
				PlayerManager.pausedDueToPlm = true;
			}
		}
		else if (PlayerManager.pausedDueToPlm)
		{
			PauseManager.Unpause();
			PlayerManager.pausedDueToPlm = false;
		}
		if (PlayerManager.shouldGoToSlotSelect)
		{
			PlayerManager.goToSlotSelect();
			PlayerManager.shouldGoToSlotSelect = false;
		}
		if (PlayerManager.shouldGoToStartScreen)
		{
			PlayerManager.goToStartScreen();
			PlayerManager.shouldGoToStartScreen = false;
		}
		for (int num = 0; num < 2; num++)
		{
			PlayerId id = (num != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			Controller lastActiveController = PlayerManager.GetPlayerInput(id).controllers.GetLastActiveController();
			if (lastActiveController != null && lastActiveController.type != PlayerManager.playerSlots[num].lastController)
			{
				PlayerManager.playerSlots[num].lastController = lastActiveController.type;
				PlayerManager.ControlsChanged();
			}
		}
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x00111EF0 File Offset: 0x001100F0
	public static void ControllerRemapped(PlayerId playerId, bool usingController, int controllerId)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		PlayerManager.playerSlots[num].controllerState = ((!usingController) ? PlayerManager.PlayerSlot.ControllerState.NoController : PlayerManager.PlayerSlot.ControllerState.UsingController);
		PlayerManager.playerSlots[num].controllerId = controllerId;
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x0002FDC3 File Offset: 0x0002DFC3
	public static void ControlsChanged()
	{
		if (PlayerManager.OnControlsChanged != null)
		{
			PlayerManager.OnControlsChanged();
		}
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x00111F34 File Offset: 0x00110134
	public static void OnUserSignedIn(OnlineUser user)
	{
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			if (PlayerManager.playerSlots[i].canJoin && PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.JoinRequested)
			{
				OnlineManager.Instance.Interface.UpdateControllerMapping();
				if (user == null || (i == 0 && user.Equals(OnlineManager.Instance.Interface.SecondaryUser)) || (i == 1 && user.Equals(OnlineManager.Instance.Interface.MainUser)))
				{
					PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
				}
				else
				{
					PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					OnlineManager.Instance.Interface.SetUser(playerId, user);
					if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController)
					{
						PlayerManager.playerSlots[i].shouldAssignController = true;
					}
					PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.Joined;
					if (playerId == PlayerId.PlayerTwo)
					{
						PlayerManager.Multiplayer = true;
					}
					PlayerManager.OnPlayerJoinedEvent(playerId);
				}
			}
		}
		for (int j = 0; j < PlayerManager.playerSlots.Length; j++)
		{
			if (PlayerManager.playerSlots[j].canSwitch && PlayerManager.playerSlots[j].requestedSwitch && PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				OnlineManager.Instance.Interface.UpdateControllerMapping();
				PlayerManager.playerSlots[j].requestedSwitch = false;
				PlayerId player = (j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (user != null && !user.Equals(OnlineManager.Instance.Interface.MainUser) && !user.Equals(OnlineManager.Instance.Interface.SecondaryUser))
				{
					OnlineManager.Instance.Interface.SetUser(player, user);
					if (j == 0)
					{
						PlayerManager.shouldGoToSlotSelect = true;
					}
				}
			}
		}
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x00112124 File Offset: 0x00110324
	public static void OnUserSignedOut(PlayerId player, string name)
	{
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			return;
		}
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		if (playerSlot.requestedSwitch)
		{
			return;
		}
		PlayerManager.PlayerLeave(player);
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00112174 File Offset: 0x00110374
	public static void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			return;
		}
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController && PlayerManager.playerSlots[i].controllerId == args.controllerId && PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				PlayerManager.playerInputs[(int)playerId].controllers.RemoveController<Joystick>(args.controllerId);
				PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected;
				if (playerId == PlayerId.PlayerOne)
				{
					PlayerManager.player1DisconnectedControllerId = args.controllerId;
				}
			}
		}
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x0002FDD9 File Offset: 0x0002DFD9
	public static void OnSuspend()
	{
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x0002FDDB File Offset: 0x0002DFDB
	public static void OnResume()
	{
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x0002FDDD File Offset: 0x0002DFDD
	public static void OnCloudStorageInitialized(bool success)
	{
		if (!success)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			PlayerId player = PlayerId.PlayerOne;
			if (PlayerManager.<>f__mg$cache7 == null)
			{
				PlayerManager.<>f__mg$cache7 = new InitializeCloudStoreHandler(PlayerManager.OnCloudStorageInitialized);
			}
			@interface.InitializeCloudStorage(player, PlayerManager.<>f__mg$cache7);
			return;
		}
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x0002FE13 File Offset: 0x0002E013
	public static void OnUnconstrained()
	{
		PlayerManager.CheckForPairingsChanges();
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x00112238 File Offset: 0x00110438
	public static void CheckForPairingsChanges()
	{
		bool flag = OnlineManager.Instance.Interface.ControllerMappingChanged();
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				if (!OnlineManager.Instance.Interface.IsUserSignedIn(playerId))
				{
					PlayerManager.PlayerLeave(playerId);
					if (playerId == PlayerId.PlayerOne)
					{
						PlayerManager.PlayerLeave(PlayerId.PlayerTwo);
					}
				}
				else if (!flag)
				{
					if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController && PlayerManager.playerInputs[(int)playerId].controllers.joystickCount == 0)
					{
						PlayerManager.playerInputs[(int)playerId].controllers.AddController<Joystick>(PlayerManager.playerSlots[i].controllerId, true);
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, 2, PlayerManager.playerSlots[i].controllerId);
					}
				}
				else
				{
					List<ulong> controllersForUser = OnlineManager.Instance.Interface.GetControllersForUser(playerId);
					if (controllersForUser == null || controllersForUser.Count != 1)
					{
						PlayerManager.playerInputs[(int)playerId].controllers.ClearControllersOfType<Joystick>();
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = true;
					}
					else
					{
						ulong num = controllersForUser[0];
						foreach (Joystick joystick in ReInput.controllers.Joysticks)
						{
							if (joystick.systemId.Value == (long)num)
							{
								if (PlayerManager.playerInputs[(int)playerId].controllers.joystickCount > 0)
								{
								}
								PlayerManager.playerInputs[(int)playerId].controllers.ClearControllersOfType<Joystick>();
								PlayerManager.playerInputs[(int)playerId].controllers.AddController(joystick, true);
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, 2, PlayerManager.playerSlots[i].controllerId);
								PlayerManager.playerSlots[i].controllerId = joystick.id;
								break;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x0011248C File Offset: 0x0011068C
	public static void LoadControllerMappings(PlayerId player)
	{
		int num = (player != PlayerId.PlayerOne) ? 1 : 0;
		ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)player].id, 2, PlayerManager.playerSlots[num].controllerId);
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x001124D0 File Offset: 0x001106D0
	public static bool IsControllerDisconnected(PlayerId playerId, bool countWaitingForReconnectAsDisconnected = true)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		return PlayerManager.playerSlots[num].joinState == PlayerManager.PlayerSlot.JoinState.Joined && (PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.Disconnected || PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed || (countWaitingForReconnectAsDisconnected && PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.WaitingForReconnect));
	}

	// Token: 0x06003AF4 RID: 15092 RVA: 0x00112540 File Offset: 0x00110740
	public static void OnDisconnectPromptDisplayed(PlayerId playerId)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		PlayerManager.playerSlots[num].controllerState = PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed;
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x00112568 File Offset: 0x00110768
	public static void goToSlotSelect()
	{
		Cuphead.Current.controlMapper.Close(true);
		PlayerManager.playerSlots[0].canSwitch = false;
		PlayerManager.playerSlots[0].requestedSwitch = false;
		PlayerManager.playerSlots[0].canJoin = false;
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.playerSlots[1] = new PlayerManager.PlayerSlot();
		PlayerManager.Multiplayer = false;
		OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerTwo, null);
		SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06003AF6 RID: 15094 RVA: 0x0002FE1A File Offset: 0x0002E01A
	public static void goToStartScreen()
	{
		Cuphead.Current.controlMapper.Close(true);
		PlayerManager.ResetPlayers();
		if (StartScreenAudio.Instance != null)
		{
			Object.Destroy(StartScreenAudio.Instance.gameObject);
		}
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06003AF7 RID: 15095 RVA: 0x001125EC File Offset: 0x001107EC
	public static void ResetPlayers()
	{
		PlayerManager.playerSlots[0] = new PlayerManager.PlayerSlot();
		PlayerManager.playerSlots[1] = new PlayerManager.PlayerSlot();
		PlayerManager.GetPlayerInput(PlayerId.PlayerOne).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.Multiplayer = false;
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers)
		{
			OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerOne, null);
			OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerTwo, null);
		}
	}

	// Token: 0x06003AF8 RID: 15096 RVA: 0x0002FE5A File Offset: 0x0002E05A
	public static Player GetPlayerInput(PlayerId id)
	{
		return PlayerManager.playerInputs[(int)id];
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x0002FE67 File Offset: 0x0002E067
	public static AbstractPlayerController Current
	{
		get
		{
			return PlayerManager.GetPlayer(PlayerManager.currentId);
		}
	}

	// Token: 0x06003AFA RID: 15098 RVA: 0x0002FE73 File Offset: 0x0002E073
	public static void SetPlayer(PlayerId id, AbstractPlayerController player)
	{
		PlayerManager.players[(int)id] = player;
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x0002FE81 File Offset: 0x0002E081
	public static void ClearPlayer(PlayerId id)
	{
		PlayerManager.players[(int)id] = null;
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x0002FE8F File Offset: 0x0002E08F
	public static void ClearPlayers()
	{
		PlayerManager.currentId = PlayerId.PlayerOne;
		PlayerManager.players[0] = null;
		PlayerManager.players[1] = null;
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x0002FEAF File Offset: 0x0002E0AF
	public static AbstractPlayerController GetPlayer(PlayerId id)
	{
		return PlayerManager.players[(int)id];
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x0002FEBC File Offset: 0x0002E0BC
	public static T GetPlayer<T>(PlayerId id) where T : AbstractPlayerController
	{
		return PlayerManager.GetPlayer(id) as T;
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x0002FECE File Offset: 0x0002E0CE
	public static AbstractPlayerController GetRandom()
	{
		if (!PlayerManager.Multiplayer || !PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			return PlayerManager.players[0];
		}
		return PlayerManager.GetPlayer(EnumUtils.Random<PlayerId>());
	}

	// Token: 0x06003B00 RID: 15104 RVA: 0x00112670 File Offset: 0x00110870
	public static AbstractPlayerController GetNext()
	{
		if (!PlayerManager.Multiplayer || !PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			return PlayerManager.players[0];
		}
		if (!PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			return PlayerManager.players[1];
		}
		AbstractPlayerController result = PlayerManager.Current;
		PlayerId playerId = PlayerManager.currentId;
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			PlayerManager.currentId = PlayerId.PlayerTwo;
		}
		else
		{
			PlayerManager.currentId = PlayerId.PlayerOne;
		}
		return result;
	}

	// Token: 0x06003B01 RID: 15105 RVA: 0x0002FEFB File Offset: 0x0002E0FB
	public static bool DoesPlayerExist(PlayerId player)
	{
		return !(PlayerManager.players[(int)player] == null) && !PlayerManager.players[(int)player].IsDead;
	}

	// Token: 0x06003B02 RID: 15106 RVA: 0x0002FF2D File Offset: 0x0002E12D
	public static bool BothPlayersActive()
	{
		return PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) && PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo);
	}

	// Token: 0x06003B03 RID: 15107 RVA: 0x0002FF43 File Offset: 0x0002E143
	public static AbstractPlayerController GetFirst()
	{
		if (!PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			return PlayerManager.players[1];
		}
		return PlayerManager.players[0];
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x0002FF67 File Offset: 0x0002E167
	public static Dictionary<int, AbstractPlayerController>.ValueCollection GetAllPlayers()
	{
		return PlayerManager.players.Values;
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06003B05 RID: 15109 RVA: 0x001126EC File Offset: 0x001108EC
	public static int Count
	{
		get
		{
			int num = 0;
			foreach (int num2 in PlayerManager.players.Keys)
			{
				if (PlayerManager.DoesPlayerExist((PlayerId)num2) && !PlayerManager.GetPlayer((PlayerId)num2).IsDead)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06003B06 RID: 15110 RVA: 0x00112768 File Offset: 0x00110968
	public static Vector2 Center
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().center;
			}
			return (PlayerManager.players[0].center + PlayerManager.players[1].center) / 2f;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06003B07 RID: 15111 RVA: 0x001127D0 File Offset: 0x001109D0
	public static Vector2 CameraCenter
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().CameraCenter;
			}
			return (PlayerManager.players[0].center + PlayerManager.players[1].CameraCenter) / 2f;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06003B08 RID: 15112 RVA: 0x00112838 File Offset: 0x00110A38
	public static Vector2 TopPlayerPosition
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().transform.position;
			}
			float num = Mathf.Max(PlayerManager.players[0].transform.position.y, PlayerManager.players[1].transform.position.y);
			return new Vector2((PlayerManager.players[0].transform.position.x + PlayerManager.players[0].transform.position.x) / 2f, num);
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06003B09 RID: 15113 RVA: 0x0002FF73 File Offset: 0x0002E173
	public static float DamageMultiplier
	{
		get
		{
			if (PlayerManager.Count > 1)
			{
				return 0.5f;
			}
			return 1f;
		}
	}

	// Token: 0x04002F28 RID: 12072
	public const float SINGLE_PLAYER_DAMAGE_MULTIPLIER = 1f;

	// Token: 0x04002F29 RID: 12073
	public const float MULTIPLAYER_DAMAGE_MULTIPLIER = 0.5f;

	// Token: 0x04002F2A RID: 12074
	public static PlayerManager.PlayerSlot[] playerSlots = new PlayerManager.PlayerSlot[]
	{
		new PlayerManager.PlayerSlot(),
		new PlayerManager.PlayerSlot()
	};

	// Token: 0x04002F2B RID: 12075
	public static bool Multiplayer;

	// Token: 0x04002F2C RID: 12076
	public static bool shouldGoToSlotSelect = false;

	// Token: 0x04002F2D RID: 12077
	public static bool shouldGoToStartScreen = false;

	// Token: 0x04002F2E RID: 12078
	public static bool pausedDueToPlm = false;

	// Token: 0x04002F32 RID: 12082
	public static int player1DisconnectedControllerId;

	// Token: 0x04002F33 RID: 12083
	public static bool player1IsMugman;

	// Token: 0x04002F34 RID: 12084
	public static bool[] playerWasChalice = new bool[2];

	// Token: 0x04002F35 RID: 12085
	public static Dictionary<int, Player> playerInputs;

	// Token: 0x04002F36 RID: 12086
	public static Dictionary<int, AbstractPlayerController> players;

	// Token: 0x04002F37 RID: 12087
	public static PlayerId currentId;

	// Token: 0x04002F38 RID: 12088
	[CompilerGenerated]
	private static SignInEventHandler <>f__mg$cache0;

	// Token: 0x04002F39 RID: 12089
	[CompilerGenerated]
	private static SignOutEventHandler <>f__mg$cache1;

	// Token: 0x04002F3A RID: 12090
	[CompilerGenerated]
	private static Action<ControllerStatusChangedEventArgs> <>f__mg$cache2;

	// Token: 0x04002F3B RID: 12091
	[CompilerGenerated]
	private static Action<ControllerStatusChangedEventArgs> <>f__mg$cache3;

	// Token: 0x04002F3C RID: 12092
	[CompilerGenerated]
	private static OnUnconstrainedHandler <>f__mg$cache4;

	// Token: 0x04002F3D RID: 12093
	[CompilerGenerated]
	private static OnResumeHandler <>f__mg$cache5;

	// Token: 0x04002F3E RID: 12094
	[CompilerGenerated]
	private static OnSuspendHandler <>f__mg$cache6;

	// Token: 0x04002F3F RID: 12095
	[CompilerGenerated]
	private static InitializeCloudStoreHandler <>f__mg$cache7;

	// Token: 0x020011FC RID: 4604
	public class PlayerSlot
	{
		// Token: 0x04007D26 RID: 32038
		public bool canJoin;

		// Token: 0x04007D27 RID: 32039
		public PlayerManager.PlayerSlot.JoinState joinState;

		// Token: 0x04007D28 RID: 32040
		public PlayerManager.PlayerSlot.ControllerState controllerState;

		// Token: 0x04007D29 RID: 32041
		public bool canSwitch;

		// Token: 0x04007D2A RID: 32042
		public bool requestedSwitch;

		// Token: 0x04007D2B RID: 32043
		public bool promptBeforeJoin;

		// Token: 0x04007D2C RID: 32044
		public int controllerId;

		// Token: 0x04007D2D RID: 32045
		public bool shouldAssignController;

		// Token: 0x04007D2E RID: 32046
		public bool controllerDisconnectFromPlm;

		// Token: 0x04007D2F RID: 32047
		public ControllerType lastController = 20;

		// Token: 0x020015F6 RID: 5622
		public enum JoinState
		{
			// Token: 0x0400923A RID: 37434
			NotJoining,
			// Token: 0x0400923B RID: 37435
			JoinPromptDisplayed,
			// Token: 0x0400923C RID: 37436
			JoinRequested,
			// Token: 0x0400923D RID: 37437
			Joined,
			// Token: 0x0400923E RID: 37438
			Leaving
		}

		// Token: 0x020015F7 RID: 5623
		public enum ControllerState
		{
			// Token: 0x04009240 RID: 37440
			NoController,
			// Token: 0x04009241 RID: 37441
			UsingController,
			// Token: 0x04009242 RID: 37442
			Disconnected,
			// Token: 0x04009243 RID: 37443
			ReconnectPromptDisplayed,
			// Token: 0x04009244 RID: 37444
			WaitingForReconnect
		}
	}

	// Token: 0x020011FD RID: 4605
	// (Invoke) Token: 0x06007FD6 RID: 32726
	public delegate void PlayerChangedDelegate(PlayerId playerId);
}
