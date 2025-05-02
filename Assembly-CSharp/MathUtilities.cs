using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public static class MathUtilities
{
	// Token: 0x0600061D RID: 1565 RVA: 0x0006EB10 File Offset: 0x0006CD10
	public static bool SameSign(float a, float b)
	{
		return (Mathf.Approximately(a, 0f) && Mathf.Approximately(b, 0f)) || (a > 0f && b > 0f) || (a < 0f && b < 0f);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0006EB70 File Offset: 0x0006CD70
	public static float LerpMapping(float value, float fromStart, float fromEnd, float toStart, float toEnd, bool clamp = false)
	{
		float num = (value - fromStart) / (fromEnd - fromStart);
		if (clamp)
		{
			num = Mathf.Max(0f, Mathf.Min(1f, num));
		}
		return toStart + (toEnd - toStart) * num;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0006EBAC File Offset: 0x0006CDAC
	public static float SqrDistanceToLine(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0006EBDC File Offset: 0x0006CDDC
	public static float DistanceToLine(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0000661E File Offset: 0x0000481E
	public static float DecimalPart(float value)
	{
		if (value < 0f)
		{
			return value - Mathf.Ceil(value);
		}
		return value - Mathf.Floor(value);
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0000663C File Offset: 0x0000483C
	public static int NextIndex(int currentIndex, int indexLength)
	{
		currentIndex++;
		if (currentIndex >= indexLength)
		{
			currentIndex = 0;
		}
		return currentIndex;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0000664E File Offset: 0x0000484E
	public static int PreviousIndex(int currentIndex, int indexLength)
	{
		currentIndex--;
		if (currentIndex < 0)
		{
			currentIndex = indexLength - 1;
		}
		return currentIndex;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0006EC0C File Offset: 0x0006CE0C
	public static bool LinesIntersect(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2, out Vector2 intersectionPoint)
	{
		float num = e1.y - s1.y;
		float num2 = s1.x - e1.x;
		float num3 = num * s1.x + num2 * s1.y;
		float num4 = e2.y - s2.y;
		float num5 = s2.x - e2.x;
		float num6 = num4 * s2.x + num5 * s2.y;
		float num7 = num * num5 - num4 * num2;
		if (Mathf.Approximately(num7, 0f))
		{
			intersectionPoint = Vector2.zero;
			return false;
		}
		float num8 = 1f / num7;
		intersectionPoint..ctor((num5 * num3 - num2 * num6) * num8, (num * num6 - num4 * num3) * num8);
		return true;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00006662 File Offset: 0x00004862
	public static Vector2 HadamardProduct(Vector2 v1, Vector2 v2)
	{
		return new Vector2(v1.x * v2.x, v1.y * v2.y);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00006687 File Offset: 0x00004887
	public static Vector3 HadamardProduct(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x000066BB File Offset: 0x000048BB
	public static bool BetweenInclusive(int value, int min, int max)
	{
		return value >= min && value <= max;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x000066CE File Offset: 0x000048CE
	public static bool BetweenInclusive(float value, float min, float max)
	{
		return value >= min && value <= max;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x000066E1 File Offset: 0x000048E1
	public static bool BetweenExclusive(float value, float min, float max)
	{
		return value > min && value < max;
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x000066F1 File Offset: 0x000048F1
	public static bool BetweenInclusiveExclusive(float value, float min, float max)
	{
		return value >= min && value < max;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00006701 File Offset: 0x00004901
	public static bool BetweenExclusiveInclusive(float value, float min, float max)
	{
		return value > min && value <= max;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00006714 File Offset: 0x00004914
	public static float ClampAngleSoft(float angle)
	{
		if (angle >= 6.28318548f)
		{
			angle -= 6.28318548f;
		}
		else if (angle < 0f)
		{
			angle += 6.28318548f;
		}
		return angle;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00006744 File Offset: 0x00004944
	public static float DirectionToAngle(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x) * 57.29578f;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0006ECD8 File Offset: 0x0006CED8
	public static Vector2 AngleToDirection(float angle)
	{
		float num = angle * 0.0174532924f;
		return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0006ED00 File Offset: 0x0006CF00
	public static Vector2 TrigonmetricVector(float t, float amplitude, float frequency, float phaseShift = 0f, float globalPhaseShift = 0f)
	{
		Vector2 result;
		result.x = amplitude * Mathf.Cos(frequency * (t + phaseShift) + globalPhaseShift);
		result.y = amplitude * Mathf.Sin(frequency * (t + phaseShift) + globalPhaseShift);
		return result;
	}

	// Token: 0x040004C5 RID: 1221
	public const float Sqrt2 = 1.41421354f;

	// Token: 0x040004C6 RID: 1222
	public const float InverseSqrt2 = 0.707106769f;

	// Token: 0x040004C7 RID: 1223
	public const float TwoPi = 6.28318548f;

	// Token: 0x040004C8 RID: 1224
	public const float HalfPi = 1.57079637f;
}
