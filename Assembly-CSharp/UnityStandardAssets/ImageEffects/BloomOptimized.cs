using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006AF RID: 1711
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x00038D24 File Offset: 0x00036F24
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x00038D5D File Offset: 0x00036F5D
		public void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0015D2A8 File Offset: 0x0015B4A8
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (this.resolution != BloomOptimized.Resolution.Low) ? 2 : 4;
			float num2 = (this.resolution != BloomOptimized.Resolution.Low) ? 1f : 0.5f;
			this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
			source.filterMode = 1;
			int num3 = source.width / num;
			int num4 = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num3, num4, 0, source.format);
			renderTexture.filterMode = 1;
			Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
			int num5 = (this.blurType != BloomOptimized.BlurType.Standard) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
				RenderTexture temporary = RenderTexture.GetTemporary(num3, num4, 0, source.format);
				temporary.filterMode = 1;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 2 + num5);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(num3, num4, 0, source.format);
				temporary.filterMode = 1;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 3 + num5);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400388F RID: 14479
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x04003890 RID: 14480
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x04003891 RID: 14481
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x04003892 RID: 14482
		public BloomOptimized.Resolution resolution;

		// Token: 0x04003893 RID: 14483
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x04003894 RID: 14484
		public BloomOptimized.BlurType blurType;

		// Token: 0x04003895 RID: 14485
		public Shader fastBloomShader;

		// Token: 0x04003896 RID: 14486
		public Material fastBloomMaterial;

		// Token: 0x02001304 RID: 4868
		public enum Resolution
		{
			// Token: 0x04008212 RID: 33298
			Low,
			// Token: 0x04008213 RID: 33299
			High
		}

		// Token: 0x02001305 RID: 4869
		public enum BlurType
		{
			// Token: 0x04008215 RID: 33301
			Standard,
			// Token: 0x04008216 RID: 33302
			Sgx
		}
	}
}
