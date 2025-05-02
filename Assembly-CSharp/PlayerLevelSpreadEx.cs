using System;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class PlayerLevelSpreadEx : AbstractProjectile
{
	// Token: 0x0600390F RID: 14607 RVA: 0x0010A8A0 File Offset: 0x00108AA0
	public void Init(float speed, float damage, int childCount, float radius)
	{
		float num = (float)(360 / childCount);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		for (int i = 0; i < childCount; i++)
		{
			BasicProjectile projectile = this.childPrefab.Create(base.transform.position, num * (float)i, Vector2.one, speed);
			this.childPrefab.Damage = damage;
			this.childPrefab.Speed = speed;
			this.childPrefab.PlayerId = this.PlayerId;
			this.childPrefab.transform.AddPositionForward2D(radius);
			meterScoreTracker.Add(projectile);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x0002E7D1 File Offset: 0x0002C9D1
	public override int GetVariants()
	{
		return 1;
	}

	// Token: 0x06003911 RID: 14609 RVA: 0x0002E7D4 File Offset: 0x0002C9D4
	public override void SetBool(string boolean, bool b)
	{
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x0002E7D6 File Offset: 0x0002C9D6
	public override void SetInt(string integer, int i)
	{
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x0002E7D8 File Offset: 0x0002C9D8
	public override void SetTrigger(string trigger)
	{
	}

	// Token: 0x04002DEB RID: 11755
	[SerializeField]
	public PlayerLevelSpreadExChild childPrefab;
}
