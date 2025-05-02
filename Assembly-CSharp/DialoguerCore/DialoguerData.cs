using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DialoguerCore
{
	// Token: 0x020005E8 RID: 1512
	public class DialoguerData
	{
		// Token: 0x06003E86 RID: 16006 RVA: 0x00032423 File Offset: 0x00030623
		public DialoguerData(DialoguerGlobalVariables globalVariables, List<DialoguerDialogue> dialogues, List<DialoguerTheme> themes)
		{
			this.globalVariables = globalVariables;
			this.dialogues = dialogues;
			this.themes = themes;
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0011C80C File Offset: 0x0011AA0C
		public void loadGlobalVariablesState(string globalVariablesXml)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			XmlReader xmlReader = XmlReader.Create(new StringReader(globalVariablesXml));
			DialoguerGlobalVariables dialoguerGlobalVariables = (DialoguerGlobalVariables)xmlSerializer.Deserialize(xmlReader);
			for (int i = 0; i < dialoguerGlobalVariables.booleans.Count; i++)
			{
				if (i >= this.globalVariables.booleans.Count)
				{
					break;
				}
				this.globalVariables.booleans[i] = dialoguerGlobalVariables.booleans[i];
			}
			for (int j = 0; j < dialoguerGlobalVariables.floats.Count; j++)
			{
				if (j >= this.globalVariables.floats.Count)
				{
					break;
				}
				this.globalVariables.floats[j] = dialoguerGlobalVariables.floats[j];
			}
			for (int k = 0; k < dialoguerGlobalVariables.strings.Count; k++)
			{
				if (k >= this.globalVariables.strings.Count)
				{
					break;
				}
				this.globalVariables.strings[k] = dialoguerGlobalVariables.strings[k];
			}
		}

		// Token: 0x04003252 RID: 12882
		public readonly DialoguerGlobalVariables globalVariables;

		// Token: 0x04003253 RID: 12883
		public readonly List<DialoguerDialogue> dialogues;

		// Token: 0x04003254 RID: 12884
		public readonly List<DialoguerTheme> themes;
	}
}
