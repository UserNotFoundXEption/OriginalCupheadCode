using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class RumRunnersLevelBouncingBeetle : AbstractProjectile
{
	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06002450 RID: 9296 RVA: 0x0001EAA1 File Offset: 0x0001CCA1
	// (set) Token: 0x06002451 RID: 9297 RVA: 0x0001EAA9 File Offset: 0x0001CCA9
	public bool leaveScreen { get; set; }

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06002452 RID: 9298 RVA: 0x0001EAB2 File Offset: 0x0001CCB2
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.initialScale = this.visualTransform.localScale;
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x0001EAF5 File Offset: 0x0001CCF5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x000C34AC File Offset: 0x000C16AC
	public virtual RumRunnersLevelBouncingBeetle Init(Vector2 pos, Vector3 velocity, float initialSpeed, float timeToSlowdown, float targetSpeed, float hp)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.SetParryable(false);
		base.transform.position = pos;
		this.velocity = velocity;
		this.currentSpeed = initialSpeed;
		this.initialSpeed = initialSpeed;
		this.targetSpeed = targetSpeed;
		this.currentSpeed = targetSpeed;
		this.slowdownDuration = timeToSlowdown;
		this.isMoving = true;
		this.hp = hp;
		this.offset = base.GetComponent<Collider2D>().bounds.size.x / 2f;
		this.Move();
		this.leaveScreen = false;
		RumRunnersLevelBouncingBeetle.LastSortingIndex--;
		if (RumRunnersLevelBouncingBeetle.LastSortingIndex < 10)
		{
			RumRunnersLevelBouncingBeetle.LastSortingIndex = 15;
		}
		this.visualTransform.GetComponent<SpriteRenderer>().sortingOrder = RumRunnersLevelBouncingBeetle.LastSortingIndex;
		return this;
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x0001EB13 File Offset: 0x0001CD13
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x000C3584 File Offset: 0x000C1784
	public override void Die()
	{
		this.SFX_RUMRUN_CaterpillarBall_DeathExplosion();
		this.explosionPrefab.Create(base.transform.position);
		for (int i = 0; i < Random.Range(3, 5); i++)
		{
			float num = Random.Range(0f, 360f);
			Vector3 vector;
			vector..ctor(Mathf.Cos(num) * 50f, Mathf.Sin(num) * 50f);
			SpriteDeathParts spriteDeathParts = this.shrapnelPrefab.CreatePart(base.transform.position + vector);
			spriteDeathParts.animator.Update(0f);
			spriteDeathParts.animator.Play(0, 0, Random.Range(0f, 1f));
		}
		for (int j = 0; j < Random.Range(3, 5); j++)
		{
			float num2 = Random.Range(0f, 360f);
			Vector3 vector2;
			vector2..ctor(Mathf.Cos(num2) * 50f, Mathf.Sin(num2) * 50f);
			SpriteDeathParts spriteDeathParts2 = this.shrapnelPrefab.CreatePart(base.transform.position + vector2);
			spriteDeathParts2.animator.Update(0f);
			spriteDeathParts2.animator.Play(0, 0, Random.Range(0f, 1f));
			spriteDeathParts2.transform.SetScale(new float?(0.75f), new float?(0.75f), null);
			SpriteRenderer component = spriteDeathParts2.GetComponent<SpriteRenderer>();
			component.sortingLayerName = "Background";
			component.sortingOrder = 95;
			component.color = new Color(0.7f, 0.7f, 0.7f, 1f);
		}
		base.Die();
		this.Recycle<RumRunnersLevelBouncingBeetle>();
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x0001EB48 File Offset: 0x0001CD48
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000C3750 File Offset: 0x000C1950
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float elapsedTime = 0f;
		for (;;)
		{
			yield return wait;
			if (this.isMoving)
			{
				if (elapsedTime <= this.slowdownDuration)
				{
					elapsedTime += CupheadTime.FixedDelta;
					this.currentSpeed = Mathf.Lerp(this.initialSpeed, this.targetSpeed, elapsedTime / this.slowdownDuration);
				}
				base.transform.position += this.velocity * this.currentSpeed * CupheadTime.FixedDelta;
				this.CheckBounds();
			}
		}
		yield break;
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x000C376C File Offset: 0x000C196C
	public void CheckBounds()
	{
		bool flag = false;
		Vector3 vector = Vector3.zero;
		Vector3 one = Vector3.one;
		float num = 0f;
		if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.offset && this.velocity.y > 0f)
		{
			flag = true;
			this.velocity.y = -Mathf.Abs(this.velocity.y);
			vector = Vector2.up;
			one.x = ((this.velocity.x <= 0f) ? -1f : 1f);
			num = 180f;
		}
		if (base.transform.position.y < (float)Level.Current.Ground + this.offset && this.velocity.y < 0f)
		{
			flag = true;
			this.velocity.y = Mathf.Abs(this.velocity.y);
			vector = Vector2.down;
			one.x = ((this.velocity.x >= 0f) ? -1f : 1f);
			one.y = 1f;
			num = 0f;
		}
		if (!this.leaveScreen)
		{
			if (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - this.offset && this.velocity.x > 0f)
			{
				flag = true;
				this.velocity.x = -Mathf.Abs(this.velocity.x);
				vector = Vector2.right;
				num = 90f;
				one.x = ((this.velocity.y >= 0f) ? -1f : 1f);
			}
			if (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + this.offset && this.velocity.x < 0f)
			{
				flag = true;
				this.velocity.x = Mathf.Abs(this.velocity.x);
				vector = Vector2.left;
				one.x = ((this.velocity.y <= 0f) ? -1f : 1f);
				num = 270f;
			}
		}
		else if (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - 100f || base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + 100f)
		{
			base.Die();
		}
		if (flag)
		{
			Effect effect = this.wallPoofEffect.Create(base.transform.position + vector * this.offset);
			effect.transform.rotation = Quaternion.Euler(0f, 0f, num);
			Vector3 localScale = effect.transform.localScale;
			localScale.x *= one.x;
			effect.transform.localScale = localScale;
			this.SFX_RUMRUN_CaterpillarBall_Bounce();
			if (this.squashCoroutine != null)
			{
				base.StopCoroutine(this.squashCoroutine);
			}
			this.squashCoroutine = base.StartCoroutine(this.squash_cr(vector));
		}
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x000C3B44 File Offset: 0x000C1D44
	public IEnumerator squash_cr(Vector2 normal)
	{
		Vector3 scale = this.initialScale;
		Vector3 visualOffset;
		if (normal.x != 0f)
		{
			scale.x *= this.squashAmount;
			scale.y *= this.squashAmountPerpendicular;
			visualOffset = new Vector3(this.offset * (1f - this.squashAmount) * Mathf.Sign(normal.x), 0f);
		}
		else
		{
			scale.y *= this.squashAmount;
			scale.x *= this.squashAmountPerpendicular;
			visualOffset = new Vector3(0f, this.offset * (1f - this.squashAmount) * Mathf.Sign(normal.y));
			this.SFX_RUMRUN_CaterpillarBall_Bounce();
		}
		this.visualTransform.localScale = scale;
		this.visualTransform.localPosition = visualOffset;
		for (float elapsedTime = 0f; elapsedTime < 0.0416666679f; elapsedTime += CupheadTime.Delta)
		{
			yield return null;
		}
		this.visualTransform.localScale = this.initialScale;
		this.visualTransform.localPosition = Vector3.zero;
		this.squashCoroutine = null;
		yield break;
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x0001EB57 File Offset: 0x0001CD57
	public void SFX_RUMRUN_CaterpillarBall_Bounce()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_bounce");
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x0001EB73 File Offset: 0x0001CD73
	public void SFX_RUMRUN_CaterpillarBall_DeathExplosion()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_deathexplosion");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_deathexplosion");
	}

	// Token: 0x04001E0A RID: 7690
	public const float DESTROY_RANGE = 100f;

	// Token: 0x04001E0B RID: 7691
	public static int LastSortingIndex;

	// Token: 0x04001E0D RID: 7693
	[SerializeField]
	public Effect wallPoofEffect;

	// Token: 0x04001E0E RID: 7694
	[SerializeField]
	public Transform visualTransform;

	// Token: 0x04001E0F RID: 7695
	[SerializeField]
	public float squashAmount;

	// Token: 0x04001E10 RID: 7696
	[SerializeField]
	public float squashAmountPerpendicular;

	// Token: 0x04001E11 RID: 7697
	public bool isMoving;

	// Token: 0x04001E12 RID: 7698
	public float initialSpeed;

	// Token: 0x04001E13 RID: 7699
	public float targetSpeed;

	// Token: 0x04001E14 RID: 7700
	public float currentSpeed;

	// Token: 0x04001E15 RID: 7701
	public float slowdownDuration;

	// Token: 0x04001E16 RID: 7702
	public float hp;

	// Token: 0x04001E17 RID: 7703
	public float offset;

	// Token: 0x04001E18 RID: 7704
	public Vector3 velocity;

	// Token: 0x04001E19 RID: 7705
	public Vector3 initialScale;

	// Token: 0x04001E1A RID: 7706
	public Coroutine squashCoroutine;

	// Token: 0x04001E1B RID: 7707
	public DamageReceiver damageReceiver;

	// Token: 0x04001E1C RID: 7708
	[SerializeField]
	public Effect explosionPrefab;

	// Token: 0x04001E1D RID: 7709
	[SerializeField]
	public SpriteDeathPartsDLC shrapnelPrefab;
}
