using System;
using System.Collections.Generic;

namespace DialoguerEditor
{
	// Token: 0x020005D9 RID: 1497
	[Serializable]
	public class DialogueEditorVariablesContainer
	{
		// Token: 0x06003E1C RID: 15900 RVA: 0x00031FA5 File Offset: 0x000301A5
		public DialogueEditorVariablesContainer()
		{
			this.selection = 0;
			this.variables = new List<DialogueEditorVariableObject>();
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x0011BDAC File Offset: 0x00119FAC
		public void addVariable()
		{
			int count = this.variables.Count;
			this.variables.Add(new DialogueEditorVariableObject());
			this.variables[count].id = count;
			this.selection = this.variables.Count - 1;
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x0011BDFC File Offset: 0x00119FFC
		public void removeVariable()
		{
			if (this.variables.Count < 1)
			{
				return;
			}
			this.variables.RemoveAt(this.variables.Count - 1);
			if (this.selection > this.variables.Count - 1)
			{
				this.selection = this.variables.Count - 1;
			}
		}

		// Token: 0x0400320E RID: 12814
		public List<DialogueEditorVariableObject> variables;

		// Token: 0x0400320F RID: 12815
		public int selection;
	}
}
