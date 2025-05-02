using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class PlanePlayerColliderManager : AbstractPlanePlayerComponent
{
	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06003996 RID: 14742 RVA: 0x0002EDE1 File Offset: 0x0002CFE1
	// (set) Token: 0x06003997 RID: 14743 RVA: 0x0002EDE9 File Offset: 0x0002CFE9
	public PlanePlayerColliderManager.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this.pairs[value].SetCollider(this.boxCollider);
			}
			this._state = value;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06003998 RID: 14744 RVA: 0x0002EE15 File Offset: 0x0002D015
	public PlanePlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x0010CC98 File Offset: 0x0010AE98
	public override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties>();
		this.pairs[PlanePlayerColliderManager.State.Default] = this.colliders.@default;
		this.pairs[PlanePlayerColliderManager.State.Shrunk] = this.colliders.shrunk;
		this.state = PlanePlayerColliderManager.State.Default;
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x0002EE22 File Offset: 0x0002D022
	public void Update()
	{
		this.UpdateColliders();
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x0010CCF8 File Offset: 0x0010AEF8
	public void UpdateColliders()
	{
		this.boxCollider.enabled = base.player.CanTakeDamage;
		if (base.player.Shrunk)
		{
			if (this.state != PlanePlayerColliderManager.State.Shrunk)
			{
				this.state = PlanePlayerColliderManager.State.Shrunk;
			}
			return;
		}
		if (this.state != PlanePlayerColliderManager.State.Default)
		{
			this.state = PlanePlayerColliderManager.State.Default;
		}
	}

	// Token: 0x04002E42 RID: 11842
	[SerializeField]
	public PlanePlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04002E43 RID: 11843
	public Dictionary<PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04002E44 RID: 11844
	public BoxCollider2D boxCollider;

	// Token: 0x04002E45 RID: 11845
	public PlanePlayerColliderManager.State _state;

	// Token: 0x020011D9 RID: 4569
	public enum State
	{
		// Token: 0x04007CA0 RID: 31904
		Default,
		// Token: 0x04007CA1 RID: 31905
		Shrunk
	}

	// Token: 0x020011DA RID: 4570
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x06007F60 RID: 32608 RVA: 0x00055597 File Offset: 0x00053797
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x00292CB0 File Offset: 0x00290EB0
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x06007F62 RID: 32610 RVA: 0x000555AD File Offset: 0x000537AD
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x04007CA2 RID: 31906
		public Vector2 center;

		// Token: 0x04007CA3 RID: 31907
		public Vector2 size;
	}

	// Token: 0x020011DB RID: 4571
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x04007CA4 RID: 31908
		public PlanePlayerColliderManager.ColliderProperties @default = new PlanePlayerColliderManager.ColliderProperties(new Vector2(-10f, 20f), new Vector2(75f, 75f));

		// Token: 0x04007CA5 RID: 31909
		public PlanePlayerColliderManager.ColliderProperties shrunk = new PlanePlayerColliderManager.ColliderProperties(new Vector2(-10f, 20f), new Vector2(45f, 45f));
	}
}
