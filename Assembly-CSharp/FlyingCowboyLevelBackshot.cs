using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class FlyingCowboyLevelBackshot : BasicUprightProjectile
{
	// Token: 0x06001AF0 RID: 6896 RVA: 0x000AA0EC File Offset: 0x000A82EC
	public virtual BasicProjectile Create(Vector3 position, float rotation, float speed, float bulletSpeed, float health, float anticipationStartDistance, bool childParryable)
	{
		FlyingCowboyLevelBackshot flyingCowboyLevelBackshot = this.Create(position, rotation, speed) as FlyingCowboyLevelBackshot;
		flyingCowboyLevelBackshot.bulletSpeed = bulletSpeed;
		flyingCowboyLevelBackshot.StartCoroutine(flyingCowboyLevelBackshot.waitToShoot_cr(speed, anticipationStartDistance));
		flyingCowboyLevelBackshot.health = health;
		flyingCowboyLevelBackshot.childParryable = childParryable;
		return flyingCowboyLevelBackshot;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x00016DC3 File Offset: 0x00014FC3
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000AA138 File Offset: 0x000A8338
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.transform.position.x < FlyingCowboyLevelBackshot.AttackPosition)
		{
			base.transform.SetPosition(new float?(FlyingCowboyLevelBackshot.AttackPosition), null, null);
		}
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x00016DEE File Offset: 0x00014FEE
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !base.dead)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000AA190 File Offset: 0x000A8390
	public override void Die()
	{
		float speed = this.Speed;
		base.Die();
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr(speed));
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x000AA1C0 File Offset: 0x000A83C0
	public IEnumerator death_cr(float speed)
	{
		Transform leftWing = this.leftWings.GetRandom<Transform>();
		leftWing.GetComponent<SpriteRenderer>().enabled = true;
		Transform rightWing = this.rightWings.GetRandom<Transform>();
		rightWing.GetComponent<SpriteRenderer>().enabled = true;
		base.animator.Play("Death");
		base.StartCoroutine(this.moveWings_cr(speed, leftWing, rightWing));
		this.SFX_COWGIRL_P1_HorseflySpit();
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Death", 0, true, false, true);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000AA1E4 File Offset: 0x000A83E4
	public IEnumerator moveWings_cr(float speed, Transform leftWing, Transform rightWing)
	{
		Vector3 wingSpeedLeft = new Vector2(-speed * Random.Range(0.25f, 0.5f), -Random.Range(75f, 125f));
		Vector3 windSpeedRight = new Vector2(-speed * Random.Range(0.25f, 0.5f), -Random.Range(75f, 125f));
		for (;;)
		{
			yield return null;
			Vector3 position = leftWing.position;
			position += wingSpeedLeft * CupheadTime.Delta;
			leftWing.position = position;
			position = rightWing.position;
			position += windSpeedRight * CupheadTime.Delta;
			rightWing.position = position;
		}
		yield break;
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000AA210 File Offset: 0x000A8410
	public IEnumerator waitToShoot_cr(float speed, float anticipationStartDistance)
	{
		float timeToAnticipation = anticipationStartDistance / speed;
		float remainder = MathUtilities.DecimalPart(timeToAnticipation / 1f);
		float offset = 1f - remainder;
		float totalNormalizedTime = timeToAnticipation / 1f + offset + 0.625f;
		base.animator.Update(0f);
		base.animator.Play(0, 0, 0.625f + offset);
		yield return base.animator.WaitForNormalizedTime(this, totalNormalizedTime, "Idle", 0, false, false, true);
		base.animator.Play("AnticipationStart");
		while (base.transform.position.x > -550f)
		{
			yield return null;
		}
		base.animator.SetTrigger("Attack");
		float initialSpeed = this.Speed;
		float decelerationTime = KinematicUtilities.CalculateTimeToChangeVelocity(initialSpeed, 0f, -550f - FlyingCowboyLevelBackshot.AttackPosition);
		float elapsedTime = 0f;
		while (elapsedTime < decelerationTime)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			this.Speed = Mathf.Lerp(initialSpeed, 0f, elapsedTime / decelerationTime);
		}
		this.move = false;
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Attack", 0, true, false, true);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000AA23C File Offset: 0x000A843C
	public void animationEvent_ShootBullet()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float rotation = MathUtils.DirectionToAngle(next.center - this.projectileSpawnPosition.position);
		BasicProjectile basicProjectile = this.projectile.Create(this.projectileSpawnPosition.position, rotation, this.bulletSpeed);
		basicProjectile.SetParryable(this.childParryable);
		basicProjectile.StartCoroutine(this.growBullet(basicProjectile.transform));
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x000AA2B4 File Offset: 0x000A84B4
	public IEnumerator growBullet(Transform transform)
	{
		transform.SetScale(new float?(0.6f), new float?(0.6f), null);
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return wait;
			elapsedTime += wait.totalDelta;
			float scale = Mathf.Lerp(0.6f, 1f, elapsedTime / 0.3f);
			transform.SetScale(new float?(scale), new float?(scale), null);
		}
		yield break;
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x00016E2E File Offset: 0x0001502E
	public void AnimationEvent_SFX_COWGIRL_P1_HorseflySpit()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Spit");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Spit");
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x00016E4A File Offset: 0x0001504A
	public void SFX_COWGIRL_P1_HorseflySpit()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Death");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Death");
	}

	// Token: 0x040015C5 RID: 5573
	public static readonly float AttackPosition = -600f;

	// Token: 0x040015C6 RID: 5574
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x040015C7 RID: 5575
	[SerializeField]
	public Transform projectileSpawnPosition;

	// Token: 0x040015C8 RID: 5576
	[SerializeField]
	public Transform[] leftWings;

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	public Transform[] rightWings;

	// Token: 0x040015CA RID: 5578
	public DamageReceiver damageReceiver;

	// Token: 0x040015CB RID: 5579
	public float bulletSpeed;

	// Token: 0x040015CC RID: 5580
	public float health;

	// Token: 0x040015CD RID: 5581
	public bool childParryable;
}
