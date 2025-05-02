using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class LevelPlatform : AbstractCollidableObject
{
	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0000952D File Offset: 0x0000772D
	// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00009535 File Offset: 0x00007735
	public List<Transform> players { get; set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0000953E File Offset: 0x0000773E
	public bool AllowShadows
	{
		get
		{
			return this.allowShadows;
		}
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0007B3A4 File Offset: 0x000795A4
	public override void Awake()
	{
		base.Awake();
		this.players = new List<Transform>();
		base.gameObject.layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0007B3E4 File Offset: 0x000795E4
	public virtual void AddChild(Transform player)
	{
		if (!this.players.Contains(player))
		{
			this.players.Add(player);
		}
		player.parent = base.transform;
		Vector3 localScale = player.localScale;
		localScale.y = 1f;
		LevelPlayerMotor component = player.GetComponent<LevelPlayerMotor>();
		if (component != null)
		{
			localScale.y *= component.GravityReversalMultiplier;
		}
		player.localScale = localScale;
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00009546 File Offset: 0x00007746
	public virtual void OnPlayerExit(Transform player)
	{
		if (this.players.Contains(player))
		{
			this.players.Remove(player);
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0007B45C File Offset: 0x0007965C
	public override void OnDestroy()
	{
		foreach (Transform transform in this.players)
		{
			if (!(transform == null))
			{
				transform.parent = null;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04000835 RID: 2101
	public bool canFallThrough = true;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	public bool allowShadows = true;
}
