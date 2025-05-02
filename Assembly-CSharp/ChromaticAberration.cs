using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
[ExecuteInEditMode]
public class ChromaticAberration : MonoBehaviour
{
	// Token: 0x17000166 RID: 358
	// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00007AD3 File Offset: 0x00005CD3
	public Material material
	{
		get
		{
			if (this.curMaterial == null)
			{
				this.curMaterial = new Material(this.shader);
				this.curMaterial.hideFlags = 61;
			}
			return this.curMaterial;
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00007B0A File Offset: 0x00005D0A
	public virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00074C0C File Offset: 0x00072E0C
	public virtual void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.shader != null)
		{
			float num = (float)destTexture.width / (float)destTexture.height;
			float num2 = (num >= 1.77777779f) ? 1f : (num / 1.77777779f);
			num2 *= 1f - 0.1f * SettingsData.Data.overscan;
			float num3 = num2 * (float)destTexture.height / 1080f;
			this.material.SetVector("_Screen", new Vector2((float)destTexture.width, (float)destTexture.height));
			this.material.SetVector("_Red", this.r * num3);
			this.material.SetVector("_Green", this.g * num3);
			this.material.SetVector("_Blue", this.b * num3);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00007B1E File Offset: 0x00005D1E
	public virtual void OnDisable()
	{
		if (this.curMaterial)
		{
			Object.DestroyImmediate(this.curMaterial);
		}
	}

	// Token: 0x04000606 RID: 1542
	public Shader shader;

	// Token: 0x04000607 RID: 1543
	public Vector2 r;

	// Token: 0x04000608 RID: 1544
	public Vector2 g;

	// Token: 0x04000609 RID: 1545
	public Vector2 b;

	// Token: 0x0400060A RID: 1546
	public Material curMaterial;
}
