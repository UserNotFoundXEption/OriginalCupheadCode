using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005D3 RID: 1491
	public class DialogueEditorPhaseType
	{
		// Token: 0x06003E03 RID: 15875 RVA: 0x00031E98 File Offset: 0x00030098
		public DialogueEditorPhaseType(DialogueEditorPhaseTypes type, string name, string info, Texture iconDark, Texture iconLight)
		{
			this.type = type;
			this.name = name;
			this.info = info;
			this.iconDark = iconDark;
			this.iconLight = iconLight;
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0011B804 File Offset: 0x00119A04
		public static Dictionary<int, DialogueEditorPhaseType> getPhases()
		{
			Dictionary<int, DialogueEditorPhaseType> dictionary = new Dictionary<int, DialogueEditorPhaseType>();
			DialogueEditorPhaseType value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.TextPhase, "Text", "A simple text page with one out-path.", DialogueEditorPhaseType.getDarkIcon("textPhase"), DialogueEditorPhaseType.getLightIcon("textPhase"));
			dictionary.Add(0, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.BranchedTextPhase, "Branched Text", "A text page with multiple, selectable out-paths.", DialogueEditorPhaseType.getDarkIcon("branchedTextPhase"), DialogueEditorPhaseType.getLightIcon("branchedTextPhase"));
			dictionary.Add(1, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.WaitPhase, "Wait", "Wait X seconds before progressing.", DialogueEditorPhaseType.getDarkIcon("waitPhase"), DialogueEditorPhaseType.getLightIcon("waitPhase"));
			dictionary.Add(2, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.SetVariablePhase, "Set Variable", "Set a local or global variable.", DialogueEditorPhaseType.getDarkIcon("setVariablePhase"), DialogueEditorPhaseType.getLightIcon("setVariablePhase"));
			dictionary.Add(3, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.ConditionalPhase, "Condition", "Moves to an out-path based on a condition.", DialogueEditorPhaseType.getDarkIcon("conditionalPhase"), DialogueEditorPhaseType.getLightIcon("conditionalPhase"));
			dictionary.Add(4, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.SendMessagePhase, "Message Event", "Dispatch an event which can be easily listened to and handled.", DialogueEditorPhaseType.getDarkIcon("sendMessagePhase"), DialogueEditorPhaseType.getLightIcon("sendMessagePhase"));
			dictionary.Add(5, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.EndPhase, "End", "Ends the dialogue and calls the dialogue's callback.", DialogueEditorPhaseType.getDarkIcon("endPhase"), DialogueEditorPhaseType.getLightIcon("endPhase"));
			dictionary.Add(6, value);
			return dictionary;
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0011B954 File Offset: 0x00119B54
		public static Texture getDarkIcon(string icon)
		{
			string str = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			str += "Dark/";
			str = str + "icon_" + icon + ".png";
			return null;
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0011B988 File Offset: 0x00119B88
		public static Texture getLightIcon(string icon)
		{
			string str = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			str += "Light/";
			str = str + "icon_" + icon + ".png";
			return null;
		}

		// Token: 0x040031FE RID: 12798
		public DialogueEditorPhaseTypes type;

		// Token: 0x040031FF RID: 12799
		public string name;

		// Token: 0x04003200 RID: 12800
		public string info;

		// Token: 0x04003201 RID: 12801
		public Texture iconDark;

		// Token: 0x04003202 RID: 12802
		public Texture iconLight;
	}
}
