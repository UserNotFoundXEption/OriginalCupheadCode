using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000647 RID: 1607
	[AddComponentMenu("")]
	public class ThemedElement : MonoBehaviour
	{
		// Token: 0x0600434E RID: 17230 RVA: 0x00035B1A File Offset: 0x00033D1A
		public void Start()
		{
			this.ApplyTheme();
			ControlMapper.OnPlayerChange += this.ApplyTheme;
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x00035B33 File Offset: 0x00033D33
		public void OnDestroy()
		{
			ControlMapper.OnPlayerChange -= this.ApplyTheme;
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x00035B46 File Offset: 0x00033D46
		public void OnEnable()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x00035B53 File Offset: 0x00033D53
		public void ApplyTheme()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x040034BD RID: 13501
		[SerializeField]
		public ThemedElement.ElementInfo[] _elements;

		// Token: 0x020012D7 RID: 4823
		[Serializable]
		public class ElementInfo
		{
			// Token: 0x170019A1 RID: 6561
			// (get) Token: 0x0600834A RID: 33610 RVA: 0x0005787A File Offset: 0x00055A7A
			public string themeClass
			{
				get
				{
					return this._themeClass;
				}
			}

			// Token: 0x170019A2 RID: 6562
			// (get) Token: 0x0600834B RID: 33611 RVA: 0x00057882 File Offset: 0x00055A82
			public Component component
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x04008178 RID: 33144
			[SerializeField]
			public string _themeClass;

			// Token: 0x04008179 RID: 33145
			[SerializeField]
			public Component _component;
		}
	}
}
