using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C5 RID: 1221
public class MapEquipUICardChecklistIcon : AbstractMapCardIcon
{
	// Token: 0x060032B2 RID: 12978 RVA: 0x0002A134 File Offset: 0x00028334
	public void SetTextColor(Color color)
	{
		this.iconText.color = color;
	}

	// Token: 0x0400297B RID: 10619
	public Text iconText;
}
