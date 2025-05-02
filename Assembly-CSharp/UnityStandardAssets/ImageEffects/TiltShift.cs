using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CC RID: 1740
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	public class TiltShift : PostEffectsBase
	{
		// Token: 0x06004832 RID: 18482 RVA: 0x00039571 File Offset: 0x00037771
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x0016411C File Offset: 0x0016231C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize >= 0f) ? this.maxBlurSize : 0f);
			this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
			source.filterMode = 1;
			RenderTexture renderTexture = destination;
			if ((float)this.downsample > 0f)
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
				renderTexture.filterMode = 1;
			}
			int num = (int)this.quality;
			num *= 2;
			Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode != TiltShift.TiltShiftMode.TiltShiftMode) ? (num + 1) : num);
			if (this.downsample > 0)
			{
				this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
				Graphics.Blit(source, destination, this.tiltShiftMaterial, 6);
			}
			if (renderTexture != destination)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040039A9 RID: 14761
		public TiltShift.TiltShiftMode mode;

		// Token: 0x040039AA RID: 14762
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x040039AB RID: 14763
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x040039AC RID: 14764
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x040039AD RID: 14765
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040039AE RID: 14766
		public Shader tiltShiftShader;

		// Token: 0x040039AF RID: 14767
		public Material tiltShiftMaterial;

		// Token: 0x02001314 RID: 4884
		public enum TiltShiftMode
		{
			// Token: 0x04008251 RID: 33361
			TiltShiftMode,
			// Token: 0x04008252 RID: 33362
			IrisMode
		}

		// Token: 0x02001315 RID: 4885
		public enum TiltShiftQuality
		{
			// Token: 0x04008254 RID: 33364
			Preview,
			// Token: 0x04008255 RID: 33365
			Normal,
			// Token: 0x04008256 RID: 33366
			High
		}
	}
}
