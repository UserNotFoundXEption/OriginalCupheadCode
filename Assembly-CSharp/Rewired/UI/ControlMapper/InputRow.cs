using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000643 RID: 1603
	[AddComponentMenu("")]
	public class InputRow : MonoBehaviour
	{
		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06004300 RID: 17152 RVA: 0x00035957 File Offset: 0x00033B57
		// (set) Token: 0x06004301 RID: 17153 RVA: 0x0003595F File Offset: 0x00033B5F
		public ButtonInfo[] buttons { get; set; }

		// Token: 0x06004302 RID: 17154 RVA: 0x00035968 File Offset: 0x00033B68
		public void Initialize(int rowIndex, string label, Action<int, ButtonInfo> inputFieldActivatedCallback)
		{
			this.rowIndex = rowIndex;
			this.label.text = label;
			this.inputFieldActivatedCallback = inputFieldActivatedCallback;
			this.buttons = base.transform.GetComponentsInChildren<ButtonInfo>(true);
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00035996 File Offset: 0x00033B96
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (this.inputFieldActivatedCallback == null)
			{
				return;
			}
			this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
		}

		// Token: 0x04003478 RID: 13432
		public Text label;

		// Token: 0x0400347A RID: 13434
		public int rowIndex;

		// Token: 0x0400347B RID: 13435
		public Action<int, ButtonInfo> inputFieldActivatedCallback;
	}
}
