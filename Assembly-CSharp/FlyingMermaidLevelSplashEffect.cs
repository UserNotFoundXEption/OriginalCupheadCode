using System;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class FlyingMermaidLevelSplashEffect : Effect
{
	// Token: 0x06001DCB RID: 7627 RVA: 0x000B1C7C File Offset: 0x000AFE7C
	public void LayerUp()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		int num = component.sortingOrder;
		string text = component.sortingLayerName;
		if (text == "Foreground" || (text == "Background" && num < 80))
		{
			num = num - num % 20 + 21;
		}
		else
		{
			text = "Foreground";
			num = 1;
		}
		component.sortingLayerName = text;
		component.sortingOrder = num;
	}
}
