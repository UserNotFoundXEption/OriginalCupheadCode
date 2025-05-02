using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class FlyingBirdLevelBirdFeather : AbstractProjectile
{
	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060019A7 RID: 6567 RVA: 0x00015E53 File Offset: 0x00014053
	// (set) Token: 0x060019A8 RID: 6568 RVA: 0x00015E5B File Offset: 0x0001405B
	public float Speed { get; set; }

	// Token: 0x060019A9 RID: 6569 RVA: 0x00015E64 File Offset: 0x00014064
	public virtual AbstractProjectile Init(float speed)
	{
		this.Speed = speed;
		base.ResetLifetime();
		base.ResetDistance();
		return this;
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000A6E5C File Offset: 0x000A505C
	public override void Update()
	{
		base.Update();
		base.transform.position += -base.transform.right * this.Speed * CupheadTime.Delta;
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x00015E7A File Offset: 0x0001407A
	public void OnEnable()
	{
		this.DamagesType.OnlyPlayer();
		this.CollisionDeath.OnlyPlayer();
		this.SetCollider(true);
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x00015E9A File Offset: 0x0001409A
	public void OnDisable()
	{
		this.SetCollider(false);
		this.StopAllCoroutines();
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x00015EA9 File Offset: 0x000140A9
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00015EC7 File Offset: 0x000140C7
	public void SetCollider(bool c)
	{
		base.GetComponent<BoxCollider2D>().enabled = c;
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000A6EB0 File Offset: 0x000A50B0
	public IEnumerator effect_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.3f));
		for (;;)
		{
			this.effectPrefab.Create(this.effectRoot.position);
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
		}
		yield break;
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x00015ED5 File Offset: 0x000140D5
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00015EE4 File Offset: 0x000140E4
	public override void OnParryDie()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x00015EEC File Offset: 0x000140EC
	public override void OnDieDistance()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x00015EF4 File Offset: 0x000140F4
	public override void OnDieLifetime()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x00015EFC File Offset: 0x000140FC
	public override void OnDieAnimationComplete()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x040014A4 RID: 5284
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x040014A5 RID: 5285
	[SerializeField]
	public Transform effectRoot;
}
