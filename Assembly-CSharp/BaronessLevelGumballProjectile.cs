using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class BaronessLevelGumballProjectile : AbstractProjectile
{
	// Token: 0x0600100E RID: 4110 RVA: 0x0008F0B0 File Offset: 0x0008D2B0
	public BaronessLevelGumballProjectile Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		BaronessLevelGumballProjectile baronessLevelGumballProjectile = base.Create() as BaronessLevelGumballProjectile;
		baronessLevelGumballProjectile.velocity = velocity;
		baronessLevelGumballProjectile.transform.position = pos;
		baronessLevelGumballProjectile.gravity = gravity;
		return baronessLevelGumballProjectile;
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0000D99C File Offset: 0x0000BB9C
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_trail_cr());
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0008F0EC File Offset: 0x0008D2EC
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (base.transform.position.y <= -360f)
		{
			this.Die();
		}
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0008F138 File Offset: 0x0008D338
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isDead)
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0000D9B1 File Offset: 0x0000BBB1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0008F1A8 File Offset: 0x0008D3A8
	public IEnumerator spawn_trail_cr()
	{
		for (;;)
		{
			yield return null;
			this.trail.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0000D9CF File Offset: 0x0000BBCF
	public override void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
		base.Die();
		base.animator.SetTrigger("Death");
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
	public void Kill()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0000DA01 File Offset: 0x0000BC01
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.trail = null;
	}

	// Token: 0x04000D0E RID: 3342
	[SerializeField]
	public Effect trail;

	// Token: 0x04000D0F RID: 3343
	public Vector2 velocity;

	// Token: 0x04000D10 RID: 3344
	public float gravity;

	// Token: 0x04000D11 RID: 3345
	public bool isDead;
}
