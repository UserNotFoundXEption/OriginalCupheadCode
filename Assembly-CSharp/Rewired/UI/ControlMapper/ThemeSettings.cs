using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000648 RID: 1608
	[Serializable]
	public class ThemeSettings : ScriptableObject
	{
		// Token: 0x06004353 RID: 17235 RVA: 0x00138B10 File Offset: 0x00136D10
		public void Apply(ThemedElement.ElementInfo[] elementInfo)
		{
			if (elementInfo == null)
			{
				return;
			}
			for (int i = 0; i < elementInfo.Length; i++)
			{
				if (elementInfo[i] != null)
				{
					this.Apply(elementInfo[i].themeClass, elementInfo[i].component);
				}
			}
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x00138B5C File Offset: 0x00136D5C
		public void Apply(string themeClass, Component component)
		{
			if (component as Selectable != null)
			{
				this.Apply(themeClass, (Selectable)component);
				return;
			}
			if (component as Image != null)
			{
				this.Apply(themeClass, (Image)component);
				return;
			}
			if (component as Text != null)
			{
				this.Apply(themeClass, (Text)component);
				return;
			}
			if (component as UIImageHelper != null)
			{
				this.Apply(themeClass, (UIImageHelper)component);
				return;
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00138BE8 File Offset: 0x00136DE8
		public void Apply(string themeClass, Selectable item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.SelectableSettings_Base selectableSettings_Base;
			if (item as Button != null)
			{
				if (themeClass != null)
				{
					if (themeClass == "inputGridField")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_A5;
					}
					if (themeClass == "windowButton")
					{
						selectableSettings_Base = this._windowButtonSettings;
						goto IL_A5;
					}
					if (themeClass == "playerButton")
					{
						selectableSettings_Base = this._playerButtonSettings;
						goto IL_A5;
					}
					if (themeClass == "playerDropdownButton")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_A5;
					}
				}
				selectableSettings_Base = this._buttonSettings;
				IL_A5:;
			}
			else if (item as Scrollbar != null)
			{
				selectableSettings_Base = this._scrollbarSettings;
			}
			else if (item as Slider != null)
			{
				selectableSettings_Base = this._sliderSettings;
			}
			else if (item as Toggle != null)
			{
				if (themeClass != null)
				{
					if (themeClass == "inputGridField")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_144;
					}
					if (themeClass == "button")
					{
						selectableSettings_Base = this._buttonSettings;
						goto IL_144;
					}
				}
				selectableSettings_Base = this._selectableSettings;
				IL_144:;
			}
			else
			{
				selectableSettings_Base = this._selectableSettings;
			}
			selectableSettings_Base.Apply(item);
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x00138D4C File Offset: 0x00136F4C
		public void Apply(string themeClass, Image item)
		{
			if (item == null)
			{
				return;
			}
			switch (themeClass)
			{
			case "area":
				this._areaBackground.CopyTo(item);
				break;
			case "popupWindow":
				this._popupWindowBackground.CopyTo(item);
				break;
			case "mainWindow":
				this._mainWindowBackground.CopyTo(item);
				break;
			case "calibrationValueMarker":
				this._calibrationValueMarker.CopyTo(item);
				break;
			case "calibrationRawValueMarker":
				this._calibrationRawValueMarker.CopyTo(item);
				break;
			case "invertToggle":
				this._invertToggle.CopyTo(item);
				break;
			case "invertToggleBackground":
				this._inputGridFieldSettings.imageSettings.CopyTo(item);
				item.sprite = this._inputGridFieldSettings.imageSettings.sprite;
				break;
			case "invertToggleButtonBackground":
				this._buttonSettings.imageSettings.CopyTo(item);
				break;
			}
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00138EC8 File Offset: 0x001370C8
		public void Apply(string themeClass, Text item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.TextSettings textSettings;
			switch (themeClass)
			{
			case "button":
				textSettings = this._buttonTextSettings;
				goto IL_171;
			case "windowButton":
				textSettings = this._windowButtonTextSettings;
				goto IL_171;
			case "playerButton":
				textSettings = this._playerButtonTextSettings;
				goto IL_171;
			case "playerDropdownButton":
				textSettings = this._playerDropdownButtonTextSettings;
				goto IL_171;
			case "restoreDefaultButton":
				textSettings = this._restoreDefaultButtonTextSettings;
				goto IL_171;
			case "inputGridField":
				textSettings = this._inputGridFieldTextSettings;
				goto IL_171;
			case "actionsColumn":
				textSettings = this._actionColumnTextSettings;
				goto IL_171;
			case "actionsColumnDeactivated":
				textSettings = this._actionColumnDeactivatedTextSettings;
				goto IL_171;
			case "actionsColumnHeader":
				textSettings = this._actionColumnHeaderTextSettings;
				goto IL_171;
			case "inputColumnHeader":
				textSettings = this._inputColumnHeaderTextSettings;
				goto IL_171;
			}
			textSettings = this._textSettings;
			IL_171:
			if (textSettings.fontTypes != null && (Localization.Languages)textSettings.fontTypes.Length > Localization.language)
			{
				item.font = FontLoader.GetFont(textSettings.fontTypes[(int)Localization.language]);
			}
			item.color = textSettings.color;
			item.lineSpacing = textSettings.lineSpacing;
			if (textSettings.sizeMultiplier != 1f)
			{
				item.fontSize = (int)((float)item.fontSize * textSettings.sizeMultiplier);
				item.resizeTextMaxSize = (int)((float)item.resizeTextMaxSize * textSettings.sizeMultiplier);
				item.resizeTextMinSize = (int)((float)item.resizeTextMinSize * textSettings.sizeMultiplier);
			}
			if (textSettings.overrideSize != 0)
			{
				item.fontSize = textSettings.overrideSize;
				item.resizeTextMaxSize = textSettings.overrideSize;
				item.resizeTextMinSize = textSettings.overrideSize;
			}
			if ((Localization.Languages)textSettings.style.Length > Localization.language && textSettings.style[(int)Localization.language] != ThemeSettings.FontStyleOverride.Default)
			{
				item.fontStyle = textSettings.style[(int)Localization.language] - ThemeSettings.FontStyleOverride.Normal;
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x00035B68 File Offset: 0x00033D68
		public void Apply(string themeClass, UIImageHelper item)
		{
			if (item == null)
			{
				return;
			}
			item.SetEnabledStateColor(this._invertToggle.color);
			item.SetDisabledStateColor(this._invertToggleDisabledColor);
			item.Refresh();
		}

		// Token: 0x040034BE RID: 13502
		[SerializeField]
		public ThemeSettings.ImageSettings _mainWindowBackground;

		// Token: 0x040034BF RID: 13503
		[SerializeField]
		public ThemeSettings.ImageSettings _popupWindowBackground;

		// Token: 0x040034C0 RID: 13504
		[SerializeField]
		public ThemeSettings.ImageSettings _areaBackground;

		// Token: 0x040034C1 RID: 13505
		[SerializeField]
		public ThemeSettings.SelectableSettings _selectableSettings;

		// Token: 0x040034C2 RID: 13506
		[SerializeField]
		public ThemeSettings.SelectableSettings _buttonSettings;

		// Token: 0x040034C3 RID: 13507
		[SerializeField]
		public ThemeSettings.SelectableSettings _windowButtonSettings;

		// Token: 0x040034C4 RID: 13508
		[SerializeField]
		public ThemeSettings.SelectableSettings _playerButtonSettings;

		// Token: 0x040034C5 RID: 13509
		[SerializeField]
		public ThemeSettings.SelectableSettings _inputGridFieldSettings;

		// Token: 0x040034C6 RID: 13510
		[SerializeField]
		public ThemeSettings.ScrollbarSettings _scrollbarSettings;

		// Token: 0x040034C7 RID: 13511
		[SerializeField]
		public ThemeSettings.SliderSettings _sliderSettings;

		// Token: 0x040034C8 RID: 13512
		[SerializeField]
		public ThemeSettings.ImageSettings _invertToggle;

		// Token: 0x040034C9 RID: 13513
		[SerializeField]
		public Color _invertToggleDisabledColor;

		// Token: 0x040034CA RID: 13514
		[SerializeField]
		public ThemeSettings.ImageSettings _calibrationValueMarker;

		// Token: 0x040034CB RID: 13515
		[SerializeField]
		public ThemeSettings.ImageSettings _calibrationRawValueMarker;

		// Token: 0x040034CC RID: 13516
		[SerializeField]
		public ThemeSettings.TextSettings _textSettings;

		// Token: 0x040034CD RID: 13517
		[SerializeField]
		public ThemeSettings.TextSettings _buttonTextSettings;

		// Token: 0x040034CE RID: 13518
		[SerializeField]
		public ThemeSettings.TextSettings _windowButtonTextSettings;

		// Token: 0x040034CF RID: 13519
		[SerializeField]
		public ThemeSettings.TextSettings _playerButtonTextSettings;

		// Token: 0x040034D0 RID: 13520
		[SerializeField]
		public ThemeSettings.TextSettings _playerDropdownButtonTextSettings;

		// Token: 0x040034D1 RID: 13521
		[SerializeField]
		public ThemeSettings.TextSettings _restoreDefaultButtonTextSettings;

		// Token: 0x040034D2 RID: 13522
		[SerializeField]
		public ThemeSettings.TextSettings _actionColumnTextSettings;

		// Token: 0x040034D3 RID: 13523
		[SerializeField]
		public ThemeSettings.TextSettings _actionColumnDeactivatedTextSettings;

		// Token: 0x040034D4 RID: 13524
		[SerializeField]
		public ThemeSettings.TextSettings _actionColumnHeaderTextSettings;

		// Token: 0x040034D5 RID: 13525
		[SerializeField]
		public ThemeSettings.TextSettings _inputColumnHeaderTextSettings;

		// Token: 0x040034D6 RID: 13526
		[SerializeField]
		public ThemeSettings.TextSettings _inputGridFieldTextSettings;

		// Token: 0x020012D8 RID: 4824
		[Serializable]
		public abstract class SelectableSettings_Base
		{
			// Token: 0x0600834C RID: 33612 RVA: 0x0005788A File Offset: 0x00055A8A
			public SelectableSettings_Base()
			{
			}

			// Token: 0x170019A3 RID: 6563
			// (get) Token: 0x0600834D RID: 33613 RVA: 0x00057892 File Offset: 0x00055A92
			public Selectable.Transition transition
			{
				get
				{
					return this._transition;
				}
			}

			// Token: 0x170019A4 RID: 6564
			// (get) Token: 0x0600834E RID: 33614 RVA: 0x0005789A File Offset: 0x00055A9A
			public ThemeSettings.CustomColorBlock selectableColors
			{
				get
				{
					return this._colors;
				}
			}

			// Token: 0x170019A5 RID: 6565
			// (get) Token: 0x0600834F RID: 33615 RVA: 0x000578A2 File Offset: 0x00055AA2
			public ThemeSettings.CustomSpriteState spriteState
			{
				get
				{
					return this._spriteState;
				}
			}

			// Token: 0x170019A6 RID: 6566
			// (get) Token: 0x06008350 RID: 33616 RVA: 0x000578AA File Offset: 0x00055AAA
			public ThemeSettings.CustomAnimationTriggers animationTriggers
			{
				get
				{
					return this._animationTriggers;
				}
			}

			// Token: 0x06008351 RID: 33617 RVA: 0x0029E92C File Offset: 0x0029CB2C
			public virtual void Apply(Selectable item)
			{
				Selectable.Transition transition = this._transition;
				bool flag = item.transition != transition;
				item.transition = transition;
				ICustomSelectable customSelectable = item as ICustomSelectable;
				ThemeSettings.CustomColorBlock colors = this._colors;
				colors.fadeDuration = 0f;
				item.colors = colors;
				colors.fadeDuration = this._colors.fadeDuration;
				item.colors = colors;
				if (customSelectable != null)
				{
					customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
				}
				if (transition == 2)
				{
					item.spriteState = this._spriteState;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
					}
				}
				else if (transition == 3)
				{
					item.animationTriggers.disabledTrigger = this._animationTriggers.disabledTrigger;
					item.animationTriggers.highlightedTrigger = this._animationTriggers.highlightedTrigger;
					item.animationTriggers.normalTrigger = this._animationTriggers.normalTrigger;
					item.animationTriggers.pressedTrigger = this._animationTriggers.pressedTrigger;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
					}
				}
				if (flag)
				{
					item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0f, true, true);
				}
			}

			// Token: 0x0400817A RID: 33146
			[SerializeField]
			public Selectable.Transition _transition;

			// Token: 0x0400817B RID: 33147
			[SerializeField]
			public ThemeSettings.CustomColorBlock _colors;

			// Token: 0x0400817C RID: 33148
			[SerializeField]
			public ThemeSettings.CustomSpriteState _spriteState;

			// Token: 0x0400817D RID: 33149
			[SerializeField]
			public ThemeSettings.CustomAnimationTriggers _animationTriggers;
		}

		// Token: 0x020012D9 RID: 4825
		[Serializable]
		public class SelectableSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x170019A7 RID: 6567
			// (get) Token: 0x06008353 RID: 33619 RVA: 0x000578BA File Offset: 0x00055ABA
			public ThemeSettings.ImageSettings imageSettings
			{
				get
				{
					return this._imageSettings;
				}
			}

			// Token: 0x06008354 RID: 33620 RVA: 0x000578C2 File Offset: 0x00055AC2
			public override void Apply(Selectable item)
			{
				if (item == null)
				{
					return;
				}
				base.Apply(item);
				if (this._imageSettings != null)
				{
					this._imageSettings.CopyTo(item.targetGraphic as Image);
				}
			}

			// Token: 0x0400817E RID: 33150
			[SerializeField]
			public ThemeSettings.ImageSettings _imageSettings;
		}

		// Token: 0x020012DA RID: 4826
		[Serializable]
		public class SliderSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x170019A8 RID: 6568
			// (get) Token: 0x06008356 RID: 33622 RVA: 0x00057901 File Offset: 0x00055B01
			public ThemeSettings.ImageSettings handleImageSettings
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x170019A9 RID: 6569
			// (get) Token: 0x06008357 RID: 33623 RVA: 0x00057909 File Offset: 0x00055B09
			public ThemeSettings.ImageSettings fillImageSettings
			{
				get
				{
					return this._fillImageSettings;
				}
			}

			// Token: 0x170019AA RID: 6570
			// (get) Token: 0x06008358 RID: 33624 RVA: 0x00057911 File Offset: 0x00055B11
			public ThemeSettings.ImageSettings backgroundImageSettings
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x06008359 RID: 33625 RVA: 0x0029EA78 File Offset: 0x0029CC78
			public void Apply(Slider item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._fillImageSettings != null)
				{
					RectTransform fillRect = item.fillRect;
					if (fillRect != null)
					{
						this._fillImageSettings.CopyTo(fillRect.GetComponent<Image>());
					}
				}
				if (this._backgroundImageSettings != null)
				{
					Transform transform = item.transform.Find("Background");
					if (transform != null)
					{
						this._backgroundImageSettings.CopyTo(transform.GetComponent<Image>());
					}
				}
			}

			// Token: 0x0600835A RID: 33626 RVA: 0x00057919 File Offset: 0x00055B19
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Slider);
			}

			// Token: 0x0400817F RID: 33151
			[SerializeField]
			public ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x04008180 RID: 33152
			[SerializeField]
			public ThemeSettings.ImageSettings _fillImageSettings;

			// Token: 0x04008181 RID: 33153
			[SerializeField]
			public ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x020012DB RID: 4827
		[Serializable]
		public class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x170019AB RID: 6571
			// (get) Token: 0x0600835C RID: 33628 RVA: 0x00057936 File Offset: 0x00055B36
			public ThemeSettings.ImageSettings handle
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x170019AC RID: 6572
			// (get) Token: 0x0600835D RID: 33629 RVA: 0x0005793E File Offset: 0x00055B3E
			public ThemeSettings.ImageSettings background
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x0600835E RID: 33630 RVA: 0x0029EB1C File Offset: 0x0029CD1C
			public void Apply(Scrollbar item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._backgroundImageSettings != null)
				{
					this._backgroundImageSettings.CopyTo(item.GetComponent<Image>());
				}
			}

			// Token: 0x0600835F RID: 33631 RVA: 0x00057946 File Offset: 0x00055B46
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Scrollbar);
			}

			// Token: 0x04008182 RID: 33154
			[SerializeField]
			public ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x04008183 RID: 33155
			[SerializeField]
			public ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x020012DC RID: 4828
		[Serializable]
		public class ImageSettings
		{
			// Token: 0x170019AD RID: 6573
			// (get) Token: 0x06008361 RID: 33633 RVA: 0x0005796E File Offset: 0x00055B6E
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x170019AE RID: 6574
			// (get) Token: 0x06008362 RID: 33634 RVA: 0x00057976 File Offset: 0x00055B76
			public Sprite sprite
			{
				get
				{
					return this._sprite;
				}
			}

			// Token: 0x170019AF RID: 6575
			// (get) Token: 0x06008363 RID: 33635 RVA: 0x0005797E File Offset: 0x00055B7E
			public Material materal
			{
				get
				{
					return this._materal;
				}
			}

			// Token: 0x170019B0 RID: 6576
			// (get) Token: 0x06008364 RID: 33636 RVA: 0x00057986 File Offset: 0x00055B86
			public Image.Type type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x170019B1 RID: 6577
			// (get) Token: 0x06008365 RID: 33637 RVA: 0x0005798E File Offset: 0x00055B8E
			public bool preserveAspect
			{
				get
				{
					return this._preserveAspect;
				}
			}

			// Token: 0x170019B2 RID: 6578
			// (get) Token: 0x06008366 RID: 33638 RVA: 0x00057996 File Offset: 0x00055B96
			public bool fillCenter
			{
				get
				{
					return this._fillCenter;
				}
			}

			// Token: 0x170019B3 RID: 6579
			// (get) Token: 0x06008367 RID: 33639 RVA: 0x0005799E File Offset: 0x00055B9E
			public Image.FillMethod fillMethod
			{
				get
				{
					return this._fillMethod;
				}
			}

			// Token: 0x170019B4 RID: 6580
			// (get) Token: 0x06008368 RID: 33640 RVA: 0x000579A6 File Offset: 0x00055BA6
			public float fillAmout
			{
				get
				{
					return this._fillAmout;
				}
			}

			// Token: 0x170019B5 RID: 6581
			// (get) Token: 0x06008369 RID: 33641 RVA: 0x000579AE File Offset: 0x00055BAE
			public bool fillClockwise
			{
				get
				{
					return this._fillClockwise;
				}
			}

			// Token: 0x170019B6 RID: 6582
			// (get) Token: 0x0600836A RID: 33642 RVA: 0x000579B6 File Offset: 0x00055BB6
			public int fillOrigin
			{
				get
				{
					return this._fillOrigin;
				}
			}

			// Token: 0x0600836B RID: 33643 RVA: 0x0029EB74 File Offset: 0x0029CD74
			public virtual void CopyTo(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this._color;
				image.sprite = this._sprite;
				image.material = this._materal;
				image.type = this._type;
				image.preserveAspect = this._preserveAspect;
				image.fillCenter = this._fillCenter;
				image.fillMethod = this._fillMethod;
				image.fillAmount = this._fillAmout;
				image.fillClockwise = this._fillClockwise;
				image.fillOrigin = this._fillOrigin;
			}

			// Token: 0x04008184 RID: 33156
			[SerializeField]
			public Color _color = Color.white;

			// Token: 0x04008185 RID: 33157
			[SerializeField]
			public Sprite _sprite;

			// Token: 0x04008186 RID: 33158
			[SerializeField]
			public Material _materal;

			// Token: 0x04008187 RID: 33159
			[SerializeField]
			public Image.Type _type;

			// Token: 0x04008188 RID: 33160
			[SerializeField]
			public bool _preserveAspect;

			// Token: 0x04008189 RID: 33161
			[SerializeField]
			public bool _fillCenter;

			// Token: 0x0400818A RID: 33162
			[SerializeField]
			public Image.FillMethod _fillMethod;

			// Token: 0x0400818B RID: 33163
			[SerializeField]
			public float _fillAmout;

			// Token: 0x0400818C RID: 33164
			[SerializeField]
			public bool _fillClockwise;

			// Token: 0x0400818D RID: 33165
			[SerializeField]
			public int _fillOrigin;
		}

		// Token: 0x020012DD RID: 4829
		[Serializable]
		public struct CustomColorBlock
		{
			// Token: 0x170019B7 RID: 6583
			// (get) Token: 0x0600836C RID: 33644 RVA: 0x000579BE File Offset: 0x00055BBE
			// (set) Token: 0x0600836D RID: 33645 RVA: 0x000579C6 File Offset: 0x00055BC6
			public float colorMultiplier
			{
				get
				{
					return this.m_ColorMultiplier;
				}
				set
				{
					this.m_ColorMultiplier = value;
				}
			}

			// Token: 0x170019B8 RID: 6584
			// (get) Token: 0x0600836E RID: 33646 RVA: 0x000579CF File Offset: 0x00055BCF
			// (set) Token: 0x0600836F RID: 33647 RVA: 0x000579D7 File Offset: 0x00055BD7
			public Color disabledColor
			{
				get
				{
					return this.m_DisabledColor;
				}
				set
				{
					this.m_DisabledColor = value;
				}
			}

			// Token: 0x170019B9 RID: 6585
			// (get) Token: 0x06008370 RID: 33648 RVA: 0x000579E0 File Offset: 0x00055BE0
			// (set) Token: 0x06008371 RID: 33649 RVA: 0x000579E8 File Offset: 0x00055BE8
			public float fadeDuration
			{
				get
				{
					return this.m_FadeDuration;
				}
				set
				{
					this.m_FadeDuration = value;
				}
			}

			// Token: 0x170019BA RID: 6586
			// (get) Token: 0x06008372 RID: 33650 RVA: 0x000579F1 File Offset: 0x00055BF1
			// (set) Token: 0x06008373 RID: 33651 RVA: 0x000579F9 File Offset: 0x00055BF9
			public Color highlightedColor
			{
				get
				{
					return this.m_HighlightedColor;
				}
				set
				{
					this.m_HighlightedColor = value;
				}
			}

			// Token: 0x170019BB RID: 6587
			// (get) Token: 0x06008374 RID: 33652 RVA: 0x00057A02 File Offset: 0x00055C02
			// (set) Token: 0x06008375 RID: 33653 RVA: 0x00057A0A File Offset: 0x00055C0A
			public Color normalColor
			{
				get
				{
					return this.m_NormalColor;
				}
				set
				{
					this.m_NormalColor = value;
				}
			}

			// Token: 0x170019BC RID: 6588
			// (get) Token: 0x06008376 RID: 33654 RVA: 0x00057A13 File Offset: 0x00055C13
			// (set) Token: 0x06008377 RID: 33655 RVA: 0x00057A1B File Offset: 0x00055C1B
			public Color pressedColor
			{
				get
				{
					return this.m_PressedColor;
				}
				set
				{
					this.m_PressedColor = value;
				}
			}

			// Token: 0x170019BD RID: 6589
			// (get) Token: 0x06008378 RID: 33656 RVA: 0x00057A24 File Offset: 0x00055C24
			// (set) Token: 0x06008379 RID: 33657 RVA: 0x00057A2C File Offset: 0x00055C2C
			public Color disabledHighlightedColor
			{
				get
				{
					return this.m_DisabledHighlightedColor;
				}
				set
				{
					this.m_DisabledHighlightedColor = value;
				}
			}

			// Token: 0x0600837A RID: 33658 RVA: 0x0029EC08 File Offset: 0x0029CE08
			public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
			{
				ColorBlock result = default(ColorBlock);
				result.colorMultiplier = item.m_ColorMultiplier;
				result.disabledColor = item.m_DisabledColor;
				result.fadeDuration = item.m_FadeDuration;
				result.highlightedColor = item.m_HighlightedColor;
				result.normalColor = item.m_NormalColor;
				result.pressedColor = item.m_PressedColor;
				return result;
			}

			// Token: 0x0400818E RID: 33166
			[SerializeField]
			public float m_ColorMultiplier;

			// Token: 0x0400818F RID: 33167
			[SerializeField]
			public Color m_DisabledColor;

			// Token: 0x04008190 RID: 33168
			[SerializeField]
			public float m_FadeDuration;

			// Token: 0x04008191 RID: 33169
			[SerializeField]
			public Color m_HighlightedColor;

			// Token: 0x04008192 RID: 33170
			[SerializeField]
			public Color m_NormalColor;

			// Token: 0x04008193 RID: 33171
			[SerializeField]
			public Color m_PressedColor;

			// Token: 0x04008194 RID: 33172
			[SerializeField]
			public Color m_DisabledHighlightedColor;
		}

		// Token: 0x020012DE RID: 4830
		[Serializable]
		public struct CustomSpriteState
		{
			// Token: 0x170019BE RID: 6590
			// (get) Token: 0x0600837B RID: 33659 RVA: 0x00057A35 File Offset: 0x00055C35
			// (set) Token: 0x0600837C RID: 33660 RVA: 0x00057A3D File Offset: 0x00055C3D
			public Sprite disabledSprite
			{
				get
				{
					return this.m_DisabledSprite;
				}
				set
				{
					this.m_DisabledSprite = value;
				}
			}

			// Token: 0x170019BF RID: 6591
			// (get) Token: 0x0600837D RID: 33661 RVA: 0x00057A46 File Offset: 0x00055C46
			// (set) Token: 0x0600837E RID: 33662 RVA: 0x00057A4E File Offset: 0x00055C4E
			public Sprite highlightedSprite
			{
				get
				{
					return this.m_HighlightedSprite;
				}
				set
				{
					this.m_HighlightedSprite = value;
				}
			}

			// Token: 0x170019C0 RID: 6592
			// (get) Token: 0x0600837F RID: 33663 RVA: 0x00057A57 File Offset: 0x00055C57
			// (set) Token: 0x06008380 RID: 33664 RVA: 0x00057A5F File Offset: 0x00055C5F
			public Sprite pressedSprite
			{
				get
				{
					return this.m_PressedSprite;
				}
				set
				{
					this.m_PressedSprite = value;
				}
			}

			// Token: 0x170019C1 RID: 6593
			// (get) Token: 0x06008381 RID: 33665 RVA: 0x00057A68 File Offset: 0x00055C68
			// (set) Token: 0x06008382 RID: 33666 RVA: 0x00057A70 File Offset: 0x00055C70
			public Sprite disabledHighlightedSprite
			{
				get
				{
					return this.m_DisabledHighlightedSprite;
				}
				set
				{
					this.m_DisabledHighlightedSprite = value;
				}
			}

			// Token: 0x06008383 RID: 33667 RVA: 0x0029EC74 File Offset: 0x0029CE74
			public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
			{
				SpriteState result = default(SpriteState);
				result.disabledSprite = item.m_DisabledSprite;
				result.highlightedSprite = item.m_HighlightedSprite;
				result.pressedSprite = item.m_PressedSprite;
				return result;
			}

			// Token: 0x04008195 RID: 33173
			[SerializeField]
			public Sprite m_DisabledSprite;

			// Token: 0x04008196 RID: 33174
			[SerializeField]
			public Sprite m_HighlightedSprite;

			// Token: 0x04008197 RID: 33175
			[SerializeField]
			public Sprite m_PressedSprite;

			// Token: 0x04008198 RID: 33176
			[SerializeField]
			public Sprite m_DisabledHighlightedSprite;
		}

		// Token: 0x020012DF RID: 4831
		[Serializable]
		public class CustomAnimationTriggers
		{
			// Token: 0x06008384 RID: 33668 RVA: 0x00057A79 File Offset: 0x00055C79
			public CustomAnimationTriggers()
			{
				this.m_DisabledTrigger = string.Empty;
				this.m_HighlightedTrigger = string.Empty;
				this.m_NormalTrigger = string.Empty;
				this.m_PressedTrigger = string.Empty;
				this.m_DisabledHighlightedTrigger = string.Empty;
			}

			// Token: 0x170019C2 RID: 6594
			// (get) Token: 0x06008385 RID: 33669 RVA: 0x00057AB8 File Offset: 0x00055CB8
			// (set) Token: 0x06008386 RID: 33670 RVA: 0x00057AC0 File Offset: 0x00055CC0
			public string disabledTrigger
			{
				get
				{
					return this.m_DisabledTrigger;
				}
				set
				{
					this.m_DisabledTrigger = value;
				}
			}

			// Token: 0x170019C3 RID: 6595
			// (get) Token: 0x06008387 RID: 33671 RVA: 0x00057AC9 File Offset: 0x00055CC9
			// (set) Token: 0x06008388 RID: 33672 RVA: 0x00057AD1 File Offset: 0x00055CD1
			public string highlightedTrigger
			{
				get
				{
					return this.m_HighlightedTrigger;
				}
				set
				{
					this.m_HighlightedTrigger = value;
				}
			}

			// Token: 0x170019C4 RID: 6596
			// (get) Token: 0x06008389 RID: 33673 RVA: 0x00057ADA File Offset: 0x00055CDA
			// (set) Token: 0x0600838A RID: 33674 RVA: 0x00057AE2 File Offset: 0x00055CE2
			public string normalTrigger
			{
				get
				{
					return this.m_NormalTrigger;
				}
				set
				{
					this.m_NormalTrigger = value;
				}
			}

			// Token: 0x170019C5 RID: 6597
			// (get) Token: 0x0600838B RID: 33675 RVA: 0x00057AEB File Offset: 0x00055CEB
			// (set) Token: 0x0600838C RID: 33676 RVA: 0x00057AF3 File Offset: 0x00055CF3
			public string pressedTrigger
			{
				get
				{
					return this.m_PressedTrigger;
				}
				set
				{
					this.m_PressedTrigger = value;
				}
			}

			// Token: 0x170019C6 RID: 6598
			// (get) Token: 0x0600838D RID: 33677 RVA: 0x00057AFC File Offset: 0x00055CFC
			// (set) Token: 0x0600838E RID: 33678 RVA: 0x00057B04 File Offset: 0x00055D04
			public string disabledHighlightedTrigger
			{
				get
				{
					return this.m_DisabledHighlightedTrigger;
				}
				set
				{
					this.m_DisabledHighlightedTrigger = value;
				}
			}

			// Token: 0x0600838F RID: 33679 RVA: 0x0029ECB4 File Offset: 0x0029CEB4
			public static implicit operator AnimationTriggers(ThemeSettings.CustomAnimationTriggers item)
			{
				return new AnimationTriggers
				{
					disabledTrigger = item.m_DisabledTrigger,
					highlightedTrigger = item.m_HighlightedTrigger,
					normalTrigger = item.m_NormalTrigger,
					pressedTrigger = item.m_PressedTrigger
				};
			}

			// Token: 0x04008199 RID: 33177
			[SerializeField]
			public string m_DisabledTrigger;

			// Token: 0x0400819A RID: 33178
			[SerializeField]
			public string m_HighlightedTrigger;

			// Token: 0x0400819B RID: 33179
			[SerializeField]
			public string m_NormalTrigger;

			// Token: 0x0400819C RID: 33180
			[SerializeField]
			public string m_PressedTrigger;

			// Token: 0x0400819D RID: 33181
			[SerializeField]
			public string m_DisabledHighlightedTrigger;
		}

		// Token: 0x020012E0 RID: 4832
		[Serializable]
		public class TextSettings
		{
			// Token: 0x170019C7 RID: 6599
			// (get) Token: 0x06008391 RID: 33681 RVA: 0x00057B36 File Offset: 0x00055D36
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x170019C8 RID: 6600
			// (get) Token: 0x06008392 RID: 33682 RVA: 0x00057B3E File Offset: 0x00055D3E
			public FontLoader.FontType[] fontTypes
			{
				get
				{
					return this._fontTypes;
				}
			}

			// Token: 0x170019C9 RID: 6601
			// (get) Token: 0x06008393 RID: 33683 RVA: 0x00057B46 File Offset: 0x00055D46
			public ThemeSettings.FontStyleOverride[] style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x170019CA RID: 6602
			// (get) Token: 0x06008394 RID: 33684 RVA: 0x00057B4E File Offset: 0x00055D4E
			public float lineSpacing
			{
				get
				{
					return this._lineSpacing;
				}
			}

			// Token: 0x170019CB RID: 6603
			// (get) Token: 0x06008395 RID: 33685 RVA: 0x00057B56 File Offset: 0x00055D56
			public float sizeMultiplier
			{
				get
				{
					return this._sizeMultiplier;
				}
			}

			// Token: 0x170019CC RID: 6604
			// (get) Token: 0x06008396 RID: 33686 RVA: 0x00057B5E File Offset: 0x00055D5E
			public int overrideSize
			{
				get
				{
					return this._overrideSize;
				}
			}

			// Token: 0x0400819E RID: 33182
			[SerializeField]
			public Color _color = Color.white;

			// Token: 0x0400819F RID: 33183
			[SerializeField]
			public FontLoader.FontType[] _fontTypes;

			// Token: 0x040081A0 RID: 33184
			[SerializeField]
			public ThemeSettings.FontStyleOverride[] _style;

			// Token: 0x040081A1 RID: 33185
			[SerializeField]
			public float _lineSpacing = 1f;

			// Token: 0x040081A2 RID: 33186
			[SerializeField]
			public float _sizeMultiplier = 1f;

			// Token: 0x040081A3 RID: 33187
			[SerializeField]
			public int _overrideSize;
		}

		// Token: 0x020012E1 RID: 4833
		public enum FontStyleOverride
		{
			// Token: 0x040081A5 RID: 33189
			Default,
			// Token: 0x040081A6 RID: 33190
			Normal,
			// Token: 0x040081A7 RID: 33191
			Bold,
			// Token: 0x040081A8 RID: 33192
			Italic,
			// Token: 0x040081A9 RID: 33193
			BoldAndItalic
		}
	}
}
