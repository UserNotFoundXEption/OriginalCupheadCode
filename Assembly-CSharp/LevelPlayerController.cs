using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public class LevelPlayerController : AbstractPlayerController
{
	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06003657 RID: 13911 RVA: 0x0002C872 File Offset: 0x0002AA72
	public bool Ducking
	{
		get
		{
			return this.motor.Ducking;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06003658 RID: 13912 RVA: 0x0002C87F File Offset: 0x0002AA7F
	public LevelPlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<LevelPlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06003659 RID: 13913 RVA: 0x0002C8A4 File Offset: 0x0002AAA4
	public LevelPlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<LevelPlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x0600365A RID: 13914 RVA: 0x0002C8C9 File Offset: 0x0002AAC9
	public LevelPlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<LevelPlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x0600365B RID: 13915 RVA: 0x0002C8EE File Offset: 0x0002AAEE
	public LevelPlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<LevelPlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x0600365C RID: 13916 RVA: 0x0002C913 File Offset: 0x0002AB13
	public LevelPlayerColliderManager colliderManager
	{
		get
		{
			if (this._colliderManager == null)
			{
				this._colliderManager = base.GetComponent<LevelPlayerColliderManager>();
			}
			return this._colliderManager;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x0600365D RID: 13917 RVA: 0x000FE39C File Offset: 0x000FC59C
	public override Vector3 center
	{
		get
		{
			if (base.transform == null)
			{
				return Vector3.zero;
			}
			return base.transform.position + new Vector3(base.collider.offset.x, base.collider.offset.y * this.motor.GravityReversalMultiplier, 0f);
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x0600365E RID: 13918 RVA: 0x000FE40C File Offset: 0x000FC60C
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || Level.IsChessBoss || !this.motor.Dashing) && (!base.stats.isChalice || !this.motor.Dashing || !this.motor.ChaliceDuckDashed) && (!base.stats.isChalice || !this.motor.Dashing || true);
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x0600365F RID: 13919 RVA: 0x000FE4C8 File Offset: 0x000FC6C8
	public override Vector3 CameraCenter
	{
		get
		{
			if (Level.Current.LevelType == Level.Type.Platforming)
			{
				this.cameraCenterPosition = Mathf.Lerp(this.cameraCenterPosition, 250f * (float)this._motor.TrueLookDirection.x.Value, 1.2f * CupheadTime.Delta);
				return this.center + new Vector3(this.cameraCenterPosition, 0f);
			}
			return base.CameraCenter;
		}
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x000FE548 File Offset: 0x000FC748
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x000FE57C File Offset: 0x000FC77C
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

	// Token: 0x06003662 RID: 13922 RVA: 0x000FE5BC File Offset: 0x000FC7BC
	public void OnPitKnockUp(float y, float velocityScale = 1f)
	{
		if (base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && base.stats.Loadout.charm != Charm.charm_float)
		{
			base.stats.OnPitKnockUp();
		}
		this.motor.OnPitKnockUp(y, velocityScale);
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x0002C938 File Offset: 0x0002AB38
	public override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.weaponManager.LevelInit(id);
		if (base.stats.Health == 0)
		{
			this.StartDead();
		}
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x000FE60C File Offset: 0x000FC80C
	public override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		Vector3 position = base.transform.position;
		if (this.motor.GravityReversed)
		{
			position.y += (this.center.y - base.transform.position.y) * 2f;
		}
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
		this.weaponManager.OnDeath();
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x000FE6E4 File Offset: 0x000FC8E4
	public override void OnLeave(PlayerId playerId)
	{
		if (!base.IsDead)
		{
			Vector3 position = base.transform.position;
			if (this.motor.GravityReversed)
			{
				position.y += (this.center.y - base.transform.position.y) * 2f;
			}
			this.deathEffect.CreateExplosionOnly(playerId, position, PlayerMode.Level);
		}
		base.OnLeave(playerId);
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x000FE768 File Offset: 0x000FC968
	public void StartDead()
	{
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		position.y += 1000f;
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x0002C96E File Offset: 0x0002AB6E
	public void DisableInput()
	{
		this.motor.DisableInput();
		this.weaponManager.DisableInput();
		AudioManager.Stop("player_default_fire_loop");
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x0002C990 File Offset: 0x0002AB90
	public void EnableInput()
	{
		this.motor.EnableInput();
		this.weaponManager.EnableInput();
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x0002C9A8 File Offset: 0x0002ABA8
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x0002C9BB File Offset: 0x0002ABBB
	public void OnLevelWinPause()
	{
		this.PauseAll();
		base.collider.enabled = false;
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x000FE7F4 File Offset: 0x000FC9F4
	public override void OnLevelWin()
	{
		this.UnpauseAll(false);
		this.weaponManager.DisableInput();
		base.collider.enabled = false;
		AudioManager.Stop("player_default_fire_loop");
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			this.motor.OnPlatformingLevelExit();
		}
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x0002C9CF File Offset: 0x0002ABCF
	public void ReverseControls(float reverseTime)
	{
		base.stats.ReverseControls(reverseTime);
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x0002C9DD File Offset: 0x0002ABDD
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.DrawCube(this.CameraCenter, Vector3.one * 50f);
		}
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x0002CA09 File Offset: 0x0002AC09
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.deathEffect = null;
	}

	// Token: 0x04002C15 RID: 11285
	public const float PLATFORMING_CAMERA_DISTANCE_RUNNING = 250f;

	// Token: 0x04002C16 RID: 11286
	public const float PLATFORMING_CAMERA_DISTANCE_STATIC = 50f;

	// Token: 0x04002C17 RID: 11287
	public const float PLATFORMING_CAMERA_TIME_RUNNING = 1.2f;

	// Token: 0x04002C18 RID: 11288
	public const float PLATFORMING_CAMERA_TIME_STATIC = 6f;

	// Token: 0x04002C19 RID: 11289
	public bool initialized;

	// Token: 0x04002C1A RID: 11290
	public LevelPlayerMotor _motor;

	// Token: 0x04002C1B RID: 11291
	public LevelPlayerAnimationController _animationController;

	// Token: 0x04002C1C RID: 11292
	public LevelPlayerWeaponManager _weaponManager;

	// Token: 0x04002C1D RID: 11293
	public LevelPlayerParryController _parryController;

	// Token: 0x04002C1E RID: 11294
	public LevelPlayerColliderManager _colliderManager;

	// Token: 0x04002C1F RID: 11295
	[SerializeField]
	public PlayerDeathEffect deathEffect;

	// Token: 0x04002C20 RID: 11296
	public float cameraCenterPosition;
}
