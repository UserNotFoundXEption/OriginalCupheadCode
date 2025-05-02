using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class BeeLevelGrunt : AbstractCollidableObject
{
	// Token: 0x06001115 RID: 4373 RVA: 0x00091B94 File Offset: 0x0008FD94
	public BeeLevelGrunt Create(Vector2 pos, int xScale, int health, float speed)
	{
		BeeLevelGrunt beeLevelGrunt = Object.Instantiate<BeeLevelGrunt>(this);
		beeLevelGrunt.speed = speed;
		beeLevelGrunt.health = (float)health;
		beeLevelGrunt.transform.SetScale(new float?((float)xScale), new float?(1f), new float?(1f));
		beeLevelGrunt.transform.position = pos;
		return beeLevelGrunt;
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x0000E6C3 File Offset: 0x0000C8C3
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 1f, true, false, false);
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x00091BF0 File Offset: 0x0008FDF0
	public void Update()
	{
		if (this.dead)
		{
			return;
		}
		if (base.transform.position.x < -1280f || base.transform.position.x > 1280f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.AddPosition(this.speed * CupheadTime.Delta * -base.transform.localScale.x, 0f, 0f);
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0000E6FA File Offset: 0x0000C8FA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x0000E718 File Offset: 0x0000C918
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x00091C8C File Offset: 0x0008FE8C
	public void Die()
	{
		AudioManager.Play("level_bee_grunt_death");
		this.dead = true;
		this.briefcasePrefab.Create((int)base.transform.localScale.x, base.transform.position);
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Die");
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x0000E743 File Offset: 0x0000C943
	public void OnDeathAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x0000E750 File Offset: 0x0000C950
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.briefcasePrefab = null;
	}

	// Token: 0x04000DDE RID: 3550
	[SerializeField]
	public BeeLevelGruntBriefcase briefcasePrefab;

	// Token: 0x04000DDF RID: 3551
	public float health;

	// Token: 0x04000DE0 RID: 3552
	public float speed;

	// Token: 0x04000DE1 RID: 3553
	public DamageDealer damageDealer;

	// Token: 0x04000DE2 RID: 3554
	public bool dead;
}
