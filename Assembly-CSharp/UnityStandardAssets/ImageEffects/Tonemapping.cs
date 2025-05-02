using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CD RID: 1741
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")]
	public class Tonemapping : PostEffectsBase
	{
		// Token: 0x06004835 RID: 18485 RVA: 0x0016429C File Offset: 0x0016249C
		public override bool CheckResources()
		{
			base.CheckSupport(false, true);
			this.tonemapMaterial = base.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
			if (!this.curveTex && this.type == Tonemapping.TonemapperType.UserCurve)
			{
				this.curveTex = new Texture2D(256, 1, 5, false, true);
				this.curveTex.filterMode = 1;
				this.curveTex.wrapMode = 1;
				this.curveTex.hideFlags = 52;
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x00164338 File Offset: 0x00162538
		public float UpdateCurve()
		{
			float num = 1f;
			if (this.remapCurve.keys.Length < 1)
			{
				this.remapCurve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(2f, 1f)
				});
			}
			if (this.remapCurve != null)
			{
				if (this.remapCurve.length > 0)
				{
					num = this.remapCurve[this.remapCurve.length - 1].time;
				}
				for (float num2 = 0f; num2 <= 1f; num2 += 0.003921569f)
				{
					float num3 = this.remapCurve.Evaluate(num2 * 1f * num);
					this.curveTex.SetPixel((int)Mathf.Floor(num2 * 255f), 0, new Color(num3, num3, num3));
				}
				this.curveTex.Apply();
			}
			return 1f / num;
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00164448 File Offset: 0x00162648
		public void OnDisable()
		{
			if (this.rt)
			{
				Object.DestroyImmediate(this.rt);
				this.rt = null;
			}
			if (this.tonemapMaterial)
			{
				Object.DestroyImmediate(this.tonemapMaterial);
				this.tonemapMaterial = null;
			}
			if (this.curveTex)
			{
				Object.DestroyImmediate(this.curveTex);
				this.curveTex = null;
			}
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x001644BC File Offset: 0x001626BC
		public bool CreateInternalRenderTexture()
		{
			if (this.rt)
			{
				return false;
			}
			this.rtFormat = ((!SystemInfo.SupportsRenderTextureFormat(13)) ? 2 : 13);
			this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
			this.rt.hideFlags = 52;
			return true;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00164518 File Offset: 0x00162718
		[ImageEffectTransformsToLDR]
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.exposureAdjustment = ((this.exposureAdjustment >= 0.001f) ? this.exposureAdjustment : 0.001f);
			if (this.type == Tonemapping.TonemapperType.UserCurve)
			{
				float num = this.UpdateCurve();
				this.tonemapMaterial.SetFloat("_RangeScale", num);
				this.tonemapMaterial.SetTexture("_Curve", this.curveTex);
				Graphics.Blit(source, destination, this.tonemapMaterial, 4);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.SimpleReinhard)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 6);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Hable)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 5);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Photographic)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 8);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.OptimizedHejiDawson)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 7);
				return;
			}
			bool flag = this.CreateInternalRenderTexture();
			RenderTexture temporary = RenderTexture.GetTemporary((int)this.adaptiveTextureSize, (int)this.adaptiveTextureSize, 0, this.rtFormat);
			Graphics.Blit(source, temporary);
			int num2 = (int)Mathf.Log((float)temporary.width * 1f, 2f);
			int num3 = 2;
			RenderTexture[] array = new RenderTexture[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i] = RenderTexture.GetTemporary(temporary.width / num3, temporary.width / num3, 0, this.rtFormat);
				num3 *= 2;
			}
			RenderTexture renderTexture = array[num2 - 1];
			Graphics.Blit(temporary, array[0], this.tonemapMaterial, 1);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				for (int j = 0; j < num2 - 1; j++)
				{
					Graphics.Blit(array[j], array[j + 1], this.tonemapMaterial, 9);
					renderTexture = array[j + 1];
				}
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				for (int k = 0; k < num2 - 1; k++)
				{
					Graphics.Blit(array[k], array[k + 1]);
					renderTexture = array[k + 1];
				}
			}
			this.adaptionSpeed = ((this.adaptionSpeed >= 0.001f) ? this.adaptionSpeed : 0.001f);
			this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
			this.rt.MarkRestoreExpected();
			Graphics.Blit(renderTexture, this.rt, this.tonemapMaterial, (!flag) ? 2 : 3);
			this.middleGrey = ((this.middleGrey >= 0.001f) ? this.middleGrey : 0.001f);
			this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
			this.tonemapMaterial.SetTexture("_SmallTex", this.rt);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 0);
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 10);
			}
			else
			{
				Debug.LogError("No valid adaptive tonemapper type found!", null);
				Graphics.Blit(source, destination);
			}
			for (int l = 0; l < num2; l++)
			{
				RenderTexture.ReleaseTemporary(array[l]);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040039B0 RID: 14768
		public Tonemapping.TonemapperType type = Tonemapping.TonemapperType.Photographic;

		// Token: 0x040039B1 RID: 14769
		public Tonemapping.AdaptiveTexSize adaptiveTextureSize = Tonemapping.AdaptiveTexSize.Square256;

		// Token: 0x040039B2 RID: 14770
		public AnimationCurve remapCurve;

		// Token: 0x040039B3 RID: 14771
		public Texture2D curveTex;

		// Token: 0x040039B4 RID: 14772
		public float exposureAdjustment = 1.5f;

		// Token: 0x040039B5 RID: 14773
		public float middleGrey = 0.4f;

		// Token: 0x040039B6 RID: 14774
		public float white = 2f;

		// Token: 0x040039B7 RID: 14775
		public float adaptionSpeed = 1.5f;

		// Token: 0x040039B8 RID: 14776
		public Shader tonemapper;

		// Token: 0x040039B9 RID: 14777
		public bool validRenderTextureFormat = true;

		// Token: 0x040039BA RID: 14778
		public Material tonemapMaterial;

		// Token: 0x040039BB RID: 14779
		public RenderTexture rt;

		// Token: 0x040039BC RID: 14780
		public RenderTextureFormat rtFormat = 2;

		// Token: 0x02001316 RID: 4886
		public enum TonemapperType
		{
			// Token: 0x04008258 RID: 33368
			SimpleReinhard,
			// Token: 0x04008259 RID: 33369
			UserCurve,
			// Token: 0x0400825A RID: 33370
			Hable,
			// Token: 0x0400825B RID: 33371
			Photographic,
			// Token: 0x0400825C RID: 33372
			OptimizedHejiDawson,
			// Token: 0x0400825D RID: 33373
			AdaptiveReinhard,
			// Token: 0x0400825E RID: 33374
			AdaptiveReinhardAutoWhite
		}

		// Token: 0x02001317 RID: 4887
		public enum AdaptiveTexSize
		{
			// Token: 0x04008260 RID: 33376
			Square16 = 16,
			// Token: 0x04008261 RID: 33377
			Square32 = 32,
			// Token: 0x04008262 RID: 33378
			Square64 = 64,
			// Token: 0x04008263 RID: 33379
			Square128 = 128,
			// Token: 0x04008264 RID: 33380
			Square256 = 256,
			// Token: 0x04008265 RID: 33381
			Square512 = 512,
			// Token: 0x04008266 RID: 33382
			Square1024 = 1024
		}
	}
}
