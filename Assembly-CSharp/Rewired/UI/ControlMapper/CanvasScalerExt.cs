using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000636 RID: 1590
	[AddComponentMenu("")]
	public class CanvasScalerExt : CanvasScaler
	{
		// Token: 0x0600412C RID: 16684 RVA: 0x00034424 File Offset: 0x00032624
		public void ForceRefresh()
		{
			this.Handle();
		}
	}
}
