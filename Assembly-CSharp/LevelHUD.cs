using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class LevelHUD : AbstractMonoBehaviour
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0000ADA9 File Offset: 0x00008FA9
	// (set) Token: 0x06000C63 RID: 3171 RVA: 0x0000ADB0 File Offset: 0x00008FB0
	public static LevelHUD Current { get; set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0000ADB8 File Offset: 0x00008FB8
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00083290 File Offset: 0x00081490
	public override void Awake()
	{
		base.Awake();
		LevelGUI.DebugOnDisableGuiEvent += this.OnDisableGUI;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		LevelHUD.Current = this;
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x000832DC File Offset: 0x000814DC
	public void OnDestroy()
	{
		LevelGUI.DebugOnDisableGuiEvent -= this.OnDisableGUI;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		if (LevelHUD.Current == this)
		{
			LevelHUD.Current = null;
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0000ADC0 File Offset: 0x00008FC0
	public void Start()
	{
		this.canvas.worldCamera = CupheadLevelCamera.Current.camera;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00083334 File Offset: 0x00081534
	public void LevelInit()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.levelHudTemplate = Object.Instantiate<LevelHUDPlayer>(this.cuphead);
		this.levelHudTemplate.gameObject.SetActive(false);
		if (PlayerManager.Multiplayer)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.mugman = Object.Instantiate<LevelHUDPlayer>(this.levelHudTemplate);
			this.mugman.gameObject.SetActive(true);
			this.mugman.transform.SetParent(this.cuphead.transform.parent, false);
			this.mugman.Init(player2, false);
		}
		this.cuphead.Init(player, false);
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0000ADD7 File Offset: 0x00008FD7
	public void OnDisableGUI()
	{
		this.canvas.enabled = false;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x000833D8 File Offset: 0x000815D8
	public void OnPlayerJoined(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.mugman = Object.Instantiate<LevelHUDPlayer>(this.levelHudTemplate);
			this.mugman.gameObject.SetActive(true);
			this.mugman.transform.SetParent(this.cuphead.transform.parent, false);
			this.mugman.Init(player2, !Level.IsTowerOfPowerMain);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0000ADE5 File Offset: 0x00008FE5
	public void OnPlayerLeave(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			Object.Destroy(this.mugman.gameObject);
		}
	}

	// Token: 0x040009DB RID: 2523
	[SerializeField]
	public Canvas canvas;

	// Token: 0x040009DC RID: 2524
	[Space(10f)]
	[SerializeField]
	public LevelHUDPlayer cuphead;

	// Token: 0x040009DD RID: 2525
	public LevelHUDPlayer levelHudTemplate;

	// Token: 0x040009DE RID: 2526
	public LevelHUDPlayer mugman;
}
