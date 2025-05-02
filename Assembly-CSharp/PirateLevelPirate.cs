using System;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class PirateLevelPirate : LevelProperties.Pirate.Entity
{
	// Token: 0x060021E4 RID: 8676 RVA: 0x000BB9F8 File Offset: 0x000B9BF8
	public override void Awake()
	{
		base.Awake();
		PirateLevel pirateLevel = Level.Current as PirateLevel;
		pirateLevel.OnWhistleEvent += this.onWhistle;
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000BBA28 File Offset: 0x000B9C28
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnIntroEvent += this.OnIntroLaugh;
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x0001CF87 File Offset: 0x0001B187
	public void OnIntroLaugh()
	{
		base.animator.SetTrigger("OnLaugh");
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x0001CF99 File Offset: 0x0001B199
	public void onWhistle(PirateLevel.Creature creature)
	{
		this.whistles = 0;
		this.creature = creature;
		base.animator.SetTrigger("OnWhistle");
		this.loops = 1000;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000BBA7C File Offset: 0x000B9C7C
	public void OnIdleEnd()
	{
		if (this.loops >= this.max)
		{
			int num = Random.Range(0, 100);
			int num2 = 0;
			if (num <= this.bothChance)
			{
				num2 = 2;
			}
			else if (num <= this.patchChance + this.bothChance)
			{
				num2 = 1;
			}
			base.animator.SetInteger("Blink", num2);
			base.animator.SetTrigger("OnBlink");
			return;
		}
		this.loops++;
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
	public void OnBlink()
	{
		this.max = Random.Range(2, 5);
		this.loops = 0;
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x0001CFDA File Offset: 0x0001B1DA
	public void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_pirate_fall_death");
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x0001CFFC File Offset: 0x0001B1FC
	public void FireGun(LevelProperties.Pirate.Peashot properties)
	{
		base.animator.Play("Gun_Shoot");
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000BBB00 File Offset: 0x000B9D00
	public void Whistle()
	{
		int num = 1;
		PirateLevel.Creature creature = this.creature;
		if (creature != PirateLevel.Creature.DogFish)
		{
			if (creature == PirateLevel.Creature.Shark)
			{
				num = 3;
			}
		}
		else
		{
			num = 2;
		}
		if (this.whistles >= num)
		{
			return;
		}
		this.whistleEffect.Create(this.whistleRoot.position);
		this.whistles++;
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x0001D00E File Offset: 0x0001B20E
	public void WhistleSFX()
	{
		AudioManager.Play("levels_pirate_whistle");
		this.emitAudioFromObject.Add("levels_pirate_whistle");
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x0001D02A File Offset: 0x0001B22A
	public void EndGun()
	{
		base.animator.SetTrigger("OnGunEnd");
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x0001D03C File Offset: 0x0001B23C
	public void PlayLaughSound()
	{
		AudioManager.Play("levels_pirate_laugh");
		this.emitAudioFromObject.Add("levels_pirate_laugh");
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000BBB6C File Offset: 0x000B9D6C
	public void StartGun()
	{
		base.animator.SetTrigger("OnGunStart");
		this.gunProperties = base.properties.CurrentState.peashot;
		this.shotIndex = Random.Range(0, this.gunProperties.shotType.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000BBBC8 File Offset: 0x000B9DC8
	public void Shoot()
	{
		if (PlayerManager.Count <= 0)
		{
			this.gunRoot.LookAt2D(new Vector2(0f, 0f));
			return;
		}
		this.gunRoot.LookAt2D(PlayerManager.GetNext().center);
		AudioManager.Play("level_pirate_gun_shoot");
		this.emitAudioFromObject.Add("level_pirate_gun_shoot");
		this.muzzleFlash.Create(this.gunRoot.position);
		BasicProjectile basicProjectile = null;
		if (this.gunProperties.shotType.Split(new char[]
		{
			','
		})[this.shotIndex][0] == 'P')
		{
			basicProjectile = this.gunProjectile.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, new Vector3(-1f, -1f, 1f), this.gunProperties.speed);
			basicProjectile.SetParryable(true);
		}
		else if (this.gunProperties.shotType.Split(new char[]
		{
			','
		})[this.shotIndex][0] == 'R')
		{
			basicProjectile = this.gunProjectileRegular.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, new Vector3(-1f, -1f, 1f), this.gunProperties.speed);
		}
		basicProjectile.CollisionDeath.OnlyBounds();
		basicProjectile.CollisionDeath.Player = true;
		this.shotIndex = (this.shotIndex + 1) % this.gunProperties.shotType.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x0001D058 File Offset: 0x0001B258
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.properties.CurrentState.stateName == LevelProperties.Pirate.States.Boat)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x0001D082 File Offset: 0x0001B282
	public void CleanUp()
	{
		base.properties.OnBossDeath -= this.OnBossDeath;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x0001D0A6 File Offset: 0x0001B2A6
	public void SoundGunStart()
	{
		AudioManager.Play("level_pirate_gun_start");
		this.emitAudioFromObject.Add("level_pirate_gun_start");
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x0001D0C2 File Offset: 0x0001B2C2
	public void SoundGunEnd()
	{
		AudioManager.Play("level_pirate_gun_end");
		this.emitAudioFromObject.Add("level_pirate_gun_end");
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x0001D0DE File Offset: 0x0001B2DE
	public void SoundPirateFoot()
	{
		AudioManager.Play("level_pirate_pirate_foot");
		this.emitAudioFromObject.Add("level_pirate_pirate_foot");
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x0001D0FA File Offset: 0x0001B2FA
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.gunProjectile = null;
		this.gunProjectileRegular = null;
		this.muzzleFlash = null;
		this.whistleEffect = null;
	}

	// Token: 0x04001BE7 RID: 7143
	public const int MIN_IDLE_LOOPS = 2;

	// Token: 0x04001BE8 RID: 7144
	public const int MAX_IDLE_LOOPS = 4;

	// Token: 0x04001BE9 RID: 7145
	[SerializeField]
	public Transform gunRoot;

	// Token: 0x04001BEA RID: 7146
	[SerializeField]
	public BasicProjectile gunProjectile;

	// Token: 0x04001BEB RID: 7147
	[SerializeField]
	public BasicProjectile gunProjectileRegular;

	// Token: 0x04001BEC RID: 7148
	[SerializeField]
	public Effect muzzleFlash;

	// Token: 0x04001BED RID: 7149
	[SerializeField]
	public Transform whistleRoot;

	// Token: 0x04001BEE RID: 7150
	[SerializeField]
	public Effect whistleEffect;

	// Token: 0x04001BEF RID: 7151
	public LevelProperties.Pirate.Peashot gunProperties;

	// Token: 0x04001BF0 RID: 7152
	public PirateLevel.Creature creature;

	// Token: 0x04001BF1 RID: 7153
	public int whistles;

	// Token: 0x04001BF2 RID: 7154
	public int patchChance = 25;

	// Token: 0x04001BF3 RID: 7155
	public int bothChance = 15;

	// Token: 0x04001BF4 RID: 7156
	public int loops;

	// Token: 0x04001BF5 RID: 7157
	public int max = 2;

	// Token: 0x04001BF6 RID: 7158
	public int shotIndex;
}
