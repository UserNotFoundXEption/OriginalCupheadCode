using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public static class Vector2Extensions
{
	// Token: 0x06000551 RID: 1361 RVA: 0x0006C6FC File Offset: 0x0006A8FC
	public static Vector2 Set(this Vector2 v, float? x = null, float? y = null)
	{
		Vector2 result = v;
		if (x != null)
		{
			result.x = x.Value;
		}
		if (y != null)
		{
			result.y = y.Value;
		}
		return result;
	}
}
