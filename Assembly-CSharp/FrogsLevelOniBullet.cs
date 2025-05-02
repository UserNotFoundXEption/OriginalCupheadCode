using System;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class FrogsLevelOniBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x06001E4F RID: 7759 RVA: 0x000B2A08 File Offset: 0x000B0C08
	public FrogsLevelOniBullet Create(Vector2 pos, float speed, LevelProperties.Frogs.Demon properties)
	{
		FrogsLevelOniBullet frogsLevelOniBullet = base.Create(pos, speed) as FrogsLevelOniBullet;
		frogsLevelOniBullet.properties = properties;
		return frogsLevelOniBullet;
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000198EE File Offset: 0x00017AEE
	public override void Start()
	{
		base.Start();
		this.SetSize();
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000B2A2C File Offset: 0x000B0C2C
	public void SetSize()
	{
		this.parryBox.SetScale(null, new float?(this.properties.demonParryHeight), null);
		this.hurtBox.SetScale(null, new float?(this.properties.demonFlameHeight), null);
	}

	// Token: 0x040018C3 RID: 6339
	[SerializeField]
	public Transform parryBox;

	// Token: 0x040018C4 RID: 6340
	[SerializeField]
	public Transform hurtBox;

	// Token: 0x040018C5 RID: 6341
	public LevelProperties.Frogs.Demon properties;
}
