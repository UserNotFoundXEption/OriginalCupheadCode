using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006D1 RID: 1745
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x06004846 RID: 18502 RVA: 0x0003964F File Offset: 0x0003784F
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040039D0 RID: 14800
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x040039D1 RID: 14801
		public float angle = 50f;

		// Token: 0x040039D2 RID: 14802
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
