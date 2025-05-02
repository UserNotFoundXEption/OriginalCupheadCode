using System;
using UnityEngine;

// Token: 0x020002D8 RID: 728
public class OldManLevelBubbleController : MonoBehaviour
{
	// Token: 0x06002038 RID: 8248 RVA: 0x000B7550 File Offset: 0x000B5750
	public void Start()
	{
		for (int i = 0; i < this.animators.Length; i++)
		{
			this.animators[i].Play("Bubble", 0, 1f);
		}
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000B7590 File Offset: 0x000B5790
	public void FixedUpdate()
	{
		this.timer -= CupheadTime.FixedDelta;
		if (this.timer <= 0f)
		{
			int num = Random.Range(0, this.animators.Length);
			if (this.animators[num].GetCurrentAnimatorStateInfo(0).normalizedTime > this.minTimeToRepeat)
			{
				this.animators[num].Play("Bubble", 0, 0f);
			}
			this.timer = Random.Range(this.minDelay, this.maxDelay);
		}
	}

	// Token: 0x04001A3B RID: 6715
	[SerializeField]
	public float minDelay = 0.05f;

	// Token: 0x04001A3C RID: 6716
	[SerializeField]
	public float maxDelay = 0.5f;

	// Token: 0x04001A3D RID: 6717
	[SerializeField]
	public float minTimeToRepeat = 2f;

	// Token: 0x04001A3E RID: 6718
	[SerializeField]
	public Animator[] animators;

	// Token: 0x04001A3F RID: 6719
	public float timer;
}
