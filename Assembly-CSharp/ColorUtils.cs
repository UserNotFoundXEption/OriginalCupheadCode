using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class ColorUtils
{
	// Token: 0x060005D2 RID: 1490 RVA: 0x00006252 File Offset: 0x00004452
	public static Color GetAverageColor(Color[] colors)
	{
		return ColorUtils.GetAverageColor(colors, 1);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0006DAD0 File Offset: 0x0006BCD0
	public static Color GetAverageColor(Color[] colors, int quality)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < colors.Length; i += quality)
		{
			if (i >= colors.Length)
			{
				break;
			}
			num2 += colors[i].r;
			num3 += colors[i].g;
			num4 += colors[i].b;
			num++;
		}
		num2 /= (float)num;
		num3 /= (float)num;
		num4 /= (float)num;
		return new Color(num2, num3, num4);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0006DB64 File Offset: 0x0006BD64
	public static string ColorToHex(Color32 color, bool alpha = false)
	{
		string text = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		if (alpha)
		{
			text += color.a.ToString("X2");
		}
		return text;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0006DBCC File Offset: 0x0006BDCC
	public static Color HexToColor(string hex)
	{
		byte b = Parser.ByteParse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte b2 = Parser.ByteParse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b3 = Parser.ByteParse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(b, b2, b3, byte.MaxValue);
	}
}
