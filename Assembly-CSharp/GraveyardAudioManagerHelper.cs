using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class GraveyardAudioManagerHelper : MonoBehaviour
{
	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00019F19 File Offset: 0x00018119
	public static GraveyardAudioManagerHelper Instance
	{
		get
		{
			return GraveyardAudioManagerHelper._instance;
		}
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x000B3468 File Offset: 0x000B1668
	public void Awake()
	{
		if (GraveyardAudioManagerHelper._instance != null && GraveyardAudioManagerHelper._instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			GraveyardAudioManagerHelper._instance = this;
			this.sceneName = Scenes.scene_level_graveyard.ToString();
			base.transform.parent = null;
			Object.DontDestroyOnLoad(base.gameObject);
			SceneLoader.instance.ResetBgmVolume();
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x000B34E8 File Offset: 0x000B16E8
	public IEnumerator exit_level_cr()
	{
		AudioManager.ChangeBGMPitch(0.7f, 5f);
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		AudioManager.ChangeBGMPitch(1f, 0f);
		yield return new WaitForEndOfFrame();
		SceneLoader.instance.ResetBgmVolume();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000B3504 File Offset: 0x000B1704
	public void Update()
	{
		if (this.exitingLevel)
		{
			return;
		}
		if (SceneLoader.CurrentlyLoading && SceneLoader.SceneName != this.sceneName)
		{
			this.exitingLevel = true;
			base.StartCoroutine(this.exit_level_cr());
		}
	}

	// Token: 0x040018FD RID: 6397
	public bool exitingLevel;

	// Token: 0x040018FE RID: 6398
	public string sceneName;

	// Token: 0x040018FF RID: 6399
	public bool started;

	// Token: 0x04001900 RID: 6400
	public static GraveyardAudioManagerHelper _instance;
}
