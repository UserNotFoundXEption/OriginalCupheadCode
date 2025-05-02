using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class AirplaneLevelTurretBullet : AbstractProjectile
{
	// Token: 0x06000EB9 RID: 3769 RVA: 0x0008C1B8 File Offset: 0x0008A3B8
	public AirplaneLevelTurretBullet Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		AirplaneLevelTurretBullet airplaneLevelTurretBullet = base.Create() as AirplaneLevelTurretBullet;
		airplaneLevelTurretBullet.velocity = velocity;
		airplaneLevelTurretBullet.transform.position = pos;
		airplaneLevelTurretBullet.gravity = gravity;
		return airplaneLevelTurretBullet;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0008C1F4 File Offset: 0x0008A3F4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0000C793 File Offset: 0x0000A993
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0000C7B1 File Offset: 0x0000A9B1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
			AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_impact");
		}
	}

	// Token: 0x04000C14 RID: 3092
	public Vector2 velocity;

	// Token: 0x04000C15 RID: 3093
	public float gravity;
}
