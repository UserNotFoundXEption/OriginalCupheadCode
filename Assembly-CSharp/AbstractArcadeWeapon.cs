using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000501 RID: 1281
public abstract class AbstractArcadeWeapon : AbstractPausableComponent
{
	// Token: 0x0600358F RID: 13711 RVA: 0x0002BF20 File Offset: 0x0002A120
	public AbstractArcadeWeapon()
	{
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06003590 RID: 13712
	public abstract bool rapidFire { get; }

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06003591 RID: 13713
	public abstract float rapidFireRate { get; }

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06003592 RID: 13714 RVA: 0x0002BF28 File Offset: 0x0002A128
	// (set) Token: 0x06003593 RID: 13715 RVA: 0x0002BF30 File Offset: 0x0002A130
	public Weapon id { get; set; }

	// Token: 0x06003594 RID: 13716 RVA: 0x000FA6F8 File Offset: 0x000F88F8
	public virtual void Initialize(ArcadePlayerWeaponManager weaponManager, Weapon id)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<ArcadePlayerController>();
		this.id = id;
		this.firing = new AbstractArcadeWeapon.FiringSwitches();
		this.StartCoroutines();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000FA748 File Offset: 0x000F8948
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x0002BF39 File Offset: 0x0002A139
	public void OnRevive(Vector3 pos)
	{
		this.StartCoroutines();
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x0002BF41 File Offset: 0x0002A141
	public void StartCoroutines()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fireWeapon_cr(AbstractArcadeWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractArcadeWeapon.Mode.Ex));
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x0002BF65 File Offset: 0x0002A165
	public void OnEnable()
	{
		this.StartCoroutines();
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x0002BF6D File Offset: 0x0002A16D
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractArcadeWeapon.Mode.Basic);
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x0002BF76 File Offset: 0x0002A176
	public virtual void EndBasic()
	{
		this.endFiring(AbstractArcadeWeapon.Mode.Basic);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x000FA7AC File Offset: 0x000F89AC
	public virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractArcadeWeapon.Mode.Basic);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x0002BF7F File Offset: 0x0002A17F
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x0002BF88 File Offset: 0x0002A188
	public virtual void EndEx()
	{
		this.endFiring(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x000FA7D4 File Offset: 0x000F89D4
	public virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x0002BF91 File Offset: 0x0002A191
	public virtual void beginFiring(AbstractArcadeWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x000FA7EC File Offset: 0x000F89EC
	public virtual AbstractProjectile fireProjectile(AbstractArcadeWeapon.Mode mode)
	{
		Vector2 position = this.weaponManager.GetBulletPosition();
		if (mode == AbstractArcadeWeapon.Mode.Ex)
		{
			position = this.weaponManager.ExPosition;
		}
		if (mode == AbstractArcadeWeapon.Mode.Basic)
		{
			this.weaponManager.UpdateAim();
		}
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null)
		{
			if (mode != AbstractArcadeWeapon.Mode.Basic && mode == AbstractArcadeWeapon.Mode.Ex)
			{
				this.weaponManager.CreateExDust(this.GetEffect(mode));
			}
		}
		this.weaponManager.UpdateAim();
		return this.GetProjectile(mode).Create(position, this.weaponManager.GetBulletRotation(), this.weaponManager.GetBulletScale());
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x0002BFAC File Offset: 0x0002A1AC
	public virtual void endFiring(AbstractArcadeWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x0002BFC7 File Offset: 0x0002A1C7
	public AbstractProjectile GetProjectile(AbstractArcadeWeapon.Mode mode)
	{
		if (mode == AbstractArcadeWeapon.Mode.Basic || mode != AbstractArcadeWeapon.Mode.Ex)
		{
			return this.basicPrefab;
		}
		return this.exPrefab;
	}

	// Token: 0x060035A3 RID: 13731 RVA: 0x0002BFE8 File Offset: 0x0002A1E8
	public Effect GetEffect(AbstractArcadeWeapon.Mode mode)
	{
		if (mode == AbstractArcadeWeapon.Mode.Basic || mode != AbstractArcadeWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x060035A4 RID: 13732 RVA: 0x0002C009 File Offset: 0x0002A209
	public AbstractArcadeWeapon.FireProjectileDelegate getFiringMethod(AbstractArcadeWeapon.Mode mode)
	{
		if (mode != AbstractArcadeWeapon.Mode.Ex)
		{
			if (mode != AbstractArcadeWeapon.Mode.Basic)
			{
			}
			return new AbstractArcadeWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractArcadeWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x000FA8B4 File Offset: 0x000F8AB4
	public virtual IEnumerator fireWeapon_cr(AbstractArcadeWeapon.Mode mode)
	{
		float t = 0f;
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (!this.player.motor.Dashing)
			{
				if (t < this.rapidFireRate)
				{
					t += CupheadTime.FixedDelta;
				}
				else if (this.firing.Get(mode) && this.weaponManager.IsShooting)
				{
					this.weaponManager.TriggerWeaponFire();
					this.getFiringMethod(mode)();
					if (mode == AbstractArcadeWeapon.Mode.Ex || !this.rapidFire)
					{
						this.firing.Set(mode, false);
						this.weaponManager.IsShooting = false;
					}
					t = 0f;
				}
			}
		}
		yield break;
	}

	// Token: 0x04002B8C RID: 11148
	[Header("Ex")]
	[SerializeField]
	public AbstractProjectile exPrefab;

	// Token: 0x04002B8D RID: 11149
	[SerializeField]
	public Effect exEffectPrefab;

	// Token: 0x04002B8E RID: 11150
	[Header("Basic")]
	[SerializeField]
	public AbstractProjectile basicPrefab;

	// Token: 0x04002B8F RID: 11151
	[SerializeField]
	public Effect basicEffectPrefab;

	// Token: 0x04002B91 RID: 11153
	public AbstractArcadeWeapon.FiringSwitches firing;

	// Token: 0x04002B92 RID: 11154
	public ArcadePlayerController player;

	// Token: 0x04002B93 RID: 11155
	public ArcadePlayerWeaponManager weaponManager;

	// Token: 0x02001174 RID: 4468
	public enum Mode
	{
		// Token: 0x04007A7F RID: 31359
		Basic,
		// Token: 0x04007A80 RID: 31360
		Ex
	}

	// Token: 0x02001175 RID: 4469
	// (Invoke) Token: 0x06007DB6 RID: 32182
	public delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x02001176 RID: 4470
	[Serializable]
	public class Prefabs
	{
		// Token: 0x06007DBA RID: 32186 RVA: 0x000546D7 File Offset: 0x000528D7
		public AbstractProjectile Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007A81 RID: 31361
		public AbstractProjectile basic;

		// Token: 0x04007A82 RID: 31362
		public AbstractProjectile ex;
	}

	// Token: 0x02001177 RID: 4471
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x06007DBC RID: 32188 RVA: 0x00054700 File Offset: 0x00052900
		public Effect Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007A83 RID: 31363
		public Effect basic;

		// Token: 0x04007A84 RID: 31364
		public Effect ex;
	}

	// Token: 0x02001178 RID: 4472
	public class FiringSwitches
	{
		// Token: 0x06007DBE RID: 32190 RVA: 0x00054729 File Offset: 0x00052929
		public bool Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x06007DBF RID: 32191 RVA: 0x0005474A File Offset: 0x0005294A
		public void Set(AbstractArcadeWeapon.Mode mode, bool val)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x04007A85 RID: 31365
		public bool basic;

		// Token: 0x04007A86 RID: 31366
		public bool ex;
	}
}
