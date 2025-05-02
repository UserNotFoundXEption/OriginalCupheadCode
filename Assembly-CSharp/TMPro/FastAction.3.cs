using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x0200065C RID: 1628
	public class FastAction<A, B>
	{
		// Token: 0x060044AC RID: 17580 RVA: 0x0003689E File Offset: 0x00034A9E
		public void Add(Action<A, B> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0013D600 File Offset: 0x0013B800
		public void Remove(Action<A, B> rhs)
		{
			LinkedListNode<Action<A, B>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0013D63C File Offset: 0x0013B83C
		public void Call(A a, B b)
		{
			for (LinkedListNode<Action<A, B>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b);
			}
		}

		// Token: 0x04003530 RID: 13616
		public LinkedList<Action<A, B>> delegates = new LinkedList<Action<A, B>>();

		// Token: 0x04003531 RID: 13617
		public Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>> lookup = new Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>>();
	}
}
