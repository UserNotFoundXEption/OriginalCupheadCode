using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class WeaponHomingProjectile : AbstractProjectile
{
	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x060038D8 RID: 14552 RVA: 0x0002E55A File Offset: 0x0002C75A
	public override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x001096B8 File Offset: 0x001078B8
	public override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_homing_impact");
			}
		}
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x00109724 File Offset: 0x00107924
	public override AbstractProjectile Create(Vector2 position, float rotation, Vector2 scale)
	{
		WeaponHomingProjectile weaponHomingProjectile = base.Create(position, rotation, scale) as WeaponHomingProjectile;
		for (int i = 0; i < this.trailPositions.Length; i++)
		{
			weaponHomingProjectile.trailPositions[i] = position;
			weaponHomingProjectile.trailRotations[i] = base.transform.eulerAngles.z;
		}
		if (MathUtils.RandomBool())
		{
			this.trail.SetScale(new float?(-1f), null, null);
		}
		if (MathUtils.RandomBool())
		{
			this.trail.SetScale(null, new float?(-1f), null);
		}
		return weaponHomingProjectile;
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x0002E561 File Offset: 0x0002C761
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x0002E569 File Offset: 0x0002C769
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x0002E589 File Offset: 0x0002C789
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		if (this.isEx)
		{
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x0002E5BF File Offset: 0x0002C7BF
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x001097EC File Offset: 0x001079EC
	public override void Die()
	{
		this.move = false;
		AudioManager.Play("player_weapon_peashot_miss");
		EffectSpawner component = base.GetComponent<EffectSpawner>();
		if (component != null)
		{
			Object.Destroy(component);
		}
		this.trail.gameObject.SetActive(false);
		base.Die();
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x0010983C File Offset: 0x00107A3C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		WeaponHomingProjectile.State state = this.state;
		if (state != WeaponHomingProjectile.State.Homing)
		{
			if (state == WeaponHomingProjectile.State.Swirling)
			{
				this.UpdateSwirling();
			}
		}
		else
		{
			this.UpdateHoming();
		}
		this.trailFollowIndex = (this.trailFollowIndex + 1) % this.trailFollowFrames;
		this.trailRotation = this.trailRotations[this.trailFollowIndex];
		this.trail.transform.position = this.trailPositions[this.trailFollowIndex];
		this.trailRotations[this.trailFollowIndex] = this.rotation;
		this.trailPositions[this.trailFollowIndex] = base.transform.position;
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x0010990C File Offset: 0x00107B0C
	public void UpdateHoming()
	{
		if (!this.move)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled && this.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime)
		{
			float num;
			for (num = MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position); num > this.rotation + 180f; num -= 360f)
			{
			}
			while (num < this.rotation - 180f)
			{
				num += 360f;
			}
			float num2 = this.rotationSpeed.min;
			if (this.t > this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime)
			{
				num2 = this.rotationSpeed.max;
			}
			else if (this.t > this.timeBeforeEaseRotationSpeed)
			{
				num2 = this.rotationSpeed.GetFloatAt((this.t - this.timeBeforeEaseRotationSpeed) / this.rotationSpeedEaseTime);
			}
			if (Mathf.Abs(num - this.rotation) < num2 * CupheadTime.FixedDelta)
			{
				this.rotation = num;
			}
			else if (num > this.rotation)
			{
				this.rotation += num2 * CupheadTime.FixedDelta;
			}
			else
			{
				this.rotation -= num2 * CupheadTime.FixedDelta;
			}
		}
		Vector3 vector = MathUtils.AngleToDirection(this.rotation);
		base.transform.position += vector * this.speed * CupheadTime.FixedDelta;
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(this.destroyPadding, this.destroyPadding)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x00109B28 File Offset: 0x00107D28
	public void UpdateSwirling()
	{
		if (!this.move)
		{
			return;
		}
		if (this.player.IsDead)
		{
			this.StopSwirling();
			return;
		}
		this.t += CupheadTime.FixedDelta;
		Vector2 vector = this.swirlLaunchPos + MathUtils.AngleToDirection(this.swirlLaunchRotation) * this.t * this.speed;
		float num = 360f * this.speed / (this.swirlDistance * 2f * 3.14159274f);
		this.swirlRotation += num * CupheadTime.FixedDelta;
		Vector2 vector2 = this.player.center + MathUtils.AngleToDirection(this.swirlRotation) * this.swirlDistance;
		if (this.t < this.swirlEaseTime)
		{
			Vector2 vector3 = base.transform.position;
			base.transform.position = Vector2.Lerp(vector, vector2, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, this.t / this.swirlEaseTime));
			this.rotation = MathUtils.DirectionToAngle(base.transform.position - vector3);
		}
		else
		{
			base.transform.position = vector2;
			this.rotation = this.swirlRotation + 90f;
		}
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x00109C98 File Offset: 0x00107E98
	public override void Update()
	{
		base.Update();
		this.timeSinceUpdateRotation += CupheadTime.Delta;
		if (this.timeSinceUpdateRotation > 0.0416666679f)
		{
			this.timeSinceUpdateRotation -= 0.0416666679f;
			Vector2 vector = this.trail.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation + this.spriteRotation));
			this.trail.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.trailRotation + this.trailSpriteRotation));
			this.trail.position = vector;
		}
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x0002E5CE File Offset: 0x0002C7CE
	public void FindTarget()
	{
		this.target = this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x00109D70 File Offset: 0x00107F70
	public Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		Vector2 vector = base.transform.position + this.speed * (this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime * 0.75f) * MathUtils.AngleToDirection(this.rotation);
		float num = float.MaxValue;
		Collider2D result = null;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D.bounds.center, collider2D.bounds.size / 2f))
					{
						float sqrMagnitude = (vector - collider2D.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = collider2D;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D2 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
						{
							float sqrMagnitude2 = (vector - collider2D2.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								result = collider2D2;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x00109FC4 File Offset: 0x001081C4
	public void StartSwirling(int index, int bulletCount, float spread, AbstractPlayerController player)
	{
		this.state = WeaponHomingProjectile.State.Swirling;
		this.swirlLaunchRotation = base.transform.eulerAngles.z + ((float)index / (float)(bulletCount - 1) - 0.5f) * spread;
		this.swirlRotation = base.transform.eulerAngles.z + ((float)index / (float)(bulletCount - 1) - 0.5f) * (((float)bulletCount - 1f) / (float)bulletCount) * 360f;
		this.swirlLaunchPos = base.transform.position;
		this.rotation = this.swirlLaunchRotation;
		base.animator.Play("A", 0, (float)index / (float)bulletCount);
		base.animator.Play("Idle", 1, (float)index / (float)bulletCount);
		this.player = player;
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x0002E5E1 File Offset: 0x0002C7E1
	public void StopSwirling()
	{
		this.state = WeaponHomingProjectile.State.Homing;
		this.FindTarget();
		this.t = 0f;
	}

	// Token: 0x04002DAD RID: 11693
	[SerializeField]
	public float spriteRotation;

	// Token: 0x04002DAE RID: 11694
	[SerializeField]
	public float trailSpriteRotation;

	// Token: 0x04002DAF RID: 11695
	[SerializeField]
	public Transform trail;

	// Token: 0x04002DB0 RID: 11696
	[SerializeField]
	public float destroyPadding;

	// Token: 0x04002DB1 RID: 11697
	public float speed;

	// Token: 0x04002DB2 RID: 11698
	public MinMax rotationSpeed;

	// Token: 0x04002DB3 RID: 11699
	public float timeBeforeEaseRotationSpeed;

	// Token: 0x04002DB4 RID: 11700
	public float rotationSpeedEaseTime;

	// Token: 0x04002DB5 RID: 11701
	public float rotation;

	// Token: 0x04002DB6 RID: 11702
	public float swirlDistance;

	// Token: 0x04002DB7 RID: 11703
	public float swirlEaseTime;

	// Token: 0x04002DB8 RID: 11704
	public int trailFollowFrames;

	// Token: 0x04002DB9 RID: 11705
	public WeaponHomingProjectile.State state;

	// Token: 0x04002DBA RID: 11706
	public Vector2 velocity;

	// Token: 0x04002DBB RID: 11707
	public float t;

	// Token: 0x04002DBC RID: 11708
	public bool move = true;

	// Token: 0x04002DBD RID: 11709
	public Collider2D target;

	// Token: 0x04002DBE RID: 11710
	public float swirlLaunchRotation;

	// Token: 0x04002DBF RID: 11711
	public float swirlRotation;

	// Token: 0x04002DC0 RID: 11712
	public AbstractPlayerController player;

	// Token: 0x04002DC1 RID: 11713
	public Vector2 swirlLaunchPos;

	// Token: 0x04002DC2 RID: 11714
	public float timeSinceUpdateRotation = 0.0416666679f;

	// Token: 0x04002DC3 RID: 11715
	public float trailRotation;

	// Token: 0x04002DC4 RID: 11716
	public bool isEx;

	// Token: 0x04002DC5 RID: 11717
	public Vector2[] trailPositions = new Vector2[10];

	// Token: 0x04002DC6 RID: 11718
	public float[] trailRotations = new float[10];

	// Token: 0x04002DC7 RID: 11719
	public int trailFollowIndex;

	// Token: 0x020011CA RID: 4554
	public enum State
	{
		// Token: 0x04007C46 RID: 31814
		Homing,
		// Token: 0x04007C47 RID: 31815
		Swirling
	}
}
