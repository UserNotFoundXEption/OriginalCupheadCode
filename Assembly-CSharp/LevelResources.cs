using System;

// Token: 0x0200011B RID: 283
public class LevelResources : AbstractMonoBehaviour
{
	// Token: 0x06000D7E RID: 3454 RVA: 0x00087554 File Offset: 0x00085754
	public void OnDestroy()
	{
		this.levelHUD = null;
		this.levelGUI = null;
		this.levelAudio = null;
		this.levelBossDeathExplosion = null;
		this.levelPlayer = null;
		this.planePlayer = null;
		this.joinEffect = null;
		this.platformingIntro = null;
		this.platformingWin = null;
		this.levelIntro = null;
		this.levelUIInteractionDialogue = null;
	}

	// Token: 0x04000A7F RID: 2687
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/LevelResources/Level_Resources.prefab";

	// Token: 0x04000A80 RID: 2688
	public LevelHUD levelHUD;

	// Token: 0x04000A81 RID: 2689
	public LevelGUI levelGUI;

	// Token: 0x04000A82 RID: 2690
	public LevelAudio levelAudio;

	// Token: 0x04000A83 RID: 2691
	public Effect levelBossDeathExplosion;

	// Token: 0x04000A84 RID: 2692
	public LevelPlayerController levelPlayer;

	// Token: 0x04000A85 RID: 2693
	public PlanePlayerController planePlayer;

	// Token: 0x04000A86 RID: 2694
	public PlayerJoinEffect joinEffect;

	// Token: 0x04000A87 RID: 2695
	public PlatformingLevelIntroAnimation platformingIntro;

	// Token: 0x04000A88 RID: 2696
	public PlatformingLevelWinAnimation platformingWin;

	// Token: 0x04000A89 RID: 2697
	public LevelIntroAnimation levelIntro;

	// Token: 0x04000A8A RID: 2698
	public LevelKOAnimation levelKO;

	// Token: 0x04000A8B RID: 2699
	public LevelUIInteractionDialogue levelUIInteractionDialogue;
}
