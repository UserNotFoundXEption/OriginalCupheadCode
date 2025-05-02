using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000234 RID: 564
public class FlyingBirdLevelNursePill : AbstractProjectile
{
	// Token: 0x060019DB RID: 6619 RVA: 0x000A72FC File Offset: 0x000A54FC
	public override void FixedUpdate()
	{
		if (this.gravity)
		{
			if (this.velocity.magnitude < this.properties.pillSpeed)
			{
			}
			this.velocity.y = this.velocity.y - 10f;
		}
		base.FixedUpdate();
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000A734C File Offset: 0x000A554C
	public void InitPill(LevelProperties.FlyingBird.Nurses properties, PlayerId target, bool parryable)
	{
		this.SetParryable(parryable);
		this.target = target;
		this.properties = properties;
		this.velocity = base.transform.up.normalized * properties.pillSpeed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x0001606B File Offset: 0x0001426B
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			this.parryPill.SetActive(true);
		}
		else
		{
			this.normalPill.SetActive(true);
		}
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x00016097 File Offset: 0x00014297
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x000A73A0 File Offset: 0x000A55A0
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			if (base.transform.position.y >= this.properties.pillMaxHeight && !this.gravity)
			{
				this.gravity = true;
				base.StartCoroutine(this.detonate_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x000A73BC File Offset: 0x000A55BC
	public IEnumerator detonate_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.pillExplodeDelay);
		AbstractPlayerController player = PlayerManager.GetPlayer(this.target);
		if (player == null || player.IsDead)
		{
			player = PlayerManager.GetNext();
		}
		base.transform.right = (player.center - base.transform.position).normalized;
		FlyingBirdLevelNursePillProjectile top = this.topHalf.Create(base.transform.position, base.transform.eulerAngles.z, this.properties.bulletSpeed) as FlyingBirdLevelNursePillProjectile;
		FlyingBirdLevelNursePillProjectile bottom = this.bottomHalf.Create(base.transform.position, base.transform.eulerAngles.z + 180f, this.properties.bulletSpeed) as FlyingBirdLevelNursePillProjectile;
		if (base.CanParry)
		{
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.LightPink);
			top.SetParryable(true);
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.DarkPink);
			bottom.SetParryable(true);
		}
		else
		{
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Yellow);
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Blue);
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040014C0 RID: 5312
	[SerializeField]
	public FlyingBirdLevelNursePillProjectile topHalf;

	// Token: 0x040014C1 RID: 5313
	[SerializeField]
	public FlyingBirdLevelNursePillProjectile bottomHalf;

	// Token: 0x040014C2 RID: 5314
	[SerializeField]
	public GameObject normalPill;

	// Token: 0x040014C3 RID: 5315
	[SerializeField]
	public GameObject parryPill;

	// Token: 0x040014C4 RID: 5316
	public bool gravity;

	// Token: 0x040014C5 RID: 5317
	public Vector3 velocity;

	// Token: 0x040014C6 RID: 5318
	public PlayerId target;

	// Token: 0x040014C7 RID: 5319
	public LevelProperties.FlyingBird.Nurses properties;
}
