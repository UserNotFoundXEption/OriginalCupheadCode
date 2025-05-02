using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class SaltbakerLevelBouncer : LevelProperties.Saltbaker.Entity
{
	// Token: 0x060026BE RID: 9918 RVA: 0x000C9C08 File Offset: 0x000C7E08
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.idleHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		foreach (CollisionChild collisionChild in this.collisionKids)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
			collisionChild.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		}
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x0002085B File Offset: 0x0001EA5B
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x00020873 File Offset: 0x0001EA73
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x00020886 File Offset: 0x0001EA86
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x000208A4 File Offset: 0x0001EAA4
	public void StartBouncer(Vector3 startPos)
	{
		this.bouncerStartPos = startPos;
		base.transform.position = startPos;
		this.saltHands.gameObject.SetActive(true);
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000C9CAC File Offset: 0x000C7EAC
	public float TimeToGround(float curYVel, float groundY, float gravity)
	{
		float num = base.transform.position.y - groundY;
		return (curYVel + Mathf.Sqrt(curYVel * curYVel + 2f * gravity * num)) / gravity;
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000C9CE8 File Offset: 0x000C7EE8
	public IEnumerator jump_cr()
	{
		LevelProperties.Saltbaker.Bouncer p = base.properties.CurrentState.bouncer;
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		while (!this.isDead)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "Explode", false, false);
			base.transform.position = this.bouncerStartPos;
			base.animator.Play("Idle");
			this.saltHands.Play();
			foreach (Collider2D collider2D in this.colliders)
			{
				collider2D.enabled = false;
			}
			yield return CupheadTime.WaitForSeconds(this, 3.5f);
			this.SFX_SALTB_Bouncer_Twirl();
			YieldInstruction wait = new WaitForFixedUpdate();
			foreach (Collider2D collider2D2 in this.colliders)
			{
				collider2D2.enabled = true;
			}
			bool goingRight = Rand.Bool();
			float velocityY = 0f;
			float velocityX = 0f;
			float gravity = p.initDropYGravity;
			this.onGroundY = (float)Level.Current.Ground + base.GetComponent<Collider2D>().bounds.size.y / 2f + 13f;
			float maxX = (float)Level.Current.Right - base.GetComponent<Collider2D>().bounds.size.x / 2f;
			this.minShadowHeight = this.onGroundY + 75f - p.jumpGravity * 0.027777778f + p.jumpYSpeed * 0.166666672f;
			this.maxShadowHeight = this.onGroundY + p.jumpYSpeed * p.jumpYSpeed / (p.jumpGravity * 2f);
			AbstractPlayerController player = PlayerManager.GetNext();
			base.transform.SetPosition(new float?(player.transform.position.x), null, null);
			float timeToGround = this.TimeToGround(velocityY, this.onGroundY + 75f, gravity);
			float animTimeOnLand = -1f;
			bool useLandB = false;
			for (int i = 0; i < p.numBounces + 1; i++)
			{
				while (base.transform.position.y > this.onGroundY + 75f)
				{
					timeToGround -= CupheadTime.FixedDelta;
					if (animTimeOnLand < 0f && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleHash)
					{
						float num = (timeToGround - 0.1f) / 0.6666667f;
						animTimeOnLand = (num + base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1f;
						float num2 = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.84375f), Mathf.Abs(0.84375f - animTimeOnLand));
						float num3 = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.40625f), Mathf.Abs(0.40625f - animTimeOnLand));
						useLandB = (num2 > num3 && i < p.numBounces);
						float num4 = animTimeOnLand - ((!useLandB) ? 0.84375f : 0.40625f);
						float num5 = num - num4;
						animHelper.Speed = num5 / num;
					}
					if (timeToGround < 0.1f)
					{
						animHelper.Speed = 1f;
						if (i == p.numBounces || this.isDead)
						{
							base.animator.Play("Explode");
						}
						else
						{
							base.animator.Play((!useLandB) ? "Land_A" : "Land_B");
						}
					}
					velocityY -= gravity * CupheadTime.FixedDelta;
					if (i > 0)
					{
						velocityX = ((!goingRight) ? (-p.jumpXSpeed) : p.jumpXSpeed);
					}
					base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
					if ((!goingRight && base.transform.position.x < -maxX) || (goingRight && base.transform.position.x > maxX))
					{
						base.transform.SetPosition(new float?((!goingRight) ? (-maxX) : maxX), null, null);
						goingRight = !goingRight;
						if (velocityY < 0f)
						{
							velocityX = 0f;
						}
					}
					yield return wait;
				}
				CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
				base.transform.SetPosition(new float?(base.transform.position.x + velocityX / Mathf.Abs(velocityY) * 75f), new float?(this.onGroundY), null);
				this.landFXAnimator.transform.position = base.transform.position;
				this.landFXAnimator.Play("LandFX");
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55f && !this.isDead)
				{
					yield return null;
				}
				if (i < p.numBounces && !this.isDead)
				{
					velocityY = p.jumpYSpeed;
					velocityX = ((!goingRight) ? (-p.jumpXSpeed) : p.jumpXSpeed);
					gravity = p.jumpGravity;
					base.transform.position += Vector3.up * 76f;
					base.transform.position += Vector3.right * (velocityX / Mathf.Abs(velocityY) * 75f);
					timeToGround = this.TimeToGround(velocityY, this.onGroundY + 75f, gravity);
					animTimeOnLand = -1f;
				}
				if (this.isDead)
				{
					break;
				}
				yield return wait;
			}
			foreach (Collider2D collider2D3 in this.colliders)
			{
				collider2D3.enabled = false;
			}
			if (this.isDead && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Explode"))
			{
				base.animator.Play("Explode", 0, 0f);
				base.animator.Update(0f);
			}
		}
		yield break;
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000208D7 File Offset: 0x0001EAD7
	public void EndBouncer()
	{
		base.StartCoroutine(this.end_bouncer_cr());
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000C9D04 File Offset: 0x000C7F04
	public IEnumerator end_bouncer_cr()
	{
		this.isDead = true;
		yield return base.animator.WaitForAnimationToStart(this, "Off", false);
		foreach (Collider2D collider2D in this.colliders)
		{
			collider2D.enabled = false;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		foreach (CollisionChild collisionChild in this.collisionKids)
		{
			collisionChild.OnPlayerCollision -= this.OnCollisionPlayer;
			collisionChild.OnPlayerProjectileCollision -= this.OnCollisionPlayerProjectile;
		}
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000C9D20 File Offset: 0x000C7F20
	public void LateUpdate()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleHash)
		{
			this.shadow.sprite = this.shadowSprites[(int)(Mathf.InverseLerp(this.maxShadowHeight, this.minShadowHeight, base.transform.position.y) * (float)(this.shadowSprites.Length - 1))];
		}
		this.shadow.transform.position = new Vector3(base.transform.position.x, this.onGroundY);
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000C9DC0 File Offset: 0x000C7FC0
	public override void OnPause()
	{
		base.OnPause();
		this.pauseShadow.sprite = this.shadow.sprite;
		this.pauseShadow.transform.position = new Vector3(this.shadow.transform.position.x, this.onGroundY);
		this.shadow.enabled = false;
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000208E6 File Offset: 0x0001EAE6
	public override void OnUnpause()
	{
		base.OnUnpause();
		this.pauseShadow.sprite = null;
		this.shadow.enabled = true;
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x00020906 File Offset: 0x0001EB06
	public void AnimationEvent_SFX_SALTB_Bouncer_Bounce()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_bounce");
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x00020922 File Offset: 0x0001EB22
	public void AnimationEvent_SFX_SALTB_Bouncer_Death()
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p3_bouncer_twirl");
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_death");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_death");
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x00020948 File Offset: 0x0001EB48
	public void SFX_SALTB_Bouncer_Twirl()
	{
		AudioManager.PlayLoop("sfx_dlc_saltbaker_p3_bouncer_twirl");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_twirl");
	}

	// Token: 0x04001FF8 RID: 8184
	public const float IDLE_ANIM_LENGTH = 0.6666667f;

	// Token: 0x04001FF9 RID: 8185
	public const float ANIM_TIME_PRE_LAND = 0.1f;

	// Token: 0x04001FFA RID: 8186
	public const float NORMALIZED_ANIM_TIME_TO_RELAUNCH = 0.55f;

	// Token: 0x04001FFB RID: 8187
	public const float TARGET_TIME_LAND_A = 0.84375f;

	// Token: 0x04001FFC RID: 8188
	public const float TARGET_TIME_LAND_B = 0.40625f;

	// Token: 0x04001FFD RID: 8189
	public const float GROUND_TRIGGER_OFFSET = 75f;

	// Token: 0x04001FFE RID: 8190
	public const float GROUND_POS_OFFSET = 13f;

	// Token: 0x04001FFF RID: 8191
	[SerializeField]
	public SaltbakerLevelBGSaltHands saltHands;

	// Token: 0x04002000 RID: 8192
	[SerializeField]
	public SpriteRenderer shadow;

	// Token: 0x04002001 RID: 8193
	[SerializeField]
	public SpriteRenderer pauseShadow;

	// Token: 0x04002002 RID: 8194
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04002003 RID: 8195
	[SerializeField]
	public CollisionChild[] collisionKids;

	// Token: 0x04002004 RID: 8196
	[SerializeField]
	public Animator landFXAnimator;

	// Token: 0x04002005 RID: 8197
	public DamageDealer damageDealer;

	// Token: 0x04002006 RID: 8198
	public DamageReceiver damageReceiver;

	// Token: 0x04002007 RID: 8199
	public Vector3 bouncerStartPos;

	// Token: 0x04002008 RID: 8200
	public float onGroundY;

	// Token: 0x04002009 RID: 8201
	[SerializeField]
	public Collider2D[] colliders;

	// Token: 0x0400200A RID: 8202
	public bool isDead;

	// Token: 0x0400200B RID: 8203
	public int shadowSprite;

	// Token: 0x0400200C RID: 8204
	public int idleHash;

	// Token: 0x0400200D RID: 8205
	public float minShadowHeight;

	// Token: 0x0400200E RID: 8206
	public float maxShadowHeight;
}
