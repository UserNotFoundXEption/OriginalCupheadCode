using System;

namespace GCFreeUtils
{
	// Token: 0x020005B7 RID: 1463
	public class GCFreePredicateList<T>
	{
		// Token: 0x06003D6A RID: 15722 RVA: 0x000318AD File Offset: 0x0002FAAD
		public GCFreePredicateList(int size) : this(size, true)
		{
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x000318B7 File Offset: 0x0002FAB7
		public GCFreePredicateList(int size, bool autoResizeable)
		{
			this.actionList = new Predicate<T>[size];
			this.autoResizeable = autoResizeable;
			this.Count = 0;
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000318D9 File Offset: 0x0002FAD9
		// (set) Token: 0x06003D6D RID: 15725 RVA: 0x000318E1 File Offset: 0x0002FAE1
		public int Count { get; set; }

		// Token: 0x06003D6E RID: 15726 RVA: 0x00118DE0 File Offset: 0x00116FE0
		public void Add(Predicate<T> action)
		{
			if (this.Count == this.actionList.Length)
			{
				if (!this.autoResizeable)
				{
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", null);
					return;
				}
				Predicate<T>[] destinationArray = new Predicate<T>[this.actionList.Length * 2];
				Array.Copy(this.actionList, destinationArray, this.actionList.Length);
				this.actionList = destinationArray;
			}
			this.actionList[this.Count] = action;
			this.Count++;
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00118E64 File Offset: 0x00117064
		public void Remove(Predicate<T> action)
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

		// Token: 0x06003D70 RID: 15728 RVA: 0x00118EEC File Offset: 0x001170EC
		public bool CallAnyTrue(T parameter)
		{
			for (int i = 0; i < this.Count; i++)
			{
				try
				{
					if (this.actionList[i] != null)
					{
						bool flag = this.actionList[i](parameter);
						if (flag)
						{
							return true;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message, null);
				}
			}
			return false;
		}

		// Token: 0x040030E0 RID: 12512
		public const string ERR_BUFFER_TOO_SMALL = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.";

		// Token: 0x040030E1 RID: 12513
		public const string LOG_RESIZING = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size.";

		// Token: 0x040030E3 RID: 12515
		public Predicate<T>[] actionList;

		// Token: 0x040030E4 RID: 12516
		public bool autoResizeable;
	}
}
