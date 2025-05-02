using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public static class KinematicUtilities
{
	// Token: 0x06000614 RID: 1556 RVA: 0x0000658D File Offset: 0x0000478D
	public static float CalculateAcceleration(float distance, float finalSpeed)
	{
		return 0.5f * finalSpeed * finalSpeed / distance;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0000659A File Offset: 0x0000479A
	public static float CalculateTimeToSpeed(float distance, float finalSpeed)
	{
		return 2f * distance / finalSpeed;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x000065A5 File Offset: 0x000047A5
	public static float CalculateTimeToTravelDistance(float distance, float speed)
	{
		return distance / speed;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x000065AA File Offset: 0x000047AA
	public static float CalculateVelocityFromZero(float distance, float time)
	{
		return 2f * distance / time;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0006EAF4 File Offset: 0x0006CCF4
	public static float CalculateAccelerationFromZero(float distance, float time)
	{
		float num = time * time;
		return 2f * distance / num;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x000065B5 File Offset: 0x000047B5
	public static float CalculateTimeToChangeVelocity(float v1, float v2, float distance)
	{
		return 2f * distance / (v1 + v2);
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x000065C2 File Offset: 0x000047C2
	public static float CalculateInitialSpeedToReachApex(float apexHeight, float gravity)
	{
		return Mathf.Sqrt(2f * gravity * apexHeight);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x000065D2 File Offset: 0x000047D2
	public static float CalculateDistanceTravelled(Vector2 initialVelocity, float startingHeight, float gravity)
	{
		return initialVelocity.x / gravity * (initialVelocity.y + Mathf.Sqrt(initialVelocity.y * initialVelocity.y + 2f * gravity * startingHeight));
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00006604 File Offset: 0x00004804
	public static float CalculateHorizontalSpeedToTravelDistance(float distance, float velocityY, float startingHeight, float gravity)
	{
		return distance * gravity / (velocityY + Mathf.Sqrt(velocityY * velocityY + 2f * gravity * startingHeight));
	}
}
