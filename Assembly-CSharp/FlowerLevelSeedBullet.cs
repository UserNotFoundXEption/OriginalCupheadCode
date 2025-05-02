using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class FlowerLevelSeedBullet : AbstractProjectile
{
	// Token: 0x06001948 RID: 6472 RVA: 0x000A5FF8 File Offset: 0x000A41F8
	public void OnBulletSeedStart(FlowerLevelFlower parent, AbstractPlayerController player, float a, float min, float max)
	{
		base.transform.LookAt2D(player.transform.position);
		base.transform.Rotate(Vector3.forward, 180f);
		this.minSpeed = min;
		this.maxSpeed = max;
		this.accelerationTime = a;
		this.player = player;
		this.parent = parent;
		parent.OnDeathEvent += this.Die;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000158AF File Offset: 0x00013AAF
	public override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000158CD File Offset: 0x00013ACD
	public void LaunchBullet()
	{
		base.StartCoroutine(this.launch_bullet_cr());
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000A6068 File Offset: 0x000A4268
	public IEnumerator launch_bullet_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.LookAt2D(this.player.transform.position);
		base.transform.Rotate(Vector3.forward, 180f);
		for (;;)
		{
			if (this.timePassed < this.accelerationTime)
			{
				this.timePassed += CupheadTime.FixedDelta;
			}
			if (!this.isDead)
			{
				this.speed = this.minSpeed + (this.maxSpeed - this.minSpeed) * this.timePassed;
			}
			if (this.speed > 0f && !this.launched)
			{
				base.animator.SetTrigger("Launch");
				this.launched = true;
				base.StartCoroutine(this.spawn_effect_cr());
			}
			base.transform.position -= base.transform.right * (this.speed * CupheadTime.FixedDelta);
			if (base.transform.position.x < (float)(Level.Current.Left - 100))
			{
				Object.Destroy(base.gameObject);
			}
			if (base.transform.position.y > (float)(Level.Current.Ceiling + 100))
			{
				Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000A6084 File Offset: 0x000A4284
	public IEnumerator spawn_effect_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		Effect puff = Object.Instantiate<Effect>(this.puffPrefab);
		puff.transform.position = this.root.transform.position;
		puff.transform.LookAt2D(this.player.transform.position);
		yield break;
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000158DC File Offset: 0x00013ADC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000A60A0 File Offset: 0x000A42A0
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		FlowerLevelMiniFlowerSpawn component = hit.GetComponent<FlowerLevelMiniFlowerSpawn>();
		if (component != null)
		{
			component.FriendlyFireDamage();
			this.Die();
			base.OnCollisionEnemy(hit, phase);
		}
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x00015905 File Offset: 0x00013B05
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.DeathAudio();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x00015915 File Offset: 0x00013B15
	public override void Die()
	{
		this.isDead = true;
		this.speed = 0f;
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x00015941 File Offset: 0x00013B41
	public void DeathAudio()
	{
		AudioManager.Play("flower_bullet_seed_poof");
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x0001594D File Offset: 0x00013B4D
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
		this.puffPrefab = null;
	}

	// Token: 0x04001466 RID: 5222
	[SerializeField]
	public Effect puffPrefab;

	// Token: 0x04001467 RID: 5223
	[SerializeField]
	public Transform root;

	// Token: 0x04001468 RID: 5224
	public bool isDead;

	// Token: 0x04001469 RID: 5225
	public bool launched;

	// Token: 0x0400146A RID: 5226
	public float speed;

	// Token: 0x0400146B RID: 5227
	public float minSpeed;

	// Token: 0x0400146C RID: 5228
	public float maxSpeed;

	// Token: 0x0400146D RID: 5229
	public float timePassed;

	// Token: 0x0400146E RID: 5230
	public float accelerationTime;

	// Token: 0x0400146F RID: 5231
	public FlowerLevelFlower parent;

	// Token: 0x04001470 RID: 5232
	public AbstractPlayerController player;
}
