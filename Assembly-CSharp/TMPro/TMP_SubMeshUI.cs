using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000673 RID: 1651
	[ExecuteInEditMode]
	public class TMP_SubMeshUI : MaskableGraphic, ITextElement, IClippable, IMaskable, IMaterialModifier
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x00037A43 File Offset: 0x00035C43
		// (set) Token: 0x06004606 RID: 17926 RVA: 0x00037A4B File Offset: 0x00035C4B
		public TMP_FontAsset fontAsset
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				this.m_fontAsset = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06004607 RID: 17927 RVA: 0x00037A54 File Offset: 0x00035C54
		// (set) Token: 0x06004608 RID: 17928 RVA: 0x00037A5C File Offset: 0x00035C5C
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x00037A65 File Offset: 0x00035C65
		public override Texture mainTexture
		{
			get
			{
				if (this.sharedMaterial != null)
				{
					return this.sharedMaterial.mainTexture;
				}
				return null;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x00037A85 File Offset: 0x00035C85
		// (set) Token: 0x0600460B RID: 17931 RVA: 0x00037A93 File Offset: 0x00035C93
		public override Material material
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
				{
					return;
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600460C RID: 17932 RVA: 0x00037ACB File Offset: 0x00035CCB
		// (set) Token: 0x0600460D RID: 17933 RVA: 0x00037AD3 File Offset: 0x00035CD3
		public Material sharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				this.SetSharedMaterial(value);
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x00037ADC File Offset: 0x00035CDC
		public override Material materialForRendering
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return null;
				}
				return this.GetModifiedMaterial(this.m_sharedMaterial);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600460F RID: 17935 RVA: 0x00037AFD File Offset: 0x00035CFD
		// (set) Token: 0x06004610 RID: 17936 RVA: 0x00037B05 File Offset: 0x00035D05
		public bool isDefaultMaterial
		{
			get
			{
				return this.m_isDefaultMaterial;
			}
			set
			{
				this.m_isDefaultMaterial = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06004611 RID: 17937 RVA: 0x00037B0E File Offset: 0x00035D0E
		// (set) Token: 0x06004612 RID: 17938 RVA: 0x00037B16 File Offset: 0x00035D16
		public float padding
		{
			get
			{
				return this.m_padding;
			}
			set
			{
				this.m_padding = value;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x00037B1F File Offset: 0x00035D1F
		public CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_canvasRenderer == null)
				{
					this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
				}
				return this.m_canvasRenderer;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x00037B44 File Offset: 0x00035D44
		// (set) Token: 0x06004615 RID: 17941 RVA: 0x00037B75 File Offset: 0x00035D75
		public Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = 61;
				}
				return this.m_mesh;
			}
			set
			{
				this.m_mesh = value;
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x0014F18C File Offset: 0x0014D38C
		public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject("TMP UI SubObject [" + materialReference.material.name + "]");
			gameObject.layer = textComponent.gameObject.layer;
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.pivot = textComponent.rectTransform.pivot;
			TMP_SubMeshUI tmp_SubMeshUI = gameObject.AddComponent<TMP_SubMeshUI>();
			tmp_SubMeshUI.m_canvasRenderer = tmp_SubMeshUI.canvasRenderer;
			tmp_SubMeshUI.m_TextComponent = textComponent;
			tmp_SubMeshUI.m_materialReferenceIndex = materialReference.index;
			tmp_SubMeshUI.m_fontAsset = materialReference.fontAsset;
			tmp_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
			tmp_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tmp_SubMeshUI.SetSharedMaterial(materialReference.material);
			gameObject.transform.SetParent(textComponent.transform, false);
			return tmp_SubMeshUI;
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x00037B7E File Offset: 0x00035D7E
		public override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x00037BA5 File Offset: 0x00035DA5
		public override void OnDisable()
		{
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			base.OnDisable();
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0014F274 File Offset: 0x0014D474
		public override void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				Object.DestroyImmediate(this.m_mesh);
			}
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
			}
			this.m_isRegisteredForEvents = false;
			this.RecalculateClipping();
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x00037BD6 File Offset: 0x00035DD6
		public override void OnTransformParentChanged()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x0014F2C8 File Offset: 0x0014D4C8
		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
			{
				this.m_StencilValue = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_StencilValue > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_StencilValue);
				if (this.m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x0014F338 File Offset: 0x0014D538
		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0014F368 File Offset: 0x0014D568
		public float GetPaddingForMaterial(Material mat)
		{
			return ShaderUtilities.GetPadding(mat, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x00037BF7 File Offset: 0x00035DF7
		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x00037C0C File Offset: 0x00035E0C
		public override void SetAllDirty()
		{
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x00037C0E File Offset: 0x00035E0E
		public override void SetVerticesDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.havePropertiesChanged = true;
				this.m_TextComponent.SetVerticesDirty();
			}
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x00037C44 File Offset: 0x00035E44
		public override void SetLayoutDirty()
		{
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00037C46 File Offset: 0x00035E46
		public override void SetMaterialDirty()
		{
			this.m_materialDirty = true;
			this.UpdateMaterial();
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00037C55 File Offset: 0x00035E55
		public void SetPivotDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			base.rectTransform.pivot = this.m_TextComponent.rectTransform.pivot;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x00037C7E File Offset: 0x00035E7E
		public override void UpdateGeometry()
		{
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x00037C80 File Offset: 0x00035E80
		public override void Rebuild(CanvasUpdate update)
		{
			if (update == 3)
			{
				if (!this.m_materialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_materialDirty = false;
			}
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x00037CA2 File Offset: 0x00035EA2
		public void RefreshMaterial()
		{
			this.UpdateMaterial();
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x0014F394 File Offset: 0x0014D594
		public override void UpdateMaterial()
		{
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, 0);
			this.m_canvasRenderer.SetTexture(this.mainTexture);
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00037CAA File Offset: 0x00035EAA
		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x00037CB2 File Offset: 0x00035EB2
		public override void RecalculateMasking()
		{
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00037CC1 File Offset: 0x00035EC1
		public Material GetMaterial()
		{
			return this.m_sharedMaterial;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0014F3F0 File Offset: 0x0014D5F0
		public Material GetMaterial(Material mat)
		{
			if (this.m_material == null || this.m_material.GetInstanceID() != mat.GetInstanceID())
			{
				this.m_material = this.CreateMaterialInstance(mat);
			}
			this.m_sharedMaterial = this.m_material;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x0014F45C File Offset: 0x0014D65C
		public Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x00037CC9 File Offset: 0x00035EC9
		public Material GetSharedMaterial()
		{
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			}
			return this.m_canvasRenderer.GetMaterial();
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00037CF3 File Offset: 0x00035EF3
		public void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_Material = this.m_sharedMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00037D1A File Offset: 0x00035F1A
		public int GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x040035DE RID: 13790
		[SerializeField]
		public TMP_FontAsset m_fontAsset;

		// Token: 0x040035DF RID: 13791
		[SerializeField]
		public TMP_SpriteAsset m_spriteAsset;

		// Token: 0x040035E0 RID: 13792
		[SerializeField]
		public Material m_material;

		// Token: 0x040035E1 RID: 13793
		[SerializeField]
		public Material m_sharedMaterial;

		// Token: 0x040035E2 RID: 13794
		[SerializeField]
		public bool m_isDefaultMaterial;

		// Token: 0x040035E3 RID: 13795
		[SerializeField]
		public float m_padding;

		// Token: 0x040035E4 RID: 13796
		[SerializeField]
		public CanvasRenderer m_canvasRenderer;

		// Token: 0x040035E5 RID: 13797
		public Mesh m_mesh;

		// Token: 0x040035E6 RID: 13798
		[SerializeField]
		public TextMeshProUGUI m_TextComponent;

		// Token: 0x040035E7 RID: 13799
		[NonSerialized]
		public bool m_isRegisteredForEvents;

		// Token: 0x040035E8 RID: 13800
		public bool m_materialDirty;

		// Token: 0x040035E9 RID: 13801
		[SerializeField]
		public int m_materialReferenceIndex;
	}
}
