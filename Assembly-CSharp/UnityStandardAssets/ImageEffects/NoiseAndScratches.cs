using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C3 RID: 1731
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x060047FF RID: 18431 RVA: 0x00162540 File Offset: 0x00160740
		public void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (this.shaderRGB == null || this.shaderYUV == null)
			{
				base.enabled = false;
			}
			else if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
			}
			else if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06004800 RID: 18432 RVA: 0x001625C0 File Offset: 0x001607C0
		public Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = 61;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = 61;
				}
				return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0003933E File Offset: 0x0003753E
		public void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x00162660 File Offset: 0x00160860
		public void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0016272C File Offset: 0x0016092C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = Random.value * 2f / this.scratchFPS;
				this.scratchX = Random.value;
				this.scratchY = Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(Random.value, Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + Random.value * this.scratchJitter, this.scratchY + Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(Random.Range(this.grainIntensityMin, this.grainIntensityMax), Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0400396C RID: 14700
		public bool monochrome = true;

		// Token: 0x0400396D RID: 14701
		public bool rgbFallback;

		// Token: 0x0400396E RID: 14702
		public float grainIntensityMin = 0.1f;

		// Token: 0x0400396F RID: 14703
		public float grainIntensityMax = 0.2f;

		// Token: 0x04003970 RID: 14704
		public float grainSize = 2f;

		// Token: 0x04003971 RID: 14705
		public float scratchIntensityMin = 0.05f;

		// Token: 0x04003972 RID: 14706
		public float scratchIntensityMax = 0.25f;

		// Token: 0x04003973 RID: 14707
		public float scratchFPS = 10f;

		// Token: 0x04003974 RID: 14708
		public float scratchJitter = 0.01f;

		// Token: 0x04003975 RID: 14709
		public Texture grainTexture;

		// Token: 0x04003976 RID: 14710
		public Texture scratchTexture;

		// Token: 0x04003977 RID: 14711
		public Shader shaderRGB;

		// Token: 0x04003978 RID: 14712
		public Shader shaderYUV;

		// Token: 0x04003979 RID: 14713
		public Material m_MaterialRGB;

		// Token: 0x0400397A RID: 14714
		public Material m_MaterialYUV;

		// Token: 0x0400397B RID: 14715
		public float scratchTimeLeft;

		// Token: 0x0400397C RID: 14716
		public float scratchX;

		// Token: 0x0400397D RID: 14717
		public float scratchY;
	}
}
