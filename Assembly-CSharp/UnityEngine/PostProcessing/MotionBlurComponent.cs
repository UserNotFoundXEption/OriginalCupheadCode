using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000612 RID: 1554
	public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
	{
		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x00033A2E File Offset: 0x00031C2E
		public MotionBlurComponent.ReconstructionFilter reconstructionFilter
		{
			get
			{
				if (this.m_ReconstructionFilter == null)
				{
					this.m_ReconstructionFilter = new MotionBlurComponent.ReconstructionFilter();
				}
				return this.m_ReconstructionFilter;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x00033A4C File Offset: 0x00031C4C
		public MotionBlurComponent.FrameBlendingFilter frameBlendingFilter
		{
			get
			{
				if (this.m_FrameBlendingFilter == null)
				{
					this.m_FrameBlendingFilter = new MotionBlurComponent.FrameBlendingFilter();
				}
				return this.m_FrameBlendingFilter;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x0012D188 File Offset: 0x0012B388
		public override bool active
		{
			get
			{
				MotionBlurModel.Settings settings = base.model.settings;
				return base.model.enabled && ((settings.shutterAngle > 0f && this.reconstructionFilter.IsSupported()) || settings.frameBlending > 0f) && SystemInfo.graphicsDeviceType != 8 && !this.context.interrupted;
			}
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00033A6A File Offset: 0x00031C6A
		public override string GetName()
		{
			return "Motion Blur";
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00033A71 File Offset: 0x00031C71
		public void ResetHistory()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
			this.m_FrameBlendingFilter = null;
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00033A90 File Offset: 0x00031C90
		public override DepthTextureMode GetCameraFlags()
		{
			return 5;
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00033A93 File Offset: 0x00031C93
		public override CameraEvent GetCameraEvent()
		{
			return 18;
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00033A97 File Offset: 0x00031C97
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0012D200 File Offset: 0x0012B400
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			if (this.m_FirstFrame)
			{
				this.m_FirstFrame = false;
				return;
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			MotionBlurModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 7 : 9;
			int tempRT = MotionBlurComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 0, renderTextureFormat);
			if (settings.shutterAngle > 0f && settings.frameBlending > 0f)
			{
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, 2, tempRT, material);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			else if (settings.shutterAngle > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, tempRT, 2, material);
			}
			else if (settings.frameBlending > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			cb.ReleaseTemporaryRT(tempRT);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00033AA0 File Offset: 0x00031CA0
		public override void OnDisable()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
		}

		// Token: 0x04003346 RID: 13126
		public MotionBlurComponent.ReconstructionFilter m_ReconstructionFilter;

		// Token: 0x04003347 RID: 13127
		public MotionBlurComponent.FrameBlendingFilter m_FrameBlendingFilter;

		// Token: 0x04003348 RID: 13128
		public bool m_FirstFrame = true;

		// Token: 0x02001279 RID: 4729
		public static class Uniforms
		{
			// Token: 0x04007F84 RID: 32644
			public static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");

			// Token: 0x04007F85 RID: 32645
			public static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

			// Token: 0x04007F86 RID: 32646
			public static readonly int _RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");

			// Token: 0x04007F87 RID: 32647
			public static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");

			// Token: 0x04007F88 RID: 32648
			public static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04007F89 RID: 32649
			public static readonly int _Tile2RT = Shader.PropertyToID("_Tile2RT");

			// Token: 0x04007F8A RID: 32650
			public static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");

			// Token: 0x04007F8B RID: 32651
			public static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");

			// Token: 0x04007F8C RID: 32652
			public static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

			// Token: 0x04007F8D RID: 32653
			public static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

			// Token: 0x04007F8E RID: 32654
			public static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");

			// Token: 0x04007F8F RID: 32655
			public static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

			// Token: 0x04007F90 RID: 32656
			public static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");

			// Token: 0x04007F91 RID: 32657
			public static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04007F92 RID: 32658
			public static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");

			// Token: 0x04007F93 RID: 32659
			public static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");

			// Token: 0x04007F94 RID: 32660
			public static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");

			// Token: 0x04007F95 RID: 32661
			public static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");

			// Token: 0x04007F96 RID: 32662
			public static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");

			// Token: 0x04007F97 RID: 32663
			public static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");

			// Token: 0x04007F98 RID: 32664
			public static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");

			// Token: 0x04007F99 RID: 32665
			public static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");

			// Token: 0x04007F9A RID: 32666
			public static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");

			// Token: 0x04007F9B RID: 32667
			public static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");

			// Token: 0x04007F9C RID: 32668
			public static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");

			// Token: 0x04007F9D RID: 32669
			public static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
		}

		// Token: 0x0200127A RID: 4730
		public enum Pass
		{
			// Token: 0x04007F9F RID: 32671
			VelocitySetup,
			// Token: 0x04007FA0 RID: 32672
			TileMax1,
			// Token: 0x04007FA1 RID: 32673
			TileMax2,
			// Token: 0x04007FA2 RID: 32674
			TileMaxV,
			// Token: 0x04007FA3 RID: 32675
			NeighborMax,
			// Token: 0x04007FA4 RID: 32676
			Reconstruction,
			// Token: 0x04007FA5 RID: 32677
			FrameCompression,
			// Token: 0x04007FA6 RID: 32678
			FrameBlendingChroma,
			// Token: 0x04007FA7 RID: 32679
			FrameBlendingRaw
		}

		// Token: 0x0200127B RID: 4731
		public class ReconstructionFilter
		{
			// Token: 0x060081BD RID: 33213 RVA: 0x00056782 File Offset: 0x00054982
			public ReconstructionFilter()
			{
				this.CheckTextureFormatSupport();
			}

			// Token: 0x060081BE RID: 33214 RVA: 0x0005679F File Offset: 0x0005499F
			public void CheckTextureFormatSupport()
			{
				if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
				{
					this.m_PackedRTFormat = 0;
				}
			}

			// Token: 0x060081BF RID: 33215 RVA: 0x000567B8 File Offset: 0x000549B8
			public bool IsSupported()
			{
				return SystemInfo.supportsMotionVectors;
			}

			// Token: 0x060081C0 RID: 33216 RVA: 0x0029B0BC File Offset: 0x002992BC
			public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				int num = (int)(5f * (float)context.height / 100f);
				int num2 = ((num - 1) / 8 + 1) * 8;
				float num3 = settings.shutterAngle / 360f;
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1f / (float)num);
				int velocityTex = MotionBlurComponent.Uniforms._VelocityTex;
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, 0, this.m_PackedRTFormat, 1);
				cb.Blit(null, velocityTex, material, 0);
				int tile2RT = MotionBlurComponent.Uniforms._Tile2RT;
				cb.GetTemporaryRT(tile2RT, context.width / 2, context.height / 2, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex);
				cb.Blit(velocityTex, tile2RT, material, 1);
				int tile4RT = MotionBlurComponent.Uniforms._Tile4RT;
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile2RT);
				cb.Blit(tile2RT, tile4RT, material, 2);
				cb.ReleaseTemporaryRT(tile2RT);
				int tile8RT = MotionBlurComponent.Uniforms._Tile8RT;
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT);
				cb.Blit(tile4RT, tile8RT, material, 2);
				cb.ReleaseTemporaryRT(tile4RT);
				Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float)((int)((float)num2 / 8f)));
				int tileVRT = MotionBlurComponent.Uniforms._TileVRT;
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT);
				cb.Blit(tile8RT, tileVRT, material, 3);
				cb.ReleaseTemporaryRT(tile8RT);
				int neighborMaxTex = MotionBlurComponent.Uniforms._NeighborMaxTex;
				int num4 = context.width / num2;
				int num5 = context.height / num2;
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT);
				cb.Blit(tileVRT, neighborMaxTex, material, 4);
				cb.ReleaseTemporaryRT(tileVRT);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float)Mathf.Clamp(settings.sampleCount / 2, 1, 64));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, 5);
				cb.ReleaseTemporaryRT(velocityTex);
				cb.ReleaseTemporaryRT(neighborMaxTex);
			}

			// Token: 0x04007FA8 RID: 32680
			public RenderTextureFormat m_VectorRTFormat = 13;

			// Token: 0x04007FA9 RID: 32681
			public RenderTextureFormat m_PackedRTFormat = 8;
		}

		// Token: 0x0200127C RID: 4732
		public class FrameBlendingFilter
		{
			// Token: 0x060081C1 RID: 33217 RVA: 0x000567BF File Offset: 0x000549BF
			public FrameBlendingFilter()
			{
				this.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression();
				this.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat();
				this.m_FrameList = new MotionBlurComponent.FrameBlendingFilter.Frame[4];
			}

			// Token: 0x060081C2 RID: 33218 RVA: 0x0029B3A0 File Offset: 0x002995A0
			public void Dispose()
			{
				foreach (MotionBlurComponent.FrameBlendingFilter.Frame frame in this.m_FrameList)
				{
					frame.Release();
				}
			}

			// Token: 0x060081C3 RID: 33219 RVA: 0x0029B3DC File Offset: 0x002995DC
			public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
			{
				int frameCount = Time.frameCount;
				if (frameCount == this.m_LastFrameCount)
				{
					return;
				}
				int num = frameCount % this.m_FrameList.Length;
				if (this.m_UseCompression)
				{
					this.m_FrameList[num].MakeRecord(cb, source, width, height, material);
				}
				else
				{
					this.m_FrameList[num].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
				}
				this.m_LastFrameCount = frameCount;
			}

			// Token: 0x060081C4 RID: 33220 RVA: 0x0029B454 File Offset: 0x00299654
			public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				float time = Time.time;
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative = this.GetFrameRelative(-1);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, (!this.m_UseCompression) ? 8 : 7);
			}

			// Token: 0x060081C5 RID: 33221 RVA: 0x000567E9 File Offset: 0x000549E9
			public static bool CheckSupportCompression()
			{
				return SystemInfo.SupportsRenderTextureFormat(16) && SystemInfo.supportedRenderTargetCount > 1;
			}

			// Token: 0x060081C6 RID: 33222 RVA: 0x0029B5BC File Offset: 0x002997BC
			public static RenderTextureFormat GetPreferredRenderTextureFormat()
			{
				RenderTextureFormat[] array = new RenderTextureFormat[3];
				RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.$field-51A7A390CD6DE245186881400B18C9D822EFE240).FieldHandle);
				RenderTextureFormat[] array2 = array;
				foreach (RenderTextureFormat renderTextureFormat in array2)
				{
					if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
					{
						return renderTextureFormat;
					}
				}
				return 7;
			}

			// Token: 0x060081C7 RID: 33223 RVA: 0x0029B604 File Offset: 0x00299804
			public MotionBlurComponent.FrameBlendingFilter.Frame GetFrameRelative(int offset)
			{
				int num = (Time.frameCount + this.m_FrameList.Length + offset) % this.m_FrameList.Length;
				return this.m_FrameList[num];
			}

			// Token: 0x04007FAA RID: 32682
			public bool m_UseCompression;

			// Token: 0x04007FAB RID: 32683
			public RenderTextureFormat m_RawTextureFormat;

			// Token: 0x04007FAC RID: 32684
			public MotionBlurComponent.FrameBlendingFilter.Frame[] m_FrameList;

			// Token: 0x04007FAD RID: 32685
			public int m_LastFrameCount;

			// Token: 0x020015F8 RID: 5624
			public struct Frame
			{
				// Token: 0x06008784 RID: 34692 RVA: 0x002A5344 File Offset: 0x002A3544
				public float CalculateWeight(float strength, float currentTime)
				{
					if (Mathf.Approximately(this.m_Time, 0f))
					{
						return 0f;
					}
					float num = Mathf.Lerp(80f, 16f, strength);
					return Mathf.Exp((this.m_Time - currentTime) * num);
				}

				// Token: 0x06008785 RID: 34693 RVA: 0x002A538C File Offset: 0x002A358C
				public void Release()
				{
					if (this.lumaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.lumaTexture);
					}
					if (this.chromaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.chromaTexture);
					}
					this.lumaTexture = null;
					this.chromaTexture = null;
				}

				// Token: 0x06008786 RID: 34694 RVA: 0x002A53E0 File Offset: 0x002A35E0
				public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, 16, 1);
					this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, 16, 1);
					this.lumaTexture.filterMode = 0;
					this.chromaTexture.filterMode = 0;
					if (this.m_MRT == null)
					{
						this.m_MRT = new RenderTargetIdentifier[2];
					}
					this.m_MRT[0] = this.lumaTexture;
					this.m_MRT[1] = this.chromaTexture;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6);
					this.m_Time = Time.time;
				}

				// Token: 0x06008787 RID: 34695 RVA: 0x002A54C0 File Offset: 0x002A36C0
				public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
					this.lumaTexture.filterMode = 0;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.Blit(source, this.lumaTexture);
					this.m_Time = Time.time;
				}

				// Token: 0x04009245 RID: 37445
				public RenderTexture lumaTexture;

				// Token: 0x04009246 RID: 37446
				public RenderTexture chromaTexture;

				// Token: 0x04009247 RID: 37447
				public float m_Time;

				// Token: 0x04009248 RID: 37448
				public RenderTargetIdentifier[] m_MRT;
			}
		}
	}
}
