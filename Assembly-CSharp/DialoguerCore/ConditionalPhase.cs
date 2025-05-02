using System;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore
{
	// Token: 0x020005F0 RID: 1520
	public class ConditionalPhase : AbstractDialoguePhase
	{
		// Token: 0x06003EA5 RID: 16037 RVA: 0x000325D3 File Offset: 0x000307D3
		public ConditionalPhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorGetEquation equation, string getValue, List<int> outs) : base(outs)
		{
			this.scope = scope;
			this.type = type;
			this.variableId = variableId;
			this.equation = equation;
			this.getValue = getValue;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x0011CF50 File Offset: 0x0011B150
		public override void onStart()
		{
			VariableEditorTypes variableEditorTypes = this.type;
			if (variableEditorTypes != VariableEditorTypes.Boolean)
			{
				if (variableEditorTypes != VariableEditorTypes.Float)
				{
					if (variableEditorTypes == VariableEditorTypes.String)
					{
						this._parsedString = this.getValue;
						if (this.scope == VariableEditorScopes.Local)
						{
							this._checkString = this._localVariables.strings[this.variableId];
						}
						else
						{
							this._checkString = Dialoguer.GetGlobalString(this.variableId);
						}
					}
				}
				else
				{
					if (!Parser.FloatTryParse(this.getValue, out this._parsedFloat))
					{
						Debug.LogError("[ConditionalPhase] Could Not Parse Float: " + this.getValue, null);
					}
					if (this.scope == VariableEditorScopes.Local)
					{
						this._checkFloat = this._localVariables.floats[this.variableId];
					}
					else
					{
						this._checkFloat = Dialoguer.GetGlobalFloat(this.variableId);
					}
				}
			}
			else
			{
				if (!bool.TryParse(this.getValue, out this._parsedBool))
				{
					Debug.LogError("[ConditionalPhase] Could Not Parse Bool: " + this.getValue, null);
				}
				if (this.scope == VariableEditorScopes.Local)
				{
					this._checkBool = this._localVariables.booleans[this.variableId];
				}
				else
				{
					this._checkBool = Dialoguer.GetGlobalBoolean(this.variableId);
				}
			}
			bool flag = false;
			VariableEditorTypes variableEditorTypes2 = this.type;
			if (variableEditorTypes2 != VariableEditorTypes.Boolean)
			{
				if (variableEditorTypes2 != VariableEditorTypes.Float)
				{
					if (variableEditorTypes2 == VariableEditorTypes.String)
					{
						VariableEditorGetEquation variableEditorGetEquation = this.equation;
						if (variableEditorGetEquation != VariableEditorGetEquation.Equals)
						{
							if (variableEditorGetEquation == VariableEditorGetEquation.NotEquals)
							{
								if (this._parsedString != this._checkString)
								{
									flag = true;
								}
							}
						}
						else if (this._parsedString == this._checkString)
						{
							flag = true;
						}
					}
				}
				else
				{
					switch (this.equation)
					{
					case VariableEditorGetEquation.Equals:
						if (this._checkFloat == this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.NotEquals:
						if (this._checkFloat != this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.GreaterThan:
						if (this._checkFloat > this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.LessThan:
						if (this._checkFloat < this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.EqualOrGreaterThan:
						if (this._checkFloat >= this._parsedFloat)
						{
							flag = true;
						}
						break;
					case VariableEditorGetEquation.EqualOrLessThan:
						if (this._checkFloat <= this._parsedFloat)
						{
							flag = true;
						}
						break;
					}
				}
			}
			else
			{
				VariableEditorGetEquation variableEditorGetEquation2 = this.equation;
				if (variableEditorGetEquation2 != VariableEditorGetEquation.Equals)
				{
					if (variableEditorGetEquation2 == VariableEditorGetEquation.NotEquals)
					{
						if (this._parsedBool != this._checkBool)
						{
							flag = true;
						}
					}
				}
				else if (this._parsedBool == this._checkBool)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.Continue(0);
			}
			else
			{
				this.Continue(1);
			}
			base.state = PhaseState.Complete;
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0011D260 File Offset: 0x0011B460
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
				"\nGet Value: ",
				this.getValue,
				"\nTrue Out: ",
				this.outs[0],
				"\nFalse Out: ",
				this.outs[1],
				"\n"
			});
		}

		// Token: 0x04003275 RID: 12917
		public readonly VariableEditorScopes scope;

		// Token: 0x04003276 RID: 12918
		public readonly VariableEditorTypes type;

		// Token: 0x04003277 RID: 12919
		public readonly int variableId;

		// Token: 0x04003278 RID: 12920
		public readonly VariableEditorGetEquation equation;

		// Token: 0x04003279 RID: 12921
		public readonly string getValue;

		// Token: 0x0400327A RID: 12922
		public bool _parsedBool;

		// Token: 0x0400327B RID: 12923
		public bool _checkBool;

		// Token: 0x0400327C RID: 12924
		public float _parsedFloat;

		// Token: 0x0400327D RID: 12925
		public float _checkFloat;

		// Token: 0x0400327E RID: 12926
		public string _parsedString;

		// Token: 0x0400327F RID: 12927
		public string _checkString;
	}
}
