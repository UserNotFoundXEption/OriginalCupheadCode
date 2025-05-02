using System;
using UnityEngine;

// Token: 0x0200033A RID: 826
public class RumRunnersGroundTest : MonoBehaviour
{
	// Token: 0x06002415 RID: 9237 RVA: 0x000C2874 File Offset: 0x000C0A74
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(base.transform.position + Vector3.up * 50f, this.collider, this.yOffset, 200f)), 20f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, this.yOffset, 200f)), this.collider, this.yOffset, 200f)), 20f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 20f);
	}

	// Token: 0x04001DE6 RID: 7654
	[SerializeField]
	public Collider2D collider;

	// Token: 0x04001DE7 RID: 7655
	[SerializeField]
	public float yOffset;
}
