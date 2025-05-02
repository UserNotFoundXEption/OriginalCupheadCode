using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class AudioNoiseHandler : AbstractMonoBehaviour
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x00072734 File Offset: 0x00070934
	public static AudioNoiseHandler Instance
	{
		get
		{
			if (AudioNoiseHandler.noiseHandler == null)
			{
				AudioNoiseHandler audioNoiseHandler = Object.Instantiate(Resources.Load("Audio/AudioNoiseHandler")) as AudioNoiseHandler;
				audioNoiseHandler.name = "NoiseHandler";
			}
			return AudioNoiseHandler.noiseHandler;
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000072F1 File Offset: 0x000054F1
	public override void Awake()
	{
		base.Awake();
		AudioNoiseHandler.noiseHandler = this;
		base.GetComponent<AudioSource>().ignoreListenerPause = true;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00007316 File Offset: 0x00005516
	public void OpticalSound()
	{
		AudioManager.Play("optical_start");
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00007322 File Offset: 0x00005522
	public void BoingSound()
	{
		AudioManager.Play("worldmap_level_select");
	}

	// Token: 0x04000599 RID: 1433
	public static AudioNoiseHandler noiseHandler;

	// Token: 0x0400059A RID: 1434
	public const string PATH = "Audio/AudioNoiseHandler";
}
