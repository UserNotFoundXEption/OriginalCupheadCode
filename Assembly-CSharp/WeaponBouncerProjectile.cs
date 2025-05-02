using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public class WeaponBouncerProjectile : AbstractProjectile
{
	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06003867 RID: 14439 RVA: 0x0002E01E File Offset: 0x0002C21E
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00107188 File Offset: 0x00105388
	public override void Start()
	{
		base.Start();
		if (this.isEx)
		{
			this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
			base.StartCoroutine(this.trail_cr());
		}
		else
		{
			switch (Random.Range(0, 4))
			{
			case 0:
				base.animator.Play("A", 0, Random.Range(0f, 1f));
				break;
			case 1:
				base.animator.Play("B", 0, Random.Range(0f, 1f));
				break;
			case 2:
				base.animator.Play("C", 0, Random.Range(0f, 1f));
				break;
			case 3:
				base.animator.Play("D", 0, Random.Range(0f, 1f));
				break;
			}
		}
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x0010727C File Offset: 0x0010547C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.firstUpdateNew)
		{
			this.firstUpdateNew = false;
			if (this.velocity.y < 0f)
			{
				return;
			}
		}
		if (base.dead)
		{
			return;
		}
		this.UpdateInAir();
		if (!this.isEx && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 100f)) && !CupheadLevelCamera.Current.ContainsPoint(new Vector3(base.transform.position.x, base.transform.position.y - 300f, 0f), new Vector2(150f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00107368 File Offset: 0x00105568
	public void UpdateInAir()
	{
		if (this.timeUntilUnfreeze > 0f)
		{
			this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
			base.transform.position += new Vector3(this.velocity.x * CupheadTime.FixedDelta, 0f, 0f);
		}
		else
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x0010741C File Offset: 0x0010561C
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if ((component == null || !component.canFallThrough) && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x0010746C File Offset: 0x0010566C
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component != null && !component.canFallThrough && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x0002E025 File Offset: 0x0002C225
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.isEx)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x001074BC File Offset: 0x001056BC
	public void HitGround(GameObject hit)
	{
		float num = this.velocity.magnitude * WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio - WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening;
		if (num <= 0f || this.numBounces >= WeaponProperties.LevelWeaponBouncer.Basic.numBounces || this.isEx)
		{
			this.Die();
		}
		else
		{
			this.velocity = this.velocity.normalized * num;
			this.velocity.y = this.velocity.y * -1f;
			this.numBounces++;
			this.timeUntilUnfreeze = 0.0416666679f;
			base.animator.SetTrigger((!Rand.Bool()) ? "Bounce_B" : "Bounce_A");
		}
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x00107580 File Offset: 0x00105780
	public IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailDelay);
			if (base.dead)
			{
				yield break;
			}
			this.trailFxPrefab.Create(base.transform.position + MathUtils.RandomPointInUnitCircle() * this.trailFxMaxOffset);
		}
		yield break;
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x0010759C File Offset: 0x0010579C
	public override void Die()
	{
		base.Die();
		if (this.isEx)
		{
			WeaponArcProjectileExplosion weaponArcProjectileExplosion = this.exExplosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.PlayerId);
			weaponArcProjectileExplosion.DamageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
			MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
			meterScoreTracker.Add(weaponArcProjectileExplosion.DamageDealer);
			AudioManager.Play("player_weapon_bouncer_ex_explosion");
			this.emitAudioFromObject.Add("player_weapon_bouncer_ex_explosion");
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.SetEulerAngles(null, null, new float?((float)Random.Range(0, 360)));
		}
	}

	// Token: 0x04002D55 RID: 11605
	[SerializeField]
	public bool isEx;

	// Token: 0x04002D56 RID: 11606
	[SerializeField]
	public WeaponArcProjectileExplosion exExplosion;

	// Token: 0x04002D57 RID: 11607
	[SerializeField]
	public Effect trailFxPrefab;

	// Token: 0x04002D58 RID: 11608
	[SerializeField]
	public float trailFxMaxOffset;

	// Token: 0x04002D59 RID: 11609
	[SerializeField]
	public float trailDelay;

	// Token: 0x04002D5A RID: 11610
	public float gravity;

	// Token: 0x04002D5B RID: 11611
	public Vector2 velocity;

	// Token: 0x04002D5C RID: 11612
	public WeaponBouncer weapon;

	// Token: 0x04002D5D RID: 11613
	public float bounceRatio;

	// Token: 0x04002D5E RID: 11614
	public float bounceSpeedDampening;

	// Token: 0x04002D5F RID: 11615
	public float timeUntilUnfreeze;

	// Token: 0x04002D60 RID: 11616
	public const float bounceFreezeTime = 0.0416666679f;

	// Token: 0x04002D61 RID: 11617
	public int numBounces;

	// Token: 0x04002D62 RID: 11618
	public bool firstUpdateNew = true;
}
