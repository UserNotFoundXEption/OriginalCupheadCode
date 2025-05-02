using System;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class MouseLevelCherryBombProjectile : AbstractProjectile
{
	// Token: 0x06001FCE RID: 8142 RVA: 0x000B69B8 File Offset: 0x000B4BB8
	public MouseLevelCherryBombProjectile Create(Vector2 pos, Vector2 velocity, float gravity, float childSpeed)
	{
		MouseLevelCherryBombProjectile mouseLevelCherryBombProjectile = this.InstantiatePrefab<MouseLevelCherryBombProjectile>();
		mouseLevelCherryBombProjectile.transform.position = pos;
		mouseLevelCherryBombProjectile.velocity = velocity;
		mouseLevelCherryBombProjectile.gravity = gravity;
		mouseLevelCherryBombProjectile.childSpeed = childSpeed;
		mouseLevelCherryBombProjectile.state = MouseLevelCherryBombProjectile.State.Moving;
		return mouseLevelCherryBombProjectile;
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000B69FC File Offset: 0x000B4BFC
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.state == MouseLevelCherryBombProjectile.State.Moving)
		{
			if (base.transform.position.y < (float)Level.Current.Ground + 60f)
			{
				this.state = MouseLevelCherryBombProjectile.State.Dead;
				base.animator.SetTrigger("OnExplode");
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x0001AE27 File Offset: 0x00019027
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x0001AE50 File Offset: 0x00019050
	public void Explode()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000B6AAC File Offset: 0x000B4CAC
	public void SpawnChildren()
	{
		this.cloud.Create(new Vector3(base.transform.position.x + 20f, base.transform.position.y + 200f), new Vector3(0.52f, 0.52f, 0.52f));
		BasicProjectile basicProjectile = this.childProjectile.Create(base.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(0.6f, 0.6f), -this.childSpeed);
		basicProjectile.GetComponent<Animator>().SetBool("isRight", false);
		BasicProjectile basicProjectile2 = this.childProjectile.Create(base.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(-0.6f, -0.6f), this.childSpeed);
		basicProjectile2.GetComponent<Animator>().SetBool("isRight", true);
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x0001AE5D File Offset: 0x0001905D
	public override void Die()
	{
		this.Explode();
		base.Die();
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x0001AE6B File Offset: 0x0001906B
	public void SoundAnimCherryBomExp()
	{
		AudioManager.Play("level_mouse_cannon_bomb_explode");
		this.emitAudioFromObject.Add("level_mouse_cannon_bomb_explode");
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x0001AE87 File Offset: 0x00019087
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.cloud = null;
		this.childProjectile = null;
	}

	// Token: 0x040019E9 RID: 6633
	public const float ChildProjectileScale = 0.6f;

	// Token: 0x040019EA RID: 6634
	[SerializeField]
	public Effect cloud;

	// Token: 0x040019EB RID: 6635
	[SerializeField]
	public BasicProjectile childProjectile;

	// Token: 0x040019EC RID: 6636
	public MouseLevelCherryBombProjectile.State state;

	// Token: 0x040019ED RID: 6637
	public Vector2 velocity;

	// Token: 0x040019EE RID: 6638
	public float gravity;

	// Token: 0x040019EF RID: 6639
	public float childSpeed;

	// Token: 0x02000DC5 RID: 3525
	public enum State
	{
		// Token: 0x04006397 RID: 25495
		Init,
		// Token: 0x04006398 RID: 25496
		Moving,
		// Token: 0x04006399 RID: 25497
		Dead
	}
}
