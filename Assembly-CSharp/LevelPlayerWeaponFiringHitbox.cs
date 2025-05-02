using System;
using UnityEngine;

// Token: 0x02000521 RID: 1313
public class LevelPlayerWeaponFiringHitbox : CollisionChild
{
	// Token: 0x0600377B RID: 14203 RVA: 0x00103774 File Offset: 0x00101974
	public LevelPlayerWeaponFiringHitbox Create(Vector2 pos, float rotation)
	{
		LevelPlayerWeaponFiringHitbox levelPlayerWeaponFiringHitbox = this.InstantiatePrefab<LevelPlayerWeaponFiringHitbox>();
		levelPlayerWeaponFiringHitbox.transform.position = pos;
		levelPlayerWeaponFiringHitbox.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		return levelPlayerWeaponFiringHitbox;
	}
}
