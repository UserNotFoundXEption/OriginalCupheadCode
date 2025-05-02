using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public abstract class AbstractPlaneWeapon : AbstractPausableComponent
{
	// Token: 0x06003A42 RID: 14914 RVA: 0x0002F67F File Offset: 0x0002D87F
	public AbstractPlaneWeapon()
	{
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06003A43 RID: 14915
	public abstract bool rapidFire { get; }

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06003A44 RID: 14916
	public abstract float rapidFireRate { get; }

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06003A45 RID: 14917 RVA: 0x0002F69D File Offset: 0x0002D89D
	// (set) Token: 0x06003A46 RID: 14918 RVA: 0x0002F6A5 File Offset: 0x0002D8A5
	public int index { get; set; }

	// Token: 0x06003A47 RID: 14919 RVA: 0x0010EE2C File Offset: 0x0010D02C
	public virtual void Initialize(PlanePlayerWeaponManager weaponManager, int index)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<PlanePlayerController>();
		this.index = index;
		this.firing = new AbstractPlaneWeapon.FiringSwitches();
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex));
		base.StartCoroutine(this.endFiringAnimation_cr());
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x0002F6AE File Offset: 0x0002D8AE
	public void OnRevive(Vector3 pos)
	{
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex));
		base.StartCoroutine(this.endFiringAnimation_cr());
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x0010EEA0 File Offset: 0x0010D0A0
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x0002F6D9 File Offset: 0x0002D8D9
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.exPrefab = null;
		this.exEffectPrefab = null;
		this.basicPrefab = null;
		this.basicEffectPrefab = null;
		this.shrunkPrefab = null;
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x0002F704 File Offset: 0x0002D904
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractPlaneWeapon.Mode.Basic);
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x0002F70D File Offset: 0x0002D90D
	public virtual void EndBasic()
	{
		this.endFiring(AbstractPlaneWeapon.Mode.Basic);
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x0010EF04 File Offset: 0x0010D104
	public virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractPlaneWeapon.Mode.Basic);
		abstractProjectile.PlayerId = this.player.id;
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x0002F716 File Offset: 0x0002D916
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x0002F71F File Offset: 0x0002D91F
	public virtual void EndEx()
	{
		this.endFiring(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x0010EF40 File Offset: 0x0010D140
	public virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x0002F728 File Offset: 0x0002D928
	public virtual void beginFiring(AbstractPlaneWeapon.Mode mode)
	{
		base.StopCoroutine("endFiringAnimation_cr");
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x0010EF58 File Offset: 0x0010D158
	public virtual AbstractProjectile fireProjectile(AbstractPlaneWeapon.Mode mode)
	{
		Vector2 vector = this.weaponManager.GetBulletPosition() + new Vector2(-10f, 0f) + new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null)
		{
			if (mode == AbstractPlaneWeapon.Mode.Basic || mode != AbstractPlaneWeapon.Mode.Ex)
			{
				this.basicEffectPrefab.Create(vector, base.transform.localScale).transform.SetParent(base.transform);
			}
		}
		AbstractProjectile abstractProjectile = this.GetProjectile(mode).Create(vector);
		if (mode == AbstractPlaneWeapon.Mode.Ex)
		{
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex;
			CupheadLevelCamera.Current.Shake(5f, 0.5f, false);
		}
		abstractProjectile.PlayerId = this.player.id;
		return abstractProjectile;
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0002F74E File Offset: 0x0002D94E
	public virtual void endFiring(AbstractPlaneWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x0002F769 File Offset: 0x0002D969
	public AbstractProjectile GetProjectile(AbstractPlaneWeapon.Mode mode)
	{
		if (mode != AbstractPlaneWeapon.Mode.Basic && mode == AbstractPlaneWeapon.Mode.Ex)
		{
			return this.exPrefab;
		}
		if (this.player.Shrunk)
		{
			return this.shrunkPrefab;
		}
		return this.basicPrefab;
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x0002F7A1 File Offset: 0x0002D9A1
	public virtual Effect GetEffect(AbstractPlaneWeapon.Mode mode)
	{
		if (mode == AbstractPlaneWeapon.Mode.Basic || mode != AbstractPlaneWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x0002F7C2 File Offset: 0x0002D9C2
	public AbstractPlaneWeapon.FireProjectileDelegate getFiringMethod(AbstractPlaneWeapon.Mode mode)
	{
		if (mode != AbstractPlaneWeapon.Mode.Ex)
		{
			if (mode != AbstractPlaneWeapon.Mode.Basic)
			{
			}
			return new AbstractPlaneWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractPlaneWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x0010F060 File Offset: 0x0010D260
	public IEnumerator fireWeapon_cr(AbstractPlaneWeapon.Mode mode)
	{
		float time = this.rapidFireRate;
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (mode == AbstractPlaneWeapon.Mode.Basic && this.t < time)
			{
				if (this.weaponManager.CurrentWeapon == this)
				{
					this.t += CupheadTime.FixedDelta;
				}
			}
			else if (this.firing.Get(mode))
			{
				this.getFiringMethod(mode)();
				if (mode == AbstractPlaneWeapon.Mode.Ex || !this.rapidFire)
				{
					this.firing.Set(mode, false);
					base.StartCoroutine(this.endFiringAnimation_cr());
				}
				this.t = 0f;
			}
		}
		yield break;
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x0010F084 File Offset: 0x0010D284
	public IEnumerator endFiringAnimation_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.166666672f);
		this.weaponManager.IsShooting = false;
		yield break;
	}

	// Token: 0x04002EAA RID: 11946
	public const int ANIMATION_FRAMES = 10;

	// Token: 0x04002EAB RID: 11947
	[Header("Ex")]
	[SerializeField]
	public AbstractProjectile exPrefab;

	// Token: 0x04002EAC RID: 11948
	[SerializeField]
	public Effect exEffectPrefab;

	// Token: 0x04002EAD RID: 11949
	[Header("Basic")]
	[SerializeField]
	public AbstractProjectile basicPrefab;

	// Token: 0x04002EAE RID: 11950
	[SerializeField]
	public Effect basicEffectPrefab;

	// Token: 0x04002EAF RID: 11951
	[Header("Shrunk")]
	[SerializeField]
	public AbstractProjectile shrunkPrefab;

	// Token: 0x04002EB0 RID: 11952
	[SerializeField]
	public float shrunkDamageMultiplier = 0.5f;

	// Token: 0x04002EB2 RID: 11954
	public AbstractPlaneWeapon.FiringSwitches firing;

	// Token: 0x04002EB3 RID: 11955
	public PlanePlayerController player;

	// Token: 0x04002EB4 RID: 11956
	public PlanePlayerWeaponManager weaponManager;

	// Token: 0x04002EB5 RID: 11957
	public float t = 1000f;

	// Token: 0x020011ED RID: 4589
	public enum Mode
	{
		// Token: 0x04007CEE RID: 31982
		Basic,
		// Token: 0x04007CEF RID: 31983
		Ex
	}

	// Token: 0x020011EE RID: 4590
	// (Invoke) Token: 0x06007FAC RID: 32684
	public delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x020011EF RID: 4591
	[Serializable]
	public class Prefabs
	{
		// Token: 0x06007FB0 RID: 32688 RVA: 0x00055819 File Offset: 0x00053A19
		public AbstractProjectile Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007CF0 RID: 31984
		public AbstractProjectile basic;

		// Token: 0x04007CF1 RID: 31985
		public AbstractProjectile ex;
	}

	// Token: 0x020011F0 RID: 4592
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x06007FB2 RID: 32690 RVA: 0x00055842 File Offset: 0x00053A42
		public Effect Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04007CF2 RID: 31986
		public Effect basic;

		// Token: 0x04007CF3 RID: 31987
		public Effect ex;
	}

	// Token: 0x020011F1 RID: 4593
	public class FiringSwitches
	{
		// Token: 0x06007FB4 RID: 32692 RVA: 0x0005586B File Offset: 0x00053A6B
		public bool Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x0005588C File Offset: 0x00053A8C
		public void Set(AbstractPlaneWeapon.Mode mode, bool val)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x04007CF4 RID: 31988
		public bool basic;

		// Token: 0x04007CF5 RID: 31989
		public bool ex;
	}
}
