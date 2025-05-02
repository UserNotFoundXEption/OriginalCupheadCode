using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class LevelGUI : AbstractMonoBehaviour
{
	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0000A95E File Offset: 0x00008B5E
	// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0000A965 File Offset: 0x00008B65
	public static LevelGUI Current { get; set; }

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000C02 RID: 3074 RVA: 0x000821DC File Offset: 0x000803DC
	// (remove) Token: 0x06000C03 RID: 3075 RVA: 0x00082210 File Offset: 0x00080410
	public static event Action DebugOnDisableGuiEvent;

	// Token: 0x06000C04 RID: 3076 RVA: 0x0000A96D File Offset: 0x00008B6D
	public static void DebugDisableGUI()
	{
		if (LevelGUI.DebugOnDisableGuiEvent != null)
		{
			LevelGUI.DebugOnDisableGuiEvent();
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0000A983 File Offset: 0x00008B83
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0000A98B File Offset: 0x00008B8B
	public override void Awake()
	{
		base.Awake();
		LevelGUI.Current = this;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00082244 File Offset: 0x00080444
	public void Start()
	{
		this.uiCamera = Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.canvas.worldCamera = this.uiCamera.camera;
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x0000A999 File Offset: 0x00008B99
	public void OnDestroy()
	{
		this.pause = null;
		this.options = null;
		this.restartTowerConfirm = null;
		if (LevelGUI.Current == this)
		{
			LevelGUI.Current = null;
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x000822A0 File Offset: 0x000804A0
	public void LevelInit()
	{
		this.options = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.options.rectTransform.SetParent(this.optionsRoot, false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievements = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievements.rectTransform.SetParent(this.achievementsRoot, false);
		}
		if (Level.IsTowerOfPower)
		{
			this.restartTowerConfirm = this.restartTowerConfirmPrefab.InstantiatePrefab<RestartTowerConfirmGUI>();
			this.restartTowerConfirm.rectTransform.SetParent(this.restartTowerConfirmRoot, false);
		}
		this.pause.Init(true, this.options, this.achievements, this.restartTowerConfirm);
	}

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	public Canvas canvas;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	public LevelPauseGUI pause;

	// Token: 0x04000999 RID: 2457
	[SerializeField]
	public LevelGameOverGUI gameOver;

	// Token: 0x0400099A RID: 2458
	[SerializeField]
	public OptionsGUI optionsPrefab;

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	public RectTransform optionsRoot;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	public RestartTowerConfirmGUI restartTowerConfirmPrefab;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	public RectTransform restartTowerConfirmRoot;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	public AchievementsGUI achievementsPrefab;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	public RectTransform achievementsRoot;

	// Token: 0x040009A0 RID: 2464
	public OptionsGUI options;

	// Token: 0x040009A1 RID: 2465
	public AchievementsGUI achievements;

	// Token: 0x040009A2 RID: 2466
	public RestartTowerConfirmGUI restartTowerConfirm;

	// Token: 0x040009A3 RID: 2467
	[Space(10f)]
	[SerializeField]
	public CupheadUICamera uiCameraPrefab;

	// Token: 0x040009A4 RID: 2468
	public CupheadUICamera uiCamera;
}
