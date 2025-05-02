using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public struct Trilean2
{
	// Token: 0x06000599 RID: 1433 RVA: 0x00005F0B File Offset: 0x0000410B
	public Trilean2(Vector2 v)
	{
		this.x = v.x;
		this.y = v.y;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00005F31 File Offset: 0x00004131
	public Trilean2(bool x, bool y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00005F4B File Offset: 0x0000414B
	public Trilean2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00005F65 File Offset: 0x00004165
	public Trilean2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00005F7F File Offset: 0x0000417F
	public static implicit operator Trilean2(Vector2 v)
	{
		return new Trilean2(v);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00005F87 File Offset: 0x00004187
	public static implicit operator Vector2(Trilean2 t)
	{
		return new Vector2(t.x, t.y);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00005FA6 File Offset: 0x000041A6
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00005FB9 File Offset: 0x000041B9
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0006CE50 File Offset: 0x0006B050
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Trilean2(x:",
			this.x.Value,
			", y:",
			this.y.Value,
			")"
		});
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00005FCB File Offset: 0x000041CB
	public static bool operator ==(Trilean2 a, Trilean2 b)
	{
		return a.x.Value == b.x.Value && a.y.Value == b.y.Value;
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00006007 File Offset: 0x00004207
	public static bool operator !=(Trilean2 a, Trilean2 b)
	{
		return a.x.Value != b.x.Value || a.y.Value != b.y.Value;
	}

	// Token: 0x040004AC RID: 1196
	public Trilean x;

	// Token: 0x040004AD RID: 1197
	public Trilean y;
}
