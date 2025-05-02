using System;

namespace TMPro
{
	// Token: 0x0200069D RID: 1693
	public struct TMP_WordInfo
	{
		// Token: 0x06004763 RID: 18275 RVA: 0x0015A408 File Offset: 0x00158608
		public string GetWord()
		{
			string text = string.Empty;
			TMP_CharacterInfo[] characterInfo = this.textComponent.textInfo.characterInfo;
			for (int i = this.firstCharacterIndex; i < this.lastCharacterIndex + 1; i++)
			{
				text += characterInfo[i].character;
			}
			return text;
		}

		// Token: 0x04003796 RID: 14230
		public TMP_Text textComponent;

		// Token: 0x04003797 RID: 14231
		public int firstCharacterIndex;

		// Token: 0x04003798 RID: 14232
		public int lastCharacterIndex;

		// Token: 0x04003799 RID: 14233
		public int characterCount;
	}
}
