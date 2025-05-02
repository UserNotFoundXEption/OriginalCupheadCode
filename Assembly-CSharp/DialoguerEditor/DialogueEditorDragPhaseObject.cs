using System;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005CD RID: 1485
	public class DialogueEditorDragPhaseObject
	{
		// Token: 0x06003DF2 RID: 15858 RVA: 0x00031DBC File Offset: 0x0002FFBC
		public DialogueEditorDragPhaseObject(int phaseId, Vector2 mouseOffset)
		{
			this.phaseId = phaseId;
			this.mouseOffset = mouseOffset;
		}

		// Token: 0x040031CE RID: 12750
		public int phaseId;

		// Token: 0x040031CF RID: 12751
		public Vector2 mouseOffset;
	}
}
