using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006BB RID: 1723
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x060047E0 RID: 18400 RVA: 0x00161570 File Offset: 0x0015F770
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00039145 File Offset: 0x00037345
		public new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x001615D8 File Offset: 0x0015F7D8
		public void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= 1;
			}
			else if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= 2;
			}
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x00039153 File Offset: 0x00037353
		public new void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x00161640 File Offset: 0x0015F840
		[ImageEffectOpaque]
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector2 vector;
			vector..ctor(this.sensitivityDepth, this.sensitivityNormals);
			this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
			this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
			this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
			this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
			this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
			this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
			Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
		}

		// Token: 0x0400393C RID: 14652
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x0400393D RID: 14653
		public float sensitivityDepth = 1f;

		// Token: 0x0400393E RID: 14654
		public float sensitivityNormals = 1f;

		// Token: 0x0400393F RID: 14655
		public float lumThreshold = 0.2f;

		// Token: 0x04003940 RID: 14656
		public float edgeExp = 1f;

		// Token: 0x04003941 RID: 14657
		public float sampleDist = 1f;

		// Token: 0x04003942 RID: 14658
		public float edgesOnly;

		// Token: 0x04003943 RID: 14659
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x04003944 RID: 14660
		public Shader edgeDetectShader;

		// Token: 0x04003945 RID: 14661
		public Material edgeDetectMaterial;

		// Token: 0x04003946 RID: 14662
		public EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x0200130F RID: 4879
		public enum EdgeDetectMode
		{
			// Token: 0x0400823A RID: 33338
			TriangleDepthNormals,
			// Token: 0x0400823B RID: 33339
			RobertsCrossDepthNormals,
			// Token: 0x0400823C RID: 33340
			SobelDepth,
			// Token: 0x0400823D RID: 33341
			SobelDepthThin,
			// Token: 0x0400823E RID: 33342
			TriangleLuminance
		}
	}
}
