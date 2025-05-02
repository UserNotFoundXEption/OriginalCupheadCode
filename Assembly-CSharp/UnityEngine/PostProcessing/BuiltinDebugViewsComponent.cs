using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000609 RID: 1545
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06004009 RID: 16393 RVA: 0x000336E8 File Offset: 0x000318E8
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0012AF88 File Offset: 0x00129188
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = 0;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= 1;
					}
				}
				else
				{
					depthTextureMode |= 5;
				}
			}
			else
			{
				depthTextureMode |= 2;
			}
			return depthTextureMode;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0012AFE4 File Offset: 0x001291E4
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? 12 : 18;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0003371B File Offset: 0x0003191B
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0012B014 File Offset: 0x00129214
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0012B0B8 File Offset: 0x001292B8
		public void DepthPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, 2, material, 0);
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0012B118 File Offset: 0x00129318
		public void DepthNormalsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, 2, material, 1);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0012B14C File Offset: 0x0012934C
		public void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int num = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(num, this.context.width, this.context.height, 0, 1);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, 2);
			cb.Blit(2, num, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
				cb.Blit(num, tempRT, material, 3);
				cb.ReleaseTemporaryRT(num);
				num = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num2 = 1f / (float)motionVectors.motionVectorsResolution;
				float num3 = num2 * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(num3, num2));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
			cb.Blit(num, 2);
			cb.ReleaseTemporaryRT(num);
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0012B354 File Offset: 0x00129554
		public void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00033722 File Offset: 0x00031922
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x0400332B RID: 13099
		public const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x0400332C RID: 13100
		public BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x0200126E RID: 4718
		public static class Uniforms
		{
			// Token: 0x04007F40 RID: 32576
			public static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x04007F41 RID: 32577
			public static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04007F42 RID: 32578
			public static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04007F43 RID: 32579
			public static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04007F44 RID: 32580
			public static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x04007F45 RID: 32581
			public static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04007F46 RID: 32582
			public static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x0200126F RID: 4719
		public enum Pass
		{
			// Token: 0x04007F48 RID: 32584
			Depth,
			// Token: 0x04007F49 RID: 32585
			Normals,
			// Token: 0x04007F4A RID: 32586
			MovecOpacity,
			// Token: 0x04007F4B RID: 32587
			MovecImaging,
			// Token: 0x04007F4C RID: 32588
			MovecArrows
		}

		// Token: 0x02001270 RID: 4720
		public class ArrowArray
		{
			// Token: 0x17001907 RID: 6407
			// (get) Token: 0x060081AC RID: 33196 RVA: 0x0005669D File Offset: 0x0005489D
			// (set) Token: 0x060081AD RID: 33197 RVA: 0x000566A5 File Offset: 0x000548A5
			public Mesh mesh { get; set; }

			// Token: 0x17001908 RID: 6408
			// (get) Token: 0x060081AE RID: 33198 RVA: 0x000566AE File Offset: 0x000548AE
			// (set) Token: 0x060081AF RID: 33199 RVA: 0x000566B6 File Offset: 0x000548B6
			public int columnCount { get; set; }

			// Token: 0x17001909 RID: 6409
			// (get) Token: 0x060081B0 RID: 33200 RVA: 0x000566BF File Offset: 0x000548BF
			// (set) Token: 0x060081B1 RID: 33201 RVA: 0x000566C7 File Offset: 0x000548C7
			public int rowCount { get; set; }

			// Token: 0x060081B2 RID: 33202 RVA: 0x0029AA94 File Offset: 0x00298C94
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item;
						item..ctor((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = 52
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, 3, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x060081B3 RID: 33203 RVA: 0x000566D0 File Offset: 0x000548D0
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
