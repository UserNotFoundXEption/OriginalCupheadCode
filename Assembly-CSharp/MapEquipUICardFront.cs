using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C6 RID: 1222
public class MapEquipUICardFront : AbstractMapEquipUICardSide
{
	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060032B4 RID: 12980 RVA: 0x0002A14A File Offset: 0x0002834A
	public MapEquipUICard.Slot Slot
	{
		get
		{
			return (MapEquipUICard.Slot)this.index;
		}
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x0002A152 File Offset: 0x00028352
	public void Update()
	{
		this.SetCursorPosition(this.index);
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x0002A160 File Offset: 0x00028360
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x0002A173 File Offset: 0x00028373
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x0002A186 File Offset: 0x00028386
	public void OnLanguageChanged()
	{
		this.ChangeSelection(0);
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x000EEA5C File Offset: 0x000ECC5C
	public override void Init(PlayerId playerID)
	{
		base.Init(playerID);
		this.icons = new MapEquipUICardFrontIcon[]
		{
			this.weaponA,
			this.weaponB,
			this.super,
			this.item,
			this.checklist
		};
		this.checklist.SetIconsManual("Icons/equip_icon_list", false, false);
		this.checkListSelected = false;
		this.Refresh();
		this.ChangeSelection(0);
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000EEAD0 File Offset: 0x000ECCD0
	public void Refresh()
	{
		this.loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		this.weaponA.SetIcons(this.loadout.primaryWeapon, false);
		this.weaponB.SetIcons(this.loadout.secondaryWeapon, false);
		this.super.SetIcons(this.loadout.super, false);
		if (this.loadout.charm == Charm.charm_curse)
		{
			this.item.SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(base.playerID) + 1).ToString(), false, true);
		}
		else
		{
			this.item.SetIcons(this.loadout.charm, false);
		}
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x000EEBA0 File Offset: 0x000ECDA0
	public void Unequip()
	{
		if (this.icons[this.index] != this.weaponA)
		{
			this.icons[this.index].SetIcons(WeaponProperties.GetIconPath(Weapon.None));
			if (this.icons[this.index] == this.weaponB)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = Weapon.None;
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = false;
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = false;
				}
			}
			else if (this.icons[this.index] == this.super)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = Super.None;
			}
			else if (this.icons[this.index] == this.item)
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
				{
					PlayerManager.OnChaliceCharmUnequipped(base.playerID);
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = Charm.None;
			}
			else
			{
				Debug.LogError("Something went wrong", null);
			}
		}
		else
		{
			AudioManager.Play("menu_locked");
			this.cursor.OnLocked();
		}
		this.Refresh();
		this.ChangeSelection(0);
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x000EED60 File Offset: 0x000ECF60
	public void ChangeSelection(int direction)
	{
		if ((this.index != this.icons.Length - 1 && direction != -1) || (this.index != 0 && direction != 1))
		{
			AudioManager.Play("menu_equipment_move");
		}
		this.index = Mathf.Clamp(this.index + direction, 0, this.icons.Length - 1);
		this.SetCursorPosition(this.index);
		this.checkListSelected = (this.index == this.icons.Length - 1);
		string text = string.Empty;
		if (this.icons[this.index] == this.weaponA)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.primaryWeapon);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_weapon_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.weaponB)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.secondaryWeapon);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_weapon_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.super)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.super);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_super_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.item)
		{
			if (this.loadout.charm == Charm.charm_curse)
			{
				if (CharmCurse.CalculateLevel(base.playerID) == -1)
				{
					text = Localization.Translate("charm_broken_name").text;
				}
				else if (CharmCurse.IsMaxLevel(base.playerID))
				{
					text = Localization.Translate("charm_paladin_name").text;
				}
				else
				{
					text = Localization.Translate("charm_curse_name").text;
				}
			}
			else
			{
				text = WeaponProperties.GetDisplayName(this.loadout.charm);
			}
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("charm_none_name").text;
			}
			this.title.text = text;
		}
		else
		{
			this.title.text = Localization.Translate("list_name").text;
		}
		this.title.font = Localization.Instance.fonts[(int)Localization.language][9].font;
		foreach (Outline outline in this.outlines)
		{
			outline.enabled = (Localization.language == Localization.Languages.Japanese);
		}
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0002A18F File Offset: 0x0002838F
	public void SetCursorPosition(int index)
	{
		if (this.icons == null || this.icons.Length <= index)
		{
			return;
		}
		this.cursor.SetPosition(this.icons[index].transform.position);
	}

	// Token: 0x0400297C RID: 10620
	public MapEquipUICardFrontIcon weaponA;

	// Token: 0x0400297D RID: 10621
	public MapEquipUICardFrontIcon weaponB;

	// Token: 0x0400297E RID: 10622
	public MapEquipUICardFrontIcon super;

	// Token: 0x0400297F RID: 10623
	public MapEquipUICardFrontIcon item;

	// Token: 0x04002980 RID: 10624
	public MapEquipUICardFrontIcon checklist;

	// Token: 0x04002981 RID: 10625
	public bool checkListSelected;

	// Token: 0x04002982 RID: 10626
	[Space(10f)]
	public MapEquipUICursor cursor;

	// Token: 0x04002983 RID: 10627
	public int index;

	// Token: 0x04002984 RID: 10628
	public MapEquipUICardFrontIcon[] icons;

	// Token: 0x04002985 RID: 10629
	public Text title;

	// Token: 0x04002986 RID: 10630
	public Outline[] outlines;

	// Token: 0x04002987 RID: 10631
	public PlayerData.PlayerLoadouts.PlayerLoadout loadout;
}
