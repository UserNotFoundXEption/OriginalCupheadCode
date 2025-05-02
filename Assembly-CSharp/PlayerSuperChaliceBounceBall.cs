using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class PlayerSuperChaliceBounceBall : AbstractProjectile
{
	// Token: 0x060037A4 RID: 14244 RVA: 0x0002D726 File Offset: 0x0002B926
	public override void OnDieLifetime()
	{
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0002D728 File Offset: 0x0002B928
	public override void OnDieDistance()
	{
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x00103F1C File Offset: 0x0010211C
	public override void Start()
	{
		base.Start();
		this.baseScale = base.transform.localScale;
		this.colliderSize = base.GetComponent<CircleCollider2D>().radius;
		this.rend = base.GetComponent<SpriteRenderer>();
		this.velocity.y = 0f;
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super);
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x00103F7C File Offset: 0x0010217C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.velocity.y = this.velocity.y - this.GRAVITY * CupheadTime.FixedDelta;
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.HandleInput();
		this.HandleJiggle();
		this.CheckEdges();
		this.CheckCollisionsThenMove();
		if (!this.super.LAUNCHED_VERSION)
		{
			this.player.transform.position = base.transform.position;
		}
		this.lastHitTimer -= CupheadTime.FixedDelta;
		if (this.super.timer < 1f)
		{
			this.rend.enabled = (Time.frameCount % 2 == 0);
		}
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0002D72A File Offset: 0x0002B92A
	public override void OnLevelEnd()
	{
		this.super.CleanUp();
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x00104044 File Offset: 0x00102244
	public void HandleJiggle()
	{
		if (this.jiggleTime > 0f)
		{
			base.transform.localScale = new Vector3(this.baseScale.x + Mathf.Sin(this.jiggleTime * 3.14159274f * 15f) * this.jiggleTime * 10f, this.baseScale.y + Mathf.Cos(this.jiggleTime * 3.14159274f * 15f) * this.jiggleTime * 10f, 1f);
			this.jiggleTime -= CupheadTime.FixedDelta;
		}
		else
		{
			base.transform.localScale = this.baseScale;
		}
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0002D737 File Offset: 0x0002B937
	public void SetJiggle()
	{
		AudioManager.Play("player_jump");
		AudioManager.Play("circus_trampoline_bounce");
		this.jiggleTime = 0.2f;
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x00104100 File Offset: 0x00102300
	public void CheckCollisionsThenMove()
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		float num2 = Vector3.Magnitude(this.velocity * CupheadTime.FixedDelta);
		int num3 = 9;
		float num4 = 3f;
		float num5 = this.baseScale.x * this.colliderSize * 0.9f;
		int num6 = 262144;
		int num7 = 1048576;
		int num8 = 524288;
		int num9 = num6 + num8 + num7;
		int num10 = 1;
		Vector3[] array = new Vector3[num3];
		while (num2 > 0f && (float)num < num4)
		{
			GameObject gameObject = null;
			bool flag = false;
			Vector3 vector2 = this.velocity.normalized;
			float num11 = (float)(180 / (array.Length - 1));
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.transform.position + Quaternion.Euler(0f, 0f, -90f + num11 * (float)i) * vector2 * num5;
			}
			float num12 = num2;
			for (int j = 0; j < num3; j++)
			{
				if (Physics2D.OverlapPoint(array[j], num9) != null && Physics2D.OverlapPoint(base.transform.position, num9) == null)
				{
					RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, array[j] - base.transform.position, num5 * 2f, num9);
					if (raycastHit2D.collider != null)
					{
						array[j] = Vector3.Lerp(raycastHit2D.point, base.transform.position, 0.001f);
					}
				}
			}
			for (int k = 0; k < num3; k++)
			{
				RaycastHit2D raycastHit2D2 = Physics2D.Raycast(array[k], this.velocity, num12, num6);
				Debug.DrawLine(array[k], array[k] + vector2 * num12, Color.red, 1f);
				if (raycastHit2D2.collider != null)
				{
					this.smokePuffEffect.Create(raycastHit2D2.point);
					if (Vector3.Distance(array[k], raycastHit2D2.point) <= num12)
					{
						flag = true;
						vector = raycastHit2D2.normal;
						num12 = Vector3.Distance(array[k], raycastHit2D2.point);
					}
				}
			}
			for (int l = 0; l < num3; l++)
			{
				RaycastHit2D raycastHit2D2 = Physics2D.Raycast(array[l], this.velocity, num12, num10);
				if (raycastHit2D2.collider != null && raycastHit2D2.collider.gameObject.CompareTag("Enemy") && (this.lastEnemyHit == null || this.lastEnemyHit != raycastHit2D2 || this.lastHitTimer <= 0f))
				{
					this.smokePuffEffect.Create(raycastHit2D2.point);
					if (Vector3.Distance(array[l], raycastHit2D2.point) <= num12)
					{
						flag = true;
						vector = raycastHit2D2.normal;
						num12 = Vector3.Distance(array[l], raycastHit2D2.point);
						gameObject = raycastHit2D2.collider.gameObject;
					}
				}
			}
			if (this.velocity.y >= 0f)
			{
				for (int m = 0; m < num3; m++)
				{
					RaycastHit2D raycastHit2D2 = Physics2D.Raycast(array[m], this.velocity, num12, num8);
					if (raycastHit2D2.collider != null)
					{
						this.smokePuffEffect.Create(raycastHit2D2.point);
						if (Vector3.Distance(array[m], raycastHit2D2.point) <= num12)
						{
							flag = true;
							vector = raycastHit2D2.normal;
							num12 = Vector3.Distance(array[m], raycastHit2D2.point);
						}
					}
				}
			}
			for (int n = 0; n < num3; n++)
			{
				RaycastHit2D raycastHit2D2 = Physics2D.Raycast(array[n], this.velocity, num12, num7);
				if (raycastHit2D2.collider != null)
				{
					LevelPlatform component = raycastHit2D2.collider.gameObject.GetComponent<LevelPlatform>();
					bool flag2 = false;
					if (component != null && (base.transform.position.y < raycastHit2D2.point.y || this.velocity.y > 0f))
					{
						flag2 = true;
					}
					if (!flag2 && (component == null || !component.canFallThrough || this.player.input.actions.GetAxis(1) > -0.35f))
					{
						this.smokePuffEffect.Create(raycastHit2D2.point);
						if (Vector3.Distance(array[n], raycastHit2D2.point) <= num12)
						{
							flag = true;
							vector = raycastHit2D2.normal;
							num12 = Vector3.Distance(array[n], raycastHit2D2.point);
						}
					}
				}
			}
			num2 -= num12;
			base.transform.position += vector2 * num12;
			if (flag)
			{
				this.SetJiggle();
				num++;
				this.velocity = Vector3.Reflect(this.velocity, vector);
				if (vector.y > 0f)
				{
					this.velocity.y = vector.y * this.BOUNCE_VEL * ((!this.player.input.actions.GetButton(2)) ? this.BOUNCE_MODIFIER_NO_JUMP : 1f);
				}
				if (gameObject != null)
				{
					this.DoCollisionEnemy(gameObject);
					this.velocity.x = this.velocity.x * this.ENEMY_REBOUND_MULTIPLIER;
				}
			}
		}
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x00104810 File Offset: 0x00102A10
	public void DoCollisionEnemy(GameObject hit)
	{
		this.lastEnemyHit = hit;
		this.lastHitTimer = this.ENEMY_MULTIHIT_DELAY;
		float num = this.damageDealer.DealDamage(hit);
		if (num > 0f)
		{
			base.animator.Play("Player_Super_Chalice_BounceBall_Flash");
			AudioManager.Play("player_parry_axe");
		}
		this.damageCount += num;
		if (this.damageCount >= this.MAX_DAMAGE)
		{
			this.super.Interrupt();
		}
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0010488C File Offset: 0x00102A8C
	public void HandleInput()
	{
		Trilean trilean = 0;
		Trilean trilean2 = 0;
		float axis = this.player.input.actions.GetAxis(0);
		if (axis > 0.35f || axis < -0.35f)
		{
			trilean = axis;
		}
		float move_ACCEL = this.MOVE_ACCEL;
		this.velocity.x = this.velocity.x + (float)trilean.Value * move_ACCEL * CupheadTime.FixedDelta;
		this.velocity.x = Mathf.Clamp(this.velocity.x, -this.MOVE_MAX_SPEED, this.MOVE_MAX_SPEED);
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0010492C File Offset: 0x00102B2C
	public void CheckEdges()
	{
		if (LevelPit.Instance != null && base.transform.position.y < LevelPit.Instance.transform.position.y && this.velocity.y < 0f)
		{
			base.transform.position += Vector3.down * 300f;
			this.super.Interrupt();
		}
		Vector2 vector = base.transform.position;
		vector.x = Mathf.Clamp(vector.x, (float)Level.Current.Left + 30f, (float)Level.Current.Right - 30f);
		if (vector.x != base.transform.position.x)
		{
			this.velocity.x = -this.velocity.x;
		}
		base.transform.position = vector;
	}

	// Token: 0x04002CB4 RID: 11444
	public const float PADDING_LEFT = 30f;

	// Token: 0x04002CB5 RID: 11445
	public const float PADDING_RIGHT = 30f;

	// Token: 0x04002CB6 RID: 11446
	public const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x04002CB7 RID: 11447
	public float MAX_DAMAGE = WeaponProperties.LevelSuperChaliceBounce.maxDamage;

	// Token: 0x04002CB8 RID: 11448
	public float MOVE_ACCEL = WeaponProperties.LevelSuperChaliceBounce.horizontalAcceleration;

	// Token: 0x04002CB9 RID: 11449
	public float MOVE_MAX_SPEED = WeaponProperties.LevelSuperChaliceBounce.maxHorizontalSpeed;

	// Token: 0x04002CBA RID: 11450
	public float BOUNCE_VEL = WeaponProperties.LevelSuperChaliceBounce.bounceVelocity;

	// Token: 0x04002CBB RID: 11451
	public float BOUNCE_MODIFIER_NO_JUMP = WeaponProperties.LevelSuperChaliceBounce.bounceModifierNoJump;

	// Token: 0x04002CBC RID: 11452
	public float GRAVITY = WeaponProperties.LevelSuperChaliceBounce.gravity;

	// Token: 0x04002CBD RID: 11453
	public float ENEMY_REBOUND_MULTIPLIER = WeaponProperties.LevelSuperChaliceBounce.enemyReboundMultiplier;

	// Token: 0x04002CBE RID: 11454
	public float ENEMY_MULTIHIT_DELAY = WeaponProperties.LevelSuperChaliceBounce.enemyMultihitDelay;

	// Token: 0x04002CBF RID: 11455
	[SerializeField]
	public Effect smokePuffEffect;

	// Token: 0x04002CC0 RID: 11456
	public Vector2 velocity;

	// Token: 0x04002CC1 RID: 11457
	public LevelPlayerController player;

	// Token: 0x04002CC2 RID: 11458
	public GameObject lastEnemyHit;

	// Token: 0x04002CC3 RID: 11459
	public float lastHitTimer;

	// Token: 0x04002CC4 RID: 11460
	public PlayerSuperChaliceBounce super;

	// Token: 0x04002CC5 RID: 11461
	public float jiggleTime;

	// Token: 0x04002CC6 RID: 11462
	public Vector3 baseScale;

	// Token: 0x04002CC7 RID: 11463
	public float colliderSize;

	// Token: 0x04002CC8 RID: 11464
	public SpriteRenderer rend;

	// Token: 0x04002CC9 RID: 11465
	public float damageCount;
}
