using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class OldManLevelGnomeLeader : LevelProperties.OldMan.Entity
{
	// Token: 0x06002059 RID: 8281 RVA: 0x000B7D30 File Offset: 0x000B5F30
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.transform.SetPosition(null, new float?(this.baseHeight + Mathf.Sin(this.GetPosition() * 3.14159274f) * this.heightRange), null);
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x0001B7B0 File Offset: 0x000199B0
	public void Update()
	{
		this.NontargetablePlatformCount();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x0001B7CF File Offset: 0x000199CF
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
		this.parryThermometer.gameObject.SetActive(false);
		this.parryString = new PatternString(properties.CurrentState.gnomeLeader.shotParryString, true);
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x0001B805 File Offset: 0x00019A05
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.StartDeath();
		}
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x0001B833 File Offset: 0x00019A33
	public void StartDeath()
	{
		this.isAlive = false;
		this.coll.enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("Dead");
		this.SFX_Death();
		AudioManager.Stop("sfx_dlc_omm_p3_ulcer_movement_loop");
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000B7DB4 File Offset: 0x000B5FB4
	public void StartGnomeLeader()
	{
		LevelProperties.OldMan.GnomeLeader gnomeLeader = base.properties.CurrentState.gnomeLeader;
		this.isAlive = true;
		this.pit.SetActive(true);
		this.stomachPlatforms = new OldManLevelStomachPlatform[this.platformPositions.Length];
		for (int i = 0; i < this.stomachPlatforms.Length; i++)
		{
			this.stomachPlatforms[i] = Object.Instantiate<OldManLevelStomachPlatform>(this.stomachPlatformPrefab);
			this.stomachPlatforms[i].transform.position = this.platformPositions[i].position;
			if (i < 3)
			{
				this.stomachPlatforms[i].FlipX();
			}
			this.stomachPlatforms[i].sparkAnimator = this.platformPositions[i].GetComponent<Animator>();
			this.stomachPlatforms[i].main = this;
		}
		base.StartCoroutine(this.moving_cr());
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000B7E8C File Offset: 0x000B608C
	public float GetPosition()
	{
		return Mathf.InverseLerp((float)Level.Current.Right - this.screenEdgeOffset, (float)Level.Current.Left + this.screenEdgeOffset, base.transform.position.x);
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000B7ED8 File Offset: 0x000B60D8
	public IEnumerator moving_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.movingRight = MathUtils.RandomBool();
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_introlaugh");
		base.animator.Play((!this.movingRight) ? "IntroRight" : "IntroLeft");
		yield return wait;
		if (!this.movingRight)
		{
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
			{
				yield return null;
			}
			base.animator.Play("Idle");
			base.animator.Update(0f);
			base.transform.localScale = new Vector3(1f, 1f);
		}
		else
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		}
		this.SFX_MoveLoop();
		base.StartCoroutine(this.gnome_leader_cr());
		this.timeForScreenCross = base.properties.CurrentState.gnomeLeader.bossMoveTime;
		this.locationEnd = 0f;
		AnimationHelper animHelper = base.animator.GetComponent<AnimationHelper>();
		for (;;)
		{
			this.locationTime = 0f;
			this.locationStart = base.transform.position.x;
			if (this.movingRight)
			{
				this.locationEnd = (float)Level.Current.Right - this.screenEdgeOffset;
			}
			else
			{
				this.locationEnd = (float)Level.Current.Left + this.screenEdgeOffset;
			}
			while (this.locationTime < this.timeForScreenCross)
			{
				base.transform.SetPosition(new float?(this.GetXPositionAtTimeValue(this.locationTime)), new float?(this.baseHeight + Mathf.Sin(this.GetPosition() * 3.14159274f) * this.heightRange), null);
				base.transform.SetEulerAngles(null, null, new float?(Mathf.Lerp((float)((!this.movingRight) ? -7 : 7), (float)((!this.movingRight) ? 7 : -7), this.locationTime / this.timeForScreenCross)));
				this.locationTime += CupheadTime.FixedDelta;
				if (this.turnTrigger && this.locationTime / this.timeForScreenCross > 0.8f)
				{
					base.animator.SetTrigger("OnTurn");
					this.turning = true;
					this.turnTrigger = false;
				}
				if (PauseManager.state != PauseManager.State.Paused)
				{
					float num = Mathf.Sin(Mathf.InverseLerp(this.locationStart, this.locationEnd, base.transform.position.x) * 3.14159274f);
					animHelper.Speed = 1f + num * (this.topAnimSpeed / 24f - 1f);
					this.SFXLoopVolume = 0.0001f + num * 0.3f;
					if (!this.spitting)
					{
						AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", this.SFXLoopVolume, 1E-05f);
					}
				}
				yield return wait;
			}
			this.turnTrigger = true;
			base.transform.SetPosition(new float?(this.locationEnd), null, null);
			this.movingRight = !this.movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000B7EF4 File Offset: 0x000B60F4
	public float GetXPositionAtTimeValue(float time)
	{
		if (time > this.timeForScreenCross)
		{
			float value = time % this.timeForScreenCross / this.timeForScreenCross;
			return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.locationEnd, this.locationStart, value);
		}
		float value2 = time / this.timeForScreenCross;
		return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.locationStart, this.locationEnd, value2);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000B7F50 File Offset: 0x000B6150
	public void AniEvent_Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		this.turning = false;
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000B7F9C File Offset: 0x000B619C
	public int NontargetablePlatformCount()
	{
		int num = 0;
		for (int i = 0; i < this.stomachPlatforms.Length; i++)
		{
			if (!this.stomachPlatforms[i].isActivated || this.stomachPlatforms[i].isTargeted)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000B7FF0 File Offset: 0x000B61F0
	public IEnumerator gnome_leader_cr()
	{
		LevelProperties.OldMan.GnomeLeader p = base.properties.CurrentState.gnomeLeader;
		PatternString platformCountToRemove = new PatternString(p.platformParryString, true, true);
		PatternString shotDelayString = new PatternString(p.shotDelayString, true, true);
		this.sequenceMainIndex = Random.Range(0, p.shotPlatformString.Length);
		int numOfPlatformsToDestroy = 0;
		int platformToTarget = 0;
		bool projectileSpawnsParryable = false;
		while (this.isAlive)
		{
			this.restartSequence = false;
			projectileSpawnsParryable = false;
			this.currentTongue = -1;
			this.sequenceMainIndex = (this.sequenceMainIndex + 1) % p.shotPlatformString.Length;
			PatternString shotPlatformString = new PatternString(p.shotPlatformString[this.sequenceMainIndex], true);
			shotPlatformString.SetSubStringIndex(-1);
			this.sequenceIndex = 0;
			for (int i = 0; i < 5; i++)
			{
				this.sequence[i] = shotPlatformString.PopInt();
			}
			numOfPlatformsToDestroy = platformCountToRemove.PopInt();
			while (!this.restartSequence)
			{
				yield return CupheadTime.WaitForSeconds(this, shotDelayString.PopFloat());
				if (this.NontargetablePlatformCount() < 5 && !this.restartSequence)
				{
					while (this.turning)
					{
						yield return null;
					}
					base.animator.SetTrigger("Spit");
					int count = 0;
					do
					{
						platformToTarget = this.sequence[this.sequenceIndex];
						this.sequenceIndex = (this.sequenceIndex + 1) % 5;
						count++;
					}
					while ((!this.stomachPlatforms[platformToTarget].isActivated || this.stomachPlatforms[platformToTarget].isTargeted) && count < 5);
					if (this.currentTongue == -1 && this.NontargetablePlatformCount() >= numOfPlatformsToDestroy)
					{
						projectileSpawnsParryable = true;
						this.currentTongue = platformToTarget;
						base.StartCoroutine(this.wait_to_parry_cr());
					}
					else
					{
						projectileSpawnsParryable = false;
					}
					this.SFX_PreSpit();
					while (!this.readyToSpit)
					{
						yield return null;
					}
					yield return base.StartCoroutine(this.shoot_cr(this.stomachPlatforms[platformToTarget], projectileSpawnsParryable));
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000B800C File Offset: 0x000B620C
	public IEnumerator wait_to_parry_cr()
	{
		while (!this.parryThermometer.isActivated)
		{
			yield return null;
		}
		foreach (OldManLevelStomachPlatform oldManLevelStomachPlatform in this.stomachPlatforms)
		{
			oldManLevelStomachPlatform.ActivatePlatform();
		}
		this.restartSequence = true;
		yield break;
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x0001B86E File Offset: 0x00019A6E
	public void SpawnParryable(Vector3 spawnPosition)
	{
		this.parryThermometer.transform.position = spawnPosition;
		this.parryThermometer.gameObject.SetActive(true);
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000B8028 File Offset: 0x000B6228
	public IEnumerator shoot_cr(OldManLevelStomachPlatform selectedPlatform, bool projectileSpawnsParryable)
	{
		float predictedPos = this.GetXPositionAtTimeValue(this.locationTime + 0.5416667f);
		this.isBehind = false;
		bool willBeMovingRight = this.movingRight;
		if (this.locationTime + 0.5416667f > this.timeForScreenCross)
		{
			willBeMovingRight = !willBeMovingRight;
		}
		if (willBeMovingRight)
		{
			this.isBehind = (predictedPos + 250f > selectedPlatform.transform.position.x);
		}
		else
		{
			this.isBehind = (predictedPos - 250f < selectedPlatform.transform.position.x);
		}
		if (this.turning)
		{
			this.isBehind = !this.isBehind;
		}
		string animationName;
		if (Mathf.Abs(predictedPos - selectedPlatform.transform.position.x) < 350f)
		{
			base.animator.SetTrigger((!this.isBehind) ? "SpitForwardClose" : "SpitBehindClose");
			animationName = ((!this.isBehind) ? "Spit_Forward_Close" : "Spit_Behind_Close");
			this.spitVFXAnimator.transform.localPosition = this.spitRoots[0].transform.localPosition;
		}
		else
		{
			base.animator.SetTrigger((!this.isBehind) ? "SpitForward" : "SpitBehind");
			animationName = ((!this.isBehind) ? "Spit_Forward" : "Spit_Behind");
			this.spitVFXAnimator.transform.localPosition = this.spitRoots[1].transform.localPosition;
		}
		yield return base.animator.WaitForAnimationToStart(this, animationName, false);
		this.spitting = true;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", Mathf.Min(0.25f, this.SFXLoopVolume), 0.25f);
		this.SFX_StartSpit();
		this.readyToSpit = false;
		while (!this.spitFrame)
		{
			yield return null;
		}
		LevelProperties.OldMan.GnomeLeader p = base.properties.CurrentState.gnomeLeader;
		this.spitVFXAnimator.transform.localPosition = new Vector3(Mathf.Abs(this.spitVFXAnimator.transform.localPosition.x) * (float)((!this.isBehind) ? -1 : 1), this.spitVFXAnimator.transform.localPosition.y);
		this.spitVFXAnimator.transform.localScale = new Vector3(Mathf.Sign(this.spitVFXAnimator.transform.localPosition.x), 1f);
		Vector3 startPos = this.spitVFXAnimator.transform.position;
		float x = selectedPlatform.transform.position.x - startPos.x;
		float y = selectedPlatform.transform.position.y - startPos.y;
		float timeToApex = p.shotApexTime;
		float height = p.shotApexHeight;
		float apexTime2 = timeToApex * timeToApex;
		float g = -2f * height / apexTime2;
		float viY = 2f * height / timeToApex;
		float viX2 = viY * viY;
		float sqrtRooted = viX2 + 2f * g * y;
		float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd3 = Mathf.Max(tEnd, tEnd2);
		float velocityX = x / tEnd3;
		Vector3 speed = new Vector3(velocityX, viY);
		selectedPlatform.Anticipation();
		this.spitVFXAnimator.Play("SpitSmoke");
		this.spitVFXAnimator.Update(0f);
		OldManLevelGnomeProjectile projectile = this.projectilePrefab.Spawn<OldManLevelGnomeProjectile>();
		projectile.Init(startPos, speed, g, projectileSpawnsParryable, this.parryString.PopLetter() == 'P' && !projectileSpawnsParryable, selectedPlatform);
		this.SFX_SpawnProjectile();
		this.spitFrame = false;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", this.SFXLoopVolume, 1f);
		this.spitting = false;
		yield break;
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x0001B892 File Offset: 0x00019A92
	public void AniEvent_ReadyToSpit()
	{
		this.readyToSpit = true;
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x0001B89B File Offset: 0x00019A9B
	public void AniEvent_Shoot()
	{
		this.spitFrame = true;
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x000B8054 File Offset: 0x000B6254
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.platformPositions != null)
		{
			foreach (Transform transform in this.platformPositions)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, 50f);
			}
		}
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x0001B8A4 File Offset: 0x00019AA4
	public void SFX_MoveLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_p3_ulcer_movement_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_movement_loop");
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x0001B8C0 File Offset: 0x00019AC0
	public void SFX_PreSpit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_bonespitpre");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_bonespitpre");
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x0001B8DC File Offset: 0x00019ADC
	public void SFX_StartSpit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_spitbonevocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_spitbonevocal");
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x0001B8F8 File Offset: 0x00019AF8
	public void SFX_SpawnProjectile()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcerspitbone");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcerspitbone");
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x0001B914 File Offset: 0x00019B14
	public void SFX_Death()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_deathvocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_deathvocal");
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0f, 2f);
	}

	// Token: 0x04001A73 RID: 6771
	public const float DROP_Y = 5f;

	// Token: 0x04001A74 RID: 6772
	public const int BULLET_COUNT = 4;

	// Token: 0x04001A75 RID: 6773
	public const float SPIT_DELAY = 0.5416667f;

	// Token: 0x04001A76 RID: 6774
	public const float SPIT_DISTANCE_OFFSET = 250f;

	// Token: 0x04001A77 RID: 6775
	public const float CLOSE_SPIT_ANIM_RANGE = 350f;

	// Token: 0x04001A78 RID: 6776
	[SerializeField]
	public OldManLevelParryThermometer parryThermometer;

	// Token: 0x04001A79 RID: 6777
	public OldManLevelSplashHandler splashHandler;

	// Token: 0x04001A7A RID: 6778
	[SerializeField]
	public GameObject pit;

	// Token: 0x04001A7B RID: 6779
	[SerializeField]
	public Transform[] spitRoots;

	// Token: 0x04001A7C RID: 6780
	[SerializeField]
	public Animator spitVFXAnimator;

	// Token: 0x04001A7D RID: 6781
	[SerializeField]
	public OldManLevelGnomeProjectile projectilePrefab;

	// Token: 0x04001A7E RID: 6782
	[SerializeField]
	public OldManLevelStomachPlatform stomachPlatformPrefab;

	// Token: 0x04001A7F RID: 6783
	public OldManLevelStomachPlatform[] stomachPlatforms;

	// Token: 0x04001A80 RID: 6784
	public int currentTongue = -1;

	// Token: 0x04001A81 RID: 6785
	[SerializeField]
	public Transform[] platformPositions;

	// Token: 0x04001A82 RID: 6786
	public DamageDealer damageDealer;

	// Token: 0x04001A83 RID: 6787
	public DamageReceiver damageReceiver;

	// Token: 0x04001A84 RID: 6788
	public AbstractPlayerController player;

	// Token: 0x04001A85 RID: 6789
	public bool isAlive;

	// Token: 0x04001A86 RID: 6790
	public bool movingRight;

	// Token: 0x04001A87 RID: 6791
	public bool readyToSpit;

	// Token: 0x04001A88 RID: 6792
	public bool spitFrame;

	// Token: 0x04001A89 RID: 6793
	public bool spitting;

	// Token: 0x04001A8A RID: 6794
	public bool restartSequence;

	// Token: 0x04001A8B RID: 6795
	public int[] sequence = new int[5];

	// Token: 0x04001A8C RID: 6796
	public int sequenceIndex;

	// Token: 0x04001A8D RID: 6797
	public int sequenceMainIndex;

	// Token: 0x04001A8E RID: 6798
	public float locationTime;

	// Token: 0x04001A8F RID: 6799
	public float locationStart;

	// Token: 0x04001A90 RID: 6800
	public float locationEnd;

	// Token: 0x04001A91 RID: 6801
	public float timeForScreenCross;

	// Token: 0x04001A92 RID: 6802
	public bool turnTrigger = true;

	// Token: 0x04001A93 RID: 6803
	public bool turning;

	// Token: 0x04001A94 RID: 6804
	public PatternString parryString;

	// Token: 0x04001A95 RID: 6805
	public bool isBehind;

	// Token: 0x04001A96 RID: 6806
	public float screenEdgeOffset = 200f;

	// Token: 0x04001A97 RID: 6807
	[SerializeField]
	public float baseHeight = 188f;

	// Token: 0x04001A98 RID: 6808
	[SerializeField]
	public float topAnimSpeed = 30f;

	// Token: 0x04001A99 RID: 6809
	[SerializeField]
	public float heightRange;

	// Token: 0x04001A9A RID: 6810
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04001A9B RID: 6811
	public float SFXLoopVolume;
}
