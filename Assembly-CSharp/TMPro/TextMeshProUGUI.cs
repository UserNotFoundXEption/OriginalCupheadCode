using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000665 RID: 1637
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/TextMeshPro - Text", 11)]
	[SelectionBase]
	public class TextMeshProUGUI : TMP_Text, ILayoutElement
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x000371B1 File Offset: 0x000353B1
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

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x000371D2 File Offset: 0x000353D2
		public override Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x000371DA File Offset: 0x000353DA
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

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06004559 RID: 17753 RVA: 0x000371FF File Offset: 0x000353FF
		public InlineGraphicManager inlineGraphicManager
		{
			get
			{
				return this.m_inlineGraphics;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x00146234 File Offset: 0x00144434
		public override Bounds bounds
		{
			get
			{
				if (this.m_mesh != null)
				{
					return this.m_mesh.bounds;
				}
				return default(Bounds);
			}
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00146268 File Offset: 0x00144468
		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_preferredWidth = base.GetPreferredWidth();
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x001462BC File Offset: 0x001444BC
		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_preferredHeight = base.GetPreferredHeight();
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
			this.m_isCalculateSizeRequired = false;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00037207 File Offset: 0x00035407
		public override void SetVerticesDirty()
		{
			if (this.m_verticesAlreadyDirty || !this.IsActive())
			{
				return;
			}
			this.m_verticesAlreadyDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x0003722D File Offset: 0x0003542D
		public override void SetLayoutDirty()
		{
			if (this.m_layoutAlreadyDirty || !this.IsActive())
			{
				return;
			}
			this.m_layoutAlreadyDirty = true;
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			this.m_isLayoutDirty = true;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x0003725F File Offset: 0x0003545F
		public override void SetMaterialDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_isMaterialDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0003727A File Offset: 0x0003547A
		public override void SetAllDirty()
		{
			this.SetLayoutDirty();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0003728E File Offset: 0x0003548E
		public override void Rebuild(CanvasUpdate update)
		{
			if (update == 3)
			{
				this.OnPreRenderCanvas();
				this.m_verticesAlreadyDirty = false;
				this.m_layoutAlreadyDirty = false;
				if (!this.m_isMaterialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_isMaterialDirty = false;
			}
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00146318 File Offset: 0x00144518
		public void UpdateSubObjectPivot()
		{
			if (this.m_textInfo == null)
			{
				return;
			}
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].SetPivotDirty();
			}
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x0014635C File Offset: 0x0014455C
		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
			{
				this.m_stencilID = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_stencilID > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_stencilID);
				if (this.m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x001463CC File Offset: 0x001445CC
		public override void UpdateMaterial()
		{
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, this.m_sharedMaterial.mainTexture);
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06004565 RID: 17765 RVA: 0x000372C4 File Offset: 0x000354C4
		// (set) Token: 0x06004566 RID: 17766 RVA: 0x000372CC File Offset: 0x000354CC
		public Vector4 maskOffset
		{
			get
			{
				return this.m_maskOffset;
			}
			set
			{
				this.m_maskOffset = value;
				this.UpdateMask();
				this.m_havePropertiesChanged = true;
			}
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x000372E2 File Offset: 0x000354E2
		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x000372EA File Offset: 0x000354EA
		public override void RecalculateMasking()
		{
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00146420 File Offset: 0x00144620
		public override void UpdateMeshPadding()
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_havePropertiesChanged = true;
			this.checkPaddingRequired = false;
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].UpdateMeshPadding(this.m_enableExtraPadding, this.m_isUsingBold);
			}
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x000372F9 File Offset: 0x000354F9
		public override void ForceMeshUpdate()
		{
			this.m_havePropertiesChanged = true;
			this.OnPreRenderCanvas();
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x001464A0 File Offset: 0x001446A0
		public override TMP_TextInfo GetTextInfo(string text)
		{
			base.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			this.m_renderMode = TextRenderFlags.DontRender;
			this.ComputeMarginSize();
			if (this.m_canvas == null)
			{
				this.m_canvas = base.canvas;
			}
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00037308 File Offset: 0x00035508
		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
			if (index == 0)
			{
				this.m_canvasRenderer.SetMesh(mesh);
			}
			else
			{
				this.m_subTextObjects[index].canvasRenderer.SetMesh(mesh);
			}
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00146508 File Offset: 0x00144708
		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				}
				mesh.RecalculateBounds();
				if (i == 0)
				{
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x0014661C File Offset: 0x0014481C
		public override void UpdateVertexData()
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				mesh.RecalculateBounds();
				if (i == 0)
				{
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x0003733A File Offset: 0x0003553A
		public void UpdateFontAsset()
		{
			this.LoadFontAsset();
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00146708 File Offset: 0x00144908
		public override void Awake()
		{
			this.m_isAwake = true;
			this.m_canvas = base.canvas;
			this.m_isOrthographic = true;
			this.m_rectTransform = base.gameObject.GetComponent<RectTransform>();
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
			}
			if (this.m_mesh == null)
			{
				this.m_mesh = new Mesh();
				this.m_mesh.hideFlags = 61;
			}
			if (this.m_text == null)
			{
				this.m_enableWordWrapping = TMP_Settings.enableWordWrapping;
				this.m_enableKerning = TMP_Settings.enableKerning;
				this.m_enableExtraPadding = TMP_Settings.enableExtraPadding;
				this.m_tintAllSprites = TMP_Settings.enableTintAllSprites;
			}
			this.LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			this.m_char_buffer = new int[this.m_max_characters];
			this.m_cached_TextElement = new TMP_Glyph();
			this.m_isFirstAllocation = true;
			this.m_textInfo = new TMP_TextInfo(this);
			if (this.m_fontAsset == null)
			{
				return;
			}
			if (this.m_fontSizeMin == 0f)
			{
				this.m_fontSizeMin = this.m_fontSize / 2f;
			}
			if (this.m_fontSizeMax == 0f)
			{
				this.m_fontSizeMax = this.m_fontSize * 2f;
			}
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x00146898 File Offset: 0x00144A98
		public override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.m_canvas = this.GetCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(this.m_canvas, this);
			this.ComputeMarginSize();
			this.m_verticesAlreadyDirty = false;
			this.m_layoutAlreadyDirty = false;
			this.m_ShouldRecalculateStencil = true;
			this.SetAllDirty();
			this.RecalculateClipping();
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001468F8 File Offset: 0x00144AF8
		public override void OnDisable()
		{
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_canvasRenderer != null)
			{
				this.m_canvasRenderer.Clear();
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_rectTransform);
			this.RecalculateClipping();
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x00146968 File Offset: 0x00144B68
		public override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			if (this.m_mesh != null)
			{
				Object.DestroyImmediate(this.m_mesh);
			}
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
			}
			this.m_isRegisteredForEvents = false;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x001469C0 File Offset: 0x00144BC0
		public override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontAsset == null)
			{
				if (TMP_Settings.defaultFontAsset != null)
				{
					this.m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					this.m_fontAsset = (Resources.Load("Fonts & Materials/ARIAL SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				}
				if (this.m_fontAsset == null)
				{
					return;
				}
				if (this.m_fontAsset.characterDictionary == null)
				{
				}
				this.m_sharedMaterial = this.m_fontAsset.material;
			}
			else
			{
				if (this.m_fontAsset.characterDictionary == null)
				{
					this.m_fontAsset.ReadFontDefinition();
				}
				if (this.m_sharedMaterial == null && this.m_baseMaterial != null)
				{
					this.m_sharedMaterial = this.m_baseMaterial;
					this.m_baseMaterial = null;
				}
				if (this.m_sharedMaterial == null || this.m_sharedMaterial.mainTexture == null || this.m_fontAsset.atlas.GetInstanceID() != this.m_sharedMaterial.mainTexture.GetInstanceID())
				{
					if (!(this.m_fontAsset.material == null))
					{
						this.m_sharedMaterial = this.m_fontAsset.material;
					}
				}
			}
			base.GetSpecialCharacters(this.m_fontAsset);
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00146B40 File Offset: 0x00144D40
		public Canvas GetCanvas()
		{
			Canvas result = null;
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				result = list[0];
			}
			TMP_ListPool<Canvas>.Release(list);
			return result;
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x00146B80 File Offset: 0x00144D80
		public void UpdateEnvMapMatrix()
		{
			if (!this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) || this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null)
			{
				return;
			}
			Vector3 vector = this.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
			this.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(vector), Vector3.one);
			this.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, this.m_EnvMapMatrix);
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x00146C08 File Offset: 0x00144E08
		public void EnableMasking()
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_canvasRenderer.SetMaterial(this.m_fontMaterial, this.m_sharedMaterial.mainTexture);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.UpdateMask();
			}
			this.m_isMaskingEnabled = true;
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x00146CB4 File Offset: 0x00144EB4
		public void DisableMasking()
		{
			if (this.m_fontMaterial != null)
			{
				if (this.m_stencilID > 0)
				{
					this.m_sharedMaterial = this.m_MaskMaterial;
				}
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.mainTexture);
				Object.DestroyImmediate(this.m_fontMaterial);
			}
			this.m_isMaskingEnabled = false;
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x00146D18 File Offset: 0x00144F18
		public void UpdateMask()
		{
			if (this.m_rectTransform != null)
			{
				if (!ShaderUtilities.isInitialized)
				{
					ShaderUtilities.GetShaderPropertyIDs();
				}
				this.m_isScrollRegionSet = true;
				float num = Mathf.Min(Mathf.Min(this.m_margin.x, this.m_margin.z), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
				float num2 = Mathf.Min(Mathf.Min(this.m_margin.y, this.m_margin.w), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
				num = ((num <= 0f) ? 0f : num);
				num2 = ((num2 <= 0f) ? 0f : num2);
				float num3 = (this.m_rectTransform.rect.width - Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f + num;
				float num4 = (this.m_rectTransform.rect.height - Mathf.Max(this.m_margin.y, 0f) - Mathf.Max(this.m_margin.w, 0f)) / 2f + num2;
				Vector2 vector = this.m_rectTransform.localPosition + new Vector3((0.5f - this.m_rectTransform.pivot.x) * this.m_rectTransform.rect.width + (Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f, (0.5f - this.m_rectTransform.pivot.y) * this.m_rectTransform.rect.height + (-Mathf.Max(this.m_margin.y, 0f) + Mathf.Max(this.m_margin.w, 0f)) / 2f);
				Vector4 vector2;
				vector2..ctor(vector.x, vector.y, num3, num4);
				this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, vector2);
			}
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x00146F78 File Offset: 0x00145178
		public override Material GetMaterial(Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontMaterial == null || this.m_fontMaterial.GetInstanceID() != mat.GetInstanceID())
			{
				this.m_fontMaterial = this.CreateMaterialInstance(mat);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_ShouldRecalculateStencil = true;
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x00146FF0 File Offset: 0x001451F0
		public override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontMaterials == null)
			{
				this.m_fontMaterials = new Material[materialCount];
			}
			else if (this.m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					this.m_fontMaterials[i] = this.m_subTextObjects[i].material;
				}
			}
			this.m_fontSharedMaterials = this.m_fontMaterials;
			return this.m_fontMaterials;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x00037342 File Offset: 0x00035542
		public override void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x00147094 File Offset: 0x00145294
		public override Material[] GetSharedMaterials()
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontSharedMaterials[i] = this.m_sharedMaterial;
				}
				else
				{
					this.m_fontSharedMaterials[i] = this.m_subTextObjects[i].sharedMaterial;
				}
			}
			return this.m_fontSharedMaterials;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0014712C File Offset: 0x0014532C
		public override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					if (!(materials[i].mainTexture == null) && materials[i].mainTexture.GetInstanceID() == this.m_sharedMaterial.mainTexture.GetInstanceID())
					{
						this.m_sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
						this.m_padding = this.GetPaddingForMaterial(this.m_sharedMaterial);
					}
				}
				else if (!(materials[i].mainTexture == null) && materials[i].mainTexture.GetInstanceID() == this.m_subTextObjects[i].sharedMaterial.mainTexture.GetInstanceID())
				{
					if (this.m_subTextObjects[i].isDefaultMaterial)
					{
						this.m_subTextObjects[i].sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x00147268 File Offset: 0x00145468
		public override void SetOutlineThickness(float thickness)
		{
			if (this.m_fontMaterial != null && this.m_sharedMaterial.GetInstanceID() != this.m_fontMaterial.GetInstanceID())
			{
				this.m_sharedMaterial = this.m_fontMaterial;
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.mainTexture);
			}
			else if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_sharedMaterial = this.m_fontMaterial;
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.mainTexture);
			}
			thickness = Mathf.Clamp01(thickness);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			this.m_padding = this.GetPaddingForMaterial();
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x00147340 File Offset: 0x00145540
		public override void SetFaceColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, color);
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x001473A0 File Offset: 0x001455A0
		public override void SetOutlineColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color);
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00147400 File Offset: 0x00145600
		public override void SetShaderDepth()
		{
			if (this.m_canvas == null || this.m_sharedMaterial == null)
			{
				return;
			}
			if (this.m_canvas.renderMode == null || this.m_isOverlay)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			}
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0014747C File Offset: 0x0014567C
		public override void SetCulling()
		{
			if (this.m_isCullingEnabled)
			{
				this.m_canvasRenderer.GetMaterial().SetFloat("_CullMode", 2f);
			}
			else
			{
				this.m_canvasRenderer.GetMaterial().SetFloat("_CullMode", 0f);
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x0003735D File Offset: 0x0003555D
		public void SetPerspectiveCorrection()
		{
			if (this.m_isOrthographic)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x001474D0 File Offset: 0x001456D0
		public override float GetPaddingForMaterial(Material mat)
		{
			this.m_padding = ShaderUtilities.GetPadding(mat, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00147520 File Offset: 0x00145720
		public override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x00037399 File Offset: 0x00035599
		public void SetMeshArrays(int size)
		{
			this.m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			this.m_canvasRenderer.SetMesh(this.m_textInfo.meshInfo[0].mesh);
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0014757C File Offset: 0x0014577C
		public override int SetArraySizes(int[] chars)
		{
			int num = 0;
			int num2 = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			this.tag_NoParsing = false;
			this.m_style = this.m_fontStyle;
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo();
			}
			this.m_textElementType = TMP_TextElementType.Character;
			int num3 = 0;
			while (chars[num3] != 0)
			{
				if (this.m_textInfo.characterInfo == null || this.m_totalCharacterCount >= this.m_textInfo.characterInfo.Length)
				{
					TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, this.m_totalCharacterCount + 1, true);
				}
				int num4 = chars[num3];
				if (!this.m_isRichText || num4 != 60)
				{
					goto IL_1FE;
				}
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!base.ValidateHtmlTag(chars, num3 + 1, out num))
				{
					goto IL_1FE;
				}
				num3 = num;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
				{
					this.m_isUsingBold = true;
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					MaterialReference[] materialReferences = this.m_materialReferences;
					int currentMaterialIndex2 = this.m_currentMaterialIndex;
					materialReferences[currentMaterialIndex2].referenceCount = materialReferences[currentMaterialIndex2].referenceCount + 1;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)(57344 + this.m_spriteIndex);
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_textElementType = TMP_TextElementType.Character;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num2++;
					this.m_totalCharacterCount++;
				}
				IL_7BE:
				num3++;
				continue;
				IL_1FE:
				bool flag = false;
				bool flag2 = false;
				TMP_FontAsset currentFontAsset = this.m_currentFontAsset;
				Material currentMaterial = this.m_currentMaterial;
				int currentMaterialIndex3 = this.m_currentMaterialIndex;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num4))
						{
							num4 = (int)char.ToUpper((char)num4);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num4))
						{
							num4 = (int)char.ToLower((char)num4);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num4))
					{
						num4 = (int)char.ToUpper((char)num4);
					}
				}
				TMP_Glyph tmp_Glyph;
				if (!this.m_currentFontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
				{
					if (this.m_currentFontAsset.fallbackFontAssets != null && this.m_currentFontAsset.fallbackFontAssets.Count > 0)
					{
						for (int i = 0; i < this.m_currentFontAsset.fallbackFontAssets.Count; i++)
						{
							TMP_FontAsset tmp_FontAsset = this.m_currentFontAsset.fallbackFontAssets[i];
							if (!(tmp_FontAsset == null))
							{
								if (tmp_FontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
								{
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									break;
								}
							}
						}
					}
					if (tmp_Glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
					{
						for (int j = 0; j < TMP_Settings.fallbackFontAssets.Count; j++)
						{
							TMP_FontAsset tmp_FontAsset = TMP_Settings.fallbackFontAssets[j];
							if (!(tmp_FontAsset == null))
							{
								if (tmp_FontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
								{
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									break;
								}
							}
						}
					}
					if (tmp_Glyph == null)
					{
						if (char.IsLower((char)num4))
						{
							if (this.m_currentFontAsset.characterDictionary.TryGetValue((int)char.ToUpper((char)num4), out tmp_Glyph))
							{
								num4 = (chars[num3] = (int)char.ToUpper((char)num4));
							}
						}
						else if (char.IsUpper((char)num4) && this.m_currentFontAsset.characterDictionary.TryGetValue((int)char.ToLower((char)num4), out tmp_Glyph))
						{
							num4 = (chars[num3] = (int)char.ToLower((char)num4));
						}
					}
					if (tmp_Glyph == null)
					{
						if (this.m_currentFontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
						{
							if (!TMP_Settings.warningsDisabled)
							{
							}
							num4 = (chars[num3] = 9633);
						}
						else
						{
							if (TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
							{
								for (int k = 0; k < TMP_Settings.fallbackFontAssets.Count; k++)
								{
									TMP_FontAsset tmp_FontAsset = TMP_Settings.fallbackFontAssets[k];
									if (!(tmp_FontAsset == null))
									{
										if (tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
										{
											if (!TMP_Settings.warningsDisabled)
											{
											}
											num4 = (chars[num3] = 9633);
											flag = true;
											this.m_currentFontAsset = tmp_FontAsset;
											this.m_currentMaterial = tmp_FontAsset.material;
											this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
											break;
										}
									}
								}
							}
							if (tmp_Glyph == null)
							{
								TMP_FontAsset tmp_FontAsset = TMP_Settings.GetFontAsset();
								if (tmp_FontAsset != null && tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
								{
									if (!TMP_Settings.warningsDisabled)
									{
									}
									num4 = (chars[num3] = 9633);
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
								}
								else
								{
									tmp_FontAsset = TMP_FontAsset.defaultFontAsset;
									if (tmp_FontAsset != null && tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
									{
										if (!TMP_Settings.warningsDisabled)
										{
										}
										num4 = (chars[num3] = 9633);
										flag = true;
										this.m_currentFontAsset = tmp_FontAsset;
										this.m_currentMaterial = tmp_FontAsset.material;
										this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									}
								}
							}
						}
					}
				}
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].textElement = tmp_Glyph;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].material = this.m_currentMaterial;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
				if (!char.IsWhiteSpace((char)num4))
				{
					MaterialReference[] materialReferences2 = this.m_materialReferences;
					int currentMaterialIndex4 = this.m_currentMaterialIndex;
					materialReferences2[currentMaterialIndex4].referenceCount = materialReferences2[currentMaterialIndex4].referenceCount + 1;
					if (flag2)
					{
						this.m_currentMaterialIndex = currentMaterialIndex3;
					}
					if (flag)
					{
						this.m_currentFontAsset = currentFontAsset;
						this.m_currentMaterial = currentMaterial;
						this.m_currentMaterialIndex = currentMaterialIndex3;
					}
				}
				this.m_totalCharacterCount++;
				goto IL_7BE;
			}
			this.m_textInfo.spriteCount = num2;
			int num5 = this.m_textInfo.materialCount = this.m_materialReferenceIndexLookup.Count;
			if (num5 > this.m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize<TMP_MeshInfo>(ref this.m_textInfo.meshInfo, num5, false);
			}
			for (int l = 0; l < num5; l++)
			{
				if (l > 0)
				{
					if (this.m_subTextObjects[l] == null)
					{
						this.m_subTextObjects[l] = TMP_SubMeshUI.AddSubTextObject(this, this.m_materialReferences[l]);
						this.m_textInfo.meshInfo[l].vertices = null;
					}
					if (this.m_rectTransform.pivot != this.m_subTextObjects[l].rectTransform.pivot)
					{
						this.m_subTextObjects[l].rectTransform.pivot = this.m_rectTransform.pivot;
					}
					if (this.m_subTextObjects[l].sharedMaterial == null || this.m_subTextObjects[l].sharedMaterial.GetInstanceID() != this.m_materialReferences[l].material.GetInstanceID())
					{
						bool isDefaultMaterial = this.m_materialReferences[l].isDefaultMaterial;
						this.m_subTextObjects[l].isDefaultMaterial = isDefaultMaterial;
						if (!isDefaultMaterial || this.m_subTextObjects[l].sharedMaterial == null || this.m_subTextObjects[l].sharedMaterial.mainTexture.GetInstanceID() != this.m_materialReferences[l].material.mainTexture.GetInstanceID())
						{
							this.m_subTextObjects[l].sharedMaterial = this.m_materialReferences[l].material;
							this.m_subTextObjects[l].fontAsset = this.m_materialReferences[l].fontAsset;
							this.m_subTextObjects[l].spriteAsset = this.m_materialReferences[l].spriteAsset;
						}
					}
				}
				int referenceCount = this.m_materialReferences[l].referenceCount;
				if (this.m_textInfo.meshInfo[l].vertices == null || this.m_textInfo.meshInfo[l].vertices.Length < referenceCount * 4)
				{
					if (this.m_textInfo.meshInfo[l].vertices == null)
					{
						if (l == 0)
						{
							this.m_textInfo.meshInfo[l] = new TMP_MeshInfo(this.m_mesh, referenceCount + 1);
						}
						else
						{
							this.m_textInfo.meshInfo[l] = new TMP_MeshInfo(this.m_subTextObjects[l].mesh, referenceCount + 1);
						}
					}
					else
					{
						this.m_textInfo.meshInfo[l].ResizeMeshInfo((referenceCount <= 1024) ? Mathf.NextPowerOfTwo(referenceCount) : (referenceCount + 256));
					}
				}
			}
			int num6 = num5;
			while (num6 < this.m_subTextObjects.Length + 1 && this.m_subTextObjects[num6] != null)
			{
				if (num6 < this.m_textInfo.meshInfo.Length)
				{
					this.m_subTextObjects[num6].canvasRenderer.SetMesh(null);
				}
				num6++;
			}
			return this.m_totalCharacterCount;
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x001480E8 File Offset: 0x001462E8
		public override void ComputeMarginSize()
		{
			if (base.rectTransform != null)
			{
				this.m_marginWidth = this.m_rectTransform.rect.width - this.m_margin.x - this.m_margin.z;
				this.m_marginHeight = this.m_rectTransform.rect.height - this.m_margin.y - this.m_margin.w;
				this.m_RectTransformCorners = this.GetTextContainerLocalCorners();
			}
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x000373D3 File Offset: 0x000355D3
		public override void OnDidApplyAnimationProperties()
		{
			this.m_havePropertiesChanged = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x000373E8 File Offset: 0x000355E8
		public override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_canvas = base.canvas;
			this.ComputeMarginSize();
			this.m_havePropertiesChanged = true;
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00037409 File Offset: 0x00035609
		public override void OnRectTransformDimensionsChange()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.ComputeMarginSize();
			this.UpdateSubObjectPivot();
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x00148174 File Offset: 0x00146374
		public void LateUpdate()
		{
			if (this.m_rectTransform.hasChanged)
			{
				Vector3 lossyScale = this.m_rectTransform.lossyScale;
				if (!this.m_havePropertiesChanged && lossyScale.y != this.m_previousLossyScale.y && this.m_text != string.Empty && this.m_text != null)
				{
					this.UpdateSDFScale(lossyScale.y);
					this.m_previousLossyScale = lossyScale;
				}
				this.m_rectTransform.hasChanged = false;
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00148200 File Offset: 0x00146400
		public void OnPreRenderCanvas()
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.m_canvas == null)
			{
				this.m_canvas = base.canvas;
				if (this.m_canvas == null)
				{
					return;
				}
			}
			this.loopCountA = 0;
			if (this.m_havePropertiesChanged || this.m_isLayoutDirty)
			{
				if (this.checkPaddingRequired)
				{
					this.UpdateMeshPadding();
				}
				if (this.m_isInputParsingRequired || this.m_isTextTruncated)
				{
					base.ParseInputText();
				}
				if (this.m_enableAutoSizing)
				{
					this.m_fontSize = Mathf.Clamp(this.m_fontSize, this.m_fontSizeMin, this.m_fontSizeMax);
				}
				this.m_maxFontSize = this.m_fontSizeMax;
				this.m_minFontSize = this.m_fontSizeMin;
				this.m_lineSpacingDelta = 0f;
				this.m_charWidthAdjDelta = 0f;
				this.m_recursiveCount = 0;
				this.m_isCharacterWrappingEnabled = false;
				this.m_isTextTruncated = false;
				this.m_isLayoutDirty = false;
				this.GenerateTextMesh();
				this.m_havePropertiesChanged = false;
			}
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00148314 File Offset: 0x00146514
		public override void GenerateTextMesh()
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				return;
			}
			if (this.m_textInfo != null)
			{
				this.m_textInfo.Clear();
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				this.ClearMesh();
				this.m_preferredWidth = 0f;
				this.m_preferredHeight = 0f;
				return;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_currentSpriteAsset = this.m_spriteAsset;
			int totalCharacterCount = this.m_totalCharacterCount;
			this.m_fontScale = this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
			float num = this.m_fontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
			float num2 = this.m_fontScale;
			this.m_fontScaleMultiplier = 1f;
			this.m_currentFontSize = this.m_fontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_lineJustification = this.m_textAlignment;
			float num3 = 0f;
			float num4 = 1f;
			this.m_baselineOffset = 0f;
			bool flag = false;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			bool flag2 = false;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			this.m_fontColor32 = this.m_fontColor;
			this.m_htmlColor = this.m_fontColor32;
			this.m_colorStack.SetDefault(this.m_htmlColor);
			this.m_styleStack.Clear();
			this.m_actionStack.Clear();
			this.m_lineOffset = 0f;
			this.m_lineHeight = 0f;
			float num5 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_visibleCharacterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_lastCharacterOfLine = 0;
			this.m_firstVisibleCharacterOfLine = 0;
			this.m_lastVisibleCharacterOfLine = 0;
			this.m_maxLineAscender = float.NegativeInfinity;
			this.m_maxLineDescender = float.PositiveInfinity;
			this.m_lineNumber = 0;
			bool flag3 = true;
			this.m_pageNumber = 0;
			int num6 = Mathf.Clamp(this.m_pageToDisplay - 1, 0, this.m_textInfo.pageInfo.Length - 1);
			int num7 = 0;
			Vector4 margin = this.m_margin;
			float marginWidth = this.m_marginWidth;
			float marginHeight = this.m_marginHeight;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			this.m_meshExtents.min = TMP_Text.k_InfinityVectorPositive;
			this.m_meshExtents.max = TMP_Text.k_InfinityVectorNegative;
			this.m_textInfo.ClearLineInfo();
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			float num8 = 0f;
			float num9 = 0f;
			bool flag4 = false;
			this.m_isNewPage = false;
			bool flag5 = true;
			bool flag6 = false;
			int num10 = 0;
			this.loopCountA++;
			int num11 = 0;
			int num12 = 0;
			while (this.m_char_buffer[num12] != 0)
			{
				int num13 = this.m_char_buffer[num12];
				this.m_textElementType = TMP_TextElementType.Character;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num13 != 60)
				{
					goto IL_44F;
				}
				this.m_isParsingText = true;
				if (!base.ValidateHtmlTag(this.m_char_buffer, num12 + 1, out num11))
				{
					goto IL_44F;
				}
				num12 = num11;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_44F;
				}
				IL_285B:
				num12++;
				continue;
				IL_44F:
				this.m_isParsingText = false;
				bool flag7 = false;
				bool flag8 = false;
				float num14 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num13))
						{
							num13 = (int)char.ToUpper((char)num13);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num13))
						{
							num13 = (int)char.ToLower((char)num13);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num13))
					{
						num14 = 0.8f;
						num13 = (int)char.ToUpper((char)num13);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					num13 = 57344 + this.m_spriteIndex;
					this.m_currentFontAsset = this.m_fontAsset;
					float num15 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num15;
					this.m_cached_TextElement = tmp_Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num15;
					this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset = this.m_currentSpriteAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num3 = 0f;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					if (this.m_cached_TextElement == null)
					{
						goto IL_285B;
					}
					this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
					this.m_currentMaterial = this.m_textInfo.characterInfo[this.m_characterCount].material;
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num14 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
					num2 = this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num2;
					num3 = ((this.m_currentMaterialIndex != 0) ? this.m_subTextObjects[this.m_currentMaterialIndex].padding : this.m_padding);
				}
				if (this.m_isRightToLeft)
				{
					this.m_xAdvance -= ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].character = (char)num13;
				this.m_textInfo.characterInfo[this.m_characterCount].pointSize = this.m_currentFontSize;
				this.m_textInfo.characterInfo[this.m_characterCount].color = this.m_htmlColor;
				this.m_textInfo.characterInfo[this.m_characterCount].style = this.m_style;
				this.m_textInfo.characterInfo[this.m_characterCount].index = (short)num12;
				if (this.m_enableKerning && this.m_characterCount >= 1)
				{
					int character = (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num13);
					KerningPair kerningPair;
					this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
					if (kerningPair != null)
					{
						this.m_xAdvance += kerningPair.XadvanceOffset * num2;
					}
				}
				float num16 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num16 = (this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2) * (1f - this.m_charWidthAdjDelta);
					this.m_xAdvance += num16;
				}
				float num17;
				if (this.m_textElementType == TMP_TextElementType.Character && !flag8 && ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold))
				{
					num17 = this.m_currentFontAsset.boldStyle * 2f;
					num4 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					num17 = this.m_currentFontAsset.normalStyle * 2f;
					num4 = 1f;
				}
				float baseline = this.m_currentFontAsset.fontInfo.Baseline;
				Vector3 vector;
				vector.x = this.m_xAdvance + (this.m_cached_TextElement.xOffset - num3 - num17) * num2 * (1f - this.m_charWidthAdjDelta);
				vector.y = (baseline + this.m_cached_TextElement.yOffset + num3) * num2 - this.m_lineOffset + this.m_baselineOffset;
				vector.z = 0f;
				Vector3 vector2;
				vector2.x = vector.x;
				vector2.y = vector.y - (this.m_cached_TextElement.height + num3 * 2f) * num2;
				vector2.z = 0f;
				Vector3 vector3;
				vector3.x = vector2.x + (this.m_cached_TextElement.width + num3 * 2f + num17 * 2f) * num2 * (1f - this.m_charWidthAdjDelta);
				vector3.y = vector.y;
				vector3.z = 0f;
				Vector3 vector4;
				vector4.x = vector3.x;
				vector4.y = vector2.y;
				vector4.z = 0f;
				if (this.m_textElementType == TMP_TextElementType.Character && !flag8 && ((this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic))
				{
					float num18 = (float)this.m_currentFontAsset.italicStyle * 0.01f;
					Vector3 vector5;
					vector5..ctor(num18 * ((this.m_cached_TextElement.yOffset + num3 + num17) * num2), 0f, 0f);
					Vector3 vector6;
					vector6..ctor(num18 * ((this.m_cached_TextElement.yOffset - this.m_cached_TextElement.height - num3 - num17) * num2), 0f, 0f);
					vector += vector5;
					vector2 += vector6;
					vector3 += vector5;
					vector4 += vector6;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft = vector2;
				this.m_textInfo.characterInfo[this.m_characterCount].topLeft = vector;
				this.m_textInfo.characterInfo[this.m_characterCount].topRight = vector3;
				this.m_textInfo.characterInfo[this.m_characterCount].bottomRight = vector4;
				this.m_textInfo.characterInfo[this.m_characterCount].origin = this.m_xAdvance;
				this.m_textInfo.characterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].aspectRatio = (vector3.x - vector2.x) / (vector.y - vector2.y);
				float num19 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : num2) + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].ascender = num19 - this.m_lineOffset;
				this.m_maxLineAscender = ((num19 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num19);
				float num20 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : num2) + this.m_baselineOffset;
				float num21 = this.m_textInfo.characterInfo[this.m_characterCount].descender = num20 - this.m_lineOffset;
				this.m_maxLineDescender = ((num20 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num20);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num22 = (num19 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num19 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num22 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num22);
					float num23 = (num20 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num20 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num23 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num23);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num19) ? num19 : this.m_maxAscender);
				}
				if (this.m_lineOffset == 0f)
				{
					num8 = ((num8 <= num19) ? num19 : num8);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
				if (num13 == 9 || !char.IsWhiteSpace((char)num13) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					this.m_textInfo.characterInfo[this.m_characterCount].isVisible = true;
					float num24 = (this.m_width == -1f) ? (marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
					this.m_textInfo.lineInfo[this.m_lineNumber].width = num24;
					this.m_textInfo.lineInfo[this.m_lineNumber].marginLeft = this.m_marginLeft;
					if (Mathf.Abs(this.m_xAdvance) + (this.m_isRightToLeft ? 0f : this.m_cached_TextElement.xAdvance) * (1f - this.m_charWidthAdjDelta) * num2 > num24)
					{
						num7 = this.m_characterCount - 1;
						if (base.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
						{
							if (num10 == this.m_SavedWordWrapState.previous_WordBreak || flag5)
							{
								if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
								{
									if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
									{
										this.loopCountA = 0;
										this.m_charWidthAdjDelta += 0.01f;
										this.GenerateTextMesh();
										return;
									}
									this.m_maxFontSize = this.m_fontSize;
									this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
									this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
									if (this.loopCountA > 20)
									{
										return;
									}
									this.GenerateTextMesh();
									return;
								}
								else
								{
									if (!this.m_isCharacterWrappingEnabled)
									{
										this.m_isCharacterWrappingEnabled = true;
									}
									else
									{
										flag6 = true;
									}
									this.m_recursiveCount++;
									if (this.m_recursiveCount > 20)
									{
										goto IL_285B;
									}
								}
							}
							num12 = base.RestoreWordWrappingState(ref this.m_SavedWordWrapState);
							num10 = num12;
							if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
							{
								float num25 = this.m_maxLineAscender - this.m_startOfLineAscender;
								this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num25);
								this.m_lineOffset += num25;
								this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
								this.m_SavedWordWrapState.previousLineAscender = this.m_maxLineAscender;
							}
							this.m_isNewPage = false;
							float num26 = this.m_maxLineAscender - this.m_lineOffset;
							float num27 = this.m_maxLineDescender - this.m_lineOffset;
							this.m_maxDescender = ((this.m_maxDescender >= num27) ? num27 : this.m_maxDescender);
							if (!flag4)
							{
								num9 = this.m_maxDescender;
							}
							if (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines)
							{
								flag4 = true;
							}
							this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = ((this.m_characterCount - 1 <= 0) ? 0 : (this.m_characterCount - 1));
							this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = this.m_lastVisibleCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num27);
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num26);
							this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x;
							this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2;
							this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
							this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num26;
							this.m_textInfo.lineInfo[this.m_lineNumber].descender = num27;
							this.m_firstCharacterOfLine = this.m_characterCount;
							base.SaveWordWrappingState(ref this.m_SavedLineState, num12, this.m_characterCount - 1);
							this.m_lineNumber++;
							flag3 = true;
							if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
							{
								base.ResizeLineExtents(this.m_lineNumber);
							}
							if (this.m_lineHeight == 0f)
							{
								float num28 = this.m_textInfo.characterInfo[this.m_characterCount].ascender - this.m_textInfo.characterInfo[this.m_characterCount].baseLine;
								float num29 = 0f - this.m_maxLineDescender + num28 + (num5 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
								this.m_lineOffset += num29;
								this.m_startOfLineAscender = num28;
							}
							else
							{
								this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
							}
							this.m_maxLineAscender = float.NegativeInfinity;
							this.m_maxLineDescender = float.PositiveInfinity;
							this.m_xAdvance = this.tag_Indent;
							goto IL_285B;
						}
						if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
						{
							if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
							{
								this.loopCountA = 0;
								this.m_charWidthAdjDelta += 0.01f;
								this.GenerateTextMesh();
								return;
							}
							this.m_maxFontSize = this.m_fontSize;
							this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
							this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
							this.m_recursiveCount = 0;
							if (this.loopCountA > 20)
							{
								return;
							}
							this.GenerateTextMesh();
							return;
						}
						else
						{
							switch (this.m_overflowMode)
							{
							case TextOverflowModes.Overflow:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								break;
							case TextOverflowModes.Ellipsis:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_isTextTruncated = true;
								if (this.m_characterCount >= 1)
								{
									this.m_char_buffer[num12 - 1] = 8230;
									this.m_char_buffer[num12] = 0;
									if (this.m_cached_Ellipsis_GlyphInfo != null)
									{
										this.m_textInfo.characterInfo[num7].character = '…';
										this.m_textInfo.characterInfo[num7].textElement = this.m_cached_Ellipsis_GlyphInfo;
										this.m_textInfo.characterInfo[num7].fontAsset = this.m_materialReferences[0].fontAsset;
										this.m_textInfo.characterInfo[num7].material = this.m_materialReferences[0].material;
										this.m_textInfo.characterInfo[num7].materialReferenceIndex = 0;
									}
									this.m_totalCharacterCount = num7 + 1;
									this.GenerateTextMesh();
									return;
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								this.m_visibleCharacterCount = 0;
								break;
							case TextOverflowModes.Masking:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							case TextOverflowModes.Truncate:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								break;
							case TextOverflowModes.ScrollRect:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							}
						}
					}
					if (num13 != 9)
					{
						Color32 vertexColor;
						if (flag7)
						{
							vertexColor = Color.red;
						}
						else if (this.m_overrideHtmlColors)
						{
							vertexColor = this.m_fontColor32;
						}
						else
						{
							vertexColor = this.m_htmlColor;
						}
						if (this.m_textElementType == TMP_TextElementType.Character)
						{
							this.SaveGlyphVertexInfo(num3, num17, vertexColor);
						}
						else if (this.m_textElementType == TMP_TextElementType.Sprite)
						{
							this.SaveSpriteVertexInfo(vertexColor);
						}
					}
					else
					{
						this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
						TMP_LineInfo[] lineInfo = this.m_textInfo.lineInfo;
						int lineNumber = this.m_lineNumber;
						lineInfo[lineNumber].spaceCount = lineInfo[lineNumber].spaceCount + 1;
						this.m_textInfo.spaceCount++;
					}
					if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
					{
						if (flag3)
						{
							flag3 = false;
							this.m_firstVisibleCharacterOfLine = this.m_characterCount;
						}
						this.m_visibleCharacterCount++;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
					}
				}
				else if (num13 == 10 || char.IsSeparator((char)num13))
				{
					TMP_LineInfo[] lineInfo2 = this.m_textInfo.lineInfo;
					int lineNumber2 = this.m_lineNumber;
					lineInfo2[lineNumber2].spaceCount = lineInfo2[lineNumber2].spaceCount + 1;
					this.m_textInfo.spaceCount++;
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
				{
					float num30 = this.m_maxLineAscender - this.m_startOfLineAscender;
					this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num30);
					num21 -= num30;
					this.m_lineOffset += num30;
					this.m_startOfLineAscender += num30;
					this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
					this.m_SavedWordWrapState.previousLineAscender = this.m_startOfLineAscender;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].lineNumber = (short)this.m_lineNumber;
				this.m_textInfo.characterInfo[this.m_characterCount].pageNumber = (short)this.m_pageNumber;
				if ((num13 != 10 && num13 != 13 && num13 != 8230) || this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
				{
					this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
				}
				if (this.m_maxAscender - num21 > marginHeight + 0.0001f)
				{
					if (this.m_enableAutoSizing && this.m_lineSpacingDelta > this.m_lineSpacingMax && this.m_lineNumber > 0)
					{
						this.m_lineSpacingDelta -= 1f;
						this.GenerateTextMesh();
						return;
					}
					if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
					{
						this.m_maxFontSize = this.m_fontSize;
						this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
						this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
						this.m_recursiveCount = 0;
						if (this.loopCountA > 20)
						{
							return;
						}
						this.GenerateTextMesh();
						return;
					}
					else
					{
						switch (this.m_overflowMode)
						{
						case TextOverflowModes.Overflow:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							break;
						case TextOverflowModes.Ellipsis:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[(int)this.m_textInfo.characterInfo[num7].index] = 8230;
								this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num7].index + 1)] = 0;
								if (this.m_cached_Ellipsis_GlyphInfo != null)
								{
									this.m_textInfo.characterInfo[num7].character = '…';
									this.m_textInfo.characterInfo[num7].textElement = this.m_cached_Ellipsis_GlyphInfo;
									this.m_textInfo.characterInfo[num7].fontAsset = this.m_materialReferences[0].fontAsset;
									this.m_textInfo.characterInfo[num7].material = this.m_materialReferences[0].material;
									this.m_textInfo.characterInfo[num7].materialReferenceIndex = 0;
								}
								this.m_totalCharacterCount = num7 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh();
							return;
						case TextOverflowModes.Masking:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Truncate:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num7].index + 1)] = 0;
								this.m_totalCharacterCount = num7 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh();
							return;
						case TextOverflowModes.ScrollRect:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Page:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (num13 != 13 && num13 != 10)
							{
								num12 = base.RestoreWordWrappingState(ref this.m_SavedLineState);
								if (num12 == 0)
								{
									this.ClearMesh();
									return;
								}
								this.m_isNewPage = true;
								this.m_xAdvance = this.tag_Indent;
								this.m_lineOffset = 0f;
								this.m_lineNumber++;
								this.m_pageNumber++;
								goto IL_285B;
							}
							break;
						}
					}
				}
				if (num13 == 9)
				{
					this.m_xAdvance += this.m_currentFontAsset.fontInfo.TabWidth * num2;
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += (this.m_monoSpacing - num16 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				else if (!this.m_isRightToLeft)
				{
					this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].xAdvance = this.m_xAdvance;
				if (num13 == 13)
				{
					this.m_xAdvance = this.tag_Indent;
				}
				if (num13 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
					{
						float num31 = this.m_maxLineAscender - this.m_startOfLineAscender;
						this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num31);
						num21 -= num31;
						this.m_lineOffset += num31;
					}
					this.m_isNewPage = false;
					float num32 = this.m_maxLineAscender - this.m_lineOffset;
					float num33 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num33) ? num33 : this.m_maxDescender);
					if (!flag4)
					{
						num9 = this.m_maxDescender;
					}
					if (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines)
					{
						flag4 = true;
					}
					this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
					this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
					this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = this.m_characterCount;
					this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = ((this.m_lastVisibleCharacterOfLine < this.m_firstVisibleCharacterOfLine) ? this.m_firstVisibleCharacterOfLine : this.m_lastVisibleCharacterOfLine);
					this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num33);
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num32);
					this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x - num3 * num2;
					this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2;
					this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
					this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num32;
					this.m_textInfo.lineInfo[this.m_lineNumber].descender = num33;
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					if (num13 == 10)
					{
						base.SaveWordWrappingState(ref this.m_SavedLineState, num12, this.m_characterCount);
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
						this.m_lineNumber++;
						flag3 = true;
						if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
						{
							base.ResizeLineExtents(this.m_lineNumber);
						}
						if (this.m_lineHeight == 0f)
						{
							float num29 = 0f - this.m_maxLineDescender + num19 + (num5 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num29;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_startOfLineAscender = num19;
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
						num7 = this.m_characterCount - 1;
						this.m_characterCount++;
						goto IL_285B;
					}
				}
				if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
				{
					this.m_meshExtents.min.x = Mathf.Min(this.m_meshExtents.min.x, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.x);
					this.m_meshExtents.min.y = Mathf.Min(this.m_meshExtents.min.y, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.y);
					this.m_meshExtents.max.x = Mathf.Max(this.m_meshExtents.max.x, this.m_textInfo.characterInfo[this.m_characterCount].topRight.x);
					this.m_meshExtents.max.y = Mathf.Max(this.m_meshExtents.max.y, this.m_textInfo.characterInfo[this.m_characterCount].topRight.y);
				}
				if (this.m_overflowMode == TextOverflowModes.Page && num13 != 13 && num13 != 10 && this.m_pageNumber < 16)
				{
					this.m_textInfo.pageInfo[this.m_pageNumber].ascender = num8;
					this.m_textInfo.pageInfo[this.m_pageNumber].descender = ((num20 >= this.m_textInfo.pageInfo[this.m_pageNumber].descender) ? this.m_textInfo.pageInfo[this.m_pageNumber].descender : num20);
					if (this.m_pageNumber == 0 && this.m_characterCount == 0)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount > 0 && this.m_pageNumber != (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].pageNumber)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber - 1].lastCharacterIndex = this.m_characterCount - 1;
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount == totalCharacterCount - 1)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].lastCharacterIndex = this.m_characterCount;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if (char.IsWhiteSpace((char)num13) && !this.m_isNonBreakingSpace)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag5 = false;
					}
					else if (num13 > 11904 && num13 < 40959 && !this.m_isNonBreakingSpace)
					{
						if (!this.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num13) && this.m_characterCount < totalCharacterCount - 1 && !this.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey((int)this.m_textInfo.characterInfo[this.m_characterCount + 1].character))
						{
							base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag5 = false;
						}
					}
					else if (flag5 || this.m_isCharacterWrappingEnabled || flag6)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_285B;
			}
			float num34 = this.m_maxFontSize - this.m_minFontSize;
			if (!this.m_isCharacterWrappingEnabled && this.m_enableAutoSizing && num34 > 0.051f && this.m_fontSize < this.m_fontSizeMax)
			{
				this.m_minFontSize = this.m_fontSize;
				this.m_fontSize += Mathf.Max((this.m_maxFontSize - this.m_fontSize) / 2f, 0.05f);
				this.m_fontSize = (float)((int)(Mathf.Min(this.m_fontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
				if (this.loopCountA > 20)
				{
					return;
				}
				this.GenerateTextMesh();
				return;
			}
			else
			{
				this.m_isCharacterWrappingEnabled = false;
				if (this.m_visibleCharacterCount == 0 && this.m_visibleSpriteCount == 0)
				{
					this.ClearMesh();
					return;
				}
				int num35 = this.m_materialReferences[0].referenceCount * 4;
				this.m_textInfo.meshInfo[0].Clear(false);
				Vector3 vector7 = Vector3.zero;
				Vector3[] rectTransformCorners = this.m_RectTransformCorners;
				switch (this.m_textAlignment)
				{
				case TextAlignmentOptions.TopLeft:
				case TextAlignmentOptions.Top:
				case TextAlignmentOptions.TopRight:
				case TextAlignmentOptions.TopJustified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						vector7 = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_maxAscender - margin.y, 0f);
					}
					else
					{
						vector7 = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].ascender - margin.y, 0f);
					}
					break;
				case TextAlignmentOptions.Left:
				case TextAlignmentOptions.Center:
				case TextAlignmentOptions.Right:
				case TextAlignmentOptions.Justified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						vector7 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxAscender + margin.y + num9 - margin.w) / 2f, 0f);
					}
					else
					{
						vector7 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_textInfo.pageInfo[num6].ascender + margin.y + this.m_textInfo.pageInfo[num6].descender - margin.w) / 2f, 0f);
					}
					break;
				case TextAlignmentOptions.BottomLeft:
				case TextAlignmentOptions.Bottom:
				case TextAlignmentOptions.BottomRight:
				case TextAlignmentOptions.BottomJustified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						vector7 = rectTransformCorners[0] + new Vector3(margin.x, 0f - num9 + margin.w, 0f);
					}
					else
					{
						vector7 = rectTransformCorners[0] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].descender + margin.w, 0f);
					}
					break;
				case TextAlignmentOptions.BaselineLeft:
				case TextAlignmentOptions.Baseline:
				case TextAlignmentOptions.BaselineRight:
				case TextAlignmentOptions.BaselineJustified:
					vector7 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
					break;
				case TextAlignmentOptions.MidlineLeft:
				case TextAlignmentOptions.Midline:
				case TextAlignmentOptions.MidlineRight:
				case TextAlignmentOptions.MidlineJustified:
					vector7 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_meshExtents.max.y + margin.y + this.m_meshExtents.min.y - margin.w) / 2f, 0f);
					break;
				}
				Vector3 vector8 = Vector3.zero;
				Vector3 vector9 = Vector3.zero;
				int index_X = 0;
				int index_X2 = 0;
				int num36 = 0;
				int num37 = 0;
				int num38 = 0;
				bool flag9 = false;
				int num39 = 0;
				bool flag10 = !(this.m_canvas.worldCamera == null);
				float num40 = (this.m_rectTransform.lossyScale.y == 0f) ? 1f : this.m_rectTransform.lossyScale.y;
				RenderMode renderMode = this.m_canvas.renderMode;
				float scaleFactor = this.m_canvas.scaleFactor;
				Color32 underlineColor = Color.white;
				Color32 underlineColor2 = Color.white;
				float num41 = 0f;
				float num42 = 0f;
				float num43 = float.PositiveInfinity;
				int num44 = 0;
				float num45 = 0f;
				float num46 = 0f;
				float b = 0f;
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				for (int i = 0; i < this.m_characterCount; i++)
				{
					char character2 = characterInfo[i].character;
					int lineNumber3 = (int)characterInfo[i].lineNumber;
					TMP_LineInfo tmp_LineInfo = this.m_textInfo.lineInfo[lineNumber3];
					num37 = lineNumber3 + 1;
					switch (tmp_LineInfo.alignment)
					{
					case TextAlignmentOptions.TopLeft:
					case TextAlignmentOptions.Left:
					case TextAlignmentOptions.BottomLeft:
					case TextAlignmentOptions.BaselineLeft:
					case TextAlignmentOptions.MidlineLeft:
						if (!this.m_isRightToLeft)
						{
							vector8..ctor(tmp_LineInfo.marginLeft, 0f, 0f);
						}
						else
						{
							vector8..ctor(0f - tmp_LineInfo.maxAdvance, 0f, 0f);
						}
						break;
					case TextAlignmentOptions.Top:
					case TextAlignmentOptions.Center:
					case TextAlignmentOptions.Bottom:
					case TextAlignmentOptions.Baseline:
					case TextAlignmentOptions.Midline:
						vector8..ctor(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - tmp_LineInfo.maxAdvance / 2f, 0f, 0f);
						break;
					case TextAlignmentOptions.TopRight:
					case TextAlignmentOptions.Right:
					case TextAlignmentOptions.BottomRight:
					case TextAlignmentOptions.BaselineRight:
					case TextAlignmentOptions.MidlineRight:
						if (!this.m_isRightToLeft)
						{
							vector8..ctor(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0f, 0f);
						}
						else
						{
							vector8..ctor(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
						}
						break;
					case TextAlignmentOptions.TopJustified:
					case TextAlignmentOptions.Justified:
					case TextAlignmentOptions.BottomJustified:
					case TextAlignmentOptions.BaselineJustified:
					case TextAlignmentOptions.MidlineJustified:
					{
						char character3 = characterInfo[tmp_LineInfo.lastCharacterIndex].character;
						if (!char.IsControl(character3) && lineNumber3 < this.m_lineNumber)
						{
							float num47 = tmp_LineInfo.width - tmp_LineInfo.maxAdvance;
							float num48 = (tmp_LineInfo.spaceCount <= 2) ? 1f : this.m_wordWrappingRatios;
							if (lineNumber3 != num38 || i == 0)
							{
								vector8..ctor(tmp_LineInfo.marginLeft, 0f, 0f);
							}
							else if (character2 == '\t' || char.IsSeparator(character2))
							{
								int num49 = (tmp_LineInfo.spaceCount - 1 <= 0) ? 1 : (tmp_LineInfo.spaceCount - 1);
								vector8 += new Vector3(num47 * (1f - num48) / (float)num49, 0f, 0f);
							}
							else
							{
								vector8 += new Vector3(num47 * num48 / (float)(tmp_LineInfo.characterCount - tmp_LineInfo.spaceCount - 1), 0f, 0f);
							}
						}
						else
						{
							vector8..ctor(tmp_LineInfo.marginLeft, 0f, 0f);
						}
						break;
					}
					}
					vector9 = vector7 + vector8;
					bool isVisible = characterInfo[i].isVisible;
					if (isVisible)
					{
						TMP_TextElementType elementType = characterInfo[i].elementType;
						if (elementType != TMP_TextElementType.Character)
						{
							if (elementType != TMP_TextElementType.Sprite)
							{
							}
						}
						else
						{
							Extents lineExtents = tmp_LineInfo.lineExtents;
							float num50 = this.m_uvLineOffset * (float)lineNumber3 % 1f + this.m_uvOffset.x;
							switch (this.m_horizontalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.x = this.m_uvOffset.x;
								characterInfo[i].vertex_TL.uv2.x = this.m_uvOffset.x;
								characterInfo[i].vertex_TR.uv2.x = 1f + this.m_uvOffset.x;
								characterInfo[i].vertex_BR.uv2.x = 1f + this.m_uvOffset.x;
								break;
							case TextureMappingOptions.Line:
								if (this.m_textAlignment != TextAlignmentOptions.Justified)
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num50;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num50;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num50;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num50;
								}
								else
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
								}
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
								characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
								characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
								characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector8.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num50;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								switch (this.m_verticalMapping)
								{
								case TextureMappingOptions.Character:
									characterInfo[i].vertex_BL.uv2.y = this.m_uvOffset.y;
									characterInfo[i].vertex_TL.uv2.y = 1f + this.m_uvOffset.y;
									characterInfo[i].vertex_TR.uv2.y = this.m_uvOffset.y;
									characterInfo[i].vertex_BR.uv2.y = 1f + this.m_uvOffset.y;
									break;
								case TextureMappingOptions.Line:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num50;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num50;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								case TextureMappingOptions.Paragraph:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num50;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num50;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								}
								float num51 = (1f - (characterInfo[i].vertex_BL.uv2.y + characterInfo[i].vertex_TL.uv2.y) * characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.x = characterInfo[i].vertex_BL.uv2.y * characterInfo[i].aspectRatio + num51 + num50;
								characterInfo[i].vertex_TL.uv2.x = characterInfo[i].vertex_BL.uv2.x;
								characterInfo[i].vertex_TR.uv2.x = characterInfo[i].vertex_TL.uv2.y * characterInfo[i].aspectRatio + num51 + num50;
								characterInfo[i].vertex_BR.uv2.x = characterInfo[i].vertex_TR.uv2.x;
								break;
							}
							}
							switch (this.m_verticalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.y = this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = 1f + this.m_uvOffset.y;
								characterInfo[i].vertex_TR.uv2.y = 1f + this.m_uvOffset.y;
								characterInfo[i].vertex_BR.uv2.y = this.m_uvOffset.y;
								break;
							case TextureMappingOptions.Line:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender) + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender) + this.m_uvOffset.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								float num52 = (1f - (characterInfo[i].vertex_BL.uv2.x + characterInfo[i].vertex_TR.uv2.x) / characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.y = num52 + characterInfo[i].vertex_BL.uv2.x / characterInfo[i].aspectRatio + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = num52 + characterInfo[i].vertex_TR.uv2.x / characterInfo[i].aspectRatio + this.m_uvOffset.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								break;
							}
							}
							float num53 = characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
							if ((characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
							{
								num53 *= -1f;
							}
							if (renderMode != null)
							{
								if (renderMode != 1)
								{
									if (renderMode == 2)
									{
										num53 *= num40;
									}
								}
								else
								{
									num53 *= ((!flag10) ? 1f : num40);
								}
							}
							else
							{
								num53 *= num40 / scaleFactor;
							}
							float num54 = characterInfo[i].vertex_BL.uv2.x;
							float num55 = characterInfo[i].vertex_BL.uv2.y;
							float num56 = characterInfo[i].vertex_TR.uv2.x;
							float num57 = characterInfo[i].vertex_TR.uv2.y;
							float num58 = Mathf.Floor(num54);
							float num59 = Mathf.Floor(num55);
							num54 -= num58;
							num56 -= num58;
							num55 -= num59;
							num57 -= num59;
							characterInfo[i].vertex_BL.uv2.x = base.PackUV(num54, num55);
							characterInfo[i].vertex_BL.uv2.y = num53;
							characterInfo[i].vertex_TL.uv2.x = base.PackUV(num54, num57);
							characterInfo[i].vertex_TL.uv2.y = num53;
							characterInfo[i].vertex_TR.uv2.x = base.PackUV(num56, num57);
							characterInfo[i].vertex_TR.uv2.y = num53;
							characterInfo[i].vertex_BR.uv2.x = base.PackUV(num56, num55);
							characterInfo[i].vertex_BR.uv2.y = num53;
						}
						if (i < this.m_maxVisibleCharacters && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode != TextOverflowModes.Page)
						{
							TMP_CharacterInfo[] array = characterInfo;
							int num60 = i;
							array[num60].vertex_BL.position = array[num60].vertex_BL.position + vector9;
							TMP_CharacterInfo[] array2 = characterInfo;
							int num61 = i;
							array2[num61].vertex_TL.position = array2[num61].vertex_TL.position + vector9;
							TMP_CharacterInfo[] array3 = characterInfo;
							int num62 = i;
							array3[num62].vertex_TR.position = array3[num62].vertex_TR.position + vector9;
							TMP_CharacterInfo[] array4 = characterInfo;
							int num63 = i;
							array4[num63].vertex_BR.position = array4[num63].vertex_BR.position + vector9;
						}
						else if (i < this.m_maxVisibleCharacters && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode == TextOverflowModes.Page && (int)characterInfo[i].pageNumber == num6)
						{
							TMP_CharacterInfo[] array5 = characterInfo;
							int num64 = i;
							array5[num64].vertex_BL.position = array5[num64].vertex_BL.position + vector9;
							TMP_CharacterInfo[] array6 = characterInfo;
							int num65 = i;
							array6[num65].vertex_TL.position = array6[num65].vertex_TL.position + vector9;
							TMP_CharacterInfo[] array7 = characterInfo;
							int num66 = i;
							array7[num66].vertex_TR.position = array7[num66].vertex_TR.position + vector9;
							TMP_CharacterInfo[] array8 = characterInfo;
							int num67 = i;
							array8[num67].vertex_BR.position = array8[num67].vertex_BR.position + vector9;
						}
						else
						{
							characterInfo[i].vertex_BL.position = Vector3.zero;
							characterInfo[i].vertex_TL.position = Vector3.zero;
							characterInfo[i].vertex_TR.position = Vector3.zero;
							characterInfo[i].vertex_BR.position = Vector3.zero;
						}
						if (elementType == TMP_TextElementType.Character)
						{
							this.FillCharacterVertexBuffers(i, index_X);
						}
						else if (elementType == TMP_TextElementType.Sprite)
						{
							this.FillSpriteVertexBuffers(i, index_X2);
						}
					}
					TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
					int num68 = i;
					characterInfo2[num68].bottomLeft = characterInfo2[num68].bottomLeft + vector9;
					TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
					int num69 = i;
					characterInfo3[num69].topLeft = characterInfo3[num69].topLeft + vector9;
					TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
					int num70 = i;
					characterInfo4[num70].topRight = characterInfo4[num70].topRight + vector9;
					TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
					int num71 = i;
					characterInfo5[num71].bottomRight = characterInfo5[num71].bottomRight + vector9;
					TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
					int num72 = i;
					characterInfo6[num72].origin = characterInfo6[num72].origin + vector9.x;
					TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
					int num73 = i;
					characterInfo7[num73].xAdvance = characterInfo7[num73].xAdvance + vector9.x;
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num74 = i;
					characterInfo8[num74].ascender = characterInfo8[num74].ascender + vector9.y;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num75 = i;
					characterInfo9[num75].descender = characterInfo9[num75].descender + vector9.y;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num76 = i;
					characterInfo10[num76].baseLine = characterInfo10[num76].baseLine + vector9.y;
					if (isVisible)
					{
					}
					if (lineNumber3 != num38 || i == this.m_characterCount - 1)
					{
						if (lineNumber3 != num38)
						{
							this.m_textInfo.lineInfo[num38].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num38].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[num38].descender);
							this.m_textInfo.lineInfo[num38].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num38].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[num38].ascender);
							TMP_LineInfo[] lineInfo3 = this.m_textInfo.lineInfo;
							int num77 = num38;
							lineInfo3[num77].baseline = lineInfo3[num77].baseline + vector9.y;
							TMP_LineInfo[] lineInfo4 = this.m_textInfo.lineInfo;
							int num78 = num38;
							lineInfo4[num78].ascender = lineInfo4[num78].ascender + vector9.y;
							TMP_LineInfo[] lineInfo5 = this.m_textInfo.lineInfo;
							int num79 = num38;
							lineInfo5[num79].descender = lineInfo5[num79].descender + vector9.y;
						}
						if (i == this.m_characterCount - 1)
						{
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[lineNumber3].descender);
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[lineNumber3].ascender);
							TMP_LineInfo[] lineInfo6 = this.m_textInfo.lineInfo;
							int num80 = lineNumber3;
							lineInfo6[num80].baseline = lineInfo6[num80].baseline + vector9.y;
							TMP_LineInfo[] lineInfo7 = this.m_textInfo.lineInfo;
							int num81 = lineNumber3;
							lineInfo7[num81].ascender = lineInfo7[num81].ascender + vector9.y;
							TMP_LineInfo[] lineInfo8 = this.m_textInfo.lineInfo;
							int num82 = lineNumber3;
							lineInfo8[num82].descender = lineInfo8[num82].descender + vector9.y;
						}
					}
					if (char.IsLetterOrDigit(character2) || character2 == '-' || character2 == '’')
					{
						if (!flag9)
						{
							flag9 = true;
							num39 = i;
						}
						if (flag9 && i == this.m_characterCount - 1)
						{
							int num83 = this.m_textInfo.wordInfo.Length;
							int wordCount = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num83)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num83 + 1);
							}
							int num84 = i;
							this.m_textInfo.wordInfo[wordCount].firstCharacterIndex = num39;
							this.m_textInfo.wordInfo[wordCount].lastCharacterIndex = num84;
							this.m_textInfo.wordInfo[wordCount].characterCount = num84 - num39 + 1;
							this.m_textInfo.wordInfo[wordCount].textComponent = this;
							num36++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo9 = this.m_textInfo.lineInfo;
							int num85 = lineNumber3;
							lineInfo9[num85].wordCount = lineInfo9[num85].wordCount + 1;
						}
					}
					else if (flag9 || (i == 0 && (!char.IsPunctuation(character2) || char.IsWhiteSpace(character2) || i == this.m_characterCount - 1)))
					{
						if (i <= 0 || i >= this.m_characterCount || character2 != '\'' || !char.IsLetterOrDigit(characterInfo[i - 1].character) || !char.IsLetterOrDigit(characterInfo[i + 1].character))
						{
							int num84 = (i != this.m_characterCount - 1 || !char.IsLetterOrDigit(character2)) ? (i - 1) : i;
							flag9 = false;
							int num86 = this.m_textInfo.wordInfo.Length;
							int wordCount2 = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num86)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num86 + 1);
							}
							this.m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num39;
							this.m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num84;
							this.m_textInfo.wordInfo[wordCount2].characterCount = num84 - num39 + 1;
							this.m_textInfo.wordInfo[wordCount2].textComponent = this;
							num36++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo10 = this.m_textInfo.lineInfo;
							int num87 = lineNumber3;
							lineInfo10[num87].wordCount = lineInfo10[num87].wordCount + 1;
						}
					}
					bool flag11 = (this.m_textInfo.characterInfo[i].style & FontStyles.Underline) == FontStyles.Underline;
					if (flag11)
					{
						bool flag12 = true;
						int pageNumber = (int)this.m_textInfo.characterInfo[i].pageNumber;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != this.m_pageToDisplay))
						{
							flag12 = false;
						}
						if (!char.IsWhiteSpace(character2))
						{
							num42 = Mathf.Max(num42, this.m_textInfo.characterInfo[i].scale);
							num43 = Mathf.Min((pageNumber != num44) ? float.PositiveInfinity : num43, this.m_textInfo.characterInfo[i].baseLine + base.font.fontInfo.Underline * num42);
							num44 = pageNumber;
						}
						if (!flag && flag12 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
							{
								flag = true;
								num41 = this.m_textInfo.characterInfo[i].scale;
								if (num42 == 0f)
								{
									num42 = num41;
								}
								zero..ctor(this.m_textInfo.characterInfo[i].bottomLeft.x, num43, 0f);
								underlineColor = this.m_textInfo.characterInfo[i].color;
							}
						}
						if (flag && this.m_characterCount == 1)
						{
							flag = false;
							zero2..ctor(this.m_textInfo.characterInfo[i].topRight.x, num43, 0f);
							float scale = this.m_textInfo.characterInfo[i].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num41, scale, num42, underlineColor);
							num42 = 0f;
							num43 = float.PositiveInfinity;
						}
						else if (flag && (i == tmp_LineInfo.lastCharacterIndex || i >= tmp_LineInfo.lastVisibleCharacterIndex))
						{
							float scale;
							if (char.IsWhiteSpace(character2))
							{
								int lastVisibleCharacterIndex = tmp_LineInfo.lastVisibleCharacterIndex;
								zero2..ctor(this.m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num43, 0f);
								scale = this.m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
							}
							else
							{
								zero2..ctor(this.m_textInfo.characterInfo[i].topRight.x, num43, 0f);
								scale = this.m_textInfo.characterInfo[i].scale;
							}
							flag = false;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num41, scale, num42, underlineColor);
							num42 = 0f;
							num43 = float.PositiveInfinity;
						}
						else if (flag && !flag12)
						{
							flag = false;
							zero2..ctor(this.m_textInfo.characterInfo[i - 1].topRight.x, num43, 0f);
							float scale = this.m_textInfo.characterInfo[i - 1].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num41, scale, num42, underlineColor);
							num42 = 0f;
							num43 = float.PositiveInfinity;
						}
					}
					else if (flag)
					{
						flag = false;
						zero2..ctor(this.m_textInfo.characterInfo[i - 1].topRight.x, num43, 0f);
						float scale = this.m_textInfo.characterInfo[i - 1].scale;
						this.DrawUnderlineMesh(zero, zero2, ref num35, num41, scale, num42, underlineColor);
						num42 = 0f;
						num43 = float.PositiveInfinity;
					}
					bool flag13 = (this.m_textInfo.characterInfo[i].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
					if (flag13)
					{
						bool flag14 = true;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && (int)(this.m_textInfo.characterInfo[i].pageNumber + 1) != this.m_pageToDisplay))
						{
							flag14 = false;
						}
						if (!flag2 && flag14 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
							{
								flag2 = true;
								num45 = this.m_textInfo.characterInfo[i].pointSize;
								num46 = this.m_textInfo.characterInfo[i].scale;
								zero3..ctor(this.m_textInfo.characterInfo[i].bottomLeft.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2.75f * num46, 0f);
								underlineColor2 = this.m_textInfo.characterInfo[i].color;
								b = this.m_textInfo.characterInfo[i].baseLine;
							}
						}
						if (flag2 && this.m_characterCount == 1)
						{
							flag2 = false;
							zero4..ctor(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num46, num46, num46, underlineColor2);
						}
						else if (flag2 && i == tmp_LineInfo.lastCharacterIndex)
						{
							if (char.IsWhiteSpace(character2))
							{
								int lastVisibleCharacterIndex2 = tmp_LineInfo.lastVisibleCharacterIndex;
								zero4..ctor(this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							}
							else
							{
								zero4..ctor(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							}
							flag2 = false;
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num46, num46, num46, underlineColor2);
						}
						else if (flag2 && i < this.m_characterCount && (this.m_textInfo.characterInfo[i + 1].pointSize != num45 || !TMP_Math.Approximately(this.m_textInfo.characterInfo[i + 1].baseLine + vector9.y, b)))
						{
							flag2 = false;
							int lastVisibleCharacterIndex3 = tmp_LineInfo.lastVisibleCharacterIndex;
							if (i > lastVisibleCharacterIndex3)
							{
								zero4..ctor(this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							}
							else
							{
								zero4..ctor(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							}
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num46, num46, num46, underlineColor2);
						}
						else if (flag2 && !flag14)
						{
							flag2 = false;
							zero4..ctor(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num46, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num46, num46, num46, underlineColor2);
						}
					}
					else if (flag2)
					{
						flag2 = false;
						zero4..ctor(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * this.m_fontScale, 0f);
						this.DrawUnderlineMesh(zero3, zero4, ref num35, num46, num46, num46, underlineColor2);
					}
					num38 = lineNumber3;
				}
				this.m_textInfo.characterCount = (int)((short)this.m_characterCount);
				this.m_textInfo.spriteCount = this.m_spriteCount;
				this.m_textInfo.lineCount = (int)((short)num37);
				this.m_textInfo.wordCount = (int)((num36 == 0 || this.m_characterCount <= 0) ? 1 : ((short)num36));
				this.m_textInfo.pageCount = this.m_pageNumber + 1;
				if (this.m_renderMode == TextRenderFlags.Render)
				{
					this.m_mesh.MarkDynamic();
					this.m_mesh.vertices = this.m_textInfo.meshInfo[0].vertices;
					this.m_mesh.uv = this.m_textInfo.meshInfo[0].uvs0;
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_mesh.colors32 = this.m_textInfo.meshInfo[0].colors32;
					this.m_mesh.RecalculateBounds();
					this.m_canvasRenderer.SetMesh(this.m_mesh);
					for (int j = 1; j < this.m_textInfo.materialCount; j++)
					{
						this.m_textInfo.meshInfo[j].ClearUnusedVertices();
						this.m_subTextObjects[j].mesh.MarkDynamic();
						this.m_subTextObjects[j].mesh.vertices = this.m_textInfo.meshInfo[j].vertices;
						this.m_subTextObjects[j].mesh.uv = this.m_textInfo.meshInfo[j].uvs0;
						this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
						this.m_subTextObjects[j].mesh.colors32 = this.m_textInfo.meshInfo[j].colors32;
						this.m_subTextObjects[j].mesh.RecalculateBounds();
						this.m_subTextObjects[j].canvasRenderer.SetMesh(this.m_subTextObjects[j].mesh);
					}
				}
				return;
			}
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x00037434 File Offset: 0x00035634
		public override Vector3[] GetTextContainerLocalCorners()
		{
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = base.rectTransform;
			}
			this.m_rectTransform.GetLocalCorners(this.m_RectTransformCorners);
			return this.m_RectTransformCorners;
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x0014D9CC File Offset: 0x0014BBCC
		public void ClearMesh()
		{
			this.m_canvasRenderer.SetMesh(null);
			int count = this.m_materialReferenceIndexLookup.Count;
			for (int i = 1; i < count; i++)
			{
				this.m_subTextObjects[i].canvasRenderer.SetMesh(null);
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x0014DA18 File Offset: 0x0014BC18
		public void UpdateSDFScale(float lossyScale)
		{
			lossyScale = ((lossyScale != 0f) ? lossyScale : 1f);
			float scaleFactor = this.m_canvas.scaleFactor;
			float num;
			if (this.m_canvas.renderMode == null)
			{
				num = lossyScale / scaleFactor;
			}
			else if (this.m_canvas.renderMode == 1)
			{
				num = ((!(this.m_canvas.worldCamera != null)) ? 1f : lossyScale);
			}
			else
			{
				num = lossyScale;
			}
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if (this.m_textInfo.characterInfo[i].isVisible && this.m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
				{
					float num2 = num * this.m_textInfo.characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
					if ((this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num2 *= -1f;
					}
					int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = (int)this.m_textInfo.characterInfo[i].vertexIndex;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num2;
				}
			}
			for (int j = 0; j < this.m_textInfo.materialCount; j++)
			{
				if (j == 0)
				{
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_canvasRenderer.SetMesh(this.m_mesh);
				}
				else
				{
					this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
					this.m_subTextObjects[j].canvasRenderer.SetMesh(this.m_subTextObjects[j].mesh);
				}
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0014DCB0 File Offset: 0x0014BEB0
		public override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			Vector3 vector;
			vector..ctor(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int num = i;
				characterInfo[num].bottomLeft = characterInfo[num].bottomLeft - vector;
				TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
				int num2 = i;
				characterInfo2[num2].topLeft = characterInfo2[num2].topLeft - vector;
				TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
				int num3 = i;
				characterInfo3[num3].topRight = characterInfo3[num3].topRight - vector;
				TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
				int num4 = i;
				characterInfo4[num4].bottomRight = characterInfo4[num4].bottomRight - vector;
				TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
				int num5 = i;
				characterInfo5[num5].ascender = characterInfo5[num5].ascender - vector.y;
				TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
				int num6 = i;
				characterInfo6[num6].baseLine = characterInfo6[num6].baseLine - vector.y;
				TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
				int num7 = i;
				characterInfo7[num7].descender = characterInfo7[num7].descender - vector.y;
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num8 = i;
					characterInfo8[num8].vertex_BL.position = characterInfo8[num8].vertex_BL.position - vector;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num9 = i;
					characterInfo9[num9].vertex_TL.position = characterInfo9[num9].vertex_TL.position - vector;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num10 = i;
					characterInfo10[num10].vertex_TR.position = characterInfo10[num10].vertex_TR.position - vector;
					TMP_CharacterInfo[] characterInfo11 = this.m_textInfo.characterInfo;
					int num11 = i;
					characterInfo11[num11].vertex_BR.position = characterInfo11[num11].vertex_BR.position - vector;
				}
			}
		}

		// Token: 0x0400357D RID: 13693
		public bool m_isRebuildingLayout;

		// Token: 0x0400357E RID: 13694
		[SerializeField]
		public Vector2 m_uvOffset = Vector2.zero;

		// Token: 0x0400357F RID: 13695
		[SerializeField]
		public float m_uvLineOffset;

		// Token: 0x04003580 RID: 13696
		[SerializeField]
		public bool m_hasFontAssetChanged;

		// Token: 0x04003581 RID: 13697
		[SerializeField]
		public TMP_SubMeshUI[] m_subTextObjects = new TMP_SubMeshUI[8];

		// Token: 0x04003582 RID: 13698
		public Vector3 m_previousLossyScale;

		// Token: 0x04003583 RID: 13699
		public Vector3[] m_RectTransformCorners = new Vector3[4];

		// Token: 0x04003584 RID: 13700
		public CanvasRenderer m_canvasRenderer;

		// Token: 0x04003585 RID: 13701
		public Canvas m_canvas;

		// Token: 0x04003586 RID: 13702
		public bool m_isFirstAllocation;

		// Token: 0x04003587 RID: 13703
		public int m_max_characters = 8;

		// Token: 0x04003588 RID: 13704
		public WordWrapState m_SavedWordWrapState = default(WordWrapState);

		// Token: 0x04003589 RID: 13705
		public WordWrapState m_SavedLineState = default(WordWrapState);

		// Token: 0x0400358A RID: 13706
		public bool m_isMaskingEnabled;

		// Token: 0x0400358B RID: 13707
		[SerializeField]
		public Material m_baseMaterial;

		// Token: 0x0400358C RID: 13708
		public bool m_isScrollRegionSet;

		// Token: 0x0400358D RID: 13709
		public int m_stencilID;

		// Token: 0x0400358E RID: 13710
		[SerializeField]
		public Vector4 m_maskOffset;

		// Token: 0x0400358F RID: 13711
		public Matrix4x4 m_EnvMapMatrix = default(Matrix4x4);

		// Token: 0x04003590 RID: 13712
		public bool m_isAwake;

		// Token: 0x04003591 RID: 13713
		[NonSerialized]
		public bool m_isRegisteredForEvents;

		// Token: 0x04003592 RID: 13714
		public int m_recursiveCount;

		// Token: 0x04003593 RID: 13715
		public int m_recursiveCountA;

		// Token: 0x04003594 RID: 13716
		public int loopCountA;
	}
}
