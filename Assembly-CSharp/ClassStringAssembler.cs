using System;

// Token: 0x0200006A RID: 106
public class ClassStringAssembler
{
	// Token: 0x0600057A RID: 1402 RVA: 0x00005D19 File Offset: 0x00003F19
	public ClassStringAssembler(int indent = 0)
	{
		this.indents = indent;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00005D33 File Offset: 0x00003F33
	public void Add(string s)
	{
		this.value += s;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0006CBC4 File Offset: 0x0006ADC4
	public void AddLine(string s)
	{
		int index = 0;
		if (s.Length > 0)
		{
			index = s.Length - 1;
		}
		if (s.Length > 0 && (s[0] == '}' || s[0] == ')' || s[0] == ']'))
		{
			this.indents--;
		}
		this.Add("\n" + this.PreIndent() + s);
		if (s.Length > 0 && (s[index] == '{' || s[index] == '(' || s[index] == '['))
		{
			this.indents++;
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00005D47 File Offset: 0x00003F47
	public void Break()
	{
		this.value += "\n";
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00005D5F File Offset: 0x00003F5F
	public void Indent()
	{
		this.indents++;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00005D6F File Offset: 0x00003F6F
	public void Undent()
	{
		this.indents--;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0006CC88 File Offset: 0x0006AE88
	public string PreIndent()
	{
		string text = string.Empty;
		for (int i = 0; i < this.indents; i++)
		{
			text += "\t";
		}
		return text;
	}

	// Token: 0x040004A3 RID: 1187
	public int indents;

	// Token: 0x040004A4 RID: 1188
	public string value = string.Empty;
}
