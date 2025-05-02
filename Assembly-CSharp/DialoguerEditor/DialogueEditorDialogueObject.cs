using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005CC RID: 1484
	[Serializable]
	public class DialogueEditorDialogueObject
	{
		// Token: 0x06003DEF RID: 15855 RVA: 0x0011AE40 File Offset: 0x00119040
		public DialogueEditorDialogueObject()
		{
			this.name = string.Empty;
			this.phases = new List<DialogueEditorPhaseObject>();
			this.floats = new DialogueEditorVariablesContainer();
			this.strings = new DialogueEditorVariablesContainer();
			this.booleans = new DialogueEditorVariablesContainer();
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0011AE94 File Offset: 0x00119094
		public void addPhase(DialogueEditorPhaseTypes phaseType, Vector2 newPhasePosition)
		{
			switch (phaseType)
			{
			case DialogueEditorPhaseTypes.TextPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newTextPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.BranchedTextPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newBranchedTextPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.WaitPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newWaitPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.SetVariablePhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newSetVariablePhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.ConditionalPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newConditionalPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.SendMessagePhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newSendMessagePhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.EndPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newEndPhase(this.phases.Count));
				break;
			}
			this.phases[this.phases.Count - 1].position = newPhasePosition;
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0011AFC8 File Offset: 0x001191C8
		public void removePhase(int phaseId)
		{
			for (int i = 0; i < this.phases.Count; i++)
			{
				DialogueEditorPhaseObject dialogueEditorPhaseObject = this.phases[i];
				for (int j = 0; j < dialogueEditorPhaseObject.outs.Count; j++)
				{
					if (dialogueEditorPhaseObject.outs[j] >= 0 && dialogueEditorPhaseObject.outs[j] > phaseId)
					{
						List<int> outs;
						int index;
						(outs = dialogueEditorPhaseObject.outs)[index = j] = outs[index] - 1;
					}
					else if (dialogueEditorPhaseObject.outs[j] >= 0 && dialogueEditorPhaseObject.outs[j] == phaseId)
					{
						dialogueEditorPhaseObject.outs[j] = -1;
					}
				}
				if (this.startPage >= 0 && this.startPage == phaseId)
				{
					this.startPage = -1;
				}
				if (i > phaseId)
				{
					dialogueEditorPhaseObject.id--;
				}
			}
			this.phases.RemoveAt(phaseId);
		}

		// Token: 0x040031C6 RID: 12742
		public int id;

		// Token: 0x040031C7 RID: 12743
		public string name;

		// Token: 0x040031C8 RID: 12744
		public int startPage = -1;

		// Token: 0x040031C9 RID: 12745
		public Vector2 scrollPosition;

		// Token: 0x040031CA RID: 12746
		public List<DialogueEditorPhaseObject> phases;

		// Token: 0x040031CB RID: 12747
		public DialogueEditorVariablesContainer floats;

		// Token: 0x040031CC RID: 12748
		public DialogueEditorVariablesContainer strings;

		// Token: 0x040031CD RID: 12749
		public DialogueEditorVariablesContainer booleans;
	}
}
