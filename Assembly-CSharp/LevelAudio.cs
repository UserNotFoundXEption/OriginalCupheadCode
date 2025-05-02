using System;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class LevelAudio : AbstractMonoBehaviour
{
	// Token: 0x06000D3C RID: 3388 RVA: 0x000869F4 File Offset: 0x00084BF4
	public static LevelAudio Create()
	{
		return Object.Instantiate<LevelAudio>(Level.Current.LevelResources.levelAudio);
	}
}
