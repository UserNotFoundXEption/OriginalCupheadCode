using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x020005EF RID: 1519
	public class BranchedTextPhase : TextPhase
	{
		// Token: 0x06003EA3 RID: 16035 RVA: 0x0011CE74 File Offset: 0x0011B074
		public BranchedTextPhase(string text, List<string> choices, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, int dialogueID, int nodeID) : base(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, outs, choices, dialogueID, nodeID)
		{
			this.choices = choices;
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0011CEA8 File Offset: 0x0011B0A8
		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < this.choices.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					i,
					": ",
					this.choices[i],
					" : Out ",
					this.outs[i],
					"\n"
				});
			}
			return "Branched Text Phase" + this.data.ToString() + "\n" + text;
		}

		// Token: 0x04003274 RID: 12916
		public readonly List<string> choices;
	}
}
