using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class SallyStagePlayLevelSally : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06002643 RID: 9795 RVA: 0x000201C6 File Offset: 0x0001E3C6
	// (set) Token: 0x06002644 RID: 9796 RVA: 0x000201CE File Offset: 0x0001E3CE
	public SallyStagePlayLevelSally.State state { get; set; }

	// Token: 0x06002645 RID: 9797 RVA: 0x000C8844 File Offset: 0x000C6A44
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isInvincible)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE && this.husband.gameObject.activeInHierarchy)
		{
			if (!AudioManager.CheckIfPlaying("sally_bg_church_fiance_ohno"))
			{
				AudioManager.Play("sally_bg_church_fiance_ohno");
				this.emitAudioFromObject.Add("sally_bg_church_fiance_ohno");
			}
			this.husband.GetComponent<Animator>().Play("OhNo");
		}
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000C88CC File Offset: 0x000C6ACC
	public override void LevelInit(LevelProperties.SallyStagePlay properties)
	{
		base.LevelInit(properties);
		this.bounds = base.GetComponent<BoxCollider2D>().bounds.size;
		this.jumpTypeIndex = Random.Range(0, properties.CurrentState.jump.JumpAttackString.Split(new char[]
		{
			','
		}).Length);
		this.jumpCountIndex = Random.Range(0, properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		}).Length);
		this.jumpRollAttackTypeIndex = Random.Range(0, properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		}).Length);
		this.heartTypeIndex = Random.Range(0, properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		}).Length);
		this.teleportOffsetIndex = Random.Range(0, properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		}).Length);
		base.transform.position = this.ground;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000201D7 File Offset: 0x0001E3D7
	public void GetParent(SallyStagePlayLevel parent)
	{
		parent.OnPhase2 += this.OnPhase2;
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000C8A04 File Offset: 0x000C6C04
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.collisionChild.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.ground = new Vector3(base.transform.position.x, (float)Level.Current.Ground + 300f, 0f);
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000C8AB0 File Offset: 0x000C6CB0
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.ground.x = base.transform.position.x;
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000201EB File Offset: 0x0001E3EB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000C8AF4 File Offset: 0x000C6CF4
	public IEnumerator intro_cr()
	{
		this.state = SallyStagePlayLevelSally.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("sally_sally_intro_phase1");
		this.emitAudioFromObject.Add("sally_sally_intro_phase1");
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x000C8B10 File Offset: 0x000C6D10
	public void OnJumpAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		this.jumpType = (SallyStagePlayLevelSally.JumpType)Parser.IntParse(base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
		{
			','
		})[this.jumpTypeIndex]);
		this.target = PlayerManager.GetNext();
		this.currentJumpAttackCount = 0;
		base.StartCoroutine(this.jump_cr());
		this.jumpCountIndex = (this.jumpCountIndex + 1) % base.properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000C8BB0 File Offset: 0x000C6DB0
	public IEnumerator jump_cr()
	{
		if (this.currentJumpAttackCount >= Parser.IntParse(base.properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		})[this.jumpCountIndex]))
		{
			float rand = base.properties.CurrentState.jump.JumpHesitate.RandomFloat();
			if (rand > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, rand);
			}
			this.state = SallyStagePlayLevelSally.State.Idle;
			yield return null;
		}
		else
		{
			this.jumpTypeIndex = (this.jumpTypeIndex + 1) % base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
			{
				','
			}).Length;
			this.jumpType = (SallyStagePlayLevelSally.JumpType)Parser.IntParse(base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
			{
				','
			})[this.jumpTypeIndex]);
			if (this.currentJumpAttackCount > 0 && base.properties.CurrentState.jump.JumpDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.jump.JumpDelay);
			}
			base.animator.SetInteger("JumpType", (int)this.jumpType);
			base.animator.SetTrigger("Jump");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_TakeOff", true, true);
			Vector3 start = base.transform.position;
			Vector3 end = start;
			SallyStagePlayLevelSally.JumpType jumpType = this.jumpType;
			if (jumpType != SallyStagePlayLevelSally.JumpType.DiveKick)
			{
				if (jumpType == SallyStagePlayLevelSally.JumpType.DoubleJump)
				{
					end += Vector3.up * base.properties.CurrentState.jumpRoll.JumpHeight.RandomFloat();
				}
			}
			else
			{
				end += Vector3.up * base.properties.CurrentState.diveKick.DiveAttackHeight.RandomFloat();
			}
			base.StartCoroutine(this.shadow_cr(true));
			float timePassed = 0f;
			while (timePassed / 0.1665f < 1f)
			{
				if (CupheadTime.Delta != 0f)
				{
					base.transform.position = start + (end - start) * (timePassed / 0.1665f);
					timePassed += CupheadTime.Delta;
				}
				yield return null;
			}
			base.animator.SetTrigger("OnAttack");
			this.currentJumpAttackCount++;
			this.StartJumpAttack(this.jumpType);
		}
		yield break;
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x00020209 File Offset: 0x0001E409
	public void StartJumpAttack(SallyStagePlayLevelSally.JumpType type)
	{
		if (type != SallyStagePlayLevelSally.JumpType.DiveKick)
		{
			if (type == SallyStagePlayLevelSally.JumpType.DoubleJump)
			{
				base.StartCoroutine(this.jumpRoll_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.diveKick_cr());
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000C8BCC File Offset: 0x000C6DCC
	public IEnumerator landing_cr(bool useTrigger = true)
	{
		base.StartCoroutine(this.shadow_cr(false));
		if (this.target == null || this.target.IsDead)
		{
			this.target = PlayerManager.GetNext();
		}
		if (this.target.center.x > base.transform.position.x)
		{
			if (base.transform.right.x > 0f)
			{
				if (useTrigger)
				{
					yield return new WaitForEndOfFrame();
					base.animator.SetTrigger("OnTurnLanding");
					yield return base.animator.WaitForAnimationToEnd(this, true);
					base.transform.right *= -1f;
					yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
				}
				else
				{
					yield return new WaitForEndOfFrame();
					base.animator.Play("Land_and_Turn");
					yield return base.animator.WaitForAnimationToEnd(this, true);
					base.transform.right *= -1f;
					yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
				}
			}
			else if (useTrigger)
			{
				base.animator.SetTrigger("OnLanding");
				yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
			}
			else
			{
				base.animator.Play("Land");
				yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
			}
		}
		else if (base.transform.right.x < 0f)
		{
			if (useTrigger)
			{
				yield return new WaitForEndOfFrame();
				base.animator.SetTrigger("OnTurnLanding");
				yield return base.animator.WaitForAnimationToEnd(this, true);
				base.transform.right *= -1f;
				yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
			}
			else
			{
				yield return new WaitForEndOfFrame();
				base.animator.Play("Land_and_Turn");
				yield return base.animator.WaitForAnimationToEnd(this, true);
				base.transform.right *= -1f;
				yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
			}
		}
		else if (useTrigger)
		{
			base.animator.SetTrigger("OnLanding");
			yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
		}
		else
		{
			base.animator.Play("Land");
			yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
		}
		if (!this.getOutOfJump)
		{
			base.StartCoroutine(this.jump_cr());
		}
		else
		{
			this.state = SallyStagePlayLevelSally.State.Idle;
		}
		yield break;
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x00020247 File Offset: 0x0001E447
	public void LandSFX()
	{
		AudioManager.Play("sally_sally_land");
		this.emitAudioFromObject.Add("sally_sally_land");
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x000C8BF0 File Offset: 0x000C6DF0
	public IEnumerator shadow_cr(bool fadeOut = true)
	{
		GameObject shadow = Object.Instantiate<GameObject>(this.shadowPrefab, new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f), Quaternion.identity);
		if (fadeOut)
		{
			shadow.GetComponent<Animator>().Play("FadeOut");
		}
		else
		{
			shadow.GetComponent<Animator>().Play("FadeIn");
		}
		yield return shadow.GetComponent<Animator>().WaitForAnimationToEnd(this, true);
		Object.Destroy(shadow);
		yield break;
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x000C8C14 File Offset: 0x000C6E14
	public IEnumerator diveKick_cr()
	{
		base.animator.Play("DiveKick_Transition");
		Vector2 direction = -base.transform.right;
		float angle = (float)base.properties.CurrentState.diveKick.DiveAngleRange.RandomInt() / 100f;
		if (angle == 0f)
		{
			angle = 0.001f;
		}
		direction.x = direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle);
		direction.y = direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle);
		direction.y = -Mathf.Abs(direction.y);
		bool attacking = true;
		AudioManager.Play("sally_divekick_loop");
		this.emitAudioFromObject.Add("sally_divekick_loop");
		Vector3 deltaPosition = Vector3.zero;
		while (attacking)
		{
			if (CupheadTime.Delta != 0f)
			{
				deltaPosition = Vector3.zero;
			}
			if (Mathf.Sign(direction.x) > 0f)
			{
				if (base.transform.position.x + this.bounds.x / 2f < (float)Level.Current.Right)
				{
					deltaPosition.x = direction.x * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
				}
			}
			else if (base.transform.position.x - this.bounds.x / 2f > (float)Level.Current.Left)
			{
				deltaPosition.x = direction.x * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
			}
			if (base.transform.position.y > this.ground.y)
			{
				deltaPosition.y = Mathf.Sign(direction.y) * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
			}
			else
			{
				deltaPosition.y = 0f;
			}
			if (deltaPosition.y == 0f)
			{
				if (CupheadTime.Delta != 0f)
				{
					attacking = false;
				}
			}
			else
			{
				base.transform.position += deltaPosition;
			}
			yield return null;
		}
		base.StartCoroutine(this.landing_cr(false));
		yield break;
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x000C8C30 File Offset: 0x000C6E30
	public IEnumerator jumpRoll_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "JumpRoll_Transition", true, true);
		if (!this.getOutOfJump)
		{
			base.StartCoroutine(this.rollAttack_cr());
			AudioManager.PlayLoop("sally_double_jump_roll_loop");
			this.emitAudioFromObject.Add("sally_double_jump_roll_loop");
		}
		Vector3 start = base.transform.position;
		Vector3 end = start + Vector3.up * base.properties.CurrentState.jumpRoll.RollJumpVerticalMovement;
		end += -base.transform.right * base.properties.CurrentState.jumpRoll.RollJumpHorizontalMovement.RandomFloat();
		if (end.x - this.bounds.x / 2f < (float)Level.Current.Left)
		{
			end.x = (float)Level.Current.Left + this.bounds.x / 2f;
		}
		else if (end.x + this.bounds.x / 2f > (float)Level.Current.Right)
		{
			end.x = (float)Level.Current.Right - this.bounds.x / 2f;
		}
		float pct = 0f;
		while (pct < base.properties.CurrentState.jumpRoll.JumpRollDuration)
		{
			base.transform.position = start + (end - start) * pct;
			pct += CupheadTime.Delta;
			yield return null;
		}
		yield return base.animator.WaitForAnimationToEnd(this, "JumpRoll_Roll", true, true);
		AudioManager.Stop("sally_double_jump_roll_loop");
		base.StartCoroutine(this.fall_cr());
		this.jumpRollAttackTypeIndex++;
		if (this.jumpRollAttackTypeIndex >= base.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		}).Length)
		{
			this.jumpRollAttackTypeIndex = 0;
		}
		yield break;
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000C8C4C File Offset: 0x000C6E4C
	public IEnumerator fall_cr()
	{
		float speed = base.properties.CurrentState.teleport.fallingSpeed.RandomFloat();
		int iteration = 1;
		float offset = 150f;
		bool useTrigger = false;
		if (this.isTeleporting)
		{
			offset = 180f;
			useTrigger = true;
		}
		else
		{
			offset = this.ground.y;
			useTrigger = false;
		}
		while (base.transform.position.y > offset)
		{
			base.transform.position += Vector3.down * speed * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				speed += base.properties.CurrentState.teleport.acceleration * (float)iteration;
				iteration++;
			}
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, offset, 0f);
		if (this.isTeleporting)
		{
			base.animator.SetTrigger("OnSawEnd");
		}
		base.StartCoroutine(this.landing_cr(useTrigger));
		yield return null;
		yield break;
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x000C8C68 File Offset: 0x000C6E68
	public IEnumerator rollAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.jumpRoll.RollShotDelayRange.RandomFloat());
		char c = base.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		})[this.jumpRollAttackTypeIndex][0];
		if (c != 'S')
		{
			if (c == 'B')
			{
				this.SpawnProjectile();
			}
		}
		else
		{
			this.SpawnShuriken();
		}
		yield break;
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x000C8C84 File Offset: 0x000C6E84
	public void SpawnShuriken()
	{
		for (int i = -1; i < 1; i++)
		{
			AbstractProjectile abstractProjectile = this.shurikenPrefab.Create(base.transform.position + Vector3.up * 0.5f);
			abstractProjectile.GetComponent<SallyStagePlayLevelShurikenBomb>().InitShuriken(base.properties, i, this.target);
		}
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x000C8CEC File Offset: 0x000C6EEC
	public void SpawnProjectile()
	{
		Vector3 vector = this.target.transform.position - this.centerPoint.transform.position;
		SallyStagePlayLevelProjectile sallyStagePlayLevelProjectile = Object.Instantiate<SallyStagePlayLevelProjectile>(this.projectilePrefab);
		sallyStagePlayLevelProjectile.Init(this.centerPoint.transform.position, MathUtils.DirectionToAngle(vector), base.properties.CurrentState.projectile);
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x00020263 File Offset: 0x0001E463
	public void OnUmbrellaAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		base.StartCoroutine(this.startUmbrella_cr());
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x000C8D64 File Offset: 0x000C6F64
	public IEnumerator startUmbrella_cr()
	{
		base.animator.SetBool("UmbrellaAttack", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Umbrella_Spin_Start", false, true);
		AudioManager.Play("sally_umbrella_spin");
		this.emitAudioFromObject.Add("sally_umbrella_spin");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.initialAttackDelay);
		for (int i = 0; i < base.properties.CurrentState.umbrella.objectCount; i++)
		{
			if (i != 0)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.objectDelay);
			}
			AudioManager.Play("sally_umbrella_spin_shoot");
			this.emitAudioFromObject.Add("sally_umbrella_spin_shoot");
			AbstractProjectile proj = this.umbrellaProjectilePrefab.Create(this.spawnPoints[0].position);
			proj.GetComponent<SallyStagePlayLevelUmbrellaProjectile>().InitProjectile(base.properties, (int)(-(int)base.transform.right.x));
			proj = this.umbrellaProjectilePrefab.Create(this.spawnPoints[1].position);
			proj.GetComponent<SallyStagePlayLevelUmbrellaProjectile>().InitProjectile(base.properties, (int)base.transform.right.x);
			if (this.getOutOfJump)
			{
				break;
			}
		}
		base.animator.SetBool("UmbrellaAttack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.hesitate);
		AudioManager.Play("sally_umbrella_spin_end");
		this.emitAudioFromObject.Add("sally_umbrella_spin_end");
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield break;
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x00020279 File Offset: 0x0001E479
	public void UmbrellaIntroSFX()
	{
		AudioManager.Play("sally_sally_umbrella_intro");
		this.emitAudioFromObject.Add("sally_sally_umbrella_intro");
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000C8D80 File Offset: 0x000C6F80
	public void OnKissAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		base.animator.SetTrigger("OnKissAttack");
		this.target = PlayerManager.GetNext();
		if (this.target.center.x > this.centerPoint.position.x)
		{
			if (base.transform.eulerAngles.y == 0f)
			{
				base.transform.right *= -1f;
			}
		}
		else if (base.transform.eulerAngles.y == 180f)
		{
			base.transform.right *= -1f;
		}
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x000C8E50 File Offset: 0x000C7050
	public void SpawnHeart()
	{
		AbstractProjectile abstractProjectile = this.heartPrefab.Create(this.spawnPoints[0].position);
		bool isParryable = base.properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		})[this.heartTypeIndex][0] != 'R';
		int direction = ((int)base.transform.eulerAngles.y != 180) ? 1 : -1;
		abstractProjectile.GetComponent<SallyStagePlayLevelHeart>().InitHeart(base.properties, direction, isParryable);
		abstractProjectile.GetComponent<Transform>().SetScale(new float?((float)((base.transform.right.x <= 0f) ? -1 : 1)), null, null);
		this.heartTypeIndex++;
		if (this.heartTypeIndex >= base.properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		}).Length)
		{
			this.heartTypeIndex = 0;
		}
		base.StartCoroutine(this.endKiss_cr());
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x00020295 File Offset: 0x0001E495
	public void KissSFX()
	{
		AudioManager.Play("sally_sally_kiss");
		this.emitAudioFromObject.Add("sally_sally_kiss");
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000C8F94 File Offset: 0x000C7194
	public IEnumerator endKiss_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.kiss.hesitate);
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield break;
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000202B1 File Offset: 0x0001E4B1
	public void OnTeleportAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		this.isTeleporting = true;
		base.animator.SetTrigger("OnTeleport");
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000C8FB0 File Offset: 0x000C71B0
	public void Teleport()
	{
		base.transform.SetPosition(null, new float?((float)Level.Current.Ceiling + this.teleportOffset), null);
		base.StartCoroutine(this.delay_cr());
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x000C9000 File Offset: 0x000C7200
	public IEnumerator delay_cr()
	{
		Vector3 pos;
		pos.y = (float)Level.Current.Ceiling + this.teleportOffset;
		pos.z = 0f;
		base.animator.SetTrigger("OnTeleport");
		yield return base.animator.WaitForAnimationToStart(this, "Teleport_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.teleport.offScreenDelay);
		this.target = PlayerManager.GetNext();
		pos.x = this.target.center.x + (float)Parser.IntParse(base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		})[this.teleportOffsetIndex]);
		if (Parser.IntParse(base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		})[this.teleportOffsetIndex]) <= 0)
		{
			if (pos.x - 75f < (float)Level.Current.Left)
			{
				pos.x = (float)(Level.Current.Left + 75);
			}
			base.transform.right *= -1f;
		}
		else
		{
			if (pos.x + 75f > (float)Level.Current.Right)
			{
				pos.x = (float)(Level.Current.Right - 75);
			}
			base.transform.right *= 1f;
		}
		base.transform.position = pos;
		base.StartCoroutine(this.fall_cr());
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.teleportOffsetIndex++;
		if (this.teleportOffsetIndex >= base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		}).Length)
		{
			this.teleportOffsetIndex = 0;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.teleport.hesitate);
		base.transform.position = this.ground;
		this.isTeleporting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x000202D1 File Offset: 0x0001E4D1
	public void MovePosition()
	{
		base.StartCoroutine(this.move_position_cr());
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x000C901C File Offset: 0x000C721C
	public IEnumerator move_position_cr()
	{
		Vector3 pos = base.transform.position;
		float speed = 700f;
		while (base.transform.position.y > this.ground.y)
		{
			pos.y -= speed * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		base.transform.position = this.ground;
		yield return null;
		yield break;
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x000202E0 File Offset: 0x0001E4E0
	public void TeleportOutSFX()
	{
		AudioManager.Play("sally_sally_teleport_out");
		this.emitAudioFromObject.Add("sally_sally_teleport_out");
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x000202FC File Offset: 0x0001E4FC
	public void TeleportEndSFX()
	{
		AudioManager.Play("sally_sally_teleport_end");
		this.emitAudioFromObject.Add("sally_sally_teleport_end");
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x00020318 File Offset: 0x0001E518
	public void OnPhase3(bool killedHusband)
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.phase2_death_cr(killedHusband));
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x000C9038 File Offset: 0x000C7238
	public IEnumerator phase2_death_cr(bool killedHusband)
	{
		float speed = 300f;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("sally_vox_death_cry");
		this.emitAudioFromObject.Add("sally_vox_death_cry");
		yield return base.animator.WaitForAnimationToEnd(this, "Death_Ph2_Start", false, true);
		while (base.transform.position.y < 660f)
		{
			base.transform.position += Vector3.up * speed * CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		this.angel.StartPhase3(killedHusband);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x0002032E File Offset: 0x0001E52E
	public void PrePhase2()
	{
		this.getOutOfJump = true;
		this.isInvincible = true;
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x0002033E File Offset: 0x0001E53E
	public void OnPhase2()
	{
		this.state = SallyStagePlayLevelSally.State.Transition;
		this.jumpTypeIndex = 0;
		this.jumpRollAttackTypeIndex = 0;
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x00020355 File Offset: 0x0001E555
	public void StartPhase2()
	{
		this.getOutOfJump = true;
		base.animator.SetTrigger("OnIntro");
		base.StartCoroutine(this.phase_2_cr());
	}

	// Token: 0x0600266B RID: 9835 RVA: 0x0002037B File Offset: 0x0001E57B
	public void Intro2SFX()
	{
		AudioManager.Play("sally_sally_intro_phase2");
		this.emitAudioFromObject.Add("sally_sally_intro_phase2");
	}

	// Token: 0x0600266C RID: 9836 RVA: 0x000C905C File Offset: 0x000C725C
	public IEnumerator phase_2_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Teleport_GONE", false, true);
		base.StartCoroutine(this.slide_cr());
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.isInvincible = false;
		this.getOutOfJump = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = SallyStagePlayLevelSally.State.Idle;
		this.house.StartAttacks();
		yield return null;
		yield break;
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x000C9078 File Offset: 0x000C7278
	public IEnumerator slide_cr()
	{
		float startPos = 0f;
		float endPos = 0f;
		float appearPos = 300f;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 == null || player.IsDead || player2.IsDead)
		{
			if (this.target == null || this.target.IsDead)
			{
				this.target = PlayerManager.GetNext();
			}
			if (this.target.transform.position.x > 0f)
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
			else
			{
				if (base.transform.right.x < 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = 840f;
				endPos = 640f - appearPos;
			}
		}
		else
		{
			float num = -640f - player.transform.position.x;
			float num2 = 640f - player.transform.position.x;
			float num3 = -640f - player2.transform.position.x;
			float num4 = 640f - player2.transform.position.x;
			if (player.transform.position.x < 0f)
			{
				if (player2.transform.position.x < 0f)
				{
					if (base.transform.right.x < 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = 840f;
					endPos = 640f - appearPos;
				}
				else if (num < num4)
				{
					if (base.transform.right.x < 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = 840f;
					endPos = 640f - appearPos;
				}
				else
				{
					if (base.transform.right.x > 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = -840f;
					endPos = -640f + appearPos;
				}
			}
			else if (player2.transform.position.x > 0f)
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
			else if (num2 < num3)
			{
				if (base.transform.right.x < 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = 840f;
				endPos = 640f - appearPos;
			}
			else
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
		}
		base.transform.position = new Vector3(startPos, base.transform.position.y, base.transform.position.z);
		float t = 0f;
		float time = 0.75f;
		YieldInstruction wait = new WaitForFixedUpdate();
		float frameTime = 0f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			frameTime += CupheadTime.FixedDelta;
			if (frameTime > 0.0416666679f)
			{
				frameTime -= 0.0416666679f;
				float num5 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
				base.transform.SetPosition(new float?(Mathf.Lerp(startPos, endPos, num5)), null, null);
			}
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600266E RID: 9838 RVA: 0x00020397 File Offset: 0x0001E597
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.shurikenPrefab = null;
		this.projectilePrefab = null;
		this.umbrellaProjectilePrefab = null;
		this.heartPrefab = null;
		this.shadowPrefab = null;
	}

	// Token: 0x0600266F RID: 9839 RVA: 0x000203C8 File Offset: 0x0001E5C8
	public void SoundSallyVoxAttackMmmYoh()
	{
		AudioManager.Play("sally_vox_attack_mmm_yoh");
		this.emitAudioFromObject.Add("sally_vox_attack_mmm_yoh");
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000203E4 File Offset: 0x0001E5E4
	public void SoundSallyVoxAttackQuick()
	{
		AudioManager.Play("sally_vox_attack_quick");
		this.emitAudioFromObject.Add("sally_vox_attack_quick");
		AudioManager.Stop("sally_vox_maniacal");
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x0002040A File Offset: 0x0001E60A
	public void SoundSallyVoxAttackDeathCry()
	{
		AudioManager.Play("sally_vox_death_cry");
		this.emitAudioFromObject.Add("sally_vox_death_cry");
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x00020426 File Offset: 0x0001E626
	public void SoundSallyVoxFrustrated()
	{
		AudioManager.Play("sally_vox_frustrated");
		this.emitAudioFromObject.Add("sally_vox_frustrated");
	}

	// Token: 0x06002673 RID: 9843 RVA: 0x00020442 File Offset: 0x0001E642
	public void SoundSallyVoxLaughBig()
	{
		AudioManager.Play("sally_vox_laugh_big");
		this.emitAudioFromObject.Add("sally_vox_laugh_big");
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x0002045E File Offset: 0x0001E65E
	public void SoundSallyVoxLaughSmall()
	{
		AudioManager.Play("sally_vox_laugh_small");
		this.emitAudioFromObject.Add("sally_vox_laugh_small");
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x0002047A File Offset: 0x0001E67A
	public void SoundSallyVoxLaughManiacal()
	{
		AudioManager.Play("sally_vox_maniacal");
		this.emitAudioFromObject.Add("sally_vox_maniacal");
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x00020496 File Offset: 0x0001E696
	public void SoundSallyVoxDeathOperatic()
	{
		AudioManager.Play("sally_vox_operatic_death");
		this.emitAudioFromObject.Add("sally_vox_operatic_death");
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x000204B2 File Offset: 0x0001E6B2
	public void SoundSallyVoxPainGrowl()
	{
		AudioManager.Play("sally_vox_pain_growl");
		this.emitAudioFromObject.Add("sally_vox_pain_growl");
	}

	// Token: 0x04001F9D RID: 8093
	[Header("Projectiles")]
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04001F9E RID: 8094
	[SerializeField]
	public CollisionChild collisionChild;

	// Token: 0x04001F9F RID: 8095
	[SerializeField]
	public Transform husband;

	// Token: 0x04001FA0 RID: 8096
	[SerializeField]
	public SallyStagePlayLevelAngel angel;

	// Token: 0x04001FA1 RID: 8097
	[SerializeField]
	public SallyStagePlayLevelShurikenBomb shurikenPrefab;

	// Token: 0x04001FA2 RID: 8098
	[SerializeField]
	public SallyStagePlayLevelProjectile projectilePrefab;

	// Token: 0x04001FA3 RID: 8099
	[SerializeField]
	public SallyStagePlayLevelUmbrellaProjectile umbrellaProjectilePrefab;

	// Token: 0x04001FA4 RID: 8100
	[SerializeField]
	public SallyStagePlayLevelHeart heartPrefab;

	// Token: 0x04001FA5 RID: 8101
	[SerializeField]
	public SallyStagePlayLevelHouse house;

	// Token: 0x04001FA7 RID: 8103
	public const float SALLY_INIT_JUMP_TIME = 0.1665f;

	// Token: 0x04001FA8 RID: 8104
	public SallyStagePlayLevelSally.JumpType jumpType;

	// Token: 0x04001FA9 RID: 8105
	public int jumpTypeIndex;

	// Token: 0x04001FAA RID: 8106
	public int jumpCountIndex;

	// Token: 0x04001FAB RID: 8107
	public int currentJumpAttackCount;

	// Token: 0x04001FAC RID: 8108
	public int jumpRollAttackTypeIndex;

	// Token: 0x04001FAD RID: 8109
	public int heartTypeIndex;

	// Token: 0x04001FAE RID: 8110
	public int teleportOffsetIndex;

	// Token: 0x04001FAF RID: 8111
	public float teleportOffset = 500f;

	// Token: 0x04001FB0 RID: 8112
	public bool getOutOfJump;

	// Token: 0x04001FB1 RID: 8113
	public bool isTeleporting;

	// Token: 0x04001FB2 RID: 8114
	public bool isInvincible;

	// Token: 0x04001FB3 RID: 8115
	public Vector2 bounds;

	// Token: 0x04001FB4 RID: 8116
	public Vector3 ground;

	// Token: 0x04001FB5 RID: 8117
	public AbstractPlayerController target;

	// Token: 0x04001FB6 RID: 8118
	public DamageDealer damageDealer;

	// Token: 0x04001FB7 RID: 8119
	public DamageReceiver damageReceiver;

	// Token: 0x04001FB8 RID: 8120
	[Space(10f)]
	[SerializeField]
	public GameObject shadowPrefab;

	// Token: 0x04001FB9 RID: 8121
	[SerializeField]
	public Transform centerPoint;

	// Token: 0x04001FBA RID: 8122
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x02000F13 RID: 3859
	public enum State
	{
		// Token: 0x04006C90 RID: 27792
		Intro,
		// Token: 0x04006C91 RID: 27793
		Idle,
		// Token: 0x04006C92 RID: 27794
		Attack,
		// Token: 0x04006C93 RID: 27795
		Transition
	}

	// Token: 0x02000F14 RID: 3860
	public enum JumpType
	{
		// Token: 0x04006C95 RID: 27797
		DiveKick = 1,
		// Token: 0x04006C96 RID: 27798
		DoubleJump
	}
}
