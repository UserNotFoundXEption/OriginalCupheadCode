using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000375 RID: 885
public class SaltbakerLevelHeart : AbstractProjectile
{
	// Token: 0x17000323 RID: 803
	// (get) Token: 0x0600270B RID: 9995 RVA: 0x00020D6D File Offset: 0x0001EF6D
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x000CA794 File Offset: 0x000C8994
	public override void Start()
	{
		base.Start();
		this.ballSize = base.GetComponent<Collider2D>().bounds.size.y / 2f;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.impactFX.transform.parent = null;
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x00020D74 File Offset: 0x0001EF74
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x00020D92 File Offset: 0x0001EF92
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.TakeDamage(info);
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000CA804 File Offset: 0x000C8A04
	public void Init(Vector3 pos, GameObject leftPillar, GameObject rightPillar, LevelProperties.Saltbaker.DarkHeart properties, SaltbakerLevelPillarHandler parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.isMoving = false;
		this.speed = properties.heartSpeed;
		this.parent = parent;
		this.leftPillarColl = leftPillar.GetComponent<Collider2D>();
		this.rightPillarColl = rightPillar.GetComponent<Collider2D>();
		this.SetParryable(true);
		this.coll.enabled = false;
		this.angleString = new PatternString(properties.angleOffsetString, true, true);
		this.dir = MathUtils.AngleToDirection(properties.baseAngle);
		base.transform.localScale = new Vector3(-Mathf.Sign(this.dir.x), 1f);
		this.lastDirNoOffset = this.dir;
		base.StartCoroutine(this.warning_cr());
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x000CA8D8 File Offset: 0x000C8AD8
	public IEnumerator warning_cr()
	{
		this.SFX_SALTB_HeartWarning();
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.isMoving = true;
		this.coll.enabled = true;
		yield break;
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x000CA8F4 File Offset: 0x000C8AF4
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.isMoving)
		{
			base.transform.position += this.dir * this.speed * CupheadTime.Delta;
		}
		this.CheckBounds();
		if (!this.isDead)
		{
			this.pinkSprite.enabled = base.CanParry;
		}
		if (base.CanParry && !this.isDead)
		{
			this.pinkSprite.color = new Color(this.pinkSprite.color.r, this.pinkSprite.color.g, this.pinkSprite.color.b, this.pinkSprite.color.a + Time.deltaTime * 2f);
			this.regularSprite.color = new Color(this.regularSprite.color.r, this.regularSprite.color.g, this.regularSprite.color.b, this.regularSprite.color.a - Time.deltaTime * 0.5f);
		}
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000CAA6C File Offset: 0x000C8C6C
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		this.pinkSprite.color = new Color(0f, 0f, 0f, 0f);
		this.regularSprite.color = Color.black;
		base.StartCoroutine(this.coolDown_cr());
		base.StartCoroutine(this.colliderCoolDown_cr());
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000CAAD0 File Offset: 0x000C8CD0
	public IEnumerator coolDown_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.parryTimeOut);
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000CAAEC File Offset: 0x000C8CEC
	public IEnumerator colliderCoolDown_cr()
	{
		this.damageDealer.SetDamageFlags(false, false, false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.collisionTimeOut);
		this.damageDealer.SetDamageFlags(true, false, false);
		yield return null;
		yield break;
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x000CAB08 File Offset: 0x000C8D08
	public void CheckBounds()
	{
		if (this.lastHit != SaltbakerLevelHeart.LastHit.Up && base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.ballSize)
		{
			this.SetNewDir(true, false);
			this.lastHit = SaltbakerLevelHeart.LastHit.Up;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Down && base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMin + this.ballSize)
		{
			this.SetNewDir(false, false);
			this.lastHit = SaltbakerLevelHeart.LastHit.Down;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Left && base.transform.position.x < this.leftPillarColl.bounds.max.x + this.ballSize)
		{
			this.SetNewDir(false, true);
			this.lastHit = SaltbakerLevelHeart.LastHit.Left;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Right && base.transform.position.x > this.rightPillarColl.bounds.min.x - this.ballSize)
		{
			this.SetNewDir(true, true);
			this.lastHit = SaltbakerLevelHeart.LastHit.Right;
		}
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x000CAC6C File Offset: 0x000C8E6C
	public void SetNewDir(bool getMin, bool isX)
	{
		this.angleOffset = this.angleString.PopFloat();
		Vector3 vector = this.lastDirNoOffset;
		if (getMin)
		{
			if (isX)
			{
				vector.x = Mathf.Min(vector.x, -vector.x);
				base.StartCoroutine(this.turn_cr());
			}
			else
			{
				vector.y = Mathf.Min(vector.y, -vector.y);
			}
		}
		else if (isX)
		{
			vector.x = Mathf.Max(vector.x, -vector.x);
			base.StartCoroutine(this.turn_cr());
		}
		else
		{
			vector.y = Mathf.Max(vector.y, -vector.y);
		}
		this.lastDirNoOffset = vector;
		float num = MathUtils.DirectionToAngle(vector);
		num += this.angleOffset;
		vector = MathUtils.AngleToDirection(num);
		this.dir = vector;
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x000CAD68 File Offset: 0x000C8F68
	public IEnumerator turn_cr()
	{
		this.isMoving = false;
		base.animator.Play("Turn");
		this.impactFX.transform.position = base.transform.position;
		this.impactFX.transform.localScale = base.transform.localScale;
		this.impactFX.Play((!Rand.Bool()) ? "B" : "A", 0, 0f);
		this.SFX_SALTB_HeartBounce();
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.181818187f)
		{
			yield return null;
		}
		this.isMoving = true;
		base.StartCoroutine(this.turn_fx_cr());
		yield break;
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x000CAD84 File Offset: 0x000C8F84
	public IEnumerator turn_fx_cr()
	{
		Vector3 pos = base.transform.position;
		int fxCount = Random.Range(2, 4);
		for (int i = 0; i < fxCount; i++)
		{
			this.turnFX.Create(pos);
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.1f));
		}
		yield break;
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x000CADA0 File Offset: 0x000C8FA0
	public void AniEvent_Turn()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x000CADD8 File Offset: 0x000C8FD8
	public new void Die()
	{
		this.StopAllCoroutines();
		this.coll.enabled = false;
		this.isMoving = false;
		this.regularSprite.enabled = false;
		this.pinkSprite.enabled = true;
		this.pinkSprite.color = new Color(0f, 0f, 0f, 1f);
		this.isDead = true;
		base.animator.Play("Death");
		AudioManager.Play("level_explosion_boss_death");
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x00020DA0 File Offset: 0x0001EFA0
	public void SFX_SALTB_HeartBounce()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Bounce");
		this.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Bounce");
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x00020DBC File Offset: 0x0001EFBC
	public void SFX_SALTB_HeartWarning()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Warning");
		this.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Warning");
	}

	// Token: 0x0400203D RID: 8253
	[SerializeField]
	public SpriteRenderer pinkSprite;

	// Token: 0x0400203E RID: 8254
	[SerializeField]
	public SpriteRenderer regularSprite;

	// Token: 0x0400203F RID: 8255
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04002040 RID: 8256
	[SerializeField]
	public Animator impactFX;

	// Token: 0x04002041 RID: 8257
	[SerializeField]
	public Effect turnFX;

	// Token: 0x04002042 RID: 8258
	public float ballSize;

	// Token: 0x04002043 RID: 8259
	public float speed;

	// Token: 0x04002044 RID: 8260
	public float angleOffset;

	// Token: 0x04002045 RID: 8261
	public bool isMoving;

	// Token: 0x04002046 RID: 8262
	public bool isDead;

	// Token: 0x04002047 RID: 8263
	public LevelProperties.Saltbaker.DarkHeart properties;

	// Token: 0x04002048 RID: 8264
	public SaltbakerLevelPillarHandler parent;

	// Token: 0x04002049 RID: 8265
	public DamageReceiver damageReceiver;

	// Token: 0x0400204A RID: 8266
	public Collider2D leftPillarColl;

	// Token: 0x0400204B RID: 8267
	public Collider2D rightPillarColl;

	// Token: 0x0400204C RID: 8268
	public Vector3 dir;

	// Token: 0x0400204D RID: 8269
	public Vector3 lastDirNoOffset;

	// Token: 0x0400204E RID: 8270
	public PatternString angleString;

	// Token: 0x0400204F RID: 8271
	public SaltbakerLevelHeart.LastHit lastHit;

	// Token: 0x02000F34 RID: 3892
	public enum LastHit
	{
		// Token: 0x04006D70 RID: 28016
		None,
		// Token: 0x04006D71 RID: 28017
		Left,
		// Token: 0x04006D72 RID: 28018
		Right,
		// Token: 0x04006D73 RID: 28019
		Up,
		// Token: 0x04006D74 RID: 28020
		Down
	}
}
