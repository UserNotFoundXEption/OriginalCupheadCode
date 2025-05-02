using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200060C RID: 1548
	public sealed class DepthOfFieldComponent : PostProcessingComponentRenderTexture<DepthOfFieldModel>
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x000338B2 File Offset: 0x00031AB2
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x000338D5 File Offset: 0x00031AD5
		public override DepthTextureMode GetCameraFlags()
		{
			return 1;
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0012C2BC File Offset: 0x0012A4BC
		public float CalculateFocalLength()
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			if (!settings.useCameraFov)
			{
				return settings.focalLength / 1000f;
			}
			float num = this.context.camera.fieldOfView * 0.0174532924f;
			return 0.012f / Mathf.Tan(0.5f * num);
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0012C318 File Offset: 0x0012A518
		public float CalculateMaxCoCRadius(int screenHeight)
		{
			float num = (float)base.model.settings.kernelSize * 4f + 6f;
			return Mathf.Min(0.05f, num / (float)screenHeight);
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0012C354 File Offset: 0x0012A554
		public bool CheckHistory(int width, int height)
		{
			return this.m_CoCHistory != null && this.m_CoCHistory.IsCreated() && this.m_CoCHistory.width == width && this.m_CoCHistory.height == height;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x000338D8 File Offset: 0x00031AD8
		public RenderTextureFormat SelectFormat(RenderTextureFormat primary, RenderTextureFormat secondary)
		{
			if (SystemInfo.SupportsRenderTextureFormat(primary))
			{
				return primary;
			}
			if (SystemInfo.SupportsRenderTextureFormat(secondary))
			{
				return secondary;
			}
			return 7;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x0012C3A4 File Offset: 0x0012A5A4
		public void Prepare(RenderTexture source, Material uberMaterial, bool antialiasCoC, Vector2 taaJitter, float taaBlending)
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			RenderTextureFormat format = 9;
			RenderTextureFormat renderTextureFormat = this.SelectFormat(16, 15);
			float num = this.CalculateFocalLength();
			float num2 = Mathf.Max(settings.focusDistance, num);
			float num3 = (float)source.width / (float)source.height;
			float num4 = num * num / (settings.aperture * (num2 - num) * 0.024f * 2f);
			float num5 = this.CalculateMaxCoCRadius(source.height);
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Depth Of Field");
			material.SetFloat(DepthOfFieldComponent.Uniforms._Distance, num2);
			material.SetFloat(DepthOfFieldComponent.Uniforms._LensCoeff, num4);
			material.SetFloat(DepthOfFieldComponent.Uniforms._MaxCoC, num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpMaxCoC, 1f / num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpAspect, 1f / num3);
			RenderTexture renderTexture = this.context.renderTextureFactory.Get(this.context.width, this.context.height, 0, renderTextureFormat, 1, 1, 1, "FactoryTempTexture");
			Graphics.Blit(null, renderTexture, material, 0);
			if (antialiasCoC)
			{
				material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
				float num6 = (!this.CheckHistory(this.context.width, this.context.height)) ? 0f : taaBlending;
				material.SetVector(DepthOfFieldComponent.Uniforms._TaaParams, new Vector3(taaJitter.x, taaJitter.y, num6));
				RenderTexture temporary = RenderTexture.GetTemporary(this.context.width, this.context.height, 0, renderTextureFormat);
				Graphics.Blit(this.m_CoCHistory, temporary, material, 1);
				this.context.renderTextureFactory.Release(renderTexture);
				if (this.m_CoCHistory != null)
				{
					RenderTexture.ReleaseTemporary(this.m_CoCHistory);
				}
				renderTexture = (this.m_CoCHistory = temporary);
			}
			RenderTexture renderTexture2 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, format, 0, 1, 1, "FactoryTempTexture");
			material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
			Graphics.Blit(source, renderTexture2, material, 2);
			RenderTexture renderTexture3 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, format, 0, 1, 1, "FactoryTempTexture");
			Graphics.Blit(renderTexture2, renderTexture3, material, (int)(3 + settings.kernelSize));
			Graphics.Blit(renderTexture3, renderTexture2, material, 7);
			uberMaterial.SetVector(DepthOfFieldComponent.Uniforms._DepthOfFieldParams, new Vector3(num2, num4, num5));
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane))
			{
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW");
				this.context.Interrupt();
			}
			else
			{
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldTex, renderTexture2);
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldCoCTex, renderTexture);
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD");
			}
			this.context.renderTextureFactory.Release(renderTexture3);
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000338F5 File Offset: 0x00031AF5
		public override void OnDisable()
		{
			if (this.m_CoCHistory != null)
			{
				RenderTexture.ReleaseTemporary(this.m_CoCHistory);
			}
			this.m_CoCHistory = null;
		}

		// Token: 0x04003333 RID: 13107
		public const string k_ShaderString = "Hidden/Post FX/Depth Of Field";

		// Token: 0x04003334 RID: 13108
		public RenderTexture m_CoCHistory;

		// Token: 0x04003335 RID: 13109
		public const float k_FilmHeight = 0.024f;

		// Token: 0x02001273 RID: 4723
		public static class Uniforms
		{
			// Token: 0x04007F66 RID: 32614
			public static readonly int _DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");

			// Token: 0x04007F67 RID: 32615
			public static readonly int _DepthOfFieldCoCTex = Shader.PropertyToID("_DepthOfFieldCoCTex");

			// Token: 0x04007F68 RID: 32616
			public static readonly int _Distance = Shader.PropertyToID("_Distance");

			// Token: 0x04007F69 RID: 32617
			public static readonly int _LensCoeff = Shader.PropertyToID("_LensCoeff");

			// Token: 0x04007F6A RID: 32618
			public static readonly int _MaxCoC = Shader.PropertyToID("_MaxCoC");

			// Token: 0x04007F6B RID: 32619
			public static readonly int _RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");

			// Token: 0x04007F6C RID: 32620
			public static readonly int _RcpAspect = Shader.PropertyToID("_RcpAspect");

			// Token: 0x04007F6D RID: 32621
			public static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04007F6E RID: 32622
			public static readonly int _CoCTex = Shader.PropertyToID("_CoCTex");

			// Token: 0x04007F6F RID: 32623
			public static readonly int _TaaParams = Shader.PropertyToID("_TaaParams");

			// Token: 0x04007F70 RID: 32624
			public static readonly int _DepthOfFieldParams = Shader.PropertyToID("_DepthOfFieldParams");
		}
	}
}
