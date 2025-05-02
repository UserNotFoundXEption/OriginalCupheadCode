using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200065F RID: 1631
	[ExecuteInEditMode]
	public class InlineGraphicManager : MonoBehaviour
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060044BD RID: 17597 RVA: 0x00036965 File Offset: 0x00034B65
		// (set) Token: 0x060044BE RID: 17598 RVA: 0x0003696D File Offset: 0x00034B6D
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.LoadSpriteAsset(value);
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x00036976 File Offset: 0x00034B76
		// (set) Token: 0x060044C0 RID: 17600 RVA: 0x0003697E File Offset: 0x00034B7E
		public InlineGraphic inlineGraphic
		{
			get
			{
				return this.m_inlineGraphic;
			}
			set
			{
				if (this.m_inlineGraphic != value)
				{
					this.m_inlineGraphic = value;
				}
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x00036998 File Offset: 0x00034B98
		public CanvasRenderer canvasRenderer
		{
			get
			{
				return this.m_inlineGraphicCanvasRenderer;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x000369A0 File Offset: 0x00034BA0
		public UIVertex[] uiVertex
		{
			get
			{
				return this.m_uiVertex;
			}
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x000369A8 File Offset: 0x00034BA8
		public void Awake()
		{
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x000369AA File Offset: 0x00034BAA
		public void OnEnable()
		{
			base.enabled = false;
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x000369B3 File Offset: 0x00034BB3
		public void OnDisable()
		{
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x000369B5 File Offset: 0x00034BB5
		public void OnDestroy()
		{
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0013D7E4 File Offset: 0x0013B9E4
		public void LoadSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			if (spriteAsset == null)
			{
				if (TMP_Settings.defaultSpriteAsset != null)
				{
					spriteAsset = TMP_Settings.defaultSpriteAsset;
				}
				else
				{
					spriteAsset = (Resources.Load("Sprite Assets/Default Sprite Asset") as TMP_SpriteAsset);
				}
			}
			this.m_spriteAsset = spriteAsset;
			this.m_inlineGraphic.texture = this.m_spriteAsset.spriteSheet;
			if (this.m_textComponent != null && this.m_isInitialized)
			{
				this.m_textComponent.havePropertiesChanged = true;
				this.m_textComponent.SetVerticesDirty();
			}
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0013D87C File Offset: 0x0013BA7C
		public void AddInlineGraphicsChild()
		{
			if (this.m_inlineGraphic != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Inline Graphic");
			this.m_inlineGraphic = gameObject.AddComponent<InlineGraphic>();
			this.m_inlineGraphicRectTransform = gameObject.GetComponent<RectTransform>();
			this.m_inlineGraphicCanvasRenderer = gameObject.GetComponent<CanvasRenderer>();
			this.m_inlineGraphicRectTransform.SetParent(base.transform, false);
			this.m_inlineGraphicRectTransform.localPosition = Vector3.zero;
			this.m_inlineGraphicRectTransform.anchoredPosition3D = Vector3.zero;
			this.m_inlineGraphicRectTransform.sizeDelta = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMin = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMax = Vector2.one;
			this.m_textComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0013D938 File Offset: 0x0013BB38
		public void AllocatedVertexBuffers(int size)
		{
			if (this.m_inlineGraphic == null)
			{
				this.AddInlineGraphicsChild();
				this.LoadSpriteAsset(this.m_spriteAsset);
			}
			if (this.m_uiVertex == null)
			{
				this.m_uiVertex = new UIVertex[4];
			}
			int num = size * 4;
			if (num > this.m_uiVertex.Length)
			{
				this.m_uiVertex = new UIVertex[Mathf.NextPowerOfTwo(num)];
			}
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x000369B7 File Offset: 0x00034BB7
		public void UpdatePivot(Vector2 pivot)
		{
			if (this.m_inlineGraphicRectTransform == null)
			{
				this.m_inlineGraphicRectTransform = this.m_inlineGraphic.GetComponent<RectTransform>();
			}
			this.m_inlineGraphicRectTransform.pivot = pivot;
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x000369E7 File Offset: 0x00034BE7
		public void ClearUIVertex()
		{
			if (this.uiVertex != null && this.uiVertex.Length > 0)
			{
				Array.Clear(this.uiVertex, 0, this.uiVertex.Length);
				this.m_inlineGraphicCanvasRenderer.Clear();
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00036A21 File Offset: 0x00034C21
		public void DrawSprite(UIVertex[] uiVertices, int spriteCount)
		{
			if (this.m_inlineGraphicCanvasRenderer == null)
			{
				this.m_inlineGraphicCanvasRenderer = this.m_inlineGraphic.GetComponent<CanvasRenderer>();
			}
			this.m_inlineGraphicCanvasRenderer.SetVertices(uiVertices, spriteCount * 4);
			this.m_inlineGraphic.UpdateMaterial();
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0013D9A4 File Offset: 0x0013BBA4
		public TMP_Sprite GetSprite(int index)
		{
			if (this.m_spriteAsset == null)
			{
				return null;
			}
			if (this.m_spriteAsset.spriteInfoList == null || index > this.m_spriteAsset.spriteInfoList.Count - 1)
			{
				return null;
			}
			return this.m_spriteAsset.spriteInfoList[index];
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x0013DA00 File Offset: 0x0013BC00
		public int GetSpriteIndexByHashCode(int hashCode)
		{
			if (this.m_spriteAsset == null || this.m_spriteAsset.spriteInfoList == null)
			{
				return -1;
			}
			return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.hashCode == hashCode);
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x0013DA5C File Offset: 0x0013BC5C
		public int GetSpriteIndexByIndex(int index)
		{
			if (this.m_spriteAsset == null || this.m_spriteAsset.spriteInfoList == null)
			{
				return -1;
			}
			return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.id == index);
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x00036A5F File Offset: 0x00034C5F
		public void SetUIVertex(UIVertex[] uiVertex)
		{
			this.m_uiVertex = uiVertex;
		}

		// Token: 0x04003538 RID: 13624
		[SerializeField]
		public TMP_SpriteAsset m_spriteAsset;

		// Token: 0x04003539 RID: 13625
		[SerializeField]
		[HideInInspector]
		public InlineGraphic m_inlineGraphic;

		// Token: 0x0400353A RID: 13626
		[SerializeField]
		[HideInInspector]
		public CanvasRenderer m_inlineGraphicCanvasRenderer;

		// Token: 0x0400353B RID: 13627
		public UIVertex[] m_uiVertex;

		// Token: 0x0400353C RID: 13628
		public RectTransform m_inlineGraphicRectTransform;

		// Token: 0x0400353D RID: 13629
		public TMP_Text m_textComponent;

		// Token: 0x0400353E RID: 13630
		public bool m_isInitialized;
	}
}
