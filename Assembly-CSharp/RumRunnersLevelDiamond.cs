using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class RumRunnersLevelDiamond : AbstractCollidableObject
{
	// Token: 0x0600247D RID: 9341 RVA: 0x0001ED34 File Offset: 0x0001CF34
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x0001ED47 File Offset: 0x0001CF47
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x0001ED5F File Offset: 0x0001CF5F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x0001ED76 File Offset: 0x0001CF76
	public void Die()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x0001ED89 File Offset: 0x0001CF89
	public void SetAttack(bool attack)
	{
		if (attack)
		{
			this.horn.enabled = false;
			this.hornAttack.enabled = true;
		}
		else
		{
			this.horn.enabled = true;
			this.hornAttack.enabled = false;
		}
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x0001EDC6 File Offset: 0x0001CFC6
	public void StartSparkle()
	{
		base.StartCoroutine(this.startSparkle_cr());
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000C41BC File Offset: 0x000C23BC
	public IEnumerator startSparkle_cr()
	{
		Color color = this.sparkle.color;
		color.a = 0f;
		this.sparkle.color = color;
		base.animator.Play("On", 1);
		float elapsedTime = 0f;
		while (elapsedTime < 0.2f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			color.a = Mathf.Lerp(0f, 1f, elapsedTime / 0.2f);
			this.sparkle.color = color;
		}
		yield break;
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x0001EDD5 File Offset: 0x0001CFD5
	public void EndSparkle()
	{
		base.StartCoroutine(this.endSparkle_cr());
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000C41D8 File Offset: 0x000C23D8
	public IEnumerator endSparkle_cr()
	{
		Color color = this.sparkle.color;
		this.sparkle.color = color;
		float elapsedTime = 0f;
		while (elapsedTime < 0.45f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.45f);
			this.sparkle.color = color;
		}
		base.animator.Play("Off", 1);
		yield break;
	}

	// Token: 0x04001E33 RID: 7731
	[SerializeField]
	public SpriteRenderer horn;

	// Token: 0x04001E34 RID: 7732
	[SerializeField]
	public SpriteRenderer hornAttack;

	// Token: 0x04001E35 RID: 7733
	[SerializeField]
	public SpriteRenderer sparkle;

	// Token: 0x04001E36 RID: 7734
	public DamageDealer damageDealer;
}
