using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class ChessQueenLevelEgg : AbstractProjectile
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0000FEF5 File Offset: 0x0000E0F5
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x000966B0 File Offset: 0x000948B0
	public ChessQueenLevelEgg Create(Vector3 position, Vector3 velocity, float gravity, float delay)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.velocity = velocity;
		this.gravity = gravity;
		this.delay = delay;
		this.coll.enabled = false;
		this.rend.flipX = Rand.Bool();
		this.anim.Play(Random.Range(0, 12).ToString());
		return this;
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0000FEFC File Offset: 0x0000E0FC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x00096730 File Offset: 0x00094930
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isDead)
		{
			return;
		}
		if (base.lifetime > this.delay)
		{
			this.rend.sortingLayerName = "Projectiles";
			this.rend.sortingOrder = 0;
			this.rend.color = Color.white;
			this.coll.enabled = true;
		}
		else
		{
			this.rend.color = Color.Lerp(new Color(0.7f, 0.7f, 0.7f, 1f), Color.white, base.lifetime / this.delay);
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		if (base.transform.position.y < (float)Level.Current.Ground + 15f)
		{
			this.HitGround();
		}
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00096858 File Offset: 0x00094A58
	public void HitGround()
	{
		this.StopAllCoroutines();
		base.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground + 15f);
		this.isDead = true;
		this.coll.enabled = false;
		this.explosionRend.flipX = Rand.Bool();
		this.anim.Play((!Rand.Bool()) ? "ExplodeB" : "ExplodeA", 1, 0f);
		this.anim.Update(0f);
	}

	// Token: 0x04000F44 RID: 3908
	public const float GROUND_OFFSET = 15f;

	// Token: 0x04000F45 RID: 3909
	public Vector2 velocity;

	// Token: 0x04000F46 RID: 3910
	public float gravity;

	// Token: 0x04000F47 RID: 3911
	public bool isDead;

	// Token: 0x04000F48 RID: 3912
	public float delay;

	// Token: 0x04000F49 RID: 3913
	[SerializeField]
	public Animator anim;

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04000F4B RID: 3915
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000F4C RID: 3916
	[SerializeField]
	public SpriteRenderer explosionRend;
}
