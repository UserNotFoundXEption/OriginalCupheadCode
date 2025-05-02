using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000670 RID: 1648
	[Serializable]
	public class TMP_Style
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x000377A8 File Offset: 0x000359A8
		// (set) Token: 0x060045D6 RID: 17878 RVA: 0x000377B0 File Offset: 0x000359B0
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				if (value != this.m_Name)
				{
					this.m_Name = value;
				}
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060045D7 RID: 17879 RVA: 0x000377CA File Offset: 0x000359CA
		// (set) Token: 0x060045D8 RID: 17880 RVA: 0x000377D2 File Offset: 0x000359D2
		public int hashCode
		{
			get
			{
				return this.m_HashCode;
			}
			set
			{
				if (value != this.m_HashCode)
				{
					this.m_HashCode = value;
				}
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060045D9 RID: 17881 RVA: 0x000377E7 File Offset: 0x000359E7
		public string styleOpeningDefinition
		{
			get
			{
				return this.m_OpeningDefinition;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060045DA RID: 17882 RVA: 0x000377EF File Offset: 0x000359EF
		public string styleClosingDefinition
		{
			get
			{
				return this.m_ClosingDefinition;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x000377F7 File Offset: 0x000359F7
		public int[] styleOpeningTagArray
		{
			get
			{
				return this.m_OpeningTagArray;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x000377FF File Offset: 0x000359FF
		public int[] styleClosingTagArray
		{
			get
			{
				return this.m_ClosingTagArray;
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x0014ECDC File Offset: 0x0014CEDC
		public void RefreshStyle()
		{
			this.m_HashCode = TMP_TextUtilities.GetSimpleHashCode(this.m_Name);
			this.m_OpeningTagArray = new int[this.m_OpeningDefinition.Length];
			for (int i = 0; i < this.m_OpeningDefinition.Length; i++)
			{
				this.m_OpeningTagArray[i] = (int)this.m_OpeningDefinition[i];
			}
			this.m_ClosingTagArray = new int[this.m_ClosingDefinition.Length];
			for (int j = 0; j < this.m_ClosingDefinition.Length; j++)
			{
				this.m_ClosingTagArray[j] = (int)this.m_ClosingDefinition[j];
			}
			TMPro_EventManager.ON_TEXT_STYLE_PROPERTY_CHANGED(true);
		}

		// Token: 0x040035CA RID: 13770
		[SerializeField]
		public string m_Name;

		// Token: 0x040035CB RID: 13771
		[SerializeField]
		public int m_HashCode;

		// Token: 0x040035CC RID: 13772
		[SerializeField]
		public string m_OpeningDefinition;

		// Token: 0x040035CD RID: 13773
		[SerializeField]
		public string m_ClosingDefinition;

		// Token: 0x040035CE RID: 13774
		[SerializeField]
		public int[] m_OpeningTagArray;

		// Token: 0x040035CF RID: 13775
		[SerializeField]
		public int[] m_ClosingTagArray;
	}
}
