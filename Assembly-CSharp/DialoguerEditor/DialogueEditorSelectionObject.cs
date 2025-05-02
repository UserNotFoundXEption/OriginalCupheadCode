using System;

namespace DialoguerEditor
{
	// Token: 0x020005D5 RID: 1493
	public class DialogueEditorSelectionObject
	{
		// Token: 0x06003E0F RID: 15887 RVA: 0x00031ECD File Offset: 0x000300CD
		public DialogueEditorSelectionObject(int phaseId, int outputIndex)
		{
			if (phaseId < 0)
			{
				phaseId = 0;
			}
			if (outputIndex < 0)
			{
				outputIndex = 0;
			}
			this.phaseId = phaseId;
			this.outputIndex = outputIndex;
			this.isStart = false;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00031EFE File Offset: 0x000300FE
		public DialogueEditorSelectionObject(bool isStart)
		{
			this.isStart = true;
			this.phaseId = int.MinValue;
			this.outputIndex = int.MinValue;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x00031F23 File Offset: 0x00030123
		// (set) Token: 0x06003E12 RID: 15890 RVA: 0x00031F2B File Offset: 0x0003012B
		public int phaseId { get; set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x00031F34 File Offset: 0x00030134
		// (set) Token: 0x06003E14 RID: 15892 RVA: 0x00031F3C File Offset: 0x0003013C
		public int outputIndex { get; set; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x00031F45 File Offset: 0x00030145
		// (set) Token: 0x06003E16 RID: 15894 RVA: 0x00031F4D File Offset: 0x0003014D
		public bool isStart { get; set; }
	}
}
