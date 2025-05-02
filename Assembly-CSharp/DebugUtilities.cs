using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public static class DebugUtilities
{
	// Token: 0x060005D6 RID: 1494 RVA: 0x0000625B File Offset: 0x0000445B
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		DebugUtilities.DrawLine(start, end, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0000626E File Offset: 0x0000446E
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
	{
		DebugUtilities.DebugDrawer.DrawLine(start, end, color, duration);
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x00006279 File Offset: 0x00004479
	public static void DrawRay(Vector3 origin, Vector3 direction)
	{
		DebugUtilities.DrawRay(origin, direction, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0000628C File Offset: 0x0000448C
	public static void DrawRay(Vector3 origin, Vector3 direction, Color color, float duration = 0f)
	{
		DebugUtilities.DebugDrawer.DrawLine(origin, origin + direction, color, duration);
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0000629D File Offset: 0x0000449D
	public static void DrawBox2D(Vector2 origin, Vector2 size, float angle)
	{
		DebugUtilities.DrawBox2D(origin, size, angle, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0006DC24 File Offset: 0x0006BE24
	public static void DrawBox2D(Vector2 origin, Vector2 size, float angle, Color color, float duration = 0f)
	{
		Vector2 vector = size * 0.5f;
		Vector2 vector2 = origin + new Vector2(-vector.x, vector.y);
		Vector2 vector3 = origin + new Vector2(-vector.x, -vector.y);
		Vector2 vector4 = origin + new Vector2(vector.x, vector.y);
		Vector2 vector5 = origin + new Vector2(vector.x, -vector.y);
		if (!Mathf.Approximately(angle, 0f))
		{
			throw new Exception("Not supported in this library");
		}
		DebugUtilities.DebugDrawer.DrawLine(vector2, vector4, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(vector4, vector5, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(vector5, vector3, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(vector3, vector2, color, duration);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000062B1 File Offset: 0x000044B1
	public static void DrawVerticalPole(Vector3 center, float height)
	{
		DebugUtilities.DrawVerticalPole(center, height, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x000062C4 File Offset: 0x000044C4
	public static void DrawVerticalPole(Vector3 center, float height, Color color, float duration = 0f)
	{
		DebugUtilities.DrawLine(center + Vector3.up * height, center - Vector3.up * height, color, duration);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000062EF File Offset: 0x000044EF
	public static void DrawHorizontalPole(Vector3 center, float width)
	{
		DebugUtilities.DrawHorizontalPole(center, width, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00006302 File Offset: 0x00004502
	public static void DrawHorizontalPole(Vector3 center, float width, Color color, float duration = 0f)
	{
		DebugUtilities.DrawLine(center + Vector3.right * width, center - Vector3.right * width, color, duration);
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0000632D File Offset: 0x0000452D
	public static void DrawCircle2D(Vector3 position, float radius)
	{
		DebugUtilities.DrawCircle2D(position, radius, DebugUtilities.DefaultColor, 0f, true);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00006341 File Offset: 0x00004541
	public static void DrawCircle2D(Vector3 position, float radius, Color color, float duration = 0f, bool depthTest = true)
	{
		DebugUtilities.DrawCircle(position, Vector3.forward, Vector3.up, radius, 20, color, duration, depthTest);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0000635A File Offset: 0x0000455A
	public static void DrawCircle(Vector3 position, Vector3 forward, Vector3 up, float radius, int segments = 20)
	{
		DebugUtilities.DrawCircle(position, forward, up, radius, segments, DebugUtilities.DefaultColor, 0f, true);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0006DD18 File Offset: 0x0006BF18
	public static void DrawCircle(Vector3 position, Vector3 forward, Vector3 up, float radius, int segments, Color color, float duration = 0f, bool depthTest = true)
	{
		DebugUtilities.DrawEllipse(position, forward, up, radius, radius, segments, color, duration, depthTest);
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0006DD38 File Offset: 0x0006BF38
	public static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0f, bool depthTest = true)
	{
		float num = 0f;
		Quaternion quaternion = Quaternion.LookRotation(forward, up);
		Vector3 vector = Vector3.zero;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < segments + 1; i++)
		{
			zero.x = Mathf.Sin(0.0174532924f * num) * radiusX;
			zero.y = Mathf.Cos(0.0174532924f * num) * radiusY;
			if (i > 0)
			{
				DebugUtilities.DebugDrawer.DrawLine(quaternion * vector + pos, quaternion * zero + pos, color, duration);
			}
			vector = zero;
			num += 360f / (float)segments;
		}
	}

	// Token: 0x040004C2 RID: 1218
	public const int DefaultEllipseSegments = 20;

	// Token: 0x040004C3 RID: 1219
	public static readonly Color DefaultColor = Color.white;

	// Token: 0x020008BB RID: 2235
	public static class DebugDrawer
	{
		// Token: 0x06005256 RID: 21078 RVA: 0x0003EFCA File Offset: 0x0003D1CA
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
			Debug.DrawLine(start, end, color, duration, false);
		}
	}
}
