using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class PlatformingLevelEnd : AbstractMonoBehaviour
{
	// Token: 0x06002F9F RID: 12191 RVA: 0x00027B14 File Offset: 0x00025D14
	public override void Awake()
	{
		base.Awake();
		base.transform.SetAsFirstSibling();
		base.gameObject.name = "PLATFORMING_LEVEL_END_CONTROLER";
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000E24D0 File Offset: 0x000E06D0
	public static PlatformingLevelEnd Create()
	{
		GameObject gameObject = new GameObject();
		return gameObject.AddComponent<PlatformingLevelEnd>();
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000E24EC File Offset: 0x000E06EC
	public static void Win()
	{
		PlatformingLevelEnd platformingLevelEnd = PlatformingLevelEnd.Create();
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.win_cr());
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x00027B37 File Offset: 0x00025D37
	public void OnWinComplete()
	{
		this.winReadyToExit = true;
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x000E250C File Offset: 0x000E070C
	public IEnumerator win_cr()
	{
		PlatformingLevelExit.OnWinCompleteEvent += this.OnWinComplete;
		PauseManager.Pause();
		PlatformingLevelWinAnimation bravoAnimation = PlatformingLevelWinAnimation.Create();
		AudioManager.Play("platforming_announcer_bravo");
		while (bravoAnimation.CurrentState == PlatformingLevelWinAnimation.State.Paused)
		{
			yield return null;
		}
		PauseManager.Unpause();
		CupheadTime.SetAll(1f);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.OnLevelWin();
			}
		}
		foreach (AbstractProjectile abstractProjectile in Object.FindObjectsOfType<AbstractProjectile>())
		{
			abstractProjectile.OnLevelEnd();
		}
		foreach (AbstractPlatformingLevelEnemy abstractPlatformingLevelEnemy in Object.FindObjectsOfType<AbstractPlatformingLevelEnemy>())
		{
			abstractPlatformingLevelEnemy.OnLevelEnd();
		}
		yield return null;
		while (!this.winReadyToExit)
		{
			yield return null;
		}
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade;
		SceneLoader.properties.transitionStartTime = 3f;
		SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		yield break;
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x000E2528 File Offset: 0x000E0728
	public static void Lose()
	{
		PlatformingLevelEnd platformingLevelEnd = PlatformingLevelEnd.Create();
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.lose_cr());
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000E2548 File Offset: 0x000E0748
	public IEnumerator lose_cr()
	{
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		LevelGameOverGUI.Current.In(false);
		yield return null;
		yield break;
	}

	// Token: 0x04002779 RID: 10105
	public const string NAME = "PLATFORMING_LEVEL_END_CONTROLER";

	// Token: 0x0400277A RID: 10106
	public const float WIN_FADE_TIME = 3f;

	// Token: 0x0400277B RID: 10107
	public bool winReadyToExit;
}
