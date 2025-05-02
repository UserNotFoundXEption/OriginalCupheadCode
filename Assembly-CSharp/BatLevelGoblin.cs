using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class BatLevelGoblin : AbstractCollidableObject
{
	// Token: 0x060010C1 RID: 4289 RVA: 0x0000E184 File Offset: 0x0000C384
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0000E1BA File Offset: 0x0000C3BA
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x0000E1FD File Offset: 0x0000C3FD
	public void Init(LevelProperties.Bat.Goblins properties, Vector2 pos, bool onLeft, bool isShooter, float health)
	{
		base.transform.position = pos;
		this.onLeft = onLeft;
		this.isShooter = isShooter;
		this.properties = properties;
		this.health = health;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0000E23B File Offset: 0x0000C43B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x00090F6C File Offset: 0x0008F16C
	public IEnumerator move_cr()
	{
		float endpos = (float)((!this.onLeft) ? -640 : 640);
		float t = 0f;
		while (base.transform.position.x != endpos)
		{
			Vector3 pos = base.transform.position;
			pos.x = Mathf.MoveTowards(base.transform.position.x, endpos, this.properties.runSpeed * CupheadTime.Delta);
			base.transform.position = pos;
			if (this.isShooter)
			{
				t += CupheadTime.Delta;
				if (t >= this.properties.timeBeforeShoot)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.initalShotDelay);
					this.ShootBullet();
					yield return CupheadTime.WaitForSeconds(this, this.properties.shooterHold);
					this.isShooter = false;
				}
			}
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x00090F88 File Offset: 0x0008F188
	public void ShootBullet()
	{
		float num;
		if (this.onLeft)
		{
			num = 15469.86f;
		}
		else
		{
			num = -5156.62f;
		}
		float rotation = Mathf.Atan2(base.transform.position.y, num) * 57.29578f;
		this.projectile.Create(base.transform.position, rotation, this.properties.bulletSpeed);
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0000E252 File Offset: 0x0000C452
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000DA0 RID: 3488
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04000DA1 RID: 3489
	public LevelProperties.Bat.Goblins properties;

	// Token: 0x04000DA2 RID: 3490
	public bool onLeft;

	// Token: 0x04000DA3 RID: 3491
	public bool isShooter;

	// Token: 0x04000DA4 RID: 3492
	public float health;

	// Token: 0x04000DA5 RID: 3493
	public DamageDealer damageDealer;

	// Token: 0x04000DA6 RID: 3494
	public DamageReceiver damageReceiver;
}
