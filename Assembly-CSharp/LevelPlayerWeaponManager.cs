using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000518 RID: 1304
public class LevelPlayerWeaponManager : AbstractLevelPlayerComponent
{
	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06003713 RID: 14099 RVA: 0x0002D136 File Offset: 0x0002B336
	// (set) Token: 0x06003714 RID: 14100 RVA: 0x0002D13E File Offset: 0x0002B33E
	public bool IsShooting { get; set; }

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x0002D147 File Offset: 0x0002B347
	// (set) Token: 0x06003716 RID: 14102 RVA: 0x0002D14F File Offset: 0x0002B34F
	public bool FreezePosition { get; set; }

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06003717 RID: 14103 RVA: 0x0002D158 File Offset: 0x0002B358
	public Vector2 ExPosition
	{
		get
		{
			return this.exRoot.position;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06003718 RID: 14104 RVA: 0x0002D16A File Offset: 0x0002B36A
	public AbstractLevelWeapon CurrentWeapon
	{
		get
		{
			return this.weaponPrefabs.GetWeapon(this.currentWeapon);
		}
	}

	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06003719 RID: 14105 RVA: 0x00101DB8 File Offset: 0x000FFFB8
	// (remove) Token: 0x0600371A RID: 14106 RVA: 0x00101DF0 File Offset: 0x000FFFF0
	public event LevelPlayerWeaponManager.OnWeaponChangeHandler OnWeaponChangeEvent;

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x0600371B RID: 14107 RVA: 0x00101E28 File Offset: 0x00100028
	// (remove) Token: 0x0600371C RID: 14108 RVA: 0x00101E60 File Offset: 0x00100060
	public event Action OnBasicStart;

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x0600371D RID: 14109 RVA: 0x00101E98 File Offset: 0x00100098
	// (remove) Token: 0x0600371E RID: 14110 RVA: 0x00101ED0 File Offset: 0x001000D0
	public event Action OnExStart;

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x0600371F RID: 14111 RVA: 0x00101F08 File Offset: 0x00100108
	// (remove) Token: 0x06003720 RID: 14112 RVA: 0x00101F40 File Offset: 0x00100140
	public event Action OnSuperStart;

	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06003721 RID: 14113 RVA: 0x00101F78 File Offset: 0x00100178
	// (remove) Token: 0x06003722 RID: 14114 RVA: 0x00101FB0 File Offset: 0x001001B0
	public event Action OnExFire;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06003723 RID: 14115 RVA: 0x00101FE8 File Offset: 0x001001E8
	// (remove) Token: 0x06003724 RID: 14116 RVA: 0x00102020 File Offset: 0x00100220
	public event Action OnWeaponFire;

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06003725 RID: 14117 RVA: 0x00102058 File Offset: 0x00100258
	// (remove) Token: 0x06003726 RID: 14118 RVA: 0x00102090 File Offset: 0x00100290
	public event Action OnExEnd;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06003727 RID: 14119 RVA: 0x001020C8 File Offset: 0x001002C8
	// (remove) Token: 0x06003728 RID: 14120 RVA: 0x00102100 File Offset: 0x00100300
	public event Action OnSuperEnd;

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06003729 RID: 14121 RVA: 0x00102138 File Offset: 0x00100338
	// (remove) Token: 0x0600372A RID: 14122 RVA: 0x00102170 File Offset: 0x00100370
	public event Action OnSuperInterrupt;

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x0600372B RID: 14123 RVA: 0x0002D17D File Offset: 0x0002B37D
	// (set) Token: 0x0600372C RID: 14124 RVA: 0x0002D185 File Offset: 0x0002B385
	public AbstractPlayerSuper activeSuper { get; set; }

	// Token: 0x0600372D RID: 14125 RVA: 0x001021A8 File Offset: 0x001003A8
	public override void OnAwake()
	{
		base.OnAwake();
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnLevelEndEvent += this.OnLevelEnd;
		base.player.motor.OnDashStartEvent += this.OnDash;
		this.basic = new LevelPlayerWeaponManager.WeaponState();
		this.ex = new LevelPlayerWeaponManager.ExState();
		this.weaponsRoot = new GameObject("Weapons").transform;
		this.weaponsRoot.parent = base.transform;
		this.weaponsRoot.localPosition = Vector3.zero;
		this.weaponsRoot.localEulerAngles = Vector3.zero;
		this.weaponsRoot.localScale = Vector3.one;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x001022A8 File Offset: 0x001004A8
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnLevelEndEvent -= this.OnLevelEnd;
		}
		if (base.player != null && base.player.motor != null)
		{
			base.player.motor.OnDashStartEvent -= this.OnDash;
		}
		this.weaponPrefabs.OnDestroy();
		this.superPrefabs.OnDestroy();
		this.exDustEffect = null;
		this.exChargeEffect = null;
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x00102350 File Offset: 0x00100550
	public void FixedUpdate()
	{
		if (!base.player.levelStarted || !this.allowInput)
		{
			return;
		}
		this.HandleWeaponSwitch();
		this.HandleWeaponFiring();
		if (base.player.motor.Grounded)
		{
			this.ex.airAble = true;
		}
	}

	// Token: 0x06003730 RID: 14128 RVA: 0x0002D18E File Offset: 0x0002B38E
	public void OnEnable()
	{
		this.EnableInput();
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x0002D196 File Offset: 0x0002B396
	public void ForceStopWeaponFiring()
	{
		this.EndBasic();
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x0002D19E File Offset: 0x0002B39E
	public override void OnLevelEnd()
	{
		this.EndBasic();
		base.OnLevelEnd();
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x0002D1AC File Offset: 0x0002B3AC
	public void OnDash()
	{
		this.EndBasic();
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.ex.firing && !base.player.stats.SuperInvincible)
		{
			this.ex.firing = false;
		}
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x0002D1E7 File Offset: 0x0002B3E7
	public void AbortEX()
	{
		this.ex.firing = false;
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x0002D1F5 File Offset: 0x0002B3F5
	public void ParrySuccess()
	{
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x001023A8 File Offset: 0x001005A8
	public void LevelInit(PlayerId id)
	{
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id);
		if (playerLoadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1)
		{
			int[] availableWeaponIDs = WeaponProperties.CharmCurse.availableWeaponIDs;
			this.currentWeapon = (Weapon)availableWeaponIDs[Random.Range(0, availableWeaponIDs.Length)];
		}
		else
		{
			this.currentWeapon = playerLoadout.primaryWeapon;
		}
		this.weaponPrefabs.Init(this, this.weaponsRoot);
		this.superPrefabs.Init(base.player);
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x0002D1F7 File Offset: 0x0002B3F7
	public void OnDeath()
	{
		this.EndBasic();
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x0002D1FF File Offset: 0x0002B3FF
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x0002D208 File Offset: 0x0002B408
	public void DisableInput()
	{
		this.allowInput = false;
		this.IsShooting = false;
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x0002D218 File Offset: 0x0002B418
	public void EnableSuper(bool value)
	{
		this.allowSuper = value;
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x0002D221 File Offset: 0x0002B421
	public void _WeaponFireEx()
	{
		this.FireEx();
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x0002D229 File Offset: 0x0002B429
	public void _WeaponEndEx()
	{
		this.EndEx();
	}

	// Token: 0x0600373E RID: 14142 RVA: 0x0002D231 File Offset: 0x0002B431
	public void StartBasic()
	{
		this.UpdateAim();
		if (!Level.IsChessBoss)
		{
			this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginBasic();
		}
		if (this.OnBasicStart != null)
		{
			this.OnBasicStart();
		}
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x0002D26F File Offset: 0x0002B46F
	public void EndBasic()
	{
		if (this.currentWeapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.basic.firing = false;
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x0002D2A4 File Offset: 0x0002B4A4
	public void TriggerWeaponFire()
	{
		this.OnWeaponFire();
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x0002D2B1 File Offset: 0x0002B4B1
	public void InterruptSuper()
	{
		if (this.OnSuperInterrupt != null)
		{
			this.OnSuperInterrupt();
		}
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x00102444 File Offset: 0x00100644
	public void StartEx()
	{
		this.EndBasic();
		this.UpdateAim();
		this.ex.firing = true;
		this.ex.airAble = false;
		base.player.stats.OnEx();
		this.exChargeEffect.Create(base.player.center);
		if (this.OnExStart != null)
		{
			this.OnExStart();
		}
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x0002D2C9 File Offset: 0x0002B4C9
	public void FireEx()
	{
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFire != null)
		{
			this.OnExFire();
		}
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x0002D2F7 File Offset: 0x0002B4F7
	public void EndEx()
	{
		this.ex.firing = false;
		if (this.OnExEnd != null)
		{
			this.OnExEnd();
		}
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x001024B4 File Offset: 0x001006B4
	public void CreateExDust(Effect starsEffect)
	{
		Transform transform = new GameObject("ExRootTemp").transform;
		transform.ResetLocalTransforms();
		transform.position = this.exRoot.position;
		Vector2 vector = transform.position;
		if (starsEffect != null)
		{
			Transform transform2 = starsEffect.Create(vector).transform;
			transform2.SetParent(transform);
			transform2.ResetLocalTransforms();
			transform2.SetParent(null);
			transform2.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform2.localScale = this.GetBulletScale();
			transform2.AddPositionForward2D(-100f);
		}
		if (this.exDustEffect != null)
		{
			Transform transform3 = this.exDustEffect.Create(vector).transform;
			transform3.SetParent(transform);
			transform3.ResetLocalTransforms();
			transform3.SetParent(null);
			transform3.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform3.localScale = this.GetBulletScale();
			transform3.AddPositionForward2D(-15f);
		}
		Object.Destroy(transform.gameObject);
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x001025E8 File Offset: 0x001007E8
	public void StartSuper()
	{
		this.EndBasic();
		this.UpdateAim();
		base.player.stats.OnSuper();
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		if (base.player.stats.isChalice)
		{
			if (super != Super.level_super_beam)
			{
				if (super != Super.level_super_ghost)
				{
					if (super == Super.level_super_invincible)
					{
						super = Super.level_super_chalice_shield;
					}
				}
				else
				{
					super = Super.level_super_chalice_iii;
				}
			}
			else
			{
				super = Super.level_super_chalice_vert_beam;
			}
		}
		AbstractPlayerSuper abstractPlayerSuper = this.superPrefabs.GetPrefab(super).Create(base.player);
		abstractPlayerSuper.OnEndedEvent += this.EndSuperFromSuper;
		this.activeSuper = abstractPlayerSuper;
		if (this.OnSuperStart != null)
		{
			this.OnSuperStart();
		}
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0002D31B File Offset: 0x0002B51B
	public void EndSuper()
	{
		if (this.OnSuperEnd != null)
		{
			this.OnSuperEnd();
		}
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0002D333 File Offset: 0x0002B533
	public void EndSuperFromSuper()
	{
		this.EndSuper();
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x001026D4 File Offset: 0x001008D4
	public void HandleWeaponFiring()
	{
		if (base.player.motor.Dashing || base.player.motor.IsHit)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4) || base.player.motor.HasBufferedInput(LevelPlayerMotor.BufferedInput.Super) || (base.player.stats.Loadout.charm == Charm.charm_EX && base.player.input.actions.GetButton(3) && !this.ex.firing))
		{
			base.player.motor.ClearBufferedInput();
			Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax && super != Super.None && !base.player.stats.ChaliceShieldOn && this.allowSuper && base.player.stats.Loadout.charm != Charm.charm_EX)
			{
				this.StartSuper();
				return;
			}
			if (base.player.stats.CanUseEx && this.ex.Able)
			{
				this.StartEx();
				return;
			}
		}
		if (this.ex.firing || base.player.stats.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		if (this.basic.firing != base.player.input.actions.GetButton(3))
		{
			if (base.player.input.actions.GetButton(3))
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1)
				{
					int[] availableWeaponIDs = WeaponProperties.CharmCurse.availableWeaponIDs;
					int num;
					for (num = (int)this.currentWeapon; num == (int)this.currentWeapon; num = availableWeaponIDs[Random.Range(0, availableWeaponIDs.Length)])
					{
					}
					this.SwitchWeapon((Weapon)num);
				}
				else
				{
					this.StartBasic();
				}
			}
			else
			{
				this.EndBasic();
			}
		}
		this.basic.firing = base.player.input.actions.GetButton(3);
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x0002D33B File Offset: 0x0002B53B
	public void ResetEx()
	{
		this.ex.firing = false;
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x00102974 File Offset: 0x00100B74
	public void HandleWeaponSwitch()
	{
		if (base.player.input.actions.GetButtonDown(5))
		{
			PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id);
			if ((playerLoadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1) || playerLoadout.secondaryWeapon == Weapon.None)
			{
				return;
			}
			if (this.currentWeapon == playerLoadout.primaryWeapon)
			{
				this.SwitchWeapon(playerLoadout.secondaryWeapon);
			}
			else
			{
				this.SwitchWeapon(playerLoadout.primaryWeapon);
			}
		}
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x00102A1C File Offset: 0x00100C1C
	public void SwitchWeapon(Weapon weapon)
	{
		if (weapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndEx();
		this.currentWeapon = weapon;
		if (this.OnWeaponChangeEvent != null)
		{
			this.OnWeaponChangeEvent(weapon);
		}
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x00102AA0 File Offset: 0x00100CA0
	public LevelPlayerWeaponManager.Pose GetCurrentPose()
	{
		if (this.ex.firing)
		{
			return LevelPlayerWeaponManager.Pose.Ex;
		}
		if (base.player.motor.Ducking)
		{
			return LevelPlayerWeaponManager.Pose.Duck;
		}
		if (!base.player.motor.Grounded)
		{
			return LevelPlayerWeaponManager.Pose.Jump;
		}
		if (base.player.motor.Locked)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return LevelPlayerWeaponManager.Pose.Up_D;
				}
				return LevelPlayerWeaponManager.Pose.Up;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return LevelPlayerWeaponManager.Pose.Down_D;
				}
				return LevelPlayerWeaponManager.Pose.Down;
			}
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				return LevelPlayerWeaponManager.Pose.Up_D_R;
			}
			return LevelPlayerWeaponManager.Pose.Forward_R;
		}
		else
		{
			if (base.player.motor.LookDirection.y < 0)
			{
				return LevelPlayerWeaponManager.Pose.Duck;
			}
			if (base.player.motor.LookDirection.y > 0)
			{
				return LevelPlayerWeaponManager.Pose.Up;
			}
		}
		return LevelPlayerWeaponManager.Pose.Forward;
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00102C34 File Offset: 0x00100E34
	public LevelPlayerWeaponManager.Pose GetDirectionPose()
	{
		if (base.player.motor.Dashing)
		{
			return LevelPlayerWeaponManager.Pose.Forward;
		}
		if (base.player.motor.LookDirection.y > 0)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				return LevelPlayerWeaponManager.Pose.Up_D;
			}
			return LevelPlayerWeaponManager.Pose.Up;
		}
		else
		{
			if (base.player.motor.LookDirection.y >= 0)
			{
				return LevelPlayerWeaponManager.Pose.Forward;
			}
			if (base.player.motor.LookDirection.x != 0)
			{
				return LevelPlayerWeaponManager.Pose.Down_D;
			}
			return LevelPlayerWeaponManager.Pose.Down;
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x00102CEC File Offset: 0x00100EEC
	public void UpdateAim()
	{
		LevelPlayerWeaponManager.Pose directionPose = this.GetDirectionPose();
		float num;
		if (base.transform.localScale.x > 0f)
		{
			switch (directionPose)
			{
			default:
				num = 0f;
				break;
			case LevelPlayerWeaponManager.Pose.Up:
				num = 90f;
				break;
			case LevelPlayerWeaponManager.Pose.Up_D:
				num = 45f;
				break;
			case LevelPlayerWeaponManager.Pose.Down:
				num = -90f;
				break;
			case LevelPlayerWeaponManager.Pose.Down_D:
				num = -45f;
				break;
			}
		}
		else
		{
			switch (directionPose)
			{
			default:
				num = 180f;
				break;
			case LevelPlayerWeaponManager.Pose.Up:
				num = 90f;
				break;
			case LevelPlayerWeaponManager.Pose.Up_D:
				num = 135f;
				break;
			case LevelPlayerWeaponManager.Pose.Down:
				num = -90f;
				break;
			case LevelPlayerWeaponManager.Pose.Down_D:
				num = -135f;
				break;
			}
		}
		num *= base.player.motor.GravityReversalMultiplier;
		this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x00102E1C File Offset: 0x0010101C
	public Vector2 GetBulletPosition()
	{
		Vector2 vector = base.transform.position;
		Vector2 vector2 = LevelPlayerWeaponManager.ProjectilePosition.Get(this.GetCurrentPose(), this.GetDirectionPose(), base.player.stats.isChalice);
		return new Vector2(vector.x + vector2.x * base.player.motor.TrueLookDirection.x, vector.y + vector2.y * base.player.motor.GravityReversalMultiplier);
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00102EB0 File Offset: 0x001010B0
	public float GetBulletRotation()
	{
		LevelPlayerWeaponManager.Pose pose = this.GetCurrentPose();
		if (pose != LevelPlayerWeaponManager.Pose.Duck)
		{
			return this.aim.eulerAngles.z;
		}
		if (base.transform.localScale.x < 0f)
		{
			return 180f;
		}
		return 0f;
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x00102F08 File Offset: 0x00101108
	public Vector3 GetBulletScale()
	{
		return new Vector3(1f, base.player.motor.TrueLookDirection.x, 1f);
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x00102F44 File Offset: 0x00101144
	public void WORKAROUND_NullifyFields()
	{
		this.activeSuper = null;
		this.weaponPrefabs = null;
		this.superPrefabs = null;
		this.exDustEffect = null;
		this.exChargeEffect = null;
		this.exRoot = null;
		this.OnWeaponChangeEvent = null;
		this.OnBasicStart = null;
		this.OnExStart = null;
		this.OnSuperStart = null;
		this.OnExFire = null;
		this.OnWeaponFire = null;
		this.OnExEnd = null;
		this.OnSuperEnd = null;
		this.OnSuperInterrupt = null;
		this.basic = null;
		this.ex = null;
		this.weaponsRoot = null;
		this.aim = null;
	}

	// Token: 0x04002C68 RID: 11368
	[SerializeField]
	public LevelPlayerWeaponManager.WeaponPrefabs weaponPrefabs;

	// Token: 0x04002C69 RID: 11369
	[SerializeField]
	public LevelPlayerWeaponManager.SuperPrefabs superPrefabs;

	// Token: 0x04002C6A RID: 11370
	[Space(10f)]
	[SerializeField]
	public Effect exDustEffect;

	// Token: 0x04002C6B RID: 11371
	[SerializeField]
	public Effect exChargeEffect;

	// Token: 0x04002C6C RID: 11372
	[SerializeField]
	public Transform exRoot;

	// Token: 0x04002C6F RID: 11375
	public Weapon currentWeapon = Weapon.None;

	// Token: 0x04002C70 RID: 11376
	public LevelPlayerWeaponManager.Pose currentPose;

	// Token: 0x04002C7A RID: 11386
	public LevelPlayerWeaponManager.WeaponState basic;

	// Token: 0x04002C7B RID: 11387
	public LevelPlayerWeaponManager.ExState ex;

	// Token: 0x04002C7C RID: 11388
	public Transform weaponsRoot;

	// Token: 0x04002C7D RID: 11389
	public Transform aim;

	// Token: 0x04002C7E RID: 11390
	public bool allowInput = true;

	// Token: 0x04002C7F RID: 11391
	public bool allowSuper = true;

	// Token: 0x020011A0 RID: 4512
	public enum Pose
	{
		// Token: 0x04007B75 RID: 31605
		Forward,
		// Token: 0x04007B76 RID: 31606
		Forward_R,
		// Token: 0x04007B77 RID: 31607
		Up,
		// Token: 0x04007B78 RID: 31608
		Up_D,
		// Token: 0x04007B79 RID: 31609
		Up_D_R,
		// Token: 0x04007B7A RID: 31610
		Down,
		// Token: 0x04007B7B RID: 31611
		Down_D,
		// Token: 0x04007B7C RID: 31612
		Duck,
		// Token: 0x04007B7D RID: 31613
		Jump,
		// Token: 0x04007B7E RID: 31614
		Ex
	}

	// Token: 0x020011A1 RID: 4513
	// (Invoke) Token: 0x06007E63 RID: 32355
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x020011A2 RID: 4514
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ProjectilePosition
	{
		// Token: 0x06007E66 RID: 32358 RVA: 0x0028EE1C File Offset: 0x0028D01C
		public static Vector2 Get(LevelPlayerWeaponManager.Pose pose, LevelPlayerWeaponManager.Pose direction, bool isChalice)
		{
			if (pose == LevelPlayerWeaponManager.Pose.Jump)
			{
				switch (direction)
				{
				case LevelPlayerWeaponManager.Pose.Forward:
					return (!isChalice) ? new Vector2(78f, 64f) : new Vector2(85f, 70f);
				case LevelPlayerWeaponManager.Pose.Up:
					return (!isChalice) ? new Vector2(0f, 158f) : new Vector2(22f, 162f);
				case LevelPlayerWeaponManager.Pose.Up_D:
					return (!isChalice) ? new Vector2(71f, 107f) : new Vector2(66f, 117f);
				case LevelPlayerWeaponManager.Pose.Down:
					return (!isChalice) ? new Vector2(0f, -11f) : new Vector2(22f, 2f);
				case LevelPlayerWeaponManager.Pose.Down_D:
					return (!isChalice) ? new Vector2(71f, 20f) : new Vector2(66f, 31f);
				}
				return (!isChalice) ? new Vector2(0f, 0f) : new Vector2(0f, 0f);
			}
			switch (pose)
			{
			case LevelPlayerWeaponManager.Pose.Forward:
				return (!isChalice) ? new Vector2(78f, 64f) : new Vector2(100f, 63f);
			case LevelPlayerWeaponManager.Pose.Forward_R:
				return (!isChalice) ? new Vector2(70f, 46f) : new Vector2(62f, 51f);
			case LevelPlayerWeaponManager.Pose.Up:
				return (!isChalice) ? new Vector2(27f, 158f) : new Vector2(32f, 162f);
			case LevelPlayerWeaponManager.Pose.Up_D:
				return (!isChalice) ? new Vector2(71f, 107f) : new Vector2(78f, 112f);
			case LevelPlayerWeaponManager.Pose.Up_D_R:
				return (!isChalice) ? new Vector2(73f, 107f) : new Vector2(66f, 108f);
			case LevelPlayerWeaponManager.Pose.Down:
				return (!isChalice) ? new Vector2(28f, -11f) : new Vector2(32f, -6f);
			case LevelPlayerWeaponManager.Pose.Down_D:
				return (!isChalice) ? new Vector2(71f, 20f) : new Vector2(78f, 17f);
			case LevelPlayerWeaponManager.Pose.Duck:
				return (!isChalice) ? new Vector2(102f, 24f) : new Vector2(103f, 33f);
			default:
				return (!isChalice) ? new Vector2(0f, 54f) : new Vector2(0f, 54f);
			}
		}
	}

	// Token: 0x020011A3 RID: 4515
	public class WeaponState
	{
		// Token: 0x04007B7F RID: 31615
		public LevelPlayerWeaponManager.WeaponState.State state;

		// Token: 0x04007B80 RID: 31616
		public bool firing;

		// Token: 0x04007B81 RID: 31617
		public bool holding;

		// Token: 0x020015F2 RID: 5618
		public enum State
		{
			// Token: 0x04009229 RID: 37417
			Ready,
			// Token: 0x0400922A RID: 37418
			Firing,
			// Token: 0x0400922B RID: 37419
			Fired,
			// Token: 0x0400922C RID: 37420
			Ended
		}
	}

	// Token: 0x020011A4 RID: 4516
	public class ExState
	{
		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06007E69 RID: 32361 RVA: 0x00054DD5 File Offset: 0x00052FD5
		public bool Able
		{
			get
			{
				return this.airAble && !this.firing;
			}
		}

		// Token: 0x04007B82 RID: 31618
		public bool airAble = true;

		// Token: 0x04007B83 RID: 31619
		public bool firing;
	}

	// Token: 0x020011A5 RID: 4517
	[Serializable]
	public class WeaponPrefabs
	{
		// Token: 0x06007E6B RID: 32363 RVA: 0x0028F0F8 File Offset: 0x0028D2F8
		public void Init(LevelPlayerWeaponManager weaponManager, Transform root)
		{
			this.weaponManager = weaponManager;
			this.root = root;
			this.weapons = new Dictionary<Weapon, AbstractLevelWeapon>();
			foreach (Weapon id in EnumUtils.GetValues<Weapon>())
			{
				if (id.ToString().ToLower().Contains("level"))
				{
					this.InitWeapon(id);
				}
			}
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x00054DF6 File Offset: 0x00052FF6
		public AbstractLevelWeapon GetWeapon(Weapon weapon)
		{
			return this.weapons[weapon];
		}

		// Token: 0x06007E6D RID: 32365 RVA: 0x0028F16C File Offset: 0x0028D36C
		public void InitWeapon(Weapon id)
		{
			AbstractLevelWeapon abstractLevelWeapon;
			if (id != Weapon.level_weapon_peashot)
			{
				if (id != Weapon.level_weapon_spreadshot)
				{
					if (id != Weapon.level_weapon_arc)
					{
						if (id != Weapon.level_weapon_homing)
						{
							if (id != Weapon.level_weapon_exploder)
							{
								if (id != Weapon.level_weapon_charge)
								{
									if (id != Weapon.level_weapon_boomerang)
									{
										if (id != Weapon.level_weapon_bouncer)
										{
											if (id != Weapon.level_weapon_wide_shot)
											{
												if (id != Weapon.level_weapon_upshot)
												{
													if (id != Weapon.level_weapon_crackshot)
													{
														if (id != Weapon.None)
														{
															return;
														}
														return;
													}
													else
													{
														abstractLevelWeapon = this.crackshot;
													}
												}
												else
												{
													abstractLevelWeapon = this.upShot;
												}
											}
											else
											{
												abstractLevelWeapon = this.wideShot;
											}
										}
										else
										{
											abstractLevelWeapon = this.bouncer;
										}
									}
									else
									{
										abstractLevelWeapon = this.boomerang;
									}
								}
								else
								{
									abstractLevelWeapon = this.charge;
								}
							}
							else
							{
								abstractLevelWeapon = this.exploder;
							}
						}
						else
						{
							abstractLevelWeapon = this.homing;
						}
					}
					else
					{
						abstractLevelWeapon = this.arc;
					}
				}
				else
				{
					abstractLevelWeapon = this.spread;
				}
			}
			else
			{
				abstractLevelWeapon = this.peashot;
			}
			if (abstractLevelWeapon == null)
			{
				return;
			}
			AbstractLevelWeapon abstractLevelWeapon2 = Object.Instantiate<AbstractLevelWeapon>(abstractLevelWeapon);
			abstractLevelWeapon2.transform.parent = this.root.transform;
			abstractLevelWeapon2.Initialize(this.weaponManager, id);
			abstractLevelWeapon2.name = abstractLevelWeapon2.name.Replace("(Clone)", string.Empty);
			this.weapons[id] = abstractLevelWeapon2;
		}

		// Token: 0x06007E6E RID: 32366 RVA: 0x0028F2EC File Offset: 0x0028D4EC
		public void OnDestroy()
		{
			this.peashot = null;
			this.spread = null;
			this.arc = null;
			this.homing = null;
			this.exploder = null;
			this.charge = null;
			this.boomerang = null;
			this.bouncer = null;
			this.wideShot = null;
		}

		// Token: 0x04007B84 RID: 31620
		[SerializeField]
		public WeaponPeashot peashot;

		// Token: 0x04007B85 RID: 31621
		[SerializeField]
		public WeaponSpread spread;

		// Token: 0x04007B86 RID: 31622
		[SerializeField]
		public WeaponArc arc;

		// Token: 0x04007B87 RID: 31623
		[SerializeField]
		public WeaponHoming homing;

		// Token: 0x04007B88 RID: 31624
		[SerializeField]
		public WeaponExploder exploder;

		// Token: 0x04007B89 RID: 31625
		[SerializeField]
		public WeaponCharge charge;

		// Token: 0x04007B8A RID: 31626
		[SerializeField]
		public WeaponBoomerang boomerang;

		// Token: 0x04007B8B RID: 31627
		[SerializeField]
		public WeaponBouncer bouncer;

		// Token: 0x04007B8C RID: 31628
		[SerializeField]
		public WeaponWideShot wideShot;

		// Token: 0x04007B8D RID: 31629
		[SerializeField]
		public WeaponUpshot upShot;

		// Token: 0x04007B8E RID: 31630
		[SerializeField]
		public WeaponCrackshot crackshot;

		// Token: 0x04007B8F RID: 31631
		public Transform root;

		// Token: 0x04007B90 RID: 31632
		public LevelPlayerWeaponManager weaponManager;

		// Token: 0x04007B91 RID: 31633
		public Dictionary<Weapon, AbstractLevelWeapon> weapons;
	}

	// Token: 0x020011A6 RID: 4518
	[Serializable]
	public class SuperPrefabs
	{
		// Token: 0x06007E70 RID: 32368 RVA: 0x00054E0C File Offset: 0x0005300C
		public void Init(LevelPlayerController player)
		{
		}

		// Token: 0x06007E71 RID: 32369 RVA: 0x0028F338 File Offset: 0x0028D538
		public AbstractPlayerSuper GetPrefab(Super super)
		{
			if (super != Super.level_super_beam)
			{
				if (super == Super.level_super_ghost)
				{
					return this.ghost;
				}
				if (super == Super.level_super_invincible)
				{
					return this.invincible;
				}
				if (super == Super.level_super_chalice_iii)
				{
					return this.chaliceIII;
				}
				if (super == Super.level_super_chalice_vert_beam)
				{
					return this.chaliceVertBeam;
				}
				if (super == Super.level_super_chalice_shield)
				{
					return this.chaliceShield;
				}
			}
			return this.beam;
		}

		// Token: 0x06007E72 RID: 32370 RVA: 0x00054E0E File Offset: 0x0005300E
		public void OnDestroy()
		{
			this.beam = null;
			this.ghost = null;
			this.invincible = null;
		}

		// Token: 0x04007B92 RID: 31634
		[SerializeField]
		public AbstractPlayerSuper beam;

		// Token: 0x04007B93 RID: 31635
		[SerializeField]
		public AbstractPlayerSuper ghost;

		// Token: 0x04007B94 RID: 31636
		[SerializeField]
		public AbstractPlayerSuper invincible;

		// Token: 0x04007B95 RID: 31637
		[SerializeField]
		public AbstractPlayerSuper chaliceIII;

		// Token: 0x04007B96 RID: 31638
		[SerializeField]
		public AbstractPlayerSuper chaliceVertBeam;

		// Token: 0x04007B97 RID: 31639
		[SerializeField]
		public AbstractPlayerSuper chaliceShield;
	}
}
