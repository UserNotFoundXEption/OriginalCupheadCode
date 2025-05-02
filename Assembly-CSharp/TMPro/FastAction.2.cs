using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x0200065B RID: 1627
	public class FastAction<A>
	{
		// Token: 0x060044A8 RID: 17576 RVA: 0x00036854 File Offset: 0x00034A54
		public void Add(Action<A> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0013D58C File Offset: 0x0013B78C
		public void Remove(Action<A> rhs)
		{
			LinkedListNode<Action<A>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0013D5C8 File Offset: 0x0013B7C8
		public void Call(A a)
		{
			for (LinkedListNode<Action<A>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a);
			}
		}

		// Token: 0x0400352E RID: 13614
		public LinkedList<Action<A>> delegates = new LinkedList<Action<A>>();

		// Token: 0x0400352F RID: 13615
		public Dictionary<Action<A>, LinkedListNode<Action<A>>> lookup = new Dictionary<Action<A>, LinkedListNode<Action<A>>>();
	}
}
