using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class FlowerLevelFlower : LevelProperties.Flower.Entity
{
	// Token: 0x14000042 RID: 66
	// (add) Token: 0x060018BD RID: 6333 RVA: 0x000A48A4 File Offset: 0x000A2AA4
	// (remove) Token: 0x060018BE RID: 6334 RVA: 0x000A48DC File Offset: 0x000A2ADC
	public event Action OnDeathEvent;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060018BF RID: 6335 RVA: 0x000A4914 File Offset: 0x000A2B14
	// (remove) Token: 0x060018C0 RID: 6336 RVA: 0x000A494C File Offset: 0x000A2B4C
	public event Action OnStateChanged;

	// Token: 0x060018C1 RID: 6337 RVA: 0x00015267 File Offset: 0x00013467
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0001527A File Offset: 0x0001347A
	public void AdditionDamageTaken(DamageDealer.DamageInfo info)
	{
		this.OnDamageTaken(info);
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00015283 File Offset: 0x00013483
	public void PhaseTwoTrigger()
	{
		base.animator.SetTrigger("PhaseTwoTransition");
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x00015295 File Offset: 0x00013495
	public void Die()
	{
		this.isDead = true;
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x000152AB File Offset: 0x000134AB
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.boomerangPrefab = null;
		this.bulletSeedPrefab = null;
		this.cloudBombPrefab = null;
		this.enemySeedPrefab = null;
		this.pollenProjectile = null;
		this.gattlingFX = null;
		this.vineHandPrefab = null;
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x000152E4 File Offset: 0x000134E4
	public void SpawnMainVine()
	{
		if (this.OnStateChanged != null)
		{
			this.OnStateChanged();
		}
		this.mainVine.SetActive(true);
		base.animator.SetTrigger("SpawnMainVine");
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x00015318 File Offset: 0x00013518
	public void MainVineSpawned()
	{
		base.StartCoroutine(this.vineHands_cr());
		this.projectileSpawned = false;
		base.StartCoroutine(this.pollenAttack_cr());
		this.attackCount = 1;
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x000A4984 File Offset: 0x000A2B84
	public IEnumerator die_cr()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("Phase One Death");
		}
		else
		{
			base.animator.Play("Phase Two Death");
		}
		yield return null;
		base.animator.enabled = false;
		base.animator.enabled = true;
		base.properties.WinInstantly();
		yield break;
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x000A49A0 File Offset: 0x000A2BA0
	public override void LevelInit(LevelProperties.Flower properties)
	{
		properties.OnBossDeath += this.Phase2DeathAudio;
		properties.OnBossDeath += this.Die;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.LevelInit(properties);
		this.attackCount = 0;
		this.miniFlowerSpawned = false;
		Level.Current.OnIntroEvent += this.OnIntro;
		int num = Random.Range(0, properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		}).Length);
		this.currentLaserAttack = num;
		this.currentGattlingGunAttackPattern = new List<string>();
		this.currentGattlingGunAttackString = Random.Range(0, properties.CurrentState.gattlingGun.seedSpawnString.Length);
		string[] array = properties.CurrentState.vineHands.handAttackString.Split(new char[]
		{
			','
		});
		this.currentVineHandsAttack = Random.Range(0, array.Length);
		this.pollenAttackCount = Random.Range(0, properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
		{
			','
		}).Length);
		this.currentPollenType = Random.Range(0, properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.find_s_cr());
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x000A4B1C File Offset: 0x000A2D1C
	public IEnumerator find_s_cr()
	{
		while (base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0] != 'S')
		{
			this.currentPodHandsAttack = Random.Range(0, base.properties.CurrentState.podHands.attacktype.Split(new char[]
			{
				','
			}).Length);
			yield return null;
		}
		this.podHandsAttackCountTarget = Random.Range(0, base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		}).Length);
		yield return null;
		yield break;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x00015342 File Offset: 0x00013542
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x0001535A File Offset: 0x0001355A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000A4B38 File Offset: 0x000A2D38
	public void StartLaser(Action callback)
	{
		this.attackCallback = callback;
		this.attackType = base.properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		})[this.currentLaserAttack][0];
		this.attackCharge = base.properties.CurrentState.laser.anticHold;
		this.OnLaserStarted();
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000A4BA8 File Offset: 0x000A2DA8
	public void OnLaserStarted()
	{
		if (this.attackType.Equals('T'))
		{
			this.topLaserAttack = true;
		}
		else
		{
			this.topLaserAttack = false;
		}
		if (this.topLaserAttack)
		{
			base.animator.SetBool("TopLaser", true);
		}
		else
		{
			base.animator.SetBool("BottomLaser", true);
		}
		base.StartCoroutine(this.laserCharge_cr());
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x000A4C1C File Offset: 0x000A2E1C
	public IEnumerator laserCharge_cr()
	{
		if (this.topLaserAttack)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "TopLaserAttackStart", true, true);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "BottomLaserAttackStart", true, true);
		}
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		base.animator.SetTrigger("OnAttackChargeComplete");
		yield break;
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x00015378 File Offset: 0x00013578
	public void OnHoldComplete()
	{
		base.StartCoroutine(this.onLaser_cr());
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000A4C38 File Offset: 0x000A2E38
	public IEnumerator onLaser_cr()
	{
		this.attackCharge = base.properties.CurrentState.laser.attackHold;
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		if (this.topLaserAttack)
		{
			base.animator.SetBool("TopLaser", false);
		}
		else
		{
			base.animator.SetBool("BottomLaser", false);
		}
		if (this.topLaserAttack)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "TopLaserAttackEnd", true, true);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "BottomLaserAttackEnd", true, true);
		}
		yield break;
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000A4C54 File Offset: 0x000A2E54
	public void OnLaserComplete()
	{
		this.currentLaserAttack++;
		if (this.currentLaserAttack >= base.properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentLaserAttack = 0;
		}
		this.topLaserAttack = false;
		if (this.attackCallback != null)
		{
			this.attackCallback();
		}
		this.attackCallback = null;
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000A4CCC File Offset: 0x000A2ECC
	public void StartPotHands(Action callback)
	{
		this.attackCount = 0;
		if (this.podHandsAttackCountTarget >= base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		}).Length)
		{
			this.podHandsAttackCountTarget = 0;
		}
		this.attackCountTarget = Parser.IntParse(base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		})[this.podHandsAttackCountTarget].ToString());
		this.attackType = base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0];
		this.attackCallback = callback;
		this.attackCharge = base.properties.CurrentState.podHands.attackHold;
		this.OnPotHandsStarted();
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x00015387 File Offset: 0x00013587
	public void OnPotHandsStarted()
	{
		base.animator.SetBool("PotHandsAttack", true);
		base.StartCoroutine(this.potHandsHold_cr());
		this.attackCount++;
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000A4DB8 File Offset: 0x000A2FB8
	public IEnumerator potHandsHold_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		base.animator.SetTrigger("OnAttackChargeComplete");
		this.OpenPotHands();
		yield break;
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000153B5 File Offset: 0x000135B5
	public void OpenPotHands()
	{
		this.attackCharge = base.properties.CurrentState.podHands.attackDelay;
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000A4DD4 File Offset: 0x000A2FD4
	public void OnPotHandsComplete()
	{
		this.projectileSpawned = false;
		if (this.attackCount >= this.attackCountTarget)
		{
			this.attackCount = 0;
			this.podHandsAttackCountTarget++;
			if (this.podHandsAttackCountTarget >= base.properties.CurrentState.podHands.attackAmount.Split(new char[]
			{
				','
			}).Length)
			{
				this.podHandsAttackCountTarget = 0;
			}
			base.animator.SetTrigger("OnAttackComplete");
			if (this.attackCallback != null)
			{
				this.attackCallback();
				this.attackCallback = null;
			}
		}
		base.animator.SetBool("PotHandsAttack", false);
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000153D2 File Offset: 0x000135D2
	public void StartGattlingGun(Action callback)
	{
		this.attackCallback = callback;
		this.attackCharge = base.properties.CurrentState.gattlingGun.loopDuration;
		base.animator.SetBool("GattlingGunAttack", true);
		this.attackType = 'G';
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000A4E88 File Offset: 0x000A3088
	public IEnumerator startGattlingFX_cr()
	{
		string animAttrib = "GattlingGunAttack";
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".GattlingGunStart");
		if (target == base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "GattlingGunStart", true, true);
		}
		while (base.animator.GetBool(animAttrib))
		{
			GameObject fxObject = Object.Instantiate<GameObject>(this.gattlingFX, this.topProjectileSpawnPoint.position, Quaternion.identity);
			Animator fx = fxObject.GetComponent<Animator>();
			yield return base.StartCoroutine(this.killGattlingFX_cr(fx));
		}
		yield break;
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000A4EA4 File Offset: 0x000A30A4
	public IEnumerator killGattlingFX_cr(Animator fx)
	{
		yield return fx.WaitForAnimationToEnd(this, true);
		Object.Destroy(fx.gameObject);
		yield break;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0001540F File Offset: 0x0001360F
	public void OnGattlingGunEnded()
	{
		base.animator.SetBool("GattlingGunAttack", false);
		this.OnGattlingGunComplete();
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x00015428 File Offset: 0x00013628
	public void OnGattlingGunComplete()
	{
		if (this.attackCallback != null)
		{
			this.attackCallback();
		}
		this.attackCallback = null;
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000A4EC8 File Offset: 0x000A30C8
	public void AddAttackTypes(string[] s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			this.currentGattlingGunAttackPattern.Add(s[i]);
		}
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x00015447 File Offset: 0x00013647
	public void StartVineHandsAttack()
	{
		base.StartCoroutine(this.vineHands_cr());
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000A4EF8 File Offset: 0x000A30F8
	public IEnumerator vineHands_cr()
	{
		while (!this.isDead)
		{
			string[] attackPositions = base.properties.CurrentState.vineHands.handAttackString.Split(new char[]
			{
				','
			});
			string[] currentWave = attackPositions[this.currentVineHandsAttack].Split(new char[]
			{
				'-'
			});
			if (attackPositions[this.currentVineHandsAttack][0] != 'D')
			{
				if (currentWave.Length > 1)
				{
					this.currentVineHandsAttack += 2;
					this.vineHandPrefab.GetComponent<FlowerLevelFlowerVineHand>().OnVineHandSpawn(base.properties.CurrentState.vineHands.firstPositionHold, base.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave[0]), Parser.IntParse(currentWave[1]));
				}
				else
				{
					this.currentVineHandsAttack++;
					this.vineHandPrefab.GetComponent<FlowerLevelFlowerVineHand>().OnVineHandSpawn(base.properties.CurrentState.vineHands.firstPositionHold, base.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave[0]), 0);
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(attackPositions[this.currentVineHandsAttack].Substring(1)));
			}
			if (this.currentVineHandsAttack >= attackPositions.Length)
			{
				this.currentVineHandsAttack = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.vineHands.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000A4F14 File Offset: 0x000A3114
	public IEnumerator pollenAttack_cr()
	{
		while (!this.isDead)
		{
			if (!this.projectileSpawned)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.pollenSpit.pollenCommaDelay);
				string delay = base.properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
				{
					','
				})[this.pollenAttackCount].ToString();
				if (delay[0].Equals('D'))
				{
					yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(delay.Substring(1).ToString()));
				}
				else
				{
					this.attackCountTarget = Parser.IntParse(delay);
				}
				this.pollenAttackCount++;
				if (this.pollenAttackCount >= base.properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
				{
					','
				}).Length)
				{
					this.pollenAttackCount = 0;
				}
				this.projectileSpawned = true;
				base.animator.SetBool("OnPollenAttack", true);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.pollenSpit.consecutiveAttackHold);
				base.animator.SetTrigger("OnAttackChargeComplete");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x000A4F30 File Offset: 0x000A3130
	public void launchPollen()
	{
		string text = base.properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		})[this.currentPollenType];
		int type;
		if (text[0].Equals('R'))
		{
			type = 0;
		}
		else
		{
			type = 1;
		}
		this.currentPollenType++;
		if (this.currentPollenType >= base.properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentPollenType = 0;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.pollenProjectile, this.topProjectileSpawnPoint.position, Quaternion.identity);
		this.currentPollenShot = gameObject.GetComponent<FlowerLevelPollenProjectile>();
		this.currentPollenShot.InitPollen((float)base.properties.CurrentState.pollenSpit.pollenSpeed, base.properties.CurrentState.pollenSpit.pollenUpDownStrength, type, this.topProjectileSpawnPoint);
		AudioManager.Play("flower_phase2_spit_projectile");
		this.attackCount++;
		if (this.attackCount > this.attackCountTarget)
		{
			base.animator.SetBool("OnPollenAttack", false);
			this.attackCount = 1;
			this.projectileSpawned = false;
		}
		else
		{
			this.projectileSpawned = true;
		}
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x00015456 File Offset: 0x00013656
	public void PollenShotEnd()
	{
		this.currentPollenShot.StartMoving();
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x00015463 File Offset: 0x00013663
	public void OnIntro()
	{
		base.animator.SetTrigger("OnIntroEnded");
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x000A508C File Offset: 0x000A328C
	public void SpawnProjectile()
	{
		char c = this.attackType;
		if (c != 'R')
		{
			if (c != 'S')
			{
				if (c != 'B')
				{
					if (c == 'G')
					{
						base.StartCoroutine(this.spawnGattlingGunSeeds_cr());
					}
				}
				else
				{
					this.SpawnBoomerang();
				}
			}
			else
			{
				this.SpawnBullets();
			}
		}
		else
		{
			this.SpawnCloudShot();
		}
		this.currentPodHandsAttack++;
		if (this.currentPodHandsAttack >= base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentPodHandsAttack = 0;
		}
		this.attackType = base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0];
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x000A517C File Offset: 0x000A337C
	public IEnumerator spawnGattlingGunSeeds_cr()
	{
		base.StartCoroutine(this.startGattlingFX_cr());
		this.currentGattlingGunAttackPattern.Clear();
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gattlingGun.initialSeedDelay);
		string[] projectileAttributes = base.properties.CurrentState.gattlingGun.seedSpawnString[this.currentGattlingGunAttackString].Split(new char[]
		{
			','
		});
		float delayNextProjectileWave = base.properties.CurrentState.gattlingGun.fallingSeedDelay;
		for (int j = 0; j < projectileAttributes.Length; j++)
		{
			string[] array = projectileAttributes[j].Split(new char[]
			{
				'-'
			});
			if (array.Length > 1)
			{
				this.AddAttackTypes(array);
			}
			else
			{
				this.currentGattlingGunAttackPattern.Add(projectileAttributes[j]);
			}
			this.currentGattlingGunAttackPattern.Add("D" + base.properties.CurrentState.gattlingGun.fallingSeedDelay.ToStringInvariant());
		}
		for (int i = 0; i < this.currentGattlingGunAttackPattern.Count; i++)
		{
			char t = this.currentGattlingGunAttackPattern[i][0];
			if (t == 'D')
			{
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(this.currentGattlingGunAttackPattern[i].Substring(1)));
			}
			else
			{
				if (this.miniFlowerSpawned)
				{
					if (t != 'C')
					{
						this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, true);
					}
					else
					{
						this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, false);
					}
				}
				else
				{
					this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, true);
				}
				if (t == 'C')
				{
					this.miniFlowerSpawned = true;
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, delayNextProjectileWave);
		this.currentGattlingGunAttackString++;
		if (this.currentGattlingGunAttackString >= base.properties.CurrentState.gattlingGun.seedSpawnString.Length)
		{
			this.currentGattlingGunAttackString = 0;
		}
		AudioManager.Stop("flower_gattling_gun_loop");
		this.OnGattlingGunEnded();
		yield break;
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x00015475 File Offset: 0x00013675
	public void OnMiniFlowerDeath()
	{
		this.miniFlowerSpawned = false;
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x000A5198 File Offset: 0x000A3398
	public void SpawnEnemySeed(int xPos, char t, bool a = true)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.enemySeedPrefab);
		gameObject.transform.position = new Vector3((float)(-600 + xPos), (float)Level.Current.Height, 0f);
		gameObject.GetComponent<FlowerLevelEnemySeed>().OnSeedSpawn(base.properties, this, t, a);
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x000A51F0 File Offset: 0x000A33F0
	public void SpawnBoomerang()
	{
		BasicProjectile proj = this.boomerangPrefab.GetComponent<FlowerLevelBoomerang>().Create(this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / 2f, 0f, 0f);
		base.StartCoroutine(this.spawnBoomerang_cr(proj));
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x000A5260 File Offset: 0x000A3460
	public IEnumerator spawnBoomerang_cr(BasicProjectile proj)
	{
		proj.GetComponent<FlowerLevelBoomerang>().OnBoomerangStart(base.properties.CurrentState.boomerang.offScreenDelay);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boomerang.initialMovementDelay);
		proj.GetComponent<BasicProjectile>().Speed = (float)(-(float)base.properties.CurrentState.boomerang.speed);
		this.OnPotHandsComplete();
		yield break;
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x000A5284 File Offset: 0x000A3484
	public void SpawnBullets()
	{
		this.bulletSpawns.Clear();
		for (int i = 0; i < base.properties.CurrentState.bullets.numberOfProjectiles; i++)
		{
			this.bulletSpawns.Add(this.bulletSeedPrefab.GetComponent<FlowerLevelSeedBullet>().Create(Vector2.zero));
			Vector3 position = this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / (float)(base.properties.CurrentState.bullets.numberOfProjectiles - 1) * (float)i;
			this.bulletSpawns[this.bulletSpawns.Count - 1].transform.position = position;
		}
		base.StartCoroutine(this.spawnBullets_cr());
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x000A5364 File Offset: 0x000A3564
	public IEnumerator spawnBullets_cr()
	{
		List<AbstractProjectile> bullets = new List<AbstractProjectile>();
		List<AbstractProjectile> activeBullets = this.bulletSpawns;
		for (int i = 0; i < base.properties.CurrentState.bullets.numberOfProjectiles; i++)
		{
			float delay = (float)base.properties.CurrentState.bullets.holdDelay / (float)base.properties.CurrentState.bullets.numberOfProjectiles;
			int rand = Random.Range(0, activeBullets.Count);
			bullets.Add(activeBullets[rand]);
			bullets[i].GetComponent<FlowerLevelSeedBullet>().OnBulletSeedStart(this, PlayerManager.GetNext(), base.properties.CurrentState.bullets.acceleration, base.properties.CurrentState.bullets.speedMinMax.min, base.properties.CurrentState.bullets.speedMinMax.max);
			activeBullets.RemoveAt(rand);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield return null;
		for (int j = 0; j < bullets.Count; j++)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bullets.delayNextShot);
			yield return null;
			if (bullets[j] != null)
			{
				bullets[j].GetComponent<FlowerLevelSeedBullet>().LaunchBullet();
			}
		}
		this.OnPotHandsComplete();
		yield return null;
		yield break;
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000A5380 File Offset: 0x000A3580
	public void SpawnCloudShot()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.cloudBombPrefab);
		Vector3 position = this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / 2f;
		gameObject.transform.position = position;
		gameObject.GetComponent<FlowerLevelCloudBomb>().OnCloudBombStart(PlayerManager.GetNext().center, (float)base.properties.CurrentState.puffUp.speed, base.properties.CurrentState.puffUp.delayExplosion);
		this.OnPotHandsComplete();
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0001547E File Offset: 0x0001367E
	public void PodHandsFX()
	{
		base.animator.Play("Twinkle", 2);
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x00015491 File Offset: 0x00013691
	public void GattlingEndAudio()
	{
		AudioManager.Play("flower_gattling_gun_end");
		this.emitAudioFromObject.Add("flower_gattling_gun_end");
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x000154AD File Offset: 0x000136AD
	public void GattlingLoopAudio()
	{
		base.StartCoroutine(this.gattlingLoopEnd_cr());
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x000A5424 File Offset: 0x000A3624
	public IEnumerator gattlingLoopEnd_cr()
	{
		yield return new WaitForEndOfFrame();
		AudioManager.PlayLoop("flower_gattling_gun_loop");
		this.emitAudioFromObject.Add("flower_gattling_gun_loop");
		yield break;
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x000154BC File Offset: 0x000136BC
	public void StopGattlingLoopAudio()
	{
		AudioManager.Stop("flower_gattling_gun_loop");
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x000154C8 File Offset: 0x000136C8
	public void GattlingStartAudio()
	{
		AudioManager.Play("flower_gattling_gun_start");
		this.emitAudioFromObject.Add("flower_gattling_gun_start");
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000154E4 File Offset: 0x000136E4
	public void Phase1IntroAudio()
	{
		AudioManager.Play("flower_intro_yell");
		this.emitAudioFromObject.Add("flower_intro_yell");
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x00015500 File Offset: 0x00013700
	public void Phase1_2TransitionAudio()
	{
		AudioManager.Play("flower_phase1_2_transition");
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0001550C File Offset: 0x0001370C
	public void Phase2DeathAudio()
	{
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0001550E File Offset: 0x0001370E
	public void PodHandsStartAudio()
	{
		AudioManager.Play("flower_pod_hands_start");
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0001551A File Offset: 0x0001371A
	public void PodHandsOpenAudio()
	{
		AudioManager.Play("flower_pod_hands_open");
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x00015526 File Offset: 0x00013726
	public void PodHandsCloseAudio()
	{
		AudioManager.Play("flower_pod_hands_end");
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x00015532 File Offset: 0x00013732
	public void SpitStartAudio()
	{
		AudioManager.Play("flower_spit_start");
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x0001553E File Offset: 0x0001373E
	public void TopLaserAttackStartAudio()
	{
		AudioManager.Play("flower_top_laser_attack_start");
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x0001554A File Offset: 0x0001374A
	public void TopLaserAttackHoldAudio()
	{
		AudioManager.PlayLoop("flower_top_laser_attack_hold");
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x00015556 File Offset: 0x00013756
	public void TopLaserAttackEndAudio()
	{
		AudioManager.Play("flower_top_laser_attack_end");
		AudioManager.Stop("flower_top_laser_attack_hold");
	}

	// Token: 0x04001403 RID: 5123
	public Action attackCallback;

	// Token: 0x04001406 RID: 5126
	public GameObject attackPoint;

	// Token: 0x04001407 RID: 5127
	public bool topLaserAttack;

	// Token: 0x04001408 RID: 5128
	public bool projectileSpawned;

	// Token: 0x04001409 RID: 5129
	public bool isDead;

	// Token: 0x0400140A RID: 5130
	public float attackCharge;

	// Token: 0x0400140B RID: 5131
	public int attackCount;

	// Token: 0x0400140C RID: 5132
	public int attackCountTarget;

	// Token: 0x0400140D RID: 5133
	public char attackType;

	// Token: 0x0400140E RID: 5134
	public int currentLaserAttack;

	// Token: 0x0400140F RID: 5135
	public int currentPodHandsAttack;

	// Token: 0x04001410 RID: 5136
	public int podHandsAttackCountTarget;

	// Token: 0x04001411 RID: 5137
	public int currentGattlingGunAttackString;

	// Token: 0x04001412 RID: 5138
	public List<string> currentGattlingGunAttackPattern;

	// Token: 0x04001413 RID: 5139
	public int currentVineHandsAttack;

	// Token: 0x04001414 RID: 5140
	public int pollenAttackCount;

	// Token: 0x04001415 RID: 5141
	public int currentPollenType;

	// Token: 0x04001416 RID: 5142
	public FlowerLevelPollenProjectile currentPollenShot;

	// Token: 0x04001417 RID: 5143
	[Header("Vines")]
	[SerializeField]
	public GameObject vineHandPrefab;

	// Token: 0x04001418 RID: 5144
	[Space(10f)]
	[Header("Prefabs")]
	[SerializeField]
	public GameObject boomerangPrefab;

	// Token: 0x04001419 RID: 5145
	[SerializeField]
	public GameObject bulletSeedPrefab;

	// Token: 0x0400141A RID: 5146
	[SerializeField]
	public GameObject cloudBombPrefab;

	// Token: 0x0400141B RID: 5147
	[SerializeField]
	public GameObject enemySeedPrefab;

	// Token: 0x0400141C RID: 5148
	public bool miniFlowerSpawned;

	// Token: 0x0400141D RID: 5149
	[SerializeField]
	public GameObject pollenProjectile;

	// Token: 0x0400141E RID: 5150
	[Space(10f)]
	[SerializeField]
	public Transform topProjectileSpawnPoint;

	// Token: 0x0400141F RID: 5151
	[SerializeField]
	public Transform bottomProjectileSpawnPoint;

	// Token: 0x04001420 RID: 5152
	[SerializeField]
	public GameObject mainVine;

	// Token: 0x04001421 RID: 5153
	[SerializeField]
	public GameObject gattlingFX;

	// Token: 0x04001422 RID: 5154
	public DamageDealer damageDealer;

	// Token: 0x04001423 RID: 5155
	public DamageReceiver damageReceiver;

	// Token: 0x04001424 RID: 5156
	public List<AbstractProjectile> bulletSpawns = new List<AbstractProjectile>();
}
