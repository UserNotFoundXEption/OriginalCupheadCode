using System;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class CollisionChild : AbstractCollidableObject
{
	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x06003D39 RID: 15673 RVA: 0x001187F8 File Offset: 0x001169F8
	// (remove) Token: 0x06003D3A RID: 15674 RVA: 0x00118830 File Offset: 0x00116A30
	public event CollisionChild.OnCollisionHandler OnAnyCollision;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x06003D3B RID: 15675 RVA: 0x00118868 File Offset: 0x00116A68
	// (remove) Token: 0x06003D3C RID: 15676 RVA: 0x001188A0 File Offset: 0x00116AA0
	public event CollisionChild.OnCollisionHandler OnWallCollision;

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x06003D3D RID: 15677 RVA: 0x001188D8 File Offset: 0x00116AD8
	// (remove) Token: 0x06003D3E RID: 15678 RVA: 0x00118910 File Offset: 0x00116B10
	public event CollisionChild.OnCollisionHandler OnGroundCollision;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06003D3F RID: 15679 RVA: 0x00118948 File Offset: 0x00116B48
	// (remove) Token: 0x06003D40 RID: 15680 RVA: 0x00118980 File Offset: 0x00116B80
	public event CollisionChild.OnCollisionHandler OnCeilingCollision;

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06003D41 RID: 15681 RVA: 0x001189B8 File Offset: 0x00116BB8
	// (remove) Token: 0x06003D42 RID: 15682 RVA: 0x001189F0 File Offset: 0x00116BF0
	public event CollisionChild.OnCollisionHandler OnPlayerCollision;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x06003D43 RID: 15683 RVA: 0x00118A28 File Offset: 0x00116C28
	// (remove) Token: 0x06003D44 RID: 15684 RVA: 0x00118A60 File Offset: 0x00116C60
	public event CollisionChild.OnCollisionHandler OnPlayerProjectileCollision;

	// Token: 0x140000CF RID: 207
	// (add) Token: 0x06003D45 RID: 15685 RVA: 0x00118A98 File Offset: 0x00116C98
	// (remove) Token: 0x06003D46 RID: 15686 RVA: 0x00118AD0 File Offset: 0x00116CD0
	public event CollisionChild.OnCollisionHandler OnEnemyCollision;

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06003D47 RID: 15687 RVA: 0x00118B08 File Offset: 0x00116D08
	// (remove) Token: 0x06003D48 RID: 15688 RVA: 0x00118B40 File Offset: 0x00116D40
	public event CollisionChild.OnCollisionHandler OnEnemyProjectileCollision;

	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x06003D49 RID: 15689 RVA: 0x00118B78 File Offset: 0x00116D78
	// (remove) Token: 0x06003D4A RID: 15690 RVA: 0x00118BB0 File Offset: 0x00116DB0
	public event CollisionChild.OnCollisionHandler OnOtherCollision;

	// Token: 0x06003D4B RID: 15691 RVA: 0x00031660 File Offset: 0x0002F860
	public bool ForwardParry(out AbstractCollidableObject collisionParent)
	{
		collisionParent = this.collisionParent;
		return this.forwardParry;
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00031670 File Offset: 0x0002F870
	public void Start()
	{
		if (this.collisionParent != null)
		{
			this.collisionParent.RegisterCollisionChild(this);
		}
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x0003168F File Offset: 0x0002F88F
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.OnAnyCollision != null)
		{
			this.OnAnyCollision(hit, phase);
		}
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x000316A9 File Offset: 0x0002F8A9
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (this.OnWallCollision != null)
		{
			this.OnWallCollision(hit, phase);
		}
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x000316C3 File Offset: 0x0002F8C3
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.OnGroundCollision != null)
		{
			this.OnGroundCollision(hit, phase);
		}
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x000316DD File Offset: 0x0002F8DD
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		if (this.OnCeilingCollision != null)
		{
			this.OnCeilingCollision(hit, phase);
		}
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x000316F7 File Offset: 0x0002F8F7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.OnPlayerCollision != null)
		{
			this.OnPlayerCollision(hit, phase);
		}
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x00031711 File Offset: 0x0002F911
	public override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.OnPlayerProjectileCollision != null)
		{
			this.OnPlayerProjectileCollision(hit, phase);
		}
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x0003172B File Offset: 0x0002F92B
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.OnEnemyCollision != null)
		{
			this.OnEnemyCollision(hit, phase);
		}
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00031745 File Offset: 0x0002F945
	public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.OnEnemyProjectileCollision != null)
		{
			this.OnEnemyProjectileCollision(hit, phase);
		}
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x0003175F File Offset: 0x0002F95F
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (this.OnOtherCollision != null)
		{
			this.OnOtherCollision(hit, phase);
		}
	}

	// Token: 0x040030CC RID: 12492
	[SerializeField]
	[Tooltip("OPTIONAL: Drag collision parent to this slot to register all collision events to this child. If null, no collisions are registered.")]
	public AbstractCollidableObject collisionParent;

	// Token: 0x040030CD RID: 12493
	[SerializeField]
	public bool forwardParry;

	// Token: 0x0200123F RID: 4671
	// (Invoke) Token: 0x060080F9 RID: 33017
	public delegate void OnCollisionHandler(GameObject hit, CollisionPhase phase);
}
