using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class SnowCultLevelYeti : LevelProperties.SnowCult.Entity
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x060028F7 RID: 10487 RVA: 0x000D0198 File Offset: 0x000CE398
	// (remove) Token: 0x060028F8 RID: 10488 RVA: 0x000D01D0 File Offset: 0x000CE3D0
	public event Action OnDeathEvent;

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060028F9 RID: 10489 RVA: 0x000227CF File Offset: 0x000209CF
	// (set) Token: 0x060028FA RID: 10490 RVA: 0x000227D7 File Offset: 0x000209D7
	public bool inBallForm { get; set; }

	// Token: 0x060028FB RID: 10491 RVA: 0x000D0208 File Offset: 0x000CE408
	public override void Awake()
	{
		base.Awake();
		this.xScale = 1f;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.ballDamageReceiver = this.ball.GetComponent<DamageReceiver>();
		this.ballDamageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.idleAnimFullPathHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		if (Level.Current.mode != Level.Mode.Easy)
		{
			this.InitBats();
		}
		else
		{
			base.properties.OnBossDeath += this.OnBossDeath;
		}
		this.ball.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000227E0 File Offset: 0x000209E0
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x000D02EC File Offset: 0x000CE4EC
	public bool InIdleAnim()
	{
		return base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleAnimFullPathHash;
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x000D0318 File Offset: 0x000CE518
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (Level.Current.mode == Level.Mode.Easy && base.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti && !this.InIdleAnim() && info.damage >= base.properties.CurrentHealth)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000227F8 File Offset: 0x000209F8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000D0380 File Offset: 0x000CE580
	public override void LevelInit(LevelProperties.SnowCult properties)
	{
		base.LevelInit(properties);
		this.offsetCoordIndex = Random.Range(0, properties.CurrentState.icePillar.offsetCoordString.Split(new char[]
		{
			','
		}).Length);
		this.snowballMainIndex = Random.Range(0, properties.CurrentState.snowball.snowballTypeString.Length);
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000D03E0 File Offset: 0x000CE5E0
	public void StartOnLeft(Vector3 reflectionPoint)
	{
		this.yetiSpawnPoint.position = new Vector3(reflectionPoint.x + (reflectionPoint.x - this.yetiSpawnPoint.position.x), this.yetiSpawnPoint.position.y);
		base.transform.localScale = new Vector3(-1f, 1f);
		this.xScale = base.transform.localScale.x;
		this.onLeft = true;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x00022816 File Offset: 0x00020A16
	public void StartYeti()
	{
		base.StartCoroutine(this.intro_cr());
		if (Level.Current.mode != Level.Mode.Easy)
		{
			base.StartCoroutine(this.bats_attack_cr());
		}
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x00022841 File Offset: 0x00020A41
	public void SetState(SnowCultLevelYeti.States s)
	{
		this.previousState = this.state;
		this.state = s;
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x000D0470 File Offset: 0x000CE670
	public IEnumerator intro_cr()
	{
		this.state = SnowCultLevelYeti.States.Intro;
		this.introRibcageClosed = false;
		base.transform.position = Vector3.zero;
		base.transform.position = this.yetiSpawnPoint.position;
		base.transform.position += Vector3.up * 300f;
		float t = 0f;
		this.sprite = base.GetComponent<SpriteRenderer>();
		base.animator.Play("Intro", 0, 0f);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".Intro"))
		{
			if (t < 0.34f)
			{
				base.transform.position = new Vector3(base.transform.position.x, EaseUtils.EaseOutBack(this.yetiSpawnPoint.position.y + 300f, this.yetiSpawnPoint.position.y, t * 3f));
				t += CupheadTime.FixedDelta;
			}
			this.introShadow.transform.position = new Vector3(base.transform.position.x, -25f);
			yield return wait;
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		base.StartCoroutine(this.do_patterns_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x00022856 File Offset: 0x00020A56
	public void ShakeScreenInIntro()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		((SnowCultLevel)Level.Current).YetiHitGround();
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x0002287C File Offset: 0x00020A7C
	public void RibcageClosedAroundWizard()
	{
		this.introRibcageClosed = true;
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000D048C File Offset: 0x000CE68C
	public void FlipSprite()
	{
		base.transform.SetScale(new float?((!this.onLeft) ? this.xScale : (-this.xScale)), null, null);
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x000D04D8 File Offset: 0x000CE6D8
	public IEnumerator do_patterns_cr()
	{
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		this.patternString = p.yetiPatternString.Split(new char[]
		{
			','
		});
		this.patternStringIndex = Random.Range(0, this.patternString.Length);
		while (!this.forceOutroToStart)
		{
			string text = this.patternString[this.patternStringIndex];
			if (text == null)
			{
				goto IL_144;
			}
			if (!(text == "S"))
			{
				if (!(text == "J"))
				{
					if (!(text == "L"))
					{
						if (!(text == "P"))
						{
							goto IL_144;
						}
						this.StartIcePillar();
					}
					else
					{
						this.Snowball();
					}
				}
				else
				{
					base.StartCoroutine(this.start_jump_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.start_dash_cr());
			}
			IL_154:
			while (this.state != SnowCultLevelYeti.States.Idle)
			{
				yield return null;
			}
			this.patternStringIndex = (this.patternStringIndex + 1) % this.patternString.Length;
			yield return null;
			continue;
			IL_144:
			this.Snowball();
			goto IL_154;
		}
		yield break;
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x000D04F4 File Offset: 0x000CE6F4
	public string PeekNextPattern()
	{
		if (this.forceOutroToStart || Level.Current.mode == Level.Mode.Easy)
		{
			return "I";
		}
		string text = this.patternString[(this.patternStringIndex + 1) % this.patternString.Length];
		if (text != null)
		{
			if (text == "S" || text == "J")
			{
				return this.patternString[(this.patternStringIndex + 1) % this.patternString.Length];
			}
			if (text == "L" || text == "P")
			{
				return "I";
			}
		}
		return null;
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000D05A8 File Offset: 0x000CE7A8
	public IEnumerator cue_reform_effect_cr(float delayTime, float position, string clipName)
	{
		yield return CupheadTime.WaitForSeconds(this, delayTime - 0.9583333f);
		this.meltFXAnimator[1].gameObject.transform.SetPosition(new float?(position), null, null);
		this.meltFXAnimator[1].gameObject.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f);
		this.meltFXAnimator[1].gameObject.SetActive(true);
		this.meltFXAnimator[1].Play(clipName);
		yield break;
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000D05D8 File Offset: 0x000CE7D8
	public IEnumerator start_dash_cr()
	{
		this.inBallForm = true;
		float PRE_DASH_TIME = 0.25f;
		float DASH_TIME = 0.375f;
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		float start = (!this.onLeft) ? 493f : -493f;
		float end = (!this.onLeft) ? -493f : 493f;
		start += this.ball.transform.localPosition.x * -base.transform.localScale.x * 2f;
		float t = 0f;
		float time = p.slideTime;
		if (this.previousState != SnowCultLevelYeti.States.Move || Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("IdleToDash");
		}
		this.SetState(SnowCultLevelYeti.States.Move);
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return base.animator.WaitForAnimationToStart(this, "PreDash", false);
		base.StartCoroutine(this.cue_reform_effect_cr(PRE_DASH_TIME + p.slideWarning + DASH_TIME + time, end, "DashReformEffect"));
		yield return base.animator.WaitForAnimationToEnd(this, "PreDash", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.slideWarning);
		base.animator.Play("Dash");
		yield return base.animator.WaitForAnimationToEnd(this, "Dash", false, true);
		this.meltFXAnimator[0].transform.position = base.transform.position;
		this.meltFXAnimator[0].transform.localScale = base.transform.localScale;
		this.meltFXAnimator[0].gameObject.SetActive(true);
		this.meltFXAnimator[0].Play("DashMeltEffect");
		this.meltFXAnimator[0].transform.parent = null;
		yield return null;
		base.transform.SetPosition(new float?(start), null, null);
		this.ball.transform.localPosition = this.BALL_DASH_OFFSET;
		this.ball.SetActive(true);
		this.dashGroundFX.SetActive(true);
		this.groundMask.SetActive(true);
		this.sprite.enabled = false;
		this.coll.enabled = false;
		base.animator.Play("DashBall", 1, 0f);
		while (t < time)
		{
			if (t < time - 0.9583333f && t + CupheadTime.FixedDelta >= time - 0.9583333f)
			{
				this.meltFXAnimator[1].gameObject.transform.SetPosition(new float?(end), null, null);
				this.meltFXAnimator[1].gameObject.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f);
				this.meltFXAnimator[1].gameObject.SetActive(true);
				this.meltFXAnimator[1].Play("DashReformEffect");
			}
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
			yield return wait;
		}
		this.onLeft = !this.onLeft;
		this.FlipSprite();
		this.sprite.enabled = true;
		this.coll.enabled = true;
		this.ball.SetActive(false);
		this.dashGroundFX.SetActive(false);
		this.groundMask.SetActive(false);
		this.meltFXAnimator[1].gameObject.SetActive(false);
		string text = this.PeekNextPattern();
		if (text != null)
		{
			if (!(text == "I"))
			{
				if (!(text == "S"))
				{
					if (text == "J")
					{
						base.animator.Play("DashToJump");
						yield return base.animator.WaitForAnimationToEnd(this, "DashToJump", false, true);
					}
				}
				else
				{
					base.animator.Play("DashToDash");
					yield return base.animator.WaitForAnimationToEnd(this, "DashToDash", false, true);
				}
			}
			else
			{
				base.animator.Play("DashToIdle");
				yield return base.animator.WaitForAnimationToEnd(this, "DashToIdle", false, true);
			}
		}
		if (this.PeekNextPattern() == "I")
		{
			yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		this.inBallForm = false;
		yield break;
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000D05F4 File Offset: 0x000CE7F4
	public IEnumerator start_jump_cr()
	{
		this.inBallForm = true;
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		float PRE_JUMP_TIME = 0.208333328f;
		float JUMP_TIME = 0.25f;
		float endArcPosX = (!this.onLeft) ? -393f : 393f;
		float reformPosX = (!this.onLeft) ? -493f : 493f;
		float xDistance = endArcPosX - base.transform.position.x;
		float ground = base.transform.position.y;
		float timeToApex = p.jumpApexTime;
		float height = p.jumpApexHeight;
		float apexTime2 = timeToApex * timeToApex;
		float g = -2f * height / apexTime2;
		float viY = 2f * height / timeToApex;
		float viX2 = viY * viY;
		float sqrtRooted = viX2 + 2f * g * ground;
		float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd3 = Mathf.Max(tEnd, tEnd2);
		float velocityX = xDistance / tEnd3;
		Vector3 speed = new Vector3(velocityX, viY);
		float t = 0f;
		if (this.previousState != SnowCultLevelYeti.States.Move || Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("IdleToJump");
		}
		this.SetState(SnowCultLevelYeti.States.Move);
		yield return base.animator.WaitForAnimationToStart(this, "PreJump", false);
		base.StartCoroutine(this.cue_reform_effect_cr(PRE_JUMP_TIME + p.jumpWarning + JUMP_TIME + tEnd3, reformPosX, "JumpReformEffect"));
		yield return base.animator.WaitForAnimationToEnd(this, "PreJump", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.jumpWarning);
		base.animator.Play("Jump");
		this.ball.transform.localPosition = this.BALL_JUMP_OFFSET;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump", false, true);
		this.meltFXAnimator[0].transform.position = base.transform.position;
		this.meltFXAnimator[0].transform.localScale = base.transform.localScale;
		this.meltFXAnimator[0].gameObject.SetActive(true);
		this.meltFXAnimator[0].Play("JumpMeltEffect");
		this.meltFXAnimator[0].transform.parent = null;
		base.transform.position += Vector3.right * (this.ball.transform.localPosition.x * -base.transform.localScale.x * 2f);
		this.ball.SetActive(true);
		this.ballShadow.sprite = this.shadowSprites[0];
		this.ballShadow.enabled = true;
		this.sprite.enabled = false;
		this.coll.enabled = false;
		base.animator.Play("JumpBall", 1, 0f);
		bool stillMoving = true;
		while (stillMoving)
		{
			speed += new Vector3(0f, g * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			this.ballShadow.transform.SetPosition(new float?(this.ball.transform.position.x), new float?(ground - 145f), null);
			this.ballShadow.sprite = this.shadowSprites[Mathf.Clamp((int)((base.transform.position.y - ground) / height * (float)this.shadowSprites.Length), 0, this.shadowSprites.Length - 1)];
			t += CupheadTime.FixedDelta;
			if (t > timeToApex && base.transform.position.y <= ground)
			{
				stillMoving = false;
			}
		}
		base.transform.SetPosition(new float?(reformPosX), new float?(ground), null);
		base.transform.SetEulerAngles(null, null, new float?(0f));
		this.onLeft = !this.onLeft;
		this.FlipSprite();
		this.sprite.enabled = true;
		this.coll.enabled = true;
		this.ballShadow.enabled = false;
		this.ball.SetActive(false);
		this.meltFXAnimator[1].gameObject.SetActive(false);
		string text = this.PeekNextPattern();
		if (text != null)
		{
			if (!(text == "I"))
			{
				if (!(text == "S"))
				{
					if (text == "J")
					{
						base.animator.Play("JumpToJump");
						yield return base.animator.WaitForAnimationToEnd(this, "JumpToJump", false, true);
					}
				}
				else
				{
					base.animator.Play("JumpToDash");
					yield return base.animator.WaitForAnimationToEnd(this, "JumpToDash", false, true);
				}
			}
			else
			{
				base.animator.Play("JumpToIdle");
				yield return base.animator.WaitForAnimationToEnd(this, "JumpToIdle", false, true);
			}
		}
		if (this.PeekNextPattern() == "I")
		{
			yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		this.inBallForm = false;
		yield break;
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x00022885 File Offset: 0x00020A85
	public void StartIcePillar()
	{
		this.SetState(SnowCultLevelYeti.States.IcePillar);
		base.animator.SetTrigger("OnSmash");
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000D0610 File Offset: 0x000CE810
	public void SpawnIcePillars()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		this.snowCultBGHandler.CandleGust();
		Vector3 pos;
		pos..ctor(base.transform.position.x + 290f * (float)((!this.onLeft) ? -1 : 1), 95f);
		this.snowBurstA.Create(pos, (float)((!this.onLeft) ? -1 : 1));
		base.StartCoroutine(this.spawn_snowfall_cr());
		base.StartCoroutine(this.ice_pillar_cr());
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x000D06B0 File Offset: 0x000CE8B0
	public IEnumerator ice_pillar_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		LevelProperties.SnowCult.IcePillar p = base.properties.CurrentState.icePillar;
		float offset = 0f;
		int dir = (!this.onLeft) ? -1 : 1;
		bool type = Rand.Bool();
		Parser.FloatTryParse(p.offsetCoordString.Split(new char[]
		{
			','
		})[this.offsetCoordIndex], out offset);
		for (int i = 0; i < p.icePillarCount; i++)
		{
			Vector3 pos = new Vector3(this.yetiMidPoint.position.x + offset * (float)dir + p.icePillarSpacing * (float)i * (float)dir, -142f);
			SnowCultLevelIcePillar icePillar = this.icePillarPrefab.Spawn<SnowCultLevelIcePillar>();
			icePillar.Init(pos, p, type, p.appearDelay * (float)(i + 1));
			type = !type;
			yield return CupheadTime.WaitForSeconds(this, p.appearDelay);
		}
		this.offsetCoordIndex = (this.offsetCoordIndex + 1) % p.offsetCoordString.Split(new char[]
		{
			','
		}).Length;
		yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		this.SetState(SnowCultLevelYeti.States.Idle);
		yield return null;
		yield break;
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000D06CC File Offset: 0x000CE8CC
	public IEnumerator spawn_snowfall_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		Vector3 pos = new Vector3(base.transform.position.x + 290f * (float)((!this.onLeft) ? -1 : 1), 510f);
		this.snowFallA.Create(pos, (float)((!this.onLeft) ? -1 : 1));
		yield return null;
		yield break;
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000D06E8 File Offset: 0x000CE8E8
	public void InitBats()
	{
		this.batAttackPositionString = new PatternString(base.properties.CurrentState.snowball.batAttackPosition, true);
		this.batAttackHeightString = new PatternString(base.properties.CurrentState.snowball.batAttackHeight, true);
		this.batAttackWidthString = new PatternString(base.properties.CurrentState.snowball.batAttackWidth, true);
		this.batAttackSideString = new PatternString(base.properties.CurrentState.snowball.batAttackSide, true);
		this.batAttackInterDelayString = new PatternString(base.properties.CurrentState.snowball.batAttackInterDelay, true);
		this.batArcModifierString = new PatternString(base.properties.CurrentState.snowball.batArcModifier, true);
		this.batParryableString = new PatternString(base.properties.CurrentState.snowball.batParryableString, true);
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000D07DC File Offset: 0x000CE9DC
	public IEnumerator bats_attack_cr()
	{
		LevelProperties.SnowCult.Snowball p = base.properties.CurrentState.snowball;
		this.batLaunchTimer = this.batAttackInterDelayString.PopFloat();
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			while (this.batCirclingList.Count > 0)
			{
				int which = Random.Range(0, this.batCirclingList.Count);
				if (this.batCirclingList[which] != null && this.batCirclingList[which].reachedCircle)
				{
					while (this.batLaunchTimer > 0f)
					{
						this.batLaunchTimer -= CupheadTime.Delta;
						yield return null;
					}
					if (this.batCirclingList.Count > which && this.batCirclingList[which] != null)
					{
						this.batLaunchTimer = this.batAttackInterDelayString.PopFloat();
						float height = this.batAttackHeightString.PopFloat();
						float num = this.batAttackWidthString.PopFloat();
						Vector3 position = this.batAttackPositions[this.batAttackPositionString.PopInt()].position;
						position.x *= (float)((!this.onLeft) ? -1 : 1);
						num *= (float)((!this.onLeft) ? 1 : -1);
						bool flag = this.batAttackSideString.PopLetter() == 'S';
						position.x *= (float)((!flag) ? 1 : -1);
						num *= (float)((!flag) ? 1 : -1);
						this.batCirclingList[which].AttackPlayer(position, height, num, this.batArcModifierString.PopFloat());
						this.batCirclingList.RemoveAt(which);
					}
				}
				else if (this.batCirclingList[which] == null)
				{
					this.batCirclingList.RemoveAt(which);
				}
				yield return null;
				player = PlayerManager.GetNext();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x0002289E File Offset: 0x00020A9E
	public void ReturnBatToList(SnowCultLevelBat bat)
	{
		this.batCirclingList.Add(bat);
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000D07F8 File Offset: 0x000CE9F8
	public IEnumerator spawn_bats_cr()
	{
		this.batSpawnEffectPrefab.Create(base.transform.position + Vector3.up * 180f + Vector3.right * (float)((!this.onLeft) ? -20 : 20));
		if (this.bats == null)
		{
			this.bats = new SnowCultLevelBat[base.properties.CurrentState.snowball.batCount];
		}
		for (int j = 0; j < this.batCirclingList.Count; j++)
		{
			Object.Destroy(this.batCirclingList[j].gameObject);
		}
		yield return null;
		this.batCirclingList.RemoveAll((SnowCultLevelBat b) => b == null);
		this.SFX_SNOWCULT_YetiFreezerScream();
		for (int i = 0; i < this.bats.Length; i++)
		{
			if (this.bats[i] == null || this.bats[i].gameObject == null || !this.bats[i].gameObject.activeInHierarchy)
			{
				Vector3 launchVelocity = new Vector3((float)((!this.onLeft) ? -1 : 1), 0.25f);
				launchVelocity *= (float)Random.Range(500, 800);
				launchVelocity = Quaternion.Euler(0f, 0f, (float)Random.Range(-30, 30)) * launchVelocity;
				this.bats[i] = this.batPrefab.Spawn<SnowCultLevelBat>();
				bool parryable = this.batParryableString.PopLetter() == 'P';
				this.bats[i].Init(base.transform.position + Vector3.up * 180f, launchVelocity, base.properties.CurrentState.snowball, this, parryable, (!parryable) ? ((!this.batColor) ? "Yellow" : string.Empty) : "Pink");
				if (!parryable)
				{
					this.batColor = !this.batColor;
				}
				this.batCirclingList.Add(this.bats[i]);
				yield return CupheadTime.WaitForSeconds(this, 0.125f);
			}
		}
		this.batLaunchTimer = base.properties.CurrentState.snowball.batInitialDelay;
		yield break;
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000D0814 File Offset: 0x000CEA14
	public void RemoveBats()
	{
		if (this.bats != null)
		{
			for (int i = 0; i < this.bats.Length; i++)
			{
				if (this.bats[i] != null)
				{
					this.bats[i].Dead();
				}
			}
		}
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000228AC File Offset: 0x00020AAC
	public void Snowball()
	{
		base.StartCoroutine(this.snowball_cr());
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000228BB File Offset: 0x00020ABB
	public bool BreakOutOfFridge()
	{
		return this.forceOutroToStart || base.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti;
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000228DE File Offset: 0x00020ADE
	public void FridgeCanShoot()
	{
		this.fridgeCanShoot = true;
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000D0868 File Offset: 0x000CEA68
	public float GetIceCubeStartFrame()
	{
		this.iceCubeStartFrame = (this.iceCubeStartFrame + 1) % 3;
		switch (this.iceCubeStartFrame)
		{
		default:
			return 0f;
		case 1:
			return 2f;
		case 2:
			return 5f;
		}
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x000228E7 File Offset: 0x00020AE7
	public int GetMediumExplosion()
	{
		this.iceCubeExplosionCounterMedium = (this.iceCubeExplosionCounterMedium + 1) % 2;
		return this.iceCubeExplosionCounterMedium;
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000228FF File Offset: 0x00020AFF
	public int GetSmallExplosion()
	{
		this.iceCubeExplosionCounterSmall = (this.iceCubeExplosionCounterSmall + 1) % 3;
		return this.iceCubeExplosionCounterSmall;
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000D08B4 File Offset: 0x000CEAB4
	public IEnumerator snowball_cr()
	{
		this.SetState(SnowCultLevelYeti.States.Snowball);
		this.fridgeCanShoot = false;
		base.animator.SetTrigger("OnFridgeMorph");
		LevelProperties.SnowCult.Snowball p = base.properties.CurrentState.snowball;
		string[] snowballType = p.snowballTypeString[this.snowballMainIndex].Split(new char[]
		{
			','
		});
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			if (this.BreakOutOfFridge())
			{
				base.animator.ResetTrigger("OnFridgeMorph");
				this.SetState(SnowCultLevelYeti.States.Idle);
				yield break;
			}
			yield return null;
		}
		int count = snowballType.Length;
		float t;
		for (int i = 0; i < count; i++)
		{
			if (this.BreakOutOfFridge())
			{
				break;
			}
			while (!this.fridgeCanShoot && !this.forceOutroToStart)
			{
				yield return null;
			}
			this.fridgeCanShoot = false;
			if (!this.BreakOutOfFridge())
			{
				base.animator.Play("FridgeShoot");
				this.SFX_SNOWCULT_YetiFreezerIceCubeLaunch();
				AbstractPlayerController next = PlayerManager.GetNext();
				float num = p.shotMaxAngle;
				float speed = p.shotMaxSpeed;
				float num2 = float.MaxValue;
				Vector2 vector = new Vector2(next.transform.position.x, (float)Level.Current.Ground) - this.cubeLaunchPosition.transform.position;
				vector.x = Mathf.Abs(vector.x);
				MinMax minMax = new MinMax(p.shotMinAngle, p.shotMaxAngle);
				MinMax minMax2 = new MinMax(p.shotMinSpeed, p.shotMaxSpeed);
				if (vector.y > 0f)
				{
					float num3 = minMax2.max / p.shotGravity;
					float num4 = minMax2.max * num3 - 0.5f * p.shotGravity * num3 * num3;
					float num5 = num4 + vector.y * 0f;
					float num6 = Mathf.Sqrt(2f * num5 / p.shotGravity);
					minMax2.max = num6 * p.shotGravity;
					minMax2.min *= minMax2.max / p.shotMaxSpeed;
				}
				float num7 = 0f;
				while (num7 < 1f)
				{
					float floatAt = minMax.GetFloatAt(num7);
					float floatAt2 = minMax2.GetFloatAt(num7);
					Vector2 vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2;
					t = vector.x / vector2.x;
					float num8 = vector2.y * t - 0.5f * p.shotGravity * t * t;
					float num9 = Mathf.Abs(vector.y - num8);
					if (p.shotGravity <= 0.01f)
					{
						goto IL_43C;
					}
					float num10 = vector2.y - p.shotGravity * t;
					if (num10 <= 0f)
					{
						goto IL_43C;
					}
					IL_450:
					num7 += 0.01f;
					continue;
					IL_43C:
					if (num9 < num2)
					{
						num2 = num9;
						num = floatAt;
						speed = floatAt2;
						goto IL_450;
					}
					goto IL_450;
				}
				if (next.transform.position.x < base.transform.position.x)
				{
					num = 180f - num;
				}
				SnowCultLevelSnowball snowCultLevelSnowball = null;
				if (snowballType[i][0] == 'S')
				{
					snowCultLevelSnowball = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				else if (snowballType[i][0] == 'M')
				{
					snowCultLevelSnowball = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				else if (snowballType[i][0] == 'L')
				{
					snowCultLevelSnowball = this.largeSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				snowCultLevelSnowball.InitOriginal(this.cubeLaunchPosition.transform.position, p.shotGravity, speed, num, p, this);
				if (i == snowballType.Length - 1 && Level.Current.mode == Level.Mode.Easy && !this.BreakOutOfFridge())
				{
					i = -1;
					this.snowballMainIndex = (this.snowballMainIndex + 1) % p.snowballTypeString.Length;
					snowballType = p.snowballTypeString[this.snowballMainIndex].Split(new char[]
					{
						','
					});
					count = snowballType.Length;
				}
			}
			if (!this.BreakOutOfFridge() && i < count - 1)
			{
				yield return CupheadTime.WaitForSeconds(this, p.snowballThrowDelay);
			}
		}
		t = 0f;
		while (t < p.batLaunchDelay && !this.BreakOutOfFridge())
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		if (!this.BreakOutOfFridge())
		{
			base.animator.SetTrigger("OnFridgeOutro");
			yield return base.animator.WaitForAnimationToStart(this, "FridgeOutroLoop", false);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "FridgeShoot", false, false);
			if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".FridgeIdle"))
			{
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2777778f && base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8333333f)
				{
					yield return null;
				}
				base.animator.Play("FridgeOutroOpen");
			}
		}
		if (!this.forceOutroToStart)
		{
			yield return base.StartCoroutine(this.spawn_bats_cr());
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.snowballMainIndex = (this.snowballMainIndex + 1) % p.snowballTypeString.Length;
			base.animator.Play("FridgeOutroMorph");
		}
		yield return CupheadTime.WaitForSeconds(this, (!this.BreakOutOfFridge()) ? p.hesitate : 0f);
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.SetState(SnowCultLevelYeti.States.Idle);
		yield return null;
		yield break;
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000D08D0 File Offset: 0x000CEAD0
	public void ToEasyPhaseThree()
	{
		LevelProperties.SnowCult.Yeti yeti = base.properties.CurrentState.yeti;
		this.patternString = yeti.yetiPatternString.Split(new char[]
		{
			','
		});
		this.patternStringIndex = Random.Range(0, this.patternString.Length);
		this.InitBats();
		base.StartCoroutine(this.bats_attack_cr());
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x00022917 File Offset: 0x00020B17
	public void ForceOutroToStart()
	{
		base.animator.SetBool("ForceOutro", true);
		this.forceOutroToStart = true;
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x00022931 File Offset: 0x00020B31
	public void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("DeathEasy");
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x00022949 File Offset: 0x00020B49
	public void OnDeath()
	{
		base.animator.SetBool("Dead", true);
		this.StopAllCoroutines();
		this.RemoveBats();
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x0002297E File Offset: 0x00020B7E
	public void ActivateLegs()
	{
		this.bucket.transform.parent = null;
		this.legs.transform.parent = null;
		this.legs.SetActive(true);
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000229AE File Offset: 0x00020BAE
	public void DeathAnimationEnded()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x000229BB File Offset: 0x00020BBB
	public void AnimationEvent_SFX_SNOWCULT_YetiIntro02DroptoGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground");
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000229D7 File Offset: 0x00020BD7
	public void AnimationEvent_SFX_SNOWCULT_YetiFridgeToSnowmonster()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster");
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000229F3 File Offset: 0x00020BF3
	public void AnimationEvent_SFX_SNOWCULT_YetiSnowmonsterToFridge()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge");
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x00022A0F File Offset: 0x00020C0F
	public void AnimationEvent_SFX_SNOWCULT_GroundSmash()
	{
		AudioManager.Play("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands");
		this.emitAudioFromObject.Add("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands");
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x00022A2B File Offset: 0x00020C2B
	public void AnimationEvent_SFX_SNOWCULT_BodyRollPre()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre");
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x00022A47 File Offset: 0x00020C47
	public void AnimationEvent_SFX_SNOWCULT_BodyRoll()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyroll");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyroll");
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x00022A63 File Offset: 0x00020C63
	public void AnimationEvent_SFX_SNOWCULT_BodyTossPre()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytosspre");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytosspre");
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x00022A7F File Offset: 0x00020C7F
	public void AnimationEvent_SFX_SNOWCULT_BodyToss()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytoss");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytoss");
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x00022A9B File Offset: 0x00020C9B
	public void AnimationEvent_SFX_SNOWCULT_YetiDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_explode");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_explode");
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x000D0934 File Offset: 0x000CEB34
	public void SFX_SNOWCULT_YetiFreezerScream()
	{
		this.batSoundLong = !this.batSoundLong;
		AudioManager.Play((!this.batSoundLong) ? "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short" : "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long");
		this.emitAudioFromObject.Add((!this.batSoundLong) ? "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short" : "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long");
	}

	// Token: 0x0600292D RID: 10541 RVA: 0x00022AB7 File Offset: 0x00020CB7
	public void SFX_SNOWCULT_YetiFreezerIceCubeLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch");
	}

	// Token: 0x04002242 RID: 8770
	public SnowCultLevelYeti.States state;

	// Token: 0x04002243 RID: 8771
	public SnowCultLevelYeti.States previousState;

	// Token: 0x04002245 RID: 8773
	public const float BURST_SPAWN_X = 290f;

	// Token: 0x04002246 RID: 8774
	public const float Y_TO_SPAWN = 95f;

	// Token: 0x04002247 RID: 8775
	public const float Y_ICE_PILLAR_SPAWN = -142f;

	// Token: 0x04002248 RID: 8776
	public const float POS_OFFSET_X = 147f;

	// Token: 0x04002249 RID: 8777
	public const float JUMP_LANDING_OFFSET_X = 247f;

	// Token: 0x0400224A RID: 8778
	public const float REFORM_TIME = 0.9583333f;

	// Token: 0x0400224B RID: 8779
	public const float BALL_RADIUS = 180f;

	// Token: 0x0400224C RID: 8780
	public Vector3 BALL_JUMP_OFFSET = new Vector3(50f, -100f);

	// Token: 0x0400224D RID: 8781
	public Vector3 BALL_DASH_OFFSET = new Vector3(50f, -180f);

	// Token: 0x0400224E RID: 8782
	[SerializeField]
	public SnowCultHandleBackground snowCultBGHandler;

	// Token: 0x0400224F RID: 8783
	[SerializeField]
	public Transform yetiMidPoint;

	// Token: 0x04002250 RID: 8784
	[SerializeField]
	public Transform yetiSpawnPoint;

	// Token: 0x04002251 RID: 8785
	[SerializeField]
	public SnowCultLevelIcePillar icePillarPrefab;

	// Token: 0x04002252 RID: 8786
	[SerializeField]
	public SnowCultLevelBat batPrefab;

	// Token: 0x04002253 RID: 8787
	[SerializeField]
	public Effect batSpawnEffectPrefab;

	// Token: 0x04002254 RID: 8788
	public bool batSoundLong;

	// Token: 0x04002255 RID: 8789
	[SerializeField]
	public SnowCultLevelBurstEffect snowBurstA;

	// Token: 0x04002256 RID: 8790
	[SerializeField]
	public SnowCultLevelBurstEffect snowFallA;

	// Token: 0x04002257 RID: 8791
	[Header("Snowballs")]
	[SerializeField]
	public SnowCultLevelSnowball smallSnowballPrefab;

	// Token: 0x04002258 RID: 8792
	[SerializeField]
	public SnowCultLevelSnowball mediumSnowballPrefab;

	// Token: 0x04002259 RID: 8793
	[SerializeField]
	public SnowCultLevelSnowball largeSnowballPrefab;

	// Token: 0x0400225A RID: 8794
	[SerializeField]
	public GameObject cubeLaunchPosition;

	// Token: 0x0400225B RID: 8795
	[SerializeField]
	public GameObject ball;

	// Token: 0x0400225C RID: 8796
	[SerializeField]
	public Animator[] meltFXAnimator;

	// Token: 0x0400225D RID: 8797
	[SerializeField]
	public GameObject dashGroundFX;

	// Token: 0x0400225E RID: 8798
	[SerializeField]
	public GameObject groundMask;

	// Token: 0x0400225F RID: 8799
	[SerializeField]
	public SpriteRenderer ballShadow;

	// Token: 0x04002260 RID: 8800
	[SerializeField]
	public GameObject introShadow;

	// Token: 0x04002261 RID: 8801
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04002262 RID: 8802
	public int offsetCoordIndex;

	// Token: 0x04002263 RID: 8803
	public int snowballMainIndex;

	// Token: 0x04002264 RID: 8804
	public string[] patternString;

	// Token: 0x04002265 RID: 8805
	public int patternStringIndex;

	// Token: 0x04002266 RID: 8806
	public PatternString batAttackPositionString;

	// Token: 0x04002267 RID: 8807
	public PatternString batAttackHeightString;

	// Token: 0x04002268 RID: 8808
	public PatternString batAttackWidthString;

	// Token: 0x04002269 RID: 8809
	public PatternString batAttackSideString;

	// Token: 0x0400226A RID: 8810
	public PatternString batAttackInterDelayString;

	// Token: 0x0400226B RID: 8811
	public PatternString batArcModifierString;

	// Token: 0x0400226C RID: 8812
	public PatternString batParryableString;

	// Token: 0x0400226D RID: 8813
	[SerializeField]
	public Transform[] batAttackPositions;

	// Token: 0x0400226E RID: 8814
	public float xScale;

	// Token: 0x0400226F RID: 8815
	public bool onLeft;

	// Token: 0x04002270 RID: 8816
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04002271 RID: 8817
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	public GameObject legs;

	// Token: 0x04002273 RID: 8819
	[SerializeField]
	public GameObject bucket;

	// Token: 0x04002274 RID: 8820
	public SnowCultLevelBat[] bats;

	// Token: 0x04002275 RID: 8821
	public List<SnowCultLevelBat> batCirclingList = new List<SnowCultLevelBat>();

	// Token: 0x04002276 RID: 8822
	public float batLaunchTimer;

	// Token: 0x04002277 RID: 8823
	public bool batColor;

	// Token: 0x04002278 RID: 8824
	public DamageDealer damageDealer;

	// Token: 0x04002279 RID: 8825
	public DamageReceiver damageReceiver;

	// Token: 0x0400227A RID: 8826
	public DamageReceiver ballDamageReceiver;

	// Token: 0x0400227B RID: 8827
	public bool fridgeCanShoot;

	// Token: 0x0400227C RID: 8828
	public bool introRibcageClosed;

	// Token: 0x0400227D RID: 8829
	public bool forceOutroToStart;

	// Token: 0x0400227E RID: 8830
	public int iceCubeStartFrame;

	// Token: 0x0400227F RID: 8831
	public int iceCubeExplosionCounterMedium;

	// Token: 0x04002280 RID: 8832
	public int iceCubeExplosionCounterSmall;

	// Token: 0x04002281 RID: 8833
	public int idleAnimFullPathHash;

	// Token: 0x02000F83 RID: 3971
	public enum States
	{
		// Token: 0x04007000 RID: 28672
		Intro,
		// Token: 0x04007001 RID: 28673
		Idle,
		// Token: 0x04007002 RID: 28674
		Move,
		// Token: 0x04007003 RID: 28675
		IcePillar,
		// Token: 0x04007004 RID: 28676
		Sled,
		// Token: 0x04007005 RID: 28677
		Snowball
	}
}
