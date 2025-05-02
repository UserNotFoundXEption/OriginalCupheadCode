using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class OldManLevelStomachCeiling : MonoBehaviour
{
	// Token: 0x0600214A RID: 8522 RVA: 0x000BA264 File Offset: 0x000B8464
	public void Update()
	{
		this.currentPosition = (int)(Mathf.Clamp(this.leader.GetPosition(), 0f, 0.99f) * (float)this.sprites.Length);
		this.timeSinceChangeFrame += CupheadTime.Delta;
		if (this.lastPosition != this.currentPosition)
		{
			this.lastPosition = this.currentPosition;
			this.timeSinceChangeFrame = 0f;
		}
		this.offset = (int)(this.timeSinceChangeFrame / 0.166666672f) % 2 * (int)(-(int)Mathf.Sign(this.leader.GetPosition() - 0.5f));
		this.rend.sprite = this.sprites[this.currentPosition + this.offset];
	}

	// Token: 0x04001B6C RID: 7020
	public const float MAX_FRAME_TIME = 0.166666672f;

	// Token: 0x04001B6D RID: 7021
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001B6E RID: 7022
	[SerializeField]
	public Sprite[] sprites;

	// Token: 0x04001B6F RID: 7023
	[SerializeField]
	public OldManLevelGnomeLeader leader;

	// Token: 0x04001B70 RID: 7024
	public int currentPosition;

	// Token: 0x04001B71 RID: 7025
	public int lastPosition;

	// Token: 0x04001B72 RID: 7026
	public float timeSinceChangeFrame;

	// Token: 0x04001B73 RID: 7027
	public int offset;
}
