using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AC RID: 1452
public class ScrollingBackgroundElevator : AbstractPausableComponent
{
	// Token: 0x06003D11 RID: 15633 RVA: 0x00117D44 File Offset: 0x00115F44
	public void SetUp(Vector3 direction, float speed)
	{
		if (this.isBackground)
		{
			this.startPos = this.firstSprite.transform.position;
		}
		else
		{
			this.startPos = base.transform.position + direction.normalized * -800f;
		}
		this.endPos = this.lastSprite.transform.position;
		this.direction = direction;
		this.speed = speed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x00117DD0 File Offset: 0x00115FD0
	public IEnumerator move_cr()
	{
		while (!this.ending)
		{
			if (this.isBackground)
			{
				while (base.transform.position != this.endPos && !this.ending)
				{
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.endPos, this.speed * CupheadTime.Delta);
					yield return null;
				}
			}
			else
			{
				while ((base.transform.position.y < this.endPos.y && base.transform.position.x > this.endPos.x && !this.ending) || (this.isClouds && this.ending))
				{
					base.transform.position -= this.direction * this.speed * CupheadTime.Delta;
					yield return null;
				}
			}
			if (!this.easingOut)
			{
				base.transform.position = this.startPos;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x0003147B File Offset: 0x0002F67B
	public void EaseoutSpeed(float time)
	{
		base.StartCoroutine(this.ease_speed_cr(time));
		this.easingOut = true;
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x00117DEC File Offset: 0x00115FEC
	public IEnumerator ease_speed_cr(float time)
	{
		float startSpeed = this.speed;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.speed = Mathf.Lerp(startSpeed, 0f, t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x00031492 File Offset: 0x0002F692
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(base.transform.position, 100f);
	}

	// Token: 0x04003093 RID: 12435
	[SerializeField]
	public bool isClouds;

	// Token: 0x04003094 RID: 12436
	[SerializeField]
	public bool isBackground;

	// Token: 0x04003095 RID: 12437
	[SerializeField]
	public SpriteRenderer firstSprite;

	// Token: 0x04003096 RID: 12438
	[SerializeField]
	public SpriteRenderer lastSprite;

	// Token: 0x04003097 RID: 12439
	public float speed;

	// Token: 0x04003098 RID: 12440
	public Vector3 startPos;

	// Token: 0x04003099 RID: 12441
	public Vector3 endPos;

	// Token: 0x0400309A RID: 12442
	public Vector3 direction;

	// Token: 0x0400309B RID: 12443
	public bool ending;

	// Token: 0x0400309C RID: 12444
	public bool easingOut;
}
