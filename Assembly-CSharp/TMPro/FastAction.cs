using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x0200065A RID: 1626
	public class FastAction
	{
		// Token: 0x060044A4 RID: 17572 RVA: 0x0003680A File Offset: 0x00034A0A
		public void Add(Action rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0013D518 File Offset: 0x0013B718
		public void Remove(Action rhs)
		{
			LinkedListNode<Action> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0013D554 File Offset: 0x0013B754
		public void Call()
		{
			for (LinkedListNode<Action> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value();
			}
		}

		// Token: 0x0400352C RID: 13612
		public LinkedList<Action> delegates = new LinkedList<Action>();

		// Token: 0x0400352D RID: 13613
		public Dictionary<Action, LinkedListNode<Action>> lookup = new Dictionary<Action, LinkedListNode<Action>>();
	}
}
