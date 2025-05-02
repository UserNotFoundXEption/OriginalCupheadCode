using System;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class PlaneWeaponBombExplosion : Effect
{
	// Token: 0x06003A61 RID: 14945 RVA: 0x0010F1F8 File Offset: 0x0010D3F8
	public void Create(Vector2 position, float damage, float damageMultiplier, float size)
	{
		PlaneWeaponBombExplosion planeWeaponBombExplosion = base.Create(position) as PlaneWeaponBombExplosion;
		planeWeaponBombExplosion.damageDealer.SetDamage(damage);
		planeWeaponBombExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		planeWeaponBombExplosion.damageDealer.SetDamageFlags(false, true, false);
		planeWeaponBombExplosion.transform.SetScale(new float?(size), new float?(size), null);
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x0002F827 File Offset: 0x0002DA27
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0002F83A File Offset: 0x0002DA3A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x0002F852 File Offset: 0x0002DA52
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04002EB6 RID: 11958
	public DamageDealer damageDealer;
}
