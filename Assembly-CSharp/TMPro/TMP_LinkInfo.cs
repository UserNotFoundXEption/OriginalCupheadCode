using System;

namespace TMPro
{
	// Token: 0x0200069C RID: 1692
	public struct TMP_LinkInfo
	{
		// Token: 0x06004760 RID: 18272 RVA: 0x0015A354 File Offset: 0x00158554
		public void SetLinkID(char[] text, int startIndex, int length)
		{
			if (this.linkID == null || this.linkID.Length < length)
			{
				this.linkID = new char[length];
			}
			for (int i = 0; i < length; i++)
			{
				this.linkID[i] = text[startIndex + i];
			}
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0015A3A8 File Offset: 0x001585A8
		public string GetLinkText()
		{
			string text = string.Empty;
			TMP_TextInfo textInfo = this.textComponent.textInfo;
			for (int i = this.linkTextfirstCharacterIndex; i < this.linkTextfirstCharacterIndex + this.linkTextLength; i++)
			{
				text += textInfo.characterInfo[i].character;
			}
			return text;
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x00038BD0 File Offset: 0x00036DD0
		public string GetLinkID()
		{
			if (this.textComponent == null)
			{
				return string.Empty;
			}
			return new string(this.linkID);
		}

		// Token: 0x0400378F RID: 14223
		public TMP_Text textComponent;

		// Token: 0x04003790 RID: 14224
		public int hashCode;

		// Token: 0x04003791 RID: 14225
		public int linkIdFirstCharacterIndex;

		// Token: 0x04003792 RID: 14226
		public int linkIdLength;

		// Token: 0x04003793 RID: 14227
		public int linkTextfirstCharacterIndex;

		// Token: 0x04003794 RID: 14228
		public int linkTextLength;

		// Token: 0x04003795 RID: 14229
		public char[] linkID;
	}
}
