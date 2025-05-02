using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class MapCastleZoneCollider : AbstractCollidableObject
{
	// Token: 0x14000067 RID: 103
	// (add) Token: 0x0600307E RID: 12414 RVA: 0x000E6564 File Offset: 0x000E4764
	// (remove) Token: 0x0600307F RID: 12415 RVA: 0x000E659C File Offset: 0x000E479C
	public event MapCastleZoneCollider.MapCastleZoneCollision OnMapCastleZoneCollision;

	// Token: 0x06003080 RID: 12416 RVA: 0x000E65D4 File Offset: 0x000E47D4
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector2 vector = this.interactionPoint.position;
		Gizmos.DrawWireSphere(vector, 0.2f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(vector + this.returnPositions.singlePlayer, 0.2f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(vector + this.returnPositions.playerOne, Vector3.one * 0.2f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(vector + this.returnPositions.playerTwo, Vector3.one * 0.2f);
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x000E669C File Offset: 0x000E489C
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (!hit.CompareTag("Player_Map"))
		{
			return;
		}
		if ((phase == CollisionPhase.Enter || phase == CollisionPhase.Exit) && this.OnMapCastleZoneCollision != null)
		{
			this.OnMapCastleZoneCollision(this, hit, phase);
		}
	}

	// Token: 0x0400281E RID: 10270
	[SerializeField]
	public MapCastleZones.Zone zone;

	// Token: 0x0400281F RID: 10271
	[SerializeField]
	public Transform interactionPoint;

	// Token: 0x04002820 RID: 10272
	[SerializeField]
	public bool enableLadderShadow = true;

	// Token: 0x04002821 RID: 10273
	[SerializeField]
	public AbstractMapInteractiveEntity.PositionProperties returnPositions;

	// Token: 0x020010F4 RID: 4340
	// (Invoke) Token: 0x06007BD1 RID: 31697
	public delegate void MapCastleZoneCollision(MapCastleZoneCollider collider, GameObject other, CollisionPhase phase);
}
