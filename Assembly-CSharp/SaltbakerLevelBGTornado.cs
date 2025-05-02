using System;
using UnityEngine;

// Token: 0x0200036C RID: 876
public class SaltbakerLevelBGTornado : AbstractMonoBehaviour
{
	// Token: 0x060026B9 RID: 9913 RVA: 0x000C9A1C File Offset: 0x000C7C1C
	public void Start()
	{
		this.t = Random.Range(0f, 3.14159274f);
		this.rend.material.SetFloat("_BlurAmount", this.blurAmount * 5f);
		this.rend.material.SetFloat("_BlurLerp", this.blurAmount * 5f);
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x000C9A80 File Offset: 0x000C7C80
	public void Update()
	{
		this.t += CupheadTime.Delta;
		base.transform.GetChild(0).localPosition = new Vector3(Mathf.Sin(this.t * this.moveSpeed) * this.moveRange, 0f);
	}

	// Token: 0x04001FEF RID: 8175
	[SerializeField]
	public float moveRange;

	// Token: 0x04001FF0 RID: 8176
	[SerializeField]
	public float moveSpeed;

	// Token: 0x04001FF1 RID: 8177
	public float t;

	// Token: 0x04001FF2 RID: 8178
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001FF3 RID: 8179
	[SerializeField]
	public float blurAmount;
}
