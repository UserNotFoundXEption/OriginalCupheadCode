using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace TMPro
{
	// Token: 0x0200066C RID: 1644
	public class TMP_ObjectPool<T> where T : new()
	{
		// Token: 0x060045B7 RID: 17847 RVA: 0x00037608 File Offset: 0x00035808
		public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00037629 File Offset: 0x00035829
		// (set) Token: 0x060045B9 RID: 17849 RVA: 0x00037631 File Offset: 0x00035831
		public int countAll { get; set; }

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060045BA RID: 17850 RVA: 0x0003763A File Offset: 0x0003583A
		public int countActive
		{
			get
			{
				return this.countAll - this.countInactive;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060045BB RID: 17851 RVA: 0x00037649 File Offset: 0x00035849
		public int countInactive
		{
			get
			{
				return this.m_Stack.Count;
			}
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x0014EB4C File Offset: 0x0014CD4C
		public T Get()
		{
			T t;
			if (this.m_Stack.Count == 0)
			{
				t = Activator.CreateInstance<T>();
				this.countAll++;
			}
			else
			{
				t = this.m_Stack.Pop();
			}
			if (this.m_ActionOnGet != null)
			{
				this.m_ActionOnGet.Invoke(t);
			}
			return t;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x0014EBA8 File Offset: 0x0014CDA8
		public void Release(T element)
		{
			if (this.m_Stack.Count > 0 && object.ReferenceEquals(this.m_Stack.Peek(), element))
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.", null);
			}
			if (this.m_ActionOnRelease != null)
			{
				this.m_ActionOnRelease.Invoke(element);
			}
			this.m_Stack.Push(element);
		}

		// Token: 0x040035B3 RID: 13747
		public readonly Stack<T> m_Stack = new Stack<T>();

		// Token: 0x040035B4 RID: 13748
		public readonly UnityAction<T> m_ActionOnGet;

		// Token: 0x040035B5 RID: 13749
		public readonly UnityAction<T> m_ActionOnRelease;
	}
}
