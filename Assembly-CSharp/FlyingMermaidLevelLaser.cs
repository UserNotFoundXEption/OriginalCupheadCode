using System;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class FlyingMermaidLevelLaser : AbstractCollidableObject
{
	// Token: 0x06001D39 RID: 7481 RVA: 0x00018C14 File Offset: 0x00016E14
	public void SetStoneTime(float stoneTime)
	{
		this.stoneTime = stoneTime;
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x00018C1D File Offset: 0x00016E1D
	public void StartLaser()
	{
		if (base.GetComponent<Collider2D>())
		{
			this.checkCollider = true;
		}
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x00018C36 File Offset: 0x00016E36
	public void StopLaser()
	{
		if (base.GetComponent<Collider2D>())
		{
			this.checkCollider = false;
		}
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x00018C4F File Offset: 0x00016E4F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.checkCollider)
		{
			hit.GetComponent<PlanePlayerController>().GetStoned(this.stoneTime);
		}
	}

	// Token: 0x040017D5 RID: 6101
	public float stoneTime = 5f;

	// Token: 0x040017D6 RID: 6102
	public bool checkCollider;
}
