using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class FlyingMermaidLevelSkullBubble : BasicSineProjectile
{
	// Token: 0x06001DC3 RID: 7619 RVA: 0x000B1BD0 File Offset: 0x000AFDD0
	public FlyingMermaidLevelSkullBubble CreateBubble(Vector2 pos, float velocity, float sinVelocity, float sinSize, float rotation)
	{
		return base.Create(pos, rotation, velocity, sinVelocity, sinSize) as FlyingMermaidLevelSkullBubble;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000192F5 File Offset: 0x000174F5
	public override void Awake()
	{
		base.Awake();
		this.smallBubbles = new List<Effect>();
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x00019308 File Offset: 0x00017508
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_small_bubbles_cr());
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x000B1BF4 File Offset: 0x000AFDF4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (!this.isDead)
		{
			if (phase != CollisionPhase.Exit)
			{
				this.damageDealer.DealDamage(hit);
			}
			this.isDead = true;
			this.StopAllCoroutines();
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000B1C44 File Offset: 0x000AFE44
	public IEnumerator spawn_small_bubbles_cr()
	{
		while (!this.isDead)
		{
			Vector3 offset = new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), 0f);
			this.smallBubbles.Add(this.smallBubblesPrefab.Create(base.transform.position + offset, new Vector3(this.smallBubblesSize, this.smallBubblesSize, this.smallBubblesSize)));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x0001931D File Offset: 0x0001751D
	public override void Die()
	{
		base.Die();
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000B1C60 File Offset: 0x000AFE60
	public IEnumerator dying_cr()
	{
		base.animator.Play("Pop");
		yield return base.animator.WaitForAnimationToEnd(this, "Pop", false, true);
		while (base.transform.position.y > -660f)
		{
			base.transform.AddPosition(0f, (-this.velocity + this.accumulatedGravity) * CupheadTime.Delta, 0f);
			this.accumulatedGravity += -25f;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x04001868 RID: 6248
	[SerializeField]
	public Effect smallBubblesPrefab;

	// Token: 0x04001869 RID: 6249
	[SerializeField]
	public float smallBubblesSize;

	// Token: 0x0400186A RID: 6250
	public const float GRAVITY = -25f;

	// Token: 0x0400186B RID: 6251
	public const float bubblesOffsetX = 15f;

	// Token: 0x0400186C RID: 6252
	public const float bubblesOffsetY = 15f;

	// Token: 0x0400186D RID: 6253
	public List<Effect> smallBubbles;

	// Token: 0x0400186E RID: 6254
	public float accumulatedGravity;

	// Token: 0x0400186F RID: 6255
	public bool isDead;
}
