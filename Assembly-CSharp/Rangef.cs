using System;

// Token: 0x02000072 RID: 114
[Serializable]
public struct Rangef
{
	// Token: 0x060005B6 RID: 1462 RVA: 0x000060E4 File Offset: 0x000042E4
	public Rangef(float minimum, float maximum)
	{
		this.minimum = minimum;
		this.maximum = maximum;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x000060F4 File Offset: 0x000042F4
	public bool ContainsInclusive(float checkValue)
	{
		return MathUtilities.BetweenInclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00006108 File Offset: 0x00004308
	public bool ContainsExclusive(float checkValue)
	{
		return MathUtilities.BetweenExclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0000611C File Offset: 0x0000431C
	public bool ContainsInclusiveExclusive(float checkValue)
	{
		return MathUtilities.BetweenInclusiveExclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00006130 File Offset: 0x00004330
	public bool ContainsExclusiveInclusive(float checkValue)
	{
		return MathUtilities.BetweenExclusiveInclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00006144 File Offset: 0x00004344
	public override string ToString()
	{
		return string.Format("({0}, {1})", this.minimum, this.maximum);
	}

	// Token: 0x040004B2 RID: 1202
	public float minimum;

	// Token: 0x040004B3 RID: 1203
	public float maximum;
}
