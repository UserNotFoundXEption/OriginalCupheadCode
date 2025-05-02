using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public static class MathUtils
{
	// Token: 0x06000630 RID: 1584 RVA: 0x0000675F File Offset: 0x0000495F
	public static float GetPercentage(float min, float max, float t)
	{
		return (t - min) / (max - min);
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00006768 File Offset: 0x00004968
	public static int PlusOrMinus()
	{
		return (Random.value <= 0.5f) ? -1 : 1;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00006780 File Offset: 0x00004980
	public static float ExpRandom(float mean)
	{
		return -Mathf.Log(Random.Range(0f, 1f)) * mean;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00006799 File Offset: 0x00004999
	public static bool RandomBool()
	{
		return Random.value > 0.5f;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x000067A7 File Offset: 0x000049A7
	public static Vector2 RandomPointInUnitCircle()
	{
		return MathUtils.AngleToDirection(Random.Range(0f, 360f)) * Mathf.Sqrt(Random.Range(0f, 1f));
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x000067D6 File Offset: 0x000049D6
	public static float DirectionToAngle(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x) * 360f / 6.28318548f;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0006ED3C File Offset: 0x0006CF3C
	public static Vector2 AngleToDirection(float angle)
	{
		float num = angle * 3.14159274f * 2f / 360f;
		return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0006ED70 File Offset: 0x0006CF70
	public static bool CircleContains(Vector2 center, float radius, Vector2 point)
	{
		return Mathf.Pow(point.x - center.x, 2f) + Mathf.Pow(point.y - center.y, 2f) < Mathf.Pow(radius, 2f);
	}
}
