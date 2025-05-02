using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000655 RID: 1621
	public class UserDataStore_PlayerPrefs : UserDataStore
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x000363BD File Offset: 0x000345BD
		// (set) Token: 0x0600441B RID: 17435 RVA: 0x000363C5 File Offset: 0x000345C5
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600441C RID: 17436 RVA: 0x000363CE File Offset: 0x000345CE
		// (set) Token: 0x0600441D RID: 17437 RVA: 0x000363D6 File Offset: 0x000345D6
		public bool LoadDataOnStart
		{
			get
			{
				return this.loadDataOnStart;
			}
			set
			{
				this.loadDataOnStart = value;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x0600441E RID: 17438 RVA: 0x000363DF File Offset: 0x000345DF
		// (set) Token: 0x0600441F RID: 17439 RVA: 0x000363E7 File Offset: 0x000345E7
		public bool LoadJoystickAssignments
		{
			get
			{
				return this.loadJoystickAssignments;
			}
			set
			{
				this.loadJoystickAssignments = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06004420 RID: 17440 RVA: 0x000363F0 File Offset: 0x000345F0
		// (set) Token: 0x06004421 RID: 17441 RVA: 0x000363F8 File Offset: 0x000345F8
		public bool LoadKeyboardAssignments
		{
			get
			{
				return this.loadKeyboardAssignments;
			}
			set
			{
				this.loadKeyboardAssignments = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x00036401 File Offset: 0x00034601
		// (set) Token: 0x06004423 RID: 17443 RVA: 0x00036409 File Offset: 0x00034609
		public bool LoadMouseAssignments
		{
			get
			{
				return this.loadMouseAssignments;
			}
			set
			{
				this.loadMouseAssignments = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06004424 RID: 17444 RVA: 0x00036412 File Offset: 0x00034612
		// (set) Token: 0x06004425 RID: 17445 RVA: 0x0003641A File Offset: 0x0003461A
		public string PlayerPrefsKeyPrefix
		{
			get
			{
				return this.playerPrefsKeyPrefix;
			}
			set
			{
				this.playerPrefsKeyPrefix = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06004426 RID: 17446 RVA: 0x00036423 File Offset: 0x00034623
		public string playerPrefsKey_controllerAssignments
		{
			get
			{
				return string.Format("{0}_{1}", this.playerPrefsKeyPrefix, "ControllerAssignments");
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06004427 RID: 17447 RVA: 0x0003643A File Offset: 0x0003463A
		public bool loadControllerAssignments
		{
			get
			{
				return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
			}
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0003645B File Offset: 0x0003465B
		public override void Save()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0003647A File Offset: 0x0003467A
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0003649C File Offset: 0x0003469C
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000364BD File Offset: 0x000346BD
		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x000364DD File Offset: 0x000346DD
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0013BB84 File Offset: 0x00139D84
		public override void Load()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadAll();
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0013BBB0 File Offset: 0x00139DB0
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0013BBE0 File Offset: 0x00139DE0
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0013BC10 File Offset: 0x00139E10
		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0013BC3C File Offset: 0x00139E3C
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x000364FE File Offset: 0x000346FE
		public override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
				if (this.loadControllerAssignments && ReInput.controllers.joystickCount > 0)
				{
					this.SaveControllerAssignments();
				}
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0013BC6C File Offset: 0x00139E6C
		public override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == 2)
			{
				int num = this.LoadJoystickData(args.controllerId);
				if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
				{
					base.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
				}
				if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
				{
					this.SaveControllerAssignments();
				}
				this.wasJoystickEverDetected = true;
			}
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x00036533 File Offset: 0x00034733
		public override void OnControllerPreDiscconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == 2)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x00036559 File Offset: 0x00034759
		public override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0013BCF0 File Offset: 0x00139EF0
		public int LoadAll()
		{
			int num = 0;
			if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
			{
				num++;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				num += this.LoadPlayerDataNow(allPlayers[i]);
			}
			return num + this.LoadAllJoystickCalibrationData();
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00036579 File Offset: 0x00034779
		public int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0013BD58 File Offset: 0x00139F58
		public int LoadPlayerDataNow(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			num += this.LoadInputBehaviors(player.id);
			num += this.LoadControllerMaps(player.id, 0, 0);
			num += this.LoadControllerMaps(player.id, 1, 0);
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				num += this.LoadControllerMaps(player.id, 2, joystick.id);
			}
			return num;
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0013BE04 File Offset: 0x0013A004
		public int LoadAllJoystickCalibrationData()
		{
			int num = 0;
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				num += this.LoadJoystickCalibrationData(joysticks[i]);
			}
			return num;
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0003658C File Offset: 0x0003478C
		public int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			return (!joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick))) ? 0 : 1;
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x000365AF File Offset: 0x000347AF
		public int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0013BE48 File Offset: 0x0013A048
		public int LoadJoystickData(int joystickId)
		{
			int num = 0;
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(2, joystickId))
				{
					num += this.LoadControllerMaps(player.id, 2, joystickId);
				}
			}
			return num + this.LoadJoystickCalibrationData(joystickId);
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0013BEB4 File Offset: 0x0013A0B4
		public int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			num += this.LoadControllerMaps(playerId, controllerType, controllerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0013BEDC File Offset: 0x0013A0DC
		public int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == 2)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0013BF00 File Offset: 0x0013A100
		public int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return num;
			}
			Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
			if (controller == null)
			{
				return num;
			}
			List<UserDataStore_PlayerPrefs.SavedControllerMapData> allControllerMapsXml = this.GetAllControllerMapsXml(player, true, controller);
			if (allControllerMapsXml.Count == 0)
			{
				return num;
			}
			num += player.controllers.maps.AddMapsFromXml(controllerType, controllerId, UserDataStore_PlayerPrefs.SavedControllerMapData.GetXmlStringList(allControllerMapsXml));
			this.AddDefaultMappingsForNewActions(player, allControllerMapsXml, controllerType, controllerId);
			return num;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0013BF78 File Offset: 0x0013A178
		public int LoadInputBehaviors(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			IList<InputBehavior> inputBehaviors = ReInput.mapping.GetInputBehaviors(player.id);
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				num += this.LoadInputBehaviorNow(player, inputBehaviors[i]);
			}
			return num;
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0013BFD8 File Offset: 0x0013A1D8
		public int LoadInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return 0;
			}
			return this.LoadInputBehaviorNow(player, inputBehavior);
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0013C018 File Offset: 0x0013A218
		public int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return 0;
			}
			string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
			if (inputBehaviorXml == null || inputBehaviorXml == string.Empty)
			{
				return 0;
			}
			return (!inputBehavior.ImportXmlString(inputBehaviorXml)) ? 0 : 1;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0013C06C File Offset: 0x0013A26C
		public bool LoadControllerAssignmentsNow()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = this.LoadControllerAssignmentData();
				if (controllerAssignmentSaveInfo == null)
				{
					return false;
				}
				if (this.loadKeyboardAssignments || this.loadMouseAssignments)
				{
					this.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo);
				}
				if (this.loadJoystickAssignments)
				{
					this.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo);
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0013C0DC File Offset: 0x0013A2DC
		public bool LoadKeyboardAndMouseAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player.id)];
						if (this.loadKeyboardAssignments)
						{
							player.controllers.hasKeyboard = playerInfo.hasKeyboard;
						}
						if (this.loadMouseAssignments)
						{
							player.controllers.hasMouse = playerInfo.hasMouse;
						}
					}
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0013C1CC File Offset: 0x0013A3CC
		public bool LoadJoystickAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (ReInput.controllers.joystickCount == 0)
				{
					return false;
				}
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					player.controllers.ClearControllersOfType(2);
				}
				List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> list = (!this.loadJoystickAssignments) ? null : new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>();
				foreach (Player player2 in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player2.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player2.id)];
						for (int i = 0; i < playerInfo.joystickCount; i++)
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo2 = playerInfo.joysticks[i];
							if (joystickInfo2 != null)
							{
								Joystick joystick = this.FindJoystickPrecise(joystickInfo2);
								if (joystick != null)
								{
									if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == joystick) == null)
									{
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id));
									}
									player2.controllers.AddController(joystick, false);
								}
							}
						}
					}
				}
				if (this.allowImpreciseJoystickAssignmentMatching)
				{
					foreach (Player player3 in ReInput.players.AllPlayers)
					{
						if (data.ContainsPlayer(player3.id))
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo2 = data.players[data.IndexOfPlayer(player3.id)];
							for (int j = 0; j < playerInfo2.joystickCount; j++)
							{
								UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerInfo2.joysticks[j];
								if (joystickInfo != null)
								{
									Joystick joystick2 = null;
									int num = list.FindIndex((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.oldJoystickId == joystickInfo.id);
									if (num >= 0)
									{
										joystick2 = list[num].joystick;
									}
									else
									{
										List<Joystick> list2;
										if (!this.TryFindJoysticksImprecise(joystickInfo, out list2))
										{
											goto IL_30F;
										}
										using (List<Joystick>.Enumerator enumerator4 = list2.GetEnumerator())
										{
											while (enumerator4.MoveNext())
											{
												Joystick match = enumerator4.Current;
												if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == match) == null)
												{
													joystick2 = match;
													break;
												}
											}
										}
										if (joystick2 == null)
										{
											goto IL_30F;
										}
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
									}
									player3.controllers.AddController(joystick2, false);
								}
								IL_30F:;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (ReInput.configuration.autoAssignJoysticks)
			{
				ReInput.controllers.AutoAssignJoysticks();
			}
			return true;
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0013C5C0 File Offset: 0x0013A7C0
		public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
		{
			UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo result;
			try
			{
				if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
				{
					result = null;
				}
				else
				{
					string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
					if (string.IsNullOrEmpty(@string))
					{
						result = null;
					}
					else
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(@string);
						if (controllerAssignmentSaveInfo == null || controllerAssignmentSaveInfo.playerCount == 0)
						{
							result = null;
						}
						else
						{
							result = controllerAssignmentSaveInfo;
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0013C644 File Offset: 0x0013A844
		public IEnumerator LoadJoystickAssignmentsDeferred()
		{
			this.deferredJoystickAssignmentLoadPending = true;
			yield return new WaitForEndOfFrame();
			if (!ReInput.isReady)
			{
				yield break;
			}
			if (this.LoadJoystickAssignmentsNow(null))
			{
			}
			this.SaveControllerAssignments();
			this.deferredJoystickAssignmentLoadPending = false;
			yield break;
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0013C660 File Offset: 0x0013A860
		public void SaveAll()
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x000365C2 File Offset: 0x000347C2
		public void SavePlayerDataNow(int playerId)
		{
			this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
			PlayerPrefs.Save();
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0013C6BC File Offset: 0x0013A8BC
		public void SavePlayerDataNow(Player player)
		{
			if (player == null)
			{
				return;
			}
			PlayerSaveData saveData = player.GetSaveData(true);
			this.SaveInputBehaviors(player, saveData);
			this.SaveControllerMaps(player, saveData);
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0013C6E8 File Offset: 0x0013A8E8
		public void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x000365DA File Offset: 0x000347DA
		public void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0013C724 File Offset: 0x0013A924
		public void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString());
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0013C758 File Offset: 0x0013A958
		public void SaveJoystickData(int joystickId)
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(2, joystickId))
				{
					this.SaveControllerMaps(player.id, 2, joystickId);
				}
			}
			this.SaveJoystickCalibrationData(joystickId);
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x000365ED File Offset: 0x000347ED
		public void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
			PlayerPrefs.Save();
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x00036605 File Offset: 0x00034805
		public void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == 2)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0013C7BC File Offset: 0x0013A9BC
		public void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			foreach (ControllerMapSaveData saveData in playerSaveData.AllControllerMapSaveData)
			{
				this.SaveControllerMap(player, saveData);
			}
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0013C818 File Offset: 0x0013AA18
		public void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(controllerType, controllerId))
			{
				return;
			}
			ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
			if (mapSaveData == null)
			{
				return;
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				this.SaveControllerMap(player, mapSaveData[i]);
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0013C884 File Offset: 0x0013AA84
		public void SaveControllerMap(Player player, ControllerMapSaveData saveData)
		{
			string text = this.GetControllerMapPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId);
			PlayerPrefs.SetString(text, saveData.map.ToXmlString());
			text = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId);
			PlayerPrefs.SetString(text, this.GetAllActionIdsString());
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0013C8E4 File Offset: 0x0013AAE4
		public void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
		{
			if (player == null)
			{
				return;
			}
			InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
			for (int i = 0; i < inputBehaviors.Length; i++)
			{
				this.SaveInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0013C920 File Offset: 0x0013AB20
		public void SaveInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.SaveInputBehaviorNow(player, inputBehavior);
			PlayerPrefs.Save();
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0013C964 File Offset: 0x0013AB64
		public void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id);
			PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString());
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0013C998 File Offset: 0x0013AB98
		public bool SaveControllerAssignments()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
				for (int i = 0; i < ReInput.players.allPlayerCount; i++)
				{
					Player player = ReInput.players.AllPlayers[i];
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
					controllerAssignmentSaveInfo.players[i] = playerInfo;
					playerInfo.id = player.id;
					playerInfo.hasKeyboard = player.controllers.hasKeyboard;
					playerInfo.hasMouse = player.controllers.hasMouse;
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] array = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
					playerInfo.joysticks = array;
					for (int j = 0; j < player.controllers.joystickCount; j++)
					{
						Joystick joystick = player.controllers.Joysticks[j];
						array[j] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo
						{
							instanceGuid = joystick.deviceInstanceGuid,
							id = joystick.id,
							hardwareIdentifier = joystick.hardwareIdentifier
						};
					}
				}
				PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson(controllerAssignmentSaveInfo));
				PlayerPrefs.Save();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0013CAE4 File Offset: 0x0013ACE4
		public bool ControllerAssignmentSaveDataExists()
		{
			if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
			{
				return false;
			}
			string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
			return !string.IsNullOrEmpty(@string);
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0013CB20 File Offset: 0x0013AD20
		public string GetBasePlayerPrefsKey(Player player)
		{
			string str = this.playerPrefsKeyPrefix;
			return str + "|playerName=" + player.name;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0013CB48 File Offset: 0x0013AD48
		public string GetControllerMapPlayerPrefsKey(Player player, Controller controller, int categoryId, int layoutId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap";
			text = text + "|controllerMapType=" + controller.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				categoryId,
				"|layoutId=",
				layoutId
			});
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier;
			if (controller.type == 2)
			{
				text = text + "|hardwareGuid=" + ((Joystick)controller).hardwareTypeGuid.ToString();
			}
			return text;
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0013CBF4 File Offset: 0x0013ADF4
		public string GetControllerMapKnownActionIdsPlayerPrefsKey(Player player, Controller controller, int categoryId, int layoutId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap_KnownActionIds";
			text = text + "|controllerMapType=" + controller.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				categoryId,
				"|layoutId=",
				layoutId
			});
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier;
			if (controller.type == 2)
			{
				text = text + "|hardwareGuid=" + ((Joystick)controller).hardwareTypeGuid.ToString();
			}
			return text;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0013CCA0 File Offset: 0x0013AEA0
		public string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
		{
			string str = this.playerPrefsKeyPrefix;
			str += "|dataType=CalibrationMap";
			str = str + "|controllerType=" + joystick.type.ToString();
			str = str + "|hardwareIdentifier=" + joystick.hardwareIdentifier;
			return str + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0013CD14 File Offset: 0x0013AF14
		public string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			return text + "|id=" + inputBehaviorId;
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0013CD48 File Offset: 0x0013AF48
		public string GetControllerMapXml(Player player, Controller controller, int categoryId, int layoutId)
		{
			string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId);
			if (!PlayerPrefs.HasKey(controllerMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(controllerMapPlayerPrefsKey);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0013CD78 File Offset: 0x0013AF78
		public List<int> GetControllerMapKnownActionIds(Player player, Controller controller, int categoryId, int layoutId)
		{
			List<int> list = new List<int>();
			string controllerMapKnownActionIdsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId);
			if (!PlayerPrefs.HasKey(controllerMapKnownActionIdsPlayerPrefsKey))
			{
				return list;
			}
			string @string = PlayerPrefs.GetString(controllerMapKnownActionIdsPlayerPrefsKey);
			if (string.IsNullOrEmpty(@string))
			{
				return list;
			}
			string[] array = @string.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					int item;
					if (int.TryParse(array[i], out item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0013CE10 File Offset: 0x0013B010
		public List<UserDataStore_PlayerPrefs.SavedControllerMapData> GetAllControllerMapsXml(Player player, bool userAssignableMapsOnly, Controller controller)
		{
			List<UserDataStore_PlayerPrefs.SavedControllerMapData> list = new List<UserDataStore_PlayerPrefs.SavedControllerMapData>();
			IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				InputMapCategory inputMapCategory = mapCategories[i];
				if (!userAssignableMapsOnly || inputMapCategory.userAssignable)
				{
					IList<InputLayout> list2 = ReInput.mapping.MapLayouts(controller.type);
					for (int j = 0; j < list2.Count; j++)
					{
						InputLayout inputLayout = list2[j];
						string controllerMapXml = this.GetControllerMapXml(player, controller, inputMapCategory.id, inputLayout.id);
						if (!(controllerMapXml == string.Empty))
						{
							List<int> controllerMapKnownActionIds = this.GetControllerMapKnownActionIds(player, controller, inputMapCategory.id, inputLayout.id);
							list.Add(new UserDataStore_PlayerPrefs.SavedControllerMapData(controllerMapXml, controllerMapKnownActionIds));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0013CEF0 File Offset: 0x0013B0F0
		public string GetJoystickCalibrationMapXml(Joystick joystick)
		{
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			if (!PlayerPrefs.HasKey(joystickCalibrationMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(joystickCalibrationMapPlayerPrefsKey);
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0013CF1C File Offset: 0x0013B11C
		public string GetInputBehaviorXml(Player player, int id)
		{
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
			if (!PlayerPrefs.HasKey(inputBehaviorPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(inputBehaviorPlayerPrefsKey);
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0013CF4C File Offset: 0x0013B14C
		public void AddDefaultMappingsForNewActions(Player player, List<UserDataStore_PlayerPrefs.SavedControllerMapData> savedData, ControllerType controllerType, int controllerId)
		{
			if (player == null || savedData == null)
			{
				return;
			}
			List<int> allActionIds = this.GetAllActionIds();
			for (int i = 0; i < savedData.Count; i++)
			{
				UserDataStore_PlayerPrefs.SavedControllerMapData savedControllerMapData = savedData[i];
				if (savedControllerMapData != null)
				{
					if (savedControllerMapData.knownActionIds != null && savedControllerMapData.knownActionIds.Count != 0)
					{
						ControllerMap controllerMap = ControllerMap.CreateFromXml(controllerType, savedData[i].xml);
						if (controllerMap != null)
						{
							ControllerMap map = player.controllers.maps.GetMap(controllerType, controllerId, controllerMap.categoryId, controllerMap.layoutId);
							if (map != null)
							{
								ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(ReInput.controllers.GetController(controllerType, controllerId), controllerMap.categoryId, controllerMap.layoutId);
								if (controllerMapInstance != null)
								{
									List<int> list = new List<int>();
									foreach (int item in allActionIds)
									{
										if (!savedControllerMapData.knownActionIds.Contains(item))
										{
											list.Add(item);
										}
									}
									if (list.Count != 0)
									{
										foreach (ActionElementMap actionElementMap in controllerMapInstance.AllMaps)
										{
											if (list.Contains(actionElementMap.actionId))
											{
												if (!map.DoesElementAssignmentConflict(actionElementMap))
												{
													ElementAssignment elementAssignment;
													elementAssignment..ctor(controllerType, actionElementMap.elementType, actionElementMap.elementIdentifierId, actionElementMap.axisRange, actionElementMap.keyCode, actionElementMap.modifierKeyFlags, actionElementMap.actionId, actionElementMap.axisContribution, actionElementMap.invert);
													map.CreateElementMap(elementAssignment);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0013D170 File Offset: 0x0013B370
		public List<int> GetAllActionIds()
		{
			List<int> list = new List<int>();
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int i = 0; i < actions.Count; i++)
			{
				list.Add(actions[i].id);
			}
			return list;
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0013D1B8 File Offset: 0x0013B3B8
		public string GetAllActionIdsString()
		{
			string text = string.Empty;
			List<int> allActionIds = this.GetAllActionIds();
			for (int i = 0; i < allActionIds.Count; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				text += allActionIds[i];
			}
			return text;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0013D210 File Offset: 0x0013B410
		public Joystick FindJoystickPrecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
		{
			if (joystickInfo == null)
			{
				return null;
			}
			if (joystickInfo.instanceGuid == Guid.Empty)
			{
				return null;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].deviceInstanceGuid == joystickInfo.instanceGuid)
				{
					return joysticks[i];
				}
			}
			return null;
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0013D284 File Offset: 0x0013B484
		public bool TryFindJoysticksImprecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches)
		{
			matches = null;
			if (joystickInfo == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
			{
				return false;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (string.Equals(joysticks[i].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
				{
					if (matches == null)
					{
						matches = new List<Joystick>();
					}
					matches.Add(joysticks[i]);
				}
			}
			return matches != null;
		}

		// Token: 0x0400351A RID: 13594
		public const string thisScriptName = "UserDataStore_PlayerPrefs";

		// Token: 0x0400351B RID: 13595
		public const string editorLoadedMessage = "\nIf unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";

		// Token: 0x0400351C RID: 13596
		public const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";

		// Token: 0x0400351D RID: 13597
		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		public bool isEnabled = true;

		// Token: 0x0400351E RID: 13598
		[Tooltip("Should saved data be loaded on start?")]
		[SerializeField]
		public bool loadDataOnStart = true;

		// Token: 0x0400351F RID: 13599
		[Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
		[SerializeField]
		public bool loadJoystickAssignments = true;

		// Token: 0x04003520 RID: 13600
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		[SerializeField]
		public bool loadKeyboardAssignments = true;

		// Token: 0x04003521 RID: 13601
		[Tooltip("Should Player Mouse assignments be saved and loaded?")]
		[SerializeField]
		public bool loadMouseAssignments = true;

		// Token: 0x04003522 RID: 13602
		[Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
		[SerializeField]
		public string playerPrefsKeyPrefix = "RewiredSaveData";

		// Token: 0x04003523 RID: 13603
		public bool allowImpreciseJoystickAssignmentMatching = true;

		// Token: 0x04003524 RID: 13604
		public bool deferredJoystickAssignmentLoadPending;

		// Token: 0x04003525 RID: 13605
		public bool wasJoystickEverDetected;

		// Token: 0x020012E7 RID: 4839
		public class SavedControllerMapData
		{
			// Token: 0x060083AB RID: 33707 RVA: 0x00057C8A File Offset: 0x00055E8A
			public SavedControllerMapData(string xml, List<int> knownActionIds)
			{
				this.xml = xml;
				this.knownActionIds = knownActionIds;
			}

			// Token: 0x060083AC RID: 33708 RVA: 0x0029EDA0 File Offset: 0x0029CFA0
			public static List<string> GetXmlStringList(List<UserDataStore_PlayerPrefs.SavedControllerMapData> data)
			{
				List<string> list = new List<string>();
				if (data == null)
				{
					return list;
				}
				for (int i = 0; i < data.Count; i++)
				{
					if (data[i] != null)
					{
						if (!string.IsNullOrEmpty(data[i].xml))
						{
							list.Add(data[i].xml);
						}
					}
				}
				return list;
			}

			// Token: 0x040081B5 RID: 33205
			public string xml;

			// Token: 0x040081B6 RID: 33206
			public List<int> knownActionIds;
		}

		// Token: 0x020012E8 RID: 4840
		public class ControllerAssignmentSaveInfo
		{
			// Token: 0x060083AD RID: 33709 RVA: 0x00057CA0 File Offset: 0x00055EA0
			public ControllerAssignmentSaveInfo()
			{
			}

			// Token: 0x060083AE RID: 33710 RVA: 0x0029EE14 File Offset: 0x0029D014
			public ControllerAssignmentSaveInfo(int playerCount)
			{
				this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
				for (int i = 0; i < playerCount; i++)
				{
					this.players[i] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
				}
			}

			// Token: 0x170019D2 RID: 6610
			// (get) Token: 0x060083AF RID: 33711 RVA: 0x00057CA8 File Offset: 0x00055EA8
			public int playerCount
			{
				get
				{
					return (this.players == null) ? 0 : this.players.Length;
				}
			}

			// Token: 0x060083B0 RID: 33712 RVA: 0x0029EE54 File Offset: 0x0029D054
			public int IndexOfPlayer(int playerId)
			{
				for (int i = 0; i < this.playerCount; i++)
				{
					if (this.players[i] != null)
					{
						if (this.players[i].id == playerId)
						{
							return i;
						}
					}
				}
				return -1;
			}

			// Token: 0x060083B1 RID: 33713 RVA: 0x00057CC3 File Offset: 0x00055EC3
			public bool ContainsPlayer(int playerId)
			{
				return this.IndexOfPlayer(playerId) >= 0;
			}

			// Token: 0x040081B7 RID: 33207
			public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

			// Token: 0x02001603 RID: 5635
			public class PlayerInfo
			{
				// Token: 0x17001A45 RID: 6725
				// (get) Token: 0x060087B6 RID: 34742 RVA: 0x0005BF23 File Offset: 0x0005A123
				public int joystickCount
				{
					get
					{
						return (this.joysticks == null) ? 0 : this.joysticks.Length;
					}
				}

				// Token: 0x060087B7 RID: 34743 RVA: 0x002A5BCC File Offset: 0x002A3DCC
				public int IndexOfJoystick(int joystickId)
				{
					for (int i = 0; i < this.joystickCount; i++)
					{
						if (this.joysticks[i] != null)
						{
							if (this.joysticks[i].id == joystickId)
							{
								return i;
							}
						}
					}
					return -1;
				}

				// Token: 0x060087B8 RID: 34744 RVA: 0x0005BF3E File Offset: 0x0005A13E
				public bool ContainsJoystick(int joystickId)
				{
					return this.IndexOfJoystick(joystickId) >= 0;
				}

				// Token: 0x04009269 RID: 37481
				public int id;

				// Token: 0x0400926A RID: 37482
				public bool hasKeyboard;

				// Token: 0x0400926B RID: 37483
				public bool hasMouse;

				// Token: 0x0400926C RID: 37484
				public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;
			}

			// Token: 0x02001604 RID: 5636
			public class JoystickInfo
			{
				// Token: 0x0400926D RID: 37485
				public Guid instanceGuid;

				// Token: 0x0400926E RID: 37486
				public string hardwareIdentifier;

				// Token: 0x0400926F RID: 37487
				public int id;
			}
		}

		// Token: 0x020012E9 RID: 4841
		public class JoystickAssignmentHistoryInfo
		{
			// Token: 0x060083B2 RID: 33714 RVA: 0x00057CD2 File Offset: 0x00055ED2
			public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
			{
				if (joystick == null)
				{
					throw new ArgumentNullException("joystick");
				}
				this.joystick = joystick;
				this.oldJoystickId = oldJoystickId;
			}

			// Token: 0x040081B8 RID: 33208
			public readonly Joystick joystick;

			// Token: 0x040081B9 RID: 33209
			public readonly int oldJoystickId;
		}
	}
}
