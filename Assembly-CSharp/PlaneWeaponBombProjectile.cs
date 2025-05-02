using System;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class PlaneWeaponBombProjectile : AbstractProjectile
{
	// Token: 0x06003A71 RID: 14961 RVA: 0x0010F784 File Offset: 0x0010D984
	public override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?(this.bulletSize), new float?(this.bulletSize), null);
		AudioManager.Play("plane_shmup_bomb_fire");
		this.emitAudioFromObject.Add("plane_shmup_bomb_fire");
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x0010F7DC File Offset: 0x0010D9DC
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.shootsUp)
		{
			this.velocity.y = this.velocity.y + this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
		else
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0002F912 File Offset: 0x0002DB12
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0002F921 File Offset: 0x0002DB21
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x0002F932 File Offset: 0x0002DB32
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag != "Parry")
		{
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x0010F8A0 File Offset: 0x0010DAA0
	public override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
		AudioManager.Play("plane_shmup_bomb_explosion");
		this.emitAudioFromObject.Add("plane_shmup_bomb_explosion");
		this.explosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.explosionSize);
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x0002F951 File Offset: 0x0002DB51
	public void SetAnimation(PlayerId player)
	{
		base.animator.Play(((player != PlayerId.PlayerOne || PlayerManager.player1IsMugman) && (player != PlayerId.PlayerTwo || !PlayerManager.player1IsMugman)) ? "Bomb_MM" : "Bomb_CH");
	}

	// Token: 0x04002EC9 RID: 11977
	[SerializeField]
	public PlaneWeaponBombExplosion explosion;

	// Token: 0x04002ECA RID: 11978
	public bool shootsUp;

	// Token: 0x04002ECB RID: 11979
	public float explosionSize;

	// Token: 0x04002ECC RID: 11980
	public float bulletSize;

	// Token: 0x04002ECD RID: 11981
	public float gravity;

	// Token: 0x04002ECE RID: 11982
	public Vector2 velocity;
}
