using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public abstract class AbstractLevelWeapon : AbstractPausableComponent
{
	// Token: 0x060037F5 RID: 14325 RVA: 0x0002DA5E File Offset: 0x0002BC5E
	public AbstractLevelWeapon()
	{
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x060037F6 RID: 14326 RVA: 0x0002DA71 File Offset: 0x0002BC71
	// (set) Token: 0x060037F7 RID: 14327 RVA: 0x0002DA78 File Offset: 0x0002BC78
	public static bool ONE_PLAYER_FIRING { get; set; }

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060037F8 RID: 14328
	public abstract bool rapidFire { get; }

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x060037F9 RID: 14329
	public abstract float rapidFireRate { get; }

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x060037FA RID: 14330 RVA: 0x0002DA80 File Offset: 0x0002BC80
	// (set) Token: 0x060037FB RID: 14331 RVA: 0x0002DA88 File Offset: 0x0002BC88
	public Weapon id { get; set; }

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x060037FC RID: 14332 RVA: 0x0002DA91 File Offset: 0x0002BC91
	public virtual bool isChargeWeapon
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x060037FD RID: 14333 RVA: 0x0002DA94 File Offset: 0x0002BC94
	public override Transform emitTransform
	{
		get
		{
			return this.player.transform;
		}
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00105B20 File Offset: 0x00103D20
	public virtual void Initialize(LevelPlayerWeaponManager weaponManager, Weapon id)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<LevelPlayerController>();
		this.id = id;
		this.firing = new AbstractLevelWeapon.FiringSwitches();
		this.StartCoroutines();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x00105B70 File Offset: 0x00103D70
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x0002DAA1 File Offset: 0x0002BCA1
	public void OnRevive(Vector3 pos)
	{
		this.StartCoroutines();
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x00105BD4 File Offset: 0x00103DD4
	public void StartCoroutines()
	{
		this.StopAllCoroutines();
		if (this.isChargeWeapon)
		{
			base.StartCoroutine(this.chargeFireWeapon_cr(AbstractLevelWeapon.Mode.Basic));
		}
		else
		{
			base.StartCoroutine(this.fireWeapon_cr(AbstractLevelWeapon.Mode.Basic));
		}
		base.StartCoroutine(this.fireWeapon_cr(AbstractLevelWeapon.Mode.Ex));
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x0002DAA9 File Offset: 0x0002BCA9
	public virtual void OnEnable()
	{
		this.StartCoroutines();
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x00105C24 File Offset: 0x00103E24
	public void Update()
	{
		if (this.firing.Get(AbstractLevelWeapon.Mode.Basic) || this.firing.Get(AbstractLevelWeapon.Mode.Ex))
		{
			AbstractLevelWeapon.ONE_PLAYER_FIRING = true;
		}
		if (this.isUsingLoop && AudioManager.CheckIfPlaying(this.WeaponSound))
		{
			this.emitAudioFromObject.Add(this.WeaponSound);
		}
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x0002DAB1 File Offset: 0x0002BCB1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.exPrefab = null;
		this.exEffectPrefab = null;
		this.exFiringHitboxPrefab = null;
		this.basicPrefab = null;
		this.basicEffectPrefab = null;
		this.basicFiringHitboxPrefab = null;
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x0002DAE3 File Offset: 0x0002BCE3
	public virtual void BasicSoundOneShot(string soundP1, string soundP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Play(soundP1);
			this.emitAudioFromObject.Add(soundP1);
		}
		else
		{
			AudioManager.Play(soundP2);
			this.emitAudioFromObject.Add(soundP2);
		}
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x0002DB1E File Offset: 0x0002BD1E
	public virtual void OneShotCooldown(string sound)
	{
		if (this.coolingDown)
		{
			return;
		}
		AudioManager.Play(sound);
		this.emitAudioFromObject.Add(sound);
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x0002DB3E File Offset: 0x0002BD3E
	public virtual void ActivateCooldown()
	{
		if (this.coolingDown)
		{
			return;
		}
		this.coolingDown = true;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.shot_cooldown_cr());
		}
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x00105C88 File Offset: 0x00103E88
	public IEnumerator shot_cooldown_cr()
	{
		float t = 0f;
		float cooldownTime = Random.Range(4f, 7f);
		while (t < cooldownTime)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.coolingDown = false;
		yield break;
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x00105CA4 File Offset: 0x00103EA4
	public virtual void BeginBasicCheckAttenuation(string soundP1, string soundP2)
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			if (this.player.id == PlayerId.PlayerOne)
			{
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
			else
			{
				AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
		}
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x00105CF8 File Offset: 0x00103EF8
	public virtual void EndBasicCheckAttenuation(string soundP1, string soundP2)
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			if (this.player.id == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
					AudioManager.Attenuation(soundP1, false, 0.1f);
				}
			}
			else
			{
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
				AudioManager.Attenuation(soundP2, false, 0.1f);
			}
		}
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x00105D74 File Offset: 0x00103F74
	public virtual void BasicSoundLoop(string loopP1, string loopP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			this.WeaponSound = loopP1;
			AudioManager.PlayLoop(loopP1);
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
		else
		{
			this.WeaponSound = loopP2;
			AudioManager.PlayLoop(loopP2);
			AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
		this.isUsingLoop = true;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x00105DD8 File Offset: 0x00103FD8
	public virtual void StopLoopSound(string loopP1, string loopP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Stop(loopP1);
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
		}
		else
		{
			AudioManager.Stop(loopP2);
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0002DB70 File Offset: 0x0002BD70
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractLevelWeapon.Mode.Basic);
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0002DB79 File Offset: 0x0002BD79
	public virtual void EndBasic()
	{
		this.endFiring(AbstractLevelWeapon.Mode.Basic);
		AbstractLevelWeapon.ONE_PLAYER_FIRING = false;
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x00105E38 File Offset: 0x00104038
	public virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractLevelWeapon.Mode.Basic, true);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x00105E64 File Offset: 0x00104064
	public AbstractProjectile fireBasicNoEffect()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractLevelWeapon.Mode.Basic, false);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x0002DB88 File Offset: 0x0002BD88
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractLevelWeapon.Mode.Ex);
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x0002DB91 File Offset: 0x0002BD91
	public virtual void EndEx()
	{
		this.endFiring(AbstractLevelWeapon.Mode.Ex);
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x00105E90 File Offset: 0x00104090
	public virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractLevelWeapon.Mode.Ex, true);
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x0002DB9A File Offset: 0x0002BD9A
	public virtual void beginFiring(AbstractLevelWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x00105EA8 File Offset: 0x001040A8
	public virtual AbstractProjectile fireProjectile(AbstractLevelWeapon.Mode mode, bool createEffect = true)
	{
		Vector2 vector = this.weaponManager.GetBulletPosition();
		if (mode == AbstractLevelWeapon.Mode.Ex)
		{
			vector = this.weaponManager.ExPosition;
		}
		if (mode == AbstractLevelWeapon.Mode.Basic)
		{
			this.weaponManager.UpdateAim();
		}
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null && createEffect)
		{
			if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
			{
				Effect effect = this.basicEffectPrefab.Create(vector, base.transform.localScale);
				WeaponSparkEffect weaponSparkEffect = effect as WeaponSparkEffect;
				if (weaponSparkEffect != null)
				{
					LevelPlayerWeaponManager.Pose directionPose = this.weaponManager.GetDirectionPose();
					if (directionPose == LevelPlayerWeaponManager.Pose.Forward || directionPose == LevelPlayerWeaponManager.Pose.Forward_R || directionPose == LevelPlayerWeaponManager.Pose.Up_D || directionPose == LevelPlayerWeaponManager.Pose.Up_D_R)
					{
						weaponSparkEffect.SetPlayer(this.player);
					}
					if (directionPose == LevelPlayerWeaponManager.Pose.Down)
					{
						weaponSparkEffect.BringToFrontOfPlayer();
					}
				}
			}
			else
			{
				this.weaponManager.CreateExDust(this.GetEffect(mode));
			}
		}
		AbstractProjectile abstractProjectile = this.GetProjectile(mode).Create(vector, this.weaponManager.GetBulletRotation(), this.weaponManager.GetBulletScale());
		if (mode == AbstractLevelWeapon.Mode.Ex)
		{
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex;
			CupheadLevelCamera.Current.Shake(5f, 0.5f, false);
		}
		if (this.GetFiringHitbox(mode) != null)
		{
			abstractProjectile.AddFiringHitbox(this.GetFiringHitbox(mode).Create(vector, this.weaponManager.GetBulletRotation()));
		}
		abstractProjectile.PlayerId = this.player.id;
		return abstractProjectile;
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x0002DBB5 File Offset: 0x0002BDB5
	public virtual void endFiring(AbstractLevelWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x0002DBD0 File Offset: 0x0002BDD0
	public AbstractProjectile GetProjectile(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicPrefab;
		}
		return this.exPrefab;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x0002DBF1 File Offset: 0x0002BDF1
	public Effect GetEffect(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x0002DC12 File Offset: 0x0002BE12
	public LevelPlayerWeaponFiringHitbox GetFiringHitbox(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicFiringHitboxPrefab;
		}
		return this.exFiringHitboxPrefab;
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x0002DC33 File Offset: 0x0002BE33
	public AbstractLevelWeapon.FireProjectileDelegate getFiringMethod(AbstractLevelWeapon.Mode mode)
	{
		if (mode != AbstractLevelWeapon.Mode.Ex)
		{
			if (mode != AbstractLevelWeapon.Mode.Basic)
			{
			}
			return new AbstractLevelWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractLevelWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x00106044 File Offset: 0x00104244
	public virtual IEnumerator fireWeapon_cr(AbstractLevelWeapon.Mode mode)
	{
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (!this.player.motor.Dashing)
			{
				if (mode == AbstractLevelWeapon.Mode.Basic && this.t < this.rapidFireRate)
				{
					if (this.weaponManager.CurrentWeapon == this)
					{
						this.t += CupheadTime.FixedDelta;
					}
				}
				else if (this.firing.Get(mode) && this.weaponManager.IsShooting)
				{
					this.weaponManager.TriggerWeaponFire();
					this.getFiringMethod(mode)();
					if (mode == AbstractLevelWeapon.Mode.Ex || !this.rapidFire)
					{
						this.firing.Set(mode, false);
						this.weaponManager.IsShooting = false;
					}
					this.t = 0f;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x00106068 File Offset: 0x00104268
	public virtual IEnumerator chargeFireWeapon_cr(AbstractLevelWeapon.Mode mode)
	{
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (mode == AbstractLevelWeapon.Mode.Basic && this.firing.Get(mode) && this.weaponManager.IsShooting)
			{
				this.alreadyHeld = true;
			}
			else if (mode == AbstractLevelWeapon.Mode.Basic && this.alreadyHeld)
			{
				this.alreadyReleased = true;
			}
			if (mode == AbstractLevelWeapon.Mode.Basic && this.t < this.rapidFireRate)
			{
				if (this.weaponManager.CurrentWeapon == this)
				{
					this.t += CupheadTime.FixedDelta;
					this.charging = false;
				}
			}
			else if (this.firing.Get(mode) && this.weaponManager.IsShooting && !this.player.motor.Dashing && !this.player.motor.IsHit && !this.player.motor.IsUsingSuperOrEx && !this.alreadyReleased)
			{
				if (!this.charging)
				{
					this.StartCharging();
				}
				this.charging = true;
			}
			else if (this.charging || this.alreadyReleased)
			{
				this.charging = false;
				this.alreadyReleased = false;
				this.alreadyHeld = false;
				this.weaponManager.TriggerWeaponFire();
				this.getFiringMethod(mode)();
				if (!this.weaponManager.IsShooting)
				{
					this.firing.Set(mode, false);
				}
				this.t = 0f;
			}
			else if (!this.charging)
			{
				this.StopCharging();
			}
		}
		yield break;
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x0002DC62 File Offset: 0x0002BE62
	public virtual void StartCharging()
	{
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x0002DC64 File Offset: 0x0002BE64
	public virtual void StopCharging()
	{
	}

	// Token: 0x04002D1A RID: 11546
	[Header("Ex")]
	[SerializeField]
	public AbstractProjectile exPrefab;

	// Token: 0x04002D1B RID: 11547
	[SerializeField]
	public Effect exEffectPrefab;

	// Token: 0x04002D1C RID: 11548
	[SerializeField]
	public LevelPlayerWeaponFiringHitbox exFiringHitboxPrefab;

	// Token: 0x04002D1D RID: 11549
	[Header("Basic")]
	[SerializeField]
	public AbstractProjectile basicPrefab;

	// Token: 0x04002D1E RID: 11550
	[SerializeField]
	public Effect basicEffectPrefab;

	// Token: 0x04002D1F RID: 11551
	[SerializeField]
	public LevelPlayerWeaponFiringHitbox basicFiringHitboxPrefab;

	// Token: 0x04002D21 RID: 11553
	public AbstractLevelWeapon.FiringSwitches firing;

	// Token: 0x04002D22 RID: 11554
	public LevelPlayerController player;

	// Token: 0x04002D23 RID: 11555
	public LevelPlayerWeaponManager weaponManager;

	// Token: 0x04002D24 RID: 11556
	public string WeaponSound;

	// Token: 0x04002D25 RID: 11557
	public bool isUsingLoop;

	// Token: 0x04002D26 RID: 11558
	public bool coolingDown;

	// Token: 0x04002D27 RID: 11559
	public float t = 1000f;

	// Token: 0x04002D28 RID: 11560
	public bool charging;

	// Token: 0x04002D29 RID: 11561
	public bool alreadyHeld;

	// Token: 0x04002D2A RID: 11562
	public bool alreadyReleased;

	// Token: 0x020011B4 RID: 4532
	public enum Mode
	{
		// Token: 0x04007BDD RID: 31709
		Basic,
		// Token: 0x04007BDE RID: 31710
		Ex
	}

	// Token: 0x020011B5 RID: 4533
	// (Invoke) Token: 0x06007EB6 RID: 32438
	public delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x020011B6 RID: 4534
	[Serializable]
	public class Prefabs
	{
		// Token: 0x06007EBA RID: 32442 RVA: 0x00055032 File Offset: 0x00053232
		public AbstractProjectile Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007BDF RID: 31711
		public AbstractProjectile basic;

		// Token: 0x04007BE0 RID: 31712
		public AbstractProjectile ex;
	}

	// Token: 0x020011B7 RID: 4535
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x06007EBC RID: 32444 RVA: 0x0005505B File Offset: 0x0005325B
		public Effect Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007BE1 RID: 31713
		public Effect basic;

		// Token: 0x04007BE2 RID: 31714
		public Effect ex;
	}

	// Token: 0x020011B8 RID: 4536
	public class FiringSwitches
	{
		// Token: 0x06007EBE RID: 32446 RVA: 0x00055084 File Offset: 0x00053284
		public bool Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x000550A5 File Offset: 0x000532A5
		public void Set(AbstractLevelWeapon.Mode mode, bool val)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x04007BE3 RID: 31715
		public bool basic;

		// Token: 0x04007BE4 RID: 31716
		public bool ex;
	}
}
