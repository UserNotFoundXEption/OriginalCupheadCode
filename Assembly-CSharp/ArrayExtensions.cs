using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public static class ArrayExtensions
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x00005756 File Offset: 0x00003956
	public static T GetRandom<T>(this T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00005767 File Offset: 0x00003967
	public static T GetLast<T>(this T[] array)
	{
		return array[array.Length - 1];
	}
}
