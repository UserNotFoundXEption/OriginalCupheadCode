using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class MapNPCMusic : MonoBehaviour
{
	// Token: 0x06003144 RID: 12612 RVA: 0x00029044 File Offset: 0x00027244
	public void Start()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x0002905C File Offset: 0x0002725C
	public void OnDestroy()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000E911C File Offset: 0x000E731C
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MinimalistMusic" && this.musicType == MapNPCMusic.MusicType.Minimalist)
		{
			PlayerData.Data.pianoAudioEnabled = true;
			PlayerData.SaveCurrentFile();
			Map.Current.OnNPCChangeMusic();
		}
		else if (message == "RegularMusic" && this.musicType == MapNPCMusic.MusicType.Regular)
		{
			PlayerData.Data.pianoAudioEnabled = false;
			PlayerData.SaveCurrentFile();
			Map.Current.OnNPCChangeMusic();
		}
	}

	// Token: 0x040028A3 RID: 10403
	[SerializeField]
	public MapNPCMusic.MusicType musicType;

	// Token: 0x02001104 RID: 4356
	public enum MusicType
	{
		// Token: 0x0400785B RID: 30811
		Regular,
		// Token: 0x0400785C RID: 30812
		Minimalist
	}
}
