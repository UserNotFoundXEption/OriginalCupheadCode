using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class MouseLevelFlame : AbstractCollidableObject
{
	// Token: 0x06001FDE RID: 8158 RVA: 0x0001AF05 File Offset: 0x00019105
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.transform.parent = null;
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x0001AF24 File Offset: 0x00019124
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x0001AF3C File Offset: 0x0001913C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000B6C8C File Offset: 0x000B4E8C
	public void SetColliderEnabled(bool enabled)
	{
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = enabled;
		}
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000B6CC0 File Offset: 0x000B4EC0
	public void UpdateParentTransform(Transform mouseTransform)
	{
		base.transform.position = ((mouseTransform.localScale.x != 1f) ? this.flippedRoot.position : this.root.position);
	}

	// Token: 0x040019F3 RID: 6643
	[SerializeField]
	public Transform root;

	// Token: 0x040019F4 RID: 6644
	[SerializeField]
	public Transform flippedRoot;

	// Token: 0x040019F5 RID: 6645
	public DamageDealer damageDealer;
}
