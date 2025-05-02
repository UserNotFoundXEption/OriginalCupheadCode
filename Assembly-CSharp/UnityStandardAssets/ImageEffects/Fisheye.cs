using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BC RID: 1724
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	public class Fisheye : PostEffectsBase
	{
		// Token: 0x060047E6 RID: 18406 RVA: 0x00039179 File Offset: 0x00037379
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x00161728 File Offset: 0x0015F928
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 0.15625f;
			float num2 = (float)source.width * 1f / ((float)source.height * 1f);
			this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
			Graphics.Blit(source, destination, this.fisheyeMaterial);
		}

		// Token: 0x04003947 RID: 14663
		public float strengthX = 0.05f;

		// Token: 0x04003948 RID: 14664
		public float strengthY = 0.05f;

		// Token: 0x04003949 RID: 14665
		public Shader fishEyeShader;

		// Token: 0x0400394A RID: 14666
		public Material fisheyeMaterial;
	}
}
