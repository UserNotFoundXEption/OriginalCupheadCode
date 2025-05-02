using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class AudioIgnorePauseBehaviour : AbstractMonoBehaviour
{
	// Token: 0x060006C6 RID: 1734 RVA: 0x00006D3A File Offset: 0x00004F3A
	public override void Awake()
	{
		base.Awake();
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource != null)
		{
			this.audioSource.ignoreListenerPause = true;
		}
	}

	// Token: 0x04000503 RID: 1283
	public AudioSource audioSource;
}
