using System;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class PlatformingLevelIntroAnimation : AbstractLevelHUDComponent
{
	// Token: 0x06002FB4 RID: 12212 RVA: 0x000E2938 File Offset: 0x000E0B38
	public static PlatformingLevelIntroAnimation Create(Action callback)
	{
		PlatformingLevelIntroAnimation platformingLevelIntroAnimation = Object.Instantiate<PlatformingLevelIntroAnimation>(Level.Current.LevelResources.platformingIntro);
		platformingLevelIntroAnimation.callback = callback;
		return platformingLevelIntroAnimation;
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x00027BA8 File Offset: 0x00025DA8
	public override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
		base.transform.SetParent(Camera.main.transform, false);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x00027BD8 File Offset: 0x00025DD8
	public void StartLevel()
	{
		if (this.callback != null)
		{
			this.callback();
		}
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x00027BF0 File Offset: 0x00025DF0
	public void OnAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x00027BFD File Offset: 0x00025DFD
	public void Play()
	{
		base.GetComponent<Animator>().Play("Intro");
	}

	// Token: 0x04002787 RID: 10119
	public Action callback;
}
