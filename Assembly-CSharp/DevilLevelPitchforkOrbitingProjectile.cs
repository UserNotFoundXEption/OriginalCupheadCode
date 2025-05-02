using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class DevilLevelPitchforkOrbitingProjectile : AbstractProjectile
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x0600152A RID: 5418 RVA: 0x00011FDF File Offset: 0x000101DF
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x0009BA1C File Offset: 0x00099C1C
	public DevilLevelPitchforkOrbitingProjectile Create(AbstractProjectile target, float angle, float rotationSpeed, float radius, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkOrbitingProjectile devilLevelPitchforkOrbitingProjectile = this.InstantiatePrefab<DevilLevelPitchforkOrbitingProjectile>();
		devilLevelPitchforkOrbitingProjectile.target = target;
		devilLevelPitchforkOrbitingProjectile.angle = angle;
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed;
		devilLevelPitchforkOrbitingProjectile.radius = radius;
		devilLevelPitchforkOrbitingProjectile.parent = parent;
		devilLevelPitchforkOrbitingProjectile.waitTime = waitTime;
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = false;
		return devilLevelPitchforkOrbitingProjectile;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0009BA68 File Offset: 0x00099C68
	public DevilLevelPitchforkOrbitingProjectile Create(AbstractProjectile target, float angle, float rotationSpeed, float radius, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkOrbitingProjectile devilLevelPitchforkOrbitingProjectile = this.InstantiatePrefab<DevilLevelPitchforkOrbitingProjectile>();
		devilLevelPitchforkOrbitingProjectile.target = target;
		devilLevelPitchforkOrbitingProjectile.angle = angle;
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed;
		devilLevelPitchforkOrbitingProjectile.radius = radius;
		devilLevelPitchforkOrbitingProjectile.parent = parent;
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = true;
		return devilLevelPitchforkOrbitingProjectile;
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00011FE6 File Offset: 0x000101E6
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x00012005 File Offset: 0x00010205
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0009BAAC File Offset: 0x00099CAC
	public IEnumerator wait_time_cr()
	{
		yield return new WaitForSeconds(this.waitTime);
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("StartAtHalf", Rand.Bool());
		yield break;
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x0009BAC8 File Offset: 0x00099CC8
	public override void Start()
	{
		base.Start();
		Vector2 vector = this.target.transform.position + MathUtils.AngleToDirection(this.angle) * this.radius;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
		if (!this.waitTimeUp)
		{
			base.GetComponent<Collider2D>().enabled = false;
			base.StartCoroutine(this.wait_time_cr());
		}
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x0009BB5C File Offset: 0x00099D5C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (base.dead)
		{
			return;
		}
		if (this.target == null || this.target.dead)
		{
			this.Die();
			return;
		}
		this.angle += this.rotationSpeed * CupheadTime.FixedDelta;
		Vector2 vector = this.target.transform.position + MathUtils.AngleToDirection(this.angle) * this.radius;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x00012023 File Offset: 0x00010223
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
		this.OrbitStopSFX();
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x0001203C File Offset: 0x0001023C
	public void OrbitStartSFX()
	{
		AudioManager.PlayLoop("devil_orbit_projectile");
		this.emitAudioFromObject.Add("devil_orbit_projectile");
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x00012058 File Offset: 0x00010258
	public void OrbitStopSFX()
	{
		AudioManager.Stop("devil_orbit_projectile");
	}

	// Token: 0x04001158 RID: 4440
	public AbstractProjectile target;

	// Token: 0x04001159 RID: 4441
	public float rotationSpeed;

	// Token: 0x0400115A RID: 4442
	public float radius;

	// Token: 0x0400115B RID: 4443
	public float angle;

	// Token: 0x0400115C RID: 4444
	public float waitTime;

	// Token: 0x0400115D RID: 4445
	public DevilLevelSittingDevil parent;

	// Token: 0x0400115E RID: 4446
	public bool waitTimeUp;
}
