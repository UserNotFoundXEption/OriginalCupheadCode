using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200064E RID: 1614
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class UIImageHelper : MonoBehaviour
	{
		// Token: 0x06004370 RID: 17264 RVA: 0x0013937C File Offset: 0x0013757C
		public void SetEnabledState(bool newState)
		{
			this.currentState = newState;
			UIImageHelper.State state = (!newState) ? this.disabledState : this.enabledState;
			if (state == null)
			{
				return;
			}
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				Debug.LogError("Image is missing!");
				return;
			}
			state.Set(component);
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x00035CC8 File Offset: 0x00033EC8
		public void SetEnabledStateColor(Color color)
		{
			this.enabledState.color = color;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x00035CD6 File Offset: 0x00033ED6
		public void SetDisabledStateColor(Color color)
		{
			this.disabledState.color = color;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x001393DC File Offset: 0x001375DC
		public void Refresh()
		{
			UIImageHelper.State state = (!this.currentState) ? this.disabledState : this.enabledState;
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				return;
			}
			state.Set(component);
		}

		// Token: 0x040034E5 RID: 13541
		[SerializeField]
		public UIImageHelper.State enabledState;

		// Token: 0x040034E6 RID: 13542
		[SerializeField]
		public UIImageHelper.State disabledState;

		// Token: 0x040034E7 RID: 13543
		public bool currentState;

		// Token: 0x020012E3 RID: 4835
		[Serializable]
		public class State
		{
			// Token: 0x0600839B RID: 33691 RVA: 0x00057BA7 File Offset: 0x00055DA7
			public void Set(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this.color;
			}

			// Token: 0x040081AD RID: 33197
			[SerializeField]
			public Color color;
		}
	}
}
