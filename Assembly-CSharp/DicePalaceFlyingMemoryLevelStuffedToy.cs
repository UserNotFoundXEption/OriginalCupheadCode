using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class DicePalaceFlyingMemoryLevelStuffedToy : LevelProperties.DicePalaceFlyingMemory.Entity
{
	// Token: 0x060016FC RID: 5884 RVA: 0x000A0A9C File Offset: 0x0009EC9C
	public override void Awake()
	{
		base.Awake();
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed;
		base.GetComponent<DamageReceiver>().enabled = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x000A0AF0 File Offset: 0x0009ECF0
	public override void LevelInit(LevelProperties.DicePalaceFlyingMemory properties)
	{
		base.LevelInit(properties);
		this.shotPattern = properties.CurrentState.stuffedToy.shotType.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.shotIndex = 0;
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x0001389F File Offset: 0x00011A9F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x000138BD File Offset: 0x00011ABD
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000138D5 File Offset: 0x00011AD5
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.properties.CurrentHealth > 0f)
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x000138FD File Offset: 0x00011AFD
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.spiralProjectile = null;
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000A0B5C File Offset: 0x0009ED5C
	public IEnumerator intro_cr()
	{
		float t = 0f;
		float time = 1.5f;
		Vector3 end = new Vector3(base.transform.position.x, 0f);
		Vector3 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_memory_monkey_intro");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		base.StartCoroutine(this.check_boundaries_cr());
		base.StartCoroutine(this.pick_angle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x000A0B78 File Offset: 0x0009ED78
	public void FireSingle(float speed)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		this.projectile.Create(this.projectileRoot.transform.position, rotation, speed);
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x000A0BD8 File Offset: 0x0009EDD8
	public void FireSpreadshot()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy stuffedToy = base.properties.CurrentState.stuffedToy;
		for (int i = 0; i < stuffedToy.spreadBullets; i++)
		{
			float floatAt = stuffedToy.spreadAngle.GetFloatAt((float)i / ((float)stuffedToy.spreadBullets - 1f));
			this.projectile.Create(this.projectileRoot.transform.position, floatAt, stuffedToy.spreadSpeed, stuffedToy.musicDeathTimer);
		}
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x000A0C54 File Offset: 0x0009EE54
	public void FireSpiral()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy stuffedToy = base.properties.CurrentState.stuffedToy;
		this.spiralProjectile.Create(this.projectileRoot.transform.position, 0f, stuffedToy.spiralSpeed, stuffedToy.spiralMovementRate, 1);
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x000A0CA8 File Offset: 0x0009EEA8
	public IEnumerator punishment_cr()
	{
		this.timer = 0f;
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		bool speedUp = true;
		this.startedPunishment = true;
		base.animator.SetTrigger("OnNoMatch");
		AudioManager.PlayLoop("dice_palace_memory_monkey_shake");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_shake");
		while (speedUp)
		{
			if (this.speed >= p.punishSpeed)
			{
				speedUp = false;
				break;
			}
			this.speed += p.incrementSpeedBy;
			yield return null;
		}
		this.speed = p.punishSpeed;
		while (this.timer < p.punishTime && this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed)
		{
			this.timer += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		while (this.speed > p.bounceSpeed)
		{
			this.speed -= p.incrementSpeedBy;
			yield return null;
		}
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		this.speed = p.bounceSpeed;
		this.startedPunishment = false;
		this.SFXAllowAnticipation();
		yield return null;
		yield break;
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x000A0CC4 File Offset: 0x0009EEC4
	public IEnumerator pick_angle_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		string[] angleString = p.angleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] countString = p.bounceCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] angleAddString = p.angleAdditionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = Random.Range(0, angleString.Length);
		int maxCountIndex = Random.Range(0, countString.Length);
		int angleAddIndex = Random.Range(0, angleAddString.Length);
		float chosenAngle = 0f;
		float angle = 0f;
		float angleAdd = 0f;
		float t = 0f;
		float dirChangeTime = p.directionChangeDelay;
		Parser.FloatTryParse(angleString[angleIndex], out angle);
		Parser.FloatTryParse(countString[maxCountIndex], out this.maxCount);
		Parser.FloatTryParse(angleAddString[angleAddIndex], out angleAdd);
		this.maxCount = 0f;
		this.currentAngle = angle;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.StartCoroutine(this.move_cr());
		yield return null;
		for (;;)
		{
			if (((float)this.bounceCounter >= this.maxCount && this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed) || this.guessedWrong)
			{
				if (this.guessedWrong)
				{
					if (!this.startedPunishment)
					{
						base.StartCoroutine(this.punishment_cr());
					}
					else
					{
						this.timer = 0f;
					}
					while (this.currentlyColliding)
					{
						yield return new WaitForEndOfFrame();
					}
					this.isMoving = false;
					while (t < dirChangeTime)
					{
						t += CupheadTime.FixedDelta;
						yield return new WaitForFixedUpdate();
					}
					this.isMoving = true;
					while (this.currentlyColliding)
					{
						yield return new WaitForEndOfFrame();
					}
					angleIndex = (angleIndex + 1) % angleString.Length;
					Parser.FloatTryParse(angleString[angleIndex], out angle);
					t = 0f;
				}
				angleAddIndex = (angleAddIndex + 1) % angleAddString.Length;
				maxCountIndex = (maxCountIndex + 1) % countString.Length;
				Parser.FloatTryParse(countString[maxCountIndex], out this.maxCount);
				Parser.FloatTryParse(angleAddString[angleAddIndex], out angleAdd);
				if (!this.guessedWrong)
				{
					chosenAngle = this.currentAngle + angleAdd;
				}
				else
				{
					chosenAngle = angle;
				}
				this.bounceCounter = 0;
				this.guessedWrong = false;
				base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(chosenAngle));
				this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
				this.velocity = base.transform.right;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x00013913 File Offset: 0x00011B13
	public void Open()
	{
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Open;
		base.animator.SetTrigger("OnMatch");
		base.animator.SetBool("OnClosing", false);
		base.StartCoroutine(this.open_cr());
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x000A0CE0 File Offset: 0x0009EEE0
	public IEnumerator open_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		int shot = 0;
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		yield return base.animator.WaitForAnimationToStart(this, "Open_Attack_A", false);
		this.isMoving = false;
		base.GetComponent<DamageReceiver>().enabled = true;
		while (this.currentlyColliding)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, p.directionChangeDelay);
		yield return CupheadTime.WaitForSeconds(this, p.attackAnti);
		this.isMoving = true;
		base.animator.SetTrigger("Continue");
		while (this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Open)
		{
			Parser.IntTryParse(this.shotPattern[this.shotIndex], out shot);
			switch (shot)
			{
			case 1:
				this.FireSingle(p.regularSpeed);
				break;
			case 2:
				this.FireSpreadshot();
				break;
			case 3:
				this.FireSpiral();
				break;
			}
			yield return null;
			this.shotIndex = (this.shotIndex + 1) % this.shotPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.shotDelayRange);
			if (this.state != DicePalaceFlyingMemoryLevelStuffedToy.State.Open)
			{
				break;
			}
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Open_Attack_B", false);
			this.isMoving = false;
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, p.attackAnti);
			base.animator.SetTrigger("Continue");
			this.isMoving = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x0001394A File Offset: 0x00011B4A
	public void Closed()
	{
		base.StartCoroutine(this.closed_cr());
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000A0CFC File Offset: 0x0009EEFC
	public IEnumerator closed_cr()
	{
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed;
		base.animator.SetBool("OnClosing", true);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_Closed", false);
		base.GetComponent<DamageReceiver>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x00013959 File Offset: 0x00011B59
	public void DisableDamageReceiver()
	{
		base.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x00013967 File Offset: 0x00011B67
	public void ChangeLayer(int layer)
	{
		this.hand.GetComponent<SpriteRenderer>().sortingOrder = layer;
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x000A0D18 File Offset: 0x0009EF18
	public IEnumerator move_cr()
	{
		bool soundLooping = true;
		this.isMoving = true;
		this.velocity = base.transform.right;
		this.speed = base.properties.CurrentState.stuffedToy.bounceSpeed;
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement");
		for (;;)
		{
			if (this.isMoving)
			{
				if (!soundLooping)
				{
					AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement");
					this.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement");
					soundLooping = true;
				}
				base.transform.position += base.transform.right * this.speed * CupheadTime.FixedDelta;
			}
			else if (soundLooping)
			{
				AudioManager.Stop("dice_palace_memory_monkey_crane_movement");
				soundLooping = false;
				this.SFXAnticipationActive = false;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x000A0D34 File Offset: 0x0009EF34
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x000A0DA0 File Offset: 0x0009EFA0
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x000A0E0C File Offset: 0x0009F00C
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			if (base.transform.position.x > 0f)
			{
				newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
				this.ChangeDir(newVelocity);
			}
			else
			{
				newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
				this.ChangeDir(newVelocity);
			}
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x000A0EBC File Offset: 0x0009F0BC
	public void ChangeDir(Vector3 newVelocity)
	{
		if (this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed)
		{
			this.bounceCounter++;
		}
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x0001397A File Offset: 0x00011B7A
	public void OnDeath()
	{
		this.StopAllCoroutines();
		AudioManager.PlayLoop("dice_palace_memory_monkey_death");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_death");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x000A0F6C File Offset: 0x0009F16C
	public IEnumerator check_boundaries_cr()
	{
		while (base.transform.position.y <= 720f && base.transform.position.y >= -720f && base.transform.position.x >= -1280f && base.transform.position.x <= 1280f)
		{
			yield return null;
		}
		base.properties.DealDamage(base.properties.CurrentHealth);
		yield break;
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x000139B8 File Offset: 0x00011BB8
	public void AttackSFX()
	{
		AudioManager.Play("dice_palace_memory_monkey_open_attack");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_open_attack");
		this.VOXAngryActive = false;
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x000139DB File Offset: 0x00011BDB
	public void AttackEndSFX()
	{
		AudioManager.Play("dice_palace_memory_monkey_attack_end");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_attack_end");
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x000139F7 File Offset: 0x00011BF7
	public void SFXOpentoClose()
	{
		AudioManager.Play("dice_palace_memory_monkey_open_to_close");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_open_to_close");
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x00013A13 File Offset: 0x00011C13
	public void SFXShake()
	{
		AudioManager.PlayLoop("shake_sound");
		this.emitAudioFromObject.Add("shake_sound");
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x00013A2F File Offset: 0x00011C2F
	public void SFXShakeStop()
	{
		AudioManager.Stop("shake_sound");
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x00013A3B File Offset: 0x00011C3B
	public void SFXVOXAngry()
	{
		if (!this.VOXAngryActive)
		{
			AudioManager.Play("vox_angry");
			this.emitAudioFromObject.Add("vox_angry");
			this.VOXAngryActive = true;
		}
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x00013A69 File Offset: 0x00011C69
	public void SFXVOXAngryAnim()
	{
		AudioManager.Play("vox_angry");
		this.emitAudioFromObject.Add("vox_angry");
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x00013A85 File Offset: 0x00011C85
	public void SFXVOXAnticipation()
	{
		if (!this.SFXAnticipationActive)
		{
			AudioManager.Play("vox_anticipation");
			this.emitAudioFromObject.Add("vox_anticipation");
			this.SFXAnticipationActive = true;
		}
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x00013AB3 File Offset: 0x00011CB3
	public void SFXAllowAnticipation()
	{
		this.SFXAnticipationActive = false;
	}

	// Token: 0x040012BC RID: 4796
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x040012BD RID: 4797
	[SerializeField]
	public DicePalaceFlyingMemoryMusicNote projectile;

	// Token: 0x040012BE RID: 4798
	[SerializeField]
	public DicePalaceFlyingMemoryLevelSpiralProjectile spiralProjectile;

	// Token: 0x040012BF RID: 4799
	[SerializeField]
	public GameObject sprite;

	// Token: 0x040012C0 RID: 4800
	[SerializeField]
	public SpriteRenderer hand;

	// Token: 0x040012C1 RID: 4801
	public bool guessedWrong;

	// Token: 0x040012C2 RID: 4802
	public DicePalaceFlyingMemoryLevelStuffedToy.State state;

	// Token: 0x040012C3 RID: 4803
	public DamageDealer damageDealer;

	// Token: 0x040012C4 RID: 4804
	public DamageReceiver damageReceiver;

	// Token: 0x040012C5 RID: 4805
	public Vector3 velocity;

	// Token: 0x040012C6 RID: 4806
	public int bounceCounter;

	// Token: 0x040012C7 RID: 4807
	public int shotIndex;

	// Token: 0x040012C8 RID: 4808
	public float speed;

	// Token: 0x040012C9 RID: 4809
	public float newAngle;

	// Token: 0x040012CA RID: 4810
	public float currentAngle;

	// Token: 0x040012CB RID: 4811
	public float maxCount;

	// Token: 0x040012CC RID: 4812
	public float timer;

	// Token: 0x040012CD RID: 4813
	public bool isMoving;

	// Token: 0x040012CE RID: 4814
	public bool startedPunishment;

	// Token: 0x040012CF RID: 4815
	public bool currentlyColliding;

	// Token: 0x040012D0 RID: 4816
	public string[] shotPattern;

	// Token: 0x040012D1 RID: 4817
	public bool VOXAngryActive;

	// Token: 0x040012D2 RID: 4818
	public bool SFXAnticipationActive;

	// Token: 0x02000BAF RID: 2991
	public enum State
	{
		// Token: 0x0400554B RID: 21835
		Open,
		// Token: 0x0400554C RID: 21836
		Closed
	}
}
