using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class CircusPlatformingLevelPoleHandler : AbstractPausableComponent
{
	// Token: 0x06002D40 RID: 11584 RVA: 0x00025C96 File Offset: 0x00023E96
	public void Start()
	{
		this.SetupBots();
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x000DC440 File Offset: 0x000DA640
	public void SetupBots()
	{
		this.poleBots = new List<CircusPlatformingLevelPoleBot>();
		float y = this.poleBot.GetComponent<BoxCollider2D>().size.y;
		for (int i = 0; i < this.poleBotCount; i++)
		{
			Vector2 vector;
			vector..ctor(this.poleRoot.transform.position.x, this.poleRoot.transform.position.y + y * 1.38f * (float)i);
			this.poleBots.Add(this.poleBot.Spawn(vector));
		}
		base.StartCoroutine(this.check_to_slide_cr());
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000DC4F8 File Offset: 0x000DA6F8
	public IEnumerator check_to_slide_cr()
	{
		int indexToSlide = 1000;
		for (;;)
		{
			for (int i = this.poleBots.Count - 1; i >= 0; i--)
			{
				if (this.poleBots[i].isDying)
				{
					this.poleBots.RemoveAt(i);
					indexToSlide = i;
					break;
				}
				if (i >= indexToSlide)
				{
					while (this.poleBots[i].isSliding)
					{
						yield return null;
					}
					this.poleBots[i].SlideDown();
				}
				if (i == 0)
				{
					indexToSlide = 1000;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400257C RID: 9596
	[SerializeField]
	public int poleBotCount;

	// Token: 0x0400257D RID: 9597
	[SerializeField]
	public Transform poleRoot;

	// Token: 0x0400257E RID: 9598
	[SerializeField]
	public CircusPlatformingLevelPoleBot poleBot;

	// Token: 0x0400257F RID: 9599
	public List<CircusPlatformingLevelPoleBot> poleBots;
}
