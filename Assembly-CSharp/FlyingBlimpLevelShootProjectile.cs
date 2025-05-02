using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class FlyingBlimpLevelShootProjectile : AbstractProjectile
{
	// Token: 0x06001ABC RID: 6844 RVA: 0x000A9860 File Offset: 0x000A7A60
	public FlyingBlimpLevelShootProjectile Create(Vector2 pos, float rotation, LevelProperties.FlyingBlimp.Shoot properties)
	{
		FlyingBlimpLevelShootProjectile flyingBlimpLevelShootProjectile = base.Create() as FlyingBlimpLevelShootProjectile;
		flyingBlimpLevelShootProjectile.properties = properties;
		flyingBlimpLevelShootProjectile.velocity = properties.speedMin;
		flyingBlimpLevelShootProjectile.transform.position = pos;
		return flyingBlimpLevelShootProjectile;
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x00016B6A File Offset: 0x00014D6A
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x00016B7F File Offset: 0x00014D7F
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x00016B9D File Offset: 0x00014D9D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000A98A0 File Offset: 0x000A7AA0
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.velocity < this.properties.speedMax)
		{
			this.velocity += this.properties.accelerationTime * CupheadTime.FixedDelta;
			yield return wait;
			base.transform.AddPosition(-this.velocity * CupheadTime.FixedDelta, 0f, 0f);
		}
		this.Die();
		yield return wait;
		yield break;
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x00016BBB File Offset: 0x00014DBB
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400158D RID: 5517
	public LevelProperties.FlyingBlimp.Shoot properties;

	// Token: 0x0400158E RID: 5518
	public float velocity;
}
