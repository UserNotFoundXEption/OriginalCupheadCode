using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000FB RID: 251
public abstract class AbstractLevelEntity : AbstractCollidableObject
{
	// Token: 0x06000BB0 RID: 2992 RVA: 0x0000A604 File Offset: 0x00008804
	public AbstractLevelEntity()
	{
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0000A60C File Offset: 0x0000880C
	public bool canParry
	{
		get
		{
			return this._canParry;
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0000A614 File Offset: 0x00008814
	public virtual void OnParry(AbstractPlayerController player)
	{
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00080F9C File Offset: 0x0007F19C
	public IEnumerator flash_cr(Color start, Color end, float time, Action onComplete = null)
	{
		SpriteRenderer renderer = base.GetComponent<SpriteRenderer>();
		renderer.color = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			renderer.color = Color.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		renderer.color = end;
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00080FD4 File Offset: 0x0007F1D4
	public IEnumerator dieFlash_cr()
	{
		for (int i = 0; i < 4; i++)
		{
			yield return base.StartCoroutine(this.flash_cr(Color.red, Color.black, 0.3f, null));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x04000963 RID: 2403
	public const float DIE_FLASH_TIME = 0.3f;

	// Token: 0x04000964 RID: 2404
	public const float DIE_FLASH_DELAY = 0.2f;

	// Token: 0x04000965 RID: 2405
	public const int DIE_FLASH_LOOPS = 4;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	public bool _canParry;
}
