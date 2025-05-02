using System;
using System.Collections;

// Token: 0x020000B4 RID: 180
public class CreditsCutscene : Cutscene
{
	// Token: 0x06000862 RID: 2146 RVA: 0x000080B7 File Offset: 0x000062B7
	public override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.music_cr());
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00075E98 File Offset: 0x00074098
	public IEnumerator music_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (CreditsScreen.goodEnding)
		{
			AudioManager.PlayBGM();
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "GoodEnding");
		}
		else
		{
			AudioManager.PlayBGMPlaylistManually(true);
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "BadEnding");
		}
		yield break;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x000080DC File Offset: 0x000062DC
	public override void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Ending", true);
	}
}
