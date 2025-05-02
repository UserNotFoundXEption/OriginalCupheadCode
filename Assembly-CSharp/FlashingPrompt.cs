using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class FlashingPrompt : AbstractMonoBehaviour
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0000A0A6 File Offset: 0x000082A6
	public virtual bool ShouldShow
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0007E0C4 File Offset: 0x0007C2C4
	public void Update()
	{
		if (this.ShouldShow)
		{
			this.flashTimer = (this.flashTimer + CupheadTime.Delta) % 1.5f;
			if (this.child != null)
			{
				this.child.SetActive(this.flashTimer < 0.75f);
			}
			else
			{
				this.childGroup.alpha = ((this.flashTimer >= 0.75f) ? 0f : 1f);
			}
		}
		else
		{
			if (this.child != null)
			{
				this.child.SetActive(false);
			}
			else
			{
				this.childGroup.alpha = 0f;
			}
			this.flashTimer = 0f;
		}
	}

	// Token: 0x040008DB RID: 2267
	public const float FLASH_TIME = 0.75f;

	// Token: 0x040008DC RID: 2268
	public float flashTimer;

	// Token: 0x040008DD RID: 2269
	[SerializeField]
	public GameObject child;

	// Token: 0x040008DE RID: 2270
	[SerializeField]
	public CanvasGroup childGroup;
}
