using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200066D RID: 1645
	[ExecuteInEditMode]
	[Serializable]
	public class TMP_Settings : ScriptableObject
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x0003765E File Offset: 0x0003585E
		public static bool enableWordWrapping
		{
			get
			{
				return TMP_Settings.instance.m_enableWordWrapping;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060045C0 RID: 17856 RVA: 0x0003766A File Offset: 0x0003586A
		public static bool enableKerning
		{
			get
			{
				return TMP_Settings.instance.m_enableKerning;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x00037676 File Offset: 0x00035876
		public static bool enableExtraPadding
		{
			get
			{
				return TMP_Settings.instance.m_enableExtraPadding;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060045C2 RID: 17858 RVA: 0x00037682 File Offset: 0x00035882
		public static bool enableTintAllSprites
		{
			get
			{
				return TMP_Settings.instance.m_enableTintAllSprites;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060045C3 RID: 17859 RVA: 0x0003768E File Offset: 0x0003588E
		public static bool warningsDisabled
		{
			get
			{
				return TMP_Settings.instance.m_warningsDisabled;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x0003769A File Offset: 0x0003589A
		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAsset;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x000376A6 File Offset: 0x000358A6
		public static List<TMP_FontAsset> fallbackFontAssets
		{
			get
			{
				return TMP_Settings.instance.m_fallbackFontAssets;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x000376B2 File Offset: 0x000358B2
		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAsset;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x000376BE File Offset: 0x000358BE
		public static TMP_StyleSheet defaultStyleSheet
		{
			get
			{
				return TMP_Settings.instance.m_defaultStyleSheet;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x000376CA File Offset: 0x000358CA
		public static TMP_Settings instance
		{
			get
			{
				if (TMP_Settings.s_Instance == null)
				{
					TMP_Settings.s_Instance = (Resources.Load("TMP Settings") as TMP_Settings);
				}
				return TMP_Settings.s_Instance;
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0014EC14 File Offset: 0x0014CE14
		public static TMP_Settings LoadDefaultSettings()
		{
			if (TMP_Settings.s_Instance == null)
			{
				TMP_Settings tmp_Settings = Resources.Load("TMP Settings") as TMP_Settings;
				if (tmp_Settings != null)
				{
					TMP_Settings.s_Instance = tmp_Settings;
				}
			}
			return TMP_Settings.s_Instance;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x000376F5 File Offset: 0x000358F5
		public static TMP_Settings GetSettings()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance;
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0003770E File Offset: 0x0003590E
		public static TMP_FontAsset GetFontAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultFontAsset;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x0003772C File Offset: 0x0003592C
		public static TMP_SpriteAsset GetSpriteAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultSpriteAsset;
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x0003774A File Offset: 0x0003594A
		public static TMP_StyleSheet GetStyleSheet()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultStyleSheet;
		}

		// Token: 0x040035B7 RID: 13751
		public static TMP_Settings s_Instance;

		// Token: 0x040035B8 RID: 13752
		[SerializeField]
		public bool m_enableWordWrapping;

		// Token: 0x040035B9 RID: 13753
		[SerializeField]
		public bool m_enableKerning;

		// Token: 0x040035BA RID: 13754
		[SerializeField]
		public bool m_enableExtraPadding;

		// Token: 0x040035BB RID: 13755
		[SerializeField]
		public bool m_enableTintAllSprites;

		// Token: 0x040035BC RID: 13756
		[SerializeField]
		public bool m_warningsDisabled;

		// Token: 0x040035BD RID: 13757
		[SerializeField]
		public TMP_FontAsset m_defaultFontAsset;

		// Token: 0x040035BE RID: 13758
		[SerializeField]
		public List<TMP_FontAsset> m_fallbackFontAssets;

		// Token: 0x040035BF RID: 13759
		[SerializeField]
		public TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040035C0 RID: 13760
		[SerializeField]
		public TMP_StyleSheet m_defaultStyleSheet;
	}
}
