using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020000A6 RID: 166
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Screen Overlay Animated")]
public class ScreenOverlayAnimated : PostEffectsBase
{
	// Token: 0x060007F4 RID: 2036 RVA: 0x00007BE3 File Offset: 0x00005DE3
	public override void Start()
	{
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00075030 File Offset: 0x00073230
	public IEnumerator animate_cr()
	{
		for (;;)
		{
			yield return new WaitForSeconds(0.025f);
			if (this.animated)
			{
				this.currentTexture++;
				if (this.currentTexture >= this.textures.Length)
				{
					this.currentTexture = 0;
				}
			}
		}
		yield break;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00007BF2 File Offset: 0x00005DF2
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

	// Token: 0x060007F7 RID: 2039 RVA: 0x0007504C File Offset: 0x0007324C
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.overlayMaterial.SetVector("_UV_Transform", this.UV_Transform);
		this.overlayMaterial.SetFloat("_Intensity", this.intensity);
		if (this.textures != null && this.textures.Length > this.currentTexture && this.textures[this.currentTexture] != null)
		{
			this.overlayMaterial.SetTexture("_Overlay", this.textures[this.currentTexture]);
		}
		Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
	}

	// Token: 0x0400061A RID: 1562
	public const float FRAME_TIME = 0.025f;

	// Token: 0x0400061B RID: 1563
	public Vector4 UV_Transform = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x0400061C RID: 1564
	public ScreenOverlayAnimated.OverlayBlendMode blendMode = ScreenOverlayAnimated.OverlayBlendMode.Overlay;

	// Token: 0x0400061D RID: 1565
	public float intensity = 1f;

	// Token: 0x0400061E RID: 1566
	public bool animated = true;

	// Token: 0x0400061F RID: 1567
	public Texture2D[] textures;

	// Token: 0x04000620 RID: 1568
	public Shader overlayShader;

	// Token: 0x04000621 RID: 1569
	public int currentTexture;

	// Token: 0x04000622 RID: 1570
	public Material overlayMaterial;

	// Token: 0x020008FF RID: 2303
	public enum OverlayBlendMode
	{
		// Token: 0x0400446F RID: 17519
		Additive,
		// Token: 0x04004470 RID: 17520
		ScreenBlend,
		// Token: 0x04004471 RID: 17521
		Multiply,
		// Token: 0x04004472 RID: 17522
		Overlay,
		// Token: 0x04004473 RID: 17523
		AlphaBlend
	}
}
