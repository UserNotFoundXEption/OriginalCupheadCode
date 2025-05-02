using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200063F RID: 1599
	[AddComponentMenu("")]
	public class CustomToggle : Toggle, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x0003571C File Offset: 0x0003391C
		// (set) Token: 0x060042AC RID: 17068 RVA: 0x00035724 File Offset: 0x00033924
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

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x0003572D File Offset: 0x0003392D
		// (set) Token: 0x060042AE RID: 17070 RVA: 0x00035735 File Offset: 0x00033935
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

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060042AF RID: 17071 RVA: 0x0003573E File Offset: 0x0003393E
		// (set) Token: 0x060042B0 RID: 17072 RVA: 0x00035746 File Offset: 0x00033946
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

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x0003574F File Offset: 0x0003394F
		// (set) Token: 0x060042B2 RID: 17074 RVA: 0x00035757 File Offset: 0x00033957
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

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x00035760 File Offset: 0x00033960
		// (set) Token: 0x060042B4 RID: 17076 RVA: 0x00035768 File Offset: 0x00033968
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

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x00035771 File Offset: 0x00033971
		// (set) Token: 0x060042B6 RID: 17078 RVA: 0x00035779 File Offset: 0x00033979
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

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00035782 File Offset: 0x00033982
		// (set) Token: 0x060042B8 RID: 17080 RVA: 0x0003578A File Offset: 0x0003398A
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

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x00035793 File Offset: 0x00033993
		public bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x060042BA RID: 17082 RVA: 0x00137364 File Offset: 0x00135564
		// (remove) Token: 0x060042BB RID: 17083 RVA: 0x0013739C File Offset: 0x0013559C
		public event UnityAction _CancelEvent;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x060042BC RID: 17084 RVA: 0x0003579E File Offset: 0x0003399E
		// (remove) Token: 0x060042BD RID: 17085 RVA: 0x000357A7 File Offset: 0x000339A7
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

		// Token: 0x060042BE RID: 17086 RVA: 0x001373D4 File Offset: 0x001355D4
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00137430 File Offset: 0x00135630
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & 1) != null || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x0013748C File Offset: 0x0013568C
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x001374E8 File Offset: 0x001356E8
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & 2) != null || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Selectable.allSelectables, base.transform.rotation * Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x000357B0 File Offset: 0x000339B0
		public override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x000357E4 File Offset: 0x000339E4
		public override void Start()
		{
			base.Start();
			ControlMapper.OnPlayerChange += this.UpdateColors;
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x000357FD File Offset: 0x000339FD
		public override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.update_text_color_on_enable_cr());
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00035812 File Offset: 0x00033A12
		public override void OnDestroy()
		{
			ControlMapper.OnPlayerChange -= this.UpdateColors;
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x00137544 File Offset: 0x00135744
		public IEnumerator update_text_color_on_enable_cr()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateAssociatedImageColors(base.currentSelectionState);
			yield break;
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x00035825 File Offset: 0x00033A25
		public void UpdateColors()
		{
			this.UpdateAssociatedImageColors(base.currentSelectionState);
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x00035833 File Offset: 0x00033A33
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			this.UpdateColors();
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x00035842 File Offset: 0x00033A42
		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			this.UpdateColors();
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x00137560 File Offset: 0x00135760
		public void UpdateAssociatedImageColors(Selectable.SelectionState state)
		{
			if (state == 1)
			{
				this.checkImage.color = this.checkOverrideColor[(!base.isOn) ? 3 : 2];
			}
			else
			{
				this.checkImage.color = this.checkOverrideColor[(!base.isOn) ? 3 : ControlMapper.CurrentPlayer()];
			}
			this.checkBoxImage.color = this.checkBoxOverrideColor[state];
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x001375F4 File Offset: 0x001357F4
		public override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			this.UpdateAssociatedImageColors(state);
			ColorBlock colors = base.colors;
			colors.normalColor = new Color(base.colors.normalColor.r, base.colors.normalColor.g, base.colors.normalColor.b, 0f);
			base.colors = colors;
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

		// Token: 0x060042CC RID: 17100 RVA: 0x00137710 File Offset: 0x00135910
		public void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, (!instant) ? base.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x00035851 File Offset: 0x00033A51
		public void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x0013775C File Offset: 0x0013595C
		public void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00035871 File Offset: 0x00033A71
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00035881 File Offset: 0x00033A81
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x001377DC File Offset: 0x001359DC
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

		// Token: 0x060042D2 RID: 17106 RVA: 0x00035891 File Offset: 0x00033A91
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent.Invoke();
			}
		}

		// Token: 0x04003459 RID: 13401
		[SerializeField]
		public Image checkImage;

		// Token: 0x0400345A RID: 13402
		[SerializeField]
		public Image checkBoxImage;

		// Token: 0x0400345B RID: 13403
		[SerializeField]
		public Color[] checkOverrideColor;

		// Token: 0x0400345C RID: 13404
		[SerializeField]
		public Color[] checkBoxOverrideColor;

		// Token: 0x0400345D RID: 13405
		[SerializeField]
		public Sprite _disabledHighlightedSprite;

		// Token: 0x0400345E RID: 13406
		[SerializeField]
		public Color _disabledHighlightedColor;

		// Token: 0x0400345F RID: 13407
		[SerializeField]
		public string _disabledHighlightedTrigger;

		// Token: 0x04003460 RID: 13408
		[SerializeField]
		public bool _autoNavUp = true;

		// Token: 0x04003461 RID: 13409
		[SerializeField]
		public bool _autoNavDown = true;

		// Token: 0x04003462 RID: 13410
		[SerializeField]
		public bool _autoNavLeft = true;

		// Token: 0x04003463 RID: 13411
		[SerializeField]
		public bool _autoNavRight = true;

		// Token: 0x04003464 RID: 13412
		public bool isHighlightDisabled;
	}
}
