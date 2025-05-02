using System;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class VeggiesLevelBeetBabyBullet : AbstractProjectile
{
	// Token: 0x06002A9A RID: 10906 RVA: 0x000D46B8 File Offset: 0x000D28B8
	public VeggiesLevelBeetBabyBullet Create(float speed, Vector2 pos, float rot)
	{
		VeggiesLevelBeetBabyBullet veggiesLevelBeetBabyBullet = this.Create(pos, rot) as VeggiesLevelBeetBabyBullet;
		veggiesLevelBeetBabyBullet.CollisionDeath.OnlyPlayer();
		veggiesLevelBeetBabyBullet.DamagesType.OnlyPlayer();
		veggiesLevelBeetBabyBullet.speed = speed;
		return veggiesLevelBeetBabyBullet;
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x00023D44 File Offset: 0x00021F44
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000D46F4 File Offset: 0x000D28F4
	public override void Update()
	{
		base.Update();
		if (this.state == VeggiesLevelBeetBabyBullet.State.Dead)
		{
			return;
		}
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (base.transform.position.y < (float)Level.Current.Ground)
		{
			this.Die();
		}
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x00023D4C File Offset: 0x00021F4C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x00023D63 File Offset: 0x00021F63
	public override void Die()
	{
		base.Die();
		this.state = VeggiesLevelBeetBabyBullet.State.Dead;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04002388 RID: 9096
	public VeggiesLevelBeetBabyBullet.State state;

	// Token: 0x04002389 RID: 9097
	public float speed;

	// Token: 0x02000FDB RID: 4059
	public enum State
	{
		// Token: 0x040071E8 RID: 29160
		Go,
		// Token: 0x040071E9 RID: 29161
		Dead
	}
}
