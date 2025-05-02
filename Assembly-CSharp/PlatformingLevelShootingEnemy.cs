using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class PlatformingLevelShootingEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002BCB RID: 11211 RVA: 0x00024BC0 File Offset: 0x00022DC0
	public override void Start()
	{
		base.Start();
		this._aim = new GameObject("Aim").transform;
		this._aim.SetParent(this._projectileRoot);
		this._aim.ResetLocalTransforms();
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x000D7AA0 File Offset: 0x000D5CA0
	public override void OnStart()
	{
		this._projectileDelay = base.Properties.ProjectileDelay.RandomFloat();
		switch (this._triggerType)
		{
		case PlatformingLevelShootingEnemy.TriggerType.Range:
			base.StartCoroutine(this.ranged_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes:
			base.StartCoroutine(this.triggerVolumes_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.OnScreen:
			base.StartCoroutine(this.onscreen_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.Indefinite:
			base.StartCoroutine(this.indefinite_cr());
			break;
		}
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x00024BF9 File Offset: 0x00022DF9
	public virtual void StartShoot()
	{
		base.animator.SetTrigger("Shoot");
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x000D7B34 File Offset: 0x000D5D34
	public virtual void Shoot()
	{
		float num = base.Properties.ProjectileAngle;
		float speed = base.Properties.ProjectileSpeed;
		if (this._target == null || this._target.IsDead)
		{
			this._target = PlayerManager.GetNext();
		}
		switch (base.Properties.ProjectileAimMode)
		{
		case EnemyProperties.AimMode.AimedAtPlayer:
			this._aim.LookAt2D(this._target.center);
			num = this._aim.transform.eulerAngles.z;
			break;
		case EnemyProperties.AimMode.ArcAimedAtPlayer:
		{
			float num2 = float.MaxValue;
			Vector2 vector = this._target.center - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax = new MinMax(base.Properties.ArcProjectileMinAngle, base.Properties.ProjectileAngle);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f && this._ArcExtraSpeedUnderPlayerMultiplier > 0f)
			{
				float num3 = minMax2.max / base.Properties.ProjectileGravity;
				float num4 = minMax2.max * num3 - 0.5f * base.Properties.ProjectileGravity * num3 * num3;
				float num5 = num4 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num6 = Mathf.Sqrt(2f * num5 / base.Properties.ProjectileGravity);
				minMax2.max = num6 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num7 = 0f;
			while (num7 < 1f)
			{
				float floatAt = minMax.GetFloatAt(num7);
				float floatAt2 = minMax2.GetFloatAt(num7);
				Vector2 vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2;
				float num8 = vector.x / vector2.x;
				float num9 = vector2.y * num8 - 0.5f * base.Properties.ProjectileGravity * num8 * num8;
				float num10 = Mathf.Abs(vector.y - num9);
				if (base.Properties.ProjectileGravity <= 0.01f)
				{
					goto IL_292;
				}
				float num11 = vector2.y - base.Properties.ProjectileGravity * num8;
				if (num11 <= 0f)
				{
					goto IL_292;
				}
				IL_2A5:
				num7 += 0.01f;
				continue;
				IL_292:
				if (num10 < num2)
				{
					num2 = num10;
					num = floatAt;
					speed = floatAt2;
					goto IL_2A5;
				}
				goto IL_2A5;
			}
			if ((!this._hasFacingDirection && this._target.center.x < base.transform.position.x) || (this._hasFacingDirection && this._direction == PlatformingLevelShootingEnemy.Direction.Left))
			{
				num = 180f - num;
			}
			break;
		}
		case EnemyProperties.AimMode.Spread:
		{
			Vector3 vector3 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle);
			float num2 = float.MaxValue;
			Vector2 vector = vector3 - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f)
			{
				float num12 = minMax2.max / base.Properties.ProjectileGravity;
				float num13 = minMax2.max * num12 - 0.5f * base.Properties.ProjectileGravity * num12 * num12;
				float num14 = num13 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num15 = Mathf.Sqrt(2f * num14 / base.Properties.ProjectileGravity);
				minMax2.max = num15 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num16 = minMax2.RandomFloat();
			Vector2 vector4 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle) * num16;
			float num17 = vector.x / vector4.x;
			float num18 = vector4.y * num17 - 0.5f * base.Properties.ProjectileGravity * num17 * num17;
			float num19 = Mathf.Abs(vector.y - num18);
			if (num19 < num2)
			{
				num = base.Properties.ProjectileAngle;
				speed = num16;
			}
			for (int i = 0; i < 2; i++)
			{
				float rotation = (i != 1) ? 90f : (180f - num);
				BasicProjectile basicProjectile = this.projectilePrefab.Create(this._projectileRoot.position, rotation, speed);
				basicProjectile.SetParryable(base.Properties.ProjectileParryable);
				basicProjectile.Gravity = base.Properties.ProjectileGravity;
			}
			break;
		}
		case EnemyProperties.AimMode.Arc:
		{
			Vector3 vector3 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle);
			float num2 = float.MaxValue;
			Vector2 vector = vector3 - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f)
			{
				float num20 = minMax2.max / base.Properties.ProjectileGravity;
				float num21 = minMax2.max * num20 - 0.5f * base.Properties.ProjectileGravity * num20 * num20;
				float num22 = num21 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num23 = Mathf.Sqrt(2f * num22 / base.Properties.ProjectileGravity);
				minMax2.max = num23 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num24 = minMax2.RandomFloat();
			Vector2 vector5 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle) * num24;
			float num25 = vector.x / vector5.x;
			float num26 = vector5.y * num25 - 0.5f * base.Properties.ProjectileGravity * num25 * num25;
			float num27 = Mathf.Abs(vector.y - num26);
			if (num27 < num2)
			{
				num = base.Properties.ProjectileAngle;
				speed = num24;
			}
			break;
		}
		}
		BasicProjectile basicProjectile2 = this.projectilePrefab.Create(this._projectileRoot.position, num, speed);
		basicProjectile2.SetParryable(base.Properties.ProjectileParryable);
		basicProjectile2.SetStoneTime(base.Properties.ProjectileStoneTime);
		basicProjectile2.Gravity = base.Properties.ProjectileGravity;
		this.SpawnShootEffect();
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x00024C0B File Offset: 0x00022E0B
	public virtual void SpawnShootEffect()
	{
		if (this._shootEffect != null)
		{
			this._shootEffect.Create(this._effectRoot.position);
		}
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000D8278 File Offset: 0x000D6478
	public void setDirection(PlatformingLevelShootingEnemy.Direction direction)
	{
		this._direction = direction;
		base.transform.SetScale(new float?((float)((this._direction != PlatformingLevelShootingEnemy.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x00024C35 File Offset: 0x00022E35
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x000D82C4 File Offset: 0x000D64C4
	public IEnumerator indefinite_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
		for (;;)
		{
			if (this._hasShootingAnimation)
			{
				this.StartShoot();
				yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
				this._target = PlayerManager.GetNext();
			}
			else
			{
				this._target = PlayerManager.GetNext();
				this.Shoot();
			}
			yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
		}
		yield break;
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x000D82E0 File Offset: 0x000D64E0
	public IEnumerator onscreen_cr()
	{
		for (;;)
		{
			while (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(-this.onScreenTriggerPadding, 0f)))
			{
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetNext();
				}
				else
				{
					this._target = PlayerManager.GetNext();
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x000D82FC File Offset: 0x000D64FC
	public IEnumerator ranged_cr()
	{
		PlayerId lastPlayer = PlayerId.None;
		for (;;)
		{
			PlayerId currentPlayer = PlayerId.PlayerOne;
			bool inRange = false;
			while (!inRange)
			{
				bool cuphead = this.IsPlayerInRange(PlayerId.PlayerOne);
				bool mugman = PlayerManager.Multiplayer && this.IsPlayerInRange(PlayerId.PlayerTwo);
				if (cuphead && mugman)
				{
					currentPlayer = ((lastPlayer != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo);
					inRange = true;
				}
				else if (cuphead && !mugman)
				{
					currentPlayer = PlayerId.PlayerOne;
					inRange = true;
				}
				else if (!cuphead && mugman)
				{
					currentPlayer = PlayerId.PlayerTwo;
					inRange = true;
				}
				lastPlayer = currentPlayer;
				this._target = PlayerManager.GetPlayer(currentPlayer);
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetPlayer(currentPlayer);
				}
				else
				{
					this._target = PlayerManager.GetPlayer(currentPlayer);
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x00024C44 File Offset: 0x00022E44
	public bool IsPlayerInRange(PlayerId player)
	{
		return Vector2.Distance(base.transform.position, PlayerManager.GetPlayer(player).center) <= this.triggerRange;
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x000D8318 File Offset: 0x000D6518
	public IEnumerator triggerVolumes_cr()
	{
		PlayerId lastPlayer = PlayerId.None;
		for (;;)
		{
			PlayerId currentPlayer = PlayerId.PlayerOne;
			bool within = false;
			while (!within)
			{
				bool cuphead = this.IsPlayerInVolumes(PlayerId.PlayerOne);
				bool mugman = PlayerManager.Multiplayer && this.IsPlayerInVolumes(PlayerId.PlayerTwo);
				if (cuphead && mugman)
				{
					currentPlayer = ((lastPlayer != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo);
					within = true;
				}
				else if (cuphead && !mugman)
				{
					currentPlayer = PlayerId.PlayerOne;
					within = true;
				}
				else if (!cuphead && mugman)
				{
					currentPlayer = PlayerId.PlayerTwo;
					within = true;
				}
				lastPlayer = currentPlayer;
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetPlayer(currentPlayer);
				}
				else
				{
					this._target = PlayerManager.GetPlayer(currentPlayer);
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x000D8334 File Offset: 0x000D6534
	public virtual bool IsPlayerInVolumes(PlayerId player)
	{
		Vector2 vector = PlayerManager.GetPlayer(player).center;
		foreach (PlatformingLevelShootingEnemy.TriggerVolumeProperties triggerVolumeProperties in this._triggerVolumes)
		{
			PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape = triggerVolumeProperties.shape;
			if (shape != PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider)
			{
				if (shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider)
				{
					Vector2 position = triggerVolumeProperties.position;
					if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
					{
						position.x += base.transform.position.x;
						position.y += base.transform.position.y;
					}
					if (MathUtils.CircleContains(position, triggerVolumeProperties.circleRadius, vector))
					{
						return true;
					}
				}
			}
			else
			{
				Rect rect = RectUtils.NewFromCenter(triggerVolumeProperties.position.x, triggerVolumeProperties.position.y, triggerVolumeProperties.boxSize.x, triggerVolumeProperties.boxSize.y);
				if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
				{
					rect.x += base.transform.position.x;
					rect.y += base.transform.position.y;
				}
				if (rect.Contains(vector))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x00024C76 File Offset: 0x00022E76
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x00024C89 File Offset: 0x00022E89
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000D84DC File Offset: 0x000D66DC
	public new void DrawGizmos(float alpha)
	{
		if (base.Properties == null)
		{
			return;
		}
		PlatformingLevelShootingEnemy.TriggerType triggerType = this._triggerType;
		if (triggerType != PlatformingLevelShootingEnemy.TriggerType.Range)
		{
			if (triggerType != PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes)
			{
				if (triggerType == PlatformingLevelShootingEnemy.TriggerType.Indefinite)
				{
					this.DrawIndefiniteTriggerGizmos(alpha);
				}
			}
			else
			{
				this.DrawTriggerVolumesTriggerGizmos(alpha);
			}
		}
		else
		{
			this.DrawRangeTriggerGizmos(alpha);
		}
		EnemyProperties.AimMode projectileAimMode = base.Properties.ProjectileAimMode;
		if (projectileAimMode != EnemyProperties.AimMode.AimedAtPlayer)
		{
			if (projectileAimMode == EnemyProperties.AimMode.Straight)
			{
				this.DrawStraightAimGizmos(alpha);
			}
		}
		else
		{
			this.DrawAimedAtPlayerAimGizmos(alpha);
		}
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000D8574 File Offset: 0x000D6774
	public void DrawStraightAimGizmos(float alpha)
	{
		Color red = Color.red;
		red.a = alpha;
		Gizmos.color = red;
		Vector3 position = base.transform.position;
		Vector3 vector = position + Quaternion.Euler(0f, 0f, base.Properties.ProjectileAngle) * Vector3.right * this.triggerRange;
		Vector3 vector2 = position + Quaternion.Euler(0f, 0f, base.Properties.ProjectileAngle) * Vector3.right * 10000f;
		Gizmos.DrawLine(position, vector);
		red.a *= 0.25f;
		Gizmos.color = red;
		Gizmos.DrawLine(position, vector2);
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000D8634 File Offset: 0x000D6834
	public void DrawAimedAtPlayerAimGizmos(float alpha)
	{
		Color red = Color.red;
		red.a = alpha;
		Gizmos.color = red;
		Vector3 vector = base.transform.position + new Vector3(-100f, 100f, 0f);
		Vector3 vector2 = Vector3.one * 40f / 2f;
		vector2.z = 0.001f;
		Vector3 vector3 = vector + new Vector3(-vector2.x / 2f, vector2.y / 2f, 0f);
		Vector3 vector4 = vector3;
		vector4.y -= vector2.y * 2f;
		Gizmos.DrawWireCube(vector, vector2);
		Gizmos.DrawLine(vector3, vector4);
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000D86FC File Offset: 0x000D68FC
	public void DrawRangeTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.triggerRange);
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x000D8734 File Offset: 0x000D6934
	public void DrawTriggerVolumesTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		foreach (PlatformingLevelShootingEnemy.TriggerVolumeProperties triggerVolumeProperties in this._triggerVolumes)
		{
			Vector2 vector = triggerVolumeProperties.position;
			if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
			{
				vector += base.transform.position;
			}
			PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape = triggerVolumeProperties.shape;
			if (shape != PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider)
			{
				if (shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider)
				{
					Gizmos.DrawWireCube(vector, triggerVolumeProperties.boxSize);
				}
			}
			else
			{
				Gizmos.DrawWireSphere(vector, triggerVolumeProperties.circleRadius);
			}
		}
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000D8814 File Offset: 0x000D6A14
	public void DrawIndefiniteTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		Vector3 vector = base.transform.position + new Vector3(100f, 100f, 0f);
		Vector3 vector2;
		vector2..ctor(vector.x, vector.y + 10f, 0f);
		Vector3 vector3 = vector2;
		vector3.y -= 40f;
		Vector3 vector4 = vector2;
		vector4.x -= 10f;
		Vector3 vector5 = vector2;
		vector5.x += 10f;
		Vector3 vector6 = vector3;
		vector6.x -= 10f;
		Vector3 vector7 = vector3;
		vector7.x += 10f;
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector6, vector7);
	}

	// Token: 0x04002443 RID: 9283
	[Header("Trigger Properties")]
	[SerializeField]
	public PlatformingLevelShootingEnemy.TriggerType _triggerType;

	// Token: 0x04002444 RID: 9284
	[SerializeField]
	public List<PlatformingLevelShootingEnemy.TriggerVolumeProperties> _triggerVolumes;

	// Token: 0x04002445 RID: 9285
	[SerializeField]
	public Effect _shootEffect;

	// Token: 0x04002446 RID: 9286
	[SerializeField]
	public Transform _effectRoot;

	// Token: 0x04002447 RID: 9287
	[SerializeField]
	public Transform _projectileRoot;

	// Token: 0x04002448 RID: 9288
	[SerializeField]
	public bool _hasShootingAnimation;

	// Token: 0x04002449 RID: 9289
	[SerializeField]
	public MinMax _initialShotDelay;

	// Token: 0x0400244A RID: 9290
	[SerializeField]
	public bool _hasFacingDirection;

	// Token: 0x0400244B RID: 9291
	[SerializeField]
	public float _ArcExtraSpeedUnderPlayerMultiplier;

	// Token: 0x0400244C RID: 9292
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x0400244D RID: 9293
	public float triggerRange = 1000f;

	// Token: 0x0400244E RID: 9294
	public float onScreenTriggerPadding;

	// Token: 0x0400244F RID: 9295
	public AbstractPlayerController _target;

	// Token: 0x04002450 RID: 9296
	public Transform _aim;

	// Token: 0x04002451 RID: 9297
	public bool _hasFired;

	// Token: 0x04002452 RID: 9298
	public float _projectileDelay;

	// Token: 0x04002453 RID: 9299
	public PlatformingLevelShootingEnemy.Direction _direction;

	// Token: 0x04002454 RID: 9300
	public const float GIZMO_LETTER_LENGTH = 40f;

	// Token: 0x0200100E RID: 4110
	public enum TriggerType
	{
		// Token: 0x040072C1 RID: 29377
		Range,
		// Token: 0x040072C2 RID: 29378
		TriggerVolumes,
		// Token: 0x040072C3 RID: 29379
		OnScreen,
		// Token: 0x040072C4 RID: 29380
		Indefinite
	}

	// Token: 0x0200100F RID: 4111
	public enum Direction
	{
		// Token: 0x040072C6 RID: 29382
		Left,
		// Token: 0x040072C7 RID: 29383
		Right
	}

	// Token: 0x02001010 RID: 4112
	[Serializable]
	public class TriggerVolumeProperties
	{
		// Token: 0x06007730 RID: 30512 RVA: 0x002728F8 File Offset: 0x00270AF8
		public Rect ToRect()
		{
			Rect result;
			result..ctor(this.position, this.boxSize);
			result.x -= result.width / 2f;
			result.y -= result.height / 2f;
			return result;
		}

		// Token: 0x040072C8 RID: 29384
		public PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape;

		// Token: 0x040072C9 RID: 29385
		public PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space space;

		// Token: 0x040072CA RID: 29386
		public Vector2 position = Vector2.zero;

		// Token: 0x040072CB RID: 29387
		public Vector2 boxSize = new Vector2(100f, 100f);

		// Token: 0x040072CC RID: 29388
		public float circleRadius = 100f;

		// Token: 0x020015DC RID: 5596
		public enum Shape
		{
			// Token: 0x040091D5 RID: 37333
			BoxCollider,
			// Token: 0x040091D6 RID: 37334
			CircleCollider
		}

		// Token: 0x020015DD RID: 5597
		public enum Space
		{
			// Token: 0x040091D8 RID: 37336
			RelativeSpace,
			// Token: 0x040091D9 RID: 37337
			WorldSpace
		}
	}
}
