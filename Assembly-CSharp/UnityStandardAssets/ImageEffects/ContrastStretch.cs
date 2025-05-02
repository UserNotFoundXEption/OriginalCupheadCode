using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B7 RID: 1719
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x00038FB9 File Offset: 0x000371B9
		public Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = 61;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x00038FF0 File Offset: 0x000371F0
		public Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = 61;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060047BB RID: 18363 RVA: 0x00039027 File Offset: 0x00037227
		public Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = 61;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x0003905E File Offset: 0x0003725E
		public Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = 61;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0015F0C0 File Offset: 0x0015D2C0
		public void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0015F128 File Offset: 0x0015D328
		public void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = 61;
				}
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0015F178 File Offset: 0x0015D378
		public void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0015F21C File Offset: 0x0015D41C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x0015F2E0 File Offset: 0x0015D4E0
		public void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}

		// Token: 0x040038E3 RID: 14563
		public float adaptationSpeed = 0.02f;

		// Token: 0x040038E4 RID: 14564
		public float limitMinimum = 0.2f;

		// Token: 0x040038E5 RID: 14565
		public float limitMaximum = 0.6f;

		// Token: 0x040038E6 RID: 14566
		public RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x040038E7 RID: 14567
		public int curAdaptIndex;

		// Token: 0x040038E8 RID: 14568
		public Shader shaderLum;

		// Token: 0x040038E9 RID: 14569
		public Material m_materialLum;

		// Token: 0x040038EA RID: 14570
		public Shader shaderReduce;

		// Token: 0x040038EB RID: 14571
		public Material m_materialReduce;

		// Token: 0x040038EC RID: 14572
		public Shader shaderAdapt;

		// Token: 0x040038ED RID: 14573
		public Material m_materialAdapt;

		// Token: 0x040038EE RID: 14574
		public Shader shaderApply;

		// Token: 0x040038EF RID: 14575
		public Material m_materialApply;
	}
}
