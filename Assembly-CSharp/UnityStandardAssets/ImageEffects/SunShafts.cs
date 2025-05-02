using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CB RID: 1739
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x0600482F RID: 18479 RVA: 0x00163CC4 File Offset: 0x00161EC4
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x00163D28 File Offset: 0x00161F28
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.useDepthTexture)
			{
				base.GetComponent<Camera>().depthTextureMode |= 1;
			}
			int num = 4;
			if (this.resolution == SunShafts.SunShaftsResolution.Normal)
			{
				num = 2;
			}
			else if (this.resolution == SunShafts.SunShaftsResolution.High)
			{
				num = 1;
			}
			Vector3 vector = Vector3.one * 0.5f;
			if (this.sunTransform)
			{
				vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
			}
			else
			{
				vector..ctor(0.5f, 0.5f, 0f);
			}
			int num2 = source.width / num;
			int num3 = source.height / num;
			RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
			if (!this.useDepthTexture)
			{
				RenderTextureFormat renderTextureFormat = (!base.GetComponent<Camera>().allowHDR) ? 7 : 9;
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, renderTextureFormat);
				RenderTexture.active = temporary2;
				GL.ClearWithSkybox(false, base.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else
			{
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
			}
			base.DrawBorder(temporary, this.simpleClearMaterial);
			this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
			float num4 = this.sunShaftBlurRadius * 0.00130208337f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(num2, num3, 0);
				Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
				num4 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
				temporary = RenderTexture.GetTemporary(num2, num3, 0);
				Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary3);
				num4 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
			}
			if (vector.z >= 0f)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != SunShafts.ShaftsScreenBlendMode.Screen) ? 4 : 0);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x0400399B RID: 14747
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x0400399C RID: 14748
		public SunShafts.ShaftsScreenBlendMode screenBlendMode;

		// Token: 0x0400399D RID: 14749
		public Transform sunTransform;

		// Token: 0x0400399E RID: 14750
		public int radialBlurIterations = 2;

		// Token: 0x0400399F RID: 14751
		public Color sunColor = Color.white;

		// Token: 0x040039A0 RID: 14752
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x040039A1 RID: 14753
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x040039A2 RID: 14754
		public float sunShaftIntensity = 1.15f;

		// Token: 0x040039A3 RID: 14755
		public float maxRadius = 0.75f;

		// Token: 0x040039A4 RID: 14756
		public bool useDepthTexture = true;

		// Token: 0x040039A5 RID: 14757
		public Shader sunShaftsShader;

		// Token: 0x040039A6 RID: 14758
		public Material sunShaftsMaterial;

		// Token: 0x040039A7 RID: 14759
		public Shader simpleClearShader;

		// Token: 0x040039A8 RID: 14760
		public Material simpleClearMaterial;

		// Token: 0x02001312 RID: 4882
		public enum SunShaftsResolution
		{
			// Token: 0x0400824A RID: 33354
			Low,
			// Token: 0x0400824B RID: 33355
			Normal,
			// Token: 0x0400824C RID: 33356
			High
		}

		// Token: 0x02001313 RID: 4883
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x0400824E RID: 33358
			Screen,
			// Token: 0x0400824F RID: 33359
			Add
		}
	}
}
