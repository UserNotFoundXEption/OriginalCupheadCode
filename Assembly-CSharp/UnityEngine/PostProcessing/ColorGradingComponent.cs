using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200060B RID: 1547
	public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06004018 RID: 16408 RVA: 0x00033775 File Offset: 0x00031975
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00033798 File Offset: 0x00031998
		public float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.275095075f;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x0012B550 File Offset: 0x00129750
		public Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float num4 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float num5 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float num6 = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(num4, num5, num6);
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0012B5CC File Offset: 0x001297CC
		public Vector3 CalculateColorBalance(float temperature, float tint)
		{
			float num = temperature / 55f;
			float num2 = tint / 55f;
			float x = 0.31271f - num * ((num >= 0f) ? 0.05f : 0.1f);
			float y = this.StandardIlluminantY(x) + num2 * 0.05f;
			Vector3 vector;
			vector..ctor(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = this.CIExyToLMS(x, y);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x0012B670 File Offset: 0x00129870
		public static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (Mathf.Approximately(num, 0f))
			{
				return new Color(1f, 1f, 1f, c.a);
			}
			Color result = default(Color);
			result.r = c.r / num;
			result.g = c.g / num;
			result.b = c.b / num;
			result.a = c.a;
			return result;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x000337B1 File Offset: 0x000319B1
		public static Vector3 ClampVector(Vector3 v, float min, float max)
		{
			return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x0012B710 File Offset: 0x00129910
		public static Vector3 GetLiftValue(Color lift)
		{
			Color color = ColorGradingComponent.NormalizeColor(lift);
			float num = (color.r + color.g + color.b) / 3f;
			float num2 = (color.r - num) * 0.1f + lift.a;
			float num3 = (color.g - num) * 0.1f + lift.a;
			float num4 = (color.b - num) * 0.1f + lift.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -1f, 1f);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x0012B7A4 File Offset: 0x001299A4
		public static Vector3 GetGammaValue(Color gamma)
		{
			Color color = ColorGradingComponent.NormalizeColor(gamma);
			float num = (color.r + color.g + color.b) / 3f;
			gamma.a *= ((gamma.a >= 0f) ? 5f : 0.8f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0f, 5f);
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x0012B8C0 File Offset: 0x00129AC0
		public static Vector3 GetGainValue(Color gain)
		{
			Color color = ColorGradingComponent.NormalizeColor(gain);
			float num = (color.r + color.g + color.b) / 3f;
			gain.a *= ((gain.a <= 0f) ? 1f : 3f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 4f);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x000337E2 File Offset: 0x000319E2
		public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
		{
			outLift = ColorGradingComponent.GetLiftValue(lift);
			outGamma = ColorGradingComponent.GetGammaValue(gamma);
			outGain = ColorGradingComponent.GetGainValue(gain);
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x0012B9A0 File Offset: 0x00129BA0
		public static Vector3 GetSlopeValue(Color slope)
		{
			Color color = ColorGradingComponent.NormalizeColor(slope);
			float num = (color.r + color.g + color.b) / 3f;
			slope.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + slope.a + 1f;
			float num3 = (color.g - num) * 0.1f + slope.a + 1f;
			float num4 = (color.b - num) * 0.1f + slope.a + 1f;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 2f);
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x0012BA58 File Offset: 0x00129C58
		public static Vector3 GetPowerValue(Color power)
		{
			Color color = ColorGradingComponent.NormalizeColor(power);
			float num = (color.r + color.g + color.b) / 3f;
			power.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + power.a + 1f;
			float num3 = (color.g - num) * 0.1f + power.a + 1f;
			float num4 = (color.b - num) * 0.1f + power.a + 1f;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0.5f, 2.5f);
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x0012BB4C File Offset: 0x00129D4C
		public static Vector3 GetOffsetValue(Color offset)
		{
			Color color = ColorGradingComponent.NormalizeColor(offset);
			float num = (color.r + color.g + color.b) / 3f;
			offset.a *= 0.5f;
			float num2 = (color.r - num) * 0.05f + offset.a;
			float num3 = (color.g - num) * 0.05f + offset.a;
			float num4 = (color.b - num) * 0.05f + offset.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -0.8f, 0.8f);
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0003380A File Offset: 0x00031A0A
		public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
		{
			outSlope = ColorGradingComponent.GetSlopeValue(slope);
			outPower = ColorGradingComponent.GetPowerValue(power);
			outOffset = ColorGradingComponent.GetOffsetValue(offset);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00033832 File Offset: 0x00031A32
		public TextureFormat GetCurveFormat()
		{
			if (SystemInfo.SupportsTextureFormat(17))
			{
				return 17;
			}
			return 4;
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x0012BBF4 File Offset: 0x00129DF4
		public Texture2D GetCurveTexture()
		{
			if (this.m_GradingCurves == null)
			{
				this.m_GradingCurves = new Texture2D(128, 2, this.GetCurveFormat(), false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = 52,
					anisoLevel = 0,
					wrapMode = 1,
					filterMode = 1
				};
			}
			ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
			curves.hueVShue.Cache();
			curves.hueVSsat.Cache();
			for (int i = 0; i < 128; i++)
			{
				float t = (float)i * 0.0078125f;
				float num = curves.hueVShue.Evaluate(t);
				float num2 = curves.hueVSsat.Evaluate(t);
				float num3 = curves.satVSsat.Evaluate(t);
				float num4 = curves.lumVSsat.Evaluate(t);
				this.m_pixels[i] = new Color(num, num2, num3, num4);
				float num5 = curves.master.Evaluate(t);
				float num6 = curves.red.Evaluate(t);
				float num7 = curves.green.Evaluate(t);
				float num8 = curves.blue.Evaluate(t);
				this.m_pixels[i + 128] = new Color(num6, num7, num8, num5);
			}
			this.m_GradingCurves.SetPixels(this.m_pixels);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00033844 File Offset: 0x00031A44
		public bool IsLogLutValid(RenderTexture lut)
		{
			return lut != null && lut.IsCreated() && lut.height == 32;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0003386A File Offset: 0x00031A6A
		public RenderTextureFormat GetLutFormat()
		{
			if (SystemInfo.SupportsRenderTextureFormat(2))
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x0012BD88 File Offset: 0x00129F88
		public void GenerateLut()
		{
			ColorGradingModel.Settings settings = base.model.settings;
			if (!this.IsLogLutValid(base.model.bakedLut))
			{
				GraphicsUtils.Destroy(base.model.bakedLut);
				base.model.bakedLut = new RenderTexture(1024, 32, 0, this.GetLutFormat())
				{
					name = "Color Grading Log LUT",
					hideFlags = 52,
					filterMode = 1,
					wrapMode = 1,
					anisoLevel = 0
				};
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			material.shaderKeywords = null;
			ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
			ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
			if (tonemapper != ColorGradingModel.Tonemapper.Neutral)
			{
				if (tonemapper == ColorGradingModel.Tonemapper.ACES)
				{
					material.EnableKeyword("TONEMAPPING_FILMIC");
				}
			}
			else
			{
				material.EnableKeyword("TONEMAPPING_NEUTRAL");
				float num = tonemapping.neutralBlackIn * 20f + 1f;
				float num2 = tonemapping.neutralBlackOut * 10f + 1f;
				float num3 = tonemapping.neutralWhiteIn / 20f;
				float num4 = 1f - tonemapping.neutralWhiteOut / 20f;
				float num5 = num / num2;
				float num6 = num3 / num4;
				float num7 = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, num5));
				float num8 = Mathf.LerpUnclamped(0.01f, 0.24f, num6);
				float num9 = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, num5));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, num7, num8, num9));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
			}
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360f);
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation);
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast);
			material.SetVector(ColorGradingComponent.Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out vector, out vector2, out vector3);
			material.SetVector(ColorGradingComponent.Uniforms._Lift, vector);
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, vector2);
			material.SetVector(ColorGradingComponent.Uniforms._Gain, vector3);
			Vector3 vector4;
			Vector3 vector5;
			Vector3 vector6;
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out vector4, out vector5, out vector6);
			material.SetVector(ColorGradingComponent.Uniforms._Slope, vector4);
			material.SetVector(ColorGradingComponent.Uniforms._Power, vector5);
			material.SetVector(ColorGradingComponent.Uniforms._Offset, vector6);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, this.GetCurveTexture());
			Graphics.Blit(null, base.model.bakedLut, material, 0);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x0012C164 File Offset: 0x0012A364
		public override void Prepare(Material uberMaterial)
		{
			if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
			{
				this.GenerateLut();
				base.model.isDirty = false;
			}
			uberMaterial.EnableKeyword((!this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog)) ? "COLOR_GRADING" : "COLOR_GRADING_LOG_VIEW");
			RenderTexture bakedLut = base.model.bakedLut;
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut);
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, new Vector3(1f / (float)bakedLut.width, 1f / (float)bakedLut.height, (float)bakedLut.height - 1f));
			float num = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, num);
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x0012C260 File Offset: 0x0012A460
		public void OnGUI()
		{
			RenderTexture bakedLut = base.model.bakedLut;
			Rect rect;
			rect..ctor(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)bakedLut.width, (float)bakedLut.height);
			GUI.DrawTexture(rect, bakedLut);
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0003387A File Offset: 0x00031A7A
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GradingCurves);
			GraphicsUtils.Destroy(base.model.bakedLut);
			this.m_GradingCurves = null;
			base.model.bakedLut = null;
		}

		// Token: 0x0400332E RID: 13102
		public const int k_InternalLogLutSize = 32;

		// Token: 0x0400332F RID: 13103
		public const int k_CurvePrecision = 128;

		// Token: 0x04003330 RID: 13104
		public const float k_CurveStep = 0.0078125f;

		// Token: 0x04003331 RID: 13105
		public Texture2D m_GradingCurves;

		// Token: 0x04003332 RID: 13106
		public Color[] m_pixels = new Color[256];

		// Token: 0x02001272 RID: 4722
		public static class Uniforms
		{
			// Token: 0x04007F52 RID: 32594
			public static readonly int _LutParams = Shader.PropertyToID("_LutParams");

			// Token: 0x04007F53 RID: 32595
			public static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");

			// Token: 0x04007F54 RID: 32596
			public static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");

			// Token: 0x04007F55 RID: 32597
			public static readonly int _HueShift = Shader.PropertyToID("_HueShift");

			// Token: 0x04007F56 RID: 32598
			public static readonly int _Saturation = Shader.PropertyToID("_Saturation");

			// Token: 0x04007F57 RID: 32599
			public static readonly int _Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x04007F58 RID: 32600
			public static readonly int _Balance = Shader.PropertyToID("_Balance");

			// Token: 0x04007F59 RID: 32601
			public static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x04007F5A RID: 32602
			public static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");

			// Token: 0x04007F5B RID: 32603
			public static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x04007F5C RID: 32604
			public static readonly int _Slope = Shader.PropertyToID("_Slope");

			// Token: 0x04007F5D RID: 32605
			public static readonly int _Power = Shader.PropertyToID("_Power");

			// Token: 0x04007F5E RID: 32606
			public static readonly int _Offset = Shader.PropertyToID("_Offset");

			// Token: 0x04007F5F RID: 32607
			public static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x04007F60 RID: 32608
			public static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x04007F61 RID: 32609
			public static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x04007F62 RID: 32610
			public static readonly int _Curves = Shader.PropertyToID("_Curves");

			// Token: 0x04007F63 RID: 32611
			public static readonly int _LogLut = Shader.PropertyToID("_LogLut");

			// Token: 0x04007F64 RID: 32612
			public static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");

			// Token: 0x04007F65 RID: 32613
			public static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
		}
	}
}
