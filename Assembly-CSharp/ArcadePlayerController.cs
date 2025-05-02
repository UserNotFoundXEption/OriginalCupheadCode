using System;
using UnityEngine;

// Token: 0x020004FC RID: 1276
public class ArcadePlayerController : AbstractPlayerController
{
	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x060034D4 RID: 13524 RVA: 0x0002B5AA File Offset: 0x000297AA
	public ArcadePlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<ArcadePlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x060034D5 RID: 13525 RVA: 0x0002B5CF File Offset: 0x000297CF
	public ArcadePlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<ArcadePlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060034D6 RID: 13526 RVA: 0x0002B5F4 File Offset: 0x000297F4
	public ArcadePlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<ArcadePlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060034D7 RID: 13527 RVA: 0x0002B619 File Offset: 0x00029819
	public ArcadePlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<ArcadePlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x060034D8 RID: 13528 RVA: 0x0002B63E File Offset: 0x0002983E
	public ArcadePlayerColliderManager colliderManager
	{
		get
		{
			if (this._colliderManager == null)
			{
				this._colliderManager = base.GetComponent<ArcadePlayerColliderManager>();
			}
			return this._colliderManager;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x060034D9 RID: 13529 RVA: 0x0002B663 File Offset: 0x00029863
	// (set) Token: 0x060034DA RID: 13530 RVA: 0x0002B66B File Offset: 0x0002986B
	public ArcadePlayerController.ControlScheme controlScheme { get; set; }

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060034DB RID: 13531 RVA: 0x000F78E4 File Offset: 0x000F5AE4
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || !this.motor.Dashing);
		}
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x0002B674 File Offset: 0x00029874
	public void Start()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Normal;
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0002B67D File Offset: 0x0002987D
	public void ChangeToRocket()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Rocket;
		this.weaponManager.ChangeToRocket();
		this.animationController.ChangeToRocket();
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x000F7940 File Offset: 0x000F5B40
	public void ChangeToJetpack()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Jetpack;
		this.weaponManager.ChangeToJetPack();
		this.animationController.ChangeToJetpack();
		base.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x000F7994 File Offset: 0x000F5B94
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x000F79C8 File Offset: 0x000F5BC8
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

	// Token: 0x060034E1 RID: 13537 RVA: 0x0002B69C File Offset: 0x0002989C
	public override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.weaponManager.LevelInit(id);
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x000F7A08 File Offset: 0x000F5C08
	public override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, base.transform.position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x0002B6BC File Offset: 0x000298BC
	public void DisableInput()
	{
		this.motor.DisableInput();
		this.weaponManager.DisableInput();
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x0002B6D4 File Offset: 0x000298D4
	public void OnLevelWinPause()
	{
		this.PauseAll();
		base.collider.enabled = false;
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0002B6E8 File Offset: 0x000298E8
	public override void OnLevelWin()
	{
		this.UnpauseAll(false);
		this.weaponManager.DisableInput();
		base.collider.enabled = false;
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x0002B708 File Offset: 0x00029908
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.DrawCube(this.CameraCenter, Vector3.one * 50f);
		}
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x0002B734 File Offset: 0x00029934
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x04002B45 RID: 11077
	public bool initialized;

	// Token: 0x04002B46 RID: 11078
	public ArcadePlayerMotor _motor;

	// Token: 0x04002B47 RID: 11079
	public ArcadePlayerAnimationController _animationController;

	// Token: 0x04002B48 RID: 11080
	public ArcadePlayerWeaponManager _weaponManager;

	// Token: 0x04002B49 RID: 11081
	public ArcadePlayerParryController _parryController;

	// Token: 0x04002B4A RID: 11082
	public ArcadePlayerColliderManager _colliderManager;

	// Token: 0x04002B4B RID: 11083
	[SerializeField]
	public PlayerDeathEffect deathEffect;

	// Token: 0x0200115B RID: 4443
	public enum ControlScheme
	{
		// Token: 0x04007A0B RID: 31243
		Normal,
		// Token: 0x04007A0C RID: 31244
		Rocket,
		// Token: 0x04007A0D RID: 31245
		Jetpack
	}
}
