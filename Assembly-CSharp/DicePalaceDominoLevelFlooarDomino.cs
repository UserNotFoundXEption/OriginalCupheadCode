using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class DicePalaceDominoLevelFlooarDomino : MonoBehaviour
{
	// Token: 0x0600166B RID: 5739 RVA: 0x0009F044 File Offset: 0x0009D244
	public void Update()
	{
		Vector3 position = base.transform.position;
		position.x -= this.speed * CupheadTime.Delta;
		base.transform.position = position;
		if (base.transform.position.x <= 0f)
		{
			position.x += this.resetPositionX;
			base.transform.position = position;
		}
	}

	// Token: 0x04001239 RID: 4665
	[SerializeField]
	public float speed = 300f;

	// Token: 0x0400123A RID: 4666
	public float resetPositionX = 2808f;
}
