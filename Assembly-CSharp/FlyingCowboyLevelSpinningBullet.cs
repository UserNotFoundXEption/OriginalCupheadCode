using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class FlyingCowboyLevelSpinningBullet : AbstractProjectile
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x000ACE64 File Offset: 0x000AB064
	public FlyingCowboyLevelSpinningBullet Create(Vector2 pos, float speed, float rotationSpeed, float rotationRadius, Vector3 direction, bool clockwise, bool parryable)
	{
		FlyingCowboyLevelSpinningBullet flyingCowboyLevelSpinningBullet = this.Create() as FlyingCowboyLevelSpinningBullet;
		flyingCowboyLevelSpinningBullet.child.localPosition = new Vector3(rotationRadius, 0f);
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.bullet_cr(pos, speed, rotationSpeed, direction, clockwise));
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.scale_cr());
		flyingCowboyLevelSpinningBullet.SetParryable(parryable);
		return flyingCowboyLevelSpinningBullet;
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x0001795F File Offset: 0x00015B5F
	public override void Update()
	{
		base.Update();
		this.child.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00017990 File Offset: 0x00015B90
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000179A6 File Offset: 0x00015BA6
	public override void Die()
	{
		this.child.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		base.Die();
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x000ACEC0 File Offset: 0x000AB0C0
	public IEnumerator scale_cr()
	{
		Vector3 initialScale = base.transform.localScale;
		base.transform.localScale = initialScale * 0.75f;
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = Vector3.Lerp(initialScale * 0.75f, initialScale, elapsedTime / 0.3f);
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000ACEDC File Offset: 0x000AB0DC
	public IEnumerator bullet_cr(Vector2 pos, float speed, float rotationSpeed, Vector3 direction, bool clockwise)
	{
		if (!clockwise)
		{
			base.animator.SetFloat("Speed", -1f);
		}
		base.transform.position = pos - this.child.localPosition;
		for (;;)
		{
			base.transform.position += direction * speed * CupheadTime.Delta;
			base.transform.AddEulerAngles(0f, 0f, (float)((!clockwise) ? 1 : -1) * rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040016A6 RID: 5798
	[SerializeField]
	public Transform child;
}
