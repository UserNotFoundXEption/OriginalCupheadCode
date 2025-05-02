using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000616 RID: 1558
	public sealed class VignetteComponent : PostProcessingComponentRenderTexture<VignetteModel>
	{
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x00033B95 File Offset: 0x00031D95
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0012E46C File Offset: 0x0012C66C
		public override void Prepare(Material uberMaterial)
		{
			VignetteModel.Settings settings = base.model.settings;
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color);
			if (settings.mode == VignetteModel.Mode.Classic)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC");
				float num = (1f - settings.roundness) * 6f + settings.roundness;
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector4(settings.intensity * 3f, settings.smoothness * 5f, num, (!settings.rounded) ? 0f : 1f));
			}
			else if (settings.mode == VignetteModel.Mode.Masked && settings.mask != null && settings.opacity > 0f)
			{
				uberMaterial.EnableKeyword("VIGNETTE_MASKED");
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask);
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity);
			}
		}

		// Token: 0x02001281 RID: 4737
		public static class Uniforms
		{
			// Token: 0x04007FE2 RID: 32738
			public static readonly int _Vignette_Color = Shader.PropertyToID("_Vignette_Color");

			// Token: 0x04007FE3 RID: 32739
			public static readonly int _Vignette_Center = Shader.PropertyToID("_Vignette_Center");

			// Token: 0x04007FE4 RID: 32740
			public static readonly int _Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

			// Token: 0x04007FE5 RID: 32741
			public static readonly int _Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

			// Token: 0x04007FE6 RID: 32742
			public static readonly int _Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
		}
	}
}
