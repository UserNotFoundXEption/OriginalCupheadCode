using System;
using System.Collections.Generic;

namespace DialoguerEditor
{
	// Token: 0x020005D7 RID: 1495
	[Serializable]
	public class DialogueEditorThemesContainer
	{
		// Token: 0x06003E18 RID: 15896 RVA: 0x00031F74 File Offset: 0x00030174
		public DialogueEditorThemesContainer()
		{
			this.themes = new List<DialogueEditorThemeObject>();
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x0011BD20 File Offset: 0x00119F20
		public void addTheme()
		{
			int count = this.themes.Count;
			this.themes.Add(new DialogueEditorThemeObject());
			this.themes[count].id = count;
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0011BD5C File Offset: 0x00119F5C
		public void removeTheme()
		{
			this.themes.RemoveAt(this.themes.Count - 1);
			if (this.selection >= this.themes.Count)
			{
				this.selection = this.themes.Count - 1;
			}
		}

		// Token: 0x04003209 RID: 12809
		public List<DialogueEditorThemeObject> themes;

		// Token: 0x0400320A RID: 12810
		public int selection;
	}
}
