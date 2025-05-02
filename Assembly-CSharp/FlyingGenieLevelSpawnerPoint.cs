using System;
using UnityEngine;

// Token: 0x02000277 RID: 631
public class FlyingGenieLevelSpawnerPoint : AbstractProjectile
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x000186D4 File Offset: 0x000168D4
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000AF590 File Offset: 0x000AD790
	public FlyingGenieLevelSpawnerPoint Create(Vector2 pos, float rotation, LevelProperties.FlyingGenie.Bullets properties)
	{
		FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint = base.Create(pos, rotation) as FlyingGenieLevelSpawnerPoint;
		flyingGenieLevelSpawnerPoint.properties = properties;
		return flyingGenieLevelSpawnerPoint;
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000AF5B4 File Offset: 0x000AD7B4
	public void Shoot()
	{
		this.effect.Create(this.root.transform.position);
		int num = Random.Range(1, 4);
		BasicProjectile basicProjectile = this.projectile.Create(this.root.transform.position, base.transform.eulerAngles.z - 90f, this.properties.childSpeed);
		basicProjectile.GetComponent<Animator>().Play("Bullet_" + num);
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x000186DB File Offset: 0x000168DB
	public void Dead()
	{
		this.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000186E9 File Offset: 0x000168E9
	public override void Die()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000186F1 File Offset: 0x000168F1
	public override void RandomizeVariant()
	{
	}

	// Token: 0x04001771 RID: 6001
	[SerializeField]
	public Effect effect;

	// Token: 0x04001772 RID: 6002
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001773 RID: 6003
	[SerializeField]
	public Transform root;

	// Token: 0x04001774 RID: 6004
	public LevelProperties.FlyingGenie.Bullets properties;
}
