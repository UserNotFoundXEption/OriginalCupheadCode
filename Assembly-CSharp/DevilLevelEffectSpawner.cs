using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class DevilLevelEffectSpawner : AbstractPausableComponent
{
	// Token: 0x06001461 RID: 5217 RVA: 0x00011366 File Offset: 0x0000F566
	public void Start()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x00099EF4 File Offset: 0x000980F4
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, this.waitTime.max));
		yield return null;
		for (;;)
		{
			this.effect = this.effectPrefab.Create(base.transform.position);
			this.effect.transform.parent = base.transform;
			while (this.effect != null)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.waitTime.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x00011375 File Offset: 0x0000F575
	public void KillSmoke()
	{
		this.StopAllCoroutines();
		if (this.isSmoke3)
		{
			base.StartCoroutine(this.fade_out_cr());
		}
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x00099F10 File Offset: 0x00098110
	public IEnumerator fade_out_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			if (this.effect != null)
			{
				this.effect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x00011395 File Offset: 0x0000F595
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x040010A9 RID: 4265
	[SerializeField]
	public bool isSmoke3;

	// Token: 0x040010AA RID: 4266
	public MinMax waitTime;

	// Token: 0x040010AB RID: 4267
	public Effect effectPrefab;

	// Token: 0x040010AC RID: 4268
	public Effect effect;
}
