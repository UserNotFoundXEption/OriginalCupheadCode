using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class SaltbakerLevelDough : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x060026DD RID: 9949 RVA: 0x000C9FF4 File Offset: 0x000C81F4
	public virtual SaltbakerLevelDough Init(Vector3 startPos, float speedX, float speedY, float gravity, float hp, int sortingOrder, int animalType)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		this.speedX = speedX;
		this.speedY = speedY;
		this.gravity = gravity;
		this.fromLeft = (speedX > 0f);
		this.hp = hp;
		this.Jump();
		base.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		this.animalType = animalType;
		base.animator.Play(this.clipNames[animalType] + "Up");
		return this;
	}

	// Token: 0x060026DE RID: 9950 RVA: 0x00020A63 File Offset: 0x0001EC63
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060026DF RID: 9951 RVA: 0x00020A8E File Offset: 0x0001EC8E
	public override bool SparksFollow()
	{
		return Rand.Bool();
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x00020A95 File Offset: 0x0001EC95
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060026E1 RID: 9953 RVA: 0x00020AB3 File Offset: 0x0001ECB3
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060026E2 RID: 9954 RVA: 0x00020ADE File Offset: 0x0001ECDE
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x00020AFC File Offset: 0x0001ECFC
	public void Jump()
	{
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000CA080 File Offset: 0x000C8280
	public IEnumerator jump_cr()
	{
		base.animator.Play(Random.Range(0, 4).ToString(), 1, 0f);
		base.transform.localScale = new Vector3(Mathf.Sign(this.speedX), 1f);
		float velocityX = this.speedX;
		float velocityY = this.speedY;
		float sizeX = base.GetComponent<Collider2D>().bounds.size.x;
		float sizeY = base.GetComponent<Collider2D>().bounds.size.y;
		float ground = (float)Level.Current.Ground + sizeY / 2f + 50f;
		bool jumping = false;
		bool goingUp = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			velocityY = this.speedY;
			velocityX = this.speedX;
			jumping = true;
			goingUp = true;
			bool arcTriggered = false;
			base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
			while (jumping)
			{
				velocityY -= this.gravity * CupheadTime.FixedDelta;
				base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
				base.HandleShadow(0f, 0f);
				if (goingUp && !arcTriggered && velocityY <= this.gravity * 4f * CupheadTime.FixedDelta)
				{
					base.animator.SetTrigger("Arc");
					arcTriggered = true;
				}
				if (velocityY < 0f && goingUp)
				{
					goingUp = false;
					arcTriggered = false;
				}
				if (velocityY < 0f && jumping && base.transform.position.y - velocityY * CupheadTime.FixedDelta <= ground)
				{
					jumping = false;
					base.transform.position = new Vector3(base.transform.position.x, ground);
					base.HandleShadow(0f, 0f);
					base.animator.SetTrigger("Bounce");
				}
				if ((base.transform.position.x < (float)Level.Current.Left - sizeX && !this.fromLeft) || (base.transform.position.x > (float)Level.Current.Right + sizeX && this.fromLeft))
				{
					this.Die();
				}
				yield return wait;
			}
			yield return base.animator.WaitForAnimationToStart(this, this.clipNames[this.animalType] + "Bounce", false);
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000CA09C File Offset: 0x000C829C
	public void AniEvent_SpawnDustCloud()
	{
		this.dustEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000CA0D8 File Offset: 0x000C82D8
	public override void Die()
	{
		this.StopAllCoroutines();
		this.coll.enabled = false;
		this.shadow.enabled = false;
		int num = Random.Range(1, 4);
		for (int i = 0; i < num; i++)
		{
			this.debris.Create(base.transform.position + MathUtils.RandomPointInUnitCircle() * 20f);
		}
		int num2 = Random.Range(1, 3);
		base.animator.Play("Death_" + num2);
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)((num2 >= 2) ? 1 : MathUtils.PlusOrMinus()));
		if (num2 < 2)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		}
		base.animator.Update(0f);
	}

	// Token: 0x060026E7 RID: 9959 RVA: 0x00020B0B File Offset: 0x0001ED0B
	public void AnimationEvent_SFX_SALTBAKER_CookieBounce()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookiebounce");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookiebounce");
	}

	// Token: 0x060026E8 RID: 9960 RVA: 0x00020B27 File Offset: 0x0001ED27
	public void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalCamel()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalcamel");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalcamel");
	}

	// Token: 0x060026E9 RID: 9961 RVA: 0x00020B43 File Offset: 0x0001ED43
	public void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalLion()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalLion");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalLion");
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x00020B5F File Offset: 0x0001ED5F
	public void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalElephant()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalElephant");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalElephant");
	}

	// Token: 0x04002018 RID: 8216
	public const float GROUND_OFFSET = 50f;

	// Token: 0x04002019 RID: 8217
	public float speedX;

	// Token: 0x0400201A RID: 8218
	public float speedY;

	// Token: 0x0400201B RID: 8219
	public float gravity;

	// Token: 0x0400201C RID: 8220
	public float hp;

	// Token: 0x0400201D RID: 8221
	public bool fromLeft;

	// Token: 0x0400201E RID: 8222
	public DamageReceiver damageReceiver;

	// Token: 0x0400201F RID: 8223
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04002020 RID: 8224
	[SerializeField]
	public Effect dustEffect;

	// Token: 0x04002021 RID: 8225
	[SerializeField]
	public Effect debris;

	// Token: 0x04002022 RID: 8226
	public string[] clipNames = new string[]
	{
		"Elephant",
		"Lion",
		"Camel"
	};

	// Token: 0x04002023 RID: 8227
	public int animalType;
}
