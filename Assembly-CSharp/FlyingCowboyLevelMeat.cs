using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class FlyingCowboyLevelMeat : LevelProperties.FlyingCowboy.Entity
{
	// Token: 0x06001B8C RID: 7052 RVA: 0x000ABC9C File Offset: 0x000A9E9C
	public void Start()
	{
		Level.Current.OnBossDeathExplosionsEvent += this.onBossDeathExplosionsEventHandler;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.nextBulletSpawnPointA.position = this.sausageHolderA.position;
		this.nextBulletSpawnPointB.position = this.sausageHolderB.position;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x00017621 File Offset: 0x00015821
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000ABD1C File Offset: 0x000A9F1C
	public override void LevelInit(LevelProperties.FlyingCowboy properties)
	{
		base.LevelInit(properties);
		this.runningSpitBulletParryPattern = new PatternString(properties.CurrentState.sausageRun.bulletParry, true);
		this.sausageTimeToMoveString = new PatternString(properties.CurrentState.sausageRun.timeTillSwitch, true, true);
		this.spitBulletParryString = new PatternString(properties.CurrentState.can.bulletParryString, true);
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x0001763F File Offset: 0x0001583F
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x000ABD88 File Offset: 0x000A9F88
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			AudioManager.Play("sfx_dlc_cowgirl_p3_can_damage_metalimpact");
		}
		base.properties.DealDamage(info.damage);
		if (!this.isDead && base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x00017657 File Offset: 0x00015857
	public void SelectPhase(FlyingCowboyLevelMeat.MeatPhase meatPhase)
	{
		base.gameObject.SetActive(true);
		this.meatPhase = meatPhase;
		if (meatPhase != FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			if (meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
			{
				this.Sausage();
			}
		}
		else
		{
			this.Can();
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x000ABDE4 File Offset: 0x000A9FE4
	public void Sausage()
	{
		Vector3 position = this.sausageSpawnPosition.position;
		position.y = 42f;
		base.transform.position = position;
		base.StartCoroutine(this.sausage_intro_cr());
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x000ABE24 File Offset: 0x000AA024
	public IEnumerator sausage_intro_cr()
	{
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		yield return CupheadTime.WaitForSeconds(this, p.mirrorTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Sg_Mirror_Cont", false, true);
		base.StartCoroutine(this.beans_cr());
		if (p.shootBullets)
		{
			base.StartCoroutine(this.sausageTurret_cr());
		}
		base.StartCoroutine(this.sausageSwitchHeight_cr());
		yield break;
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x00017694 File Offset: 0x00015894
	public void animationEvent_RepositionSausage()
	{
		base.StartCoroutine(this.repositionSausage_cr());
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000ABE40 File Offset: 0x000AA040
	public IEnumerator repositionSausage_cr()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
		float startX = base.transform.position.x;
		float elapsedTime = 0f;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage && elapsedTime < 4f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 position = base.transform.position;
			position.x = Mathf.Lerp(startX, 340f, elapsedTime / 4f);
			base.transform.position = position;
		}
		base.StartCoroutine(this.wobble_cr(base.transform, this.sausageWobbleRadius, this.sausageWobbleDuration, base.transform.position, FlyingCowboyLevelMeat.MeatPhase.Sausage, false, false));
		yield break;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000ABE5C File Offset: 0x000AA05C
	public IEnumerator sausageSwitchHeight_cr()
	{
		for (;;)
		{
			float time = this.sausageTimeToMoveString.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, time);
			bool newFlyingStatus = !this.isFlying;
			base.animator.SetBool("IsFlying", newFlyingStatus);
			if (newFlyingStatus)
			{
				AudioManager.Stop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
			}
			string transitionAnimation = (!newFlyingStatus) ? "Sg_Fly_To_Run" : "Sg_Run_To_Fly";
			yield return base.animator.WaitForAnimationToEnd(this, transitionAnimation, false, true);
			if (!newFlyingStatus)
			{
				AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
			}
			this.isFlying = newFlyingStatus;
		}
		yield break;
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000ABE78 File Offset: 0x000AA078
	public IEnumerator beans_cr()
	{
		float startBeansHealthPercentage = base.properties.CurrentState.healthTrigger;
		float endBeansPercentage = base.properties.GetNextStateHealthTrigger();
		float startBeansHealth = startBeansHealthPercentage * base.properties.TotalHealth;
		float endBeansHealth = endBeansPercentage * base.properties.TotalHealth;
		float seventyFivePercentof = startBeansHealth + (endBeansHealth - startBeansHealth) * 0.75f;
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		PatternString groupDelayPattern = new PatternString(p.groupBeansDelayString, true, true);
		PatternString positionPattern = new PatternString(p.beansPositionString, true, false);
		PatternString extendTimerPattern = new PatternString(p.beansExtendTimer, true);
		float positionX = 690f;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
		{
			string[] positionValues = positionPattern.GetString().Split(new char[]
			{
				':'
			});
			positionPattern.IncrementString();
			float positionY;
			Parser.FloatTryParse(positionValues[0], out positionY);
			bool pointingUp = positionValues[1] == "U";
			float currentPercentage = (base.properties.CurrentHealth - startBeansHealth) / (seventyFivePercentof - startBeansHealth);
			float speed = Mathf.Lerp(p.beansSpeed.min, p.beansSpeed.max, currentPercentage);
			FlyingCowboyLevelBeans beans = this.beansPrefab.Spawn<FlyingCowboyLevelBeans>();
			beans.Init(new Vector3(positionX, positionY), pointingUp, speed, extendTimerPattern.PopFloat());
			if (positionPattern.GetSubStringIndex() != 0)
			{
				float spawnDelay = Mathf.Lerp(p.beansSpawnDelay.max, p.beansSpawnDelay.min, currentPercentage);
				yield return CupheadTime.WaitForSeconds(this, spawnDelay);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, groupDelayPattern.PopFloat());
			}
		}
		yield break;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000ABE94 File Offset: 0x000AA094
	public IEnumerator sausageTurret_cr()
	{
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		AbstractPlayerController player = PlayerManager.GetNext();
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
		{
			base.animator.SetTrigger("OnShoot");
			this.waitingToShoot = true;
			while (this.waitingToShoot)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.bulletDelay);
		}
		yield break;
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000ABEB0 File Offset: 0x000AA0B0
	public void aniEvent_ShootTurret()
	{
		this.player = PlayerManager.GetNext();
		LevelProperties.FlyingCowboy.SausageRun sausageRun = base.properties.CurrentState.sausageRun;
		Vector3 vector = (!this.isFlying) ? this.runBottomSpitBulletSpawn.position : this.runTopSpitBulletSpawn.position;
		Vector3 position = (!this.isFlying) ? this.runBottomSpitBulletEffectSpawn.position : this.runTopSpitBulletEffectSpawn.position;
		this.player = PlayerManager.GetNext();
		Vector3 vector2 = this.player.transform.position - vector;
		float num;
		for (num = MathUtils.DirectionToAngle(vector2); num < 0f; num += 360f)
		{
		}
		float num2;
		float num3;
		bool clockwise;
		if (this.isFlying)
		{
			num2 = 180f - sausageRun.bulletTopMaxUpAngle;
			num3 = 180f + sausageRun.bulletTopMaxDownAngle;
			clockwise = sausageRun.bulletTopRotateClockwise;
		}
		else
		{
			num2 = 180f - sausageRun.bulletBottomMaxUpAngle;
			num3 = 180f + sausageRun.bulletBottomMaxDownAngle;
			clockwise = sausageRun.bulletBottomRotateClockwise;
		}
		num = Mathf.Clamp(num, num2, num3);
		vector2 = MathUtilities.AngleToDirection(num);
		this.sausageRunSpitBullet.Create(vector, sausageRun.bulletSpeed, sausageRun.bulletRotationSpeed, sausageRun.bulletRotationRadius, vector2, clockwise, this.runningSpitBulletParryPattern.PopLetter() == 'P');
		Effect effect = this.sausageRunSpitBulletEffect.Create(position);
		if (!this.isFlying)
		{
			effect.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
		}
		this.waitingToShoot = false;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000176A3 File Offset: 0x000158A3
	public void Can()
	{
		base.StartCoroutine(this.toCan_cr());
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000AC05C File Offset: 0x000AA25C
	public IEnumerator toCan_cr()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
		base.animator.SetBool("ToCan", true);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "SausageToCanEnd", 0, true, false, true);
		base.animator.Play("CanIntro", 0);
		base.animator.Update(0f);
		base.StartCoroutine(this.repositionCan_cr());
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		base.StartCoroutine(this.wobble_cr(this.canTransform, new Vector2(p.wobbleRadiusX, p.wobbleRadiusY), new Vector2(p.wobbleDurationX, p.wobbleDurationY), this.canTransform.localPosition, FlyingCowboyLevelMeat.MeatPhase.Can, true, true));
		yield break;
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000AC078 File Offset: 0x000AA278
	public IEnumerator repositionCan_cr()
	{
		Vector3 startPosition = base.transform.position;
		Vector3 targetPosition = new Vector3(340f, 61f, startPosition.z);
		float elapsedTime = 0f;
		while (elapsedTime < 3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3f);
		}
		yield break;
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000AC094 File Offset: 0x000AA294
	public void animationEvent_StartSausageLinks()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.sausageTransforms.SetActive(true);
		base.StartCoroutine(this.sausageTrain_cr(true));
		base.StartCoroutine(this.sausageRotation_cr(this.sausageHolderA, 0));
		base.StartCoroutine(this.sausageTrain_cr(false));
		base.StartCoroutine(this.sausageRotation_cr(this.sausageHolderB, 1));
		if (base.properties.CurrentState.can.shootBullets)
		{
			base.StartCoroutine(this.shootCanBullets_cr());
		}
		base.StartCoroutine(this.beanCanTriggerZone_cr());
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000176B2 File Offset: 0x000158B2
	public void animationEvent_TriggerCanBullets()
	{
		this.canBulletsTriggered = true;
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x000AC140 File Offset: 0x000AA340
	public IEnumerator shootCanBullets_cr()
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		int variant = 0;
		int fxVariant = Random.Range(0, 3);
		PatternString bulletCountPattern = new PatternString(p.bulletCount, true, true);
		this.canBulletsTriggered = false;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			base.animator.SetTrigger("OnShoot");
			while (!this.canBulletsTriggered)
			{
				yield return null;
			}
			this.canBulletsTriggered = false;
			Effect muzzleFX = this.canBulletMuzzleFX.Create(this.bulletRoot.position);
			muzzleFX.animator.SetInteger("Effect", fxVariant);
			fxVariant = MathUtilities.NextIndex(fxVariant, 3);
			this.SFX_CanSpitBurningFire();
			while (!this.canBulletsTriggered)
			{
				yield return null;
			}
			this.canBulletsTriggered = false;
			int count = bulletCountPattern.PopInt();
			float startAngle = -p.bulletSpreadAngle * 0.5f;
			float angleIncrement = p.bulletSpreadAngle / (float)(count - 1);
			for (int i = 0; i < count; i++)
			{
				float num = startAngle + angleIncrement * (float)i;
				float rotation = 180f - num;
				BasicProjectile basicProjectile = this.canBullet.Create(this.bulletRoot.position, rotation, p.bulletSpeed);
				bool flag = this.spitBulletParryString.PopLetter() == 'P';
				basicProjectile.SetParryable(flag);
				basicProjectile.animator.SetInteger("Variant", variant);
				basicProjectile.animator.Update(0f);
				basicProjectile.animator.Play(0, 0, Random.Range(0f, 1f));
				basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = i;
				basicProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-num));
				if (!flag)
				{
					variant = ((variant != 0) ? 0 : 1);
				}
			}
		}
		this.sausageTransforms.SetActive(false);
		yield break;
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x000AC15C File Offset: 0x000AA35C
	public IEnumerator sausageRotation_cr(Transform sausageHolder, int index)
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		Transform holder = (index != 0) ? this.sausageHolderB : this.sausageHolderA;
		float topAngle = -p.maxSausageAngle;
		float bottomAngle = p.maxSausageAngle;
		bool goingUp = index == 0;
		float startAngle = (!goingUp) ? bottomAngle : 0f;
		float endAngle = (!goingUp) ? 0f : topAngle;
		int sortingOffset = index * 100;
		float elapsedTime = 0f;
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			while (elapsedTime < 2f)
			{
				if (this.meatPhase != FlyingCowboyLevelMeat.MeatPhase.Can)
				{
					break;
				}
				float t = (!goingUp) ? (1f - elapsedTime / 2f) : (elapsedTime / 2f);
				float angle = Mathf.Lerp(startAngle, endAngle, t);
				sausageHolder.transform.SetEulerAngles(null, null, new float?(angle));
				int sortingOrder;
				if (angle >= 15f)
				{
					sortingOrder = FlyingCowboyLevelMeat.LowSausageLinkSortingOrder + sortingOffset;
				}
				else if (angle <= -15f)
				{
					sortingOrder = FlyingCowboyLevelMeat.HighSausageLinkSortingOrder + sortingOffset;
				}
				else
				{
					sortingOrder = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + sortingOffset;
				}
				int childCount = holder.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = holder.GetChild(i);
					SpriteRenderer component = child.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						component.sortingOrder = sortingOrder + childCount - i;
					}
				}
				if (index == 0)
				{
					this.currentSausageLinkSortingOrderA = sortingOrder;
				}
				else if (index == 1)
				{
					this.currentSausageLinkSortingOrderB = sortingOrder;
				}
				elapsedTime += CupheadTime.FixedDelta;
				yield return wait;
			}
			if ((goingUp && startAngle == 0f) || (!goingUp && endAngle == 0f))
			{
				goingUp = !goingUp;
			}
			else if (!goingUp && startAngle == 0f)
			{
				startAngle = bottomAngle;
				endAngle = 0f;
			}
			else if (goingUp && startAngle == bottomAngle)
			{
				startAngle = 0f;
				endAngle = topAngle;
			}
			elapsedTime = 0f;
		}
		yield break;
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x000AC188 File Offset: 0x000AA388
	public IEnumerator sausageTrain_cr(bool isTypeA)
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		Transform sausageHolder = (!isTypeA) ? this.sausageHolderB : this.sausageHolderA;
		Transform nextSpawn = (!isTypeA) ? this.nextBulletSpawnPointB : this.nextBulletSpawnPointA;
		string[] sausageMainString = (!isTypeA) ? p.sausageStringB : p.sausageStringA;
		PatternString sausageAmountPattern = new PatternString(sausageMainString, true, true);
		PatternString gapPattern = new PatternString((!isTypeA) ? p.gapDistB : p.gapDistA, true, true);
		int sausageCounter = 0;
		int sausageMax = sausageAmountPattern.PopInt();
		FlyingCowboyLevelMeat.SausageType previousSausageType = FlyingCowboyLevelMeat.SausageType.H1;
		FlyingCowboyLevelSausageLink previousSausage = null;
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			if (sausageCounter < sausageMax)
			{
				bool flag = false;
				FlyingCowboyLevelMeat.SausageType sausageType;
				if (previousSausageType == FlyingCowboyLevelMeat.SausageType.U1 || previousSausageType == FlyingCowboyLevelMeat.SausageType.U2 || previousSausageType == FlyingCowboyLevelMeat.SausageType.U3)
				{
					sausageType = FlyingCowboyLevelMeat.SausageTypeDown.RandomChoice<FlyingCowboyLevelMeat.SausageType>();
					flag = true;
				}
				else if (sausageMax - sausageCounter < 2)
				{
					sausageType = FlyingCowboyLevelMeat.SausageTypeEnd.RandomChoice<FlyingCowboyLevelMeat.SausageType>();
				}
				else
				{
					for (sausageType = previousSausageType; sausageType == previousSausageType; sausageType = FlyingCowboyLevelMeat.SausageTypeAny.RandomChoice<FlyingCowboyLevelMeat.SausageType>())
					{
					}
				}
				previousSausageType = sausageType;
				sausageCounter++;
				FlyingCowboyLevelSausageLink flyingCowboyLevelSausageLink = this.sausage.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed) as FlyingCowboyLevelSausageLink;
				flyingCowboyLevelSausageLink.transform.parent = sausageHolder;
				flyingCowboyLevelSausageLink.Initialize(sausageType, this.sausageLinkSqueezePoint, (!flag) ? null : previousSausage);
				if (flag)
				{
					flyingCowboyLevelSausageLink.animator.Play("SqueezeLoopDown");
				}
				previousSausage = flyingCowboyLevelSausageLink;
				nextSpawn.parent = flyingCowboyLevelSausageLink.transform;
				nextSpawn.localPosition = new Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0f);
				SpriteRenderer component = flyingCowboyLevelSausageLink.GetComponent<SpriteRenderer>();
				component.sortingOrder = ((!isTypeA) ? this.currentSausageLinkSortingOrderB : this.currentSausageLinkSortingOrderA);
			}
			else
			{
				int num = gapPattern.PopInt() - 1;
				float num2 = FlyingCowboyLevelMeat.SausageGapWidths[num];
				nextSpawn.localPosition = new Vector3(num2, 0f);
				BasicProjectile basicProjectile = this.sausageString.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed);
				basicProjectile.animator.Play(FlyingCowboyLevelMeat.SausageGapAnimationNames[num]);
				SpriteRenderer component2 = basicProjectile.GetComponent<SpriteRenderer>();
				component2.sortingOrder = ((!isTypeA) ? this.currentSausageLinkSortingOrderB : this.currentSausageLinkSortingOrderA);
				basicProjectile.transform.parent = sausageHolder;
				nextSpawn.parent = basicProjectile.transform;
				nextSpawn.localPosition = new Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0f);
				sausageCounter = 0;
				sausageMax = sausageAmountPattern.PopInt();
			}
			while (nextSpawn.position.x > sausageHolder.position.x + 175f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x000AC1AC File Offset: 0x000AA3AC
	public IEnumerator beanCanTriggerZone_cr()
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		PatternString extendTimerPattern = new PatternString(p.beanCanExtendTimer, true);
		PatternString topSpawnPattern = new PatternString(p.beanCanPostionUpper, true, true);
		PatternString bottomSpawnPattern = new PatternString(p.beanCanPositionLower, true, true);
		float[] timers = new float[this.beanCanTriggerZones.Length];
		for (;;)
		{
			yield return null;
			for (int i = 0; i < this.beanCanTriggerZones.Length; i++)
			{
				bool flag = false;
				Vector3 vector = Vector3.zero;
				foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
				{
					TriggerZone triggerZone = this.beanCanTriggerZones[i];
					if (abstractPlayerController != null && triggerZone.Contains(abstractPlayerController.center))
					{
						timers[i] += CupheadTime.Delta;
						vector = abstractPlayerController.center;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					timers[i] = 0f;
				}
				else if (timers[i] > p.beanCanTriggerTime)
				{
					timers[i] -= p.beanCanTriggerTime;
					bool flag2 = this.beanCanTriggerZones[i].transform.position.y > 0f;
					string[] array = ((!flag2) ? bottomSpawnPattern.PopString() : topSpawnPattern.PopString()).Split(new char[]
					{
						':'
					});
					float num;
					Parser.FloatTryParse(array[0], out num);
					bool pointingUp = array[1] == "U";
					FlyingCowboyLevelBeans flyingCowboyLevelBeans = this.beansPrefab.Spawn<FlyingCowboyLevelBeans>();
					flyingCowboyLevelBeans.Init(new Vector3(690f, num), pointingUp, p.beanCanSpeed, extendTimerPattern.PopFloat());
				}
			}
		}
		yield break;
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x000AC1C8 File Offset: 0x000AA3C8
	public void die()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("DeathEasy");
		}
		else
		{
			base.animator.Play("Death");
			base.StartCoroutine(this.spawnFloatingSausages_cr());
			AudioManager.Stop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		}
		for (int i = 0; i < 2; i++)
		{
			Transform transform = (i != 0) ? this.sausageHolderB : this.sausageHolderA;
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = transform.GetChild(j);
				if (child.position.x > this.sausageLinkSqueezePoint.position.x)
				{
					Object.Destroy(child.gameObject);
				}
			}
		}
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x000AC2B0 File Offset: 0x000AA4B0
	public IEnumerator spawnFloatingSausages_cr()
	{
		float delay = 1f;
		string[] animations = new string[]
		{
			"A",
			"B",
			"C"
		};
		float[] spawnFactors = new float[]
		{
			0.2f,
			0.6f,
			0f,
			0.8f,
			0.4f,
			1f
		};
		int spawnFactorIndex = Random.Range(0, spawnFactors.Length);
		int animationIndex = Random.Range(0, animations.Length);
		for (;;)
		{
			float factor = spawnFactors[spawnFactorIndex];
			Vector3 position = Vector3.Lerp(this.floatingSausageSpawnPointLeft.position, this.floatingSausageSpawnPointRight.position, factor);
			FlyingCowboyFloatingSausages s = this.floatingSausage.Create(position) as FlyingCowboyFloatingSausages;
			s.SetAnimation(animations[animationIndex]);
			spawnFactorIndex = MathUtilities.NextIndex(spawnFactorIndex, spawnFactors.Length);
			animationIndex = MathUtilities.NextIndex(animationIndex, animations.Length);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x000AC2CC File Offset: 0x000AA4CC
	public void onBossDeathExplosionsEventHandler()
	{
		Level.Current.OnBossDeathExplosionsEvent -= this.onBossDeathExplosionsEventHandler;
		string[] list = new string[]
		{
			"A",
			"B",
			"C",
			"H"
		};
		string[] list2 = new string[]
		{
			"D",
			"E",
			"F",
			"G",
			"I"
		};
		string[] array = new string[]
		{
			"E",
			"F",
			"I"
		};
		for (int i = 0; i < 2; i++)
		{
			Transform transform = (i != 0) ? this.sausageHolderB : this.sausageHolderA;
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = transform.GetChild(j);
				if (child.name.Contains("String"))
				{
					Effect effect = this.sausageStringDeathEffect.Create(child.GetComponent<SpriteRenderer>().bounds.center);
					effect.transform.rotation = child.rotation;
					Animator animator = effect.animator;
					AnimatorStateInfo currentAnimatorStateInfo = child.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
					if (currentAnimatorStateInfo.IsName("String1"))
					{
						animator.Play(list.RandomChoice<string>());
					}
					else if (currentAnimatorStateInfo.IsName("String2"))
					{
						animator.Play(list2.RandomChoice<string>());
					}
					else
					{
						animator.Play(list2.RandomChoice<string>());
					}
				}
				else
				{
					SpriteRenderer component = child.GetComponent<SpriteRenderer>();
					this.sausageDeathEffect.Create(component.bounds.center);
				}
				Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000176BB File Offset: 0x000158BB
	public void AnimationEvent_SFX_VocalSausageScreaming()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_p3sausagescreaming");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_p3sausagescreaming");
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x000176D7 File Offset: 0x000158D7
	public void AnimationEvent_SFX_CanSlam()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P3_CanSlam_Transition");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P3_CanSlam_Transition");
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x000176F3 File Offset: 0x000158F3
	public void AnimationEvent_SFX_CanHoleBurst()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_holeburst_pop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_holeburst_pop");
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x0001770F File Offset: 0x0001590F
	public void SFX_CanSpitBurningFire()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_canspitburningfire");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canspitburningfire");
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x0001772B File Offset: 0x0001592B
	public void SFX_CanSpit()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_spit");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_spit");
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x00017747 File Offset: 0x00015947
	public void AnimationEvent_SFX_SausageBullRoar()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullroar");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullroar");
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00017763 File Offset: 0x00015963
	public void AnimationEvent_SFX_SausageBullSpit()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullspit");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullspit");
	}

	// Token: 0x06001BAD RID: 7085 RVA: 0x0001777F File Offset: 0x0001597F
	public void AnimationEvent_SFX_SausageBullWingUp()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingup");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingup");
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x0001779B File Offset: 0x0001599B
	public void AnimationEvent_SFX_SausageBullWingDown()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingdown");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingdown");
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x000177B7 File Offset: 0x000159B7
	public void AnimationEvent_SFX_SausageBullRunToFly()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebull_runtofly");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebull_runtofly");
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000177D3 File Offset: 0x000159D3
	public void AnimationEvent_SFX_SausageBullPositionTransfer()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausage_position_transfer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausage_position_transfer");
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000AC4A8 File Offset: 0x000AA6A8
	public IEnumerator wobble_cr(Transform transform, Vector2 wobbleRadius, Vector2 wobbleDuration, Vector3 initialPosition, FlyingCowboyLevelMeat.MeatPhase phase, bool useLocal, bool easeWobble)
	{
		float elapsedEaseTime = 0f;
		Vector3 shadowInitialPosition = this.shadowTransform.position;
		Vector2 wobbleTimeElapsed = wobbleDuration * 0.5f;
		while (this.meatPhase == phase)
		{
			if (easeWobble && elapsedEaseTime < 2f)
			{
				elapsedEaseTime += CupheadTime.Delta;
				float easeFactor = Mathf.Lerp(0f, 1f, elapsedEaseTime / 2f);
			}
			wobbleTimeElapsed.x += CupheadTime.Delta;
			wobbleTimeElapsed.y += CupheadTime.Delta;
			if (wobbleTimeElapsed.x >= 2f * wobbleDuration.x)
			{
				wobbleTimeElapsed.x -= 2f * wobbleDuration.x;
			}
			float tx;
			if (wobbleTimeElapsed.x > wobbleDuration.x)
			{
				tx = 1f - (wobbleTimeElapsed.x - wobbleDuration.x) / wobbleDuration.x;
			}
			else
			{
				tx = wobbleTimeElapsed.x / wobbleDuration.x;
			}
			if (wobbleTimeElapsed.y >= 2f * wobbleDuration.y)
			{
				wobbleTimeElapsed.y -= 2f * wobbleDuration.y;
			}
			float ty;
			if (wobbleTimeElapsed.y > wobbleDuration.y)
			{
				ty = 1f - (wobbleTimeElapsed.y - wobbleDuration.y) / wobbleDuration.y;
			}
			else
			{
				ty = wobbleTimeElapsed.y / wobbleDuration.y;
			}
			Vector3 positionChange = new Vector3(EaseUtils.EaseInOutSine(wobbleRadius.x, -wobbleRadius.x, tx), EaseUtils.EaseInOutSine(wobbleRadius.y, -wobbleRadius.y, ty));
			if (useLocal)
			{
				transform.localPosition = initialPosition + positionChange;
			}
			else
			{
				transform.position = initialPosition + positionChange;
			}
			if (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can && !Mathf.Approximately(wobbleRadius.y, 0f))
			{
				Vector3 vector = positionChange;
				vector.y *= 0.2f;
				Vector3 position = shadowInitialPosition + vector;
				float num = 0f;
				if (position.y >= -220f)
				{
					num = MathUtilities.LerpMapping(position.y, -220f, -150f, 0f, 0.650000036f, true);
					position.y = -220f;
				}
				this.shadowTransform.position = position;
				float num2 = 0.1f * (positionChange.y / wobbleRadius.y);
				float value = 0.8f - num2 - num;
				this.shadowTransform.SetScale(new float?(value), new float?(value), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400164B RID: 5707
	public static readonly float SausageLinkWidth = 120f;

	// Token: 0x0400164C RID: 5708
	public static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeAny = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.H1,
		FlyingCowboyLevelMeat.SausageType.H2,
		FlyingCowboyLevelMeat.SausageType.H3,
		FlyingCowboyLevelMeat.SausageType.H4,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.U1,
		FlyingCowboyLevelMeat.SausageType.U2,
		FlyingCowboyLevelMeat.SausageType.U3
	};

	// Token: 0x0400164D RID: 5709
	public static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeEnd = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.H1,
		FlyingCowboyLevelMeat.SausageType.H2,
		FlyingCowboyLevelMeat.SausageType.H3,
		FlyingCowboyLevelMeat.SausageType.H4,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5
	};

	// Token: 0x0400164E RID: 5710
	public static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeDown = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.D1,
		FlyingCowboyLevelMeat.SausageType.D2,
		FlyingCowboyLevelMeat.SausageType.D3
	};

	// Token: 0x0400164F RID: 5711
	public static readonly float[] SausageGapWidths = new float[]
	{
		146f,
		189f,
		286f
	};

	// Token: 0x04001650 RID: 5712
	public static readonly string[] SausageGapAnimationNames = new string[]
	{
		"String1",
		"String2",
		"String3"
	};

	// Token: 0x04001651 RID: 5713
	public static readonly int LowSausageLinkSortingOrder = 20;

	// Token: 0x04001652 RID: 5714
	public static readonly int MidSausageLinkSortingOrder = 40;

	// Token: 0x04001653 RID: 5715
	public static readonly int HighSausageLinkSortingOrder = 60;

	// Token: 0x04001654 RID: 5716
	[Header("Sausage")]
	[SerializeField]
	public Transform sausageSpawnPosition;

	// Token: 0x04001655 RID: 5717
	[SerializeField]
	public FlyingCowboyLevelBeans beansPrefab;

	// Token: 0x04001656 RID: 5718
	[SerializeField]
	public FlyingCowboyLevelSpinningBullet sausageRunSpitBullet;

	// Token: 0x04001657 RID: 5719
	[SerializeField]
	public Effect sausageRunSpitBulletEffect;

	// Token: 0x04001658 RID: 5720
	[SerializeField]
	public Transform runTopSpitBulletSpawn;

	// Token: 0x04001659 RID: 5721
	[SerializeField]
	public Transform runTopSpitBulletEffectSpawn;

	// Token: 0x0400165A RID: 5722
	[SerializeField]
	public Transform runBottomSpitBulletSpawn;

	// Token: 0x0400165B RID: 5723
	[SerializeField]
	public Transform runBottomSpitBulletEffectSpawn;

	// Token: 0x0400165C RID: 5724
	[SerializeField]
	public Vector2 sausageWobbleRadius;

	// Token: 0x0400165D RID: 5725
	[SerializeField]
	public Vector2 sausageWobbleDuration;

	// Token: 0x0400165E RID: 5726
	[Header("Can")]
	[SerializeField]
	public Transform canTransform;

	// Token: 0x0400165F RID: 5727
	[SerializeField]
	public GameObject sausageTransforms;

	// Token: 0x04001660 RID: 5728
	[SerializeField]
	public BasicProjectile canBullet;

	// Token: 0x04001661 RID: 5729
	[SerializeField]
	public Effect canBulletMuzzleFX;

	// Token: 0x04001662 RID: 5730
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x04001663 RID: 5731
	[SerializeField]
	public Transform shadowTransform;

	// Token: 0x04001664 RID: 5732
	[SerializeField]
	public BasicProjectile sausage;

	// Token: 0x04001665 RID: 5733
	[SerializeField]
	public Transform sausageLinkSqueezePoint;

	// Token: 0x04001666 RID: 5734
	[SerializeField]
	public Transform sausageHolderA;

	// Token: 0x04001667 RID: 5735
	[SerializeField]
	public Transform sausageHolderB;

	// Token: 0x04001668 RID: 5736
	[SerializeField]
	public Transform nextBulletSpawnPointA;

	// Token: 0x04001669 RID: 5737
	[SerializeField]
	public Transform nextBulletSpawnPointB;

	// Token: 0x0400166A RID: 5738
	[SerializeField]
	public FlyingCowboyFloatingSausages floatingSausage;

	// Token: 0x0400166B RID: 5739
	[SerializeField]
	public Transform floatingSausageSpawnPointLeft;

	// Token: 0x0400166C RID: 5740
	[SerializeField]
	public Transform floatingSausageSpawnPointRight;

	// Token: 0x0400166D RID: 5741
	[SerializeField]
	public BasicProjectile sausageString;

	// Token: 0x0400166E RID: 5742
	[SerializeField]
	public TriggerZone[] beanCanTriggerZones;

	// Token: 0x0400166F RID: 5743
	[SerializeField]
	public Effect sausageDeathEffect;

	// Token: 0x04001670 RID: 5744
	[SerializeField]
	public Effect sausageStringDeathEffect;

	// Token: 0x04001671 RID: 5745
	public AbstractPlayerController player;

	// Token: 0x04001672 RID: 5746
	public FlyingCowboyLevelMeat.MeatPhase meatPhase;

	// Token: 0x04001673 RID: 5747
	public DamageDealer damageDealer;

	// Token: 0x04001674 RID: 5748
	public DamageReceiver damageReceiver;

	// Token: 0x04001675 RID: 5749
	public PatternString runningSpitBulletParryPattern;

	// Token: 0x04001676 RID: 5750
	public PatternString sausageTimeToMoveString;

	// Token: 0x04001677 RID: 5751
	public PatternString spitBulletParryString;

	// Token: 0x04001678 RID: 5752
	public bool isFlying;

	// Token: 0x04001679 RID: 5753
	public bool waitingToShoot;

	// Token: 0x0400167A RID: 5754
	public bool isDead;

	// Token: 0x0400167B RID: 5755
	public bool canBulletsTriggered;

	// Token: 0x0400167C RID: 5756
	public int currentSausageLinkSortingOrderA = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder;

	// Token: 0x0400167D RID: 5757
	public int currentSausageLinkSortingOrderB = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + 1;

	// Token: 0x02000CCB RID: 3275
	public enum MeatPhase
	{
		// Token: 0x04005C7E RID: 23678
		Can,
		// Token: 0x04005C7F RID: 23679
		Sausage,
		// Token: 0x04005C80 RID: 23680
		Switching
	}

	// Token: 0x02000CCC RID: 3276
	public enum SausageType
	{
		// Token: 0x04005C82 RID: 23682
		H1,
		// Token: 0x04005C83 RID: 23683
		H2,
		// Token: 0x04005C84 RID: 23684
		H3,
		// Token: 0x04005C85 RID: 23685
		H4,
		// Token: 0x04005C86 RID: 23686
		L5,
		// Token: 0x04005C87 RID: 23687
		U1,
		// Token: 0x04005C88 RID: 23688
		U2,
		// Token: 0x04005C89 RID: 23689
		U3,
		// Token: 0x04005C8A RID: 23690
		D1,
		// Token: 0x04005C8B RID: 23691
		D2,
		// Token: 0x04005C8C RID: 23692
		D3
	}
}
