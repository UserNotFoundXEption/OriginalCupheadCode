using System;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class MausoleumLevelBigGhost : MausoleumLevelGhostBase
{
	// Token: 0x06001F1A RID: 7962 RVA: 0x000B5548 File Offset: 0x000B3748
	public MausoleumLevelBigGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.BigGhost properties, GameObject urn)
	{
		MausoleumLevelBigGhost mausoleumLevelBigGhost = base.Create(position, rotation, speed) as MausoleumLevelBigGhost;
		mausoleumLevelBigGhost.properties = properties;
		mausoleumLevelBigGhost.urn = urn;
		return mausoleumLevelBigGhost;
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x000B5578 File Offset: 0x000B3778
	public override void OnParry(AbstractPlayerController player)
	{
		Vector2 vector = this.smallRoot1.transform.position;
		Vector2 vector2 = vector;
		Vector2 vector3;
		vector3..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector = vector2 + vector3.normalized * this.smallRoot1.radius * Random.value;
		Vector2 vector4 = this.smallRoot2.transform.position;
		Vector2 vector5 = vector4;
		Vector2 vector6;
		vector6..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector4 = vector5 + vector6.normalized * this.smallRoot2.radius * Random.value;
		Vector3 vector7 = this.urn.transform.position - vector;
		Vector3 vector8 = this.urn.transform.position - vector4;
		MausoleumLevelRegularGhost mausoleumLevelRegularGhost = this.regGhost.Create(vector, MathUtils.DirectionToAngle(vector7), this.properties.littleGhostSpeed) as MausoleumLevelRegularGhost;
		mausoleumLevelRegularGhost.GetParent(this.parent);
		MausoleumLevelRegularGhost mausoleumLevelRegularGhost2 = this.regGhost.Create(vector4, MathUtils.DirectionToAngle(vector8), this.properties.littleGhostSpeed) as MausoleumLevelRegularGhost;
		mausoleumLevelRegularGhost2.GetParent(this.parent);
		mausoleumLevelRegularGhost.transform.SetScale(new float?(0.7f), new float?(0.7f), new float?(0.7f));
		mausoleumLevelRegularGhost2.transform.SetScale(new float?(0.7f), new float?(0.7f), new float?(0.7f));
		base.OnParry(player);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001967 RID: 6503
	[SerializeField]
	public MausoleumLevelRegularGhost regGhost;

	// Token: 0x04001968 RID: 6504
	[SerializeField]
	public FlyingBlimpLevelSpawnRadius smallRoot1;

	// Token: 0x04001969 RID: 6505
	[SerializeField]
	public FlyingBlimpLevelSpawnRadius smallRoot2;

	// Token: 0x0400196A RID: 6506
	public LevelProperties.Mausoleum.BigGhost properties;

	// Token: 0x0400196B RID: 6507
	public GameObject urn;
}
