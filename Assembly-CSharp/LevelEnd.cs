using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class LevelEnd : AbstractMonoBehaviour
{
	// Token: 0x06000D59 RID: 3417 RVA: 0x0000B701 File Offset: 0x00009901
	public override void Awake()
	{
		base.Awake();
		base.transform.SetAsFirstSibling();
		base.gameObject.name = "LEVEL_END_CONTROLER";
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x000870B4 File Offset: 0x000852B4
	public static LevelEnd Create()
	{
		GameObject gameObject = new GameObject();
		return gameObject.AddComponent<LevelEnd>();
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x000870D0 File Offset: 0x000852D0
	public static void Win(IEnumerator knockoutSFXCoroutine, Action onBossDeathCallback, Action explosionsCallback, Action explosionsFalloffCallback, Action explosionsEndCallback, AbstractPlayerController[] players, float bossDeathTime, bool goToWinScreen, bool isMausoleum, bool isDevil, bool isTowerOfPower)
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.win_cr(knockoutSFXCoroutine, onBossDeathCallback, explosionsCallback, explosionsFalloffCallback, explosionsEndCallback, players, bossDeathTime, goToWinScreen, isMausoleum, isDevil, isTowerOfPower));
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00087104 File Offset: 0x00085304
	public IEnumerator win_cr(IEnumerator knockoutSFXCoroutine, Action onBossDeathCallback, Action explosionsCallback, Action explosionsFalloffCallback, Action explosionsEndCallback, AbstractPlayerController[] players, float bossDeathTime, bool goToWinScreen, bool isMausoleum, bool isDevil, bool isTowerOfPower)
	{
		PauseManager.Pause();
		LevelKOAnimation koAnim = LevelKOAnimation.Create(isMausoleum);
		if (Level.IsChessBoss)
		{
			AudioManager.StartBGMAlternate(0);
		}
		if (Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			AudioManager.StartBGMAlternate(2);
		}
		base.StartCoroutine(knockoutSFXCoroutine);
		yield return koAnim.StartCoroutine(koAnim.anim_cr());
		PauseManager.Unpause();
		explosionsCallback();
		CupheadTime.SetAll(1f);
		if (!isMausoleum)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.OnLevelWin();
				}
			}
		}
		if (onBossDeathCallback != null)
		{
			onBossDeathCallback();
		}
		yield return new WaitForSeconds(bossDeathTime + 0.3f);
		foreach (AbstractProjectile abstractProjectile in Object.FindObjectsOfType<AbstractProjectile>())
		{
			abstractProjectile.OnLevelEnd();
		}
		if (Level.IsTowerOfPower)
		{
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne);
			if (PlayerManager.Multiplayer)
			{
				TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo);
			}
		}
		else if (Level.IsDicePalace && !Level.IsDicePalaceMain)
		{
			DicePalaceMainLevelGameInfo.SetPlayersStats();
		}
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade;
		SceneLoader.properties.transitionStartTime = 3f;
		if (Level.IsChessBoss || Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			yield return new WaitForSeconds(2f);
		}
		if (goToWinScreen)
		{
			SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		}
		else if (Level.IsTowerOfPower)
		{
			SceneLoader.ContinueTowerOfPower();
		}
		else if (Level.IsGraveyard)
		{
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
		}
		else if (Level.IsChessBoss)
		{
			if (SceneLoader.CurrentContext is GauntletContext)
			{
				int currentIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, Level.Current.CurrentLevel);
				int num = MathUtilities.NextIndex(currentIndex, Level.kingOfGamesLevels.Length);
				if (num == 0)
				{
					SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, new GauntletContext(true));
				}
				else
				{
					Levels level = Level.kingOfGamesLevels[num];
					SceneLoader.Transition transitionStart = SceneLoader.Transition.Fade;
					GauntletContext context = new GauntletContext(false);
					SceneLoader.LoadLevel(level, transitionStart, SceneLoader.Icon.Hourglass, context);
				}
			}
			else
			{
				SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			}
		}
		else if (!isMausoleum)
		{
			SceneLoader.ReloadLevel();
		}
		yield return new WaitForSeconds(2.5f);
		explosionsEndCallback();
		yield break;
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x00087154 File Offset: 0x00085354
	public static void Lose(bool isMausoleum, bool secretTriggered)
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.lose_cr(isMausoleum, secretTriggered));
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x00087178 File Offset: 0x00085378
	public IEnumerator lose_cr(bool isMausoleum, bool secretTriggered)
	{
		if (isMausoleum)
		{
			AudioManager.Play("level_announcer_fail");
		}
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		LevelGameOverGUI.Current.In(secretTriggered);
		if (Level.IsChessBoss)
		{
			yield return new WaitForSeconds(1f);
			AudioManager.StartBGMAlternate(1);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0008719C File Offset: 0x0008539C
	public static void PlayerJoined()
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.playerJoined_cr());
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x000871BC File Offset: 0x000853BC
	public IEnumerator playerJoined_cr()
	{
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		yield return new WaitForSeconds(1f);
		yield return new WaitForSeconds(1f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x04000A71 RID: 2673
	public const string NAME = "LEVEL_END_CONTROLER";

	// Token: 0x04000A72 RID: 2674
	public const float WIN_FADE_TIME = 3f;

	// Token: 0x04000A73 RID: 2675
	public const float JOIN_WAIT = 1f;
}
