using System;
using UnityEngine;

// Token: 0x0200042C RID: 1068
public class HarbourPlatformingLevelIceberg : AbstractCollidableObject
{
	// Token: 0x06002E21 RID: 11809 RVA: 0x00026757 File Offset: 0x00024957
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x00026764 File Offset: 0x00024964
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002E23 RID: 11811 RVA: 0x0002677C File Offset: 0x0002497C
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<HarbourPlatformingLevelOctoProjectile>())
		{
			this.SmashSFX();
			Object.Destroy(hit.gameObject);
			this.DeathParts();
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002E24 RID: 11812 RVA: 0x000267B8 File Offset: 0x000249B8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E25 RID: 11813 RVA: 0x000DEBDC File Offset: 0x000DCDDC
	public void DeathParts()
	{
		this.explosion.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06002E26 RID: 11814 RVA: 0x000267D6 File Offset: 0x000249D6
	public void SmashSFX()
	{
		AudioManager.Play("harbour_iceberg_smash");
		this.emitAudioFromObject.Add("harbour_iceberg_smash");
	}

	// Token: 0x0400263B RID: 9787
	[SerializeField]
	public Effect explosion;

	// Token: 0x0400263C RID: 9788
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x0400263D RID: 9789
	public DamageDealer damageDealer;
}
