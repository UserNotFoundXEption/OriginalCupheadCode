using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200062D RID: 1581
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x0400338B RID: 13195
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x0400338C RID: 13196
		public FogModel fog = new FogModel();

		// Token: 0x0400338D RID: 13197
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x0400338E RID: 13198
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x0400338F RID: 13199
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04003390 RID: 13200
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04003391 RID: 13201
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04003392 RID: 13202
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04003393 RID: 13203
		public BloomModel bloom = new BloomModel();

		// Token: 0x04003394 RID: 13204
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04003395 RID: 13205
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04003396 RID: 13206
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04003397 RID: 13207
		public GrainModel grain = new GrainModel();

		// Token: 0x04003398 RID: 13208
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04003399 RID: 13209
		public DitheringModel dithering = new DitheringModel();
	}
}
