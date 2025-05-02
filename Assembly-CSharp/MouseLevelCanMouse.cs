using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C5 RID: 709
public class MouseLevelCanMouse : LevelProperties.Mouse.Entity
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06001F6D RID: 8045 RVA: 0x0001A7FA File Offset: 0x000189FA
	// (set) Token: 0x06001F6E RID: 8046 RVA: 0x0001A802 File Offset: 0x00018A02
	public MouseLevelCanMouse.State state { get; set; }

	// Token: 0x06001F6F RID: 8047 RVA: 0x0001A80B File Offset: 0x00018A0B
	public override void Awake()
	{
		base.Awake();
		this.SetWheels(false);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000B5FA4 File Offset: 0x000B41A4
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.peeking && base.properties.CurrentHealth < base.properties.TotalHealth * this.catPeeking.Peek1Threshold)
		{
			this.catPeeking.StartPeeking();
			this.peeking = true;
		}
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x0001A848 File Offset: 0x00018A48
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x0001A871 File Offset: 0x00018A71
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.OnLevelStart;
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x0001A890 File Offset: 0x00018A90
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x0001A8A3 File Offset: 0x00018AA3
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x0001A8C4 File Offset: 0x00018AC4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.cherryBombPrefab = null;
		this.catapultProjectilePrefab = null;
		this.romanCandlePrefab = null;
		this.wheelSprites = null;
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x0001A8E8 File Offset: 0x00018AE8
	public void OnLevelStart()
	{
		base.StartCoroutine(this.wheels_cr());
		base.StartCoroutine(this.levelStart_cr());
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x000B600C File Offset: 0x000B420C
	public IEnumerator levelStart_cr()
	{
		base.animator.Play("Intro", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		base.animator.Play("Intro_Down", 1);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000B6028 File Offset: 0x000B4228
	public IEnumerator moveBack_cr()
	{
		this.exitAfterMoveBack = true;
		while (this.exitAfterMoveBack)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000B6044 File Offset: 0x000B4244
	public IEnumerator moveToX_cr(float x)
	{
		this.overrideMove = true;
		this.overrideMoveX = x;
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
		while (this.overrideMove)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x0001A904 File Offset: 0x00018B04
	public void SetWheels(bool b)
	{
		this.wheelRenderer.enabled = b;
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000B6068 File Offset: 0x000B4268
	public IEnumerator move_cr()
	{
		base.animator.Play("Move", 1);
		this.SetWheels(true);
		this.moving = true;
		bool movingBack = false;
		for (;;)
		{
			LevelProperties.Mouse.CanMove p = base.properties.CurrentState.canMove;
			Vector2 end = new Vector2(500f * base.transform.localScale.x, base.transform.position.y);
			bool overriden = false;
			if (this.overrideMove)
			{
				end.x = this.overrideMoveX;
				overriden = true;
			}
			else if (!movingBack)
			{
				end.x -= p.maxXPositionRange.RandomFloat() * base.transform.localScale.x;
			}
			float time = Mathf.Abs(base.transform.position.x - end.x) / p.speed;
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, end, EaseUtils.EaseType.easeInOutSine, time));
			yield return CupheadTime.WaitForSeconds(this, p.stopTime);
			if (this.overrideMove && overriden)
			{
				break;
			}
			if (movingBack && this.exitAfterMoveBack)
			{
				goto Block_6;
			}
			movingBack = !movingBack;
		}
		this.overrideMove = false;
		goto IL_270;
		Block_6:
		this.exitAfterMoveBack = false;
		IL_270:
		this.SetWheels(false);
		base.animator.Play("IdleDown", 1);
		this.moving = false;
		yield break;
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000B6084 File Offset: 0x000B4284
	public IEnumerator wheels_cr()
	{
		int currentFrame = 0;
		Vector2 lastPos = base.transform.position;
		int direction = 1;
		for (;;)
		{
			float distance = 0f;
			while (distance < 6f)
			{
				float speed = lastPos.x - base.transform.position.x;
				distance += Mathf.Abs(speed);
				if (base.transform.localScale.x > 0f)
				{
					if (speed < 0f)
					{
						direction = -1;
					}
					else
					{
						direction = 1;
					}
				}
				else if (speed < 0f)
				{
					direction = 1;
				}
				else
				{
					direction = -1;
				}
				lastPos = base.transform.position;
				yield return null;
			}
			currentFrame += direction;
			currentFrame = (int)Mathf.Repeat((float)currentFrame, (float)this.wheelSprites.Length);
			this.wheelRenderer.sprite = this.wheelSprites[currentFrame];
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x0001A912 File Offset: 0x00018B12
	public void StartDash()
	{
		this.state = MouseLevelCanMouse.State.Dash;
		base.StartCoroutine(this.dash_cr());
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x0001A928 File Offset: 0x00018B28
	public void StartDashMove()
	{
		this.dash = true;
		base.animator.SetTrigger("CanContinue");
		AudioManager.Play("level_mouse_can_dash_start");
		this.emitAudioFromObject.Add("level_mouse_can_dash_start");
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000B60A0 File Offset: 0x000B42A0
	public void DashFlipX()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		if (base.transform.localScale.x < 0f)
		{
			this.direction = MouseLevelCanMouse.Direction.Right;
			base.transform.AddPosition(40f, 0f, 0f);
		}
		else
		{
			this.direction = MouseLevelCanMouse.Direction.Left;
			base.transform.AddPosition(-40f, 0f, 0f);
		}
		base.animator.SetTrigger("CanContinue");
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x000B615C File Offset: 0x000B435C
	public IEnumerator dash_cr()
	{
		LevelProperties.Mouse.CanDash dashProperties = base.properties.CurrentState.canDash;
		for (int i = 0; i < this.springs.Length; i++)
		{
			Vector2 velocity;
			velocity..ctor(dashProperties.springVelocityX[i].RandomFloat(), dashProperties.springVelocityY[i].RandomFloat());
			if (this.direction == MouseLevelCanMouse.Direction.Right)
			{
				velocity.x *= -1f;
			}
			this.springs[i].LaunchSpring(new Vector2(base.transform.position.x, base.transform.position.y + 200f), velocity, dashProperties.springGravity);
			AudioManager.Play("level_mouse_can_springboard_shoot");
			this.emitAudioFromObject.Add("level_mouse_can_springboard_shoot");
			base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		}
		if (this.moving)
		{
			yield return base.StartCoroutine(this.moveBack_cr());
		}
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(-450f * base.transform.localScale.x, base.transform.position.y);
		base.animator.Play("Dash", 1);
		AudioManager.PlayLoop("level_mouse_can_dash_loop");
		this.dash = false;
		while (!this.dash)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.tween_cr(base.transform, start, end, EaseUtils.EaseType.easeInSine, dashProperties.time));
		base.animator.SetTrigger("CanContinue");
		AudioManager.Stop("level_mouse_can_dash_loop");
		AudioManager.Play("level_mouse_can_dash_stop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_stop");
		yield return base.animator.WaitForAnimationToEnd(this, "Dash_End", 1, false, true);
		yield return CupheadTime.WaitForSeconds(this, dashProperties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x0001A95B File Offset: 0x00018B5B
	public void StartCherryBomb()
	{
		this.state = MouseLevelCanMouse.State.CherryBomb;
		base.StartCoroutine(this.cherryBomb_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000B6178 File Offset: 0x000B4378
	public void FireCherryBomb()
	{
		base.animator.SetTrigger("Shoot");
		Vector2 velocity;
		velocity..ctor(base.properties.CurrentState.canCherryBomb.xVelocity * base.transform.localScale.x, base.properties.CurrentState.canCherryBomb.yVelocity);
		this.cherryBombPrefab.Create(this.cherryBombRoot.position, velocity, base.properties.CurrentState.canCherryBomb.gravity, (float)base.properties.CurrentState.canCherryBomb.childSpeed);
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000B622C File Offset: 0x000B442C
	public IEnumerator cherryBomb_cr()
	{
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		LevelProperties.Mouse.CanCherryBomb properties = base.properties.CurrentState.canCherryBomb;
		KeyValue[] pattern = KeyValue.ListFromString(properties.patterns[Random.Range(0, properties.patterns.Length)], new char[]
		{
			'P',
			'D'
		});
		base.animator.Play("Cannon_Start", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Cannon_Start", 0, false, true);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, properties.delay);
					this.FireCherryBomb();
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle_Down", 0, false);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x0001A996 File Offset: 0x00018B96
	public void StartCatapult()
	{
		this.state = MouseLevelCanMouse.State.Catapult;
		base.StartCoroutine(this.catapult_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000B6248 File Offset: 0x000B4448
	public void FireCatapult()
	{
		LevelProperties.Mouse.CanCatapult canCatapult = base.properties.CurrentState.canCatapult;
		char[] array = canCatapult.patterns.GetRandom<string>().ToLower().ToCharArray();
		float num = (float)((this.direction != MouseLevelCanMouse.Direction.Right) ? 165 : -45);
		if (array.Length <= 1)
		{
			this.catapultProjectilePrefab.CreateFromPrefab(this.catapultRoot.position, num + canCatapult.angleOffset, (float)canCatapult.projectileSpeed, array[0]);
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			float rotation = num + canCatapult.spreadAngle / (float)(array.Length - 1) * (float)i;
			this.catapultProjectilePrefab.CreateFromPrefab(this.catapultRoot.position, rotation, (float)canCatapult.projectileSpeed, array[i]);
		}
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x000B6320 File Offset: 0x000B4520
	public IEnumerator catapult_cr()
	{
		LevelProperties.Mouse.CanCatapult properties = base.properties.CurrentState.canCatapult;
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		base.animator.Play("Catapult_Idle", 0);
		yield return base.StartCoroutine(this.tweenCatapultY_cr(-280f, 0f, properties.timeIn, EaseUtils.EaseType.easeOutSine));
		yield return CupheadTime.WaitForSeconds(this, properties.pumpDelay);
		for (int i = 0; i < properties.count; i++)
		{
			base.animator.SetTrigger("Continue");
			this.SoundMouseCatapultGlug();
			yield return base.animator.WaitForAnimationToEnd(this, "Catapult_Pump", 0, false, true);
			yield return CupheadTime.WaitForSeconds(this, properties.pumpDelay);
			base.animator.SetTrigger("Shoot");
			yield return base.animator.WaitForAnimationToEnd(this, "Catapult_Shoot", 0, false, true);
			yield return CupheadTime.WaitForSeconds(this, properties.repeatDelay);
		}
		yield return base.StartCoroutine(this.tweenCatapultY_cr(0f, -280f, properties.timeOut, EaseUtils.EaseType.easeOutSine));
		base.animator.Play("Idle_Down", 0);
		yield return CupheadTime.WaitForSeconds(this, (float)properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000B633C File Offset: 0x000B453C
	public IEnumerator tweenCatapultY_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		this.catapult.transform.SetLocalPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.catapult.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.catapult.transform.SetLocalPosition(null, new float?(end), null);
		yield return null;
		yield break;
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x0001A9D1 File Offset: 0x00018BD1
	public void StartRomanCandle()
	{
		this.state = MouseLevelCanMouse.State.RomanCandle;
		base.StartCoroutine(this.romanCandle_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x000B6374 File Offset: 0x000B4574
	public void FireRomanCandle()
	{
		this.romanCandlePrefab.Create(this.romanCandleRoot.position, (float)((base.transform.localScale.x <= 0f) ? 0 : 180), base.properties.CurrentState.canRomanCandle.speed, base.properties.CurrentState.canRomanCandle.speed, base.properties.CurrentState.canRomanCandle.rotationSpeed, 100f, base.properties.CurrentState.canRomanCandle.timeBeforeHoming, PlayerManager.GetNext());
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000B6424 File Offset: 0x000B4624
	public IEnumerator romanCandle_cr()
	{
		LevelProperties.Mouse.CanRomanCandle properties = base.properties.CurrentState.canRomanCandle;
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		for (int i = 0; i < properties.count.RandomInt(); i++)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.repeatDelay);
			base.animator.Play("Roman_Candle", 0);
			yield return base.animator.WaitForAnimationToEnd(this, "Roman_Candle", 0, false, true);
		}
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0001AA0C File Offset: 0x00018C0C
	public void Explode(Action onStartPlatform, Action onTransitionComplete)
	{
		this.onStartPlatform = onStartPlatform;
		this.onTransitionComplete = onTransitionComplete;
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000B6440 File Offset: 0x000B4640
	public IEnumerator explode_cr()
	{
		while (this.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.sawBlades.Begin(base.properties);
		yield return base.StartCoroutine(this.moveToX_cr(0f));
		base.animator.Play("Explode", 1);
		yield break;
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x0001AA29 File Offset: 0x00018C29
	public void OnExplodedAnim()
	{
		this.onStartPlatform();
		this.SetWheels(false);
		if (this.brokenCan.state != MouseLevelBrokenCanMouse.State.Dying)
		{
			this.catPeeking.IsPhase2 = true;
		}
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x0001AA5A File Offset: 0x00018C5A
	public void SpawnBrokenCan()
	{
		this.brokenCan.StartPattern(base.transform);
		this.onTransitionComplete();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x000B645C File Offset: 0x000B465C
	public IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta * this.hitPauseCoefficient();
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x0001AA83 File Offset: 0x00018C83
	public void SoundMouseCanIntro()
	{
		AudioManager.Play("level_mouse_can_intro");
		this.emitAudioFromObject.Add("level_mouse_can_intro");
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x0001AA9F File Offset: 0x00018C9F
	public void SoundMouseCannonShoot()
	{
		AudioManager.Play("level_mouse_can_cannon_shoot");
		this.emitAudioFromObject.Add("level_mouse_can_cannon_shoot");
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x0001AABB File Offset: 0x00018CBB
	public void SoundMouseCannonEnd()
	{
		AudioManager.Play("level_mouse_can_cannon_end");
		this.emitAudioFromObject.Add("level_mouse_can_cannon_end");
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x0001AAD7 File Offset: 0x00018CD7
	public void SoundMouseCatapultShoot()
	{
		AudioManager.Play("level_mouse_can_catapult_shoot");
		this.emitAudioFromObject.Add("level_mouse_can_catapult_shoot");
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x0001AAF3 File Offset: 0x00018CF3
	public void SoundMouseCatapultGlug()
	{
		AudioManager.Play("level_mouse_can_catapult_glug");
		this.emitAudioFromObject.Add("level_mouse_can_catapult_glug");
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x0001AB0F File Offset: 0x00018D0F
	public void SoundMouseCanDashStart()
	{
		AudioManager.Play("level_mouse_can_dash_start");
		this.emitAudioFromObject.Add("level_mouse_can_dash_start");
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x0001AB2B File Offset: 0x00018D2B
	public void SoundMouseCanDashLoop()
	{
		AudioManager.PlayLoop("level_mouse_can_dash_loop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_loop");
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x0001AB47 File Offset: 0x00018D47
	public void SoundMouseCanDashStop()
	{
		AudioManager.PlayLoop("level_mouse_can_dash_stop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_stop");
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x0001AB63 File Offset: 0x00018D63
	public void SoundMouseCanDashEndAnim()
	{
		AudioManager.Play("level_mouse_can_dash_end");
		this.emitAudioFromObject.Add("level_mouse_can_dash_end");
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x0001AB7F File Offset: 0x00018D7F
	public void SoundMouseCanExplode()
	{
		AudioManager.Play("level_mouse_can_explode");
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x0001AB8B File Offset: 0x00018D8B
	public void SoundMouseCanExplodePre()
	{
		AudioManager.Play("level_mouse_can_explode_pre");
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x0001AB97 File Offset: 0x00018D97
	public void SoundMouseCanRomanCandle()
	{
		AudioManager.Play("level_mouse_can_roman_candle");
		this.emitAudioFromObject.Add("level_mouse_can_roman_candle");
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x0001ABB3 File Offset: 0x00018DB3
	public void SoundMouseChargeVoice()
	{
		AudioManager.Play("level_mouse_charge_voice");
		this.emitAudioFromObject.Add("level_mouse_charge_voice");
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000B649C File Offset: 0x000B469C
	public IEnumerator timedAudioMouseSnarky_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("level_mouse_snarky_voice");
		yield break;
	}

	// Token: 0x040019A2 RID: 6562
	public const int MOUSE_LAYER = 0;

	// Token: 0x040019A3 RID: 6563
	public const int CAN_LAYER = 1;

	// Token: 0x040019A4 RID: 6564
	public const float MOVE_START_X = 500f;

	// Token: 0x040019A5 RID: 6565
	public const float DASH_END_X = 450f;

	// Token: 0x040019A6 RID: 6566
	[Header("Cannon")]
	[SerializeField]
	public Transform cherryBombRoot;

	// Token: 0x040019A7 RID: 6567
	[SerializeField]
	public MouseLevelCherryBombProjectile cherryBombPrefab;

	// Token: 0x040019A8 RID: 6568
	[Header("Catapult")]
	[SerializeField]
	public Transform catapult;

	// Token: 0x040019A9 RID: 6569
	[SerializeField]
	public MouseLevelCanCatapultProjectile catapultProjectilePrefab;

	// Token: 0x040019AA RID: 6570
	[SerializeField]
	public Transform catapultRoot;

	// Token: 0x040019AB RID: 6571
	[Header("Roman Candle")]
	[SerializeField]
	public Transform romanCandleRoot;

	// Token: 0x040019AC RID: 6572
	[SerializeField]
	public MouseLevelRomanCandleProjectile romanCandlePrefab;

	// Token: 0x040019AD RID: 6573
	[Header("Wheels")]
	[SerializeField]
	public SpriteRenderer wheelRenderer;

	// Token: 0x040019AE RID: 6574
	[SerializeField]
	public Sprite[] wheelSprites;

	// Token: 0x040019AF RID: 6575
	[SerializeField]
	public MouseLevelBrokenCanMouse brokenCan;

	// Token: 0x040019B0 RID: 6576
	[SerializeField]
	public MouseLevelSawBladeManager sawBlades;

	// Token: 0x040019B1 RID: 6577
	[SerializeField]
	public MouseLevelCatPeeking catPeeking;

	// Token: 0x040019B2 RID: 6578
	[Header("Springs")]
	[SerializeField]
	public MouseLevelSpring[] springs;

	// Token: 0x040019B4 RID: 6580
	public MouseLevelCanMouse.Direction direction;

	// Token: 0x040019B5 RID: 6581
	public DamageReceiver damageReceiver;

	// Token: 0x040019B6 RID: 6582
	public DamageDealer damageDealer;

	// Token: 0x040019B7 RID: 6583
	public bool moving;

	// Token: 0x040019B8 RID: 6584
	public bool peeking;

	// Token: 0x040019B9 RID: 6585
	public bool overrideMove;

	// Token: 0x040019BA RID: 6586
	public bool exitAfterMoveBack;

	// Token: 0x040019BB RID: 6587
	public float overrideMoveX;

	// Token: 0x040019BC RID: 6588
	public bool dash;

	// Token: 0x040019BD RID: 6589
	public const float FLIP_OFFSET = 40f;

	// Token: 0x040019BE RID: 6590
	public Action onStartPlatform;

	// Token: 0x040019BF RID: 6591
	public Action onTransitionComplete;

	// Token: 0x02000DA9 RID: 3497
	public enum State
	{
		// Token: 0x040062CB RID: 25291
		Intro,
		// Token: 0x040062CC RID: 25292
		Idle,
		// Token: 0x040062CD RID: 25293
		Dash,
		// Token: 0x040062CE RID: 25294
		CherryBomb,
		// Token: 0x040062CF RID: 25295
		Catapult,
		// Token: 0x040062D0 RID: 25296
		RomanCandle
	}

	// Token: 0x02000DAA RID: 3498
	public enum Direction
	{
		// Token: 0x040062D2 RID: 25298
		Left,
		// Token: 0x040062D3 RID: 25299
		Right
	}
}
