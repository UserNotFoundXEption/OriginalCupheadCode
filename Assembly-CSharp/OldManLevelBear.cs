using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D5 RID: 725
public class OldManLevelBear : BasicDamageDealingObject
{
	// Token: 0x06002028 RID: 8232 RVA: 0x000B7434 File Offset: 0x000B5634
	public IEnumerator fall_cr()
	{
		this.thrown = true;
		Vector3 startPos = new Vector3(-520f, 525f);
		Vector3 endPos = new Vector3(-600f, -90f);
		Vector3 startScale = new Vector3(1f, 1f);
		Vector3 endScale = new Vector3(0.15f, 0.15f);
		base.transform.position = startPos;
		base.animator.Play("Falling");
		float t = 0f;
		this.rend.sortingLayerName = "Background";
		this.rend.sortingOrder = 1000;
		while (t < 0.7916667f)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.020833334f);
			t += 0.020833334f;
			float tn = t / 0.7916667f;
			base.transform.position = new Vector3(Mathf.Lerp(startPos.x, endPos.x, EaseUtils.EaseOutSine(0f, 1f, tn)), Mathf.Lerp(startPos.y, endPos.y, EaseUtils.EaseInSine(0f, 1f, tn)));
			base.transform.localScale = Vector3.Lerp(startScale, endScale, EaseUtils.EaseOutSine(0f, 1f, tn));
		}
		yield return null;
		base.transform.localScale = new Vector3(1f, 1f);
		base.animator.Play("FX");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "FX", false, true);
		this.rend.enabled = false;
		this.rend.sortingOrder = 0;
		this.rend.sortingLayerName = "Projectiles";
		yield break;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x0001B41E File Offset: 0x0001961E
	public void AnimationEvent_SFX_OMM_BearAttackClawing()
	{
		AudioManager.Play("sfx_dlc_omm_bearattack_clawing");
		this.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_clawing");
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x0001B43A File Offset: 0x0001963A
	public void AnimationEvent_SFX_OMM_BearAttackGrowling()
	{
		AudioManager.Play("sfx_dlc_omm_bearattack_growling");
		this.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_growling");
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x0001B456 File Offset: 0x00019656
	public void AnimationEvent_SFX_OMM_BearAttackEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_bearattack_growling");
		AudioManager.Play("sfx_dlc_omm_bearattack_end");
		this.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_end");
	}

	// Token: 0x04001A26 RID: 6694
	public const float FALL_TIME = 0.7916667f;

	// Token: 0x04001A27 RID: 6695
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001A28 RID: 6696
	public bool thrown;
}
