using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class TrainLevelForegroundDynamicizer : AbstractPausableComponent
{
	// Token: 0x060029D0 RID: 10704 RVA: 0x00023366 File Offset: 0x00021566
	public override void Awake()
	{
		base.Awake();
		this.ResetPositions();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000D2B7C File Offset: 0x000D0D7C
	public void ResetPositions()
	{
		foreach (SpriteRenderer spriteRenderer in this.sprites)
		{
			spriteRenderer.transform.SetLocalPosition(new float?(-1280f), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x000D2BD4 File Offset: 0x000D0DD4
	public IEnumerator loop_cr()
	{
		for (;;)
		{
			this.ResetPositions();
			float t = 0f;
			Transform trans = this.sprites[Random.Range(0, this.sprites.Length)].transform;
			trans.SetScale(new float?((float)((Random.value <= 0.5f) ? -1 : 1)), new float?(1f), new float?(1f));
			while (t < 1.3f)
			{
				float x = Mathf.Lerp(1280f, -1280f, t / 1.3f);
				trans.SetLocalPosition(new float?(x), new float?(0f), new float?(0f));
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, Random.Range(4f, 4f));
		}
		yield break;
	}

	// Token: 0x040022FE RID: 8958
	public const float X_OUT = -1280f;

	// Token: 0x040022FF RID: 8959
	public const float X_IN = 1280f;

	// Token: 0x04002300 RID: 8960
	public const float DELAY_MIN = 1f;

	// Token: 0x04002301 RID: 8961
	public const float DELAY_MAX = 4f;

	// Token: 0x04002302 RID: 8962
	public const float TIME = 1.3f;

	// Token: 0x04002303 RID: 8963
	[SerializeField]
	public SpriteRenderer[] sprites;
}
