using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C7 RID: 1735
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x0600481E RID: 18462 RVA: 0x00039437 File Offset: 0x00037637
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x00163508 File Offset: 0x00161708
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector4 vector;
			vector..ctor(1f, 0f, 0f, 1f);
			this.overlayMaterial.SetVector("_UV_Transform", vector);
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			this.overlayMaterial.SetTexture("_Overlay", this.texture);
			Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
		}

		// Token: 0x04003983 RID: 14723
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x04003984 RID: 14724
		public float intensity = 1f;

		// Token: 0x04003985 RID: 14725
		public Texture2D texture;

		// Token: 0x04003986 RID: 14726
		public Shader overlayShader;

		// Token: 0x04003987 RID: 14727
		public Material overlayMaterial;

		// Token: 0x02001310 RID: 4880
		public enum OverlayBlendMode
		{
			// Token: 0x04008240 RID: 33344
			Additive,
			// Token: 0x04008241 RID: 33345
			ScreenBlend,
			// Token: 0x04008242 RID: 33346
			Multiply,
			// Token: 0x04008243 RID: 33347
			Overlay,
			// Token: 0x04008244 RID: 33348
			AlphaBlend
		}
	}
}
