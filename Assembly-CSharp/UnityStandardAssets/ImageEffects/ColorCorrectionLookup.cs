using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B4 RID: 1716
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
	public class ColorCorrectionLookup : PostEffectsBase
	{
		// Token: 0x060047AC RID: 18348 RVA: 0x0015EB88 File Offset: 0x0015CD88
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
			if (!this.isSupported || !SystemInfo.supports3DTextures)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x00038EF1 File Offset: 0x000370F1
		public void OnDisable()
		{
			if (this.material)
			{
				Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x00038F15 File Offset: 0x00037115
		public void OnDestroy()
		{
			if (this.converted3DLut)
			{
				Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = null;
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0015EBD8 File Offset: 0x0015CDD8
		public void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			if (this.converted3DLut)
			{
				Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = new Texture3D(num, num, num, 5, false);
			this.converted3DLut.SetPixels(array);
			this.converted3DLut.Apply();
			this.basedOnTempTex = string.Empty;
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0015ECD8 File Offset: 0x0015CED8
		public bool ValidDimensions(Texture2D tex2d)
		{
			if (!tex2d)
			{
				return false;
			}
			int height = tex2d.height;
			return height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x0015ED14 File Offset: 0x0015CF14
		public void Convert(Texture2D temp2DTex, string path)
		{
			if (temp2DTex)
			{
				int num = temp2DTex.width * temp2DTex.height;
				num = temp2DTex.height;
				if (!this.ValidDimensions(temp2DTex))
				{
					this.basedOnTempTex = string.Empty;
					return;
				}
				Color[] pixels = temp2DTex.GetPixels();
				Color[] array = new Color[pixels.Length];
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						for (int k = 0; k < num; k++)
						{
							int num2 = num - j - 1;
							array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
						}
					}
				}
				if (this.converted3DLut)
				{
					Object.DestroyImmediate(this.converted3DLut);
				}
				this.converted3DLut = new Texture3D(num, num, num, 5, false);
				this.converted3DLut.SetPixels(array);
				this.converted3DLut.Apply();
				this.basedOnTempTex = path;
			}
			else
			{
				Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.", null);
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0015EE38 File Offset: 0x0015D038
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || !SystemInfo.supports3DTextures)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.converted3DLut == null)
			{
				this.SetIdentityLut();
			}
			int width = this.converted3DLut.width;
			this.converted3DLut.wrapMode = 1;
			this.material.SetFloat("_Scale", (float)(width - 1) / (1f * (float)width));
			this.material.SetFloat("_Offset", 1f / (2f * (float)width));
			this.material.SetTexture("_ClutTex", this.converted3DLut);
			Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace != 1) ? 0 : 1);
		}

		// Token: 0x040038D7 RID: 14551
		public Shader shader;

		// Token: 0x040038D8 RID: 14552
		public Material material;

		// Token: 0x040038D9 RID: 14553
		public Texture3D converted3DLut;

		// Token: 0x040038DA RID: 14554
		public string basedOnTempTex = string.Empty;
	}
}
