using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000674 RID: 1652
	public interface ITextElement
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06004630 RID: 17968
		Material sharedMaterial { get; }

		// Token: 0x06004631 RID: 17969
		void Rebuild(CanvasUpdate update);

		// Token: 0x06004632 RID: 17970
		int GetInstanceID();
	}
}
