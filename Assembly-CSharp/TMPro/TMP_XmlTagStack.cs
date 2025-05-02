using System;

namespace TMPro
{
	// Token: 0x02000688 RID: 1672
	public struct TMP_XmlTagStack<T>
	{
		// Token: 0x06004726 RID: 18214 RVA: 0x0003891A File Offset: 0x00036B1A
		public TMP_XmlTagStack(T[] tagStack)
		{
			this.itemStack = tagStack;
			this.index = 0;
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x0003892A File Offset: 0x00036B2A
		public void Clear()
		{
			this.index = 0;
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x00038933 File Offset: 0x00036B33
		public void SetDefault(T item)
		{
			this.itemStack[0] = item;
			this.index = 1;
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x00038949 File Offset: 0x00036B49
		public void Add(T item)
		{
			if (this.index < this.itemStack.Length)
			{
				this.itemStack[this.index] = item;
				this.index++;
			}
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x00159648 File Offset: 0x00157848
		public T Remove()
		{
			this.index--;
			if (this.index <= 0)
			{
				this.index = 0;
				return this.itemStack[0];
			}
			return this.itemStack[this.index - 1];
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0003897E File Offset: 0x00036B7E
		public T CurrentItem()
		{
			return this.itemStack[this.index - 1];
		}

		// Token: 0x04003708 RID: 14088
		public T[] itemStack;

		// Token: 0x04003709 RID: 14089
		public int index;
	}
}
