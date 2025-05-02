using System;
using System.Collections.Generic;
using System.Linq;

namespace TMPro
{
	// Token: 0x02000694 RID: 1684
	[Serializable]
	public class KerningTable
	{
		// Token: 0x0600474D RID: 18253 RVA: 0x00038B44 File Offset: 0x00036D44
		public KerningTable()
		{
			this.kerningPairs = new List<KerningPair>();
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x00159BE0 File Offset: 0x00157DE0
		public void AddKerningPair()
		{
			if (this.kerningPairs.Count == 0)
			{
				this.kerningPairs.Add(new KerningPair(0, 0, 0f));
			}
			else
			{
				int ascII_Left = this.kerningPairs.Last<KerningPair>().AscII_Left;
				int ascII_Right = this.kerningPairs.Last<KerningPair>().AscII_Right;
				float xadvanceOffset = this.kerningPairs.Last<KerningPair>().XadvanceOffset;
				this.kerningPairs.Add(new KerningPair(ascII_Left, ascII_Right, xadvanceOffset));
			}
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x00159C60 File Offset: 0x00157E60
		public int AddKerningPair(int left, int right, float offset)
		{
			int num = this.kerningPairs.FindIndex((KerningPair item) => item.AscII_Left == left && item.AscII_Right == right);
			if (num == -1)
			{
				this.kerningPairs.Add(new KerningPair(left, right, offset));
				return 0;
			}
			return -1;
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x00159CC0 File Offset: 0x00157EC0
		public void RemoveKerningPair(int left, int right)
		{
			int num = this.kerningPairs.FindIndex((KerningPair item) => item.AscII_Left == left && item.AscII_Right == right);
			if (num != -1)
			{
				this.kerningPairs.RemoveAt(num);
			}
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x00038B57 File Offset: 0x00036D57
		public void RemoveKerningPair(int index)
		{
			this.kerningPairs.RemoveAt(index);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x00159D0C File Offset: 0x00157F0C
		public void SortKerningPairs()
		{
			if (this.kerningPairs.Count > 0)
			{
				this.kerningPairs = (from s in this.kerningPairs
				orderby s.AscII_Left, s.AscII_Right
				select s).ToList<KerningPair>();
			}
		}

		// Token: 0x04003749 RID: 14153
		public List<KerningPair> kerningPairs;
	}
}
