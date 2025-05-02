using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class AirplaneLevelHydrantAttack : MonoBehaviour
{
	// Token: 0x06000E10 RID: 3600 RVA: 0x0000BFC5 File Offset: 0x0000A1C5
	public void AniEvent_SpawnHydrant(AnimationEvent ev)
	{
		((GameObject)ev.objectReferenceParameter).GetComponent<BasicProjectile>().Create(this.spawnPos.position, ev.floatParameter, (float)ev.intParameter * 1.5f);
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0000C000 File Offset: 0x0000A200
	public void SFX_DOGFIGHT_Leader_CopterBG()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground");
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0000C00C File Offset: 0x0000A20C
	public void SFX_DOGFIGHT_Leader_CopterBGCannonFire()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground_canon");
	}

	// Token: 0x04000B28 RID: 2856
	public const float SPEED_MODIFIER = 1.5f;

	// Token: 0x04000B29 RID: 2857
	[SerializeField]
	public Transform spawnPos;

	// Token: 0x04000B2A RID: 2858
	public float speed = 800f;
}
