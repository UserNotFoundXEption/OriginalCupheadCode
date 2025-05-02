using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200064C RID: 1612
	[AddComponentMenu("")]
	public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x06004366 RID: 17254 RVA: 0x00035C1D File Offset: 0x00033E1D
		public UIElementInfo()
		{
		}

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06004367 RID: 17255 RVA: 0x0013930C File Offset: 0x0013750C
		// (remove) Token: 0x06004368 RID: 17256 RVA: 0x00139344 File Offset: 0x00137544
		public event Action<GameObject> OnSelectedEvent;

		// Token: 0x06004369 RID: 17257 RVA: 0x00035C25 File Offset: 0x00033E25
		public void OnSelect(BaseEventData eventData)
		{
			if (this.OnSelectedEvent != null)
			{
				this.OnSelectedEvent(base.gameObject);
			}
		}

		// Token: 0x040034DF RID: 13535
		public string identifier;

		// Token: 0x040034E0 RID: 13536
		public int intData;

		// Token: 0x040034E1 RID: 13537
		public Text text;
	}
}
