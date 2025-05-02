using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005A1 RID: 1441
public class AnimatedUISprite : MonoBehaviour
{
	// Token: 0x06003CE8 RID: 15592 RVA: 0x000312BE File Offset: 0x0002F4BE
	public void ResetAnimation()
	{
		this._currentSpriteIndex = 0;
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x000312C7 File Offset: 0x0002F4C7
	public void OnEnable()
	{
		this.ResetAnimation();
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x000312CF File Offset: 0x0002F4CF
	public void OnDisable()
	{
		this.ResetAnimation();
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x001173C4 File Offset: 0x001155C4
	public void Update()
	{
		if (!this.Loop && this._currentSpriteIndex >= this.Sprites.Length - 1)
		{
			return;
		}
		if (this.Animating && Time.time >= this._lastRefreshTime + 1f / (float)this.FrameRate)
		{
			this._currentSpriteIndex++;
			if (this._currentSpriteIndex >= this.Sprites.Length)
			{
				this._currentSpriteIndex = 0;
			}
			this.UIImage.sprite = this.Sprites[this._currentSpriteIndex];
			this._lastRefreshTime = Time.time;
		}
	}

	// Token: 0x04003066 RID: 12390
	public bool Animating;

	// Token: 0x04003067 RID: 12391
	public bool Loop;

	// Token: 0x04003068 RID: 12392
	public Image UIImage;

	// Token: 0x04003069 RID: 12393
	public Sprite[] Sprites;

	// Token: 0x0400306A RID: 12394
	public int FrameRate = 24;

	// Token: 0x0400306B RID: 12395
	public int _currentSpriteIndex;

	// Token: 0x0400306C RID: 12396
	public float _lastRefreshTime;
}
