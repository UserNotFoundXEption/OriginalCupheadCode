using System;

// Token: 0x020000C7 RID: 199
public class CutscenePauseGUI : AbstractPauseGUI
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x0600090A RID: 2314 RVA: 0x00077120 File Offset: 0x00075320
	// (remove) Token: 0x0600090B RID: 2315 RVA: 0x00077154 File Offset: 0x00075354
	public static event Action OnPauseEvent;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x0600090C RID: 2316 RVA: 0x00077188 File Offset: 0x00075388
	// (remove) Token: 0x0600090D RID: 2317 RVA: 0x000771BC File Offset: 0x000753BC
	public static event Action OnUnpauseEvent;

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x0600090E RID: 2318 RVA: 0x000088B5 File Offset: 0x00006AB5
	public override bool CanPause
	{
		get
		{
			return PauseManager.state != PauseManager.State.Paused && this.pauseAllowed;
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x000088CB File Offset: 0x00006ACB
	public override void OnPause()
	{
		base.OnPause();
		CupheadCutsceneCamera.Current.StartBlur();
		if (CutscenePauseGUI.OnPauseEvent != null)
		{
			CutscenePauseGUI.OnPauseEvent();
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000088F1 File Offset: 0x00006AF1
	public override void OnUnpause()
	{
		base.OnUnpause();
		CupheadCutsceneCamera.Current.EndBlur();
		if (CutscenePauseGUI.OnUnpauseEvent != null)
		{
			CutscenePauseGUI.OnUnpauseEvent();
		}
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00008917 File Offset: 0x00006B17
	public void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000771F0 File Offset: 0x000753F0
	public override void Update()
	{
		base.Update();
		if (base.state != AbstractPauseGUI.State.Paused)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Pause))
		{
			this.Unpause();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			this.Unpause();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			Cutscene.Current.Skip();
			return;
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0000891E File Offset: 0x00006B1E
	public void Restart()
	{
		base.state = AbstractPauseGUI.State.Animating;
		SceneLoader.ReloadLevel();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0000892C File Offset: 0x00006B2C
	public void StartNewGame()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00008944 File Offset: 0x00006B44
	public override void InAnimation(float i)
	{
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00008946 File Offset: 0x00006B46
	public override void OutAnimation(float i)
	{
	}

	// Token: 0x040006DC RID: 1756
	public bool pauseAllowed = true;
}
