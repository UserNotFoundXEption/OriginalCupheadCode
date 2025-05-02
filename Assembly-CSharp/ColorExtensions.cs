using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public static class ColorExtensions
{
	// Token: 0x060004E7 RID: 1255 RVA: 0x00005774 File Offset: 0x00003974
	public static string ToHex(this Color color)
	{
		return ColorUtils.ColorToHex(color, false);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00005782 File Offset: 0x00003982
	public static string ToHex(this Color color, bool alpha)
	{
		return ColorUtils.ColorToHex(color, alpha);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0006B360 File Offset: 0x00069560
	public static string ToNiceString(this Color color)
	{
		return string.Concat(new object[]
		{
			"R:",
			color.r,
			" G:",
			color.g,
			" B:",
			color.b,
			" A:",
			color.a
		});
	}
}
