using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020000A5 RID: 165
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Chromatic Aberration Film Grain")]
public class ChromaticAberrationFilmGrain : PostEffectsBase
{
	// Token: 0x060007EC RID: 2028 RVA: 0x00074D70 File Offset: 0x00072F70
	public void Initialize(Texture2D[] filmGrain)
	{
		base.enabled = true;
		this.rStart = this.r;
		this.gStart = this.g;
		this.bStart = this.b;
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		this.textures = filmGrain;
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00074DD0 File Offset: 0x00072FD0
	public IEnumerator animate_cr()
	{
		float t = 0f;
		int loopsUntilFullLoop = Random.Range(7, 15);
		for (;;)
		{
			t += Time.deltaTime;
			while (t > 0.025f)
			{
				t -= 0.025f;
				if (this.animated)
				{
					this.currentTexture++;
					if (loopsUntilFullLoop > 0)
					{
						if (this.currentTexture >= this.earlyLoopPoint)
						{
							this.currentTexture = 0;
							loopsUntilFullLoop--;
							this.UV_Transform = new Vector4((float)MathUtils.PlusOrMinus(), 0f, 0f, (float)MathUtils.PlusOrMinus());
						}
					}
					else if (this.currentTexture >= this.textures.Length)
					{
						this.currentTexture = 0;
						loopsUntilFullLoop = Random.Range(7, 15);
						this.UV_Transform = new Vector4((float)MathUtils.PlusOrMinus(), 0f, 0f, (float)MathUtils.PlusOrMinus());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00007B3B File Offset: 0x00005D3B
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00074DEC File Offset: 0x00072FEC
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.material.SetVector("_UV_Transform", this.UV_Transform);
		this.material.SetFloat("_Intensity", this.intensity);
		if (this.textures != null && this.textures.Length > this.currentTexture && this.textures[this.currentTexture] != null)
		{
			this.material.SetTexture("_Overlay", this.textures[this.currentTexture]);
		}
		float num = (float)source.width / (float)source.height;
		float num2 = (num >= 1.77777779f) ? 1f : (num / 1.77777779f);
		num2 *= 1f - 0.1f * SettingsData.Data.overscan;
		float num3 = SettingsData.Data.chromaticAberration * num2 * (float)source.height / 1080f;
		Vector2 vector = this.r * num3;
		Vector2 vector2 = this.g * num3;
		Vector2 vector3 = this.b * num3;
		if (SettingsData.Data.filter == BlurGamma.Filter.TwoStrip)
		{
			Vector2 vector4 = vector3 * 0.4f + vector2 * 0.6f;
			vector2 = vector4;
		}
		this.material.SetVector("_Screen", new Vector2((float)source.width, (float)source.height));
		this.material.SetVector("_Red", vector);
		this.material.SetVector("_Green", vector2);
		this.material.SetVector("_Blue", vector3);
		int num4 = 0;
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
		Graphics.Blit(source, destination, this.material, num4);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00007B74 File Offset: 0x00005D74
	public virtual void OnDisable()
	{
		if (this.material)
		{
			Object.DestroyImmediate(this.material);
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00007B91 File Offset: 0x00005D91
	public void PsychedelicEffect(float amount, float speed, float time)
	{
		base.StartCoroutine(this.psychedelic_effect(amount, speed, time));
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00075000 File Offset: 0x00073200
	public IEnumerator psychedelic_effect(float amount, float speed, float time)
	{
		float t = 0f;
		float slowdownTime = 0.5f;
		while (amount > 0f)
		{
			t += Time.deltaTime;
			float angle = speed * t;
			float phase = Mathf.Sin(angle) * amount;
			this.r = Vector2.up * phase;
			this.g = Vector2.up * phase / 2f;
			this.b = Vector2.down * phase;
			if (t >= time)
			{
				amount -= slowdownTime;
			}
			yield return null;
		}
		this.r = this.rStart;
		this.g = this.gStart;
		this.b = this.bStart;
		yield return null;
		yield break;
	}

	// Token: 0x0400060B RID: 1547
	public Shader shader;

	// Token: 0x0400060C RID: 1548
	public Material material;

	// Token: 0x0400060D RID: 1549
	public const float FRAME_TIME = 0.025f;

	// Token: 0x0400060E RID: 1550
	public Vector4 UV_Transform = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x0400060F RID: 1551
	public float intensity = 1f;

	// Token: 0x04000610 RID: 1552
	public bool animated = true;

	// Token: 0x04000611 RID: 1553
	public int earlyLoopPoint = 102;

	// Token: 0x04000612 RID: 1554
	public int currentTexture;

	// Token: 0x04000613 RID: 1555
	public Vector2 r;

	// Token: 0x04000614 RID: 1556
	public Vector2 g;

	// Token: 0x04000615 RID: 1557
	public Vector2 b;

	// Token: 0x04000616 RID: 1558
	public Texture2D[] textures;

	// Token: 0x04000617 RID: 1559
	public Vector2 rStart;

	// Token: 0x04000618 RID: 1560
	public Vector2 gStart;

	// Token: 0x04000619 RID: 1561
	public Vector2 bStart;
}
