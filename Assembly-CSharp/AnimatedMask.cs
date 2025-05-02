using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
[RequireComponent(typeof(SpriteMask))]
public class AnimatedMask : MonoBehaviour
{
	// Token: 0x06000B07 RID: 2823 RVA: 0x00009E17 File Offset: 0x00008017
	public void Start()
	{
		this.currentMask = this.maskRequest;
		this.mask = base.GetComponent<SpriteMask>();
		this.mask.sprite = this.currentMask;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00009E42 File Offset: 0x00008042
	public void LateUpdate()
	{
		if (this.currentMask != this.maskRequest)
		{
			this.currentMask = this.maskRequest;
			this.mask.sprite = this.currentMask;
		}
	}

	// Token: 0x0400089F RID: 2207
	public Sprite maskRequest;

	// Token: 0x040008A0 RID: 2208
	public Sprite currentMask;

	// Token: 0x040008A1 RID: 2209
	public SpriteMask mask;
}
