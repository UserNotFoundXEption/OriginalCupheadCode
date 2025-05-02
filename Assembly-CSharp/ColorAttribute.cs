using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class ColorAttribute : PropertyAttribute
{
	// Token: 0x06000485 RID: 1157 RVA: 0x0000529C File Offset: 0x0000349C
	public ColorAttribute(float w)
	{
		this.color = new Color(w, w, w, 1f);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000052B7 File Offset: 0x000034B7
	public ColorAttribute(float r, float g, float b)
	{
		this.color = new Color(r, g, b, 1f);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000052D2 File Offset: 0x000034D2
	public ColorAttribute(float r, float g, float b, float a)
	{
		this.color = new Color(r, g, b, a);
	}

	// Token: 0x04000462 RID: 1122
	public Color color;
}
