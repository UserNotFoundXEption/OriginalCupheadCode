using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class FlyingCowboyLevelRicochetDebrisExplode : Effect
{
	// Token: 0x06001BCE RID: 7118 RVA: 0x000ACD54 File Offset: 0x000AAF54
	public void Start()
	{
		Vector3 position = base.transform.position;
		position.z = Random.Range(0f, 1f);
		base.transform.position = position;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000ACD90 File Offset: 0x000AAF90
	public IEnumerator movement_cr()
	{
		SpriteRenderer renderer = base.GetComponent<SpriteRenderer>();
		float elapsedTime = 0f;
		while (elapsedTime < 0.5f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 position = base.transform.position;
			position.x -= 900f * CupheadTime.Delta;
			base.transform.position = position;
			Color color = renderer.color;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.5f);
			renderer.color = color;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000178AA File Offset: 0x00015AAA
	public void animationEvent_StartMovement()
	{
		base.StartCoroutine(this.movement_cr());
	}
}
