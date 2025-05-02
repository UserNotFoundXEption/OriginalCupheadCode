using System;

// Token: 0x0200004D RID: 77
public static class WeaponProperties
{
	// Token: 0x06000476 RID: 1142 RVA: 0x0006A0D8 File Offset: 0x000682D8
	public static string GetDisplayName(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0006A11C File Offset: 0x0006831C
	public static string GetDisplayName(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0006A160 File Offset: 0x00068360
	public static string GetDisplayName(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0006A1A4 File Offset: 0x000683A4
	public static string GetSubtext(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0006A1E8 File Offset: 0x000683E8
	public static string GetSubtext(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0006A22C File Offset: 0x0006842C
	public static string GetSubtext(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0006A270 File Offset: 0x00068470
	public static string GetIconPath(Weapon weapon)
	{
		if (weapon == Weapon.level_weapon_peashot)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_spreadshot)
		{
			return "Icons/equip_icon_weapon_spread";
		}
		if (weapon == Weapon.plane_weapon_peashot)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_arc)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_homing)
		{
			return "Icons/equip_icon_weapon_homing";
		}
		if (weapon == Weapon.level_weapon_exploder)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_charge)
		{
			return "Icons/equip_icon_weapon_charge";
		}
		if (weapon == Weapon.level_weapon_boomerang)
		{
			return "Icons/equip_icon_weapon_boomerang";
		}
		if (weapon == Weapon.level_weapon_bouncer)
		{
			return "Icons/equip_icon_weapon_bouncer";
		}
		if (weapon == Weapon.arcade_weapon_peashot)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_weapon_laser)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_wide_shot)
		{
			return "Icons/equip_icon_weapon_wide_shot";
		}
		if (weapon == Weapon.plane_weapon_bomb)
		{
			return "Icons/";
		}
		if (weapon == Weapon.arcade_weapon_rocket_peashot)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_chalice_weapon_3way)
		{
			return "Icons/equip_icon_chalice_shmup_3way";
		}
		if (weapon == Weapon.level_weapon_accuracy)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_firecracker)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_firecrackerB)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_upshot)
		{
			return "Icons/equip_icon_weapon_upshot";
		}
		if (weapon == Weapon.level_weapon_pushback)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_chalice_weapon_bomb)
		{
			return "Icons/equip_icon_chalice_shmup_bomb";
		}
		if (weapon == Weapon.level_weapon_crackshot)
		{
			return "Icons/equip_icon_weapon_crackshot";
		}
		if (weapon == Weapon.level_weapon_splitter)
		{
			return "Icons/";
		}
		if (weapon != Weapon.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0006A420 File Offset: 0x00068620
	public static string GetIconPath(Super super)
	{
		if (super == Super.level_super_beam)
		{
			return "Icons/equip_icon_super_beam";
		}
		if (super == Super.level_super_ghost)
		{
			return "Icons/equip_icon_super_ghost";
		}
		if (super == Super.level_super_invincible)
		{
			return "Icons/equip_icon_super_invincible";
		}
		if (super == Super.plane_super_bomb)
		{
			return "Icons/";
		}
		if (super == Super.plane_super_chalice_bomb)
		{
			return "Icons/";
		}
		if (super == Super.level_super_chalice_iii)
		{
			return "Icons/equip_icon_super_ghost";
		}
		if (super == Super.level_super_chalice_vert_beam)
		{
			return "Icons/equip_icon_super_beam";
		}
		if (super == Super.level_super_chalice_shield)
		{
			return "Icons/equip_icon_super_invincible";
		}
		if (super == Super.level_super_chalice_bounce)
		{
			return "Icons/";
		}
		if (super != Super.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0006A4E4 File Offset: 0x000686E4
	public static string GetIconPath(Charm charm)
	{
		if (charm == Charm.charm_health_up_1)
		{
			return "Icons/equip_icon_charm_hp1";
		}
		if (charm == Charm.charm_super_builder)
		{
			return "Icons/equip_icon_charm_coffee";
		}
		if (charm == Charm.charm_smoke_dash)
		{
			return "Icons/equip_icon_charm_smoke-dash";
		}
		if (charm == Charm.charm_parry_plus)
		{
			return "Icons/equip_icon_charm_parry_slapper";
		}
		if (charm == Charm.charm_pit_saver)
		{
			return "Icons/equip_icon_charm_pitsaver";
		}
		if (charm == Charm.charm_parry_attack)
		{
			return "Icons/equip_icon_charm_parry_attack";
		}
		if (charm == Charm.charm_health_up_2)
		{
			return "Icons/equip_icon_charm_hp2";
		}
		if (charm == Charm.charm_chalice)
		{
			return "Icons/equip_icon_charm_chalice";
		}
		if (charm == Charm.charm_directional_dash)
		{
			return "Icons/";
		}
		if (charm == Charm.charm_healer)
		{
			return "Icons/equip_icon_charm_healer";
		}
		if (charm == Charm.charm_EX)
		{
			return "Icons/";
		}
		if (charm == Charm.charm_curse)
		{
			return "Icons/equip_icon_charm_curse";
		}
		if (charm == Charm.charm_float)
		{
			return "Icons/";
		}
		if (charm != Charm.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0006A5EC File Offset: 0x000687EC
	public static string GetDescription(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0006A630 File Offset: 0x00068830
	public static string GetDescription(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0006A674 File Offset: 0x00068874
	public static string GetDescription(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0006A6B8 File Offset: 0x000688B8
	public static int GetValue(Weapon weapon)
	{
		if (weapon == Weapon.level_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_spreadshot)
		{
			return 4;
		}
		if (weapon == Weapon.plane_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_arc)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_homing)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_exploder)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_charge)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_boomerang)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_bouncer)
		{
			return 4;
		}
		if (weapon == Weapon.arcade_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.plane_weapon_laser)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_wide_shot)
		{
			return 4;
		}
		if (weapon == Weapon.plane_weapon_bomb)
		{
			return 2;
		}
		if (weapon == Weapon.arcade_weapon_rocket_peashot)
		{
			return 10;
		}
		if (weapon == Weapon.plane_chalice_weapon_3way)
		{
			return 10;
		}
		if (weapon == Weapon.level_weapon_accuracy)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_firecracker)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_firecrackerB)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_upshot)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_pushback)
		{
			return 4;
		}
		if (weapon == Weapon.plane_chalice_weapon_bomb)
		{
			return 10;
		}
		if (weapon == Weapon.level_weapon_crackshot)
		{
			return 4;
		}
		if (weapon != Weapon.level_weapon_splitter)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0006A7FC File Offset: 0x000689FC
	public static int GetValue(Super super)
	{
		if (super == Super.level_super_beam)
		{
			return 0;
		}
		if (super == Super.level_super_ghost)
		{
			return 0;
		}
		if (super == Super.level_super_invincible)
		{
			return 0;
		}
		if (super == Super.plane_super_bomb)
		{
			return 10;
		}
		if (super == Super.plane_super_chalice_bomb)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_iii)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_vert_beam)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_shield)
		{
			return 10;
		}
		if (super != Super.level_super_chalice_bounce)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0006A88C File Offset: 0x00068A8C
	public static int GetValue(Charm charm)
	{
		if (charm == Charm.charm_health_up_1)
		{
			return 3;
		}
		if (charm == Charm.charm_super_builder)
		{
			return 3;
		}
		if (charm == Charm.charm_smoke_dash)
		{
			return 3;
		}
		if (charm == Charm.charm_parry_plus)
		{
			return 3;
		}
		if (charm == Charm.charm_pit_saver)
		{
			return 3;
		}
		if (charm == Charm.charm_parry_attack)
		{
			return 3;
		}
		if (charm == Charm.charm_health_up_2)
		{
			return 5;
		}
		if (charm == Charm.charm_chalice)
		{
			return 10;
		}
		if (charm == Charm.charm_directional_dash)
		{
			return 10;
		}
		if (charm == Charm.charm_healer)
		{
			return 3;
		}
		if (charm == Charm.charm_EX)
		{
			return 4;
		}
		if (charm == Charm.charm_curse)
		{
			return 1;
		}
		if (charm != Charm.charm_float)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x0200086C RID: 2156
	public static class ArcadeWeaponPeashot
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060050F5 RID: 20725 RVA: 0x0003DF8C File Offset: 0x0003C18C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060050F6 RID: 20726 RVA: 0x0003DF98 File Offset: 0x0003C198
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060050F7 RID: 20727 RVA: 0x0003DFA4 File Offset: 0x0003C1A4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x0400411B RID: 16667
		public static readonly int value = 2;

		// Token: 0x0400411C RID: 16668
		public static readonly string iconPath = "Icons/";

		// Token: 0x0400411D RID: 16669
		public static readonly Weapon id = Weapon.arcade_weapon_peashot;

		// Token: 0x0200159C RID: 5532
		public static class Basic
		{
			// Token: 0x04009034 RID: 36916
			public static readonly float damage = 4f;

			// Token: 0x04009035 RID: 36917
			public static readonly float speed = 850f;

			// Token: 0x04009036 RID: 36918
			public static readonly bool rapidFire;

			// Token: 0x04009037 RID: 36919
			public static readonly float rapidFireRate;
		}

		// Token: 0x0200159D RID: 5533
		public static class Ex
		{
		}
	}

	// Token: 0x0200086D RID: 2157
	public static class ArcadeWeaponRocketPeashot
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x0003DFCC File Offset: 0x0003C1CC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x0003DFE4 File Offset: 0x0003C1E4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x0400411E RID: 16670
		public static readonly int value = 10;

		// Token: 0x0400411F RID: 16671
		public static readonly string iconPath = "Icons/";

		// Token: 0x04004120 RID: 16672
		public static readonly Weapon id = Weapon.arcade_weapon_rocket_peashot;

		// Token: 0x0200159E RID: 5534
		public static class Basic
		{
			// Token: 0x04009038 RID: 36920
			public static readonly float damage = 4f;

			// Token: 0x04009039 RID: 36921
			public static readonly float speed = 700f;

			// Token: 0x0400903A RID: 36922
			public static readonly bool rapidFire;

			// Token: 0x0400903B RID: 36923
			public static readonly float rapidFireRate;
		}

		// Token: 0x0200159F RID: 5535
		public static class Ex
		{
		}
	}

	// Token: 0x0200086E RID: 2158
	public static class CharmChalice
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x0003E00D File Offset: 0x0003C20D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_chalice);
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x0003E019 File Offset: 0x0003C219
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_chalice);
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060050FF RID: 20735 RVA: 0x0003E025 File Offset: 0x0003C225
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_chalice);
			}
		}

		// Token: 0x04004121 RID: 16673
		public static readonly int value = 10;

		// Token: 0x04004122 RID: 16674
		public static readonly string iconPath = "Icons/equip_icon_charm_chalice";

		// Token: 0x04004123 RID: 16675
		public static readonly Charm id = Charm.charm_chalice;
	}

	// Token: 0x0200086F RID: 2159
	public static class CharmCharmParryPlus
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06005101 RID: 20737 RVA: 0x0003E04E File Offset: 0x0003C24E
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_parry_plus);
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06005102 RID: 20738 RVA: 0x0003E05A File Offset: 0x0003C25A
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_parry_plus);
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06005103 RID: 20739 RVA: 0x0003E066 File Offset: 0x0003C266
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_parry_plus);
			}
		}

		// Token: 0x04004124 RID: 16676
		public static readonly int value = 3;

		// Token: 0x04004125 RID: 16677
		public static readonly string iconPath = "Icons/equip_icon_charm_parry_slapper";

		// Token: 0x04004126 RID: 16678
		public static readonly Charm id = Charm.charm_parry_plus;
	}

	// Token: 0x02000870 RID: 2160
	public static class CharmCurse
	{
		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06005105 RID: 20741 RVA: 0x0003E08E File Offset: 0x0003C28E
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_curse);
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06005106 RID: 20742 RVA: 0x0003E09A File Offset: 0x0003C29A
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_curse);
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06005107 RID: 20743 RVA: 0x0003E0A6 File Offset: 0x0003C2A6
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_curse);
			}
		}

		// Token: 0x04004127 RID: 16679
		public static readonly int value = 1;

		// Token: 0x04004128 RID: 16680
		public static readonly string iconPath = "Icons/equip_icon_charm_curse";

		// Token: 0x04004129 RID: 16681
		public static readonly Charm id = Charm.charm_curse;

		// Token: 0x0400412A RID: 16682
		public static readonly int[] availableWeaponIDs = new int[]
		{
			1456773641,
			1456773649,
			1460621839,
			1466518900,
			1466416941,
			1467024095,
			1487081743,
			1568276855,
			1614768724
		};

		// Token: 0x0400412B RID: 16683
		public static readonly int[] availableShmupWeaponIDs = new int[]
		{
			1457006169,
			1492758857
		};

		// Token: 0x0400412C RID: 16684
		public static readonly int[] healthModifierValues = new int[]
		{
			-2,
			-2,
			-2,
			-2,
			0
		};

		// Token: 0x0400412D RID: 16685
		public static readonly float superMeterDelay = 1f;

		// Token: 0x0400412E RID: 16686
		public static readonly float[] superMeterAmount = new float[]
		{
			0f,
			0.13f,
			0.26f,
			0.39f,
			0.52f
		};

		// Token: 0x0400412F RID: 16687
		public static readonly int[] smokeDashInterval = new int[]
		{
			7,
			4,
			2,
			1,
			0
		};

		// Token: 0x04004130 RID: 16688
		public static readonly int[] whetstoneInterval = new int[]
		{
			7,
			4,
			2,
			1,
			0
		};

		// Token: 0x04004131 RID: 16689
		public static readonly string[] healerInterval = new string[]
		{
			"3,3,4",
			"2,3,4",
			"1,3,4",
			"1,2,4",
			"1,2,3"
		};

		// Token: 0x04004132 RID: 16690
		public static readonly int[] levelThreshold = new int[]
		{
			0,
			4,
			8,
			12,
			16
		};
	}

	// Token: 0x02000871 RID: 2161
	public static class CharmDirectionalDash
	{
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x0003E0B2 File Offset: 0x0003C2B2
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_directional_dash);
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x0003E0BE File Offset: 0x0003C2BE
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_directional_dash);
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x0003E0CA File Offset: 0x0003C2CA
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_directional_dash);
			}
		}

		// Token: 0x04004133 RID: 16691
		public static readonly int value = 10;

		// Token: 0x04004134 RID: 16692
		public static readonly string iconPath = "Icons/";

		// Token: 0x04004135 RID: 16693
		public static readonly Charm id = Charm.charm_directional_dash;
	}

	// Token: 0x02000872 RID: 2162
	public static class CharmEXCharm
	{
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x0600510D RID: 20749 RVA: 0x0003E0F3 File Offset: 0x0003C2F3
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_EX);
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x0003E0FF File Offset: 0x0003C2FF
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_EX);
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600510F RID: 20751 RVA: 0x0003E10B File Offset: 0x0003C30B
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_EX);
			}
		}

		// Token: 0x04004136 RID: 16694
		public static readonly int value = 4;

		// Token: 0x04004137 RID: 16695
		public static readonly string iconPath = "Icons/";

		// Token: 0x04004138 RID: 16696
		public static readonly Charm id = Charm.charm_EX;

		// Token: 0x04004139 RID: 16697
		public static readonly float planePeashotEXDebuff = 0.15f;
	}

	// Token: 0x02000873 RID: 2163
	public static class CharmFloat
	{
		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06005111 RID: 20753 RVA: 0x0003E13D File Offset: 0x0003C33D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_float);
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x0003E149 File Offset: 0x0003C349
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_float);
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06005113 RID: 20755 RVA: 0x0003E155 File Offset: 0x0003C355
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_float);
			}
		}

		// Token: 0x0400413A RID: 16698
		public static readonly int value = 10;

		// Token: 0x0400413B RID: 16699
		public static readonly string iconPath = "Icons/";

		// Token: 0x0400413C RID: 16700
		public static readonly Charm id = Charm.charm_float;

		// Token: 0x0400413D RID: 16701
		public static readonly float maxTime = 2f;

		// Token: 0x0400413E RID: 16702
		public static readonly float falloffStartTime = 1f;

		// Token: 0x0400413F RID: 16703
		public static readonly float minFallSpeed = 0.1f;

		// Token: 0x04004140 RID: 16704
		public static readonly float maxFallSpeed = 0.5f;
	}

	// Token: 0x02000874 RID: 2164
	public static class CharmHealer
	{
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06005115 RID: 20757 RVA: 0x0003E161 File Offset: 0x0003C361
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_healer);
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x0003E16D File Offset: 0x0003C36D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_healer);
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06005117 RID: 20759 RVA: 0x0003E179 File Offset: 0x0003C379
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_healer);
			}
		}

		// Token: 0x04004141 RID: 16705
		public static readonly int value = 3;

		// Token: 0x04004142 RID: 16706
		public static readonly string iconPath = "Icons/equip_icon_charm_healer";

		// Token: 0x04004143 RID: 16707
		public static readonly Charm id = Charm.charm_healer;
	}

	// Token: 0x02000875 RID: 2165
	public static class CharmHealthUpOne
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06005119 RID: 20761 RVA: 0x0003E1A1 File Offset: 0x0003C3A1
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_health_up_1);
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x0003E1AD File Offset: 0x0003C3AD
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_health_up_1);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600511B RID: 20763 RVA: 0x0003E1B9 File Offset: 0x0003C3B9
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_health_up_1);
			}
		}

		// Token: 0x04004144 RID: 16708
		public static readonly int value = 3;

		// Token: 0x04004145 RID: 16709
		public static readonly string iconPath = "Icons/equip_icon_charm_hp1";

		// Token: 0x04004146 RID: 16710
		public static readonly Charm id = Charm.charm_health_up_1;

		// Token: 0x04004147 RID: 16711
		public static readonly int healthIncrease = 1;

		// Token: 0x04004148 RID: 16712
		public static readonly float weaponDebuff = 0.05f;
	}

	// Token: 0x02000876 RID: 2166
	public static class CharmHealthUpTwo
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x0003E1F1 File Offset: 0x0003C3F1
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_health_up_2);
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x0003E1FD File Offset: 0x0003C3FD
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_health_up_2);
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x0600511F RID: 20767 RVA: 0x0003E209 File Offset: 0x0003C409
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_health_up_2);
			}
		}

		// Token: 0x04004149 RID: 16713
		public static readonly int value = 5;

		// Token: 0x0400414A RID: 16714
		public static readonly string iconPath = "Icons/equip_icon_charm_hp2";

		// Token: 0x0400414B RID: 16715
		public static readonly Charm id = Charm.charm_health_up_2;

		// Token: 0x0400414C RID: 16716
		public static readonly int healthIncrease = 2;

		// Token: 0x0400414D RID: 16717
		public static readonly float weaponDebuff = 0.1f;
	}

	// Token: 0x02000877 RID: 2167
	public static class CharmParryAttack
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x0003E241 File Offset: 0x0003C441
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_parry_attack);
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x0003E24D File Offset: 0x0003C44D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_parry_attack);
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x0003E259 File Offset: 0x0003C459
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_parry_attack);
			}
		}

		// Token: 0x0400414E RID: 16718
		public static readonly int value = 3;

		// Token: 0x0400414F RID: 16719
		public static readonly string iconPath = "Icons/equip_icon_charm_parry_attack";

		// Token: 0x04004150 RID: 16720
		public static readonly Charm id = Charm.charm_parry_attack;

		// Token: 0x04004151 RID: 16721
		public static readonly float damage = 16f;

		// Token: 0x04004152 RID: 16722
		public static readonly float bounce;
	}

	// Token: 0x02000878 RID: 2168
	public static class CharmPitSaver
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x0003E28B File Offset: 0x0003C48B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_pit_saver);
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x0003E297 File Offset: 0x0003C497
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_pit_saver);
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06005127 RID: 20775 RVA: 0x0003E2A3 File Offset: 0x0003C4A3
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_pit_saver);
			}
		}

		// Token: 0x04004153 RID: 16723
		public static readonly int value = 3;

		// Token: 0x04004154 RID: 16724
		public static readonly string iconPath = "Icons/equip_icon_charm_pitsaver";

		// Token: 0x04004155 RID: 16725
		public static readonly Charm id = Charm.charm_pit_saver;

		// Token: 0x04004156 RID: 16726
		public static readonly float meterAmount = 10f;

		// Token: 0x04004157 RID: 16727
		public static readonly float invulnerabilityMultiplier = 1.6f;
	}

	// Token: 0x02000879 RID: 2169
	public static class CharmSmokeDash
	{
		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06005129 RID: 20777 RVA: 0x0003E2DF File Offset: 0x0003C4DF
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x0003E2EB File Offset: 0x0003C4EB
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600512B RID: 20779 RVA: 0x0003E2F7 File Offset: 0x0003C4F7
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x04004158 RID: 16728
		public static readonly int value = 3;

		// Token: 0x04004159 RID: 16729
		public static readonly string iconPath = "Icons/equip_icon_charm_smoke-dash";

		// Token: 0x0400415A RID: 16730
		public static readonly Charm id = Charm.charm_smoke_dash;
	}

	// Token: 0x0200087A RID: 2170
	public static class CharmSuperBuilder
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600512D RID: 20781 RVA: 0x0003E31F File Offset: 0x0003C51F
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_super_builder);
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0003E32B File Offset: 0x0003C52B
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_super_builder);
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x0003E337 File Offset: 0x0003C537
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_super_builder);
			}
		}

		// Token: 0x0400415B RID: 16731
		public static readonly int value = 3;

		// Token: 0x0400415C RID: 16732
		public static readonly string iconPath = "Icons/equip_icon_charm_coffee";

		// Token: 0x0400415D RID: 16733
		public static readonly Charm id = Charm.charm_super_builder;

		// Token: 0x0400415E RID: 16734
		public static readonly float delay = 1f;

		// Token: 0x0400415F RID: 16735
		public static readonly float amount = 0.4f;
	}

	// Token: 0x0200087B RID: 2171
	public static class LevelSuperBeam
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06005131 RID: 20785 RVA: 0x0003E373 File Offset: 0x0003C573
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_beam);
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06005132 RID: 20786 RVA: 0x0003E37F File Offset: 0x0003C57F
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_beam);
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x0003E38B File Offset: 0x0003C58B
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_beam);
			}
		}

		// Token: 0x04004160 RID: 16736
		public static readonly int value;

		// Token: 0x04004161 RID: 16737
		public static readonly string iconPath = "Icons/equip_icon_super_beam";

		// Token: 0x04004162 RID: 16738
		public static readonly Super id = Super.level_super_beam;

		// Token: 0x04004163 RID: 16739
		public static readonly float time = 1.25f;

		// Token: 0x04004164 RID: 16740
		public static readonly float damage = 14.5f;

		// Token: 0x04004165 RID: 16741
		public static readonly float damageRate = 0.25f;
	}

	// Token: 0x0200087C RID: 2172
	public static class LevelSuperChaliceBounce
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06005135 RID: 20789 RVA: 0x0003E3CB File Offset: 0x0003C5CB
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06005136 RID: 20790 RVA: 0x0003E3D7 File Offset: 0x0003C5D7
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06005137 RID: 20791 RVA: 0x0003E3E3 File Offset: 0x0003C5E3
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x04004166 RID: 16742
		public static readonly int value = 10;

		// Token: 0x04004167 RID: 16743
		public static readonly string iconPath = "Icons/";

		// Token: 0x04004168 RID: 16744
		public static readonly Super id = Super.level_super_chalice_bounce;

		// Token: 0x04004169 RID: 16745
		public static readonly bool launchedVersion = true;

		// Token: 0x0400416A RID: 16746
		public static readonly float damage = 30f;

		// Token: 0x0400416B RID: 16747
		public static readonly float damageRate = 1f;

		// Token: 0x0400416C RID: 16748
		public static readonly float maxDamage = 300f;

		// Token: 0x0400416D RID: 16749
		public static readonly float duration = 7.5f;

		// Token: 0x0400416E RID: 16750
		public static readonly float horizontalAcceleration = 1200f;

		// Token: 0x0400416F RID: 16751
		public static readonly float maxHorizontalSpeed = 1000f;

		// Token: 0x04004170 RID: 16752
		public static readonly float bounceVelocity = 2250f;

		// Token: 0x04004171 RID: 16753
		public static readonly float bounceModifierNoJump = 1f;

		// Token: 0x04004172 RID: 16754
		public static readonly float gravity = 7000f;

		// Token: 0x04004173 RID: 16755
		public static readonly float enemyReboundMultiplier = 2f;

		// Token: 0x04004174 RID: 16756
		public static readonly float enemyMultihitDelay = 0.5f;
	}

	// Token: 0x0200087D RID: 2173
	public static class LevelSuperChaliceIII
	{
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06005139 RID: 20793 RVA: 0x0003E3EF File Offset: 0x0003C5EF
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600513A RID: 20794 RVA: 0x0003E3FB File Offset: 0x0003C5FB
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x0003E407 File Offset: 0x0003C607
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x04004175 RID: 16757
		public static readonly int value = 10;

		// Token: 0x04004176 RID: 16758
		public static readonly string iconPath = "Icons/equip_icon_super_ghost";

		// Token: 0x04004177 RID: 16759
		public static readonly Super id = Super.level_super_chalice_iii;

		// Token: 0x04004178 RID: 16760
		public static readonly float superDuration = 6.5f;
	}

	// Token: 0x0200087E RID: 2174
	public static class LevelSuperChaliceShield
	{
		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x0003E43A File Offset: 0x0003C63A
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x0003E446 File Offset: 0x0003C646
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x0003E452 File Offset: 0x0003C652
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x04004179 RID: 16761
		public static readonly int value = 10;

		// Token: 0x0400417A RID: 16762
		public static readonly string iconPath = "Icons/equip_icon_super_invincible";

		// Token: 0x0400417B RID: 16763
		public static readonly Super id = Super.level_super_chalice_shield;
	}

	// Token: 0x0200087F RID: 2175
	public static class LevelSuperChaliceVertBeam
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06005141 RID: 20801 RVA: 0x0003E47B File Offset: 0x0003C67B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x0003E487 File Offset: 0x0003C687
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06005143 RID: 20803 RVA: 0x0003E493 File Offset: 0x0003C693
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x0400417C RID: 16764
		public static readonly int value = 10;

		// Token: 0x0400417D RID: 16765
		public static readonly string iconPath = "Icons/equip_icon_super_beam";

		// Token: 0x0400417E RID: 16766
		public static readonly Super id = Super.level_super_chalice_vert_beam;

		// Token: 0x0400417F RID: 16767
		public static readonly float time = 1.25f;

		// Token: 0x04004180 RID: 16768
		public static readonly float damage = 21.5f;

		// Token: 0x04004181 RID: 16769
		public static readonly float damageRate = 0.25f;
	}

	// Token: 0x02000880 RID: 2176
	public static class LevelSuperGhost
	{
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x0003E4DA File Offset: 0x0003C6DA
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_ghost);
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06005146 RID: 20806 RVA: 0x0003E4E6 File Offset: 0x0003C6E6
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_ghost);
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x0003E4F2 File Offset: 0x0003C6F2
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_ghost);
			}
		}

		// Token: 0x04004182 RID: 16770
		public static readonly int value;

		// Token: 0x04004183 RID: 16771
		public static readonly string iconPath = "Icons/equip_icon_super_ghost";

		// Token: 0x04004184 RID: 16772
		public static readonly Super id = Super.level_super_ghost;

		// Token: 0x04004185 RID: 16773
		public static readonly float initialSpeed = 700f;

		// Token: 0x04004186 RID: 16774
		public static readonly float maxSpeed = 1250f;

		// Token: 0x04004187 RID: 16775
		public static readonly float initialSpeedTime = 1.8f;

		// Token: 0x04004188 RID: 16776
		public static readonly float maxSpeedTime = 3.8f;

		// Token: 0x04004189 RID: 16777
		public static readonly float noHeartMaxSpeedTime = 3.7f;

		// Token: 0x0400418A RID: 16778
		public static readonly float accelerationTime = 1f;

		// Token: 0x0400418B RID: 16779
		public static readonly float heartSpeed = 100f;

		// Token: 0x0400418C RID: 16780
		public static readonly float damage = 5.1f;

		// Token: 0x0400418D RID: 16781
		public static readonly float damageRate = 0.22f;

		// Token: 0x0400418E RID: 16782
		public static readonly float turnaroundEaseMultiplier = 4f;
	}

	// Token: 0x02000881 RID: 2177
	public static class LevelSuperInvincibility
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06005149 RID: 20809 RVA: 0x0003E4FE File Offset: 0x0003C6FE
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_invincible);
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x0600514A RID: 20810 RVA: 0x0003E50A File Offset: 0x0003C70A
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_invincible);
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x0003E516 File Offset: 0x0003C716
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_invincible);
			}
		}

		// Token: 0x0400418F RID: 16783
		public static readonly int value;

		// Token: 0x04004190 RID: 16784
		public static readonly string iconPath = "Icons/equip_icon_super_invincible";

		// Token: 0x04004191 RID: 16785
		public static readonly Super id = Super.level_super_invincible;

		// Token: 0x04004192 RID: 16786
		public static readonly float durationInvincible = 4.85f;

		// Token: 0x04004193 RID: 16787
		public static readonly float durationFX = 4.55f;
	}

	// Token: 0x02000882 RID: 2178
	public static class LevelWeaponAccuracy
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x0600514D RID: 20813 RVA: 0x0003E54C File Offset: 0x0003C74C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x0600514E RID: 20814 RVA: 0x0003E558 File Offset: 0x0003C758
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x0003E564 File Offset: 0x0003C764
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x04004194 RID: 16788
		public static readonly int value = 4;

		// Token: 0x04004195 RID: 16789
		public static readonly string iconPath = "Icons/";

		// Token: 0x04004196 RID: 16790
		public static readonly Weapon id = Weapon.level_weapon_accuracy;

		// Token: 0x020015A0 RID: 5536
		public static class Basic
		{
			// Token: 0x0400903C RID: 36924
			public static readonly float LvlOneFireRate = 0.28f;

			// Token: 0x0400903D RID: 36925
			public static readonly float LvlOneSpeed = 1200f;

			// Token: 0x0400903E RID: 36926
			public static readonly float LvlOneSize = 1.2f;

			// Token: 0x0400903F RID: 36927
			public static readonly float LvlOneDamage = 4f;

			// Token: 0x04009040 RID: 36928
			public static readonly int LvlTwoCounter = 20;

			// Token: 0x04009041 RID: 36929
			public static readonly float LvlTwoFireRate = 0.23f;

			// Token: 0x04009042 RID: 36930
			public static readonly float LvlTwoSpeed = 1500f;

			// Token: 0x04009043 RID: 36931
			public static readonly float LvlTwoSize = 1.8f;

			// Token: 0x04009044 RID: 36932
			public static readonly float LvlTwoDamage = 5.5f;

			// Token: 0x04009045 RID: 36933
			public static readonly int LvlThreeCounter = 40;

			// Token: 0x04009046 RID: 36934
			public static readonly float LvlThreeFireRate = 0.2f;

			// Token: 0x04009047 RID: 36935
			public static readonly float LvlThreeSpeed = 1900f;

			// Token: 0x04009048 RID: 36936
			public static readonly float LvlThreeSize = 3.2f;

			// Token: 0x04009049 RID: 36937
			public static readonly float LvlThreeDamage = 7.5f;

			// Token: 0x0400904A RID: 36938
			public static readonly int LvlFourCounter = 60;

			// Token: 0x0400904B RID: 36939
			public static readonly float LvlFourFireRate = 0.18f;

			// Token: 0x0400904C RID: 36940
			public static readonly float LvlFourSpeed = 2350f;

			// Token: 0x0400904D RID: 36941
			public static readonly float LvlFourSize = 5f;

			// Token: 0x0400904E RID: 36942
			public static readonly float LvlFourDamage = 8.5f;
		}

		// Token: 0x020015A1 RID: 5537
		public static class Ex
		{
			// Token: 0x0400904F RID: 36943
			public static readonly float exSpeed = 1800f;

			// Token: 0x04009050 RID: 36944
			public static readonly float exDamage = 25f;

			// Token: 0x04009051 RID: 36945
			public static readonly int exShotEquivalent = 15;

			// Token: 0x04009052 RID: 36946
			public static readonly float exShotSize = 8f;
		}
	}

	// Token: 0x02000883 RID: 2179
	public static class LevelWeaponArc
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x0003E58C File Offset: 0x0003C78C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x0003E598 File Offset: 0x0003C798
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x0003E5A4 File Offset: 0x0003C7A4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x04004197 RID: 16791
		public static readonly int value = 2;

		// Token: 0x04004198 RID: 16792
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x04004199 RID: 16793
		public static readonly Weapon id = Weapon.level_weapon_arc;

		// Token: 0x020015A2 RID: 5538
		public static class Basic
		{
			// Token: 0x04009053 RID: 36947
			public static readonly int Movement;

			// Token: 0x04009054 RID: 36948
			public static readonly float launchSpeed = 1600f;

			// Token: 0x04009055 RID: 36949
			public static readonly float gravity = 2750f;

			// Token: 0x04009056 RID: 36950
			public static readonly float straightShotAngle = 65f;

			// Token: 0x04009057 RID: 36951
			public static readonly float fireRate = 0.4f;

			// Token: 0x04009058 RID: 36952
			public static readonly bool rapidFire = true;

			// Token: 0x04009059 RID: 36953
			public static readonly int maxNumMines = 1;

			// Token: 0x0400905A RID: 36954
			public static readonly float baseDamage = 14f;

			// Token: 0x0400905B RID: 36955
			public static readonly float timeStateTwo = 1.25f;

			// Token: 0x0400905C RID: 36956
			public static readonly float damageStateTwo = 7.5f;

			// Token: 0x0400905D RID: 36957
			public static readonly float timeStateThree = 2.5f;

			// Token: 0x0400905E RID: 36958
			public static readonly float damageStateThree = 11.25f;

			// Token: 0x0400905F RID: 36959
			public static readonly float diagLaunchSpeed = 600f;

			// Token: 0x04009060 RID: 36960
			public static readonly float diagGravity = 1000f;

			// Token: 0x04009061 RID: 36961
			public static readonly float diagShotAngle = 45f;
		}

		// Token: 0x020015A3 RID: 5539
		public static class Ex
		{
			// Token: 0x04009062 RID: 36962
			public static readonly float launchSpeed = 1600f;

			// Token: 0x04009063 RID: 36963
			public static readonly float gravity = 2750f;

			// Token: 0x04009064 RID: 36964
			public static readonly float damage = 28f;

			// Token: 0x04009065 RID: 36965
			public static readonly float explodeDelay = 2f;
		}
	}

	// Token: 0x02000884 RID: 2180
	public static class LevelWeaponBoomerang
	{
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x0003E5CC File Offset: 0x0003C7CC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x0003E5D8 File Offset: 0x0003C7D8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06005157 RID: 20823 RVA: 0x0003E5E4 File Offset: 0x0003C7E4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x0400419A RID: 16794
		public static readonly int value = 4;

		// Token: 0x0400419B RID: 16795
		public static readonly string iconPath = "Icons/equip_icon_weapon_boomerang";

		// Token: 0x0400419C RID: 16796
		public static readonly Weapon id = Weapon.level_weapon_boomerang;

		// Token: 0x020015A4 RID: 5540
		public static class Basic
		{
			// Token: 0x04009066 RID: 36966
			public static readonly float fireRate = 0.25f;

			// Token: 0x04009067 RID: 36967
			public static readonly float speed = 1400f;

			// Token: 0x04009068 RID: 36968
			public static readonly float damage = 8.5f;

			// Token: 0x04009069 RID: 36969
			public static readonly string xDistanceString = "550,450,520,480";

			// Token: 0x0400906A RID: 36970
			public static readonly string yDistanceString = "100,  50,  80, 70";
		}

		// Token: 0x020015A5 RID: 5541
		public static class Ex
		{
			// Token: 0x0400906B RID: 36971
			public static readonly float speed = 1000f;

			// Token: 0x0400906C RID: 36972
			public static readonly float damage = 5f;

			// Token: 0x0400906D RID: 36973
			public static readonly float damageRate = 0.2f;

			// Token: 0x0400906E RID: 36974
			public static readonly float maxDamage = 35f;

			// Token: 0x0400906F RID: 36975
			public static readonly float xDistance = 400f;

			// Token: 0x04009070 RID: 36976
			public static readonly float yDistance = 110f;

			// Token: 0x04009071 RID: 36977
			public static readonly string pinkString = "2,3,2,4";

			// Token: 0x04009072 RID: 36978
			public static readonly float hitFreezeTime = 0.1f;
		}
	}

	// Token: 0x02000885 RID: 2181
	public static class LevelWeaponBouncer
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x0003E60C File Offset: 0x0003C80C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x0003E618 File Offset: 0x0003C818
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600515B RID: 20827 RVA: 0x0003E624 File Offset: 0x0003C824
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x0400419D RID: 16797
		public static readonly int value = 4;

		// Token: 0x0400419E RID: 16798
		public static readonly string iconPath = "Icons/equip_icon_weapon_bouncer";

		// Token: 0x0400419F RID: 16799
		public static readonly Weapon id = Weapon.level_weapon_bouncer;

		// Token: 0x020015A6 RID: 5542
		public static class Basic
		{
			// Token: 0x04009073 RID: 36979
			public static readonly float launchSpeed = 1200f;

			// Token: 0x04009074 RID: 36980
			public static readonly float gravity = 3600f;

			// Token: 0x04009075 RID: 36981
			public static readonly float bounceRatio = 1.3f;

			// Token: 0x04009076 RID: 36982
			public static readonly float bounceSpeedDampening = 800f;

			// Token: 0x04009077 RID: 36983
			public static readonly float straightExtraAngle = 22.5f;

			// Token: 0x04009078 RID: 36984
			public static readonly float diagonalUpExtraAngle;

			// Token: 0x04009079 RID: 36985
			public static readonly float diagonalDownExtraAngle = 10f;

			// Token: 0x0400907A RID: 36986
			public static readonly float damage = 11.6f;

			// Token: 0x0400907B RID: 36987
			public static readonly float fireRate = 0.33f;

			// Token: 0x0400907C RID: 36988
			public static readonly int numBounces = 2;
		}

		// Token: 0x020015A7 RID: 5543
		public static class Ex
		{
			// Token: 0x0400907D RID: 36989
			public static readonly float launchSpeed = 1600f;

			// Token: 0x0400907E RID: 36990
			public static readonly float gravity = 2750f;

			// Token: 0x0400907F RID: 36991
			public static readonly float damage = 28f;

			// Token: 0x04009080 RID: 36992
			public static readonly float explodeDelay = 2f;
		}
	}

	// Token: 0x02000886 RID: 2182
	public static class LevelWeaponCharge
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600515D RID: 20829 RVA: 0x0003E64C File Offset: 0x0003C84C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600515E RID: 20830 RVA: 0x0003E658 File Offset: 0x0003C858
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600515F RID: 20831 RVA: 0x0003E664 File Offset: 0x0003C864
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x040041A0 RID: 16800
		public static readonly int value = 4;

		// Token: 0x040041A1 RID: 16801
		public static readonly string iconPath = "Icons/equip_icon_weapon_charge";

		// Token: 0x040041A2 RID: 16802
		public static readonly Weapon id = Weapon.level_weapon_charge;

		// Token: 0x020015A8 RID: 5544
		public static class Basic
		{
			// Token: 0x04009081 RID: 36993
			public static readonly float fireRate = 0.25f;

			// Token: 0x04009082 RID: 36994
			public static readonly float baseDamage = 6f;

			// Token: 0x04009083 RID: 36995
			public static readonly float speed = 1050f;

			// Token: 0x04009084 RID: 36996
			public static readonly float timeStateTwo = 9999f;

			// Token: 0x04009085 RID: 36997
			public static readonly float damageStateTwo = 20f;

			// Token: 0x04009086 RID: 36998
			public static readonly float speedStateTwo = 1300f;

			// Token: 0x04009087 RID: 36999
			public static readonly float timeStateThree = 1f;

			// Token: 0x04009088 RID: 37000
			public static readonly float damageStateThree = 46f;
		}

		// Token: 0x020015A9 RID: 5545
		public static class Ex
		{
			// Token: 0x04009089 RID: 37001
			public static readonly float damage = 26f;

			// Token: 0x0400908A RID: 37002
			public static readonly float radius = 300f;
		}
	}

	// Token: 0x02000887 RID: 2183
	public static class LevelWeaponCrackshot
	{
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06005161 RID: 20833 RVA: 0x0003E68C File Offset: 0x0003C88C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x0003E698 File Offset: 0x0003C898
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x0003E6A4 File Offset: 0x0003C8A4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x040041A3 RID: 16803
		public static readonly int value = 4;

		// Token: 0x040041A4 RID: 16804
		public static readonly string iconPath = "Icons/equip_icon_weapon_crackshot";

		// Token: 0x040041A5 RID: 16805
		public static readonly Weapon id = Weapon.level_weapon_crackshot;

		// Token: 0x020015AA RID: 5546
		public static class Basic
		{
			// Token: 0x0400908B RID: 37003
			public static readonly float fireRate = 0.32f;

			// Token: 0x0400908C RID: 37004
			public static readonly float initialSpeed = 1050f;

			// Token: 0x0400908D RID: 37005
			public static readonly float crackDistance = 290f;

			// Token: 0x0400908E RID: 37006
			public static readonly float crackedSpeed = 2500f;

			// Token: 0x0400908F RID: 37007
			public static readonly float initialDamage = 10.56f;

			// Token: 0x04009090 RID: 37008
			public static readonly float crackedDamage = 6.7f;

			// Token: 0x04009091 RID: 37009
			public static readonly bool enableMaxAngle = true;

			// Token: 0x04009092 RID: 37010
			public static readonly float maxAngle = 170f;
		}

		// Token: 0x020015AB RID: 5547
		public static class Ex
		{
			// Token: 0x04009093 RID: 37011
			public static readonly float launchDistance = 100f;

			// Token: 0x04009094 RID: 37012
			public static readonly float timeToHoverPoint = 0.5f;

			// Token: 0x04009095 RID: 37013
			public static readonly float hoverWidth = 37f;

			// Token: 0x04009096 RID: 37014
			public static readonly float hoverHeight = 35f;

			// Token: 0x04009097 RID: 37015
			public static readonly float hoverSpeed = 0.9f;

			// Token: 0x04009098 RID: 37016
			public static readonly float bulletSpeed = 2000f;

			// Token: 0x04009099 RID: 37017
			public static readonly float bulletDamage = 3.5f;

			// Token: 0x0400909A RID: 37018
			public static readonly float collideDamage = 12f;

			// Token: 0x0400909B RID: 37019
			public static readonly int shotNumber = 5;

			// Token: 0x0400909C RID: 37020
			public static readonly float shootDelay = 1f;

			// Token: 0x0400909D RID: 37021
			public static readonly float riseSpeed;

			// Token: 0x0400909E RID: 37022
			public static readonly bool isPink = true;

			// Token: 0x0400909F RID: 37023
			public static readonly float parryBulletDamage = 14f;

			// Token: 0x040090A0 RID: 37024
			public static readonly float parryBulletSpeed = 2000f;

			// Token: 0x040090A1 RID: 37025
			public static readonly float parryTimeOut = 0.15f;
		}
	}

	// Token: 0x02000888 RID: 2184
	public static class LevelWeaponExploder
	{
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06005165 RID: 20837 RVA: 0x0003E6CC File Offset: 0x0003C8CC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x0003E6D8 File Offset: 0x0003C8D8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x0003E6E4 File Offset: 0x0003C8E4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x040041A6 RID: 16806
		public static readonly int value = 2;

		// Token: 0x040041A7 RID: 16807
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041A8 RID: 16808
		public static readonly Weapon id = Weapon.level_weapon_exploder;

		// Token: 0x020015AC RID: 5548
		public static class Basic
		{
			// Token: 0x040090A2 RID: 37026
			public static readonly float fireRate = 0.35f;

			// Token: 0x040090A3 RID: 37027
			public static readonly bool rapideFire = true;

			// Token: 0x040090A4 RID: 37028
			public static readonly float speed = 1200f;

			// Token: 0x040090A5 RID: 37029
			public static readonly float sinSpeed = 10f;

			// Token: 0x040090A6 RID: 37030
			public static readonly float sinSize = 0.1f;

			// Token: 0x040090A7 RID: 37031
			public static readonly float baseDamage = 6f;

			// Token: 0x040090A8 RID: 37032
			public static readonly float baseExplosionRadius = 15f;

			// Token: 0x040090A9 RID: 37033
			public static readonly float baseScale = 0.1f;

			// Token: 0x040090AA RID: 37034
			public static readonly float timeStateTwo = 0.25f;

			// Token: 0x040090AB RID: 37035
			public static readonly float damageStateTwo = 10f;

			// Token: 0x040090AC RID: 37036
			public static readonly float explosionRadiusStateTwo = 70f;

			// Token: 0x040090AD RID: 37037
			public static readonly float scaleStateTwo = 0.5f;

			// Token: 0x040090AE RID: 37038
			public static readonly float timeStateThree = 0.5f;

			// Token: 0x040090AF RID: 37039
			public static readonly float damageStateThree = 12.75f;

			// Token: 0x040090B0 RID: 37040
			public static readonly float explosionRadiusStateThree = 130f;

			// Token: 0x040090B1 RID: 37041
			public static readonly float scaleStateThree = 1f;

			// Token: 0x040090B2 RID: 37042
			public static readonly bool easing = true;

			// Token: 0x040090B3 RID: 37043
			public static readonly MinMax easeSpeed = new MinMax(900f, 2500f);

			// Token: 0x040090B4 RID: 37044
			public static readonly float easeTime = 1f;
		}

		// Token: 0x020015AD RID: 5549
		public static class Ex
		{
			// Token: 0x040090B5 RID: 37045
			public static readonly float speed = 1300f;

			// Token: 0x040090B6 RID: 37046
			public static readonly float damage = 35f;

			// Token: 0x040090B7 RID: 37047
			public static readonly float hitRate;

			// Token: 0x040090B8 RID: 37048
			public static readonly float explodeRadius = 300f;

			// Token: 0x040090B9 RID: 37049
			public static readonly float shrapnelSpeed = 1200f;

			// Token: 0x040090BA RID: 37050
			public static readonly bool damageOn = true;
		}
	}

	// Token: 0x02000889 RID: 2185
	public static class LevelWeaponFirecracker
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06005169 RID: 20841 RVA: 0x0003E70C File Offset: 0x0003C90C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x0003E718 File Offset: 0x0003C918
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600516B RID: 20843 RVA: 0x0003E724 File Offset: 0x0003C924
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x040041A9 RID: 16809
		public static readonly int value = 4;

		// Token: 0x040041AA RID: 16810
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041AB RID: 16811
		public static readonly Weapon id = Weapon.level_weapon_firecracker;

		// Token: 0x020015AE RID: 5550
		public static class Basic
		{
			// Token: 0x040090BB RID: 37051
			public static readonly float fireRate = 0.06f;

			// Token: 0x040090BC RID: 37052
			public static readonly float bulletSpeed = 2250f;

			// Token: 0x040090BD RID: 37053
			public static readonly float bulletLife = 0.17f;

			// Token: 0x040090BE RID: 37054
			public static readonly float explosionDamage = 2.6f;

			// Token: 0x040090BF RID: 37055
			public static readonly float explosionSize = 10f;

			// Token: 0x040090C0 RID: 37056
			public static readonly float explosionDuration = 0.1f;
		}

		// Token: 0x020015AF RID: 5551
		public static class Ex
		{
			// Token: 0x040090C1 RID: 37057
			public static readonly float exSpeed = 1700f;

			// Token: 0x040090C2 RID: 37058
			public static readonly float explosionRadius = 20f;

			// Token: 0x040090C3 RID: 37059
			public static readonly float damageRate = 0.5f;

			// Token: 0x040090C4 RID: 37060
			public static readonly float explosionDamage = 3f;

			// Token: 0x040090C5 RID: 37061
			public static readonly float explosionTime = 2f;

			// Token: 0x040090C6 RID: 37062
			public static readonly float exLife = 1f;
		}
	}

	// Token: 0x0200088A RID: 2186
	public static class LevelWeaponFirecrackerB
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600516D RID: 20845 RVA: 0x0003E74C File Offset: 0x0003C94C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600516E RID: 20846 RVA: 0x0003E758 File Offset: 0x0003C958
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x0003E764 File Offset: 0x0003C964
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x040041AC RID: 16812
		public static readonly int value = 4;

		// Token: 0x040041AD RID: 16813
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041AE RID: 16814
		public static readonly Weapon id = Weapon.level_weapon_firecrackerB;

		// Token: 0x020015B0 RID: 5552
		public static class Basic
		{
			// Token: 0x040090C7 RID: 37063
			public static readonly float fireRate = 0.09f;

			// Token: 0x040090C8 RID: 37064
			public static readonly float bulletSpeed = 2000f;

			// Token: 0x040090C9 RID: 37065
			public static readonly float bulletLife = 0.2f;

			// Token: 0x040090CA RID: 37066
			public static readonly float explosionDamage = 0.5f;

			// Token: 0x040090CB RID: 37067
			public static readonly float explosionSize = 5f;

			// Token: 0x040090CC RID: 37068
			public static readonly float explosionDuration = 0.16f;

			// Token: 0x040090CD RID: 37069
			public static readonly string explosionAngleString = "45,180,270,135,315,225,90,0";

			// Token: 0x040090CE RID: 37070
			public static readonly float explosionsRadiusSize = 68f;
		}

		// Token: 0x020015B1 RID: 5553
		public static class Ex
		{
			// Token: 0x040090CF RID: 37071
			public static readonly float exSpeed = 1700f;

			// Token: 0x040090D0 RID: 37072
			public static readonly float explosionRadius = 20f;

			// Token: 0x040090D1 RID: 37073
			public static readonly float damageRate = 0.5f;

			// Token: 0x040090D2 RID: 37074
			public static readonly float explosionDamage = 3f;

			// Token: 0x040090D3 RID: 37075
			public static readonly float explosionTime = 2f;

			// Token: 0x040090D4 RID: 37076
			public static readonly float exLife = 1f;
		}
	}

	// Token: 0x0200088B RID: 2187
	public static class LevelWeaponHoming
	{
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x0003E78C File Offset: 0x0003C98C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x0003E798 File Offset: 0x0003C998
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06005173 RID: 20851 RVA: 0x0003E7A4 File Offset: 0x0003C9A4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x040041AF RID: 16815
		public static readonly int value = 4;

		// Token: 0x040041B0 RID: 16816
		public static readonly string iconPath = "Icons/equip_icon_weapon_homing";

		// Token: 0x040041B1 RID: 16817
		public static readonly Weapon id = Weapon.level_weapon_homing;

		// Token: 0x020015B2 RID: 5554
		public static class Basic
		{
			// Token: 0x040090D5 RID: 37077
			public static readonly MinMax fireRate = new MinMax(0.15f, 0.15f);

			// Token: 0x040090D6 RID: 37078
			public static readonly float speed = 1000f;

			// Token: 0x040090D7 RID: 37079
			public static readonly float damage = 2.85f;

			// Token: 0x040090D8 RID: 37080
			public static readonly MinMax rotationSpeed = new MinMax(0f, 500f);

			// Token: 0x040090D9 RID: 37081
			public static readonly float timeBeforeEaseRotationSpeed = 0f;

			// Token: 0x040090DA RID: 37082
			public static readonly float rotationSpeedEaseTime = 0.4f;

			// Token: 0x040090DB RID: 37083
			public static readonly float lockedShotAccelerationTime = 0.5f;

			// Token: 0x040090DC RID: 37084
			public static readonly float speedVariation = 100f;

			// Token: 0x040090DD RID: 37085
			public static readonly float angleVariation = 5f;

			// Token: 0x040090DE RID: 37086
			public static readonly int trailFrameDelay = 2;

			// Token: 0x040090DF RID: 37087
			public static readonly float maxHomingTime = 2.5f;
		}

		// Token: 0x020015B3 RID: 5555
		public static class Ex
		{
			// Token: 0x040090E0 RID: 37088
			public static readonly float speed = 1500f;

			// Token: 0x040090E1 RID: 37089
			public static readonly float damage = 7f;

			// Token: 0x040090E2 RID: 37090
			public static readonly float spread = 90f;

			// Token: 0x040090E3 RID: 37091
			public static readonly int bulletCount = 4;

			// Token: 0x040090E4 RID: 37092
			public static readonly float swirlDistance = 100f;

			// Token: 0x040090E5 RID: 37093
			public static readonly float swirlEaseTime = 0.75f;

			// Token: 0x040090E6 RID: 37094
			public static readonly int trailFrameDelay = 2;
		}
	}

	// Token: 0x0200088C RID: 2188
	public static class LevelWeaponPeashot
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x0003E7CC File Offset: 0x0003C9CC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x0003E7D8 File Offset: 0x0003C9D8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x0003E7E4 File Offset: 0x0003C9E4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x040041B2 RID: 16818
		public static readonly int value = 2;

		// Token: 0x040041B3 RID: 16819
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x040041B4 RID: 16820
		public static readonly Weapon id = Weapon.level_weapon_peashot;

		// Token: 0x020015B4 RID: 5556
		public static class Basic
		{
			// Token: 0x040090E7 RID: 37095
			public static readonly float damage = 4f;

			// Token: 0x040090E8 RID: 37096
			public static readonly float speed = 2250f;

			// Token: 0x040090E9 RID: 37097
			public static readonly bool rapidFire = true;

			// Token: 0x040090EA RID: 37098
			public static readonly float rapidFireRate = 0.11f;
		}

		// Token: 0x020015B5 RID: 5557
		public static class Ex
		{
			// Token: 0x040090EB RID: 37099
			public static readonly float damage = 8.334f;

			// Token: 0x040090EC RID: 37100
			public static readonly float maxDamage = 25f;

			// Token: 0x040090ED RID: 37101
			public static readonly float damageDistance = 80f;

			// Token: 0x040090EE RID: 37102
			public static readonly float speed = 1500f;

			// Token: 0x040090EF RID: 37103
			public static readonly float freezeTime = 0.05f;
		}
	}

	// Token: 0x0200088D RID: 2189
	public static class LevelWeaponPushback
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06005179 RID: 20857 RVA: 0x0003E80C File Offset: 0x0003CA0C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600517A RID: 20858 RVA: 0x0003E818 File Offset: 0x0003CA18
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x0003E824 File Offset: 0x0003CA24
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x040041B5 RID: 16821
		public static readonly int value = 4;

		// Token: 0x040041B6 RID: 16822
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041B7 RID: 16823
		public static readonly Weapon id = Weapon.level_weapon_pushback;

		// Token: 0x020015B6 RID: 5558
		public static class Basic
		{
			// Token: 0x040090F0 RID: 37104
			public static readonly float damage = 4f;

			// Token: 0x040090F1 RID: 37105
			public static readonly MinMax fireRate = new MinMax(0.1f, 0.7f);

			// Token: 0x040090F2 RID: 37106
			public static readonly MinMax speed = new MinMax(700f, 1300f);

			// Token: 0x040090F3 RID: 37107
			public static readonly float speedTime = 3f;

			// Token: 0x040090F4 RID: 37108
			public static readonly float pushbackSpeed = 30f;
		}

		// Token: 0x020015B7 RID: 5559
		public static class Ex
		{
		}
	}

	// Token: 0x0200088E RID: 2190
	public static class LevelWeaponSplitter
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600517D RID: 20861 RVA: 0x0003E84C File Offset: 0x0003CA4C
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600517E RID: 20862 RVA: 0x0003E858 File Offset: 0x0003CA58
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x0003E864 File Offset: 0x0003CA64
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x040041B8 RID: 16824
		public static readonly int value = 10;

		// Token: 0x040041B9 RID: 16825
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041BA RID: 16826
		public static readonly Weapon id = Weapon.level_weapon_splitter;

		// Token: 0x020015B8 RID: 5560
		public static class Basic
		{
			// Token: 0x040090F5 RID: 37109
			public static readonly float fireRate = 0.22f;

			// Token: 0x040090F6 RID: 37110
			public static readonly float speed = 1700f;

			// Token: 0x040090F7 RID: 37111
			public static readonly float splitDistanceA = 200f;

			// Token: 0x040090F8 RID: 37112
			public static readonly float splitDistanceB = 550f;

			// Token: 0x040090F9 RID: 37113
			public static readonly float bulletDamage = 4f;

			// Token: 0x040090FA RID: 37114
			public static readonly float bulletDamageA = 2.15f;

			// Token: 0x040090FB RID: 37115
			public static readonly float bulletDamageB = 1.65f;

			// Token: 0x040090FC RID: 37116
			public static readonly float splitAngle = 20f;

			// Token: 0x040090FD RID: 37117
			public static readonly float angleDistance = 100f;
		}

		// Token: 0x020015B9 RID: 5561
		public static class Ex
		{
		}
	}

	// Token: 0x0200088F RID: 2191
	public static class LevelWeaponSpreadshot
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06005181 RID: 20865 RVA: 0x0003E88D File Offset: 0x0003CA8D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06005182 RID: 20866 RVA: 0x0003E899 File Offset: 0x0003CA99
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x0003E8A5 File Offset: 0x0003CAA5
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x040041BB RID: 16827
		public static readonly int value = 4;

		// Token: 0x040041BC RID: 16828
		public static readonly string iconPath = "Icons/equip_icon_weapon_spread";

		// Token: 0x040041BD RID: 16829
		public static readonly Weapon id = Weapon.level_weapon_spreadshot;

		// Token: 0x020015BA RID: 5562
		public static class Basic
		{
			// Token: 0x040090FE RID: 37118
			public static readonly float damage = 1.24f;

			// Token: 0x040090FF RID: 37119
			public static readonly float speed = 2250f;

			// Token: 0x04009100 RID: 37120
			public static readonly float distance = 375f;

			// Token: 0x04009101 RID: 37121
			public static readonly float rapidFireRate = 0.13f;
		}

		// Token: 0x020015BB RID: 5563
		public static class Ex
		{
			// Token: 0x04009102 RID: 37122
			public static readonly float damage = 4.3f;

			// Token: 0x04009103 RID: 37123
			public static readonly float speed = 500f;

			// Token: 0x04009104 RID: 37124
			public static readonly int childCount = 8;

			// Token: 0x04009105 RID: 37125
			public static readonly float radius = 100f;
		}
	}

	// Token: 0x02000890 RID: 2192
	public static class LevelWeaponUpshot
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06005185 RID: 20869 RVA: 0x0003E8CD File Offset: 0x0003CACD
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x0003E8D9 File Offset: 0x0003CAD9
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06005187 RID: 20871 RVA: 0x0003E8E5 File Offset: 0x0003CAE5
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x040041BE RID: 16830
		public static readonly int value = 4;

		// Token: 0x040041BF RID: 16831
		public static readonly string iconPath = "Icons/equip_icon_weapon_upshot";

		// Token: 0x040041C0 RID: 16832
		public static readonly Weapon id = Weapon.level_weapon_upshot;

		// Token: 0x020015BC RID: 5564
		public static class Basic
		{
			// Token: 0x04009106 RID: 37126
			public static readonly float damage = 2.33f;

			// Token: 0x04009107 RID: 37127
			public static readonly float fireRate = 0.2f;

			// Token: 0x04009108 RID: 37128
			public static readonly float[] xSpeed = new float[]
			{
				630f,
				819f,
				945f
			};

			// Token: 0x04009109 RID: 37129
			public static readonly MinMax[] ySpeed = new MinMax[]
			{
				new MinMax(0f, 3240f),
				new MinMax(0f, 3240f),
				new MinMax(0f, 3240f)
			};

			// Token: 0x0400910A RID: 37130
			public static readonly float[] timeToMaxSpeed = new float[]
			{
				1.08f,
				0.81f,
				0.945f
			};
		}

		// Token: 0x020015BD RID: 5565
		public static class Ex
		{
			// Token: 0x0400910B RID: 37131
			public static readonly float minRotationSpeed = 375f;

			// Token: 0x0400910C RID: 37132
			public static readonly float maxRotationSpeed = 185f;

			// Token: 0x0400910D RID: 37133
			public static readonly float rotationRampTime = 1.8f;

			// Token: 0x0400910E RID: 37134
			public static readonly float minRadiusSpeed = 195f;

			// Token: 0x0400910F RID: 37135
			public static readonly float maxRadiusSpeed = 365f;

			// Token: 0x04009110 RID: 37136
			public static readonly float radiusRampTime = 1.8f;

			// Token: 0x04009111 RID: 37137
			public static readonly float damage = 8f;

			// Token: 0x04009112 RID: 37138
			public static readonly float damageRate = 0.3f;

			// Token: 0x04009113 RID: 37139
			public static readonly float maxDamage = 37f;

			// Token: 0x04009114 RID: 37140
			public static readonly float freezeTime = 0.1f;
		}
	}

	// Token: 0x02000891 RID: 2193
	public static class LevelWeaponWideShot
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06005189 RID: 20873 RVA: 0x0003E90D File Offset: 0x0003CB0D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x0600518A RID: 20874 RVA: 0x0003E919 File Offset: 0x0003CB19
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x0003E925 File Offset: 0x0003CB25
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x040041C1 RID: 16833
		public static readonly int value = 4;

		// Token: 0x040041C2 RID: 16834
		public static readonly string iconPath = "Icons/equip_icon_weapon_wide_shot";

		// Token: 0x040041C3 RID: 16835
		public static readonly Weapon id = Weapon.level_weapon_wide_shot;

		// Token: 0x020015BE RID: 5566
		public static class Basic
		{
			// Token: 0x04009115 RID: 37141
			public static readonly float damage = 2.67f;

			// Token: 0x04009116 RID: 37142
			public static readonly float speed = 1800f;

			// Token: 0x04009117 RID: 37143
			public static readonly float distance = 2000f;

			// Token: 0x04009118 RID: 37144
			public static readonly float rapidFireRate = 0.22f;

			// Token: 0x04009119 RID: 37145
			public static readonly MinMax angleRange = new MinMax(50f, 8f);

			// Token: 0x0400911A RID: 37146
			public static readonly float closingAngleSpeed = 1.1f;

			// Token: 0x0400911B RID: 37147
			public static readonly float openingAngleSpeed = 1.8f;

			// Token: 0x0400911C RID: 37148
			public static readonly float projectileSpeed = 2f;
		}

		// Token: 0x020015BF RID: 5567
		public static class Ex
		{
			// Token: 0x0400911D RID: 37149
			public static readonly float exDamage = 21f;

			// Token: 0x0400911E RID: 37150
			public static readonly float exDuration = 0.3f;

			// Token: 0x0400911F RID: 37151
			public static readonly float exHeight = 86.5f;
		}
	}

	// Token: 0x02000892 RID: 2194
	public static class PlaneSuperBomb
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x0003E94D File Offset: 0x0003CB4D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.plane_super_bomb);
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x0003E959 File Offset: 0x0003CB59
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.plane_super_bomb);
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x0003E965 File Offset: 0x0003CB65
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.plane_super_bomb);
			}
		}

		// Token: 0x040041C4 RID: 16836
		public static readonly int value = 10;

		// Token: 0x040041C5 RID: 16837
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041C6 RID: 16838
		public static readonly Super id = Super.plane_super_bomb;

		// Token: 0x040041C7 RID: 16839
		public static readonly float damage = 38f;

		// Token: 0x040041C8 RID: 16840
		public static readonly float damageRate = 0.25f;

		// Token: 0x040041C9 RID: 16841
		public static readonly float countdownTime = 3f;
	}

	// Token: 0x02000893 RID: 2195
	public static class PlaneSuperChaliceSuperBomb
	{
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x0003E9AC File Offset: 0x0003CBAC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x0003E9B8 File Offset: 0x0003CBB8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x040041CA RID: 16842
		public static readonly int value = 10;

		// Token: 0x040041CB RID: 16843
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041CC RID: 16844
		public static readonly Super id = Super.plane_super_chalice_bomb;

		// Token: 0x040041CD RID: 16845
		public static readonly float damage = 25.5f;

		// Token: 0x040041CE RID: 16846
		public static readonly float damageRate = 0.25f;

		// Token: 0x040041CF RID: 16847
		public static readonly float turnRate = 1f;

		// Token: 0x040041D0 RID: 16848
		public static readonly float maxAngle = 60f;

		// Token: 0x040041D1 RID: 16849
		public static readonly float angleDamp = 0.98f;

		// Token: 0x040041D2 RID: 16850
		public static readonly float accel = 600f;
	}

	// Token: 0x02000894 RID: 2196
	public static class PlaneWeaponBomb
	{
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x0003E9D0 File Offset: 0x0003CBD0
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x0003E9DC File Offset: 0x0003CBDC
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x040041D3 RID: 16851
		public static readonly int value = 2;

		// Token: 0x040041D4 RID: 16852
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041D5 RID: 16853
		public static readonly Weapon id = Weapon.plane_weapon_bomb;

		// Token: 0x020015C0 RID: 5568
		public static class Basic
		{
			// Token: 0x04009120 RID: 37152
			public static readonly float damage = 11.5f;

			// Token: 0x04009121 RID: 37153
			public static readonly float speed = 1200f;

			// Token: 0x04009122 RID: 37154
			public static readonly bool Up;

			// Token: 0x04009123 RID: 37155
			public static readonly float sizeExplosion = 1f;

			// Token: 0x04009124 RID: 37156
			public static readonly float size = 1f;

			// Token: 0x04009125 RID: 37157
			public static readonly float angle = 45f;

			// Token: 0x04009126 RID: 37158
			public static readonly float gravity = 4500f;

			// Token: 0x04009127 RID: 37159
			public static readonly bool rapidFire = true;

			// Token: 0x04009128 RID: 37160
			public static readonly float rapidFireRate = 0.6f;
		}

		// Token: 0x020015C1 RID: 5569
		public static class Ex
		{
			// Token: 0x04009129 RID: 37161
			public static readonly float damage = 6f;

			// Token: 0x0400912A RID: 37162
			public static readonly float speed = 700f;

			// Token: 0x0400912B RID: 37163
			public static readonly float[] angles = new float[]
			{
				180f,
				170f
			};

			// Token: 0x0400912C RID: 37164
			public static readonly int[] counts = new int[]
			{
				6,
				3
			};

			// Token: 0x0400912D RID: 37165
			public static readonly MinMax rotationSpeed = new MinMax(0f, 250f);

			// Token: 0x0400912E RID: 37166
			public static readonly float timeBeforeEaseRotationSpeed = 0f;

			// Token: 0x0400912F RID: 37167
			public static readonly float rotationSpeedEaseTime = 1f;

			// Token: 0x04009130 RID: 37168
			public static readonly float maxHomingTime = 2.5f;
		}
	}

	// Token: 0x02000895 RID: 2197
	public static class PlaneWeaponChaliceBomb
	{
		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x0003EA10 File Offset: 0x0003CC10
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x0003EA1C File Offset: 0x0003CC1C
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x0003EA28 File Offset: 0x0003CC28
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x040041D6 RID: 16854
		public static readonly int value = 10;

		// Token: 0x040041D7 RID: 16855
		public static readonly string iconPath = "Icons/equip_icon_chalice_shmup_bomb";

		// Token: 0x040041D8 RID: 16856
		public static readonly Weapon id = Weapon.plane_chalice_weapon_bomb;

		// Token: 0x020015C2 RID: 5570
		public static class Basic
		{
			// Token: 0x04009131 RID: 37169
			public static readonly float damage = 6.6f;

			// Token: 0x04009132 RID: 37170
			public static readonly float size = 1f;

			// Token: 0x04009133 RID: 37171
			public static readonly float sizeExplosion = 1f;

			// Token: 0x04009134 RID: 37172
			public static readonly float angleRange = 35f;

			// Token: 0x04009135 RID: 37173
			public static readonly float gravity = 1700f;

			// Token: 0x04009136 RID: 37174
			public static readonly float speed = 700f;

			// Token: 0x04009137 RID: 37175
			public static readonly bool rapidFire = true;

			// Token: 0x04009138 RID: 37176
			public static readonly float rapidFireRate = 0.2f;

			// Token: 0x04009139 RID: 37177
			public static readonly float damageExplosion = 2.5f;
		}

		// Token: 0x020015C3 RID: 5571
		public static class Ex
		{
			// Token: 0x0400913A RID: 37178
			public static readonly float damage = 15.5f;

			// Token: 0x0400913B RID: 37179
			public static readonly float damageRate = 0.17f;

			// Token: 0x0400913C RID: 37180
			public static readonly float damageRateIncrease = 0.07f;

			// Token: 0x0400913D RID: 37181
			public static readonly float startSpeed = 600f;

			// Token: 0x0400913E RID: 37182
			public static readonly float gravity = 1900f;

			// Token: 0x0400913F RID: 37183
			public static readonly float freezeTime = 0.125f;
		}
	}

	// Token: 0x02000896 RID: 2198
	public static class PlaneWeaponChaliceWay
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x0003EA51 File Offset: 0x0003CC51
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x0003EA5D File Offset: 0x0003CC5D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x0003EA69 File Offset: 0x0003CC69
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x040041D9 RID: 16857
		public static readonly int value = 10;

		// Token: 0x040041DA RID: 16858
		public static readonly string iconPath = "Icons/equip_icon_chalice_shmup_3way";

		// Token: 0x040041DB RID: 16859
		public static readonly Weapon id = Weapon.plane_chalice_weapon_3way;

		// Token: 0x020015C4 RID: 5572
		public static class Basic
		{
			// Token: 0x04009140 RID: 37184
			public static readonly float damage = 3.65f;

			// Token: 0x04009141 RID: 37185
			public static readonly float speed = 1650f;

			// Token: 0x04009142 RID: 37186
			public static readonly float distance;

			// Token: 0x04009143 RID: 37187
			public static readonly float rapidFireRate = 0.23f;

			// Token: 0x04009144 RID: 37188
			public static readonly float angle = 9f;
		}

		// Token: 0x020015C5 RID: 5573
		public static class Ex
		{
			// Token: 0x04009145 RID: 37189
			public static readonly float damageBeforeLaunch = 2.4f;

			// Token: 0x04009146 RID: 37190
			public static readonly float damageRateBeforeLaunch = 0.25f;

			// Token: 0x04009147 RID: 37191
			public static readonly float arcSpeed = 5f;

			// Token: 0x04009148 RID: 37192
			public static readonly float arcX = 250f;

			// Token: 0x04009149 RID: 37193
			public static readonly float arcY = 40f;

			// Token: 0x0400914A RID: 37194
			public static readonly float pauseTime;

			// Token: 0x0400914B RID: 37195
			public static readonly float damageAfterLaunch = 17f;

			// Token: 0x0400914C RID: 37196
			public static readonly float speedAfterLaunch = -1250f;

			// Token: 0x0400914D RID: 37197
			public static readonly float accelAfterLaunch = 8000f;

			// Token: 0x0400914E RID: 37198
			public static readonly float freezeTime = 0.125f;

			// Token: 0x0400914F RID: 37199
			public static readonly float minXDistance = 75f;

			// Token: 0x04009150 RID: 37200
			public static readonly int xDistanceNoTarget = 500;
		}
	}

	// Token: 0x02000897 RID: 2199
	public static class PlaneWeaponLaser
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x0003EA92 File Offset: 0x0003CC92
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x0003EA9E File Offset: 0x0003CC9E
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x0003EAAA File Offset: 0x0003CCAA
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x040041DC RID: 16860
		public static readonly int value = 2;

		// Token: 0x040041DD RID: 16861
		public static readonly string iconPath = "Icons/";

		// Token: 0x040041DE RID: 16862
		public static readonly Weapon id = Weapon.plane_weapon_laser;

		// Token: 0x020015C6 RID: 5574
		public static class Basic
		{
			// Token: 0x04009151 RID: 37201
			public static readonly float damage = 8f;

			// Token: 0x04009152 RID: 37202
			public static readonly float speed = 4000f;

			// Token: 0x04009153 RID: 37203
			public static readonly bool rapidFire = true;

			// Token: 0x04009154 RID: 37204
			public static readonly float rapidFireRate = 0.1f;
		}

		// Token: 0x020015C7 RID: 5575
		public static class Ex
		{
			// Token: 0x04009155 RID: 37205
			public static readonly float damage = 3f;

			// Token: 0x04009156 RID: 37206
			public static readonly float speed = 2000f;

			// Token: 0x04009157 RID: 37207
			public static readonly float[] angles = new float[]
			{
				180f,
				170f
			};

			// Token: 0x04009158 RID: 37208
			public static readonly int[] counts = new int[]
			{
				12,
				6
			};
		}
	}

	// Token: 0x02000898 RID: 2200
	public static class PlaneWeaponPeashot
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060051A5 RID: 20901 RVA: 0x0003EAD2 File Offset: 0x0003CCD2
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060051A6 RID: 20902 RVA: 0x0003EADE File Offset: 0x0003CCDE
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060051A7 RID: 20903 RVA: 0x0003EAEA File Offset: 0x0003CCEA
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x040041DF RID: 16863
		public static readonly int value = 2;

		// Token: 0x040041E0 RID: 16864
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x040041E1 RID: 16865
		public static readonly Weapon id = Weapon.plane_weapon_peashot;

		// Token: 0x020015C8 RID: 5576
		public static class Basic
		{
			// Token: 0x04009159 RID: 37209
			public static readonly float damage = 4f;

			// Token: 0x0400915A RID: 37210
			public static readonly float speed = 1800f;

			// Token: 0x0400915B RID: 37211
			public static readonly bool rapidFire = true;

			// Token: 0x0400915C RID: 37212
			public static readonly float rapidFireRate = 0.07f;
		}

		// Token: 0x020015C9 RID: 5577
		public static class Ex
		{
			// Token: 0x0400915D RID: 37213
			public static readonly float damage = 15f;

			// Token: 0x0400915E RID: 37214
			public static readonly float damageDistance = 100f;

			// Token: 0x0400915F RID: 37215
			public static readonly float acceleration = 2500f;

			// Token: 0x04009160 RID: 37216
			public static readonly float maxSpeed = 1500f;

			// Token: 0x04009161 RID: 37217
			public static readonly float freezeTime = 0.125f;
		}
	}
}
