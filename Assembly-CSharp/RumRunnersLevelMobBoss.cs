using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class RumRunnersLevelMobBoss : AbstractCollidableObject
{
	// Token: 0x060024D8 RID: 9432 RVA: 0x0001F1BB File Offset: 0x0001D3BB
	public override void Awake()
	{
		base.Awake();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000C4E90 File Offset: 0x000C3090
	public void Setup(LevelProperties.RumRunners properties, RumRunnersLevelAnteater anteater, Transform positioner)
	{
		this.properties = properties;
		this.anteater = anteater;
		this.positioner = positioner;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.parryString = new PatternString(properties.CurrentState.boss.bossProjectileParryString, true);
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000C4EF4 File Offset: 0x000C30F4
	public void Begin()
	{
		this.begun = true;
		base.gameObject.SetActive(true);
		this.setActiveDirection(RumRunnersLevelMobBoss.Direction.Attack0);
		base.animator.Update(0f);
		base.StartCoroutine(this.timer_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x0001F1CF File Offset: 0x0001D3CF
	public void LateUpdate()
	{
		if (!this.begun)
		{
			return;
		}
		this.updatePosition();
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000C4F48 File Offset: 0x000C3148
	public IEnumerator shoot_cr()
	{
		LevelProperties.RumRunners.Boss p = this.properties.CurrentState.boss;
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		for (;;)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			this.targetedPlayer = player.id;
			this.targetedPosition = player.center;
			Vector3 bossCenter = this.circleCollider.bounds.center;
			float angle = MathUtils.DirectionToAngle(this.targetedPosition - bossCenter);
			if ((this.shootingRight && this.targetedPosition.x < bossCenter.x) || (!this.shootingRight && this.targetedPosition.x > bossCenter.x))
			{
				base.animator.SetTrigger("Turn");
			}
			this.targetedDirection = this.chooseDirection(angle, true);
			this.setActiveDirection(this.targetedDirection);
			base.animator.SetTrigger("Attack");
			int animatorHash = Animator.StringToHash("AttackMiddle");
			while (this.getAnimatorCurrentStateInfo().shortNameHash != animatorHash)
			{
				yield return null;
			}
			while (this.getAnimatorCurrentStateInfo().normalizedTime < 1f)
			{
				yield return null;
			}
			this.shoot();
			base.animator.SetTrigger("Continue");
			animatorHash = Animator.StringToHash("AttackEnd");
			while (this.getAnimatorCurrentStateInfo().shortNameHash != animatorHash)
			{
				yield return null;
			}
			while (this.getAnimatorCurrentStateInfo().shortNameHash == animatorHash)
			{
				yield return null;
			}
			float totalAttackDelay = p.coinDelay.GetFloatAt(this.minMaxParameter);
			if (totalAttackDelay > 0.6666667f)
			{
				if (totalAttackDelay <= 0.7083333f)
				{
					base.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, 1.2f);
				}
				float waitTime = totalAttackDelay - 0.6666667f;
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
			}
			else
			{
				float num;
				float num2;
				if (totalAttackDelay > 0.5833333f)
				{
					num = 1.2f;
					num2 = 1f;
				}
				else if (totalAttackDelay > 0.5416667f)
				{
					num = 1.2f;
					num2 = 1.33333337f;
				}
				else if (totalAttackDelay > 0.5f)
				{
					num = 1.5f;
					num2 = 1.33333337f;
				}
				else if (totalAttackDelay > 0.458333343f)
				{
					num = 2f;
					num2 = 1.33333337f;
				}
				else
				{
					num = 2f;
					num2 = 2f;
				}
				base.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, num);
				base.animator.SetFloat(RumRunnersLevelMobBoss.EndSpeedParameter, num2);
			}
		}
		yield break;
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000C4F64 File Offset: 0x000C3164
	public IEnumerator timer_cr()
	{
		LevelProperties.RumRunners.Boss p = this.properties.CurrentState.boss;
		float totalTime = p.coinMinMaxTime;
		float elapsedTime = 0f;
		while (elapsedTime < totalTime)
		{
			elapsedTime += CupheadTime.Delta;
			this.minMaxParameter = Mathf.Clamp01(elapsedTime / totalTime);
			yield return null;
		}
		this.minMaxParameter = 1f;
		yield break;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x0001F1E3 File Offset: 0x0001D3E3
	public void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.StopAllCoroutines();
		base.gameObject.SetActive(false);
		this.anteater.RealDeath();
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x0001F215 File Offset: 0x0001D415
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.DealDamage(info.damage);
		if (this.properties.CurrentHealth <= 0f && !this.dead)
		{
			this.die();
		}
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000C4F80 File Offset: 0x000C3180
	public void animationEvent_Flip()
	{
		this.shootingRight = !this.shootingRight;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
		if (PlayerManager.DoesPlayerExist(this.targetedPlayer))
		{
			this.targetedPosition = PlayerManager.GetPlayer(this.targetedPlayer).center;
		}
		Vector3 center = this.circleCollider.bounds.center;
		float angle = MathUtils.DirectionToAngle(this.targetedPosition - center);
		this.targetedDirection = this.chooseDirection(angle, true);
		this.setActiveDirection(this.targetedDirection);
		this.updatePosition();
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000C5038 File Offset: 0x000C3238
	public void shoot()
	{
		Vector3 center;
		if (PlayerManager.DoesPlayerExist(this.targetedPlayer))
		{
			center = PlayerManager.GetPlayer(this.targetedPlayer).center;
		}
		else
		{
			center = this.targetedPosition;
		}
		Vector3 vector = base.transform.TransformPoint(this.projectileRoots[(int)this.targetedDirection]);
		float num = MathUtils.DirectionToAngle(center - vector);
		float num2 = (!this.shootingRight) ? RumRunnersLevelMobBoss.ReferenceAnglesLeft[(int)this.targetedDirection] : RumRunnersLevelMobBoss.ReferenceAnglesRight[(int)this.targetedDirection];
		float num3 = (!this.shootingRight) ? ((num <= 0f) ? (-180f - num) : (180f - num)) : num;
		float num4 = (!this.shootingRight) ? ((num2 <= 0f) ? (-180f - num2) : (180f - num2)) : num2;
		if (Mathf.Abs(num3 - num4) > RumRunnersLevelMobBoss.AcceptableAngleVariance)
		{
			RumRunnersLevelMobBoss.Direction direction = this.chooseDirection(num, true);
			int num5 = direction - this.targetedDirection;
			if (Mathf.Abs(num5) > 1)
			{
				num5 = (int)Mathf.Sign((float)num5);
				this.targetedDirection = (RumRunnersLevelMobBoss.Direction)Mathf.Clamp((int)(this.targetedDirection + num5), 0, 8);
			}
			else
			{
				this.targetedDirection = direction;
			}
			this.setActiveDirection(this.targetedDirection);
			vector = base.transform.TransformPoint(this.projectileRoots[(int)this.targetedDirection]);
		}
		if (this.shootingRight)
		{
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesRight[(int)this.targetedDirection];
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance);
		}
		else if (this.targetedDirection == RumRunnersLevelMobBoss.Direction.Attack0)
		{
			if (num < 0f)
			{
				num = Mathf.Clamp(num, -180f - RumRunnersLevelMobBoss.AcceptableAngleVariance, -180f + RumRunnersLevelMobBoss.AcceptableAngleVariance);
			}
			else
			{
				num = Mathf.Clamp(num, 180f - RumRunnersLevelMobBoss.AcceptableAngleVariance, 180f + RumRunnersLevelMobBoss.AcceptableAngleVariance);
			}
		}
		else
		{
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesLeft[(int)this.targetedDirection];
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance);
		}
		float floatAt = this.properties.CurrentState.boss.coinSpeed.GetFloatAt(this.minMaxParameter);
		BasicProjectile basicProjectile = this.projectile.Create(vector, num, floatAt);
		this.projectileMuzzleFX.Create(vector).transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
		basicProjectile.SetParryable(this.parryString.PopLetter() == 'P');
		this.SFX_RUMRUN_P4_Snail_ProjectileShoot();
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000C5300 File Offset: 0x000C3500
	public void updatePosition()
	{
		Vector3 vector = this.positioner.position;
		if (!this.shootingRight)
		{
			vector += this.flippedOffset;
		}
		base.transform.position = vector + this.positionOffset;
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x000C5354 File Offset: 0x000C3554
	public RumRunnersLevelMobBoss.Direction chooseDirection(float angle, bool canOvershoot)
	{
		RumRunnersLevelMobBoss.Direction result;
		if (this.shootingRight)
		{
			if (angle > 33.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack45;
			}
			else if (angle > 11.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack22;
			}
			else if (angle > -11.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack0;
			}
			else if (angle > -33.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack337;
			}
			else if (angle > -56.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack315;
			}
			else if (angle > -78.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack292;
			}
			else if (!canOvershoot)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack270;
			}
			else if (angle > -101.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack270;
			}
			else
			{
				result = RumRunnersLevelMobBoss.Direction.Attack247;
			}
		}
		else if (angle >= 168.75f || angle <= -168.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack0;
		}
		else if (angle < -146.25f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack337;
		}
		else if (angle < -123.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack315;
		}
		else if (angle < -101.25f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack292;
		}
		else if ((!canOvershoot && angle < 0f) || (canOvershoot && angle < -78.75f))
		{
			result = RumRunnersLevelMobBoss.Direction.Attack270;
		}
		else if (canOvershoot && angle < 0f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack247;
		}
		else if (angle < 168.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack22;
		}
		else
		{
			result = RumRunnersLevelMobBoss.Direction.Attack45;
		}
		return result;
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x0001F24E File Offset: 0x0001D44E
	public AnimatorStateInfo getAnimatorCurrentStateInfo()
	{
		return base.animator.GetCurrentAnimatorStateInfo((int)(this.targetedDirection + 1));
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x000C54A8 File Offset: 0x000C36A8
	public void setActiveDirection(RumRunnersLevelMobBoss.Direction direction)
	{
		for (int i = 1; i <= 8; i++)
		{
			float num = (direction != (RumRunnersLevelMobBoss.Direction)(i - 1)) ? 0f : 1f;
			base.animator.SetLayerWeight(i, num);
		}
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x0001F263 File Offset: 0x0001D463
	public void SFX_RUMRUN_P4_Snail_ProjectileShoot()
	{
		AudioManager.Play("sfx_dlc_rumrun_p4_snail_projectile_shoot");
	}

	// Token: 0x04001E7A RID: 7802
	public static readonly float AcceptableAngleVariance = 15f;

	// Token: 0x04001E7B RID: 7803
	public static readonly float[] ReferenceAnglesRight = new float[]
	{
		45f,
		22.5f,
		0f,
		-22.5f,
		-45f,
		-67.5f,
		-90f,
		-112.5f
	};

	// Token: 0x04001E7C RID: 7804
	public static readonly float[] ReferenceAnglesLeft = new float[]
	{
		135f,
		157.5f,
		180f,
		-157.5f,
		-135f,
		-112.5f,
		-90f,
		-67.5f
	};

	// Token: 0x04001E7D RID: 7805
	public static readonly int StartSpeedParameter = Animator.StringToHash("StartSpeed");

	// Token: 0x04001E7E RID: 7806
	public static readonly int EndSpeedParameter = Animator.StringToHash("EndSpeed");

	// Token: 0x04001E7F RID: 7807
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001E80 RID: 7808
	[SerializeField]
	public Effect projectileMuzzleFX;

	// Token: 0x04001E81 RID: 7809
	[SerializeField]
	public Vector2[] projectileRoots;

	// Token: 0x04001E82 RID: 7810
	[SerializeField]
	public Vector2 positionOffset;

	// Token: 0x04001E83 RID: 7811
	[SerializeField]
	public Vector2 flippedOffset;

	// Token: 0x04001E84 RID: 7812
	public bool begun;

	// Token: 0x04001E85 RID: 7813
	public bool dead;

	// Token: 0x04001E86 RID: 7814
	public float minMaxParameter;

	// Token: 0x04001E87 RID: 7815
	public bool shootingRight = true;

	// Token: 0x04001E88 RID: 7816
	public PatternString parryString;

	// Token: 0x04001E89 RID: 7817
	public PlayerId targetedPlayer;

	// Token: 0x04001E8A RID: 7818
	public Vector3 targetedPosition;

	// Token: 0x04001E8B RID: 7819
	public RumRunnersLevelMobBoss.Direction targetedDirection;

	// Token: 0x04001E8C RID: 7820
	public LevelProperties.RumRunners properties;

	// Token: 0x04001E8D RID: 7821
	public RumRunnersLevelAnteater anteater;

	// Token: 0x04001E8E RID: 7822
	public Transform positioner;

	// Token: 0x04001E8F RID: 7823
	public DamageReceiver damageReceiver;

	// Token: 0x04001E90 RID: 7824
	public CircleCollider2D circleCollider;

	// Token: 0x02000EB7 RID: 3767
	public enum Direction
	{
		// Token: 0x040069D8 RID: 27096
		Attack45,
		// Token: 0x040069D9 RID: 27097
		Attack22,
		// Token: 0x040069DA RID: 27098
		Attack0,
		// Token: 0x040069DB RID: 27099
		Attack337,
		// Token: 0x040069DC RID: 27100
		Attack315,
		// Token: 0x040069DD RID: 27101
		Attack292,
		// Token: 0x040069DE RID: 27102
		Attack270,
		// Token: 0x040069DF RID: 27103
		Attack247,
		// Token: 0x040069E0 RID: 27104
		AttackCount
	}
}
