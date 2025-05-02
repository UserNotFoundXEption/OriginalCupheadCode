using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class OldManLevelGnomeClimber : AbstractProjectile
{
	// Token: 0x0600204C RID: 8268 RVA: 0x000B7A90 File Offset: 0x000B5C90
	public virtual OldManLevelGnomeClimber Init(float startXPosition, float facing, Transform smashPos, LevelProperties.OldMan.ClimberGnomes properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(startXPosition, -165f);
		this.smashPos = smashPos;
		this.properties = properties;
		base.transform.SetScale(new float?(facing), null, null);
		this.smashFXA = Rand.Bool();
		if (!properties.canDestroy)
		{
			this.rigidbody.simulated = false;
		}
		this.hp = properties.health;
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.StartMoving();
		return this;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x0001B6B7 File Offset: 0x000198B7
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x0001B6C5 File Offset: 0x000198C5
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000B7B40 File Offset: 0x000B5D40
	public override void Die()
	{
		this.deathPuff.Create(base.transform.position);
		this.deathParts[0].Create(base.transform.position);
		this.deathParts[1].Create(base.transform.position);
		SpriteDeathParts spriteDeathParts = this.hat.CreatePart(base.transform.position);
		spriteDeathParts.animator.Play("_Teal");
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
		this.Recycle<OldManLevelGnomeClimber>();
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x0001B6FA File Offset: 0x000198FA
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x0001B718 File Offset: 0x00019918
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x0001B736 File Offset: 0x00019936
	public void StartMoving()
	{
		base.StartCoroutine(this.move_up_cr());
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000B7BE0 File Offset: 0x000B5DE0
	public IEnumerator move_up_cr()
	{
		base.animator.SetBool("DualSmash", this.properties.dualSmash);
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.climbSpeed;
		yield return base.animator.WaitForAnimationToEnd(this, "Appear", false, true);
		while (this.smashPos != null && base.transform.position.y < this.smashPos.position.y + 60f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		base.transform.parent = this.smashPos;
		base.animator.SetTrigger("ReachedTop");
		yield return base.animator.WaitForAnimationToEnd(this, "ReachedTop", false, true);
		if (this.smashPos != null)
		{
			base.transform.SetPosition(new float?(this.smashPos.position.x), new float?(this.smashPos.position.y + 100f), null);
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.preAttackDelay);
		base.animator.Play("Anticipation");
		yield return base.animator.WaitForAnimationToEnd(this, "Anticipation", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelay);
		base.animator.SetTrigger("Attack");
		string vanishAnimation = (!this.properties.dualSmash) ? "Vanish" : "Vanish_Flipped";
		yield return base.animator.WaitForAnimationToEnd(this, vanishAnimation, false, true);
		this.Recycle<OldManLevelGnomeClimber>();
		yield return null;
		yield break;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000B7BFC File Offset: 0x000B5DFC
	public void AniEvent_SpawnEffect(AnimationEvent ev)
	{
		Effect effect = this.smashEffect.Create(base.transform.position + new Vector3(this.smashRoot.localPosition.x * base.transform.localScale.x * ev.floatParameter, this.smashRoot.localPosition.y));
		effect.transform.SetScale(new float?(base.transform.localScale.x * ev.floatParameter), null, null);
		effect.GetComponent<Animator>().Play((!this.smashFXA) ? "B" : "A");
		this.smashFXA = !this.smashFXA;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x0001B745 File Offset: 0x00019945
	public void AnimationEvent_SFX_OMM_Gnome_ClimberHammer()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attack");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attack");
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x0001B761 File Offset: 0x00019961
	public void AnimationEvent_SFX_OMM_Gnome_ClimberHammerVocal()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attackvocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attackvocal");
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x0001B77D File Offset: 0x0001997D
	public void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.hat = null;
		this.smashEffect = null;
		this.smashRoot = null;
		this.smashPos = null;
		this.rigidbody = null;
	}

	// Token: 0x04001A64 RID: 6756
	public const float START_Y = -165f;

	// Token: 0x04001A65 RID: 6757
	public const float CLIMB_X_OFFSET = 120f;

	// Token: 0x04001A66 RID: 6758
	public const float TOP_Y_OFFSET = 60f;

	// Token: 0x04001A67 RID: 6759
	public const float SMASH_Y_OFFSET = 100f;

	// Token: 0x04001A68 RID: 6760
	[SerializeField]
	public Effect deathPuff;

	// Token: 0x04001A69 RID: 6761
	[SerializeField]
	public Effect[] deathParts;

	// Token: 0x04001A6A RID: 6762
	[SerializeField]
	public SpriteDeathPartsDLC hat;

	// Token: 0x04001A6B RID: 6763
	[SerializeField]
	public Effect smashEffect;

	// Token: 0x04001A6C RID: 6764
	[SerializeField]
	public Transform smashRoot;

	// Token: 0x04001A6D RID: 6765
	public LevelProperties.OldMan.ClimberGnomes properties;

	// Token: 0x04001A6E RID: 6766
	public Transform smashPos;

	// Token: 0x04001A6F RID: 6767
	[SerializeField]
	public DamageReceiver damageReceiver;

	// Token: 0x04001A70 RID: 6768
	[SerializeField]
	public new Rigidbody2D rigidbody;

	// Token: 0x04001A71 RID: 6769
	public bool smashFXA;

	// Token: 0x04001A72 RID: 6770
	public float hp;
}
