using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class AirplaneLevelBulldogPlaneLookLoopCounter : MonoBehaviour
{
	// Token: 0x06000DE8 RID: 3560 RVA: 0x0000BDC9 File Offset: 0x00009FC9
	public void OnDestroy()
	{
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0000BDD1 File Offset: 0x00009FD1
	public void aniEvent_IncreaseIdleLookLoopCount()
	{
		this.bullDogPlane.SetInteger("IdleLoopCount", this.bullDogPlane.GetInteger("IdleLoopCount") + 1);
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0000BDF5 File Offset: 0x00009FF5
	public void AniEvent_RecedeIntoDistance()
	{
		base.StartCoroutine(this.recede_cr());
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x000887B4 File Offset: 0x000869B4
	public IEnumerator recede_cr()
	{
		float startTime = this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100f, base.transform.position.z);
		endPos = Vector3.Lerp(startPos, endPos, 0.8f);
		while (this.bullDogPlane.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			float t = Mathf.InverseLerp(startTime, 1f, this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0000BE04 File Offset: 0x0000A004
	public void AnimationEvent_SFX_DOGFIGHT_Intro_BulldogPlaneFlyby()
	{
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introflyby");
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0000BE10 File Offset: 0x0000A010
	public void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectDown()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectdown");
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0000BE1C File Offset: 0x0000A01C
	public void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectUp()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectUp");
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0000BE28 File Offset: 0x0000A028
	public void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectLeverPull()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectleverpull");
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0000BE34 File Offset: 0x0000A034
	public void AnimationEvent_SFX_DOGFIGHT_Bulldog_LandsCockpit()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_landscockpit");
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0000BE40 File Offset: 0x0000A040
	public void SFX_DOGFIGHT_Bulldog_WingExtend_WhimperOut()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whimperout");
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0000BE4C File Offset: 0x0000A04C
	public void SFX_DOGFIGHT_Bulldog_WingExtend_WhistleOut()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_Whistle_Out");
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x0000BE58 File Offset: 0x0000A058
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchOut()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_Out");
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0000BE64 File Offset: 0x0000A064
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchIn()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_In");
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0000BE70 File Offset: 0x0000A070
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_planeexplodes");
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x0000BE7C File Offset: 0x0000A07C
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes_VO()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_PlaneExplodes_VO");
		CupheadLevelCamera.Current.Shake(30f, 0.291666657f, false);
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x0000BE9D File Offset: 0x0000A09D
	public void WORKAROUND_NullifyFields()
	{
		this.bullDogPlane = null;
	}

	// Token: 0x04000B02 RID: 2818
	[SerializeField]
	public Animator bullDogPlane;
}
