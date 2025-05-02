using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
[Serializable]
public class MinMax
{
	// Token: 0x06000585 RID: 1413 RVA: 0x00005DC6 File Offset: 0x00003FC6
	public MinMax(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00005DDC File Offset: 0x00003FDC
	public float RandomFloat()
	{
		return Random.Range(this.min, this.max);
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0006CE2C File Offset: 0x0006B02C
	public int RandomInt()
	{
		int num = (int)this.min;
		int num2 = (int)this.max;
		return Random.Range(num, num2);
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00005DEF File Offset: 0x00003FEF
	public float GetFloatAt(float i)
	{
		return Mathf.Lerp(this.min, this.max, i);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00005E03 File Offset: 0x00004003
	public float GetIntAt(float i)
	{
		return (float)((int)Mathf.Lerp(this.min, this.max, i));
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00005E19 File Offset: 0x00004019
	public MinMax Clone()
	{
		return new MinMax(this.min, this.max);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00005E2C File Offset: 0x0000402C
	public static implicit operator float(MinMax m)
	{
		return m.RandomFloat();
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00005E34 File Offset: 0x00004034
	public static implicit operator int(MinMax m)
	{
		return m.RandomInt();
	}

	// Token: 0x040004A9 RID: 1193
	public float min;

	// Token: 0x040004AA RID: 1194
	public float max;
}
