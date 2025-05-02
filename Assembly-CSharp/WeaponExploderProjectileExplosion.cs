using System;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class WeaponExploderProjectileExplosion : Effect
{
	// Token: 0x060038B5 RID: 14517 RVA: 0x00108EDC File Offset: 0x001070DC
	public void Create(Vector2 position, float radius, float damage, float damageMultiplier, WeaponExploder weapon, MeterScoreTracker tracker)
	{
		float num = radius / 15f;
		WeaponExploderProjectileExplosion weaponExploderProjectileExplosion = base.Create(position, new Vector3(num, num, 1f)) as WeaponExploderProjectileExplosion;
		weaponExploderProjectileExplosion.damageDealer.SetDamage(damage);
		weaponExploderProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		weaponExploderProjectileExplosion.damageDealer.SetDamageFlags(false, true, false);
		weaponExploderProjectileExplosion.weapon = weapon;
		weaponExploderProjectileExplosion.damageDealer.OnDealDamage += weaponExploderProjectileExplosion.OnDealDamage;
		if (tracker != null)
		{
			tracker.Add(weaponExploderProjectileExplosion.damageDealer);
		}
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x0002E349 File Offset: 0x0002C549
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0f);
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x0002E366 File Offset: 0x0002C566
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x0002E37E File Offset: 0x0002C57E
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x0002E3A6 File Offset: 0x0002C5A6
	public void OnDealDamage(float damage, DamageReceiver damageReceiver, DamageDealer damageDealer)
	{
		if (this.weapon != null)
		{
			this.weapon.OnDealDamage(damage, damageReceiver, damageDealer);
		}
	}

	// Token: 0x04002D90 RID: 11664
	public DamageDealer damageDealer;

	// Token: 0x04002D91 RID: 11665
	public const float BASE_RADIUS = 15f;

	// Token: 0x04002D92 RID: 11666
	public WeaponExploder weapon;
}
