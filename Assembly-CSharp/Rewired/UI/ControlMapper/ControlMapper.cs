using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000639 RID: 1593
	[AddComponentMenu("")]
	public class ControlMapper : MonoBehaviour
	{
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06004135 RID: 16693 RVA: 0x000344CF File Offset: 0x000326CF
		// (remove) Token: 0x06004136 RID: 16694 RVA: 0x000344E8 File Offset: 0x000326E8
		public event Action ScreenClosedEvent
		{
			add
			{
				this._ScreenClosedEvent = (Action)Delegate.Combine(this._ScreenClosedEvent, value);
			}
			remove
			{
				this._ScreenClosedEvent = (Action)Delegate.Remove(this._ScreenClosedEvent, value);
			}
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06004137 RID: 16695 RVA: 0x00034501 File Offset: 0x00032701
		// (remove) Token: 0x06004138 RID: 16696 RVA: 0x0003451A File Offset: 0x0003271A
		public event Action ScreenOpenedEvent
		{
			add
			{
				this._ScreenOpenedEvent = (Action)Delegate.Combine(this._ScreenOpenedEvent, value);
			}
			remove
			{
				this._ScreenOpenedEvent = (Action)Delegate.Remove(this._ScreenOpenedEvent, value);
			}
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06004139 RID: 16697 RVA: 0x00034533 File Offset: 0x00032733
		// (remove) Token: 0x0600413A RID: 16698 RVA: 0x0003454C File Offset: 0x0003274C
		public event Action PopupWindowClosedEvent
		{
			add
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Combine(this._PopupWindowClosedEvent, value);
			}
			remove
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Remove(this._PopupWindowClosedEvent, value);
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x0600413B RID: 16699 RVA: 0x00034565 File Offset: 0x00032765
		// (remove) Token: 0x0600413C RID: 16700 RVA: 0x0003457E File Offset: 0x0003277E
		public event Action PopupWindowOpenedEvent
		{
			add
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Combine(this._PopupWindowOpenedEvent, value);
			}
			remove
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Remove(this._PopupWindowOpenedEvent, value);
			}
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x0600413D RID: 16701 RVA: 0x00034597 File Offset: 0x00032797
		// (remove) Token: 0x0600413E RID: 16702 RVA: 0x000345B0 File Offset: 0x000327B0
		public event Action InputPollingStartedEvent
		{
			add
			{
				this._InputPollingStartedEvent = (Action)Delegate.Combine(this._InputPollingStartedEvent, value);
			}
			remove
			{
				this._InputPollingStartedEvent = (Action)Delegate.Remove(this._InputPollingStartedEvent, value);
			}
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x0600413F RID: 16703 RVA: 0x000345C9 File Offset: 0x000327C9
		// (remove) Token: 0x06004140 RID: 16704 RVA: 0x000345E2 File Offset: 0x000327E2
		public event Action InputPollingEndedEvent
		{
			add
			{
				this._InputPollingEndedEvent = (Action)Delegate.Combine(this._InputPollingEndedEvent, value);
			}
			remove
			{
				this._InputPollingEndedEvent = (Action)Delegate.Remove(this._InputPollingEndedEvent, value);
			}
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06004141 RID: 16705 RVA: 0x000345FB File Offset: 0x000327FB
		// (remove) Token: 0x06004142 RID: 16706 RVA: 0x00034609 File Offset: 0x00032809
		public event UnityAction onScreenClosed
		{
			add
			{
				this._onScreenClosed.AddListener(value);
			}
			remove
			{
				this._onScreenClosed.RemoveListener(value);
			}
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06004143 RID: 16707 RVA: 0x00034617 File Offset: 0x00032817
		// (remove) Token: 0x06004144 RID: 16708 RVA: 0x00034625 File Offset: 0x00032825
		public event UnityAction onScreenOpened
		{
			add
			{
				this._onScreenOpened.AddListener(value);
			}
			remove
			{
				this._onScreenOpened.RemoveListener(value);
			}
		}

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06004145 RID: 16709 RVA: 0x00034633 File Offset: 0x00032833
		// (remove) Token: 0x06004146 RID: 16710 RVA: 0x00034641 File Offset: 0x00032841
		public event UnityAction onPopupWindowClosed
		{
			add
			{
				this._onPopupWindowClosed.AddListener(value);
			}
			remove
			{
				this._onPopupWindowClosed.RemoveListener(value);
			}
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06004147 RID: 16711 RVA: 0x0003464F File Offset: 0x0003284F
		// (remove) Token: 0x06004148 RID: 16712 RVA: 0x0003465D File Offset: 0x0003285D
		public event UnityAction onPopupWindowOpened
		{
			add
			{
				this._onPopupWindowOpened.AddListener(value);
			}
			remove
			{
				this._onPopupWindowOpened.RemoveListener(value);
			}
		}

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x06004149 RID: 16713 RVA: 0x0003466B File Offset: 0x0003286B
		// (remove) Token: 0x0600414A RID: 16714 RVA: 0x00034679 File Offset: 0x00032879
		public event UnityAction onInputPollingStarted
		{
			add
			{
				this._onInputPollingStarted.AddListener(value);
			}
			remove
			{
				this._onInputPollingStarted.RemoveListener(value);
			}
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x0600414B RID: 16715 RVA: 0x00034687 File Offset: 0x00032887
		// (remove) Token: 0x0600414C RID: 16716 RVA: 0x00034695 File Offset: 0x00032895
		public event UnityAction onInputPollingEnded
		{
			add
			{
				this._onInputPollingEnded.AddListener(value);
			}
			remove
			{
				this._onInputPollingEnded.RemoveListener(value);
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x000346A3 File Offset: 0x000328A3
		// (set) Token: 0x0600414E RID: 16718 RVA: 0x000346AB File Offset: 0x000328AB
		public InputManager rewiredInputManager
		{
			get
			{
				return this._rewiredInputManager;
			}
			set
			{
				this._rewiredInputManager = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x000346BB File Offset: 0x000328BB
		// (set) Token: 0x06004150 RID: 16720 RVA: 0x000346C3 File Offset: 0x000328C3
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				if (value != this._dontDestroyOnLoad && value)
				{
					Object.DontDestroyOnLoad(base.transform.gameObject);
				}
				this._dontDestroyOnLoad = value;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x000346EE File Offset: 0x000328EE
		// (set) Token: 0x06004152 RID: 16722 RVA: 0x000346F6 File Offset: 0x000328F6
		public int keyboardMapDefaultLayout
		{
			get
			{
				return this._keyboardMapDefaultLayout;
			}
			set
			{
				this._keyboardMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x00034706 File Offset: 0x00032906
		// (set) Token: 0x06004154 RID: 16724 RVA: 0x0003470E File Offset: 0x0003290E
		public int mouseMapDefaultLayout
		{
			get
			{
				return this._mouseMapDefaultLayout;
			}
			set
			{
				this._mouseMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06004155 RID: 16725 RVA: 0x0003471E File Offset: 0x0003291E
		// (set) Token: 0x06004156 RID: 16726 RVA: 0x00034726 File Offset: 0x00032926
		public int joystickMapDefaultLayout
		{
			get
			{
				return this._joystickMapDefaultLayout;
			}
			set
			{
				this._joystickMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x00034736 File Offset: 0x00032936
		// (set) Token: 0x06004158 RID: 16728 RVA: 0x00034753 File Offset: 0x00032953
		public bool showPlayers
		{
			get
			{
				return this._showPlayers && ReInput.players.playerCount > 1;
			}
			set
			{
				this._showPlayers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x00034763 File Offset: 0x00032963
		// (set) Token: 0x0600415A RID: 16730 RVA: 0x0003476B File Offset: 0x0003296B
		public bool showControllers
		{
			get
			{
				return this._showControllers;
			}
			set
			{
				this._showControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600415B RID: 16731 RVA: 0x0003477B File Offset: 0x0003297B
		// (set) Token: 0x0600415C RID: 16732 RVA: 0x00034783 File Offset: 0x00032983
		public bool showKeyboard
		{
			get
			{
				return this._showKeyboard;
			}
			set
			{
				this._showKeyboard = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x00034793 File Offset: 0x00032993
		// (set) Token: 0x0600415E RID: 16734 RVA: 0x0003479B File Offset: 0x0003299B
		public bool showMouse
		{
			get
			{
				return this._showMouse;
			}
			set
			{
				this._showMouse = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600415F RID: 16735 RVA: 0x000347AB File Offset: 0x000329AB
		// (set) Token: 0x06004160 RID: 16736 RVA: 0x000347B3 File Offset: 0x000329B3
		public int maxControllersPerPlayer
		{
			get
			{
				return this._maxControllersPerPlayer;
			}
			set
			{
				this._maxControllersPerPlayer = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06004161 RID: 16737 RVA: 0x000347C3 File Offset: 0x000329C3
		// (set) Token: 0x06004162 RID: 16738 RVA: 0x000347CB File Offset: 0x000329CB
		public bool showActionCategoryLabels
		{
			get
			{
				return this._showActionCategoryLabels;
			}
			set
			{
				this._showActionCategoryLabels = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06004163 RID: 16739 RVA: 0x000347DB File Offset: 0x000329DB
		// (set) Token: 0x06004164 RID: 16740 RVA: 0x000347E3 File Offset: 0x000329E3
		public int keyboardInputFieldCount
		{
			get
			{
				return this._keyboardInputFieldCount;
			}
			set
			{
				this._keyboardInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06004165 RID: 16741 RVA: 0x000347F3 File Offset: 0x000329F3
		// (set) Token: 0x06004166 RID: 16742 RVA: 0x000347FB File Offset: 0x000329FB
		public int mouseInputFieldCount
		{
			get
			{
				return this._mouseInputFieldCount;
			}
			set
			{
				this._mouseInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06004167 RID: 16743 RVA: 0x0003480B File Offset: 0x00032A0B
		// (set) Token: 0x06004168 RID: 16744 RVA: 0x00034813 File Offset: 0x00032A13
		public int controllerInputFieldCount
		{
			get
			{
				return this._controllerInputFieldCount;
			}
			set
			{
				this._controllerInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x00034823 File Offset: 0x00032A23
		// (set) Token: 0x0600416A RID: 16746 RVA: 0x0003482B File Offset: 0x00032A2B
		public bool showFullAxisInputFields
		{
			get
			{
				return this._showFullAxisInputFields;
			}
			set
			{
				this._showFullAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600416B RID: 16747 RVA: 0x0003483B File Offset: 0x00032A3B
		// (set) Token: 0x0600416C RID: 16748 RVA: 0x00034843 File Offset: 0x00032A43
		public bool showSplitAxisInputFields
		{
			get
			{
				return this._showSplitAxisInputFields;
			}
			set
			{
				this._showSplitAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600416D RID: 16749 RVA: 0x00034853 File Offset: 0x00032A53
		// (set) Token: 0x0600416E RID: 16750 RVA: 0x0003485B File Offset: 0x00032A5B
		public bool allowElementAssignmentConflicts
		{
			get
			{
				return this._allowElementAssignmentConflicts;
			}
			set
			{
				this._allowElementAssignmentConflicts = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x0003486B File Offset: 0x00032A6B
		// (set) Token: 0x06004170 RID: 16752 RVA: 0x00034873 File Offset: 0x00032A73
		public int actionLabelWidth
		{
			get
			{
				return this._actionLabelWidth;
			}
			set
			{
				this._actionLabelWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x00034883 File Offset: 0x00032A83
		// (set) Token: 0x06004172 RID: 16754 RVA: 0x0003488B File Offset: 0x00032A8B
		public int keyboardColMaxWidth
		{
			get
			{
				return this._keyboardColMaxWidth;
			}
			set
			{
				this._keyboardColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x0003489B File Offset: 0x00032A9B
		// (set) Token: 0x06004174 RID: 16756 RVA: 0x000348A3 File Offset: 0x00032AA3
		public int mouseColMaxWidth
		{
			get
			{
				return this._mouseColMaxWidth;
			}
			set
			{
				this._mouseColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x000348B3 File Offset: 0x00032AB3
		// (set) Token: 0x06004176 RID: 16758 RVA: 0x000348BB File Offset: 0x00032ABB
		public int controllerColMaxWidth
		{
			get
			{
				return this._controllerColMaxWidth;
			}
			set
			{
				this._controllerColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06004177 RID: 16759 RVA: 0x000348CB File Offset: 0x00032ACB
		// (set) Token: 0x06004178 RID: 16760 RVA: 0x000348D3 File Offset: 0x00032AD3
		public float inputRowHeight
		{
			get
			{
				return this._inputRowHeight;
			}
			set
			{
				this._inputRowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06004179 RID: 16761 RVA: 0x000348E3 File Offset: 0x00032AE3
		// (set) Token: 0x0600417A RID: 16762 RVA: 0x000348EB File Offset: 0x00032AEB
		public float inputColumnSpacing
		{
			get
			{
				return this._inputColumnSpacing;
			}
			set
			{
				this._inputColumnSpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600417B RID: 16763 RVA: 0x000348FB File Offset: 0x00032AFB
		// (set) Token: 0x0600417C RID: 16764 RVA: 0x00034903 File Offset: 0x00032B03
		public int inputRowCategorySpacing
		{
			get
			{
				return this._inputRowCategorySpacing;
			}
			set
			{
				this._inputRowCategorySpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x00034913 File Offset: 0x00032B13
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x0003491B File Offset: 0x00032B1B
		public int invertToggleWidth
		{
			get
			{
				return this._invertToggleWidth;
			}
			set
			{
				this._invertToggleWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x0003492B File Offset: 0x00032B2B
		// (set) Token: 0x06004180 RID: 16768 RVA: 0x00034933 File Offset: 0x00032B33
		public int defaultWindowWidth
		{
			get
			{
				return this._defaultWindowWidth;
			}
			set
			{
				this._defaultWindowWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x00034943 File Offset: 0x00032B43
		// (set) Token: 0x06004182 RID: 16770 RVA: 0x0003494B File Offset: 0x00032B4B
		public int defaultWindowHeight
		{
			get
			{
				return this._defaultWindowHeight;
			}
			set
			{
				this._defaultWindowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06004183 RID: 16771 RVA: 0x0003495B File Offset: 0x00032B5B
		// (set) Token: 0x06004184 RID: 16772 RVA: 0x00034963 File Offset: 0x00032B63
		public float controllerAssignmentTimeout
		{
			get
			{
				return this._controllerAssignmentTimeout;
			}
			set
			{
				this._controllerAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06004185 RID: 16773 RVA: 0x00034973 File Offset: 0x00032B73
		// (set) Token: 0x06004186 RID: 16774 RVA: 0x0003497B File Offset: 0x00032B7B
		public float preInputAssignmentTimeout
		{
			get
			{
				return this._preInputAssignmentTimeout;
			}
			set
			{
				this._preInputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x0003498B File Offset: 0x00032B8B
		// (set) Token: 0x06004188 RID: 16776 RVA: 0x00034993 File Offset: 0x00032B93
		public float inputAssignmentTimeout
		{
			get
			{
				return this._inputAssignmentTimeout;
			}
			set
			{
				this._inputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x000349A3 File Offset: 0x00032BA3
		// (set) Token: 0x0600418A RID: 16778 RVA: 0x000349AB File Offset: 0x00032BAB
		public float axisCalibrationTimeout
		{
			get
			{
				return this._axisCalibrationTimeout;
			}
			set
			{
				this._axisCalibrationTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x000349BB File Offset: 0x00032BBB
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x000349C3 File Offset: 0x00032BC3
		public bool ignoreMouseXAxisAssignment
		{
			get
			{
				return this._ignoreMouseXAxisAssignment;
			}
			set
			{
				this._ignoreMouseXAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x000349D3 File Offset: 0x00032BD3
		// (set) Token: 0x0600418E RID: 16782 RVA: 0x000349DB File Offset: 0x00032BDB
		public bool ignoreMouseYAxisAssignment
		{
			get
			{
				return this._ignoreMouseYAxisAssignment;
			}
			set
			{
				this._ignoreMouseYAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x000349EB File Offset: 0x00032BEB
		// (set) Token: 0x06004190 RID: 16784 RVA: 0x000349F3 File Offset: 0x00032BF3
		public bool universalCancelClosesScreen
		{
			get
			{
				return this._universalCancelClosesScreen;
			}
			set
			{
				this._universalCancelClosesScreen = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x00034A03 File Offset: 0x00032C03
		// (set) Token: 0x06004192 RID: 16786 RVA: 0x00034A0B File Offset: 0x00032C0B
		public bool showInputBehaviorSettings
		{
			get
			{
				return this._showInputBehaviorSettings;
			}
			set
			{
				this._showInputBehaviorSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x00034A1B File Offset: 0x00032C1B
		// (set) Token: 0x06004194 RID: 16788 RVA: 0x00034A23 File Offset: 0x00032C23
		public bool useThemeSettings
		{
			get
			{
				return this._useThemeSettings;
			}
			set
			{
				this._useThemeSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x00034A33 File Offset: 0x00032C33
		// (set) Token: 0x06004196 RID: 16790 RVA: 0x00034A3B File Offset: 0x00032C3B
		public LanguageData language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				if (this._language != null)
				{
					this._language.Initialize();
				}
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x00034A67 File Offset: 0x00032C67
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x00034A6F File Offset: 0x00032C6F
		public bool showPlayersGroupLabel
		{
			get
			{
				return this._showPlayersGroupLabel;
			}
			set
			{
				this._showPlayersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x00034A7F File Offset: 0x00032C7F
		// (set) Token: 0x0600419A RID: 16794 RVA: 0x00034A87 File Offset: 0x00032C87
		public bool showControllerGroupLabel
		{
			get
			{
				return this._showControllerGroupLabel;
			}
			set
			{
				this._showControllerGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x00034A97 File Offset: 0x00032C97
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x00034A9F File Offset: 0x00032C9F
		public bool showAssignedControllersGroupLabel
		{
			get
			{
				return this._showAssignedControllersGroupLabel;
			}
			set
			{
				this._showAssignedControllersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x00034AAF File Offset: 0x00032CAF
		// (set) Token: 0x0600419E RID: 16798 RVA: 0x00034AB7 File Offset: 0x00032CB7
		public bool showSettingsGroupLabel
		{
			get
			{
				return this._showSettingsGroupLabel;
			}
			set
			{
				this._showSettingsGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600419F RID: 16799 RVA: 0x00034AC7 File Offset: 0x00032CC7
		// (set) Token: 0x060041A0 RID: 16800 RVA: 0x00034ACF File Offset: 0x00032CCF
		public bool showMapCategoriesGroupLabel
		{
			get
			{
				return this._showMapCategoriesGroupLabel;
			}
			set
			{
				this._showMapCategoriesGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060041A1 RID: 16801 RVA: 0x00034ADF File Offset: 0x00032CDF
		// (set) Token: 0x060041A2 RID: 16802 RVA: 0x00034AE7 File Offset: 0x00032CE7
		public bool showControllerNameLabel
		{
			get
			{
				return this._showControllerNameLabel;
			}
			set
			{
				this._showControllerNameLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060041A3 RID: 16803 RVA: 0x00034AF7 File Offset: 0x00032CF7
		// (set) Token: 0x060041A4 RID: 16804 RVA: 0x00034AFF File Offset: 0x00032CFF
		public bool showAssignedControllers
		{
			get
			{
				return this._showAssignedControllers;
			}
			set
			{
				this._showAssignedControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x060041A5 RID: 16805 RVA: 0x00034B0F File Offset: 0x00032D0F
		// (set) Token: 0x060041A6 RID: 16806 RVA: 0x00034B17 File Offset: 0x00032D17
		public bool showControllerGroupButtons { get; set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x00034B20 File Offset: 0x00032D20
		// (set) Token: 0x060041A8 RID: 16808 RVA: 0x00034B28 File Offset: 0x00032D28
		public Action restoreDefaultsDelegate
		{
			get
			{
				return this._restoreDefaultsDelegate;
			}
			set
			{
				this._restoreDefaultsDelegate = value;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x060041A9 RID: 16809 RVA: 0x001308B4 File Offset: 0x0012EAB4
		public bool isOpen
		{
			get
			{
				if (!this.initialized)
				{
					return this.references.canvas != null && this.references.canvas.gameObject.activeInHierarchy;
				}
				return this.canvas.activeInHierarchy;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x00034B31 File Offset: 0x00032D31
		public bool isFocused
		{
			get
			{
				return this.initialized && !this.windowManager.isWindowOpen;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x00034B4E File Offset: 0x00032D4E
		public bool inputAllowed
		{
			get
			{
				return this.blockInputOnFocusEndTime <= Time.unscaledTime && !InterruptingPrompt.IsInterrupting();
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x0013090C File Offset: 0x0012EB0C
		public int inputGridColumnCount
		{
			get
			{
				int num = 1;
				if (this._showKeyboard)
				{
					num++;
				}
				if (this._showMouse)
				{
					num++;
				}
				if (this._showControllers)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x0013094C File Offset: 0x0012EB4C
		public int inputGridWidth
		{
			get
			{
				return this._actionLabelWidth + ((!this._showKeyboard) ? 0 : this._keyboardColMaxWidth) + ((!this._showMouse) ? 0 : this._mouseColMaxWidth) + ((!this._showControllers) ? 0 : this._controllerColMaxWidth) + (int)((float)(this.inputGridColumnCount - 1) * this._inputColumnSpacing);
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x00034B6F File Offset: 0x00032D6F
		public Player currentPlayer
		{
			get
			{
				return ReInput.players.GetPlayer(this.currentPlayerId);
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x00034B81 File Offset: 0x00032D81
		public InputCategory currentMapCategory
		{
			get
			{
				return ReInput.mapping.GetMapCategory(this.currentMapCategoryId);
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x001309BC File Offset: 0x0012EBBC
		public ControlMapper.MappingSet currentMappingSet
		{
			get
			{
				if (this.currentMapCategoryId < 0)
				{
					return null;
				}
				for (int i = 0; i < this._mappingSets.Length; i++)
				{
					if (this._mappingSets[i].mapCategoryId == this.currentMapCategoryId)
					{
						return this._mappingSets[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00034B93 File Offset: 0x00032D93
		public Joystick currentJoystick
		{
			get
			{
				return ReInput.controllers.GetJoystick(this.currentJoystickId);
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x00034BA5 File Offset: 0x00032DA5
		public bool isJoystickSelected
		{
			get
			{
				return this.currentJoystickId >= 0;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x00034BB3 File Offset: 0x00032DB3
		public GameObject currentUISelection
		{
			get
			{
				return (!(EventSystem.current != null)) ? null : EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x00034BD5 File Offset: 0x00032DD5
		public bool showSettings
		{
			get
			{
				return this._showInputBehaviorSettings && this._inputBehaviorSettings.Length > 0;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00034BF0 File Offset: 0x00032DF0
		public bool showMapCategories
		{
			get
			{
				return this._mappingSets != null && this._mappingSets.Length > 1;
			}
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x00034C10 File Offset: 0x00032E10
		public void Awake()
		{
			if (this._dontDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(base.transform.gameObject);
			}
			this.PreInitialize();
			if (this.isOpen)
			{
				this.Initialize();
				this.Open(true);
			}
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x00034C4B File Offset: 0x00032E4B
		public void Start()
		{
			if (this._openOnStart)
			{
				this.Open(false);
			}
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x00034C5F File Offset: 0x00032E5F
		public void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			this.CheckUISelection();
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x00034C7F File Offset: 0x00032E7F
		public void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent -= this.OnJoystickPreDisconnect;
			this.UnsubscribeMenuControlInputEvents();
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x00034CBA File Offset: 0x00032EBA
		public void PreInitialize()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			this.SubscribeMenuControlInputEvents();
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00130A14 File Offset: 0x0012EC14
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.currentPlayerId = Mathf.Clamp(this.currentPlayerId, 0, 1);
			if (this._rewiredInputManager == null)
			{
				this._rewiredInputManager = Object.FindObjectOfType<InputManager>();
				if (this._rewiredInputManager == null)
				{
					Debug.LogError("Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
					return;
				}
			}
			if (ControlMapper.Instance != null)
			{
				Debug.LogError("Rewired Control Mapper: Only one ControlMapper can exist at one time!");
				return;
			}
			ControlMapper.Instance = this;
			if (this.prefabs == null || !this.prefabs.Check())
			{
				Debug.LogError("Rewired Control Mapper: All prefabs must be assigned in the inspector!");
				return;
			}
			if (this.references == null || !this.references.Check())
			{
				Debug.LogError("Rewired Control Mapper: All references must be assigned in the inspector!");
				return;
			}
			this.references.inputGridLayoutElement = this.references.inputGridContainer.GetComponent<LayoutElement>();
			if (this.references.inputGridLayoutElement == null)
			{
				Debug.LogError("Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
				return;
			}
			if (this._showKeyboard && this._keyboardInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
				this._keyboardInputFieldCount = 1;
			}
			if (this._showMouse && this._mouseInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Mouse Input Fields must be at least 1!");
				this._mouseInputFieldCount = 1;
			}
			if (this._showControllers && this._controllerInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Controller Input Fields must be at least 1!");
				this._controllerInputFieldCount = 1;
			}
			if (this._maxControllersPerPlayer < 0)
			{
				Debug.LogWarning("Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
				this._maxControllersPerPlayer = 0;
			}
			if (this._useThemeSettings && this._themeSettings == null)
			{
				Debug.LogWarning("Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
				this._useThemeSettings = false;
			}
			if (this._language == null)
			{
				Debug.LogError("Rawired UI: Language must be set in the inspector!");
				return;
			}
			this._language.Initialize();
			this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
			this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
			ReInput.ControllerConnectedEvent += this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent += this.OnJoystickPreDisconnect;
			PlayerManager.OnControlsChanged += this.OnControlsChanged;
			this.playerCount = ReInput.players.playerCount;
			this.canvas = this.references.canvas.gameObject;
			this.windowManager = new ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, this.references.canvas.transform);
			this.playerButtons = new List<ControlMapper.GUIButton>();
			this.mapCategoryButtons = new List<ControlMapper.GUIButton>();
			this.assignedControllerButtons = new List<ControlMapper.GUIButton>();
			this.miscInstantiatedObjects = new List<GameObject>();
			this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
			this.Draw();
			this.CreateInputGrid();
			this.CreateLayout();
			this.SubscribeFixedUISelectionEvents();
			this.initialized = true;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x00034CD7 File Offset: 0x00032ED7
		public void OnJoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00034CFD File Offset: 0x00032EFD
		public void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00034D23 File Offset: 0x00032F23
		public void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x00130D2C File Offset: 0x0012EF2C
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			string identifier = buttonInfo.identifier;
			switch (identifier)
			{
			case "PlayerSelection":
				this.OnPlayerSelected(buttonInfo.intData, true);
				break;
			case "AssignedControllerSelection":
				this.OnControllerSelected(buttonInfo.intData);
				break;
			case "RemoveController":
				this.OnRemoveCurrentController();
				break;
			case "AssignController":
				this.ShowAssignControllerWindow();
				break;
			case "CalibrateController":
				this.ShowCalibrateControllerWindow();
				break;
			case "EditInputBehaviors":
				this.ShowEditInputBehaviorsWindow();
				break;
			case "MapCategorySelection":
				this.OnMapCategorySelected(buttonInfo.intData, true);
				break;
			case "Done":
				this.Close(true);
				break;
			case "RestoreDefaults":
				this.OnRestoreDefaults();
				break;
			case "ToggleRumble":
				this.ToggleRumble();
				break;
			}
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00034D3D File Offset: 0x00032F3D
		public void ToggleRumble()
		{
			SettingsData.Data.canVibrate = !SettingsData.Data.canVibrate;
			SettingsData.Save();
			this.UpdateRumbleText();
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x00130EC4 File Offset: 0x0012F0C4
		public void UpdateRumbleText()
		{
			if (this._rumbleButtonText != null)
			{
				this._rumbleButtonText.text = Localization.Translate((!SettingsData.Data.canVibrate) ? "ToggleRumbleOff" : "ToggleRumbleOn").text;
			}
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x00034D61 File Offset: 0x00032F61
		public void OnEnable()
		{
			this.UpdateRumbleText();
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x00130F18 File Offset: 0x0012F118
		public void OnInputFieldActivated(InputFieldInfo fieldInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			if (this.currentPlayer == null)
			{
				return;
			}
			InputAction action = ReInput.mapping.GetAction(fieldInfo.actionId);
			if (action == null)
			{
				return;
			}
			string text;
			if (action.type == 1)
			{
				text = action.descriptiveName;
			}
			else
			{
				if (action.type != null)
				{
					throw new NotImplementedException();
				}
				if (fieldInfo.axisRange == null)
				{
					text = action.descriptiveName;
				}
				else if (fieldInfo.axisRange == 1)
				{
					if (string.IsNullOrEmpty(action.positiveDescriptiveName))
					{
						text = action.descriptiveName + " +";
					}
					else
					{
						text = action.positiveDescriptiveName;
					}
				}
				else
				{
					if (fieldInfo.axisRange != 2)
					{
						throw new NotImplementedException();
					}
					if (string.IsNullOrEmpty(action.negativeDescriptiveName))
					{
						text = action.descriptiveName + " -";
					}
					else
					{
						text = action.negativeDescriptiveName;
					}
				}
			}
			text = Localization.Translate(text).text;
			ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
			if (controllerMap == null)
			{
				return;
			}
			ActionElementMap actionElementMap = (fieldInfo.actionElementMapId < 0) ? null : controllerMap.GetElementMap(fieldInfo.actionElementMapId);
			if (actionElementMap != null)
			{
				this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, actionElementMap, text);
			}
			else
			{
				this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, text);
			}
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x00034D69 File Offset: 0x00032F69
		public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x060041C5 RID: 16837 RVA: 0x00131098 File Offset: 0x0012F298
		// (remove) Token: 0x060041C6 RID: 16838 RVA: 0x001310CC File Offset: 0x0012F2CC
		public static event ControlMapper.PlayerChangeAction OnPlayerChange;

		// Token: 0x060041C7 RID: 16839 RVA: 0x00131100 File Offset: 0x0012F300
		public void OnPlayerSelected(int playerId, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentPlayerId = playerId;
			this.ClearVarsOnPlayerChange();
			this.Redraw(true, true);
			for (int i = 0; i < this.axisToggleObjects.Count; i++)
			{
				this.axisToggleObjects[i].SetActive(this.currentPlayer.controllers.joystickCount > 0);
			}
			for (int j = 0; j < this.inactiveAxisToggleObjects.Count; j++)
			{
				this.inactiveAxisToggleObjects[j].SetActive(this.currentPlayer.controllers.joystickCount == 0);
			}
			if (ControlMapper.OnPlayerChange != null)
			{
				ControlMapper.OnPlayerChange();
			}
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x00034DA0 File Offset: 0x00032FA0
		public void OnControllerSelected(int joystickId)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentJoystickId = joystickId;
			this.Redraw(true, true);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x00034DBD File Offset: 0x00032FBD
		public void OnRemoveCurrentController()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentJoystickId < 0)
			{
				return;
			}
			this.RemoveController(this.currentPlayer, this.currentJoystickId);
			this.ClearVarsOnJoystickChange();
			this.Redraw(false, false);
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x00034DF8 File Offset: 0x00032FF8
		public void OnMapCategorySelected(int id, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentMapCategoryId = id;
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x00034E1B File Offset: 0x0003301B
		public void OnRestoreDefaults()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ShowRestoreDefaultsWindow();
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x00034E2F File Offset: 0x0003302F
		public void OnScreenToggleActionPressed(InputActionEventData data)
		{
			if (!this.isOpen)
			{
				this.Open();
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x00034E62 File Offset: 0x00033062
		public void OnScreenOpenActionPressed(InputActionEventData data)
		{
			this.Open();
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x00034E6A File Offset: 0x0003306A
		public void OnScreenCloseActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x001311C4 File Offset: 0x0012F3C4
		public void OnUniversalCancelActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (this._universalCancelClosesScreen)
			{
				if (this.isFocused)
				{
					this.Close(true);
					return;
				}
			}
			else if (this.isFocused)
			{
				return;
			}
			if (this.isPollingForInput)
			{
				return;
			}
			this.CloseAllWindows();
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x00034E97 File Offset: 0x00033097
		public void OnWindowCancel(int windowId)
		{
			if (!this.initialized)
			{
				return;
			}
			if (windowId < 0)
			{
				return;
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x00034EB4 File Offset: 0x000330B4
		public void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
		{
			if (map == null || aem == null)
			{
				return;
			}
			map.DeleteElementMap(aem.id);
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0013122C File Offset: 0x0012F42C
		public void OnBeginElementAssignment(InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, string actionName)
		{
			if (fieldInfo == null || map == null)
			{
				return;
			}
			this.pendingInputMapping = new ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
			switch (fieldInfo.controllerType)
			{
			case 0:
				this.ShowElementAssignmentPollingWindow();
				break;
			case 1:
				this.ShowElementAssignmentPollingWindow();
				break;
			case 2:
				this.ShowElementAssignmentPrePollingWindow();
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00034ED7 File Offset: 0x000330D7
		public void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
		{
			if (windowId < 0 || player == null || controllerId < 0)
			{
				return;
			}
			this.AssignController(player, controllerId);
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x001312B0 File Offset: 0x0012F4B0
		public void OnMouseAssignmentConfirmed(int windowId, Player player)
		{
			if (windowId < 0 || player == null)
			{
				return;
			}
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != player)
				{
					players[i].controllers.hasMouse = false;
				}
			}
			player.controllers.hasMouse = true;
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x00131324 File Offset: 0x0012F524
		public void OnElementAssignmentConflictReplaceConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				this.CloseWindow(windowId);
				return;
			}
			if (skipOtherPlayers)
			{
				ReInput.players.SystemPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck);
				this.currentPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck);
			}
			else
			{
				ReInput.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck);
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00034EFD File Offset: 0x000330FD
		public void OnElementAssignmentAddConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x001313C8 File Offset: 0x0012F5C8
		public void OnRestoreDefaultsConfirmed(int windowId)
		{
			if (this._restoreDefaultsDelegate == null)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (this._showControllers)
					{
						player.controllers.maps.LoadDefaultMaps(2);
					}
					if (this._showKeyboard)
					{
						player.controllers.maps.LoadDefaultMaps(0);
					}
					if (this._showMouse)
					{
						player.controllers.maps.LoadDefaultMaps(1);
					}
				}
			}
			this.CloseWindow(windowId);
			if (this._restoreDefaultsDelegate != null)
			{
				this._restoreDefaultsDelegate();
			}
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0013147C File Offset: 0x0012F67C
		public void OnAssignControllerWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(2);
			if (!controllerPollingInfo.success)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				return;
			}
			this.InputPollingStopped();
			if (ReInput.controllers.IsControllerAssigned(2, controllerPollingInfo.controllerId) && !this.currentPlayer.controllers.ContainsController(2, controllerPollingInfo.controllerId))
			{
				return;
			}
			this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, controllerPollingInfo.controllerId);
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x00131560 File Offset: 0x0012F760
		public void OnElementAssignmentPrePollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				ControllerPollingInfo controllerPollingInfo;
				switch (this.pendingInputMapping.controllerType)
				{
				case 0:
				case 1:
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
					break;
				case 2:
					if (this.currentPlayer.controllers.joystickCount == 0)
					{
						return;
					}
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, this.currentJoystick.id);
					break;
				default:
					throw new NotImplementedException();
				}
				if (!controllerPollingInfo.success)
				{
					return;
				}
			}
			this.ShowElementAssignmentPollingWindow();
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x00131678 File Offset: 0x0012F878
		public void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			ControllerPollingInfo pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(2, this.currentJoystick.id);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (pollingInfo.elementIdentifierName.Contains("Trigger") && elementAssignment.axisRange == 2)
			{
				return;
			}
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, false);
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x001317CC File Offset: 0x0012F9CC
		public void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo pollingInfo;
			bool flag;
			ModifierKeyFlags modifierKeyFlags;
			string text;
			this.PollKeyboardForAssignment(out pollingInfo, out flag, out modifierKeyFlags, out text);
			if (flag)
			{
				window.timer.Start(this._inputAssignmentTimeout);
			}
			window.SetContentText((!flag) ? Mathf.CeilToInt(window.timer.remaining).ToString() : string.Empty, 2);
			window.SetContentText(text, 1);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo, modifierKeyFlags);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, false);
			}
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x0013190C File Offset: 0x0012FB0C
		public void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			ControllerPollingInfo pollingInfo;
			if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
			{
				pollingInfo = default(ControllerPollingInfo);
				foreach (ControllerPollingInfo controllerPollingInfo in ReInput.controllers.polling.PollControllerForAllElementsDown(1, 0))
				{
					if (controllerPollingInfo.elementType == null)
					{
						if (this._ignoreMouseXAxisAssignment && controllerPollingInfo.elementIndex == 0)
						{
							continue;
						}
						if (this._ignoreMouseYAxisAssignment && controllerPollingInfo.elementIndex == 1)
						{
							continue;
						}
					}
					pollingInfo = controllerPollingInfo;
					break;
				}
			}
			else
			{
				pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(1, 0);
			}
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, true);
			}
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x00131AE4 File Offset: 0x0012FCE4
		public void OnCalibrateAxisStep1WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.pendingAxisCalibration.RecordZero();
			this.CloseWindow(windowId);
			this.ShowCalibrateAxisStep2Window();
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x00131BBC File Offset: 0x0012FDBC
		public void OnCalibrateAxisStep2WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				this.pendingAxisCalibration.RecordMinMax();
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.EndAxisCalibration();
			this.InputPollingStopped();
			this.CloseWindow(windowId);
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x00131C94 File Offset: 0x0012FE94
		public void ShowAssignControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.assignControllerWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.assignControllerWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.timer.Start(this._controllerAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x00131D7C File Offset: 0x0012FF7C
		public void ShowControllerAssignmentConflictWindow(int controllerId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer)
				{
					if (players[i].controllers.ContainsController(2, controllerId))
					{
						otherPlayerName = players[i].descriptiveName;
						break;
					}
				}
			}
			Joystick joystick = ReInput.controllers.GetJoystick(controllerId);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.controllerAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(joystick.name, otherPlayerName, this.currentPlayer.descriptiveName));
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate
			{
				this.OnControllerAssignmentConfirmed(window.id, this.currentPlayer, controllerId);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x00131F70 File Offset: 0x00130170
		public void ShowBeginElementAssignmentReplacementWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, ActionElementMap aem, string actionName)
		{
			ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData);
			if (guiinputField == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), guiinputField.GetLabel());
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate
			{
				this.OnBeginElementAssignment(fieldInfo, map, aem, actionName);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.remove, delegate
			{
				this.OnRemoveElementAssignment(window.id, map, aem);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x00132138 File Offset: 0x00130338
		public void ShowCreateNewElementAssignmentWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, string actionName)
		{
			if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
			{
				return;
			}
			this.OnBeginElementAssignment(fieldInfo, map, null, actionName);
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x00132184 File Offset: 0x00130384
		public void ShowElementAssignmentPrePollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 10f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
			window.timer.Start(this._preInputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x0013229C File Offset: 0x0013049C
		public void ShowElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			switch (this.pendingInputMapping.controllerType)
			{
			case 0:
				this.ShowKeyboardElementAssignmentPollingWindow();
				break;
			case 1:
				if (this.currentPlayer.controllers.hasMouse)
				{
					this.ShowMouseElementAssignmentPollingWindow();
				}
				else
				{
					this.ShowMouseAssignmentConflictWindow();
				}
				break;
			case 2:
				this.ShowJoystickElementAssignmentPollingWindow();
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x00132320 File Offset: 0x00130520
		public void ShowJoystickElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange != null || !this._showFullAxisInputFields || this._showSplitAxisInputFields) ? this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x00132448 File Offset: 0x00130648
		public void ShowKeyboardElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -(window.GetContentTextHeight(0) + 50f)), string.Empty);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x00132564 File Offset: 0x00130764
		public void ShowMouseElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange != null || !this._showFullAxisInputFields || this._showSplitAxisInputFields) ? this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x0013268C File Offset: 0x0013088C
		public void ShowElementAssignmentConflictWindow(ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, assignment, skipOtherPlayers);
			string text = (!flag) ? this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts) : this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName);
			int elementAlreadyInUseCanReplaceFontSize = this._language.GetElementAlreadyInUseCanReplaceFontSize(this._allowElementAssignmentConflicts);
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.elementAssignmentConflictWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text, elementAlreadyInUseCanReplaceFontSize);
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			if (flag)
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.okay, unityAction, unityAction, true);
			}
			else
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate
				{
					this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers);
				}, unityAction, true);
				if (this._allowElementAssignmentConflicts)
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.add, delegate
					{
						this.OnElementAssignmentAddConfirmed(window.id, this.pendingInputMapping, assignment);
					}, unityAction, false);
				}
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			}
			this.windowManager.Focus(window);
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x001328CC File Offset: 0x00130ACC
		public void ShowMouseAssignmentConflictWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer)
				{
					if (players[i].controllers.hasMouse)
					{
						otherPlayerName = players[i].descriptiveName;
						break;
					}
				}
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.mouseAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(otherPlayerName, this.currentPlayer.descriptiveName));
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate
			{
				this.OnMouseAssignmentConfirmed(window.id, this.currentPlayer);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00132A8C File Offset: 0x00130C8C
		public void ShowCalibrateControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
			if (calibrationWindow == null)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateControllerWindowTitle);
			calibrationWindow.SetJoystick(this.currentPlayer.id, currentJoystick);
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(calibrationWindow);
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00132B64 File Offset: 0x00130D64
		public void ShowCalibrateAxisStep1Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep1WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(joystick.AxisElementIdentifiers[axisIndex].name));
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 10f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x00132CD4 File Offset: 0x00130ED4
		public void ShowCalibrateAxisStep2Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep2WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(joystick.AxisElementIdentifiers[axisIndex].name));
			if (this.prefabs.moveStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x00132E44 File Offset: 0x00131044
		public void ShowEditInputBehaviorsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this._inputBehaviorSettings == null)
			{
				return;
			}
			InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
			if (inputBehaviorWindow == null)
			{
				return;
			}
			inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.inputBehaviorSettingsWindowTitle);
			inputBehaviorWindow.SetData(this.currentPlayer.id, this._inputBehaviorSettings);
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(inputBehaviorWindow);
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x00132F00 File Offset: 0x00131100
		public void ShowRestoreDefaultsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x00034F25 File Offset: 0x00033125
		public void CreateInputGrid()
		{
			this.InitializeInputGrid();
			this.CreateHeaderLabels();
			this.CreateActionLabelColumn();
			this.CreateKeyboardInputFieldColumn();
			this.CreateMouseInputFieldColumn();
			this.CreateControllerInputFieldColumn();
			this.CreateInputActionLabels();
			this.CreateInputFields();
			this.inputGrid.HideAll();
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00132F64 File Offset: 0x00131164
		public void InitializeInputGrid()
		{
			if (this.inputGrid == null)
			{
				this.inputGrid = new ControlMapper.InputGrid();
			}
			else
			{
				this.inputGrid.ClearAll();
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						if (mapCategory.userAssignable)
						{
							this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
							if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
							{
								IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
								for (int j = 0; j < actionCategoryIds.Count; j++)
								{
									int num = actionCategoryIds[j];
									InputCategory actionCategory = ReInput.mapping.GetActionCategory(num);
									if (actionCategory != null)
									{
										if (actionCategory.userAssignable)
										{
											this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num);
											foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(num))
											{
												if (inputAction.type == null)
												{
													if (this._showFullAxisInputFields)
													{
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, 0);
													}
													if (this._showSplitAxisInputFields)
													{
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, 1);
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, 2);
													}
												}
												else if (inputAction.type == 1)
												{
													this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, 1);
												}
											}
										}
									}
								}
							}
							else
							{
								IList<int> actionIds = mappingSet.actionIds;
								for (int k = 0; k < actionIds.Count; k++)
								{
									InputAction action = ReInput.mapping.GetAction(actionIds[k]);
									if (action != null)
									{
										if (action.type == null)
										{
											if (this._showFullAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, 0);
											}
											if (this._showSplitAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, 1);
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, 2);
											}
										}
										else if (action.type == 1)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, 1);
										}
									}
								}
							}
						}
					}
				}
			}
			this.references.inputGridLayoutElement.flexibleWidth = 0f;
			this.references.inputGridLayoutElement.preferredWidth = (float)this.inputGridWidth;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x00133250 File Offset: 0x00131450
		public void RefreshInputGridStructure()
		{
			if (this.currentMappingSet == null)
			{
				return;
			}
			this.inputGrid.HideAll();
			this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(1, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x001332B8 File Offset: 0x001314B8
		public void CreateHeaderLabels()
		{
			this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.actionsColumnHeadersGroup, this._actionLabelWidth).transform;
			ControlMapper.GUILabel guilabel = this.CreateLabel(this.prefabs.actionsHeaderLabel, this._language.actionColumnLabel.ToUpper(), this.references.inputGridHeader1, new Vector2(14f, -10f));
			guilabel.rectTransform.offsetMax = Vector2.zero;
			if (this._showKeyboard)
			{
				this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeyboardHeader", this.references.inputGridHeadersGroup, 226).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, new Vector2(14f, -10f));
				guilabel2.rectTransform.offsetMax = Vector2.zero;
			}
			if (this._showMouse)
			{
				this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.zero);
				guilabel2.SetTextAlignment(4);
			}
			if (this._showControllers)
			{
				this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, 230).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, new Vector2(14f, -10f));
				guilabel2.rectTransform.offsetMax = Vector2.zero;
			}
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x001334A4 File Offset: 0x001316A4
		public void CreateActionLabelColumn()
		{
			Transform transform = this.CreateNewColumnGroup("ActionLabelColumn", this.references.actionsColumn, this._actionLabelWidth).transform;
			this.references.inputGridActionColumn = transform;
			RectTransform component = transform.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(0.5f, 0.5f);
			component.offsetMin = new Vector2(14f, 0f);
			component.offsetMax = new Vector2(0f, -70f);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x00034F62 File Offset: 0x00033162
		public void CreateKeyboardInputFieldColumn()
		{
			if (!this._showKeyboard)
			{
				return;
			}
			this.CreateInputFieldColumn("KeyboardColumn", 0, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x00034F89 File Offset: 0x00033189
		public void CreateMouseInputFieldColumn()
		{
			if (!this._showMouse)
			{
				return;
			}
			this.CreateInputFieldColumn("MouseColumn", 1, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00034FB0 File Offset: 0x000331B0
		public void CreateControllerInputFieldColumn()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.CreateInputFieldColumn("ControllerColumn", 2, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00133550 File Offset: 0x00131750
		public void CreateInputFieldColumn(string name, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).transform;
			switch (controllerType)
			{
			case 0:
				this.references.inputGridKeyboardColumn = transform;
				break;
			case 1:
				this.references.inputGridMouseColumn = transform;
				break;
			case 2:
				this.references.inputGridControllerColumn = transform;
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x001335C8 File Offset: 0x001317C8
		public void CreateInputActionLabels()
		{
			Transform inputGridActionColumn = this.references.inputGridActionColumn;
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					float num = 6f;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						int num2 = 0;
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null)
							{
								if (actionCategory.userAssignable)
								{
									if (this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
									{
										if (this._showActionCategoryLabels)
										{
											if (num2 > 0)
											{
												num -= (float)this._inputRowCategorySpacing;
											}
											ControlMapper.GUILabel guilabel = this.CreateLabel(Localization.Translate(actionCategory.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
											guilabel.SetFontStyle(1);
											guilabel.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
											this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, guilabel);
											num -= this._inputRowHeight;
										}
										foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
										{
											if (inputAction.type == null)
											{
												if (this._showFullAxisInputFields)
												{
													ControlMapper.GUILabel guilabel2 = this.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, 0, guilabel2);
													num -= this._inputRowHeight;
												}
												if (this._showSplitAxisInputFields)
												{
													string labelText = string.IsNullOrEmpty(inputAction.positiveDescriptiveName) ? (Localization.Translate(inputAction.descriptiveName).text + " +") : Localization.Translate(inputAction.positiveDescriptiveName).text;
													ControlMapper.GUILabel guilabel2 = this.CreateLabel(labelText, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, 1, guilabel2);
													num -= this._inputRowHeight;
													string labelText2 = string.IsNullOrEmpty(inputAction.negativeDescriptiveName) ? (Localization.Translate(inputAction.descriptiveName).text + " -") : Localization.Translate(inputAction.negativeDescriptiveName).text;
													guilabel2 = this.CreateLabel(labelText2, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, 2, guilabel2);
													num -= this._inputRowHeight;
												}
											}
											else if (inputAction.type == 1)
											{
												ControlMapper.GUILabel guilabel2 = this.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
												guilabel2.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
												this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, 1, guilabel2);
												num -= this._inputRowHeight;
											}
										}
										num2++;
									}
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null)
							{
								if (action.userAssignable)
								{
									InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
									if (actionCategory2 != null)
									{
										if (actionCategory2.userAssignable)
										{
											if (action.type == null)
											{
												if (this._showFullAxisInputFields)
												{
													ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, 0, guilabel3);
													num -= this._inputRowHeight;
												}
												if (this._showSplitAxisInputFields)
												{
													ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.positiveDescriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, 1, guilabel3);
													num -= this._inputRowHeight;
													guilabel3 = this.CreateLabel(Localization.Translate(action.negativeDescriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, 2, guilabel3);
													num -= this._inputRowHeight;
												}
											}
											else if (action.type == 1)
											{
												ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
												guilabel3.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
												this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, 1, guilabel3);
												num -= this._inputRowHeight;
											}
										}
									}
								}
							}
						}
						for (int l = 0; l < 2; l++)
						{
							ControlMapper.GUILabel guilabel4 = this.CreateDeactivatedLabel(Localization.Translate((l != 0) ? "RemapFlipYAxis" : "RemapFlipXAxis").text, inputGridActionColumn, new Vector2(0f, num));
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
							this.inactiveAxisToggleObjects.Add(guilabel4.gameObject);
							guilabel4 = this.CreateLabel(Localization.Translate((l != 0) ? "RemapFlipYAxis" : "RemapFlipXAxis").text, inputGridActionColumn, new Vector2(0f, num));
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
							this.axisToggleObjects.Add(guilabel4.gameObject);
							num -= this._inputRowHeight;
						}
					}
					this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, -num);
				}
			}
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00133CE8 File Offset: 0x00131EE8
		public void CreateInputFields()
		{
			if (this._showControllers)
			{
				this.CreateInputFields(this.references.inputGridControllerColumn, 2, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
			}
			if (this._showKeyboard)
			{
				this.CreateInputFields(this.references.inputGridKeyboardColumn, 0, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
			}
			if (this._showMouse)
			{
				this.CreateInputFields(this.references.inputGridMouseColumn, 1, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00133D74 File Offset: 0x00131F74
		public void CreateInputFields(Transform columnXform, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int fieldWidth = maxWidth / cols;
					float num = 6f;
					int num2 = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null)
							{
								if (actionCategory.userAssignable)
								{
									if (this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
									{
										if (this._showActionCategoryLabels)
										{
											num -= ((num2 <= 0) ? this._inputRowHeight : (this._inputRowHeight + (float)this._inputRowCategorySpacing));
										}
										foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
										{
											if (inputAction.type == null)
											{
												if (this._showFullAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, 0, controllerType, cols, fieldWidth, ref num, disableFullAxis);
												}
												if (this._showSplitAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, 1, controllerType, cols, fieldWidth, ref num, false);
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, 2, controllerType, cols, fieldWidth, ref num, false);
												}
											}
											else if (inputAction.type == 1)
											{
												this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, 1, controllerType, cols, fieldWidth, ref num, false);
											}
											num2++;
										}
									}
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null)
							{
								if (action.userAssignable)
								{
									InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
									if (actionCategory2 != null)
									{
										if (actionCategory2.userAssignable)
										{
											if (action.type == null)
											{
												if (this._showFullAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, 0, controllerType, cols, fieldWidth, ref num, disableFullAxis);
												}
												if (this._showSplitAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, 1, controllerType, cols, fieldWidth, ref num, false);
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, 2, controllerType, cols, fieldWidth, ref num, false);
												}
											}
											else if (action.type == 1)
											{
												this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, 1, controllerType, cols, fieldWidth, ref num, false);
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

		// Token: 0x060041FB RID: 16891 RVA: 0x00134088 File Offset: 0x00132288
		public void CreateInputFieldSet(Transform parent, int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int cols, int fieldWidth, ref float yPos, bool disableFullAxis)
		{
			GameObject gameObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, yPos));
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(0f, 1f);
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = Vector2.zero;
			component.offsetMin = new Vector2(5f, component.offsetMin.y - 1f);
			component.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
			component.SetSizeWithCurrentAnchors(0, 227f);
			this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, gameObject);
			for (int i = 0; i < cols; i++)
			{
				int num = (axisRange != null) ? 0 : this._invertToggleWidth;
				ControlMapper.GUIInputField guiinputField = this.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, string.Empty, action.id, axisRange, controllerType, i);
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num);
				this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, i, guiinputField);
				if (axisRange == null && controllerType == 2)
				{
					GameObject gameObject2 = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, yPos - this._inputRowHeight * (float)((!(action.name == "MoveHorizontal")) ? 9 : 13)));
					HorizontalLayoutGroup horizontalLayoutGroup2 = gameObject2.AddComponent<HorizontalLayoutGroup>();
					RectTransform component2 = gameObject2.GetComponent<RectTransform>();
					component2.anchorMin = new Vector2(0f, 1f);
					component2.anchorMax = new Vector2(0f, 1f);
					component2.pivot = new Vector2(0f, 1f);
					component2.sizeDelta = Vector2.zero;
					component2.SetSizeWithCurrentAnchors(1, this._inputRowHeight);
					component2.SetSizeWithCurrentAnchors(0, 227f);
					component2.offsetMin = new Vector2(5f, component2.offsetMin.y);
					ControlMapper.GUIToggle guitoggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup2.transform, Vector2.zero, string.Empty, action.id, axisRange, controllerType, i);
					guitoggle.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, num);
					guiinputField.AddToggle(guitoggle);
					this.axisToggleObjects.Add(guitoggle.gameObject);
				}
			}
			yPos -= this._inputRowHeight;
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00134318 File Offset: 0x00132518
		public void PopulateInputFields()
		{
			this.inputGrid.InitializeFields(this.currentMapCategoryId);
			if (this.currentPlayer == null)
			{
				return;
			}
			this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
			foreach (ControlMapper.InputActionSet actionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
			{
				if (this._showKeyboard)
				{
					if (this.currentPlayerId == 0)
					{
						ControllerType controllerType = 0;
						int controllerId = 0;
						int layoutId = this._keyboardMapDefaultLayout;
						int maxFields = this._keyboardInputFieldCount;
						ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
						this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, maxFields);
					}
					else
					{
						this.DisableInputFieldGroup(actionSet, 0, this._keyboardInputFieldCount);
					}
				}
				if (this._showMouse)
				{
					ControllerType controllerType = 1;
					int controllerId = 0;
					int layoutId = this._mouseMapDefaultLayout;
					int maxFields = this._mouseInputFieldCount;
					ControllerMap controllerMapOrCreateNew2 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					if (this.currentPlayer.controllers.hasMouse)
					{
						this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew2, controllerType, controllerId, maxFields);
					}
				}
				if (this.isJoystickSelected && this.currentPlayer.controllers.joystickCount > 0)
				{
					ControllerType controllerType = 2;
					int controllerId = this.currentJoystick.id;
					int layoutId = this._joystickMapDefaultLayout;
					int maxFields = this._controllerInputFieldCount;
					ControllerMap controllerMapOrCreateNew3 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew3, controllerType, controllerId, maxFields);
				}
				else
				{
					this.DisableInputFieldGroup(actionSet, 2, this._controllerInputFieldCount);
				}
			}
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x001344C0 File Offset: 0x001326C0
		public void PopulateInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerMap controllerMap, ControllerType controllerType, int controllerId, int maxFields)
		{
			if (controllerMap == null)
			{
				return;
			}
			int num = 0;
			this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
			foreach (ActionElementMap actionElementMap in controllerMap.ElementMapsWithAction(actionSet.actionId))
			{
				if (actionElementMap.elementType == 1)
				{
					if (actionSet.axisRange == null)
					{
						continue;
					}
					if (actionSet.axisRange == 1)
					{
						if (actionElementMap.axisContribution == 1)
						{
							continue;
						}
					}
					else if (actionSet.axisRange == 2 && actionElementMap.axisContribution == null)
					{
						continue;
					}
					this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
				}
				else if (actionElementMap.elementType == null)
				{
					if (actionSet.axisRange == null)
					{
						if (actionElementMap.axisRange != null)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, actionElementMap.invert);
					}
					else if (actionSet.axisRange == 1)
					{
						if (actionElementMap.axisRange == null)
						{
							continue;
						}
						if (actionElementMap.axisContribution == 1)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
					}
					else if (actionSet.axisRange == 2)
					{
						if (actionElementMap.axisRange == null)
						{
							continue;
						}
						if (actionElementMap.axisContribution == null)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
					}
				}
				num++;
				if (num > maxFields)
				{
					break;
				}
			}
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x00134704 File Offset: 0x00132904
		public void DisableInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerType controllerType, int fieldCount)
		{
			for (int i = 0; i < fieldCount; i++)
			{
				ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, i);
				if (guiinputField != null)
				{
					guiinputField.SetInteractible(false, false);
				}
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x00134758 File Offset: 0x00132958
		public void CreateLayout()
		{
			this.references.playersGroup.gameObject.SetActive(this.showPlayers);
			this.references.controllerGroup.gameObject.SetActive(this._showControllers);
			this.references.removeControllerButton.gameObject.SetActive(this.showControllerGroupButtons);
			this.references.assignControllerButton.gameObject.SetActive(this.showControllerGroupButtons);
			this.references.assignedControllersGroup.gameObject.SetActive(this._showControllers && this.ShowAssignedControllers());
			this.references.settingsAndMapCategoriesGroup.gameObject.SetActive(this.showSettings || this.showMapCategories);
			this.references.settingsGroup.gameObject.SetActive(this.showSettings);
			this.references.mapCategoriesGroup.gameObject.SetActive(this.showMapCategories);
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x00034FD7 File Offset: 0x000331D7
		public void Draw()
		{
			this.DrawPlayersGroup();
			this.DrawControllersGroup();
			this.DrawSettingsGroup();
			this.DrawMapCategoriesGroup();
			this.DrawWindowButtonsGroup();
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x0013485C File Offset: 0x00132A5C
		public void DrawPlayersGroup()
		{
			this.references.playersGroup.labelText = this._language.playersGroupLabel;
			this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
			for (int i = 0; i < this.playerCount; i++)
			{
				Player player = ReInput.players.GetPlayer(i);
				if (player != null)
				{
					GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.playerButton, this.references.playersGroup.content, "Player" + i + "Button");
					ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(gameObject);
					guibutton.SetLabel(Localization.Translate(player.descriptiveName).text);
					guibutton.SetButtonInfoData("PlayerSelection", player.id);
					guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					guibutton.gameObject.GetComponent<CustomButtonPlayerSelect>().mapper = this;
					this.playerButtons.Add(guibutton);
				}
			}
			this.playerButtons[0].SetInteractible(false, true);
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00034FF7 File Offset: 0x000331F7
		public Selectable GetUnselectedPlayerButton()
		{
			return this.playerButtons[1 - this.currentPlayerId].gameObject.GetComponent<Selectable>();
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0013498C File Offset: 0x00132B8C
		public void DrawControllersGroup()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
			this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
			this.references.controllerNameLabel.gameObject.SetActive(this._showControllerNameLabel);
			this.references.controllerGroupLabelGroup.gameObject.SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
			if (this.ShowAssignedControllers())
			{
				this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
				this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
			}
			ButtonInfo component = this.references.removeControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.removeControllerButtonLabel;
			component = this.references.calibrateControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.calibrateControllerButtonLabel;
			component = this.references.assignControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.assignControllerButtonLabel;
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.zero);
			guibutton.SetInteractible(false, false, true);
			this.assignedControllerButtonsPlaceholder = guibutton;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x00134B04 File Offset: 0x00132D04
		public void DrawSettingsGroup()
		{
			if (!this.showSettings)
			{
				return;
			}
			this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
			this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.zero);
			this.miscInstantiatedObjects.Add(guibutton.gameObject);
			guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
			guibutton.SetButtonInfoData("EditInputBehaviors", 0);
			guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00134BBC File Offset: 0x00132DBC
		public void DrawMapCategoriesGroup()
		{
			if (!this.showMapCategories)
			{
				return;
			}
			if (this._mappingSets == null)
			{
				return;
			}
			this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
			this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, mapCategory.name + "Button");
						ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(gameObject);
						guibutton.SetLabel(Localization.Translate(mapCategory.descriptiveName).text);
						guibutton.SetButtonInfoData("MapCategorySelection", mapCategory.id);
						guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
						guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
						this.mapCategoryButtons.Add(guibutton);
					}
				}
			}
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x00134CFC File Offset: 0x00132EFC
		public void DrawWindowButtonsGroup()
		{
			this.references.doneButton.GetComponent<ButtonInfo>().text.text = this._language.doneButtonLabel;
			this.references.restoreDefaultsButton.GetComponent<ButtonInfo>().text.text = this._language.restoreDefaultsButtonLabel;
			this.UpdateRumbleText();
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x00134D5C File Offset: 0x00132F5C
		public void Redraw(bool listsChanged, bool playTransitions)
		{
			this.RedrawPlayerGroup(playTransitions);
			this.RedrawControllerGroup();
			this.RedrawMapCategoriesGroup(playTransitions);
			this.RedrawInputGrid(listsChanged);
			if (this.currentUISelection == null || !this.currentUISelection.activeInHierarchy)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00134DAC File Offset: 0x00132FAC
		public void RedrawPlayerGroup(bool playTransitions)
		{
			if (!this.showPlayers)
			{
				return;
			}
			for (int i = 0; i < this.playerButtons.Count; i++)
			{
				bool flag = this.currentPlayerId != this.playerButtons[i].buttonInfo.intData;
				this.playerButtons[i].SetInteractible(flag, playTransitions);
				this.playerButtons[i].gameObject.GetComponent<CustomButton>().SetNavOnToggle(flag);
			}
			this.playerButtons[1].SetInteractible(PlayerManager.Multiplayer, playTransitions);
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00134E4C File Offset: 0x0013304C
		public void RedrawControllerGroup()
		{
			int num = -1;
			this.references.controllerNameLabel.text = this._language.none;
			UITools.SetInteractable(this.references.removeControllerButton, false, false);
			UITools.SetInteractable(this.references.assignControllerButton, false, false);
			UITools.SetInteractable(this.references.calibrateControllerButton, false, false);
			if (this.ShowAssignedControllers())
			{
				foreach (ControlMapper.GUIButton guibutton in this.assignedControllerButtons)
				{
					if (!(guibutton.gameObject == null))
					{
						if (this.currentUISelection == guibutton.gameObject)
						{
							num = guibutton.buttonInfo.intData;
						}
						Object.Destroy(guibutton.gameObject);
					}
				}
				this.assignedControllerButtons.Clear();
				this.assignedControllerButtonsPlaceholder.SetActive(true);
			}
			Player player = ReInput.players.GetPlayer(this.currentPlayerId);
			if (player == null)
			{
				return;
			}
			if (this.ShowAssignedControllers())
			{
				if (player.controllers.joystickCount > 0)
				{
					this.assignedControllerButtonsPlaceholder.SetActive(false);
				}
				foreach (Joystick joystick in player.controllers.Joysticks)
				{
					ControlMapper.GUIButton guibutton2 = this.CreateButton(joystick.name, this.references.assignedControllersGroup.content, Vector2.zero);
					guibutton2.SetButtonInfoData("AssignedControllerSelection", joystick.id);
					guibutton2.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton2.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.assignedControllerButtons.Add(guibutton2);
					if (joystick.id == this.currentJoystickId)
					{
						guibutton2.SetInteractible(false, true);
					}
				}
				if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
				{
					this.currentJoystickId = player.controllers.Joysticks[0].id;
					this.assignedControllerButtons[0].SetInteractible(false, false);
				}
				if (num >= 0)
				{
					foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
					{
						if (guibutton3.buttonInfo.intData == num)
						{
							this.SetUISelection(guibutton3.gameObject);
							break;
						}
					}
				}
			}
			else if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
			{
				this.currentJoystickId = player.controllers.Joysticks[0].id;
			}
			if (this.isJoystickSelected && player.controllers.joystickCount > 0 && this.currentPlayerId == 0)
			{
				this.references.removeControllerButton.interactable = true;
				this.references.controllerNameLabel.text = this.currentJoystick.name;
				if (this.currentJoystick.axisCount > 0)
				{
					this.references.calibrateControllerButton.interactable = true;
				}
			}
			int joystickCount = player.controllers.joystickCount;
			int joystickCount2 = ReInput.controllers.joystickCount;
			int maxControllersPerPlayer = this.GetMaxControllersPerPlayer();
			bool flag = maxControllersPerPlayer == 0;
			if (joystickCount2 > 0 && joystickCount < joystickCount2 && (maxControllersPerPlayer == 1 || flag || joystickCount < maxControllersPerPlayer))
			{
				UITools.SetInteractable(this.references.assignControllerButton, true, false);
			}
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00135240 File Offset: 0x00133440
		public void RedrawMapCategoriesGroup(bool playTransitions)
		{
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this.mapCategoryButtons.Count; i++)
			{
				bool state = this.currentMapCategoryId != this.mapCategoryButtons[i].buttonInfo.intData;
				this.mapCategoryButtons[i].SetInteractible(state, playTransitions);
			}
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x00035016 File Offset: 0x00033216
		public void RedrawInputGrid(bool listsChanged)
		{
			if (listsChanged)
			{
				this.RefreshInputGridStructure();
			}
			this.PopulateInputFields();
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0003502A File Offset: 0x0003322A
		public void ForceRefresh()
		{
			if (this.windowManager.isWindowOpen)
			{
				this.CloseAllWindows();
			}
			else
			{
				this.Redraw(false, false);
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x001352AC File Offset: 0x001334AC
		public void CreateInputCategoryRow(ref int rowCount, InputCategory category)
		{
			this.CreateLabel(Localization.Translate(category.descriptiveName).text, this.references.inputGridActionColumn, new Vector2(0f, (float)rowCount * this._inputRowHeight * -1f));
			rowCount++;
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x0003504F File Offset: 0x0003324F
		public ControlMapper.GUILabel CreateLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x00035065 File Offset: 0x00033265
		public ControlMapper.GUILabel CreateDeactivatedLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridDeactivatedLabel, labelText, parent, offset);
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00135300 File Offset: 0x00133500
		public ControlMapper.GUILabel CreateLabel(GameObject prefab, string labelText, Transform parent, Vector2 offset)
		{
			GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
			Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
			if (componentInSelfOrChildren == null)
			{
				Debug.LogError("Rewired Control Mapper: Label prefab is missing Text component!");
				return null;
			}
			componentInSelfOrChildren.text = labelText;
			return new ControlMapper.GUILabel(gameObject);
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x00135344 File Offset: 0x00133544
		public ControlMapper.GUIButton CreateButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x00135374 File Offset: 0x00133574
		public ControlMapper.GUIButton CreateFitButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x001353A4 File Offset: 0x001335A4
		public ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIInputField guiinputField = this.CreateInputField(parent, offset);
			guiinputField.SetLabel(string.Empty);
			guiinputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
			guiinputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
			guiinputField.fieldInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guiinputField;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0003507B File Offset: 0x0003327B
		public ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x001353F8 File Offset: 0x001335F8
		public ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIToggle guitoggle = this.CreateToggle(prefab, parent, offset);
			guitoggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
			guitoggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
			guitoggle.toggleInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guitoggle;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00035095 File Offset: 0x00033295
		public ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00135444 File Offset: 0x00133644
		public GameObject InstantiateGUIObject(GameObject prefab, Transform parent, Vector2 offset)
		{
			if (prefab == null)
			{
				Debug.LogError("Rewired Control Mapper: Prefab is null!");
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(prefab);
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x0013547C File Offset: 0x0013367C
		public GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			gameObject.AddComponent<RectTransform>();
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x001354A8 File Offset: 0x001336A8
		public GameObject InitializeNewGUIGameObject(GameObject gameObject, Transform parent, Vector2 offset)
		{
			if (gameObject == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject is null!");
				return null;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject does not have a RectTransform component!");
				return gameObject;
			}
			if (parent != null)
			{
				component.SetParent(parent, false);
			}
			component.anchoredPosition = offset;
			return gameObject;
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x00135508 File Offset: 0x00133708
		public GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
		{
			GameObject gameObject = this.CreateNewGUIObject(name, parent, Vector2.zero);
			this.inputGrid.AddGroup(gameObject);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			if (maxWidth >= 0)
			{
				layoutElement.preferredWidth = (float)maxWidth;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 0f);
			return gameObject;
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x000350A5 File Offset: 0x000332A5
		public Window OpenWindow(bool closeOthers)
		{
			return this.OpenWindow(string.Empty, closeOthers);
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x00135578 File Offset: 0x00133778
		public Window OpenWindow(string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x000350B3 File Offset: 0x000332B3
		public Window OpenWindow(GameObject windowPrefab, bool closeOthers)
		{
			return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x001355C4 File Offset: 0x001337C4
		public Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(windowPrefab, name);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x00135608 File Offset: 0x00133808
		public void OpenModal(string title, string message, string confirmText, Action<int> confirmAction, string cancelText, Action<int> cancelAction, bool closeOthers)
		{
			Window window = this.OpenWindow(closeOthers);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, title);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), message);
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, delegate
			{
				this.OnRestoreDefaultsConfirmed(window.id);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, true);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x000350C2 File Offset: 0x000332C2
		public void CloseWindow(int windowId)
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseWindow(windowId);
			this.ChildWindowClosed();
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x000350E7 File Offset: 0x000332E7
		public void CloseTopWindow()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseTop();
			this.ChildWindowClosed();
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0003510B File Offset: 0x0003330B
		public void CloseAllWindows()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CancelAll();
			this.ChildWindowClosed();
			this.InputPollingStopped();
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x00135718 File Offset: 0x00133918
		public void ChildWindowOpened()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(false);
			if (this._PopupWindowOpenedEvent != null)
			{
				this._PopupWindowOpenedEvent();
			}
			if (this._onPopupWindowOpened != null)
			{
				this._onPopupWindowOpened.Invoke();
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0013576C File Offset: 0x0013396C
		public void ChildWindowClosed()
		{
			if (this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(true);
			if (this._PopupWindowClosedEvent != null)
			{
				this._PopupWindowClosedEvent();
			}
			if (this._onPopupWindowClosed != null)
			{
				this._onPopupWindowClosed.Invoke();
			}
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x001357C0 File Offset: 0x001339C0
		public bool HasElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck) || player.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck);
			}
			return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck);
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00135840 File Offset: 0x00133A40
		public bool IsBlockingAssignmentConflict(ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						return true;
					}
				}
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo2 in this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck))
				{
					if (!elementAssignmentConflictInfo2.isUserAssignable)
					{
						return true;
					}
				}
			}
			else
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck))
				{
					if (!elementAssignmentConflictInfo3.isUserAssignable)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x001359A8 File Offset: 0x00133BA8
		public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				yield break;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				yield break;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo conflict in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict.isUserAssignable)
					{
						yield return conflict;
					}
				}
				foreach (ElementAssignmentConflictInfo conflict2 in player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict2.isUserAssignable)
					{
						yield return conflict2;
					}
				}
			}
			else
			{
				foreach (ElementAssignmentConflictInfo conflict3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict3.isUserAssignable)
					{
						yield return conflict3;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x001359E8 File Offset: 0x00133BE8
		public bool CreateConflictCheck(ControlMapper.InputMapping mapping, ElementAssignment assignment, out ElementAssignmentConflictCheck conflictCheck)
		{
			if (mapping == null || this.currentPlayer == null)
			{
				conflictCheck = default(ElementAssignmentConflictCheck);
				return false;
			}
			conflictCheck = assignment.ToElementAssignmentConflictCheck();
			conflictCheck.playerId = this.currentPlayer.id;
			conflictCheck.controllerType = mapping.controllerType;
			conflictCheck.controllerId = mapping.controllerId;
			conflictCheck.controllerMapId = mapping.map.id;
			conflictCheck.controllerMapCategoryId = mapping.map.categoryId;
			if (mapping.aem != null)
			{
				conflictCheck.elementMapId = mapping.aem.id;
			}
			return true;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x00135A84 File Offset: 0x00133C84
		public void PollKeyboardForAssignment(out ControllerPollingInfo pollingInfo, out bool modifierKeyPressed, out ModifierKeyFlags modifierFlags, out string label)
		{
			pollingInfo = default(ControllerPollingInfo);
			label = string.Empty;
			modifierKeyPressed = false;
			modifierFlags = 0;
			int num = 0;
			ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
			ControllerPollingInfo controllerPollingInfo2 = default(ControllerPollingInfo);
			ModifierKeyFlags modifierKeyFlags = 0;
			foreach (ControllerPollingInfo controllerPollingInfo3 in ReInput.controllers.Keyboard.PollForAllKeys())
			{
				KeyCode keyboardKey = controllerPollingInfo3.keyboardKey;
				if (keyboardKey != 313)
				{
					if (Keyboard.IsModifierKey(controllerPollingInfo3.keyboardKey))
					{
						if (num == 0)
						{
							controllerPollingInfo2 = controllerPollingInfo3;
							modifierKeyFlags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
							num++;
						}
					}
					else if (controllerPollingInfo.keyboardKey == null)
					{
						controllerPollingInfo = controllerPollingInfo3;
					}
				}
			}
			if (controllerPollingInfo.keyboardKey == null)
			{
				if (num > 0)
				{
					modifierKeyPressed = true;
					if (num == 1)
					{
						if (ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1f)
						{
							pollingInfo = controllerPollingInfo2;
							return;
						}
						label = Localization.Translate(Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey)).text;
					}
					else
					{
						label = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags);
					}
				}
				return;
			}
			if (!ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo.keyboardKey))
			{
				return;
			}
			if (num == 0)
			{
				pollingInfo = controllerPollingInfo;
				return;
			}
			pollingInfo = controllerPollingInfo;
			modifierFlags = modifierKeyFlags;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x00135C10 File Offset: 0x00133E10
		public void StartAxisCalibration(int axisIndex)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			if (axisIndex < 0 || axisIndex >= currentJoystick.axisCount)
			{
				return;
			}
			this.pendingAxisCalibration = new ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
			this.ShowCalibrateAxisStep1Window();
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00035135 File Offset: 0x00033335
		public void EndAxisCalibration()
		{
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			this.pendingAxisCalibration.Commit();
			this.pendingAxisCalibration = null;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00035155 File Offset: 0x00033355
		public void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00035173 File Offset: 0x00033373
		public void RestoreLastUISelection()
		{
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetDefaultUISelection();
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x00135C70 File Offset: 0x00133E70
		public void SetDefaultUISelection()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.references.defaultSelection == null)
			{
				this.SetUISelection(null);
			}
			else
			{
				this.SetUISelection(this.references.defaultSelection.gameObject);
			}
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x00135CC4 File Offset: 0x00133EC4
		public void SelectDefaultMapCategory(bool redraw)
		{
			this.currentMapCategoryId = this.GetDefaultMapCategoryId();
			this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					this.currentMapCategoryId = this._mappingSets[i].mapCategoryId;
					break;
				}
			}
			if (this.currentMapCategoryId < 0)
			{
				return;
			}
			for (int j = 0; j < this._mappingSets.Length; j++)
			{
				bool state = this._mappingSets[j].mapCategoryId != this.currentMapCategoryId;
				this.mapCategoryButtons[j].SetInteractible(state, false);
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x000351A9 File Offset: 0x000333A9
		public void CheckUISelection()
		{
			if (!this.isFocused)
			{
				return;
			}
			if (this.currentUISelection == null)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x000351CE File Offset: 0x000333CE
		public void OnUIElementSelected(GameObject selectedObject)
		{
			this.lastUISelection = selectedObject;
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x000351D7 File Offset: 0x000333D7
		public void SetIsFocused(bool state)
		{
			this.references.mainCanvasGroup.interactable = state;
			if (state)
			{
				this.Redraw(false, false);
				this.RestoreLastUISelection();
				this.blockInputOnFocusEndTime = Time.unscaledTime + 0.1f;
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x0003520F File Offset: 0x0003340F
		public void Toggle()
		{
			if (this.isOpen)
			{
				this.Close(true);
			}
			else
			{
				this.Open();
			}
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x0003522E File Offset: 0x0003342E
		public void Open()
		{
			this.Open(false);
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x00135DA4 File Offset: 0x00133FA4
		public void Open(bool force)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!this.initialized)
			{
				return;
			}
			if (!force && this.isOpen)
			{
				return;
			}
			this.Clear();
			this.canvas.SetActive(true);
			this.OnPlayerSelected(0, false);
			this.SelectDefaultMapCategory(false);
			this.SetDefaultUISelection();
			this.Redraw(true, false);
			if (this._ScreenOpenedEvent != null)
			{
				this._ScreenOpenedEvent();
			}
			if (this._onScreenOpened != null)
			{
				this._onScreenOpened.Invoke();
			}
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00135E3C File Offset: 0x0013403C
		public void Close(bool save)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (save && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
			this.Clear();
			this.canvas.SetActive(false);
			this.SetUISelection(null);
			if (this._ScreenClosedEvent != null)
			{
				this._ScreenClosedEvent();
			}
			if (this._onScreenClosed != null)
			{
				this._onScreenClosed.Invoke();
			}
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x00035237 File Offset: 0x00033437
		public void Clear()
		{
			this.windowManager.CancelAll();
			this.lastUISelection = null;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.InputPollingStopped();
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0003525F File Offset: 0x0003345F
		public void ClearCompletely()
		{
			this.ClearSpawnedObjects();
			this.ClearAllVars();
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00135EC0 File Offset: 0x001340C0
		public void ClearSpawnedObjects()
		{
			this.windowManager.ClearCompletely();
			this.inputGrid.ClearAll();
			foreach (ControlMapper.GUIButton guibutton in this.playerButtons)
			{
				Object.Destroy(guibutton.gameObject);
			}
			this.playerButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton2 in this.mapCategoryButtons)
			{
				Object.Destroy(guibutton2.gameObject);
			}
			this.mapCategoryButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
			{
				Object.Destroy(guibutton3.gameObject);
			}
			this.assignedControllerButtons.Clear();
			if (this.assignedControllerButtonsPlaceholder != null)
			{
				Object.Destroy(this.assignedControllerButtonsPlaceholder.gameObject);
				this.assignedControllerButtonsPlaceholder = null;
			}
			foreach (GameObject gameObject in this.miscInstantiatedObjects)
			{
				Object.Destroy(gameObject);
			}
			this.miscInstantiatedObjects.Clear();
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0003526D File Offset: 0x0003346D
		public void ClearVarsOnPlayerChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00035276 File Offset: 0x00033476
		public void ClearVarsOnJoystickChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00136074 File Offset: 0x00134274
		public void ClearAllVars()
		{
			this.initialized = false;
			ControlMapper.Instance = null;
			this.playerCount = 0;
			this.inputGrid = null;
			this.windowManager = null;
			this.currentPlayerId = -1;
			this.currentMapCategoryId = -1;
			this.playerButtons = null;
			this.mapCategoryButtons = null;
			this.miscInstantiatedObjects = null;
			this.canvas = null;
			this.lastUISelection = null;
			this.currentJoystickId = -1;
			this.axisToggleObjects.Clear();
			this.inactiveAxisToggleObjects.Clear();
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.inputFieldActivatedDelegate = null;
			this.inputFieldInvertToggleStateChangedDelegate = null;
			this.isPollingForInput = false;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x0003527F File Offset: 0x0003347F
		public void Reset()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ClearCompletely();
			this.Initialize();
			if (this.isOpen)
			{
				this.Open(true);
			}
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00136114 File Offset: 0x00134314
		public void SetActionAxisInverted(bool state, ControllerType controllerType, int actionElementMapId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			ControllerMapWithAxes controllerMapWithAxes = this.GetControllerMap(controllerType) as ControllerMapWithAxes;
			if (controllerMapWithAxes == null)
			{
				return;
			}
			ActionElementMap elementMap = controllerMapWithAxes.GetElementMap(actionElementMapId);
			if (elementMap == null)
			{
				return;
			}
			elementMap.invert = state;
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00136158 File Offset: 0x00134358
		public ControllerMap GetControllerMap(ControllerType type)
		{
			if (this.currentPlayer == null)
			{
				return null;
			}
			int num = 0;
			switch (type)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				if (this.currentPlayer.controllers.joystickCount <= 0)
				{
					return null;
				}
				num = this.currentJoystick.id;
				break;
			default:
				throw new NotImplementedException();
			}
			return this.currentPlayer.controllers.maps.GetFirstMapInCategory(type, num, this.currentMapCategoryId);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x001361E8 File Offset: 0x001343E8
		public ControllerMap GetControllerMapOrCreateNew(ControllerType controllerType, int controllerId, int layoutId)
		{
			ControllerMap controllerMap = this.GetControllerMap(controllerType);
			if (controllerMap == null)
			{
				this.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
				controllerMap = this.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
			}
			return controllerMap;
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x00136244 File Offset: 0x00134444
		public int CountIEnumerable<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			if (enumerator == null)
			{
				return 0;
			}
			int num = 0;
			while (enumerator.MoveNext())
			{
				num++;
			}
			return num;
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x00136280 File Offset: 0x00134480
		public int GetDefaultMapCategoryId()
		{
			if (this._mappingSets.Length == 0)
			{
				return 0;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					return this._mappingSets[i].mapCategoryId;
				}
			}
			return 0;
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x001362E8 File Offset: 0x001344E8
		public void SubscribeFixedUISelectionEvents()
		{
			if (this.references.fixedSelectableUIElements == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.references.fixedSelectableUIElements)
			{
				UIElementInfo component = UnityTools.GetComponent<UIElementInfo>(gameObject);
				if (!(component == null))
				{
					component.OnSelectedEvent += this.OnUIElementSelected;
				}
			}
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x00136354 File Offset: 0x00134554
		public void SubscribeMenuControlInputEvents()
		{
			this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x001363C4 File Offset: 0x001345C4
		public void UnsubscribeMenuControlInputEvents()
		{
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00136434 File Offset: 0x00134634
		public void SubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.AddInputEventDelegate(callback, 0, 3, actionId);
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x001364D0 File Offset: 0x001346D0
		public void UnsubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.RemoveInputEventDelegate(callback, 0, 3, actionId);
			}
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x000352AB File Offset: 0x000334AB
		public int GetMaxControllersPerPlayer()
		{
			if (this._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks)
			{
				return this._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer;
			}
			return this._maxControllersPerPlayer;
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x000352E3 File Offset: 0x000334E3
		public bool ShowAssignedControllers()
		{
			return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0003530E File Offset: 0x0003350E
		public void InspectorPropertyChanged(bool reset = false)
		{
			if (reset)
			{
				this.Reset();
			}
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00136574 File Offset: 0x00134774
		public void AssignController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (player.controllers.ContainsController(2, controllerId))
			{
				return;
			}
			if (this.GetMaxControllersPerPlayer() == 1)
			{
				this.RemoveAllControllers(player);
				this.ClearVarsOnJoystickChange();
			}
			foreach (Player player2 in ReInput.players.Players)
			{
				if (player2 != player)
				{
					this.RemoveController(player2, controllerId);
				}
			}
			player.controllers.AddController(2, controllerId, false);
			PlayerManager.ControllerRemapped((this.currentPlayerId != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne, true, controllerId);
			this.OnPlayerSelected(this.currentPlayerId, true);
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.LoadControllerData(player.id, 2, controllerId);
			}
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x00136664 File Offset: 0x00134864
		public void RemoveAllControllers(Player player)
		{
			if (player == null)
			{
				return;
			}
			IList<Joystick> joysticks = player.controllers.Joysticks;
			for (int i = joysticks.Count - 1; i >= 0; i--)
			{
				this.RemoveController(player, joysticks[i].id);
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x001366B0 File Offset: 0x001348B0
		public void RemoveController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(2, controllerId))
			{
				return;
			}
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.SaveControllerData(player.id, 2, controllerId);
			}
			player.controllers.RemoveController(2, controllerId);
			PlayerManager.ControllerRemapped((this.currentPlayerId != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne, false, 0);
			this.OnPlayerSelected(this.currentPlayerId, true);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0003531C File Offset: 0x0003351C
		public bool IsAllowedAssignment(ControlMapper.InputMapping pendingInputMapping, ControllerPollingInfo pollingInfo)
		{
			return pendingInputMapping != null && (pendingInputMapping.axisRange != null || this._showSplitAxisInputFields || pollingInfo.elementType != 1);
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x00136728 File Offset: 0x00134928
		public void InputPollingStarted()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = true;
			if (!flag)
			{
				if (this._InputPollingStartedEvent != null)
				{
					this._InputPollingStartedEvent();
				}
				if (this._onInputPollingStarted != null)
				{
					this._onInputPollingStarted.Invoke();
				}
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x00136778 File Offset: 0x00134978
		public void InputPollingStopped()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = false;
			if (flag)
			{
				if (this._InputPollingEndedEvent != null)
				{
					this._InputPollingEndedEvent();
				}
				if (this._onInputPollingEnded != null)
				{
					this._onInputPollingEnded.Invoke();
				}
			}
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0003534C File Offset: 0x0003354C
		public void OnControlsChanged()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.Redraw(false, false);
			}
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x001367C8 File Offset: 0x001349C8
		public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
		{
			if (ControlMapper.Instance == null)
			{
				return;
			}
			if (ControlMapper.Instance._themeSettings == null)
			{
				return;
			}
			if (!ControlMapper.Instance._useThemeSettings)
			{
				return;
			}
			ControlMapper.Instance._themeSettings[Mathf.Clamp(ControlMapper.Instance.currentPlayerId, 0, 1)].Apply(elementInfo);
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x00035366 File Offset: 0x00033566
		public static LanguageData GetLanguage()
		{
			if (ControlMapper.Instance == null)
			{
				return null;
			}
			return ControlMapper.Instance._language;
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x00035384 File Offset: 0x00033584
		public static int CurrentPlayer()
		{
			return ControlMapper.Instance.currentPlayerId;
		}

		// Token: 0x040033D3 RID: 13267
		public const float blockInputOnFocusTimeout = 0.1f;

		// Token: 0x040033D4 RID: 13268
		public const string buttonIdentifier_playerSelection = "PlayerSelection";

		// Token: 0x040033D5 RID: 13269
		public const string buttonIdentifier_removeController = "RemoveController";

		// Token: 0x040033D6 RID: 13270
		public const string buttonIdentifier_assignController = "AssignController";

		// Token: 0x040033D7 RID: 13271
		public const string buttonIdentifier_calibrateController = "CalibrateController";

		// Token: 0x040033D8 RID: 13272
		public const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";

		// Token: 0x040033D9 RID: 13273
		public const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";

		// Token: 0x040033DA RID: 13274
		public const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";

		// Token: 0x040033DB RID: 13275
		public const string buttonIdentifier_done = "Done";

		// Token: 0x040033DC RID: 13276
		public const string buttonIdentifier_restoreDefaults = "RestoreDefaults";

		// Token: 0x040033DD RID: 13277
		public const string buttonIdentifier_toggleRumble = "ToggleRumble";

		// Token: 0x040033DE RID: 13278
		[SerializeField]
		[Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
		public InputManager _rewiredInputManager;

		// Token: 0x040033DF RID: 13279
		[SerializeField]
		[Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
		public bool _dontDestroyOnLoad;

		// Token: 0x040033E0 RID: 13280
		[SerializeField]
		[Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
		public bool _openOnStart;

		// Token: 0x040033E1 RID: 13281
		[SerializeField]
		[Tooltip("The Layout of the Keyboard Maps to be displayed.")]
		public int _keyboardMapDefaultLayout;

		// Token: 0x040033E2 RID: 13282
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		public int _mouseMapDefaultLayout;

		// Token: 0x040033E3 RID: 13283
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		public int _joystickMapDefaultLayout;

		// Token: 0x040033E4 RID: 13284
		[SerializeField]
		public ControlMapper.MappingSet[] _mappingSets = new ControlMapper.MappingSet[]
		{
			ControlMapper.MappingSet.Default
		};

		// Token: 0x040033E5 RID: 13285
		[SerializeField]
		[Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
		public bool _showPlayers = true;

		// Token: 0x040033E6 RID: 13286
		[SerializeField]
		[Tooltip("Display the Controller column for input mapping.")]
		public bool _showControllers = true;

		// Token: 0x040033E7 RID: 13287
		[SerializeField]
		[Tooltip("Display the Keyboard column for input mapping.")]
		public bool _showKeyboard = true;

		// Token: 0x040033E8 RID: 13288
		[SerializeField]
		[Tooltip("Display the Mouse column for input mapping.")]
		public bool _showMouse = true;

		// Token: 0x040033E9 RID: 13289
		[SerializeField]
		[Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
		public int _maxControllersPerPlayer = 1;

		// Token: 0x040033EA RID: 13290
		[SerializeField]
		[Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
		public bool _showActionCategoryLabels;

		// Token: 0x040033EB RID: 13291
		[SerializeField]
		[Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		public int _keyboardInputFieldCount = 2;

		// Token: 0x040033EC RID: 13292
		[SerializeField]
		[Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		public int _mouseInputFieldCount = 1;

		// Token: 0x040033ED RID: 13293
		[SerializeField]
		[Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		public int _controllerInputFieldCount = 1;

		// Token: 0x040033EE RID: 13294
		[SerializeField]
		[Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
		public bool _showFullAxisInputFields = true;

		// Token: 0x040033EF RID: 13295
		[SerializeField]
		[Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
		public bool _showSplitAxisInputFields = true;

		// Token: 0x040033F0 RID: 13296
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
		public bool _allowElementAssignmentConflicts;

		// Token: 0x040033F1 RID: 13297
		[SerializeField]
		[Tooltip("The width in relative pixels of the Action label column.")]
		public int _actionLabelWidth = 360;

		// Token: 0x040033F2 RID: 13298
		[SerializeField]
		[Tooltip("The width in relative pixels of the Keyboard column.")]
		public int _keyboardColMaxWidth = 360;

		// Token: 0x040033F3 RID: 13299
		[SerializeField]
		[Tooltip("The width in relative pixels of the Mouse column.")]
		public int _mouseColMaxWidth = 200;

		// Token: 0x040033F4 RID: 13300
		[SerializeField]
		[Tooltip("The width in relative pixels of the Controller column.")]
		public int _controllerColMaxWidth = 200;

		// Token: 0x040033F5 RID: 13301
		[SerializeField]
		[Tooltip("The height in relative pixels of the input grid button rows.")]
		public float _inputRowHeight = 40f;

		// Token: 0x040033F6 RID: 13302
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between columns.")]
		public float _inputColumnSpacing = 40f;

		// Token: 0x040033F7 RID: 13303
		[SerializeField]
		[Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
		public int _inputRowCategorySpacing = 20;

		// Token: 0x040033F8 RID: 13304
		[SerializeField]
		[Tooltip("The width in relative pixels of the invert toggle buttons.")]
		public int _invertToggleWidth = 40;

		// Token: 0x040033F9 RID: 13305
		[SerializeField]
		[Tooltip("The width in relative pixels of generated popup windows.")]
		public int _defaultWindowWidth = 500;

		// Token: 0x040033FA RID: 13306
		[SerializeField]
		[Tooltip("The height in relative pixels of generated popup windows.")]
		public int _defaultWindowHeight = 400;

		// Token: 0x040033FB RID: 13307
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
		public float _controllerAssignmentTimeout = 5f;

		// Token: 0x040033FC RID: 13308
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
		public float _preInputAssignmentTimeout = 5f;

		// Token: 0x040033FD RID: 13309
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
		public float _inputAssignmentTimeout = 5f;

		// Token: 0x040033FE RID: 13310
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
		public float _axisCalibrationTimeout = 5f;

		// Token: 0x040033FF RID: 13311
		[SerializeField]
		[Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
		public bool _ignoreMouseXAxisAssignment = true;

		// Token: 0x04003400 RID: 13312
		[SerializeField]
		[Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
		public bool _ignoreMouseYAxisAssignment = true;

		// Token: 0x04003401 RID: 13313
		[SerializeField]
		[Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
		public int _screenToggleAction = -1;

		// Token: 0x04003402 RID: 13314
		[SerializeField]
		[Tooltip("An Action that when activated will open the main screen if it is closed.")]
		public int _screenOpenAction = -1;

		// Token: 0x04003403 RID: 13315
		[SerializeField]
		[Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
		public int _screenCloseAction = -1;

		// Token: 0x04003404 RID: 13316
		[SerializeField]
		[Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
		public int _universalCancelAction = -1;

		// Token: 0x04003405 RID: 13317
		[SerializeField]
		[Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
		public bool _universalCancelClosesScreen = true;

		// Token: 0x04003406 RID: 13318
		[SerializeField]
		[Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
		public bool _showInputBehaviorSettings;

		// Token: 0x04003407 RID: 13319
		[SerializeField]
		[Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
		public ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;

		// Token: 0x04003408 RID: 13320
		[SerializeField]
		[Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
		public bool _useThemeSettings = true;

		// Token: 0x04003409 RID: 13321
		[SerializeField]
		[Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
		public ThemeSettings[] _themeSettings;

		// Token: 0x0400340A RID: 13322
		[SerializeField]
		[Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
		public LanguageData _language;

		// Token: 0x0400340B RID: 13323
		[SerializeField]
		[Tooltip("A list of prefabs. You should not have to modify this.")]
		public ControlMapper.Prefabs prefabs;

		// Token: 0x0400340C RID: 13324
		[SerializeField]
		[Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
		public ControlMapper.References references;

		// Token: 0x0400340D RID: 13325
		[SerializeField]
		[Tooltip("Show the label for the Players button group?")]
		public bool _showPlayersGroupLabel = true;

		// Token: 0x0400340E RID: 13326
		[SerializeField]
		[Tooltip("Show the label for the Controller button group?")]
		public bool _showControllerGroupLabel = true;

		// Token: 0x0400340F RID: 13327
		[SerializeField]
		[Tooltip("Show the label for the Assigned Controllers button group?")]
		public bool _showAssignedControllersGroupLabel = true;

		// Token: 0x04003410 RID: 13328
		[SerializeField]
		[Tooltip("Show the label for the Settings button group?")]
		public bool _showSettingsGroupLabel = true;

		// Token: 0x04003411 RID: 13329
		[SerializeField]
		[Tooltip("Show the label for the Map Categories button group?")]
		public bool _showMapCategoriesGroupLabel = true;

		// Token: 0x04003412 RID: 13330
		[SerializeField]
		[Tooltip("Show the label for the current controller name?")]
		public bool _showControllerNameLabel = true;

		// Token: 0x04003413 RID: 13331
		[SerializeField]
		[Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
		public bool _showAssignedControllers = true;

		// Token: 0x04003414 RID: 13332
		[SerializeField]
		public Text _rumbleButtonText;

		// Token: 0x04003415 RID: 13333
		public List<GameObject> axisToggleObjects = new List<GameObject>();

		// Token: 0x04003416 RID: 13334
		public List<GameObject> inactiveAxisToggleObjects = new List<GameObject>();

		// Token: 0x04003417 RID: 13335
		public Action _ScreenClosedEvent;

		// Token: 0x04003418 RID: 13336
		public Action _ScreenOpenedEvent;

		// Token: 0x04003419 RID: 13337
		public Action _PopupWindowOpenedEvent;

		// Token: 0x0400341A RID: 13338
		public Action _PopupWindowClosedEvent;

		// Token: 0x0400341B RID: 13339
		public Action _InputPollingStartedEvent;

		// Token: 0x0400341C RID: 13340
		public Action _InputPollingEndedEvent;

		// Token: 0x0400341D RID: 13341
		[SerializeField]
		[Tooltip("Event sent when the UI is closed.")]
		public UnityEvent _onScreenClosed;

		// Token: 0x0400341E RID: 13342
		[SerializeField]
		[Tooltip("Event sent when the UI is opened.")]
		public UnityEvent _onScreenOpened;

		// Token: 0x0400341F RID: 13343
		[SerializeField]
		[Tooltip("Event sent when a popup window is closed.")]
		public UnityEvent _onPopupWindowClosed;

		// Token: 0x04003420 RID: 13344
		[SerializeField]
		[Tooltip("Event sent when a popup window is opened.")]
		public UnityEvent _onPopupWindowOpened;

		// Token: 0x04003421 RID: 13345
		[SerializeField]
		[Tooltip("Event sent when polling for input has started.")]
		public UnityEvent _onInputPollingStarted;

		// Token: 0x04003422 RID: 13346
		[SerializeField]
		[Tooltip("Event sent when polling for input has ended.")]
		public UnityEvent _onInputPollingEnded;

		// Token: 0x04003423 RID: 13347
		public static ControlMapper Instance;

		// Token: 0x04003424 RID: 13348
		public bool initialized;

		// Token: 0x04003425 RID: 13349
		public int playerCount;

		// Token: 0x04003426 RID: 13350
		public ControlMapper.InputGrid inputGrid;

		// Token: 0x04003427 RID: 13351
		public ControlMapper.WindowManager windowManager;

		// Token: 0x04003428 RID: 13352
		public int currentPlayerId;

		// Token: 0x04003429 RID: 13353
		public int currentMapCategoryId;

		// Token: 0x0400342A RID: 13354
		public List<ControlMapper.GUIButton> playerButtons;

		// Token: 0x0400342B RID: 13355
		public List<ControlMapper.GUIButton> mapCategoryButtons;

		// Token: 0x0400342C RID: 13356
		public List<ControlMapper.GUIButton> assignedControllerButtons;

		// Token: 0x0400342D RID: 13357
		public ControlMapper.GUIButton assignedControllerButtonsPlaceholder;

		// Token: 0x0400342E RID: 13358
		public List<GameObject> miscInstantiatedObjects;

		// Token: 0x0400342F RID: 13359
		public GameObject canvas;

		// Token: 0x04003430 RID: 13360
		public GameObject lastUISelection;

		// Token: 0x04003431 RID: 13361
		public int currentJoystickId = -1;

		// Token: 0x04003432 RID: 13362
		public float blockInputOnFocusEndTime;

		// Token: 0x04003433 RID: 13363
		public bool isPollingForInput;

		// Token: 0x04003434 RID: 13364
		public ControlMapper.InputMapping pendingInputMapping;

		// Token: 0x04003435 RID: 13365
		public ControlMapper.AxisCalibrator pendingAxisCalibration;

		// Token: 0x04003436 RID: 13366
		public Action<InputFieldInfo> inputFieldActivatedDelegate;

		// Token: 0x04003437 RID: 13367
		public Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;

		// Token: 0x04003438 RID: 13368
		public Action _restoreDefaultsDelegate;

		// Token: 0x020012B3 RID: 4787
		// (Invoke) Token: 0x060081F7 RID: 33271
		public delegate void PlayerChangeAction();

		// Token: 0x020012B4 RID: 4788
		public abstract class GUIElement
		{
			// Token: 0x060081FA RID: 33274 RVA: 0x0029C6A8 File Offset: 0x0029A8A8
			public GUIElement(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.selectable = gameObject.GetComponent<Selectable>();
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.gameObject = gameObject;
				this.rectTransform = gameObject.GetComponent<RectTransform>();
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.uiElementInfo = gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x060081FB RID: 33275 RVA: 0x0029C730 File Offset: 0x0029A930
			public GUIElement(Selectable selectable, Text label)
			{
				if (selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.selectable = selectable;
				this.gameObject = selectable.gameObject;
				this.rectTransform = this.gameObject.GetComponent<RectTransform>();
				this.text = label;
				this.uiElementInfo = this.gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x17001929 RID: 6441
			// (get) Token: 0x060081FC RID: 33276 RVA: 0x000568A6 File Offset: 0x00054AA6
			// (set) Token: 0x060081FD RID: 33277 RVA: 0x000568AE File Offset: 0x00054AAE
			public RectTransform rectTransform { get; set; }

			// Token: 0x060081FE RID: 33278 RVA: 0x000568B7 File Offset: 0x00054AB7
			public virtual void SetInteractible(bool state, bool playTransition)
			{
				this.SetInteractible(state, playTransition, false);
			}

			// Token: 0x060081FF RID: 33279 RVA: 0x0029C7A4 File Offset: 0x0029A9A4
			public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						this.children[i].SetInteractible(state, playTransition, permanent);
					}
				}
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.selectable == null)
				{
					return;
				}
				if (permanent)
				{
					this.permanentStateSet = true;
				}
				if (this.selectable.interactable == state)
				{
					return;
				}
				UITools.SetInteractable(this.selectable, state, playTransition);
			}

			// Token: 0x06008200 RID: 33280 RVA: 0x0029C844 File Offset: 0x0029AA44
			public virtual void SetTextWidth(int value)
			{
				if (this.text == null)
				{
					return;
				}
				LayoutElement layoutElement = this.text.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = this.text.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.preferredWidth = (float)value;
			}

			// Token: 0x06008201 RID: 33281 RVA: 0x0029C894 File Offset: 0x0029AA94
			public virtual void SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType type, int value)
			{
				if (this.rectTransform.childCount == 0)
				{
					return;
				}
				Transform child = this.rectTransform.GetChild(0);
				LayoutElement layoutElement = child.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = child.gameObject.AddComponent<LayoutElement>();
				}
				if (type == ControlMapper.LayoutElementSizeType.MinSize)
				{
					layoutElement.minWidth = (float)value;
				}
				else
				{
					if (type != ControlMapper.LayoutElementSizeType.PreferredSize)
					{
						throw new NotImplementedException();
					}
					layoutElement.preferredWidth = (float)value;
				}
			}

			// Token: 0x06008202 RID: 33282 RVA: 0x000568C2 File Offset: 0x00054AC2
			public virtual void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06008203 RID: 33283 RVA: 0x000568E2 File Offset: 0x00054AE2
			public virtual string GetLabel()
			{
				if (this.text == null)
				{
					return string.Empty;
				}
				return this.text.text;
			}

			// Token: 0x06008204 RID: 33284 RVA: 0x00056906 File Offset: 0x00054B06
			public virtual void AddChild(ControlMapper.GUIElement child)
			{
				this.children.Add(child);
			}

			// Token: 0x06008205 RID: 33285 RVA: 0x00056914 File Offset: 0x00054B14
			public void SetElementInfoData(string identifier, int intData)
			{
				if (this.uiElementInfo == null)
				{
					return;
				}
				this.uiElementInfo.identifier = identifier;
				this.uiElementInfo.intData = intData;
			}

			// Token: 0x06008206 RID: 33286 RVA: 0x00056940 File Offset: 0x00054B40
			public virtual void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06008207 RID: 33287 RVA: 0x0029C90C File Offset: 0x0029AB0C
			public virtual bool Init()
			{
				bool result = true;
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						if (!this.children[i].Init())
						{
							result = false;
						}
					}
				}
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing Selectable component!");
					result = false;
				}
				if (this.rectTransform == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing RectTransform component!");
					result = false;
				}
				if (this.uiElementInfo == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing UIElementInfo component!");
					result = false;
				}
				return result;
			}

			// Token: 0x040080B4 RID: 32948
			public readonly GameObject gameObject;

			// Token: 0x040080B5 RID: 32949
			public readonly Text text;

			// Token: 0x040080B6 RID: 32950
			public readonly Selectable selectable;

			// Token: 0x040080B7 RID: 32951
			public readonly UIElementInfo uiElementInfo;

			// Token: 0x040080B8 RID: 32952
			public bool permanentStateSet;

			// Token: 0x040080B9 RID: 32953
			public readonly List<ControlMapper.GUIElement> children;
		}

		// Token: 0x020012B5 RID: 4789
		public class GUIButton : ControlMapper.GUIElement
		{
			// Token: 0x06008208 RID: 33288 RVA: 0x00056960 File Offset: 0x00054B60
			public GUIButton(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x06008209 RID: 33289 RVA: 0x00056975 File Offset: 0x00054B75
			public GUIButton(Button button, Text label) : base(button, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x1700192A RID: 6442
			// (get) Token: 0x0600820A RID: 33290 RVA: 0x0005698B File Offset: 0x00054B8B
			public Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x1700192B RID: 6443
			// (get) Token: 0x0600820B RID: 33291 RVA: 0x00056998 File Offset: 0x00054B98
			public ButtonInfo buttonInfo
			{
				get
				{
					return this.uiElementInfo as ButtonInfo;
				}
			}

			// Token: 0x0600820C RID: 33292 RVA: 0x000569A5 File Offset: 0x00054BA5
			public void SetButtonInfoData(string identifier, int intData)
			{
				base.SetElementInfoData(identifier, intData);
			}

			// Token: 0x0600820D RID: 33293 RVA: 0x0029C9C0 File Offset: 0x0029ABC0
			public void SetOnClickCallback(Action<ButtonInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate
				{
					callback(this.buttonInfo);
				});
			}
		}

		// Token: 0x020012B6 RID: 4790
		public class GUIInputField : ControlMapper.GUIElement
		{
			// Token: 0x0600820E RID: 33294 RVA: 0x000569AF File Offset: 0x00054BAF
			public GUIInputField(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x0600820F RID: 33295 RVA: 0x000569C4 File Offset: 0x00054BC4
			public GUIInputField(Button button, Text label) : base(button, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x1700192C RID: 6444
			// (get) Token: 0x06008210 RID: 33296 RVA: 0x000569DA File Offset: 0x00054BDA
			public Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x1700192D RID: 6445
			// (get) Token: 0x06008211 RID: 33297 RVA: 0x000569E7 File Offset: 0x00054BE7
			public InputFieldInfo fieldInfo
			{
				get
				{
					return this.uiElementInfo as InputFieldInfo;
				}
			}

			// Token: 0x1700192E RID: 6446
			// (get) Token: 0x06008212 RID: 33298 RVA: 0x000569F4 File Offset: 0x00054BF4
			public bool hasToggle
			{
				get
				{
					return this.toggle != null;
				}
			}

			// Token: 0x1700192F RID: 6447
			// (get) Token: 0x06008213 RID: 33299 RVA: 0x00056A02 File Offset: 0x00054C02
			// (set) Token: 0x06008214 RID: 33300 RVA: 0x00056A0A File Offset: 0x00054C0A
			public ControlMapper.GUIToggle toggle { get; set; }

			// Token: 0x17001930 RID: 6448
			// (get) Token: 0x06008215 RID: 33301 RVA: 0x00056A13 File Offset: 0x00054C13
			// (set) Token: 0x06008216 RID: 33302 RVA: 0x00056A33 File Offset: 0x00054C33
			public int actionElementMapId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.actionElementMapId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.actionElementMapId = value;
				}
			}

			// Token: 0x17001931 RID: 6449
			// (get) Token: 0x06008217 RID: 33303 RVA: 0x00056A53 File Offset: 0x00054C53
			// (set) Token: 0x06008218 RID: 33304 RVA: 0x00056A73 File Offset: 0x00054C73
			public int controllerId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.controllerId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.controllerId = value;
				}
			}

			// Token: 0x06008219 RID: 33305 RVA: 0x0029CA10 File Offset: 0x0029AC10
			public void SetFieldInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.fieldInfo == null)
				{
					return;
				}
				this.fieldInfo.actionId = actionId;
				this.fieldInfo.axisRange = axisRange;
				this.fieldInfo.controllerType = controllerType;
			}

			// Token: 0x0600821A RID: 33306 RVA: 0x0029CA60 File Offset: 0x0029AC60
			public void SetOnClickCallback(Action<InputFieldInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate
				{
					callback(this.fieldInfo);
				});
			}

			// Token: 0x0600821B RID: 33307 RVA: 0x00056A93 File Offset: 0x00054C93
			public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
			{
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.hasToggle && !state)
				{
					this.toggle.SetInteractible(state, playTransition, permanent);
				}
				base.SetInteractible(state, playTransition, permanent);
			}

			// Token: 0x0600821C RID: 33308 RVA: 0x00056AC9 File Offset: 0x00054CC9
			public void AddToggle(ControlMapper.GUIToggle toggle)
			{
				if (toggle == null)
				{
					return;
				}
				this.toggle = toggle;
			}
		}

		// Token: 0x020012B7 RID: 4791
		public class GUIToggle : ControlMapper.GUIElement
		{
			// Token: 0x0600821D RID: 33309 RVA: 0x00056AD9 File Offset: 0x00054CD9
			public GUIToggle(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x0600821E RID: 33310 RVA: 0x00056AEE File Offset: 0x00054CEE
			public GUIToggle(Toggle toggle, Text label) : base(toggle, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x17001932 RID: 6450
			// (get) Token: 0x0600821F RID: 33311 RVA: 0x00056B04 File Offset: 0x00054D04
			public Toggle toggle
			{
				get
				{
					return this.selectable as Toggle;
				}
			}

			// Token: 0x17001933 RID: 6451
			// (get) Token: 0x06008220 RID: 33312 RVA: 0x00056B11 File Offset: 0x00054D11
			public ToggleInfo toggleInfo
			{
				get
				{
					return this.uiElementInfo as ToggleInfo;
				}
			}

			// Token: 0x17001934 RID: 6452
			// (get) Token: 0x06008221 RID: 33313 RVA: 0x00056B1E File Offset: 0x00054D1E
			// (set) Token: 0x06008222 RID: 33314 RVA: 0x00056B3E File Offset: 0x00054D3E
			public int actionElementMapId
			{
				get
				{
					if (this.toggleInfo == null)
					{
						return -1;
					}
					return this.toggleInfo.actionElementMapId;
				}
				set
				{
					if (this.toggleInfo == null)
					{
						return;
					}
					this.toggleInfo.actionElementMapId = value;
				}
			}

			// Token: 0x06008223 RID: 33315 RVA: 0x0029CAB0 File Offset: 0x0029ACB0
			public void SetToggleInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.toggleInfo == null)
				{
					return;
				}
				this.toggleInfo.actionId = actionId;
				this.toggleInfo.axisRange = axisRange;
				this.toggleInfo.controllerType = controllerType;
			}

			// Token: 0x06008224 RID: 33316 RVA: 0x0029CB00 File Offset: 0x0029AD00
			public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
			{
				if (this.toggle == null)
				{
					return;
				}
				EventTrigger eventTrigger = this.toggle.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.toggle.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
				triggerEvent.AddListener(delegate(BaseEventData data)
				{
					PointerEventData pointerEventData = data as PointerEventData;
					if (pointerEventData != null && pointerEventData.button != null)
					{
						return;
					}
					callback(this.toggleInfo, this.toggle.isOn);
				});
				EventTrigger.Entry item = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = 15
				};
				EventTrigger.Entry item2 = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = 4
				};
				if (eventTrigger.triggers != null)
				{
					eventTrigger.triggers.Clear();
				}
				else
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(item);
				eventTrigger.triggers.Add(item2);
			}

			// Token: 0x06008225 RID: 33317 RVA: 0x00056B5E File Offset: 0x00054D5E
			public void SetToggleState(bool state)
			{
				if (this.toggle == null)
				{
					return;
				}
				this.toggle.isOn = state;
			}
		}

		// Token: 0x020012B8 RID: 4792
		public class GUILabel
		{
			// Token: 0x06008226 RID: 33318 RVA: 0x00056B7E File Offset: 0x00054D7E
			public GUILabel(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.Check();
			}

			// Token: 0x06008227 RID: 33319 RVA: 0x00056BB0 File Offset: 0x00054DB0
			public GUILabel(Text label)
			{
				this.text = label;
				if (!this.Check())
				{
					return;
				}
			}

			// Token: 0x17001935 RID: 6453
			// (get) Token: 0x06008228 RID: 33320 RVA: 0x00056BCB File Offset: 0x00054DCB
			// (set) Token: 0x06008229 RID: 33321 RVA: 0x00056BD3 File Offset: 0x00054DD3
			public GameObject gameObject { get; set; }

			// Token: 0x17001936 RID: 6454
			// (get) Token: 0x0600822A RID: 33322 RVA: 0x00056BDC File Offset: 0x00054DDC
			// (set) Token: 0x0600822B RID: 33323 RVA: 0x00056BE4 File Offset: 0x00054DE4
			public Text text { get; set; }

			// Token: 0x17001937 RID: 6455
			// (get) Token: 0x0600822C RID: 33324 RVA: 0x00056BED File Offset: 0x00054DED
			// (set) Token: 0x0600822D RID: 33325 RVA: 0x00056BF5 File Offset: 0x00054DF5
			public RectTransform rectTransform { get; set; }

			// Token: 0x0600822E RID: 33326 RVA: 0x00056BFE File Offset: 0x00054DFE
			public void SetSize(int width, int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(0, (float)width);
				this.rectTransform.SetSizeWithCurrentAnchors(1, (float)height);
			}

			// Token: 0x0600822F RID: 33327 RVA: 0x00056C2E File Offset: 0x00054E2E
			public void SetWidth(int width)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(0, (float)width);
			}

			// Token: 0x06008230 RID: 33328 RVA: 0x00056C50 File Offset: 0x00054E50
			public void SetHeight(int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(1, (float)height);
			}

			// Token: 0x06008231 RID: 33329 RVA: 0x00056C72 File Offset: 0x00054E72
			public void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06008232 RID: 33330 RVA: 0x00056C92 File Offset: 0x00054E92
			public void SetFontStyle(FontStyle style)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.fontStyle = style;
			}

			// Token: 0x06008233 RID: 33331 RVA: 0x00056CB2 File Offset: 0x00054EB2
			public void SetTextAlignment(TextAnchor alignment)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.alignment = alignment;
			}

			// Token: 0x06008234 RID: 33332 RVA: 0x00056CD2 File Offset: 0x00054ED2
			public void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06008235 RID: 33333 RVA: 0x0029CBEC File Offset: 0x0029ADEC
			public bool Check()
			{
				bool result = true;
				if (this.text == null)
				{
					Debug.LogError("Rewired Control Mapper: Button is missing Text child component!");
					result = false;
				}
				this.gameObject = this.text.gameObject;
				this.rectTransform = this.text.GetComponent<RectTransform>();
				return result;
			}
		}

		// Token: 0x020012B9 RID: 4793
		[Serializable]
		public class MappingSet
		{
			// Token: 0x06008236 RID: 33334 RVA: 0x00056CF2 File Offset: 0x00054EF2
			public MappingSet()
			{
				this._mapCategoryId = -1;
				this._actionCategoryIds = new int[0];
				this._actionIds = new int[0];
				this._actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory;
			}

			// Token: 0x06008237 RID: 33335 RVA: 0x00056D20 File Offset: 0x00054F20
			public MappingSet(int mapCategoryId, ControlMapper.MappingSet.ActionListMode actionListMode, int[] actionCategoryIds, int[] actionIds)
			{
				this._mapCategoryId = mapCategoryId;
				this._actionListMode = actionListMode;
				this._actionCategoryIds = actionCategoryIds;
				this._actionIds = actionIds;
			}

			// Token: 0x17001938 RID: 6456
			// (get) Token: 0x06008238 RID: 33336 RVA: 0x00056D45 File Offset: 0x00054F45
			public int mapCategoryId
			{
				get
				{
					return this._mapCategoryId;
				}
			}

			// Token: 0x17001939 RID: 6457
			// (get) Token: 0x06008239 RID: 33337 RVA: 0x00056D4D File Offset: 0x00054F4D
			public ControlMapper.MappingSet.ActionListMode actionListMode
			{
				get
				{
					return this._actionListMode;
				}
			}

			// Token: 0x1700193A RID: 6458
			// (get) Token: 0x0600823A RID: 33338 RVA: 0x00056D55 File Offset: 0x00054F55
			public IList<int> actionCategoryIds
			{
				get
				{
					if (this._actionCategoryIds == null)
					{
						return null;
					}
					if (this._actionCategoryIdsReadOnly == null)
					{
						this._actionCategoryIdsReadOnly = new ReadOnlyCollection<int>(this._actionCategoryIds);
					}
					return this._actionCategoryIdsReadOnly;
				}
			}

			// Token: 0x1700193B RID: 6459
			// (get) Token: 0x0600823B RID: 33339 RVA: 0x00056D86 File Offset: 0x00054F86
			public IList<int> actionIds
			{
				get
				{
					if (this._actionIds == null)
					{
						return null;
					}
					if (this._actionIdsReadOnly == null)
					{
						this._actionIdsReadOnly = new ReadOnlyCollection<int>(this._actionIds);
					}
					return this._actionIds;
				}
			}

			// Token: 0x1700193C RID: 6460
			// (get) Token: 0x0600823C RID: 33340 RVA: 0x00056DB7 File Offset: 0x00054FB7
			public bool isValid
			{
				get
				{
					return this._mapCategoryId >= 0 && ReInput.mapping.GetMapCategory(this._mapCategoryId) != null;
				}
			}

			// Token: 0x1700193D RID: 6461
			// (get) Token: 0x0600823D RID: 33341 RVA: 0x00056DDD File Offset: 0x00054FDD
			public static ControlMapper.MappingSet Default
			{
				get
				{
					return new ControlMapper.MappingSet(0, ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
				}
			}

			// Token: 0x040080BF RID: 32959
			[SerializeField]
			[Tooltip("The Map Category that will be displayed to the user for remapping.")]
			public int _mapCategoryId;

			// Token: 0x040080C0 RID: 32960
			[SerializeField]
			[Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
			public ControlMapper.MappingSet.ActionListMode _actionListMode;

			// Token: 0x040080C1 RID: 32961
			[SerializeField]
			public int[] _actionCategoryIds;

			// Token: 0x040080C2 RID: 32962
			[SerializeField]
			public int[] _actionIds;

			// Token: 0x040080C3 RID: 32963
			public IList<int> _actionCategoryIdsReadOnly;

			// Token: 0x040080C4 RID: 32964
			public IList<int> _actionIdsReadOnly;

			// Token: 0x020015FC RID: 5628
			public enum ActionListMode
			{
				// Token: 0x04009250 RID: 37456
				ActionCategory,
				// Token: 0x04009251 RID: 37457
				Action
			}
		}

		// Token: 0x020012BA RID: 4794
		[Serializable]
		public class InputBehaviorSettings
		{
			// Token: 0x1700193E RID: 6462
			// (get) Token: 0x0600823F RID: 33343 RVA: 0x00056DF2 File Offset: 0x00054FF2
			public int inputBehaviorId
			{
				get
				{
					return this._inputBehaviorId;
				}
			}

			// Token: 0x1700193F RID: 6463
			// (get) Token: 0x06008240 RID: 33344 RVA: 0x00056DFA File Offset: 0x00054FFA
			public bool showJoystickAxisSensitivity
			{
				get
				{
					return this._showJoystickAxisSensitivity;
				}
			}

			// Token: 0x17001940 RID: 6464
			// (get) Token: 0x06008241 RID: 33345 RVA: 0x00056E02 File Offset: 0x00055002
			public bool showMouseXYAxisSensitivity
			{
				get
				{
					return this._showMouseXYAxisSensitivity;
				}
			}

			// Token: 0x17001941 RID: 6465
			// (get) Token: 0x06008242 RID: 33346 RVA: 0x00056E0A File Offset: 0x0005500A
			public string labelLanguageKey
			{
				get
				{
					return this._labelLanguageKey;
				}
			}

			// Token: 0x17001942 RID: 6466
			// (get) Token: 0x06008243 RID: 33347 RVA: 0x00056E12 File Offset: 0x00055012
			public string joystickAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._joystickAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x17001943 RID: 6467
			// (get) Token: 0x06008244 RID: 33348 RVA: 0x00056E1A File Offset: 0x0005501A
			public string mouseXYAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._mouseXYAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x17001944 RID: 6468
			// (get) Token: 0x06008245 RID: 33349 RVA: 0x00056E22 File Offset: 0x00055022
			public Sprite joystickAxisSensitivityIcon
			{
				get
				{
					return this._joystickAxisSensitivityIcon;
				}
			}

			// Token: 0x17001945 RID: 6469
			// (get) Token: 0x06008246 RID: 33350 RVA: 0x00056E2A File Offset: 0x0005502A
			public Sprite mouseXYAxisSensitivityIcon
			{
				get
				{
					return this._mouseXYAxisSensitivityIcon;
				}
			}

			// Token: 0x17001946 RID: 6470
			// (get) Token: 0x06008247 RID: 33351 RVA: 0x00056E32 File Offset: 0x00055032
			public float joystickAxisSensitivityMin
			{
				get
				{
					return this._joystickAxisSensitivityMin;
				}
			}

			// Token: 0x17001947 RID: 6471
			// (get) Token: 0x06008248 RID: 33352 RVA: 0x00056E3A File Offset: 0x0005503A
			public float joystickAxisSensitivityMax
			{
				get
				{
					return this._joystickAxisSensitivityMax;
				}
			}

			// Token: 0x17001948 RID: 6472
			// (get) Token: 0x06008249 RID: 33353 RVA: 0x00056E42 File Offset: 0x00055042
			public float mouseXYAxisSensitivityMin
			{
				get
				{
					return this._mouseXYAxisSensitivityMin;
				}
			}

			// Token: 0x17001949 RID: 6473
			// (get) Token: 0x0600824A RID: 33354 RVA: 0x00056E4A File Offset: 0x0005504A
			public float mouseXYAxisSensitivityMax
			{
				get
				{
					return this._mouseXYAxisSensitivityMax;
				}
			}

			// Token: 0x1700194A RID: 6474
			// (get) Token: 0x0600824B RID: 33355 RVA: 0x00056E52 File Offset: 0x00055052
			public bool isValid
			{
				get
				{
					return this._inputBehaviorId >= 0 && (this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity);
				}
			}

			// Token: 0x040080C5 RID: 32965
			[SerializeField]
			[Tooltip("The Input Behavior that will be displayed to the user for modification.")]
			public int _inputBehaviorId = -1;

			// Token: 0x040080C6 RID: 32966
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			public bool _showJoystickAxisSensitivity = true;

			// Token: 0x040080C7 RID: 32967
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			public bool _showMouseXYAxisSensitivity = true;

			// Token: 0x040080C8 RID: 32968
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
			public string _labelLanguageKey = string.Empty;

			// Token: 0x040080C9 RID: 32969
			[SerializeField]
			[Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			public string _joystickAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x040080CA RID: 32970
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			public string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x040080CB RID: 32971
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			public Sprite _joystickAxisSensitivityIcon;

			// Token: 0x040080CC RID: 32972
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			public Sprite _mouseXYAxisSensitivityIcon;

			// Token: 0x040080CD RID: 32973
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			public float _joystickAxisSensitivityMin;

			// Token: 0x040080CE RID: 32974
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			public float _joystickAxisSensitivityMax = 2f;

			// Token: 0x040080CF RID: 32975
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			public float _mouseXYAxisSensitivityMin;

			// Token: 0x040080D0 RID: 32976
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			public float _mouseXYAxisSensitivityMax = 2f;
		}

		// Token: 0x020012BB RID: 4795
		[Serializable]
		public class Prefabs
		{
			// Token: 0x1700194B RID: 6475
			// (get) Token: 0x0600824D RID: 33357 RVA: 0x00056E7F File Offset: 0x0005507F
			public GameObject button
			{
				get
				{
					return this._button;
				}
			}

			// Token: 0x1700194C RID: 6476
			// (get) Token: 0x0600824E RID: 33358 RVA: 0x00056E87 File Offset: 0x00055087
			public GameObject playerButton
			{
				get
				{
					return this._playerButton;
				}
			}

			// Token: 0x1700194D RID: 6477
			// (get) Token: 0x0600824F RID: 33359 RVA: 0x00056E8F File Offset: 0x0005508F
			public GameObject fitButton
			{
				get
				{
					return this._fitButton;
				}
			}

			// Token: 0x1700194E RID: 6478
			// (get) Token: 0x06008250 RID: 33360 RVA: 0x00056E97 File Offset: 0x00055097
			public GameObject inputGridLabel
			{
				get
				{
					return this._inputGridLabel;
				}
			}

			// Token: 0x1700194F RID: 6479
			// (get) Token: 0x06008251 RID: 33361 RVA: 0x00056E9F File Offset: 0x0005509F
			public GameObject inputGridDeactivatedLabel
			{
				get
				{
					return this._inputGridDeactivatedLabel;
				}
			}

			// Token: 0x17001950 RID: 6480
			// (get) Token: 0x06008252 RID: 33362 RVA: 0x00056EA7 File Offset: 0x000550A7
			public GameObject inputGridHeaderLabel
			{
				get
				{
					return this._inputGridHeaderLabel;
				}
			}

			// Token: 0x17001951 RID: 6481
			// (get) Token: 0x06008253 RID: 33363 RVA: 0x00056EAF File Offset: 0x000550AF
			public GameObject actionsHeaderLabel
			{
				get
				{
					return this._actionsHeaderLabel;
				}
			}

			// Token: 0x17001952 RID: 6482
			// (get) Token: 0x06008254 RID: 33364 RVA: 0x00056EB7 File Offset: 0x000550B7
			public GameObject inputGridFieldButton
			{
				get
				{
					return this._inputGridFieldButton;
				}
			}

			// Token: 0x17001953 RID: 6483
			// (get) Token: 0x06008255 RID: 33365 RVA: 0x00056EBF File Offset: 0x000550BF
			public GameObject inputGridFieldInvertToggle
			{
				get
				{
					return this._inputGridFieldInvertToggle;
				}
			}

			// Token: 0x17001954 RID: 6484
			// (get) Token: 0x06008256 RID: 33366 RVA: 0x00056EC7 File Offset: 0x000550C7
			public GameObject window
			{
				get
				{
					return this._window;
				}
			}

			// Token: 0x17001955 RID: 6485
			// (get) Token: 0x06008257 RID: 33367 RVA: 0x00056ECF File Offset: 0x000550CF
			public GameObject windowTitleText
			{
				get
				{
					return this._windowTitleText;
				}
			}

			// Token: 0x17001956 RID: 6486
			// (get) Token: 0x06008258 RID: 33368 RVA: 0x00056ED7 File Offset: 0x000550D7
			public GameObject windowContentText
			{
				get
				{
					return this._windowContentText;
				}
			}

			// Token: 0x17001957 RID: 6487
			// (get) Token: 0x06008259 RID: 33369 RVA: 0x00056EDF File Offset: 0x000550DF
			public GameObject fader
			{
				get
				{
					return this._fader;
				}
			}

			// Token: 0x17001958 RID: 6488
			// (get) Token: 0x0600825A RID: 33370 RVA: 0x00056EE7 File Offset: 0x000550E7
			public GameObject calibrationWindow
			{
				get
				{
					return this._calibrationWindow;
				}
			}

			// Token: 0x17001959 RID: 6489
			// (get) Token: 0x0600825B RID: 33371 RVA: 0x00056EEF File Offset: 0x000550EF
			public GameObject inputBehaviorsWindow
			{
				get
				{
					return this._inputBehaviorsWindow;
				}
			}

			// Token: 0x1700195A RID: 6490
			// (get) Token: 0x0600825C RID: 33372 RVA: 0x00056EF7 File Offset: 0x000550F7
			public GameObject centerStickGraphic
			{
				get
				{
					return this._centerStickGraphic;
				}
			}

			// Token: 0x1700195B RID: 6491
			// (get) Token: 0x0600825D RID: 33373 RVA: 0x00056EFF File Offset: 0x000550FF
			public GameObject moveStickGraphic
			{
				get
				{
					return this._moveStickGraphic;
				}
			}

			// Token: 0x0600825E RID: 33374 RVA: 0x0029CC9C File Offset: 0x0029AE9C
			public bool Check()
			{
				return !(this._button == null) && !(this._fitButton == null) && !(this._inputGridLabel == null) && !(this._inputGridHeaderLabel == null) && !(this._inputGridFieldButton == null) && !(this._inputGridFieldInvertToggle == null) && !(this._window == null) && !(this._windowTitleText == null) && !(this._windowContentText == null) && !(this._fader == null) && !(this._calibrationWindow == null) && !(this._inputBehaviorsWindow == null);
			}

			// Token: 0x040080D1 RID: 32977
			[SerializeField]
			public GameObject _button;

			// Token: 0x040080D2 RID: 32978
			[SerializeField]
			public GameObject _playerButton;

			// Token: 0x040080D3 RID: 32979
			[SerializeField]
			public GameObject _fitButton;

			// Token: 0x040080D4 RID: 32980
			[SerializeField]
			public GameObject _inputGridLabel;

			// Token: 0x040080D5 RID: 32981
			[SerializeField]
			public GameObject _inputGridDeactivatedLabel;

			// Token: 0x040080D6 RID: 32982
			[SerializeField]
			public GameObject _inputGridHeaderLabel;

			// Token: 0x040080D7 RID: 32983
			[SerializeField]
			public GameObject _actionsHeaderLabel;

			// Token: 0x040080D8 RID: 32984
			[SerializeField]
			public GameObject _inputGridFieldButton;

			// Token: 0x040080D9 RID: 32985
			[SerializeField]
			public GameObject _inputGridFieldInvertToggle;

			// Token: 0x040080DA RID: 32986
			[SerializeField]
			public GameObject _window;

			// Token: 0x040080DB RID: 32987
			[SerializeField]
			public GameObject _windowTitleText;

			// Token: 0x040080DC RID: 32988
			[SerializeField]
			public GameObject _windowContentText;

			// Token: 0x040080DD RID: 32989
			[SerializeField]
			public GameObject _fader;

			// Token: 0x040080DE RID: 32990
			[SerializeField]
			public GameObject _calibrationWindow;

			// Token: 0x040080DF RID: 32991
			[SerializeField]
			public GameObject _inputBehaviorsWindow;

			// Token: 0x040080E0 RID: 32992
			[SerializeField]
			public GameObject _centerStickGraphic;

			// Token: 0x040080E1 RID: 32993
			[SerializeField]
			public GameObject _moveStickGraphic;
		}

		// Token: 0x020012BC RID: 4796
		[Serializable]
		public class References
		{
			// Token: 0x1700195C RID: 6492
			// (get) Token: 0x06008260 RID: 33376 RVA: 0x00056F0F File Offset: 0x0005510F
			public Canvas canvas
			{
				get
				{
					return this._canvas;
				}
			}

			// Token: 0x1700195D RID: 6493
			// (get) Token: 0x06008261 RID: 33377 RVA: 0x00056F17 File Offset: 0x00055117
			public CanvasGroup mainCanvasGroup
			{
				get
				{
					return this._mainCanvasGroup;
				}
			}

			// Token: 0x1700195E RID: 6494
			// (get) Token: 0x06008262 RID: 33378 RVA: 0x00056F1F File Offset: 0x0005511F
			public Transform mainContent
			{
				get
				{
					return this._mainContent;
				}
			}

			// Token: 0x1700195F RID: 6495
			// (get) Token: 0x06008263 RID: 33379 RVA: 0x00056F27 File Offset: 0x00055127
			public Transform mainContentInner
			{
				get
				{
					return this._mainContentInner;
				}
			}

			// Token: 0x17001960 RID: 6496
			// (get) Token: 0x06008264 RID: 33380 RVA: 0x00056F2F File Offset: 0x0005512F
			public UIGroup playersGroup
			{
				get
				{
					return this._playersGroup;
				}
			}

			// Token: 0x17001961 RID: 6497
			// (get) Token: 0x06008265 RID: 33381 RVA: 0x00056F37 File Offset: 0x00055137
			public Transform controllerGroup
			{
				get
				{
					return this._controllerGroup;
				}
			}

			// Token: 0x17001962 RID: 6498
			// (get) Token: 0x06008266 RID: 33382 RVA: 0x00056F3F File Offset: 0x0005513F
			public Transform controllerGroupLabelGroup
			{
				get
				{
					return this._controllerGroupLabelGroup;
				}
			}

			// Token: 0x17001963 RID: 6499
			// (get) Token: 0x06008267 RID: 33383 RVA: 0x00056F47 File Offset: 0x00055147
			public UIGroup controllerSettingsGroup
			{
				get
				{
					return this._controllerSettingsGroup;
				}
			}

			// Token: 0x17001964 RID: 6500
			// (get) Token: 0x06008268 RID: 33384 RVA: 0x00056F4F File Offset: 0x0005514F
			public UIGroup assignedControllersGroup
			{
				get
				{
					return this._assignedControllersGroup;
				}
			}

			// Token: 0x17001965 RID: 6501
			// (get) Token: 0x06008269 RID: 33385 RVA: 0x00056F57 File Offset: 0x00055157
			public Transform settingsAndMapCategoriesGroup
			{
				get
				{
					return this._settingsAndMapCategoriesGroup;
				}
			}

			// Token: 0x17001966 RID: 6502
			// (get) Token: 0x0600826A RID: 33386 RVA: 0x00056F5F File Offset: 0x0005515F
			public UIGroup settingsGroup
			{
				get
				{
					return this._settingsGroup;
				}
			}

			// Token: 0x17001967 RID: 6503
			// (get) Token: 0x0600826B RID: 33387 RVA: 0x00056F67 File Offset: 0x00055167
			public UIGroup mapCategoriesGroup
			{
				get
				{
					return this._mapCategoriesGroup;
				}
			}

			// Token: 0x17001968 RID: 6504
			// (get) Token: 0x0600826C RID: 33388 RVA: 0x00056F6F File Offset: 0x0005516F
			public Transform inputGridGroup
			{
				get
				{
					return this._inputGridGroup;
				}
			}

			// Token: 0x17001969 RID: 6505
			// (get) Token: 0x0600826D RID: 33389 RVA: 0x00056F77 File Offset: 0x00055177
			public Transform inputGridContainer
			{
				get
				{
					return this._inputGridContainer;
				}
			}

			// Token: 0x1700196A RID: 6506
			// (get) Token: 0x0600826E RID: 33390 RVA: 0x00056F7F File Offset: 0x0005517F
			public Transform inputGridHeadersGroup
			{
				get
				{
					return this._inputGridHeadersGroup;
				}
			}

			// Token: 0x1700196B RID: 6507
			// (get) Token: 0x0600826F RID: 33391 RVA: 0x00056F87 File Offset: 0x00055187
			public Transform inputGridInnerGroup
			{
				get
				{
					return this._inputGridInnerGroup;
				}
			}

			// Token: 0x1700196C RID: 6508
			// (get) Token: 0x06008270 RID: 33392 RVA: 0x00056F8F File Offset: 0x0005518F
			public Transform actionsColumnHeadersGroup
			{
				get
				{
					return this._actionsColumnHeadersGroup;
				}
			}

			// Token: 0x1700196D RID: 6509
			// (get) Token: 0x06008271 RID: 33393 RVA: 0x00056F97 File Offset: 0x00055197
			public Transform actionsColumn
			{
				get
				{
					return this._actionsColumn;
				}
			}

			// Token: 0x1700196E RID: 6510
			// (get) Token: 0x06008272 RID: 33394 RVA: 0x00056F9F File Offset: 0x0005519F
			public Text controllerNameLabel
			{
				get
				{
					return this._controllerNameLabel;
				}
			}

			// Token: 0x1700196F RID: 6511
			// (get) Token: 0x06008273 RID: 33395 RVA: 0x00056FA7 File Offset: 0x000551A7
			public Button removeControllerButton
			{
				get
				{
					return this._removeControllerButton;
				}
			}

			// Token: 0x17001970 RID: 6512
			// (get) Token: 0x06008274 RID: 33396 RVA: 0x00056FAF File Offset: 0x000551AF
			public Button assignControllerButton
			{
				get
				{
					return this._assignControllerButton;
				}
			}

			// Token: 0x17001971 RID: 6513
			// (get) Token: 0x06008275 RID: 33397 RVA: 0x00056FB7 File Offset: 0x000551B7
			public Button calibrateControllerButton
			{
				get
				{
					return this._calibrateControllerButton;
				}
			}

			// Token: 0x17001972 RID: 6514
			// (get) Token: 0x06008276 RID: 33398 RVA: 0x00056FBF File Offset: 0x000551BF
			public Button doneButton
			{
				get
				{
					return this._doneButton;
				}
			}

			// Token: 0x17001973 RID: 6515
			// (get) Token: 0x06008277 RID: 33399 RVA: 0x00056FC7 File Offset: 0x000551C7
			public Button restoreDefaultsButton
			{
				get
				{
					return this._restoreDefaultsButton;
				}
			}

			// Token: 0x17001974 RID: 6516
			// (get) Token: 0x06008278 RID: 33400 RVA: 0x00056FCF File Offset: 0x000551CF
			public Selectable defaultSelection
			{
				get
				{
					return this._defaultSelection;
				}
			}

			// Token: 0x17001975 RID: 6517
			// (get) Token: 0x06008279 RID: 33401 RVA: 0x00056FD7 File Offset: 0x000551D7
			public GameObject[] fixedSelectableUIElements
			{
				get
				{
					return this._fixedSelectableUIElements;
				}
			}

			// Token: 0x17001976 RID: 6518
			// (get) Token: 0x0600827A RID: 33402 RVA: 0x00056FDF File Offset: 0x000551DF
			public Image mainBackgroundImage
			{
				get
				{
					return this._mainBackgroundImage;
				}
			}

			// Token: 0x17001977 RID: 6519
			// (get) Token: 0x0600827B RID: 33403 RVA: 0x00056FE7 File Offset: 0x000551E7
			// (set) Token: 0x0600827C RID: 33404 RVA: 0x00056FEF File Offset: 0x000551EF
			public LayoutElement inputGridLayoutElement { get; set; }

			// Token: 0x17001978 RID: 6520
			// (get) Token: 0x0600827D RID: 33405 RVA: 0x00056FF8 File Offset: 0x000551F8
			// (set) Token: 0x0600827E RID: 33406 RVA: 0x00057000 File Offset: 0x00055200
			public Transform inputGridActionColumn { get; set; }

			// Token: 0x17001979 RID: 6521
			// (get) Token: 0x0600827F RID: 33407 RVA: 0x00057009 File Offset: 0x00055209
			// (set) Token: 0x06008280 RID: 33408 RVA: 0x00057011 File Offset: 0x00055211
			public Transform inputGridKeyboardColumn { get; set; }

			// Token: 0x1700197A RID: 6522
			// (get) Token: 0x06008281 RID: 33409 RVA: 0x0005701A File Offset: 0x0005521A
			// (set) Token: 0x06008282 RID: 33410 RVA: 0x00057022 File Offset: 0x00055222
			public Transform inputGridMouseColumn { get; set; }

			// Token: 0x1700197B RID: 6523
			// (get) Token: 0x06008283 RID: 33411 RVA: 0x0005702B File Offset: 0x0005522B
			// (set) Token: 0x06008284 RID: 33412 RVA: 0x00057033 File Offset: 0x00055233
			public Transform inputGridControllerColumn { get; set; }

			// Token: 0x1700197C RID: 6524
			// (get) Token: 0x06008285 RID: 33413 RVA: 0x0005703C File Offset: 0x0005523C
			// (set) Token: 0x06008286 RID: 33414 RVA: 0x00057044 File Offset: 0x00055244
			public Transform inputGridHeader1 { get; set; }

			// Token: 0x1700197D RID: 6525
			// (get) Token: 0x06008287 RID: 33415 RVA: 0x0005704D File Offset: 0x0005524D
			// (set) Token: 0x06008288 RID: 33416 RVA: 0x00057055 File Offset: 0x00055255
			public Transform inputGridHeader2 { get; set; }

			// Token: 0x1700197E RID: 6526
			// (get) Token: 0x06008289 RID: 33417 RVA: 0x0005705E File Offset: 0x0005525E
			// (set) Token: 0x0600828A RID: 33418 RVA: 0x00057066 File Offset: 0x00055266
			public Transform inputGridHeader3 { get; set; }

			// Token: 0x1700197F RID: 6527
			// (get) Token: 0x0600828B RID: 33419 RVA: 0x0005706F File Offset: 0x0005526F
			// (set) Token: 0x0600828C RID: 33420 RVA: 0x00057077 File Offset: 0x00055277
			public Transform inputGridHeader4 { get; set; }

			// Token: 0x0600828D RID: 33421 RVA: 0x0029CD78 File Offset: 0x0029AF78
			public bool Check()
			{
				return !(this._canvas == null) && !(this._mainCanvasGroup == null) && !(this._mainContent == null) && !(this._mainContentInner == null) && !(this._playersGroup == null) && !(this._controllerGroup == null) && !(this._controllerGroupLabelGroup == null) && !(this._controllerSettingsGroup == null) && !(this._assignedControllersGroup == null) && !(this._settingsAndMapCategoriesGroup == null) && !(this._settingsGroup == null) && !(this._mapCategoriesGroup == null) && !(this._inputGridGroup == null) && !(this._inputGridContainer == null) && !(this._inputGridHeadersGroup == null) && !(this._inputGridInnerGroup == null) && !(this._controllerNameLabel == null) && !(this._removeControllerButton == null) && !(this._assignControllerButton == null) && !(this._calibrateControllerButton == null) && !(this._doneButton == null) && !(this._restoreDefaultsButton == null) && !(this._defaultSelection == null);
			}

			// Token: 0x040080E2 RID: 32994
			[SerializeField]
			public Canvas _canvas;

			// Token: 0x040080E3 RID: 32995
			[SerializeField]
			public CanvasGroup _mainCanvasGroup;

			// Token: 0x040080E4 RID: 32996
			[SerializeField]
			public Transform _mainContent;

			// Token: 0x040080E5 RID: 32997
			[SerializeField]
			public Transform _mainContentInner;

			// Token: 0x040080E6 RID: 32998
			[SerializeField]
			public UIGroup _playersGroup;

			// Token: 0x040080E7 RID: 32999
			[SerializeField]
			public Transform _controllerGroup;

			// Token: 0x040080E8 RID: 33000
			[SerializeField]
			public Transform _controllerGroupLabelGroup;

			// Token: 0x040080E9 RID: 33001
			[SerializeField]
			public UIGroup _controllerSettingsGroup;

			// Token: 0x040080EA RID: 33002
			[SerializeField]
			public UIGroup _assignedControllersGroup;

			// Token: 0x040080EB RID: 33003
			[SerializeField]
			public Transform _settingsAndMapCategoriesGroup;

			// Token: 0x040080EC RID: 33004
			[SerializeField]
			public UIGroup _settingsGroup;

			// Token: 0x040080ED RID: 33005
			[SerializeField]
			public UIGroup _mapCategoriesGroup;

			// Token: 0x040080EE RID: 33006
			[SerializeField]
			public Transform _inputGridGroup;

			// Token: 0x040080EF RID: 33007
			[SerializeField]
			public Transform _inputGridContainer;

			// Token: 0x040080F0 RID: 33008
			[SerializeField]
			public Transform _inputGridHeadersGroup;

			// Token: 0x040080F1 RID: 33009
			[SerializeField]
			public Transform _inputGridInnerGroup;

			// Token: 0x040080F2 RID: 33010
			[SerializeField]
			public Transform _actionsColumnHeadersGroup;

			// Token: 0x040080F3 RID: 33011
			[SerializeField]
			public Transform _actionsColumn;

			// Token: 0x040080F4 RID: 33012
			[SerializeField]
			public Text _controllerNameLabel;

			// Token: 0x040080F5 RID: 33013
			[SerializeField]
			public Button _removeControllerButton;

			// Token: 0x040080F6 RID: 33014
			[SerializeField]
			public Button _assignControllerButton;

			// Token: 0x040080F7 RID: 33015
			[SerializeField]
			public Button _calibrateControllerButton;

			// Token: 0x040080F8 RID: 33016
			[SerializeField]
			public Button _doneButton;

			// Token: 0x040080F9 RID: 33017
			[SerializeField]
			public Button _restoreDefaultsButton;

			// Token: 0x040080FA RID: 33018
			[SerializeField]
			public Selectable _defaultSelection;

			// Token: 0x040080FB RID: 33019
			[SerializeField]
			public GameObject[] _fixedSelectableUIElements;

			// Token: 0x040080FC RID: 33020
			[SerializeField]
			public Image _mainBackgroundImage;
		}

		// Token: 0x020012BD RID: 4797
		public class InputActionSet
		{
			// Token: 0x0600828E RID: 33422 RVA: 0x00057080 File Offset: 0x00055280
			public InputActionSet(int actionId, AxisRange axisRange)
			{
				this._actionId = actionId;
				this._axisRange = axisRange;
			}

			// Token: 0x17001980 RID: 6528
			// (get) Token: 0x0600828F RID: 33423 RVA: 0x00057096 File Offset: 0x00055296
			public int actionId
			{
				get
				{
					return this._actionId;
				}
			}

			// Token: 0x17001981 RID: 6529
			// (get) Token: 0x06008290 RID: 33424 RVA: 0x0005709E File Offset: 0x0005529E
			public AxisRange axisRange
			{
				get
				{
					return this._axisRange;
				}
			}

			// Token: 0x04008106 RID: 33030
			public int _actionId;

			// Token: 0x04008107 RID: 33031
			public AxisRange _axisRange;
		}

		// Token: 0x020012BE RID: 4798
		public class InputMapping
		{
			// Token: 0x06008291 RID: 33425 RVA: 0x000570A6 File Offset: 0x000552A6
			public InputMapping(string actionName, InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, ControllerType controllerType, int controllerId)
			{
				this.actionName = actionName;
				this.fieldInfo = fieldInfo;
				this.map = map;
				this.aem = aem;
				this.controllerType = controllerType;
				this.controllerId = controllerId;
			}

			// Token: 0x17001982 RID: 6530
			// (get) Token: 0x06008292 RID: 33426 RVA: 0x000570DB File Offset: 0x000552DB
			// (set) Token: 0x06008293 RID: 33427 RVA: 0x000570E3 File Offset: 0x000552E3
			public string actionName { get; set; }

			// Token: 0x17001983 RID: 6531
			// (get) Token: 0x06008294 RID: 33428 RVA: 0x000570EC File Offset: 0x000552EC
			// (set) Token: 0x06008295 RID: 33429 RVA: 0x000570F4 File Offset: 0x000552F4
			public InputFieldInfo fieldInfo { get; set; }

			// Token: 0x17001984 RID: 6532
			// (get) Token: 0x06008296 RID: 33430 RVA: 0x000570FD File Offset: 0x000552FD
			// (set) Token: 0x06008297 RID: 33431 RVA: 0x00057105 File Offset: 0x00055305
			public ControllerMap map { get; set; }

			// Token: 0x17001985 RID: 6533
			// (get) Token: 0x06008298 RID: 33432 RVA: 0x0005710E File Offset: 0x0005530E
			// (set) Token: 0x06008299 RID: 33433 RVA: 0x00057116 File Offset: 0x00055316
			public ActionElementMap aem { get; set; }

			// Token: 0x17001986 RID: 6534
			// (get) Token: 0x0600829A RID: 33434 RVA: 0x0005711F File Offset: 0x0005531F
			// (set) Token: 0x0600829B RID: 33435 RVA: 0x00057127 File Offset: 0x00055327
			public ControllerType controllerType { get; set; }

			// Token: 0x17001987 RID: 6535
			// (get) Token: 0x0600829C RID: 33436 RVA: 0x00057130 File Offset: 0x00055330
			// (set) Token: 0x0600829D RID: 33437 RVA: 0x00057138 File Offset: 0x00055338
			public int controllerId { get; set; }

			// Token: 0x17001988 RID: 6536
			// (get) Token: 0x0600829E RID: 33438 RVA: 0x00057141 File Offset: 0x00055341
			// (set) Token: 0x0600829F RID: 33439 RVA: 0x00057149 File Offset: 0x00055349
			public ControllerPollingInfo pollingInfo { get; set; }

			// Token: 0x17001989 RID: 6537
			// (get) Token: 0x060082A0 RID: 33440 RVA: 0x00057152 File Offset: 0x00055352
			// (set) Token: 0x060082A1 RID: 33441 RVA: 0x0005715A File Offset: 0x0005535A
			public ModifierKeyFlags modifierKeyFlags { get; set; }

			// Token: 0x1700198A RID: 6538
			// (get) Token: 0x060082A2 RID: 33442 RVA: 0x0029CF10 File Offset: 0x0029B110
			public AxisRange axisRange
			{
				get
				{
					AxisRange result = 1;
					if (this.pollingInfo.elementType == null)
					{
						if (this.fieldInfo.axisRange == null)
						{
							result = 0;
						}
						else
						{
							result = ((this.pollingInfo.axisPole != null) ? 2 : 1);
						}
					}
					return result;
				}
			}

			// Token: 0x1700198B RID: 6539
			// (get) Token: 0x060082A3 RID: 33443 RVA: 0x0029CF68 File Offset: 0x0029B168
			public string elementName
			{
				get
				{
					if (this.controllerType == null && this.modifierKeyFlags != null)
					{
						return string.Format("{0} + {1}", Keyboard.ModifierKeyFlagsToString(this.modifierKeyFlags), this.pollingInfo.elementIdentifierName);
					}
					string text = this.pollingInfo.elementIdentifierName;
					if (this.pollingInfo.elementType == null)
					{
						if (this.axisRange == 1)
						{
							text = this.pollingInfo.elementIdentifier.positiveName;
						}
						else if (this.axisRange == 2)
						{
							text = this.pollingInfo.elementIdentifier.negativeName;
						}
					}
					TranslationElement translationElement = Localization.Find(text);
					if (translationElement != null)
					{
						return translationElement.translations[(int)Localization.language].text;
					}
					return text;
				}
			}

			// Token: 0x060082A4 RID: 33444 RVA: 0x00057163 File Offset: 0x00055363
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo)
			{
				this.pollingInfo = pollingInfo;
				return this.ToElementAssignment();
			}

			// Token: 0x060082A5 RID: 33445 RVA: 0x00057172 File Offset: 0x00055372
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo, ModifierKeyFlags modifierKeyFlags)
			{
				this.pollingInfo = pollingInfo;
				this.modifierKeyFlags = modifierKeyFlags;
				return this.ToElementAssignment();
			}

			// Token: 0x060082A6 RID: 33446 RVA: 0x0029D040 File Offset: 0x0029B240
			public ElementAssignment ToElementAssignment()
			{
				return new ElementAssignment(this.controllerType, this.pollingInfo.elementType, this.pollingInfo.elementIdentifierId, this.axisRange, this.pollingInfo.keyboardKey, this.modifierKeyFlags, this.fieldInfo.actionId, (this.fieldInfo.axisRange != 2) ? 0 : 1, false, (this.aem == null) ? -1 : this.aem.id);
			}
		}

		// Token: 0x020012BF RID: 4799
		public class AxisCalibrator
		{
			// Token: 0x060082A7 RID: 33447 RVA: 0x0029D0D0 File Offset: 0x0029B2D0
			public AxisCalibrator(Joystick joystick, int axisIndex)
			{
				this.data = default(AxisCalibrationData);
				this.joystick = joystick;
				this.axisIndex = axisIndex;
				if (joystick != null && axisIndex >= 0 && joystick.axisCount > axisIndex)
				{
					this.axis = joystick.Axes[axisIndex];
					this.data = joystick.calibrationMap.GetAxis(axisIndex).GetData();
				}
				this.firstRun = true;
			}

			// Token: 0x1700198C RID: 6540
			// (get) Token: 0x060082A8 RID: 33448 RVA: 0x00057188 File Offset: 0x00055388
			public bool isValid
			{
				get
				{
					return this.axis != null;
				}
			}

			// Token: 0x060082A9 RID: 33449 RVA: 0x0029D14C File Offset: 0x0029B34C
			public void RecordMinMax()
			{
				if (this.axis == null)
				{
					return;
				}
				float valueRaw = this.axis.valueRaw;
				if (this.firstRun || valueRaw < this.data.min)
				{
					this.data.min = valueRaw;
				}
				if (this.firstRun || valueRaw > this.data.max)
				{
					this.data.max = valueRaw;
				}
				this.firstRun = false;
			}

			// Token: 0x060082AA RID: 33450 RVA: 0x00057196 File Offset: 0x00055396
			public void RecordZero()
			{
				if (this.axis == null)
				{
					return;
				}
				this.data.zero = this.axis.valueRaw;
			}

			// Token: 0x060082AB RID: 33451 RVA: 0x0029D1C8 File Offset: 0x0029B3C8
			public void Commit()
			{
				if (this.axis == null)
				{
					return;
				}
				AxisCalibration axisCalibration = this.joystick.calibrationMap.GetAxis(this.axisIndex);
				if (axisCalibration == null)
				{
					return;
				}
				if ((double)Mathf.Abs(this.data.max - this.data.min) < 0.1)
				{
					return;
				}
				axisCalibration.SetData(this.data);
			}

			// Token: 0x04008110 RID: 33040
			public AxisCalibrationData data;

			// Token: 0x04008111 RID: 33041
			public readonly Joystick joystick;

			// Token: 0x04008112 RID: 33042
			public readonly int axisIndex;

			// Token: 0x04008113 RID: 33043
			public Controller.Axis axis;

			// Token: 0x04008114 RID: 33044
			public bool firstRun;
		}

		// Token: 0x020012C0 RID: 4800
		public class IndexedDictionary<TKey, TValue>
		{
			// Token: 0x060082AC RID: 33452 RVA: 0x000571BA File Offset: 0x000553BA
			public IndexedDictionary()
			{
				this.list = new List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
			}

			// Token: 0x1700198D RID: 6541
			// (get) Token: 0x060082AD RID: 33453 RVA: 0x000571CD File Offset: 0x000553CD
			public int Count
			{
				get
				{
					return this.list.Count;
				}
			}

			// Token: 0x1700198E RID: 6542
			public TValue this[int index]
			{
				get
				{
					return this.list[index].value;
				}
			}

			// Token: 0x060082AF RID: 33455 RVA: 0x0029D238 File Offset: 0x0029B438
			public TValue Get(TKey key)
			{
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					throw new Exception("Key does not exist!");
				}
				return this.list[num].value;
			}

			// Token: 0x060082B0 RID: 33456 RVA: 0x0029D270 File Offset: 0x0029B470
			public bool TryGet(TKey key, out TValue value)
			{
				value = default(TValue);
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					return false;
				}
				value = this.list[num].value;
				return true;
			}

			// Token: 0x060082B1 RID: 33457 RVA: 0x0029D2B8 File Offset: 0x0029B4B8
			public void Add(TKey key, TValue value)
			{
				if (this.ContainsKey(key))
				{
					throw new Exception("Key " + key.ToString() + " is already in use!");
				}
				this.list.Add(new ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
			}

			// Token: 0x060082B2 RID: 33458 RVA: 0x0029D308 File Offset: 0x0029B508
			public int IndexOfKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060082B3 RID: 33459 RVA: 0x0029D358 File Offset: 0x0029B558
			public bool ContainsKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060082B4 RID: 33460 RVA: 0x000571ED File Offset: 0x000553ED
			public void Clear()
			{
				this.list.Clear();
			}

			// Token: 0x04008115 RID: 33045
			public List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

			// Token: 0x020015FD RID: 5629
			public class Entry
			{
				// Token: 0x0600878E RID: 34702 RVA: 0x0005BD1D File Offset: 0x00059F1D
				public Entry(TKey key, TValue value)
				{
					this.key = key;
					this.value = value;
				}

				// Token: 0x04009252 RID: 37458
				public TKey key;

				// Token: 0x04009253 RID: 37459
				public TValue value;
			}
		}

		// Token: 0x020012C1 RID: 4801
		public enum LayoutElementSizeType
		{
			// Token: 0x04008117 RID: 33047
			MinSize,
			// Token: 0x04008118 RID: 33048
			PreferredSize
		}

		// Token: 0x020012C2 RID: 4802
		public enum WindowType
		{
			// Token: 0x0400811A RID: 33050
			None,
			// Token: 0x0400811B RID: 33051
			ChooseJoystick,
			// Token: 0x0400811C RID: 33052
			JoystickAssignmentConflict,
			// Token: 0x0400811D RID: 33053
			ElementAssignment,
			// Token: 0x0400811E RID: 33054
			ElementAssignmentPrePolling,
			// Token: 0x0400811F RID: 33055
			ElementAssignmentPolling,
			// Token: 0x04008120 RID: 33056
			ElementAssignmentResult,
			// Token: 0x04008121 RID: 33057
			ElementAssignmentConflict,
			// Token: 0x04008122 RID: 33058
			Calibration,
			// Token: 0x04008123 RID: 33059
			CalibrateStep1,
			// Token: 0x04008124 RID: 33060
			CalibrateStep2
		}

		// Token: 0x020012C3 RID: 4803
		public class InputGrid
		{
			// Token: 0x060082B5 RID: 33461 RVA: 0x000571FA File Offset: 0x000553FA
			public InputGrid()
			{
				this.list = new ControlMapper.InputGridEntryList();
				this.groups = new List<GameObject>();
			}

			// Token: 0x060082B6 RID: 33462 RVA: 0x00057218 File Offset: 0x00055418
			public void AddMapCategory(int mapCategoryId)
			{
				this.list.AddMapCategory(mapCategoryId);
			}

			// Token: 0x060082B7 RID: 33463 RVA: 0x00057226 File Offset: 0x00055426
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.list.AddAction(mapCategoryId, action, axisRange);
			}

			// Token: 0x060082B8 RID: 33464 RVA: 0x00057236 File Offset: 0x00055436
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.list.AddActionCategory(mapCategoryId, actionCategoryId);
			}

			// Token: 0x060082B9 RID: 33465 RVA: 0x00057245 File Offset: 0x00055445
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
			}

			// Token: 0x060082BA RID: 33466 RVA: 0x00057259 File Offset: 0x00055459
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
			}

			// Token: 0x060082BB RID: 33467 RVA: 0x0005726F File Offset: 0x0005546F
			public void AddGroup(GameObject group)
			{
				this.groups.Add(group);
			}

			// Token: 0x060082BC RID: 33468 RVA: 0x0005727D File Offset: 0x0005547D
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
			}

			// Token: 0x060082BD RID: 33469 RVA: 0x0005728F File Offset: 0x0005548F
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
			}

			// Token: 0x060082BE RID: 33470 RVA: 0x0005729F File Offset: 0x0005549F
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x060082BF RID: 33471 RVA: 0x000572B3 File Offset: 0x000554B3
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x060082C0 RID: 33472 RVA: 0x000572C7 File Offset: 0x000554C7
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				return this.list.GetActionSets(mapCategoryId);
			}

			// Token: 0x060082C1 RID: 33473 RVA: 0x000572D5 File Offset: 0x000554D5
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				this.list.SetColumnHeight(mapCategoryId, height);
			}

			// Token: 0x060082C2 RID: 33474 RVA: 0x000572E4 File Offset: 0x000554E4
			public float GetColumnHeight(int mapCategoryId)
			{
				return this.list.GetColumnHeight(mapCategoryId);
			}

			// Token: 0x060082C3 RID: 33475 RVA: 0x000572F2 File Offset: 0x000554F2
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				this.list.SetFieldsActive(mapCategoryId, state);
			}

			// Token: 0x060082C4 RID: 33476 RVA: 0x00057301 File Offset: 0x00055501
			public void SetFieldLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
			}

			// Token: 0x060082C5 RID: 33477 RVA: 0x0029D3A8 File Offset: 0x0029B5A8
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x060082C6 RID: 33478 RVA: 0x00057317 File Offset: 0x00055517
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
			}

			// Token: 0x060082C7 RID: 33479 RVA: 0x0005732B File Offset: 0x0005552B
			public void InitializeFields(int mapCategoryId)
			{
				this.list.InitializeFields(mapCategoryId);
			}

			// Token: 0x060082C8 RID: 33480 RVA: 0x00057339 File Offset: 0x00055539
			public void Show(int mapCategoryId)
			{
				this.list.Show(mapCategoryId);
			}

			// Token: 0x060082C9 RID: 33481 RVA: 0x00057347 File Offset: 0x00055547
			public void HideAll()
			{
				this.list.HideAll();
			}

			// Token: 0x060082CA RID: 33482 RVA: 0x00057354 File Offset: 0x00055554
			public void ClearLabels(int mapCategoryId)
			{
				this.list.ClearLabels(mapCategoryId);
			}

			// Token: 0x060082CB RID: 33483 RVA: 0x0029D3D0 File Offset: 0x0029B5D0
			public void ClearGroups()
			{
				for (int i = 0; i < this.groups.Count; i++)
				{
					if (!(this.groups[i] == null))
					{
						Object.Destroy(this.groups[i]);
					}
				}
			}

			// Token: 0x060082CC RID: 33484 RVA: 0x00057362 File Offset: 0x00055562
			public void ClearAll()
			{
				this.ClearGroups();
				this.list.Clear();
			}

			// Token: 0x04008125 RID: 33061
			public ControlMapper.InputGridEntryList list;

			// Token: 0x04008126 RID: 33062
			public List<GameObject> groups;
		}

		// Token: 0x020012C4 RID: 4804
		public class InputGridEntryList
		{
			// Token: 0x060082CD RID: 33485 RVA: 0x00057375 File Offset: 0x00055575
			public InputGridEntryList()
			{
				this.entries = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry>();
			}

			// Token: 0x060082CE RID: 33486 RVA: 0x00057388 File Offset: 0x00055588
			public void AddMapCategory(int mapCategoryId)
			{
				if (mapCategoryId < 0)
				{
					return;
				}
				if (this.entries.ContainsKey(mapCategoryId))
				{
					return;
				}
				this.entries.Add(mapCategoryId, new ControlMapper.InputGridEntryList.MapCategoryEntry());
			}

			// Token: 0x060082CF RID: 33487 RVA: 0x000573B5 File Offset: 0x000555B5
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.AddActionEntry(mapCategoryId, action, axisRange);
			}

			// Token: 0x060082D0 RID: 33488 RVA: 0x0029D428 File Offset: 0x0029B628
			public ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddAction(action, axisRange);
			}

			// Token: 0x060082D1 RID: 33489 RVA: 0x0029D45C File Offset: 0x0029B65C
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = mapCategoryEntry.GetActionEntry(actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetLabel(label);
			}

			// Token: 0x060082D2 RID: 33490 RVA: 0x000573C1 File Offset: 0x000555C1
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
			}

			// Token: 0x060082D3 RID: 33491 RVA: 0x0029D498 File Offset: 0x0029B698
			public ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(int mapCategoryId, int actionCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddActionCategory(actionCategoryId);
			}

			// Token: 0x060082D4 RID: 33492 RVA: 0x0029D4C4 File Offset: 0x0029B6C4
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionCategoryEntry actionCategoryEntry = mapCategoryEntry.GetActionCategoryEntry(actionCategoryId);
				if (actionCategoryEntry == null)
				{
					return;
				}
				actionCategoryEntry.SetLabel(label);
			}

			// Token: 0x060082D5 RID: 33493 RVA: 0x0029D4FC File Offset: 0x0029B6FC
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputFieldSet(controllerType, fieldSetContainer);
			}

			// Token: 0x060082D6 RID: 33494 RVA: 0x0029D524 File Offset: 0x0029B724
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputField(controllerType, fieldIndex, inputField);
			}

			// Token: 0x060082D7 RID: 33495 RVA: 0x000573CC File Offset: 0x000555CC
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
			}

			// Token: 0x060082D8 RID: 33496 RVA: 0x0029D550 File Offset: 0x0029B750
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
			}

			// Token: 0x060082D9 RID: 33497 RVA: 0x0029D57C File Offset: 0x0029B77C
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.columnHeight = height;
			}

			// Token: 0x060082DA RID: 33498 RVA: 0x0029D5A4 File Offset: 0x0029B7A4
			public float GetColumnHeight(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return 0f;
				}
				return mapCategoryEntry.columnHeight;
			}

			// Token: 0x060082DB RID: 33499 RVA: 0x0029D5D0 File Offset: 0x0029B7D0
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return null;
				}
				return actionEntry.GetGUIInputField(controllerType, fieldIndex);
			}

			// Token: 0x060082DC RID: 33500 RVA: 0x0029D5FC File Offset: 0x0029B7FC
			public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				if (actionId < 0)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.GetActionEntry(actionId, axisRange);
			}

			// Token: 0x060082DD RID: 33501 RVA: 0x000573DD File Offset: 0x000555DD
			public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				return this.GetActionEntry(mapCategoryId, action.id, axisRange);
			}

			// Token: 0x060082DE RID: 33502 RVA: 0x0029D634 File Offset: 0x0029B834
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry entry;
				if (!this.entries.TryGet(mapCategoryId, out entry))
				{
					yield break;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> list = entry.actionList;
				int count = (list == null) ? 0 : list.Count;
				for (int i = 0; i < count; i++)
				{
					yield return list[i].actionSet;
				}
				yield break;
			}

			// Token: 0x060082DF RID: 33503 RVA: 0x0029D660 File Offset: 0x0029B860
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].SetFieldsActive(state);
				}
			}

			// Token: 0x060082E0 RID: 33504 RVA: 0x0029D6BC File Offset: 0x0029B8BC
			public void SetLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFieldLabel(controllerType, index, label);
			}

			// Token: 0x060082E1 RID: 33505 RVA: 0x0029D6E8 File Offset: 0x0029B8E8
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x060082E2 RID: 33506 RVA: 0x0029D718 File Offset: 0x0029B918
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFixedFieldData(controllerType, controllerId);
			}

			// Token: 0x060082E3 RID: 33507 RVA: 0x0029D740 File Offset: 0x0029B940
			public void InitializeFields(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].Initialize();
				}
			}

			// Token: 0x060082E4 RID: 33508 RVA: 0x0029D79C File Offset: 0x0029B99C
			public void Show(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.SetAllActive(true);
			}

			// Token: 0x060082E5 RID: 33509 RVA: 0x0029D7C4 File Offset: 0x0029B9C4
			public void HideAll()
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].SetAllActive(false);
				}
			}

			// Token: 0x060082E6 RID: 33510 RVA: 0x0029D800 File Offset: 0x0029BA00
			public void ClearLabels(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].ClearLabels();
				}
			}

			// Token: 0x060082E7 RID: 33511 RVA: 0x000573F5 File Offset: 0x000555F5
			public void Clear()
			{
				this.entries.Clear();
			}

			// Token: 0x04008127 RID: 33063
			public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

			// Token: 0x020015FE RID: 5630
			public class MapCategoryEntry
			{
				// Token: 0x0600878F RID: 34703 RVA: 0x0005BD33 File Offset: 0x00059F33
				public MapCategoryEntry()
				{
					this._actionList = new List<ControlMapper.InputGridEntryList.ActionEntry>();
					this._actionCategoryList = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry>();
				}

				// Token: 0x17001A40 RID: 6720
				// (get) Token: 0x06008790 RID: 34704 RVA: 0x0005BD51 File Offset: 0x00059F51
				public List<ControlMapper.InputGridEntryList.ActionEntry> actionList
				{
					get
					{
						return this._actionList;
					}
				}

				// Token: 0x17001A41 RID: 6721
				// (get) Token: 0x06008791 RID: 34705 RVA: 0x0005BD59 File Offset: 0x00059F59
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
				{
					get
					{
						return this._actionCategoryList;
					}
				}

				// Token: 0x17001A42 RID: 6722
				// (get) Token: 0x06008792 RID: 34706 RVA: 0x0005BD61 File Offset: 0x00059F61
				// (set) Token: 0x06008793 RID: 34707 RVA: 0x0005BD69 File Offset: 0x00059F69
				public float columnHeight
				{
					get
					{
						return this._columnHeight;
					}
					set
					{
						this._columnHeight = value;
					}
				}

				// Token: 0x06008794 RID: 34708 RVA: 0x002A5568 File Offset: 0x002A3768
				public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int actionId, AxisRange axisRange)
				{
					int num = this.IndexOfActionEntry(actionId, axisRange);
					if (num < 0)
					{
						return null;
					}
					return this._actionList[num];
				}

				// Token: 0x06008795 RID: 34709 RVA: 0x002A5594 File Offset: 0x002A3794
				public int IndexOfActionEntry(int actionId, AxisRange axisRange)
				{
					int count = this._actionList.Count;
					for (int i = 0; i < count; i++)
					{
						if (this._actionList[i].Matches(actionId, axisRange))
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x06008796 RID: 34710 RVA: 0x0005BD72 File Offset: 0x00059F72
				public bool ContainsActionEntry(int actionId, AxisRange axisRange)
				{
					return this.IndexOfActionEntry(actionId, axisRange) >= 0;
				}

				// Token: 0x06008797 RID: 34711 RVA: 0x002A55DC File Offset: 0x002A37DC
				public ControlMapper.InputGridEntryList.ActionEntry AddAction(InputAction action, AxisRange axisRange)
				{
					if (action == null)
					{
						return null;
					}
					if (this.ContainsActionEntry(action.id, axisRange))
					{
						return null;
					}
					this._actionList.Add(new ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
					return this._actionList[this._actionList.Count - 1];
				}

				// Token: 0x06008798 RID: 34712 RVA: 0x0005BD82 File Offset: 0x00059F82
				public ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(int actionCategoryId)
				{
					if (!this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x06008799 RID: 34713 RVA: 0x0005BDA3 File Offset: 0x00059FA3
				public ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(int actionCategoryId)
				{
					if (actionCategoryId < 0)
					{
						return null;
					}
					if (this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					this._actionCategoryList.Add(actionCategoryId, new ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x0600879A RID: 34714 RVA: 0x002A5630 File Offset: 0x002A3830
				public void SetAllActive(bool state)
				{
					for (int i = 0; i < this._actionCategoryList.Count; i++)
					{
						this._actionCategoryList[i].SetActive(state);
					}
					for (int j = 0; j < this._actionList.Count; j++)
					{
						this._actionList[j].SetActive(state);
					}
				}

				// Token: 0x04009254 RID: 37460
				public List<ControlMapper.InputGridEntryList.ActionEntry> _actionList;

				// Token: 0x04009255 RID: 37461
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;

				// Token: 0x04009256 RID: 37462
				public float _columnHeight;
			}

			// Token: 0x020015FF RID: 5631
			public class ActionEntry
			{
				// Token: 0x0600879B RID: 34715 RVA: 0x0005BDDF File Offset: 0x00059FDF
				public ActionEntry(InputAction action, AxisRange axisRange)
				{
					this.action = action;
					this.axisRange = axisRange;
					this.actionSet = new ControlMapper.InputActionSet(action.id, axisRange);
					this.fieldSets = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet>();
				}

				// Token: 0x0600879C RID: 34716 RVA: 0x0005BE12 File Offset: 0x0005A012
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x0600879D RID: 34717 RVA: 0x0005BE1B File Offset: 0x0005A01B
				public bool Matches(int actionId, AxisRange axisRange)
				{
					return this.action.id == actionId && this.axisRange == axisRange;
				}

				// Token: 0x0600879E RID: 34718 RVA: 0x0005BE3F File Offset: 0x0005A03F
				public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
				{
					if (this.fieldSets.ContainsKey(controllerType))
					{
						return;
					}
					this.fieldSets.Add(controllerType, new ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
				}

				// Token: 0x0600879F RID: 34719 RVA: 0x002A569C File Offset: 0x002A389C
				public void AddInputField(ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
				{
					if (!this.fieldSets.ContainsKey(controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get(controllerType);
					if (fieldSet.fields.ContainsKey(fieldIndex))
					{
						return;
					}
					fieldSet.fields.Add(fieldIndex, inputField);
				}

				// Token: 0x060087A0 RID: 34720 RVA: 0x002A56E8 File Offset: 0x002A38E8
				public ControlMapper.GUIInputField GetGUIInputField(ControllerType controllerType, int fieldIndex)
				{
					if (!this.fieldSets.ContainsKey(controllerType))
					{
						return null;
					}
					if (!this.fieldSets.Get(controllerType).fields.ContainsKey(fieldIndex))
					{
						return null;
					}
					return this.fieldSets.Get(controllerType).fields.Get(fieldIndex);
				}

				// Token: 0x060087A1 RID: 34721 RVA: 0x0005BE65 File Offset: 0x0005A065
				public bool Contains(ControllerType controllerType, int fieldId)
				{
					return this.fieldSets.ContainsKey(controllerType) && this.fieldSets.Get(controllerType).fields.ContainsKey(fieldId);
				}

				// Token: 0x060087A2 RID: 34722 RVA: 0x002A5740 File Offset: 0x002A3940
				public void SetFieldLabel(ControllerType controllerType, int index, string label)
				{
					if (!this.fieldSets.ContainsKey(controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get(controllerType).fields.ContainsKey(index))
					{
						return;
					}
					this.fieldSets.Get(controllerType).fields.Get(index).SetLabel(label);
				}

				// Token: 0x060087A3 RID: 34723 RVA: 0x002A579C File Offset: 0x002A399C
				public void PopulateField(ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
				{
					if (!this.fieldSets.ContainsKey(controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get(controllerType).fields.ContainsKey(index))
					{
						return;
					}
					ControlMapper.GUIInputField guiinputField = this.fieldSets.Get(controllerType).fields.Get(index);
					guiinputField.SetLabel(label);
					guiinputField.actionElementMapId = actionElementMapId;
					guiinputField.controllerId = controllerId;
					if (guiinputField.hasToggle)
					{
						guiinputField.toggle.SetInteractible(true, false);
						guiinputField.toggle.SetToggleState(invert);
						guiinputField.toggle.actionElementMapId = actionElementMapId;
					}
				}

				// Token: 0x060087A4 RID: 34724 RVA: 0x002A583C File Offset: 0x002A3A3C
				public void SetFixedFieldData(ControllerType controllerType, int controllerId)
				{
					if (!this.fieldSets.ContainsKey(controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get(controllerType);
					int count = fieldSet.fields.Count;
					for (int i = 0; i < count; i++)
					{
						fieldSet.fields[i].controllerId = controllerId;
					}
				}

				// Token: 0x060087A5 RID: 34725 RVA: 0x002A5898 File Offset: 0x002A3A98
				public void Initialize()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(false, false);
								guiinputField.toggle.SetToggleState(false);
								guiinputField.toggle.actionElementMapId = -1;
							}
							guiinputField.SetLabel(string.Empty);
							guiinputField.actionElementMapId = -1;
							guiinputField.controllerId = -1;
						}
					}
				}

				// Token: 0x060087A6 RID: 34726 RVA: 0x002A594C File Offset: 0x002A3B4C
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
					int count = this.fieldSets.Count;
					for (int i = 0; i < count; i++)
					{
						this.fieldSets[i].groupContainer.SetActive(state);
					}
				}

				// Token: 0x060087A7 RID: 34727 RVA: 0x002A59A8 File Offset: 0x002A3BA8
				public void ClearLabels()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetLabel(string.Empty);
						}
					}
				}

				// Token: 0x060087A8 RID: 34728 RVA: 0x002A5A18 File Offset: 0x002A3C18
				public void SetFieldsActive(bool state)
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetInteractible(state, false);
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(state, false);
							}
						}
					}
				}

				// Token: 0x04009257 RID: 37463
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet> fieldSets;

				// Token: 0x04009258 RID: 37464
				public ControlMapper.GUILabel label;

				// Token: 0x04009259 RID: 37465
				public readonly InputAction action;

				// Token: 0x0400925A RID: 37466
				public readonly AxisRange axisRange;

				// Token: 0x0400925B RID: 37467
				public readonly ControlMapper.InputActionSet actionSet;
			}

			// Token: 0x02001600 RID: 5632
			public class FieldSet
			{
				// Token: 0x060087A9 RID: 34729 RVA: 0x0005BE99 File Offset: 0x0005A099
				public FieldSet(GameObject groupContainer)
				{
					this.groupContainer = groupContainer;
					this.fields = new ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField>();
				}

				// Token: 0x0400925C RID: 37468
				public readonly GameObject groupContainer;

				// Token: 0x0400925D RID: 37469
				public readonly ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField> fields;
			}

			// Token: 0x02001601 RID: 5633
			public class ActionCategoryEntry
			{
				// Token: 0x060087AA RID: 34730 RVA: 0x0005BEB3 File Offset: 0x0005A0B3
				public ActionCategoryEntry(int actionCategoryId)
				{
					this.actionCategoryId = actionCategoryId;
				}

				// Token: 0x060087AB RID: 34731 RVA: 0x0005BEC2 File Offset: 0x0005A0C2
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x060087AC RID: 34732 RVA: 0x0005BECB File Offset: 0x0005A0CB
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
				}

				// Token: 0x0400925E RID: 37470
				public readonly int actionCategoryId;

				// Token: 0x0400925F RID: 37471
				public ControlMapper.GUILabel label;
			}
		}

		// Token: 0x020012C5 RID: 4805
		public class WindowManager
		{
			// Token: 0x060082E8 RID: 33512 RVA: 0x0029D85C File Offset: 0x0029BA5C
			public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
			{
				this.windowPrefab = windowPrefab;
				this.parent = parent;
				this.windows = new List<Window>();
				this.fader = Object.Instantiate<GameObject>(faderPrefab);
				this.fader.transform.SetParent(parent, false);
				this.fader.GetComponent<RectTransform>().localScale = Vector2.one;
				this.SetFaderActive(false);
			}

			// Token: 0x1700198F RID: 6543
			// (get) Token: 0x060082E9 RID: 33513 RVA: 0x0029D8C8 File Offset: 0x0029BAC8
			public bool isWindowOpen
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x17001990 RID: 6544
			// (get) Token: 0x060082EA RID: 33514 RVA: 0x0029D914 File Offset: 0x0029BB14
			public Window topWindow
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return this.windows[i];
						}
					}
					return null;
				}
			}

			// Token: 0x060082EB RID: 33515 RVA: 0x0029D96C File Offset: 0x0029BB6C
			public Window OpenWindow(string name, int width, int height)
			{
				Window result = this.InstantiateWindow(name, width, height);
				this.UpdateFader();
				return result;
			}

			// Token: 0x060082EC RID: 33516 RVA: 0x0029D98C File Offset: 0x0029BB8C
			public Window OpenWindow(GameObject windowPrefab, string name)
			{
				if (windowPrefab == null)
				{
					Debug.LogError("Rewired Control Mapper: Window Prefab is null!");
					return null;
				}
				Window result = this.InstantiateWindow(name, windowPrefab);
				this.UpdateFader();
				return result;
			}

			// Token: 0x060082ED RID: 33517 RVA: 0x0029D9C4 File Offset: 0x0029BBC4
			public void CloseTop()
			{
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
					this.windows.RemoveAt(i);
				}
				this.UpdateFader();
			}

			// Token: 0x060082EE RID: 33518 RVA: 0x00057402 File Offset: 0x00055602
			public void CloseWindow(int windowId)
			{
				this.CloseWindow(this.GetWindow(windowId));
			}

			// Token: 0x060082EF RID: 33519 RVA: 0x0029DA40 File Offset: 0x0029BC40
			public void CloseWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else if (!(this.windows[i] != window))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
				}
				this.UpdateFader();
				this.FocusTopWindow();
			}

			// Token: 0x060082F0 RID: 33520 RVA: 0x0029DAEC File Offset: 0x0029BCEC
			public void CloseAll()
			{
				this.SetFaderActive(false);
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
					}
				}
				this.UpdateFader();
			}

			// Token: 0x060082F1 RID: 33521 RVA: 0x0029DB6C File Offset: 0x0029BD6C
			public void CancelAll()
			{
				if (!this.isWindowOpen)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.windows[i].Cancel();
					}
				}
				this.CloseAll();
			}

			// Token: 0x060082F2 RID: 33522 RVA: 0x0029DBD8 File Offset: 0x0029BDD8
			public Window GetWindow(int windowId)
			{
				if (windowId < 0)
				{
					return null;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						if (this.windows[i].id == windowId)
						{
							return this.windows[i];
						}
					}
				}
				return null;
			}

			// Token: 0x060082F3 RID: 33523 RVA: 0x00057411 File Offset: 0x00055611
			public bool IsFocused(int windowId)
			{
				return windowId >= 0 && !(this.topWindow == null) && this.topWindow.id == windowId;
			}

			// Token: 0x060082F4 RID: 33524 RVA: 0x0005743D File Offset: 0x0005563D
			public void Focus(int windowId)
			{
				this.Focus(this.GetWindow(windowId));
			}

			// Token: 0x060082F5 RID: 33525 RVA: 0x0005744C File Offset: 0x0005564C
			public void Focus(Window window)
			{
				if (window == null)
				{
					return;
				}
				window.TakeInputFocus();
				this.DefocusOtherWindows(window.id);
			}

			// Token: 0x060082F6 RID: 33526 RVA: 0x0029DC54 File Offset: 0x0029BE54
			public void DefocusOtherWindows(int focusedWindowId)
			{
				if (focusedWindowId < 0)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						if (this.windows[i].id != focusedWindowId)
						{
							this.windows[i].Disable();
						}
					}
				}
			}

			// Token: 0x060082F7 RID: 33527 RVA: 0x0029DCD0 File Offset: 0x0029BED0
			public void UpdateFader()
			{
				if (!this.isWindowOpen)
				{
					this.SetFaderActive(false);
					return;
				}
				Transform transform = this.topWindow.transform.parent;
				if (transform == null)
				{
					return;
				}
				this.SetFaderActive(true);
				this.fader.transform.SetAsLastSibling();
				int siblingIndex = this.topWindow.transform.GetSiblingIndex();
				this.fader.transform.SetSiblingIndex(siblingIndex);
			}

			// Token: 0x060082F8 RID: 33528 RVA: 0x0005746D File Offset: 0x0005566D
			public void FocusTopWindow()
			{
				if (this.topWindow == null)
				{
					return;
				}
				this.topWindow.TakeInputFocus();
			}

			// Token: 0x060082F9 RID: 33529 RVA: 0x0005748C File Offset: 0x0005568C
			public void SetFaderActive(bool state)
			{
				this.fader.SetActive(state);
			}

			// Token: 0x060082FA RID: 33530 RVA: 0x0029DD48 File Offset: 0x0029BF48
			public Window InstantiateWindow(string name, int width, int height)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
					component.SetSize(width, height);
				}
				return component;
			}

			// Token: 0x060082FB RID: 33531 RVA: 0x0029DDC8 File Offset: 0x0029BFC8
			public Window InstantiateWindow(string name, GameObject windowPrefab)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				if (windowPrefab == null)
				{
					return null;
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
				}
				return component;
			}

			// Token: 0x060082FC RID: 33532 RVA: 0x0005749A File Offset: 0x0005569A
			public void DestroyWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				Object.Destroy(window.gameObject);
			}

			// Token: 0x060082FD RID: 33533 RVA: 0x0029DE4C File Offset: 0x0029C04C
			public int GetNewId()
			{
				int result = this.idCounter;
				this.idCounter++;
				return result;
			}

			// Token: 0x060082FE RID: 33534 RVA: 0x000574B4 File Offset: 0x000556B4
			public void ClearCompletely()
			{
				this.CloseAll();
				if (this.fader != null)
				{
					Object.Destroy(this.fader);
				}
			}

			// Token: 0x04008128 RID: 33064
			public List<Window> windows;

			// Token: 0x04008129 RID: 33065
			public GameObject windowPrefab;

			// Token: 0x0400812A RID: 33066
			public Transform parent;

			// Token: 0x0400812B RID: 33067
			public GameObject fader;

			// Token: 0x0400812C RID: 33068
			public int idCounter;
		}
	}
}
