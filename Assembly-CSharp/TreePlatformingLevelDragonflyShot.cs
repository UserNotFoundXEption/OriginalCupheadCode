using System;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class TreePlatformingLevelDragonflyShot : PlatformingLevelPathMovementEnemy
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06002C7A RID: 11386 RVA: 0x00025323 File Offset: 0x00023523
	// (set) Token: 0x06002C7B RID: 11387 RVA: 0x0002532B File Offset: 0x0002352B
	public bool isActivated { get; set; }

	// Token: 0x06002C7C RID: 11388 RVA: 0x00025334 File Offset: 0x00023534
	public override void Awake()
	{
		base.Awake();
		this.isActivated = false;
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x00025343 File Offset: 0x00023543
	public override void Die()
	{
		this.Deactivate();
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x0002534B File Offset: 0x0002354B
	public void Activate()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.isActivated = true;
		this.PrepareShot();
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x00025372 File Offset: 0x00023572
	public void Deactivate()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.isActivated = false;
		base.ResetStartingCondition();
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x00025399 File Offset: 0x00023599
	public void PrepareShot()
	{
		if (Rand.Bool())
		{
			this.startPosition = 0f;
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Forward;
		}
		else
		{
			this.startPosition = 1f;
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Back;
		}
		base.StartFromCustom();
	}
}
