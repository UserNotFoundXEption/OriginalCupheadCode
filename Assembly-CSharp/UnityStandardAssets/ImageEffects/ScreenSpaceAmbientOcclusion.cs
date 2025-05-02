using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C9 RID: 1737
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		// Token: 0x06004825 RID: 18469 RVA: 0x0016384C File Offset: 0x00161A4C
		public static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = 61
			};
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x000394FD File Offset: 0x000376FD
		public static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x00039513 File Offset: 0x00037713
		public void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x00163878 File Offset: 0x00161A78
		public void Start()
		{
			if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(1))
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.m_Supported = true;
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x00039520 File Offset: 0x00037720
		public void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= 2;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x001638E8 File Offset: 0x00161AE8
		public void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0016393C File Offset: 0x00161B3C
		[ImageEffectOpaque]
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
			this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
			this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
			this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
			this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
			this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
			float fieldOfView = base.GetComponent<Camera>().fieldOfView;
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float num = Mathf.Tan(fieldOfView * 0.0174532924f * 0.5f) * farClipPlane;
			float num2 = num * base.GetComponent<Camera>().aspect;
			this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(num2, num, farClipPlane));
			int num3;
			int num4;
			if (this.m_RandomTexture)
			{
				num3 = this.m_RandomTexture.width;
				num4 = this.m_RandomTexture.height;
			}
			else
			{
				num3 = 1;
				num4 = 1;
			}
			this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num3, (float)renderTexture.height / (float)num4, 0f));
			this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
			bool flag = this.m_Blur > 0;
			Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
			if (flag)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
				Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
				Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
				renderTexture = temporary2;
			}
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04003990 RID: 14736
		public float m_Radius = 0.4f;

		// Token: 0x04003991 RID: 14737
		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		// Token: 0x04003992 RID: 14738
		public float m_OcclusionIntensity = 1.5f;

		// Token: 0x04003993 RID: 14739
		public int m_Blur = 2;

		// Token: 0x04003994 RID: 14740
		public int m_Downsampling = 2;

		// Token: 0x04003995 RID: 14741
		public float m_OcclusionAttenuation = 1f;

		// Token: 0x04003996 RID: 14742
		public float m_MinZ = 0.01f;

		// Token: 0x04003997 RID: 14743
		public Shader m_SSAOShader;

		// Token: 0x04003998 RID: 14744
		public Material m_SSAOMaterial;

		// Token: 0x04003999 RID: 14745
		public Texture2D m_RandomTexture;

		// Token: 0x0400399A RID: 14746
		public bool m_Supported;

		// Token: 0x02001311 RID: 4881
		public enum SSAOSamples
		{
			// Token: 0x04008246 RID: 33350
			Low,
			// Token: 0x04008247 RID: 33351
			Medium,
			// Token: 0x04008248 RID: 33352
			High
		}
	}
}
