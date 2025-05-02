using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000644 RID: 1604
	public class LanguageData : ScriptableObject
	{
		// Token: 0x06004305 RID: 17157 RVA: 0x000359B6 File Offset: 0x00033BB6
		public void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
			this._initialized = true;
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x001380B4 File Offset: 0x001362B4
		public string GetCustomEntry(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			string result;
			if (!this.customDict.TryGetValue(key, out result))
			{
				return string.Empty;
			}
			return result;
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x000359DC File Offset: 0x00033BDC
		public bool ContainsCustomEntryKey(string key)
		{
			return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06004308 RID: 17160 RVA: 0x001380EC File Offset: 0x001362EC
		public string yes
		{
			get
			{
				return Localization.Translate(this._yes).text;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x0013810C File Offset: 0x0013630C
		public string no
		{
			get
			{
				return Localization.Translate(this._no).text;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x0013812C File Offset: 0x0013632C
		public string add
		{
			get
			{
				return Localization.Translate(this._add).text;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600430B RID: 17163 RVA: 0x0013814C File Offset: 0x0013634C
		public string replace
		{
			get
			{
				return Localization.Translate(this._replace).text;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600430C RID: 17164 RVA: 0x0013816C File Offset: 0x0013636C
		public string remove
		{
			get
			{
				return Localization.Translate(this._remove).text;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600430D RID: 17165 RVA: 0x0013818C File Offset: 0x0013638C
		public string cancel
		{
			get
			{
				return Localization.Translate(this._cancel).text;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600430E RID: 17166 RVA: 0x001381AC File Offset: 0x001363AC
		public string none
		{
			get
			{
				return Localization.Translate(this._none).text;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600430F RID: 17167 RVA: 0x001381CC File Offset: 0x001363CC
		public string okay
		{
			get
			{
				return Localization.Translate(this._okay).text;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06004310 RID: 17168 RVA: 0x001381EC File Offset: 0x001363EC
		public string done
		{
			get
			{
				return Localization.Translate(this._done).text;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06004311 RID: 17169 RVA: 0x0013820C File Offset: 0x0013640C
		public string default_
		{
			get
			{
				return Localization.Translate(this._default).text;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06004312 RID: 17170 RVA: 0x0013822C File Offset: 0x0013642C
		public string assignControllerWindowTitle
		{
			get
			{
				return Localization.Translate(this._assignControllerWindowTitle).text;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06004313 RID: 17171 RVA: 0x0013824C File Offset: 0x0013644C
		public string assignControllerWindowMessage
		{
			get
			{
				return Localization.Translate(this._assignControllerWindowMessage).text;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06004314 RID: 17172 RVA: 0x0013826C File Offset: 0x0013646C
		public string controllerAssignmentConflictWindowTitle
		{
			get
			{
				return Localization.Translate(this._controllerAssignmentConflictWindowTitle).text;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06004315 RID: 17173 RVA: 0x0013828C File Offset: 0x0013648C
		public string elementAssignmentPrePollingWindowMessage
		{
			get
			{
				return Localization.Translate(this._elementAssignmentPrePollingWindowMessage).text;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x001382AC File Offset: 0x001364AC
		public string elementAssignmentConflictWindowMessage
		{
			get
			{
				return Localization.Translate(this._elementAssignmentConflictWindowMessage).text;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06004317 RID: 17175 RVA: 0x001382CC File Offset: 0x001364CC
		public string mouseAssignmentConflictWindowTitle
		{
			get
			{
				return Localization.Translate(this._mouseAssignmentConflictWindowTitle).text;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06004318 RID: 17176 RVA: 0x001382EC File Offset: 0x001364EC
		public string calibrateControllerWindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateControllerWindowTitle).text;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06004319 RID: 17177 RVA: 0x0013830C File Offset: 0x0013650C
		public string calibrateAxisStep1WindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateAxisStep1WindowTitle).text;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600431A RID: 17178 RVA: 0x0013832C File Offset: 0x0013652C
		public string calibrateAxisStep2WindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateAxisStep2WindowTitle).text;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600431B RID: 17179 RVA: 0x0013834C File Offset: 0x0013654C
		public string inputBehaviorSettingsWindowTitle
		{
			get
			{
				return Localization.Translate(this._inputBehaviorSettingsWindowTitle).text;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600431C RID: 17180 RVA: 0x0013836C File Offset: 0x0013656C
		public string restoreDefaultsWindowTitle
		{
			get
			{
				return Localization.Translate(this._restoreDefaultsWindowTitle).text;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600431D RID: 17181 RVA: 0x0013838C File Offset: 0x0013658C
		public string actionColumnLabel
		{
			get
			{
				return Localization.Translate(this._actionColumnLabel).text;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600431E RID: 17182 RVA: 0x001383AC File Offset: 0x001365AC
		public string keyboardColumnLabel
		{
			get
			{
				return Localization.Translate(this._keyboardColumnLabel).text;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x001383CC File Offset: 0x001365CC
		public string mouseColumnLabel
		{
			get
			{
				return Localization.Translate(this._mouseColumnLabel).text;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06004320 RID: 17184 RVA: 0x001383EC File Offset: 0x001365EC
		public string controllerColumnLabel
		{
			get
			{
				return Localization.Translate(this._controllerColumnLabel).text;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06004321 RID: 17185 RVA: 0x0013840C File Offset: 0x0013660C
		public string removeControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._removeControllerButtonLabel).text;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x0013842C File Offset: 0x0013662C
		public string calibrateControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._calibrateControllerButtonLabel).text;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06004323 RID: 17187 RVA: 0x0013844C File Offset: 0x0013664C
		public string assignControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._assignControllerButtonLabel).text;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06004324 RID: 17188 RVA: 0x0013846C File Offset: 0x0013666C
		public string inputBehaviorSettingsButtonLabel
		{
			get
			{
				return Localization.Translate(this._inputBehaviorSettingsButtonLabel).text;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06004325 RID: 17189 RVA: 0x0013848C File Offset: 0x0013668C
		public string doneButtonLabel
		{
			get
			{
				return Localization.Translate(this._doneButtonLabel).text;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06004326 RID: 17190 RVA: 0x001384AC File Offset: 0x001366AC
		public string restoreDefaultsButtonLabel
		{
			get
			{
				return Localization.Translate(this._restoreDefaultsButtonLabel).text;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06004327 RID: 17191 RVA: 0x001384CC File Offset: 0x001366CC
		public string controllerSettingsGroupLabel
		{
			get
			{
				return Localization.Translate(this._controllerSettingsGroupLabel).text;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06004328 RID: 17192 RVA: 0x001384EC File Offset: 0x001366EC
		public string playersGroupLabel
		{
			get
			{
				return Localization.Translate(this._playersGroupLabel).text;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x0013850C File Offset: 0x0013670C
		public string assignedControllersGroupLabel
		{
			get
			{
				return Localization.Translate(this._assignedControllersGroupLabel).text;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x0013852C File Offset: 0x0013672C
		public string settingsGroupLabel
		{
			get
			{
				return Localization.Translate(this._settingsGroupLabel).text;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600432B RID: 17195 RVA: 0x0013854C File Offset: 0x0013674C
		public string mapCategoriesGroupLabel
		{
			get
			{
				return Localization.Translate(this._mapCategoriesGroupLabel).text;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x0013856C File Offset: 0x0013676C
		public string restoreDefaultsWindowMessage
		{
			get
			{
				if (ReInput.players.playerCount > 1)
				{
					return Localization.Translate(this._restoreDefaultsWindowMessage_multiPlayer).text;
				}
				return Localization.Translate(this._restoreDefaultsWindowMessage_onePlayer).text;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x001385B0 File Offset: 0x001367B0
		public string calibrateWindow_deadZoneSliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_deadZoneSliderLabel).text;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x001385D0 File Offset: 0x001367D0
		public string calibrateWindow_zeroSliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_zeroSliderLabel).text;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x0600432F RID: 17199 RVA: 0x001385F0 File Offset: 0x001367F0
		public string calibrateWindow_sensitivitySliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_sensitivitySliderLabel).text;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x00138610 File Offset: 0x00136810
		public string calibrateWindow_invertToggleLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_invertToggleLabel).text;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06004331 RID: 17201 RVA: 0x00138630 File Offset: 0x00136830
		public string calibrateWindow_calibrateButtonLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_calibrateButtonLabel).text;
			}
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00138650 File Offset: 0x00136850
		public string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName)
		{
			return string.Format(Localization.Translate(this._controllerAssignmentConflictWindowMessage).text, joystickName, otherPlayerName, currentPlayerName);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00138678 File Offset: 0x00136878
		public string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._joystickElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x000359F7 File Offset: 0x00033BF7
		public string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x001386A0 File Offset: 0x001368A0
		public string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._keyboardElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x001386C8 File Offset: 0x001368C8
		public string GetMouseElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._mouseElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00035A05 File Offset: 0x00033C05
		public string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x001386F0 File Offset: 0x001368F0
		public string GetElementAlreadyInUseBlocked(string elementName)
		{
			return string.Format(Localization.Translate(this._elementAlreadyInUseBlocked).text, elementName);
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00138718 File Offset: 0x00136918
		public string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return string.Format(Localization.Translate(this._elementAlreadyInUseCanReplace).text, elementName);
			}
			return string.Format(Localization.Translate(this._elementAlreadyInUseCanReplace_conflictAllowed).text, elementName);
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x00138760 File Offset: 0x00136960
		public int GetElementAlreadyInUseCanReplaceFontSize(bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return Localization.Translate(this._elementAlreadyInUseCanReplace).fonts.fontSize;
			}
			return Localization.Translate(this._elementAlreadyInUseCanReplace_conflictAllowed).fonts.fontSize;
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x001387A4 File Offset: 0x001369A4
		public string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName)
		{
			return string.Format(Localization.Translate(this._mouseAssignmentConflictWindowMessage).text, otherPlayerName, thisPlayerName);
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x001387CC File Offset: 0x001369CC
		public string GetCalibrateAxisStep1WindowMessage(string axisName)
		{
			return string.Format(Localization.Translate(this._calibrateAxisStep1WindowMessage).text, axisName);
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001387F4 File Offset: 0x001369F4
		public string GetCalibrateAxisStep2WindowMessage(string axisName)
		{
			return string.Format(Localization.Translate(this._calibrateAxisStep2WindowMessage).text, axisName);
		}

		// Token: 0x0400347C RID: 13436
		[SerializeField]
		public string _yes = "Yes";

		// Token: 0x0400347D RID: 13437
		[SerializeField]
		public string _no = "No";

		// Token: 0x0400347E RID: 13438
		[SerializeField]
		public string _add = "Add";

		// Token: 0x0400347F RID: 13439
		[SerializeField]
		public string _replace = "Replace";

		// Token: 0x04003480 RID: 13440
		[SerializeField]
		public string _remove = "Remove";

		// Token: 0x04003481 RID: 13441
		[SerializeField]
		public string _cancel = "Cancel";

		// Token: 0x04003482 RID: 13442
		[SerializeField]
		public string _none = "None";

		// Token: 0x04003483 RID: 13443
		[SerializeField]
		public string _okay = "Okay";

		// Token: 0x04003484 RID: 13444
		[SerializeField]
		public string _done = "Done";

		// Token: 0x04003485 RID: 13445
		[SerializeField]
		public string _default = "Default";

		// Token: 0x04003486 RID: 13446
		[SerializeField]
		public string _assignControllerWindowTitle = "Choose Controller";

		// Token: 0x04003487 RID: 13447
		[SerializeField]
		public string _assignControllerWindowMessage = "Press any button or move an axis on the controller you would like to use.";

		// Token: 0x04003488 RID: 13448
		[SerializeField]
		public string _controllerAssignmentConflictWindowTitle = "Controller Assignment";

		// Token: 0x04003489 RID: 13449
		[SerializeField]
		[Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
		public string _controllerAssignmentConflictWindowMessage = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?";

		// Token: 0x0400348A RID: 13450
		[SerializeField]
		public string _elementAssignmentPrePollingWindowMessage = "First center or zero all sticks and axes and press any button or wait for the timer to finish.";

		// Token: 0x0400348B RID: 13451
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		public string _joystickElementAssignmentPollingWindowMessage = "Now press a button or move an axis to assign it to {0}.";

		// Token: 0x0400348C RID: 13452
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		public string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Now move an axis to assign it to {0}.";

		// Token: 0x0400348D RID: 13453
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		public string _keyboardElementAssignmentPollingWindowMessage = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second.";

		// Token: 0x0400348E RID: 13454
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		public string _mouseElementAssignmentPollingWindowMessage = "Press a mouse button or move an axis to assign it to {0}.";

		// Token: 0x0400348F RID: 13455
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		public string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Move an axis to assign it to {0}.";

		// Token: 0x04003490 RID: 13456
		[SerializeField]
		public string _elementAssignmentConflictWindowMessage = "Assignment Conflict";

		// Token: 0x04003491 RID: 13457
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		public string _elementAlreadyInUseBlocked = "{0} is already in use cannot be replaced.";

		// Token: 0x04003492 RID: 13458
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		public string _elementAlreadyInUseCanReplace = "{0} is already in use. Do you want to replace it?";

		// Token: 0x04003493 RID: 13459
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		public string _elementAlreadyInUseCanReplace_conflictAllowed = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway.";

		// Token: 0x04003494 RID: 13460
		[SerializeField]
		public string _mouseAssignmentConflictWindowTitle = "Mouse Assignment";

		// Token: 0x04003495 RID: 13461
		[SerializeField]
		[Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
		public string _mouseAssignmentConflictWindowMessage = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?";

		// Token: 0x04003496 RID: 13462
		[SerializeField]
		public string _calibrateControllerWindowTitle = "Calibrate Controller";

		// Token: 0x04003497 RID: 13463
		[SerializeField]
		public string _calibrateAxisStep1WindowTitle = "Calibrate Zero";

		// Token: 0x04003498 RID: 13464
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		public string _calibrateAxisStep1WindowMessage = "Center or zero {0} and press any button or wait for the timer to finish.";

		// Token: 0x04003499 RID: 13465
		[SerializeField]
		public string _calibrateAxisStep2WindowTitle = "Calibrate Range";

		// Token: 0x0400349A RID: 13466
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		public string _calibrateAxisStep2WindowMessage = "Move {0} through its entire range then press any button or wait for the timer to finish.";

		// Token: 0x0400349B RID: 13467
		[SerializeField]
		public string _inputBehaviorSettingsWindowTitle = "Sensitivity Settings";

		// Token: 0x0400349C RID: 13468
		[SerializeField]
		public string _restoreDefaultsWindowTitle = "Restore Defaults";

		// Token: 0x0400349D RID: 13469
		[SerializeField]
		[Tooltip("Message for a single player game.")]
		public string _restoreDefaultsWindowMessage_onePlayer = "This will restore the default input configuration. Are you sure you want to do this?";

		// Token: 0x0400349E RID: 13470
		[SerializeField]
		[Tooltip("Message for a multi-player game.")]
		public string _restoreDefaultsWindowMessage_multiPlayer = "This will restore the default input configuration for all players. Are you sure you want to do this?";

		// Token: 0x0400349F RID: 13471
		[SerializeField]
		public string _actionColumnLabel = "Actions";

		// Token: 0x040034A0 RID: 13472
		[SerializeField]
		public string _keyboardColumnLabel = "Keyboard";

		// Token: 0x040034A1 RID: 13473
		[SerializeField]
		public string _mouseColumnLabel = "Mouse";

		// Token: 0x040034A2 RID: 13474
		[SerializeField]
		public string _controllerColumnLabel = "Controller";

		// Token: 0x040034A3 RID: 13475
		[SerializeField]
		public string _removeControllerButtonLabel = "Remove";

		// Token: 0x040034A4 RID: 13476
		[SerializeField]
		public string _calibrateControllerButtonLabel = "Calibrate";

		// Token: 0x040034A5 RID: 13477
		[SerializeField]
		public string _assignControllerButtonLabel = "Assign Controller";

		// Token: 0x040034A6 RID: 13478
		[SerializeField]
		public string _inputBehaviorSettingsButtonLabel = "Sensitivity";

		// Token: 0x040034A7 RID: 13479
		[SerializeField]
		public string _doneButtonLabel = "Done";

		// Token: 0x040034A8 RID: 13480
		[SerializeField]
		public string _restoreDefaultsButtonLabel = "Restore Defaults";

		// Token: 0x040034A9 RID: 13481
		[SerializeField]
		public string _playersGroupLabel = "Players:";

		// Token: 0x040034AA RID: 13482
		[SerializeField]
		public string _controllerSettingsGroupLabel = "Controller:";

		// Token: 0x040034AB RID: 13483
		[SerializeField]
		public string _assignedControllersGroupLabel = "Assigned Controllers:";

		// Token: 0x040034AC RID: 13484
		[SerializeField]
		public string _settingsGroupLabel = "Settings:";

		// Token: 0x040034AD RID: 13485
		[SerializeField]
		public string _mapCategoriesGroupLabel = "Categories:";

		// Token: 0x040034AE RID: 13486
		[SerializeField]
		public string _calibrateWindow_deadZoneSliderLabel = "Dead Zone:";

		// Token: 0x040034AF RID: 13487
		[SerializeField]
		public string _calibrateWindow_zeroSliderLabel = "Zero:";

		// Token: 0x040034B0 RID: 13488
		[SerializeField]
		public string _calibrateWindow_sensitivitySliderLabel = "Sensitivity:";

		// Token: 0x040034B1 RID: 13489
		[SerializeField]
		public string _calibrateWindow_invertToggleLabel = "Invert";

		// Token: 0x040034B2 RID: 13490
		[SerializeField]
		public string _calibrateWindow_calibrateButtonLabel = "Calibrate";

		// Token: 0x040034B3 RID: 13491
		[SerializeField]
		public LanguageData.CustomEntry[] _customEntries;

		// Token: 0x040034B4 RID: 13492
		public bool _initialized;

		// Token: 0x040034B5 RID: 13493
		public Dictionary<string, string> customDict;

		// Token: 0x020012D5 RID: 4821
		[Serializable]
		public class CustomEntry
		{
			// Token: 0x06008340 RID: 33600 RVA: 0x00057825 File Offset: 0x00055A25
			public CustomEntry()
			{
			}

			// Token: 0x06008341 RID: 33601 RVA: 0x0005782D File Offset: 0x00055A2D
			public CustomEntry(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x06008342 RID: 33602 RVA: 0x0029E800 File Offset: 0x0029CA00
			public static Dictionary<string, string> ToDictionary(LanguageData.CustomEntry[] array)
			{
				if (array == null)
				{
					return new Dictionary<string, string>();
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						if (!string.IsNullOrEmpty(array[i].key) && !string.IsNullOrEmpty(array[i].value))
						{
							if (dictionary.ContainsKey(array[i].key))
							{
								Debug.LogError("Key \"" + array[i].key + "\" is already in dictionary!");
							}
							else
							{
								dictionary.Add(array[i].key, array[i].value);
							}
						}
					}
				}
				return dictionary;
			}

			// Token: 0x04008171 RID: 33137
			public string key;

			// Token: 0x04008172 RID: 33138
			public string value;
		}
	}
}
