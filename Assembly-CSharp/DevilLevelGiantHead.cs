using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class DevilLevelGiantHead : LevelProperties.Devil.Entity
{
	// Token: 0x06001467 RID: 5223 RVA: 0x00099F2C File Offset: 0x0009812C
	public override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle");
		base.animator.Play("Idle_Body", 1);
		this.state = DevilLevelGiantHead.State.Intro;
		this.child.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnWinEvent += this.Death;
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x000113AC File Offset: 0x0000F5AC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000113BF File Offset: 0x0000F5BF
	public void StartIntroTransform()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x00099FB8 File Offset: 0x000981B8
	public IEnumerator intro_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.child.GetComponent<Collider2D>().enabled = false;
		this.platformCr = base.StartCoroutine(this.platforms_cr());
		base.StartCoroutine(this.fireballs_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = DevilLevelGiantHead.State.Idle;
		this.waitingForTransform = false;
		yield break;
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000113CE File Offset: 0x0000F5CE
	public void OnNeck()
	{
		base.animator.Play("Idle_Body");
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x000113E0 File Offset: 0x0000F5E0
	public void NoNeck()
	{
		base.animator.Play("Off_Body");
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000113F2 File Offset: 0x0000F5F2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.fireballPrefab = null;
		this.bombPrefab = null;
		this.skullPrefab = null;
		this.swooperPrefab = null;
		this.tearPrefab = null;
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x00099FD4 File Offset: 0x000981D4
	public IEnumerator platforms_cr()
	{
		LevelProperties.Devil.GiantHeadPlatforms p = base.properties.CurrentState.giantHeadPlatforms;
		string[] pattern = p.riseString.Split(new char[]
		{
			','
		});
		int patternIndex = Random.Range(0, pattern.Length);
		for (;;)
		{
			while (this.waitingForTransform)
			{
				yield return null;
			}
			p = base.properties.CurrentState.giantHeadPlatforms;
			patternIndex = (patternIndex + 1) % pattern.Length;
			int platformIndex;
			Parser.IntTryParse(pattern[patternIndex], out platformIndex);
			DevilLevelPlatform platform = this.raisablePlatforms[platformIndex - 1];
			if (platform.state != DevilLevelPlatform.State.Idle)
			{
				bool noIdlePlatforms = true;
				while (noIdlePlatforms)
				{
					foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
					{
						if (devilLevelPlatform.state == DevilLevelPlatform.State.Idle)
						{
							noIdlePlatforms = false;
						}
					}
					if (noIdlePlatforms)
					{
						yield return CupheadTime.WaitForSeconds(this, p.riseDelayRange.RandomFloat());
					}
				}
			}
			else
			{
				platform.Raise(p.riseSpeed, p.maxHeight, p.holdDelay);
				yield return CupheadTime.WaitForSeconds(this, p.riseDelayRange.RandomFloat());
			}
		}
		yield break;
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x00099FF0 File Offset: 0x000981F0
	public IEnumerator fireballs_cr()
	{
		bool fromRight = Rand.Bool();
		int index = (!fromRight) ? 0 : (this.raisablePlatforms.Length - 1);
		LevelProperties.Devil.Fireballs p = base.properties.CurrentState.fireballs;
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		for (;;)
		{
			p = base.properties.CurrentState.fireballs;
			DevilLevelPlatform platform = this.raisablePlatforms[index];
			index = (((!fromRight) ? (index + 1) : (index - 1)) + this.raisablePlatforms.Length) % this.raisablePlatforms.Length;
			if (platform.state == DevilLevelPlatform.State.Dead)
			{
				yield return null;
			}
			else
			{
				this.fireballPrefab.Create(platform.transform.position.x, p.fallSpeed, p.fallAcceleration, p.size / 200f);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
		}
		yield break;
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x0001141D File Offset: 0x0000F61D
	public void StartBombEye()
	{
		this.state = DevilLevelGiantHead.State.BombEye;
		base.StartCoroutine(this.eye_cr(base.properties.CurrentState.bombEye.hesitate.RandomFloat()));
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x0001144D File Offset: 0x0000F64D
	public void StartSkullEye()
	{
		this.state = DevilLevelGiantHead.State.SkullEye;
		base.StartCoroutine(this.eye_cr(base.properties.CurrentState.skullEye.hesitate.RandomFloat()));
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x0009A00C File Offset: 0x0009820C
	public IEnumerator eye_cr(float hesitateTime)
	{
		if (this.state == DevilLevelGiantHead.State.BombEye)
		{
			this.bombOnLeft = Rand.Bool();
			this.spawnPos = ((!this.bombOnLeft) ? this.rightEyeRoot.position : this.leftEyeRoot.position);
			base.animator.SetTrigger("OnBomb");
			base.animator.SetBool("BombLeft", this.bombOnLeft);
		}
		else
		{
			this.spawnPos = this.middleRoot.transform.position;
			base.animator.SetTrigger("OnSpiral");
		}
		yield return CupheadTime.WaitForSeconds(this, hesitateTime);
		this.state = DevilLevelGiantHead.State.Idle;
		yield break;
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x0001147D File Offset: 0x0000F67D
	public void SpawnBomb()
	{
		this.bombPrefab.Create(this.spawnPos, base.properties.CurrentState.bombEye, this.bombOnLeft);
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x0009A030 File Offset: 0x00098230
	public void Offset()
	{
		if (base.GetComponent<SpriteRenderer>().flipX)
		{
			base.transform.AddPosition(-60f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(60f, 0f, 0f);
		}
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000114A7 File Offset: 0x0000F6A7
	public void SpawnSpiral()
	{
		this.skullPrefab.Create(this.spawnPos, base.properties.CurrentState.skullEye);
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000114CB File Offset: 0x0000F6CB
	public void StartHands()
	{
		base.animator.SetTrigger("OnTransA");
		this.handsCr = base.StartCoroutine(this.hands_cr());
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x0009A088 File Offset: 0x00098288
	public IEnumerator hands_cr()
	{
		this.waitingForTransform = true;
		while (this.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		bool platformsDown = false;
		while (!platformsDown)
		{
			platformsDown = true;
			foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
			{
				if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
				{
					platformsDown = false;
				}
			}
			yield return null;
		}
		this.waitingForTransform = false;
		foreach (DevilLevelPlatform devilLevelPlatform2 in this.HandsPhaseExit)
		{
			devilLevelPlatform2.Lower(base.properties.CurrentState.giantHeadPlatforms.exitSpeed);
		}
		this.StartSwoopers();
		bool leftHandShoot = Rand.Bool();
		this.hands[0].StartPattern(base.properties.CurrentState.hands);
		this.hands[1].StartPattern(base.properties.CurrentState.hands);
		this.handsSpawnCr = base.StartCoroutine(this.spawn_hand_cr());
		for (;;)
		{
			int handIndex = (!leftHandShoot) ? 1 : 0;
			if (this.hands[handIndex] != null)
			{
				this.hands[handIndex].animator.SetTrigger("OnAttack");
			}
			leftHandShoot = !leftHandShoot;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.hands.shotDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x0009A0A4 File Offset: 0x000982A4
	public IEnumerator spawn_hand_cr()
	{
		LevelProperties.Devil.Hands p = base.properties.CurrentState.hands;
		yield return CupheadTime.WaitForSeconds(this, p.initialSpawnDelay.RandomFloat());
		this.hands[0].SpawnIn();
		yield return CupheadTime.WaitForSeconds(this, p.initialSpawnDelay.RandomFloat());
		this.hands[1].SpawnIn();
		while (!this.hands[0].isDead)
		{
			while (!this.hands[0].despawned && !this.hands[1].despawned)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			if (this.hands[0].despawned)
			{
				this.hands[0].SpawnIn();
			}
			else if (this.hands[1].despawned)
			{
				this.hands[1].SpawnIn();
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000114EF File Offset: 0x0000F6EF
	public void StartSwoopers()
	{
		this.swooperSpawnCr = base.StartCoroutine(this.swooper_spawn_cr());
		this.swooperSwoopCr = base.StartCoroutine(this.swooper_swoop_cr());
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x0009A0C0 File Offset: 0x000982C0
	public IEnumerator swooper_spawn_cr()
	{
		LevelProperties.Devil.Swoopers p = base.properties.CurrentState.swoopers;
		string[] swooperSlotPositions = p.positions.Split(new char[]
		{
			','
		});
		this.swoopers = new List<DevilLevelSwooper>();
		this.swooperSlots = new DevilLevelGiantHead.SwooperSlot[swooperSlotPositions.Length];
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			float num = 0f;
			Parser.FloatTryParse(swooperSlotPositions[i], out num);
			this.swooperSlots[i] = new DevilLevelGiantHead.SwooperSlot(num - 600f);
		}
		int swooperSlotIndex = Random.Range(0, this.swooperSlots.Length);
		float delay = p.initialSpawnDelay.RandomFloat();
		int spawnPoint = Random.Range(0, this.spawnPoints.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			delay = p.spawnDelay.RandomFloat();
			if (this.swoopers.Count < p.maxCount)
			{
				int numToSpawn = p.spawnCount.RandomInt();
				int numSpawned = 0;
				base.animator.SetBool("IsWhincing", true);
				while (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Whince"))
				{
					yield return null;
				}
				while (numSpawned < numToSpawn && this.swoopers.Count < p.maxCount)
				{
					swooperSlotIndex = (swooperSlotIndex + 1) % this.swooperSlots.Length;
					DevilLevelGiantHead.SwooperSlot slot = this.swooperSlots[swooperSlotIndex];
					if (slot.swooper == null)
					{
						DevilLevelSwooper devilLevelSwooper = this.swooperPrefab.Create(this, p, this.spawnPoints[spawnPoint].position, slot.xPos);
						slot.swooper = devilLevelSwooper;
						this.swoopers.Add(devilLevelSwooper);
						numSpawned++;
					}
					spawnPoint = (spawnPoint + 1) % this.spawnPoints.Length;
					yield return CupheadTime.WaitForSeconds(this, 0.4f);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
			base.animator.SetBool("IsWhincing", false);
		}
		yield break;
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x0009A0DC File Offset: 0x000982DC
	public IEnumerator swooper_swoop_cr()
	{
		LevelProperties.Devil.Swoopers p = base.properties.CurrentState.swoopers;
		for (;;)
		{
			while (this.swoopers.Count == 0)
			{
				yield return null;
			}
			List<DevilLevelSwooper> attackSwoopers = new List<DevilLevelSwooper>(this.swoopers);
			attackSwoopers.Shuffle<DevilLevelSwooper>();
			yield return CupheadTime.WaitForSeconds(this, p.attackDelay.RandomFloat());
			foreach (DevilLevelSwooper swooper in attackSwoopers)
			{
				if (swooper != null && swooper.state == DevilLevelSwooper.State.Idle)
				{
					swooper.Swoop();
					if (swooper == attackSwoopers[attackSwoopers.Count - 1])
					{
						swooper.finalSwooping = true;
					}
					this.RemoveSwooperFromSlot(swooper);
					if (swooper != attackSwoopers[attackSwoopers.Count - 1])
					{
						yield return CupheadTime.WaitForSeconds(this, p.attackDelay.RandomFloat());
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x00011515 File Offset: 0x0000F715
	public void OnSwooperDeath(DevilLevelSwooper swooper)
	{
		this.swoopers.Remove(swooper);
		this.RemoveSwooperFromSlot(swooper);
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x0009A0F8 File Offset: 0x000982F8
	public void RemoveSwooperFromSlot(DevilLevelSwooper swooper)
	{
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			if (this.swooperSlots[i].swooper == swooper)
			{
				this.swooperSlots[i].swooper = null;
			}
		}
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0009A14C File Offset: 0x0009834C
	public float PutSwooperInSlot(DevilLevelSwooper swooper)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			if (!(this.swooperSlots[i].swooper != null))
			{
				float num3 = Mathf.Abs(this.swooperSlots[i].xPos - swooper.transform.position.x);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
		}
		this.swooperSlots[num2].swooper = swooper;
		return this.swooperSlots[num2].xPos;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0001152B File Offset: 0x0000F72B
	public void StartTears()
	{
		base.StartCoroutine(this.tears_cr());
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0009A1F8 File Offset: 0x000983F8
	public IEnumerator tears_cr()
	{
		base.animator.SetTrigger("OnTransB");
		this.waitingForTransform = true;
		while (this.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		bool platformsDown = false;
		while (!platformsDown)
		{
			platformsDown = true;
			foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
			{
				if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
				{
					platformsDown = false;
				}
			}
			yield return null;
		}
		this.waitingForTransform = false;
		foreach (DevilLevelPlatform devilLevelPlatform2 in this.TearsPhaseExit)
		{
			devilLevelPlatform2.Lower(base.properties.CurrentState.giantHeadPlatforms.exitSpeed);
		}
		if (!base.properties.CurrentState.giantHeadPlatforms.riseDuringTearPhase)
		{
			base.StopCoroutine(this.platformCr);
		}
		base.StopCoroutine(this.handsCr);
		base.StopCoroutine(this.handsSpawnCr);
		base.StopCoroutine(this.swooperSpawnCr);
		base.StopCoroutine(this.swooperSwoopCr);
		while (this.swoopers.Count > 0)
		{
			this.swoopers[0].Die();
		}
		foreach (DevilLevelHand devilLevelHand in this.hands)
		{
			devilLevelHand.isDead = true;
			devilLevelHand.Die();
		}
		bool spawnLeft = true;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			this.tearPrefab.CreateTear((!spawnLeft) ? this.rightTearRoot.transform.position : this.leftTearRoot.transform.position, base.properties.CurrentState.tears.speed);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tears.delay);
			spawnLeft = !spawnLeft;
		}
		yield break;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x0001153A File Offset: 0x0000F73A
	public void Death()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDead");
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x0001155E File Offset: 0x0000F75E
	public void sfx_p3_bomb_appear()
	{
		AudioManager.Play("p3_bomb_appear");
		this.emitAudioFromObject.Add("p3_bomb_appear");
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x0001157A File Offset: 0x0000F77A
	public void sfx_p3_bomb_attack()
	{
		AudioManager.Play("p3_bomb_attack");
		this.emitAudioFromObject.Add("p3_bomb_attack");
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x00011596 File Offset: 0x0000F796
	public void sfx_p3_cry_idle()
	{
		AudioManager.Play("p3_cry_idle");
		this.emitAudioFromObject.Add("p3_cry_idle");
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000115B2 File Offset: 0x0000F7B2
	public void sfx_p3_dead_loop()
	{
		if (!this.DeadLoopSFXActive)
		{
			AudioManager.PlayLoop("p3_dead_loop");
			this.emitAudioFromObject.Add("p3_dead_loop");
			this.DeadLoopSFXActive = true;
		}
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000115E0 File Offset: 0x0000F7E0
	public void sfx_p3_dead_loop_stop()
	{
		AudioManager.Stop("p3_dead_loop");
		this.DeadLoopSFXActive = false;
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000115F3 File Offset: 0x0000F7F3
	public void sfx_p3_hand_release_start()
	{
		AudioManager.Play("p3_hand_release_start");
		this.emitAudioFromObject.Add("p3_hand_release_start");
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x0001160F File Offset: 0x0000F80F
	public void sfx_p3_hurt_trans_a()
	{
		AudioManager.Play("p3_hurt_trans_a");
		this.emitAudioFromObject.Add("p3_hurt_trans_a");
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x0001162B File Offset: 0x0000F82B
	public void sfx_p3_spiral_attack()
	{
		AudioManager.Play("p3_spiral_attack");
		this.emitAudioFromObject.Add("p3_spiral_attack");
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00011647 File Offset: 0x0000F847
	public void sfx_p3_intro_end()
	{
		AudioManager.Play("p3_intro_end");
		this.emitAudioFromObject.Add("p3_intro_end");
	}

	// Token: 0x040010AD RID: 4269
	public DevilLevelGiantHead.State state;

	// Token: 0x040010AE RID: 4270
	[SerializeField]
	public GameObject[] groundPieces;

	// Token: 0x040010AF RID: 4271
	[SerializeField]
	public DevilLevelPlatform[] HandsPhaseExit;

	// Token: 0x040010B0 RID: 4272
	[SerializeField]
	public DevilLevelPlatform[] TearsPhaseExit;

	// Token: 0x040010B1 RID: 4273
	[SerializeField]
	public DevilLevelPlatform[] raisablePlatforms;

	// Token: 0x040010B2 RID: 4274
	[SerializeField]
	public Transform stage3Platforms;

	// Token: 0x040010B3 RID: 4275
	[SerializeField]
	public DevilLevelFireball fireballPrefab;

	// Token: 0x040010B4 RID: 4276
	[SerializeField]
	public DevilLevelBomb bombPrefab;

	// Token: 0x040010B5 RID: 4277
	[SerializeField]
	public DevilLevelSkull skullPrefab;

	// Token: 0x040010B6 RID: 4278
	[SerializeField]
	public Transform leftEyeRoot;

	// Token: 0x040010B7 RID: 4279
	[SerializeField]
	public Transform rightEyeRoot;

	// Token: 0x040010B8 RID: 4280
	[SerializeField]
	public Transform middleRoot;

	// Token: 0x040010B9 RID: 4281
	[SerializeField]
	public Transform leftTearRoot;

	// Token: 0x040010BA RID: 4282
	[SerializeField]
	public Transform rightTearRoot;

	// Token: 0x040010BB RID: 4283
	[SerializeField]
	public DevilLevelHand[] hands;

	// Token: 0x040010BC RID: 4284
	[SerializeField]
	public DevilLevelSwooper swooperPrefab;

	// Token: 0x040010BD RID: 4285
	[SerializeField]
	public DevilLevelTear tearPrefab;

	// Token: 0x040010BE RID: 4286
	[SerializeField]
	public SpriteRenderer bottomSprite;

	// Token: 0x040010BF RID: 4287
	[SerializeField]
	public DamageReceiver child;

	// Token: 0x040010C0 RID: 4288
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x040010C1 RID: 4289
	public bool waitingForTransform;

	// Token: 0x040010C2 RID: 4290
	public bool bombOnLeft;

	// Token: 0x040010C3 RID: 4291
	public bool DeadLoopSFXActive;

	// Token: 0x040010C4 RID: 4292
	public DamageReceiver damageReceiver;

	// Token: 0x040010C5 RID: 4293
	public Coroutine platformCr;

	// Token: 0x040010C6 RID: 4294
	public Coroutine handsCr;

	// Token: 0x040010C7 RID: 4295
	public Coroutine handsSpawnCr;

	// Token: 0x040010C8 RID: 4296
	public Coroutine swooperSpawnCr;

	// Token: 0x040010C9 RID: 4297
	public Coroutine swooperSwoopCr;

	// Token: 0x040010CA RID: 4298
	public Vector2 spawnPos;

	// Token: 0x040010CB RID: 4299
	public Color color;

	// Token: 0x040010CC RID: 4300
	public DevilLevelGiantHead.SwooperSlot[] swooperSlots;

	// Token: 0x040010CD RID: 4301
	public List<DevilLevelSwooper> swoopers;

	// Token: 0x02000B28 RID: 2856
	public enum State
	{
		// Token: 0x040051C8 RID: 20936
		Intro,
		// Token: 0x040051C9 RID: 20937
		Idle,
		// Token: 0x040051CA RID: 20938
		BombEye,
		// Token: 0x040051CB RID: 20939
		SkullEye
	}

	// Token: 0x02000B29 RID: 2857
	public struct SwooperSlot
	{
		// Token: 0x06005DDE RID: 24030 RVA: 0x00044A36 File Offset: 0x00042C36
		public SwooperSlot(float xPos)
		{
			this.xPos = xPos;
			this.swooper = null;
		}

		// Token: 0x040051CC RID: 20940
		public float xPos;

		// Token: 0x040051CD RID: 20941
		public DevilLevelSwooper swooper;
	}
}
