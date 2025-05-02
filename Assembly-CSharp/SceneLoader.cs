using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000598 RID: 1432
public class SceneLoader : AbstractMonoBehaviour
{
	// Token: 0x06003C5F RID: 15455 RVA: 0x00030D89 File Offset: 0x0002EF89
	static SceneLoader()
	{
		SceneLoader.CurrentLevel = Levels.Veggies;
		SceneLoader.properties = new SceneLoader.Properties();
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06003C61 RID: 15457 RVA: 0x00030DA5 File Offset: 0x0002EFA5
	public static bool Exists
	{
		get
		{
			return SceneLoader._instance != null;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06003C62 RID: 15458 RVA: 0x00030DB2 File Offset: 0x0002EFB2
	public static SceneLoader instance
	{
		get
		{
			if (SceneLoader._instance == null)
			{
				SceneLoader._instance = (Object.Instantiate(Resources.Load("UI/Scene_Loader")) as GameObject).GetComponent<SceneLoader>();
			}
			return SceneLoader._instance;
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06003C63 RID: 15459 RVA: 0x00030DE7 File Offset: 0x0002EFE7
	// (set) Token: 0x06003C64 RID: 15460 RVA: 0x00030DEE File Offset: 0x0002EFEE
	public static Levels CurrentLevel { get; set; }

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06003C65 RID: 15461 RVA: 0x00030DF6 File Offset: 0x0002EFF6
	// (set) Token: 0x06003C66 RID: 15462 RVA: 0x00030DFD File Offset: 0x0002EFFD
	public static string SceneName { get; set; } = string.Empty;

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00030E05 File Offset: 0x0002F005
	// (set) Token: 0x06003C68 RID: 15464 RVA: 0x00030E0C File Offset: 0x0002F00C
	public static SceneLoader.Properties properties { get; set; }

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06003C69 RID: 15465 RVA: 0x00030E14 File Offset: 0x0002F014
	// (set) Token: 0x06003C6A RID: 15466 RVA: 0x00030E1B File Offset: 0x0002F01B
	public static SceneLoader.Context CurrentContext { get; set; }

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06003C6B RID: 15467 RVA: 0x00030E23 File Offset: 0x0002F023
	public static bool CurrentlyLoading
	{
		get
		{
			return SceneLoader.currentlyLoading;
		}
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x0011562C File Offset: 0x0011382C
	public static void LoadScene(string sceneName, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		Scenes scene = Scenes.scene_start;
		if (!EnumUtils.TryParse<Scenes>(sceneName, out scene))
		{
			return;
		}
		SceneLoader.LoadScene(scene, transitionStart, transitionEnd, icon, context);
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x00115654 File Offset: 0x00113854
	public static void LoadScene(Scenes scene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		if (SceneLoader.currentlyLoading)
		{
			return;
		}
		InterruptingPrompt.SetCanInterrupt(false);
		SceneLoader.properties.transitionStart = transitionStart;
		SceneLoader.properties.transitionEnd = transitionEnd;
		SceneLoader.properties.icon = icon;
		SceneLoader.EndTransitionDelay = 0.6f;
		SceneLoader.previousSceneName = SceneLoader.SceneName;
		SceneLoader.SceneName = scene.ToString();
		SceneLoader.CurrentContext = context;
		SceneLoader.instance.load();
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x00030E2A File Offset: 0x0002F02A
	public static void LoadLevel(Levels level, SceneLoader.Transition transitionStart, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		SceneLoader.CurrentLevel = level;
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(level), transitionStart, SceneLoader.Transition.Iris, icon, context);
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x001156CC File Offset: 0x001138CC
	public static void LoadDicePalaceLevel(DicePalaceLevels dicePalaceLevel)
	{
		Levels dicePalaceLevel2 = LevelProperties.GetDicePalaceLevel(dicePalaceLevel);
		SceneLoader.CurrentLevel = dicePalaceLevel2;
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(dicePalaceLevel2), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x00030E41 File Offset: 0x0002F041
	public static void SetCurrentLevel(Levels level)
	{
		SceneLoader.CurrentLevel = level;
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x001156F8 File Offset: 0x001138F8
	public static void ContinueTowerOfPower()
	{
		int current_TURN = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		int turn_COUNTER = TowerOfPowerLevelGameInfo.TURN_COUNTER;
		if (current_TURN == turn_COUNTER)
		{
			TowerOfPowerLevelGameInfo.TURN_COUNTER++;
		}
		if (TowerOfPowerLevelGameInfo.TURN_COUNTER == TowerOfPowerLevelGameInfo.allStageSpaces.Count)
		{
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp();
			SceneLoader.LoadLastMap();
		}
		else
		{
			SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		}
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00030E49 File Offset: 0x0002F049
	public static void ResetTheTowerOfPower()
	{
		TowerOfPowerLevelGameInfo.ResetTowerOfPower();
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x00115760 File Offset: 0x00113960
	public static void ReloadLevel()
	{
		if (Level.IsTowerOfPower)
		{
			if (TowerOfPowerLevelGameInfo.IsTokenLeft(0))
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP = 3;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].BonusHP = 3;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].SuperCharge = 0f;
				TowerOfPowerLevelGameInfo.ReduceToken(0);
			}
			else
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP = 0;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].BonusHP = 0;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].SuperCharge = 0f;
			}
			if (PlayerManager.Multiplayer)
			{
				if (TowerOfPowerLevelGameInfo.IsTokenLeft(1))
				{
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP = 3;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].BonusHP = 3;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].SuperCharge = 0f;
					TowerOfPowerLevelGameInfo.ReduceToken(1);
				}
				else
				{
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP = 0;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].BonusHP = 0;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].SuperCharge = 0f;
				}
			}
		}
		else
		{
			if (Level.IsDicePalace)
			{
				SceneLoader.LoadDicePalaceLevel(DicePalaceLevels.DicePalaceMain);
				return;
			}
			if (Level.IsGraveyard)
			{
				SceneLoader.LoadScene(LevelProperties.GetLevelScene(SceneLoader.CurrentLevel), SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.None, null);
				return;
			}
			if (Level.IsChessBoss)
			{
				if (SceneLoader.CurrentContext is GauntletContext && !((GauntletContext)SceneLoader.CurrentContext).complete)
				{
					Scenes scene = Scenes.scene_level_chess_pawn;
					SceneLoader.Transition transitionStart = SceneLoader.Transition.Fade;
					SceneLoader.Transition transitionEnd = SceneLoader.Transition.Iris;
					GauntletContext context = new GauntletContext(false);
					SceneLoader.LoadScene(scene, transitionStart, transitionEnd, SceneLoader.Icon.Hourglass, context);
					return;
				}
				PlayerData.Data.IncrementKingOfGamesCounter();
				PlayerData.SaveCurrentFile();
			}
		}
		float transitionStartTime = SceneLoader.properties.transitionStartTime;
		SceneLoader.properties.transitionStartTime = 0.25f;
		SceneLoader.LoadLevel(SceneLoader.CurrentLevel, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		SceneLoader.properties.transitionStartTime = transitionStartTime;
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x0011591C File Offset: 0x00113B1C
	public static void LoadLastMap()
	{
		if (Level.IsGraveyard)
		{
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.Hourglass, null);
			SceneLoader.IsInBlurTransition = true;
		}
		else
		{
			Scenes scene = PlayerData.Data.CurrentMap;
			if (Level.IsChessBoss)
			{
				PlayerData.Data.IncrementKingOfGamesCounter();
				PlayerData.SaveCurrentFile();
				bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
				if (flag)
				{
					scene = Scenes.scene_level_chess_castle;
				}
			}
			SceneLoader.LoadScene(scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x00030E63 File Offset: 0x0002F063
	public static void TransitionOut()
	{
		SceneLoader.TransitionOut(SceneLoader.properties.transitionStart);
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x00030E74 File Offset: 0x0002F074
	public static void TransitionOut(SceneLoader.Transition transition)
	{
		SceneLoader.TransitionOut(transition, SceneLoader.properties.transitionStartTime);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x00030E86 File Offset: 0x0002F086
	public static void TransitionOut(SceneLoader.Transition transition, float time)
	{
		SceneLoader.properties.transitionStart = transition;
		SceneLoader.properties.transitionStartTime = time;
		SceneLoader.instance.Out();
	}

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x06003C78 RID: 15480 RVA: 0x001159A4 File Offset: 0x00113BA4
	// (remove) Token: 0x06003C79 RID: 15481 RVA: 0x001159D8 File Offset: 0x00113BD8
	public static event SceneLoader.FadeHandler OnFadeInStartEvent;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x06003C7A RID: 15482 RVA: 0x00115A0C File Offset: 0x00113C0C
	// (remove) Token: 0x06003C7B RID: 15483 RVA: 0x00115A40 File Offset: 0x00113C40
	public static event Action OnFadeInEndEvent;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x06003C7C RID: 15484 RVA: 0x00115A74 File Offset: 0x00113C74
	// (remove) Token: 0x06003C7D RID: 15485 RVA: 0x00115AA8 File Offset: 0x00113CA8
	public static event SceneLoader.FadeHandler OnFadeOutStartEvent;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x06003C7E RID: 15486 RVA: 0x00115ADC File Offset: 0x00113CDC
	// (remove) Token: 0x06003C7F RID: 15487 RVA: 0x00115B10 File Offset: 0x00113D10
	public static event Action OnFadeOutEndEvent;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x06003C80 RID: 15488 RVA: 0x00115B44 File Offset: 0x00113D44
	// (remove) Token: 0x06003C81 RID: 15489 RVA: 0x00115B78 File Offset: 0x00113D78
	public static event SceneLoader.FadeHandler OnFaderValue;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x06003C82 RID: 15490 RVA: 0x00115BAC File Offset: 0x00113DAC
	// (remove) Token: 0x06003C83 RID: 15491 RVA: 0x00115BE0 File Offset: 0x00113DE0
	public static event Action OnLoaderCompleteEvent;

	// Token: 0x06003C84 RID: 15492 RVA: 0x00030EA8 File Offset: 0x0002F0A8
	public override void Awake()
	{
		base.Awake();
		SceneLoader._instance = this;
		this.SetIconAlpha(0f);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x00115C14 File Offset: 0x00113E14
	public void load()
	{
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Loadscreen.ToString(), 5f);
		}
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00030ECC File Offset: 0x0002F0CC
	public void In()
	{
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x00030EDB File Offset: 0x0002F0DB
	public void Out()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			if (SceneLoader.OnFadeOutEndEvent != null)
			{
				SceneLoader.OnFadeOutEndEvent();
			}
			return;
		}
		base.StartCoroutine(this.out_cr());
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x00030F0F File Offset: 0x0002F10F
	public void UpdateProgress(float progress)
	{
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x00030F11 File Offset: 0x0002F111
	public void SetIconAlpha(float a)
	{
		this.SetImageAlpha(this.icon, a);
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x00030F20 File Offset: 0x0002F120
	public void SetFaderAlpha(float a)
	{
		this.SetImageAlpha(this.fader, a);
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00115C84 File Offset: 0x00113E84
	public void SetImageAlpha(Image i, float a)
	{
		Color color = i.color;
		color.a = a;
		i.color = color;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00115CA8 File Offset: 0x00113EA8
	public IEnumerator loop_cr()
	{
		SceneLoader.currentlyLoading = true;
		yield return base.StartCoroutine(this.in_cr());
		base.StartCoroutine(this.load_cr());
		yield return base.StartCoroutine(this.iconFadeIn_cr());
		while (!this.doneLoadingSceneAsync)
		{
			yield return null;
		}
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString())
		{
			AudioManager.SnapshotReset(SceneLoader.SceneName, 0.15f);
		}
		AsyncOperation op = Resources.UnloadUnusedAssets();
		while (!op.isDone)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.iconFadeOut_cr());
		yield return base.StartCoroutine(this.out_cr());
		SceneLoader.properties.Reset();
		SceneLoader.currentlyLoading = false;
		yield break;
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00115CC4 File Offset: 0x00113EC4
	public IEnumerator load_cr()
	{
		this.doneLoadingSceneAsync = false;
		GC.Collect();
		if (SceneLoader.SceneName != SceneLoader.previousSceneName && SceneLoader.SceneName != Scenes.scene_slot_select.ToString())
		{
			string text = null;
			if (!Array.Exists<Levels>(Level.kingOfGamesLevelsWithCastle, (Levels level) => LevelProperties.GetLevelScene(level) == SceneLoader.SceneName))
			{
				text = Scenes.scene_level_chess_castle.ToString();
			}
			AssetBundleLoader.UnloadAssetBundles();
			AssetLoader<SpriteAtlas>.UnloadAssets(new string[]
			{
				text
			});
			if (SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
			{
				AssetLoader<AudioClip>.UnloadAssets(new string[0]);
			}
			AssetLoader<Texture2D[]>.UnloadAssets(new string[0]);
		}
		if (SceneLoader.SceneName == Scenes.scene_title.ToString())
		{
			DLCManager.RefreshDLC();
		}
		AssetLoaderOption atlasOption = AssetLoaderOption.None();
		if (SceneLoader.SceneName == Scenes.scene_level_chess_castle.ToString())
		{
			atlasOption = AssetLoaderOption.PersistInCacheTagged(SceneLoader.SceneName);
		}
		string[] preloadAtlases = AssetLoader<SpriteAtlas>.GetPreloadAssetNames(SceneLoader.SceneName);
		string[] preloadMusic = AssetLoader<AudioClip>.GetPreloadAssetNames(SceneLoader.SceneName);
		if (SceneLoader.SceneName != SceneLoader.previousSceneName && (preloadAtlases.Length > 0 || preloadMusic.Length > 0))
		{
			AsyncOperation intermediateSceneAsyncOp = SceneManager.LoadSceneAsync(this.LOAD_SCENE_NAME);
			while (!intermediateSceneAsyncOp.isDone)
			{
				yield return null;
			}
			for (int i = 0; i < preloadAtlases.Length; i++)
			{
				yield return AssetLoader<SpriteAtlas>.LoadAsset(preloadAtlases[i], atlasOption);
			}
			AssetLoaderOption musicOption = AssetLoaderOption.None();
			for (int j = 0; j < preloadMusic.Length; j++)
			{
				yield return AssetLoader<AudioClip>.LoadAsset(preloadMusic[j], musicOption);
			}
			Coroutine[] persistentAssetsCoroutines = DLCManager.LoadPersistentAssets();
			if (persistentAssetsCoroutines != null)
			{
				for (int k = 0; k < persistentAssetsCoroutines.Length; k++)
				{
					yield return persistentAssetsCoroutines[k];
				}
			}
			yield return null;
		}
		AsyncOperation async = SceneManager.LoadSceneAsync(SceneLoader.SceneName);
		while (!async.isDone || AssetBundleLoader.loadCounter > 0)
		{
			this.UpdateProgress(async.progress);
			yield return null;
		}
		this.doneLoadingSceneAsync = true;
		yield break;
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00115CE0 File Offset: 0x00113EE0
	public IEnumerator in_cr()
	{
		switch (SceneLoader.properties.transitionStart)
		{
		default:
			if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
			{
				this.FadeOutBGM(0.6f);
			}
			yield return base.StartCoroutine(this.irisIn_cr());
			break;
		case SceneLoader.Transition.Fade:
			if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_level_graveyard.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() && (SceneLoader.CurrentLevel != Levels.Saltbaker || SceneLoader.SceneName != Scenes.scene_win.ToString()))
			{
				this.FadeOutBGM(SceneLoader.properties.transitionEndTime);
			}
			yield return base.StartCoroutine(this.faderFadeIn_cr());
			break;
		case SceneLoader.Transition.Blur:
			yield return base.StartCoroutine(this.blurIn_cr());
			break;
		case SceneLoader.Transition.None:
			this.SetFaderAlpha(1f);
			break;
		}
		yield break;
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00115CFC File Offset: 0x00113EFC
	public IEnumerator out_cr()
	{
		yield return null;
		switch (SceneLoader.properties.transitionEnd)
		{
		default:
			yield return base.StartCoroutine(this.irisOut_cr());
			break;
		case SceneLoader.Transition.Fade:
			yield return base.StartCoroutine(this.faderFadeOut_cr());
			break;
		case SceneLoader.Transition.Blur:
			yield return base.StartCoroutine(this.blurOut_cr());
			break;
		case SceneLoader.Transition.None:
			this.SetFaderAlpha(0f);
			break;
		}
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && !Level.IsGraveyard && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			this.ResetBgmVolume();
		}
		if (SceneLoader.OnLoaderCompleteEvent != null)
		{
			SceneLoader.OnLoaderCompleteEvent();
		}
		SceneLoader.OnLoaderCompleteEvent = null;
		yield break;
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00115D18 File Offset: 0x00113F18
	public IEnumerator irisIn_cr()
	{
		SceneLoader.IsInIrisTransition = true;
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Iris_In");
		this.SetFaderAlpha(1f);
		if (SceneLoader.OnFadeInStartEvent != null)
		{
			SceneLoader.OnFadeInStartEvent(0.6f);
		}
		yield return new WaitForSeconds(0.6f);
		if (SceneLoader.OnFadeInEndEvent != null)
		{
			SceneLoader.OnFadeInEndEvent();
		}
		yield break;
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00115D34 File Offset: 0x00113F34
	public IEnumerator irisOut_cr()
	{
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Iris_Out");
		this.SetFaderAlpha(1f);
		if (SceneLoader.OnFadeOutStartEvent != null)
		{
			SceneLoader.OnFadeOutStartEvent(0.6f);
		}
		yield return new WaitForSeconds(0.6f);
		if (SceneLoader.OnFadeOutEndEvent != null)
		{
			SceneLoader.OnFadeOutEndEvent();
		}
		SceneLoader.IsInIrisTransition = false;
		yield break;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00115D50 File Offset: 0x00113F50
	public IEnumerator faderFadeIn_cr()
	{
		SceneLoader.IsInIrisTransition = false;
		this.SetFaderAlpha(0f);
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Black");
		if (SceneLoader.OnFadeInStartEvent != null)
		{
			SceneLoader.OnFadeInStartEvent(SceneLoader.properties.transitionStartTime);
		}
		yield return base.StartCoroutine(this.imageFade_cr(this.fader, SceneLoader.properties.transitionStartTime, 0f, 1f, false));
		if (SceneLoader.OnFadeInEndEvent != null)
		{
			SceneLoader.OnFadeInEndEvent();
		}
		yield break;
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x00115D6C File Offset: 0x00113F6C
	public IEnumerator faderFadeOut_cr()
	{
		if (SceneLoader.OnFadeOutStartEvent != null)
		{
			SceneLoader.OnFadeOutStartEvent(SceneLoader.properties.transitionEndTime);
		}
		yield return base.StartCoroutine(this.imageFade_cr(this.fader, SceneLoader.properties.transitionEndTime, 1f, 0f, false));
		if (SceneLoader.OnFadeOutEndEvent != null)
		{
			SceneLoader.OnFadeOutEndEvent();
		}
		yield break;
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x00115D88 File Offset: 0x00113F88
	public IEnumerator blurIn_cr()
	{
		SceneLoader.IsInBlurTransition = true;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AbstractCupheadGameCamera cam = (!(CupheadLevelCamera.Current != null)) ? CupheadMapCamera.Current : CupheadLevelCamera.Current;
		cam.StartBlur(0.5f, 2f);
		AudioManager.ChangeBGMPitch(0.9f, 0.5f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		cam.EndBlur(0.5f);
		AudioManager.ChangeBGMPitch(1f, 0.5f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		cam.StartBlur(3f, 5f);
		AudioManager.ChangeBGMPitch(0.7f, 7f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.properties.transitionStartTime = 3f;
		this.FadeOutBGM(6f);
		yield return base.StartCoroutine(this.faderFadeIn_cr());
		yield break;
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x00115DA4 File Offset: 0x00113FA4
	public IEnumerator blurOut_cr()
	{
		SceneLoader.IsInBlurTransition = true;
		AbstractCupheadGameCamera cam = (!(CupheadLevelCamera.Current != null)) ? CupheadMapCamera.Current : CupheadLevelCamera.Current;
		cam.StartBlur(0.01f, 5f);
		yield return new WaitForSeconds(0.015f);
		cam.EndBlur(2.5f, 5f);
		SceneLoader.properties.transitionEndTime = 2f;
		yield return base.StartCoroutine(this.faderFadeOut_cr());
		cam.StartBlur(0.5f, 5f);
		yield return new WaitForSeconds(0.5f);
		cam.EndBlur(0.5f, 5f);
		yield return new WaitForSeconds(0.5f);
		SceneLoader.IsInBlurTransition = false;
		yield break;
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x00115DC0 File Offset: 0x00113FC0
	public IEnumerator iconFadeIn_cr()
	{
		if (SceneLoader.properties.icon == SceneLoader.Icon.None)
		{
			this.SetIconAlpha(0f);
		}
		else
		{
			Animator animator = this.icon.GetComponent<Animator>();
			animator.SetTrigger(SceneLoader.properties.icon.ToString());
			yield return base.StartCoroutine(this.imageFade_cr(this.icon, 0.4f, 0f, 1f, true));
		}
		yield break;
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x00115DDC File Offset: 0x00113FDC
	public IEnumerator iconFadeOut_cr()
	{
		if (SceneLoader.properties.icon == SceneLoader.Icon.None)
		{
			this.SetIconAlpha(0f);
			yield return new WaitForSeconds(0.6f);
		}
		else
		{
			float startAlpha = this.icon.color.a;
			yield return base.StartCoroutine(this.imageFade_cr(this.icon, 0.6f * startAlpha, startAlpha, 0f, false));
			if (startAlpha < 1f)
			{
				yield return new WaitForSeconds(0.6f * (1f - startAlpha));
			}
		}
		yield break;
	}

	// Token: 0x06003C98 RID: 15512 RVA: 0x00115DF8 File Offset: 0x00113FF8
	public IEnumerator imageFade_cr(Image image, float time, float start, float end, bool interruptOnLoad = false)
	{
		float t = 0f;
		this.SetImageAlpha(image, start);
		while (t < time && (!interruptOnLoad || !this.doneLoadingSceneAsync))
		{
			float val = Mathf.Lerp(start, end, t / time);
			this.SetImageAlpha(image, val);
			t += Time.deltaTime;
			if (SceneLoader.OnFaderValue != null)
			{
				SceneLoader.OnFaderValue(t / time);
			}
			if (interruptOnLoad)
			{
				SceneLoader.EndTransitionDelay = val * 0.6f;
			}
			yield return null;
		}
		this.SetImageAlpha(image, end);
		if (interruptOnLoad && !this.doneLoadingSceneAsync)
		{
			SceneLoader.EndTransitionDelay = 0.6f;
		}
		yield break;
	}

	// Token: 0x06003C99 RID: 15513 RVA: 0x00115E38 File Offset: 0x00114038
	public IEnumerator fadeBGM_cr(float time)
	{
		if (AudioNoiseHandler.Instance != null)
		{
			AudioNoiseHandler.Instance.OpticalSound();
		}
		this.bgmVolumeStart = AudioManager.bgmOptionsVolume;
		this.bgmVolume = AudioManager.bgmOptionsVolume;
		this.sfxVolumeStart = AudioManager.sfxOptionsVolume;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			AudioManager.bgmOptionsVolume = Mathf.Lerp(this.bgmVolume, -80f, val);
			t += Time.deltaTime;
			yield return null;
		}
		AudioManager.bgmOptionsVolume = -80f;
		AudioManager.StopBGM();
		yield break;
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x00030F2F File Offset: 0x0002F12F
	public void FadeOutBGM(float time)
	{
		if (this.bgmCoroutine != null)
		{
			base.StopCoroutine(this.bgmCoroutine);
		}
		this.bgmCoroutine = base.StartCoroutine(this.fadeBGM_cr(time));
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x00030F5B File Offset: 0x0002F15B
	public void ResetBgmVolume()
	{
		if (this.bgmCoroutine != null)
		{
			base.StopCoroutine(this.bgmCoroutine);
		}
		AudioManager.bgmOptionsVolume = this.bgmVolumeStart;
		AudioManager.sfxOptionsVolume = this.sfxVolumeStart;
	}

	// Token: 0x04002FEF RID: 12271
	public const string SCENE_LOADER_PATH = "UI/Scene_Loader";

	// Token: 0x04002FF0 RID: 12272
	public const float ICON_IN_TIME = 0.4f;

	// Token: 0x04002FF1 RID: 12273
	public const float ICON_OUT_TIME = 0.6f;

	// Token: 0x04002FF2 RID: 12274
	public const float ICON_WAIT_TIME = 1f;

	// Token: 0x04002FF3 RID: 12275
	public const float ICON_NONE_TIME = 1f;

	// Token: 0x04002FF4 RID: 12276
	public const float FADER_DELAY = 0.5f;

	// Token: 0x04002FF5 RID: 12277
	public const float IRIS_TIME = 0.6f;

	// Token: 0x04002FF6 RID: 12278
	public readonly string LOAD_SCENE_NAME = Scenes.scene_load_helper.ToString();

	// Token: 0x04002FF7 RID: 12279
	public static float EndTransitionDelay;

	// Token: 0x04002FF8 RID: 12280
	public static bool IsInIrisTransition;

	// Token: 0x04002FF9 RID: 12281
	public static bool IsInBlurTransition;

	// Token: 0x04002FFA RID: 12282
	public static SceneLoader _instance;

	// Token: 0x04002FFC RID: 12284
	public static string previousSceneName;

	// Token: 0x04003000 RID: 12288
	public static bool currentlyLoading;

	// Token: 0x04003007 RID: 12295
	[SerializeField]
	public Canvas canvas;

	// Token: 0x04003008 RID: 12296
	[SerializeField]
	public Image fader;

	// Token: 0x04003009 RID: 12297
	[SerializeField]
	public Image icon;

	// Token: 0x0400300A RID: 12298
	[SerializeField]
	public SceneLoaderCamera camera;

	// Token: 0x0400300B RID: 12299
	public bool doneLoadingSceneAsync;

	// Token: 0x0400300C RID: 12300
	public float bgmVolume;

	// Token: 0x0400300D RID: 12301
	public float bgmLevelVolume;

	// Token: 0x0400300E RID: 12302
	public float bgmVolumeStart;

	// Token: 0x0400300F RID: 12303
	public float bgmLevelVolumeStart;

	// Token: 0x04003010 RID: 12304
	public float sfxVolumeStart;

	// Token: 0x04003011 RID: 12305
	public Coroutine bgmCoroutine;

	// Token: 0x02001218 RID: 4632
	public abstract class Context
	{
		// Token: 0x06008046 RID: 32838 RVA: 0x00055D4D File Offset: 0x00053F4D
		public Context()
		{
		}
	}

	// Token: 0x02001219 RID: 4633
	// (Invoke) Token: 0x06008048 RID: 32840
	public delegate void FadeHandler(float time);

	// Token: 0x0200121A RID: 4634
	public enum Transition
	{
		// Token: 0x04007D84 RID: 32132
		Iris,
		// Token: 0x04007D85 RID: 32133
		Fade,
		// Token: 0x04007D86 RID: 32134
		Blur,
		// Token: 0x04007D87 RID: 32135
		None
	}

	// Token: 0x0200121B RID: 4635
	public enum Icon
	{
		// Token: 0x04007D89 RID: 32137
		None,
		// Token: 0x04007D8A RID: 32138
		Random,
		// Token: 0x04007D8B RID: 32139
		Cuphead_Head,
		// Token: 0x04007D8C RID: 32140
		Cuphead_Running,
		// Token: 0x04007D8D RID: 32141
		Cuphead_Jumping,
		// Token: 0x04007D8E RID: 32142
		Screen_OneMoment,
		// Token: 0x04007D8F RID: 32143
		Hourglass,
		// Token: 0x04007D90 RID: 32144
		HourglassBroken
	}

	// Token: 0x0200121C RID: 4636
	public class Properties
	{
		// Token: 0x0600804B RID: 32843 RVA: 0x00055D55 File Offset: 0x00053F55
		public Properties()
		{
			this.Reset();
		}

		// Token: 0x0600804C RID: 32844 RVA: 0x00055D63 File Offset: 0x00053F63
		public void Reset()
		{
			this.icon = SceneLoader.Icon.Hourglass;
			this.transitionStart = SceneLoader.Transition.Fade;
			this.transitionEnd = SceneLoader.Transition.Fade;
			this.transitionStartTime = 0.4f;
			this.transitionEndTime = 0.4f;
		}

		// Token: 0x04007D91 RID: 32145
		public const float FADE_START_DEFAULT = 0.4f;

		// Token: 0x04007D92 RID: 32146
		public const float FADE_END_DEFAULT = 0.4f;

		// Token: 0x04007D93 RID: 32147
		public SceneLoader.Icon icon;

		// Token: 0x04007D94 RID: 32148
		public SceneLoader.Transition transitionStart;

		// Token: 0x04007D95 RID: 32149
		public SceneLoader.Transition transitionEnd;

		// Token: 0x04007D96 RID: 32150
		public float transitionStartTime;

		// Token: 0x04007D97 RID: 32151
		public float transitionEndTime;
	}
}
