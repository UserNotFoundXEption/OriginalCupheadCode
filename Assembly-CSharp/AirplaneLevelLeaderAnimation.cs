using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class AirplaneLevelLeaderAnimation : MonoBehaviour
{
	// Token: 0x06000E39 RID: 3641 RVA: 0x0000C1FE File Offset: 0x0000A3FE
	public void Start()
	{
		this.rootPosition = base.transform.position;
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0000C211 File Offset: 0x0000A411
	public void AniEvent_StartBulldog()
	{
		this.bulldogAnimation.SetTrigger("Continue");
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0000C223 File Offset: 0x0000A423
	public void AniEvent_SFX_LeaderBark()
	{
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_introbark");
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00089814 File Offset: 0x00087A14
	public void Update()
	{
		base.transform.position = this.rootPosition + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX * Vector3.right + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY * Vector3.up;
		this.wobbleTimer += CupheadTime.Delta * this.wobbleSpeed;
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0000C22F File Offset: 0x0000A42F
	public void AnimationEvent_SFX_DOGFIGHT_Intro_LeaderCopterFlyby()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterflybyexit");
	}

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	public Animator bulldogAnimation;

	// Token: 0x04000B4F RID: 2895
	public Vector3 rootPosition;

	// Token: 0x04000B50 RID: 2896
	public float wobbleTimer;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	public float wobbleSpeed = 1f;
}
