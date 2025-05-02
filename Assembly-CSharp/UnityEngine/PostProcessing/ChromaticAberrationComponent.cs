using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200060A RID: 1546
	public sealed class ChromaticAberrationComponent : PostProcessingComponentRenderTexture<ChromaticAberrationModel>
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06004014 RID: 16404 RVA: 0x0012B3D8 File Offset: 0x001295D8
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x00033749 File Offset: 0x00031949
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_SpectrumLut);
			this.m_SpectrumLut = null;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0012B424 File Offset: 0x00129624
		public override void Prepare(Material uberMaterial)
		{
			ChromaticAberrationModel.Settings settings = base.model.settings;
			Texture2D texture2D = settings.spectralTexture;
			if (texture2D == null)
			{
				if (this.m_SpectrumLut == null)
				{
					this.m_SpectrumLut = new Texture2D(3, 1, 3, false)
					{
						name = "Chromatic Aberration Spectrum Lookup",
						filterMode = 1,
						wrapMode = 1,
						anisoLevel = 0,
						hideFlags = 52
					};
					Color[] pixels = new Color[]
					{
						new Color(1f, 0f, 0f),
						new Color(0f, 1f, 0f),
						new Color(0f, 0f, 1f)
					};
					this.m_SpectrumLut.SetPixels(pixels);
					this.m_SpectrumLut.Apply();
				}
				texture2D = this.m_SpectrumLut;
			}
			uberMaterial.EnableKeyword("CHROMATIC_ABERRATION");
			uberMaterial.SetFloat(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03f);
			uberMaterial.SetTexture(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Spectrum, texture2D);
		}

		// Token: 0x0400332D RID: 13101
		public Texture2D m_SpectrumLut;

		// Token: 0x02001271 RID: 4721
		public static class Uniforms
		{
			// Token: 0x04007F50 RID: 32592
			public static readonly int _ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");

			// Token: 0x04007F51 RID: 32593
			public static readonly int _ChromaticAberration_Spectrum = Shader.PropertyToID("_ChromaticAberration_Spectrum");
		}
	}
}
