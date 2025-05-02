using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class AbstractPausableComponent : AbstractMonoBehaviour
{
	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06003436 RID: 13366 RVA: 0x0002AE58 File Offset: 0x00029058
	public virtual Transform emitTransform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x0002AE60 File Offset: 0x00029060
	public override void Awake()
	{
		base.Awake();
		PauseManager.AddChild(this);
		this.preEnabled = base.enabled;
		this.emitAudioFromObject = new SoundEmitter(this);
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x0002AE86 File Offset: 0x00029086
	public virtual void OnDestroy()
	{
		PauseManager.RemoveChild(this);
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x0002AE8E File Offset: 0x0002908E
	public virtual void OnPause()
	{
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x0002AE90 File Offset: 0x00029090
	public virtual void OnUnpause()
	{
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x000F5F34 File Offset: 0x000F4134
	public IEnumerator WaitForPause_CR()
	{
		while (PauseManager.state == PauseManager.State.Paused)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x0002AE92 File Offset: 0x00029092
	public virtual void OnLevelEnd()
	{
		if (this != null)
		{
			this.StopAllCoroutines();
			base.enabled = false;
		}
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x0002AEAD File Offset: 0x000290AD
	public void EmitSound(string key)
	{
		AudioManager.FollowObject(key, this.emitTransform);
	}

	// Token: 0x04002AF9 RID: 11001
	[NonSerialized]
	public bool preEnabled;

	// Token: 0x04002AFA RID: 11002
	public SoundEmitter emitAudioFromObject;
}
