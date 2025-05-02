using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class SnowCultLevelJackFrost : LevelProperties.SnowCult.Entity
{
	// Token: 0x06002848 RID: 10312 RVA: 0x000CE058 File Offset: 0x000CC258
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.properties.OnBossDeath += this.OnBossDeath;
		this.rend = base.GetComponent<SpriteRenderer>();
		this.blinkCount = Random.Range(1, 5);
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x00021D04 File Offset: 0x0001FF04
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x00021D1C File Offset: 0x0001FF1C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x00021D2F File Offset: 0x0001FF2F
	public void Intro()
	{
		this.state = SnowCultLevelJackFrost.States.Intro;
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000CE0CC File Offset: 0x000CC2CC
	public void StartPhase3()
	{
		this.bucket.SetActive(true);
		base.gameObject.SetActive(true);
		this.scale = base.transform.localScale;
		this.positionX = base.transform.position.x;
		this.onRight = Rand.Bool();
		this.ChangeSide();
		this.rightSideUp = true;
		this.faceOrientation = new PatternString(base.properties.CurrentState.face.faceOrientationString, true, true);
		this.splitShotPink = new PatternString(base.properties.CurrentState.splitShot.pinkString, true, true);
		this.shotCoord = new PatternString(base.properties.CurrentState.splitShot.shotCoordString, true, true);
		this.splitShotPink.SetSubStringIndex(-1);
		this.shotCoord.SetSubStringIndex(-1);
		this.shardAngleOffsetString = new PatternString(base.properties.CurrentState.shardAttack.angleOffset, true);
		base.StartCoroutine(this.remove_platforms_cr());
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000CE1E0 File Offset: 0x000CC3E0
	public IEnumerator remove_platforms_cr()
	{
		int count = this.presetPlatforms.Length;
		while (count > 0)
		{
			foreach (SnowCultLevelPlatform snowCultLevelPlatform in this.presetPlatforms)
			{
				if (snowCultLevelPlatform != null && snowCultLevelPlatform.transform.position.y < Camera.main.transform.position.y - 450f)
				{
					snowCultLevelPlatform.transform.DetachChildren();
					Object.Destroy(snowCultLevelPlatform.gameObject);
					count--;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x00021D38 File Offset: 0x0001FF38
	public void EndIntro()
	{
		this.state = SnowCultLevelJackFrost.States.Idle;
		this.boxCollider.enabled = true;
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x000CE1FC File Offset: 0x000CC3FC
	public void CreatePlatforms()
	{
		this.presetPlatforms = new SnowCultLevelPlatform[this.platformsPresetPositions.Length];
		this.isClockwise = Rand.Bool();
		LevelProperties.SnowCult.Platforms platforms = base.properties.CurrentState.platforms;
		this.circlePlatforms = new SnowCultLevelPlatform[platforms.platformNum];
		float num = 360f / (float)platforms.platformNum;
		for (int i = 0; i < platforms.platformNum; i++)
		{
			this.circlePlatforms[i] = Object.Instantiate<GameObject>(this.platformPrefab).transform.GetChild(0).GetComponent<SnowCultLevelPlatform>();
			this.circlePlatforms[i].transform.parent.position = this.platformPivotPoint.transform.position;
			this.circlePlatforms[i].StartRotate(num * (float)i, new Vector3(this.platformPivotPoint.transform.position.x, this.platformPivotPoint.transform.position.y + platforms.pivotPointYOffset), platforms.loopSizeX, platforms.loopSizeY, platforms.platformSpeed, platforms.pivotPointYOffset, this.isClockwise);
			this.circlePlatforms[i].SetID(i);
		}
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x000CE334 File Offset: 0x000CC534
	public void CreateAscendingPlatform(int i)
	{
		this.presetPlatforms[i] = Object.Instantiate<GameObject>(this.platformPrefab).transform.GetChild(0).GetComponent<SnowCultLevelPlatform>();
		this.presetPlatforms[i].transform.parent.position = this.platformsPresetPositions[i].transform.position;
		this.presetPlatforms[i].SetID(i);
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x000CE39C File Offset: 0x000CC59C
	public void AniEvent_CheckBlink()
	{
		this.blinkCounter++;
		if (this.blinkCounter >= this.blinkCount)
		{
			base.animator.SetTrigger("Blink");
			this.blinkCount = Random.Range(1, 5);
			this.blinkCounter = 0;
		}
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x00021D4D File Offset: 0x0001FF4D
	public void StartSwitch()
	{
		if (this.firstAttack)
		{
			this.firstAttack = false;
		}
		else
		{
			base.StartCoroutine(this.switch_cr());
		}
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000CE3EC File Offset: 0x000CC5EC
	public IEnumerator switch_cr()
	{
		this.state = SnowCultLevelJackFrost.States.Switch;
		LevelProperties.SnowCult.Face p = base.properties.CurrentState.face;
		bool flippedY = false;
		char c = this.faceOrientation.PopLetter();
		if ((c == 'U' && base.transform.parent.localScale.y == -1f) || (c == 'D' && base.transform.parent.localScale.y == 1f))
		{
			flippedY = true;
		}
		bool isFront = false;
		string triggerName;
		string stateName;
		if (Random.Range(0f, 1f) < 0.25f)
		{
			triggerName = ((!flippedY) ? "FrontSwap" : "FrontSwapFlip");
			stateName = ((!flippedY) ? "SideSwapFront" : "SideSwapFrontFlip");
			isFront = true;
		}
		else
		{
			triggerName = ((!flippedY) ? "BackSwap" : "BackSwapFlip");
			stateName = ((!flippedY) ? "SideSwapBack" : "SideSwapBackFlip");
		}
		base.animator.SetTrigger(triggerName);
		yield return base.animator.WaitForAnimationToEnd(this, "Idle", false, true);
		if (isFront)
		{
			this.rend.sortingLayerName = "Foreground";
		}
		if (flippedY)
		{
			this.rightSideUp = !this.rightSideUp;
		}
		yield return base.animator.WaitForAnimationToEnd(this, stateName, false, true);
		yield return new WaitForEndOfFrame();
		this.rend.sortingLayerName = "Default";
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield break;
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x00021D73 File Offset: 0x0001FF73
	public void FlipParentTransformX()
	{
		this.onRight = !this.onRight;
		this.ChangeSide();
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000CE408 File Offset: 0x000CC608
	public void FlipParentTransformXY()
	{
		base.transform.parent.SetScale(null, new float?((float)((!this.rightSideUp) ? -1 : 1)), null);
		this.FlipParentTransformX();
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000CE458 File Offset: 0x000CC658
	public void ChangeSide()
	{
		base.transform.parent.SetScale(new float?((!this.onRight) ? (-this.scale.x) : this.scale.x), null, null);
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x00021D8A File Offset: 0x0001FF8A
	public void StartEyeAttack()
	{
		base.animator.SetTrigger("EyeAttack");
		this.state = SnowCultLevelJackFrost.States.Eye;
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x00021DA3 File Offset: 0x0001FFA3
	public void aniEvent_LaunchEye()
	{
		base.StartCoroutine(this.eye_attack_cr());
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000CE4B4 File Offset: 0x000CC6B4
	public IEnumerator eye_attack_cr()
	{
		LevelProperties.SnowCult.EyeAttack p = base.properties.CurrentState.eyeAttack;
		this.activeEyeProjectile = this.eyeProjectile.Spawn<SnowCultLevelEyeProjectile>();
		this.activeEyeProjectile.Init(this.eyeRoot.position, this.mouthRoot.position, this.onRight, this.rightSideUp, p);
		this.activeEyeProjectile.main = this;
		while (!this.activeEyeProjectile.readyToOpenMouth)
		{
			yield return null;
		}
		base.animator.SetTrigger("EyeAttackOpenMouth");
		while (!this.activeEyeProjectile.readyToCloseMouth)
		{
			yield return null;
		}
		base.animator.Play("EyeAttackEnd");
		base.animator.Update(0f);
		this.SFX_SNOWCULT_JackFrostEyeballReturn();
		yield return base.animator.WaitForAnimationToEnd(this, "EyeAttackEnd", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield break;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x00021DB2 File Offset: 0x0001FFB2
	public void AniEvent_RemoveEye()
	{
		this.activeEyeProjectile.ReturnToSnowflake();
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x00021DBF File Offset: 0x0001FFBF
	public void StartShardAttack()
	{
		base.StartCoroutine(this.shard_attack_cr());
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000CE4D0 File Offset: 0x000CC6D0
	public IEnumerator shard_attack_cr()
	{
		this.state = SnowCultLevelJackFrost.States.Shard;
		LevelProperties.SnowCult.ShardAttack p = base.properties.CurrentState.shardAttack;
		float degrees = 360f / (float)p.shardNumber;
		float loopSizeX = p.circleSizeX;
		float loopSizeY = p.circleSizeY;
		SnowCultLevelShard[] shards = new SnowCultLevelShard[p.shardNumber];
		string[] angleOffsetString = p.angleOffset.Split(new char[]
		{
			','
		});
		float angleOffset = this.shardAngleOffsetString.PopFloat();
		List<float> angleList = new List<float>();
		for (int k = 0; k < p.shardNumber; k++)
		{
			angleList.Add((degrees * (float)k + angleOffset) % 360f);
		}
		angleList.Sort((float a, float b) => ((a + 90f) % 360f).CompareTo((b + 90f) % 360f));
		this.iceCreamGhostRenderer.sortingOrder = -12;
		base.animator.SetTrigger("IceCreamAttack");
		yield return base.animator.WaitForAnimationToStart(this, "IceCream", false);
		this.SFX_SNOWCULT_JackFrostIcecream();
		YieldInstruction wait = new WaitForFixedUpdate();
		int count = 0;
		float sparkleDelay = Random.Range(0.1f, 0.3f);
		while (count < p.shardNumber)
		{
			float normalizedAngle = Mathf.InverseLerp(0.116279073f, 0.7906977f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			sparkleDelay -= CupheadTime.FixedDelta;
			if (sparkleDelay <= 0f)
			{
				float num = (normalizedAngle + 0.25f) % 1f * 3.14159274f * 2f;
				this.iceCreamSparkle.Create(this.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.right * base.transform.parent.localScale.x * Mathf.Sin(num) * loopSizeX + Vector3.down * base.transform.parent.localScale.y * Mathf.Cos(num) * loopSizeY);
				sparkleDelay += Random.Range(0.1f, 0.3f);
			}
			if ((float)count < normalizedAngle * (float)p.shardNumber)
			{
				float num2 = angleList[count];
				if (base.transform.parent.localScale.x < 0f)
				{
					num2 = 360f - num2;
				}
				if (base.transform.parent.localScale.y < 0f)
				{
					num2 = (num2 + (90f - num2) * 2f) % 360f;
				}
				SnowCultLevelShard snowCultLevelShard = this.shardPrefab.Spawn<SnowCultLevelShard>();
				snowCultLevelShard.Init(this.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.forward * (float)count * 0.001f, num2, loopSizeX, loopSizeY, p);
				shards[count] = snowCultLevelShard;
				count++;
			}
			yield return wait;
		}
		yield return CupheadTime.WaitForSeconds(this, p.warningLength);
		for (int l = 0; l < shards.Length; l++)
		{
			shards[l].Appear();
		}
		yield return CupheadTime.WaitForSeconds(this, p.shardHesitation);
		if (this.isClockwise)
		{
			for (int i = shards.Length - 1; i >= 0; i--)
			{
				yield return CupheadTime.WaitForSeconds(this, p.shardDelay);
				if (shards[i] != null)
				{
					shards[i].LaunchProjectile();
				}
			}
		}
		else
		{
			for (int j = 0; j < shards.Length; j++)
			{
				yield return CupheadTime.WaitForSeconds(this, p.shardDelay);
				if (shards[j] != null)
				{
					shards[j].LaunchProjectile();
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x00021DCE File Offset: 0x0001FFCE
	public void AniEvent_SetGhostLayerBehindSnowflakeMiddleLayer()
	{
		this.iceCreamGhostRenderer.sortingOrder = -17;
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x00021DDD File Offset: 0x0001FFDD
	public void StartMouthShot()
	{
		base.StartCoroutine(this.split_shot_cr());
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x000CE4EC File Offset: 0x000CC6EC
	public IEnumerator split_shot_cr()
	{
		LevelProperties.SnowCult.SplitShot p = base.properties.CurrentState.splitShot;
		this.state = SnowCultLevelJackFrost.States.SplitShot;
		float posX = 0f;
		float posY = 0f;
		int timesToShoot = this.shotCoord.SubStringLength();
		base.animator.SetBool("SplitShot", true);
		yield return base.animator.WaitForAnimationToStart(this, "SplitShotStart", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingStart();
		yield return base.animator.WaitForAnimationToStart(this, "SplitShotAnti", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop();
		for (int i = 0; i < timesToShoot; i++)
		{
			posY = this.shotCoord.PopFloat();
			posX = (float)((!this.onRight) ? 640 : -640);
			Vector3 pos = new Vector3(posX, base.transform.position.y + posY);
			Vector3 dir = pos - this.splitShotRoot.position;
			SnowCultLevelSplitShotBullet splitShot = (this.splitShotPink.PopLetter() != 'P') ? this.mouthPrefab.Spawn<SnowCultLevelSplitShotBullet>() : this.mouthPinkPrefab.Spawn<SnowCultLevelSplitShotBullet>();
			splitShot.Init(this.splitShotRoot.position, MathUtils.DirectionToAngle(dir), p.shotSpeed, p.shatterCount, p.spreadAngle, p);
			splitShot.transform.localScale = new Vector3(base.transform.parent.localScale.x, 1f);
			splitShot.main = this;
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay - 0.45f);
			if (splitShot)
			{
				splitShot.Grow();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.45f);
			if (splitShot)
			{
				base.animator.SetTrigger("SplitShotFire");
				this.SFX_SNOWCULT_JackFrostSplitshotBucketLaunch();
				while (!this.fireSplitShot)
				{
					yield return null;
				}
				this.fireSplitShot = false;
				if (splitShot)
				{
					splitShot.Fire();
				}
			}
		}
		base.animator.SetBool("SplitShot", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop();
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x00021DEC File Offset: 0x0001FFEC
	public void AniEvent_FireSplitShot()
	{
		this.fireSplitShot = true;
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000CE508 File Offset: 0x000CC708
	public void OnBossDeath()
	{
		this.dead = true;
		base.transform.parent.SetScale(null, new float?(1f), null);
		base.animator.Play((!this.rightSideUp) ? "FlipDeath" : "Death");
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x00021DF5 File Offset: 0x0001FFF5
	public void EnableWizardDeathAnimation()
	{
		this.wizardDeath.SetActive(true);
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x00021E03 File Offset: 0x00020003
	public void AnimationEvent_SFX_SNOWCULT_JackFrostIntroThumblick()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_intro_thumblick");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_intro_thumblick");
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x00021E1F File Offset: 0x0002001F
	public void AnimationEvent_SFX_SNOWCULT_JackFrostEyeballAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack");
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x00021E3B File Offset: 0x0002003B
	public void SFX_SNOWCULT_JackFrostEyeballReturn()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_return");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_return");
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x00021E57 File Offset: 0x00020057
	public void SFX_SNOWCULT_JackFrostIcecream()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamattack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamattack");
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x00021E73 File Offset: 0x00020073
	public void AnimationEvent_SFX_SNOWCULT_JackFrostSideSwap()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_sideswap");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_sideswap");
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x00021E8F File Offset: 0x0002008F
	public void SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x00021EAB File Offset: 0x000200AB
	public void SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x00021EB7 File Offset: 0x000200B7
	public void SFX_SNOWCULT_JackFrostSplitshotHandwavingStart()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start");
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x00021ED3 File Offset: 0x000200D3
	public void SFX_SNOWCULT_JackFrostSplitshotBucketLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch");
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x00021EEF File Offset: 0x000200EF
	public void AnimationEvent_SFX_SNOWCULT_JackFrostDeath()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_death");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_death");
	}

	// Token: 0x0400217D RID: 8573
	public const int BLINK_LOOP_COUNT_MIN = 1;

	// Token: 0x0400217E RID: 8574
	public const int BLINK_LOOP_COUNT_MAX = 5;

	// Token: 0x0400217F RID: 8575
	[SerializeField]
	public BoxCollider2D boxCollider;

	// Token: 0x04002180 RID: 8576
	[SerializeField]
	public SnowCultLevelSplitShotBullet mouthPrefab;

	// Token: 0x04002181 RID: 8577
	[SerializeField]
	public SnowCultLevelSplitShotBullet mouthPinkPrefab;

	// Token: 0x04002182 RID: 8578
	[SerializeField]
	public SnowCultLevelShard shardPrefab;

	// Token: 0x04002183 RID: 8579
	[SerializeField]
	public Effect iceCreamSparkle;

	// Token: 0x04002184 RID: 8580
	[SerializeField]
	public SnowCultLevelEyeProjectile eyeProjectile;

	// Token: 0x04002185 RID: 8581
	public SnowCultLevelEyeProjectile activeEyeProjectile;

	// Token: 0x04002186 RID: 8582
	public Transform eyeProjectileGuide;

	// Token: 0x04002187 RID: 8583
	[SerializeField]
	public Transform eyeRoot;

	// Token: 0x04002188 RID: 8584
	[SerializeField]
	public Transform mouthRoot;

	// Token: 0x04002189 RID: 8585
	[SerializeField]
	public Transform splitShotRoot;

	// Token: 0x0400218A RID: 8586
	[SerializeField]
	public Transform platformPivotPoint;

	// Token: 0x0400218B RID: 8587
	[SerializeField]
	public GameObject platformPrefab;

	// Token: 0x0400218C RID: 8588
	[SerializeField]
	public Transform[] platformsPresetPositions;

	// Token: 0x0400218D RID: 8589
	[SerializeField]
	public GameObject wizardDeath;

	// Token: 0x0400218E RID: 8590
	[SerializeField]
	public GameObject bucket;

	// Token: 0x0400218F RID: 8591
	[SerializeField]
	public SpriteRenderer iceCreamGhostRenderer;

	// Token: 0x04002190 RID: 8592
	public bool onRight;

	// Token: 0x04002191 RID: 8593
	public bool rightSideUp;

	// Token: 0x04002192 RID: 8594
	public bool isClockwise;

	// Token: 0x04002193 RID: 8595
	public bool firstAttack = true;

	// Token: 0x04002194 RID: 8596
	public float positionX;

	// Token: 0x04002195 RID: 8597
	public Vector3 scale;

	// Token: 0x04002196 RID: 8598
	public SnowCultLevelPlatform[] presetPlatforms;

	// Token: 0x04002197 RID: 8599
	public SnowCultLevelPlatform[] circlePlatforms;

	// Token: 0x04002198 RID: 8600
	public AbstractPlayerController player;

	// Token: 0x04002199 RID: 8601
	public DamageDealer damageDealer;

	// Token: 0x0400219A RID: 8602
	public DamageReceiver damageReceiver;

	// Token: 0x0400219B RID: 8603
	public SnowCultLevelJackFrost.States state;

	// Token: 0x0400219C RID: 8604
	public PatternString faceOrientation;

	// Token: 0x0400219D RID: 8605
	public PatternString splitShotPink;

	// Token: 0x0400219E RID: 8606
	public PatternString shotCoord;

	// Token: 0x0400219F RID: 8607
	public PatternString shardAngleOffsetString;

	// Token: 0x040021A0 RID: 8608
	public SpriteRenderer rend;

	// Token: 0x040021A1 RID: 8609
	public bool fireSplitShot;

	// Token: 0x040021A2 RID: 8610
	public int blinkCounter;

	// Token: 0x040021A3 RID: 8611
	public int blinkCount;

	// Token: 0x040021A4 RID: 8612
	public bool dead;

	// Token: 0x02000F6A RID: 3946
	public enum States
	{
		// Token: 0x04006F1E RID: 28446
		Intro,
		// Token: 0x04006F1F RID: 28447
		Idle,
		// Token: 0x04006F20 RID: 28448
		Switch,
		// Token: 0x04006F21 RID: 28449
		Eye,
		// Token: 0x04006F22 RID: 28450
		Beam,
		// Token: 0x04006F23 RID: 28451
		Hazard,
		// Token: 0x04006F24 RID: 28452
		Shard,
		// Token: 0x04006F25 RID: 28453
		SplitShot,
		// Token: 0x04006F26 RID: 28454
		Arc
	}
}
