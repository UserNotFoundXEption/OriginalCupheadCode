using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public struct Trilean
{
	// Token: 0x0600058D RID: 1421 RVA: 0x00005E3C File Offset: 0x0000403C
	public Trilean(bool b)
	{
		if (b)
		{
			this.value = 1;
		}
		else
		{
			this.value = -1;
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00005E57 File Offset: 0x00004057
	public Trilean(int i)
	{
		this.value = i;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00005E60 File Offset: 0x00004060
	public Trilean(float f)
	{
		if (f == 0f)
		{
			this.value = 0;
		}
		else
		{
			this.value = (int)Mathf.Sign(f);
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000590 RID: 1424 RVA: 0x00005E86 File Offset: 0x00004086
	// (set) Token: 0x06000591 RID: 1425 RVA: 0x00005E8E File Offset: 0x0000408E
	public int Value
	{
		get
		{
			return this.value;
		}
		set
		{
			if (value > 0)
			{
				this.value = 1;
			}
			else if (value < 0)
			{
				this.value = -1;
			}
			else
			{
				this.value = 0;
			}
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00005EBD File Offset: 0x000040BD
	public static implicit operator Trilean(bool b)
	{
		return new Trilean(b);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00005EC5 File Offset: 0x000040C5
	public static implicit operator bool(Trilean t)
	{
		return t.Value >= 0;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00005EDB File Offset: 0x000040DB
	public static implicit operator Trilean(int i)
	{
		return new Trilean(i);
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00005EE3 File Offset: 0x000040E3
	public static implicit operator int(Trilean t)
	{
		return t.Value;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00005EEC File Offset: 0x000040EC
	public static implicit operator Trilean(float f)
	{
		return new Trilean(f);
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00005EF4 File Offset: 0x000040F4
	public static implicit operator float(Trilean t)
	{
		return (float)t.Value;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00005EFE File Offset: 0x000040FE
	public override string ToString()
	{
		return this.Value.ToStringInvariant();
	}

	// Token: 0x040004AB RID: 1195
	public int value;
}
