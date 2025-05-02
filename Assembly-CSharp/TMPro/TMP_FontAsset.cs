using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000669 RID: 1641
	[Serializable]
	public class TMP_FontAsset : TMP_Asset
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x000374B5 File Offset: 0x000356B5
		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				if (TMP_FontAsset.s_defaultFontAsset == null)
				{
					TMP_FontAsset.s_defaultFontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/ARIAL SDF");
				}
				return TMP_FontAsset.s_defaultFontAsset;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06004598 RID: 17816 RVA: 0x000374DB File Offset: 0x000356DB
		public FaceInfo fontInfo
		{
			get
			{
				return this.m_fontInfo;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06004599 RID: 17817 RVA: 0x000374E3 File Offset: 0x000356E3
		public Dictionary<int, TMP_Glyph> characterDictionary
		{
			get
			{
				return this.m_characterDictionary;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x000374EB File Offset: 0x000356EB
		public Dictionary<int, KerningPair> kerningDictionary
		{
			get
			{
				return this.m_kerningDictionary;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x000374F3 File Offset: 0x000356F3
		public KerningTable kerningInfo
		{
			get
			{
				return this.m_kerningInfo;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600459C RID: 17820 RVA: 0x000374FB File Offset: 0x000356FB
		public LineBreakingTable lineBreakingInfo
		{
			get
			{
				return this.m_lineBreakingInfo;
			}
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00037503 File Offset: 0x00035703
		public void OnEnable()
		{
			if (this.m_characterDictionary == null)
			{
				this.ReadFontDefinition();
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x0003751B File Offset: 0x0003571B
		public void OnDisable()
		{
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0003751D File Offset: 0x0003571D
		public void AddFaceInfo(FaceInfo faceInfo)
		{
			this.m_fontInfo = faceInfo;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0014DE8C File Offset: 0x0014C08C
		public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
		{
			this.m_glyphInfoList = new List<TMP_Glyph>();
			int num = glyphInfo.Length;
			this.m_fontInfo.CharacterCount = num;
			this.m_characterSet = new int[num];
			for (int i = 0; i < num; i++)
			{
				TMP_Glyph tmp_Glyph = new TMP_Glyph();
				tmp_Glyph.id = glyphInfo[i].id;
				tmp_Glyph.x = glyphInfo[i].x;
				tmp_Glyph.y = glyphInfo[i].y;
				tmp_Glyph.width = glyphInfo[i].width;
				tmp_Glyph.height = glyphInfo[i].height;
				tmp_Glyph.xOffset = glyphInfo[i].xOffset;
				tmp_Glyph.yOffset = glyphInfo[i].yOffset + this.m_fontInfo.Padding;
				tmp_Glyph.xAdvance = glyphInfo[i].xAdvance;
				this.m_glyphInfoList.Add(tmp_Glyph);
				this.m_characterSet[i] = tmp_Glyph.id;
			}
			this.m_glyphInfoList = (from s in this.m_glyphInfoList
			orderby s.id
			select s).ToList<TMP_Glyph>();
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00037526 File Offset: 0x00035726
		public void AddKerningInfo(KerningTable kerningTable)
		{
			this.m_kerningInfo = kerningTable;
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x0014DFA4 File Offset: 0x0014C1A4
		public void ReadFontDefinition()
		{
			if (this.m_fontInfo == null)
			{
				return;
			}
			this.m_characterDictionary = new Dictionary<int, TMP_Glyph>();
			foreach (TMP_Glyph tmp_Glyph in this.m_glyphInfoList)
			{
				if (!this.m_characterDictionary.ContainsKey(tmp_Glyph.id))
				{
					this.m_characterDictionary.Add(tmp_Glyph.id, tmp_Glyph);
				}
			}
			TMP_Glyph tmp_Glyph2 = new TMP_Glyph();
			if (this.m_characterDictionary.ContainsKey(32))
			{
				this.m_characterDictionary[32].width = this.m_characterDictionary[32].xAdvance;
				this.m_characterDictionary[32].height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				this.m_characterDictionary[32].yOffset = this.m_fontInfo.Ascender;
			}
			else
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 32;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = this.m_fontInfo.Ascender / 5f;
				tmp_Glyph2.height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_fontInfo.Ascender;
				tmp_Glyph2.xAdvance = this.m_fontInfo.PointSize / 4f;
				this.m_characterDictionary.Add(32, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(160))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[32]);
				this.m_characterDictionary.Add(160, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(8203))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[32]);
				tmp_Glyph2.width = 0f;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(8203, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(10))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 10;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = 10f;
				tmp_Glyph2.height = this.m_characterDictionary[32].height;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_characterDictionary[32].yOffset;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(10, tmp_Glyph2);
				if (!this.m_characterDictionary.ContainsKey(13))
				{
					this.m_characterDictionary.Add(13, tmp_Glyph2);
				}
			}
			if (!this.m_characterDictionary.ContainsKey(9))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 9;
				tmp_Glyph2.x = this.m_characterDictionary[32].x;
				tmp_Glyph2.y = this.m_characterDictionary[32].y;
				tmp_Glyph2.width = this.m_characterDictionary[32].width * (float)this.tabSize + (this.m_characterDictionary[32].xAdvance - this.m_characterDictionary[32].width) * (float)(this.tabSize - 1);
				tmp_Glyph2.height = this.m_characterDictionary[32].height;
				tmp_Glyph2.xOffset = this.m_characterDictionary[32].xOffset;
				tmp_Glyph2.yOffset = this.m_characterDictionary[32].yOffset;
				tmp_Glyph2.xAdvance = this.m_characterDictionary[32].xAdvance * (float)this.tabSize;
				this.m_characterDictionary.Add(9, tmp_Glyph2);
			}
			this.m_fontInfo.TabWidth = this.m_characterDictionary[9].xAdvance;
			if (this.m_fontInfo.Scale == 0f)
			{
				this.m_fontInfo.Scale = 1f;
			}
			this.m_kerningDictionary = new Dictionary<int, KerningPair>();
			List<KerningPair> kerningPairs = this.m_kerningInfo.kerningPairs;
			for (int i = 0; i < kerningPairs.Count; i++)
			{
				KerningPair kerningPair = kerningPairs[i];
				KerningPairKey kerningPairKey = new KerningPairKey(kerningPair.AscII_Left, kerningPair.AscII_Right);
				if (!this.m_kerningDictionary.ContainsKey(kerningPairKey.key))
				{
					this.m_kerningDictionary.Add(kerningPairKey.key, kerningPair);
				}
				else if (!TMP_Settings.warningsDisabled)
				{
				}
			}
			this.m_lineBreakingInfo = new LineBreakingTable();
			TextAsset textAsset = Resources.Load("LineBreaking Leading Characters", typeof(TextAsset)) as TextAsset;
			if (textAsset != null)
			{
				this.m_lineBreakingInfo.leadingCharacters = this.GetCharacters(textAsset);
			}
			TextAsset textAsset2 = Resources.Load("LineBreaking Following Characters", typeof(TextAsset)) as TextAsset;
			if (textAsset2 != null)
			{
				this.m_lineBreakingInfo.followingCharacters = this.GetCharacters(textAsset2);
			}
			this.hashCode = TMP_TextUtilities.GetSimpleHashCode(base.name);
			this.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.material.name);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0014E520 File Offset: 0x0014C720
		public Dictionary<int, char> GetCharacters(TextAsset file)
		{
			Dictionary<int, char> dictionary = new Dictionary<int, char>();
			foreach (char c in file.text)
			{
				if (!dictionary.ContainsKey((int)c))
				{
					dictionary.Add((int)c, c);
				}
			}
			return dictionary;
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x0003752F File Offset: 0x0003572F
		public bool HasCharacter(int character)
		{
			return this.m_characterDictionary != null && this.m_characterDictionary.ContainsKey(character);
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x00037552 File Offset: 0x00035752
		public bool HasCharacter(char character)
		{
			return this.m_characterDictionary != null && this.m_characterDictionary.ContainsKey((int)character);
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x0014E570 File Offset: 0x0014C770
		public bool HasCharacters(string text, out List<char> missingCharacters)
		{
			if (this.m_characterDictionary == null)
			{
				missingCharacters = null;
				return false;
			}
			missingCharacters = new List<char>();
			for (int i = 0; i < text.Length; i++)
			{
				if (!this.m_characterDictionary.ContainsKey((int)text[i]))
				{
					missingCharacters.Add(text[i]);
				}
			}
			return missingCharacters.Count == 0;
		}

		// Token: 0x0400359A RID: 13722
		public static TMP_FontAsset s_defaultFontAsset;

		// Token: 0x0400359B RID: 13723
		public TMP_FontAsset.FontAssetTypes fontAssetType;

		// Token: 0x0400359C RID: 13724
		[SerializeField]
		public FaceInfo m_fontInfo;

		// Token: 0x0400359D RID: 13725
		[SerializeField]
		public Texture2D atlas;

		// Token: 0x0400359E RID: 13726
		[SerializeField]
		public List<TMP_Glyph> m_glyphInfoList;

		// Token: 0x0400359F RID: 13727
		public Dictionary<int, TMP_Glyph> m_characterDictionary;

		// Token: 0x040035A0 RID: 13728
		public Dictionary<int, KerningPair> m_kerningDictionary;

		// Token: 0x040035A1 RID: 13729
		[SerializeField]
		public KerningTable m_kerningInfo;

		// Token: 0x040035A2 RID: 13730
		[SerializeField]
		public KerningPair m_kerningPair;

		// Token: 0x040035A3 RID: 13731
		[SerializeField]
		public LineBreakingTable m_lineBreakingInfo;

		// Token: 0x040035A4 RID: 13732
		[SerializeField]
		public List<TMP_FontAsset> fallbackFontAssets;

		// Token: 0x040035A5 RID: 13733
		[SerializeField]
		public FontCreationSetting fontCreationSettings;

		// Token: 0x040035A6 RID: 13734
		public TMP_FontWeights[] fontWeights = new TMP_FontWeights[10];

		// Token: 0x040035A7 RID: 13735
		public int[] m_characterSet;

		// Token: 0x040035A8 RID: 13736
		public float normalStyle;

		// Token: 0x040035A9 RID: 13737
		public float normalSpacingOffset;

		// Token: 0x040035AA RID: 13738
		public float boldStyle = 0.75f;

		// Token: 0x040035AB RID: 13739
		public float boldSpacing = 7f;

		// Token: 0x040035AC RID: 13740
		public byte italicStyle = 35;

		// Token: 0x040035AD RID: 13741
		public byte tabSize = 10;

		// Token: 0x040035AE RID: 13742
		public byte m_oldTabSize;

		// Token: 0x020012F4 RID: 4852
		public enum FontAssetTypes
		{
			// Token: 0x040081E7 RID: 33255
			None,
			// Token: 0x040081E8 RID: 33256
			SDF,
			// Token: 0x040081E9 RID: 33257
			Bitmap
		}
	}
}
