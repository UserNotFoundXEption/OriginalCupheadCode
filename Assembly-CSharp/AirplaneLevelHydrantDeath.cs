using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class AirplaneLevelHydrantDeath : MonoBehaviour
{
	// Token: 0x06000E14 RID: 3604 RVA: 0x0000C020 File Offset: 0x0000A220
	public void Start()
	{
		if (this.pieces)
		{
			base.StartCoroutine(this.recede_cr());
		}
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x00089424 File Offset: 0x00087624
	public IEnumerator recede_cr()
	{
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100f, base.transform.position.z);
		endPos = Vector3.Lerp(startPos, endPos, 0.35f);
		for (;;)
		{
			float t = this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
			this.pieces.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000B2B RID: 2859
	[SerializeField]
	public Animator anim;

	// Token: 0x04000B2C RID: 2860
	[SerializeField]
	public GameObject pieces;
}
