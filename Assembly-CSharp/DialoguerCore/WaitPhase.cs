using System;
using System.Collections.Generic;
using DialoguerEditor;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x020005F6 RID: 1526
	public class WaitPhase : AbstractDialoguePhase
	{
		// Token: 0x06003EB6 RID: 16054 RVA: 0x000326BB File Offset: 0x000308BB
		public WaitPhase(DialogueEditorWaitTypes type, float duration, List<int> outs) : base(outs)
		{
			this.type = type;
			this.duration = duration;
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0011D8B0 File Offset: 0x0011BAB0
		public override void onStart()
		{
			DialoguerEventManager.dispatchOnWaitStart();
			if (this.type == DialogueEditorWaitTypes.Continue)
			{
				return;
			}
			GameObject gameObject = new GameObject("Dialoguer WaitPhaseTimer");
			WaitPhaseComponent waitPhaseComponent = gameObject.AddComponent<WaitPhaseComponent>();
			waitPhaseComponent.Init(this, this.type, this.duration);
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x000326D2 File Offset: 0x000308D2
		public void waitComplete()
		{
			DialoguerEventManager.dispatchOnWaitComplete();
			base.state = PhaseState.Complete;
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x000326E0 File Offset: 0x000308E0
		public override void Continue(int outId)
		{
			if (this.type != DialogueEditorWaitTypes.Continue)
			{
				return;
			}
			DialoguerEventManager.dispatchOnWaitComplete();
			base.Continue(outId);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0011D8F4 File Offset: 0x0011BAF4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Wait Phase\nType: ",
				this.type.ToString(),
				"\nDuration: ",
				this.duration,
				"\n"
			});
		}

		// Token: 0x0400328B RID: 12939
		public readonly DialogueEditorWaitTypes type;

		// Token: 0x0400328C RID: 12940
		public readonly float duration;
	}
}
