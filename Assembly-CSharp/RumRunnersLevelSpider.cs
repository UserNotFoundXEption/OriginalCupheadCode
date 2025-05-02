using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class RumRunnersLevelSpider : LevelProperties.RumRunners.Entity
{
	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06002544 RID: 9540 RVA: 0x000C5F4C File Offset: 0x000C414C
	// (remove) Token: 0x06002545 RID: 9541 RVA: 0x000C5F84 File Offset: 0x000C4184
	public event Action OnDeathEvent;

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06002546 RID: 9542 RVA: 0x0001F721 File Offset: 0x0001D921
	// (set) Token: 0x06002547 RID: 9543 RVA: 0x0001F729 File Offset: 0x0001D929
	public bool goingLeft { get; set; }

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06002548 RID: 9544 RVA: 0x0001F732 File Offset: 0x0001D932
	// (set) Token: 0x06002549 RID: 9545 RVA: 0x0001F73A File Offset: 0x0001D93A
	public bool moving { get; set; }

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x0600254A RID: 9546 RVA: 0x0001F743 File Offset: 0x0001D943
	public float dir
	{
		get
		{
			return (float)((!this.goingLeft) ? 1 : -1);
		}
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000C5FBC File Offset: 0x000C41BC
	public void Start()
	{
		this.goingLeft = true;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collider = base.GetComponent<Collider2D>();
		this.scaleX = base.transform.localScale.x;
		base.transform.SetScale(new float?(this.scaleX * this.dir), null, null);
		this.SetUpMinePositions();
		this.grubEnterVariant = Random.Range(0, 3);
		this.grubVariant = Random.Range(0, 4);
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000C6074 File Offset: 0x000C4274
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		this.mineMainIndex = Random.Range(0, properties.CurrentState.mine.minePlacementString.Length);
		this.mineIndex = Random.Range(0, properties.CurrentState.mine.minePlacementString[this.mineMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.bouncingPattern = new PatternString(properties.CurrentState.bouncing.shootBeetleAngleString, true, true);
		this.grubDelayString = new PatternString(properties.CurrentState.grubs.delayString, true, true);
		this.grubPositionString = new PatternString(properties.CurrentState.grubs.appearPositionString, true, false);
		Level.Current.OnLevelStartEvent += this.OnIntroEnd;
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000C6148 File Offset: 0x000C4348
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float x = base.transform.position.x;
		float num = (float)(-(float)Level.Current.Width) * 0.5f + this.deathInvincibilityBuffer;
		float num2 = (float)Level.Current.Width * 0.5f - this.deathInvincibilityBuffer;
		float num3 = base.properties.CurrentHealth - base.properties.GetNextStateHealthTrigger() * base.properties.TotalHealth;
		if (info.damage > num3 && (x > num2 || x < num))
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x0001F758 File Offset: 0x0001D958
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x0001F780 File Offset: 0x0001D980
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000C61F0 File Offset: 0x000C43F0
	public void OnIntroEnd()
	{
		this.policeman.SetProperties(base.properties.CurrentState.spider, this);
		Level.Current.OnLevelStartEvent -= this.OnIntroEnd;
		base.StartCoroutine(this.introExit());
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000C623C File Offset: 0x000C443C
	public IEnumerator introExit()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		base.animator.SetTrigger("IntroExit");
		yield return base.animator.WaitForAnimationToEnd(this, "IntroExit", false, true);
		base.StartCoroutine(this.run_cr());
		yield break;
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x0001F798 File Offset: 0x0001D998
	public void SummonSelection()
	{
		base.StartCoroutine(this.check_to_start_summon_cr());
	}

	// Token: 0x06002553 RID: 9555 RVA: 0x000C6258 File Offset: 0x000C4458
	public IEnumerator check_to_start_summon_cr()
	{
		RumRunnersLevelSpider.SummonType summonType = this.summonType;
		if (summonType != RumRunnersLevelSpider.SummonType.Bouncing)
		{
			if (summonType != RumRunnersLevelSpider.SummonType.Mine)
			{
				if (summonType == RumRunnersLevelSpider.SummonType.Grubs)
				{
					base.animator.Play("GrubSummonWait");
					yield return null;
					this.isSummoning = true;
					this.StartGrubs();
					yield return base.animator.WaitForAnimationToEnd(this, "GrubSummonWait", false, true);
					this.isSummoning = false;
				}
			}
			else
			{
				base.animator.Play("MineSummon");
				yield return null;
				this.isSummoning = true;
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
				{
					yield return null;
				}
				this.StartMine();
				yield return base.animator.WaitForAnimationToEnd(this, "MineSummon", false, true);
				this.isSummoning = false;
			}
		}
		else
		{
			base.animator.Play("Kick");
			yield return null;
			this.isSummoning = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Kick", false, true);
			this.isSummoning = false;
		}
		yield break;
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x0001F7A7 File Offset: 0x0001D9A7
	public void animationEvent_SpawnFrontPuffEffect()
	{
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
	public void animationEvent_SpawnBackPuffEffect()
	{
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000C6274 File Offset: 0x000C4474
	public IEnumerator run_cr()
	{
		LevelProperties.RumRunners.Spider p = base.properties.CurrentState.spider;
		bool hasSummoned = false;
		bool spawnedCop = false;
		this.moving = false;
		PatternString copPositionString = new PatternString(p.copPositionString, true, true);
		PatternString copBulletTypeString = new PatternString(p.copBulletTypeString, true, true);
		PatternString spiderPositionString = new PatternString(p.spiderPositionString, true, true);
		PatternString spiderActionString = new PatternString(p.spiderActionString, true, true);
		PatternString spiderActionPositionString = new PatternString(p.spiderActionPositionString, true);
		YieldInstruction wait = new WaitForFixedUpdate();
		this.copSpawnPos = p.copSpawnSpiderDist;
		bool isInitial = true;
		for (;;)
		{
			char summonChar;
			if (isInitial)
			{
				summonChar = ((!Rand.Bool()) ? 'M' : 'N');
				spawnedCop = true;
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, p.spiderEnterDelay);
				summonChar = spiderActionString.PopLetter();
			}
			int spawnPointIndex = spiderPositionString.PopInt();
			float popOutX = (float)((!this.goingLeft) ? -640 : 640);
			float summonPos = Mathf.Lerp(popOutX, -popOutX, spiderActionPositionString.PopFloat());
			if (summonChar != 'B')
			{
				if (summonChar != 'G')
				{
					if (summonChar != 'M')
					{
						this.summonType = RumRunnersLevelSpider.SummonType.None;
						hasSummoned = true;
					}
					else
					{
						this.summonType = RumRunnersLevelSpider.SummonType.Mine;
					}
				}
				else
				{
					this.summonType = RumRunnersLevelSpider.SummonType.Grubs;
				}
			}
			else
			{
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.05f);
				this.summonType = RumRunnersLevelSpider.SummonType.Bouncing;
				if (spawnPointIndex == 2)
				{
					spawnPointIndex = ((!Rand.Bool()) ? 1 : 0);
				}
			}
			if (!isInitial)
			{
				base.transform.position = new Vector3(((float)Level.Current.Right + 350f) * -this.dir, this.spawnPoints[spawnPointIndex].position.y);
			}
			if (isInitial && this.summonType == RumRunnersLevelSpider.SummonType.Mine)
			{
				hasSummoned = false;
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.75f);
			}
			float timeToSummon = Mathf.Abs(base.transform.position.x - summonPos) / p.spiderSpeed;
			float animatorStartTime = 1f - timeToSummon / this.runClip.length;
			int copPos = copPositionString.PopInt();
			this.nextCopPosition = new Vector3(this.spawnPoints[copPos].position.x * this.dir, this.spawnPoints[copPos].position.y);
			base.transform.SetScale(new float?(this.scaleX * this.dir), null, null);
			if (!isInitial)
			{
				string text = "Run";
				if (this.summonType == RumRunnersLevelSpider.SummonType.Grubs)
				{
					text = "GrubSummonEnter";
				}
				else if (this.summonType == RumRunnersLevelSpider.SummonType.Bouncing)
				{
					text = "RunCaterpillar";
				}
				base.animator.Play(text, 0, animatorStartTime);
			}
			bool isPink = copBulletTypeString.PopLetter() == 'P';
			this.moving = true;
			if (this.summonType == RumRunnersLevelSpider.SummonType.Grubs)
			{
				this.SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice();
			}
			while ((this.goingLeft && base.transform.position.x > popOutX) || (!this.goingLeft && base.transform.position.x < popOutX))
			{
				base.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * this.dir;
				base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
				yield return wait;
			}
			while (this.moving)
			{
				if (!hasSummoned && ((this.goingLeft && base.transform.position.x <= summonPos) || (!this.goingLeft && base.transform.position.x >= summonPos)))
				{
					this.SummonSelection();
					hasSummoned = true;
				}
				while (this.isSummoning)
				{
					yield return null;
				}
				if ((!this.goingLeft && (float)Level.Current.Right + 350f > base.transform.position.x) || (this.goingLeft && (float)Level.Current.Left - 350f < base.transform.position.x))
				{
					float copSpawnDistanceRemaining = this.copSpawnPos - this.dir * base.transform.position.x;
					if (!spawnedCop && copSpawnDistanceRemaining < 0f)
					{
						this.policeman.CopAppear(this.nextCopPosition, isPink, this.goingLeft);
						spawnedCop = true;
						this.nextCopPosition = Vector3.up * 5000f;
					}
					base.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * this.dir;
					base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
					yield return wait;
				}
				else
				{
					this.moving = false;
				}
			}
			hasSummoned = false;
			spawnedCop = false;
			this.goingLeft = !this.goingLeft;
			isInitial = false;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x000C6290 File Offset: 0x000C4490
	public bool GrubCanEnter(Vector3 pos, float enterTime)
	{
		if (!this.moving)
		{
			return false;
		}
		if (Mathf.Abs(base.transform.position.y - pos.y) < 100f && (Mathf.Abs(base.transform.position.x + base.properties.CurrentState.spider.spiderSpeed * this.dir * enterTime - pos.x) < 500f || Mathf.Abs(base.transform.position.x - pos.x) < 500f))
		{
			return false;
		}
		if (Mathf.Abs(base.transform.position.x) > 400f)
		{
			if (this.policeman.isActive && Mathf.Abs(this.policeman.transform.position.y - pos.y) < 100f && Mathf.Sign(this.policeman.transform.position.x) == Mathf.Sign(pos.x))
			{
				return false;
			}
			if (Mathf.Abs(this.nextCopPosition.y - pos.y) < 100f && Mathf.Sign(this.nextCopPosition.x) == Mathf.Sign(pos.x))
			{
				return false;
			}
		}
		this.grubList.RemoveAll((RumRunnersLevelGrub g) => g == null);
		for (int i = 0; i < this.grubList.Count; i++)
		{
			if (this.grubList[i].startedEntering && Mathf.Abs(this.grubList[i].transform.position.y - pos.y) < 100f)
			{
				bool flag = !this.grubList[i].moving;
				if (Mathf.Abs((this.grubList[i].transform.position + Vector3.right * this.grubList[i].speed * (enterTime - this.grubList[i].GetTimeToMove())).x - pos.x) < 200f)
				{
					return false;
				}
				if (flag && Mathf.Abs((this.grubList[i].transform.position + Vector3.left * this.grubList[i].speed * (enterTime - this.grubList[i].GetTimeToMove())).x - pos.x) < 200f)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x0001F7AB File Offset: 0x0001D9AB
	public void StartGrubs()
	{
		base.StartCoroutine(this.grubs_cr());
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x000C65C0 File Offset: 0x000C47C0
	public IEnumerator grubs_cr()
	{
		LevelProperties.RumRunners.Grubs p = base.properties.CurrentState.grubs;
		float delay = 0f;
		int y = 0;
		int x = 0;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.grubs.grubSummonWarning);
		int count = this.grubPositionString.SubStringLength();
		this.grubPositionString.SetSubStringIndex(count);
		for (int i = 0; i < count; i++)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			int appearPosition = this.grubPositionString.PopInt();
			x = appearPosition % 6;
			y = appearPosition / 6;
			if (x != 5 || y != 2)
			{
				this.grubList.RemoveAll((RumRunnersLevelGrub g) => g == null);
				bool canSpawn = true;
				for (int j = 0; j < this.grubList.Count; j++)
				{
					if (!this.grubList[j].startedEntering && this.grubList[j].x == x && this.grubList[j].y == y)
					{
						canSpawn = false;
					}
				}
				if (canSpawn)
				{
					this.grubList.Add(this.grubPrefab.Create(this.grubPaths[y * 6 + x], 0f, p.movementSpeed, p.warningDuration, p.hp, this, this.grubEnterVariant, this.grubVariant, count - i, x, y));
					this.grubEnterVariant = (this.grubEnterVariant + 1) % 3;
					this.grubVariant = (this.grubVariant + 1) % 4;
					if (i < count - 1)
					{
						delay = this.grubDelayString.PopFloat();
						yield return CupheadTime.WaitForSeconds(this, delay);
					}
				}
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000C65DC File Offset: 0x000C47DC
	public void SetUpMinePositions()
	{
		this.minePositions = new Vector3[5, 3];
		this.minePositions[0, 0] = new Vector3(-553f, 354f);
		this.minePositions[1, 0] = new Vector3(-282f, 321f);
		this.minePositions[2, 0] = new Vector3(16f, 354f);
		this.minePositions[3, 0] = new Vector3(311f, 313f);
		this.minePositions[4, 0] = new Vector3(545f, 343f);
		this.minePositions[0, 1] = new Vector3(-492f, 33f);
		this.minePositions[1, 1] = new Vector3(-247f, 19f);
		this.minePositions[2, 1] = new Vector3(42f, 35f);
		this.minePositions[3, 1] = new Vector3(287f, 7f);
		this.minePositions[4, 1] = new Vector3(509f, 36f);
		this.minePositions[0, 2] = new Vector3(-524f, -284f);
		this.minePositions[1, 2] = new Vector3(-224f, -265f);
		this.minePositions[2, 2] = new Vector3(-17f, -294f);
		this.minePositions[3, 2] = new Vector3(253f, -252f);
		this.minePositions[4, 2] = new Vector3(575f, -291f);
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x0001F7BA File Offset: 0x0001D9BA
	public void StartMine()
	{
		base.StartCoroutine(this.mine_cr());
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x000C67E8 File Offset: 0x000C49E8
	public IEnumerator mine_cr()
	{
		LevelProperties.RumRunners.Mine p = base.properties.CurrentState.mine;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		Vector2 pos = Vector2.zero;
		string[] minePlacementString = p.minePlacementString[this.mineMainIndex].Split(new char[]
		{
			','
		});
		this.mineList.RemoveAll((RumRunnersLevelMine m) => m == null);
		this.mineIndex = 0;
		int i = 0;
		while ((float)i < Mathf.Min(p.mineNumber, (float)minePlacementString.Length))
		{
			pos = this.GetMinePos(Parser.IntParse(minePlacementString[this.mineIndex]));
			bool foundFreeSpot = false;
			int checkedPositionsCount = 0;
			while (!foundFreeSpot && checkedPositionsCount < 15)
			{
				float distP = 1000f;
				float distP2 = 1000f;
				bool spotOccupied = false;
				for (int j = 0; j < this.mineList.Count; j++)
				{
					if (this.mineList[j].xPos == (int)pos.x && this.mineList[j].yPos == (int)pos.y)
					{
						spotOccupied = true;
					}
				}
				if (!spotOccupied)
				{
					if (!player.IsDead)
					{
						distP = Vector3.Distance(player.transform.position, this.minePositions[(int)pos.x, (int)pos.y]);
					}
					if (player2 != null && !player2.IsDead)
					{
						distP2 = Vector3.Distance(player2.transform.position, this.minePositions[(int)pos.x, (int)pos.y]);
					}
				}
				if (distP > p.mineCheckToLand && distP2 > p.mineCheckToLand && !spotOccupied)
				{
					foundFreeSpot = true;
					break;
				}
				if (this.mineIndex < minePlacementString.Length - 1)
				{
					this.mineIndex++;
				}
				else
				{
					this.mineMainIndex = (this.mineMainIndex + 1) % p.minePlacementString.Length;
					this.mineIndex = 0;
				}
				pos = this.GetMinePos(Parser.IntParse(minePlacementString[this.mineIndex]));
				checkedPositionsCount++;
				yield return null;
			}
			if (checkedPositionsCount < 15)
			{
				RumRunnersLevelMine rumRunnersLevelMine = this.minePrefab.Spawn<RumRunnersLevelMine>();
				this.mineList.Add(rumRunnersLevelMine.Init(this.minePositions[(int)pos.x, (int)pos.y], p, this, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
			}
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
			if (this.mineIndex < minePlacementString.Length - 1)
			{
				this.mineIndex++;
			}
			else
			{
				this.mineMainIndex = (this.mineMainIndex + 1) % p.minePlacementString.Length;
				this.mineIndex = 0;
			}
			i++;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x000C6804 File Offset: 0x000C4A04
	public Vector2 GetMinePos(int mineNum)
	{
		Vector2 zero = Vector2.zero;
		zero.x = (float)(mineNum % 5);
		zero.y = (float)(mineNum / 5);
		if (zero.x > 4f || zero.x < 0f || zero.y > 2f || zero.y < 0f)
		{
			Debug.Break();
		}
		return zero;
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x000C6878 File Offset: 0x000C4A78
	public void animationEvent_StartBouncing()
	{
		LevelProperties.RumRunners.Bouncing bouncing = base.properties.CurrentState.bouncing;
		do
		{
			this.beetleList.RemoveAll((RumRunnersLevelBouncingBeetle b) => b == null || b.leaveScreen);
			if (this.beetleList.Count >= bouncing.maxBeetleCount)
			{
				this.beetleList[0].leaveScreen = true;
			}
		}
		while (this.beetleList.Count >= bouncing.maxBeetleCount);
		float num = this.bouncingPattern.PopFloat();
		num = Mathf.Clamp(num, 10f, 80f);
		if (this.dir < 0f)
		{
			num = 180f - num;
		}
		Vector3 vector = MathUtils.AngleToDirection(num);
		RumRunnersLevelBouncingBeetle rumRunnersLevelBouncingBeetle = this.caterpillarPrefab.Spawn<RumRunnersLevelBouncingBeetle>();
		rumRunnersLevelBouncingBeetle.Init(this.caterpillarSpawnPoint.position + vector * 110f, vector, bouncing.shootBeetleInitialSpeed, (float)bouncing.shootBeetleTimeToSlowdown, bouncing.shootBeetleSpeed, bouncing.shootBeetleHealth);
		this.beetleList.Add(rumRunnersLevelBouncingBeetle);
		this.kickFXEffect.Create(this.kickFXSpawnPoint.position + vector * 110f * 0.8f);
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000C69C8 File Offset: 0x000C4BC8
	public void Die()
	{
		base.animator.SetTrigger("Dead");
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Effects.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.StopAllCoroutines();
		this.deathExplodeEffect.Create(base.transform.position);
		foreach (RumRunnersLevelBouncingBeetle rumRunnersLevelBouncingBeetle in this.beetleList)
		{
			rumRunnersLevelBouncingBeetle.leaveScreen = true;
		}
		this.mineList.RemoveAll((RumRunnersLevelMine m) => m == null);
		this.mineList.Sort((RumRunnersLevelMine m1, RumRunnersLevelMine m2) => m1.endPhaseExplodePriority.CompareTo(m2.endPhaseExplodePriority));
		for (int i = 0; i < this.mineList.Count; i++)
		{
			this.mineList[i].SetTimer((float)i * 0.6f);
		}
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.SFX_RUMRUN_ExitPhase1_SpiderFalling();
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000C6B1C File Offset: 0x000C4D1C
	public void AniEvent_ChangeToForeground()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Foreground.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 100;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x0001F7C9 File Offset: 0x0001D9C9
	public void animationEvent_ShakeScreen()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.6f, false);
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
	public void AniEvent_DeathComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x000C6B50 File Offset: 0x000C4D50
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Level.Current)
		{
			float num = this.copSpawnPos * this.dir;
			Vector3 vector;
			vector..ctor(num, -360f);
			Vector3 vector2;
			vector2..ctor(num, 360f);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(vector, vector2);
		}
		Gizmos.DrawWireSphere(this.caterpillarSpawnPoint.position, 110f);
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x0001F7ED File Offset: 0x0001D9ED
	public void AnimationEvent_SFX_RUMRUN_Mine_SpiderButtonPress()
	{
		AudioManager.Play("sfx_dlc_rumrun_mine_spiderbuttonpress");
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x0001F7F9 File Offset: 0x0001D9F9
	public void AnimationEvent_SFX_RUMRUN_Spider_GrubSummon_Phone()
	{
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phone");
		AudioManager.Stop("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x0001F80F File Offset: 0x0001DA0F
	public void SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice()
	{
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x0001F82B File Offset: 0x0001DA2B
	public void AnimationEvent_SFX_RUMRUN_CaterpillarBall_SpiderKick()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_spiderkick");
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x0001F837 File Offset: 0x0001DA37
	public void SFX_RUMRUN_ExitPhase1_SpiderFalling()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling");
		AudioManager.FadeSFXVolume("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling", 1f, 10f);
	}

	// Token: 0x04001ED1 RID: 7889
	public const float INTRO_EXIT_DELAY = 0.2f;

	// Token: 0x04001ED2 RID: 7890
	public const float EDGE_OFFSET = 350f;

	// Token: 0x04001ED3 RID: 7891
	public const float MINE_DELAY = 0.2f;

	// Token: 0x04001ED4 RID: 7892
	public const float MINE_EXPLODE_INTERVAL_ON_PHASE_END = 0.6f;

	// Token: 0x04001ED5 RID: 7893
	public const float GRUB_MIN_SPIDER_DISTANCE_TO_ENTER = 500f;

	// Token: 0x04001ED6 RID: 7894
	public const float GRUB_MIN_OTHER_GRUB_DISTANCE_TO_ENTER = 200f;

	// Token: 0x04001ED7 RID: 7895
	public const float BOUNCER_MIN_ANGLE = 10f;

	// Token: 0x04001ED8 RID: 7896
	public const float BOUNCER_MAX_ANGLE = 80f;

	// Token: 0x04001ED9 RID: 7897
	public const float KICK_SPAWN_RADIUS = 110f;

	// Token: 0x04001EDA RID: 7898
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04001EDB RID: 7899
	[SerializeField]
	public AnimationClip runClip;

	// Token: 0x04001EDC RID: 7900
	[SerializeField]
	public RumRunnersLevelPoliceman policeman;

	// Token: 0x04001EDD RID: 7901
	[SerializeField]
	public float deathInvincibilityBuffer;

	// Token: 0x04001EDE RID: 7902
	[SerializeField]
	public Effect deathExplodeEffect;

	// Token: 0x04001EDF RID: 7903
	[Header("Summons")]
	[SerializeField]
	public RumRunnersLevelGrub grubPrefab;

	// Token: 0x04001EE0 RID: 7904
	[SerializeField]
	public RumRunnersLevelGrubPath[] grubPaths;

	// Token: 0x04001EE1 RID: 7905
	[SerializeField]
	public RumRunnersLevelMine minePrefab;

	// Token: 0x04001EE2 RID: 7906
	[SerializeField]
	public RumRunnersLevelBouncingBeetle caterpillarPrefab;

	// Token: 0x04001EE3 RID: 7907
	[SerializeField]
	public Transform caterpillarSpawnPoint;

	// Token: 0x04001EE4 RID: 7908
	[SerializeField]
	public Effect kickFXEffect;

	// Token: 0x04001EE5 RID: 7909
	[SerializeField]
	public Transform kickFXSpawnPoint;

	// Token: 0x04001EE8 RID: 7912
	public RumRunnersLevelSpider.SummonType summonType;

	// Token: 0x04001EE9 RID: 7913
	public bool isSummoning;

	// Token: 0x04001EEA RID: 7914
	public Vector3 nextCopPosition;

	// Token: 0x04001EEB RID: 7915
	public PatternString grubDelayString;

	// Token: 0x04001EEC RID: 7916
	public PatternString grubPositionString;

	// Token: 0x04001EED RID: 7917
	public List<RumRunnersLevelGrub> grubList = new List<RumRunnersLevelGrub>();

	// Token: 0x04001EEE RID: 7918
	public int mineMainIndex;

	// Token: 0x04001EEF RID: 7919
	public int mineIndex;

	// Token: 0x04001EF0 RID: 7920
	public List<RumRunnersLevelMine> mineList = new List<RumRunnersLevelMine>();

	// Token: 0x04001EF1 RID: 7921
	public Vector3[,] minePositions;

	// Token: 0x04001EF2 RID: 7922
	public PatternString bouncingPattern;

	// Token: 0x04001EF3 RID: 7923
	public int grubEnterVariant;

	// Token: 0x04001EF4 RID: 7924
	public int grubVariant;

	// Token: 0x04001EF5 RID: 7925
	public DamageDealer damageDealer;

	// Token: 0x04001EF6 RID: 7926
	public DamageReceiver damageReceiver;

	// Token: 0x04001EF7 RID: 7927
	public Collider2D collider;

	// Token: 0x04001EF8 RID: 7928
	public float scaleX;

	// Token: 0x04001EF9 RID: 7929
	public float copSpawnPos;

	// Token: 0x04001EFA RID: 7930
	public List<RumRunnersLevelBouncingBeetle> beetleList = new List<RumRunnersLevelBouncingBeetle>();

	// Token: 0x02000EC6 RID: 3782
	public enum SummonType
	{
		// Token: 0x04006A35 RID: 27189
		Grubs,
		// Token: 0x04006A36 RID: 27190
		Mine,
		// Token: 0x04006A37 RID: 27191
		Bouncing,
		// Token: 0x04006A38 RID: 27192
		None
	}
}
