using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000642 RID: 1602
	[AddComponentMenu("")]
	public class InputFieldInfo : UIElementInfo
	{
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x000358FA File Offset: 0x00033AFA
		// (set) Token: 0x060042F6 RID: 17142 RVA: 0x00035902 File Offset: 0x00033B02
		public int actionId { get; set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060042F7 RID: 17143 RVA: 0x0003590B File Offset: 0x00033B0B
		// (set) Token: 0x060042F8 RID: 17144 RVA: 0x00035913 File Offset: 0x00033B13
		public AxisRange axisRange { get; set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060042F9 RID: 17145 RVA: 0x0003591C File Offset: 0x00033B1C
		// (set) Token: 0x060042FA RID: 17146 RVA: 0x00035924 File Offset: 0x00033B24
		public int actionElementMapId { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x0003592D File Offset: 0x00033B2D
		// (set) Token: 0x060042FC RID: 17148 RVA: 0x00035935 File Offset: 0x00033B35
		public ControllerType controllerType { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x0003593E File Offset: 0x00033B3E
		// (set) Token: 0x060042FE RID: 17150 RVA: 0x00035946 File Offset: 0x00033B46
		public int controllerId { get; set; }
	}
}
