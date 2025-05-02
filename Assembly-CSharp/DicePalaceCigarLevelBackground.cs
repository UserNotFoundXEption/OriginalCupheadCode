using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class DicePalaceCigarLevelBackground : AbstractPausableComponent
{
	// Token: 0x060015F9 RID: 5625 RVA: 0x0009E4C4 File Offset: 0x0009C6C4
	public IEnumerator circulate_fire_cr()
	{
		float loopSize = 6f;
		float angle = 0f;
		for (;;)
		{
			angle += 0.5f * CupheadTime.Delta;
			Vector3 handleRotationX = new Vector3(-Mathf.Sin(angle) * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(angle) * loopSize, 0f);
			this.foregroundFireSprite.position = this.firePivot.position;
			this.foregroundFireSprite.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040011E6 RID: 4582
	[SerializeField]
	public Transform foregroundFireSprite;

	// Token: 0x040011E7 RID: 4583
	[SerializeField]
	public Transform firePivot;
}
