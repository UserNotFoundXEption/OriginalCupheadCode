using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class FlyingGenieLevelRing : BasicProjectile
{
	// Token: 0x06001CC9 RID: 7369 RVA: 0x000AF298 File Offset: 0x000AD498
	public override void Start()
	{
		base.Start();
		if (this.isMain)
		{
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play("Off");
		}
		else
		{
			base.StartCoroutine(this.fade_cr());
		}
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000AF2E4 File Offset: 0x000AD4E4
	public IEnumerator fade_cr()
	{
		float frameTime = 0f;
		float color = 1f;
		while (color > 0f)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, color);
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				color -= 0.05f;
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		this.OnComplete();
		yield return null;
		yield break;
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x00018671 File Offset: 0x00016871
	public void OnComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x0001867E File Offset: 0x0001687E
	public void DisableCollision()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04001766 RID: 5990
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04001767 RID: 5991
	public bool isMain;
}
