using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
public static class ListExtensions
{
	// Token: 0x060004EA RID: 1258 RVA: 0x0006B3D4 File Offset: 0x000695D4
	public static void Move<T>(this List<T> list, int index, int direction)
	{
		if (direction < 0)
		{
			if (index == 0)
			{
				return;
			}
			T value = list[index - 1];
			list[index - 1] = list[index];
			list[index] = value;
		}
		else if (direction > 0)
		{
			if (index >= list.Count - 1)
			{
				return;
			}
			T value2 = list[index + 1];
			list[index + 1] = list[index];
			list[index] = value2;
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0006B450 File Offset: 0x00069650
	public static void Shuffle<T>(this IList<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			int index = Random.Range(i, list.Count);
			T value = list[i];
			list[i] = list[index];
			list[index] = value;
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00005790 File Offset: 0x00003990
	public static T RandomChoice<T>(this IList<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}
}
