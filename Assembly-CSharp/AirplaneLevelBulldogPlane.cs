using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class AirplaneLevelBulldogPlane : LevelProperties.Airplane.Entity
{
	// Token: 0x06000DCC RID: 3532 RVA: 0x00088058 File Offset: 0x00086258
	public void Start()
	{
		this.state = AirplaneLevelBulldogPlane.State.Intro;
		this.startPosY = 256f;
		this.baseX = base.transform.position.x;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.bullDogPlane.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.bulldogParachute.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.bulldogCatAttack.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.bulldogParachute.gameObject.SetActive(false);
		this.bulldogCatAttack.gameObject.SetActive(false);
		base.StartCoroutine(this.idle_timer_cr());
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0000BC9F File Offset: 0x00009E9F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x0000BCB5 File Offset: 0x00009EB5
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00088138 File Offset: 0x00086338
	public void FixedUpdate()
	{
		if (this.state == AirplaneLevelBulldogPlane.State.Intro)
		{
			return;
		}
		this.moveTime += Time.fixedDeltaTime;
		Vector2 vector = base.transform.position;
		vector.y = Mathf.Sin(this.moveTime / 0.3f) * 4f;
		if (this.bounceXTimer > 0f)
		{
			this.bounceXTimer -= CupheadTime.FixedDelta * 3f * ((this.bounceXTimer <= 0.5f) ? 0.25f : 1f);
			this.bounceX = ((this.bounceXTimer <= 0.5f) ? (this.bounceX = EaseUtils.EaseInOutSine(30f, 0f, 1f - this.bounceXTimer * 2f)) : (Mathf.Sin(this.bounceXTimer * 3.14159274f) * 30f));
			this.bounceX *= this.bounceXDir;
		}
		else
		{
			this.bounceXTimer = 0f;
			this.bounceX = 0f;
		}
		if (this.bounceYTimer > 0f)
		{
			this.bounceYTimer -= CupheadTime.FixedDelta * ((!this.exitBounce) ? 1.7f : 1.6f);
			this.bounceY = ((this.bounceYTimer <= 0.5f) ? (this.bounceY = EaseUtils.EaseInOutSine((!this.exitBounce) ? 60f : 40f, 0f, 1f - this.bounceYTimer * 2f)) : (Mathf.Sin(this.bounceYTimer * 3.14159274f) * ((!this.exitBounce) ? 60f : 40f)));
		}
		else
		{
			this.bounceYTimer = 0f;
			this.bounceY = 0f;
		}
		base.transform.SetPosition(new float?(this.baseX + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX + this.bounceX), new float?(this.startPosY + vector.y - this.bounceY + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY), null);
		this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
		if (!this.isDead)
		{
			this.smokePuffLTimer -= CupheadTime.FixedDelta;
			this.smokePuffRTimer -= CupheadTime.FixedDelta;
			if (this.smokePuffLTimer <= 0f)
			{
				this.smokePuff[this.smokePuffLCounter % 3].Play((this.smokePuffLCounter % 4).ToString(), 0, 0f);
				this.smokePuff[this.smokePuffLCounter % 3].transform.localPosition = Vector3.left * 300f + Vector3.up * 50f;
				this.smokePuffLTimer += 0.25f;
				this.smokePuffLCounter++;
			}
			if (this.smokePuffRTimer <= 0f)
			{
				this.smokePuff[this.smokePuffRCounter % 3 + 3].Play((this.smokePuffRCounter % 4).ToString(), 0, 0f);
				this.smokePuff[this.smokePuffRCounter % 3 + 3].transform.localPosition = Vector3.right * 300f + Vector3.up * 50f;
				this.smokePuffRTimer += 0.27f;
				this.smokePuffRCounter++;
			}
		}
		for (int i = 0; i < this.smokePuff.Length; i++)
		{
			this.smokePuff[i].transform.localPosition += new Vector3((float)((i >= 3) ? -3 : 3), 2f - 4f * this.smokePuff[i].GetCurrentAnimatorStateInfo(0).normalizedTime) * CupheadTime.FixedDelta * 100f;
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x000885C4 File Offset: 0x000867C4
	public IEnumerator rotate_cr()
	{
		float t = 0f;
		float time = 4f;
		float maxAngle = 1f;
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -maxAngle));
		for (;;)
		{
			while (t < time)
			{
				t += CupheadTime.Delta;
				base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Lerp(-maxAngle, maxAngle, EaseUtils.EaseInOutSine(0f, 1f, t / time))));
				yield return null;
			}
			t = 0f;
			maxAngle = -maxAngle;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0000BCCD File Offset: 0x00009ECD
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dontDamage)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0000BCEC File Offset: 0x00009EEC
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.sideString = new PatternString(properties.CurrentState.parachute.sideString, true);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x000885E0 File Offset: 0x000867E0
	public IEnumerator intro_cr()
	{
		this.leaderIntroBG.SetTrigger("Continue");
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Intro", false);
		int target = Animator.StringToHash(this.bullDogPlane.GetLayerName(0) + ".Intro");
		while (this.bullDogPlane.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			float s = this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime;
			if (s > 0.7f && s < 0.95f)
			{
				((AirplaneLevel)Level.Current).UpdateShadow(1f - Mathf.Sin(Mathf.InverseLerp(0.7f, 0.95f, s) * 3.14159274f) * 0.2f);
			}
			else
			{
				((AirplaneLevel)Level.Current).UpdateShadow(1f);
			}
			yield return wait;
		}
		((AirplaneLevel)Level.Current).UpdateShadow(1f);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		this.SFX_DOGFIGHT_BulldogPlane_Loop();
		this.SFX_DOGFIGHT_Intro_BulldogPlaneDecend();
		base.StartCoroutine(this.turret_cr());
		base.StartCoroutine(this.mainattack_cr());
		float t = 0f;
		float time = 0.8f;
		float endTime = 0.4f;
		float start = base.transform.position.y;
		base.StartCoroutine(this.scale_in_cr());
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 156f, val)), null);
			yield return wait;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < endTime)
		{
			t += CupheadTime.FixedDelta;
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / endTime);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 256f, val2)), null);
			yield return wait;
		}
		base.transform.SetPosition(null, new float?(256f), null);
		if (this.state == AirplaneLevelBulldogPlane.State.Intro)
		{
			this.state = AirplaneLevelBulldogPlane.State.Main;
		}
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x000885FC File Offset: 0x000867FC
	public IEnumerator scale_in_cr()
	{
		base.transform.localScale = new Vector3(0.8f, 0.8f);
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float frameTime = 0f;
		while (t < 1.2f)
		{
			while (frameTime < 0.0416666679f)
			{
				frameTime += CupheadTime.FixedDelta;
				yield return wait;
			}
			t += frameTime;
			frameTime -= 0.0416666679f;
			base.transform.localScale = Vector3.Lerp(new Vector3(0.8f, 0.8f), new Vector3(1f, 1f), EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, 1.2f, t)));
		}
		base.transform.localScale = new Vector3(1f, 1f);
		yield break;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0000BD1E File Offset: 0x00009F1E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x00088618 File Offset: 0x00086818
	public IEnumerator mainattack_cr()
	{
		LevelProperties.Airplane.Main p = base.properties.CurrentState.main;
		PatternString attackType = new PatternString(p.attackType, true);
		for (;;)
		{
			if (this.firstAttack)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.6f);
				yield return CupheadTime.WaitForSeconds(this, p.firstAttackDelay);
				this.firstAttack = false;
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, p.attackDelayRange.RandomFloat());
			}
			char c = attackType.PopLetter();
			if (c != 'P')
			{
				if (c == 'T')
				{
					yield return base.StartCoroutine(this.catattack_cr());
				}
			}
			else
			{
				yield return base.StartCoroutine(this.parachute_cr());
			}
			this.state = AirplaneLevelBulldogPlane.State.Main;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x00088634 File Offset: 0x00086834
	public IEnumerator idle_timer_cr()
	{
		bool pickSide = Rand.Bool();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(2f, 3f));
			if (this.state == AirplaneLevelBulldogPlane.State.Main)
			{
				pickSide = ((!Rand.Bool()) ? (base.transform.position.x > this.canteenPlane.transform.position.x) : Rand.Bool());
				string side = (!pickSide) ? "Right" : "Left";
				this.bullDogPlane.SetTrigger("OnIdle" + side);
				this.bullDogPlane.SetInteger("IdleLoopCount", (pickSide != base.transform.position.x > this.canteenPlane.transform.position.x) ? 2 : 0);
				yield return this.bullDogPlane.WaitForAnimationToStart(this, "Idle", false);
			}
			while (this.state != AirplaneLevelBulldogPlane.State.Main)
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x00088650 File Offset: 0x00086850
	public IEnumerator move_cr()
	{
		this.movingRight = Rand.Bool();
		float t = 0f;
		float time = base.properties.CurrentState.main.moveTime;
		float start = 0f;
		float end = 0f;
		float speedModifier = 1f;
		for (;;)
		{
			t = 0f;
			start = base.transform.position.x;
			end = ((!this.movingRight) ? -245f : 245f);
			while (t < time)
			{
				t += CupheadTime.FixedDelta * speedModifier;
				speedModifier = Mathf.Clamp(speedModifier + ((this.state != AirplaneLevelBulldogPlane.State.Main) ? -0.01f : 0.01f), 0f, 1f);
				float val = t / time;
				this.baseX = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
				yield return new WaitForFixedUpdate();
			}
			base.transform.SetPosition(new float?(end), null, null);
			this.movingRight = !this.movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x0008866C File Offset: 0x0008686C
	public IEnumerator turret_cr()
	{
		LevelProperties.Airplane.Turrets p = base.properties.CurrentState.turrets;
		PatternString positionString = new PatternString(p.positionString, true, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.attackDelayRange.RandomFloat());
			this.turretSpawnPoints[positionString.PopInt()].StartAttack(p.velocityX, p.velocityY, p.gravity);
		}
		yield break;
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0000BD2C File Offset: 0x00009F2C
	public void StartRocket()
	{
		base.StartCoroutine(this.rocket_cr());
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x00088688 File Offset: 0x00086888
	public IEnumerator rocket_cr()
	{
		LevelProperties.Airplane.Rocket p = base.properties.CurrentState.rocket;
		PatternString delayString = new PatternString(p.attackDelayString, true, true);
		PatternString dirString = new PatternString(p.attackOrderString, true, true);
		this.hydrantAttackBG.Play("Fly");
		yield return this.hydrantAttackBG.WaitForAnimationToEnd(this, "Fly", false, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, (float)delayString.PopInt());
			Vector3 position = (dirString.PopLetter() != 'R') ? this.rocketSpawnLeft.position : this.rocketSpawnRight.position;
			this.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime);
		}
		yield break;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x000886A4 File Offset: 0x000868A4
	public IEnumerator parachute_cr()
	{
		bool onLeft = this.sideString.PopLetter() == 'L';
		this.bullDogPlane.SetInteger("IdleLoopCount", 3);
		this.bullDogPlane.SetBool("InParachuteATK", true);
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Parachute_Start", false);
		this.state = AirplaneLevelBulldogPlane.State.Parachute;
		yield return CupheadTime.WaitForSeconds(this, 0.6666667f);
		this.exitBounce = true;
		this.bounceYTimer = 1f;
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_Start", false, false);
		this.SFX_DOGFIGHT_Bulldog_ParachuteDown();
		float posX = (!onLeft) ? 575f : -575f;
		Vector3 pos = new Vector3(posX, 0f);
		float scale = (float)((!onLeft) ? -1 : 1);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		this.bulldogParachute.gameObject.SetActive(true);
		this.bulldogParachute.StartDescent(pos, scale);
		while (this.bulldogParachute.isMoving)
		{
			yield return null;
		}
		this.bulldogParachute.gameObject.SetActive(false);
		this.bullDogPlane.SetBool("InParachuteATK", false);
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.exitBounce = false;
		this.bounceYTimer = 1f;
		this.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop();
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_End", false, false);
		this.bullDogPlane.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x000886C0 File Offset: 0x000868C0
	public IEnumerator catattack_cr()
	{
		LevelProperties.Airplane.Triple p = base.properties.CurrentState.triple;
		bool onLeft = this.sideString.PopLetter() == 'L';
		this.bullDogPlane.SetInteger("IdleLoopCount", 3);
		this.bullDogPlane.SetBool("InParachuteATK", true);
		this.bullDogPlane.SetBool("OnLeft", onLeft);
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Parachute_Start", false);
		this.state = AirplaneLevelBulldogPlane.State.CatAttack;
		yield return CupheadTime.WaitForSeconds(this, 0.6666667f);
		this.exitBounce = true;
		this.bounceYTimer = 1f;
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_Start", false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		float posX = (!onLeft) ? 600f : -600f;
		Vector3 startPos = new Vector3(posX, p.yHeight);
		this.bulldogCatAttack.gameObject.SetActive(true);
		yield return null;
		this.bulldogCatAttack.StartCat(startPos);
		while (this.bulldogCatAttack.isAttacking)
		{
			yield return null;
		}
		this.bulldogCatAttack.gameObject.SetActive(false);
		this.bullDogPlane.SetBool("InParachuteATK", false);
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.exitBounce = false;
		this.bounceYTimer = 1f;
		this.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop();
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_End", false, false);
		this.bullDogPlane.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000886DC File Offset: 0x000868DC
	public void OnStageChange()
	{
		this.endPhaseOne = true;
		this.dontDamage = true;
		if (this.bulldogCatAttack.isAttacking)
		{
			this.bulldogCatAttack.EarlyExit();
		}
		if (this.bulldogParachute.isMoving)
		{
			this.bulldogParachute.EarlyExit();
		}
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0000BD3B File Offset: 0x00009F3B
	public void BulldogDeath()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00088730 File Offset: 0x00086930
	public IEnumerator death_cr()
	{
		this.bullDogPlane.SetBool("Dead", true);
		this.bullDogPlane.SetBool("InTripleATK", false);
		if (!this.bulldogParachute.gameObject.activeInHierarchy)
		{
			this.bullDogPlane.SetBool("InParachuteATK", false);
		}
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Death", false);
		this.startPhaseTwo = true;
		this.SFX_DOGFIGHT_BulldogPlane_StopLoop();
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		this.bullDogPlane.SetLayerWeight(1, 0f);
		this.bullDogPlane.SetLayerWeight(2, 0f);
		this.bullDogPlane.SetLayerWeight(3, 0f);
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Death", false, true);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0000BD57 File Offset: 0x00009F57
	public void SFX_DOGFIGHT_Intro_BulldogPlaneDecend()
	{
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introdecend");
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0000BD63 File Offset: 0x00009F63
	public void SFX_DOGFIGHT_BulldogPlane_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_bulldogplane_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_bulldogplane_loop", 0.25f, 3f);
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0000BD83 File Offset: 0x00009F83
	public void SFX_DOGFIGHT_BulldogPlane_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_bulldogplane_loop");
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x0000BD8F File Offset: 0x00009F8F
	public void SFX_DOGFIGHT_Bulldog_ParachuteDown()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_parachutedown");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_parachutedown");
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_ParachuteFlump");
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0000BDB5 File Offset: 0x00009FB5
	public void SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_p1_bulldog_parachutedown");
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0008874C File Offset: 0x0008694C
	public void WORKAROUND_NullifyFields()
	{
		this.leaderIntroBG = null;
		this.hydrantAttackBG = null;
		this.turretSpawnPoints = null;
		this.rocketSpawnLeft = null;
		this.rocketSpawnRight = null;
		this.rocketPrefab = null;
		this.bullDogPlane = null;
		this.bulldogParachute = null;
		this.bulldogCatAttack = null;
		this.canteenPlane = null;
		this.damageDealer = null;
		this.sideString = null;
		this.smokePuff = null;
	}

	// Token: 0x04000ACC RID: 2764
	public const float MOVE_POS_X = 245f;

	// Token: 0x04000ACD RID: 2765
	public const float PARACHUTE_POS_X = 575f;

	// Token: 0x04000ACE RID: 2766
	public const float PARACHUTE_APPEAR_DELAY = 0.35f;

	// Token: 0x04000ACF RID: 2767
	public const float PARACHUTE_EXIT_BOUNCE_HEIGHT = 40f;

	// Token: 0x04000AD0 RID: 2768
	public const float PARACHUTE_EXIT_BOUNCE_SPEED = 1.6f;

	// Token: 0x04000AD1 RID: 2769
	public const float PARACHUTE_EXIT_BOUNCE_FRAME_DELAY = 16f;

	// Token: 0x04000AD2 RID: 2770
	public const float PARACHUTE_RETURN_BOUNCE_HEIGHT = 60f;

	// Token: 0x04000AD3 RID: 2771
	public const float PARACHUTE_RETURN_BOUNCE_SPEED = 1.7f;

	// Token: 0x04000AD4 RID: 2772
	public const float PARACHUTE_RETURN_BOUNCE_FRAME_DELAY = 3f;

	// Token: 0x04000AD5 RID: 2773
	public const float BUMP_RETURN_BOUNCE_HEIGHT = 30f;

	// Token: 0x04000AD6 RID: 2774
	public const float BUMP_RETURN_BOUNCE_SPEED = 3f;

	// Token: 0x04000AD7 RID: 2775
	public const float CAT_ATTACK_POS_X = 600f;

	// Token: 0x04000AD8 RID: 2776
	public const float CAT_ATTACK_APPEAR_DELAY = 0.35f;

	// Token: 0x04000AD9 RID: 2777
	public const float MOVE_DOWN_POS = 256f;

	// Token: 0x04000ADA RID: 2778
	public const float MOVE_TIME = 0.3f;

	// Token: 0x04000ADB RID: 2779
	public const float MOVE_LENGTH = 4f;

	// Token: 0x04000ADC RID: 2780
	public AirplaneLevelBulldogPlane.State state;

	// Token: 0x04000ADD RID: 2781
	[SerializeField]
	public Animator leaderIntroBG;

	// Token: 0x04000ADE RID: 2782
	[SerializeField]
	public Animator hydrantAttackBG;

	// Token: 0x04000ADF RID: 2783
	[Header("Roots")]
	[SerializeField]
	public AirplaneLevelTurretDog[] turretSpawnPoints;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	public Transform rocketSpawnLeft;

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	public Transform rocketSpawnRight;

	// Token: 0x04000AE2 RID: 2786
	[Header("Prefabs")]
	[SerializeField]
	public AirplaneLevelRocket rocketPrefab;

	// Token: 0x04000AE3 RID: 2787
	[Header("Bulldog")]
	[SerializeField]
	public Animator bullDogPlane;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	public AirplaneLevelBulldogParachute bulldogParachute;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	public AirplaneLevelBulldogCatAttack bulldogCatAttack;

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	public GameObject canteenPlane;

	// Token: 0x04000AE7 RID: 2791
	public DamageDealer damageDealer;

	// Token: 0x04000AE8 RID: 2792
	public DamageReceiver damageReceiver;

	// Token: 0x04000AE9 RID: 2793
	public float moveTime;

	// Token: 0x04000AEA RID: 2794
	public float startPosY;

	// Token: 0x04000AEB RID: 2795
	public float bounceY;

	// Token: 0x04000AEC RID: 2796
	public float bounceYTimer;

	// Token: 0x04000AED RID: 2797
	public float bounceX;

	// Token: 0x04000AEE RID: 2798
	public float bounceXTimer;

	// Token: 0x04000AEF RID: 2799
	public float bounceXDir = 1f;

	// Token: 0x04000AF0 RID: 2800
	public bool exitBounce;

	// Token: 0x04000AF1 RID: 2801
	public float baseX;

	// Token: 0x04000AF2 RID: 2802
	public float wobbleTimer;

	// Token: 0x04000AF3 RID: 2803
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x04000AF4 RID: 2804
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x04000AF5 RID: 2805
	[SerializeField]
	public float wobbleSpeed = 1f;

	// Token: 0x04000AF6 RID: 2806
	public bool movingRight;

	// Token: 0x04000AF7 RID: 2807
	public bool dontDamage;

	// Token: 0x04000AF8 RID: 2808
	public bool endPhaseOne;

	// Token: 0x04000AF9 RID: 2809
	public bool firstAttack = true;

	// Token: 0x04000AFA RID: 2810
	public PatternString sideString;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	public Animator[] smokePuff;

	// Token: 0x04000AFC RID: 2812
	public int smokePuffLCounter;

	// Token: 0x04000AFD RID: 2813
	public int smokePuffRCounter = 2;

	// Token: 0x04000AFE RID: 2814
	public float smokePuffLTimer;

	// Token: 0x04000AFF RID: 2815
	public float smokePuffRTimer;

	// Token: 0x04000B00 RID: 2816
	public bool isDead;

	// Token: 0x04000B01 RID: 2817
	public bool startPhaseTwo;

	// Token: 0x020009B0 RID: 2480
	public enum State
	{
		// Token: 0x04004810 RID: 18448
		Intro,
		// Token: 0x04004811 RID: 18449
		Main,
		// Token: 0x04004812 RID: 18450
		Parachute,
		// Token: 0x04004813 RID: 18451
		TripleAttack,
		// Token: 0x04004814 RID: 18452
		CatAttack
	}
}
