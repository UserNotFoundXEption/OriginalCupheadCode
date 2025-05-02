using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class FlyingBirdLevelNursePillProjectile : BasicProjectile
{
	// Token: 0x060019E2 RID: 6626 RVA: 0x000A73D8 File Offset: 0x000A55D8
	public void SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor color)
	{
		if (color == FlyingBirdLevelNursePillProjectile.PillColor.Yellow)
		{
			this.yellowPill.SetActive(true);
		}
		else if (color == FlyingBirdLevelNursePillProjectile.PillColor.Blue)
		{
			this.bluePill.SetActive(true);
		}
		else if (color == FlyingBirdLevelNursePillProjectile.PillColor.LightPink)
		{
			this.lightPinkPill.SetActive(true);
		}
		else
		{
			this.darkPinkPill.SetActive(true);
		}
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000160BD File Offset: 0x000142BD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040014C8 RID: 5320
	[SerializeField]
	public GameObject yellowPill;

	// Token: 0x040014C9 RID: 5321
	[SerializeField]
	public GameObject bluePill;

	// Token: 0x040014CA RID: 5322
	[SerializeField]
	public GameObject lightPinkPill;

	// Token: 0x040014CB RID: 5323
	[SerializeField]
	public GameObject darkPinkPill;

	// Token: 0x02000C57 RID: 3159
	public enum PillColor
	{
		// Token: 0x0400595B RID: 22875
		Yellow,
		// Token: 0x0400595C RID: 22876
		Blue,
		// Token: 0x0400595D RID: 22877
		LightPink,
		// Token: 0x0400595E RID: 22878
		DarkPink
	}
}
