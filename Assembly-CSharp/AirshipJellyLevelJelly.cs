using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class AirshipJellyLevelJelly : LevelProperties.AirshipJelly.Entity
{
	// Token: 0x06000F12 RID: 3858 RVA: 0x0000CBED File Offset: 0x0000ADED
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x0008CE38 File Offset: 0x0008B038
	public void Start()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.startColor = base.GetComponent<SpriteRenderer>().color;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		CupheadLevelCamera.Current.StartFloat(25f, 3f);
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0008CE8C File Offset: 0x0008B08C
	public override void LevelInit(LevelProperties.AirshipJelly properties)
	{
		base.LevelInit(properties);
		this.knobSwitch = AirshipJellyLevelKnob.Create(this);
		this.knobSwitch.OnActivate += this.OnKnobParry;
		this.knobSwitch.OnPrePauseActivate += this.OnKnobPreParry;
		this.maxHealth = properties.CurrentHealth;
		this.defaultSpeed = properties.CurrentState.main.speed.min;
		this.speed = this.defaultSpeed;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0008CF28 File Offset: 0x0008B128
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		base.StartCoroutine(this.flash_cr());
		this.GetNewSpeed();
		if (base.properties.CurrentHealth <= 0f && this.state != AirshipJellyLevelJelly.State.Dead)
		{
			this.state = AirshipJellyLevelJelly.State.Dead;
			base.animator.Play("Death");
		}
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0000CC18 File Offset: 0x0000AE18
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0000CC30 File Offset: 0x0000AE30
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0000CC52 File Offset: 0x0000AE52
	public void OnLevelStart()
	{
		this.state = AirshipJellyLevelJelly.State.Running;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0008CF94 File Offset: 0x0008B194
	public void GetNewSpeed()
	{
		MinMax minMax = base.properties.CurrentState.main.speed;
		float num = base.properties.CurrentHealth / this.maxHealth;
		float num2 = 1f - num;
		this.speed = this.defaultSpeed + minMax.max * num2;
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0008CFE8 File Offset: 0x0008B1E8
	public void OnTurnComplete()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0000CC68 File Offset: 0x0000AE68
	public void OnKnobParry()
	{
		base.StartCoroutine(this.hurt_cr());
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0000CC77 File Offset: 0x0000AE77
	public void OnKnobPreParry()
	{
		AudioManager.Play("levels_airship_jelly_hit");
		this.smashEffect.Create(this.knobRoot.position);
		CupheadLevelCamera.Current.Shake(10f, 0.6f, false);
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0000CCAF File Offset: 0x0000AEAF
	public void SfxWalk()
	{
		AudioManager.Play("levels_airship_jelly_walk");
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0000CCBB File Offset: 0x0000AEBB
	public void ResetMove()
	{
		if (this.moveCoroutine != null)
		{
			base.StopCoroutine(this.moveCoroutine);
			this.moveCoroutine = null;
		}
		this.moveCoroutine = base.StartCoroutine(this.jelly_cr());
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0008D02C File Offset: 0x0008B22C
	public IEnumerator start_cr()
	{
		base.animator.SetTrigger("OnIntroComplete");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Transition", false, true);
		this.ResetMove();
		yield break;
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0008D048 File Offset: 0x0008B248
	public IEnumerator jelly_cr()
	{
		while (base.properties == null)
		{
		}
		float offset = 100f;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			if (this.direction == AirshipJellyLevelJelly.Direction.Left)
			{
				while (base.transform.position.x > -640f + offset)
				{
					if (!this.Moving)
					{
						yield return base.StartCoroutine(this.waitForMove_cr());
					}
					pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + offset, this.speed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				this.direction = AirshipJellyLevelJelly.Direction.Right;
			}
			else if (this.direction == AirshipJellyLevelJelly.Direction.Right)
			{
				while (base.transform.position.x < 640f - offset)
				{
					if (!this.Moving)
					{
						yield return base.StartCoroutine(this.waitForMove_cr());
					}
					pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, this.speed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				this.direction = AirshipJellyLevelJelly.Direction.Left;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0000CCED File Offset: 0x0000AEED
	public bool Moving
	{
		get
		{
			return this.state == AirshipJellyLevelJelly.State.Running;
		}
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0008D064 File Offset: 0x0008B264
	public IEnumerator waitForMove_cr()
	{
		while (!this.Moving)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0008D080 File Offset: 0x0008B280
	public IEnumerator hurt_cr()
	{
		base.properties.DealDamage(base.properties.CurrentState.main.parryDamage);
		this.GetNewSpeed();
		this.knobSprite.enabled = false;
		this.knobSwitch.enabled = false;
		AudioManager.Play("levels_airship_jelly_hurt");
		if (base.properties.CurrentHealth <= 0f)
		{
			this.state = AirshipJellyLevelJelly.State.Dead;
			base.animator.Play("Death");
		}
		else
		{
			base.animator.SetTrigger("OnHurt");
			AirshipJellyLevelJelly.State lastState = this.state;
			this.state = AirshipJellyLevelJelly.State.Hurt;
			this.ResetMove();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.hurtDelay);
			this.state = lastState;
			base.animator.SetTrigger("OnHurtComplete");
			yield return base.animator.WaitForAnimationToEnd(this, "Hurt_Loop", false, true);
			this.state = AirshipJellyLevelJelly.State.Running;
			base.StartCoroutine(this.enableKnob_cr());
		}
		yield break;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0008D09C File Offset: 0x0008B29C
	public IEnumerator enableKnob_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.orbDelay);
		this.knobSprite.enabled = true;
		this.knobSwitch.enabled = true;
		yield break;
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0008D0B8 File Offset: 0x0008B2B8
	public void SetColor(float t)
	{
		Color color = Color.Lerp(this.flashColor, Color.black, t);
		this.spriteRenderer.color = color;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0008D0E4 File Offset: 0x0008B2E4
	public IEnumerator flash_cr()
	{
		float t = 0f;
		float time = 0.15f;
		while (t < time)
		{
			float val = t / time;
			this.SetColor(val);
			t += Time.deltaTime;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = this.startColor;
		yield break;
	}

	// Token: 0x04000C56 RID: 3158
	public Color startColor;

	// Token: 0x04000C57 RID: 3159
	public Color flashColor = Color.red;

	// Token: 0x04000C58 RID: 3160
	public Transform knobRoot;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	public SpriteRenderer knobSprite;

	// Token: 0x04000C5A RID: 3162
	[Space(10f)]
	[SerializeField]
	public Effect smashEffect;

	// Token: 0x04000C5B RID: 3163
	public float speed;

	// Token: 0x04000C5C RID: 3164
	public float defaultSpeed;

	// Token: 0x04000C5D RID: 3165
	public float maxHealth;

	// Token: 0x04000C5E RID: 3166
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000C5F RID: 3167
	public DamageDealer damageDealer;

	// Token: 0x04000C60 RID: 3168
	public DamageReceiver damageReceiver;

	// Token: 0x04000C61 RID: 3169
	public AirshipJellyLevelKnob knobSwitch;

	// Token: 0x04000C62 RID: 3170
	public AirshipJellyLevelJelly.State state;

	// Token: 0x04000C63 RID: 3171
	public AirshipJellyLevelJelly.Direction direction = AirshipJellyLevelJelly.Direction.Left;

	// Token: 0x04000C64 RID: 3172
	public const float MIN_X = -550f;

	// Token: 0x04000C65 RID: 3173
	public const float MAX_X = 550f;

	// Token: 0x04000C66 RID: 3174
	public Coroutine moveCoroutine;

	// Token: 0x020009E9 RID: 2537
	public enum State
	{
		// Token: 0x04004995 RID: 18837
		Init,
		// Token: 0x04004996 RID: 18838
		Running,
		// Token: 0x04004997 RID: 18839
		Hurt,
		// Token: 0x04004998 RID: 18840
		Dead
	}

	// Token: 0x020009EA RID: 2538
	public enum Direction
	{
		// Token: 0x0400499A RID: 18842
		Right,
		// Token: 0x0400499B RID: 18843
		Left
	}
}
