using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class FlyingBirdLevelEnemy : AbstractProjectile
{
	// Token: 0x060019B6 RID: 6582 RVA: 0x000A6ECC File Offset: 0x000A50CC
	public FlyingBirdLevelEnemy Create(Vector2 pos, FlyingBirdLevelEnemy.Properties properties)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		FlyingBirdLevelEnemy component = gameObject.GetComponent<FlyingBirdLevelEnemy>();
		component.transform.position = pos;
		component.properties = properties;
		component.Init();
		return component;
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x00015F0C File Offset: 0x0001410C
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x00015F2B File Offset: 0x0001412B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000A6F0C File Offset: 0x000A510C
	public void Init()
	{
		this.startPos = base.transform.position;
		this.health = (float)this.properties.health;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x00015F54 File Offset: 0x00014154
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x00015F7F File Offset: 0x0001417F
	public void Shoot()
	{
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x00015F81 File Offset: 0x00014181
	public override void OnParryDie()
	{
		this.Die();
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x00015F89 File Offset: 0x00014189
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		AudioManager.Play("level_flying_bird_smallbird_death");
		this.emitAudioFromObject.Add("level_flying_bird_smallbird_death");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000A6F5C File Offset: 0x000A515C
	public IEnumerator y_cr()
	{
		float start = this.startPos.y + this.properties.floatRange / 2f;
		float end = this.startPos.y - this.properties.floatRange / 2f;
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		for (;;)
		{
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val2 = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, end, start, val2)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000A6F78 File Offset: 0x000A5178
	public IEnumerator x_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(-this.properties.speed * CupheadTime.Delta, 0f, 0f);
			if (base.transform.position.x < -740f)
			{
				this.Die();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000A6F94 File Offset: 0x000A5194
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.projectileDelay);
			this.Shoot();
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014A6 RID: 5286
	[SerializeField]
	public FlyingBirdLevelEnemyProjectile projectilePrefab;

	// Token: 0x040014A7 RID: 5287
	public FlyingBirdLevelEnemy.Properties properties;

	// Token: 0x040014A8 RID: 5288
	public Vector2 startPos;

	// Token: 0x040014A9 RID: 5289
	public float health;

	// Token: 0x02000C4B RID: 3147
	public class Properties
	{
		// Token: 0x060063E1 RID: 25569 RVA: 0x0004793B File Offset: 0x00045B3B
		public Properties(int health, float speed, float floatRange, float floatTime, float projectileHeight, float projectileFallTime, float projectileDelay)
		{
			this.health = health;
			this.speed = speed;
			this.floatRange = floatRange;
			this.floatTime = floatTime;
			this.projectileHeight = projectileHeight;
			this.projectileFallTime = projectileFallTime;
			this.projectileDelay = projectileDelay;
		}

		// Token: 0x0400590D RID: 22797
		public readonly int health;

		// Token: 0x0400590E RID: 22798
		public readonly float speed;

		// Token: 0x0400590F RID: 22799
		public readonly float floatRange;

		// Token: 0x04005910 RID: 22800
		public readonly float floatTime;

		// Token: 0x04005911 RID: 22801
		public readonly float projectileHeight;

		// Token: 0x04005912 RID: 22802
		public readonly float projectileFallTime;

		// Token: 0x04005913 RID: 22803
		public readonly float projectileDelay;
	}
}
