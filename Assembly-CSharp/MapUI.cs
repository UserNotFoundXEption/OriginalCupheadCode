using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public class MapUI : AbstractMonoBehaviour
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06003337 RID: 13111 RVA: 0x0002A684 File Offset: 0x00028884
	// (set) Token: 0x06003338 RID: 13112 RVA: 0x0002A68B File Offset: 0x0002888B
	public static MapUI Current { get; set; }

	// Token: 0x06003339 RID: 13113 RVA: 0x000F3AC0 File Offset: 0x000F1CC0
	public static MapUI Create()
	{
		return Object.Instantiate<MapUI>(Map.Current.MapResources.mapUI);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x0002A693 File Offset: 0x00028893
	public override void Awake()
	{
		base.Awake();
		MapUI.Current = this;
		CupheadEventSystem.Init();
		LevelGUI.DebugOnDisableGuiEvent += this.OnDisableGUI;
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x000F3AE4 File Offset: 0x000F1CE4
	public void Start()
	{
		this.uiCamera = Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.screenCanvas.worldCamera = this.uiCamera.camera;
		this.sceneCanvas.worldCamera = CupheadMapCamera.Current.camera;
		this.hudCanvas.worldCamera = CupheadMapCamera.Current.camera;
		base.StartCoroutine(this.HandleReturnToMapTooltipEvents());
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0002A6B7 File Offset: 0x000288B7
	public void OnDestroy()
	{
		LevelGUI.DebugOnDisableGuiEvent -= this.OnDisableGUI;
		if (MapUI.Current == this)
		{
			MapUI.Current = null;
		}
		this.pauseUI = null;
		this.equipUI = null;
		this.optionsPrefab = null;
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x000F3B78 File Offset: 0x000F1D78
	public void Init(MapPlayerController[] players)
	{
		this.optionsUI = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.optionsUI.rectTransform.SetParent(this.optionsRoot, false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievementsUI = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievementsUI.rectTransform.SetParent(this.achievementsRoot, false);
		}
		this.pauseUI.Init(false, this.optionsUI, this.achievementsUI);
		this.equipUI.Init(false);
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0002A6F5 File Offset: 0x000288F5
	public void OnDisableGUI()
	{
		this.hudCanvas.enabled = false;
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0002A703 File Offset: 0x00028903
	public void Refresh()
	{
		this.optionsUI.SetupButtons();
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0002A710 File Offset: 0x00028910
	public void Update()
	{
		if (!MapEventNotification.Current.showing && MapEventNotification.Current.EventQueue.Count > 0)
		{
			MapEventNotification.Current.EventQueue.Dequeue()();
		}
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x000F3C04 File Offset: 0x000F1E04
	public IEnumerator HandleReturnToMapTooltipEvents()
	{
		yield return new WaitForSeconds(1f);
		if (PlayerData.Data.shouldShowBoatmanTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Boatman);
			PlayerData.Data.shouldShowBoatmanTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowShopkeepTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ShopKeep);
			PlayerData.Data.shouldShowShopkeepTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowTurtleTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle);
			PlayerData.Data.shouldShowTurtleTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowForkTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional);
			PlayerData.Data.shouldShowForkTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		yield break;
	}

	// Token: 0x04002A4F RID: 10831
	[SerializeField]
	public MapPauseUI pauseUI;

	// Token: 0x04002A50 RID: 10832
	[SerializeField]
	public MapEquipUI equipUI;

	// Token: 0x04002A51 RID: 10833
	[SerializeField]
	public OptionsGUI optionsPrefab;

	// Token: 0x04002A52 RID: 10834
	[SerializeField]
	public RectTransform optionsRoot;

	// Token: 0x04002A53 RID: 10835
	[SerializeField]
	public AchievementsGUI achievementsPrefab;

	// Token: 0x04002A54 RID: 10836
	[SerializeField]
	public RectTransform achievementsRoot;

	// Token: 0x04002A55 RID: 10837
	[Space(10f)]
	[SerializeField]
	public Canvas sceneCanvas;

	// Token: 0x04002A56 RID: 10838
	[SerializeField]
	public Canvas screenCanvas;

	// Token: 0x04002A57 RID: 10839
	[SerializeField]
	public Canvas hudCanvas;

	// Token: 0x04002A58 RID: 10840
	[Space(10f)]
	[SerializeField]
	public CupheadUICamera uiCameraPrefab;

	// Token: 0x04002A59 RID: 10841
	public OptionsGUI optionsUI;

	// Token: 0x04002A5A RID: 10842
	public AchievementsGUI achievementsUI;

	// Token: 0x04002A5B RID: 10843
	public CupheadUICamera uiCamera;
}
