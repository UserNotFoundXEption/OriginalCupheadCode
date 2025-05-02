using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x0200065D RID: 1629
	public class FastAction<A, B, C>
	{
		// Token: 0x060044B0 RID: 17584 RVA: 0x000368E8 File Offset: 0x00034AE8
		public void Add(Action<A, B, C> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0013D674 File Offset: 0x0013B874
		public void Remove(Action<A, B, C> rhs)
		{
			LinkedListNode<Action<A, B, C>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0013D6B0 File Offset: 0x0013B8B0
		public void Call(A a, B b, C c)
		{
			for (LinkedListNode<Action<A, B, C>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b, c);
			}
		}

		// Token: 0x04003532 RID: 13618
		public LinkedList<Action<A, B, C>> delegates = new LinkedList<Action<A, B, C>>();

		// Token: 0x04003533 RID: 13619
		public Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>> lookup = new Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>>();
	}
}
