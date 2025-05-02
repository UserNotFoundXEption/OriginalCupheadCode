using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x020005E9 RID: 1513
	public class DialoguerDialogue
	{
		// Token: 0x06003E88 RID: 16008 RVA: 0x00032440 File Offset: 0x00030640
		public DialoguerDialogue(string name, int startPhaseId, DialoguerVariables localVariables, List<AbstractDialoguePhase> phases)
		{
			this.name = name;
			this.startPhaseId = startPhaseId;
			this.phases = phases;
			this._originalLocalVariables = localVariables;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00032465 File Offset: 0x00030665
		public void Reset()
		{
			this.localVariables = this._originalLocalVariables.Clone();
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0011C94C File Offset: 0x0011AB4C
		public override string ToString()
		{
			string text = "Dialogue: " + this.name + "\n-";
			text = text + "\nLocal Booleans: " + this._originalLocalVariables.booleans.Count;
			text = text + "\nLocal Floats: " + this._originalLocalVariables.floats.Count;
			text = text + "\nLocal Strings: " + this._originalLocalVariables.strings.Count;
			text += "\n";
			for (int i = 0; i < this.phases.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\nPhase ",
					i,
					": ",
					this.phases[i].ToString()
				});
			}
			return text;
		}

		// Token: 0x04003255 RID: 12885
		public readonly string name;

		// Token: 0x04003256 RID: 12886
		public readonly int startPhaseId;

		// Token: 0x04003257 RID: 12887
		public readonly List<AbstractDialoguePhase> phases;

		// Token: 0x04003258 RID: 12888
		public readonly DialoguerVariables _originalLocalVariables;

		// Token: 0x04003259 RID: 12889
		public DialoguerVariables localVariables;
	}
}
