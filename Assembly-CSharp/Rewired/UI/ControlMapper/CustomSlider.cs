using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200063E RID: 1598
	[AddComponentMenu("")]
	public class CustomSlider : Slider, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600428A RID: 17034 RVA: 0x000355D8 File Offset: 0x000337D8
		// (set) Token: 0x0600428B RID: 17035 RVA: 0x000355E0 File Offset: 0x000337E0
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

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600428C RID: 17036 RVA: 0x000355E9 File Offset: 0x000337E9
		// (set) Token: 0x0600428D RID: 17037 RVA: 0x000355F1 File Offset: 0x000337F1
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

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x000355FA File Offset: 0x000337FA
		// (set) Token: 0x0600428F RID: 17039 RVA: 0x00035602 File Offset: 0x00033802
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

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06004290 RID: 17040 RVA: 0x0003560B File Offset: 0x0003380B
		// (set) Token: 0x06004291 RID: 17041 RVA: 0x00035613 File Offset: 0x00033813
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

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x0003561C File Offset: 0x0003381C
		// (set) Token: 0x06004293 RID: 17043 RVA: 0x00035624 File Offset: 0x00033824
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

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06004294 RID: 17044 RVA: 0x0003562D File Offset: 0x0003382D
		// (set) Token: 0x06004295 RID: 17045 RVA: 0x00035635 File Offset: 0x00033835
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

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x0003563E File Offset: 0x0003383E
		// (set) Token: 0x06004297 RID: 17047 RVA: 0x00035646 File Offset: 0x00033846
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

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06004298 RID: 17048 RVA: 0x0003564F File Offset: 0x0003384F
		public bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06004299 RID: 17049 RVA: 0x00136FAC File Offset: 0x001351AC
		// (remove) Token: 0x0600429A RID: 17050 RVA: 0x00136FE4 File Offset: 0x001351E4
		public event UnityAction _CancelEvent;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x0600429B RID: 17051 RVA: 0x0003565A File Offset: 0x0003385A
		// (remove) Token: 0x0600429C RID: 17052 RVA: 0x00035663 File Offset: 0x00033863
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

		// Token: 0x0600429D RID: 17053 RVA: 0x0013701C File Offset: 0x0013521C
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x00137078 File Offset: 0x00135278
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x001370D4 File Offset: 0x001352D4
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00137130 File Offset: 0x00135330
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x0003566C File Offset: 0x0003386C
		public override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x0013718C File Offset: 0x0013538C
		public override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
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
							this.DoSpriteSwap(disabledHighlightedSprite);
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

		// Token: 0x060042A3 RID: 17059 RVA: 0x00137230 File Offset: 0x00135430
		public void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x000356A0 File Offset: 0x000338A0
		public void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x0013727C File Offset: 0x0013547C
		public void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x000356C0 File Offset: 0x000338C0
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x000356D0 File Offset: 0x000338D0
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x001372FC File Offset: 0x001354FC
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

		// Token: 0x060042A9 RID: 17065 RVA: 0x000356E0 File Offset: 0x000338E0
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent.Invoke();
			}
		}

		// Token: 0x04003450 RID: 13392
		[SerializeField]
		public Sprite _disabledHighlightedSprite;

		// Token: 0x04003451 RID: 13393
		[SerializeField]
		public Color _disabledHighlightedColor;

		// Token: 0x04003452 RID: 13394
		[SerializeField]
		public string _disabledHighlightedTrigger;

		// Token: 0x04003453 RID: 13395
		[SerializeField]
		public bool _autoNavUp = true;

		// Token: 0x04003454 RID: 13396
		[SerializeField]
		public bool _autoNavDown = true;

		// Token: 0x04003455 RID: 13397
		[SerializeField]
		public bool _autoNavLeft = true;

		// Token: 0x04003456 RID: 13398
		[SerializeField]
		public bool _autoNavRight = true;

		// Token: 0x04003457 RID: 13399
		public bool isHighlightDisabled;
	}
}
