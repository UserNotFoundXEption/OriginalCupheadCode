using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public static class BoundsUtilities
{
	// Token: 0x060005D0 RID: 1488 RVA: 0x0006D8E0 File Offset: 0x0006BAE0
	public static Bounds CalculateBounds(Vector2 size, Vector2 offset, Transform transform)
	{
		Vector2 vector = size * 0.5f;
		Vector3 vector2 = new Vector2(-vector.x, vector.y) + offset;
		Vector3 vector3 = new Vector2(vector.x, vector.y) + offset;
		Vector3 vector4 = new Vector2(-vector.x, -vector.y) + offset;
		Vector3 vector5 = new Vector2(vector.x, -vector.y) + offset;
		vector2 = transform.TransformPoint(vector2);
		vector3 = transform.TransformPoint(vector3);
		vector4 = transform.TransformPoint(vector4);
		vector5 = transform.TransformPoint(vector5);
		float num = Mathf.Min(Mathf.Min(Mathf.Min(vector2.x, vector3.x), vector4.x), vector5.x);
		float num2 = Mathf.Min(Mathf.Min(Mathf.Min(vector2.y, vector3.y), vector4.y), vector5.y);
		float num3 = Mathf.Max(Mathf.Max(Mathf.Max(vector2.x, vector3.x), vector4.x), vector5.x);
		float num4 = Mathf.Max(Mathf.Max(Mathf.Max(vector2.y, vector3.y), vector4.y), vector5.y);
		float num5 = Mathf.Min(Mathf.Min(Mathf.Min(vector2.z, vector3.z), vector4.z), vector5.z);
		float num6 = Mathf.Max(Mathf.Max(Mathf.Max(vector2.z, vector3.z), vector4.z), vector5.z);
		Bounds result = default(Bounds);
		result.SetMinMax(new Vector3(num, num2), new Vector3(num3, num4));
		return result;
	}
}
