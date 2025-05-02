using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class PirateLevelBoat : LevelProperties.Pirate.Entity
{
	// Token: 0x0600216C RID: 8556 RVA: 0x0001C818 File Offset: 0x0001AA18
	public override void Awake()
	{
		base.Awake();
		this.idle = new PirateLevelBoat.IdleManager();
		this.ully.gameObject.SetActive(false);
		base.GetComponent<LevelBossDeathExploder>().enabled = false;
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x0001C848 File Offset: 0x0001AA48
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		properties.OnStateChange += this.OnStateChange;
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000BA8A4 File Offset: 0x000B8AA4
	public void OnStateChange()
	{
		base.animator.Play("Idle");
		this.StopAllCoroutines();
		if (base.properties.CurrentState.cannon.firing)
		{
			base.StartCoroutine(this.cannon_cr(base.properties.CurrentState.cannon.delayRange.RandomFloat()));
		}
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x0001C875 File Offset: 0x0001AA75
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.cannonProjectile = null;
		this.cannonSmokePrefab = null;
		this.projectilePrefab = null;
		this.beamPrefab = null;
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000BA908 File Offset: 0x000B8B08
	public void OnIdleEnd()
	{
		if (this.idle.loops >= this.idle.max)
		{
			base.animator.SetTrigger("OnBlink");
			return;
		}
		this.idle.loops++;
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x0001C899 File Offset: 0x0001AA99
	public void OnBlink()
	{
		this.idle.OnBlink();
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x000BA954 File Offset: 0x000B8B54
	public void FireCannon()
	{
		AudioManager.Play("level_pirate_ship_cannon_fire");
		if (!base.properties.CurrentState.cannon.firing)
		{
			return;
		}
		this.cannonSmokePrefab.Create(new Vector2(this.cannonRoot.position.x + 50f, this.cannonRoot.position.y));
		BasicProjectile basicProjectile = this.cannonProjectile.Create(this.cannonRoot.position, 0f, -base.properties.CurrentState.cannon.speed);
		basicProjectile.CollisionDeath.None();
		basicProjectile.DamagesType.OnlyPlayer();
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000BAA18 File Offset: 0x000B8C18
	public IEnumerator cannon_cr(float delay)
	{
		if (delay < 1f)
		{
			delay = 1f;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			base.animator.Play("Cannon");
		}
		yield break;
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x0001C8A6 File Offset: 0x0001AAA6
	public void ChewSound()
	{
		AudioManager.Play("level_pirate_ship_cannon_chew");
		this.emitAudioFromObject.Add("level_pirate_ship_cannon_chew");
	}

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06002175 RID: 8565 RVA: 0x000BAA3C File Offset: 0x000B8C3C
	// (remove) Token: 0x06002176 RID: 8566 RVA: 0x000BAA74 File Offset: 0x000B8C74
	public event Action OnLaunchPirate;

	// Token: 0x06002177 RID: 8567 RVA: 0x0001C8C2 File Offset: 0x0001AAC2
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x0001C8D5 File Offset: 0x0001AAD5
	public void StartTransformation()
	{
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		this.hasTransformed = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.transform_cr());
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x0001C914 File Offset: 0x0001AB14
	public void LaunchPirate()
	{
		CupheadLevelCamera.Current.Shake(15f, 1f, false);
		if (this.OnLaunchPirate != null)
		{
			this.OnLaunchPirate();
		}
		this.SetNewHeight();
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x000BAAAC File Offset: 0x000B8CAC
	public void Shoot()
	{
		AudioManager.Play("level_pirate_ship_uvula_shoot");
		this.emitAudioFromObject.Add("level_pirate_ship_uvula_shoot");
		this.projectilePrefab.Create(this.projectileRoot.position, this.boatProperties.bulletSpeed, this.boatProperties.bulletRotationSpeed);
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000BAB08 File Offset: 0x000B8D08
	public void SetNewHeight()
	{
		Object.FindObjectOfType<PirateLevelBoatContainer>().EndBobbing();
		base.transform.parent.SetLocalPosition(null, new float?(70f), null);
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000BAB4C File Offset: 0x000B8D4C
	public void OnBossDeath()
	{
		this.StopAllCoroutines();
		CupheadLevelCamera.Current.ResetShake();
		if (this.hasTransformed)
		{
			if (this.beam != null)
			{
				this.beam.EndBeam();
			}
			base.animator.SetTrigger("OnDeath");
		}
		else
		{
			base.animator.SetTrigger("OnEasyDeath");
		}
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000BABB8 File Offset: 0x000B8DB8
	public IEnumerator transform_cr()
	{
		this.boatProperties = base.properties.CurrentState.boat;
		base.animator.Play("Idle");
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		base.animator.SetTrigger("OnTransform");
		AudioManager.Play("level_pirate_boat_transform");
		this.emitAudioFromObject.Add("level_pirate_boat_transform");
		yield return CupheadTime.WaitForSeconds(this, this.boatProperties.winceDuration);
		base.animator.SetTrigger("OnTransformContinue");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ully.gameObject.SetActive(true);
		for (;;)
		{
			for (int count = 0; count < this.boatProperties.bulletCount; count++)
			{
				yield return CupheadTime.WaitForSeconds(this, this.boatProperties.attackDelay);
				base.animator.SetTrigger("OnShoot");
			}
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.bulletPostWait);
			base.animator.SetTrigger("OnBeamStart");
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamDelay + 1f);
			base.animator.SetTrigger("OnBeamContinue");
			CupheadLevelCamera.Current.StartShake(2f);
			this.beam = this.beamPrefab.Create(this.beamRoot);
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamDuration);
			base.animator.SetTrigger("OnBeamEnd");
			CupheadLevelCamera.Current.EndShake(0.4f);
			this.beam.EndBeam();
			this.beam = null;
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamPostWait);
			base.animator.Play("Transform_Idle");
		}
		yield break;
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x000BABD4 File Offset: 0x000B8DD4
	public IEnumerator delay_cr(int frameDelay)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		base.StartCoroutine(this.transform_cr());
		yield break;
	}

	// Token: 0x04001B94 RID: 7060
	[SerializeField]
	public DamageReceiver damageReceiver;

	// Token: 0x04001B95 RID: 7061
	[Space(10f)]
	[SerializeField]
	public Transform cannonRoot;

	// Token: 0x04001B96 RID: 7062
	[SerializeField]
	public BasicProjectile cannonProjectile;

	// Token: 0x04001B97 RID: 7063
	[SerializeField]
	public Effect cannonSmokePrefab;

	// Token: 0x04001B98 RID: 7064
	[Space(10f)]
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001B99 RID: 7065
	[SerializeField]
	public Transform beamRoot;

	// Token: 0x04001B9A RID: 7066
	[SerializeField]
	public PirateLevelBoatProjectile projectilePrefab;

	// Token: 0x04001B9B RID: 7067
	[SerializeField]
	public PirateLevelBoatBeam beamPrefab;

	// Token: 0x04001B9C RID: 7068
	[Space(10f)]
	[SerializeField]
	public SpriteRenderer ully;

	// Token: 0x04001B9D RID: 7069
	public PirateLevelBoat.IdleManager idle;

	// Token: 0x04001B9E RID: 7070
	public bool hasTransformed;

	// Token: 0x04001B9F RID: 7071
	public PirateLevelBoatBeam beam;

	// Token: 0x04001BA0 RID: 7072
	public const float Y_TRANSFORMED = 70f;

	// Token: 0x04001BA2 RID: 7074
	public LevelProperties.Pirate.Boat boatProperties;

	// Token: 0x02000E09 RID: 3593
	public class IdleManager
	{
		// Token: 0x06006CF8 RID: 27896 RVA: 0x0004C113 File Offset: 0x0004A313
		public void OnBlink()
		{
			this.max = Random.Range(20, 61);
			this.loops = 0;
		}

		// Token: 0x040065B5 RID: 26037
		public const int MIN_LOOPS = 20;

		// Token: 0x040065B6 RID: 26038
		public const int MAX_LOOPS = 60;

		// Token: 0x040065B7 RID: 26039
		public int loops;

		// Token: 0x040065B8 RID: 26040
		public int max = 20;
	}
}
