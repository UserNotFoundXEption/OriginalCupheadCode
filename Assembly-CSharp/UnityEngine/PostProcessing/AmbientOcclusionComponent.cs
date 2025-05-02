using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000607 RID: 1543
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06003FFE RID: 16382 RVA: 0x0012A69C File Offset: 0x0012889C
		public AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && (!this.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility))
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x0012A718 File Offset: 0x00128918
		public bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x0012A778 File Offset: 0x00128978
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0012A7C4 File Offset: 0x001289C4
		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = 0;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= 1;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= 2;
			}
			return depthTextureMode;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x00033686 File Offset: 0x00031886
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0003368D File Offset: 0x0003188D
		public override CameraEvent GetCameraEvent()
		{
			return (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? 12 : 21;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0012A7F4 File Offset: 0x001289F4
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material2.shaderKeywords = null;
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, (!settings.downsampling) ? 1f : 0.5f);
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			if (!this.context.isGBufferAvailable && RenderSettings.fog)
			{
				material2.SetVector(AmbientOcclusionComponent.Uniforms._FogParams, new Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));
				FogMode fogMode = RenderSettings.fogMode;
				if (fogMode != 1)
				{
					if (fogMode != 2)
					{
						if (fogMode == 3)
						{
							material2.EnableKeyword("FOG_EXP2");
						}
					}
					else
					{
						material2.EnableKeyword("FOG_EXP");
					}
				}
				else
				{
					material2.EnableKeyword("FOG_LINEAR");
				}
			}
			else
			{
				material2.EnableKeyword("FOG_OFF");
			}
			int width = this.context.width;
			int height = this.context.height;
			int num = (!settings.downsampling) ? 1 : 2;
			int num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(num2, width / num, height / num, 0, 1, 0, 1);
			cb.Blit(null, num2, material2, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
			cb.Blit(num2, occlusionTexture, material2, (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 3 : 4);
			cb.ReleaseTemporaryRT(num2);
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(num2, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, num2, material2, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
				cb.Blit(num2, 2, material2, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, 2);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
			}
			else
			{
				RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 7 : 9;
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1, renderTextureFormat);
				cb.Blit(2, tempRT, material, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, 2, material2, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(num2);
		}

		// Token: 0x04003325 RID: 13093
		public const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x04003326 RID: 13094
		public const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x04003327 RID: 13095
		public readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			10,
			2
		};

		// Token: 0x0200126B RID: 4715
		public static class Uniforms
		{
			// Token: 0x04007F28 RID: 32552
			public static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04007F29 RID: 32553
			public static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x04007F2A RID: 32554
			public static readonly int _FogParams = Shader.PropertyToID("_FogParams");

			// Token: 0x04007F2B RID: 32555
			public static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x04007F2C RID: 32556
			public static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			// Token: 0x04007F2D RID: 32557
			public static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			// Token: 0x04007F2E RID: 32558
			public static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			// Token: 0x04007F2F RID: 32559
			public static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			// Token: 0x04007F30 RID: 32560
			public static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04007F31 RID: 32561
			public static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		// Token: 0x0200126C RID: 4716
		public enum OcclusionSource
		{
			// Token: 0x04007F33 RID: 32563
			DepthTexture,
			// Token: 0x04007F34 RID: 32564
			DepthNormalsTexture,
			// Token: 0x04007F35 RID: 32565
			GBuffer
		}
	}
}
