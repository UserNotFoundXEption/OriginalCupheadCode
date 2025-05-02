using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class BatLevelCross : AbstractCollidableObject
{
	// Token: 0x060010B6 RID: 4278 RVA: 0x0000E0EE File Offset: 0x0000C2EE
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00090E4C File Offset: 0x0008F04C
	public void Init(Vector2 pos, LevelProperties.Bat.CrossToss properties, int maxCount, AbstractPlayerController player)
	{
		base.transform.position = pos;
		this.startPos = pos;
		this.properties = properties;
		this.maxCount = maxCount;
		this.player = player;
		this.FindPlayer();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0000E0FB File Offset: 0x0000C2FB
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0000E113 File Offset: 0x0000C313
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0000E12A File Offset: 0x0000C32A
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0000E141 File Offset: 0x0000C341
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0000E158 File Offset: 0x0000C358
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x00090EA0 File Offset: 0x0008F0A0
	public IEnumerator move_cr()
	{
		int count = 0;
		bool startAgain = false;
		while (count < this.maxCount)
		{
			if (!this.goBack)
			{
				base.transform.position += this.velocity * this.properties.projectileSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				startAgain = false;
				Vector2 vector = base.transform.position;
				vector = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.projectileSpeed * CupheadTime.Delta);
				base.transform.position = vector;
				if (base.transform.position == this.startPos && !startAgain)
				{
					count++;
					this.goBack = false;
					this.FindPlayer();
					startAgain = true;
				}
			}
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x00090EBC File Offset: 0x0008F0BC
	public void FindPlayer()
	{
		float num = this.player.transform.position.x - base.transform.position.x;
		float num2 = this.player.transform.position.y - base.transform.position.y;
		float value = Mathf.Atan2(num2, num) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
		this.velocity = base.transform.right;
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0000E16F File Offset: 0x0000C36F
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000D99 RID: 3481
	public LevelProperties.Bat.CrossToss properties;

	// Token: 0x04000D9A RID: 3482
	public AbstractPlayerController player;

	// Token: 0x04000D9B RID: 3483
	public DamageDealer damageDealer;

	// Token: 0x04000D9C RID: 3484
	public Vector3 velocity;

	// Token: 0x04000D9D RID: 3485
	public Vector3 startPos;

	// Token: 0x04000D9E RID: 3486
	public int maxCount;

	// Token: 0x04000D9F RID: 3487
	public bool goBack;
}
