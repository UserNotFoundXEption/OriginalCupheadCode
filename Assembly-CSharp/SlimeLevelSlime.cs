using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class SlimeLevelSlime : LevelProperties.Slime.Entity
{
	// Token: 0x17000326 RID: 806
	// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000215A8 File Offset: 0x0001F7A8
	// (set) Token: 0x060027C1 RID: 10177 RVA: 0x000215AF File Offset: 0x0001F7AF
	public static bool TINIES { get; set; }

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000215B7 File Offset: 0x0001F7B7
	// (set) Token: 0x060027C3 RID: 10179 RVA: 0x000215BF File Offset: 0x0001F7BF
	public SlimeLevelSlime.State state { get; set; }

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000215C8 File Offset: 0x0001F7C8
	// (set) Token: 0x060027C5 RID: 10181 RVA: 0x000215D0 File Offset: 0x0001F7D0
	public LevelProperties.Slime.State CurrentPropertyState { get; set; }

	// Token: 0x060027C6 RID: 10182 RVA: 0x000CC940 File Offset: 0x000CAB40
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.onGroundY = base.transform.position.y;
		this.shadowY = this.shadow.transform.position.y;
		SlimeLevelSlime.TINIES = false;
		this.shadow.enabled = false;
		if (this.isBig)
		{
			foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
			{
				collider2D.enabled = false;
			}
		}
		foreach (Animator animator in this.questionMarks)
		{
			animator.GetComponent<Collider2D>().enabled = false;
		}
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x000215D9 File Offset: 0x0001F7D9
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000CCA38 File Offset: 0x000CAC38
	public void LateUpdate()
	{
		this.updateShadow(base.transform.position.y - this.onGroundY);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000215F1 File Offset: 0x0001F7F1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000CCA68 File Offset: 0x000CAC68
	public override void LevelInit(LevelProperties.Slime properties)
	{
		base.LevelInit(properties);
		this.CurrentPropertyState = properties.CurrentState;
		this.jumpsBeforeFirstPunch = this.CurrentPropertyState.jump.bigSlimeInitialJumpPunchCount;
		if (this.isBig)
		{
			properties.OnBossDeath += this.OnBossDeath;
		}
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000CCABC File Offset: 0x000CACBC
	public void QuestionMarksOn()
	{
		foreach (Animator animator in this.questionMarks)
		{
			animator.transform.SetScale(new float?(base.transform.localScale.x), null, null);
			animator.SetBool("IsOn", true);
			animator.GetComponent<Collider2D>().enabled = true;
		}
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000CCB38 File Offset: 0x000CAD38
	public void QuestionMarksOff()
	{
		foreach (Animator animator in this.questionMarks)
		{
			if (animator != null)
			{
				animator.SetBool("IsOn", false);
				animator.GetComponent<Collider2D>().enabled = false;
			}
		}
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x0002160F File Offset: 0x0001F80F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dustPrefab = null;
		this.explosionPrefab = null;
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x00021625 File Offset: 0x0001F825
	public void IntroContinue()
	{
		this.StartJump();
	}

	// Token: 0x060027CF RID: 10191 RVA: 0x0002162D File Offset: 0x0001F82D
	public void StartJump()
	{
		this.state = SlimeLevelSlime.State.Jump;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x000CCB88 File Offset: 0x000CAD88
	public IEnumerator jump_cr()
	{
		LevelProperties.Slime.Jump p = this.CurrentPropertyState.jump;
		string[] pattern = p.patternString.Split(new char[]
		{
			','
		});
		int i = Random.Range(0, pattern.Length);
		int numJumpsLeft = 0;
		if (this.firstTime && this.isBig)
		{
			numJumpsLeft = p.bigSlimeInitialJumpPunchCount;
			this.firstTime = false;
		}
		else
		{
			numJumpsLeft = p.numJumps.RandomInt();
		}
		base.animator.SetTrigger("Jump");
		float delay = p.groundDelay;
		this.playerToPunch = PlayerManager.GetNext();
		for (;;)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out delay);
			}
			else
			{
				yield return base.animator.WaitForAnimationToStart(this, "Jump_Squish_Loop", false);
				if (this.isBig)
				{
					this.BigJumpAudio();
				}
				else
				{
					this.SmallJumpAudio();
				}
				yield return CupheadTime.WaitForSeconds(this, delay);
				base.animator.SetTrigger("Continue");
				yield return base.animator.WaitForAnimationToStart(this, "Up", false);
				bool goingUp = true;
				bool highJump = pattern[i][0] == 'H';
				if (pattern[i][0] == 'R')
				{
					highJump = Rand.Bool();
				}
				this.velocityY = ((!highJump) ? p.lowJumpVerticalSpeed : p.highJumpVerticalSpeed);
				float speedX = (!highJump) ? p.lowJumpHorizontalSpeed : p.highJumpHorizontalSpeed;
				this.gravity = ((!highJump) ? p.lowJumpGravity : p.highJumpGravity);
				this.inAir = true;
				SlimeLevelSlime.Direction moveDir = this.facingDirection;
				this.shadow.enabled = true;
				while (goingUp || base.transform.position.y > this.onGroundY)
				{
					this.velocityY -= this.gravity * CupheadTime.FixedDelta * this.hitPauseCoefficient();
					float velocityX = (moveDir != SlimeLevelSlime.Direction.Left) ? speedX : (-speedX);
					base.transform.AddPosition(velocityX * CupheadTime.FixedDelta * this.hitPauseCoefficient(), this.velocityY * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
					if (this.velocityY < 0f && goingUp)
					{
						goingUp = false;
						base.animator.SetTrigger("Apex");
					}
					if ((moveDir == SlimeLevelSlime.Direction.Left && base.transform.position.x < -this.maxX) || (moveDir == SlimeLevelSlime.Direction.Right && base.transform.position.x > this.maxX))
					{
						if (moveDir == SlimeLevelSlime.Direction.Left)
						{
							base.transform.SetPosition(new float?(-this.maxX), null, null);
							moveDir = SlimeLevelSlime.Direction.Right;
						}
						else
						{
							base.transform.SetPosition(new float?(this.maxX), null, null);
							moveDir = SlimeLevelSlime.Direction.Left;
						}
						if (!goingUp)
						{
							speedX = 0f;
						}
						this.Turn();
					}
					yield return new WaitForFixedUpdate();
				}
				base.transform.SetPosition(null, new float?(this.onGroundY), null);
				this.shadow.enabled = false;
				this.inAir = false;
				delay = p.groundDelay;
				float screenShakeCoefficient = (!highJump) ? 1f : 1.5f;
				screenShakeCoefficient *= ((!this.isBig) ? 1f : 2f);
				CupheadLevelCamera.Current.Shake(5f * screenShakeCoefficient, 0.2f * screenShakeCoefficient, false);
				this.dustPrefab.Create(base.transform.position);
				if (this.wantsToTransform && base.transform.position.x > -350f && base.transform.position.x < 350f)
				{
					break;
				}
				if (this.dieOnLand)
				{
					goto Block_23;
				}
				base.animator.SetTrigger("Land");
				if (this.isBig && !this.firstPunch)
				{
					this.jumpsBeforeFirstPunch--;
					if (this.jumpsBeforeFirstPunch == 0)
					{
						goto Block_26;
					}
				}
				else
				{
					numJumpsLeft--;
					if (numJumpsLeft <= 0 && this.inPunchPosition())
					{
						goto Block_28;
					}
				}
			}
			i = (i + 1) % pattern.Length;
		}
		base.animator.SetTrigger("Transform");
		yield break;
		Block_23:
		base.animator.SetTrigger("LandingDeath");
		this.state = SlimeLevelSlime.State.Dying;
		yield break;
		Block_26:
		this.firstPunch = true;
		yield return this.Punch();
		yield break;
		Block_28:
		yield return this.Punch();
		yield break;
		yield break;
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x000CCBA4 File Offset: 0x000CADA4
	public IEnumerator Punch()
	{
		if (this.playerToPunch == null || this.playerToPunch.IsDead)
		{
			this.playerToPunch = PlayerManager.GetNext();
		}
		this.punchDirection = ((this.playerToPunch.transform.position.x <= base.transform.position.x) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		if (!this.isBig)
		{
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_Squish_Loop", false, true);
		}
		this.StartPunch();
		yield break;
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x000CCBC0 File Offset: 0x000CADC0
	public void updateShadow(float jumpY)
	{
		this.shadow.transform.SetPosition(null, new float?(this.shadowY), null);
		float num = Mathf.Clamp01(jumpY / this.shadowMaxY);
		Animator component = this.shadow.GetComponent<Animator>();
		component.Play("Idle", 0, num);
		component.speed = 0f;
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x00021643 File Offset: 0x0001F843
	public void Turn()
	{
		base.animator.SetTrigger("Turn");
		base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x060027D4 RID: 10196 RVA: 0x000CCC2C File Offset: 0x000CAE2C
	public IEnumerator turn_cr()
	{
		int upTurn = Animator.StringToHash(base.animator.GetLayerName(0) + ".Up_Turn");
		int downTurn = Animator.StringToHash(base.animator.GetLayerName(0) + ".Down_Turn");
		int startSquish = Animator.StringToHash(base.animator.GetLayerName(0) + ".Jump_Squish_Start");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != upTurn && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != downTurn && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != startSquish)
		{
			yield return new WaitForEndOfFrame();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == upTurn || base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == downTurn)
		{
			yield return new WaitForEndOfFrame();
		}
		this.facingDirection = ((this.facingDirection != SlimeLevelSlime.Direction.Left) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		base.transform.SetScale(new float?((float)((this.facingDirection != SlimeLevelSlime.Direction.Right) ? 1 : -1)), null, null);
		yield break;
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x000CCC48 File Offset: 0x000CAE48
	public bool inPunchPosition()
	{
		if (this.playerToPunch == null || this.playerToPunch.IsDead)
		{
			this.playerToPunch = PlayerManager.GetNext();
		}
		return (this.playerToPunch.transform.position.x > base.transform.position.x && base.transform.position.x < this.punchMaxX && base.transform.position.x > this.punchMinX) || (this.playerToPunch.transform.position.x < base.transform.position.x && base.transform.position.x > -this.punchMaxX && base.transform.position.x < -this.punchMinX);
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x00021662 File Offset: 0x0001F862
	public void StartPunch()
	{
		this.state = SlimeLevelSlime.State.Punch;
		base.StartCoroutine(this.punch_cr());
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x000CCD64 File Offset: 0x000CAF64
	public IEnumerator punch_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		bool turn = this.punchDirection != this.facingDirection;
		base.animator.SetTrigger((!turn) ? "StartPunch" : "StartPunchTurn");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_Pre_Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.CurrentPropertyState.punch.preHold);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.CurrentPropertyState.punch.mainHold);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_End", false);
		this.BigPunchPlaying = false;
		this.StartJump();
		if (this.isBig)
		{
			base.animator.SetBool("FirstPunch", false);
		}
		yield break;
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000CCD80 File Offset: 0x000CAF80
	public void PunchTurn()
	{
		base.animator.SetTrigger("Continue");
		this.facingDirection = ((this.facingDirection != SlimeLevelSlime.Direction.Left) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		base.transform.SetScale(new float?((float)((this.facingDirection != SlimeLevelSlime.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x00021678 File Offset: 0x0001F878
	public void Transform()
	{
		this.wantsToTransform = true;
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000CCDEC File Offset: 0x000CAFEC
	public void TurnBig()
	{
		this.bigSlime.transform.position = base.transform.position;
		this.bigSlime.StartJump();
		this.bigSlime.facingDirection = this.facingDirection;
		this.bigSlime.transform.localScale = base.transform.localScale;
		foreach (Collider2D collider2D in this.bigSlime.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x00021681 File Offset: 0x0001F881
	public void OnBossDeath()
	{
		this.Die(false);
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000CCE84 File Offset: 0x000CB084
	public void DeathTransform()
	{
		if (this.state != SlimeLevelSlime.State.Dying)
		{
			base.properties.OnBossDeath -= this.OnBossDeath;
			if (this.inAir)
			{
				this.dieOnLand = true;
			}
			else
			{
				this.Die(false);
			}
		}
		base.StartCoroutine(this.transformToTombstone_cr());
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000CCEE0 File Offset: 0x000CB0E0
	public IEnumerator transformToTombstone_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Death_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, 3.5f);
		this.tombStone.StartIntro(base.transform.position.x);
		yield break;
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000CCEFC File Offset: 0x000CB0FC
	public void Die(bool earlyKnockout)
	{
		this.StopAllCoroutines();
		this.state = SlimeLevelSlime.State.Dying;
		base.animator.ResetTrigger("Continue");
		if (earlyKnockout)
		{
			base.animator.SetTrigger("EarlyKnockout");
			if (Level.Current.mode == Level.Mode.Easy)
			{
				base.properties.WinInstantly();
			}
			else
			{
				base.properties.DealDamageToNextNamedState();
			}
		}
		else if (this.inAir)
		{
			base.animator.SetTrigger("AirDeath");
			base.StartCoroutine(this.airDeath_cr());
		}
		else
		{
			base.animator.SetTrigger("GroundDeath");
		}
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x000CCFA8 File Offset: 0x000CB1A8
	public IEnumerator airDeath_cr()
	{
		this.velocityY = Mathf.Min(0f, this.velocityY);
		while (base.transform.position.y > this.onGroundY)
		{
			this.velocityY -= this.gravity * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.AddPosition(0f, this.velocityY * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.onGroundY), null);
		float screenShakeCoefficient = 2.5f;
		CupheadLevelCamera.Current.Shake(5f * screenShakeCoefficient, 0.2f * screenShakeCoefficient, false);
		this.dustPrefab.Create(base.transform.position);
		this.shadow.enabled = false;
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x0002168A File Offset: 0x0001F88A
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x000216AB File Offset: 0x0001F8AB
	public void Explode()
	{
		this.explosionPrefab.Create(base.transform.position);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x000216CF File Offset: 0x0001F8CF
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x000216E2 File Offset: 0x0001F8E2
	public void IntroAudio()
	{
		AudioManager.Play("slime_small_intro_anim");
		this.emitAudioFromObject.Add("slime_small_intro_anim");
		AudioManager.Play("slime_tiphat");
		this.emitAudioFromObject.Add("slime_tiphat");
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x00021718 File Offset: 0x0001F918
	public void BlinkAudio()
	{
		AudioManager.Play("slime_blink");
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x00021724 File Offset: 0x0001F924
	public void SmallJumpAudio()
	{
		AudioManager.Play("slime_small_jump");
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x00021730 File Offset: 0x0001F930
	public void SmallLandAudio()
	{
		AudioManager.Play("slime_small_land");
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x0002173C File Offset: 0x0001F93C
	public void SmallStretchPunchAudio()
	{
		AudioManager.Play("slime_small_stretch_punch");
		this.emitAudioFromObject.Add("slime_small_stretch_punch");
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x00021758 File Offset: 0x0001F958
	public void SmallTransformAudio()
	{
		AudioManager.Play("slime_small_transform");
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x00021764 File Offset: 0x0001F964
	public void BigJumpAudio()
	{
		AudioManager.Play("slime_big_jump");
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x00021770 File Offset: 0x0001F970
	public void BigLandAudio()
	{
		AudioManager.Play("slime_big_land");
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000CCFC4 File Offset: 0x000CB1C4
	public void BigPunchAudio()
	{
		if (!this.BigPunchPlaying)
		{
			AudioManager.Play("slime_big_punch");
			this.emitAudioFromObject.Add("slime_big_punch");
			AudioManager.Play("slime_big_punch_voice");
			this.emitAudioFromObject.Add("slime_big_punch_voice");
			this.BigPunchPlaying = true;
		}
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x0002177C File Offset: 0x0001F97C
	public void BigDeathAudio()
	{
		if (!this.deathAudioPlayed)
		{
			AudioManager.Play("slime_big_death");
			AudioManager.Play("slime_big_death_voice");
			this.deathAudioPlayed = true;
		}
	}

	// Token: 0x040020EF RID: 8431
	[SerializeField]
	public Animator[] questionMarks;

	// Token: 0x040020F1 RID: 8433
	public const float TRANSFORM_MAX_X = 350f;

	// Token: 0x040020F3 RID: 8435
	public SlimeLevelSlime.Direction facingDirection;

	// Token: 0x040020F4 RID: 8436
	public DamageDealer damageDealer;

	// Token: 0x040020F5 RID: 8437
	public DamageReceiver damageReceiver;

	// Token: 0x040020F6 RID: 8438
	public float onGroundY;

	// Token: 0x040020F7 RID: 8439
	public float shadowY;

	// Token: 0x040020F8 RID: 8440
	public bool wantsToTransform;

	// Token: 0x040020F9 RID: 8441
	public SlimeLevelSlime.Direction punchDirection;

	// Token: 0x040020FA RID: 8442
	public bool inAir;

	// Token: 0x040020FB RID: 8443
	public float velocityY;

	// Token: 0x040020FC RID: 8444
	public float gravity;

	// Token: 0x040020FD RID: 8445
	public bool dieOnLand;

	// Token: 0x040020FE RID: 8446
	public bool deathAudioPlayed;

	// Token: 0x040020FF RID: 8447
	public bool firstTime = true;

	// Token: 0x04002100 RID: 8448
	public bool firstPunch;

	// Token: 0x04002101 RID: 8449
	public bool BigPunchPlaying;

	// Token: 0x04002102 RID: 8450
	public int jumpsBeforeFirstPunch;

	// Token: 0x04002104 RID: 8452
	public AbstractPlayerController playerToPunch;

	// Token: 0x04002105 RID: 8453
	[SerializeField]
	public SpriteRenderer shadow;

	// Token: 0x04002106 RID: 8454
	[SerializeField]
	public float shadowMaxY;

	// Token: 0x04002107 RID: 8455
	[SerializeField]
	public SlimeLevelSlime bigSlime;

	// Token: 0x04002108 RID: 8456
	[SerializeField]
	public SlimeLevelTombstone tombStone;

	// Token: 0x04002109 RID: 8457
	[SerializeField]
	public Effect explosionPrefab;

	// Token: 0x0400210A RID: 8458
	[SerializeField]
	public Effect dustPrefab;

	// Token: 0x0400210B RID: 8459
	[SerializeField]
	public bool isBig;

	// Token: 0x0400210C RID: 8460
	[SerializeField]
	public float punchMaxX;

	// Token: 0x0400210D RID: 8461
	[SerializeField]
	public float punchMinX;

	// Token: 0x0400210E RID: 8462
	[SerializeField]
	public float maxX;

	// Token: 0x0400210F RID: 8463
	[SerializeField]
	public Transform eyeMaxPosition;

	// Token: 0x02000F52 RID: 3922
	public enum State
	{
		// Token: 0x04006E86 RID: 28294
		Intro,
		// Token: 0x04006E87 RID: 28295
		Jump,
		// Token: 0x04006E88 RID: 28296
		Punch,
		// Token: 0x04006E89 RID: 28297
		Dying
	}

	// Token: 0x02000F53 RID: 3923
	public enum Direction
	{
		// Token: 0x04006E8B RID: 28299
		Left,
		// Token: 0x04006E8C RID: 28300
		Right
	}
}
