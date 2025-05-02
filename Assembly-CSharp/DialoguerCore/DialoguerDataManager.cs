using System;
using System.IO;
using System.Xml.Serialization;
using DialoguerEditor;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x020005E6 RID: 1510
	public class DialoguerDataManager
	{
		// Token: 0x06003E78 RID: 15992 RVA: 0x0011C710 File Offset: 0x0011A910
		public static void Initialize()
		{
			DialogueEditorMasterObject data = (Resources.Load("dialoguer_data_object") as DialogueEditorMasterObjectWrapper).data;
			DialoguerDataManager._data = data.getDialoguerData();
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x0011C740 File Offset: 0x0011A940
		public static string GetGlobalVariablesState()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, DialoguerDataManager._data.globalVariables);
			return stringWriter.ToString();
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00032309 File Offset: 0x00030509
		public static void LoadGlobalVariablesState(string globalVariablesXml)
		{
			DialoguerDataManager._data.loadGlobalVariablesState(globalVariablesXml);
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00032316 File Offset: 0x00030516
		public static float GetGlobalFloat(int floatId)
		{
			return DialoguerDataManager._data.globalVariables.floats[floatId];
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0003232D File Offset: 0x0003052D
		public static void SetGlobalFloat(int floatId, float floatValue)
		{
			DialoguerDataManager._data.globalVariables.floats[floatId] = floatValue;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00032345 File Offset: 0x00030545
		public static bool GetGlobalBoolean(int booleanId)
		{
			return DialoguerDataManager._data.globalVariables.booleans[booleanId];
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0003235C File Offset: 0x0003055C
		public static void SetGlobalBoolean(int booleanId, bool booleanValue)
		{
			DialoguerDataManager._data.globalVariables.booleans[booleanId] = booleanValue;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00032374 File Offset: 0x00030574
		public static string GetGlobalString(int stringId)
		{
			return DialoguerDataManager._data.globalVariables.strings[stringId];
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0003238B File Offset: 0x0003058B
		public static void SetGlobalString(int stringId, string stringValue)
		{
			DialoguerDataManager._data.globalVariables.strings[stringId] = stringValue;
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x000323A3 File Offset: 0x000305A3
		public static DialoguerDialogue GetDialogueById(int dialogueId)
		{
			if (DialoguerDataManager._data.dialogues.Count <= dialogueId)
			{
				return null;
			}
			return DialoguerDataManager._data.dialogues[dialogueId];
		}

		// Token: 0x0400324C RID: 12876
		public static DialoguerData _data;
	}
}
