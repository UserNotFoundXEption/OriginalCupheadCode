using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006A8 RID: 1704
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x06004776 RID: 18294 RVA: 0x0015B98C File Offset: 0x00159B8C
		public Material CurrentAAMaterial()
		{
			Material result;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				result = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				result = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				result = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				result = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				result = this.nfaa;
				break;
			case AAMode.SSAA:
				result = this.ssaa;
				break;
			case AAMode.DLAA:
				result = this.dlaa;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0015BA28 File Offset: 0x00159C28
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0015BB08 File Offset: 0x00159D08
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
			}
			else if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
			}
			else if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
			}
			else if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
			}
			else if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
			}
			else if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, (!this.dlaaSharp) ? 1 : 2);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, (!this.showGeneratedNormals) ? 0 : 1);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}

		// Token: 0x0400382B RID: 14379
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x0400382C RID: 14380
		public bool showGeneratedNormals;

		// Token: 0x0400382D RID: 14381
		public float offsetScale = 0.2f;

		// Token: 0x0400382E RID: 14382
		public float blurRadius = 18f;

		// Token: 0x0400382F RID: 14383
		public float edgeThresholdMin = 0.05f;

		// Token: 0x04003830 RID: 14384
		public float edgeThreshold = 0.2f;

		// Token: 0x04003831 RID: 14385
		public float edgeSharpness = 4f;

		// Token: 0x04003832 RID: 14386
		public bool dlaaSharp;

		// Token: 0x04003833 RID: 14387
		public Shader ssaaShader;

		// Token: 0x04003834 RID: 14388
		public Material ssaa;

		// Token: 0x04003835 RID: 14389
		public Shader dlaaShader;

		// Token: 0x04003836 RID: 14390
		public Material dlaa;

		// Token: 0x04003837 RID: 14391
		public Shader nfaaShader;

		// Token: 0x04003838 RID: 14392
		public Material nfaa;

		// Token: 0x04003839 RID: 14393
		public Shader shaderFXAAPreset2;

		// Token: 0x0400383A RID: 14394
		public Material materialFXAAPreset2;

		// Token: 0x0400383B RID: 14395
		public Shader shaderFXAAPreset3;

		// Token: 0x0400383C RID: 14396
		public Material materialFXAAPreset3;

		// Token: 0x0400383D RID: 14397
		public Shader shaderFXAAII;

		// Token: 0x0400383E RID: 14398
		public Material materialFXAAII;

		// Token: 0x0400383F RID: 14399
		public Shader shaderFXAAIII;

		// Token: 0x04003840 RID: 14400
		public Material materialFXAAIII;
	}
}
