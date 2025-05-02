using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class EaseUtils
{
	// Token: 0x060005E7 RID: 1511 RVA: 0x0006DDDC File Offset: 0x0006BFDC
	public static float EaseInOut(EaseUtils.EaseType inEase, EaseUtils.EaseType outEase, float start, float end, float value)
	{
		if (value < 0.5f)
		{
			float value2 = Mathf.Clamp(value * 2f, 0f, 1f);
			float end2 = Mathf.Lerp(start, end, 0.5f);
			return EaseUtils.Ease(inEase, start, end2, value2);
		}
		if (value > 0.5f)
		{
			float value2 = Mathf.Clamp(value * 2f - 1f, 0f, 1f);
			float start2 = Mathf.Lerp(start, end, 0.5f);
			return EaseUtils.Ease(outEase, start2, end, value2);
		}
		return Mathf.Lerp(start, end, 0.5f);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0006DE78 File Offset: 0x0006C078
	public static float Ease(EaseUtils.EaseType ease, float start, float end, float value)
	{
		switch (ease)
		{
		case EaseUtils.EaseType.easeInQuad:
			return EaseUtils.EaseInQuad(start, end, value);
		case EaseUtils.EaseType.easeOutQuad:
			return EaseUtils.EaseOutQuad(start, end, value);
		case EaseUtils.EaseType.easeInOutQuad:
			return EaseUtils.EaseInOutQuad(start, end, value);
		case EaseUtils.EaseType.easeInCubic:
			return EaseUtils.EaseInCubic(start, end, value);
		case EaseUtils.EaseType.easeOutCubic:
			return EaseUtils.EaseOutCubic(start, end, value);
		case EaseUtils.EaseType.easeInOutCubic:
			return EaseUtils.EaseInOutCubic(start, end, value);
		case EaseUtils.EaseType.easeInQuart:
			return EaseUtils.EaseInQuart(start, end, value);
		case EaseUtils.EaseType.easeOutQuart:
			return EaseUtils.EaseOutQuart(start, end, value);
		case EaseUtils.EaseType.easeInOutQuart:
			return EaseUtils.EaseInOutQuart(start, end, value);
		case EaseUtils.EaseType.easeInQuint:
			return EaseUtils.EaseInQuint(start, end, value);
		case EaseUtils.EaseType.easeOutQuint:
			return EaseUtils.EaseOutQuint(start, end, value);
		case EaseUtils.EaseType.easeInOutQuint:
			return EaseUtils.EaseInOutQuint(start, end, value);
		case EaseUtils.EaseType.easeInSine:
			return EaseUtils.EaseInSine(start, end, value);
		case EaseUtils.EaseType.easeOutSine:
			return EaseUtils.EaseOutSine(start, end, value);
		case EaseUtils.EaseType.easeInOutSine:
			return EaseUtils.EaseInOutSine(start, end, value);
		case EaseUtils.EaseType.easeInExpo:
			return EaseUtils.EaseInExpo(start, end, value);
		case EaseUtils.EaseType.easeOutExpo:
			return EaseUtils.EaseOutExpo(start, end, value);
		case EaseUtils.EaseType.easeInOutExpo:
			return EaseUtils.EaseInOutExpo(start, end, value);
		case EaseUtils.EaseType.easeInCirc:
			return EaseUtils.EaseInCirc(start, end, value);
		case EaseUtils.EaseType.easeOutCirc:
			return EaseUtils.EaseOutCirc(start, end, value);
		case EaseUtils.EaseType.easeInOutCirc:
			return EaseUtils.EaseInOutCirc(start, end, value);
		case EaseUtils.EaseType.spring:
			return EaseUtils.Spring(start, end, value);
		case EaseUtils.EaseType.easeInBounce:
			return EaseUtils.EaseInBounce(start, end, value);
		case EaseUtils.EaseType.easeOutBounce:
			return EaseUtils.EaseOutBounce(start, end, value);
		case EaseUtils.EaseType.easeInOutBounce:
			return EaseUtils.EaseInOutBounce(start, end, value);
		case EaseUtils.EaseType.easeInBack:
			return EaseUtils.EaseInBack(start, end, value);
		case EaseUtils.EaseType.easeOutBack:
			return EaseUtils.EaseOutBack(start, end, value);
		case EaseUtils.EaseType.easeInOutBack:
			return EaseUtils.EaseInOutBack(start, end, value);
		case EaseUtils.EaseType.easeInElastic:
			return EaseUtils.EaseInElastic(start, end, value);
		case EaseUtils.EaseType.easeOutElastic:
			return EaseUtils.EaseOutElastic(start, end, value);
		case EaseUtils.EaseType.easeInOutElastic:
			return EaseUtils.EaseInOutElastic(start, end, value);
		}
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00006386 File Offset: 0x00004586
	public static float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0006E034 File Offset: 0x0006C234
	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0006E0AC File Offset: 0x0006C2AC
	public static float Spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.14159274f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00006390 File Offset: 0x00004590
	public static float EaseInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0000639E File Offset: 0x0000459E
	public static float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0006E110 File Offset: 0x0006C310
	public static float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x000063B3 File Offset: 0x000045B3
	public static float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x000063C3 File Offset: 0x000045C3
	public static float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0006E168 File Offset: 0x0006C368
	public static float EaseInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000063E2 File Offset: 0x000045E2
	public static float EaseInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x000063F4 File Offset: 0x000045F4
	public static float EaseOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0006E1BC File Offset: 0x0006C3BC
	public static float EaseInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00006416 File Offset: 0x00004616
	public static float EaseInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0000642A File Offset: 0x0000462A
	public static float EaseOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0006E218 File Offset: 0x0006C418
	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0006E274 File Offset: 0x0006C474
	public static float EaseInOutArbitraryCoefficient(float start, float end, float value, float c)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(value, c) + start;
		}
		value -= 2f;
		return end * 2f + start - end / 2f * (Mathf.Pow(Mathf.Abs(value), c - 1f) * Mathf.Abs(value) + 2f);
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0000644D File Offset: 0x0000464D
	public static float EaseInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.57079637f) + end + start;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0000646D File Offset: 0x0000466D
	public static float EaseOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.57079637f) + start;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0000648A File Offset: 0x0000468A
	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.14159274f * value / 1f) - 1f) + start;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000064B4 File Offset: 0x000046B4
	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000064DC File Offset: 0x000046DC
	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0006E2E8 File Offset: 0x0006C4E8
	public static float EaseInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00006505 File Offset: 0x00004705
	public static float EaseInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00006525 File Offset: 0x00004725
	public static float EaseOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0006E35C File Offset: 0x0006C55C
	public static float EaseInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0006E3CC File Offset: 0x0006C5CC
	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - EaseUtils.EaseOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0006E3F8 File Offset: 0x0006C5F8
	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.363636374f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.727272749f)
		{
			value -= 0.545454562f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.90909090909090906)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.954545438f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0006E4A0 File Offset: 0x0006C6A0
	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return EaseUtils.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return EaseUtils.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0006E504 File Offset: 0x0006C704
	public static float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0006E538 File Offset: 0x0006C738
	public static float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0006E578 File Offset: 0x0006C778
	public static float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0006E5F8 File Offset: 0x0006C7F8
	public static float Punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.28318548f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.28318548f / num);
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0006E670 File Offset: 0x0006C870
	public static float EaseInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2)) + start;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0006E728 File Offset: 0x0006C928
	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.28318548f / num2) + end + start;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0006E7D8 File Offset: 0x0006C9D8
	public static float EaseInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2) * 0.5f + end + start;
	}

	// Token: 0x040004C4 RID: 1220
	public const float EaseOutBounceTime = 0.363636374f;

	// Token: 0x020008BC RID: 2236
	public enum EaseType
	{
		// Token: 0x040042C5 RID: 17093
		easeInQuad,
		// Token: 0x040042C6 RID: 17094
		easeOutQuad,
		// Token: 0x040042C7 RID: 17095
		easeInOutQuad,
		// Token: 0x040042C8 RID: 17096
		easeInCubic,
		// Token: 0x040042C9 RID: 17097
		easeOutCubic,
		// Token: 0x040042CA RID: 17098
		easeInOutCubic,
		// Token: 0x040042CB RID: 17099
		easeInQuart,
		// Token: 0x040042CC RID: 17100
		easeOutQuart,
		// Token: 0x040042CD RID: 17101
		easeInOutQuart,
		// Token: 0x040042CE RID: 17102
		easeInQuint,
		// Token: 0x040042CF RID: 17103
		easeOutQuint,
		// Token: 0x040042D0 RID: 17104
		easeInOutQuint,
		// Token: 0x040042D1 RID: 17105
		easeInSine,
		// Token: 0x040042D2 RID: 17106
		easeOutSine,
		// Token: 0x040042D3 RID: 17107
		easeInOutSine,
		// Token: 0x040042D4 RID: 17108
		easeInExpo,
		// Token: 0x040042D5 RID: 17109
		easeOutExpo,
		// Token: 0x040042D6 RID: 17110
		easeInOutExpo,
		// Token: 0x040042D7 RID: 17111
		easeInCirc,
		// Token: 0x040042D8 RID: 17112
		easeOutCirc,
		// Token: 0x040042D9 RID: 17113
		easeInOutCirc,
		// Token: 0x040042DA RID: 17114
		linear,
		// Token: 0x040042DB RID: 17115
		spring,
		// Token: 0x040042DC RID: 17116
		easeInBounce,
		// Token: 0x040042DD RID: 17117
		easeOutBounce,
		// Token: 0x040042DE RID: 17118
		easeInOutBounce,
		// Token: 0x040042DF RID: 17119
		easeInBack,
		// Token: 0x040042E0 RID: 17120
		easeOutBack,
		// Token: 0x040042E1 RID: 17121
		easeInOutBack,
		// Token: 0x040042E2 RID: 17122
		easeInElastic,
		// Token: 0x040042E3 RID: 17123
		easeOutElastic,
		// Token: 0x040042E4 RID: 17124
		easeInOutElastic,
		// Token: 0x040042E5 RID: 17125
		punch
	}
}
