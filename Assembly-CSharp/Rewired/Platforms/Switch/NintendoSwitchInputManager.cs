using System;
using System.Collections.Generic;
using Rewired.Data;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch
{
	// Token: 0x02000658 RID: 1624
	[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x06004499 RID: 17561 RVA: 0x00036777 File Offset: 0x00034977
		public object Initialize(Platform platform, ConfigVars configVars)
		{
			return null;
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0003677A File Offset: 0x0003497A
		public void Deinitialize()
		{
		}

		// Token: 0x04003527 RID: 13607
		[SerializeField]
		public NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

		// Token: 0x020012EE RID: 4846
		[Serializable]
		public class UserData : IKeyedData<int>
		{
			// Token: 0x170019D5 RID: 6613
			// (get) Token: 0x060083C0 RID: 33728 RVA: 0x00057D75 File Offset: 0x00055F75
			// (set) Token: 0x060083C1 RID: 33729 RVA: 0x00057D7D File Offset: 0x00055F7D
			public int allowedNpadStyles
			{
				get
				{
					return this._allowedNpadStyles;
				}
				set
				{
					this._allowedNpadStyles = value;
				}
			}

			// Token: 0x170019D6 RID: 6614
			// (get) Token: 0x060083C2 RID: 33730 RVA: 0x00057D86 File Offset: 0x00055F86
			// (set) Token: 0x060083C3 RID: 33731 RVA: 0x00057D8E File Offset: 0x00055F8E
			public int joyConGripStyle
			{
				get
				{
					return this._joyConGripStyle;
				}
				set
				{
					this._joyConGripStyle = value;
				}
			}

			// Token: 0x170019D7 RID: 6615
			// (get) Token: 0x060083C4 RID: 33732 RVA: 0x00057D97 File Offset: 0x00055F97
			// (set) Token: 0x060083C5 RID: 33733 RVA: 0x00057D9F File Offset: 0x00055F9F
			public bool adjustIMUsForGripStyle
			{
				get
				{
					return this._adjustIMUsForGripStyle;
				}
				set
				{
					this._adjustIMUsForGripStyle = value;
				}
			}

			// Token: 0x170019D8 RID: 6616
			// (get) Token: 0x060083C6 RID: 33734 RVA: 0x00057DA8 File Offset: 0x00055FA8
			// (set) Token: 0x060083C7 RID: 33735 RVA: 0x00057DB0 File Offset: 0x00055FB0
			public int handheldActivationMode
			{
				get
				{
					return this._handheldActivationMode;
				}
				set
				{
					this._handheldActivationMode = value;
				}
			}

			// Token: 0x170019D9 RID: 6617
			// (get) Token: 0x060083C8 RID: 33736 RVA: 0x00057DB9 File Offset: 0x00055FB9
			// (set) Token: 0x060083C9 RID: 33737 RVA: 0x00057DC1 File Offset: 0x00055FC1
			public bool assignJoysticksByNpadId
			{
				get
				{
					return this._assignJoysticksByNpadId;
				}
				set
				{
					this._assignJoysticksByNpadId = value;
				}
			}

			// Token: 0x170019DA RID: 6618
			// (get) Token: 0x060083CA RID: 33738 RVA: 0x00057DCA File Offset: 0x00055FCA
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x170019DB RID: 6619
			// (get) Token: 0x060083CB RID: 33739 RVA: 0x00057DD2 File Offset: 0x00055FD2
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x170019DC RID: 6620
			// (get) Token: 0x060083CC RID: 33740 RVA: 0x00057DDA File Offset: 0x00055FDA
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x170019DD RID: 6621
			// (get) Token: 0x060083CD RID: 33741 RVA: 0x00057DE2 File Offset: 0x00055FE2
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x170019DE RID: 6622
			// (get) Token: 0x060083CE RID: 33742 RVA: 0x00057DEA File Offset: 0x00055FEA
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x170019DF RID: 6623
			// (get) Token: 0x060083CF RID: 33743 RVA: 0x00057DF2 File Offset: 0x00055FF2
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x170019E0 RID: 6624
			// (get) Token: 0x060083D0 RID: 33744 RVA: 0x00057DFA File Offset: 0x00055FFA
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x170019E1 RID: 6625
			// (get) Token: 0x060083D1 RID: 33745 RVA: 0x00057E02 File Offset: 0x00056002
			public NintendoSwitchInputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x170019E2 RID: 6626
			// (get) Token: 0x060083D2 RID: 33746 RVA: 0x00057E0A File Offset: 0x0005600A
			public NintendoSwitchInputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x170019E3 RID: 6627
			// (get) Token: 0x060083D3 RID: 33747 RVA: 0x00057E12 File Offset: 0x00056012
			public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x170019E4 RID: 6628
			// (get) Token: 0x060083D4 RID: 33748 RVA: 0x0029EFE4 File Offset: 0x0029D1E4
			public Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					dictionary.Add(0, new object[]
					{
						new Func<int>(this.get_allowedNpadStyles),
						new Action<int>(delegate(int x)
						{
							this.allowedNpadStyles = x;
						})
					});
					dictionary.Add(1, new object[]
					{
						new Func<int>(this.get_joyConGripStyle),
						new Action<int>(delegate(int x)
						{
							this.joyConGripStyle = x;
						})
					});
					dictionary.Add(2, new object[]
					{
						new Func<bool>(this.get_adjustIMUsForGripStyle),
						new Action<bool>(delegate(bool x)
						{
							this.adjustIMUsForGripStyle = x;
						})
					});
					dictionary.Add(3, new object[]
					{
						new Func<int>(this.get_handheldActivationMode),
						new Action<int>(delegate(int x)
						{
							this.handheldActivationMode = x;
						})
					});
					dictionary.Add(4, new object[]
					{
						new Func<bool>(this.get_assignJoysticksByNpadId),
						new Action<bool>(delegate(bool x)
						{
							this.assignJoysticksByNpadId = x;
						})
					});
					Dictionary<int, object[]> dictionary2 = dictionary;
					int key = 5;
					object[] array = new object[2];
					array[0] = new Func<object>(this.get_npadNo1);
					dictionary2.Add(key, array);
					Dictionary<int, object[]> dictionary3 = dictionary;
					int key2 = 6;
					object[] array2 = new object[2];
					array2[0] = new Func<object>(this.get_npadNo2);
					dictionary3.Add(key2, array2);
					Dictionary<int, object[]> dictionary4 = dictionary;
					int key3 = 7;
					object[] array3 = new object[2];
					array3[0] = new Func<object>(this.get_npadNo3);
					dictionary4.Add(key3, array3);
					Dictionary<int, object[]> dictionary5 = dictionary;
					int key4 = 8;
					object[] array4 = new object[2];
					array4[0] = new Func<object>(this.get_npadNo4);
					dictionary5.Add(key4, array4);
					Dictionary<int, object[]> dictionary6 = dictionary;
					int key5 = 9;
					object[] array5 = new object[2];
					array5[0] = new Func<object>(this.get_npadNo5);
					dictionary6.Add(key5, array5);
					Dictionary<int, object[]> dictionary7 = dictionary;
					int key6 = 10;
					object[] array6 = new object[2];
					array6[0] = new Func<object>(this.get_npadNo6);
					dictionary7.Add(key6, array6);
					Dictionary<int, object[]> dictionary8 = dictionary;
					int key7 = 11;
					object[] array7 = new object[2];
					array7[0] = new Func<object>(this.get_npadNo7);
					dictionary8.Add(key7, array7);
					Dictionary<int, object[]> dictionary9 = dictionary;
					int key8 = 12;
					object[] array8 = new object[2];
					array8[0] = new Func<object>(this.get_npadNo8);
					dictionary9.Add(key8, array8);
					Dictionary<int, object[]> dictionary10 = dictionary;
					int key9 = 13;
					object[] array9 = new object[2];
					array9[0] = new Func<object>(this.get_npadHandheld);
					dictionary10.Add(key9, array9);
					Dictionary<int, object[]> dictionary11 = dictionary;
					int key10 = 14;
					object[] array10 = new object[2];
					array10[0] = new Func<object>(this.get_debugPad);
					dictionary11.Add(key10, array10);
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x060083D5 RID: 33749 RVA: 0x0029F208 File Offset: 0x0029D408
			public bool TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060083D6 RID: 33750 RVA: 0x0029F268 File Offset: 0x0029D468
			public bool TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040081C1 RID: 33217
			[SerializeField]
			public int _allowedNpadStyles = -1;

			// Token: 0x040081C2 RID: 33218
			[SerializeField]
			public int _joyConGripStyle = 1;

			// Token: 0x040081C3 RID: 33219
			[SerializeField]
			public bool _adjustIMUsForGripStyle = true;

			// Token: 0x040081C4 RID: 33220
			[SerializeField]
			public int _handheldActivationMode;

			// Token: 0x040081C5 RID: 33221
			[SerializeField]
			public bool _assignJoysticksByNpadId = true;

			// Token: 0x040081C6 RID: 33222
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040081C7 RID: 33223
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);

			// Token: 0x040081C8 RID: 33224
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);

			// Token: 0x040081C9 RID: 33225
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);

			// Token: 0x040081CA RID: 33226
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);

			// Token: 0x040081CB RID: 33227
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);

			// Token: 0x040081CC RID: 33228
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);

			// Token: 0x040081CD RID: 33229
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);

			// Token: 0x040081CE RID: 33230
			[SerializeField]
			public NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040081CF RID: 33231
			[SerializeField]
			public NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);

			// Token: 0x040081D0 RID: 33232
			public Dictionary<int, object[]> __delegates;
		}

		// Token: 0x020012EF RID: 4847
		[Serializable]
		public sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x060083DC RID: 33756 RVA: 0x00057E47 File Offset: 0x00056047
			public NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x170019E5 RID: 6629
			// (get) Token: 0x060083DD RID: 33757 RVA: 0x00057E64 File Offset: 0x00056064
			// (set) Token: 0x060083DE RID: 33758 RVA: 0x00057E6C File Offset: 0x0005606C
			public bool isAllowed
			{
				get
				{
					return this._isAllowed;
				}
				set
				{
					this._isAllowed = value;
				}
			}

			// Token: 0x170019E6 RID: 6630
			// (get) Token: 0x060083DF RID: 33759 RVA: 0x00057E75 File Offset: 0x00056075
			// (set) Token: 0x060083E0 RID: 33760 RVA: 0x00057E7D File Offset: 0x0005607D
			public int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x170019E7 RID: 6631
			// (get) Token: 0x060083E1 RID: 33761 RVA: 0x00057E86 File Offset: 0x00056086
			// (set) Token: 0x060083E2 RID: 33762 RVA: 0x00057E8E File Offset: 0x0005608E
			public int joyConAssignmentMode
			{
				get
				{
					return this._joyConAssignmentMode;
				}
				set
				{
					this._joyConAssignmentMode = value;
				}
			}

			// Token: 0x170019E8 RID: 6632
			// (get) Token: 0x060083E3 RID: 33763 RVA: 0x0029F2A4 File Offset: 0x0029D4A4
			public Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(this.get_isAllowed),
								new Action<bool>(delegate(bool x)
								{
									this.isAllowed = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(this.get_rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						},
						{
							2,
							new object[]
							{
								new Func<int>(this.get_joyConAssignmentMode),
								new Action<int>(delegate(int x)
								{
									this.joyConAssignmentMode = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x060083E4 RID: 33764 RVA: 0x0029F354 File Offset: 0x0029D554
			public bool TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060083E5 RID: 33765 RVA: 0x0029F3B4 File Offset: 0x0029D5B4
			public bool TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040081D1 RID: 33233
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			public bool _isAllowed = true;

			// Token: 0x040081D2 RID: 33234
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			public int _rewiredPlayerId;

			// Token: 0x040081D3 RID: 33235
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			public int _joyConAssignmentMode = -1;

			// Token: 0x040081D4 RID: 33236
			public Dictionary<int, object[]> __delegates;
		}

		// Token: 0x020012F0 RID: 4848
		[Serializable]
		public sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x060083E9 RID: 33769 RVA: 0x00057EB2 File Offset: 0x000560B2
			public DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x170019E9 RID: 6633
			// (get) Token: 0x060083EA RID: 33770 RVA: 0x00057EC1 File Offset: 0x000560C1
			// (set) Token: 0x060083EB RID: 33771 RVA: 0x00057EC9 File Offset: 0x000560C9
			public int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x170019EA RID: 6634
			// (get) Token: 0x060083EC RID: 33772 RVA: 0x00057ED2 File Offset: 0x000560D2
			// (set) Token: 0x060083ED RID: 33773 RVA: 0x00057EDA File Offset: 0x000560DA
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					this._enabled = value;
				}
			}

			// Token: 0x170019EB RID: 6635
			// (get) Token: 0x060083EE RID: 33774 RVA: 0x0029F3F0 File Offset: 0x0029D5F0
			public Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(this.get_enabled),
								new Action<bool>(delegate(bool x)
								{
									this.enabled = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(this.get_rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x060083EF RID: 33775 RVA: 0x0029F478 File Offset: 0x0029D678
			public bool TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060083F0 RID: 33776 RVA: 0x0029F4D8 File Offset: 0x0029D6D8
			public bool TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040081D5 RID: 33237
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			public bool _enabled;

			// Token: 0x040081D6 RID: 33238
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			public int _rewiredPlayerId;

			// Token: 0x040081D7 RID: 33239
			public Dictionary<int, object[]> __delegates;
		}
	}
}
