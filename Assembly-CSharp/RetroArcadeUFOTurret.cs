using System;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class RetroArcadeUFOTurret : AbstractCollidableObject
{
	// Token: 0x0600231B RID: 8987 RVA: 0x000BF91C File Offset: 0x000BDB1C
	public RetroArcadeUFOTurret Create(RetroArcadeUFO parent, LevelProperties.RetroArcade.UFO properties, float t)
	{
		RetroArcadeUFOTurret retroArcadeUFOTurret = this.InstantiatePrefab<RetroArcadeUFOTurret>();
		retroArcadeUFOTurret.properties = properties;
		retroArcadeUFOTurret.parent = parent;
		retroArcadeUFOTurret.t = t;
		retroArcadeUFOTurret.transform.parent = parent.transform;
		retroArcadeUFOTurret.transform.position = parent.transform.position;
		return retroArcadeUFOTurret;
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000BF970 File Offset: 0x000BDB70
	public void FixedUpdate()
	{
		this.t += CupheadTime.FixedDelta * (this.properties.projectileSpeed / 600f);
		float num = this.t % 1f * 3.14159274f;
		Vector2 vector;
		vector..ctor(Mathf.Cos(num) * 600f / 2f, -Mathf.Sin(num) * 300f / 2f);
		base.transform.SetPosition(new float?(this.parent.transform.position.x + vector.x), new float?(this.parent.transform.position.y + vector.y), null);
		float value = MathUtils.DirectionToAngle(new Vector2(Mathf.Cos(num) * 300f / 2f, -Mathf.Sin(num) * 600f / 2f)) + -90f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000BFA9C File Offset: 0x000BDC9C
	public void Shoot()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, base.transform.eulerAngles.z - -90f, this.properties.projectileSpeed);
	}

	// Token: 0x04001D26 RID: 7462
	public const float ANGLE_OFFSET = -90f;

	// Token: 0x04001D27 RID: 7463
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x04001D28 RID: 7464
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001D29 RID: 7465
	public LevelProperties.RetroArcade.UFO properties;

	// Token: 0x04001D2A RID: 7466
	public RetroArcadeUFO parent;

	// Token: 0x04001D2B RID: 7467
	public float t;
}
