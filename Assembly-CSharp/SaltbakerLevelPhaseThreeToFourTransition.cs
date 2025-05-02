using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200037C RID: 892
public class SaltbakerLevelPhaseThreeToFourTransition : MonoBehaviour
{
	// Token: 0x06002745 RID: 10053 RVA: 0x00021022 File Offset: 0x0001F222
	public void StartSaltman()
	{
		this.anim.Play("Start");
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x00021034 File Offset: 0x0001F234
	public void StartHeart()
	{
		base.StartCoroutine(this.move_heart_cr());
		this.anim.Play("Heart", 1, 0f);
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x000CB370 File Offset: 0x000C9570
	public IEnumerator move_heart_cr()
	{
		yield return this.anim.WaitForAnimationToStart(this, "HeartLoop", 1, false);
		Vector3 start = this.heart.transform.position;
		Vector3 end = start + Vector3.up * 300f;
		for (float t = 0f; t < 1f; t += 0.0833333358f)
		{
			this.heart.transform.position = Vector3.Lerp(start, end, EaseUtils.EaseInSine(0f, 1f, t));
			yield return CupheadTime.WaitForSeconds(this, 0.0833333358f);
		}
		base.enabled = false;
		yield break;
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x00021059 File Offset: 0x0001F259
	public void AnimationEvent_SFX_SALTB_Phase3to4_HeartRise()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_heartrise");
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x00021065 File Offset: 0x0001F265
	public void AnimationEvent_SFX_SALTB_Phase3to4_Transition()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition");
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x00021071 File Offset: 0x0001F271
	public void AnimationEvent_SFX_SALTB_Phase3to4_TransitionStart()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_start");
	}

	// Token: 0x04002081 RID: 8321
	[SerializeField]
	public Animator anim;

	// Token: 0x04002082 RID: 8322
	[SerializeField]
	public GameObject heart;
}
