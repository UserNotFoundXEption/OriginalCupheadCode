using System;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class OldManLevelBeardController : MonoBehaviour
{
	// Token: 0x0600202D RID: 8237 RVA: 0x000B7450 File Offset: 0x000B5650
	public void CueRuffle(int which)
	{
		if (this.ruffles[which] != null && this.ruffles[which].GetCurrentAnimatorStateInfo(0).IsName("None"))
		{
			this.ruffleCued[which] = true;
		}
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x0001B491 File Offset: 0x00019691
	public void FixedUpdate()
	{
		this.frameTimer += CupheadTime.FixedDelta;
		if (this.frameTimer > 0.0416666679f)
		{
			this.frameTimer -= 0.0416666679f;
			this.Step();
		}
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x0001B4CD File Offset: 0x000196CD
	public void Step()
	{
		this.frameNum = (this.frameNum + 1) % 6;
		this.rend.sprite = this.sprites[this.frameNum / 2];
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000B749C File Offset: 0x000B569C
	public void LateUpdate()
	{
		for (int i = 0; i < this.ruffleCued.Length; i++)
		{
			if (this.ruffleCued[i])
			{
				int num = this.ruffleStartFrames[i] - this.frameNum;
				if (num == 0 || num == 1)
				{
					float num2 = this.frameTimer * 24f;
					float num3 = ((float)num + num2) * 0.0714285746f;
					this.ruffles[i].Play("Ruffle", 0, num3);
					this.ruffles[i].Update(0f);
					this.ruffleCued[i] = false;
				}
			}
		}
	}

	// Token: 0x04001A29 RID: 6697
	public const float RUFFLE_FRAME_TIME_NORMALIZED = 0.0714285746f;

	// Token: 0x04001A2A RID: 6698
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001A2B RID: 6699
	[SerializeField]
	public Sprite[] sprites;

	// Token: 0x04001A2C RID: 6700
	[SerializeField]
	public Animator[] ruffles;

	// Token: 0x04001A2D RID: 6701
	[SerializeField]
	public int[] ruffleStartFrames;

	// Token: 0x04001A2E RID: 6702
	public bool[] ruffleCued = new bool[10];

	// Token: 0x04001A2F RID: 6703
	public float frameTimer;

	// Token: 0x04001A30 RID: 6704
	public int frameNum;
}
