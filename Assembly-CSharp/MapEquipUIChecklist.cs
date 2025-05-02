using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class MapEquipUIChecklist : AbstractMapEquipUICardSide
{
	// Token: 0x060032C0 RID: 12992 RVA: 0x000EF180 File Offset: 0x000ED380
	public override void Init(PlayerId playerID)
	{
		base.Init(playerID);
		this.darkText = new Color(0.2f, 0.188f, 0.188f);
		this.lightText = new Color(0.827f, 0.765f, 0.702f);
		this.disabledText = new Color(0.537f, 0.498f, 0.463f);
		this.selectableLength = this.worldSelectionIcons.Length;
		for (int i = 0; i < this.worldSelectionIcons.Length; i++)
		{
			this.worldSelectionIcons[i].SetIcons("Icons/" + this.worldPaths[i] + "_dark");
			this.worldSelectionIcons[i].SetTextColor(this.darkText);
		}
		this.worldSelectionIcons[this.index].SetIcons("Icons/" + this.worldPaths[this.index] + "_light");
		this.worldSelectionIcons[this.index].SetTextColor(this.lightText);
		if (!PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 2].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 3].SetTextColor(this.disabledText);
			this.selectableLength -= 3;
		}
		else if (!PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 2].SetTextColor(this.disabledText);
			this.selectableLength -= 2;
		}
		else if (!PlayerData.Data.CheckLevelsCompleted(Level.world3BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.selectableLength--;
		}
		this.UpdateList();
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x0002A1D0 File Offset: 0x000283D0
	public void SetArrow(bool showRight)
	{
		this.rightArrow.SetActive(showRight);
		this.leftArrow.SetActive(!showRight);
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x000EF3A4 File Offset: 0x000ED5A4
	public void ChangeSelection(int direction)
	{
		this.index = Mathf.Clamp(this.index + direction, 0, this.selectableLength - 1);
		bool flag = false;
		this.skippedOver = false;
		if (this.showDLCMenu)
		{
			if (this.selectableLength < this.worldSelectionIcons.Length)
			{
				flag = true;
				if (this.DLCIndex == this.worldNames.Length - 1 && direction < 0)
				{
					this.DLCIndex = this.selectableLength - 1;
					this.skippedOver = true;
					this.SetArrow(true);
				}
				else if (this.DLCIndex + direction > this.selectableLength - 1)
				{
					this.DLCIndex = this.worldNames.Length - 1;
					this.skippedOver = true;
					this.SetArrow(false);
				}
				else if (this.DLCIndex + direction < 0)
				{
					this.DLCIndex = 0;
					this.SetArrow(true);
				}
				else
				{
					this.DLCIndex += direction;
				}
			}
			else if (this.DLCIndex + direction < 0)
			{
				this.DLCIndex = 0;
				this.SetArrow(true);
			}
			else if (this.DLCIndex + direction > this.worldNames.Length - 1)
			{
				this.DLCIndex = this.worldNames.Length - 1;
				this.SetArrow(false);
			}
			else
			{
				this.DLCIndex += direction;
			}
			this.ChangeDLCMenu(this.index, this.lastIndex);
		}
		if (flag)
		{
			int num = (this.DLCIndex != this.worldNames.Length - 1) ? this.index : (this.worldSelectionIcons.Length - 1);
			this.SetCursorPosition(num, false);
		}
		else
		{
			this.SetCursorPosition(this.index, false);
		}
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x000EF560 File Offset: 0x000ED760
	public void SetCursorPosition(int index, bool openingChecklist)
	{
		if (openingChecklist)
		{
			this.showDLCMenu = ((DLCManager.DLCEnabled() && PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted) || this.editorShowDLCChecklist);
			if (index >= this.worldNames.Length - 1)
			{
				this.DLCIndex = index;
				index = this.worldSelectionIcons.Length - 1;
			}
			else
			{
				this.DLCIndex = index;
			}
		}
		this.index = index;
		if (this.lastIndex != index)
		{
			this.worldSelectionIcons[index].SetIcons("Icons/" + this.worldPaths[index] + "_light");
			this.worldSelectionIcons[index].SetTextColor(new Color(0.827f, 0.765f, 0.702f));
			if (!this.skippedOver)
			{
				this.worldSelectionIcons[this.lastIndex].SetIcons("Icons/" + this.worldPaths[this.lastIndex] + "_dark");
				this.worldSelectionIcons[this.lastIndex].SetTextColor(new Color(0.2f, 0.188f, 0.188f));
			}
			AudioManager.Play("menu_equipment_move");
			this.lastIndex = index;
		}
		if (this.showDLCMenu)
		{
			if (openingChecklist)
			{
				this.skippedOver = true;
				if (this.DLCIndex < this.worldNames.Length - 1)
				{
					this.SetArrow(true);
				}
				this.ChangeDLCMenu(index, this.lastIndex);
			}
			if (this.DLCIndex == 0)
			{
				this.SetArrow(true);
			}
			else if (this.DLCIndex == this.worldNames.Length - 1)
			{
				this.SetArrow(false);
			}
		}
		this.cursor.SetPosition(this.worldSelectionIcons[index].transform.position);
		this.UpdateList();
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x000EF730 File Offset: 0x000ED930
	public void ChangeDLCMenu(int index, int lastIndex)
	{
		if ((index == lastIndex && (index <= 0 || index >= this.selectableLength - 1)) || this.skippedOver)
		{
			int num = (this.DLCIndex != this.worldNames.Length - 1) ? 0 : 1;
			int num2 = 0;
			bool flag = index >= this.worldSelectionIcons.Length - 1;
			for (int i = 0; i < this.worldSelectionIcons.Length; i++)
			{
				TranslationElement translationElement = Localization.Find(this.worldNames[num].ToString());
				this.worldSelectionIcons[num2].iconText.text = translationElement.translation.text;
				int num3 = (this.DLCIndex != this.worldNames.Length - 1) ? 0 : 1;
				if (i > this.selectableLength - 1 - num3 && (this.DLCIndex != this.worldNames.Length - 1 || i != this.worldSelectionIcons.Length - 1))
				{
					this.worldSelectionIcons[i].SetTextColor(this.disabledText);
				}
				num = (num + 1) % this.worldNames.Length;
				num2 = (num2 + 1) % this.worldSelectionIcons.Length;
			}
		}
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x000EF86C File Offset: 0x000EDA6C
	public void UpdateList()
	{
		List<Levels> list = new List<Levels>();
		List<string> list2 = new List<string>();
		list.Clear();
		list2.Clear();
		for (int i = 0; i < this.checklistItems.Count; i++)
		{
			this.checklistItems[i].gameObject.SetActive(false);
			this.checklistItems[i].ClearDescription(this.selectedFinale);
		}
		for (int j = 0; j < this.finaleItems.Count; j++)
		{
			this.finaleItems[j].gameObject.SetActive(false);
			if (this.finaleItems[j].checkMark != null)
			{
				this.finaleItems[j].checkMark.enabled = false;
				this.finaleItems[j].ClearDescription(this.selectedFinale);
			}
		}
		bool flag = false;
		switch ((!this.showDLCMenu) ? this.index : this.DLCIndex)
		{
		case 0:
			list.AddRange(this.world1Levels);
			this.selectedFinale = false;
			break;
		case 1:
			list.AddRange(this.world2Levels);
			this.selectedFinale = false;
			break;
		case 2:
			list.AddRange(this.world3Levels);
			this.selectedFinale = false;
			break;
		case 3:
			list.AddRange(this.finaleLevels);
			this.selectedFinale = true;
			break;
		case 4:
			list.AddRange(this.DLClevels);
			this.selectedFinale = false;
			flag = true;
			break;
		}
		foreach (Levels level in list)
		{
			list2.Add(Level.GetLevelName(level).Replace("\\n", " "));
		}
		this.worldTop.SetActive(false);
		this.finaleTop.SetActive(false);
		this.localizedTop.SetActive(true);
		this.worldTopLocalized.SetActive(!this.selectedFinale && !flag);
		this.finaleTopLocalized.SetActive(this.selectedFinale);
		this.worldTopDLCLocalized.SetActive(flag);
		this.finaleGrid.SetActive(this.selectedFinale);
		bool played = PlayerData.Data.GetLevelData(Levels.Saltbaker).played;
		for (int k = 0; k < list2.Count; k++)
		{
			if (flag)
			{
				if (k != list2.Count - 1 || (k == list2.Count - 1 && played))
				{
					this.checklistItems[k].gameObject.SetActive(true);
					this.checklistItems[k].EnableCheckbox(k < list2.Count - 1);
					this.checklistItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
				}
			}
			else if (!this.selectedFinale)
			{
				this.checklistItems[k].gameObject.SetActive(true);
				this.checklistItems[k].EnableCheckbox(k < list2.Count - 2);
				this.checklistItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
			}
			else
			{
				this.finaleItems[k].gameObject.SetActive(true);
				this.finaleItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
			}
		}
	}

	// Token: 0x04002988 RID: 10632
	public readonly string[] worldPaths = new string[]
	{
		"equip_checklist_world_1",
		"equip_checklist_world_2",
		"equip_checklist_world_3",
		"equip_checklist_finale"
	};

	// Token: 0x04002989 RID: 10633
	public readonly Levels[] world1Levels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs,
		Levels.Platforming_Level_1_1,
		Levels.Platforming_Level_1_2
	};

	// Token: 0x0400298A RID: 10634
	public readonly Levels[] world2Levels = new Levels[]
	{
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird,
		Levels.Platforming_Level_2_1,
		Levels.Platforming_Level_2_2
	};

	// Token: 0x0400298B RID: 10635
	public readonly Levels[] world3Levels = new Levels[]
	{
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train,
		Levels.Platforming_Level_3_1,
		Levels.Platforming_Level_3_2
	};

	// Token: 0x0400298C RID: 10636
	public readonly Levels[] finaleLevels = new Levels[]
	{
		Levels.DicePalaceBooze,
		Levels.DicePalaceChips,
		Levels.DicePalaceCigar,
		Levels.DicePalaceDomino,
		Levels.DicePalaceEightBall,
		Levels.DicePalaceFlyingHorse,
		Levels.DicePalaceFlyingMemory,
		Levels.DicePalaceRabbit,
		Levels.DicePalaceRoulette,
		Levels.DicePalaceMain,
		Levels.Devil
	};

	// Token: 0x0400298D RID: 10637
	public readonly Levels[] DLClevels = new Levels[]
	{
		Levels.OldMan,
		Levels.RumRunners,
		Levels.Airplane,
		Levels.SnowCult,
		Levels.FlyingCowboy,
		Levels.Saltbaker
	};

	// Token: 0x0400298E RID: 10638
	public readonly string[] worldNames = new string[]
	{
		"CheckListWorld1",
		"CheckListWorld2",
		"CheckListWorld3",
		"CheckListFinale",
		"ChecklistDLC"
	};

	// Token: 0x0400298F RID: 10639
	[Header("Headers")]
	[SerializeField]
	public GameObject worldTop;

	// Token: 0x04002990 RID: 10640
	[SerializeField]
	public GameObject finaleTop;

	// Token: 0x04002991 RID: 10641
	[SerializeField]
	public GameObject localizedTop;

	// Token: 0x04002992 RID: 10642
	[SerializeField]
	public GameObject worldTopLocalized;

	// Token: 0x04002993 RID: 10643
	[SerializeField]
	public GameObject worldTopDLCLocalized;

	// Token: 0x04002994 RID: 10644
	[SerializeField]
	public GameObject finaleTopLocalized;

	// Token: 0x04002995 RID: 10645
	[Header("Cursors")]
	[SerializeField]
	public MapEquipUICursor cursor;

	// Token: 0x04002996 RID: 10646
	[Header("Icons")]
	[SerializeField]
	public MapEquipUICardChecklistIcon[] worldSelectionIcons;

	// Token: 0x04002997 RID: 10647
	[Header("Bosses + Platforming items")]
	[SerializeField]
	public List<MapEquipUIChecklistItem> checklistItems;

	// Token: 0x04002998 RID: 10648
	[SerializeField]
	public List<MapEquipUIChecklistItem> finaleItems;

	// Token: 0x04002999 RID: 10649
	[SerializeField]
	public GameObject finaleGrid;

	// Token: 0x0400299A RID: 10650
	[SerializeField]
	public GameObject rightArrow;

	// Token: 0x0400299B RID: 10651
	[SerializeField]
	public GameObject leftArrow;

	// Token: 0x0400299C RID: 10652
	public int index;

	// Token: 0x0400299D RID: 10653
	public int lastIndex;

	// Token: 0x0400299E RID: 10654
	public int DLCIndex;

	// Token: 0x0400299F RID: 10655
	public bool selectedFinale;

	// Token: 0x040029A0 RID: 10656
	public bool showDLCMenu;

	// Token: 0x040029A1 RID: 10657
	public bool skippedOver;

	// Token: 0x040029A2 RID: 10658
	public bool editorShowDLCChecklist;

	// Token: 0x040029A3 RID: 10659
	public Color darkText;

	// Token: 0x040029A4 RID: 10660
	public Color lightText;

	// Token: 0x040029A5 RID: 10661
	public Color disabledText;

	// Token: 0x040029A6 RID: 10662
	public int selectableLength;
}
