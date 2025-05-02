using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020000A3 RID: 163
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlurGamma : PostEffectsBase
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00007A75 File Offset: 0x00005C75
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00007AAE File Offset: 0x00005CAE
	public void OnDisable()
	{
		if (this.blurMaterial)
		{
			Object.DestroyImmediate(this.blurMaterial);
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00074ABC File Offset: 0x00072CBC
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		float num = (float)source.width / (float)source.height;
		float num2 = (num >= 1.77777779f) ? 1f : (num / 1.77777779f);
		num2 *= 1f - 0.1f * SettingsData.Data.overscan;
		float num3 = (float)source.height / 1080f;
		num3 *= num2;
		if (SettingsData.Data.filter == BlurGamma.Filter.BW)
		{
			num3 *= 1.35f;
		}
		this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3, -this.blurSize * num3, Mathf.Pow(1.4f, -SettingsData.Data.Brightness), 0f));
		source.filterMode = 1;
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		Graphics.Blit(source, temporary, this.blurMaterial, 0);
		int num4 = 1;
		BlurGamma.Filter filter = SettingsData.Data.filter;
		if (filter != BlurGamma.Filter.TwoStrip)
		{
			if (filter == BlurGamma.Filter.BW)
			{
				num4 += 2;
			}
		}
		else
		{
			num4++;
		}
		Graphics.Blit(temporary, destination, this.blurMaterial, num4);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x04000602 RID: 1538
	[Range(0f, 10f)]
	public float blurSize = 3f;

	// Token: 0x04000603 RID: 1539
	[Range(1f, 4f)]
	public int blurIterations = 2;

	// Token: 0x04000604 RID: 1540
	public Shader blurShader;

	// Token: 0x04000605 RID: 1541
	public Material blurMaterial;

	// Token: 0x020008FC RID: 2300
	public enum Filter
	{
		// Token: 0x04004459 RID: 17497
		None,
		// Token: 0x0400445A RID: 17498
		TwoStrip,
		// Token: 0x0400445B RID: 17499
		BW,
		// Token: 0x0400445C RID: 17500
		Chalice
	}
}
