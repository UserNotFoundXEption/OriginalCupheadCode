using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class RectUtils
{
	// Token: 0x06000642 RID: 1602 RVA: 0x0000683F File Offset: 0x00004A3F
	public static Rect OffsetRect(Rect rect, int offset)
	{
		return new Rect(rect.x - (float)offset, rect.y - (float)offset, rect.width + (float)(offset * 2), rect.height + (float)(offset * 2));
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0006F040 File Offset: 0x0006D240
	public static Rect[] HorizontalDivide(Rect rect, int sections, float space)
	{
		Rect[] array = new Rect[sections];
		float num = rect.width / (float)sections - space * (float)(sections - 1) / (float)sections;
		for (int i = 0; i < sections; i++)
		{
			array[i] = new Rect(rect.x + num * (float)i + space * (float)i, rect.y, num, rect.height);
		}
		return array;
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00006872 File Offset: 0x00004A72
	public static Rect NewFromCenter(float xCenter, float yCenter, float width, float height)
	{
		return new Rect(xCenter - width * 0.5f, yCenter - height * 0.5f, width, height);
	}
}
