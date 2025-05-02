using System;

namespace DialoguerEditor
{
	// Token: 0x020005D8 RID: 1496
	[Serializable]
	public class DialogueEditorVariableObject
	{
		// Token: 0x06003E1B RID: 15899 RVA: 0x00031F87 File Offset: 0x00030187
		public DialogueEditorVariableObject()
		{
			this.name = string.Empty;
			this.variable = string.Empty;
		}

		// Token: 0x0400320B RID: 12811
		public string name;

		// Token: 0x0400320C RID: 12812
		public string variable;

		// Token: 0x0400320D RID: 12813
		public int id;
	}
}
