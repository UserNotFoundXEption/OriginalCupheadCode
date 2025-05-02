using System;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class WinScreenCharacterAnimation : AbstractMonoBehaviour
{
	// Token: 0x06003DB3 RID: 15795 RVA: 0x00119E30 File Offset: 0x00118030
	public void Start()
	{
		this.blinkLayer.enabled = false;
		if (this.blinkLayer2 != null)
		{
			this.blinkLayer2.enabled = false;
		}
		this.results.SetBool("pickedA", Rand.Bool());
		this.singleBlinkAmount = Random.Range(0, 4) + 4;
		this.doubleBlinkAmount = Random.Range(0, 10) + 16;
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00119E9C File Offset: 0x0011809C
	public void EndCycle()
	{
		this.blinkLayer.enabled = false;
		if (this.blinkLayer2 != null)
		{
			this.blinkLayer2.enabled = false;
		}
		if (this.singleCount < this.singleBlinkAmount)
		{
			this.singleCount++;
		}
		else
		{
			if (this.blinkLayer2 != null)
			{
				if (this.is2Player)
				{
					this.blinkLayer2.enabled = true;
				}
				else
				{
					this.blinkLayer.enabled = true;
				}
				this.is2Player = !this.is2Player;
			}
			else
			{
				this.blinkLayer.enabled = true;
			}
			this.singleCount = 0;
			this.singleBlinkAmount = Random.Range(0, 4) + 4;
		}
		if (this.blinkLayer2 != null)
		{
			if (this.doubleCount < this.doubleBlinkAmount)
			{
				this.doubleCount++;
			}
			else
			{
				this.blinkLayer2.enabled = true;
				this.blinkLayer.enabled = true;
				this.doubleCount = 0;
				this.doubleBlinkAmount = Random.Range(0, 10) + 16;
			}
		}
	}

	// Token: 0x0400317E RID: 12670
	[SerializeField]
	public Animator results;

	// Token: 0x0400317F RID: 12671
	[SerializeField]
	public SpriteRenderer blinkLayer;

	// Token: 0x04003180 RID: 12672
	[SerializeField]
	public SpriteRenderer blinkLayer2;

	// Token: 0x04003181 RID: 12673
	public bool is2Player;

	// Token: 0x04003182 RID: 12674
	public int singleBlinkAmount;

	// Token: 0x04003183 RID: 12675
	public int doubleBlinkAmount;

	// Token: 0x04003184 RID: 12676
	public int singleCount;

	// Token: 0x04003185 RID: 12677
	public int doubleCount;

	// Token: 0x04003186 RID: 12678
	public bool p1Turn;
}
