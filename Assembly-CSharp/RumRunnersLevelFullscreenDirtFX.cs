using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class RumRunnersLevelFullscreenDirtFX : Effect
{
	// Token: 0x06002487 RID: 9351 RVA: 0x000C41F4 File Offset: 0x000C23F4
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale, randomR);
		int num = base.animator.GetInteger("Effect");
		while (num == RumRunnersLevelFullscreenDirtFX.PreviousEffectA || num == RumRunnersLevelFullscreenDirtFX.PreviousEffectB)
		{
			num = Random.Range(0, base.animator.GetInteger("Count"));
			base.animator.SetInteger("Effect", num);
		}
		RumRunnersLevelFullscreenDirtFX.PreviousEffectB = RumRunnersLevelFullscreenDirtFX.PreviousEffectA;
		RumRunnersLevelFullscreenDirtFX.PreviousEffectA = num;
		base.animator.Update(0f);
		float y = base.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		if (num == 0 || num == 1)
		{
			base.StartCoroutine(this.fall_cr(this.loopDirtSpeed, y));
		}
		else
		{
			float currentClipLength = base.animator.GetCurrentClipLength(0);
			if (currentClipLength == 0f)
			{
				this.OnEffectComplete();
			}
			base.animator.Update(0f);
			float speed = (887.792847f + y) / currentClipLength;
			base.StartCoroutine(this.fall_cr(speed, y));
		}
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000C4314 File Offset: 0x000C2514
	public IEnumerator fall_cr(float speed, float spriteHeight)
	{
		while (base.transform.position.y > -360f - spriteHeight - 100f)
		{
			yield return null;
			Vector3 position = base.transform.position;
			position.y -= speed * CupheadTime.Delta;
			base.transform.position = position;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04001E37 RID: 7735
	public static int PreviousEffectA = -1;

	// Token: 0x04001E38 RID: 7736
	public static int PreviousEffectB = -1;

	// Token: 0x04001E39 RID: 7737
	[SerializeField]
	public float loopDirtSpeed;
}
