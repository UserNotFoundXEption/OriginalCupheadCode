using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class OldManLevelScubaGnome : AbstractProjectile
{
	// Token: 0x060020D7 RID: 8407 RVA: 0x000B8F20 File Offset: 0x000B7120
	public virtual OldManLevelScubaGnome Init(Vector3 pos, AbstractPlayerController player, bool isTypeA, bool onLeft, bool dartParryable, LevelProperties.OldMan.ScubaGnomes properties, OldManLevelGnomeLeader leader)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		base.transform.SetScale(new float?((float)((!onLeft) ? 1 : -1)), null, null);
		this.properties = properties;
		this.player = player;
		this.hp = properties.hp;
		this.isTypeA = isTypeA;
		this.onLeft = onLeft;
		this.leader = leader;
		this.dartParryable = dartParryable;
		base.animator.SetBool("IsGreen", Rand.Bool());
		base.animator.Play("Start");
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x0001BF7F File Offset: 0x0001A17F
	public void OnEnable()
	{
		Level.Current.OnLevelEndEvent += this.Dead;
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x0001BF97 File Offset: 0x0001A197
	public void OnDisable()
	{
		if (Level.Current != null)
		{
			Level.Current.OnLevelEndEvent -= this.Dead;
		}
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x0001BFBF File Offset: 0x0001A1BF
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x0001BFEA File Offset: 0x0001A1EA
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x0001C00F File Offset: 0x0001A20F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x0001C02D File Offset: 0x0001A22D
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000B8FE4 File Offset: 0x000B71E4
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float start = base.transform.position.y;
		bool shotBullet = false;
		bool toTop = false;
		bool splashed = false;
		while (t < this.properties.scubaMoveTime)
		{
			float val = t / this.properties.scubaMoveTime;
			base.transform.SetPosition(null, new float?(start + Mathf.Sin(val * 3.14159274f / 2f) * this.properties.jumpHeight), null);
			t += CupheadTime.FixedDelta;
			if (!splashed && base.transform.position.y > this.leader.splashHandler.transform.position.y)
			{
				this.leader.splashHandler.SplashIn(base.transform.position.x + 35f * base.transform.localScale.x);
				splashed = true;
				this.SFX_JumpOut();
				this.SFX_Vocal();
			}
			this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.leader.splashHandler.transform.position.y + -50f, this.leader.splashHandler.transform.position.y + -50f - 140f, base.transform.position.y)) * 0.5f);
			if (!toTop && t >= 0.8f)
			{
				base.animator.SetTrigger("ToTop");
				toTop = true;
			}
			yield return wait;
		}
		splashed = false;
		t = 0f;
		while (t < this.properties.scubaMoveTime)
		{
			float val = t / this.properties.scubaMoveTime;
			base.transform.SetPosition(null, new float?(start + Mathf.Sin((val + 1f) * 3.14159274f / 2f) * this.properties.jumpHeight), null);
			t += CupheadTime.Delta;
			if (!splashed && base.transform.position.y < this.leader.splashHandler.transform.position.y - 75f)
			{
				this.leader.splashHandler.SplashIn(base.transform.position.x + 35f * base.transform.localScale.x);
				splashed = true;
				this.SFX_DiveDown();
			}
			this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.leader.splashHandler.transform.position.y + -50f, this.leader.splashHandler.transform.position.y + -50f - 140f, base.transform.position.y)) * 0.5f);
			if (!toTop && t >= 0.8f)
			{
				base.animator.SetTrigger("ToTop");
				toTop = true;
			}
			float dist = this.shootRoot.position.y - (this.player.center.y + this.properties.shootDistOffset);
			if (!shotBullet && (dist < 10f || this.shootRoot.position.y < 0f))
			{
				base.animator.SetTrigger("Shoot");
				shotBullet = true;
			}
			yield return wait;
		}
		this.Recycle<OldManLevelScubaGnome>();
		yield return null;
		yield break;
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000B9000 File Offset: 0x000B7200
	public void Shoot()
	{
		float rotation = (base.transform.position.x >= 0f) ? 180f : 0f;
		float speed = (!this.isTypeA) ? this.properties.shotSpeedB : this.properties.shotSpeedA;
		BasicProjectile basicProjectile = this.projectile.Create(this.shootRoot.position, rotation, speed);
		basicProjectile.SetParryable(this.dartParryable);
		if (this.dartParryable)
		{
			basicProjectile.GetComponent<Animator>().Play("Pink");
		}
		basicProjectile.GetComponent<SpriteRenderer>().flipY = (base.transform.position.x > 0f);
		this.SFX_ShootDart();
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000B90D4 File Offset: 0x000B72D4
	public void Dead()
	{
		this.deathPuff.Create(base.transform.position);
		for (int i = 0; i < this.deathParts.Length; i++)
		{
			if (i != 0 || Random.Range(0, 10) == 0)
			{
				SpriteDeathParts spriteDeathParts = this.deathParts[i].CreatePart(base.transform.position);
				if (i != 0)
				{
					spriteDeathParts.animator.Play((!base.animator.GetBool("IsGreen")) ? "_Blue" : "_Teal");
				}
			}
		}
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
		AudioManager.Stop("sfx_dlc_omm_p3_gnomediver_vocal");
		this.Recycle<OldManLevelScubaGnome>();
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x0001C062 File Offset: 0x0001A262
	public void SFX_DiveDown()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_divedown");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_divedown");
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x0001C07E File Offset: 0x0001A27E
	public void SFX_JumpOut()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_jumpout");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_jumpout");
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x0001C09A File Offset: 0x0001A29A
	public void SFX_ShootDart()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_shootdart");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_shootdart");
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x0001C0B6 File Offset: 0x0001A2B6
	public void SFX_Vocal()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_vocal");
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x0001C0D2 File Offset: 0x0001A2D2
	public void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.projectile = null;
		this.shootRoot = null;
		this.player = null;
		this.leader = null;
		this.underwaterSprite = null;
	}

	// Token: 0x04001AFA RID: 6906
	public const float SPLASH_IN_TRIGGER_OFFSET = 75f;

	// Token: 0x04001AFB RID: 6907
	public const float SPLASH_X_POSITION_OFFSET = 35f;

	// Token: 0x04001AFC RID: 6908
	public const float UNDERWATER_FADE_OFFSET = -50f;

	// Token: 0x04001AFD RID: 6909
	public const float LOWEST_SHOOT_POS = 0f;

	// Token: 0x04001AFE RID: 6910
	[Header("Death FX")]
	[SerializeField]
	public Effect deathPuff;

	// Token: 0x04001AFF RID: 6911
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04001B00 RID: 6912
	[Header("Prefabs")]
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001B01 RID: 6913
	[SerializeField]
	public Transform shootRoot;

	// Token: 0x04001B02 RID: 6914
	public const float Y_DIST_TO_SHOOT = 10f;

	// Token: 0x04001B03 RID: 6915
	public float hp;

	// Token: 0x04001B04 RID: 6916
	public bool isTypeA;

	// Token: 0x04001B05 RID: 6917
	public bool onLeft;

	// Token: 0x04001B06 RID: 6918
	public LevelProperties.OldMan.ScubaGnomes properties;

	// Token: 0x04001B07 RID: 6919
	public AbstractPlayerController player;

	// Token: 0x04001B08 RID: 6920
	public OldManLevelGnomeLeader leader;

	// Token: 0x04001B09 RID: 6921
	public DamageReceiver damageReceiver;

	// Token: 0x04001B0A RID: 6922
	public bool dartParryable;

	// Token: 0x04001B0B RID: 6923
	[SerializeField]
	public SpriteRenderer underwaterSprite;
}
