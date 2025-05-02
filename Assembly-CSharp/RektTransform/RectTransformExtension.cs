using System;
using System.Diagnostics;
using UnityEngine;

namespace RektTransform
{
	// Token: 0x02000061 RID: 97
	public static class RectTransformExtension
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x00005839 File Offset: 0x00003A39
		[Conditional("REKT_LOG_ACTIVE")]
		public static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00005841 File Offset: 0x00003A41
		public static void DebugOutput(this RectTransform RT)
		{
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0006B7B4 File Offset: 0x000699B4
		public static Rect GetWorldRect(this RectTransform RT)
		{
			Vector3[] array = new Vector3[4];
			RT.GetWorldCorners(array);
			Vector2 vector;
			vector..ctor(array[2].x - array[1].x, array[1].y - array[0].y);
			return new Rect(new Vector2(array[1].x, -array[1].y), vector);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00005843 File Offset: 0x00003A43
		public static MinMax GetAnchors(this RectTransform RT)
		{
			return new MinMax(RT.anchorMin, RT.anchorMax);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00005856 File Offset: 0x00003A56
		public static void SetAnchors(this RectTransform RT, MinMax anchors)
		{
			RT.anchorMin = anchors.min;
			RT.anchorMax = anchors.max;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00005872 File Offset: 0x00003A72
		public static RectTransform GetParent(this RectTransform RT)
		{
			return RT.parent as RectTransform;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0006B82C File Offset: 0x00069A2C
		public static float GetWidth(this RectTransform RT)
		{
			return RT.rect.width;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0006B848 File Offset: 0x00069A48
		public static float GetHeight(this RectTransform RT)
		{
			return RT.rect.height;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000587F File Offset: 0x00003A7F
		public static Vector2 GetSize(this RectTransform RT)
		{
			return new Vector2(RT.GetWidth(), RT.GetHeight());
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00005892 File Offset: 0x00003A92
		public static void SetWidth(this RectTransform RT, float width)
		{
			RT.SetSizeWithCurrentAnchors(0, width);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000589C File Offset: 0x00003A9C
		public static void SetHeight(this RectTransform RT, float height)
		{
			RT.SetSizeWithCurrentAnchors(1, height);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000058A6 File Offset: 0x00003AA6
		public static void SetSize(this RectTransform RT, float width, float height)
		{
			RT.SetSizeWithCurrentAnchors(0, width);
			RT.SetSizeWithCurrentAnchors(1, height);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000058B8 File Offset: 0x00003AB8
		public static void SetSize(this RectTransform RT, Vector2 size)
		{
			RT.SetSizeWithCurrentAnchors(0, size.x);
			RT.SetSizeWithCurrentAnchors(1, size.y);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0006B864 File Offset: 0x00069A64
		public static Vector2 GetLeft(this RectTransform RT)
		{
			return new Vector2(RT.offsetMin.x, RT.anchoredPosition.y);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0006B894 File Offset: 0x00069A94
		public static Vector2 GetRight(this RectTransform RT)
		{
			return new Vector2(RT.offsetMax.x, RT.anchoredPosition.y);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0006B8C4 File Offset: 0x00069AC4
		public static Vector2 GetTop(this RectTransform RT)
		{
			return new Vector2(RT.anchoredPosition.x, RT.offsetMax.y);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0006B8F4 File Offset: 0x00069AF4
		public static Vector2 GetBottom(this RectTransform RT)
		{
			return new Vector2(RT.anchoredPosition.x, RT.offsetMin.y);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0006B924 File Offset: 0x00069B24
		public static void SetLeft(this RectTransform RT, float left)
		{
			float xMin = RT.GetParent().rect.xMin;
			float num = RT.anchorMin.x * 2f - 1f;
			RT.offsetMin = new Vector2(xMin + xMin * num + left, RT.offsetMin.y);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0006B984 File Offset: 0x00069B84
		public static void SetRight(this RectTransform RT, float right)
		{
			float xMax = RT.GetParent().rect.xMax;
			float num = RT.anchorMax.x * 2f - 1f;
			RT.offsetMax = new Vector2(xMax - xMax * num + right, RT.offsetMax.y);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0006B9E4 File Offset: 0x00069BE4
		public static void SetTop(this RectTransform RT, float top)
		{
			float yMax = RT.GetParent().rect.yMax;
			float num = RT.anchorMax.y * 2f - 1f;
			RT.offsetMax = new Vector2(RT.offsetMax.x, yMax - yMax * num + top);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0006BA44 File Offset: 0x00069C44
		public static void SetBottom(this RectTransform RT, float bottom)
		{
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMin.y * 2f - 1f;
			RT.offsetMin = new Vector2(RT.offsetMin.x, yMin + yMin * num + bottom);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000058D6 File Offset: 0x00003AD6
		public static void Left(this RectTransform RT, float left)
		{
			RT.SetLeft(left);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000058DF File Offset: 0x00003ADF
		public static void Right(this RectTransform RT, float right)
		{
			RT.SetRight(-right);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000058E9 File Offset: 0x00003AE9
		public static void Top(this RectTransform RT, float top)
		{
			RT.SetTop(-top);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000058F3 File Offset: 0x00003AF3
		public static void Bottom(this RectTransform RT, float bottom)
		{
			RT.SetRight(bottom);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0006BAA4 File Offset: 0x00069CA4
		public static void SetLeftFrom(this RectTransform RT, MinMax anchor, float left)
		{
			RT.offsetMin = new Vector2(RT.AnchorToParentSpace(anchor.min - RT.anchorMin).x + left, RT.offsetMin.y);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0006BAEC File Offset: 0x00069CEC
		public static void SetRightFrom(this RectTransform RT, MinMax anchor, float right)
		{
			RT.offsetMax = new Vector2(RT.AnchorToParentSpace(anchor.max - RT.anchorMax).x + right, RT.offsetMax.y);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0006BB34 File Offset: 0x00069D34
		public static void SetTopFrom(this RectTransform RT, MinMax anchor, float top)
		{
			Vector2 vector = RT.AnchorToParentSpace(anchor.max - RT.anchorMax);
			RT.offsetMax = new Vector2(RT.offsetMax.x, vector.y + top);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0006BB7C File Offset: 0x00069D7C
		public static void SetBottomFrom(this RectTransform RT, MinMax anchor, float bottom)
		{
			Vector2 vector = RT.AnchorToParentSpace(anchor.min - RT.anchorMin);
			RT.offsetMin = new Vector2(RT.offsetMin.x, vector.y + bottom);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0006BBC4 File Offset: 0x00069DC4
		public static void SetRelativeLeft(this RectTransform RT, float left)
		{
			RT.offsetMin = new Vector2(RT.anchoredPosition.x + left, RT.offsetMin.y);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0006BBFC File Offset: 0x00069DFC
		public static void SetRelativeRight(this RectTransform RT, float right)
		{
			RT.offsetMax = new Vector2(RT.anchoredPosition.x + right, RT.offsetMax.y);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0006BC34 File Offset: 0x00069E34
		public static void SetRelativeTop(this RectTransform RT, float top)
		{
			RT.offsetMax = new Vector2(RT.offsetMax.x, RT.anchoredPosition.y + top);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0006BC6C File Offset: 0x00069E6C
		public static void SetRelativeBottom(this RectTransform RT, float bottom)
		{
			RT.offsetMin = new Vector2(RT.offsetMin.x, RT.anchoredPosition.y + bottom);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0006BCA4 File Offset: 0x00069EA4
		public static void MoveLeft(this RectTransform RT, float left = 0f)
		{
			float xMin = RT.GetParent().rect.xMin;
			float num = RT.anchorMax.x - RT.anchorMin.x;
			float num2 = RT.anchorMax.x * 2f - 1f;
			RT.anchoredPosition = new Vector2(xMin + xMin * num2 + left - num * xMin, RT.anchoredPosition.y);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0006BD28 File Offset: 0x00069F28
		public static void MoveRight(this RectTransform RT, float right = 0f)
		{
			float xMax = RT.GetParent().rect.xMax;
			float num = RT.anchorMax.x - RT.anchorMin.x;
			float num2 = RT.anchorMax.x * 2f - 1f;
			RT.anchoredPosition = new Vector2(xMax - xMax * num2 - right + num * xMax, RT.anchoredPosition.y);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0006BDAC File Offset: 0x00069FAC
		public static void MoveTop(this RectTransform RT, float top = 0f)
		{
			float yMax = RT.GetParent().rect.yMax;
			float num = RT.anchorMax.y - RT.anchorMin.y;
			float num2 = RT.anchorMax.y * 2f - 1f;
			RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, yMax - yMax * num2 - top + num * yMax);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0006BE30 File Offset: 0x0006A030
		public static void MoveBottom(this RectTransform RT, float bottom = 0f)
		{
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMax.y - RT.anchorMin.y;
			float num2 = RT.anchorMax.y * 2f - 1f;
			RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, yMin + yMin * num2 + bottom - num * yMin);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000058FC File Offset: 0x00003AFC
		public static void MoveLeftInside(this RectTransform RT, float left = 0f)
		{
			RT.MoveLeft(left + RT.GetWidth() / 2f);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00005912 File Offset: 0x00003B12
		public static void MoveRightInside(this RectTransform RT, float right = 0f)
		{
			RT.MoveRight(right + RT.GetWidth() / 2f);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00005928 File Offset: 0x00003B28
		public static void MoveTopInside(this RectTransform RT, float top = 0f)
		{
			RT.MoveTop(top + RT.GetHeight() / 2f);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000593E File Offset: 0x00003B3E
		public static void MoveBottomInside(this RectTransform RT, float bottom = 0f)
		{
			RT.MoveBottom(bottom + RT.GetHeight() / 2f);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00005954 File Offset: 0x00003B54
		public static void MoveLeftOutside(this RectTransform RT, float left = 0f)
		{
			RT.MoveLeft(left - RT.GetWidth() / 2f);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000596A File Offset: 0x00003B6A
		public static void MoveRightOutside(this RectTransform RT, float right = 0f)
		{
			RT.MoveRight(right - RT.GetWidth() / 2f);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00005980 File Offset: 0x00003B80
		public static void MoveTopOutside(this RectTransform RT, float top = 0f)
		{
			RT.MoveTop(top - RT.GetHeight() / 2f);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00005996 File Offset: 0x00003B96
		public static void MoveBottomOutside(this RectTransform RT, float bottom = 0f)
		{
			RT.MoveBottom(bottom - RT.GetHeight() / 2f);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000059AC File Offset: 0x00003BAC
		public static void Move(this RectTransform RT, float x, float y)
		{
			RT.MoveLeft(x);
			RT.MoveBottom(y);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000059BC File Offset: 0x00003BBC
		public static void Move(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeft(point.x);
			RT.MoveBottom(point.y);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000059D8 File Offset: 0x00003BD8
		public static void MoveInside(this RectTransform RT, float x, float y)
		{
			RT.MoveLeftInside(x);
			RT.MoveBottomInside(y);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000059E8 File Offset: 0x00003BE8
		public static void MoveInside(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeftInside(point.x);
			RT.MoveBottomInside(point.y);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00005A04 File Offset: 0x00003C04
		public static void MoveOutside(this RectTransform RT, float x, float y)
		{
			RT.MoveLeftOutside(x);
			RT.MoveBottomOutside(y);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00005A14 File Offset: 0x00003C14
		public static void MoveOutside(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeftOutside(point.x);
			RT.MoveBottomOutside(point.y);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00005A30 File Offset: 0x00003C30
		public static void MoveFrom(this RectTransform RT, MinMax anchor, Vector2 point)
		{
			RT.MoveFrom(anchor, point.x, point.y);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0006BEB4 File Offset: 0x0006A0B4
		public static void MoveFrom(this RectTransform RT, MinMax anchor, float x, float y)
		{
			Vector2 vector = RT.AnchorToParentSpace(RectTransformExtension.AnchorOrigin(anchor) - RT.AnchorOrigin());
			RT.anchoredPosition = new Vector2(vector.x + x, vector.y + y);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00005A47 File Offset: 0x00003C47
		public static Vector2 ParentToChildSpace(this RectTransform RT, Vector2 point)
		{
			return RT.ParentToChildSpace(point.x, point.y);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0006BEF8 File Offset: 0x0006A0F8
		public static Vector2 ParentToChildSpace(this RectTransform RT, float x, float y)
		{
			float xMin = RT.GetParent().rect.xMin;
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMin.x * 2f - 1f;
			float num2 = RT.anchorMin.y * 2f - 1f;
			return new Vector2(xMin + xMin * num + x, yMin + yMin * num2 + y);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00005A5D File Offset: 0x00003C5D
		public static Vector2 ChildToParentSpace(this RectTransform RT, float x, float y)
		{
			return RT.AnchorOriginParent() + new Vector2(x, y);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00005A71 File Offset: 0x00003C71
		public static Vector2 ChildToParentSpace(this RectTransform RT, Vector2 point)
		{
			return RT.AnchorOriginParent() + point;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00005A7F File Offset: 0x00003C7F
		public static Vector2 ParentToAnchorSpace(this RectTransform RT, Vector2 point)
		{
			return RT.ParentToAnchorSpace(point.x, point.y);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0006BF7C File Offset: 0x0006A17C
		public static Vector2 ParentToAnchorSpace(this RectTransform RT, float x, float y)
		{
			Rect rect = RT.GetParent().rect;
			if (rect.width != 0f)
			{
				x /= rect.width;
			}
			else
			{
				x = 0f;
			}
			if (rect.height != 0f)
			{
				y /= rect.height;
			}
			else
			{
				y = 0f;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0006BFEC File Offset: 0x0006A1EC
		public static Vector2 AnchorToParentSpace(this RectTransform RT, float x, float y)
		{
			return new Vector2(x * RT.GetParent().rect.width, y * RT.GetParent().rect.height);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0006C028 File Offset: 0x0006A228
		public static Vector2 AnchorToParentSpace(this RectTransform RT, Vector2 point)
		{
			return new Vector2(point.x * RT.GetParent().rect.width, point.y * RT.GetParent().rect.height);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00005A95 File Offset: 0x00003C95
		public static Vector2 AnchorOrigin(this RectTransform RT)
		{
			return RectTransformExtension.AnchorOrigin(RT.GetAnchors());
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0006C070 File Offset: 0x0006A270
		public static Vector2 AnchorOrigin(MinMax anchor)
		{
			float num = anchor.min.x + (anchor.max.x - anchor.min.x) / 2f;
			float num2 = anchor.min.y + (anchor.max.y - anchor.min.y) / 2f;
			return new Vector2(num, num2);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0006C0E0 File Offset: 0x0006A2E0
		public static Vector2 AnchorOriginParent(this RectTransform RT)
		{
			return Vector2.Scale(RT.AnchorOrigin(), new Vector2(RT.GetParent().rect.width, RT.GetParent().rect.height));
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0006C124 File Offset: 0x0006A324
		public static Canvas GetRootCanvas(this RectTransform RT)
		{
			Canvas componentInParent = RT.GetComponentInParent<Canvas>();
			while (!componentInParent.isRootCanvas)
			{
				componentInParent = componentInParent.transform.parent.GetComponentInParent<Canvas>();
			}
			return componentInParent;
		}
	}
}
