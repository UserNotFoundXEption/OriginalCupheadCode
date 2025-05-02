using System;

namespace TMPro
{
	// Token: 0x0200068F RID: 1679
	[Serializable]
	public class TMP_Glyph : TMP_TextElement
	{
		// Token: 0x06004749 RID: 18249 RVA: 0x00159B6C File Offset: 0x00157D6C
		public static TMP_Glyph Clone(TMP_Glyph source)
		{
			return new TMP_Glyph
			{
				id = source.id,
				x = source.x,
				y = source.y,
				width = source.width,
				height = source.height,
				xOffset = source.xOffset,
				yOffset = source.yOffset,
				xAdvance = source.xAdvance
			};
		}
	}
}
