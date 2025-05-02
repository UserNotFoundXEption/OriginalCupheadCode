using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public static class EnumUtils
{
	// Token: 0x0600060C RID: 1548 RVA: 0x00006547 File Offset: 0x00004747
	public static T[] GetValues<T>()
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("T must be an enum type");
		}
		return (T[])Enum.GetValues(typeof(T));
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0006E8E0 File Offset: 0x0006CAE0
	public static string[] GetValuesAsStrings<T>()
	{
		T[] values = EnumUtils.GetValues<T>();
		string[] array = new string[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = values[i].ToString();
		}
		return array;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0000657C File Offset: 0x0000477C
	public static int GetCount<T>()
	{
		return EnumUtils.GetValues<T>().Length;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0006E928 File Offset: 0x0006CB28
	public static T Random<T>()
	{
		T[] values = EnumUtils.GetValues<T>();
		return values[UnityEngine.Random.Range(0, values.Length)];
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0006E94C File Offset: 0x0006CB4C
	public static T Parse<T>(string name)
	{
		T[] values = EnumUtils.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			if (name == values[i].ToString())
			{
				return values[i];
			}
		}
		return values[0];
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0006E9A4 File Offset: 0x0006CBA4
	public static bool TryParse<T>(string name, out T result)
	{
		T[] values = EnumUtils.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			if (name == values[i].ToString())
			{
				result = values[i];
				return true;
			}
		}
		result = values[0];
		return false;
	}
}
