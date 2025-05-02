using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class FlowerLevelPollenPetal : Effect
{
	// Token: 0x0600193A RID: 6458 RVA: 0x000A5E04 File Offset: 0x000A4004
	public void Start()
	{
		string text = (!Rand.Bool()) ? "Petal_B" : "Petal_A";
		base.animator.Play(text);
		base.StartCoroutine(this.fall_cr());
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000A5E54 File Offset: 0x000A4054
	public IEnumerator fall_cr()
	{
		float fallSpeed = 100f;
		for (;;)
		{
			base.transform.position -= Vector3.up * fallSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000A5E70 File Offset: 0x000A4070
	public IEnumerator fade_cr()
	{
		float t = 0f;
		float time = 2f;
		Color currentColor = base.GetComponent<SpriteRenderer>().color;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		while (t < time)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		this.OnEffectComplete();
		yield return null;
		yield break;
	}
}
