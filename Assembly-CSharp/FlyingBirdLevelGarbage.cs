using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000230 RID: 560
public class FlyingBirdLevelGarbage : BasicProjectile
{
	// Token: 0x060019C8 RID: 6600 RVA: 0x000A7054 File Offset: 0x000A5254
	public override void Start()
	{
		base.Start();
		if (!this.isBoot)
		{
			base.StartCoroutine(this.not_boot_cr());
		}
		else
		{
			base.animator.SetBool("OnClockwise", Rand.Bool());
		}
		base.StartCoroutine(this.change_layer_cr());
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000A70A8 File Offset: 0x000A52A8
	public IEnumerator not_boot_cr()
	{
		float frameTime = 0f;
		this.bootSpeed = ((!Rand.Bool()) ? 600f : 300f);
		this.bootSpeed = ((!Rand.Bool()) ? this.bootSpeed : (-this.bootSpeed));
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				frameTime -= 0.0416666679f;
				base.transform.Rotate(0f, 0f, this.bootSpeed * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000A70C4 File Offset: 0x000A52C4
	public IEnumerator change_layer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
		yield break;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00015FFA File Offset: 0x000141FA
	public override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x040014AC RID: 5292
	public const float ROTATE_FRAME_TIME = 0.0416666679f;

	// Token: 0x040014AD RID: 5293
	[SerializeField]
	public bool isBoot;

	// Token: 0x040014AE RID: 5294
	public float bootSpeed;
}
