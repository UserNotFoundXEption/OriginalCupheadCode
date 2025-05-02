using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class FlyingCowboyLevelCowboy : LevelProperties.FlyingCowboy.Entity
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0001708E File Offset: 0x0001528E
	// (set) Token: 0x06001B27 RID: 6951 RVA: 0x00017096 File Offset: 0x00015296
	public FlyingCowboyLevelCowboy.State state { get; set; }

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06001B28 RID: 6952 RVA: 0x0001709F File Offset: 0x0001529F
	// (set) Token: 0x06001B29 RID: 6953 RVA: 0x000170A7 File Offset: 0x000152A7
	public bool IsDead { get; set; }

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06001B2A RID: 6954 RVA: 0x000170B0 File Offset: 0x000152B0
	// (set) Token: 0x06001B2B RID: 6955 RVA: 0x000170B8 File Offset: 0x000152B8
	public bool onBottom { get; set; }

	// Token: 0x06001B2C RID: 6956 RVA: 0x000170C1 File Offset: 0x000152C1
	public void OnEnable()
	{
		SceneLoader.OnFadeOutStartEvent += this.onFadeOutStartEvent;
		PlayerManager.OnPlayerJoinedEvent += this.onPlayerJoinedEvent;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000170E5 File Offset: 0x000152E5
	public void OnDisable()
	{
		SceneLoader.OnFadeOutStartEvent -= this.onFadeOutStartEvent;
		PlayerManager.OnPlayerJoinedEvent -= this.onPlayerJoinedEvent;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000AA7C8 File Offset: 0x000A89C8
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allDebris = new List<FlyingCowboyLevelDebris>();
		this.topPositions = new Vector3[6];
		this.sidePositions = new Vector3[6];
		this.bottomPositions = new Vector3[6];
		this.topCurvePositions = new Vector3[4];
		this.bottomCurvePositions = new Vector3[4];
		LevelProperties.FlyingCowboy.State currentState = base.properties.CurrentState;
		this.debrisCurveString = new PatternString(currentState.debris.debrisCurveShotString, true, true);
		this.debrisParryString = new PatternString(currentState.debris.debrisParryString, true);
		this.ricochetParryString = new PatternString(currentState.ricochet.splitParryString, true);
		this.backshotHighSpawnPosition = new PatternString(currentState.backshotEnemy.highSpawnPosition, true, true);
		this.backshotLowSpawnPosition = new PatternString(currentState.backshotEnemy.lowSpawnPosition, true, true);
		this.backshotSpawnDelay = new PatternString(currentState.backshotEnemy.spawnDelay, true, true);
		this.backshotBulletParryable = new PatternString(currentState.backshotEnemy.bulletParryString, true);
		this.backshotAnticipationStartDistancePattern = new PatternString(currentState.backshotEnemy.anticipationStartDistance, true, true);
		this.SetupDebrisSpawnPoints();
		base.StartCoroutine(this.wobble_cr());
		this.introBird = this.birdPrefab.Spawn(this.birdEndPosition.position);
		this.introBird.InitializeIntro(this.birdEndPosition.position);
		this.SFX_COWGIRL_COWGIRL_WheelConstantLoop();
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000AA960 File Offset: 0x000A8B60
	public void Update()
	{
		if (this.forcePlayer1 != null)
		{
			this.forcePlayer1.UpdateStrength(CupheadTime.Delta);
		}
		if (this.forcePlayer2 != null)
		{
			this.forcePlayer2.UpdateStrength(CupheadTime.Delta);
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x000AA9C4 File Offset: 0x000A8BC4
	public void SetupDebrisSpawnPoints()
	{
		this.sidePositions = new Vector3[6];
		float num = (this.vacuumSpawnTop.position.y - this.vacuumSpawnBottom.position.y) / 5f;
		for (int i = 0; i < 6; i++)
		{
			float x = this.vacuumSpawnBottom.position.x;
			float num2 = (i != 5) ? (this.vacuumSpawnBottom.position.y + num * (float)i) : this.vacuumSpawnTop.position.y;
			this.sidePositions[i] = new Vector3(x, num2);
		}
		float num3 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnTop.position.x) / 6f;
		for (int j = 0; j < 6; j++)
		{
			float num4 = this.vacuumSpawnTop.position.x + num3 + num3 * (float)j;
			float y = this.vacuumSpawnTop.position.y;
			this.topPositions[j] = new Vector3(num4, y);
		}
		for (int k = 0; k < 4; k++)
		{
			float num5 = this.vacuumSpawnTop.position.x + num3 + num3 * (float)(6 + k);
			float y2 = this.vacuumSpawnTop.position.y;
			this.topCurvePositions[k] = new Vector3(num5, y2);
		}
		for (int l = 0; l < 6; l++)
		{
			float num6 = this.vacuumSpawnBottom.position.x + num3 + num3 * (float)l;
			float y3 = this.vacuumSpawnBottom.position.y;
			this.bottomPositions[l] = new Vector3(num6, y3);
		}
		for (int m = 0; m < 4; m++)
		{
			float num7 = this.vacuumSpawnTop.position.x + num3 + num3 * (float)(6 + m);
			float y4 = this.vacuumSpawnTop.position.y;
			this.topCurvePositions[m] = new Vector3(num7, y4);
		}
		for (int n = 0; n < 4; n++)
		{
			float num8 = this.vacuumSpawnBottom.position.x + num3 + num3 * (float)(6 + n);
			float y5 = this.vacuumSpawnBottom.position.y;
			this.bottomCurvePositions[n] = new Vector3(num8, y5);
		}
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000AACB8 File Offset: 0x000A8EB8
	public override void LevelInit(LevelProperties.FlyingCowboy properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		this.snakeOilShotsPerAttackString = new PatternString(properties.CurrentState.snakeAttack.shotsPerAttack, true, true);
		this.snakeOffsetString = new PatternString(properties.CurrentState.snakeAttack.snakeOffsetString, true, true);
		this.snakeWidthString = new PatternString(properties.CurrentState.snakeAttack.snakeWidthString, true, true);
		this.debrisTopMainIndex = Random.Range(0, properties.CurrentState.debris.debrisTopSpawn.Length);
		this.debrisBottomMainIndex = Random.Range(0, properties.CurrentState.debris.debrisBottomSpawn.Length);
		this.debrisSideMainIndex = Random.Range(0, properties.CurrentState.debris.debrisSideSpawn.Length);
		this.initialSaloonPosition = base.transform.position;
		this.state = FlyingCowboyLevelCowboy.State.Idle;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x00017109 File Offset: 0x00015309
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0001711C File Offset: 0x0001531C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0001713A File Offset: 0x0001533A
	public void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000AADAC File Offset: 0x000A8FAC
	public IEnumerator intro_cr()
	{
		base.StartCoroutine(this.introBird_cr(0f));
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.animator.Play("Intro", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		base.StartCoroutine(this.main_cr());
		yield break;
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000AADC8 File Offset: 0x000A8FC8
	public IEnumerator introBird_cr(float time)
	{
		if (this.introBirdTriggered)
		{
			yield break;
		}
		this.introBirdTriggered = true;
		yield return CupheadTime.WaitForSeconds(this, time);
		this.introBird.MoveIntro(this.birdStartPosition.position, base.properties.CurrentState.bird);
		this.introBird = null;
		yield break;
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000AADEC File Offset: 0x000A8FEC
	public IEnumerator main_cr()
	{
		LevelProperties.FlyingCowboy.Cart p = base.properties.CurrentState.cart;
		PatternString pattern = new PatternString(p.cartAttackString, true, true);
		base.StartCoroutine(this.spawnBirdEnemies_cr());
		while (pattern.GetString() != "S")
		{
			pattern.PopString();
			yield return null;
		}
		for (;;)
		{
			while (this.state != FlyingCowboyLevelCowboy.State.Idle || this.phase2Trigger)
			{
				yield return null;
			}
			yield return null;
			string @string = pattern.GetString();
			if (@string != null)
			{
				if (!(@string == "M"))
				{
					if (!(@string == "S"))
					{
						if (@string == "B")
						{
							base.StartCoroutine(this.beamAttack_cr());
							base.StartCoroutine(this.spawnBackshotEnemy_cr());
						}
					}
					else
					{
						base.StartCoroutine(this.snakeAttack_cr());
						base.StartCoroutine(this.spawnBackshotEnemy_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.wait_cr());
				}
			}
			pattern.PopString();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000AAE08 File Offset: 0x000A9008
	public IEnumerator breakableRecoveryPhase1_cr(float duration)
	{
		float t = 0f;
		while (t < duration && !this.phase2Trigger)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000AAE2C File Offset: 0x000A902C
	public IEnumerator wobble_cr()
	{
		for (;;)
		{
			this.wobbleTimeElapsed.x = this.wobbleTimeElapsed.x + CupheadTime.Delta;
			this.wobbleTimeElapsed.y = this.wobbleTimeElapsed.y + CupheadTime.Delta;
			if (this.wobbleTimeElapsed.x >= 2f * this.wobbleDuration.x)
			{
				this.wobbleTimeElapsed.x = this.wobbleTimeElapsed.x - 2f * this.wobbleDuration.x;
			}
			float tx;
			if (this.wobbleTimeElapsed.x > this.wobbleDuration.x)
			{
				tx = 1f - (this.wobbleTimeElapsed.x - this.wobbleDuration.x) / this.wobbleDuration.x;
			}
			else
			{
				tx = this.wobbleTimeElapsed.x / this.wobbleDuration.x;
			}
			if (this.wobbleTimeElapsed.y >= 2f * this.wobbleDuration.y)
			{
				this.wobbleTimeElapsed.y = this.wobbleTimeElapsed.y - 2f * this.wobbleDuration.y;
			}
			float ty;
			if (this.wobbleTimeElapsed.y > this.wobbleDuration.y)
			{
				ty = 1f - (this.wobbleTimeElapsed.y - this.wobbleDuration.y) / this.wobbleDuration.y;
			}
			else
			{
				ty = this.wobbleTimeElapsed.y / this.wobbleDuration.y;
			}
			Vector3 position = this.initialSaloonPosition;
			position.x += EaseUtils.EaseInOutSine(this.wobbleRadius.x, -this.wobbleRadius.x, tx);
			position.y += EaseUtils.EaseInOutSine(this.wobbleRadius.y, -this.wobbleRadius.y, ty);
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000AAE48 File Offset: 0x000A9048
	public IEnumerator wait_cr()
	{
		this.state = FlyingCowboyLevelCowboy.State.Wait;
		LevelProperties.FlyingCowboy.Cart p = base.properties.CurrentState.cart;
		base.animator.SetBool("OnHide", true);
		string animationBaseName = (!this.onBottom) ? "HideToLow" : "HideToHigh";
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationBaseName + "Start", 0, false, false, true);
		base.animator.Play((!this.onBottom) ? "ToOpen" : "ToClosed", FlyingCowboyLevelCowboy.DoorsAnimatorLayer);
		yield return CupheadTime.WaitForSeconds(this, p.cartPopinTime);
		this.onBottom = !this.onBottom;
		base.animator.SetBool("IsLow", this.onBottom);
		yield return base.animator.WaitForAnimationToStart(this, animationBaseName + "End", false);
		base.animator.SetBool("OnHide", false);
		yield return base.animator.WaitForAnimationToEnd(this, animationBaseName + "End", false, true);
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x000AAE64 File Offset: 0x000A9064
	public IEnumerator snakeAttack_cr()
	{
		LevelProperties.FlyingCowboy.SnakeAttack p = base.properties.CurrentState.snakeAttack;
		this.state = FlyingCowboyLevelCowboy.State.SnakeAttack;
		string animationPrefix = "SnakeOil" + ((!this.onBottom) ? "_High" : "_Low") + ".";
		base.animator.SetTrigger("OnSnakeOil");
		base.animator.SetBool("SnakeInitialDelay", false);
		int shotsPerAttack = this.snakeOilShotsPerAttackString.PopInt();
		for (int shotCount = 0; shotCount < shotsPerAttack; shotCount++)
		{
			if (this.phase2Trigger && shotCount > 0)
			{
				break;
			}
			if (shotCount > 0)
			{
				yield return base.animator.WaitForAnimationToEnd(this, animationPrefix + "SnakeOilShoot", false, true);
				yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
				base.animator.SetTrigger("OnSnakeShoot");
			}
			yield return base.animator.WaitForAnimationToStart(this, animationPrefix + "SnakeOilShoot", false);
		}
		base.animator.SetTrigger("OnSnakeEnd");
		yield return base.animator.WaitForAnimationToEnd(this, animationPrefix + "SnakeOilExit", false, true);
		yield return base.StartCoroutine(this.breakableRecoveryPhase1_cr(p.attackRecovery));
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x000AAE80 File Offset: 0x000A9080
	public void animationEvent_SnakeShoot()
	{
		LevelProperties.FlyingCowboy.SnakeAttack snakeAttack = base.properties.CurrentState.snakeAttack;
		float num = this.snakeOffsetString.PopFloat();
		float num2 = this.snakeWidthString.PopFloat();
		AbstractPlayerController next = PlayerManager.GetNext();
		float snakeSpawnX = 640f - snakeAttack.breakLinePosition;
		float num3 = next.transform.position.y + num;
		for (int i = 0; i < 2; i++)
		{
			float num4 = (i != 0) ? (-num2) : num2;
			float num5 = num3 + num4;
			float finalYPosition = (num5 <= 0f) ? ((num5 <= -360f) ? -340f : num5) : ((num5 >= 360f) ? 340f : num5);
			Vector3 position = (!this.onBottom) ? this.snakeTopRoot[i].position : this.snakeBottomRoot[i].position;
			this.snakeOilMuzzleFXPrefab.Create(position);
			this.oilBlobPrefab.Create(position, finalYPosition, snakeSpawnX, snakeAttack, i == 0);
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000AAFA8 File Offset: 0x000A91A8
	public IEnumerator beamAttack_cr()
	{
		LevelProperties.FlyingCowboy.BeamAttack p = base.properties.CurrentState.beamAttack;
		this.state = FlyingCowboyLevelCowboy.State.BeamAttack;
		this.cactus.SetActive(true);
		string prefix = (!this.onBottom) ? "Cactus_High." : "Cactus_Low.";
		base.animator.SetTrigger("OnCactus");
		yield return base.animator.WaitForAnimationToEnd(this, prefix + "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.beamWarningTime);
		base.animator.SetTrigger("EndLasso");
		this.SFX_COWGIRL_COWGIRL_LassoSpinLoopStop();
		yield return base.animator.WaitForAnimationToStart(this, prefix + "Hold", false);
		yield return CupheadTime.WaitForSeconds(this, p.beamDuration);
		base.animator.SetTrigger("EndCactusHold");
		yield return base.animator.WaitForAnimationToEnd(this, prefix + "End", false, true);
		this.cactus.SetActive(false);
		yield return base.StartCoroutine(this.breakableRecoveryPhase1_cr(p.attackRecovery));
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000AAFC4 File Offset: 0x000A91C4
	public IEnumerator spawnBackshotEnemy_cr()
	{
		LevelProperties.FlyingCowboy.BackshotEnemy p = base.properties.CurrentState.backshotEnemy;
		yield return CupheadTime.WaitForSeconds(this, this.backshotSpawnDelay.PopFloat());
		float positionY = (!this.onBottom) ? this.backshotLowSpawnPosition.PopFloat() : this.backshotHighSpawnPosition.PopFloat();
		Vector3 position = new Vector3(740f, positionY);
		this.backshotPrefab.Create(position, 180f, p.enemySpeed, p.bulletSpeed, p.enemyHealth, this.backshotAnticipationStartDistancePattern.PopFloat(), this.backshotBulletParryable.PopLetter() == 'P');
		yield break;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000AAFE0 File Offset: 0x000A91E0
	public IEnumerator spawnBirdEnemies_cr()
	{
		LevelProperties.FlyingCowboy.Bird p = base.properties.CurrentState.bird;
		PatternString bulletLandingPositionPattern = new PatternString(p.bulletLandingPosition, true, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			bool canSpawn = false;
			float safetyTimer = 0f;
			while (!canSpawn)
			{
				bool found = false;
				foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
				{
					if (abstractPlayerController != null && this.birdSafetyZone.Contains(abstractPlayerController.center))
					{
						found = true;
						safetyTimer += CupheadTime.Delta;
						break;
					}
				}
				if (found && safetyTimer < p.safetyZoneMaxDuration)
				{
					yield return null;
				}
				else
				{
					canSpawn = true;
				}
			}
			FlyingCowboyLevelBird bird = this.birdPrefab.Spawn(this.birdStartPosition.position);
			bird.Initialize(this.birdStartPosition.position, this.birdEndPosition.position, bulletLandingPositionPattern.PopFloat(), p, this);
			while (bird != null)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000AAFFC File Offset: 0x000A91FC
	public void SpawnUFOs()
	{
		LevelProperties.FlyingCowboy.UFOEnemy uFOEnemy = base.properties.CurrentState.uFOEnemy;
		Vector3 pos;
		pos..ctor(740f, uFOEnemy.topUFOVerticalPosition);
		this.ufo = this.ufoPrefab.Spawn<FlyingCowboyLevelUFO>();
		this.ufo.Init(pos, base.properties.CurrentState.uFOEnemy, uFOEnemy.UFOHealth);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x00017149 File Offset: 0x00015349
	public void OnPhase2(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		this.phase2Trigger = true;
		base.animator.SetBool("OnPhase2", true);
		base.StartCoroutine(this.phase2TransStart_cr(postTransitionPattern));
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000AB060 File Offset: 0x000A9260
	public IEnumerator phase2TransStart_cr(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		int hash = Animator.StringToHash("HideToLowStart");
		int hash2 = Animator.StringToHash("HideToHighStart");
		for (;;)
		{
			int hash3 = base.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			if (hash3 == hash || hash3 == hash2)
			{
				break;
			}
			yield return null;
		}
		float previousT = float.MaxValue;
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		for (;;)
		{
			yield return waitForEndOfFrame;
			float t = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelCowboy.SaloonAnimatorLayer).normalizedTime);
			if (previousT < 0.0416666679f && t > 0.0416666679f)
			{
				break;
			}
			if (previousT < 0.5416667f && t > 0.5416667f)
			{
				goto Block_5;
			}
			previousT = t;
		}
		this.lanternARenderer.enabled = false;
		this.lanternBRenderer.enabled = true;
		goto IL_1A8;
		Block_5:
		this.lanternARenderer.enabled = true;
		this.lanternBRenderer.enabled = false;
		IL_1A8:
		base.animator.Play("Ph1_To_Ph2", 0);
		this.StopAllCoroutines();
		if (this.ufo != null)
		{
			this.ufo.Dead();
		}
		this.state = FlyingCowboyLevelCowboy.State.PhaseTrans;
		base.StartCoroutine(this.phase2_trans_cr(postTransitionPattern));
		yield break;
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000AB084 File Offset: 0x000A9284
	public IEnumerator phase2_trans_cr(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		yield return null;
		yield return base.animator.WaitForNormalizedTime(this, 0.8666667f, "Ph1_To_Ph2", 0, false, false, true);
		this.SFX_COWGIRL_COWGIRL_WheelConstantLoopStop();
		this.Vacuum(false, postTransitionPattern);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Ph1_To_Ph2", 0, false, false, true);
		base.animator.Play("Vacuum", 0);
		base.animator.Play("TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer);
		yield return base.animator.WaitForAnimationToEnd(this, "TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer, false, true);
		this.endTransitionTrigger = true;
		while (this.transitionVacuumAttackCoroutine != null)
		{
			yield return null;
		}
		if (postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Vacuum || this.phase3Trigger)
		{
			this.endVacuumPullPlayer();
		}
		this.endTransitionTrigger = false;
		yield return null;
		yield return null;
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x000AB0A8 File Offset: 0x000A92A8
	public IEnumerator moveDown_cr()
	{
		this.phase2BasePosition = this.initialSaloonPosition;
		this.phase2BasePosition.y = -183f;
		Vector3 initialPosition = base.transform.position;
		Vector3 targetPosition = this.phase2BasePosition;
		float elapsedTime = 0f;
		while (elapsedTime < 2f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / 2f);
		}
		yield break;
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x000AB0C4 File Offset: 0x000A92C4
	public void Vacuum(bool initial, LevelProperties.FlyingCowboy.Pattern postTransitionPattern = LevelProperties.FlyingCowboy.Pattern.Default)
	{
		base.animator.SetBool("OnRicochet", false);
		if (postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Default)
		{
			this.transitionVacuumAttackCoroutine = base.StartCoroutine(this.vacuum_cr(initial, postTransitionPattern));
		}
		else
		{
			base.StartCoroutine(this.vacuum_cr(initial, postTransitionPattern));
		}
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000AB110 File Offset: 0x000A9310
	public IEnumerator vacuumCurveShots_cr(bool transition)
	{
		LevelProperties.FlyingCowboy.Debris p = base.properties.CurrentState.debris;
		string[] debrisCurveShotString;
		if (transition)
		{
			PatternString patternString = new PatternString(p.transitionCurveShotString, true, true);
			debrisCurveShotString = patternString.PopString().Split(new char[]
			{
				','
			});
		}
		else
		{
			debrisCurveShotString = this.debrisCurveString.PopString().Split(new char[]
			{
				','
			});
		}
		Vector3[] positions = this.topPositions;
		int spawnIndex = 0;
		float angle = 0f;
		Vector3 root = this.vacuumDebrisAimTransform.position;
		for (int i = 0; i < debrisCurveShotString.Length; i++)
		{
			string[] spawn = debrisCurveShotString[i].Split(new char[]
			{
				':'
			});
			foreach (string text in spawn)
			{
				if (text == "B")
				{
					positions = this.bottomCurvePositions;
				}
				else if (text == "T")
				{
					positions = this.topCurvePositions;
				}
				else
				{
					Parser.IntTryParse(text, out spawnIndex);
				}
			}
			float apexHeight = Mathf.Abs(this.vacuumDebrisAimTransform.position.x - positions[spawnIndex].x) + 300f;
			float timeToApex = p.debrisCurveApexTime;
			float height = -apexHeight;
			float apexTime2 = timeToApex * timeToApex;
			float g = -2f * height / apexTime2;
			float viX = 2f * height / timeToApex;
			float viY2 = viX * viX;
			float x = root.x - positions[spawnIndex].x;
			float y = root.y - positions[spawnIndex].y;
			float sqrtRooted = viY2 + 2f * g * x;
			float tEnd = (-viX + Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd2 = (-viX - Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd3 = Mathf.Max(tEnd, tEnd2);
			float velocityY = y / tEnd3;
			FlyingCowboyLevelDebris debris = this.largeVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>().Create(positions[spawnIndex], angle * 57.29578f, p.debrisOneSpeedStartEnd.min) as FlyingCowboyLevelDebris;
			debris.GetComponent<SpriteRenderer>().sortingOrder = i;
			bool parryable = this.debrisParryString.PopLetter() == 'P';
			debris.SetParryable(parryable);
			Vector3 velocity = new Vector3(viX, velocityY);
			debris.ToCurve(velocity, g);
			debris.SetupVacuum(this.vacuumDebrisAimTransform, this.vacuumDebrisDisappearTransform);
			this.allDebris.Add(debris);
			yield return CupheadTime.WaitForSeconds(this, p.debrisDelay);
			if (this.phase3Trigger || this.endTransitionTrigger)
			{
				break;
			}
		}
		yield break;
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x000AB134 File Offset: 0x000A9334
	public IEnumerator vacuum_cr(bool initial, LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		bool transition = postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Default;
		if (!initial)
		{
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop();
		}
		if (!transition)
		{
			this.state = FlyingCowboyLevelCowboy.State.Vacuum;
		}
		LevelProperties.FlyingCowboy.Debris p = base.properties.CurrentState.debris;
		if (!initial && !transition)
		{
			this.startVacuumPullPlayer(false);
		}
		if (!initial && !transition)
		{
			yield return CupheadTime.WaitForSeconds(this, p.warningDelayRange.RandomFloat());
		}
		PatternString debrisTypePattern = new PatternString(p.debrisTypeString, true);
		base.StartCoroutine(this.vacuumCurveShots_cr(transition));
		string[] debrisTop;
		string[] debrisBottom;
		string[] debrisSide;
		if (transition)
		{
			int num = Random.Range(0, base.properties.CurrentState.debris.transitionTopSpawn.Length);
			int num2 = Random.Range(0, base.properties.CurrentState.debris.transitionBottomSpawn.Length);
			int num3 = Random.Range(0, base.properties.CurrentState.debris.transitionSideSpawn.Length);
			debrisTop = p.transitionTopSpawn[num].Split(new char[]
			{
				','
			});
			debrisBottom = p.transitionBottomSpawn[num2].Split(new char[]
			{
				','
			});
			debrisSide = p.transitionSideSpawn[num3].Split(new char[]
			{
				','
			});
		}
		else
		{
			debrisTop = p.debrisTopSpawn[this.debrisTopMainIndex].Split(new char[]
			{
				','
			});
			debrisBottom = p.debrisBottomSpawn[this.debrisBottomMainIndex].Split(new char[]
			{
				','
			});
			debrisSide = p.debrisSideSpawn[this.debrisSideMainIndex].Split(new char[]
			{
				','
			});
		}
		int debrisTopCount = 0;
		int debrisBottomCount = 0;
		int debrisSideCount = 0;
		int maxLength = Mathf.Max(new int[]
		{
			debrisTop.Length,
			debrisBottom.Length,
			debrisSide.Length
		});
		if (transition && postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Ricochet)
		{
			this.vacuumSizeCoroutine = base.StartCoroutine(this.growVacuum_cr());
		}
		for (int i = 0; i < maxLength; i++)
		{
			if (i < debrisTop.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisTop[debrisTopCount], out posIndex);
				this.createLinearDebris(this.topPositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisTopCount++;
			}
			if (i < debrisBottom.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisBottom[debrisBottomCount], out posIndex);
				this.createLinearDebris(this.bottomPositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisBottomCount++;
			}
			if (i < debrisSide.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisSide[debrisSideCount], out posIndex);
				this.createLinearDebris(this.sidePositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisSideCount++;
			}
			yield return CupheadTime.WaitForSeconds(this, p.debrisDelay);
			if (this.phase3Trigger || this.endTransitionTrigger)
			{
				break;
			}
		}
		if (transition && postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Vacuum && !this.phase3Trigger)
		{
			this.allDebris.Clear();
		}
		else
		{
			if (!transition)
			{
				this.vacuumSizeCoroutine = base.StartCoroutine(this.growVacuum_cr());
			}
			bool allDebrisGone = false;
			while (!allDebrisGone)
			{
				allDebrisGone = true;
				for (int j = 0; j < this.allDebris.Count; j++)
				{
					if (this.allDebris[j] != null && !this.allDebris[j].dead)
					{
						allDebrisGone = false;
					}
				}
				yield return null;
			}
			this.allDebris.Clear();
			while (this.vacuumSizeCoroutine != null)
			{
				yield return null;
			}
		}
		if (!transition || (transition && (postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Ricochet || this.phase3Trigger)))
		{
			this.endVacuumPullPlayer();
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
		}
		if (transition)
		{
			this.transitionVacuumAttackCoroutine = null;
			if (this.phase3Trigger)
			{
				this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
				base.animator.SetBool("OnPhase3", true);
			}
		}
		else
		{
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
			this.debrisTopMainIndex = (this.debrisTopMainIndex + 1) % p.debrisTopSpawn.Length;
			this.debrisBottomMainIndex = (this.debrisBottomMainIndex + 1) % p.debrisBottomSpawn.Length;
			this.debrisSideMainIndex = (this.debrisSideMainIndex + 1) % p.debrisSideSpawn.Length;
			bool manualDeathHandling = true;
			if (this.phase3Trigger)
			{
				manualDeathHandling = false;
				base.animator.SetBool("OnPhase3", true);
			}
			else
			{
				base.animator.SetBool("OnRicochet", true);
				yield return CupheadTime.WaitForSeconds(this, p.hesitate);
			}
			this.state = FlyingCowboyLevelCowboy.State.Idle;
			if (this.phase3Trigger && manualDeathHandling)
			{
				this.Ricochet();
			}
		}
		yield break;
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000AB160 File Offset: 0x000A9360
	public void createLinearDebris(Vector3 rootPosition, int type, int sortingIndex)
	{
		LevelProperties.FlyingCowboy.Debris debris = base.properties.CurrentState.debris;
		MinMax minMax;
		FlyingCowboyLevelDebris random;
		if (type == 1)
		{
			minMax = debris.debrisOneSpeedStartEnd;
			random = this.largeVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		else if (type == 2)
		{
			minMax = debris.debrisTwoSpeedStartEnd;
			random = this.mediumVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		else
		{
			minMax = debris.debrisThreeSpeedStartEnd;
			random = this.smallVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		Vector3 vector = this.vacuumDebrisAimTransform.position - rootPosition;
		FlyingCowboyLevelDebris flyingCowboyLevelDebris = random.Create(rootPosition, MathUtils.DirectionToAngle(vector), minMax.min) as FlyingCowboyLevelDebris;
		flyingCowboyLevelDebris.GetComponent<SpriteRenderer>().sortingOrder = 50 * type + sortingIndex;
		bool parryable = this.debrisParryString.PopLetter() == 'P';
		flyingCowboyLevelDebris.SetParryable(parryable);
		flyingCowboyLevelDebris.SetupLinearSpeed(minMax, debris.debrisSpeedUpDistance, this.vacuumDebrisAimTransform);
		flyingCowboyLevelDebris.SetupVacuum(this.vacuumDebrisAimTransform, this.vacuumDebrisDisappearTransform);
		this.allDebris.Add(flyingCowboyLevelDebris);
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x000AB268 File Offset: 0x000A9468
	public void startVacuumPullPlayer(bool immediateFullStrength)
	{
		this.endVacuumPullPlayer();
		LevelProperties.FlyingCowboy.Debris debris = base.properties.CurrentState.debris;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				FlyingCowboyLevelCowboy.VacuumForce force = new FlyingCowboyLevelCowboy.VacuumForce(planePlayerController, this.vacuumDebrisDisappearTransform, debris.vacuumWindStrength * 0.5f, (!immediateFullStrength) ? debris.vacuumTimeToFullStrength : 0f);
				planePlayerController.motor.AddForce(force);
				if (planePlayerController.id == PlayerId.PlayerOne)
				{
					this.forcePlayer1 = force;
				}
				else if (planePlayerController.id == PlayerId.PlayerTwo)
				{
					this.forcePlayer2 = force;
				}
			}
		}
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000AB350 File Offset: 0x000A9550
	public void endVacuumPullPlayer()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null) && !(planePlayerController.motor == null))
			{
				planePlayerController.motor.RemoveForce(this.forcePlayer1);
				planePlayerController.motor.RemoveForce(this.forcePlayer2);
			}
		}
		this.forcePlayer1 = null;
		this.forcePlayer2 = null;
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000AB3FC File Offset: 0x000A95FC
	public IEnumerator growVacuum_cr()
	{
		int hash = Animator.StringToHash("Vacuum");
		float previousTime = float.MaxValue;
		for (;;)
		{
			float normalizedTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			if (previousTime < 0.416666657f && normalizedTime >= 0.416666657f)
			{
				break;
			}
			previousTime = normalizedTime;
			yield return null;
		}
		this.setVacuumTransition();
		previousTime = float.MaxValue;
		for (;;)
		{
			float normalizedTime2 = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			if (previousTime < 0.291666657f && normalizedTime2 >= 0.291666657f)
			{
				break;
			}
			previousTime = normalizedTime2;
			yield return null;
		}
		this.setVacuumBig();
		this.vacuumSizeCoroutine = null;
		yield break;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x00017171 File Offset: 0x00015371
	public void Ricochet()
	{
		base.animator.SetBool("OnRicochet", true);
		base.StartCoroutine(this.ricochet_cr());
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000AB418 File Offset: 0x000A9618
	public IEnumerator ricochet_cr()
	{
		this.nextSafeShoot = 1;
		this.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart();
		this.state = FlyingCowboyLevelCowboy.State.Ricochet;
		LevelProperties.FlyingCowboy.Ricochet p = base.properties.CurrentState.ricochet;
		this.setVacuumBig();
		yield return base.animator.WaitForAnimationToStart(this, "Ricochet", false);
		this.vacuumSizeCoroutine = base.StartCoroutine(this.shrinkVacuum_cr());
		base.transform.position = this.phase2BasePosition;
		float elapsedTime = 0f;
		PatternString delayPattern = new PatternString(p.rainDelayString, true);
		PatternString bulletTypePattern = new PatternString(p.rainTypeString, true);
		PatternString xPositionPattern = new PatternString(p.rainSpawnString, true);
		PatternString speedPattern = new PatternString(p.rainSpeedString, true);
		while (elapsedTime < p.rainDuration)
		{
			if (this.phase3Trigger && elapsedTime >= 2f)
			{
				break;
			}
			float delayTime = delayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delayTime);
			FlyingCowboyLevelRicochetDebris.BulletType bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Nothing;
			if (bulletTypePattern.PopLetter() == 'R')
			{
				bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Ricochet;
			}
			float xPosition = xPositionPattern.PopFloat();
			float speed = speedPattern.PopFloat();
			this.ricochetPrefab.Create(new Vector3(-xPosition, 430f), speed, p.splitBulletSpeed, bulletType, bulletType != FlyingCowboyLevelRicochetDebris.BulletType.Nothing && this.ricochetParryString.PopLetter() == 'P');
			elapsedTime += delayTime;
		}
		base.animator.SetBool("OnRicochet", false);
		this.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop();
		if (this.phase3Trigger)
		{
			if (this.vacuumSizeCoroutine != null)
			{
				base.StopCoroutine(this.vacuumSizeCoroutine);
				this.vacuumSizeCoroutine = null;
				this.setVacuumRegular();
			}
			base.animator.SetBool("OnPhase3", true);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, p.rainRecoveryTime);
		}
		if (this.phase3Trigger)
		{
			base.animator.SetBool("OnPhase3", true);
		}
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000AB434 File Offset: 0x000A9634
	public void animationEvent_SafeShoot(int eventType)
	{
		if (eventType == 0)
		{
			if (this.nextSafeShoot == 0)
			{
				AbstractProjectile abstractProjectile = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
				abstractProjectile.animator.Play("A");
				abstractProjectile.animator.Update(0f);
				this.nextSafeShoot = 1;
			}
			else if (this.nextSafeShoot == 2)
			{
				AbstractProjectile abstractProjectile2 = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
				abstractProjectile2.animator.Play("C");
				abstractProjectile2.animator.Update(0f);
				this.nextSafeShoot = 1;
			}
		}
		else if (eventType == 1 && this.nextSafeShoot == 1)
		{
			AbstractProjectile abstractProjectile3 = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
			abstractProjectile3.animator.Play("B");
			abstractProjectile3.animator.Update(0f);
			this.nextSafeShoot = ((!Rand.Bool()) ? 2 : 0);
			this.shootCoins();
		}
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000AB55C File Offset: 0x000A975C
	public void shootCoins()
	{
		LevelProperties.FlyingCowboy.Ricochet ricochet = base.properties.CurrentState.ricochet;
		int num = ricochet.coinCountRange.RandomInt();
		for (int i = 0; i < num; i++)
		{
			Vector2 vector;
			vector..ctor(ricochet.coinSpeedXRange.RandomFloat(), KinematicUtilities.CalculateInitialSpeedToReachApex(ricochet.coinHeightRange.RandomFloat(), ricochet.coinGravity));
			float rotation = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			BasicProjectile basicProjectile = this.coinProjectile.Create(this.ricochetUpSpawnPoint.transform.position, rotation, vector.magnitude);
			basicProjectile.Gravity = ricochet.coinGravity;
			basicProjectile.GetComponent<SpriteRenderer>().maskInteraction = 0;
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000AB620 File Offset: 0x000A9820
	public IEnumerator shrinkVacuum_cr()
	{
		yield return base.animator.WaitForNormalizedTime(this, 2f, "Ricochet", 0, false, false, true);
		this.setVacuumTransition();
		yield return base.animator.WaitForNormalizedTime(this, 2.9375f, "Ricochet", 0, false, false, true);
		this.setVacuumRegular();
		this.vacuumSizeCoroutine = null;
		yield break;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000AB63C File Offset: 0x000A983C
	public void setVacuumRegular()
	{
		Renderer renderer = this.regularVacuumRenderer;
		bool flag = true;
		this.regularHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.transitionVacuumRenderer;
		flag = false;
		this.bigHoseRenderer.enabled = flag;
		flag = flag;
		this.bigVacuumRenderer.enabled = flag;
		flag = flag;
		this.transitionHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000AB69C File Offset: 0x000A989C
	public void setVacuumTransition()
	{
		Renderer renderer = this.transitionVacuumRenderer;
		bool flag = true;
		this.transitionHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.regularVacuumRenderer;
		flag = false;
		this.bigHoseRenderer.enabled = flag;
		flag = flag;
		this.bigVacuumRenderer.enabled = flag;
		flag = flag;
		this.regularHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000AB6FC File Offset: 0x000A98FC
	public void setVacuumBig()
	{
		Renderer renderer = this.bigVacuumRenderer;
		bool flag = true;
		this.bigHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.regularVacuumRenderer;
		flag = false;
		this.transitionHoseRenderer.enabled = flag;
		flag = flag;
		this.transitionVacuumRenderer.enabled = flag;
		flag = flag;
		this.regularHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x00017191 File Offset: 0x00015391
	public void Death()
	{
		base.StartCoroutine(this.phase3_cr());
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000AB75C File Offset: 0x000A995C
	public IEnumerator phase3_cr()
	{
		this.phase3Trigger = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Ph2_To_Ph3", false, true);
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000171A0 File Offset: 0x000153A0
	public void aniEvent_SpawnMeat()
	{
		this.IsDead = true;
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000171A9 File Offset: 0x000153A9
	public void animationEvent_PosterFlyAway()
	{
		if (!this.posterFlyAwayTriggered)
		{
			base.animator.Play("FlyAway", FlyingCowboyLevelCowboy.PosterAnimatorLayer);
			this.posterRenderer.sortingLayerName = "Effects";
		}
		this.posterFlyAwayTriggered = true;
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000AB778 File Offset: 0x000A9978
	public void animationEvent_DisablePhase1Saloon()
	{
		foreach (SpriteRenderer spriteRenderer in this.saloonTransitionDisableRenderers)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000171E2 File Offset: 0x000153E2
	public void animationEvent_DisableSaloonCollider()
	{
		this.saloonCollidersParent.SetActive(false);
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000171F0 File Offset: 0x000153F0
	public void animationEvent_EnablePlayerVacuumForce()
	{
		this.startVacuumPullPlayer(false);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000171F9 File Offset: 0x000153F9
	public void animationEvent_DisableFrontSaloonWheel()
	{
		this.frontWheelRenderer.enabled = false;
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x00017207 File Offset: 0x00015407
	public void animationEvent_DisableBackSaloonWheel()
	{
		this.backWheelRenderer.enabled = false;
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x000AB7AC File Offset: 0x000A99AC
	public void animationEvent_TurnOffPhase1Animators()
	{
		base.animator.Play("Off", FlyingCowboyLevelCowboy.SaloonAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.PosterAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.DoorsAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.WheelSmokeAnimatorLayer);
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x00017215 File Offset: 0x00015415
	public void animationEvent_MoveCowgirlDown()
	{
		base.StartCoroutine(this.moveDown_cr());
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x00017224 File Offset: 0x00015424
	public void animationEvent_SwapPhase2Puffs()
	{
		this.phase2PuffARenderer.enabled = this.phase2PuffBRenderer.enabled;
		this.phase2PuffBRenderer.enabled = !this.phase2PuffARenderer.enabled;
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x00017255 File Offset: 0x00015455
	public void onFadeOutStartEvent(float time)
	{
		base.StartCoroutine(this.introBird_cr(0.25f));
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x00017269 File Offset: 0x00015469
	public void onPlayerJoinedEvent(PlayerId playerId)
	{
		if (this.forcePlayer1 != null || this.forcePlayer2 != null)
		{
			this.startVacuumPullPlayer(true);
		}
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x000AB810 File Offset: 0x000A9A10
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.green;
		float num = (this.vacuumSpawnTop.position.y - this.vacuumSpawnBottom.position.y) / 5f;
		for (int i = 0; i < 6; i++)
		{
			float x = this.vacuumSpawnBottom.position.x;
			float num2 = (i != 5) ? (this.vacuumSpawnBottom.position.y + num * (float)i) : this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(x, num2), 10f);
		}
		Gizmos.color = Color.yellow;
		float num3 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnTop.position.x) / 6f;
		for (int j = 0; j < 6; j++)
		{
			float num4 = this.vacuumSpawnTop.position.x + num3 + num3 * (float)j;
			float y = this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(num4, y), 10f);
		}
		Gizmos.color = Color.yellow;
		num3 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnBottom.position.x) / 6f;
		for (int k = 0; k < 6; k++)
		{
			float num5 = this.vacuumSpawnBottom.position.x + num3 + num3 * (float)k;
			float y2 = this.vacuumSpawnBottom.position.y;
			Gizmos.DrawWireSphere(new Vector3(num5, y2), 10f);
		}
		Gizmos.color = Color.red;
		for (int l = 0; l < 4; l++)
		{
			float num6 = this.vacuumSpawnTop.position.x + num3 + num3 * (float)(6 + l);
			float y3 = this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(num6, y3), 10f);
		}
		Gizmos.color = Color.red;
		for (int m = 0; m < 4; m++)
		{
			float num7 = this.vacuumSpawnBottom.position.x + num3 + num3 * (float)(6 + m);
			float y4 = this.vacuumSpawnBottom.position.y;
			Gizmos.DrawWireSphere(new Vector3(num7, y4), 10f);
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x00017288 File Offset: 0x00015488
	public void AnimationEvent_SFX_COWGIRL_Vocal_Laugh()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_laugh");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_laugh");
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000172A4 File Offset: 0x000154A4
	public void AnimationEvent_SFX_COWGIRL_Vocal_MooHa()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_mooha");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_mooha");
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000172C0 File Offset: 0x000154C0
	public void AnimationEvent_SFX_COWGIRL_Vocal_Surprised()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_surprised");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_surprised");
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000172DC File Offset: 0x000154DC
	public void AnimationEvent_SFX_COWGIRL_Vocal_YeeHaw()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_yeehaw");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_yeehaw");
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000172F8 File Offset: 0x000154F8
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunRaise()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_raise");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x00017314 File Offset: 0x00015514
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunHolster()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster");
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x00017330 File Offset: 0x00015530
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlow()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow");
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x0001734C File Offset: 0x0001554C
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlowAndHolster()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster");
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x00017368 File Offset: 0x00015568
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlast()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast");
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x0001738E File Offset: 0x0001558E
	public void SFX_COWGIRL_COWGIRL_JugGunSpinLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000173AA File Offset: 0x000155AA
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P1toP2VacuumSuckup()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup");
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000173C6 File Offset: 0x000155C6
	public void SFX_COWGIRL_COWGIRL_LassoSpinLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_lasso_spin_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0.7f, 0.01f);
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_spin_loop");
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000173F6 File Offset: 0x000155F6
	public void SFX_COWGIRL_COWGIRL_LassoSpinLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0f, 0.2f);
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x0001740C File Offset: 0x0001560C
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_LassoThrowCatchRelease()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_lasso_throw_catch_release");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_throw_catch_release");
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x00017428 File Offset: 0x00015628
	public void SFX_COWGIRL_COWGIRL_WheelConstantLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x00017444 File Offset: 0x00015644
	public void SFX_COWGIRL_COWGIRL_WheelConstantLoopStop()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x00017450 File Offset: 0x00015650
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_PositionLowtoHigh()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh");
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0001746C File Offset: 0x0001566C
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_PositionHightoLow()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow");
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x00017488 File Offset: 0x00015688
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_Stirrups()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_Stirrups");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_Stirrups");
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000174A4 File Offset: 0x000156A4
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumBlowback()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback");
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000174C0 File Offset: 0x000156C0
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumCrouchPosition()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition");
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000174DC File Offset: 0x000156DC
	public void SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0f, 1f, 1f);
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x00017511 File Offset: 0x00015711
	public void SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0f, 1f);
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x00017527 File Offset: 0x00015727
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumSuckIn()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_suckin");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_suckin");
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x00017543 File Offset: 0x00015743
	public void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumeExplosionDeath()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition");
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0001755F File Offset: 0x0001575F
	public void SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_stirrupswheels_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_stirrupswheels_loop");
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0001757B File Offset: 0x0001577B
	public void SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_stirrupswheels_loop");
	}

	// Token: 0x040015ED RID: 5613
	public const int SNAKE_BULLET_COUNT = 2;

	// Token: 0x040015EE RID: 5614
	public const int DEBRIS_SPAWN_COUNT = 6;

	// Token: 0x040015EF RID: 5615
	public const int DEBRIS_CURVE_SPAWN_COUNT = 4;

	// Token: 0x040015F0 RID: 5616
	public static readonly int SaloonAnimatorLayer = 1;

	// Token: 0x040015F1 RID: 5617
	public static readonly int PosterAnimatorLayer = 2;

	// Token: 0x040015F2 RID: 5618
	public static readonly int DoorsAnimatorLayer = 3;

	// Token: 0x040015F3 RID: 5619
	public static readonly int WheelSmokeAnimatorLayer = 4;

	// Token: 0x040015F4 RID: 5620
	public static readonly int TransitionSmokeLayer = 5;

	// Token: 0x040015F5 RID: 5621
	[SerializeField]
	public SpriteRenderer posterRenderer;

	// Token: 0x040015F6 RID: 5622
	[SerializeField]
	public FlyingCowboyLevelUFO ufoPrefab;

	// Token: 0x040015F7 RID: 5623
	[SerializeField]
	public FlyingCowboyLevelBird birdPrefab;

	// Token: 0x040015F8 RID: 5624
	[SerializeField]
	public Transform birdStartPosition;

	// Token: 0x040015F9 RID: 5625
	[SerializeField]
	public Transform birdEndPosition;

	// Token: 0x040015FA RID: 5626
	[SerializeField]
	public TriggerZone birdSafetyZone;

	// Token: 0x040015FB RID: 5627
	[SerializeField]
	public FlyingCowboyLevelBackshot backshotPrefab;

	// Token: 0x040015FC RID: 5628
	[SerializeField]
	public Transform[] snakeTopRoot;

	// Token: 0x040015FD RID: 5629
	[SerializeField]
	public Transform[] snakeBottomRoot;

	// Token: 0x040015FE RID: 5630
	[SerializeField]
	public FlyingCowboyLevelOilBlob oilBlobPrefab;

	// Token: 0x040015FF RID: 5631
	[SerializeField]
	public Effect snakeOilMuzzleFXPrefab;

	// Token: 0x04001600 RID: 5632
	[SerializeField]
	public GameObject cactus;

	// Token: 0x04001601 RID: 5633
	[SerializeField]
	public Vector2 wobbleRadius;

	// Token: 0x04001602 RID: 5634
	[SerializeField]
	public Vector2 wobbleDuration;

	// Token: 0x04001603 RID: 5635
	[SerializeField]
	public GameObject saloonCollidersParent;

	// Token: 0x04001604 RID: 5636
	[SerializeField]
	public SpriteRenderer lanternARenderer;

	// Token: 0x04001605 RID: 5637
	[SerializeField]
	public SpriteRenderer lanternBRenderer;

	// Token: 0x04001606 RID: 5638
	[SerializeField]
	public SpriteRenderer[] saloonTransitionDisableRenderers;

	// Token: 0x04001607 RID: 5639
	[SerializeField]
	public SpriteRenderer frontWheelRenderer;

	// Token: 0x04001608 RID: 5640
	[SerializeField]
	public SpriteRenderer backWheelRenderer;

	// Token: 0x04001609 RID: 5641
	[SerializeField]
	public FlyingCowboyLevelDebris[] smallVacuumDebrisPrefabs;

	// Token: 0x0400160A RID: 5642
	[SerializeField]
	public FlyingCowboyLevelDebris[] mediumVacuumDebrisPrefabs;

	// Token: 0x0400160B RID: 5643
	[SerializeField]
	public FlyingCowboyLevelDebris[] largeVacuumDebrisPrefabs;

	// Token: 0x0400160C RID: 5644
	[SerializeField]
	public FlyingCowboyLevelRicochetDebris ricochetPrefab;

	// Token: 0x0400160D RID: 5645
	[SerializeField]
	public AbstractProjectile ricochetUpPrefab;

	// Token: 0x0400160E RID: 5646
	[SerializeField]
	public Transform ricochetUpSpawnPoint;

	// Token: 0x0400160F RID: 5647
	[SerializeField]
	public BasicProjectile coinProjectile;

	// Token: 0x04001610 RID: 5648
	[SerializeField]
	public Transform pistolShootRoot;

	// Token: 0x04001611 RID: 5649
	[SerializeField]
	public int debrisSpawnHorizontalSpacing = 140;

	// Token: 0x04001612 RID: 5650
	[SerializeField]
	public Transform vacuumDebrisAimTransform;

	// Token: 0x04001613 RID: 5651
	[SerializeField]
	public Transform vacuumDebrisDisappearTransform;

	// Token: 0x04001614 RID: 5652
	[SerializeField]
	public Transform vacuumSpawnTop;

	// Token: 0x04001615 RID: 5653
	[SerializeField]
	public Transform vacuumSpawnBottom;

	// Token: 0x04001616 RID: 5654
	[SerializeField]
	public SpriteRenderer bigVacuumRenderer;

	// Token: 0x04001617 RID: 5655
	[SerializeField]
	public SpriteRenderer transitionVacuumRenderer;

	// Token: 0x04001618 RID: 5656
	[SerializeField]
	public SpriteRenderer regularVacuumRenderer;

	// Token: 0x04001619 RID: 5657
	[SerializeField]
	public SpriteRenderer bigHoseRenderer;

	// Token: 0x0400161A RID: 5658
	[SerializeField]
	public SpriteRenderer transitionHoseRenderer;

	// Token: 0x0400161B RID: 5659
	[SerializeField]
	public SpriteRenderer regularHoseRenderer;

	// Token: 0x0400161C RID: 5660
	[SerializeField]
	public SpriteRenderer phase2PuffARenderer;

	// Token: 0x0400161D RID: 5661
	[SerializeField]
	public SpriteRenderer phase2PuffBRenderer;

	// Token: 0x04001620 RID: 5664
	public bool introBirdTriggered;

	// Token: 0x04001621 RID: 5665
	public FlyingCowboyLevelBird introBird;

	// Token: 0x04001622 RID: 5666
	public bool phase2Trigger;

	// Token: 0x04001623 RID: 5667
	public bool endTransitionTrigger;

	// Token: 0x04001624 RID: 5668
	public bool phase3Trigger;

	// Token: 0x04001625 RID: 5669
	public Vector3 initialSaloonPosition;

	// Token: 0x04001626 RID: 5670
	public Vector2 wobbleTimeElapsed;

	// Token: 0x04001627 RID: 5671
	public Vector3[] topPositions;

	// Token: 0x04001628 RID: 5672
	public Vector3[] bottomPositions;

	// Token: 0x04001629 RID: 5673
	public Vector3[] sidePositions;

	// Token: 0x0400162A RID: 5674
	public Vector3[] topCurvePositions;

	// Token: 0x0400162B RID: 5675
	public Vector3[] bottomCurvePositions;

	// Token: 0x0400162C RID: 5676
	public Vector3 phase2BasePosition;

	// Token: 0x0400162D RID: 5677
	public DamageDealer damageDealer;

	// Token: 0x0400162E RID: 5678
	public DamageReceiver damageReceiver;

	// Token: 0x0400162F RID: 5679
	public List<FlyingCowboyLevelDebris> allDebris;

	// Token: 0x04001630 RID: 5680
	public FlyingCowboyLevelUFO ufo;

	// Token: 0x04001631 RID: 5681
	public PatternString snakeOilShotsPerAttackString;

	// Token: 0x04001632 RID: 5682
	public PatternString snakeOffsetString;

	// Token: 0x04001633 RID: 5683
	public PatternString snakeWidthString;

	// Token: 0x04001634 RID: 5684
	public PatternString backshotHighSpawnPosition;

	// Token: 0x04001635 RID: 5685
	public PatternString backshotLowSpawnPosition;

	// Token: 0x04001636 RID: 5686
	public PatternString backshotSpawnDelay;

	// Token: 0x04001637 RID: 5687
	public PatternString backshotBulletParryable;

	// Token: 0x04001638 RID: 5688
	public PatternString backshotAnticipationStartDistancePattern;

	// Token: 0x04001639 RID: 5689
	public int debrisTopMainIndex;

	// Token: 0x0400163A RID: 5690
	public int debrisBottomMainIndex;

	// Token: 0x0400163B RID: 5691
	public int debrisSideMainIndex;

	// Token: 0x0400163C RID: 5692
	public FlyingCowboyLevelCowboy.VacuumForce forcePlayer1;

	// Token: 0x0400163D RID: 5693
	public FlyingCowboyLevelCowboy.VacuumForce forcePlayer2;

	// Token: 0x0400163E RID: 5694
	public PatternString debrisCurveString;

	// Token: 0x0400163F RID: 5695
	public PatternString debrisParryString;

	// Token: 0x04001640 RID: 5696
	public PatternString ricochetParryString;

	// Token: 0x04001641 RID: 5697
	public int nextSafeShoot;

	// Token: 0x04001642 RID: 5698
	public bool posterFlyAwayTriggered;

	// Token: 0x04001643 RID: 5699
	public Coroutine transitionVacuumAttackCoroutine;

	// Token: 0x04001644 RID: 5700
	public Coroutine vacuumSizeCoroutine;

	// Token: 0x02000CB1 RID: 3249
	public enum State
	{
		// Token: 0x04005BA1 RID: 23457
		Idle,
		// Token: 0x04005BA2 RID: 23458
		Wait,
		// Token: 0x04005BA3 RID: 23459
		SnakeAttack,
		// Token: 0x04005BA4 RID: 23460
		BeamAttack,
		// Token: 0x04005BA5 RID: 23461
		Vacuum,
		// Token: 0x04005BA6 RID: 23462
		Ricochet,
		// Token: 0x04005BA7 RID: 23463
		PhaseTrans
	}

	// Token: 0x02000CB2 RID: 3250
	public class VacuumForce : PlanePlayerMotor.Force
	{
		// Token: 0x060065FA RID: 26106 RVA: 0x00216E80 File Offset: 0x00215080
		public VacuumForce(PlanePlayerController player, Transform aimPointTransform, float strength, float timeToFullStrength) : base(Vector2.zero, true)
		{
			this.player = player;
			this.aimPointTransform = aimPointTransform;
			this.strength = strength;
			this.currentStrength = 0f;
			this.timeToFullStrength = timeToFullStrength;
			this.elapsedTime = 0f;
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x060065FB RID: 26107 RVA: 0x00216ECC File Offset: 0x002150CC
		public override Vector2 force
		{
			get
			{
				if (this.player == null)
				{
					return Vector2.zero;
				}
				return (this.player.center - this.aimPointTransform.position).normalized * this.currentStrength;
			}
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x00048A1F File Offset: 0x00046C1F
		public void UpdateStrength(float deltaTime)
		{
			this.elapsedTime += deltaTime;
			this.currentStrength = Mathf.Lerp(0f, this.strength, this.elapsedTime / this.timeToFullStrength);
		}

		// Token: 0x04005BA8 RID: 23464
		public PlanePlayerController player;

		// Token: 0x04005BA9 RID: 23465
		public Transform aimPointTransform;

		// Token: 0x04005BAA RID: 23466
		public float strength;

		// Token: 0x04005BAB RID: 23467
		public float currentStrength;

		// Token: 0x04005BAC RID: 23468
		public float timeToFullStrength;

		// Token: 0x04005BAD RID: 23469
		public float elapsedTime;
	}
}
