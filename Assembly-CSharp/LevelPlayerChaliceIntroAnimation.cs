using System;
using UnityEngine;

// Token: 0x0200050F RID: 1295
public class LevelPlayerChaliceIntroAnimation : Effect
{
	// Token: 0x06003646 RID: 13894 RVA: 0x000FE078 File Offset: 0x000FC278
	public LevelPlayerChaliceIntroAnimation Create(Vector3 position, bool isMugman, bool isScared)
	{
		LevelPlayerChaliceIntroAnimation levelPlayerChaliceIntroAnimation = base.Create(position) as LevelPlayerChaliceIntroAnimation;
		levelPlayerChaliceIntroAnimation.SetSprites(isMugman);
		string text = "Intro_CH_MM" + ((!isScared) ? "_Hold" : "_Scared");
		levelPlayerChaliceIntroAnimation.animator.Play(text);
		return levelPlayerChaliceIntroAnimation;
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x0002C79F File Offset: 0x0002A99F
	public void EndHold()
	{
		base.animator.SetTrigger("Continue");
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x000FE0C8 File Offset: 0x000FC2C8
	public void SetSprites(bool isMugman)
	{
		if (Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			this.cuphead.SetActive(false);
			this.mugman.SetActive(false);
		}
		else
		{
			this.cuphead.SetActive(!isMugman);
			this.mugman.SetActive(isMugman);
		}
	}

	// Token: 0x04002C0F RID: 11279
	[SerializeField]
	public GameObject cuphead;

	// Token: 0x04002C10 RID: 11280
	[SerializeField]
	public GameObject mugman;
}
