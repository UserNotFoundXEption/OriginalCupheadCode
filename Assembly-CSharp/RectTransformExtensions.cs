using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public static class RectTransformExtensions
{
	// Token: 0x060004ED RID: 1261 RVA: 0x0006B4A0 File Offset: 0x000696A0
	public static RectTransform Copy(this RectTransform transform, RectTransform target)
	{
		transform.SetParent(target.parent);
		transform.position = target.position;
		transform.localScale = target.localScale;
		transform.rotation = target.rotation;
		transform.anchoredPosition3D = target.anchoredPosition3D;
		transform.anchorMax = target.anchorMax;
		transform.anchorMin = target.anchorMin;
		transform.offsetMax = target.offsetMax;
		transform.offsetMin = target.offsetMin;
		transform.pivot = target.pivot;
		transform.sizeDelta = target.sizeDelta;
		return transform;
	}
}
