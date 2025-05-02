using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200063B RID: 1595
	[AddComponentMenu("")]
	public class CustomButton : Button, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06004258 RID: 16984 RVA: 0x000353BC File Offset: 0x000335BC
		// (set) Token: 0x06004259 RID: 16985 RVA: 0x000353C4 File Offset: 0x000335C4
		public Sprite disabledHighlightedSprite
		{
			get
			{
				return this._disabledHighlightedSprite;
			}
			set
			{
				this._disabledHighlightedSprite = value;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600425A RID: 16986 RVA: 0x000353CD File Offset: 0x000335CD
		// (set) Token: 0x0600425B RID: 16987 RVA: 0x000353D5 File Offset: 0x000335D5
		public Color disabledHighlightedColor
		{
			get
			{
				return this._disabledHighlightedColor;
			}
			set
			{
				this._disabledHighlightedColor = value;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x000353DE File Offset: 0x000335DE
		// (set) Token: 0x0600425D RID: 16989 RVA: 0x000353E6 File Offset: 0x000335E6
		public string disabledHighlightedTrigger
		{
			get
			{
				return this._disabledHighlightedTrigger;
			}
			set
			{
				this._disabledHighlightedTrigger = value;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x000353EF File Offset: 0x000335EF
		// (set) Token: 0x0600425F RID: 16991 RVA: 0x000353F7 File Offset: 0x000335F7
		public bool autoNavUp
		{
			get
			{
				return this._autoNavUp;
			}
			set
			{
				this._autoNavUp = value;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x00035400 File Offset: 0x00033600
		// (set) Token: 0x06004261 RID: 16993 RVA: 0x00035408 File Offset: 0x00033608
		public bool autoNavDown
		{
			get
			{
				return this._autoNavDown;
			}
			set
			{
				this._autoNavDown = value;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06004262 RID: 16994 RVA: 0x00035411 File Offset: 0x00033611
		// (set) Token: 0x06004263 RID: 16995 RVA: 0x00035419 File Offset: 0x00033619
		public bool autoNavLeft
		{
			get
			{
				return this._autoNavLeft;
			}
			set
			{
				this._autoNavLeft = value;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x00035422 File Offset: 0x00033622
		// (set) Token: 0x06004265 RID: 16997 RVA: 0x0003542A File Offset: 0x0003362A
		public bool autoNavRight
		{
			get
			{
				return this._autoNavRight;
			}
			set
			{
				this._autoNavRight = value;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x00035433 File Offset: 0x00033633
		public bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06004267 RID: 16999 RVA: 0x00136880 File Offset: 0x00134A80
		// (remove) Token: 0x06004268 RID: 17000 RVA: 0x001368B8 File Offset: 0x00134AB8
		public event UnityAction _CancelEvent;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06004269 RID: 17001 RVA: 0x0003543E File Offset: 0x0003363E
		// (remove) Token: 0x0600426A RID: 17002 RVA: 0x00035447 File Offset: 0x00033647
		public event UnityAction CancelEvent
		{
			add
			{
				this._CancelEvent += value;
			}
			remove
			{
				this._CancelEvent -= value;
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x001368F0 File Offset: 0x00134AF0
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0013694C File Offset: 0x00134B4C
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x001369A8 File Offset: 0x00134BA8
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x00136A04 File Offset: 0x00134C04
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00035450 File Offset: 0x00033650
		public override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00035484 File Offset: 0x00033684
		public override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.update_text_color_on_enable_cr());
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x00136A60 File Offset: 0x00134C60
		public IEnumerator update_text_color_on_enable_cr()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateAssociatedTextColor(base.currentSelectionState);
			yield break;
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x00136A7C File Offset: 0x00134C7C
		public void UpdateAssociatedTextColor(Selectable.SelectionState state)
		{
			if (this.associatedText)
			{
				if (this.textOverrideColor.Length == 4)
				{
					this.associatedText.color = this.textOverrideColor[state];
				}
				else
				{
					switch (state)
					{
					case 0:
						this.associatedText.color = base.colors.normalColor;
						break;
					case 1:
						this.associatedText.color = base.colors.highlightedColor;
						break;
					case 2:
						this.associatedText.color = base.colors.pressedColor;
						break;
					case 3:
						this.associatedText.color = base.colors.disabledColor;
						break;
					}
				}
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x00136B60 File Offset: 0x00134D60
		public override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			this.UpdateAssociatedTextColor(state);
			if (this.isHighlightDisabled)
			{
				Color disabledHighlightedColor = this._disabledHighlightedColor;
				Sprite disabledHighlightedSprite = this._disabledHighlightedSprite;
				string disabledHighlightedTrigger = this._disabledHighlightedTrigger;
				if (base.gameObject.activeInHierarchy)
				{
					Selectable.Transition transition = base.transition;
					if (transition != 1)
					{
						if (transition != 2)
						{
							if (transition == 3)
							{
								this.TriggerAnimation(disabledHighlightedTrigger);
							}
						}
						else
						{
							this.DoSpriteSwap((state != 1) ? disabledHighlightedSprite : base.spriteState.highlightedSprite);
						}
					}
					else
					{
						this.StartColorTween(disabledHighlightedColor * base.colors.colorMultiplier, instant);
					}
				}
			}
			else
			{
				base.DoStateTransition(state, instant);
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00136C28 File Offset: 0x00134E28
		public void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x00035499 File Offset: 0x00033699
		public void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x00136C74 File Offset: 0x00134E74
		public void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x000354B9 File Offset: 0x000336B9
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			AudioManager.Play("level_menu_move");
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x000354D3 File Offset: 0x000336D3
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00136CF4 File Offset: 0x00134EF4
		public void SetNavOnToggle(bool setting)
		{
			Navigation navigation = default(Navigation);
			navigation.mode = ((!setting) ? 0 : 3);
			base.navigation = navigation;
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x000354E3 File Offset: 0x000336E3
		public void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			base.onClick.Invoke();
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x00136D24 File Offset: 0x00134F24
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(3, false);
			}
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x00136D80 File Offset: 0x00134F80
		public override void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(3, false);
				return;
			}
			this.DoStateTransition(2, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x00136DD0 File Offset: 0x00134FD0
		public IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x00136DEC File Offset: 0x00134FEC
		public void EvaluateHightlightDisabled(bool isSelected)
		{
			if (!isSelected)
			{
				if (this.isHighlightDisabled)
				{
					this.isHighlightDisabled = false;
					Selectable.SelectionState selectionState = (!this.isDisabled) ? base.currentSelectionState : 3;
					this.DoStateTransition(selectionState, false);
				}
			}
			else
			{
				if (!this.isDisabled)
				{
					return;
				}
				this.isHighlightDisabled = true;
				this.DoStateTransition(3, false);
			}
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x00035507 File Offset: 0x00033707
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent.Invoke();
			}
		}

		// Token: 0x0400343D RID: 13373
		[SerializeField]
		public Text associatedText;

		// Token: 0x0400343E RID: 13374
		[SerializeField]
		public Color[] textOverrideColor;

		// Token: 0x0400343F RID: 13375
		[SerializeField]
		public Sprite _disabledHighlightedSprite;

		// Token: 0x04003440 RID: 13376
		[SerializeField]
		public Color _disabledHighlightedColor;

		// Token: 0x04003441 RID: 13377
		[SerializeField]
		public string _disabledHighlightedTrigger;

		// Token: 0x04003442 RID: 13378
		[SerializeField]
		public bool _autoNavUp = true;

		// Token: 0x04003443 RID: 13379
		[SerializeField]
		public bool _autoNavDown = true;

		// Token: 0x04003444 RID: 13380
		[SerializeField]
		public bool _autoNavLeft = true;

		// Token: 0x04003445 RID: 13381
		[SerializeField]
		public bool _autoNavRight = true;

		// Token: 0x04003446 RID: 13382
		public bool isHighlightDisabled;
	}
}
