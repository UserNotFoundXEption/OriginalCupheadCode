using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class DevilLevelPitchforkBouncingProjectile : AbstractProjectile
{
	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06001513 RID: 5395 RVA: 0x00011EC1 File Offset: 0x000100C1
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06001514 RID: 5396 RVA: 0x00011EC8 File Offset: 0x000100C8
	// (set) Token: 0x06001515 RID: 5397 RVA: 0x00011ED0 File Offset: 0x000100D0
	public int BouncesRemaining { get; set; }

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06001516 RID: 5398 RVA: 0x00011ED9 File Offset: 0x000100D9
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x0009B228 File Offset: 0x00099428
	public DevilLevelPitchforkBouncingProjectile Create(Vector2 pos, float attackDelay, float speed, float angle, int numBounces, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkBouncingProjectile devilLevelPitchforkBouncingProjectile = this.InstantiatePrefab<DevilLevelPitchforkBouncingProjectile>();
		devilLevelPitchforkBouncingProjectile.transform.position = pos;
		devilLevelPitchforkBouncingProjectile.attackDelay = attackDelay;
		devilLevelPitchforkBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle);
		devilLevelPitchforkBouncingProjectile.BouncesRemaining = numBounces;
		devilLevelPitchforkBouncingProjectile.parent = parent;
		devilLevelPitchforkBouncingProjectile.state = DevilLevelPitchforkBouncingProjectile.State.Idle;
		devilLevelPitchforkBouncingProjectile.waitTime = waitTime;
		devilLevelPitchforkBouncingProjectile.StartCoroutine(devilLevelPitchforkBouncingProjectile.main_cr());
		devilLevelPitchforkBouncingProjectile.animator.SetFloat("Variation", (float)Random.Range(0, 3) / 2f);
		return devilLevelPitchforkBouncingProjectile;
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0009B2B4 File Offset: 0x000994B4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (base.CanParry)
		{
			LevelPlayerParryController component = hit.GetComponent<LevelPlayerParryController>();
			if (component != null && component.State == LevelPlayerParryController.ParryState.Parrying)
			{
				return;
			}
		}
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x00011EDC File Offset: 0x000100DC
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x0009B308 File Offset: 0x00099508
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (!base.dead && this.state != DevilLevelPitchforkBouncingProjectile.State.Idle)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			if (this.velocity == Vector2.zero)
			{
				this.bounceTime += CupheadTime.FixedDelta;
				if (this.bounceTime > 0.0833333358f)
				{
					this.velocity = this.velocityOld;
				}
			}
			float radius = base.GetComponent<CircleCollider2D>().radius;
			if (this.BouncesRemaining > 0)
			{
				if ((this.velocity.x < 0f && base.transform.position.x < (float)Level.Current.Left + radius) || (this.velocity.x > 0f && base.transform.position.x > (float)Level.Current.Right - radius))
				{
					if (this.bounceTime == 0f)
					{
						base.animator.Play("BounceWall");
						this.BounceSFX();
						this.velocityOld = this.velocity;
						this.velocity = Vector2.zero;
					}
					else if (this.bounceTime > 0.0833333358f)
					{
						this.BouncesRemaining--;
						this.velocity.x = this.velocity.x * -1f;
						this.bounceTime = 0f;
					}
				}
				if (this.velocity.y > 0f && base.transform.position.y > (float)Level.Current.Ceiling + radius)
				{
					if (this.bounceTime == 0f)
					{
						base.animator.Play("BounceGround");
						this.BounceSFX();
						this.velocityOld = this.velocity;
						this.velocity = Vector2.zero;
					}
					else if (this.bounceTime > 0.0833333358f)
					{
						this.BouncesRemaining--;
						this.velocity.y = this.velocity.y * -1f;
						this.bounceTime = 0f;
					}
				}
			}
			if (this.velocity.y < 0f && base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				if (this.bounceTime == 0f)
				{
					base.animator.Play("BounceGround");
					this.BounceSFX();
					this.velocityOld = this.velocity;
					this.velocity = Vector2.zero;
					if (base.CanParry)
					{
						this.bounceEffectPink.Create(base.transform.position);
					}
					else
					{
						this.bounceEffect.Create(base.transform.position);
					}
				}
				else if (this.bounceTime > 0.0833333358f)
				{
					this.BouncesRemaining--;
					this.velocity.y = this.velocity.y * -1f;
					this.bounceTime = 0f;
				}
			}
		}
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x0009B678 File Offset: 0x00099878
	public IEnumerator main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		base.animator.SetTrigger("Continue");
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, this.attackDelay);
		this.state = DevilLevelPitchforkBouncingProjectile.State.Attacking;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			Effect selectedSparkle = (!base.CanParry) ? this.blueSparkle : this.pinkSparkle;
			Effect inst = selectedSparkle.Create(base.transform.position);
			SpriteRenderer r = inst.GetComponent<SpriteRenderer>();
			r.sortingLayerName = "Projectiles";
			r.sortingOrder = -1;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x00011EFB File Offset: 0x000100FB
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x00011F0E File Offset: 0x0001010E
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("IsPink", parryable);
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00011F28 File Offset: 0x00010128
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.BouncesRemaining = 0;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x00011F38 File Offset: 0x00010138
	public void BounceSFX()
	{
		AudioManager.Play("devil_projectile_bounce");
		this.emitAudioFromObject.Add("devil_projectile_bounce");
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x00011F54 File Offset: 0x00010154
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.blueSparkle = null;
		this.pinkSparkle = null;
		this.bounceEffect = null;
		this.bounceEffectPink = null;
	}

	// Token: 0x04001141 RID: 4417
	public const string ProjectilesLayerName = "Projectiles";

	// Token: 0x04001142 RID: 4418
	public const int VariationMax = 3;

	// Token: 0x04001143 RID: 4419
	public const float BounceTimeThreshold = 0.0833333358f;

	// Token: 0x04001144 RID: 4420
	public DevilLevelPitchforkBouncingProjectile.State state;

	// Token: 0x04001146 RID: 4422
	public float attackDelay;

	// Token: 0x04001147 RID: 4423
	public Vector2 velocity;

	// Token: 0x04001148 RID: 4424
	public Vector2 velocityOld;

	// Token: 0x04001149 RID: 4425
	public DevilLevelSittingDevil parent;

	// Token: 0x0400114A RID: 4426
	public float waitTime;

	// Token: 0x0400114B RID: 4427
	public bool waitTimeUp;

	// Token: 0x0400114C RID: 4428
	public float bounceTime;

	// Token: 0x0400114D RID: 4429
	[SerializeField]
	public Effect blueSparkle;

	// Token: 0x0400114E RID: 4430
	[SerializeField]
	public Effect pinkSparkle;

	// Token: 0x0400114F RID: 4431
	[SerializeField]
	public Effect bounceEffect;

	// Token: 0x04001150 RID: 4432
	[SerializeField]
	public Effect bounceEffectPink;

	// Token: 0x02000B56 RID: 2902
	public enum State
	{
		// Token: 0x04005310 RID: 21264
		Idle,
		// Token: 0x04005311 RID: 21265
		Attacking
	}
}
