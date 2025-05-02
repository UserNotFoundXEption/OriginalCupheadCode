using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class BaronessLevelJawbreakerGhost : AbstractCollidableObject
{
	// Token: 0x0600102F RID: 4143 RVA: 0x0000DAD1 File Offset: 0x0000BCD1
	public void Start()
	{
		base.StartCoroutine(this.deathrotation_cr());
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0008F628 File Offset: 0x0008D828
	public IEnumerator deathrotation_cr()
	{
		float frameTime = 0f;
		float t = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			this.deathPosition = new Vector3(base.transform.position.x + Mathf.Sin(t) * this.sinWaveStrength * CupheadTime.Delta * 60f, base.transform.position.y + this.deathSpeed, 0f);
			t += CupheadTime.Delta;
			if (frameTime > 0.0833333358f)
			{
				frameTime -= 0.0833333358f;
				base.transform.up = (this.deathPosition - base.transform.position).normalized * CupheadTime.Delta;
			}
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position = this.deathPosition;
			}
			if (base.transform.position.y > 540f)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, CupheadTime.Delta);
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0000DAE0 File Offset: 0x0000BCE0
	public void Die()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000D27 RID: 3367
	public const float ROTATE_FRAME_TIME = 0.0833333358f;

	// Token: 0x04000D28 RID: 3368
	public Vector3 deathPosition;

	// Token: 0x04000D29 RID: 3369
	public float deathSpeed = 2.3f;

	// Token: 0x04000D2A RID: 3370
	public float sinWaveStrength = 0.4f;
}
