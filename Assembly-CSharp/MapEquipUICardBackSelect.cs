using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C2 RID: 1218
public class MapEquipUICardBackSelect : AbstractMapEquipUICardSide
{
	// Token: 0x0600329B RID: 12955 RVA: 0x0002A05F File Offset: 0x0002825F
	public void ChangeSelection(Trilean2 direction)
	{
		this.index = this.selectedIcons[this.index].GetIndexOfNeighbor(direction);
		this.SetCursorPosition(this.index);
		this.UpdateText();
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x000ED5C0 File Offset: 0x000EB7C0
	public void ChangeSlot(int direction)
	{
		this.cursor.Show();
		int num = (int)this.slot;
		this.slot = (MapEquipUICard.Slot)Mathf.Repeat((float)(num + direction), (float)EnumUtils.GetCount<MapEquipUICard.Slot>());
		this.Setup(this.slot);
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x000ED604 File Offset: 0x000EB804
	public void UpdateText()
	{
		bool flag = false;
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
		case MapEquipUICard.Slot.SHOT_B:
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]);
			this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.WEAPONS[this.index]).ToUpper());
			this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(MapEquipUICardBackSelect.WEAPONS[this.index]));
			this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(MapEquipUICardBackSelect.WEAPONS[this.index]));
			break;
		case MapEquipUICard.Slot.SUPER:
		{
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]);
			PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
			Super super = (playerLoadout.charm != Charm.charm_chalice) ? MapEquipUICardBackSelect.SUPERS[this.index] : MapEquipUICardBackSelect.CHALICESUPERS[this.index];
			this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.SUPERS[this.index]).ToUpper());
			this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(super));
			this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(super));
			break;
		}
		case MapEquipUICard.Slot.CHARM:
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]);
			if (MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse && CharmCurse.IsMaxLevel(base.playerID))
			{
				this.titleText.text = Localization.Translate("charm_paladin_name").text;
				this.exText.text = Localization.Translate("charm_paladin_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_paladin_description").text;
			}
			else if (flag && MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse && (CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 || CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
			{
				this.titleText.text = Localization.Translate("charm_curse_name").text;
				this.exText.text = Localization.Translate("charm_curse_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_curse_description").text;
			}
			else if (flag && MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse)
			{
				this.titleText.text = Localization.Translate("charm_broken_name").text;
				this.exText.text = Localization.Translate("charm_broken_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_broken_description").text;
			}
			else
			{
				this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.CHARMS[this.index]).ToUpper());
				this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(MapEquipUICardBackSelect.CHARMS[this.index]));
				this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(MapEquipUICardBackSelect.CHARMS[this.index]));
			}
			break;
		}
		this.titleText.font = Localization.Instance.fonts[(int)Localization.language][9].font;
		if (flag)
		{
			this.exText.font = Localization.Instance.fonts[(int)Localization.language][10].font;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][11].fontAsset;
		}
		else
		{
			this.exText.font = Localization.Instance.fonts[(int)Localization.language1][10].font;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language1][11].fontAsset;
		}
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x000EDB0C File Offset: 0x000EBD0C
	public void SetCursorPosition(int index)
	{
		if (this.lastIndex != index)
		{
			AudioManager.Play("menu_equipment_move");
			this.lastIndex = index;
		}
		this.cursor.SetPosition(this.selectedIcons[index].transform.position);
		if (!this.noneUnlocked && this.itemSelected)
		{
			this.selectionCursor.Show();
		}
		else
		{
			this.selectionCursor.Hide();
		}
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x000EDB84 File Offset: 0x000EBD84
	public void Setup(MapEquipUICard.Slot slot)
	{
		this.slot = slot;
		this.headerText.ApplyTranslation(Localization.Find(MapEquipUICardBackSelect.slotLocalesKey[(int)slot]), null);
		bool flag = slot == MapEquipUICard.Slot.SUPER;
		bool flag2 = DLCManager.DLCEnabled();
		this.selectedIcons = ((!flag) ? ((!flag2) ? this.normalIcons : this.DLCIcons) : this.superIcons);
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon in this.superIcons)
		{
			mapEquipUICardBackSelectIcon.gameObject.SetActive(flag);
		}
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon2 in this.normalIcons)
		{
			mapEquipUICardBackSelectIcon2.gameObject.SetActive(!flag && !flag2);
		}
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon3 in this.DLCIcons)
		{
			mapEquipUICardBackSelectIcon3.gameObject.SetActive(!flag && flag2);
		}
		this.superIconsBack.enabled = flag;
		this.iconsBack.enabled = (!flag && !flag2);
		this.DLCIconsBack.enabled = (!flag && flag2);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		this.selectionCursor.Hide();
		this.noneUnlocked = true;
		this.index = -1;
		bool isGrey = false;
		this.itemSelected = false;
		for (int l = 0; l < this.selectedIcons.Length; l++)
		{
			this.selectedIcons[l].Index = l;
			string str = "_000" + (l % 8 + 1).ToStringInvariant();
			string iconPath = Weapon.None.ToString();
			switch (slot)
			{
			case MapEquipUICard.Slot.SHOT_A:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[l]))
				{
					isGrey = (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.secondaryWeapon);
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.WEAPONS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Weapon.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.primaryWeapon && playerLoadout.primaryWeapon != Weapon.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.SHOT_B:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[l]))
				{
					isGrey = (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.primaryWeapon);
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.WEAPONS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Weapon.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.secondaryWeapon && playerLoadout.secondaryWeapon != Weapon.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.SUPER:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[l]))
				{
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.SUPERS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Super.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (playerLoadout.super != Super.None && (MapEquipUICardBackSelect.SUPERS[l] == playerLoadout.super || (playerLoadout.charm == Charm.charm_chalice && MapEquipUICardBackSelect.CHALICESUPERS[l] == playerLoadout.super)))
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.CHARM:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[l]))
				{
					this.noneUnlocked = false;
					if (MapEquipUICardBackSelect.CHARMS[l] == Charm.charm_curse)
					{
						this.selectedIcons[l].SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(base.playerID) + 1).ToString(), false, true);
					}
					else
					{
						this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.CHARMS[l], isGrey);
					}
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Charm.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.CHARMS[l] == playerLoadout.charm && playerLoadout.charm != Charm.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			}
			if (this.index == -1)
			{
				this.index = 0;
			}
			this.cursor.SetPosition(this.selectedIcons[this.index].transform.position);
			this.UpdateText();
		}
		this.selectionCursor.selectedIndex = -1;
		if (!this.noneUnlocked && this.itemSelected)
		{
			if (slot != this.lastSlot)
			{
				this.selectionCursor.Show();
				this.selectionCursor.selectedIndex = this.index;
				this.selectionCursor.SetPosition(this.selectedIcons[this.index].transform.position);
			}
			else
			{
				base.StartCoroutine(this.set_selection_cursor());
			}
			this.cursor.SelectIcon(true);
		}
		this.lastSlot = slot;
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x000EE188 File Offset: 0x000EC388
	public IEnumerator set_selection_cursor()
	{
		base.StartCoroutine(this.lock_input_cr());
		while (!this.selectionCursor.animator.GetCurrentAnimatorStateInfo(0).IsName("Off") && this.lockInput)
		{
			yield return null;
		}
		this.selectionCursor.Show();
		this.selectionCursor.selectedIndex = this.index;
		this.selectionCursor.SetPosition(this.selectedIcons[this.index].transform.position);
		yield return null;
		yield break;
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x000EE1A4 File Offset: 0x000EC3A4
	public IEnumerator lock_input_cr()
	{
		this.lockInput = true;
		yield return new WaitForSeconds(0.2f);
		this.lockInput = false;
		yield break;
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000EE1C0 File Offset: 0x000EC3C0
	public void Accept()
	{
		AudioManager.Play("menu_equipment_equip");
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) && (playerLoadout.primaryWeapon != MapEquipUICardBackSelect.WEAPONS[this.index] || playerLoadout.secondaryWeapon != Weapon.None))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.SUPER:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.CHARM:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]))
			{
				this.Selection();
			}
			break;
		}
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			if (MapEquipUICardBackSelect.WEAPONS[this.index] == Weapon.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]))
			{
				this.OnLocked();
				return;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index])
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon = MapEquipUICardBackSelect.WEAPONS[this.index];
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (MapEquipUICardBackSelect.WEAPONS[this.index] == Weapon.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) || (playerLoadout.primaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index] && playerLoadout.secondaryWeapon == Weapon.None))
			{
				this.OnLocked();
				return;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index])
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = MapEquipUICardBackSelect.WEAPONS[this.index];
			if (!PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = true;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = true;
			break;
		case MapEquipUICard.Slot.SUPER:
			if (MapEquipUICardBackSelect.SUPERS[this.index] == Super.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]))
			{
				this.OnLocked();
				return;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = MapEquipUICardBackSelect.SUPERS[this.index];
			break;
		case MapEquipUICard.Slot.CHARM:
			if (MapEquipUICardBackSelect.CHARMS[this.index] == Charm.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]))
			{
				this.OnLocked();
				return;
			}
			if (MapEquipUICardBackSelect.CHARMS[this.index] != Charm.charm_chalice && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
			{
				PlayerManager.OnChaliceCharmUnequipped(base.playerID);
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = MapEquipUICardBackSelect.CHARMS[this.index];
			break;
		}
		this.Setup(this.slot);
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000EE63C File Offset: 0x000EC83C
	public void Unequip()
	{
		AudioManager.Play("menu_equipment_equip");
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			this.OnLocked();
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon != Weapon.None)
			{
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = Weapon.None;
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = false;
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = false;
				}
			}
			else
			{
				this.OnLocked();
			}
			break;
		case MapEquipUICard.Slot.SUPER:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super != Super.None)
			{
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = Super.None;
			}
			else
			{
				this.OnLocked();
			}
			break;
		case MapEquipUICard.Slot.CHARM:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm != Charm.None)
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
				{
					PlayerManager.OnChaliceCharmUnequipped(base.playerID);
				}
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = Charm.None;
			}
			else
			{
				this.OnLocked();
			}
			break;
		}
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000EE874 File Offset: 0x000ECA74
	public void Selection()
	{
		this.selectedIcons[this.index].SelectIcon();
		if (this.selectionCursor.selectedIndex >= 0)
		{
			this.selectedIcons[this.selectionCursor.selectedIndex].UnselectIcon();
		}
		if (this.cursor.transform.position != this.selectionCursor.transform.position)
		{
			this.selectionCursor.selectedIndex = this.index;
			this.selectionCursor.Select();
			this.cursor.SelectIcon(false);
		}
		else
		{
			this.cursor.SelectIcon(true);
		}
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x0002A08C File Offset: 0x0002828C
	public void Deselect()
	{
		base.StartCoroutine(this.remove_selection_cr());
		this.itemSelected = false;
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x000EE920 File Offset: 0x000ECB20
	public IEnumerator remove_selection_cr()
	{
		this.selectionCursor.Select();
		yield return this.selectionCursor.animator.WaitForAnimationToEnd(this, "Select", false, true);
		this.selectionCursor.Hide();
		yield return null;
		yield break;
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x0002A0A2 File Offset: 0x000282A2
	public void OnLocked()
	{
		AudioManager.Play("menu_locked");
		this.selectedIcons[this.index].OnLocked();
		this.cursor.OnLocked();
	}

	// Token: 0x0400295C RID: 10588
	public static readonly Weapon[] WEAPONS = new Weapon[]
	{
		Weapon.level_weapon_peashot,
		Weapon.level_weapon_spreadshot,
		Weapon.level_weapon_homing,
		Weapon.level_weapon_bouncer,
		Weapon.level_weapon_charge,
		Weapon.level_weapon_boomerang,
		Weapon.level_weapon_crackshot,
		Weapon.level_weapon_wide_shot,
		Weapon.level_weapon_upshot
	};

	// Token: 0x0400295D RID: 10589
	public static readonly Super[] SUPERS = new Super[]
	{
		Super.level_super_beam,
		Super.level_super_invincible,
		Super.level_super_ghost
	};

	// Token: 0x0400295E RID: 10590
	public static readonly Super[] CHALICESUPERS = new Super[]
	{
		Super.level_super_chalice_vert_beam,
		Super.level_super_chalice_shield,
		Super.level_super_chalice_iii
	};

	// Token: 0x0400295F RID: 10591
	public static readonly Charm[] CHARMS = new Charm[]
	{
		Charm.charm_health_up_1,
		Charm.charm_super_builder,
		Charm.charm_smoke_dash,
		Charm.charm_parry_plus,
		Charm.charm_health_up_2,
		Charm.charm_parry_attack,
		Charm.charm_chalice,
		Charm.charm_curse,
		Charm.charm_healer
	};

	// Token: 0x04002960 RID: 10592
	public static readonly string[] slotLocalesKey = new string[]
	{
		"ShotABackTitle",
		"ShotBBackTitle",
		"SuperBackTitle",
		"CharmBackTitle"
	};

	// Token: 0x04002961 RID: 10593
	public bool lockInput;

	// Token: 0x04002962 RID: 10594
	[Header("Text")]
	[SerializeField]
	public LocalizationHelper headerText;

	// Token: 0x04002963 RID: 10595
	[SerializeField]
	public Text titleText;

	// Token: 0x04002964 RID: 10596
	[SerializeField]
	public Text exText;

	// Token: 0x04002965 RID: 10597
	[SerializeField]
	public TMP_Text descriptionText;

	// Token: 0x04002966 RID: 10598
	[Header("Cursors")]
	[SerializeField]
	public MapEquipUICursor cursor;

	// Token: 0x04002967 RID: 10599
	[SerializeField]
	public MapEquipUICardBackSelectSelectionCursor selectionCursor;

	// Token: 0x04002968 RID: 10600
	[Header("Backs")]
	[SerializeField]
	public Image iconsBack;

	// Token: 0x04002969 RID: 10601
	[SerializeField]
	public Image superIconsBack;

	// Token: 0x0400296A RID: 10602
	[SerializeField]
	public Image DLCIconsBack;

	// Token: 0x0400296B RID: 10603
	[Header("Icons")]
	[SerializeField]
	public MapEquipUICardBackSelectIcon[] normalIcons;

	// Token: 0x0400296C RID: 10604
	[Header("Super Icons")]
	[SerializeField]
	public MapEquipUICardBackSelectIcon[] superIcons;

	// Token: 0x0400296D RID: 10605
	[Header("DLC Icons")]
	[SerializeField]
	public MapEquipUICardBackSelectIcon[] DLCIcons;

	// Token: 0x0400296E RID: 10606
	public int index;

	// Token: 0x0400296F RID: 10607
	public int lastIndex;

	// Token: 0x04002970 RID: 10608
	public MapEquipUICard.Slot slot;

	// Token: 0x04002971 RID: 10609
	public MapEquipUICard.Slot lastSlot;

	// Token: 0x04002972 RID: 10610
	public MapEquipUICardBackSelectIcon[] selectedIcons;

	// Token: 0x04002973 RID: 10611
	public bool noneUnlocked;

	// Token: 0x04002974 RID: 10612
	public bool itemSelected;
}
