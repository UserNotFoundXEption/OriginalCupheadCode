using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000646 RID: 1606
	[AddComponentMenu("")]
	[RequireComponent(typeof(Selectable))]
	public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06004348 RID: 17224 RVA: 0x00035AC9 File Offset: 0x00033CC9
		public RectTransform parentScrollRectContentTransform
		{
			get
			{
				return this.parentScrollRect.content;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06004349 RID: 17225 RVA: 0x00138984 File Offset: 0x00136B84
		public Selectable selectable
		{
			get
			{
				Selectable result;
				if ((result = this._selectable) == null)
				{
					result = (this._selectable = base.GetComponent<Selectable>());
				}
				return result;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600434A RID: 17226 RVA: 0x00035AD6 File Offset: 0x00033CD6
		public RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x00035AE3 File Offset: 0x00033CE3
		public void Start()
		{
			this.parentScrollRect = base.transform.GetComponentInParent<ScrollRect>();
			if (this.parentScrollRect == null)
			{
				Debug.LogError("Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
				return;
			}
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x001389B0 File Offset: 0x00136BB0
		public void OnSelect(BaseEventData eventData)
		{
			if (this.parentScrollRect == null)
			{
				return;
			}
			if (!(eventData is AxisEventData))
			{
				return;
			}
			RectTransform rectTransform = this.parentScrollRect.transform as RectTransform;
			Rect rect = MathTools.TransformRect(this.rectTransform.rect, this.rectTransform, rectTransform);
			Rect rect2 = rectTransform.rect;
			Rect rect3 = rectTransform.rect;
			float height;
			if (this.useCustomEdgePadding)
			{
				height = this.customEdgePadding;
			}
			else
			{
				height = rect.height;
			}
			rect3.yMax -= height;
			rect3.yMin += height;
			if (MathTools.RectContains(rect3, rect))
			{
				return;
			}
			Vector2 vector;
			if (!MathTools.GetOffsetToContainRect(rect3, rect, ref vector))
			{
				return;
			}
			Vector2 anchoredPosition = this.parentScrollRectContentTransform.anchoredPosition;
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + vector.x, 0f, Mathf.Abs(rect2.width - this.parentScrollRectContentTransform.sizeDelta.x));
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + vector.y, 0f, Mathf.Abs(rect2.height - this.parentScrollRectContentTransform.sizeDelta.y));
			this.parentScrollRectContentTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x040034B9 RID: 13497
		public bool useCustomEdgePadding;

		// Token: 0x040034BA RID: 13498
		public float customEdgePadding = 50f;

		// Token: 0x040034BB RID: 13499
		public ScrollRect parentScrollRect;

		// Token: 0x040034BC RID: 13500
		public Selectable _selectable;
	}
}
