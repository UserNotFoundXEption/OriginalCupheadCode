using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RektTransform
{
	// Token: 0x02000060 RID: 96
	public static class Cast
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x000057D5 File Offset: 0x000039D5
		public static RectTransform RT(this GameObject go)
		{
			if (go == null || go.transform == null)
			{
				return null;
			}
			return go.GetComponent<RectTransform>();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000057FC File Offset: 0x000039FC
		public static RectTransform RT(this Transform t)
		{
			if (!(t is RectTransform))
			{
				return null;
			}
			return t as RectTransform;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00005811 File Offset: 0x00003A11
		public static RectTransform RT(this Component c)
		{
			return c.transform.RT();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000581E File Offset: 0x00003A1E
		public static RectTransform RT(this UIBehaviour ui)
		{
			if (ui == null)
			{
				return null;
			}
			return ui.transform as RectTransform;
		}
	}
}
