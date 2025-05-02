using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public static class RectUtilities
{
	// Token: 0x06000638 RID: 1592 RVA: 0x000067F7 File Offset: 0x000049F7
	public static Rect AdjustSize(this Rect rect, float left, float right, float top, float bottom)
	{
		rect.xMin += left;
		rect.xMax += right;
		rect.yMin += top;
		rect.yMax += bottom;
		return rect;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0006EDC0 File Offset: 0x0006CFC0
	public static Rect SliceLeft(ref Rect rect, float amount)
	{
		Rect result;
		result..ctor(rect);
		result.xMax = result.xMin + amount;
		rect.xMin += amount;
		return result;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0006EDFC File Offset: 0x0006CFFC
	public static Rect SliceRight(ref Rect rect, float amount)
	{
		Rect result;
		result..ctor(rect);
		result.xMin = result.xMax - amount;
		rect.xMax -= amount;
		return result;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0006EE38 File Offset: 0x0006D038
	public static Rect SliceTop(ref Rect rect, float amount)
	{
		Rect result;
		result..ctor(rect);
		result.yMax = result.yMin + amount;
		rect.yMin += amount;
		return result;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0006EE74 File Offset: 0x0006D074
	public static Rect SliceBottom(ref Rect rect, float amount)
	{
		Rect result;
		result..ctor(rect);
		result.yMin = result.yMax - amount;
		rect.yMax -= amount;
		return result;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0006EEB0 File Offset: 0x0006D0B0
	public static Rect[] SplitVertical(Rect rect, int numberOfGeneratedRects)
	{
		float[] array = new float[numberOfGeneratedRects];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 1f;
		}
		return RectUtilities.SplitVertical(rect, array);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0006EEE8 File Offset: 0x0006D0E8
	public static Rect[] SplitVertical(Rect rect, params float[] weights)
	{
		float totalWeight = 0f;
		Array.ForEach<float>(weights, delegate(float weight)
		{
			totalWeight += weight;
		});
		Rect[] array = new Rect[weights.Length];
		float height = rect.height;
		for (int i = 0; i < weights.Length - 1; i++)
		{
			array[i] = RectUtilities.SliceTop(ref rect, Mathf.Floor(height * weights[i] / totalWeight));
		}
		array[array.Length - 1] = rect;
		return array;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0006EF78 File Offset: 0x0006D178
	public static Rect[] SplitHorizontal(Rect rect, int numberOfGeneratedRects)
	{
		float[] array = new float[numberOfGeneratedRects];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 1f;
		}
		return RectUtilities.SplitHorizontal(rect, array);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0006EFB0 File Offset: 0x0006D1B0
	public static Rect[] SplitHorizontal(Rect rect, params float[] weights)
	{
		float totalWeight = 0f;
		Array.ForEach<float>(weights, delegate(float weight)
		{
			totalWeight += weight;
		});
		Rect[] array = new Rect[weights.Length];
		float width = rect.width;
		for (int i = 0; i < weights.Length - 1; i++)
		{
			array[i] = RectUtilities.SliceLeft(ref rect, Mathf.Floor(width * weights[i] / totalWeight));
		}
		array[array.Length - 1] = rect;
		return array;
	}
}
