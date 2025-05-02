using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B8 RID: 1720
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
	public class CreaseShading : PostEffectsBase
	{
		// Token: 0x060047C3 RID: 18371 RVA: 0x0015F3B0 File Offset: 0x0015D5B0
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			this.depthFetchMaterial = base.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
			this.creaseApplyMaterial = base.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0015F424 File Offset: 0x0015D624
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			float num = 1f * (float)width / (1f * (float)height);
			float num2 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
			Graphics.Blit(source, temporary, this.depthFetchMaterial);
			Graphics.Blit(temporary, renderTexture);
			for (int i = 0; i < this.softness; i++)
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(0f, this.spread * num2, 0f, 0f));
				Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary2;
				temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary2;
			}
			this.creaseApplyMaterial.SetTexture("_HrDepthTex", temporary);
			this.creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture);
			this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
			Graphics.Blit(source, destination, this.creaseApplyMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040038F0 RID: 14576
		public float intensity = 0.5f;

		// Token: 0x040038F1 RID: 14577
		public int softness = 1;

		// Token: 0x040038F2 RID: 14578
		public float spread = 1f;

		// Token: 0x040038F3 RID: 14579
		public Shader blurShader;

		// Token: 0x040038F4 RID: 14580
		public Material blurMaterial;

		// Token: 0x040038F5 RID: 14581
		public Shader depthFetchShader;

		// Token: 0x040038F6 RID: 14582
		public Material depthFetchMaterial;

		// Token: 0x040038F7 RID: 14583
		public Shader creaseApplyShader;

		// Token: 0x040038F8 RID: 14584
		public Material creaseApplyMaterial;
	}
}
