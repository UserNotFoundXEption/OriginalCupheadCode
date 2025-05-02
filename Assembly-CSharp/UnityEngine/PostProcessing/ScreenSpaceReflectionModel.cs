using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000623 RID: 1571
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x00033E9E File Offset: 0x0003209E
		// (set) Token: 0x060040B0 RID: 16560 RVA: 0x00033EA6 File Offset: 0x000320A6
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x00033EAF File Offset: 0x000320AF
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x04003363 RID: 13155
		[SerializeField]
		public ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x020012A5 RID: 4773
		public enum SSRResolution
		{
			// Token: 0x04008081 RID: 32897
			High,
			// Token: 0x04008082 RID: 32898
			Low = 2
		}

		// Token: 0x020012A6 RID: 4774
		public enum SSRReflectionBlendType
		{
			// Token: 0x04008084 RID: 32900
			PhysicallyBased,
			// Token: 0x04008085 RID: 32901
			Additive
		}

		// Token: 0x020012A7 RID: 4775
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04008086 RID: 32902
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04008087 RID: 32903
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04008088 RID: 32904
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04008089 RID: 32905
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x020012A8 RID: 4776
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x0400808A RID: 32906
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x0400808B RID: 32907
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x0400808C RID: 32908
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x0400808D RID: 32909
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x0400808E RID: 32910
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x0400808F RID: 32911
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x04008090 RID: 32912
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04008091 RID: 32913
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x020012A9 RID: 4777
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04008092 RID: 32914
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x020012AA RID: 4778
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001924 RID: 6436
			// (get) Token: 0x060081E9 RID: 33257 RVA: 0x0029C4B4 File Offset: 0x0029A6B4
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04008093 RID: 32915
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04008094 RID: 32916
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04008095 RID: 32917
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
