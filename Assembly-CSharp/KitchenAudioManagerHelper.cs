using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class KitchenAudioManagerHelper : MonoBehaviour
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0001A249 File Offset: 0x00018449
	public static KitchenAudioManagerHelper Instance
	{
		get
		{
			return KitchenAudioManagerHelper._instance;
		}
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x000B526C File Offset: 0x000B346C
	public void Awake()
	{
		if (KitchenAudioManagerHelper._instance != null && KitchenAudioManagerHelper._instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			KitchenAudioManagerHelper._instance = this;
			this.sceneName = Scenes.scene_level_kitchen.ToString();
			base.transform.parent = null;
			Object.DontDestroyOnLoad(base.gameObject);
			SceneLoader.instance.ResetBgmVolume();
		}
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x000B52E8 File Offset: 0x000B34E8
	public IEnumerator exit_level_cr()
	{
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x000B5304 File Offset: 0x000B3504
	public void Update()
	{
		if (this.exitingLevel)
		{
			return;
		}
		if (SceneLoader.CurrentlyLoading && SceneLoader.SceneName != this.sceneName && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			this.exitingLevel = true;
			base.StartCoroutine(this.exit_level_cr());
		}
	}

	// Token: 0x0400195A RID: 6490
	public bool exitingLevel;

	// Token: 0x0400195B RID: 6491
	public string sceneName;

	// Token: 0x0400195C RID: 6492
	public bool started;

	// Token: 0x0400195D RID: 6493
	public static KitchenAudioManagerHelper _instance;
}
