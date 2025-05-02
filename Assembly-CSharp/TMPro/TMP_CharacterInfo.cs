using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000698 RID: 1688
	public struct TMP_CharacterInfo
	{
		// Token: 0x04003762 RID: 14178
		public char character;

		// Token: 0x04003763 RID: 14179
		public short index;

		// Token: 0x04003764 RID: 14180
		public TMP_TextElementType elementType;

		// Token: 0x04003765 RID: 14181
		public TMP_TextElement textElement;

		// Token: 0x04003766 RID: 14182
		public TMP_FontAsset fontAsset;

		// Token: 0x04003767 RID: 14183
		public TMP_SpriteAsset spriteAsset;

		// Token: 0x04003768 RID: 14184
		public int spriteIndex;

		// Token: 0x04003769 RID: 14185
		public Material material;

		// Token: 0x0400376A RID: 14186
		public int materialReferenceIndex;

		// Token: 0x0400376B RID: 14187
		public float pointSize;

		// Token: 0x0400376C RID: 14188
		public short lineNumber;

		// Token: 0x0400376D RID: 14189
		public short pageNumber;

		// Token: 0x0400376E RID: 14190
		public short vertexIndex;

		// Token: 0x0400376F RID: 14191
		public TMP_Vertex vertex_TL;

		// Token: 0x04003770 RID: 14192
		public TMP_Vertex vertex_BL;

		// Token: 0x04003771 RID: 14193
		public TMP_Vertex vertex_TR;

		// Token: 0x04003772 RID: 14194
		public TMP_Vertex vertex_BR;

		// Token: 0x04003773 RID: 14195
		public Vector3 topLeft;

		// Token: 0x04003774 RID: 14196
		public Vector3 bottomLeft;

		// Token: 0x04003775 RID: 14197
		public Vector3 topRight;

		// Token: 0x04003776 RID: 14198
		public Vector3 bottomRight;

		// Token: 0x04003777 RID: 14199
		public float origin;

		// Token: 0x04003778 RID: 14200
		public float ascender;

		// Token: 0x04003779 RID: 14201
		public float baseLine;

		// Token: 0x0400377A RID: 14202
		public float descender;

		// Token: 0x0400377B RID: 14203
		public float xAdvance;

		// Token: 0x0400377C RID: 14204
		public float aspectRatio;

		// Token: 0x0400377D RID: 14205
		public float scale;

		// Token: 0x0400377E RID: 14206
		public Color32 color;

		// Token: 0x0400377F RID: 14207
		public FontStyles style;

		// Token: 0x04003780 RID: 14208
		public bool isVisible;
	}
}
