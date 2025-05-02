using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
public class EffectSpawner : AbstractMonoBehaviour
{
	// Token: 0x06003CFC RID: 15612 RVA: 0x0003138E File Offset: 0x0002F58E
	public void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x001177B4 File Offset: 0x001159B4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(base.baseTransform.position, 5f);
		Gizmos.color = Color.red;
		Vector3 vector = base.baseTransform.position + this.offset;
		Gizmos.DrawLine(base.transform.position, vector);
		Gizmos.DrawWireSphere(vector, 5f);
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x00117828 File Offset: 0x00115A28
	public IEnumerator loop_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			Transform t = this.effectPrefab.Create(base.transform.position).transform;
			t.SetParent(base.transform);
			t.ResetLocalTransforms();
			t.localPosition = this.offset;
			t.SetParent(null);
		}
		yield break;
	}

	// Token: 0x04003079 RID: 12409
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x0400307A RID: 12410
	public Vector2 offset;

	// Token: 0x0400307B RID: 12411
	public float delay = 1f;
}
