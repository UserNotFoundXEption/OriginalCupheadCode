using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class MapNPCBoatman : AbstractMonoBehaviour
{
	// Token: 0x060030FF RID: 12543 RVA: 0x000E8380 File Offset: 0x000E6580
	public void Start()
	{
		this.AddDialoguerEvents();
		Dialoguer.SetGlobalFloat(22, (float)((!PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted) ? 0 : 1));
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			base.GetComponent<SpriteRenderer>().sortingOrder = 1000;
		}
		PlayerData.Data.hasUnlockedBoatman = true;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x00028C6D File Offset: 0x00026E6D
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x000E83E8 File Offset: 0x000E65E8
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onStarted += this.OnDialoguerStart;
		Dialoguer.events.onEnded += this.OnDialoguerEnd;
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x000E8438 File Offset: 0x000E6638
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onStarted -= this.OnDialoguerStart;
		Dialoguer.events.onEnded -= this.OnDialoguerEnd;
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x000E8488 File Offset: 0x000E6688
	public void SetOptions()
	{
		SpeechBubble instance = SpeechBubble.Instance;
		Scenes currentMap = PlayerData.Data.CurrentMap;
		switch (currentMap)
		{
		case Scenes.scene_map_world_1:
			instance.HideOptionByIndex(0);
			break;
		case Scenes.scene_map_world_2:
			instance.HideOptionByIndex(1);
			break;
		case Scenes.scene_map_world_3:
			instance.HideOptionByIndex(2);
			break;
		default:
			if (currentMap == Scenes.scene_map_world_DLC)
			{
				instance.HideOptionByIndex(3);
			}
			break;
		}
		if (!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
		{
			instance.HideOptionByIndex(1);
		}
		if (!PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
		{
			instance.HideOptionByIndex(2);
		}
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x000E8530 File Offset: 0x000E6730
	public void SelectWorld(string metadata)
	{
		if (this.selectionMade)
		{
			return;
		}
		int num;
		Parser.IntTryParse(metadata, out num);
		if (num > -1)
		{
			base.GetComponent<MapDialogueInteraction>().enabled = false;
			this.selectionMade = true;
			AudioManager.Play("sfx_worldmap_boattravel_accept");
			if (num == 3)
			{
				if (PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted)
				{
					SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
				}
				else
				{
					PlayerData.Data.Gift(PlayerId.PlayerOne, Charm.charm_chalice);
					PlayerData.Data.Gift(PlayerId.PlayerTwo, Charm.charm_chalice);
					PlayerData.Data.shouldShowChaliceTooltip = true;
					Cutscene.Load(Scenes.scene_level_kitchen, Scenes.scene_cutscene_dlc_intro, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.None;
					PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC;
				}
			}
			else if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
						SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
					}
				}
				else
				{
					PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
					SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
				}
			}
			else
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
				SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x000E8688 File Offset: 0x000E6888
	public void Update()
	{
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x00028C75 File Offset: 0x00026E75
	public void OnDialoguerStart()
	{
		base.animator.SetBool("Talk", true);
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x00028C88 File Offset: 0x00026E88
	public void OnDialoguerEnd()
	{
		base.animator.SetBool("Talk", false);
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x00028C9B File Offset: 0x00026E9B
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "BoatmanSetOptions")
		{
			this.SetOptions();
		}
		if (message == "BoatmanSelection")
		{
			this.SelectWorld(metadata);
		}
	}

	// Token: 0x04002873 RID: 10355
	public const int DIALOGUER_BOATMAN_STATE = 22;

	// Token: 0x04002874 RID: 10356
	public const int W1 = 0;

	// Token: 0x04002875 RID: 10357
	public const int W2 = 1;

	// Token: 0x04002876 RID: 10358
	public const int W3 = 2;

	// Token: 0x04002877 RID: 10359
	public const int WDLC = 3;

	// Token: 0x04002878 RID: 10360
	[SerializeField]
	public MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04002879 RID: 10361
	public float blinkTimer;

	// Token: 0x0400287A RID: 10362
	public bool selectionMade;
}
