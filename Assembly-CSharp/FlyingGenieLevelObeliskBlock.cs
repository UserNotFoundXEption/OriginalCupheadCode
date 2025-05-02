using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class FlyingGenieLevelObeliskBlock : AbstractProjectile
{
	// Token: 0x06001CB3 RID: 7347 RVA: 0x000AEFBC File Offset: 0x000AD1BC
	public void Init(Vector3 pos, LevelProperties.FlyingGenie.Obelisk properties)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.rootPos = new Vector3(base.GetComponent<Renderer>().bounds.size.x / 2f + 10f, 0f, 0f);
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x00018514 File Offset: 0x00016714
	public override void Start()
	{
		base.Start();
		this.darkSprite.sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder + 1;
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x00018534 File Offset: 0x00016734
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x00018552 File Offset: 0x00016752
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x00018570 File Offset: 0x00016770
	public void ShootRegular(float angle)
	{
		this.projectile.Create(base.transform.position + this.rootPos, angle, this.properties.obeliskShootSpeed);
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x000185A5 File Offset: 0x000167A5
	public void ShootPink(float angle)
	{
		this.pinkProjectile.Create(base.transform.position + this.rootPos, angle, this.properties.obeliskShootSpeed);
	}

	// Token: 0x04001753 RID: 5971
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001754 RID: 5972
	[SerializeField]
	public BasicProjectile pinkProjectile;

	// Token: 0x04001755 RID: 5973
	[SerializeField]
	public SpriteRenderer darkSprite;

	// Token: 0x04001756 RID: 5974
	public LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x04001757 RID: 5975
	public Vector3 rootPos;
}
