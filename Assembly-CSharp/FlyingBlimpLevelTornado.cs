using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class FlyingBlimpLevelTornado : AbstractCollidableObject
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00016CB6 File Offset: 0x00014EB6
	// (set) Token: 0x06001AD6 RID: 6870 RVA: 0x00016CBE File Offset: 0x00014EBE
	public FlyingBlimpLevelTornado.State state { get; set; }

	// Token: 0x06001AD7 RID: 6871 RVA: 0x00016CC7 File Offset: 0x00014EC7
	public void Init(Vector2 pos, AbstractPlayerController player, LevelProperties.FlyingBlimp.Tornado properties)
	{
		base.transform.position = pos;
		this.player = player;
		this.properties = properties;
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x00016CE8 File Offset: 0x00014EE8
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.state = FlyingBlimpLevelTornado.State.Alive;
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x00016CFC File Offset: 0x00014EFC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000A9BBC File Offset: 0x000A7DBC
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		while (base.transform.position.x >= -1280f)
		{
			t += CupheadTime.FixedDelta;
			Vector2 homingDirection = this.player.transform.position - base.transform.position;
			Vector2 homingVelocity = homingDirection * this.properties.homingSpeed;
			float velocity = homingVelocity.y;
			if (base.transform.position.x > this.player.transform.position.x)
			{
				velocity = Mathf.Lerp(this.properties.moveSpeed, homingVelocity.y, 1f);
			}
			else if (this.state != FlyingBlimpLevelTornado.State.Dead)
			{
				velocity = homingVelocity.y;
			}
			else
			{
				velocity = 0f;
			}
			base.transform.AddPosition(-this.properties.moveSpeed * CupheadTime.FixedDelta, velocity * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x00016D1A File Offset: 0x00014F1A
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040015A2 RID: 5538
	public LevelProperties.FlyingBlimp.Tornado properties;

	// Token: 0x040015A3 RID: 5539
	public AbstractPlayerController player;

	// Token: 0x040015A4 RID: 5540
	public float movementSpeed;

	// Token: 0x040015A5 RID: 5541
	public DamageDealer damageDealer;

	// Token: 0x02000C9D RID: 3229
	public enum State
	{
		// Token: 0x04005B0F RID: 23311
		Alive,
		// Token: 0x04005B10 RID: 23312
		Dead
	}
}
