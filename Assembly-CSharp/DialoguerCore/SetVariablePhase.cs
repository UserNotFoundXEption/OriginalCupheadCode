using System;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore
{
	// Token: 0x020005F4 RID: 1524
	public class SetVariablePhase : AbstractDialoguePhase
	{
		// Token: 0x06003EAF RID: 16047 RVA: 0x0003268A File Offset: 0x0003088A
		public SetVariablePhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorSetEquation equation, string setValue, List<int> outs) : base(outs)
		{
			this.scope = scope;
			this.type = type;
			this.variableId = variableId;
			this.equation = equation;
			this.setValue = setValue;
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0011D33C File Offset: 0x0011B53C
		public override void onStart()
		{
			bool flag = false;
			VariableEditorTypes variableEditorTypes = this.type;
			if (variableEditorTypes != VariableEditorTypes.Boolean)
			{
				if (variableEditorTypes != VariableEditorTypes.Float)
				{
					if (variableEditorTypes == VariableEditorTypes.String)
					{
						flag = true;
						this._setString = this.setValue;
						VariableEditorSetEquation variableEditorSetEquation = this.equation;
						if (variableEditorSetEquation != VariableEditorSetEquation.Equals)
						{
							if (variableEditorSetEquation == VariableEditorSetEquation.Add)
							{
								if (this.scope == VariableEditorScopes.Local)
								{
									List<string> strings;
									int index;
									(strings = this._localVariables.strings)[index = this.variableId] = strings[index] + this._setString;
								}
								else
								{
									Dialoguer.SetGlobalString(this.variableId, Dialoguer.GetGlobalString(this.variableId) + this._setString);
								}
							}
						}
						else if (this.scope == VariableEditorScopes.Local)
						{
							this._localVariables.strings[this.variableId] = this._setString;
						}
						else
						{
							Dialoguer.SetGlobalString(this.variableId, this._setString);
						}
					}
				}
				else
				{
					flag = Parser.FloatTryParse(this.setValue, out this._setFloat);
					switch (this.equation)
					{
					case VariableEditorSetEquation.Equals:
						if (this.scope == VariableEditorScopes.Local)
						{
							this._localVariables.floats[this.variableId] = this._setFloat;
						}
						else
						{
							Dialoguer.SetGlobalFloat(this.variableId, this._setFloat);
						}
						break;
					case VariableEditorSetEquation.Add:
						if (this.scope == VariableEditorScopes.Local)
						{
							List<float> floats;
							int index2;
							(floats = this._localVariables.floats)[index2 = this.variableId] = floats[index2] + this._setFloat;
						}
						else
						{
							Dialoguer.SetGlobalFloat(this.variableId, Dialoguer.GetGlobalFloat(this.variableId) + this._setFloat);
						}
						break;
					case VariableEditorSetEquation.Subtract:
						if (this.scope == VariableEditorScopes.Local)
						{
							List<float> floats;
							int index3;
							(floats = this._localVariables.floats)[index3 = this.variableId] = floats[index3] - this._setFloat;
						}
						else
						{
							Dialoguer.SetGlobalFloat(this.variableId, Dialoguer.GetGlobalFloat(this.variableId) - this._setFloat);
						}
						break;
					case VariableEditorSetEquation.Multiply:
						if (this.scope == VariableEditorScopes.Local)
						{
							List<float> floats;
							int index4;
							(floats = this._localVariables.floats)[index4 = this.variableId] = floats[index4] * this._setFloat;
						}
						else
						{
							Dialoguer.SetGlobalFloat(this.variableId, Dialoguer.GetGlobalFloat(this.variableId) * this._setFloat);
						}
						break;
					case VariableEditorSetEquation.Divide:
						if (this.scope == VariableEditorScopes.Local)
						{
							List<float> floats;
							int index5;
							(floats = this._localVariables.floats)[index5 = this.variableId] = floats[index5] / this._setFloat;
						}
						else
						{
							Dialoguer.SetGlobalFloat(this.variableId, Dialoguer.GetGlobalFloat(this.variableId) / this._setFloat);
						}
						break;
					}
				}
			}
			else
			{
				flag = bool.TryParse(this.setValue, out this._setBool);
				VariableEditorSetEquation variableEditorSetEquation2 = this.equation;
				if (variableEditorSetEquation2 != VariableEditorSetEquation.Equals)
				{
					if (variableEditorSetEquation2 == VariableEditorSetEquation.Toggle)
					{
						if (this.scope == VariableEditorScopes.Local)
						{
							this._localVariables.booleans[this.variableId] = !this._localVariables.booleans[this.variableId];
						}
						else
						{
							Dialoguer.SetGlobalBoolean(this.variableId, !Dialoguer.GetGlobalBoolean(this.variableId));
						}
						flag = true;
					}
				}
				else if (this.scope == VariableEditorScopes.Local)
				{
					this._localVariables.booleans[this.variableId] = this._setBool;
				}
				else
				{
					Dialoguer.SetGlobalBoolean(this.variableId, this._setBool);
				}
			}
			if (!flag)
			{
			}
			this.Continue(0);
			base.state = PhaseState.Complete;
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x0011D728 File Offset: 0x0011B928
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Set Variable Phase\nScope: ",
				this.scope.ToString(),
				"\nType: ",
				this.type.ToString(),
				"\nVariable ID: ",
				this.variableId,
				"\nEquation: ",
				this.equation.ToString(),
				"\nSet Value: ",
				this.setValue,
				"\nOut: ",
				this.outs[0],
				"\n"
			});
		}

		// Token: 0x04003282 RID: 12930
		public readonly VariableEditorScopes scope;

		// Token: 0x04003283 RID: 12931
		public readonly VariableEditorTypes type;

		// Token: 0x04003284 RID: 12932
		public readonly int variableId;

		// Token: 0x04003285 RID: 12933
		public readonly VariableEditorSetEquation equation;

		// Token: 0x04003286 RID: 12934
		public readonly string setValue;

		// Token: 0x04003287 RID: 12935
		public bool _setBool;

		// Token: 0x04003288 RID: 12936
		public float _setFloat;

		// Token: 0x04003289 RID: 12937
		public string _setString;
	}
}
