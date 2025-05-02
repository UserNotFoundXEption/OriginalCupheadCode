using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200060E RID: 1550
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x00033969 File Offset: 0x00031B69
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.supportsComputeShaders && !this.context.interrupted;
			}
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x00033996 File Offset: 0x00031B96
		public void ResetHistory()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0003399F File Offset: 0x00031B9F
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x0012C7B8 File Offset: 0x0012A9B8
		public override void OnDisable()
		{
			foreach (RenderTexture obj in this.m_AutoExposurePool)
			{
				GraphicsUtils.Destroy(obj);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0012C82C File Offset: 0x0012AA2C
		public Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float num3 = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, num3, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x0012C8A0 File Offset: 0x0012AAA0
		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			material.shaderKeywords = null;
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, 0, 1, 1, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, 14);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, 14);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int num = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(num, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(num, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(num, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue);
			if (settings.dynamicKeyValue)
			{
				material.EnableKeyword("AUTO_KEY_VALUE");
			}
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num2 = this.m_AutoExposurePingPing;
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num2 % 2];
				RenderTexture renderTexture3 = this.m_AutoExposurePool[++num2 % 2];
				Graphics.Blit(renderTexture2, renderTexture3, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num2 + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture3;
			}
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, 0)
					{
						filterMode = 0,
						wrapMode = 1
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0012CCA4 File Offset: 0x0012AEA4
		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			Rect rect;
			rect..ctor(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height);
			GUI.DrawTexture(rect, this.m_DebugHistogram);
		}

		// Token: 0x04003339 RID: 13113
		public ComputeShader m_EyeCompute;

		// Token: 0x0400333A RID: 13114
		public ComputeBuffer m_HistogramBuffer;

		// Token: 0x0400333B RID: 13115
		public readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		// Token: 0x0400333C RID: 13116
		public int m_AutoExposurePingPing;

		// Token: 0x0400333D RID: 13117
		public RenderTexture m_CurrentAutoExposure;

		// Token: 0x0400333E RID: 13118
		public RenderTexture m_DebugHistogram;

		// Token: 0x0400333F RID: 13119
		public static uint[] s_EmptyHistogramBuffer;

		// Token: 0x04003340 RID: 13120
		public bool m_FirstFrame = true;

		// Token: 0x04003341 RID: 13121
		public const int k_HistogramBins = 64;

		// Token: 0x04003342 RID: 13122
		public const int k_HistogramThreadX = 16;

		// Token: 0x04003343 RID: 13123
		public const int k_HistogramThreadY = 16;

		// Token: 0x02001275 RID: 4725
		public static class Uniforms
		{
			// Token: 0x04007F73 RID: 32627
			public static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x04007F74 RID: 32628
			public static readonly int _Speed = Shader.PropertyToID("_Speed");

			// Token: 0x04007F75 RID: 32629
			public static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			// Token: 0x04007F76 RID: 32630
			public static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			// Token: 0x04007F77 RID: 32631
			public static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x04007F78 RID: 32632
			public static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}
