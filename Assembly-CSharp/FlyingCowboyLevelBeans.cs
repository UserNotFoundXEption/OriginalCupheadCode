using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class FlyingCowboyLevelBeans : AbstractProjectile
{
	// Token: 0x06001AFE RID: 6910 RVA: 0x000AA2D0 File Offset: 0x000A84D0
	public virtual void Init(Vector3 position, bool pointingUp, float speed, float extendTimer)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		GameObject[] array = (!Rand.Bool()) ? this.versionB : this.versionA;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		if (!pointingUp)
		{
			base.animator.Play("BottomIdle");
			base.animator.Update(0f);
		}
		base.animator.Play(0, 0, Random.Range(0f, 1f));
		base.StartCoroutine(this.move_cr(speed));
		base.StartCoroutine(this.extend_cr(extendTimer));
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x00016E7A File Offset: 0x0001507A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000AA394 File Offset: 0x000A8594
	public IEnumerator move_cr(float speed)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += new Vector3(-speed * CupheadTime.FixedDelta, 0f);
			if (base.transform.position.x < -745f)
			{
				Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000AA3B8 File Offset: 0x000A85B8
	public IEnumerator extend_cr(float extendTimer)
	{
		yield return CupheadTime.WaitForSeconds(this, extendTimer);
		base.animator.SetTrigger("Extend");
		yield break;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x00016E98 File Offset: 0x00015098
	public void SFX_COWGIRL_P3_CanPropellerLoop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p3_canpropeller_loop", 0.4f, 0.5f);
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_loop");
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x00016EBE File Offset: 0x000150BE
	public void AnimationEvent_SFX_COWGIRL_P3_CanUnfurl()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_canpropeller_unfurl");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_unfurl");
	}

	// Token: 0x040015CE RID: 5582
	[SerializeField]
	public GameObject[] versionA;

	// Token: 0x040015CF RID: 5583
	[SerializeField]
	public GameObject[] versionB;
}
