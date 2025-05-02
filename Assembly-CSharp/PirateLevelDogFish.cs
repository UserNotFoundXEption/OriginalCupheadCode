using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class PirateLevelDogFish : AbstractProjectile
{
	// Token: 0x0600219D RID: 8605 RVA: 0x000BAD5C File Offset: 0x000B8F5C
	public override void Awake()
	{
		base.Awake();
		base.transform.position = PirateLevelDogFish.START_POS;
		this.normalHitBox.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.normalHitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTaken;
		this.secretHitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTakenFromBehind;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000BADDC File Offset: 0x000B8FDC
	public override void Update()
	{
		base.Update();
		if (this.state == PirateLevelDogFish.State.Slide)
		{
			base.transform.AddPosition(-this.speedY * CupheadTime.Delta, 0f, 0f);
			float num = this.slideTime / this.dogfish.speedFalloffTime;
			if (num < 1f)
			{
				this.speedY = EaseUtils.EaseOutQuart(this.dogfish.startSpeed, this.dogfish.endSpeed, num);
				this.slideTime += CupheadTime.Delta;
			}
			else
			{
				this.speedY = this.dogfish.endSpeed;
			}
			if (base.transform.position.x < -1000f)
			{
				this.properties.OnBossDeath -= this.OnBossDeath;
				Object.Destroy(base.gameObject);
			}
			if (this.bossDied)
			{
				this.Die();
			}
			if (PirateLevelDogFish.dogKilled && this.isSecret)
			{
				this.isSecret = false;
				this.OnEnableCollider();
			}
		}
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x0001CC18 File Offset: 0x0001AE18
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.state != PirateLevelDogFish.State.Death)
		{
			base.OnCollisionPlayer(hit, phase);
			if (phase != CollisionPhase.Exit)
			{
				this.damageDealer.DealDamage(hit);
			}
		}
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000BAF04 File Offset: 0x000B9104
	public void Init(LevelProperties.Pirate properties, bool isSecret)
	{
		this.properties = properties;
		this.dogfish = properties.CurrentState.dogFish;
		this.isSecret = isSecret;
		this.hp = (float)this.dogfish.hp;
		this.state = PirateLevelDogFish.State.Jump;
		this.normalHitBox.GetComponent<DamageReceiver>().enabled = false;
		AudioManager.Play("level_pirate_dogfish_jump");
		this.emitAudioFromObject.Add("level_pirate_dogfish_jump");
		this.splashEffect.Create(this.splashRoot.position);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000BAFA0 File Offset: 0x000B91A0
	public void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f & this.state != PirateLevelDogFish.State.Death)
		{
			this.OnDying();
			this.secretHitBox.GetComponent<Collider2D>().enabled = false;
			this.Die();
		}
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000BAFFC File Offset: 0x000B91FC
	public void OnDamageTakenFromBehind(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != PirateLevelDogFish.State.Death)
		{
			this.OnDying();
			this.SetParryable(true);
			this.Die();
		}
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000BB04C File Offset: 0x000B924C
	public void OnDying()
	{
		AudioManager.Stop("level_pirate_dogfish_jump");
		AudioManager.Play("level_pirate_dogfish_death_poof");
		this.emitAudioFromObject.Add("level_pirate_dogfish_death_poof");
		PirateLevelDogFish.dogKilled = true;
		this.normalHitBox.GetComponent<Collider2D>().enabled = false;
		this.secretHitBox.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x0001CC42 File Offset: 0x0001AE42
	public void OnEnableCollider()
	{
		this.normalHitBox.GetComponent<DamageReceiver>().enabled = true;
		this.secretHitBox.GetComponent<DamageReceiver>().enabled = false;
		base.gameObject.layer = 0;
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000BB0A8 File Offset: 0x000B92A8
	public void OnJumpAnimationComplete()
	{
		if (this.state != PirateLevelDogFish.State.Death)
		{
			this.state = PirateLevelDogFish.State.Slide;
			AudioManager.Play("level_pirate_dogfish_slide");
			this.emitAudioFromObject.Add("level_pirate_dogfish_slide");
			this.slideTime = 0f;
			this.speedY = this.dogfish.startSpeed;
		}
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000BB100 File Offset: 0x000B9300
	public override void Die()
	{
		this.state = PirateLevelDogFish.State.Death;
		this.properties.OnBossDeath -= this.OnBossDeath;
		base.animator.SetTrigger("OnDeath");
		this.deathEffect.Create(base.transform.position);
		base.StartCoroutine(this.deathFloat_cr());
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x0001CC72 File Offset: 0x0001AE72
	public void OnBossDeath()
	{
		this.bossDied = true;
		if (this.state == PirateLevelDogFish.State.Slide)
		{
			this.Die();
		}
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000BB160 File Offset: 0x000B9360
	public IEnumerator deathFloat_cr()
	{
		AudioManager.Play("level_pirate_dogfish_death_flap");
		this.emitAudioFromObject.Add("level_pirate_dogfish_death_flap");
		while (base.transform.position.y < 360f)
		{
			base.transform.AddPosition(0f, this.properties.CurrentState.dogFish.deathSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x0001CC8D File Offset: 0x0001AE8D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.splashEffect = null;
		this.deathEffect = null;
	}

	// Token: 0x04001BB2 RID: 7090
	public static readonly Vector2 START_POS = new Vector2(235f, -245f);

	// Token: 0x04001BB3 RID: 7091
	public const float DEATH_Y = 450f;

	// Token: 0x04001BB4 RID: 7092
	[SerializeField]
	public Collider2D secretHitBox;

	// Token: 0x04001BB5 RID: 7093
	[SerializeField]
	public Collider2D normalHitBox;

	// Token: 0x04001BB6 RID: 7094
	[SerializeField]
	public Effect splashEffect;

	// Token: 0x04001BB7 RID: 7095
	[SerializeField]
	public Transform splashRoot;

	// Token: 0x04001BB8 RID: 7096
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x04001BB9 RID: 7097
	public PirateLevelDogFish.State state;

	// Token: 0x04001BBA RID: 7098
	public float hp;

	// Token: 0x04001BBB RID: 7099
	public float speedY;

	// Token: 0x04001BBC RID: 7100
	public float slideTime;

	// Token: 0x04001BBD RID: 7101
	public LevelProperties.Pirate properties;

	// Token: 0x04001BBE RID: 7102
	public LevelProperties.Pirate.DogFish dogfish;

	// Token: 0x04001BBF RID: 7103
	public bool bossDied;

	// Token: 0x04001BC0 RID: 7104
	public bool isSecret;

	// Token: 0x04001BC1 RID: 7105
	public static bool dogKilled = false;

	// Token: 0x02000E10 RID: 3600
	public enum State
	{
		// Token: 0x040065D9 RID: 26073
		Init,
		// Token: 0x040065DA RID: 26074
		Jump,
		// Token: 0x040065DB RID: 26075
		Slide,
		// Token: 0x040065DC RID: 26076
		Death
	}
}
