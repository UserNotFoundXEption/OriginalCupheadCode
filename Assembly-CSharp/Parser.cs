using System;
using System.Globalization;

// Token: 0x020005B9 RID: 1465
public static class Parser
{
	// Token: 0x06003D73 RID: 15731 RVA: 0x00031900 File Offset: 0x0002FB00
	public static string ToStringInvariant(this int value)
	{
		return value.ToString(Parser.InvariantInfo);
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x0003190E File Offset: 0x0002FB0E
	public static string ToStringInvariant(this float value)
	{
		return value.ToString(Parser.InvariantInfo);
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x0003191C File Offset: 0x0002FB1C
	public static int IntParse(string s)
	{
		return int.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x00031929 File Offset: 0x0002FB29
	public static bool IntTryParse(string s, out int result)
	{
		return int.TryParse(s, NumberStyles.Integer, Parser.InvariantInfo, out result);
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x00031938 File Offset: 0x0002FB38
	public static float FloatParse(string s)
	{
		return float.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x00031945 File Offset: 0x0002FB45
	public static bool FloatTryParse(string s, out float result)
	{
		return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, Parser.InvariantInfo, out result);
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x00031958 File Offset: 0x0002FB58
	public static byte ByteParse(string s)
	{
		return byte.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00031965 File Offset: 0x0002FB65
	public static byte ByteParse(string s, NumberStyles style)
	{
		return byte.Parse(s, style, Parser.InvariantInfo);
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x00031973 File Offset: 0x0002FB73
	public static bool ByteTryParse(string s, out byte result)
	{
		return byte.TryParse(s, NumberStyles.Integer, Parser.InvariantInfo, out result);
	}

	// Token: 0x040030E7 RID: 12519
	public static NumberFormatInfo InvariantInfo = CultureInfo.InvariantCulture.NumberFormat;
}
