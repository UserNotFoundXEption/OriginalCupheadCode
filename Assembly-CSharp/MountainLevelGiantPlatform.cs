using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class MountainLevelGiantPlatform : LevelPlatform
{
	// Token: 0x06002E7D RID: 11901 RVA: 0x000DFB14 File Offset: 0x000DDD14
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MountainPlatformingLevelCyclops>())
		{
			this.explosion.Create(base.transform.position);
			this.SpawnParts();
			if (base.transform.childCount > 0 && base.GetComponentInChildren<LevelPlayerMotor>())
			{
				base.GetComponentInChildren<LevelPlayerMotor>().OnPitKnockUp(10f, 1f);
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x000DFBA0 File Offset: 0x000DDDA0
	public void SpawnParts()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.sprites)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x04002698 RID: 9880
	[SerializeField]
	public Effect explosion;

	// Token: 0x04002699 RID: 9881
	[SerializeField]
	public SpriteDeathParts[] sprites;
}
