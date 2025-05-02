using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B1 RID: 1713
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x06004795 RID: 18325 RVA: 0x00038E05 File Offset: 0x00037005
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x00038E3E File Offset: 0x0003703E
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0015D618 File Offset: 0x0015B818
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = (float)destination.width / (float)destination.height;
			float num2 = (num >= 1.77777779f) ? 1f : (num / 1.77777779f);
			num2 *= 1f - 0.1f * SettingsData.Data.overscan;
			float num3 = (float)destination.height / 1080f * 1f / (1f * (float)(1 << this.downsample));
			num3 *= num2;
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3, -this.blurSize * num3, 0f, 0f));
			source.filterMode = 1;
			int num4 = source.width >> this.downsample;
			int num5 = source.height >> this.downsample;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num4, num5, 0, source.format);
			renderTexture.filterMode = 1;
			Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
			int num6 = (this.blurType != BlurOptimized.BlurType.StandardGauss) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				float num7 = (float)i * 1f;
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3 + num7, -this.blurSize * num3 - num7, 0f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(num4, num5, 0, source.format);
				temporary.filterMode = 1;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num6);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(num4, num5, 0, source.format);
				temporary.filterMode = 1;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num6);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400389B RID: 14491
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x0400389C RID: 14492
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x0400389D RID: 14493
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x0400389E RID: 14494
		public BlurOptimized.BlurType blurType;

		// Token: 0x0400389F RID: 14495
		public Shader blurShader;

		// Token: 0x040038A0 RID: 14496
		public Material blurMaterial;

		// Token: 0x02001306 RID: 4870
		public enum BlurType
		{
			// Token: 0x04008218 RID: 33304
			StandardGauss,
			// Token: 0x04008219 RID: 33305
			SgxGauss
		}
	}
}
