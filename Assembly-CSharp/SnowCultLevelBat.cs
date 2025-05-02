using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class SnowCultLevelBat : AbstractProjectile
{
	// Token: 0x0600281B RID: 10267 RVA: 0x000CD754 File Offset: 0x000CB954
	public virtual SnowCultLevelBat Init(Vector3 startPos, Vector3 launchVel, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti parent, bool parryable, string suffix)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Dead;
		base.transform.position = startPos;
		this.speed = properties.batAttackSpeed;
		this.readdOnEscape = properties.batsReaddedOnEscape;
		this.moving = false;
		this.launchVelocity = launchVel;
		base.transform.localScale = new Vector3(Mathf.Sign(-launchVel.x), 1f);
		this.Health = properties.batHP;
		this.shotSpeed = properties.batShotSpeed;
		this.animatorSuffix = suffix;
		this.SetParryable(parryable);
		base.animator.Play("Slowdown" + this.animatorSuffix, 0, Random.Range(0f, 0.33f));
		return this;
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x00021A32 File Offset: 0x0001FC32
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x00021A5D File Offset: 0x0001FC5D
	public override void OnDieLifetime()
	{
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x00021A5F File Offset: 0x0001FC5F
	public override void OnDieDistance()
	{
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x00021A61 File Offset: 0x0001FC61
	public override void OnParryDie()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.EasyModeDie();
		}
		else
		{
			base.OnParryDie();
		}
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x00021A83 File Offset: 0x0001FC83
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x00021AA1 File Offset: 0x0001FCA1
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000CD834 File Offset: 0x000CBA34
	public void AttackPlayer(Vector3 startPos, float height, float width, float arc)
	{
		this.moving = true;
		this.attackStart = startPos;
		this.attackHeight = startPos.y - (CupheadLevelCamera.Current.Bounds.y + 100f) - height;
		this.attackWidth = width;
		base.transform.localScale = new Vector3(Mathf.Sign(-this.attackWidth), 1f);
		this.attackTime = 0f;
		this.arcModifier = arc;
		base.animator.SetFloat("YSpeed", -10f);
		base.animator.Play("Enter" + this.animatorSuffix);
		this.spriteRenderer.sortingOrder = 30;
		this.collider.enabled = true;
		this.reachedCircle = true;
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x000CD904 File Offset: 0x000CBB04
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.reachedCircle)
		{
			if (base.transform.position.y >= 460f)
			{
				this.reachedCircle = true;
				this.collider.enabled = false;
			}
			else
			{
				base.transform.position += this.launchVelocity * CupheadTime.FixedDelta;
				this.launchVelocity += Vector3.up * 500f * CupheadTime.FixedDelta;
			}
		}
		if (this.moving)
		{
			this.attackTime += CupheadTime.FixedDelta * this.speed;
			if (this.attackTime < 0.9f)
			{
				this.lastPos = base.transform.position;
				base.transform.position = new Vector3(Mathf.Lerp(this.attackStart.x, this.attackStart.x + this.attackWidth, this.attackTime), this.attackStart.y + Mathf.Pow(Mathf.Sin(this.attackTime * 3.14159274f), this.arcModifier) * -this.attackHeight);
				base.animator.SetFloat("YSpeed", base.transform.position.y - this.lastPos.y);
			}
			else
			{
				Vector3 vector = new Vector3(this.attackStart.x + this.attackWidth, this.attackStart.y) - this.lastPos;
				if (vector.magnitude > 15f)
				{
					vector = vector.normalized * 15f;
				}
				base.transform.position += vector;
				base.animator.SetFloat("YSpeed", vector.y);
			}
			if (base.transform.position.y - this.lastPos.y > -6f)
			{
				this.dripTimer -= CupheadTime.FixedDelta;
				if (this.dripTimer <= 0f)
				{
					SnowCultLevelBatDrip snowCultLevelBatDrip = this.dripPrefab.Create(base.transform.position + Vector3.down * 50f) as SnowCultLevelBatDrip;
					snowCultLevelBatDrip.SetColor(this.animatorSuffix);
					snowCultLevelBatDrip.vel.x = (base.transform.position.x - this.lastPos.x) / 2f;
					this.dripTimer = Random.Range(0.3f, 0.7f);
				}
			}
			if (this.attackTime > 1.2f)
			{
				if (this.readdOnEscape)
				{
					this.moving = false;
					this.collider.enabled = false;
					this.parent.ReturnBatToList(this);
				}
				else
				{
					this.Dead();
				}
			}
		}
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x000CDC1C File Offset: 0x000CBE1C
	public void Dead()
	{
		if (base.transform.position.y < 360f)
		{
			((SnowCultLevelBatEffect)this.explosionPrefab.Create(base.transform.position)).SetColor(this.animatorSuffix);
			this.SFX_SNOWCULT_BatDie();
		}
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.EasyModeDie();
		}
		else
		{
			this.StopAllCoroutines();
			this.Recycle<SnowCultLevelBat>();
		}
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x000CDC98 File Offset: 0x000CBE98
	public void EasyModeDie()
	{
		this.moving = false;
		this.collider.enabled = false;
		this.parent.ReturnBatToList(this);
		base.transform.position = new Vector3(base.transform.position.x, 460f);
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x00021AD6 File Offset: 0x0001FCD6
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Dead;
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x00021AF5 File Offset: 0x0001FCF5
	public void SFX_SNOWCULT_BatDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_popsicle_bat_death");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_popsicle_bat_death");
	}

	// Token: 0x04002142 RID: 8514
	public const float PADDING_FLOOR = 100f;

	// Token: 0x04002143 RID: 8515
	public const float PADDING_CEILING = 100f;

	// Token: 0x04002144 RID: 8516
	public const float DRIP_TIME_MIN = 0.3f;

	// Token: 0x04002145 RID: 8517
	public const float DRIP_TIME_MAX = 0.7f;

	// Token: 0x04002146 RID: 8518
	public DamageReceiver damageReceiver;

	// Token: 0x04002147 RID: 8519
	public SnowCultLevelYeti parent;

	// Token: 0x04002148 RID: 8520
	public float speed;

	// Token: 0x04002149 RID: 8521
	public float Health;

	// Token: 0x0400214A RID: 8522
	public bool reachedCircle;

	// Token: 0x0400214B RID: 8523
	public bool moving;

	// Token: 0x0400214C RID: 8524
	public Vector3 launchVelocity;

	// Token: 0x0400214D RID: 8525
	public float attackHeight;

	// Token: 0x0400214E RID: 8526
	public float attackWidth;

	// Token: 0x0400214F RID: 8527
	public Vector3 attackStart;

	// Token: 0x04002150 RID: 8528
	public float attackTime;

	// Token: 0x04002151 RID: 8529
	public float arcModifier = 1f;

	// Token: 0x04002152 RID: 8530
	public float dripTimer;

	// Token: 0x04002153 RID: 8531
	public float shotSpeed;

	// Token: 0x04002154 RID: 8532
	public bool readdOnEscape;

	// Token: 0x04002155 RID: 8533
	public Vector3 lastPos;

	// Token: 0x04002156 RID: 8534
	[SerializeField]
	public SnowCultLevelBatEffect explosionPrefab;

	// Token: 0x04002157 RID: 8535
	[SerializeField]
	public SnowCultLevelBatEffect dripPrefab;

	// Token: 0x04002158 RID: 8536
	[SerializeField]
	public Collider2D collider;

	// Token: 0x04002159 RID: 8537
	[SerializeField]
	public SpriteRenderer spriteRenderer;

	// Token: 0x0400215A RID: 8538
	public string animatorSuffix;
}
