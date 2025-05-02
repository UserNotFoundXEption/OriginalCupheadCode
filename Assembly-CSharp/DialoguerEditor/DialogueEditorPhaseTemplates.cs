using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005D4 RID: 1492
	public class DialogueEditorPhaseTemplates
	{
		// Token: 0x06003E08 RID: 15880 RVA: 0x0011B9BC File Offset: 0x00119BBC
		public static DialogueEditorPhaseObject newTextPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.TextPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				name = string.Empty,
				portrait = string.Empty,
				audio = string.Empty,
				audioDelay = 0f,
				rect = new Rect(0f, 0f, 0f, 0f),
				newWindow = false,
				outs = new List<int>(),
				outs = 
				{
					-1
				}
			};
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0011BA64 File Offset: 0x00119C64
		public static DialogueEditorPhaseObject newBranchedTextPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.BranchedTextPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				name = string.Empty,
				portrait = string.Empty,
				audio = string.Empty,
				audioDelay = 0f,
				rect = new Rect(0f, 0f, 0f, 0f),
				newWindow = false,
				outs = new List<int>(),
				outs = 
				{
					-1,
					-1
				},
				choices = new List<string>(),
				choices = 
				{
					string.Empty,
					string.Empty
				}
			};
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0011BB44 File Offset: 0x00119D44
		public static DialogueEditorPhaseObject newWaitPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.WaitPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				}
			};
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0011BB9C File Offset: 0x00119D9C
		public static DialogueEditorPhaseObject newSetVariablePhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.SetVariablePhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				},
				variableScope = VariableEditorScopes.Local,
				variableType = VariableEditorTypes.Boolean,
				variableSetEquation = VariableEditorSetEquation.Equals,
				variableScrollPosition = default(Vector2),
				variableId = 0,
				variableSetValue = string.Empty
			};
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0011BC28 File Offset: 0x00119E28
		public static DialogueEditorPhaseObject newConditionalPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.ConditionalPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1,
					-1
				}
			};
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0011BC8C File Offset: 0x00119E8C
		public static DialogueEditorPhaseObject newSendMessagePhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.SendMessagePhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				},
				messageName = string.Empty
			};
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0011BCF0 File Offset: 0x00119EF0
		public static DialogueEditorPhaseObject newEndPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.EndPhase,
				position = Vector2.zero
			};
		}
	}
}
