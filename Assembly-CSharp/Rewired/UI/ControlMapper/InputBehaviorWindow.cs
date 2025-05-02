using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000641 RID: 1601
	[AddComponentMenu("")]
	public class InputBehaviorWindow : Window
	{
		// Token: 0x060042E4 RID: 17124 RVA: 0x00137844 File Offset: 0x00135A44
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.spawnTransform == null || this.doneButton == null || this.cancelButton == null || this.defaultButton == null || this.uiControlSetPrefab == null || this.uiSliderControlPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00137954 File Offset: 0x00135B54
		public void SetData(int playerId, ControlMapper.InputBehaviorSettings[] data)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			foreach (ControlMapper.InputBehaviorSettings inputBehaviorSettings in data)
			{
				if (inputBehaviorSettings != null && inputBehaviorSettings.isValid)
				{
					InputBehavior inputBehavior = this.GetInputBehavior(inputBehaviorSettings.inputBehaviorId);
					if (inputBehavior != null)
					{
						UIControlSet uicontrolSet = this.CreateControlSet();
						Dictionary<int, InputBehaviorWindow.PropertyType> dictionary = new Dictionary<int, InputBehaviorWindow.PropertyType>();
						string customEntry = ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.labelLanguageKey);
						if (!string.IsNullOrEmpty(customEntry))
						{
							uicontrolSet.SetTitle(customEntry);
						}
						else
						{
							uicontrolSet.SetTitle(inputBehavior.name);
						}
						if (inputBehaviorSettings.showJoystickAxisSensitivity)
						{
							UISliderControl uisliderControl = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.joystickAxisSensitivityLabelLanguageKey), inputBehaviorSettings.joystickAxisSensitivityIcon, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
							uisliderControl.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax);
							dictionary.Add(uisliderControl.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
						}
						if (inputBehaviorSettings.showMouseXYAxisSensitivity)
						{
							UISliderControl uisliderControl2 = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), inputBehaviorSettings.mouseXYAxisSensitivityIcon, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
							uisliderControl2.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax);
							dictionary.Add(uisliderControl2.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
						}
						this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, uicontrolSet, dictionary));
					}
				}
			}
			base.defaultUIElement = this.doneButton.gameObject;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00137B34 File Offset: 0x00135D34
		public void SetButtonCallback(InputBehaviorWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
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

		// Token: 0x060042E7 RID: 17127 RVA: 0x00137B84 File Offset: 0x00135D84
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestorePreviousData();
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

		// Token: 0x060042E8 RID: 17128 RVA: 0x00137C24 File Offset: 0x00135E24
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

		// Token: 0x060042E9 RID: 17129 RVA: 0x000358B1 File Offset: 0x00033AB1
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00137C60 File Offset: 0x00135E60
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestoreDefaultData();
			}
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x000358B9 File Offset: 0x00033AB9
		public void JoystickAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x000358C8 File Offset: 0x00033AC8
		public void MouseXYAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value;
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00137CC8 File Offset: 0x00135EC8
		public void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00137CEC File Offset: 0x00135EEC
		public void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x000358D7 File Offset: 0x00033AD7
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x00137D10 File Offset: 0x00135F10
		public UIControlSet CreateControlSet()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.uiControlSetPrefab);
			gameObject.transform.SetParent(this.spawnTransform, false);
			return gameObject.GetComponent<UIControlSet>();
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00137D44 File Offset: 0x00135F44
		public UISliderControl CreateSlider(UIControlSet set, int inputBehaviorId, string defaultTitle, string overrideTitle, Sprite icon, float minValue, float maxValue, Action<int, int, float> valueChangedCallback, Action<int, int> cancelCallback)
		{
			UISliderControl uisliderControl = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, delegate(int cId, float value)
			{
				valueChangedCallback(inputBehaviorId, cId, value);
			}, delegate(int cId)
			{
				cancelCallback(inputBehaviorId, cId);
			});
			string text = (!string.IsNullOrEmpty(overrideTitle)) ? overrideTitle : defaultTitle;
			if (!string.IsNullOrEmpty(text))
			{
				uisliderControl.showTitle = true;
				uisliderControl.title.text = text;
			}
			else
			{
				uisliderControl.showTitle = false;
			}
			uisliderControl.showIcon = (icon != null);
			return uisliderControl;
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x000358DF File Offset: 0x00033ADF
		public InputBehavior GetInputBehavior(int id)
		{
			return ReInput.mapping.GetInputBehavior(this.playerId, id);
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x00137DE8 File Offset: 0x00135FE8
		public InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(int inputBehaviorId)
		{
			int count = this.inputBehaviorInfo.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.inputBehaviorInfo[i].inputBehavior.id == inputBehaviorId)
				{
					return this.inputBehaviorInfo[i];
				}
			}
			return null;
		}

		// Token: 0x04003466 RID: 13414
		public const float minSensitivity = 0.1f;

		// Token: 0x04003467 RID: 13415
		[SerializeField]
		public RectTransform spawnTransform;

		// Token: 0x04003468 RID: 13416
		[SerializeField]
		public Button doneButton;

		// Token: 0x04003469 RID: 13417
		[SerializeField]
		public Button cancelButton;

		// Token: 0x0400346A RID: 13418
		[SerializeField]
		public Button defaultButton;

		// Token: 0x0400346B RID: 13419
		[SerializeField]
		public Text doneButtonLabel;

		// Token: 0x0400346C RID: 13420
		[SerializeField]
		public Text cancelButtonLabel;

		// Token: 0x0400346D RID: 13421
		[SerializeField]
		public Text defaultButtonLabel;

		// Token: 0x0400346E RID: 13422
		[SerializeField]
		public GameObject uiControlSetPrefab;

		// Token: 0x0400346F RID: 13423
		[SerializeField]
		public GameObject uiSliderControlPrefab;

		// Token: 0x04003470 RID: 13424
		public List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;

		// Token: 0x04003471 RID: 13425
		public Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x04003472 RID: 13426
		public int playerId;

		// Token: 0x020012D1 RID: 4817
		public class InputBehaviorInfo
		{
			// Token: 0x06008336 RID: 33590 RVA: 0x00057794 File Offset: 0x00055994
			public InputBehaviorInfo(InputBehavior inputBehavior, UIControlSet controlSet, Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
			{
				this._inputBehavior = inputBehavior;
				this._controlSet = controlSet;
				this.idToProperty = idToProperty;
				this.copyOfOriginal = new InputBehavior(inputBehavior);
			}

			// Token: 0x1700199D RID: 6557
			// (get) Token: 0x06008337 RID: 33591 RVA: 0x000577BD File Offset: 0x000559BD
			public InputBehavior inputBehavior
			{
				get
				{
					return this._inputBehavior;
				}
			}

			// Token: 0x1700199E RID: 6558
			// (get) Token: 0x06008338 RID: 33592 RVA: 0x000577C5 File Offset: 0x000559C5
			public UIControlSet controlSet
			{
				get
				{
					return this._controlSet;
				}
			}

			// Token: 0x06008339 RID: 33593 RVA: 0x000577CD File Offset: 0x000559CD
			public void RestorePreviousData()
			{
				this._inputBehavior.ImportData(this.copyOfOriginal);
			}

			// Token: 0x0600833A RID: 33594 RVA: 0x000577E1 File Offset: 0x000559E1
			public void RestoreDefaultData()
			{
				this._inputBehavior.Reset();
				this.RefreshControls();
			}

			// Token: 0x0600833B RID: 33595 RVA: 0x0029E674 File Offset: 0x0029C874
			public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
			{
				if (propertyType != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
				{
					if (propertyType == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
					{
						float mouseXYAxisSensitivity = this.copyOfOriginal.mouseXYAxisSensitivity;
						this._inputBehavior.mouseXYAxisSensitivity = mouseXYAxisSensitivity;
						UISliderControl control = this._controlSet.GetControl<UISliderControl>(controlId);
						if (control != null)
						{
							control.slider.value = mouseXYAxisSensitivity;
						}
					}
				}
				else
				{
					float joystickAxisSensitivity = this.copyOfOriginal.joystickAxisSensitivity;
					this._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity;
					UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control2 != null)
					{
						control2.slider.value = joystickAxisSensitivity;
					}
				}
			}

			// Token: 0x0600833C RID: 33596 RVA: 0x0029E718 File Offset: 0x0029C918
			public void RefreshControls()
			{
				if (this._controlSet == null)
				{
					return;
				}
				if (this.idToProperty == null)
				{
					return;
				}
				foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
				{
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
					if (!(control == null))
					{
						InputBehaviorWindow.PropertyType value = keyValuePair.Value;
						if (value != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
						{
							if (value == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
							{
								control.slider.value = this._inputBehavior.mouseXYAxisSensitivity;
							}
						}
						else
						{
							control.slider.value = this._inputBehavior.joystickAxisSensitivity;
						}
					}
				}
			}

			// Token: 0x04008163 RID: 33123
			public InputBehavior _inputBehavior;

			// Token: 0x04008164 RID: 33124
			public UIControlSet _controlSet;

			// Token: 0x04008165 RID: 33125
			public Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;

			// Token: 0x04008166 RID: 33126
			public InputBehavior copyOfOriginal;
		}

		// Token: 0x020012D2 RID: 4818
		public enum ButtonIdentifier
		{
			// Token: 0x04008168 RID: 33128
			Done,
			// Token: 0x04008169 RID: 33129
			Cancel,
			// Token: 0x0400816A RID: 33130
			Default
		}

		// Token: 0x020012D3 RID: 4819
		public enum PropertyType
		{
			// Token: 0x0400816C RID: 33132
			JoystickAxisSensitivity,
			// Token: 0x0400816D RID: 33133
			MouseXYAxisSensitivity
		}
	}
}
