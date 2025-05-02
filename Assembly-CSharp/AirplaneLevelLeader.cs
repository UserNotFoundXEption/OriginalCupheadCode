using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class AirplaneLevelLeader : LevelProperties.Airplane.Entity
{
	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0000C0A5 File Offset: 0x0000A2A5
	// (set) Token: 0x06000E1B RID: 3611 RVA: 0x0000C0AD File Offset: 0x0000A2AD
	public bool IsAttacking { get; set; }

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
	// (set) Token: 0x06000E1D RID: 3613 RVA: 0x0000C0BE File Offset: 0x0000A2BE
	public bool camRotatedHorizontally { get; set; }

	// Token: 0x06000E1E RID: 3614 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0000C0F2 File Offset: 0x0000A2F2
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x00089440 File Offset: 0x00087640
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.bulletDelayString = new PatternString(properties.CurrentState.dropshot.bulletDelayStrings, true, true);
		this.bulletColorString = new PatternString(properties.CurrentState.dropshot.bulletColorString, true, true);
		this.laserPositionStringsMainIndex = Random.Range(0, properties.CurrentState.laser.laserPositionStrings.Length);
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x000894AC File Offset: 0x000876AC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (((AirplaneLevel)Level.Current).Rotating)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && !this.isDead)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0000C117 File Offset: 0x0000A317
	public void StartLeader()
	{
		base.animator.Play("Intro");
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0000C129 File Offset: 0x0000A329
	public void RotateCamera()
	{
		this.camRotatedHorizontally = !this.camRotatedHorizontally;
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x0000C13A File Offset: 0x0000A33A
	public void AniEvent_PawGrab()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.8f, false);
		((AirplaneLevel)Level.Current).MoveBoundsIn();
		((AirplaneLevel)Level.Current).BlurBGCamera();
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00089514 File Offset: 0x00087714
	public void AniEvent_StartButtonPush()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Push_Wait"))
		{
			base.animator.Play("Push_Start", 3, 0f);
			base.animator.Update(0f);
			AudioManager.Play("sfx_dlc_dogfight_leadervocal_buttonbashbegin");
			this.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_buttonbashbegin");
		}
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00089580 File Offset: 0x00087780
	public void LateUpdate()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Push") && base.animator.GetCurrentAnimatorStateInfo(0).IsName("Sideways_Idle") && base.animator.GetCurrentAnimatorStateInfo(3).normalizedTime != base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		{
			base.animator.Play("Push", 3, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.animator.Update(0f);
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0000C16F File Offset: 0x0000A36F
	public void StartDropshot()
	{
		base.StartCoroutine(this.drop_shot_cr());
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0008962C File Offset: 0x0008782C
	public IEnumerator drop_shot_cr()
	{
		this.IsAttacking = true;
		LevelProperties.Airplane.Dropshot p = base.properties.CurrentState.dropshot;
		this.bulletDelayString.SetSubStringIndex(0);
		AirplaneLevelDropBullet[] bullets = new AirplaneLevelDropBullet[this.bulletDelayString.SubStringLength()];
		bool onLeft = true;
		AudioManager.PlayLoop("sfx_dlc_dogfight_p3_leader_buttonpresses_loop");
		for (int i = 0; i < bullets.Length; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.bulletDelayString.PopFloat());
			bool isRed = this.bulletColorString.PopLetter() == 'R';
			AirplaneLevelDropBullet bullet = (!isRed) ? this.yellowBullet : this.redBullet;
			Vector3 pos = Vector3.zero;
			pos.x = ((!onLeft) ? 380f : -380f);
			pos.y = ((!isRed) ? this.yellowPosSideways.position.y : this.redPosSideways.position.y);
			Transform startPos = (!onLeft) ? this.rightDogBowlSpawn : this.leftDogBowlSpawn;
			AirplaneLevelDropBullet b = bullet.Spawn<AirplaneLevelDropBullet>();
			b.Init(pos, startPos.position, p.bulletDropSpeed, p.bulletShootSpeed, onLeft, this.camRotatedHorizontally);
			bullets[i] = b;
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_dogbowl_fire");
			Transform flashPos = (!onLeft) ? this.flashRootRight : this.flashRootLeft;
			Effect flash = this.flashEffect.Create(flashPos.position, flashPos.localScale);
			flash.transform.rotation = flashPos.rotation;
			onLeft = !onLeft;
		}
		base.animator.SetTrigger("EndButtonPush");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_leader_buttonpresses_loop", 0f, 0.25f);
		bool stillAttacking = true;
		while (stillAttacking)
		{
			bool bulletsAlive = false;
			foreach (AirplaneLevelDropBullet airplaneLevelDropBullet in bullets)
			{
				if (airplaneLevelDropBullet.isMoving)
				{
					bulletsAlive = true;
				}
			}
			if (!bulletsAlive)
			{
				stillAttacking = false;
				break;
			}
			yield return null;
		}
		this.IsAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x00089648 File Offset: 0x00087848
	public void OpenPawHoles()
	{
		for (int i = 0; i < this.laserAnimator.Length; i++)
		{
			this.laserAnimator[i].Play("SecretOpen");
		}
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0000C17E File Offset: 0x0000A37E
	public void StartLaser()
	{
		base.StartCoroutine(this.laser_main_cr());
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x00089680 File Offset: 0x00087880
	public List<int> GetLasersToShoot(string[] lasers)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < lasers.Length; i++)
		{
			list.Add((int)(lasers[i][0] - 'A'));
		}
		return list;
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x000896BC File Offset: 0x000878BC
	public IEnumerator laser_main_cr()
	{
		this.IsAttacking = true;
		LevelProperties.Airplane.Laser p = base.properties.CurrentState.laser;
		string[] laserPositionStrings = p.laserPositionStrings[this.laserPositionStringsMainIndex].Split(new char[]
		{
			','
		});
		for (int i = 0; i < laserPositionStrings.Length; i++)
		{
			string[] lasers = laserPositionStrings[i].Split(new char[]
			{
				':'
			});
			this.lasersToShoot = this.GetLasersToShoot(lasers);
			if (i + 1 < laserPositionStrings.Length)
			{
				string[] lasers2 = laserPositionStrings[i + 1].Split(new char[]
				{
					':'
				});
				this.lasersNextToShoot = this.GetLasersToShoot(lasers2);
			}
			else
			{
				this.lasersNextToShoot = new List<int>();
			}
			base.StartCoroutine(this.fire_lasers_cr(this.lasersToShoot, this.lasersNextToShoot, i));
			yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length + p.laserHesitation + p.warningTime + p.laserDuration + p.laserDelay);
		}
		yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length);
		this.laserPositionStringsMainIndex = (this.laserPositionStringsMainIndex + 1) % p.laserPositionStrings.Length;
		this.IsAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x000896D8 File Offset: 0x000878D8
	public IEnumerator fire_lasers_cr(List<int> lasers, List<int> lasersNext, int round)
	{
		LevelProperties.Airplane.Laser p = base.properties.CurrentState.laser;
		for (int i = 0; i < lasers.Count; i++)
		{
			if (!this.laserOut[lasers[i]])
			{
				this.laserAnimator[lasers[i]].Play("In");
			}
			this.laserOut[lasers[i]] = true;
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_buildout");
		yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length);
		yield return CupheadTime.WaitForSeconds(this, p.laserHesitation);
		for (int j = 0; j < lasers.Count; j++)
		{
			this.laserAnimator[lasers[j]].Play("WarningStart");
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_prefire_warning");
		yield return CupheadTime.WaitForSeconds(this, p.warningTime);
		for (int k = 0; k < lasers.Count; k++)
		{
			this.laserAnimator[lasers[k]].Play("FireStart");
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_fire");
		yield return CupheadTime.WaitForSeconds(this, p.laserDuration);
		bool puttingAtLeastOneLaserAway = false;
		for (int l = 0; l < lasers.Count; l++)
		{
			this.laserOut[lasers[l]] = (lasersNext.Contains(lasers[l]) && !p.forceHide);
			this.laserAnimator[lasers[l]].SetBool("StayOut", this.laserOut[lasers[l]]);
			this.laserAnimator[lasers[l]].Play("End");
			if (!this.laserOut[lasers[l]])
			{
				puttingAtLeastOneLaserAway = true;
			}
		}
		if (puttingAtLeastOneLaserAway)
		{
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_unbuild");
		}
		yield break;
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00089704 File Offset: 0x00087904
	public IEnumerator rocket_cr()
	{
		LevelProperties.Airplane.Rocket p = base.properties.CurrentState.rocket;
		int delayMainIndex = Random.Range(0, p.attackDelayString.Length);
		string[] delayString = p.attackDelayString[delayMainIndex].Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		int dirMainIndex = Random.Range(0, p.attackOrderString.Length);
		string[] dirString = p.attackOrderString[dirMainIndex].Split(new char[]
		{
			','
		});
		int dirIndex = Random.Range(0, dirString.Length);
		for (;;)
		{
			delayString = p.attackDelayString[delayMainIndex].Split(new char[]
			{
				','
			});
			dirString = p.attackOrderString[dirMainIndex].Split(new char[]
			{
				','
			});
			int delay = 0;
			Parser.IntTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, (float)delay);
			Vector3 position;
			if (dirString[dirIndex][0] == 'R')
			{
				position = this.rocketSpawnRight.position;
			}
			else
			{
				position = this.rocketSpawnLeft.position;
			}
			this.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime);
			if (dirIndex < dirString.Length - 1)
			{
				dirIndex++;
			}
			else
			{
				dirMainIndex = (dirMainIndex + 1) % p.attackOrderString.Length;
			}
			if (delayIndex < delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				delayMainIndex = (delayMainIndex + 1) % p.attackDelayString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00089720 File Offset: 0x00087920
	public IEnumerator death_cr()
	{
		this.isDead = true;
		base.GetComponent<BoxCollider2D>().enabled = false;
		this.rotatedExploder.enabled = this.camRotatedHorizontally;
		this.pawRightExploder.enabled = !this.camRotatedHorizontally;
		this.pawLeftExploder.enabled = !this.camRotatedHorizontally;
		base.animator.Play(this.camRotatedHorizontally ? "Copter_Death_Closeup" : "Copter_Death", base.animator.GetLayerIndex("Death"));
		if (!this.camRotatedHorizontally)
		{
			base.animator.Play("Off");
			base.animator.Play("Blades", base.animator.GetLayerIndex("DeathBlades"));
		}
		else
		{
			base.animator.Play("Death_Closeup", 3);
			base.animator.Play("SidewaysTears", 4);
		}
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death");
		base.animator.Update(0f);
		base.StartCoroutine(this.activate_death_puffs_cr());
		if (!this.camRotatedHorizontally)
		{
			for (int i = 0; i < this.laserAnimator.Length; i++)
			{
				if (!this.laserAnimator[i].GetCurrentAnimatorStateInfo(0).IsName("Off"))
				{
					this.laserAnimator[i].Play((i != 2) ? "Out" : "Dead", 0, this.laserDeathTime[i]);
				}
				else if (i == 2)
				{
					this.laserAnimator[i].Play("SecretOpen");
				}
			}
			while (PauseManager.state == PauseManager.State.Paused)
			{
				yield return null;
			}
			for (int j = 0; j < this.laserAnimator.Length; j++)
			{
				this.laserAnimator[j].GetComponent<AnimationHelper>().Speed = 1.25f;
			}
			while (!this.laserAnimator[2].GetCurrentAnimatorStateInfo(0).IsName("HoldOpen"))
			{
				yield return null;
			}
			((AirplaneLevel)Level.Current).LeaderDeath();
		}
		yield break;
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0008973C File Offset: 0x0008793C
	public IEnumerator activate_death_puffs_cr()
	{
		while (this.deathPuffs.Count > 0)
		{
			int i = Random.Range(0, this.deathPuffs.Count);
			this.deathPuffs[i].gameObject.SetActive(true);
			if (this.camRotatedHorizontally)
			{
				this.deathPuffs[i].Play("Sideways");
				this.deathPuffs[i].Update(0f);
			}
			this.deathPuffs.RemoveAt(i);
			yield return CupheadTime.WaitForSeconds(this, 0.166666672f);
		}
		yield break;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0000C18D File Offset: 0x0000A38D
	public void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateChomp()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_screenrotate_chomp");
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0000C199 File Offset: 0x0000A399
	public void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotate()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P3_DogCopter_ScreenRotate");
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0000C1A5 File Offset: 0x0000A3A5
	public void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_GrabScreen()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_settle_grabscreen");
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
	public void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro");
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x0000C1BD File Offset: 0x0000A3BD
	public void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro2()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro2");
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0000C1C9 File Offset: 0x0000A3C9
	public void AnimationEvent_SFX_DOGFIGHT_P3_LeaderVocalEnd()
	{
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command");
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x00089758 File Offset: 0x00087958
	public void WORKAROUND_NullifyFields()
	{
		this.laserPositions = null;
		this.rocketSpawnLeft = null;
		this.rocketSpawnRight = null;
		this.yellowPosSideways = null;
		this.redPosSideways = null;
		this.flashRootLeft = null;
		this.flashRootRight = null;
		this.leftDogBowlSpawn = null;
		this.rightDogBowlSpawn = null;
		this.buildLaserAni = null;
		this.rocketPrefab = null;
		this.yellowBullet = null;
		this.redBullet = null;
		this.laserAnimator = null;
		this.flashEffect = null;
		this.lasersToShoot = null;
		this.lasersNextToShoot = null;
		this.laserOut = null;
		this.laserDeathTime = null;
		this.bulletDelayString = null;
		this.bulletColorString = null;
		this.rotatedExploder = null;
		this.pawRightExploder = null;
		this.pawLeftExploder = null;
		this.deathPuffs = null;
	}

	// Token: 0x04000B2E RID: 2862
	public const float PAW_MOVE_X = 750f;

	// Token: 0x04000B2F RID: 2863
	public const float BULLET_SPAWN_X = 380f;

	// Token: 0x04000B30 RID: 2864
	[Header("Spawn Positions")]
	[SerializeField]
	public Transform[] laserPositions;

	// Token: 0x04000B31 RID: 2865
	[SerializeField]
	public Transform rocketSpawnLeft;

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	public Transform rocketSpawnRight;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	public Transform yellowPosSideways;

	// Token: 0x04000B34 RID: 2868
	[SerializeField]
	public Transform redPosSideways;

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	public Transform flashRootLeft;

	// Token: 0x04000B36 RID: 2870
	[SerializeField]
	public Transform flashRootRight;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	public Transform leftDogBowlSpawn;

	// Token: 0x04000B38 RID: 2872
	[SerializeField]
	public Transform rightDogBowlSpawn;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	public AnimationClip buildLaserAni;

	// Token: 0x04000B3A RID: 2874
	[Header("Prefabs")]
	[SerializeField]
	public AirplaneLevelRocket rocketPrefab;

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	public AirplaneLevelDropBullet yellowBullet;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	public AirplaneLevelDropBullet redBullet;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	public Animator[] laserAnimator;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	public Effect flashEffect;

	// Token: 0x04000B41 RID: 2881
	public List<int> lasersToShoot;

	// Token: 0x04000B42 RID: 2882
	public List<int> lasersNextToShoot;

	// Token: 0x04000B43 RID: 2883
	public bool[] laserOut = new bool[5];

	// Token: 0x04000B44 RID: 2884
	public float[] laserDeathTime = new float[]
	{
		0.2f,
		0.6f,
		0.8f,
		0.266666681f,
		0.4f
	};

	// Token: 0x04000B45 RID: 2885
	public PatternString bulletDelayString;

	// Token: 0x04000B46 RID: 2886
	public PatternString bulletColorString;

	// Token: 0x04000B47 RID: 2887
	public int laserPositionStringsMainIndex;

	// Token: 0x04000B48 RID: 2888
	public bool isDead;

	// Token: 0x04000B49 RID: 2889
	public DamageReceiver damageReceiver;

	// Token: 0x04000B4A RID: 2890
	[SerializeField]
	public LevelBossDeathExploder rotatedExploder;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	public LevelBossDeathExploder pawRightExploder;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	public LevelBossDeathExploder pawLeftExploder;

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	public List<Animator> deathPuffs;
}
