using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class ArcadePlayerColliderManager : AbstractArcadePlayerComponent
{
	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060034C8 RID: 13512 RVA: 0x0002B4CF File Offset: 0x000296CF
	public ArcadePlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060034C9 RID: 13513 RVA: 0x0002B4DC File Offset: 0x000296DC
	public float DefaultWidth
	{
		get
		{
			return this.colliders.@default.size.x;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060034CA RID: 13514 RVA: 0x0002B4F3 File Offset: 0x000296F3
	public float DefaultHeight
	{
		get
		{
			return this.colliders.@default.size.y;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x060034CB RID: 13515 RVA: 0x0002B50A File Offset: 0x0002970A
	public float Width
	{
		get
		{
			return this.pairs[this._state].size.x;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x060034CC RID: 13516 RVA: 0x0002B527 File Offset: 0x00029727
	public float Height
	{
		get
		{
			return this.pairs[this._state].size.y;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x060034CD RID: 13517 RVA: 0x0002B544 File Offset: 0x00029744
	public Vector2 Center
	{
		get
		{
			return this.boxCollider.offset + base.transform.position;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x060034CE RID: 13518 RVA: 0x0002B566 File Offset: 0x00029766
	// (set) Token: 0x060034CF RID: 13519 RVA: 0x0002B56E File Offset: 0x0002976E
	public ArcadePlayerColliderManager.State state
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

	// Token: 0x060034D0 RID: 13520 RVA: 0x000F77A8 File Offset: 0x000F59A8
	public override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties>();
		this.pairs[ArcadePlayerColliderManager.State.Default] = this.colliders.@default;
		this.pairs[ArcadePlayerColliderManager.State.Air] = this.colliders.air;
		this.pairs[ArcadePlayerColliderManager.State.Dashing] = this.colliders.dashing;
		this.pairs[ArcadePlayerColliderManager.State.Rocket] = this.colliders.rocket;
		this.state = ArcadePlayerColliderManager.State.Default;
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x0002B59A File Offset: 0x0002979A
	public void Update()
	{
		this.UpdateColliders();
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x000F7838 File Offset: 0x000F5A38
	public void UpdateColliders()
	{
		this.boxCollider.enabled = base.player.CanTakeDamage;
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			if (this.state != ArcadePlayerColliderManager.State.Rocket)
			{
				this.state = ArcadePlayerColliderManager.State.Rocket;
			}
			return;
		}
		if (base.player.motor.Dashing)
		{
			if (this.state != ArcadePlayerColliderManager.State.Dashing)
			{
				this.state = ArcadePlayerColliderManager.State.Dashing;
			}
			return;
		}
		if (!base.player.motor.Grounded)
		{
			if (this.state != ArcadePlayerColliderManager.State.Air)
			{
				this.state = ArcadePlayerColliderManager.State.Air;
			}
			return;
		}
		if (this.state != ArcadePlayerColliderManager.State.Default)
		{
			this.state = ArcadePlayerColliderManager.State.Default;
		}
	}

	// Token: 0x04002B41 RID: 11073
	[SerializeField]
	public ArcadePlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04002B42 RID: 11074
	public Dictionary<ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04002B43 RID: 11075
	public BoxCollider2D boxCollider;

	// Token: 0x04002B44 RID: 11076
	public ArcadePlayerColliderManager.State _state;

	// Token: 0x02001158 RID: 4440
	public enum State
	{
		// Token: 0x04007A00 RID: 31232
		Default,
		// Token: 0x04007A01 RID: 31233
		Air,
		// Token: 0x04007A02 RID: 31234
		Dashing,
		// Token: 0x04007A03 RID: 31235
		Rocket
	}

	// Token: 0x02001159 RID: 4441
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x06007D5F RID: 32095 RVA: 0x00054280 File Offset: 0x00052480
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x0028C55C File Offset: 0x0028A75C
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x00054296 File Offset: 0x00052496
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x04007A04 RID: 31236
		public Vector2 center;

		// Token: 0x04007A05 RID: 31237
		public Vector2 size;
	}

	// Token: 0x0200115A RID: 4442
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x04007A06 RID: 31238
		public ArcadePlayerColliderManager.ColliderProperties @default = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 40f), new Vector2(33f, 70f));

		// Token: 0x04007A07 RID: 31239
		public ArcadePlayerColliderManager.ColliderProperties air = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 33f), new Vector2(33f, 33f));

		// Token: 0x04007A08 RID: 31240
		public ArcadePlayerColliderManager.ColliderProperties dashing = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 18f), new Vector2(33f, 23f));

		// Token: 0x04007A09 RID: 31241
		public ArcadePlayerColliderManager.ColliderProperties rocket = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(3.2f, 4f), new Vector2(4f, 66f));
	}
}
