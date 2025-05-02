using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023E RID: 574
public class FlyingBlimpLevelEnemy : AbstractCollidableObject
{
	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001A6A RID: 6762 RVA: 0x000167CE File Offset: 0x000149CE
	// (set) Token: 0x06001A6B RID: 6763 RVA: 0x000167D6 File Offset: 0x000149D6
	public FlyingBlimpLevelEnemy.State state { get; set; }

	// Token: 0x06001A6C RID: 6764 RVA: 0x000167DF File Offset: 0x000149DF
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x00016815 File Offset: 0x00014A15
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000A8678 File Offset: 0x000A6878
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state == FlyingBlimpLevelEnemy.State.Spawned)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0001682D File Offset: 0x00014A2D
	public void Start()
	{
		this.startPoint = base.transform.position;
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000A86C8 File Offset: 0x000A68C8
	public void Init(LevelProperties.FlyingBlimp properties, Vector3 startPoint, float stopPoint, bool Aparryable, FlyingBlimpLevelBlimpLady parent)
	{
		this.enemyProperties = properties.CurrentState.enemy;
		this.properties = properties;
		this.parent = parent;
		this.startPoint = startPoint;
		this.stopPoint = stopPoint;
		this.parent.OnDeathEvent += this.Die;
		this.parryable = Aparryable;
		base.StartCoroutine(this.emerge_cr());
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x000A8730 File Offset: 0x000A6930
	public void CreatePieces()
	{
		foreach (FlyingBlimpLevelEnemyDeathPart flyingBlimpLevelEnemyDeathPart in this.deathPieces)
		{
			flyingBlimpLevelEnemyDeathPart.CreatePart(base.transform.position, this.properties.CurrentState.gear);
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x00016840 File Offset: 0x00014A40
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Die;
		this.projectilePrefab = null;
		this.parryablePrefab = null;
		this.deathPieces = null;
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x00016874 File Offset: 0x00014A74
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x000A8780 File Offset: 0x000A6980
	public IEnumerator emerge_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.state = FlyingBlimpLevelEnemy.State.Spawned;
		this.hp = (float)this.enemyProperties.hp;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		while (base.transform.position.x > this.stopPoint)
		{
			base.transform.position += base.transform.right * -this.enemyProperties.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield return CupheadTime.WaitForSeconds(this, this.enemyProperties.shotDelay);
		base.animator.Play("Enemy_Attack");
		AudioManager.Play("level_flying_blimp_cannon_ship_fire");
		yield return base.animator.WaitForAnimationToEnd(this, "Enemy_Attack", false, true);
		while (base.transform.position.x <= this.startPoint.x)
		{
			base.transform.position += base.transform.right * this.enemyProperties.speed * CupheadTime.FixedDelta;
			yield return wait;
			if (base.transform.position.x > this.startPoint.x)
			{
				this.Die();
			}
		}
		yield break;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x000A879C File Offset: 0x000A699C
	public void FireSpreadshot()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float num = next.transform.position.x - base.transform.position.x;
		float num2 = next.transform.position.y - base.transform.position.y;
		Effect effect = Object.Instantiate<Effect>(this.bulletEffect);
		effect.transform.position = this.projectileRoot.transform.position;
		effect.GetComponent<Animator>().SetInteger("PickAni", Random.Range(0, 3));
		for (int i = 0; i < this.enemyProperties.numBullets; i++)
		{
			float num3 = this.enemyProperties.spreadAngle.GetFloatAt((float)i / ((float)this.enemyProperties.numBullets - 1f));
			float num4 = this.enemyProperties.spreadAngle.max / 2f;
			num3 -= num4;
			float num5 = Mathf.Atan2(num2, num) * 57.29578f;
			this.animationPicker = Random.Range(0, 3);
			if (next.transform.position.x > base.transform.position.x)
			{
				num5 = (float)((next.transform.position.y <= base.transform.position.y) ? -90 : 90);
			}
			int num6 = this.animationPicker;
			if (num6 != 0)
			{
				if (num6 != 1)
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num5 + num3, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_3");
				}
				else
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num5 + num3, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_2");
				}
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.position, num5 + num3, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000A8A08 File Offset: 0x000A6C08
	public void FireSingle()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float num = next.transform.position.x - base.transform.position.x;
		float num2 = next.transform.position.y - base.transform.position.y;
		float num3 = -3f;
		float num4 = Mathf.Atan2(num2, num) * 57.29578f;
		Effect effect = Object.Instantiate<Effect>(this.bulletEffect);
		effect.transform.position = this.projectileRoot.transform.position;
		effect.GetComponent<Animator>().SetInteger("PickAni", Random.Range(0, 3));
		if (next.transform.position.x > base.transform.position.x)
		{
			num4 = (float)((next.transform.position.y <= base.transform.position.y) ? -90 : 90);
		}
		if (!this.parryable)
		{
			this.animationPicker = Random.Range(0, 3);
		}
		else
		{
			this.animationPicker = Random.Range(0, 2);
		}
		if (!this.parryable)
		{
			int num5 = this.animationPicker;
			if (num5 != 0)
			{
				if (num5 != 1)
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num4 + num3, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_3");
				}
				else
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num4 + num3, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
				}
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.position, num4 + num3, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
		else
		{
			int num6 = this.animationPicker;
			if (num6 != 0)
			{
				this.parryablePrefab.Create(this.projectileRoot.position, num4 + num3, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
			}
			else
			{
				this.parryablePrefab.Create(this.projectileRoot.position, num4 + num3, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x00016892 File Offset: 0x00014A92
	public void FlipEnemy()
	{
		base.GetComponent<SpriteRenderer>().flipX = !base.GetComponent<SpriteRenderer>().flipX;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000A8CD4 File Offset: 0x000A6ED4
	public IEnumerator dying_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		AudioManager.Play("level_flying_blimp_cannon_ship_death");
		base.animator.Play("Enemy_Explode");
		yield return base.animator.WaitForAnimationToEnd(this, "Enemy_Explode", false, true);
		this.Die();
		yield break;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000168AD File Offset: 0x00014AAD
	public void Die()
	{
		this.state = FlyingBlimpLevelEnemy.State.Unspawned;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400152C RID: 5420
	public LevelProperties.FlyingBlimp.Enemy enemyProperties;

	// Token: 0x0400152D RID: 5421
	public LevelProperties.FlyingBlimp properties;

	// Token: 0x0400152E RID: 5422
	public Vector3 startPoint;

	// Token: 0x0400152F RID: 5423
	[SerializeField]
	public Effect bulletEffect;

	// Token: 0x04001530 RID: 5424
	[SerializeField]
	public FlyingBlimpLevelEnemyDeathPart[] deathPieces;

	// Token: 0x04001531 RID: 5425
	[SerializeField]
	public FlyingBlimpLevelEnemyProjectile projectilePrefab;

	// Token: 0x04001532 RID: 5426
	[SerializeField]
	public FlyingBlimpLevelEnemyProjectile parryablePrefab;

	// Token: 0x04001533 RID: 5427
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001534 RID: 5428
	public AbstractPlayerController player;

	// Token: 0x04001535 RID: 5429
	public FlyingBlimpLevelBlimpLady parent;

	// Token: 0x04001536 RID: 5430
	public DamageDealer damageDealer;

	// Token: 0x04001537 RID: 5431
	public DamageReceiver damageReceiver;

	// Token: 0x04001538 RID: 5432
	public float hp;

	// Token: 0x04001539 RID: 5433
	public float stopPoint;

	// Token: 0x0400153A RID: 5434
	public bool parryable;

	// Token: 0x0400153B RID: 5435
	public int animationPicker;

	// Token: 0x02000C85 RID: 3205
	public enum State
	{
		// Token: 0x04005A71 RID: 23153
		Unspawned,
		// Token: 0x04005A72 RID: 23154
		Spawned
	}
}
