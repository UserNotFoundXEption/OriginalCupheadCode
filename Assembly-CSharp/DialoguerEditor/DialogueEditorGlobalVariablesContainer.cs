using System;

namespace DialoguerEditor
{
	// Token: 0x020005CE RID: 1486
	[Serializable]
	public class DialogueEditorGlobalVariablesContainer
	{
		// Token: 0x06003DF3 RID: 15859 RVA: 0x00031DD2 File Offset: 0x0002FFD2
		public DialogueEditorGlobalVariablesContainer()
		{
			this.booleans = new DialogueEditorVariablesContainer();
			this.floats = new DialogueEditorVariablesContainer();
			this.strings = new DialogueEditorVariablesContainer();
		}

		// Token: 0x040031D0 RID: 12752
		public DialogueEditorVariablesContainer booleans;

		// Token: 0x040031D1 RID: 12753
		public DialogueEditorVariablesContainer floats;

		// Token: 0x040031D2 RID: 12754
		public DialogueEditorVariablesContainer strings;
	}
}
