using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class DicePalacePachinkoLevelPachinko : LevelProperties.DicePalacePachinko.Entity
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x0600176E RID: 5998 RVA: 0x00013F24 File Offset: 0x00012124
	// (set) Token: 0x0600176F RID: 5999 RVA: 0x00013F2C File Offset: 0x0001212C
	public bool attacking { get; set; }

	// Token: 0x06001770 RID: 6000 RVA: 0x000A1980 File Offset: 0x0009FB80
	public override void Awake()
	{
		this.reversing = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnWinEvent += this.OnDeath;
		base.Awake();
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x00013F35 File Offset: 0x00012135
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x00013F42 File Offset: 0x00012142
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.pct = 1f - base.properties.CurrentHealth / this.initialHP;
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000A19E0 File Offset: 0x0009FBE0
	public override void LevelInit(LevelProperties.DicePalacePachinko properties)
	{
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		base.LevelInit(properties);
		this.attacking = false;
		this.direction = 1;
		this.pct = 0f;
		this.initialHP = properties.CurrentHealth;
		this.baseSpeed = properties.CurrentState.boss.movementSpeed.min;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000A1A58 File Offset: 0x0009FC58
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_pachinko_intro");
		this.emitAudioFromObject.Add("dice_palace_pachinko_intro");
		yield return null;
		yield break;
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x00013F73 File Offset: 0x00012173
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.check_position_cr());
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x00013F9C File Offset: 0x0001219C
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x000A1A74 File Offset: 0x0009FC74
	public IEnumerator move_cr()
	{
		AudioManager.PlayLoop("dice_palace_pachinko_movement_loop");
		this.emitAudioFromObject.Add("dice_palace_pachinko_movement_loop");
		for (;;)
		{
			float speed = this.baseSpeed + (base.properties.CurrentState.boss.movementSpeed.max - base.properties.CurrentState.boss.movementSpeed.min) * this.pct;
			base.transform.position += Vector3.right * speed * (float)this.direction * CupheadTime.Delta * this.hitPauseCoefficient();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x000A1A90 File Offset: 0x0009FC90
	public IEnumerator check_position_cr()
	{
		for (;;)
		{
			if ((base.transform.position.x < -640f + base.properties.CurrentState.boss.leftBoundaryOffset && this.direction == -1) || (base.transform.position.x > 640f - base.properties.CurrentState.boss.rightBoundaryOffset && this.direction == 1))
			{
				base.StartCoroutine(this.reverse_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x000A1AAC File Offset: 0x0009FCAC
	public IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.initialAttackDelay);
		for (;;)
		{
			base.StartCoroutine(this.lights_cr());
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Warning_Start", false, true);
			this.BeamWarning();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration);
			base.animator.SetTrigger("Continue");
			AudioManager.Play("dice_palace_pachinko_warning_trans");
			this.emitAudioFromObject.Add("dice_palace_pachinko_warning_trans");
			this.attacking = true;
			this.BeamOn();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.beamDuration);
			this.BeamOff();
			this.attacking = false;
			base.animator.SetTrigger("OnEnd");
			AudioManager.Play("dice_palace_pachinko_trans_out");
			this.emitAudioFromObject.Add("dice_palace_pachinko_trans_out");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Trans_Out", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x00013FBD File Offset: 0x000121BD
	public void BeamWarning()
	{
		this.beam.SetActive(true);
		this.beam.GetComponent<Collider2D>().enabled = false;
		this.beam.GetComponent<SpriteRenderer>().sprite = this.beamSprites[0];
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x00013FF4 File Offset: 0x000121F4
	public void BeamOn()
	{
		this.beam.SetActive(true);
		this.beam.GetComponent<Collider2D>().enabled = true;
		this.beam.GetComponent<SpriteRenderer>().sprite = this.beamSprites[1];
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x0001402B File Offset: 0x0001222B
	public void BeamOff()
	{
		this.beam.SetActive(false);
		this.beam.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x000A1AC8 File Offset: 0x0009FCC8
	public IEnumerator lights_cr()
	{
		float fastSpeed = 6f;
		float fadeTime = 0.01f;
		foreach (Transform light in this.lights)
		{
			light.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration / (float)this.lights.Length);
		}
		bool fadingOut = false;
		while (!this.attacking)
		{
			yield return null;
		}
		this.fire.speed = fastSpeed;
		while (this.attacking)
		{
			foreach (Transform transform in this.lights)
			{
				if (fadingOut)
				{
					transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
				}
				else
				{
					transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				}
			}
			fadingOut = !fadingOut;
			yield return CupheadTime.WaitForSeconds(this, fadeTime);
			yield return null;
		}
		base.StartCoroutine(this.fire_speed_cr(fastSpeed));
		foreach (Transform transform2 in this.lights)
		{
			transform2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
		foreach (Transform light2 in this.lights)
		{
			light2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration / (float)this.lights.Length);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000A1AE4 File Offset: 0x0009FCE4
	public IEnumerator fire_speed_cr(float fastSpeed)
	{
		while (fastSpeed > 0f)
		{
			fastSpeed -= 0.1f;
			this.fire.speed = fastSpeed;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000A1B08 File Offset: 0x0009FD08
	public IEnumerator reverse_cr()
	{
		if (!this.reversing)
		{
			this.reversing = true;
			this.direction *= -1;
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.reversing = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x0001404A File Offset: 0x0001224A
	public void OnDeath()
	{
		AudioManager.Stop("dice_palace_pachinko_movement_loop");
		AudioManager.Play("dice_palace_pachinko_death");
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x00014082 File Offset: 0x00012282
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x00014099 File Offset: 0x00012299
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x0400130E RID: 4878
	[SerializeField]
	public Animator fire;

	// Token: 0x0400130F RID: 4879
	[SerializeField]
	public Transform[] lights;

	// Token: 0x04001310 RID: 4880
	[SerializeField]
	public Sprite[] beamSprites;

	// Token: 0x04001311 RID: 4881
	[SerializeField]
	public GameObject beam;

	// Token: 0x04001312 RID: 4882
	public bool reversing;

	// Token: 0x04001313 RID: 4883
	public int direction;

	// Token: 0x04001314 RID: 4884
	public float baseSpeed;

	// Token: 0x04001315 RID: 4885
	public float pct;

	// Token: 0x04001316 RID: 4886
	public float initialHP;

	// Token: 0x04001317 RID: 4887
	public DamageDealer damageDealer;

	// Token: 0x04001318 RID: 4888
	public DamageReceiver damageReceiver;
}
