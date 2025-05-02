using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x0200065E RID: 1630
	public class InlineGraphic : MaskableGraphic
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x0003691C File Offset: 0x00034B1C
		public override Texture mainTexture
		{
			get
			{
				if (this.texture == null)
				{
					return Graphic.s_WhiteTexture;
				}
				return this.texture;
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0003693B File Offset: 0x00034B3B
		public override void Awake()
		{
			this.m_manager = base.GetComponentInParent<InlineGraphicManager>();
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0013D6EC File Offset: 0x0013B8EC
		public override void OnEnable()
		{
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_manager != null && this.m_manager.spriteAsset != null)
			{
				this.texture = this.m_manager.spriteAsset.spriteSheet;
			}
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00036949 File Offset: 0x00034B49
		public override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x00036951 File Offset: 0x00034B51
		public override void OnTransformParentChanged()
		{
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0013D758 File Offset: 0x0013B958
		public override void OnRectTransformDimensionsChange()
		{
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_ParentRectTransform == null)
			{
				this.m_ParentRectTransform = this.m_RectTransform.parent.GetComponent<RectTransform>();
			}
			if (this.m_RectTransform.pivot != this.m_ParentRectTransform.pivot)
			{
				this.m_RectTransform.pivot = this.m_ParentRectTransform.pivot;
			}
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x00036953 File Offset: 0x00034B53
		public void UpdateMaterial()
		{
			base.UpdateMaterial();
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0003695B File Offset: 0x00034B5B
		public override void UpdateGeometry()
		{
		}

		// Token: 0x04003534 RID: 13620
		public Texture texture;

		// Token: 0x04003535 RID: 13621
		public InlineGraphicManager m_manager;

		// Token: 0x04003536 RID: 13622
		public RectTransform m_RectTransform;

		// Token: 0x04003537 RID: 13623
		public RectTransform m_ParentRectTransform;
	}
}
