using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class FlowerLevelChomperSeed : AbstractCollidableObject
{
	// Token: 0x0600188F RID: 6287 RVA: 0x000A4034 File Offset: 0x000A2234
	public void OnChomperStart(FlowerLevelFlower parent, LevelProperties.Flower.EnemyPlants properties)
	{
		AudioManager.Play("flower_plants_chomper");
		this.currentHP = (float)properties.chomperPlantHP;
		this.parent = parent;
		this.parent.OnDeathEvent += this.StartDeath;
		this.explosion = base.transform.GetChild(0);
		int integer = base.animator.GetInteger("MaxVariants");
		base.animator.SetInteger("Variant", Random.Range(0, integer));
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x000A40B0 File Offset: 0x000A22B0
	public IEnumerator grow_cr()
	{
		float pct = 0.3f;
		while (pct < 1f)
		{
			this.chomperSprite.transform.localScale = Vector3.one * pct;
			pct += CupheadTime.Delta * 6f;
			if (pct > 1f)
			{
				pct = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000A40CC File Offset: 0x000A22CC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			AudioManager.Stop("flower_plants_chomper");
			if (this.isDead)
			{
				return;
			}
			this.isDead = true;
			this.StopAllCoroutines();
			base.GetComponent<BoxCollider2D>().enabled = false;
			base.animator.Play("Death");
			base.StartCoroutine(this.die_cr());
			base.StartCoroutine(this.spawnPetals_cr());
		}
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000A4158 File Offset: 0x000A2358
	public IEnumerator spawnPetals_cr()
	{
		Animator child = this.explosion.GetComponent<Animator>();
		child.SetInteger("Variant", 0);
		yield return CupheadTime.WaitForSeconds(this, base.animator.GetCurrentAnimatorStateInfo(0).length / 4f);
		child.Play("Death");
		yield return new WaitForEndOfFrame();
		float delay = child.GetCurrentAnimatorStateInfo(0).length;
		yield return CupheadTime.WaitForSeconds(this, delay / 4f);
		this.SpawnPetals();
		yield return CupheadTime.WaitForSeconds(this, delay);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000A4174 File Offset: 0x000A2374
	public void SpawnPetals()
	{
		Vector3 vector = base.transform.position + Vector3.up * (float)(Random.Range(-10, 10) + 70);
		GameObject gameObject = Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafA", Random.Range(0, 1));
		base.StartCoroutine(this.fade_cr(gameObject, 0.8f, 125f, false));
		gameObject = Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
		gameObject.GetComponent<Animator>().Play("Plant_LeafB");
		base.StartCoroutine(this.fade_cr(gameObject, 1f, 100f, false));
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000A423C File Offset: 0x000A243C
	public IEnumerator die_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, false, true);
		this.explosion.GetComponent<Animator>().Play("Death");
		yield break;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000A4258 File Offset: 0x000A2458
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
			this.Die();
		}
		yield break;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000A4290 File Offset: 0x000A2490
	public void StartDeath()
	{
		AudioManager.Play("flower_minion_simple_deathpop_low");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_low");
		this.StopAllCoroutines();
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.Play("Death");
		base.StartCoroutine(this.die_cr());
		base.StartCoroutine(this.spawnPetals_cr());
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x00014F7C File Offset: 0x0001317C
	public void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x000A42F4 File Offset: 0x000A24F4
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
		base.Awake();
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00014F9B File Offset: 0x0001319B
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x00014FB3 File Offset: 0x000131B3
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x000A4388 File Offset: 0x000A2588
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<FlowerLevelFlowerDamageRegion>() != null)
		{
			DamageDealer.DamageInfo info = new DamageDealer.DamageInfo(1f, DamageDealer.Direction.Neutral, hit.transform.position, DamageDealer.DamageSource.Enemy);
			this.OnDamageTaken(info);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x00014FD1 File Offset: 0x000131D1
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.StartDeath;
		base.OnDestroy();
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x00014FF0 File Offset: 0x000131F0
	public void OnDeath()
	{
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x00014FF2 File Offset: 0x000131F2
	public void SpawnChomper()
	{
		base.animator.Play("Trigger_Plant", 1);
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x00015012 File Offset: 0x00013212
	public void GroundBurstStartAudio()
	{
		AudioManager.Play("flower_ground_pop");
	}

	// Token: 0x040013E8 RID: 5096
	[SerializeField]
	public GameObject petalA;

	// Token: 0x040013E9 RID: 5097
	[SerializeField]
	public GameObject petalB;

	// Token: 0x040013EA RID: 5098
	public float currentHP;

	// Token: 0x040013EB RID: 5099
	public Transform explosion;

	// Token: 0x040013EC RID: 5100
	public FlowerLevelFlower parent;

	// Token: 0x040013ED RID: 5101
	[SerializeField]
	public SpriteRenderer chomperSprite;

	// Token: 0x040013EE RID: 5102
	public DamageDealer damageDealer;

	// Token: 0x040013EF RID: 5103
	public DamageReceiver damageReceiver;

	// Token: 0x040013F0 RID: 5104
	public bool isDead;
}
