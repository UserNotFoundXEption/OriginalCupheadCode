using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200064A RID: 1610
	[AddComponentMenu("")]
	public class UIControl : MonoBehaviour
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600435B RID: 17243 RVA: 0x00035BAA File Offset: 0x00033DAA
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00035BB2 File Offset: 0x00033DB2
		public void Awake()
		{
			this._id = UIControl.GetNextUid();
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600435D RID: 17245 RVA: 0x00035BBF File Offset: 0x00033DBF
		// (set) Token: 0x0600435E RID: 17246 RVA: 0x00035BC7 File Offset: 0x00033DC7
		public bool showTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				if (this.title == null)
				{
					return;
				}
				this.title.gameObject.SetActive(value);
				this._showTitle = value;
			}
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00035BF3 File Offset: 0x00033DF3
		public virtual void SetCancelCallback(Action cancelCallback)
		{
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00139148 File Offset: 0x00137348
		public static int GetNextUid()
		{
			if (UIControl._uidCounter == 2147483647)
			{
				UIControl._uidCounter = 0;
			}
			int uidCounter = UIControl._uidCounter;
			UIControl._uidCounter++;
			return uidCounter;
		}

		// Token: 0x040034D9 RID: 13529
		public Text title;

		// Token: 0x040034DA RID: 13530
		public int _id;

		// Token: 0x040034DB RID: 13531
		public bool _showTitle;

		// Token: 0x040034DC RID: 13532
		public static int _uidCounter;
	}
}
