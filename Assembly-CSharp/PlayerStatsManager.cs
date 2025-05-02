using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000581 RID: 1409
public class PlayerStatsManager : AbstractPlayerComponent
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06003B12 RID: 15122 RVA: 0x0003005D File Offset: 0x0002E25D
	// (set) Token: 0x06003B13 RID: 15123 RVA: 0x00030064 File Offset: 0x0002E264
	public static bool GlobalInvincibility { get; set; }

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06003B14 RID: 15124 RVA: 0x0003006C File Offset: 0x0002E26C
	// (set) Token: 0x06003B15 RID: 15125 RVA: 0x00030074 File Offset: 0x0002E274
	public int HealthMax { get; set; }

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06003B16 RID: 15126 RVA: 0x0003007D File Offset: 0x0002E27D
	// (set) Token: 0x06003B17 RID: 15127 RVA: 0x00030085 File Offset: 0x0002E285
	public int Health { get; set; }

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06003B18 RID: 15128 RVA: 0x0003008E File Offset: 0x0002E28E
	// (set) Token: 0x06003B19 RID: 15129 RVA: 0x00030096 File Offset: 0x0002E296
	public int HealerHP { get; set; }

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06003B1A RID: 15130 RVA: 0x0003009F File Offset: 0x0002E29F
	// (set) Token: 0x06003B1B RID: 15131 RVA: 0x000300A7 File Offset: 0x0002E2A7
	public int HealerHPReceived { get; set; }

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06003B1C RID: 15132 RVA: 0x000300B0 File Offset: 0x0002E2B0
	// (set) Token: 0x06003B1D RID: 15133 RVA: 0x000300B8 File Offset: 0x0002E2B8
	public int HealerHPCounter { get; set; }

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06003B1E RID: 15134 RVA: 0x000300C1 File Offset: 0x0002E2C1
	// (set) Token: 0x06003B1F RID: 15135 RVA: 0x000300C9 File Offset: 0x0002E2C9
	public int CurseCharmLevel
	{
		get
		{
			return this._curseCharmLevel;
		}
		set
		{
			this._curseCharmLevel = value;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06003B20 RID: 15136 RVA: 0x000300D2 File Offset: 0x0002E2D2
	public bool CurseSmokeDash
	{
		get
		{
			return this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && this.curseCharmDashCounter == 0;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00030101 File Offset: 0x0002E301
	public bool CurseWhetsone
	{
		get
		{
			return this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && this.curseCharmWhetstoneCounter == 0;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06003B22 RID: 15138 RVA: 0x00030130 File Offset: 0x0002E330
	// (set) Token: 0x06003B23 RID: 15139 RVA: 0x00030138 File Offset: 0x0002E338
	public float SuperMeterMax { get; set; }

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06003B24 RID: 15140 RVA: 0x00030141 File Offset: 0x0002E341
	// (set) Token: 0x06003B25 RID: 15141 RVA: 0x00030149 File Offset: 0x0002E349
	public float SuperMeter { get; set; }

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06003B26 RID: 15142 RVA: 0x00030152 File Offset: 0x0002E352
	// (set) Token: 0x06003B27 RID: 15143 RVA: 0x0003015A File Offset: 0x0002E35A
	public bool SuperInvincible { get; set; }

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06003B28 RID: 15144 RVA: 0x00030163 File Offset: 0x0002E363
	// (set) Token: 0x06003B29 RID: 15145 RVA: 0x0003016B File Offset: 0x0002E36B
	public bool ChaliceShieldOn { get; set; }

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06003B2A RID: 15146 RVA: 0x00030174 File Offset: 0x0002E374
	public bool CanGainSuperMeter
	{
		get
		{
			return this.Loadout.charm != Charm.charm_EX && (!this.SuperInvincible || this.ChaliceShieldOn);
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06003B2B RID: 15147 RVA: 0x000301A2 File Offset: 0x0002E3A2
	// (set) Token: 0x06003B2C RID: 15148 RVA: 0x000301AA File Offset: 0x0002E3AA
	public float ExCost { get; set; }

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06003B2D RID: 15149 RVA: 0x000301B3 File Offset: 0x0002E3B3
	// (set) Token: 0x06003B2E RID: 15150 RVA: 0x000301BB File Offset: 0x0002E3BB
	public int Deaths { get; set; }

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06003B2F RID: 15151 RVA: 0x000301C4 File Offset: 0x0002E3C4
	// (set) Token: 0x06003B30 RID: 15152 RVA: 0x000301CC File Offset: 0x0002E3CC
	public int ParriesThisJump { get; set; }

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06003B31 RID: 15153 RVA: 0x000301D5 File Offset: 0x0002E3D5
	// (set) Token: 0x06003B32 RID: 15154 RVA: 0x000301DD File Offset: 0x0002E3DD
	public float StoneTime { get; set; }

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06003B33 RID: 15155 RVA: 0x000301E6 File Offset: 0x0002E3E6
	// (set) Token: 0x06003B34 RID: 15156 RVA: 0x000301EE File Offset: 0x0002E3EE
	public float ReverseTime { get; set; }

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06003B35 RID: 15157 RVA: 0x000301F7 File Offset: 0x0002E3F7
	public bool CanUseEx
	{
		get
		{
			return this.Loadout.charm == Charm.charm_EX || (this.SuperMeter >= this.ExCost && this.CanGainSuperMeter);
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06003B36 RID: 15158 RVA: 0x0003022B File Offset: 0x0002E42B
	// (set) Token: 0x06003B37 RID: 15159 RVA: 0x00030233 File Offset: 0x0002E433
	public PlayerData.PlayerLoadouts.PlayerLoadout Loadout { get; set; }

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06003B38 RID: 15160 RVA: 0x0003023C File Offset: 0x0002E43C
	// (set) Token: 0x06003B39 RID: 15161 RVA: 0x00030244 File Offset: 0x0002E444
	public PlayerStatsManager.PlayerState State { get; set; }

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06003B3A RID: 15162 RVA: 0x0003024D File Offset: 0x0002E44D
	// (set) Token: 0x06003B3B RID: 15163 RVA: 0x00030255 File Offset: 0x0002E455
	public bool DiceGameBonusHP { get; set; }

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06003B3C RID: 15164 RVA: 0x001128F8 File Offset: 0x00110AF8
	// (remove) Token: 0x06003B3D RID: 15165 RVA: 0x00112930 File Offset: 0x00110B30
	public event PlayerStatsManager.OnPlayerHealthChangeHandler OnHealthChangedEvent;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x06003B3E RID: 15166 RVA: 0x00112968 File Offset: 0x00110B68
	// (remove) Token: 0x06003B3F RID: 15167 RVA: 0x001129A0 File Offset: 0x00110BA0
	public event PlayerStatsManager.OnPlayerSuperChangedHandler OnSuperChangedEvent;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x06003B40 RID: 15168 RVA: 0x001129D8 File Offset: 0x00110BD8
	// (remove) Token: 0x06003B41 RID: 15169 RVA: 0x00112A10 File Offset: 0x00110C10
	public event PlayerStatsManager.OnPlayerWeaponChangedHandler OnWeaponChangedEvent;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x06003B42 RID: 15170 RVA: 0x00112A48 File Offset: 0x00110C48
	// (remove) Token: 0x06003B43 RID: 15171 RVA: 0x00112A80 File Offset: 0x00110C80
	public event PlayerStatsManager.OnPlayerDeathHandler OnPlayerDeathEvent;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x06003B44 RID: 15172 RVA: 0x00112AB8 File Offset: 0x00110CB8
	// (remove) Token: 0x06003B45 RID: 15173 RVA: 0x00112AF0 File Offset: 0x00110CF0
	public event PlayerStatsManager.OnPlayerDeathHandler OnPlayerReviveEvent;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x06003B46 RID: 15174 RVA: 0x00112B28 File Offset: 0x00110D28
	// (remove) Token: 0x06003B47 RID: 15175 RVA: 0x00112B60 File Offset: 0x00110D60
	public event PlayerStatsManager.OnStoneHandler OnStoneShake;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x06003B48 RID: 15176 RVA: 0x00112B98 File Offset: 0x00110D98
	// (remove) Token: 0x06003B49 RID: 15177 RVA: 0x00112BD0 File Offset: 0x00110DD0
	public event PlayerStatsManager.OnStoneHandler OnStoned;

	// Token: 0x06003B4A RID: 15178 RVA: 0x00112C08 File Offset: 0x00110E08
	public override void OnAwake()
	{
		base.OnAwake();
		PlayerStatsManager.GlobalInvincibility = false;
		PlayerStatsManager.DebugInvincible = false;
		this.SuperInvincible = false;
		this.ChaliceShieldOn = false;
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		LevelPlayerController levelPlayerController = base.basePlayer as LevelPlayerController;
		PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
		if (levelPlayerController != null)
		{
			levelPlayerController.motor.OnDashStartEvent += this.onDashStartEventHandler;
			levelPlayerController.motor.OnParryEvent += this.onParryEventHandler;
		}
		else if (planePlayerController != null)
		{
			planePlayerController.animationController.OnShrinkEvent += this.onShrinkEventHandler;
			planePlayerController.parryController.OnParryStartEvent += this.onParryEventHandler;
		}
		LevelPlayerWeaponManager component = base.GetComponent<LevelPlayerWeaponManager>();
		if (component != null)
		{
			component.OnWeaponChangeEvent += this.OnWeaponChange;
			component.OnSuperEnd += this.OnSuperEnd;
		}
		PlanePlayerWeaponManager component2 = base.GetComponent<PlanePlayerWeaponManager>();
		if (component2 != null)
		{
			component2.OnWeaponChangeEvent += this.OnWeaponChange;
		}
		this.Deaths = 0;
		this.hardInvincibility = false;
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0003025E File Offset: 0x0002E45E
	public void OnEnable()
	{
		if (this.superBuilderRoutine != null)
		{
			base.StopCoroutine(this.superBuilderRoutine);
		}
		this.superBuilderRoutine = this.charmSuperBuilder_cr();
		base.StartCoroutine(this.superBuilderRoutine);
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x00030290 File Offset: 0x0002E490
	public void FixedUpdate()
	{
		this.UpdateStone();
		this.UpdateReverse();
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x00112D4C File Offset: 0x00110F4C
	public void LevelInit()
	{
		Level.Current.OnWinEvent += this.OnWin;
		Level.Current.OnLoseEvent += this.OnLose;
		this.Loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.basePlayer.id);
		this.isChalice = (this.Loadout.charm == Charm.charm_chalice && !Level.Current.BlockChaliceCharm[(int)base.basePlayer.id]);
		if (!Level.Current.blockChalice && (PlayerManager.playerWasChalice[0] || PlayerManager.playerWasChalice[1]))
		{
			this.isChalice = PlayerManager.playerWasChalice[(int)base.basePlayer.id];
		}
		if (Level.IsDicePalace && !DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY)
		{
			this.isChalice = (base.basePlayer.id == (PlayerId)DicePalaceMainLevelGameInfo.CHALICE_PLAYER);
		}
		if (this.Loadout.charm == Charm.charm_curse)
		{
			this.CurseCharmLevel = CharmCurse.CalculateLevel(base.basePlayer.id);
		}
		this.ExCost = 10f;
		this.SuperMeterMax = 50f;
		this.CalculateHealthMax();
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			this.Health = playerStats.HP;
			this.SuperMeter = playerStats.SuperCharge;
			this.HealerHP = playerStats.healerHP;
			this.HealerHPReceived = playerStats.healerHPReceived;
			this.HealerHPCounter = playerStats.healerHPCounter;
		}
		else
		{
			this.Health = this.HealthMax;
			this.SuperMeter = 0f;
		}
		if (this.Health >= OnlineAchievementData.DLC.Triggers.HP9Trigger)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, OnlineAchievementData.DLC.HP9);
		}
		this.UpdateHealerStats();
		if (this.isChalice)
		{
			if (this.Loadout.super == Super.level_super_beam)
			{
				this.Loadout.super = Super.level_super_chalice_vert_beam;
			}
			else if (this.Loadout.super == Super.level_super_invincible)
			{
				this.Loadout.super = Super.level_super_chalice_shield;
			}
			else if (this.Loadout.super == Super.level_super_ghost)
			{
				this.Loadout.super = Super.level_super_chalice_iii;
			}
		}
		else if (this.Loadout.super == Super.level_super_chalice_vert_beam)
		{
			this.Loadout.super = Super.level_super_beam;
		}
		else if (this.Loadout.super == Super.level_super_chalice_shield)
		{
			this.Loadout.super = Super.level_super_invincible;
		}
		else if (this.Loadout.super == Super.level_super_chalice_iii)
		{
			this.Loadout.super = Super.level_super_ghost;
		}
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x00113040 File Offset: 0x00111240
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnWinEvent -= this.OnWin;
			Level.Current.OnLoseEvent -= this.OnLose;
		}
		if (this.Loadout != null)
		{
			if (this.Loadout.super == Super.level_super_chalice_vert_beam)
			{
				this.Loadout.super = Super.level_super_beam;
			}
			else if (this.Loadout.super == Super.level_super_chalice_shield)
			{
				this.Loadout.super = Super.level_super_invincible;
			}
			else if (this.Loadout.super == Super.level_super_chalice_iii)
			{
				this.Loadout.super = Super.level_super_ghost;
			}
		}
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x00113114 File Offset: 0x00111314
	public void UpdateHealerStats()
	{
		if ((this.Loadout.charm == Charm.charm_healer || (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0)) && !Level.IsChessBoss)
		{
			PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
			if (playerStats != null)
			{
				this.HealthMax += playerStats.healerHP;
			}
		}
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0003029E File Offset: 0x0002E49E
	public bool DjimmiInUse()
	{
		return PlayerData.Data.DjimmiActivatedCurrentRegion() && Level.Current.AllowDjimmi() && Level.Current.mode != Level.Mode.Hard;
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x0011318C File Offset: 0x0011138C
	public void CalculateHealthMax()
	{
		this.HealthMax = 3;
		if (this.Loadout.charm == Charm.charm_health_up_1 && !Level.IsChessBoss)
		{
			this.HealthMax += WeaponProperties.CharmHealthUpOne.healthIncrease;
		}
		else if (this.Loadout.charm == Charm.charm_health_up_2 && !Level.IsChessBoss)
		{
			this.HealthMax += WeaponProperties.CharmHealthUpTwo.healthIncrease;
		}
		else if (this.Loadout.charm == Charm.charm_healer && !Level.IsChessBoss)
		{
			this.HealthMax += this.HealerHP;
		}
		else if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && !Level.IsChessBoss)
		{
			this.HealthMax += this.HealerHP;
			this.HealthMax += CharmCurse.GetHealthModifier(this.CurseCharmLevel);
		}
		else if (this.isChalice)
		{
			this.HealthMax++;
		}
		if (this.DjimmiInUse())
		{
			this.HealthMax *= 2;
		}
		if (Level.IsInBossesHub)
		{
			PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
			if (playerStats != null)
			{
				this.HealthMax += playerStats.BonusHP;
			}
		}
		if (this.HealthMax > 9)
		{
			this.HealthMax = 9;
		}
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x000302D1 File Offset: 0x0002E4D1
	public void OnWin()
	{
		PlayerStatsManager.GlobalInvincibility = true;
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x000302D9 File Offset: 0x0002E4D9
	public void OnLose()
	{
		PlayerStatsManager.GlobalInvincibility = true;
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0011331C File Offset: 0x0011151C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.SuperInvincible)
		{
			return;
		}
		if (this.ChaliceShieldOn)
		{
			return;
		}
		if (this.Loadout.charm == Charm.charm_pit_saver && !Level.IsChessBoss)
		{
			if (info.damageSource == DamageDealer.DamageSource.Pit)
			{
				return;
			}
			this.SuperMeter += WeaponProperties.CharmPitSaver.meterAmount;
			this.OnSuperChanged(true);
		}
		if (info.stoneTime > 0f)
		{
			this.GetStoned(info.stoneTime);
		}
		if (info.damage > 0f)
		{
			this.TakeDamage();
		}
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x001133B8 File Offset: 0x001115B8
	public void GetStoned(float time)
	{
		if (time > 0f && this.StoneTime <= 0f && this.timeSinceStoned > 1f)
		{
			this.StoneTime = time;
			this.timeSinceStoned = 0f;
			this.OnStoned();
		}
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x00113410 File Offset: 0x00111610
	public void ReverseControls(float reverseTime)
	{
		if (this.timeSinceReversed > 0f && this.ReverseTime <= 0f && this.timeSinceReversed > 1f)
		{
			this.ReverseTime = reverseTime;
			this.timeSinceReversed = 0f;
		}
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x00113460 File Offset: 0x00111660
	public void TakeDamage()
	{
		if (this.SuperInvincible)
		{
			return;
		}
		if (this.hardInvincibility)
		{
			return;
		}
		if (Level.Current.Ending)
		{
			return;
		}
		if (this.State != PlayerStatsManager.PlayerState.Ready && (!this.isChalice || this.Loadout.super != Super.level_super_ghost))
		{
			return;
		}
		if (this.StoneTime > 0f)
		{
			this.StoneTime = 0f;
		}
		if (PlayerStatsManager.GlobalInvincibility || PlayerStatsManager.DebugInvincible)
		{
			return;
		}
		this.Health--;
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			if (playerStats.BonusHP > 0)
			{
				playerStats.LoseBonusHP();
			}
			else if (playerStats.healerHP > 0)
			{
				playerStats.LoseHealerHP();
			}
			this.CalculateHealthMax();
		}
		this.OnHealthChanged();
		if (this.Health < 3)
		{
			Level.ScoringData.numTimesHit++;
		}
		Vibrator.Vibrate(1f, 0.2f, base.basePlayer.id);
		if (this.Health <= 0)
		{
			this.OnStatsDeath();
		}
		else
		{
			base.StartCoroutine(this.hit_cr());
		}
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x000302E1 File Offset: 0x0002E4E1
	public void OnPitKnockUp()
	{
		base.basePlayer.damageReceiver.TakeDamage(new DamageDealer.DamageInfo(1f, DamageDealer.Direction.Neutral, base.transform.position, DamageDealer.DamageSource.Pit));
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0003030F File Offset: 0x0002E50F
	public void OnDealDamage(float damage, DamageDealer dealer)
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 0.0625f * damage / dealer.DamageMultiplier;
			this.OnSuperChanged(false);
		}
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x001135B4 File Offset: 0x001117B4
	public void OnParry(float multiplier = 1f, bool countParryTowardsScore = true)
	{
		if ((this.Loadout.charm == Charm.charm_healer || (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0)) && !Level.IsChessBoss)
		{
			if (this.HealerHPReceived < 3)
			{
				this.HealerCharm();
			}
			else
			{
				this.SuperChangedFromParry(multiplier);
			}
		}
		else
		{
			this.SuperChangedFromParry(multiplier);
		}
		if (countParryTowardsScore && !Level.Current.Ending)
		{
			Level.ScoringData.numParries++;
		}
		OnlineManager.Instance.Interface.IncrementStat(base.basePlayer.id, "Parries", 1);
		if (Level.Current.CurrentLevel != Levels.Tutorial && Level.Current.CurrentLevel != Levels.ShmupTutorial && (Level.Current.playerMode == PlayerMode.Level || Level.Current.playerMode == PlayerMode.Arcade))
		{
			this.ParriesThisJump++;
			if (this.ParriesThisJump > PlayerData.Data.GetNumParriesInRow(base.basePlayer.id))
			{
				PlayerData.Data.SetNumParriesInRow(base.basePlayer.id, this.ParriesThisJump);
			}
			if (this.ParriesThisJump == 5)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, "ParryChain");
			}
		}
		if (this.SuperMeter == this.SuperMeterMax)
		{
			AudioManager.Play("player_parry_power_up_full");
		}
		else
		{
			AudioManager.Play("player_parry_power_up");
		}
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0003033E File Offset: 0x0002E53E
	public void SuperChangedFromParry(float multiplier)
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 10f * multiplier;
			this.OnSuperChanged(true);
		}
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x00030366 File Offset: 0x0002E566
	public bool NextParryActivatesHealerCharm()
	{
		return !Level.IsChessBoss && this.Loadout.charm == Charm.charm_healer && this.HealerHPReceived == this.HealerHPCounter;
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x00113754 File Offset: 0x00111954
	public void HealerCharm()
	{
		int num = this.HealerHPReceived + 1;
		if (this.Loadout.charm == Charm.charm_curse)
		{
			num = CharmCurse.GetHealerInterval(this.CurseCharmLevel, this.HealerHPReceived);
		}
		this.HealerHPCounter++;
		if (this.HealerHPCounter >= num)
		{
			this.HealerHP++;
			this.HealerHPReceived++;
			this.SetHealth(this.Health + 1);
			this.OnHealthChanged();
			this.HealerHPCounter = 0;
			LevelPlayerController levelPlayerController = base.basePlayer as LevelPlayerController;
			PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.animationController.OnHealerCharm();
			}
			else if (planePlayerController != null)
			{
				planePlayerController.animationController.OnHealerCharm();
			}
		}
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x00030399 File Offset: 0x0002E599
	public void ParryOneQuarter()
	{
		this.OnParry(0.25f, true);
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x000303A7 File Offset: 0x0002E5A7
	public void ResetJumpParries()
	{
		this.ParriesThisJump = 0;
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06003B60 RID: 15200 RVA: 0x000303B0 File Offset: 0x0002E5B0
	public bool PartnerCanSteal
	{
		get
		{
			return this.Health > 1;
		}
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x00113830 File Offset: 0x00111A30
	public void OnPartnerStealHealth()
	{
		if (!this.PartnerCanSteal)
		{
			return;
		}
		this.Health--;
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			playerStats.LoseBonusHP();
			this.CalculateHealthMax();
		}
		this.OnHealthChanged();
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x000303BB File Offset: 0x0002E5BB
	public void OnSuper()
	{
		if (this.Loadout.super != Super.level_super_invincible || Level.Current.playerMode != PlayerMode.Level)
		{
			this.SuperMeter = 0f;
			this.OnSuperChanged(true);
		}
		this.State = PlayerStatsManager.PlayerState.Super;
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x000303FA File Offset: 0x0002E5FA
	public void OnSuperEnd()
	{
		if (this.Loadout.super == Super.level_super_invincible && Level.Current.playerMode == PlayerMode.Level)
		{
			base.StartCoroutine(this.emptySuper_cr());
		}
		this.State = PlayerStatsManager.PlayerState.Ready;
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x00030434 File Offset: 0x0002E634
	public void OnEx()
	{
		if (this.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		this.SuperMeter -= 10f;
		this.OnSuperChanged(true);
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x00030465 File Offset: 0x0002E665
	public void OnWeaponChange(Weapon weapon)
	{
		if (this.OnWeaponChangedEvent != null)
		{
			this.OnWeaponChangedEvent(weapon);
		}
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x0011388C File Offset: 0x00111A8C
	public void OnHealthChanged()
	{
		this.Health = Mathf.Clamp(this.Health, 0, this.HealthMax);
		if (this.OnHealthChangedEvent != null)
		{
			this.OnHealthChangedEvent(this.Health, base.basePlayer.id);
		}
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x001138D8 File Offset: 0x00111AD8
	public void OnSuperChanged(bool playEffect = true)
	{
		this.SuperMeter = Mathf.Clamp(this.SuperMeter, 0f, this.SuperMeterMax);
		if (this.OnSuperChangedEvent != null)
		{
			this.OnSuperChangedEvent(this.SuperMeter, base.basePlayer.id, playEffect);
		}
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0011392C File Offset: 0x00111B2C
	public void OnStatsDeath()
	{
		AudioManager.Play("player_die");
		base.StartCoroutine(this.death_sound_cr());
		if (this.OnPlayerDeathEvent != null)
		{
			this.OnPlayerDeathEvent(base.basePlayer.id);
		}
		EventManager.Instance.Raise(PlayerEvent<PlayerStatsManager.DeathEvent>.Shared(base.basePlayer.id));
		this.Deaths++;
		PlayerData.Data.Die(base.basePlayer.id);
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x0003047E File Offset: 0x0002E67E
	public void OnPreRevive()
	{
		if (!Level.IsTowerOfPowerMain || TowerOfPowerLevelGameInfo.CURRENT_TURN > 0)
		{
			this.Health = 1;
		}
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x001139B0 File Offset: 0x00111BB0
	public void OnRevive()
	{
		this.OnHealthChanged();
		if (this.OnPlayerReviveEvent != null)
		{
			this.OnPlayerReviveEvent(base.basePlayer.id);
		}
		EventManager.Instance.Raise(PlayerEvent<PlayerStatsManager.ReviveEvent>.Shared(base.basePlayer.id));
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x0003049C File Offset: 0x0002E69C
	public void SetHealth(int health)
	{
		this.Health = health;
		this.CalculateHealthMax();
		this.OnHealthChanged();
		if (health >= OnlineAchievementData.DLC.Triggers.HP9Trigger)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, OnlineAchievementData.DLC.HP9);
		}
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x000304DB File Offset: 0x0002E6DB
	public void SetInvincible(bool superInvincible)
	{
		this.SuperInvincible = superInvincible;
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x000304E4 File Offset: 0x0002E6E4
	public void SetChaliceShield(bool chaliceShield)
	{
		this.ChaliceShieldOn = chaliceShield;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x000304ED File Offset: 0x0002E6ED
	public void AddEx()
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 10f;
			this.OnSuperChanged(true);
		}
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x00113A00 File Offset: 0x00111C00
	public void onDashStartEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmDashCounter++;
			if (this.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(this.CurseCharmLevel))
			{
				this.curseCharmDashCounter = 0;
			}
		}
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x00113A5C File Offset: 0x00111C5C
	public void onShrinkEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmDashCounter++;
			if (this.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(this.CurseCharmLevel))
			{
				this.curseCharmDashCounter = 0;
			}
		}
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x00113AB8 File Offset: 0x00111CB8
	public void onParryEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmWhetstoneCounter++;
			if (this.curseCharmWhetstoneCounter > CharmCurse.GetWhetstoneInterval(this.CurseCharmLevel))
			{
				this.curseCharmWhetstoneCounter = 0;
			}
		}
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x00113B14 File Offset: 0x00111D14
	public void UpdateStone()
	{
		PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
		if (planePlayerController != null)
		{
			this.currentMoveDir = planePlayerController.motor.MoveDirection;
		}
		this.lastMoveDir = this.currentMoveDir;
		this.timeSinceStoned += CupheadTime.FixedDelta;
		if (this.StoneTime <= 0f)
		{
			return;
		}
		if ((this.lastMoveDir != this.currentMoveDir && (this.currentMoveDir.x != 0 || this.currentMoveDir.y != 0)) || base.basePlayer.input.actions.GetAnyButtonDown())
		{
			this.StoneTime -= CupheadTime.Delta;
			this.StoneTime -= 0.1f;
			this.OnStoneShake();
		}
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x00113C08 File Offset: 0x00111E08
	public void UpdateReverse()
	{
		LevelPlayerController levelPlayerController = base.basePlayer as LevelPlayerController;
		if (levelPlayerController == null)
		{
			return;
		}
		this.timeSinceReversed += CupheadTime.FixedDelta;
		if (this.ReverseTime <= 0f)
		{
			return;
		}
		this.ReverseTime -= CupheadTime.Delta;
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x00030513 File Offset: 0x0002E713
	public override void StopAllCoroutines()
	{
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x00113C68 File Offset: 0x00111E68
	public IEnumerator death_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("player_die_vinylscratch");
		yield break;
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x00113C84 File Offset: 0x00111E84
	public IEnumerator hit_cr()
	{
		this.hardInvincibility = true;
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		this.hardInvincibility = false;
		yield break;
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x00113CA0 File Offset: 0x00111EA0
	public IEnumerator charmSuperBuilder_cr()
	{
		while (this.Loadout == null)
		{
			yield return null;
		}
		if ((this.Loadout.charm != Charm.charm_super_builder && (this.Loadout.charm != Charm.charm_curse || this.CurseCharmLevel <= -1)) || Level.IsChessBoss)
		{
			yield break;
		}
		float delay = 0f;
		float amount = 0f;
		if (this.Loadout.charm == Charm.charm_super_builder)
		{
			delay = WeaponProperties.CharmSuperBuilder.delay;
			amount = WeaponProperties.CharmSuperBuilder.amount;
		}
		else if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			delay = WeaponProperties.CharmCurse.superMeterDelay;
			amount = CharmCurse.GetSuperMeterAmount(this.CurseCharmLevel);
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (this.CanGainSuperMeter)
			{
				this.SuperMeter += amount;
				this.OnSuperChanged(false);
			}
		}
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x00113CBC File Offset: 0x00111EBC
	public IEnumerator emptySuper_cr()
	{
		while (this.SuperMeter > 0f)
		{
			this.SuperMeter -= this.SuperMeterMax * CupheadTime.Delta / WeaponProperties.LevelSuperInvincibility.durationFX;
			this.OnSuperChanged(true);
			yield return null;
		}
		this.SuperMeter = 0f;
		this.OnSuperChanged(true);
		yield break;
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06003B79 RID: 15225 RVA: 0x00030515 File Offset: 0x0002E715
	// (set) Token: 0x06003B7A RID: 15226 RVA: 0x0003051C File Offset: 0x0002E71C
	public static bool DebugInvincible { get; set; }

	// Token: 0x06003B7B RID: 15227 RVA: 0x00030524 File Offset: 0x0002E724
	public void DebugAddSuper()
	{
		this.AddEx();
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0003052C File Offset: 0x0002E72C
	public void DebugFillSuper()
	{
		this.SuperMeter = 50f;
		this.OnSuperChanged(true);
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00113CD8 File Offset: 0x00111ED8
	public static void DebugToggleInvincible()
	{
		PlayerStatsManager.DebugInvincible = !PlayerStatsManager.DebugInvincible;
		string text = (!PlayerStatsManager.DebugInvincible) ? "red" : "green";
	}

	// Token: 0x04002F47 RID: 12103
	public const int HEALTH_MAX = 3;

	// Token: 0x04002F48 RID: 12104
	public const int HEALTH_TRUE_MAX = 9;

	// Token: 0x04002F49 RID: 12105
	public const float TIME_HIT = 2f;

	// Token: 0x04002F4A RID: 12106
	public const float TIME_REVIVED = 3f;

	// Token: 0x04002F4B RID: 12107
	public const int SUPER_MAX = 50;

	// Token: 0x04002F4C RID: 12108
	public const float SUPER_ON_PARRY = 10f;

	// Token: 0x04002F4D RID: 12109
	public const float SUPER_ON_DEAL_DAMAGE = 0.0625f;

	// Token: 0x04002F4E RID: 12110
	public const float EX_COST = 10f;

	// Token: 0x04002F4F RID: 12111
	public const int HEALER_HP_MAX = 3;

	// Token: 0x04002F56 RID: 12118
	public bool isChalice;

	// Token: 0x04002F57 RID: 12119
	public int _curseCharmLevel;

	// Token: 0x04002F58 RID: 12120
	public int curseCharmDashCounter;

	// Token: 0x04002F59 RID: 12121
	public int curseCharmWhetstoneCounter;

	// Token: 0x04002F63 RID: 12131
	public float timeSinceStoned = 1000f;

	// Token: 0x04002F64 RID: 12132
	public float timeSinceReversed = 1000f;

	// Token: 0x04002F65 RID: 12133
	public bool hardInvincibility;

	// Token: 0x04002F70 RID: 12144
	public IEnumerator superBuilderRoutine;

	// Token: 0x04002F71 RID: 12145
	public const float STONE_REDUCTION = 0.1f;

	// Token: 0x04002F72 RID: 12146
	public Trilean2 lastMoveDir;

	// Token: 0x04002F73 RID: 12147
	public Trilean2 currentMoveDir;

	// Token: 0x020011FE RID: 4606
	public class DeathEvent : PlayerEvent<PlayerStatsManager.DeathEvent>
	{
	}

	// Token: 0x020011FF RID: 4607
	public class ReviveEvent : PlayerEvent<PlayerStatsManager.ReviveEvent>
	{
	}

	// Token: 0x02001200 RID: 4608
	public enum PlayerState
	{
		// Token: 0x04007D31 RID: 32049
		Ready,
		// Token: 0x04007D32 RID: 32050
		Super
	}

	// Token: 0x02001201 RID: 4609
	// (Invoke) Token: 0x06007FDC RID: 32732
	public delegate void OnPlayerHealthChangeHandler(int health, PlayerId playerId);

	// Token: 0x02001202 RID: 4610
	// (Invoke) Token: 0x06007FE0 RID: 32736
	public delegate void OnPlayerSuperChangedHandler(float super, PlayerId playerId, bool playEffect);

	// Token: 0x02001203 RID: 4611
	// (Invoke) Token: 0x06007FE4 RID: 32740
	public delegate void OnPlayerWeaponChangedHandler(Weapon weapon);

	// Token: 0x02001204 RID: 4612
	// (Invoke) Token: 0x06007FE8 RID: 32744
	public delegate void OnPlayerDeathHandler(PlayerId playerId);

	// Token: 0x02001205 RID: 4613
	// (Invoke) Token: 0x06007FEC RID: 32748
	public delegate void OnStoneHandler();
}
