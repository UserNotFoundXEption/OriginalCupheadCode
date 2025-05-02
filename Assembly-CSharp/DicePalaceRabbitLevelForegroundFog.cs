using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class DicePalaceRabbitLevelForegroundFog : AbstractPausableComponent
{
	// Token: 0x06001797 RID: 6039 RVA: 0x000A21B8 File Offset: 0x000A03B8
	public void Update()
	{
		this.angle += this.speed * CupheadTime.Delta;
		Vector3 vector;
		vector..ctor(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 vector2;
		vector2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += vector + vector2;
		if (this.fadingOut)
		{
			if (this.time < this.fadeTime)
			{
				if (base.GetComponent<SpriteRenderer>().color.a > 0.5f)
				{
					base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - this.time / this.fadeTime);
				}
				this.time += CupheadTime.Delta;
			}
			else
			{
				this.fadingOut = !this.fadingOut;
				this.time = 0f;
			}
		}
		else if (this.time < this.fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f + this.time / this.fadeTime);
			this.time += CupheadTime.Delta;
		}
		else
		{
			this.fadingOut = !this.fadingOut;
			this.time = 0f;
		}
	}

	// Token: 0x0400132A RID: 4906
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x0400132B RID: 4907
	public float loopSize = 5f;

	// Token: 0x0400132C RID: 4908
	public float speed = 2f;

	// Token: 0x0400132D RID: 4909
	public float angle;

	// Token: 0x0400132E RID: 4910
	public float time;

	// Token: 0x0400132F RID: 4911
	public float fadeTime = 5f;

	// Token: 0x04001330 RID: 4912
	public bool fadingOut = true;
}
