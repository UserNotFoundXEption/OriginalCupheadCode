using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000613 RID: 1555
	public sealed class ScreenSpaceReflectionComponent : PostProcessingComponentCommandBuffer<ScreenSpaceReflectionModel>
	{
		// Token: 0x0600405D RID: 16477 RVA: 0x00033ADA File Offset: 0x00031CDA
		public override DepthTextureMode GetCameraFlags()
		{
			return 1;
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x00033ADD File Offset: 0x00031CDD
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && !this.context.interrupted;
			}
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0012D418 File Offset: 0x0012B618
		public override void OnEnable()
		{
			this.m_ReflectionTextures[0] = Shader.PropertyToID("_ReflectionTexture0");
			this.m_ReflectionTextures[1] = Shader.PropertyToID("_ReflectionTexture1");
			this.m_ReflectionTextures[2] = Shader.PropertyToID("_ReflectionTexture2");
			this.m_ReflectionTextures[3] = Shader.PropertyToID("_ReflectionTexture3");
			this.m_ReflectionTextures[4] = Shader.PropertyToID("_ReflectionTexture4");
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00033B10 File Offset: 0x00031D10
		public override string GetName()
		{
			return "Screen Space Reflection";
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00033B17 File Offset: 0x00031D17
		public override CameraEvent GetCameraEvent()
		{
			return 9;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0012D480 File Offset: 0x0012B680
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ScreenSpaceReflectionModel.Settings settings = base.model.settings;
			Camera camera = this.context.camera;
			int num = (settings.reflection.reflectionQuality != ScreenSpaceReflectionModel.SSRResolution.High) ? 2 : 1;
			int num2 = this.context.width / num;
			int num3 = this.context.height / num;
			float num4 = (float)this.context.width;
			float num5 = (float)this.context.height;
			float num6 = num4 / 2f;
			float num7 = num5 / 2f;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Screen Space Reflection");
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._RayStepSize, settings.reflection.stepSize);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AdditiveReflection, (settings.reflection.blendType != ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._BilateralUpsampling, (!this.k_BilateralUpsample) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TreatBackfaceHitAsMiss, (!this.k_TreatBackfaceHitAsMiss) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AllowBackwardsRays, (!settings.reflection.reflectBackfaces) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TraceBehindObjects, (!this.k_TraceBehindObjects) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._MaxSteps, settings.reflection.iterationCount);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._FullResolutionFiltering, 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HalfResolution, (settings.reflection.reflectionQuality == ScreenSpaceReflectionModel.SSRResolution.High) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HighlightSuppression, (!this.k_HighlightSuppression) ? 0 : 1);
			float num8 = num4 / (-2f * Mathf.Tan(camera.fieldOfView / 180f * 3.14159274f * 0.5f));
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._PixelsPerMeterAtOneMeter, num8);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ReflectionBlur, settings.reflection.reflectionBlur);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FadeDistance, settings.intensity.fadeDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._LayerThickness, settings.reflection.widthModifier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFade, settings.intensity.fresnelFade);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower);
			Matrix4x4 projectionMatrix = camera.projectionMatrix;
			Vector4 vector;
			vector..ctor(-2f / (num4 * projectionMatrix[0]), -2f / (num5 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			Vector3 vector2 = (!float.IsPositiveInfinity(camera.farClipPlane)) ? new Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane) : new Vector3(camera.nearClipPlane, -1f, 1f);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ReflectionBufferSize, new Vector2((float)num2, (float)num3));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ScreenSize, new Vector2(num4, num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._InvScreenSize, new Vector2(1f / num4, 1f / num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ProjInfo, vector);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._CameraClipInfo, vector2);
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetRow(0, new Vector4(num6, 0f, 0f, num6));
			matrix4x.SetRow(1, new Vector4(0f, num7, 0f, num7));
			matrix4x.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
			matrix4x.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			Matrix4x4 matrix4x2 = matrix4x * projectionMatrix;
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._ProjectToPixelMatrix, matrix4x2);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse);
			RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 0 : 2;
			int normalAndRoughnessTexture = ScreenSpaceReflectionComponent.Uniforms._NormalAndRoughnessTexture;
			int hitPointTexture = ScreenSpaceReflectionComponent.Uniforms._HitPointTexture;
			int blurTexture = ScreenSpaceReflectionComponent.Uniforms._BlurTexture;
			int filteredReflections = ScreenSpaceReflectionComponent.Uniforms._FilteredReflections;
			int finalReflectionTexture = ScreenSpaceReflectionComponent.Uniforms._FinalReflectionTexture;
			int tempTexture = ScreenSpaceReflectionComponent.Uniforms._TempTexture;
			cb.GetTemporaryRT(normalAndRoughnessTexture, -1, -1, 0, 0, 0, 1);
			cb.GetTemporaryRT(hitPointTexture, num2, num3, 0, 1, 2, 1);
			for (int i = 0; i < 5; i++)
			{
				cb.GetTemporaryRT(this.m_ReflectionTextures[i], num2 >> i, num3 >> i, 0, 1, renderTextureFormat);
			}
			cb.GetTemporaryRT(filteredReflections, num2, num3, 0, (!this.k_BilateralUpsample) ? 1 : 0, renderTextureFormat);
			cb.GetTemporaryRT(finalReflectionTexture, num2, num3, 0, 0, renderTextureFormat);
			cb.Blit(2, normalAndRoughnessTexture, material, 6);
			cb.Blit(2, hitPointTexture, material, 0);
			cb.Blit(2, filteredReflections, material, 5);
			cb.Blit(filteredReflections, this.m_ReflectionTextures[0], material, 8);
			for (int j = 1; j < 5; j++)
			{
				int num9 = this.m_ReflectionTextures[j - 1];
				int num10 = j;
				cb.GetTemporaryRT(blurTexture, num2 >> num10, num3 >> num10, 0, 1, renderTextureFormat);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(1f, 0f, 0f, 0f));
				cb.SetGlobalFloat(ScreenSpaceReflectionComponent.Uniforms._CurrentMipLevel, (float)j - 1f);
				cb.Blit(num9, blurTexture, material, 2);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(0f, 1f, 0f, 0f));
				num9 = this.m_ReflectionTextures[j];
				cb.Blit(blurTexture, num9, material, 2);
				cb.ReleaseTemporaryRT(blurTexture);
			}
			cb.Blit(this.m_ReflectionTextures[0], finalReflectionTexture, material, 3);
			cb.GetTemporaryRT(tempTexture, camera.pixelWidth, camera.pixelHeight, 0, 1, renderTextureFormat);
			cb.Blit(2, tempTexture, material, 1);
			cb.Blit(tempTexture, 2);
			cb.ReleaseTemporaryRT(tempTexture);
		}

		// Token: 0x04003349 RID: 13129
		public bool k_HighlightSuppression;

		// Token: 0x0400334A RID: 13130
		public bool k_TraceBehindObjects = true;

		// Token: 0x0400334B RID: 13131
		public bool k_TreatBackfaceHitAsMiss;

		// Token: 0x0400334C RID: 13132
		public bool k_BilateralUpsample = true;

		// Token: 0x0400334D RID: 13133
		public readonly int[] m_ReflectionTextures = new int[5];

		// Token: 0x0200127D RID: 4733
		public static class Uniforms
		{
			// Token: 0x04007FAE RID: 32686
			public static readonly int _RayStepSize = Shader.PropertyToID("_RayStepSize");

			// Token: 0x04007FAF RID: 32687
			public static readonly int _AdditiveReflection = Shader.PropertyToID("_AdditiveReflection");

			// Token: 0x04007FB0 RID: 32688
			public static readonly int _BilateralUpsampling = Shader.PropertyToID("_BilateralUpsampling");

			// Token: 0x04007FB1 RID: 32689
			public static readonly int _TreatBackfaceHitAsMiss = Shader.PropertyToID("_TreatBackfaceHitAsMiss");

			// Token: 0x04007FB2 RID: 32690
			public static readonly int _AllowBackwardsRays = Shader.PropertyToID("_AllowBackwardsRays");

			// Token: 0x04007FB3 RID: 32691
			public static readonly int _TraceBehindObjects = Shader.PropertyToID("_TraceBehindObjects");

			// Token: 0x04007FB4 RID: 32692
			public static readonly int _MaxSteps = Shader.PropertyToID("_MaxSteps");

			// Token: 0x04007FB5 RID: 32693
			public static readonly int _FullResolutionFiltering = Shader.PropertyToID("_FullResolutionFiltering");

			// Token: 0x04007FB6 RID: 32694
			public static readonly int _HalfResolution = Shader.PropertyToID("_HalfResolution");

			// Token: 0x04007FB7 RID: 32695
			public static readonly int _HighlightSuppression = Shader.PropertyToID("_HighlightSuppression");

			// Token: 0x04007FB8 RID: 32696
			public static readonly int _PixelsPerMeterAtOneMeter = Shader.PropertyToID("_PixelsPerMeterAtOneMeter");

			// Token: 0x04007FB9 RID: 32697
			public static readonly int _ScreenEdgeFading = Shader.PropertyToID("_ScreenEdgeFading");

			// Token: 0x04007FBA RID: 32698
			public static readonly int _ReflectionBlur = Shader.PropertyToID("_ReflectionBlur");

			// Token: 0x04007FBB RID: 32699
			public static readonly int _MaxRayTraceDistance = Shader.PropertyToID("_MaxRayTraceDistance");

			// Token: 0x04007FBC RID: 32700
			public static readonly int _FadeDistance = Shader.PropertyToID("_FadeDistance");

			// Token: 0x04007FBD RID: 32701
			public static readonly int _LayerThickness = Shader.PropertyToID("_LayerThickness");

			// Token: 0x04007FBE RID: 32702
			public static readonly int _SSRMultiplier = Shader.PropertyToID("_SSRMultiplier");

			// Token: 0x04007FBF RID: 32703
			public static readonly int _FresnelFade = Shader.PropertyToID("_FresnelFade");

			// Token: 0x04007FC0 RID: 32704
			public static readonly int _FresnelFadePower = Shader.PropertyToID("_FresnelFadePower");

			// Token: 0x04007FC1 RID: 32705
			public static readonly int _ReflectionBufferSize = Shader.PropertyToID("_ReflectionBufferSize");

			// Token: 0x04007FC2 RID: 32706
			public static readonly int _ScreenSize = Shader.PropertyToID("_ScreenSize");

			// Token: 0x04007FC3 RID: 32707
			public static readonly int _InvScreenSize = Shader.PropertyToID("_InvScreenSize");

			// Token: 0x04007FC4 RID: 32708
			public static readonly int _ProjInfo = Shader.PropertyToID("_ProjInfo");

			// Token: 0x04007FC5 RID: 32709
			public static readonly int _CameraClipInfo = Shader.PropertyToID("_CameraClipInfo");

			// Token: 0x04007FC6 RID: 32710
			public static readonly int _ProjectToPixelMatrix = Shader.PropertyToID("_ProjectToPixelMatrix");

			// Token: 0x04007FC7 RID: 32711
			public static readonly int _WorldToCameraMatrix = Shader.PropertyToID("_WorldToCameraMatrix");

			// Token: 0x04007FC8 RID: 32712
			public static readonly int _CameraToWorldMatrix = Shader.PropertyToID("_CameraToWorldMatrix");

			// Token: 0x04007FC9 RID: 32713
			public static readonly int _Axis = Shader.PropertyToID("_Axis");

			// Token: 0x04007FCA RID: 32714
			public static readonly int _CurrentMipLevel = Shader.PropertyToID("_CurrentMipLevel");

			// Token: 0x04007FCB RID: 32715
			public static readonly int _NormalAndRoughnessTexture = Shader.PropertyToID("_NormalAndRoughnessTexture");

			// Token: 0x04007FCC RID: 32716
			public static readonly int _HitPointTexture = Shader.PropertyToID("_HitPointTexture");

			// Token: 0x04007FCD RID: 32717
			public static readonly int _BlurTexture = Shader.PropertyToID("_BlurTexture");

			// Token: 0x04007FCE RID: 32718
			public static readonly int _FilteredReflections = Shader.PropertyToID("_FilteredReflections");

			// Token: 0x04007FCF RID: 32719
			public static readonly int _FinalReflectionTexture = Shader.PropertyToID("_FinalReflectionTexture");

			// Token: 0x04007FD0 RID: 32720
			public static readonly int _TempTexture = Shader.PropertyToID("_TempTexture");
		}

		// Token: 0x0200127E RID: 4734
		public enum PassIndex
		{
			// Token: 0x04007FD2 RID: 32722
			RayTraceStep,
			// Token: 0x04007FD3 RID: 32723
			CompositeFinal,
			// Token: 0x04007FD4 RID: 32724
			Blur,
			// Token: 0x04007FD5 RID: 32725
			CompositeSSR,
			// Token: 0x04007FD6 RID: 32726
			MinMipGeneration,
			// Token: 0x04007FD7 RID: 32727
			HitPointToReflections,
			// Token: 0x04007FD8 RID: 32728
			BilateralKeyPack,
			// Token: 0x04007FD9 RID: 32729
			BlitDepthAsCSZ,
			// Token: 0x04007FDA RID: 32730
			PoissonBlur
		}
	}
}
