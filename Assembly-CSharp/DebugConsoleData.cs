using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class DebugConsoleData : ScriptableObject
{
	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x00008F92 File Offset: 0x00007192
	public DebugConsoleData.Command Current
	{
		get
		{
			this.CleanIndex();
			return this.commands[this.index];
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00008FAB File Offset: 0x000071AB
	public void PrepareForSave()
	{
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00008FAD File Offset: 0x000071AD
	public void CleanIndex()
	{
		if (this.index >= this.commands.Count)
		{
			this.index = this.commands.Count - 1;
		}
	}

	// Token: 0x04000741 RID: 1857
	public static string PATH = "TC_DebugConsole/tc_debug_console_data";

	// Token: 0x04000742 RID: 1858
	public int index;

	// Token: 0x04000743 RID: 1859
	public List<DebugConsoleData.Command> commands = new List<DebugConsoleData.Command>();

	// Token: 0x02000934 RID: 2356
	[Serializable]
	public class Command
	{
		// Token: 0x0400456C RID: 17772
		public string command = "new.command";

		// Token: 0x0400456D RID: 17773
		public KeyCode key;

		// Token: 0x0400456E RID: 17774
		public string rewiredAction = string.Empty;

		// Token: 0x0400456F RID: 17775
		public List<DebugConsoleData.Command.Argument> arguments = new List<DebugConsoleData.Command.Argument>();

		// Token: 0x04004570 RID: 17776
		public string help = string.Empty;

		// Token: 0x04004571 RID: 17777
		public string code = string.Empty;

		// Token: 0x04004572 RID: 17778
		public bool closeConsole;

		// Token: 0x020015D2 RID: 5586
		[Serializable]
		public class Argument
		{
			// Token: 0x040091AD RID: 37293
			public DebugConsoleData.Command.Argument.Type type;

			// Token: 0x040091AE RID: 37294
			public string name = "argName";

			// Token: 0x02001608 RID: 5640
			public enum Type
			{
				// Token: 0x04009291 RID: 37521
				Int,
				// Token: 0x04009292 RID: 37522
				Float,
				// Token: 0x04009293 RID: 37523
				Bool,
				// Token: 0x04009294 RID: 37524
				String
			}
		}
	}
}
