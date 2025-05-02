using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public abstract class ChessLevel : Level
{
	// Token: 0x060012B4 RID: 4788 RVA: 0x0000FC69 File Offset: 0x0000DE69
	public ChessLevel()
	{
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0000FC71 File Offset: 0x0000DE71
	public override void Awake()
	{
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(Level.Mode.Normal);
		base.Awake();
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0000FC8A File Offset: 0x0000DE8A
	public override void OnDestroy()
	{
		base.OnDestroy();
		Level.SetCurrentMode(this.originalMode);
		this.levelIntroAnimation = null;
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0000FCA4 File Offset: 0x0000DEA4
	public override LevelIntroAnimation CreateLevelIntro(Action callback)
	{
		return LevelIntroAnimation.CreateCustom(this.levelIntroAnimation, callback);
	}

	// Token: 0x04000F05 RID: 3845
	[SerializeField]
	public LevelIntroAnimation levelIntroAnimation;

	// Token: 0x04000F06 RID: 3846
	public Level.Mode originalMode;
}
