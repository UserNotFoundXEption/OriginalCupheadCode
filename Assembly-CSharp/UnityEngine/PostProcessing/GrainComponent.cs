using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000611 RID: 1553
	public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0012CFA8 File Offset: 0x0012B1A8
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && SystemInfo.SupportsRenderTextureFormat(2) && !this.context.interrupted;
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x00033A0B File Offset: 0x00031C0B
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GrainLookupRT);
			this.m_GrainLookupRT = null;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0012D000 File Offset: 0x0012B200
		public override void Prepare(Material uberMaterial)
		{
			GrainModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("GRAIN");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float value = Random.value;
			float value2 = Random.value;
			if (this.m_GrainLookupRT == null || !this.m_GrainLookupRT.IsCreated())
			{
				GraphicsUtils.Destroy(this.m_GrainLookupRT);
				this.m_GrainLookupRT = new RenderTexture(192, 192, 0, 2)
				{
					filterMode = 1,
					wrapMode = 0,
					anisoLevel = 0,
					name = "Grain Lookup Texture"
				};
				this.m_GrainLookupRT.Create();
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20f);
			Graphics.Blit(null, this.m_GrainLookupRT, material, (!settings.colored) ? 0 : 1);
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, this.m_GrainLookupRT);
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, new Vector4((float)this.context.width / (float)this.m_GrainLookupRT.width / settings.size, (float)this.context.height / (float)this.m_GrainLookupRT.height / settings.size, value, value2));
		}

		// Token: 0x04003345 RID: 13125
		public RenderTexture m_GrainLookupRT;

		// Token: 0x02001278 RID: 4728
		public static class Uniforms
		{
			// Token: 0x04007F80 RID: 32640
			public static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

			// Token: 0x04007F81 RID: 32641
			public static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

			// Token: 0x04007F82 RID: 32642
			public static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");

			// Token: 0x04007F83 RID: 32643
			public static readonly int _Phase = Shader.PropertyToID("_Phase");
		}
	}
}
