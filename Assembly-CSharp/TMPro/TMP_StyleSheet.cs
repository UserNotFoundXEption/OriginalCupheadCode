using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000671 RID: 1649
	[Serializable]
	public class TMP_StyleSheet : ScriptableObject
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x0014ED8C File Offset: 0x0014CF8C
		public static TMP_StyleSheet instance
		{
			get
			{
				if (TMP_StyleSheet.s_Instance == null)
				{
					TMP_StyleSheet.s_Instance = TMP_Settings.defaultStyleSheet;
					if (TMP_StyleSheet.s_Instance == null)
					{
						TMP_StyleSheet.s_Instance = (Resources.Load("Style Sheets/TMP Default Style Sheet") as TMP_StyleSheet);
					}
					if (TMP_StyleSheet.s_Instance == null)
					{
						return null;
					}
					TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal();
				}
				return TMP_StyleSheet.s_Instance;
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00037826 File Offset: 0x00035A26
		public static TMP_StyleSheet LoadDefaultStyleSheet()
		{
			return TMP_StyleSheet.instance;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0003782D File Offset: 0x00035A2D
		public static TMP_Style GetStyle(int hashCode)
		{
			return TMP_StyleSheet.instance.GetStyleInternal(hashCode);
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x0014EDF8 File Offset: 0x0014CFF8
		public TMP_Style GetStyleInternal(int hashCode)
		{
			TMP_Style result;
			if (this.m_StyleDictionary.TryGetValue(hashCode, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x0014EE1C File Offset: 0x0014D01C
		public void UpdateStyleDictionaryKey(int old_key, int new_key)
		{
			if (this.m_StyleDictionary.ContainsKey(old_key))
			{
				TMP_Style value = this.m_StyleDictionary[old_key];
				this.m_StyleDictionary.Add(new_key, value);
				this.m_StyleDictionary.Remove(old_key);
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x0003783A File Offset: 0x00035A3A
		public static void RefreshStyles()
		{
			TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal();
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0014EE64 File Offset: 0x0014D064
		public void LoadStyleDictionaryInternal()
		{
			this.m_StyleDictionary.Clear();
			for (int i = 0; i < this.m_StyleList.Count; i++)
			{
				this.m_StyleList[i].RefreshStyle();
				if (!this.m_StyleDictionary.ContainsKey(this.m_StyleList[i].hashCode))
				{
					this.m_StyleDictionary.Add(this.m_StyleList[i].hashCode, this.m_StyleList[i]);
				}
			}
		}

		// Token: 0x040035D0 RID: 13776
		public static TMP_StyleSheet s_Instance;

		// Token: 0x040035D1 RID: 13777
		[SerializeField]
		public List<TMP_Style> m_StyleList = new List<TMP_Style>(1);

		// Token: 0x040035D2 RID: 13778
		public Dictionary<int, TMP_Style> m_StyleDictionary = new Dictionary<int, TMP_Style>();
	}
}
