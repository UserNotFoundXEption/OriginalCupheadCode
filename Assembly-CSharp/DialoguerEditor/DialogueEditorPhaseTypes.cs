using System;

namespace DialoguerEditor
{
	// Token: 0x020005D2 RID: 1490
	public enum DialogueEditorPhaseTypes
	{
		// Token: 0x040031F6 RID: 12790
		TextPhase,
		// Token: 0x040031F7 RID: 12791
		BranchedTextPhase,
		// Token: 0x040031F8 RID: 12792
		WaitPhase,
		// Token: 0x040031F9 RID: 12793
		SetVariablePhase,
		// Token: 0x040031FA RID: 12794
		ConditionalPhase,
		// Token: 0x040031FB RID: 12795
		SendMessagePhase,
		// Token: 0x040031FC RID: 12796
		EndPhase,
		// Token: 0x040031FD RID: 12797
		EmptyPhase
	}
}
