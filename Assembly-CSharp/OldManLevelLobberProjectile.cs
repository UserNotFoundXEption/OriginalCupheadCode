using System;
using UnityEngine;

// Token: 0x020002DE RID: 734
public class OldManLevelLobberProjectile : BasicProjectile
{
	// Token: 0x06002082 RID: 8322 RVA: 0x000B8690 File Offset: 0x000B6890
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<LevelPlatform>())
		{
			this.Die();
			foreach (AbstractPlayerController abstractPlayerController in hit.GetComponentsInChildren<AbstractPlayerController>())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.transform.parent = null;
				}
			}
			hit.SetActive(false);
		}
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x0001BA13 File Offset: 0x00019C13
	public override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}
}
