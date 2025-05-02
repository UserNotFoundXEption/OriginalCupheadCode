using System;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore
{
	// Token: 0x020005FB RID: 1531
	public class DialoguerUtils
	{
		// Token: 0x06003EC1 RID: 16065 RVA: 0x0011DAD0 File Offset: 0x0011BCD0
		public static string insertTextPhaseStringVariables(string input)
		{
			int dialogueId = 0;
			string input2 = DialoguerUtils.substituteStringVariable(input, VariableEditorScopes.Global, VariableEditorTypes.Boolean, dialogueId);
			input2 = DialoguerUtils.substituteStringVariable(input2, VariableEditorScopes.Global, VariableEditorTypes.Float, dialogueId);
			return DialoguerUtils.substituteStringVariable(input2, VariableEditorScopes.Global, VariableEditorTypes.String, dialogueId);
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0011DB00 File Offset: 0x0011BD00
		public static string substituteStringVariable(string input, VariableEditorScopes scope, VariableEditorTypes type, int dialogueId)
		{
			if (input == null)
			{
				return input;
			}
			string text = string.Empty;
			string[] separator = new string[]
			{
				"<" + DialoguerUtils.scopeStrings[scope] + DialoguerUtils.typeStrings[type] + ">"
			};
			string[] separator2 = new string[]
			{
				"</" + DialoguerUtils.scopeStrings[scope] + DialoguerUtils.typeStrings[type] + ">"
			};
			string[] array = input.Split(separator, StringSplitOptions.None);
			if (array.Length < 2)
			{
				return input;
			}
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(separator2, StringSplitOptions.None);
				int num;
				bool flag = int.TryParse(array2[0], out num);
				if (flag)
				{
					if (scope != VariableEditorScopes.Global)
					{
						if (scope == VariableEditorScopes.Local)
						{
							if (type != VariableEditorTypes.Boolean)
							{
								if (type != VariableEditorTypes.Float)
								{
									if (type != VariableEditorTypes.String)
									{
									}
								}
							}
						}
					}
					else if (type != VariableEditorTypes.Boolean)
					{
						if (type != VariableEditorTypes.Float)
						{
							if (type == VariableEditorTypes.String)
							{
								array2[0] = Dialoguer.GetGlobalString(num);
							}
						}
						else
						{
							array2[0] = Dialoguer.GetGlobalFloat(num).ToString();
						}
					}
					else
					{
						array2[0] = Dialoguer.GetGlobalBoolean(num).ToString();
					}
				}
				text += string.Join(string.Empty, array2);
			}
			return text;
		}

		// Token: 0x0400329A RID: 12954
		public static Dictionary<VariableEditorScopes, string> scopeStrings = new Dictionary<VariableEditorScopes, string>
		{
			{
				VariableEditorScopes.Global,
				"g"
			},
			{
				VariableEditorScopes.Local,
				"l"
			}
		};

		// Token: 0x0400329B RID: 12955
		public static Dictionary<VariableEditorTypes, string> typeStrings = new Dictionary<VariableEditorTypes, string>
		{
			{
				VariableEditorTypes.Boolean,
				"b"
			},
			{
				VariableEditorTypes.Float,
				"f"
			},
			{
				VariableEditorTypes.String,
				"s"
			}
		};
	}
}
