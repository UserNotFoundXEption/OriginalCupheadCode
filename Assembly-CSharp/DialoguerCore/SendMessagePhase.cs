using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x020005F3 RID: 1523
	public class SendMessagePhase : AbstractDialoguePhase
	{
		// Token: 0x06003EAC RID: 16044 RVA: 0x00032622 File Offset: 0x00030822
		public SendMessagePhase(string message, string metadata, List<int> outs) : base(outs)
		{
			this.message = message;
			this.metadata = metadata;
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x00032639 File Offset: 0x00030839
		public override void onStart()
		{
			DialoguerEventManager.dispatchOnMessageEvent(this.message, this.metadata);
			base.state = PhaseState.Complete;
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00032653 File Offset: 0x00030853
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Send Message Phase\nMessage: ",
				this.message,
				"\nMetadata: ",
				this.metadata,
				"\n"
			});
		}

		// Token: 0x04003280 RID: 12928
		public readonly string message;

		// Token: 0x04003281 RID: 12929
		public readonly string metadata;
	}
}
