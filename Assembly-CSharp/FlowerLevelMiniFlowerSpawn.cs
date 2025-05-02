using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class FlowerLevelMiniFlowerSpawn : AbstractCollidableObject
{
	// Token: 0x0600191D RID: 6429 RVA: 0x000A5780 File Offset: 0x000A3980
	public void OnMiniFlowerSpawn(FlowerLevelFlower parent, LevelProperties.Flower.EnemyPlants properties)
	{
		this.properties = properties;
		this.currentSpeed = this.properties.miniFlowerMovmentSpeed;
		this.currentHP = (float)this.properties.miniFlowerPlantHP;
		this.parent = parent;
		this.parent.OnDeathEvent += this.HandleEnd;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x000A57E4 File Offset: 0x000A39E4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			if (this.isDead)
			{
				return;
			}
			this.isDead = true;
			this.parent.OnMiniFlowerDeath();
			base.animator.SetTrigger("OnDeath");
			this.explosion.GetComponent<Animator>().SetInteger("Variant", 1);
			this.explosion.GetComponent<Animator>().SetTrigger("OnDeath");
			base.GetComponent<Collider2D>().enabled = false;
			base.StartCoroutine(this.die_cr());
			this.currentSpeed = 0;
			this.explosion.Rotate(Vector3.forward, (float)Random.Range(0, 360));
		}
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000A58AC File Offset: 0x000A3AAC
	public void SpawnPetals()
	{
		base.GetComponent<Collider2D>().enabled = false;
		Vector3 vector = this.spawnPoint.transform.position + Vector3.up * (float)Random.Range(-10, 10);
		if (!this.isFriendly)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
			gameObject.GetComponent<Animator>().Play("PetalA_Red", Random.Range(0, 1));
			base.StartCoroutine(this.fade_cr(gameObject, 0.8f, false));
			gameObject = Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
			gameObject.GetComponent<Animator>().Play("PetalB_Red_Loop");
			base.StartCoroutine(this.fade_cr(gameObject, 1f, false));
		}
		else
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
			gameObject2.GetComponent<Animator>().Play("PetalA_Blue", Random.Range(0, 1));
			base.StartCoroutine(this.fade_cr(gameObject2, 0.8f, false));
			gameObject2 = Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
			gameObject2.GetComponent<Animator>().Play("PetalB_Blue_Loop");
			base.StartCoroutine(this.fade_cr(gameObject2, 1f, true));
		}
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x000A5A10 File Offset: 0x000A3C10
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!this.isAttacking && this.isActive)
			{
				float num = Mathf.Sin(this.attackTime * (float)this.currentSpeed / 3f);
				num = Mathf.Clamp(num, -2f, 2f);
				this.attackTime += CupheadTime.FixedDelta;
				Vector3 position = Vector3.Lerp(base.transform.position, this.pivotPoint + this.flightDirection * num * 4000f * CupheadTime.FixedDelta, 0.03f * CupheadTime.GlobalSpeed);
				base.transform.position = position;
				float num2 = 15f * Mathf.Sin(num) * -Mathf.Sign(this.flightDirection.x);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(0f, 0f, num2), 8f);
			}
			else
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(0f, 0f, 0f), 10f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000A5A2C File Offset: 0x000A3C2C
	public IEnumerator die_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, true, true);
		base.GetComponent<SpriteRenderer>().enabled = false;
		yield break;
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x000A5A48 File Offset: 0x000A3C48
	public IEnumerator fade_cr(GameObject petal, float duration, bool lastPetal = false)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		SpriteRenderer petalSprite = petal.GetComponent<SpriteRenderer>();
		float currentTime = duration;
		float pct = currentTime / duration;
		while (pct >= 0f)
		{
			Color c = petalSprite.material.color;
			c.a = pct;
			petalSprite.material.color = c;
			petalSprite.transform.position += Vector3.down * 100f * CupheadTime.FixedDelta;
			currentTime -= CupheadTime.FixedDelta;
			pct = currentTime / duration;
			yield return wait;
		}
		Object.Destroy(petal);
		if (lastPetal)
		{
			this.Die();
		}
		yield break;
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x00015752 File Offset: 0x00013952
	public void FriendlyFireDamage()
	{
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x00015754 File Offset: 0x00013954
	public void HandleEnd()
	{
		if (this.isFriendly)
		{
			base.GetComponent<Collider2D>().enabled = false;
			this.StopAllCoroutines();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x0001577E File Offset: 0x0001397E
	public void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000A5A78 File Offset: 0x000A3C78
	public IEnumerator initialFlight_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < this.pivotPoint.y)
		{
			base.transform.position += this.flightDirection * (float)this.currentSpeed * CupheadTime.GlobalSpeed;
			yield return wait;
		}
		this.isActive = true;
		this.attackTime = 0f;
		if (base.transform.position.x < this.pivotPoint.x)
		{
			this.flightDirection = Vector3.right * (float)this.currentSpeed;
		}
		else
		{
			this.flightDirection = Vector3.left * (float)this.currentSpeed;
		}
		base.StartCoroutine(this.attackDelay_cr());
		yield return wait;
		yield break;
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x000A5A94 File Offset: 0x000A3C94
	public IEnumerator attackDelay_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(this.properties.miniFlowerShootDelay.min, this.properties.miniFlowerShootDelay.max));
			if (!this.isAttacking)
			{
				base.animator.SetTrigger("OnAttack");
				this.isAttacking = true;
			}
		}
		yield break;
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000A5AB0 File Offset: 0x000A3CB0
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.isFriendly = false;
		this.flightDirection = Vector3.up;
		this.pivotPoint = new Vector3((float)Level.Current.Left + (float)Level.Current.Width / 2.5f, (float)(Level.Current.Ceiling - Level.Current.Height / 8), 0f);
		base.Awake();
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x0001579D File Offset: 0x0001399D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.damageReceiver.enabled = this.isAttacking;
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000A5B48 File Offset: 0x000A3D48
	public override void OnDestroy()
	{
		AudioManager.Play("flower_minion_simple_deathpop_low");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_low");
		this.StopAllCoroutines();
		this.parent.OnDeathEvent -= this.HandleEnd;
		base.OnDestroy();
		this.bulletPrefab = null;
		this.petalA = null;
		this.petalB = null;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000157C6 File Offset: 0x000139C6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000157E4 File Offset: 0x000139E4
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.initialFlight_cr());
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x000A5BA8 File Offset: 0x000A3DA8
	public void StartedShooting()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.bulletPrefab, this.spawnPoint.transform.position, Quaternion.identity);
		if (this.isFriendly)
		{
			gameObject.GetComponent<FlowerLevelMiniFlowerBullet>().OnBulletSpawned(this.parent.attackPoint.transform.position, this.properties.miniFlowerProjectileSpeed, (float)this.properties.miniFlowerProjectileDamage, true);
		}
		else
		{
			gameObject.GetComponent<FlowerLevelMiniFlowerBullet>().OnBulletSpawned(PlayerManager.GetNext().center, this.properties.miniFlowerProjectileSpeed, (float)this.properties.miniFlowerProjectileDamage, false);
		}
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x000157F3 File Offset: 0x000139F3
	public void EndedShooting()
	{
		this.isAttacking = false;
	}

	// Token: 0x04001439 RID: 5177
	public const float easingValue = 0.03f;

	// Token: 0x0400143A RID: 5178
	public const float strength = 4000f;

	// Token: 0x0400143B RID: 5179
	public float attackTime;

	// Token: 0x0400143C RID: 5180
	public float currentHP;

	// Token: 0x0400143D RID: 5181
	public int currentSpeed;

	// Token: 0x0400143E RID: 5182
	public bool isFriendly;

	// Token: 0x0400143F RID: 5183
	public bool isAttacking;

	// Token: 0x04001440 RID: 5184
	public bool isActive;

	// Token: 0x04001441 RID: 5185
	public Vector3 flightDirection;

	// Token: 0x04001442 RID: 5186
	public Vector3 pivotPoint;

	// Token: 0x04001443 RID: 5187
	public FlowerLevelFlower parent;

	// Token: 0x04001444 RID: 5188
	[SerializeField]
	public GameObject bulletPrefab;

	// Token: 0x04001445 RID: 5189
	[SerializeField]
	public GameObject spawnPoint;

	// Token: 0x04001446 RID: 5190
	[SerializeField]
	public Transform explosion;

	// Token: 0x04001447 RID: 5191
	[SerializeField]
	public GameObject petalA;

	// Token: 0x04001448 RID: 5192
	[SerializeField]
	public GameObject petalB;

	// Token: 0x04001449 RID: 5193
	public LevelProperties.Flower.EnemyPlants properties;

	// Token: 0x0400144A RID: 5194
	public DamageDealer damageDealer;

	// Token: 0x0400144B RID: 5195
	public DamageReceiver damageReceiver;

	// Token: 0x0400144C RID: 5196
	public bool isDead;
}
