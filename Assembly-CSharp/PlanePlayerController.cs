using System;
using UnityEngine;

// Token: 0x02000560 RID: 1376
public class PlanePlayerController : AbstractPlayerController
{
	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600399D RID: 14749 RVA: 0x0002EE32 File Offset: 0x0002D032
	public bool Shrunk
	{
		get
		{
			return this.animationController.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Shrunk;
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x0600399E RID: 14750 RVA: 0x0002EE42 File Offset: 0x0002D042
	public bool Parrying
	{
		get
		{
			return this.parryController.State == PlanePlayerParryController.ParryState.Parrying;
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x0002EE52 File Offset: 0x0002D052
	public bool WeaponBusy
	{
		get
		{
			return this.weaponManager.state != PlanePlayerWeaponManager.State.Ready || !this.weaponManager.CanInterupt;
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x060039A0 RID: 14752 RVA: 0x0002EE76 File Offset: 0x0002D076
	public PlanePlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<PlanePlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x0002EE9B File Offset: 0x0002D09B
	public PlanePlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<PlanePlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x060039A2 RID: 14754 RVA: 0x0002EEC0 File Offset: 0x0002D0C0
	public PlanePlayerAudioController audioController
	{
		get
		{
			if (this._audioController == null)
			{
				this._audioController = base.GetComponent<PlanePlayerAudioController>();
			}
			return this._audioController;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x060039A3 RID: 14755 RVA: 0x0002EEE5 File Offset: 0x0002D0E5
	public PlanePlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<PlanePlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x060039A4 RID: 14756 RVA: 0x0002EF0A File Offset: 0x0002D10A
	public PlanePlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<PlanePlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0010CD54 File Offset: 0x0010AF54
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || !this.animationController.Shrinking);
		}
	}

	// Token: 0x060039A6 RID: 14758 RVA: 0x0002EF2F File Offset: 0x0002D12F
	public void Start()
	{
		if (!Level.Current.Started)
		{
			this.motor.enabled = false;
		}
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x0002EF4C File Offset: 0x0002D14C
	public override void PlayIntro()
	{
		base.PlayIntro();
		this.animationController.PlayIntro();
	}

	// Token: 0x060039A8 RID: 14760 RVA: 0x0002EF5F File Offset: 0x0002D15F
	public override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.audioController.LevelInit();
		if (base.stats.Health == 0)
		{
			this.StartDead();
		}
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x0002EF94 File Offset: 0x0002D194
	public override void LevelStart()
	{
		base.LevelStart();
		this.motor.enabled = true;
	}

	// Token: 0x060039AA RID: 14762 RVA: 0x0002EFA8 File Offset: 0x0002D1A8
	public void GetStoned(float stoneTime)
	{
		base.stats.GetStoned(stoneTime);
	}

	// Token: 0x060039AB RID: 14763 RVA: 0x0010CDB0 File Offset: 0x0010AFB0
	public override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, base.transform.position, base.stats.Deaths, PlayerMode.Plane, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
	}

	// Token: 0x060039AC RID: 14764 RVA: 0x0002EFB6 File Offset: 0x0002D1B6
	public override void OnLeave(PlayerId playerId)
	{
		if (!base.IsDead)
		{
			this.deathEffect.CreateExplosionOnly(base.id, base.transform.position, PlayerMode.Plane);
		}
		base.OnLeave(playerId);
	}

	// Token: 0x060039AD RID: 14765 RVA: 0x0010CE34 File Offset: 0x0010B034
	public void StartDead()
	{
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		position.y += 1000f;
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Plane, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x060039AE RID: 14766 RVA: 0x0010CEC0 File Offset: 0x0010B0C0
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x060039AF RID: 14767 RVA: 0x0010CEF4 File Offset: 0x0010B0F4
	public void UnpauseAll(bool forced = false)
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			if (forced)
			{
				abstractPausableComponent.preEnabled = true;
			}
			abstractPausableComponent.enabled = true;
		}
	}

	// Token: 0x060039B0 RID: 14768 RVA: 0x0002EFEC File Offset: 0x0002D1EC
	public void SetSpriteVisible(bool visibility)
	{
		this.animationController.SetSpriteVisible(visibility);
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x0002EFFA File Offset: 0x0002D1FA
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x04002E46 RID: 11846
	public const float INTRO_TIME = 1f;

	// Token: 0x04002E47 RID: 11847
	public PlanePlayerMotor _motor;

	// Token: 0x04002E48 RID: 11848
	public PlanePlayerAnimationController _animationController;

	// Token: 0x04002E49 RID: 11849
	public PlanePlayerAudioController _audioController;

	// Token: 0x04002E4A RID: 11850
	public PlanePlayerWeaponManager _weaponManager;

	// Token: 0x04002E4B RID: 11851
	public PlanePlayerParryController _parryController;

	// Token: 0x04002E4C RID: 11852
	[SerializeField]
	public PlayerDeathEffect deathEffect;
}
