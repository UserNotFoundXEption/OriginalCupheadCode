using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D2 RID: 978
public class VeggiesLevelPotato : LevelProperties.Veggies.Entity
{
	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x0002433B File Offset: 0x0002253B
	// (set) Token: 0x06002B1D RID: 11037 RVA: 0x00024343 File Offset: 0x00022543
	public VeggiesLevelPotato.State state { get; set; }

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06002B1E RID: 11038 RVA: 0x000D56AC File Offset: 0x000D38AC
	// (remove) Token: 0x06002B1F RID: 11039 RVA: 0x000D56E4 File Offset: 0x000D38E4
	public event VeggiesLevelPotato.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06002B20 RID: 11040 RVA: 0x0002434C File Offset: 0x0002254C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x00024371 File Offset: 0x00022571
	public void Start()
	{
		this.SfxGround();
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000D571C File Offset: 0x000D391C
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Potato);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.potato_cr());
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x00024379 File Offset: 0x00022579
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000D5774 File Offset: 0x000D3974
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x00024391 File Offset: 0x00022591
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x000243AF File Offset: 0x000225AF
	public void Die()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x000243D3 File Offset: 0x000225D3
	public void StartExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000243E0 File Offset: 0x000225E0
	public void EndExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x000243ED File Offset: 0x000225ED
	public void SfxGround()
	{
		AudioManager.Play("level_veggies_potato_ground");
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x000243F9 File Offset: 0x000225F9
	public void OnInAnimComplete()
	{
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000243FB File Offset: 0x000225FB
	public void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelPotato.State.Complete;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000D57C8 File Offset: 0x000D39C8
	public void Shoot()
	{
		if (!this.projectileParryFlag)
		{
			AudioManager.Play("levels_veggies_potato_spit");
		}
		else
		{
			AudioManager.Play("level_veggies_potato_spit_worm");
		}
		this.didShoot = true;
		BasicProjectile basicProjectile = this.projectilePrefab.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, this.properties.bulletSpeed);
		basicProjectile.SetParryable(this.projectileParryFlag);
		this.spitEffect.Create(this.gunRoot.position);
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000D5860 File Offset: 0x000D3A60
	public IEnumerator potato_cr()
	{
		for (;;)
		{
			int groups = 0;
			int shots = 0;
			while (groups < this.properties.seriesCount)
			{
				float delay = this.properties.bulletDelay.GetFloatAt(1f - (float)groups / ((float)this.properties.seriesCount - 1f));
				while (shots < this.properties.bulletCount)
				{
					shots++;
					base.animator.SetTrigger("Shoot");
					this.didShoot = false;
					this.projectileParryFlag = (shots == this.properties.bulletCount);
					while (!this.didShoot)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, delay);
				}
				groups++;
				shots = 0;
				if (groups != this.properties.seriesCount)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.seriesDelay);
					yield return CupheadTime.WaitForSeconds(this, 0.6f);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime);
		}
		yield break;
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x0002440F File Offset: 0x0002260F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
		this.spitEffect = null;
	}

	// Token: 0x040023D2 RID: 9170
	public const float START_SHOOTING_TIME = 0.6f;

	// Token: 0x040023D4 RID: 9172
	[SerializeField]
	public Transform gunRoot;

	// Token: 0x040023D5 RID: 9173
	[SerializeField]
	public VeggiesLevelSpit projectilePrefab;

	// Token: 0x040023D6 RID: 9174
	[SerializeField]
	public Effect spitEffect;

	// Token: 0x040023D7 RID: 9175
	public new LevelProperties.Veggies.Potato properties;

	// Token: 0x040023D8 RID: 9176
	public float hp;

	// Token: 0x040023D9 RID: 9177
	public DamageDealer damageDealer;

	// Token: 0x040023DA RID: 9178
	public bool didShoot = true;

	// Token: 0x040023DB RID: 9179
	public bool projectileParryFlag;

	// Token: 0x02000FF6 RID: 4086
	public enum State
	{
		// Token: 0x0400725B RID: 29275
		Incomplete,
		// Token: 0x0400725C RID: 29276
		Complete
	}

	// Token: 0x02000FF7 RID: 4087
	// (Invoke) Token: 0x060076D3 RID: 30419
	public delegate void OnDamageTakenHandler(float damage);
}
