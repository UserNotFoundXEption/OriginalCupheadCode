using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B0 RID: 1712
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600478D RID: 18317 RVA: 0x00038D94 File Offset: 0x00036F94
		public Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = 52;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x00038DC7 File Offset: 0x00036FC7
		public void OnDisable()
		{
			if (Blur.m_Material)
			{
				Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0015D460 File Offset: 0x0015B660
		public void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0015D4AC File Offset: 0x0015B6AC
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0015D52C File Offset: 0x0015B72C
		public void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0015D5A4 File Offset: 0x0015B7A4
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int num = source.width / 4;
			int num2 = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(num, num2, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04003897 RID: 14487
		public int iterations = 3;

		// Token: 0x04003898 RID: 14488
		public float blurSpread = 0.6f;

		// Token: 0x04003899 RID: 14489
		public Shader blurShader;

		// Token: 0x0400389A RID: 14490
		public static Material m_Material;
	}
}
