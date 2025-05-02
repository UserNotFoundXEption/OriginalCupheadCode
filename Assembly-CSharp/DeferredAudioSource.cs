using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class DeferredAudioSource : MonoBehaviour
{
	// Token: 0x0600069E RID: 1694 RVA: 0x0006FF0C File Offset: 0x0006E10C
	public void Initialize()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (!DLCManager.DLCEnabled() && AssetLoader<AudioClip>.IsDLCAsset(this.audioClipName))
		{
			component.clip = null;
		}
		else
		{
			component.clip = AssetLoader<AudioClip>.GetCachedAsset(this.audioClipName);
		}
		if (this.playOnInitialize)
		{
			component.Play();
		}
	}

	// Token: 0x040004F1 RID: 1265
	[SerializeField]
	public string audioClipName;

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	public bool playOnInitialize;
}
