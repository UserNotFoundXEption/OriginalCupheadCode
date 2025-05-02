using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B3 RID: 1715
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		// Token: 0x060047A5 RID: 18341 RVA: 0x00038EC5 File Offset: 0x000370C5
		public new void Start()
		{
			base.Start();
			this.updateTexturesOnStartup = true;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x00038ED4 File Offset: 0x000370D4
		public void Awake()
		{
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x0015E6C0 File Offset: 0x0015C8C0
		public override bool CheckResources()
		{
			base.CheckSupport(this.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced);
			this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
			this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
			this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
			if (!this.rgbChannelTex)
			{
				this.rgbChannelTex = new Texture2D(256, 4, 5, false, true);
			}
			if (!this.rgbDepthChannelTex)
			{
				this.rgbDepthChannelTex = new Texture2D(256, 4, 5, false, true);
			}
			if (!this.zCurveTex)
			{
				this.zCurveTex = new Texture2D(256, 1, 5, false, true);
			}
			this.rgbChannelTex.hideFlags = 52;
			this.rgbDepthChannelTex.hideFlags = 52;
			this.zCurveTex.hideFlags = 52;
			this.rgbChannelTex.wrapMode = 1;
			this.rgbDepthChannelTex.wrapMode = 1;
			this.zCurveTex.wrapMode = 1;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x0015E7F4 File Offset: 0x0015C9F4
		public void UpdateParameters()
		{
			this.CheckResources();
			if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(this.redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(this.greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(this.blueChannel.Evaluate(num), 0f, 1f);
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(this.zCurve.Evaluate(num), 0f, 1f);
					this.zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(this.depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(num), 0f, 1f);
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				this.rgbChannelTex.Apply();
				this.rgbDepthChannelTex.Apply();
				this.zCurveTex.Apply();
			}
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x00038ED6 File Offset: 0x000370D6
		public void UpdateTextures()
		{
			this.UpdateParameters();
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0015EA18 File Offset: 0x0015CC18
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.updateTexturesOnStartup)
			{
				this.UpdateParameters();
				this.updateTexturesOnStartup = false;
			}
			if (this.useDepthCorrection)
			{
				base.GetComponent<Camera>().depthTextureMode |= 1;
			}
			RenderTexture renderTexture = destination;
			if (this.selectiveCc)
			{
				renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (this.useDepthCorrection)
			{
				this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
				this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
				this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccDepthMaterial);
			}
			else
			{
				this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccMaterial);
			}
			if (this.selectiveCc)
			{
				this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
				this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
				Graphics.Blit(renderTexture, destination, this.selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040038BF RID: 14527
		public AnimationCurve redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C0 RID: 14528
		public AnimationCurve greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C1 RID: 14529
		public AnimationCurve blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C2 RID: 14530
		public bool useDepthCorrection;

		// Token: 0x040038C3 RID: 14531
		public AnimationCurve zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C4 RID: 14532
		public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C5 RID: 14533
		public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C6 RID: 14534
		public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040038C7 RID: 14535
		public Material ccMaterial;

		// Token: 0x040038C8 RID: 14536
		public Material ccDepthMaterial;

		// Token: 0x040038C9 RID: 14537
		public Material selectiveCcMaterial;

		// Token: 0x040038CA RID: 14538
		public Texture2D rgbChannelTex;

		// Token: 0x040038CB RID: 14539
		public Texture2D rgbDepthChannelTex;

		// Token: 0x040038CC RID: 14540
		public Texture2D zCurveTex;

		// Token: 0x040038CD RID: 14541
		public float saturation = 1f;

		// Token: 0x040038CE RID: 14542
		public bool selectiveCc;

		// Token: 0x040038CF RID: 14543
		public Color selectiveFromColor = Color.white;

		// Token: 0x040038D0 RID: 14544
		public Color selectiveToColor = Color.white;

		// Token: 0x040038D1 RID: 14545
		public ColorCorrectionCurves.ColorCorrectionMode mode;

		// Token: 0x040038D2 RID: 14546
		public bool updateTextures = true;

		// Token: 0x040038D3 RID: 14547
		public Shader colorCorrectionCurvesShader;

		// Token: 0x040038D4 RID: 14548
		public Shader simpleColorCorrectionCurvesShader;

		// Token: 0x040038D5 RID: 14549
		public Shader colorCorrectionSelectiveShader;

		// Token: 0x040038D6 RID: 14550
		public bool updateTexturesOnStartup = true;

		// Token: 0x02001308 RID: 4872
		public enum ColorCorrectionMode
		{
			// Token: 0x04008221 RID: 33313
			Simple,
			// Token: 0x04008222 RID: 33314
			Advanced
		}
	}
}
