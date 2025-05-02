using System;

// Token: 0x02000062 RID: 98
public static class StringExtensions
{
	// Token: 0x06000536 RID: 1334 RVA: 0x0006C15C File Offset: 0x0006A35C
	public static string UpperFirst(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		array[0] = char.ToUpper(array[0]);
		return new string(array);
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0006C194 File Offset: 0x0006A394
	public static string LowerFirst(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		array[0] = char.ToLower(array[0]);
		return new string(array);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0006C1CC File Offset: 0x0006A3CC
	public static string UppercaseWords(this string str)
	{
		char[] array = str.ToCharArray();
		if (array.Length >= 1 && char.IsLower(array[0]))
		{
			array[0] = char.ToUpper(array[0]);
		}
		for (int i = 1; i < array.Length; i++)
		{
			if ((array[i - 1] == ' ' || array[i - 1] == '_' || array[i - 1] == '/') && char.IsLower(array[i]))
			{
				array[i] = char.ToUpper(array[i]);
			}
		}
		return new string(array);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0006C258 File Offset: 0x0006A458
	public static string ToLowerIfNecessary(this string str)
	{
		if (str == null)
		{
			throw new NullReferenceException();
		}
		bool flag = false;
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			if (char.IsUpper(str[i]))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			return str.ToLower();
		}
		return str;
	}
}
