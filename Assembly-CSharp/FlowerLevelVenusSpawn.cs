using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class FlowerLevelVenusSpawn : AbstractCollidableObject
{
	// Token: 0x06001954 RID: 6484 RVA: 0x000A60D4 File Offset: 0x000A42D4
	public void OnVenusSpawn(FlowerLevelFlower parent, int hp, float rotSpeed, int moveSpeed, float rotDelay)
	{
		AudioManager.Play("flower_venus_a_chomp");
		this.rotationDelay = rotDelay;
		this.rotationSpeed = rotSpeed;
		this.movementSpeed = moveSpeed;
		this.lockRotation = false;
		this.currentHP = (float)hp;
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
		base.animator.SetInteger("Variant", Random.Range(0, 2));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x0001597C File Offset: 0x00013B7C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			this.Die();
			this.damageReceiver.enabled = false;
		}
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000A6154 File Offset: 0x000A4354
	public IEnumerator spawnPetals_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return CupheadTime.WaitForSeconds(this, base.animator.GetCurrentAnimatorStateInfo(0).length / 4f);
		this.SpawnPetals();
		yield break;
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000A6170 File Offset: 0x000A4370
	public void SpawnPetals()
	{
		Vector3 vector = base.transform.position + Vector3.up * (float)(Random.Range(-10, 10) + 70);
		GameObject gameObject = Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafA", Random.Range(0, 1));
		base.StartCoroutine(this.fade_cr(gameObject, 0.8f, 125f, false));
		gameObject = Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafB");
		base.StartCoroutine(this.fade_cr(gameObject, 1f, 100f, true));
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000A6238 File Offset: 0x000A4438
	public IEnumerator die_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, false, true);
		base.GetComponent<SpriteRenderer>().enabled = false;
		yield break;
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000A6254 File Offset: 0x000A4454
	public IEnumerator fade_cr(GameObject petal, float duration, float speed, bool lastPetal = false)
	{
		SpriteRenderer petalSprite = petal.GetComponent<SpriteRenderer>();
		float currentTime = duration;
		float pct = currentTime / duration;
		while (pct >= 0f)
		{
			Color c = petalSprite.material.color;
			c.a = pct;
			petalSprite.material.color = c;
			petalSprite.transform.position += Vector3.down * speed * CupheadTime.Delta;
			currentTime -= CupheadTime.Delta;
			pct = currentTime / duration;
			yield return null;
		}
		Object.Destroy(petal);
		if (lastPetal)
		{
			this.StopAllCoroutines();
			Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000A628C File Offset: 0x000A448C
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return base.animator.WaitForAnimationToEnd(this, true);
		for (;;)
		{
			if (!this.lockRotation && this.rotationDelay <= 0f)
			{
				Vector3 vector = PlayerManager.GetNext().center - base.transform.position;
				base.transform.right = Vector3.RotateTowards(base.transform.right, -vector.normalized * base.transform.localScale.x, this.rotationSpeed * CupheadTime.FixedDelta, 0f);
				if (base.transform.localScale.x == 1f)
				{
					if (Vector3.Angle(base.transform.right, -vector.normalized) < 5f)
					{
						this.lockRotation = true;
					}
				}
				else if (Vector3.Angle(base.transform.right, -vector.normalized) > 175f)
				{
					this.lockRotation = true;
				}
			}
			base.transform.position -= base.transform.right * (float)this.movementSpeed * CupheadTime.FixedDelta * base.transform.localScale.x;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000A62A8 File Offset: 0x000A44A8
	public void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("flower_minion_simple_deathpop");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop");
		base.StartCoroutine(this.die_cr());
		base.StartCoroutine(this.spawnPetals_cr());
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000159B3 File Offset: 0x00013BB3
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x000159E9 File Offset: 0x00013BE9
	public void Update()
	{
		if (this.rotationDelay > 0f)
		{
			this.rotationDelay -= CupheadTime.Delta;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x00015A28 File Offset: 0x00013C28
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x00015A46 File Offset: 0x00013C46
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.Die();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x00015A56 File Offset: 0x00013C56
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.StartCoroutine(this.offScreenDeath_cr());
		base.OnCollisionCeiling(hit, phase);
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x00015A6D File Offset: 0x00013C6D
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.StartCoroutine(this.offScreenDeath_cr());
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x00015A84 File Offset: 0x00013C84
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		this.StopAllCoroutines();
		base.OnDestroy();
		this.petalA = null;
		this.petalB = null;
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000A630C File Offset: 0x000A450C
	public IEnumerator offScreenDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x00015AB7 File Offset: 0x00013CB7
	public void RandomiseVariant()
	{
		base.animator.SetInteger("Variant", Random.Range(0, 3));
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x00015AD0 File Offset: 0x00013CD0
	public void VenusGrowEndAudio()
	{
	}

	// Token: 0x04001471 RID: 5233
	[SerializeField]
	public GameObject petalA;

	// Token: 0x04001472 RID: 5234
	[SerializeField]
	public GameObject petalB;

	// Token: 0x04001473 RID: 5235
	public bool lockRotation;

	// Token: 0x04001474 RID: 5236
	public float rotationSpeed;

	// Token: 0x04001475 RID: 5237
	public int movementSpeed;

	// Token: 0x04001476 RID: 5238
	public float rotationDelay;

	// Token: 0x04001477 RID: 5239
	public float currentHP;

	// Token: 0x04001478 RID: 5240
	public DamageDealer damageDealer;

	// Token: 0x04001479 RID: 5241
	public DamageReceiver damageReceiver;

	// Token: 0x0400147A RID: 5242
	public FlowerLevelFlower parent;
}
