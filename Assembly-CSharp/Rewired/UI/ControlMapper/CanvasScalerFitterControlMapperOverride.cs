using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000638 RID: 1592
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitterControlMapperOverride : MonoBehaviour
	{
		// Token: 0x06004132 RID: 16690 RVA: 0x000344AE File Offset: 0x000326AE
		public void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x000344BC File Offset: 0x000326BC
		public void LateUpdate()
		{
			this.canvasScaler.referenceResolution = this.targetResolution;
		}

		// Token: 0x040033D1 RID: 13265
		[SerializeField]
		public Vector2 targetResolution = new Vector2(1885f, 600f);

		// Token: 0x040033D2 RID: 13266
		public CanvasScalerExt canvasScaler;
	}
}
