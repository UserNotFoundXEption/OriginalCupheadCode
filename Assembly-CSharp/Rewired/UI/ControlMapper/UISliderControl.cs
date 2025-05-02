using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000650 RID: 1616
	[AddComponentMenu("")]
	public class UISliderControl : UIControl
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06004376 RID: 17270 RVA: 0x00035CEC File Offset: 0x00033EEC
		// (set) Token: 0x06004377 RID: 17271 RVA: 0x00035CF4 File Offset: 0x00033EF4
		public bool showIcon
		{
			get
			{
				return this._showIcon;
			}
			set
			{
				if (this.iconImage == null)
				{
					return;
				}
				this.iconImage.gameObject.SetActive(value);
				this._showIcon = value;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06004378 RID: 17272 RVA: 0x00035D20 File Offset: 0x00033F20
		// (set) Token: 0x06004379 RID: 17273 RVA: 0x00035D28 File Offset: 0x00033F28
		public bool showSlider
		{
			get
			{
				return this._showSlider;
			}
			set
			{
				if (this.slider == null)
				{
					return;
				}
				this.slider.gameObject.SetActive(value);
				this._showSlider = value;
			}
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x00139650 File Offset: 0x00137850
		public override void SetCancelCallback(Action cancelCallback)
		{
			base.SetCancelCallback(cancelCallback);
			if (cancelCallback == null || this.slider == null)
			{
				return;
			}
			if (this.slider is ICustomSelectable)
			{
				(this.slider as ICustomSelectable).CancelEvent += delegate
				{
					cancelCallback();
				};
			}
			else
			{
				EventTrigger eventTrigger = this.slider.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.slider.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.callback = new EventTrigger.TriggerEvent();
				entry.eventID = 16;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					cancelCallback();
				});
				if (eventTrigger.triggers == null)
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(entry);
			}
		}

		// Token: 0x040034E8 RID: 13544
		public Image iconImage;

		// Token: 0x040034E9 RID: 13545
		public Slider slider;

		// Token: 0x040034EA RID: 13546
		public bool _showIcon;

		// Token: 0x040034EB RID: 13547
		public bool _showSlider;
	}
}
