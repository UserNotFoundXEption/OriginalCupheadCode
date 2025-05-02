using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BF RID: 1727
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x060047EF RID: 18415 RVA: 0x00039262 File Offset: 0x00037462
		public virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060047F0 RID: 18416 RVA: 0x0003929D File Offset: 0x0003749D
		public Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = 61;
				}
				return this.m_Material;
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x000392D4 File Offset: 0x000374D4
		public virtual void OnDisable()
		{
			if (this.m_Material)
			{
				Object.DestroyImmediate(this.m_Material);
			}
		}

		// Token: 0x04003955 RID: 14677
		public Shader shader;

		// Token: 0x04003956 RID: 14678
		public Material m_Material;
	}
}
