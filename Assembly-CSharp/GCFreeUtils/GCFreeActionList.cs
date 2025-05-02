using System;

namespace GCFreeUtils
{
	// Token: 0x020005B6 RID: 1462
	public class GCFreeActionList
	{
		// Token: 0x06003D63 RID: 15715 RVA: 0x00031870 File Offset: 0x0002FA70
		public GCFreeActionList(int size) : this(size, true)
		{
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x0003187A File Offset: 0x0002FA7A
		public GCFreeActionList(int size, bool autoResizeable)
		{
			this.actionList = new Action[size];
			this.autoResizeable = autoResizeable;
			this.Count = 0;
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x0003189C File Offset: 0x0002FA9C
		// (set) Token: 0x06003D66 RID: 15718 RVA: 0x000318A4 File Offset: 0x0002FAA4
		public int Count { get; set; }

		// Token: 0x06003D67 RID: 15719 RVA: 0x00118C74 File Offset: 0x00116E74
		public void Add(Action action)
		{
			if (this.Count == this.actionList.Length)
			{
				if (!this.autoResizeable)
				{
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", null);
					return;
				}
				Action[] destinationArray = new Action[this.actionList.Length * 2];
				Array.Copy(this.actionList, destinationArray, this.actionList.Length);
				this.actionList = destinationArray;
			}
			this.actionList[this.Count] = action;
			this.Count++;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00118CF8 File Offset: 0x00116EF8
		public void Remove(Action action)
		{
			if (this.Count > 0)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.actionList[i] == action)
					{
						if (this.Count > 1)
						{
							this.actionList[i] = this.actionList[this.Count - 1];
						}
						else
						{
							this.actionList[i] = null;
						}
						this.Count--;
						break;
					}
				}
			}
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00118D80 File Offset: 0x00116F80
		public void Call()
		{
			for (int i = 0; i < this.Count; i++)
			{
				try
				{
					if (this.actionList[i] != null)
					{
						this.actionList[i]();
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message, null);
				}
			}
		}

		// Token: 0x040030DB RID: 12507
		public const string ERR_BUFFER_TOO_SMALL = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.";

		// Token: 0x040030DC RID: 12508
		public const string LOG_RESIZING = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size.";

		// Token: 0x040030DE RID: 12510
		public Action[] actionList;

		// Token: 0x040030DF RID: 12511
		public bool autoResizeable;
	}
}
