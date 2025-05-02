using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class MapPlayerDust : Effect
{
	// Token: 0x06003211 RID: 12817 RVA: 0x000EBF7C File Offset: 0x000EA17C
	public Effect Create(Vector3 position, float offsetRotation, bool isLeft, int sortingOrder)
	{
		Vector3 vector = Vector3.right * this.offset.x;
		if (isLeft)
		{
			vector *= -1f;
		}
		Vector3 vector2 = Quaternion.Euler(offsetRotation, 0f, 0f) * vector;
		vector2.y += this.offset.y;
		this.spriteRenderer.sortingOrder = sortingOrder;
		position.z = position.y - 0.01f;
		return this.Create(position + vector2);
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x000EC010 File Offset: 0x000EA210
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale * Random.Range(this.scaleRange.min, this.scaleRange.max), randomR);
		Color color = this.spriteRenderer.color;
		color.a *= Random.Range(this.opacityRange.min, this.opacityRange.max);
		this.spriteRenderer.color = color;
		base.animator.SetTrigger("startAnim");
		base.StartCoroutine(this.dust_cr());
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x000EC0A4 File Offset: 0x000EA2A4
	public IEnumerator dust_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04002917 RID: 10519
	public const string StartAnimTrigger = "startAnim";

	// Token: 0x04002918 RID: 10520
	[SerializeField]
	public MinMax scaleRange;

	// Token: 0x04002919 RID: 10521
	[SerializeField]
	public MinMax opacityRange;

	// Token: 0x0400291A RID: 10522
	[SerializeField]
	public Vector3 offset;

	// Token: 0x0400291B RID: 10523
	[SerializeField]
	public SpriteRenderer spriteRenderer;
}
