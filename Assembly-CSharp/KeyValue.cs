using System;
using System.Collections.Generic;

// Token: 0x0200006B RID: 107
[Serializable]
public class KeyValue
{
	// Token: 0x06000581 RID: 1409 RVA: 0x00005D7F File Offset: 0x00003F7F
	public KeyValue()
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00005D92 File Offset: 0x00003F92
	public KeyValue(string key, float value)
	{
		this.key = key;
		this.value = value;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0006CCC0 File Offset: 0x0006AEC0
	public static KeyValue[] ListFromString(string keyValueString, char[] allowedCharacters)
	{
		List<KeyValue> list = new List<KeyValue>();
		List<char> list2 = new List<char>(allowedCharacters);
		list2.Add(',');
		list2.Add(':');
		keyValueString.Replace(" ", string.Empty);
		for (int i = 0; i < keyValueString.Length; i++)
		{
			bool flag = true;
			foreach (char c in list2)
			{
				if (keyValueString[i] == c)
				{
					flag = false;
				}
			}
			if (flag)
			{
				keyValueString.Remove(i, 1);
			}
		}
		string[] array = keyValueString.Split(new char[]
		{
			','
		});
		for (int j = 0; j < array.Length; j++)
		{
			string[] array2 = array[j].Split(new char[]
			{
				':'
			});
			if (array2.Length == 2)
			{
				string text = array2[0].Replace(" ", string.Empty);
				float num = 0f;
				bool flag2 = Parser.FloatTryParse(array2[1], out num);
				if (flag2 && text != null && !(text == string.Empty))
				{
					list.Add(new KeyValue(text, num));
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00005DB3 File Offset: 0x00003FB3
	public KeyValue Clone()
	{
		return new KeyValue(this.key, this.value);
	}

	// Token: 0x040004A5 RID: 1189
	public const char PAIR_SEPARATOR = ',';

	// Token: 0x040004A6 RID: 1190
	public const char VALUE_SEPARATOR = ':';

	// Token: 0x040004A7 RID: 1191
	public string key = string.Empty;

	// Token: 0x040004A8 RID: 1192
	public float value;
}
