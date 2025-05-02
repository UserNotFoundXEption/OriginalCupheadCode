using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class LevelIntroAnimation : AbstractLevelHUDComponent
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x0008720C File Offset: 0x0008540C
	public static LevelIntroAnimation Create(Action callback)
	{
		LevelIntroAnimation levelIntroAnimation = Object.Instantiate<LevelIntroAnimation>(Level.Current.LevelResources.levelIntro);
		levelIntroAnimation.callback = callback;
		return levelIntroAnimation;
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00087238 File Offset: 0x00085438
	public static LevelIntroAnimation CreateCustom(LevelIntroAnimation prefab, Action callback)
	{
		LevelIntroAnimation levelIntroAnimation = Object.Instantiate<LevelIntroAnimation>(prefab);
		levelIntroAnimation.callback = callback;
		return levelIntroAnimation;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0000B786 File Offset: 0x00009986
	public override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
		base.transform.SetParent(Camera.main.transform, false);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0000B7B6 File Offset: 0x000099B6
	public void StartLevel()
	{
		if (this.callback != null)
		{
			this.callback();
		}
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x0000B7CE File Offset: 0x000099CE
	public void OnAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0000B7DB File Offset: 0x000099DB
	public void Play()
	{
		base.GetComponent<Animator>().Play("Intro");
	}

	// Token: 0x04000A77 RID: 2679
	public Action callback;
}
