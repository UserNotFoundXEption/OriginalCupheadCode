using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BA RID: 1722
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
	public class DepthOfFieldDeprecated : PostEffectsBase
	{
		// Token: 0x060047CF RID: 18383 RVA: 0x00160520 File Offset: 0x0015E720
		public void CreateMaterials()
		{
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x001605AC File Offset: 0x0015E7AC
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x0003910B File Offset: 0x0003730B
		public void OnDisable()
		{
			Quads.Cleanup();
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x00039112 File Offset: 0x00037312
		public new void OnEnable()
		{
			this._camera = base.GetComponent<Camera>();
			this._camera.depthTextureMode |= 1;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00160658 File Offset: 0x0015E858
		public float FocalDistance01(float worldDist)
		{
			return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x001606C8 File Offset: 0x0015E8C8
		public int GetDividerBasedOnQuality()
		{
			int result = 1;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
			{
				result = 2;
			}
			else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x001606FC File Offset: 0x0015E8FC
		public int GetLowResolutionDividerBasedOnQuality(int baseDivider)
		{
			int num = baseDivider;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
			{
				num *= 2;
			}
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0016072C File Offset: 0x0015E92C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.smoothness < 0.1f)
			{
				this.smoothness = 0.1f;
			}
			this.bokeh = (this.bokeh && this.bokehSupport);
			float num = (!this.bokeh) ? 1f : DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR;
			bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
			float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
			if (this.simpleTweakMode)
			{
				this.focalDistance01 = ((!this.objectFocus) ? this.FocalDistance01(this.focalPoint) : (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane));
				this.focalStartCurve = this.focalDistance01 * this.smoothness;
				this.focalEndCurve = this.focalStartCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			else
			{
				if (this.objectFocus)
				{
					Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
					vector.z /= this._camera.farClipPlane;
					this.focalDistance01 = vector.z;
				}
				else
				{
					this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
				}
				this.focalStartCurve = this.focalZStartCurve;
				this.focalEndCurve = this.focalZEndCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			this.widthOverHeight = 1f * (float)source.width / (1f * (float)source.height);
			this.oneOverBaseSize = 0.001953125f;
			this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
			this.dofMaterial.SetVector("_CurveParams", new Vector4((!this.simpleTweakMode) ? this.focalStartCurve : (1f / this.focalStartCurve), (!this.simpleTweakMode) ? this.focalEndCurve : (1f / this.focalEndCurve), num2 * 0.5f, this.focalDistance01));
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), 0f, 0f));
			int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
			int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
			this.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
			Graphics.Blit(source, source, this.dofMaterial, 3);
			this.Downsample(source, this.mediumRezWorkTexture);
			this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
				Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
				Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
			}
			else
			{
				this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
			}
			this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
			this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
			}
			this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
			this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(source, (!flag) ? destination : this.foregroundTexture, this.dofMaterial, (!this.visualize) ? 0 : 2);
			if (flag)
			{
				Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
				this.Downsample(source, this.mediumRezWorkTexture);
				this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
					Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
					Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
				}
				else
				{
					this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
				}
				Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
				this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
				Graphics.Blit(source, destination, this.dofMaterial, (!this.visualize) ? 4 : 1);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
				}
			}
			this.ReleaseTextures();
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00160D48 File Offset: 0x0015EF48
		public void Blur(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00160E8C File Offset: 0x0015F08C
		public void BlurFg(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			this.dofBlurMaterial.SetTexture("_TapHigh", from);
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00160FE0 File Offset: 0x0015F1E0
		public void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
		{
			this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x001610FC File Offset: 0x0015F2FC
		public void Downsample(RenderTexture from, RenderTexture to)
		{
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)to.width), 1f / (1f * (float)to.height), 0f, 0f));
			Graphics.Blit(from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x00161160 File Offset: 0x0015F360
		public void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
		{
			if (this.bokehMaterial)
			{
				Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
				RenderTexture.active = tempTex;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
				GL.PushMatrix();
				GL.LoadIdentity();
				bokehInfo.filterMode = 0;
				float num = (float)bokehInfo.width * 1f / ((float)bokehInfo.height * 1f);
				float num2 = 2f / (1f * (float)bokehInfo.width);
				num2 += this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
				this.bokehMaterial.SetTexture("_Source", bokehInfo);
				this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
				this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
				this.bokehMaterial.SetPass(0);
				foreach (Mesh mesh in meshes)
				{
					if (mesh)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
				GL.PopMatrix();
				Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
				bokehInfo.filterMode = 1;
			}
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x001612D4 File Offset: 0x0015F4D4
		public void ReleaseTextures()
		{
			if (this.foregroundTexture)
			{
				RenderTexture.ReleaseTemporary(this.foregroundTexture);
			}
			if (this.finalDefocus)
			{
				RenderTexture.ReleaseTemporary(this.finalDefocus);
			}
			if (this.mediumRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
			}
			if (this.lowRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
			}
			if (this.bokehSource)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource);
			}
			if (this.bokehSource2)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource2);
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00161384 File Offset: 0x0015F584
		public void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
		{
			this.foregroundTexture = null;
			if (blurForeground)
			{
				this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			}
			this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
			this.bokehSource = null;
			this.bokehSource2 = null;
			if (this.bokeh)
			{
				this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, 2);
				this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, 2);
				this.bokehSource.filterMode = 1;
				this.bokehSource2.filterMode = 1;
				RenderTexture.active = this.bokehSource2;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			source.filterMode = 1;
			this.finalDefocus.filterMode = 1;
			this.mediumRezWorkTexture.filterMode = 1;
			this.lowRezWorkTexture.filterMode = 1;
			if (this.foregroundTexture)
			{
				this.foregroundTexture.filterMode = 1;
			}
		}

		// Token: 0x04003911 RID: 14609
		public static int SMOOTH_DOWNSAMPLE_PASS = 6;

		// Token: 0x04003912 RID: 14610
		public static float BOKEH_EXTRA_BLUR = 2f;

		// Token: 0x04003913 RID: 14611
		public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;

		// Token: 0x04003914 RID: 14612
		public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;

		// Token: 0x04003915 RID: 14613
		public bool simpleTweakMode = true;

		// Token: 0x04003916 RID: 14614
		public float focalPoint = 1f;

		// Token: 0x04003917 RID: 14615
		public float smoothness = 0.5f;

		// Token: 0x04003918 RID: 14616
		public float focalZDistance;

		// Token: 0x04003919 RID: 14617
		public float focalZStartCurve = 1f;

		// Token: 0x0400391A RID: 14618
		public float focalZEndCurve = 1f;

		// Token: 0x0400391B RID: 14619
		public float focalStartCurve = 2f;

		// Token: 0x0400391C RID: 14620
		public float focalEndCurve = 2f;

		// Token: 0x0400391D RID: 14621
		public float focalDistance01 = 0.1f;

		// Token: 0x0400391E RID: 14622
		public Transform objectFocus;

		// Token: 0x0400391F RID: 14623
		public float focalSize;

		// Token: 0x04003920 RID: 14624
		public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;

		// Token: 0x04003921 RID: 14625
		public float maxBlurSpread = 1.75f;

		// Token: 0x04003922 RID: 14626
		public float foregroundBlurExtrude = 1.15f;

		// Token: 0x04003923 RID: 14627
		public Shader dofBlurShader;

		// Token: 0x04003924 RID: 14628
		public Material dofBlurMaterial;

		// Token: 0x04003925 RID: 14629
		public Shader dofShader;

		// Token: 0x04003926 RID: 14630
		public Material dofMaterial;

		// Token: 0x04003927 RID: 14631
		public bool visualize;

		// Token: 0x04003928 RID: 14632
		public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;

		// Token: 0x04003929 RID: 14633
		public float widthOverHeight = 1.25f;

		// Token: 0x0400392A RID: 14634
		public float oneOverBaseSize = 0.001953125f;

		// Token: 0x0400392B RID: 14635
		public bool bokeh;

		// Token: 0x0400392C RID: 14636
		public bool bokehSupport = true;

		// Token: 0x0400392D RID: 14637
		public Shader bokehShader;

		// Token: 0x0400392E RID: 14638
		public Texture2D bokehTexture;

		// Token: 0x0400392F RID: 14639
		public float bokehScale = 2.4f;

		// Token: 0x04003930 RID: 14640
		public float bokehIntensity = 0.15f;

		// Token: 0x04003931 RID: 14641
		public float bokehThresholdContrast = 0.1f;

		// Token: 0x04003932 RID: 14642
		public float bokehThresholdLuminance = 0.55f;

		// Token: 0x04003933 RID: 14643
		public int bokehDownsample = 1;

		// Token: 0x04003934 RID: 14644
		public Material bokehMaterial;

		// Token: 0x04003935 RID: 14645
		public Camera _camera;

		// Token: 0x04003936 RID: 14646
		public RenderTexture foregroundTexture;

		// Token: 0x04003937 RID: 14647
		public RenderTexture mediumRezWorkTexture;

		// Token: 0x04003938 RID: 14648
		public RenderTexture finalDefocus;

		// Token: 0x04003939 RID: 14649
		public RenderTexture lowRezWorkTexture;

		// Token: 0x0400393A RID: 14650
		public RenderTexture bokehSource;

		// Token: 0x0400393B RID: 14651
		public RenderTexture bokehSource2;

		// Token: 0x0200130B RID: 4875
		public enum Dof34QualitySetting
		{
			// Token: 0x0400822B RID: 33323
			OnlyBackground = 1,
			// Token: 0x0400822C RID: 33324
			BackgroundAndForeground
		}

		// Token: 0x0200130C RID: 4876
		public enum DofResolution
		{
			// Token: 0x0400822E RID: 33326
			High = 2,
			// Token: 0x0400822F RID: 33327
			Medium,
			// Token: 0x04008230 RID: 33328
			Low
		}

		// Token: 0x0200130D RID: 4877
		public enum DofBlurriness
		{
			// Token: 0x04008232 RID: 33330
			Low = 1,
			// Token: 0x04008233 RID: 33331
			High,
			// Token: 0x04008234 RID: 33332
			VeryHigh = 4
		}

		// Token: 0x0200130E RID: 4878
		public enum BokehDestination
		{
			// Token: 0x04008236 RID: 33334
			Background = 1,
			// Token: 0x04008237 RID: 33335
			Foreground,
			// Token: 0x04008238 RID: 33336
			BackgroundAndForeground
		}
	}
}
