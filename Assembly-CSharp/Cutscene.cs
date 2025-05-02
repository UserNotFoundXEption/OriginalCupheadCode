using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public abstract class Cutscene : AbstractPausableComponent
{
	// Token: 0x06000852 RID: 2130 RVA: 0x00007F92 File Offset: 0x00006192
	public Cutscene()
	{
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000853 RID: 2131 RVA: 0x00007F9A File Offset: 0x0000619A
	// (set) Token: 0x06000854 RID: 2132 RVA: 0x00007FA1 File Offset: 0x000061A1
	public static Cutscene Current { get; set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x00007FA9 File Offset: 0x000061A9
	// (set) Token: 0x06000856 RID: 2134 RVA: 0x00007FB0 File Offset: 0x000061B0
	public static SceneLoader.Properties transitionProperties { get; set; } = new SceneLoader.Properties();

	// Token: 0x06000857 RID: 2135 RVA: 0x00007FB8 File Offset: 0x000061B8
	public static void Load(Levels level, Scenes cutscene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass)
	{
		Cutscene.transitionProperties.transitionStart = transitionStart;
		Cutscene.transitionProperties.transitionEnd = transitionEnd;
		Cutscene.transitionProperties.icon = icon;
		Cutscene.mode = Cutscene.Mode.Level;
		Cutscene.levelAfterCutscene = level;
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, null);
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00007FF2 File Offset: 0x000061F2
	public static void Load(Scenes scene, Scenes cutscene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass)
	{
		Cutscene.transitionProperties.transitionStart = transitionStart;
		Cutscene.transitionProperties.transitionEnd = transitionEnd;
		Cutscene.transitionProperties.icon = icon;
		Cutscene.mode = Cutscene.Mode.Scene;
		Cutscene.sceneAfterCutscene = scene;
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, null);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0000802C File Offset: 0x0000622C
	public override void Awake()
	{
		base.Awake();
		Cutscene.Current = this;
		Cuphead.Init(false);
		this.CreateUI();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00075DBC File Offset: 0x00073FBC
	public virtual void Start()
	{
		CupheadTime.SetAll(1f);
		InterruptingPrompt.SetCanInterrupt(true);
		this.CreateCamera();
		this.gui.CutseneInit();
		this.SetRichPresence();
		if (this.translationText != null)
		{
			this.translationText.SetActive(Localization.language != Localization.Languages.English);
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00008046 File Offset: 0x00006246
	public void CreateUI()
	{
		this.gui = Object.FindObjectOfType<CutsceneGUI>();
		if (this.gui == null)
		{
			this.gui = Resources.Load<CutsceneGUI>("UI/Cutscene_UI").InstantiatePrefab<CutsceneGUI>();
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00075E18 File Offset: 0x00074018
	public void CreateCamera()
	{
		CupheadCutsceneCamera cupheadCutsceneCamera = Object.FindObjectOfType<CupheadCutsceneCamera>();
		cupheadCutsceneCamera.Init();
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00075E34 File Offset: 0x00074034
	public virtual void OnCutsceneOver()
	{
		Cutscene.Mode mode = Cutscene.mode;
		if (mode != Cutscene.Mode.Scene)
		{
			if (mode == Cutscene.Mode.Level)
			{
				SceneLoader.LoadLevel(Cutscene.levelAfterCutscene, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
			}
		}
		else
		{
			SceneLoader.LoadScene(Cutscene.sceneAfterCutscene, SceneLoader.Transition.Fade, Cutscene.transitionProperties.transitionEnd, Cutscene.transitionProperties.icon, null);
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0000807E File Offset: 0x0000627E
	public void Skip()
	{
		this.OnCutsceneOver();
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00008086 File Offset: 0x00006286
	public virtual void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Cutscene", true);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x000080A2 File Offset: 0x000062A2
	public bool IsTranslationTextActive()
	{
		return this.translationText.activeSelf;
	}

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	public GameObject translationText;

	// Token: 0x04000660 RID: 1632
	public static Scenes sceneAfterCutscene;

	// Token: 0x04000661 RID: 1633
	public static Levels levelAfterCutscene;

	// Token: 0x04000662 RID: 1634
	public static Cutscene.Mode mode;

	// Token: 0x04000663 RID: 1635
	public CutsceneGUI gui;

	// Token: 0x0200090B RID: 2315
	public enum Mode
	{
		// Token: 0x040044B4 RID: 17588
		Scene,
		// Token: 0x040044B5 RID: 17589
		Level
	}
}
