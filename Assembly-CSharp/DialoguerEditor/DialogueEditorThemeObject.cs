using System;

namespace DialoguerEditor
{
	// Token: 0x020005D6 RID: 1494
	[Serializable]
	public class DialogueEditorThemeObject
	{
		// Token: 0x06003E17 RID: 15895 RVA: 0x00031F56 File Offset: 0x00030156
		public DialogueEditorThemeObject()
		{
			this.name = string.Empty;
			this.linkage = string.Empty;
		}

		// Token: 0x04003206 RID: 12806
		public int id;

		// Token: 0x04003207 RID: 12807
		public string name;

		// Token: 0x04003208 RID: 12808
		public string linkage;
	}
}
