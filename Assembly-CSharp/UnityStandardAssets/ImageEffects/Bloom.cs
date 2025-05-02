using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006A9 RID: 1705
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom")]
	public class Bloom : PostEffectsBase
	{
		// Token: 0x0600477A RID: 18298 RVA: 0x0015BE4C File Offset: 0x0015A04C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.blurAndFlaresMaterial = base.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0015BED8 File Offset: 0x0015A0D8
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == Bloom.HDRBloomMode.Auto)
			{
				this.doHdr = (source.format == 2 && base.GetComponent<Camera>().allowHDR);
			}
			else
			{
				this.doHdr = (this.hdr == Bloom.HDRBloomMode.On);
			}
			this.doHdr = (this.doHdr && this.supportHDRTextures);
			Bloom.BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
			if (this.doHdr)
			{
				bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add;
			}
			RenderTextureFormat renderTextureFormat = (!this.doHdr) ? 7 : 2;
			int num = source.width / 2;
			int num2 = source.height / 2;
			int num3 = source.width / 4;
			int num4 = source.height / 4;
			float num5 = 1f * (float)source.width / (1f * (float)source.height);
			float num6 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.Blit(source, temporary2, this.screenBlend, 2);
				RenderTexture temporary3 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				Graphics.Blit(temporary2, temporary3, this.screenBlend, 2);
				Graphics.Blit(temporary3, temporary, this.screenBlend, 6);
				RenderTexture.ReleaseTemporary(temporary3);
			}
			else
			{
				Graphics.Blit(source, temporary2);
				Graphics.Blit(temporary2, temporary, this.screenBlend, 6);
			}
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture renderTexture = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
			this.BrightFilter(this.bloomThreshold * this.bloomThresholdColor, temporary, renderTexture);
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			else if (this.bloomBlurIterations > 10)
			{
				this.bloomBlurIterations = 10;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num7 = (1f + (float)i * 0.25f) * this.sepBlurSpread;
				RenderTexture temporary4 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, num7 * num6, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				temporary4 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num7 / num5 * num6, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				if (this.quality > Bloom.BloomQuality.Cheap)
				{
					if (i == 0)
					{
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(renderTexture, temporary);
					}
					else
					{
						temporary.MarkRestoreExpected();
						Graphics.Blit(renderTexture, temporary, this.screenBlend, 10);
					}
				}
			}
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.SetRenderTarget(renderTexture);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(temporary, renderTexture, this.screenBlend, 6);
			}
			if (this.lensflareIntensity > Mathf.Epsilon)
			{
				RenderTexture temporary5 = RenderTexture.GetTemporary(num3, num4, 0, renderTextureFormat);
				if (this.lensflareMode == Bloom.LensFlareStyle.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, renderTexture, temporary5);
					if (this.quality > Bloom.BloomQuality.Cheap)
					{
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * (float)temporary.height), 0f, 0f));
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * (float)temporary.width), 0f, 0f, 0f));
						Graphics.SetRenderTarget(temporary5);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
					}
					this.Vignette(0.975f, temporary5, temporary5);
					this.BlendFlares(temporary5, renderTexture);
				}
				else
				{
					float num8 = 1f * Mathf.Cos(this.flareRotation);
					float num9 = 1f * Mathf.Sin(this.flareRotation);
					float num10 = this.hollyStretchWidth * 1f / num5 * num6;
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num8, num9, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshold, 1f, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_TintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 2);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 3);
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num8 * num10, num9 * num10, 0f, 0f));
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
					{
						num10 = this.hollyStretchWidth * 2f / num5 * num6;
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num10 * num8, num10 * num9, 0f, 0f));
						temporary5.DiscardContents();
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num10 * num8, num10 * num9, 0f, 0f));
						temporary.DiscardContents();
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
					}
					if (this.lensflareMode == Bloom.LensFlareStyle.Anamorphic)
					{
						this.AddTo(1f, temporary, renderTexture);
					}
					else
					{
						this.Vignette(1f, temporary, temporary5);
						this.BlendFlares(temporary5, temporary);
						this.AddTo(1f, temporary, renderTexture);
					}
				}
				RenderTexture.ReleaseTemporary(temporary5);
			}
			int num11 = (int)bloomScreenBlendMode;
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				RenderTexture temporary6 = RenderTexture.GetTemporary(num, num2, 0, renderTextureFormat);
				Graphics.Blit(renderTexture, temporary6);
				Graphics.Blit(temporary6, destination, this.screenBlend, num11);
				RenderTexture.ReleaseTemporary(temporary6);
			}
			else
			{
				Graphics.Blit(renderTexture, destination, this.screenBlend, num11);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x00038C5D File Offset: 0x00036E5D
		public void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.screenBlend.SetFloat("_Intensity", intensity_);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.screenBlend, 9);
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0015C67C File Offset: 0x0015A87C
		public void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x00038C85 File Offset: 0x00036E85
		public void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x00038CAE File Offset: 0x00036EAE
		public void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", threshColor);
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0015C7CC File Offset: 0x0015A9CC
		public void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				to.MarkRestoreExpected();
				Graphics.Blit((!(from == to)) ? from : null, to, this.screenBlend, (!(from == to)) ? 3 : 7);
			}
			else if (from != to)
			{
				Graphics.SetRenderTarget(to);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(from, to);
			}
		}

		// Token: 0x04003841 RID: 14401
		public Bloom.TweakMode tweakMode;

		// Token: 0x04003842 RID: 14402
		public Bloom.BloomScreenBlendMode screenBlendMode = Bloom.BloomScreenBlendMode.Add;

		// Token: 0x04003843 RID: 14403
		public Bloom.HDRBloomMode hdr;

		// Token: 0x04003844 RID: 14404
		public bool doHdr;

		// Token: 0x04003845 RID: 14405
		public float sepBlurSpread = 2.5f;

		// Token: 0x04003846 RID: 14406
		public Bloom.BloomQuality quality = Bloom.BloomQuality.High;

		// Token: 0x04003847 RID: 14407
		public float bloomIntensity = 0.5f;

		// Token: 0x04003848 RID: 14408
		public float bloomThreshold = 0.5f;

		// Token: 0x04003849 RID: 14409
		public Color bloomThresholdColor = Color.white;

		// Token: 0x0400384A RID: 14410
		public int bloomBlurIterations = 2;

		// Token: 0x0400384B RID: 14411
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x0400384C RID: 14412
		public float flareRotation;

		// Token: 0x0400384D RID: 14413
		public Bloom.LensFlareStyle lensflareMode = Bloom.LensFlareStyle.Anamorphic;

		// Token: 0x0400384E RID: 14414
		public float hollyStretchWidth = 2.5f;

		// Token: 0x0400384F RID: 14415
		public float lensflareIntensity;

		// Token: 0x04003850 RID: 14416
		public float lensflareThreshold = 0.3f;

		// Token: 0x04003851 RID: 14417
		public float lensFlareSaturation = 0.75f;

		// Token: 0x04003852 RID: 14418
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04003853 RID: 14419
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x04003854 RID: 14420
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04003855 RID: 14421
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04003856 RID: 14422
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04003857 RID: 14423
		public Shader lensFlareShader;

		// Token: 0x04003858 RID: 14424
		public Material lensFlareMaterial;

		// Token: 0x04003859 RID: 14425
		public Shader screenBlendShader;

		// Token: 0x0400385A RID: 14426
		public Material screenBlend;

		// Token: 0x0400385B RID: 14427
		public Shader blurAndFlaresShader;

		// Token: 0x0400385C RID: 14428
		public Material blurAndFlaresMaterial;

		// Token: 0x0400385D RID: 14429
		public Shader brightPassFilterShader;

		// Token: 0x0400385E RID: 14430
		public Material brightPassFilterMaterial;

		// Token: 0x020012FF RID: 4863
		public enum LensFlareStyle
		{
			// Token: 0x04008201 RID: 33281
			Ghosting,
			// Token: 0x04008202 RID: 33282
			Anamorphic,
			// Token: 0x04008203 RID: 33283
			Combined
		}

		// Token: 0x02001300 RID: 4864
		public enum TweakMode
		{
			// Token: 0x04008205 RID: 33285
			Basic,
			// Token: 0x04008206 RID: 33286
			Complex
		}

		// Token: 0x02001301 RID: 4865
		public enum HDRBloomMode
		{
			// Token: 0x04008208 RID: 33288
			Auto,
			// Token: 0x04008209 RID: 33289
			On,
			// Token: 0x0400820A RID: 33290
			Off
		}

		// Token: 0x02001302 RID: 4866
		public enum BloomScreenBlendMode
		{
			// Token: 0x0400820C RID: 33292
			Screen,
			// Token: 0x0400820D RID: 33293
			Add
		}

		// Token: 0x02001303 RID: 4867
		public enum BloomQuality
		{
			// Token: 0x0400820F RID: 33295
			Cheap,
			// Token: 0x04008210 RID: 33296
			High
		}
	}
}
