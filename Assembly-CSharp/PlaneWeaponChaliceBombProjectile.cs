using System;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class PlaneWeaponChaliceBombProjectile : AbstractProjectile
{
	// Token: 0x06003A85 RID: 14981 RVA: 0x0010FBC8 File Offset: 0x0010DDC8
	public override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?(this.size), new float?(this.size), null);
		AudioManager.Play("plane_shmup_bomb_fire");
		this.emitAudioFromObject.Add("plane_shmup_bomb_fire");
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0010FC20 File Offset: 0x0010DE20
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0002F9F6 File Offset: 0x0002DBF6
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0002FA05 File Offset: 0x0002DC05
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0002FA16 File Offset: 0x0002DC16
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag != "Parry")
		{
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x0010FC88 File Offset: 0x0010DE88
	public override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
		AudioManager.Play("plane_shmup_bomb_explosion");
		this.emitAudioFromObject.Add("plane_shmup_bomb_explosion");
		this.explosion.Create(base.transform.position, this.damageExplosion, base.DamageMultiplier, this.explosionSize);
		this.explosion.animator.Play((!this.isA) ? "B" : "A");
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0002FA35 File Offset: 0x0002DC35
	public void SetAnimation(bool isA)
	{
		this.isA = isA;
		base.animator.Play((!isA) ? "B" : "A");
	}

	// Token: 0x04002EDB RID: 11995
	[SerializeField]
	public PlaneWeaponBombExplosion explosion;

	// Token: 0x04002EDC RID: 11996
	public float explosionSize;

	// Token: 0x04002EDD RID: 11997
	public float gravity;

	// Token: 0x04002EDE RID: 11998
	public float damageExplosion;

	// Token: 0x04002EDF RID: 11999
	public float size;

	// Token: 0x04002EE0 RID: 12000
	public bool isA;

	// Token: 0x04002EE1 RID: 12001
	public Vector2 velocity;
}
