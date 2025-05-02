using System;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class SaltbakerLevelHand : AbstractCollidableObject
{
	// Token: 0x06002706 RID: 9990 RVA: 0x00020D22 File Offset: 0x0001EF22
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x00020D2F File Offset: 0x0001EF2F
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x00020D47 File Offset: 0x0001EF47
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x000CA74C File Offset: 0x000C894C
	public void Shoot(float speed)
	{
		float rotation = (!this.leftHand) ? 180f : 0f;
		this.projectile.Create(this.root.position, rotation, speed);
	}

	// Token: 0x04002039 RID: 8249
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x0400203A RID: 8250
	[SerializeField]
	public Transform root;

	// Token: 0x0400203B RID: 8251
	[SerializeField]
	public bool leftHand;

	// Token: 0x0400203C RID: 8252
	public DamageDealer damageDealer;
}
