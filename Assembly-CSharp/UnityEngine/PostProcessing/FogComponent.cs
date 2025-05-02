using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200060F RID: 1551
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x000339B0 File Offset: 0x00031BB0
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x000339ED File Offset: 0x00031BED
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x000339F4 File Offset: 0x00031BF4
		public override DepthTextureMode GetCameraFlags()
		{
			return 1;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x000339F7 File Offset: 0x00031BF7
		public override CameraEvent GetCameraEvent()
		{
			return 13;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0012CD24 File Offset: 0x0012AF24
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color color = (!GraphicsUtils.isLinearColorSpace) ? RenderSettings.fogColor : RenderSettings.fogColor.linear;
			material.SetColor(FogComponent.Uniforms._FogColor, color);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != 1)
			{
				if (fogMode != 2)
				{
					if (fogMode == 3)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 7 : 9;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, 1, renderTextureFormat);
			cb.Blit(2, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, 2, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x04003344 RID: 13124
		public const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x02001276 RID: 4726
		public static class Uniforms
		{
			// Token: 0x04007F79 RID: 32633
			public static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04007F7A RID: 32634
			public static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x04007F7B RID: 32635
			public static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x04007F7C RID: 32636
			public static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x04007F7D RID: 32637
			public static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
