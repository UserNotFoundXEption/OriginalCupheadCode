using System;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class WeaponArcProjectileExplosion : Effect
{
	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06003845 RID: 14405 RVA: 0x0002DED7 File Offset: 0x0002C0D7
	public DamageDealer DamageDealer
	{
		get
		{
			return this.damageDealer;
		}
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x001067F0 File Offset: 0x001049F0
	public WeaponArcProjectileExplosion Create(Vector2 position, float damage, float damageMultiplier, PlayerId playerId)
	{
		WeaponArcProjectileExplosion weaponArcProjectileExplosion = base.Create(position) as WeaponArcProjectileExplosion;
		weaponArcProjectileExplosion.damageDealer.SetDamage(damage);
		weaponArcProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		weaponArcProjectileExplosion.damageDealer.SetDamageFlags(false, true, false);
		weaponArcProjectileExplosion.damageDealer.PlayerId = playerId;
		return weaponArcProjectileExplosion;
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x0002DEDF File Offset: 0x0002C0DF
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0f);
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x0002DEFC File Offset: 0x0002C0FC
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x0002DF14 File Offset: 0x0002C114
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04002D3C RID: 11580
	public DamageDealer damageDealer;
}
