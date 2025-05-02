using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C2 RID: 1730
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
	public class NoiseAndGrain : PostEffectsBase
	{
		// Token: 0x060047FA RID: 18426 RVA: 0x00161ECC File Offset: 0x001600CC
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x00161F40 File Offset: 0x00160140
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || null == this.noiseTexture)
			{
				Graphics.Blit(source, destination);
				if (null == this.noiseTexture)
				{
				}
				return;
			}
			this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float)Time.frameCount);
				this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11NoiseMaterial.SetVector("_NoisePerChannel", (!this.monochrome) ? this.intensities : Vector3.one);
				this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.dx11NoiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, (!this.monochrome) ? 2 : 3);
					this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
					Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
					RenderTexture.ReleaseTemporary(temporary);
				}
				else
				{
					NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, (!this.monochrome) ? 0 : 1);
				}
			}
			else
			{
				if (this.noiseTexture)
				{
					this.noiseTexture.wrapMode = 0;
					this.noiseTexture.filterMode = this.filterMode;
				}
				this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.noiseMaterial.SetVector("_NoisePerChannel", (!this.monochrome) ? this.intensities : Vector3.one);
				this.noiseMaterial.SetVector("_NoiseTilingPerChannel", (!this.monochrome) ? this.tiling : (Vector3.one * this.monochromeTiling));
				this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.noiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary2, this.noiseMaterial, this.noiseTexture, 2);
					this.noiseMaterial.SetTexture("_NoiseTex", temporary2);
					Graphics.Blit(source, destination, this.noiseMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				else
				{
					NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
				}
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x001622F4 File Offset: 0x001604F4
		public static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
		{
			RenderTexture.active = dest;
			float num = (float)noise.width * 1f;
			float num2 = 1f * (float)source.width / NoiseAndGrain.TILE_AMOUNT;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			float num3 = 1f * (float)source.width / (1f * (float)source.height);
			float num4 = 1f / num2;
			float num5 = num4 * num3;
			float num6 = num / ((float)noise.width * 1f);
			fxMaterial.SetPass(passNr);
			GL.Begin(7);
			for (float num7 = 0f; num7 < 1f; num7 += num4)
			{
				for (float num8 = 0f; num8 < 1f; num8 += num5)
				{
					float num9 = Random.Range(0f, 1f);
					float num10 = Random.Range(0f, 1f);
					num9 = Mathf.Floor(num9 * num) / num;
					num10 = Mathf.Floor(num10 * num) / num;
					float num11 = 1f / num;
					GL.MultiTexCoord2(0, num9, num10);
					GL.MultiTexCoord2(1, 0f, 0f);
					GL.Vertex3(num7, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10);
					GL.MultiTexCoord2(1, 1f, 0f);
					GL.Vertex3(num7 + num4, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 1f, 1f);
					GL.Vertex3(num7 + num4, num8 + num5, 0.1f);
					GL.MultiTexCoord2(0, num9, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 0f, 1f);
					GL.Vertex3(num7, num8 + num5, 0.1f);
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0400395A RID: 14682
		public float intensityMultiplier = 0.25f;

		// Token: 0x0400395B RID: 14683
		public float generalIntensity = 0.5f;

		// Token: 0x0400395C RID: 14684
		public float blackIntensity = 1f;

		// Token: 0x0400395D RID: 14685
		public float whiteIntensity = 1f;

		// Token: 0x0400395E RID: 14686
		public float midGrey = 0.2f;

		// Token: 0x0400395F RID: 14687
		public bool dx11Grain;

		// Token: 0x04003960 RID: 14688
		public float softness;

		// Token: 0x04003961 RID: 14689
		public bool monochrome;

		// Token: 0x04003962 RID: 14690
		public Vector3 intensities = new Vector3(1f, 1f, 1f);

		// Token: 0x04003963 RID: 14691
		public Vector3 tiling = new Vector3(64f, 64f, 64f);

		// Token: 0x04003964 RID: 14692
		public float monochromeTiling = 64f;

		// Token: 0x04003965 RID: 14693
		public FilterMode filterMode = 1;

		// Token: 0x04003966 RID: 14694
		public Texture2D noiseTexture;

		// Token: 0x04003967 RID: 14695
		public Shader noiseShader;

		// Token: 0x04003968 RID: 14696
		public Material noiseMaterial;

		// Token: 0x04003969 RID: 14697
		public Shader dx11NoiseShader;

		// Token: 0x0400396A RID: 14698
		public Material dx11NoiseMaterial;

		// Token: 0x0400396B RID: 14699
		public static float TILE_AMOUNT = 64f;
	}
}
