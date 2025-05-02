using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x020005F5 RID: 1525
	public class TextPhase : AbstractDialoguePhase
	{
		// Token: 0x06003EB2 RID: 16050 RVA: 0x0011D7EC File Offset: 0x0011B9EC
		public TextPhase(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, List<string> choices, int dialogueID, int nodeID) : base(outs)
		{
			this.data = new DialoguerTextData(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, choices, dialogueID, nodeID);
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x000326B9 File Offset: 0x000308B9
		public override void onStart()
		{
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0011D824 File Offset: 0x0011BA24
		public override void Continue(int nextPhaseId)
		{
			if (this.data.newWindow)
			{
				DialoguerEventManager.dispatchOnWindowClose();
			}
			base.Continue(nextPhaseId);
			base.state = PhaseState.Complete;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0011D858 File Offset: 0x0011BA58
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Text Phase",
				this.data.ToString(),
				"\nOut: ",
				this.outs[0],
				"\n"
			});
		}

		// Token: 0x0400328A RID: 12938
		public readonly DialoguerTextData data;
	}
}
