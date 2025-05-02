using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class CircusPlatformingLevelBell : PlatformingLevelPathMovementEnemy
{
	// Token: 0x06002CEA RID: 11498 RVA: 0x0002585B File Offset: 0x00023A5B
	public override void OnParry(AbstractPlayerController player)
	{
		base.animator.Play("Ring");
		AudioManager.Play("circus_bell_ding");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x00025890 File Offset: 0x00023A90
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x000DB6C8 File Offset: 0x000D98C8
	public IEnumerator timer_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x00025892 File Offset: 0x00023A92
	public override void CalculateCollider()
	{
	}

	// Token: 0x04002525 RID: 9509
	public float coolDown = 0.4f;
}
