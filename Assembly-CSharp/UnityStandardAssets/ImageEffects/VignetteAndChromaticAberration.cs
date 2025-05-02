using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D0 RID: 1744
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		// Token: 0x06004843 RID: 18499 RVA: 0x00164C40 File Offset: 0x00162E40
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x00164CB4 File Offset: 0x00162EB4
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
			float num = 1f * (float)width / (1f * (float)height);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (flag)
			{
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				if (Mathf.Abs(this.blur) > 0f)
				{
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
					for (int i = 0; i < 2; i++)
					{
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
						RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(renderTexture2);
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(temporary);
					}
				}
				this.m_VignetteMaterial.SetFloat("_Intensity", this.intensity);
				this.m_VignetteMaterial.SetFloat("_Blur", this.blur);
				this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
				Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
			}
			this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
			this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
			this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
			this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
			if (flag)
			{
				renderTexture.wrapMode = 1;
			}
			else
			{
				source.wrapMode = 1;
			}
			Graphics.Blit((!flag) ? source : renderTexture, destination, this.m_ChromAberrationMaterial, (this.mode != VignetteAndChromaticAberration.AberrationMode.Advanced) ? 1 : 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture2);
		}

		// Token: 0x040039C2 RID: 14786
		public VignetteAndChromaticAberration.AberrationMode mode;

		// Token: 0x040039C3 RID: 14787
		public float intensity = 0.375f;

		// Token: 0x040039C4 RID: 14788
		public float chromaticAberration = 0.2f;

		// Token: 0x040039C5 RID: 14789
		public float axialAberration = 0.5f;

		// Token: 0x040039C6 RID: 14790
		public float blur;

		// Token: 0x040039C7 RID: 14791
		public float blurSpread = 0.75f;

		// Token: 0x040039C8 RID: 14792
		public float luminanceDependency = 0.25f;

		// Token: 0x040039C9 RID: 14793
		public float blurDistance = 2.5f;

		// Token: 0x040039CA RID: 14794
		public Shader vignetteShader;

		// Token: 0x040039CB RID: 14795
		public Shader separableBlurShader;

		// Token: 0x040039CC RID: 14796
		public Shader chromAberrationShader;

		// Token: 0x040039CD RID: 14797
		public Material m_VignetteMaterial;

		// Token: 0x040039CE RID: 14798
		public Material m_SeparableBlurMaterial;

		// Token: 0x040039CF RID: 14799
		public Material m_ChromAberrationMaterial;

		// Token: 0x02001318 RID: 4888
		public enum AberrationMode
		{
			// Token: 0x04008268 RID: 33384
			Simple,
			// Token: 0x04008269 RID: 33385
			Advanced
		}
	}
}
