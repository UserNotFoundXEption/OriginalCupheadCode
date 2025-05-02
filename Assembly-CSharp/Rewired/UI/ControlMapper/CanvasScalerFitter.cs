using System;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000637 RID: 1591
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitter : MonoBehaviour
	{
		// Token: 0x0600412E RID: 16686 RVA: 0x00034434 File Offset: 0x00032634
		public void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
			this.Update();
			this.canvasScaler.ForceRefresh();
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x00034453 File Offset: 0x00032653
		public void Update()
		{
			if (Screen.width != this.screenWidth || Screen.height != this.screenHeight)
			{
				this.screenWidth = Screen.width;
				this.screenHeight = Screen.height;
				this.UpdateSize();
			}
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00130660 File Offset: 0x0012E860
		public void UpdateSize()
		{
			if (this.canvasScaler.uiScaleMode != 1)
			{
				return;
			}
			if (this.breakPoints == null)
			{
				return;
			}
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = float.PositiveInfinity;
			int num3 = 0;
			for (int i = 0; i < this.breakPoints.Length; i++)
			{
				float num4 = Mathf.Abs(num - this.breakPoints[i].screenAspectRatio);
				if (num4 <= this.breakPoints[i].screenAspectRatio || MathTools.IsNear(this.breakPoints[i].screenAspectRatio, 0.01f))
				{
					if (num4 < num2)
					{
						num2 = num4;
						num3 = i;
					}
				}
			}
			this.canvasScaler.referenceResolution = this.breakPoints[num3].referenceResolution;
		}

		// Token: 0x040033CC RID: 13260
		[SerializeField]
		public CanvasScalerFitter.BreakPoint[] breakPoints;

		// Token: 0x040033CD RID: 13261
		public CanvasScalerExt canvasScaler;

		// Token: 0x040033CE RID: 13262
		public int screenWidth;

		// Token: 0x040033CF RID: 13263
		public int screenHeight;

		// Token: 0x040033D0 RID: 13264
		public Action ScreenSizeChanged;

		// Token: 0x020012B2 RID: 4786
		[Serializable]
		public class BreakPoint
		{
			// Token: 0x040080B1 RID: 32945
			[SerializeField]
			public string name;

			// Token: 0x040080B2 RID: 32946
			[SerializeField]
			public float screenAspectRatio;

			// Token: 0x040080B3 RID: 32947
			[SerializeField]
			public Vector2 referenceResolution;
		}
	}
}
