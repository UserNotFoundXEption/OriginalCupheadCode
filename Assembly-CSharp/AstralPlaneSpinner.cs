using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class AstralPlaneSpinner : MonoBehaviour
{
	// Token: 0x06001EAE RID: 7854 RVA: 0x00019F02 File Offset: 0x00018102
	public void Start()
	{
		base.StartCoroutine(this.rotate_space_bg_cr());
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000B344C File Offset: 0x000B164C
	public IEnumerator rotate_space_bg_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
			this.spaceLayers[0].Rotate(new Vector3(0f, 0f, 0.3f));
			this.spaceLayers[1].Rotate(new Vector3(0f, 0f, 0.5f));
			this.spaceLayers[2].Rotate(new Vector3(0f, 0f, 1f));
		}
		yield break;
	}

	// Token: 0x040018FC RID: 6396
	[SerializeField]
	public Transform[] spaceLayers;
}
