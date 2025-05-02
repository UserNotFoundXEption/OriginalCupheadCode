using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x020006A2 RID: 1698
	public struct WordWrapState
	{
		// Token: 0x040037B4 RID: 14260
		public int previous_WordBreak;

		// Token: 0x040037B5 RID: 14261
		public int total_CharacterCount;

		// Token: 0x040037B6 RID: 14262
		public int visible_CharacterCount;

		// Token: 0x040037B7 RID: 14263
		public int visible_SpriteCount;

		// Token: 0x040037B8 RID: 14264
		public int visible_LinkCount;

		// Token: 0x040037B9 RID: 14265
		public int firstCharacterIndex;

		// Token: 0x040037BA RID: 14266
		public int firstVisibleCharacterIndex;

		// Token: 0x040037BB RID: 14267
		public int lastCharacterIndex;

		// Token: 0x040037BC RID: 14268
		public int lastVisibleCharIndex;

		// Token: 0x040037BD RID: 14269
		public int lineNumber;

		// Token: 0x040037BE RID: 14270
		public float maxAscender;

		// Token: 0x040037BF RID: 14271
		public float maxDescender;

		// Token: 0x040037C0 RID: 14272
		public float maxLineAscender;

		// Token: 0x040037C1 RID: 14273
		public float maxLineDescender;

		// Token: 0x040037C2 RID: 14274
		public float previousLineAscender;

		// Token: 0x040037C3 RID: 14275
		public float xAdvance;

		// Token: 0x040037C4 RID: 14276
		public float preferredWidth;

		// Token: 0x040037C5 RID: 14277
		public float preferredHeight;

		// Token: 0x040037C6 RID: 14278
		public float previousLineScale;

		// Token: 0x040037C7 RID: 14279
		public int wordCount;

		// Token: 0x040037C8 RID: 14280
		public FontStyles fontStyle;

		// Token: 0x040037C9 RID: 14281
		public float fontScale;

		// Token: 0x040037CA RID: 14282
		public float fontScaleMultiplier;

		// Token: 0x040037CB RID: 14283
		public float currentFontSize;

		// Token: 0x040037CC RID: 14284
		public float baselineOffset;

		// Token: 0x040037CD RID: 14285
		public float lineOffset;

		// Token: 0x040037CE RID: 14286
		public TMP_TextInfo textInfo;

		// Token: 0x040037CF RID: 14287
		public TMP_LineInfo lineInfo;

		// Token: 0x040037D0 RID: 14288
		public Color32 vertexColor;

		// Token: 0x040037D1 RID: 14289
		public TMP_XmlTagStack<Color32> colorStack;

		// Token: 0x040037D2 RID: 14290
		public TMP_XmlTagStack<float> sizeStack;

		// Token: 0x040037D3 RID: 14291
		public TMP_XmlTagStack<int> fontWeightStack;

		// Token: 0x040037D4 RID: 14292
		public TMP_XmlTagStack<int> styleStack;

		// Token: 0x040037D5 RID: 14293
		public TMP_XmlTagStack<int> actionStack;

		// Token: 0x040037D6 RID: 14294
		public TMP_XmlTagStack<MaterialReference> materialReferenceStack;

		// Token: 0x040037D7 RID: 14295
		public TMP_FontAsset currentFontAsset;

		// Token: 0x040037D8 RID: 14296
		public TMP_SpriteAsset currentSpriteAsset;

		// Token: 0x040037D9 RID: 14297
		public Material currentMaterial;

		// Token: 0x040037DA RID: 14298
		public int currentMaterialIndex;

		// Token: 0x040037DB RID: 14299
		public Extents meshExtents;

		// Token: 0x040037DC RID: 14300
		public bool tagNoParsing;
	}
}
