using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class FrogsLevelMorphed : LevelProperties.Frogs.Entity
{
	// Token: 0x06001E10 RID: 7696 RVA: 0x000B24A8 File Offset: 0x000B06A8
	public override void Awake()
	{
		base.Awake();
		FrogsLevelMorphed.Current = this;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.slots.Init(this);
		this.handle = FrogsLevelMorphedSwitch.Create(this);
		this.handle.enabled = false;
		this.handle.OnActivate += this.OnHandleActivated;
		this.slotsParent.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x000B2550 File Offset: 0x000B0750
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (FrogsLevelMorphed.Current == this)
		{
			FrogsLevelMorphed.Current = null;
		}
		this.coin = null;
		this.snakeBullet = null;
		this.oniBullet = null;
		this.bisonBullet = null;
		this.tigerBullet = null;
		this.dustEffect = null;
		this.slots.OnDestroy();
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000195CF File Offset: 0x000177CF
	public void Start()
	{
		this.damageReceiver.enabled = false;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000195DD File Offset: 0x000177DD
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000195EA File Offset: 0x000177EA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		AudioManager.Play("level_frogs_short_clap_shock");
		this.emitAudioFromObject.Add("level_frogs_short_clap_shock");
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00019622 File Offset: 0x00017822
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x0001962B File Offset: 0x0001782B
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000B25B0 File Offset: 0x000B07B0
	public void Enable(bool demonTriggered)
	{
		this.demonTriggered = demonTriggered;
		base.gameObject.SetActive(true);
		this.dustEffect.gameObject.SetActive(true);
		base.properties.OnBossDeath += this.OnBossDeath;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x0001963E File Offset: 0x0001783E
	public void OnBossDeath()
	{
		AudioManager.PlayLoop("level_frogs_morphed_death_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_death_loop");
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		this.slotsParent.SetActive(false);
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000B2614 File Offset: 0x000B0814
	public void ShootCoin()
	{
		AudioManager.Play("level_frogs_morphed_mouth");
		this.emitAudioFromObject.Add("level_frogs_morphed_mouth");
		this.coinRoot.LookAt2D(PlayerManager.GetNext().center);
		FrogsLevelMorphedCoin frogsLevelMorphedCoin = this.coin.CreateCoin(this.coinRoot.position, this.coinSpeed, this.coinRoot.eulerAngles.z);
		frogsLevelMorphedCoin.transform.SetPosition(null, null, new float?(-600f));
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x0001967C File Offset: 0x0001787C
	public void StartShooting()
	{
		this.EndShooting();
		this.shootingCoroutine = this.shootingLoop_cr();
		base.StartCoroutine(this.shootingCoroutine);
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x0001969D File Offset: 0x0001789D
	public void EndShooting()
	{
		if (this.shootingCoroutine != null)
		{
			base.StopCoroutine(this.shootingCoroutine);
		}
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000196B6 File Offset: 0x000178B6
	public void OnHandleActivated()
	{
		this.handleActivated = true;
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000B26AC File Offset: 0x000B08AC
	public IEnumerator loop_cr()
	{
		if (this.demonTriggered)
		{
			this.mainIndex = Random.Range(0, base.properties.CurrentState.demon.demonString.Length);
			this.index = Random.Range(0, base.properties.CurrentState.demon.demonString[this.mainIndex].Split(new char[]
			{
				','
			}).Length);
		}
		AudioManager.Play("level_frogs_morphed_open");
		this.emitAudioFromObject.Add("level_frogs_morphed_open");
		this.slotsParent.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.Play("Open");
		AudioManager.Play("level_frogs_morphed_open");
		this.emitAudioFromObject.Add("level_frogs_morphed_open");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			LevelProperties.Frogs.Morph p = base.properties.CurrentState.morph;
			this.StartShooting();
			yield return CupheadTime.WaitForSeconds(this, p.armDownDelay);
			yield return base.StartCoroutine(this.waitForActivate_cr());
			this.EndShooting();
			base.animator.SetTrigger("OnActivated");
			yield return base.StartCoroutine(this.pattern_cr(p));
			this.slotsParent.SetActive(true);
			base.animator.SetTrigger("OnAttackEnd");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
		}
		yield break;
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000B26C8 File Offset: 0x000B08C8
	public IEnumerator waitForActivate_cr()
	{
		this.handleActivated = false;
		this.handle.enabled = true;
		base.animator.SetTrigger("OnArmDown");
		AudioManager.Play("level_frogs_morphed_arm_down");
		this.emitAudioFromObject.Add("level_frogs_morphed_arm_down");
		while (!this.handleActivated)
		{
			yield return null;
		}
		this.handle.enabled = false;
		yield break;
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000B26E4 File Offset: 0x000B08E4
	public IEnumerator shootingLoop_cr()
	{
		LevelProperties.Frogs.Morph p = base.properties.CurrentState.morph;
		float time = p.coinMinMaxTime;
		float t = 0f;
		float val = 0f;
		float coinDelay = 0f;
		for (;;)
		{
			float delay = p.coinDelay.GetFloatAt(val);
			this.coinSpeed = p.coinSpeed.GetFloatAt(val);
			if (coinDelay >= delay)
			{
				base.animator.SetTrigger("OnShoot");
				coinDelay = 0f;
			}
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			coinDelay += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000B2700 File Offset: 0x000B0900
	public IEnumerator pattern_cr(LevelProperties.Frogs.Morph p)
	{
		Slots.Mode mode = Slots.Mode.Snake;
		if (!this.demonTriggered)
		{
			int num = Random.Range(0, 3);
			mode = (Slots.Mode)num;
		}
		else
		{
			mode = Slots.Mode.Oni;
			yield return null;
		}
		this.slots.Spin();
		yield return CupheadTime.WaitForSeconds(this, 3f * p.slotSelectionDurationPercentage);
		this.slots.Stop(mode);
		yield return CupheadTime.WaitForSeconds(this, 1f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.damageReceiver.enabled = true;
		base.animator.SetTrigger("OnAttack");
		AudioManager.Play("level_frogs_morphed_attack");
		this.emitAudioFromObject.Add("level_frogs_morphed_attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		this.slotsParent.SetActive(false);
		AudioManager.PlayLoop("level_frogs_platform_loop");
		this.emitAudioFromObject.Add("level_frogs_platform_loop");
		switch (mode)
		{
		case Slots.Mode.Snake:
			yield return base.StartCoroutine(this.snake_cr(p));
			break;
		case Slots.Mode.Tiger:
			yield return base.StartCoroutine(this.tiger_cr(p));
			break;
		case Slots.Mode.Bison:
			yield return base.StartCoroutine(this.bison_cr(p));
			break;
		case Slots.Mode.Oni:
			yield return base.StartCoroutine(this.oni_cr());
			break;
		}
		AudioManager.Stop("level_frogs_platform_loop");
		this.damageReceiver.enabled = false;
		yield break;
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000196BF File Offset: 0x000178BF
	public void ShootSnake(float speed)
	{
		this.snakeBullet.Create(this.slotBulletRoot.position, speed);
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000196DE File Offset: 0x000178DE
	public void ShootBison(float speed, FrogsLevelBisonBullet.Direction dir, float bigX, float smallX)
	{
		this.bisonBullet.Create(this.slotBulletRoot.position, speed, dir, bigX, smallX);
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x00019701 File Offset: 0x00017901
	public void ShootTiger(float speed)
	{
		this.tigerBullet.Create(this.slotBulletRoot.position, speed);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x00019720 File Offset: 0x00017920
	public void ShootOni(float speed)
	{
		this.oniBullet.Create(this.slotBulletRoot.position, speed, base.properties.CurrentState.demon);
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000B2724 File Offset: 0x000B0924
	public IEnumerator snake_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = p.snakeDuration;
		float val = 0f;
		float bulletDelay = 1000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.snakeSpeed.GetFloatAt(val);
				this.ShootSnake(bulletSpeed);
				bulletDelay = 0f;
			}
			delay = p.snakeDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000B2748 File Offset: 0x000B0948
	public IEnumerator bison_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = (float)p.bisonDuration;
		float val = 0f;
		float bulletDelay = 10000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		int sameDirCount = 0;
		FrogsLevelBisonBullet.Direction lastDir = FrogsLevelBisonBullet.Direction.Down;
		FrogsLevelBisonBullet.Direction dir = FrogsLevelBisonBullet.Direction.Up;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.bisonSpeed.GetFloatAt(val);
				this.ShootBison(bulletSpeed, dir, p.bisonBigX, p.bisonSmallX);
				bulletDelay = 0f;
				lastDir = dir;
				dir = (FrogsLevelBisonBullet.Direction)Random.Range(0, 2);
				if (lastDir == dir)
				{
					sameDirCount++;
				}
				else
				{
					sameDirCount = 0;
				}
				if (sameDirCount >= 3)
				{
					if (dir == FrogsLevelBisonBullet.Direction.Up)
					{
						dir = FrogsLevelBisonBullet.Direction.Down;
					}
					else
					{
						dir = FrogsLevelBisonBullet.Direction.Up;
					}
					sameDirCount = 0;
				}
			}
			delay = p.bisonDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000B276C File Offset: 0x000B096C
	public IEnumerator tiger_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = p.tigerDuration;
		float val = 0f;
		float bulletDelay = 1000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.tigerSpeed;
				this.ShootTiger(bulletSpeed);
				bulletDelay = 0f;
			}
			delay = p.tigerDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000B2790 File Offset: 0x000B0990
	public IEnumerator oni_cr()
	{
		LevelProperties.Frogs.Demon p = base.properties.CurrentState.demon;
		float bulletSpeed = 0f;
		float bulletDelay = 1000f;
		float delay = 0f;
		float val = 0f;
		float time = p.demonMaxTime;
		float t = 0f;
		for (;;)
		{
			FrogsLevelBisonBullet.Direction dir = (FrogsLevelBisonBullet.Direction)Random.Range(0, 2);
			string[] demonPattern = p.demonString[this.mainIndex].Split(new char[]
			{
				','
			});
			if (bulletDelay >= delay)
			{
				demonPattern = p.demonString[this.mainIndex].Split(new char[]
				{
					','
				});
				bulletSpeed = p.demonSpeed.GetFloatAt(val);
				char c = demonPattern[this.index][0];
				if (c != 'S')
				{
					if (c != 'T')
					{
						if (c != 'B')
						{
							if (c == 'O')
							{
								this.ShootOni(bulletSpeed);
							}
						}
						else
						{
							dir = (FrogsLevelBisonBullet.Direction)Random.Range(0, 2);
							this.ShootBison(bulletSpeed, dir, base.properties.CurrentState.morph.bisonBigX, base.properties.CurrentState.morph.bisonSmallX);
							if (dir == FrogsLevelBisonBullet.Direction.Up)
							{
								dir = FrogsLevelBisonBullet.Direction.Down;
							}
							else
							{
								dir = FrogsLevelBisonBullet.Direction.Up;
							}
						}
					}
					else
					{
						this.ShootTiger(bulletSpeed);
					}
				}
				else
				{
					this.ShootSnake(bulletSpeed);
				}
				if (this.index < demonPattern.Length - 1)
				{
					this.index++;
				}
				else
				{
					this.mainIndex = (this.mainIndex + 1) % p.demonString.Length;
					this.index = 0;
				}
				bulletDelay = 0f;
			}
			delay = p.demonDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400189E RID: 6302
	public static FrogsLevelMorphed Current;

	// Token: 0x0400189F RID: 6303
	[SerializeField]
	public FrogsLevelMorphedCoin coin;

	// Token: 0x040018A0 RID: 6304
	[SerializeField]
	public Transform coinRoot;

	// Token: 0x040018A1 RID: 6305
	[Space(10f)]
	public Transform switchRoot;

	// Token: 0x040018A2 RID: 6306
	[Space(10f)]
	[SerializeField]
	public GameObject slotsParent;

	// Token: 0x040018A3 RID: 6307
	[SerializeField]
	public Slots slots;

	// Token: 0x040018A4 RID: 6308
	[Space(10f)]
	[SerializeField]
	public FrogsLevelSnakeBullet snakeBullet;

	// Token: 0x040018A5 RID: 6309
	[SerializeField]
	public FrogsLevelBisonBullet bisonBullet;

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	public FrogsLevelTigerBullet tigerBullet;

	// Token: 0x040018A7 RID: 6311
	[SerializeField]
	public FrogsLevelOniBullet oniBullet;

	// Token: 0x040018A8 RID: 6312
	[SerializeField]
	public Transform slotBulletRoot;

	// Token: 0x040018A9 RID: 6313
	[Space(10f)]
	[SerializeField]
	public Effect dustEffect;

	// Token: 0x040018AA RID: 6314
	public DamageReceiver damageReceiver;

	// Token: 0x040018AB RID: 6315
	public DamageDealer damageDealer;

	// Token: 0x040018AC RID: 6316
	public FrogsLevelMorphedSwitch handle;

	// Token: 0x040018AD RID: 6317
	public bool demonTriggered;

	// Token: 0x040018AE RID: 6318
	public int mainIndex;

	// Token: 0x040018AF RID: 6319
	public int index;

	// Token: 0x040018B0 RID: 6320
	public bool handleActivated;

	// Token: 0x040018B1 RID: 6321
	public IEnumerator shootingCoroutine;

	// Token: 0x040018B2 RID: 6322
	public float coinSpeed;
}
