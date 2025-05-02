using System;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class WeaponSparkEffect : Effect
{
	// Token: 0x06003776 RID: 14198 RVA: 0x00103680 File Offset: 0x00101880
	public void SetPlayer(LevelPlayerController player)
	{
		if (player.motor.Grounded)
		{
			this.player = player;
			Vector3 localScale = base.transform.localScale;
			base.transform.parent = player.transform;
			base.transform.localScale = localScale;
			this.playerXScale = player.transform.localScale.x;
		}
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0002D4C1 File Offset: 0x0002B6C1
	public void BringToFrontOfPlayer()
	{
		base.GetComponent<SpriteRenderer>().sortingOrder = 100;
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0002D4D0 File Offset: 0x0002B6D0
	public void FixedUpdate()
	{
		if (this.player != null && !this.player.motor.Grounded)
		{
			this.player = null;
			base.transform.parent = null;
		}
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x001036E8 File Offset: 0x001018E8
	public void LateUpdate()
	{
		if (this.player != null && this.player.transform.localScale.x != this.playerXScale)
		{
			base.transform.SetLocalPosition(new float?(-base.transform.localPosition.x), null, null);
			this.player = null;
			base.transform.parent = null;
		}
	}

	// Token: 0x04002C9D RID: 11421
	public LevelPlayerController player;

	// Token: 0x04002C9E RID: 11422
	public float playerXScale;
}
