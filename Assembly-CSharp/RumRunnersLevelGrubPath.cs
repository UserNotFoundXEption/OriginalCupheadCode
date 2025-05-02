using System;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class RumRunnersLevelGrubPath : MonoBehaviour
{
	// Token: 0x060024B1 RID: 9393 RVA: 0x0001F048 File Offset: 0x0001D248
	public Vector2 GetPoint(float t)
	{
		return Vector2.LerpUnclamped(Vector2.LerpUnclamped(this.start, this.controlPoint, t), Vector2.LerpUnclamped(this.controlPoint, base.transform.position, t), t);
	}

	// Token: 0x04001E61 RID: 7777
	public Vector2 start;

	// Token: 0x04001E62 RID: 7778
	public Vector2 controlPoint;

	// Token: 0x04001E63 RID: 7779
	public float forceFGSet = 2f;
}
