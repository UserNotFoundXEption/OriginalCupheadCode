using System;
using UnityEngine;

// Token: 0x020005BA RID: 1466
public class PatternString
{
	// Token: 0x06003D7D RID: 15741 RVA: 0x00118FBC File Offset: 0x001171BC
	public PatternString(string[] patternString, bool randomizeMain = true, bool randomizeSub = true)
	{
		this.mainPatternString = patternString;
		this.mainIndex = ((!randomizeMain) ? 0 : Random.Range(0, patternString.Length));
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : Random.Range(0, this.subPatternString.Length));
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x00119034 File Offset: 0x00117234
	public PatternString(string patternString, bool randomizeSub = true)
	{
		this.mainIndex = 0;
		this.mainPatternString = new string[1];
		this.mainPatternString[0] = patternString;
		this.subPatternString = this.mainPatternString[0].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : Random.Range(0, this.subPatternString.Length));
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x001190A4 File Offset: 0x001172A4
	public PatternString(string[] patternString, char subSubStringSplitter, bool randomizeMain = true, bool randomizeSub = true)
	{
		this.mainPatternString = patternString;
		this.mainIndex = ((!randomizeMain) ? 0 : Random.Range(0, patternString.Length));
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : Random.Range(0, this.subPatternString.Length));
		this.subsubPatternString = this.subPatternString[this.subIndex].Split(new char[]
		{
			subSubStringSplitter
		});
		this.subSubStringSplitter = subSubStringSplitter;
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00031993 File Offset: 0x0002FB93
	public int SubStringLength()
	{
		return this.subPatternString.Length;
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x0003199D File Offset: 0x0002FB9D
	public void SetMainStringIndex(int value)
	{
		this.mainIndex = value % this.mainPatternString.Length;
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x000319AF File Offset: 0x0002FBAF
	public void SetSubStringIndex(int value)
	{
		this.subIndex = value % this.subPatternString.Length;
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x000319C1 File Offset: 0x0002FBC1
	public int GetMainStringIndex()
	{
		return this.mainIndex;
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x000319C9 File Offset: 0x0002FBC9
	public int GetSubStringIndex()
	{
		return this.subIndex;
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x000319D1 File Offset: 0x0002FBD1
	public char GetSubsubstringLetter(int index)
	{
		return this.subsubPatternString[index][0];
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x00119148 File Offset: 0x00117348
	public float GetSubsubstringFloat(int index)
	{
		float result = 0f;
		if (Parser.FloatTryParse(this.subsubPatternString[index], out result))
		{
			return result;
		}
		Debug.LogError("Syntax Error in" + this.subsubPatternString, null);
		return result;
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x000319E1 File Offset: 0x0002FBE1
	public char GetLetter()
	{
		return this.subPatternString[this.subIndex][0];
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x000319F6 File Offset: 0x0002FBF6
	public char PopLetter()
	{
		this.IncrementString();
		return this.GetLetter();
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x00031A04 File Offset: 0x0002FC04
	public string GetString()
	{
		return this.subPatternString[this.subIndex];
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x00031A13 File Offset: 0x0002FC13
	public string PopString()
	{
		this.IncrementString();
		return this.GetString();
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00119188 File Offset: 0x00117388
	public float GetFloat()
	{
		float result = 0f;
		if (Parser.FloatTryParse(this.subPatternString[this.subIndex], out result))
		{
			return result;
		}
		Debug.LogError("Syntax Error in" + this.mainPatternString, null);
		return result;
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00031A21 File Offset: 0x0002FC21
	public float PopFloat()
	{
		this.IncrementString();
		return this.GetFloat();
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x001191D0 File Offset: 0x001173D0
	public int GetInt()
	{
		int result = 0;
		if (Parser.IntTryParse(this.subPatternString[this.subIndex], out result))
		{
			return result;
		}
		Debug.LogError("Syntax Error in" + this.mainPatternString, null);
		return result;
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00031A2F File Offset: 0x0002FC2F
	public int PopInt()
	{
		this.IncrementString();
		return this.GetInt();
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x00119214 File Offset: 0x00117414
	public void IncrementString()
	{
		if (this.subIndex < this.subPatternString.Length - 1)
		{
			this.subIndex++;
		}
		else
		{
			this.mainIndex = (this.mainIndex + 1) % this.mainPatternString.Length;
			this.subIndex = 0;
		}
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		if (this.subsubPatternString != null)
		{
			this.subsubPatternString = this.subPatternString[this.subIndex].Split(new char[]
			{
				this.subSubStringSplitter
			});
		}
	}

	// Token: 0x040030E8 RID: 12520
	public int mainIndex;

	// Token: 0x040030E9 RID: 12521
	public int subIndex;

	// Token: 0x040030EA RID: 12522
	public char subSubStringSplitter;

	// Token: 0x040030EB RID: 12523
	public string[] mainPatternString;

	// Token: 0x040030EC RID: 12524
	public string[] subPatternString;

	// Token: 0x040030ED RID: 12525
	public string[] subsubPatternString;
}
