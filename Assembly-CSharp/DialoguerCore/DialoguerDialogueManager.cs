using System;
using System.Runtime.CompilerServices;

namespace DialoguerCore
{
	// Token: 0x020005E4 RID: 1508
	public class DialoguerDialogueManager
	{
		// Token: 0x06003E56 RID: 15958 RVA: 0x00032176 File Offset: 0x00030376
		public static void startDialogueWithCallback(int dialogueId, DialoguerCallback callback)
		{
			DialoguerDialogueManager.onEndCallback = callback;
			DialoguerDialogueManager.startDialogue(dialogueId);
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x00032184 File Offset: 0x00030384
		public static void startDialogue(int dialogueId)
		{
			if (DialoguerDialogueManager.dialogue != null)
			{
				DialoguerEventManager.dispatchOnSuddenlyEnded();
			}
			DialoguerEventManager.dispatchOnStarted();
			DialoguerDialogueManager.dialogue = DialoguerDataManager.GetDialogueById(dialogueId);
			DialoguerDialogueManager.dialogue.Reset();
			DialoguerDialogueManager.setupPhase(DialoguerDialogueManager.dialogue.startPhaseId);
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x000321BE File Offset: 0x000303BE
		public static void continueDialogue(int outId)
		{
			if (DialoguerDialogueManager.currentPhase != null)
			{
				DialoguerDialogueManager.currentPhase.Continue(outId);
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x000321D5 File Offset: 0x000303D5
		public static void endDialogue()
		{
			if (DialoguerDialogueManager.dialogue == null)
			{
				return;
			}
			if (DialoguerDialogueManager.onEndCallback != null)
			{
				DialoguerDialogueManager.onEndCallback();
			}
			DialoguerEventManager.dispatchOnWindowClose();
			DialoguerEventManager.dispatchOnEnded();
			DialoguerDialogueManager.dialogue.Reset();
			DialoguerDialogueManager.reset();
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0011C320 File Offset: 0x0011A520
		public static void setupPhase(int nextPhaseId)
		{
			if (DialoguerDialogueManager.dialogue == null)
			{
				return;
			}
			AbstractDialoguePhase abstractDialoguePhase = DialoguerDialogueManager.dialogue.phases[nextPhaseId];
			if (abstractDialoguePhase is EndPhase)
			{
				DialoguerDialogueManager.endDialogue();
				return;
			}
			if (DialoguerDialogueManager.currentPhase != null)
			{
				DialoguerDialogueManager.currentPhase.resetEvents();
			}
			AbstractDialoguePhase abstractDialoguePhase2 = abstractDialoguePhase;
			if (DialoguerDialogueManager.<>f__mg$cache0 == null)
			{
				DialoguerDialogueManager.<>f__mg$cache0 = new AbstractDialoguePhase.PhaseCompleteHandler(DialoguerDialogueManager.phaseComplete);
			}
			abstractDialoguePhase2.onPhaseComplete += DialoguerDialogueManager.<>f__mg$cache0;
			if (abstractDialoguePhase is TextPhase || abstractDialoguePhase is BranchedTextPhase)
			{
				DialoguerEventManager.dispatchOnTextPhase((abstractDialoguePhase as TextPhase).data);
			}
			DialoguerDialogueManager.currentPhase = abstractDialoguePhase;
			abstractDialoguePhase.Start(DialoguerDialogueManager.dialogue.localVariables);
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0003220F File Offset: 0x0003040F
		public static void phaseComplete(int nextPhaseId)
		{
			DialoguerDialogueManager.setupPhase(nextPhaseId);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00032217 File Offset: 0x00030417
		public static bool isWindowed(AbstractDialoguePhase phase)
		{
			return phase is TextPhase || phase is BranchedTextPhase;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00032232 File Offset: 0x00030432
		public static void reset()
		{
			DialoguerDialogueManager.currentPhase = null;
			DialoguerDialogueManager.dialogue = null;
			DialoguerDialogueManager.onEndCallback = null;
		}

		// Token: 0x04003240 RID: 12864
		public static AbstractDialoguePhase currentPhase;

		// Token: 0x04003241 RID: 12865
		public static DialoguerDialogue dialogue;

		// Token: 0x04003242 RID: 12866
		public static DialoguerCallback onEndCallback;

		// Token: 0x04003243 RID: 12867
		[CompilerGenerated]
		private static AbstractDialoguePhase.PhaseCompleteHandler <>f__mg$cache0;
	}
}
