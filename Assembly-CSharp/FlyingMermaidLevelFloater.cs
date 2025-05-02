using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class FlyingMermaidLevelFloater : AbstractPausableComponent
{
	// Token: 0x06001D32 RID: 7474 RVA: 0x00018BD5 File Offset: 0x00016DD5
	public void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x000B031C File Offset: 0x000AE51C
	public IEnumerator loop_cr()
	{
		float waveWidth = 0f;
		if (this.trackingWater != null)
		{
			SpriteRenderer component = this.trackingWater.GetComponent<SpriteRenderer>();
			waveWidth = component.bounds.size.x / 2f;
		}
		float lastY = base.transform.localPosition.y;
		float relativeBobY = 0f;
		float originY = base.transform.localPosition.y;
		float t = 0f;
		float frameTime = 0f;
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				t += CupheadTime.Delta;
				frameTime += CupheadTime.Delta;
				while (frameTime > 0.0416666679f)
				{
					frameTime -= 0.0416666679f;
					Quaternion rotation = base.transform.rotation;
					float num;
					float num2;
					if (this.trackingWater != null)
					{
						float x = this.trackingWater.transform.position.x;
						float x2 = base.transform.position.x;
						num = 1f - Mathf.Cos((x2 - x) * 2f * (3.14159274f / waveWidth));
						num2 = Mathf.Sin((x2 - x) * 2f * (3.14159274f / waveWidth));
					}
					else
					{
						num = 1f - Mathf.Cos(t * this.bobSpeed * 2f * 3.14159274f);
						num2 = Mathf.Sin(t * this.bobSpeed * 2f * 3.14159274f);
					}
					relativeBobY = num * this.bobAmount;
					originY += base.transform.localPosition.y - lastY;
					rotation = Quaternion.AngleAxis(this.defaultRotation + num2 * this.rotateAmount, Vector3.forward);
					base.transform.SetPosition(null, new float?(originY + relativeBobY), null);
					base.transform.rotation = rotation;
					lastY = base.transform.localPosition.y;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040017CD RID: 6093
	public const float BOB_FRAME_TIME = 0.0416666679f;

	// Token: 0x040017CE RID: 6094
	public float bobAmount;

	// Token: 0x040017CF RID: 6095
	public float rotateAmount;

	// Token: 0x040017D0 RID: 6096
	public float defaultRotation;

	// Token: 0x040017D1 RID: 6097
	public GameObject trackingWater;

	// Token: 0x040017D2 RID: 6098
	public float bobSpeed;
}
