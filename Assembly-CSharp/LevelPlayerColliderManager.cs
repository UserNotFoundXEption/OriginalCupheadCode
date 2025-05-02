using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class LevelPlayerColliderManager : AbstractLevelPlayerComponent
{
	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x0600364A RID: 13898 RVA: 0x0002C7B9 File Offset: 0x0002A9B9
	public LevelPlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x0600364B RID: 13899 RVA: 0x0002C7C6 File Offset: 0x0002A9C6
	public float DefaultWidth
	{
		get
		{
			return this.colliders.@default.size.x;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x0600364C RID: 13900 RVA: 0x0002C7DD File Offset: 0x0002A9DD
	public float DefaultHeight
	{
		get
		{
			return this.colliders.@default.size.y;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x0600364D RID: 13901 RVA: 0x0002C7F4 File Offset: 0x0002A9F4
	public float Width
	{
		get
		{
			return this.pairs[(int)this._state].size.x;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x0600364E RID: 13902 RVA: 0x0002C811 File Offset: 0x0002AA11
	public float Height
	{
		get
		{
			return this.pairs[(int)this._state].size.y;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x0600364F RID: 13903 RVA: 0x000FE124 File Offset: 0x000FC324
	public Vector2 Center
	{
		get
		{
			return new Vector2(this.boxCollider.offset.x, this.boxCollider.offset.y * base.player.motor.GravityReversalMultiplier) + base.transform.position;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06003650 RID: 13904 RVA: 0x000FE184 File Offset: 0x000FC384
	public Vector2 DefaultCenter
	{
		get
		{
			return new Vector2(this.colliders.@default.center.x, this.colliders.@default.center.y * base.player.motor.GravityReversalMultiplier) + base.transform.position;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06003651 RID: 13905 RVA: 0x0002C82E File Offset: 0x0002AA2E
	// (set) Token: 0x06003652 RID: 13906 RVA: 0x0002C836 File Offset: 0x0002AA36
	public LevelPlayerColliderManager.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this.pairs[(int)value].SetCollider(this.boxCollider);
			}
			this._state = value;
		}
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x000FE1E8 File Offset: 0x000FC3E8
	public override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<int, LevelPlayerColliderManager.ColliderProperties>();
		this.pairs[0] = this.colliders.@default;
		this.pairs[1] = this.colliders.air;
		this.pairs[2] = this.colliders.ducking;
		this.pairs[3] = this.colliders.ducking;
		this.pairs[4] = this.colliders.chaliceFirstJump;
		this.state = LevelPlayerColliderManager.State.Default;
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x0002C862 File Offset: 0x0002AA62
	public void FixedUpdate()
	{
		this.UpdateColliders();
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x000FE28C File Offset: 0x000FC48C
	public void UpdateColliders()
	{
		base.gameObject.layer = ((!base.player.CanTakeDamage) ? 9 : 8);
		if (base.player.motor.Dashing)
		{
			if (this.state != LevelPlayerColliderManager.State.Dashing)
			{
				this.state = LevelPlayerColliderManager.State.Dashing;
			}
			return;
		}
		if (!base.player.motor.Grounded)
		{
			if (!base.player.stats.isChalice)
			{
				if (this.state != LevelPlayerColliderManager.State.Air)
				{
					this.state = LevelPlayerColliderManager.State.Air;
				}
				return;
			}
			if (!base.player.motor.ChaliceDoubleJumped)
			{
				if (this.state != LevelPlayerColliderManager.State.ChaliceFirstJump)
				{
					this.state = LevelPlayerColliderManager.State.ChaliceFirstJump;
				}
				return;
			}
			if (this.state != LevelPlayerColliderManager.State.Air)
			{
				this.state = LevelPlayerColliderManager.State.Air;
			}
			return;
		}
		else
		{
			if (base.player.motor.Ducking)
			{
				if (this.state != LevelPlayerColliderManager.State.Ducking)
				{
					this.state = LevelPlayerColliderManager.State.Ducking;
				}
				return;
			}
			if (this.state != LevelPlayerColliderManager.State.Default)
			{
				this.state = LevelPlayerColliderManager.State.Default;
			}
			return;
		}
	}

	// Token: 0x04002C11 RID: 11281
	[SerializeField]
	public LevelPlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04002C12 RID: 11282
	public Dictionary<int, LevelPlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04002C13 RID: 11283
	public BoxCollider2D boxCollider;

	// Token: 0x04002C14 RID: 11284
	public LevelPlayerColliderManager.State _state;

	// Token: 0x02001187 RID: 4487
	public enum State
	{
		// Token: 0x04007AE7 RID: 31463
		Default,
		// Token: 0x04007AE8 RID: 31464
		Air,
		// Token: 0x04007AE9 RID: 31465
		Ducking,
		// Token: 0x04007AEA RID: 31466
		Dashing,
		// Token: 0x04007AEB RID: 31467
		ChaliceFirstJump
	}

	// Token: 0x02001188 RID: 4488
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x06007DF9 RID: 32249 RVA: 0x0005491D File Offset: 0x00052B1D
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x06007DFA RID: 32250 RVA: 0x0028DEF8 File Offset: 0x0028C0F8
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x00054933 File Offset: 0x00052B33
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x04007AEC RID: 31468
		public Vector2 center;

		// Token: 0x04007AED RID: 31469
		public Vector2 size;
	}

	// Token: 0x02001189 RID: 4489
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x04007AEE RID: 31470
		public LevelPlayerColliderManager.ColliderProperties @default = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 62f), new Vector2(50f, 105f));

		// Token: 0x04007AEF RID: 31471
		public LevelPlayerColliderManager.ColliderProperties air = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 50f), new Vector2(50f, 50f));

		// Token: 0x04007AF0 RID: 31472
		public LevelPlayerColliderManager.ColliderProperties ducking = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 27f), new Vector2(50f, 35f));

		// Token: 0x04007AF1 RID: 31473
		public LevelPlayerColliderManager.ColliderProperties dashing;

		// Token: 0x04007AF2 RID: 31474
		public LevelPlayerColliderManager.ColliderProperties chaliceFirstJump = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 78f), new Vector2(50f, 75f));
	}
}
