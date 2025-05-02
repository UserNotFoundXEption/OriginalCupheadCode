using System;

namespace DialoguerCore
{
	// Token: 0x020005F1 RID: 1521
	public class EmptyPhase : AbstractDialoguePhase
	{
		// Token: 0x06003EA8 RID: 16040 RVA: 0x00032602 File Offset: 0x00030802
		public EmptyPhase() : base(null)
		{
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x0003260B File Offset: 0x0003080B
		public override string ToString()
		{
			return "Empty Phase\nEmpty Phases should not be generated, something went wrong.\n";
		}
	}
}
