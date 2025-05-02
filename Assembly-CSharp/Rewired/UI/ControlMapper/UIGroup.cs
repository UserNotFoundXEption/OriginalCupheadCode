using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200064D RID: 1613
	[AddComponentMenu("")]
	public class UIGroup : MonoBehaviour
	{
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x00035C4B File Offset: 0x00033E4B
		// (set) Token: 0x0600436C RID: 17260 RVA: 0x00035C73 File Offset: 0x00033E73
		public string labelText
		{
			get
			{
				return (!(this._label != null)) ? string.Empty : this._label.text;
			}
			set
			{
				if (this._label == null)
				{
					return;
				}
				this._label.text = value;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600436D RID: 17261 RVA: 0x00035C93 File Offset: 0x00033E93
		public Transform content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00035C9B File Offset: 0x00033E9B
		public void SetLabelActive(bool state)
		{
			if (this._label == null)
			{
				return;
			}
			this._label.gameObject.SetActive(state);
		}

		// Token: 0x040034E3 RID: 13539
		[SerializeField]
		public Text _label;

		// Token: 0x040034E4 RID: 13540
		[SerializeField]
		public Transform _content;
	}
}
