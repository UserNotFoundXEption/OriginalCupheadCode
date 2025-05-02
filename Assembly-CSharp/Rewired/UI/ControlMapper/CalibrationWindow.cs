using System;
using System.Collections.Generic;
using Rewired.Integration.UnityUI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000635 RID: 1589
	[AddComponentMenu("")]
	public class CalibrationWindow : Window
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x000341E9 File Offset: 0x000323E9
		public bool axisSelected
		{
			get
			{
				return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < this.joystick.calibrationMap.axisCount;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600410D RID: 16653 RVA: 0x00034222 File Offset: 0x00032422
		public AxisCalibration axisCalibration
		{
			get
			{
				if (!this.axisSelected)
				{
					return null;
				}
				return this.joystick.calibrationMap.GetAxis(this.selectedAxis);
			}
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0012FAA8 File Offset: 0x0012DCA8
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.rightContentContainer == null || this.valueDisplayGroup == null || this.calibratedValueMarker == null || this.rawValueMarker == null || this.calibratedZeroMarker == null || this.deadzoneArea == null || this.deadzoneSlider == null || this.sensitivitySlider == null || this.zeroSlider == null || this.invertToggle == null || this.axisScrollAreaContent == null || this.doneButton == null || this.calibrateButton == null || this.axisButtonPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null || this.deadzoneSliderLabel == null || this.zeroSliderLabel == null || this.sensitivitySliderLabel == null || this.invertToggleLabel == null || this.calibrateButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.axisButtons = new List<Button>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			this.deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
			this.zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
			this.sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
			this.invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
			this.calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0012FCFC File Offset: 0x0012DEFC
		public void SetJoystick(int playerId, Joystick joystick)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			this.joystick = joystick;
			if (joystick == null)
			{
				Debug.LogError("Rewired Control Mapper: Joystick cannot be null!");
				return;
			}
			float num = 0f;
			for (int i = 0; i < joystick.axisCount; i++)
			{
				int index = i;
				GameObject gameObject = UITools.InstantiateGUIObject<Button>(this.axisButtonPrefab, this.axisScrollAreaContent, "Axis" + i);
				Button button = gameObject.GetComponent<Button>();
				button.onClick.AddListener(delegate
				{
					this.OnAxisSelected(index, button);
				});
				Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				if (componentInSelfOrChildren != null)
				{
					componentInSelfOrChildren.text = joystick.AxisElementIdentifiers[i].name;
				}
				if (num == 0f)
				{
					num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
				}
				this.axisButtons.Add(button);
			}
			float spacing = this.axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
			this.axisScrollAreaContent.sizeDelta = new Vector2(this.axisScrollAreaContent.sizeDelta.x, Mathf.Max((float)joystick.axisCount * (num + spacing) - spacing, this.axisScrollAreaContent.sizeDelta.y));
			this.origCalibrationData = joystick.calibrationMap.ToXmlString();
			this.displayAreaWidth = this.rightContentContainer.sizeDelta.x;
			this.rewiredStandaloneInputModule = base.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
			if (this.rewiredStandaloneInputModule != null)
			{
				this.menuHorizActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
				this.menuVertActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
			}
			if (joystick.axisCount > 0)
			{
				this.SelectAxis(0);
			}
			base.defaultUIElement = this.doneButton.gameObject;
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0012FF20 File Offset: 0x0012E120
		public void SetButtonCallback(CalibrationWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
		{
			if (!base.initialized)
			{
				return;
			}
			if (callback == null)
			{
				return;
			}
			if (this.buttonCallbacks.ContainsKey((int)buttonIdentifier))
			{
				this.buttonCallbacks[(int)buttonIdentifier] = callback;
			}
			else
			{
				this.buttonCallbacks.Add((int)buttonIdentifier, callback);
			}
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x0012FF70 File Offset: 0x0012E170
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick != null)
			{
				this.joystick.ImportCalibrationMapFromXmlString(this.origCalibrationData);
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(1, out action))
			{
				if (this.cancelCallback != null)
				{
					this.cancelCallback.Invoke();
				}
				return;
			}
			action(base.id);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00034247 File Offset: 0x00032447
		public override void Update()
		{
			if (!base.initialized)
			{
				return;
			}
			base.Update();
			this.UpdateDisplay();
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0012FFDC File Offset: 0x0012E1DC
		public void OnDone()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(0, out action))
			{
				return;
			}
			action(base.id);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00034261 File Offset: 0x00032461
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00034269 File Offset: 0x00032469
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.joystick.calibrationMap.Reset();
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00130018 File Offset: 0x0012E218
		public void OnCalibrate()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(3, out action))
			{
				return;
			}
			action(this.selectedAxis);
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0003429F File Offset: 0x0003249F
		public void OnInvert(bool state)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.invert = state;
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x000342C5 File Offset: 0x000324C5
		public void OnZeroValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = value;
			this.RedrawCalibratedZero();
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x000342F1 File Offset: 0x000324F1
		public void OnZeroCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = this.origSelectedAxisCalibrationData.zero;
			this.RedrawCalibratedZero();
			this.RefreshControls();
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x00130054 File Offset: 0x0012E254
		public void OnDeadzoneValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = Mathf.Clamp(value, 0f, 0.8f);
			if (value > 0.8f)
			{
				this.deadzoneSlider.value = 0.8f;
			}
			this.RedrawDeadzone();
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0003432D File Offset: 0x0003252D
		public void OnDeadzoneCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = this.origSelectedAxisCalibrationData.deadZone;
			this.RedrawDeadzone();
			this.RefreshControls();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x001300B8 File Offset: 0x0012E2B8
		public void OnSensitivityValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = Mathf.Clamp(value, this.minSensitivity, float.PositiveInfinity);
			if (value < this.minSensitivity)
			{
				this.sensitivitySlider.value = this.minSensitivity;
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00034369 File Offset: 0x00032569
		public void OnSensitivityCancel(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = this.origSelectedAxisCalibrationData.sensitivity;
			this.RefreshControls();
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x0003439F File Offset: 0x0003259F
		public void OnAxisScrollRectScroll(Vector2 pos)
		{
			if (!base.initialized)
			{
				return;
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000343AD File Offset: 0x000325AD
		public void OnAxisSelected(int axisIndex, Button button)
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.SelectAxis(axisIndex);
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000343DA File Offset: 0x000325DA
		public void UpdateDisplay()
		{
			this.RedrawValueMarkers();
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x000343E2 File Offset: 0x000325E2
		public void Redraw()
		{
			this.RedrawCalibratedZero();
			this.RedrawValueMarkers();
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00130118 File Offset: 0x0012E318
		public void RefreshControls()
		{
			if (!this.axisSelected)
			{
				this.deadzoneSlider.value = 0f;
				this.zeroSlider.value = 0f;
				this.sensitivitySlider.value = 0f;
				this.invertToggle.isOn = false;
			}
			else
			{
				this.deadzoneSlider.value = this.axisCalibration.deadZone;
				this.zeroSlider.value = this.axisCalibration.calibratedZero;
				this.sensitivitySlider.value = this.axisCalibration.sensitivity;
				this.invertToggle.isOn = this.axisCalibration.invert;
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x001301CC File Offset: 0x0012E3CC
		public void RedrawDeadzone()
		{
			if (!this.axisSelected)
			{
				return;
			}
			float num = this.displayAreaWidth * this.axisCalibration.deadZone;
			this.deadzoneArea.sizeDelta = new Vector2(num, this.deadzoneArea.sizeDelta.y);
			this.deadzoneArea.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.deadzoneArea.anchoredPosition.y);
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00130264 File Offset: 0x0012E464
		public void RedrawCalibratedZero()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.calibratedZeroMarker.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.calibratedZeroMarker.anchoredPosition.y);
			this.RedrawDeadzone();
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x001302CC File Offset: 0x0012E4CC
		public void RedrawValueMarkers()
		{
			if (!this.axisSelected)
			{
				this.calibratedValueMarker.anchoredPosition = new Vector2(0f, this.calibratedValueMarker.anchoredPosition.y);
				this.rawValueMarker.anchoredPosition = new Vector2(0f, this.rawValueMarker.anchoredPosition.y);
				return;
			}
			float axis = this.joystick.GetAxis(this.selectedAxis);
			float num = Mathf.Clamp(this.joystick.GetAxisRaw(this.selectedAxis), -1f, 1f);
			this.calibratedValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * axis, this.calibratedValueMarker.anchoredPosition.y);
			this.rawValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * num, this.rawValueMarker.anchoredPosition.y);
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x001303CC File Offset: 0x0012E5CC
		public void SelectAxis(int index)
		{
			if (index < 0 || index >= this.axisButtons.Count)
			{
				return;
			}
			if (this.axisButtons[index] == null)
			{
				return;
			}
			this.axisButtons[index].interactable = false;
			this.axisButtons[index].Select();
			for (int i = 0; i < this.axisButtons.Count; i++)
			{
				if (i != index)
				{
					this.axisButtons[i].interactable = true;
				}
			}
			this.selectedAxis = index;
			this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
			this.SetMinSensitivity();
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x000343F0 File Offset: 0x000325F0
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
			if (this.selectedAxis >= 0)
			{
				this.SelectAxis(this.selectedAxis);
			}
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00130488 File Offset: 0x0012E688
		public void SetMinSensitivity()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.minSensitivity = 0.1f;
			if (this.rewiredStandaloneInputModule != null)
			{
				if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
				}
				else if (this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
				}
			}
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x00130520 File Offset: 0x0012E720
		public bool IsMenuAxis(int actionId, int axisIndex)
		{
			if (this.rewiredStandaloneInputModule == null)
			{
				return false;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			int count = allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				IList<JoystickMap> maps = allPlayers[i].controllers.maps.GetMaps<JoystickMap>(this.joystick.id);
				if (maps != null)
				{
					int count2 = maps.Count;
					for (int j = 0; j < count2; j++)
					{
						IList<ActionElementMap> axisMaps = maps[j].AxisMaps;
						if (axisMaps != null)
						{
							int count3 = axisMaps.Count;
							for (int k = 0; k < count3; k++)
							{
								ActionElementMap actionElementMap = axisMaps[k];
								if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00130614 File Offset: 0x0012E814
		public void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			int behaviorId = action.behaviorId;
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			value = inputBehavior.buttonDeadZone + 0.1f;
		}

		// Token: 0x040033A8 RID: 13224
		public const float minSensitivityOtherAxes = 0.1f;

		// Token: 0x040033A9 RID: 13225
		public const float maxDeadzone = 0.8f;

		// Token: 0x040033AA RID: 13226
		[SerializeField]
		public RectTransform rightContentContainer;

		// Token: 0x040033AB RID: 13227
		[SerializeField]
		public RectTransform valueDisplayGroup;

		// Token: 0x040033AC RID: 13228
		[SerializeField]
		public RectTransform calibratedValueMarker;

		// Token: 0x040033AD RID: 13229
		[SerializeField]
		public RectTransform rawValueMarker;

		// Token: 0x040033AE RID: 13230
		[SerializeField]
		public RectTransform calibratedZeroMarker;

		// Token: 0x040033AF RID: 13231
		[SerializeField]
		public RectTransform deadzoneArea;

		// Token: 0x040033B0 RID: 13232
		[SerializeField]
		public Slider deadzoneSlider;

		// Token: 0x040033B1 RID: 13233
		[SerializeField]
		public Slider zeroSlider;

		// Token: 0x040033B2 RID: 13234
		[SerializeField]
		public Slider sensitivitySlider;

		// Token: 0x040033B3 RID: 13235
		[SerializeField]
		public Toggle invertToggle;

		// Token: 0x040033B4 RID: 13236
		[SerializeField]
		public RectTransform axisScrollAreaContent;

		// Token: 0x040033B5 RID: 13237
		[SerializeField]
		public Button doneButton;

		// Token: 0x040033B6 RID: 13238
		[SerializeField]
		public Button calibrateButton;

		// Token: 0x040033B7 RID: 13239
		[SerializeField]
		public Text doneButtonLabel;

		// Token: 0x040033B8 RID: 13240
		[SerializeField]
		public Text cancelButtonLabel;

		// Token: 0x040033B9 RID: 13241
		[SerializeField]
		public Text defaultButtonLabel;

		// Token: 0x040033BA RID: 13242
		[SerializeField]
		public Text deadzoneSliderLabel;

		// Token: 0x040033BB RID: 13243
		[SerializeField]
		public Text zeroSliderLabel;

		// Token: 0x040033BC RID: 13244
		[SerializeField]
		public Text sensitivitySliderLabel;

		// Token: 0x040033BD RID: 13245
		[SerializeField]
		public Text invertToggleLabel;

		// Token: 0x040033BE RID: 13246
		[SerializeField]
		public Text calibrateButtonLabel;

		// Token: 0x040033BF RID: 13247
		[SerializeField]
		public GameObject axisButtonPrefab;

		// Token: 0x040033C0 RID: 13248
		public Joystick joystick;

		// Token: 0x040033C1 RID: 13249
		public string origCalibrationData;

		// Token: 0x040033C2 RID: 13250
		public int selectedAxis = -1;

		// Token: 0x040033C3 RID: 13251
		public AxisCalibrationData origSelectedAxisCalibrationData;

		// Token: 0x040033C4 RID: 13252
		public float displayAreaWidth;

		// Token: 0x040033C5 RID: 13253
		public List<Button> axisButtons;

		// Token: 0x040033C6 RID: 13254
		public Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x040033C7 RID: 13255
		public int playerId;

		// Token: 0x040033C8 RID: 13256
		public RewiredStandaloneInputModule rewiredStandaloneInputModule;

		// Token: 0x040033C9 RID: 13257
		public int menuHorizActionId = -1;

		// Token: 0x040033CA RID: 13258
		public int menuVertActionId = -1;

		// Token: 0x040033CB RID: 13259
		public float minSensitivity;

		// Token: 0x020012B0 RID: 4784
		public enum ButtonIdentifier
		{
			// Token: 0x040080AA RID: 32938
			Done,
			// Token: 0x040080AB RID: 32939
			Cancel,
			// Token: 0x040080AC RID: 32940
			Default,
			// Token: 0x040080AD RID: 32941
			Calibrate
		}
	}
}
