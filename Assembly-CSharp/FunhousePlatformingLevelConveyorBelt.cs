using System;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class FunhousePlatformingLevelConveyorBelt : ScrollingSprite
{
	// Token: 0x06002D74 RID: 11636 RVA: 0x00025EEF File Offset: 0x000240EF
	public override void Start()
	{
		base.Start();
		this.point += base.transform.position;
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00025F13 File Offset: 0x00024113
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawSphere(this.point + base.transform.position, 10f);
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x000DCCF8 File Offset: 0x000DAEF8
	public override void Update()
	{
		this.wait -= CupheadTime.Delta;
		if (this.wait > 0f)
		{
			return;
		}
		base.Update();
		for (int i = 0; i < base.copyRenderers.Count; i++)
		{
			Vector3 position = base.copyRenderers[i].transform.position;
			position.z = position.x - this.point.x;
			if (this.rightToCenter)
			{
				position.z *= -1f;
			}
			base.copyRenderers[i].transform.position = position;
		}
	}

	// Token: 0x040025A6 RID: 9638
	public Vector3 point;

	// Token: 0x040025A7 RID: 9639
	public bool rightToCenter;

	// Token: 0x040025A8 RID: 9640
	public float wait;
}
