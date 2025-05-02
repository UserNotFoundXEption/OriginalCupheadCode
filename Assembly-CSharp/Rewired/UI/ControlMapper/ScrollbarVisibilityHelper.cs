using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000645 RID: 1605
	[AddComponentMenu("")]
	public class ScrollbarVisibilityHelper : MonoBehaviour
	{
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600433F RID: 17215 RVA: 0x00035A1B File Offset: 0x00033C1B
		public Scrollbar hScrollBar
		{
			get
			{
				return (!(this.scrollRect != null)) ? null : this.scrollRect.horizontalScrollbar;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06004340 RID: 17216 RVA: 0x00035A3F File Offset: 0x00033C3F
		public Scrollbar vScrollBar
		{
			get
			{
				return (!(this.scrollRect != null)) ? null : this.scrollRect.verticalScrollbar;
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0013881C File Offset: 0x00136A1C
		public void Awake()
		{
			if (this.scrollRect != null)
			{
				this.target = this.scrollRect.gameObject.AddComponent<ScrollbarVisibilityHelper>();
				this.target.onlySendMessage = true;
				this.target.target = this;
			}
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00035A63 File Offset: 0x00033C63
		public void OnRectTransformDimensionsChange()
		{
			if (this.onlySendMessage)
			{
				if (this.target != null)
				{
					this.target.ScrollRectTransformDimensionsChanged();
				}
			}
			else
			{
				this.EvaluateScrollbar();
			}
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00035A97 File Offset: 0x00033C97
		public void ScrollRectTransformDimensionsChanged()
		{
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00138868 File Offset: 0x00136A68
		public void EvaluateScrollbar()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			if (this.vScrollBar == null && this.hScrollBar == null)
			{
				return;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Rect rect = this.scrollRect.content.rect;
			Rect rect2 = (this.scrollRect.transform as RectTransform).rect;
			if (this.vScrollBar != null)
			{
				bool value = rect.height > rect2.height;
				this.SetActiveDeferred(this.vScrollBar.gameObject, value);
			}
			if (this.hScrollBar != null)
			{
				bool value2 = rect.width > rect2.width;
				this.SetActiveDeferred(this.hScrollBar.gameObject, value2);
			}
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00035A9F File Offset: 0x00033C9F
		public void SetActiveDeferred(GameObject obj, bool value)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.SetActiveCoroutine(obj, value));
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00138960 File Offset: 0x00136B60
		public IEnumerator SetActiveCoroutine(GameObject obj, bool value)
		{
			yield return null;
			if (obj != null)
			{
				obj.SetActive(value);
			}
			yield break;
		}

		// Token: 0x040034B6 RID: 13494
		public ScrollRect scrollRect;

		// Token: 0x040034B7 RID: 13495
		public bool onlySendMessage;

		// Token: 0x040034B8 RID: 13496
		public ScrollbarVisibilityHelper target;
	}
}
