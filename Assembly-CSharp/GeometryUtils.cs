using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class GeometryUtils
{
	// Token: 0x06000613 RID: 1555 RVA: 0x0006EA08 File Offset: 0x0006CC08
	public static Vector3[] GetCircle(Vector3 center, float radius, GeometryUtils.Axis axis = GeometryUtils.Axis.Y, int resolution = 128)
	{
		Vector3[] array = new Vector3[resolution];
		float num = 6.28318548f / (float)resolution;
		for (int i = 0; i < resolution; i++)
		{
			float num2 = num * (float)i;
			float num3 = radius * Mathf.Cos(num2);
			float num4 = radius * Mathf.Sin(num2);
			array[i] = new Vector3(num3, num4, 0f);
		}
		Quaternion quaternion;
		if (axis == GeometryUtils.Axis.X)
		{
			quaternion = Quaternion.AngleAxis(90f, Vector3.up);
		}
		else if (axis == GeometryUtils.Axis.Y)
		{
			quaternion = Quaternion.AngleAxis(90f, Vector3.right);
		}
		else
		{
			quaternion = Quaternion.AngleAxis(0f, Vector3.up);
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = quaternion * array[j] + center;
		}
		return array;
	}

	// Token: 0x020008BD RID: 2237
	public enum Axis
	{
		// Token: 0x040042E7 RID: 17127
		X,
		// Token: 0x040042E8 RID: 17128
		Y,
		// Token: 0x040042E9 RID: 17129
		Z
	}
}
