using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class DevilLevelPitchforkSpinnerProjectile : AbstractProjectile
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06001544 RID: 5444 RVA: 0x00012118 File Offset: 0x00010318
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x0009BDA8 File Offset: 0x00099FA8
	public DevilLevelPitchforkSpinnerProjectile Create(Vector2 pos, float maxSpeed, float acceleration, float homingDuration, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkSpinnerProjectile devilLevelPitchforkSpinnerProjectile = this.InstantiatePrefab<DevilLevelPitchforkSpinnerProjectile>();
		devilLevelPitchforkSpinnerProjectile.transform.position = pos;
		devilLevelPitchforkSpinnerProjectile.homingDuration = homingDuration;
		devilLevelPitchforkSpinnerProjectile.startingY = pos.y;
		devilLevelPitchforkSpinnerProjectile.parent = parent;
		devilLevelPitchforkSpinnerProjectile.waitTime = waitTime;
		devilLevelPitchforkSpinnerProjectile.homingMaxSpeed = maxSpeed;
		devilLevelPitchforkSpinnerProjectile.homingAcceleration = acceleration;
		devilLevelPitchforkSpinnerProjectile.StartCoroutine(devilLevelPitchforkSpinnerProjectile.main_cr());
		devilLevelPitchforkSpinnerProjectile.SetParryable(true);
		devilLevelPitchforkSpinnerProjectile.animator.SetBool("IsPink", true);
		devilLevelPitchforkSpinnerProjectile.OrbitStartSFX();
		return devilLevelPitchforkSpinnerProjectile;
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x0001211F File Offset: 0x0001031F
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x0001213E File Offset: 0x0001033E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x0009BE2C File Offset: 0x0009A02C
	public override void FixedUpdate()
	{
		if (!this.waitTimeUp)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		base.transform.SetPosition(null, new float?(this.startingY + Mathf.Sin(this.t * 3.14159274f * 2f / 1.5f) * 10f), null);
		if (Mathf.Abs(base.transform.position.x) > 1500f)
		{
			Object.Destroy(base.gameObject);
		}
		base.Update();
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x0009BED8 File Offset: 0x0009A0D8
	public IEnumerator main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<GroundHomingMovement>().EnableHoming = false;
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		this.waitTimeUp = true;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("StartAtHalf", Rand.Bool());
		GroundHomingMovement homingMovement = base.GetComponent<GroundHomingMovement>();
		homingMovement.maxSpeed = this.homingMaxSpeed;
		homingMovement.acceleration = this.homingAcceleration;
		homingMovement.bounceEnabled = false;
		homingMovement.destroyOffScreen = false;
		homingMovement.TrackingPlayer = PlayerManager.GetNext();
		homingMovement.EnableHoming = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<GroundHomingMovement>().EnableHoming = true;
		yield return CupheadTime.WaitForSeconds(this, this.homingDuration);
		base.GetComponent<GroundHomingMovement>().EnableHoming = false;
		yield break;
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x0001215C File Offset: 0x0001035C
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
		this.OrbitStopSFX();
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00012175 File Offset: 0x00010375
	public override void OnParry(AbstractPlayerController player)
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x0001218F File Offset: 0x0001038F
	public void OrbitStartSFX()
	{
		if (!this.SpinSFXPlaying)
		{
			AudioManager.PlayLoop("devil_orbit_projectile");
			this.emitAudioFromObject.Add("devil_orbit_projectile");
			this.SpinSFXPlaying = true;
		}
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x000121BD File Offset: 0x000103BD
	public void OrbitStopSFX()
	{
		AudioManager.Stop("devil_orbit_projectile");
		this.SpinSFXPlaying = false;
	}

	// Token: 0x04001167 RID: 4455
	public const float SIN_HEIGHT = 10f;

	// Token: 0x04001168 RID: 4456
	public const float SIN_PERIOD = 1.5f;

	// Token: 0x04001169 RID: 4457
	public const float DESTROY_X = 1500f;

	// Token: 0x0400116A RID: 4458
	public float waitTime;

	// Token: 0x0400116B RID: 4459
	public float homingDuration;

	// Token: 0x0400116C RID: 4460
	public float homingMaxSpeed;

	// Token: 0x0400116D RID: 4461
	public float homingAcceleration;

	// Token: 0x0400116E RID: 4462
	public float startingY;

	// Token: 0x0400116F RID: 4463
	public float t;

	// Token: 0x04001170 RID: 4464
	public bool waitTimeUp;

	// Token: 0x04001171 RID: 4465
	public bool SpinSFXPlaying;

	// Token: 0x04001172 RID: 4466
	public DevilLevelSittingDevil parent;
}
