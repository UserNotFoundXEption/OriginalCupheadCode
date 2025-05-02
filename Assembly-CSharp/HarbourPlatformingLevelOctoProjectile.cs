using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class HarbourPlatformingLevelOctoProjectile : AbstractProjectile
{
	// Token: 0x06002E47 RID: 11847 RVA: 0x00026983 File Offset: 0x00024B83
	public override void Start()
	{
		base.Start();
		this.velocity.y = this.speedY;
		this.velocity.x = this.speedX;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x000DF2EC File Offset: 0x000DD4EC
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400265E RID: 9822
	[SerializeField]
	public float speedX;

	// Token: 0x0400265F RID: 9823
	[SerializeField]
	public float speedY;

	// Token: 0x04002660 RID: 9824
	[SerializeField]
	public float gravity;

	// Token: 0x04002661 RID: 9825
	public Vector2 velocity;
}
