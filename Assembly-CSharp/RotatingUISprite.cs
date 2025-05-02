using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AA RID: 1450
public class RotatingUISprite : MonoBehaviour
{
	// Token: 0x06003D0A RID: 15626 RVA: 0x00031448 File Offset: 0x0002F648
	public void Start()
	{
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x001179F0 File Offset: 0x00115BF0
	public IEnumerator rotate_cr()
	{
		for (;;)
		{
			base.transform.Rotate(new Vector3(0f, 0f, this.speed / (float)this.frameRate));
			yield return CupheadTime.WaitForSeconds(this, 1f / (float)this.frameRate);
		}
		yield break;
	}

	// Token: 0x04003089 RID: 12425
	[SerializeField]
	public float speed = 1f;

	// Token: 0x0400308A RID: 12426
	[SerializeField]
	public int frameRate = 12;
}
