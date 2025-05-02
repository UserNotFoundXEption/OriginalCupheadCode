using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000672 RID: 1650
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class TMP_SubMesh : MonoBehaviour
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x0003784E File Offset: 0x00035A4E
		// (set) Token: 0x060045E8 RID: 17896 RVA: 0x00037856 File Offset: 0x00035A56
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

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060045E9 RID: 17897 RVA: 0x0003785F File Offset: 0x00035A5F
		// (set) Token: 0x060045EA RID: 17898 RVA: 0x00037867 File Offset: 0x00035A67
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

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x00037870 File Offset: 0x00035A70
		// (set) Token: 0x060045EC RID: 17900 RVA: 0x0003787E File Offset: 0x00035A7E
		public Material material
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

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x000378B6 File Offset: 0x00035AB6
		// (set) Token: 0x060045EE RID: 17902 RVA: 0x000378BE File Offset: 0x00035ABE
		public Material sharedMaterial
		{
			get
			{
				return this.GetSharedMaterial();
			}
			set
			{
				this.SetSharedMaterial(value);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x000378C7 File Offset: 0x00035AC7
		// (set) Token: 0x060045F0 RID: 17904 RVA: 0x000378CF File Offset: 0x00035ACF
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

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x000378D8 File Offset: 0x00035AD8
		// (set) Token: 0x060045F2 RID: 17906 RVA: 0x000378E0 File Offset: 0x00035AE0
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

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x000378E9 File Offset: 0x00035AE9
		public Renderer renderer
		{
			get
			{
				if (this.m_renderer == null)
				{
					this.m_renderer = base.GetComponent<Renderer>();
				}
				return this.m_renderer;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x0003790E File Offset: 0x00035B0E
		public MeshFilter meshFilter
		{
			get
			{
				if (this.m_meshFilter == null)
				{
					this.m_meshFilter = base.GetComponent<MeshFilter>();
				}
				return this.m_meshFilter;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x0014EEF4 File Offset: 0x0014D0F4
		// (set) Token: 0x060045F6 RID: 17910 RVA: 0x00037933 File Offset: 0x00035B33
		public Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = 61;
					this.meshFilter.mesh = this.m_mesh;
				}
				return this.m_mesh;
			}
			set
			{
				this.m_mesh = value;
			}
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x0014EF44 File Offset: 0x0014D144
		public void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			if (this.m_sharedMaterial != null)
			{
				this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, new Vector4(-10000f, -10000f, 10000f, 10000f));
			}
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x0003793C File Offset: 0x00035B3C
		public void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				Object.DestroyImmediate(this.m_mesh);
			}
			this.m_isRegisteredForEvents = false;
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x0014EFA0 File Offset: 0x0014D1A0
		public static TMP_SubMesh AddSubTextObject(TextMeshPro textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject("TMP SubMesh [" + materialReference.material.name + "]");
			TMP_SubMesh tmp_SubMesh = gameObject.AddComponent<TMP_SubMesh>();
			gameObject.transform.SetParent(textComponent.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = textComponent.gameObject.layer;
			tmp_SubMesh.m_meshFilter = gameObject.GetComponent<MeshFilter>();
			tmp_SubMesh.m_TextComponent = textComponent;
			tmp_SubMesh.m_fontAsset = materialReference.fontAsset;
			tmp_SubMesh.m_spriteAsset = materialReference.spriteAsset;
			tmp_SubMesh.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tmp_SubMesh.SetSharedMaterial(materialReference.material);
			tmp_SubMesh.renderer.sortingLayerID = textComponent.renderer.sortingLayerID;
			tmp_SubMesh.renderer.sortingOrder = textComponent.renderer.sortingOrder;
			return tmp_SubMesh;
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x00037961 File Offset: 0x00035B61
		public void DestroySelf()
		{
			Object.Destroy(base.gameObject, 1f);
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x0014F09C File Offset: 0x0014D29C
		public Material GetMaterial(Material mat)
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = base.GetComponent<Renderer>();
			}
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

		// Token: 0x060045FC RID: 17916 RVA: 0x0014F124 File Offset: 0x0014D324
		public Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00037973 File Offset: 0x00035B73
		public Material GetSharedMaterial()
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = base.GetComponent<Renderer>();
			}
			return this.m_renderer.sharedMaterial;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x0003799D File Offset: 0x00035B9D
		public void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x0014F15C File Offset: 0x0014D35C
		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x000379B8 File Offset: 0x00035BB8
		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x000379CD File Offset: 0x00035BCD
		public void SetVerticesDirty()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.havePropertiesChanged = true;
				this.m_TextComponent.SetVerticesDirty();
			}
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x00037A03 File Offset: 0x00035C03
		public void SetMaterialDirty()
		{
			this.UpdateMaterial();
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x00037A0B File Offset: 0x00035C0B
		public void UpdateMaterial()
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = this.renderer;
			}
			this.m_renderer.sharedMaterial = this.m_sharedMaterial;
		}

		// Token: 0x040035D3 RID: 13779
		[SerializeField]
		public TMP_FontAsset m_fontAsset;

		// Token: 0x040035D4 RID: 13780
		[SerializeField]
		public TMP_SpriteAsset m_spriteAsset;

		// Token: 0x040035D5 RID: 13781
		[SerializeField]
		public Material m_material;

		// Token: 0x040035D6 RID: 13782
		[SerializeField]
		public Material m_sharedMaterial;

		// Token: 0x040035D7 RID: 13783
		[SerializeField]
		public bool m_isDefaultMaterial;

		// Token: 0x040035D8 RID: 13784
		[SerializeField]
		public float m_padding;

		// Token: 0x040035D9 RID: 13785
		[SerializeField]
		public Renderer m_renderer;

		// Token: 0x040035DA RID: 13786
		[SerializeField]
		public MeshFilter m_meshFilter;

		// Token: 0x040035DB RID: 13787
		public Mesh m_mesh;

		// Token: 0x040035DC RID: 13788
		[SerializeField]
		public TextMeshPro m_TextComponent;

		// Token: 0x040035DD RID: 13789
		[NonSerialized]
		public bool m_isRegisteredForEvents;
	}
}
