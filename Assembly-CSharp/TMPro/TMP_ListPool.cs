using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x0200066A RID: 1642
	public static class TMP_ListPool<T>
	{
		// Token: 0x060045A8 RID: 17832 RVA: 0x0003757D File Offset: 0x0003577D
		public static List<T> Get()
		{
			return TMP_ListPool<T>.s_ListPool.Get();
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00037589 File Offset: 0x00035789
		public static void Release(List<T> toRelease)
		{
			TMP_ListPool<T>.s_ListPool.Release(toRelease);
		}

		// Token: 0x040035B0 RID: 13744
		public static readonly TMP_ObjectPool<List<T>> s_ListPool = new TMP_ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		});
	}
}
