using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CF RID: 1743
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x000395F1 File Offset: 0x000377F1
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040039BF RID: 14783
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x040039C0 RID: 14784
		public float angle = 50f;

		// Token: 0x040039C1 RID: 14785
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
