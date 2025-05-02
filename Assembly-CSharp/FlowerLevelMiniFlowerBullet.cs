using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class FlowerLevelMiniFlowerBullet : AbstractProjectile
{
	// Token: 0x06001912 RID: 6418 RVA: 0x000A55BC File Offset: 0x000A37BC
	public void OnBulletSpawned(Vector3 target, int speed, float damage, bool friendlyFireDamage = false)
	{
		this.friendlyFire = friendlyFireDamage;
		this.targetDirection = (target - base.transform.position).normalized;
		this.bulletSpeed = speed;
		this.damage = damage;
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x000156FF File Offset: 0x000138FF
	public override void Awake()
	{
		this.initDamage = false;
		base.Awake();
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x0001570E File Offset: 0x0001390E
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x000A560C File Offset: 0x000A380C
	public override void Update()
	{
		if (!this.initDamage)
		{
			this.damageDealer.SetDamage(this.damage);
			this.damageDealer.SetDamageFlags(true, this.friendlyFire, false);
			this.initDamage = true;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000A566C File Offset: 0x000A386C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.targetDirection * (float)this.bulletSpeed * CupheadTime.FixedDelta;
		base.transform.up = -this.targetDirection;
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x00015716 File Offset: 0x00013916
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x000A56C8 File Offset: 0x000A38C8
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.friendlyFire && hit.GetComponent<FlowerLevelFlower>() != null)
		{
			base.OnCollisionEnemy(hit, phase);
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x0001573A File Offset: 0x0001393A
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.Die();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000A5714 File Offset: 0x000A3914
	public override void Die()
	{
		this.bulletSpeed = 0;
		base.transform.Rotate(Vector3.forward, 360f);
		this.StopAllCoroutines();
		AudioManager.Play("flower_minion_simple_deathpop_high");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_high");
		base.Die();
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000A5764 File Offset: 0x000A3964
	public IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.puff.Create(base.transform.position).transform.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(this.targetDirection)));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x04001433 RID: 5171
	[SerializeField]
	public Effect puff;

	// Token: 0x04001434 RID: 5172
	public bool friendlyFire;

	// Token: 0x04001435 RID: 5173
	public bool initDamage;

	// Token: 0x04001436 RID: 5174
	public float damage;

	// Token: 0x04001437 RID: 5175
	public int bulletSpeed;

	// Token: 0x04001438 RID: 5176
	public Vector3 targetDirection;
}
