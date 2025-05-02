using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x020005D1 RID: 1489
	[Serializable]
	public class DialogueEditorPhaseObject
	{
		// Token: 0x06003DFE RID: 15870 RVA: 0x0011B7B4 File Offset: 0x001199B4
		public DialogueEditorPhaseObject()
		{
			this.type = DialogueEditorPhaseTypes.EmptyPhase;
			this.position = Vector2.zero;
			this.text = string.Empty;
			this.outs = new List<int>();
			this.choices = new List<string>();
			this.waitType = DialogueEditorWaitTypes.Seconds;
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00031E38 File Offset: 0x00030038
		public void addNewOut()
		{
			this.outs.Add(-1);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x00031E46 File Offset: 0x00030046
		public void removeOut()
		{
			this.outs.RemoveAt(this.outs.Count - 1);
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x00031E60 File Offset: 0x00030060
		public void addNewChoice()
		{
			this.addNewOut();
			this.choices.Add(string.Empty);
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x00031E78 File Offset: 0x00030078
		public void removeChoice()
		{
			this.removeOut();
			this.choices.RemoveAt(this.choices.Count - 1);
		}

		// Token: 0x040031DA RID: 12762
		public int id;

		// Token: 0x040031DB RID: 12763
		public DialogueEditorPhaseTypes type;

		// Token: 0x040031DC RID: 12764
		public object paramaters;

		// Token: 0x040031DD RID: 12765
		public string theme;

		// Token: 0x040031DE RID: 12766
		public Vector2 position;

		// Token: 0x040031DF RID: 12767
		public List<int> outs;

		// Token: 0x040031E0 RID: 12768
		public bool advanced;

		// Token: 0x040031E1 RID: 12769
		public string metadata;

		// Token: 0x040031E2 RID: 12770
		public string text;

		// Token: 0x040031E3 RID: 12771
		public string name;

		// Token: 0x040031E4 RID: 12772
		public string portrait;

		// Token: 0x040031E5 RID: 12773
		public string audio;

		// Token: 0x040031E6 RID: 12774
		public float audioDelay;

		// Token: 0x040031E7 RID: 12775
		public Rect rect;

		// Token: 0x040031E8 RID: 12776
		public bool newWindow;

		// Token: 0x040031E9 RID: 12777
		public List<string> choices;

		// Token: 0x040031EA RID: 12778
		public DialogueEditorWaitTypes waitType;

		// Token: 0x040031EB RID: 12779
		public float waitDuration;

		// Token: 0x040031EC RID: 12780
		public VariableEditorScopes variableScope;

		// Token: 0x040031ED RID: 12781
		public VariableEditorTypes variableType;

		// Token: 0x040031EE RID: 12782
		public int variableId;

		// Token: 0x040031EF RID: 12783
		public Vector2 variableScrollPosition;

		// Token: 0x040031F0 RID: 12784
		public VariableEditorSetEquation variableSetEquation;

		// Token: 0x040031F1 RID: 12785
		public string variableSetValue;

		// Token: 0x040031F2 RID: 12786
		public VariableEditorGetEquation variableGetEquation;

		// Token: 0x040031F3 RID: 12787
		public string variableGetValue;

		// Token: 0x040031F4 RID: 12788
		public string messageName;
	}
}
