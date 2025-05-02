using System;
using System.Collections.Generic;
using DialoguerCore;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005CF RID: 1487
	[Serializable]
	public class DialogueEditorMasterObject
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0011B0D0 File Offset: 0x001192D0
		public DialogueEditorMasterObject()
		{
			this.dialogues = new List<DialogueEditorDialogueObject>();
			this.globals = new DialogueEditorGlobalVariablesContainer();
			this.themes = new DialogueEditorThemesContainer();
			this.selectorScrollPosition = Vector2.zero;
			this.currentDialogueId = -1;
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x00031DFB File Offset: 0x0002FFFB
		public int count
		{
			get
			{
				return this.dialogues.Count;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x00031E08 File Offset: 0x00030008
		// (set) Token: 0x06003DF7 RID: 15863 RVA: 0x00031E10 File Offset: 0x00030010
		public int currentDialogueId
		{
			get
			{
				return this.__currentDialogueId;
			}
			set
			{
				this.__currentDialogueId = Mathf.Clamp(value, 0, this.count - 1);
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x0011B120 File Offset: 0x00119320
		public void addDialogue(int count)
		{
			for (int i = 0; i < count; i++)
			{
				int count2 = this.dialogues.Count;
				this.dialogues.Add(new DialogueEditorDialogueObject());
				this.dialogues[count2].id = count2;
				this.currentDialogueId = this.dialogues[count2].id;
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0011B184 File Offset: 0x00119384
		public void removeDialogue(int removeCount)
		{
			if (this.count < 1)
			{
				return;
			}
			for (int i = 0; i < removeCount; i++)
			{
				int index = this.dialogues.Count - 1;
				this.dialogues.RemoveAt(index);
			}
			this.currentDialogueId = this.currentDialogueId;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00031E27 File Offset: 0x00030027
		public string[] getThemeNames()
		{
			return this.getThemeNames(false);
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0011B1D8 File Offset: 0x001193D8
		public string[] getThemeNames(bool includeId)
		{
			string[] array = new string[this.themes.themes.Count];
			for (int i = 0; i < this.themes.themes.Count; i++)
			{
				array[i] = string.Empty;
				string[] array2;
				if (includeId)
				{
					int num;
					(array2 = array)[num = i] = array2[num] + this.themes.themes[i].id + " ";
				}
				int num2;
				(array2 = array)[num2 = i] = array2[num2] + this.themes.themes[i].name;
			}
			return array;
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0011B284 File Offset: 0x00119484
		public DialoguerData getDialoguerData()
		{
			List<bool> list = new List<bool>();
			List<float> list2 = new List<float>();
			List<string> list3 = new List<string>();
			for (int i = 0; i < this.globals.booleans.variables.Count; i++)
			{
				bool item;
				if (!bool.TryParse(this.globals.booleans.variables[i].variable, out item))
				{
				}
				list.Add(item);
			}
			for (int j = 0; j < this.globals.floats.variables.Count; j++)
			{
				float item2;
				if (!float.TryParse(this.globals.floats.variables[j].variable, out item2))
				{
				}
				list2.Add(item2);
			}
			for (int k = 0; k < this.globals.strings.variables.Count; k++)
			{
				list3.Add(this.globals.strings.variables[k].variable);
			}
			DialoguerGlobalVariables globalVariables = new DialoguerGlobalVariables(list, list2, list3);
			List<DialoguerDialogue> list4 = new List<DialoguerDialogue>();
			for (int l = 0; l < this.dialogues.Count; l++)
			{
				DialogueEditorDialogueObject dialogueEditorDialogueObject = this.dialogues[l];
				List<AbstractDialoguePhase> list5 = new List<AbstractDialoguePhase>();
				for (int m = 0; m < dialogueEditorDialogueObject.phases.Count; m++)
				{
					DialogueEditorPhaseObject dialogueEditorPhaseObject = dialogueEditorDialogueObject.phases[m];
					switch (dialogueEditorPhaseObject.type)
					{
					case DialogueEditorPhaseTypes.TextPhase:
						list5.Add(new TextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, null, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id));
						break;
					case DialogueEditorPhaseTypes.BranchedTextPhase:
						list5.Add(new BranchedTextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.choices, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id));
						break;
					case DialogueEditorPhaseTypes.WaitPhase:
						list5.Add(new WaitPhase(dialogueEditorPhaseObject.waitType, dialogueEditorPhaseObject.waitDuration, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.SetVariablePhase:
						list5.Add(new SetVariablePhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableSetEquation, dialogueEditorPhaseObject.variableSetValue, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.ConditionalPhase:
						list5.Add(new ConditionalPhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableGetEquation, dialogueEditorPhaseObject.variableGetValue, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.SendMessagePhase:
						list5.Add(new SendMessagePhase(dialogueEditorPhaseObject.messageName, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.EndPhase:
						list5.Add(new EndPhase());
						break;
					default:
						list5.Add(new EmptyPhase());
						break;
					}
				}
				List<bool> list6 = new List<bool>();
				for (int n = 0; n < dialogueEditorDialogueObject.booleans.variables.Count; n++)
				{
					bool item3;
					if (!bool.TryParse(dialogueEditorDialogueObject.booleans.variables[n].variable, out item3))
					{
					}
					list6.Add(item3);
				}
				List<float> list7 = new List<float>();
				for (int num = 0; num < dialogueEditorDialogueObject.floats.variables.Count; num++)
				{
					float item4;
					if (!float.TryParse(dialogueEditorDialogueObject.floats.variables[num].variable, out item4))
					{
					}
					list7.Add(item4);
				}
				List<string> list8 = new List<string>();
				for (int num2 = 0; num2 < dialogueEditorDialogueObject.strings.variables.Count; num2++)
				{
					list8.Add(dialogueEditorDialogueObject.strings.variables[num2].variable);
				}
				DialoguerVariables localVariables = new DialoguerVariables(list6, list7, list8);
				DialoguerDialogue item5 = new DialoguerDialogue(dialogueEditorDialogueObject.name, dialogueEditorDialogueObject.startPage, localVariables, list5);
				list4.Add(item5);
			}
			List<DialoguerTheme> list9 = new List<DialoguerTheme>();
			for (int num3 = 0; num3 < this.themes.themes.Count; num3++)
			{
				list9.Add(new DialoguerTheme(this.themes.themes[num3].name, this.themes.themes[num3].linkage));
			}
			return new DialoguerData(globalVariables, list4, list9);
		}

		// Token: 0x040031D3 RID: 12755
		public int __currentDialogueId;

		// Token: 0x040031D4 RID: 12756
		public bool generateEnum = true;

		// Token: 0x040031D5 RID: 12757
		public List<DialogueEditorDialogueObject> dialogues;

		// Token: 0x040031D6 RID: 12758
		public DialogueEditorGlobalVariablesContainer globals;

		// Token: 0x040031D7 RID: 12759
		public DialogueEditorThemesContainer themes;

		// Token: 0x040031D8 RID: 12760
		public Vector2 selectorScrollPosition;
	}
}
