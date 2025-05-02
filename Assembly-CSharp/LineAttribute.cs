using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class LineAttribute : PropertyAttribute
{
	// Token: 0x06000488 RID: 1160 RVA: 0x000052EA File Offset: 0x000034EA
	public LineAttribute(int height)
	{
		this.height = height;
	}

	// Token: 0x04000463 RID: 1123
	public int height;
}
