using System;

// Token: 0x020004D2 RID: 1234
public class MapPauseUI : LevelPauseGUI
{
	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06003335 RID: 13109 RVA: 0x000F3A24 File Offset: 0x000F1C24
	public override bool CanPause
	{
		get
		{
			return base.state != AbstractPauseGUI.State.Animating && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (SpeechBubble.Instance == null || (SpeechBubble.Instance != null && SpeechBubble.Instance.displayState != SpeechBubble.DisplayState.WaitForSelection)) && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}
}
