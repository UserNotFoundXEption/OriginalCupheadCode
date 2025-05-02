using System;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class SaltbakerLevelGlassChunk : AbstractMonoBehaviour
{
	// Token: 0x06002703 RID: 9987 RVA: 0x000CA60C File Offset: 0x000C880C
	public void Reset(Vector3 pos, float fallSpeed, bool isChunk, bool flip, bool reverse, bool inBack, int variant)
	{
		base.transform.position = pos;
		this.fallSpeed = fallSpeed;
		base.animator.SetFloat("Reverse", (float)((!reverse || isChunk) ? 1 : -1));
		base.animator.Play(((!isChunk) ? "Bit" : "Chunk") + variant.ToString(), 0, (float)Random.Range(0, 1));
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)((!isChunk) ? Random.Range(-30, 30) : 0));
		base.transform.localScale = new Vector3((float)((!flip) ? 1 : -1), 1f);
		foreach (SpriteRenderer spriteRenderer in this.rend)
		{
			spriteRenderer.sortingLayerName = ((!inBack) ? "Foreground" : "Background");
			spriteRenderer.color = ((!inBack) ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f));
		}
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x00020CE3 File Offset: 0x0001EEE3
	public void Update()
	{
		base.transform.position += Vector3.down * this.fallSpeed * CupheadTime.Delta;
	}

	// Token: 0x04002037 RID: 8247
	public float fallSpeed;

	// Token: 0x04002038 RID: 8248
	[SerializeField]
	public SpriteRenderer[] rend;
}
