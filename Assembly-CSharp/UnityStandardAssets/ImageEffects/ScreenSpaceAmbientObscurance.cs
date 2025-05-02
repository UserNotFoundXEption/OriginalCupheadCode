using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C8 RID: 1736
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")]
	public class ScreenSpaceAmbientObscurance : PostEffectsBase
	{
		// Token: 0x06004821 RID: 18465 RVA: 0x000394A0 File Offset: 0x000376A0
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.aoMaterial = base.CheckShaderAndCreateMaterial(this.aoShader, this.aoMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x000394D9 File Offset: 0x000376D9
		public void OnDisable()
		{
			if (this.aoMaterial)
			{
				Object.DestroyImmediate(this.aoMaterial);
			}
			this.aoMaterial = null;
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x00163594 File Offset: 0x00161794
		[ImageEffectOpaque]
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
			Matrix4x4 inverse = projectionMatrix.inverse;
			Vector4 vector;
			vector..ctor(-2f / ((float)Screen.width * projectionMatrix[0]), -2f / ((float)Screen.height * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			this.aoMaterial.SetVector("_ProjInfo", vector);
			this.aoMaterial.SetMatrix("_ProjectionInv", inverse);
			this.aoMaterial.SetTexture("_Rand", this.rand);
			this.aoMaterial.SetFloat("_Radius", this.radius);
			this.aoMaterial.SetFloat("_Radius2", this.radius * this.radius);
			this.aoMaterial.SetFloat("_Intensity", this.intensity);
			this.aoMaterial.SetFloat("_BlurFilterDistance", this.blurFilterDistance);
			int width = source.width;
			int height = source.height;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width >> this.downsample, height >> this.downsample);
			Graphics.Blit(source, renderTexture, this.aoMaterial, 0);
			if (this.downsample > 0)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, this.aoMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture);
				this.aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
				renderTexture = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(temporary, renderTexture, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.aoMaterial.SetTexture("_AOTex", renderTexture);
			Graphics.Blit(source, destination, this.aoMaterial, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04003988 RID: 14728
		[Range(0f, 3f)]
		public float intensity = 0.5f;

		// Token: 0x04003989 RID: 14729
		[Range(0.1f, 3f)]
		public float radius = 0.2f;

		// Token: 0x0400398A RID: 14730
		[Range(0f, 3f)]
		public int blurIterations = 1;

		// Token: 0x0400398B RID: 14731
		[Range(0f, 5f)]
		public float blurFilterDistance = 1.25f;

		// Token: 0x0400398C RID: 14732
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x0400398D RID: 14733
		public Texture2D rand;

		// Token: 0x0400398E RID: 14734
		public Shader aoShader;

		// Token: 0x0400398F RID: 14735
		public Material aoMaterial;
	}
}
