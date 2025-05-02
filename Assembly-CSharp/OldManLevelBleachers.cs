using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class OldManLevelBleachers : AbstractPausableComponent
{
	// Token: 0x06002032 RID: 8242 RVA: 0x0001B538 File Offset: 0x00019738
	public void Start()
	{
		base.StartCoroutine(this.move_bleachers_cr());
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000B7534 File Offset: 0x000B5734
	public IEnumerator move_bleachers_cr()
	{
		Vector3 rightStartPos = this.gnomeBleacherRight.transform.localPosition;
		Vector3 leftStartPos = this.gnomeBleacherLeft.transform.localPosition;
		Vector3 rightStepStartPos = rightStartPos;
		Vector3 leftStepStartPos = leftStartPos;
		this.SFX_OMM_P2_PuppetBleachersRaiseUp();
		this.SFX_OMM_BleachersCrowdLoop();
		yield return null;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.15f, 0.5f);
		for (int i = 0; i < 3; i++)
		{
			float t = 0f;
			float time = this.enterStepTime;
			Vector3 rightEndPos = Vector3.Lerp(rightStartPos, this.gnomeBleacherRightEnd.position, 0.5f + (float)i * 0.25f);
			Vector3 leftEndPos = Vector3.Lerp(leftStartPos, this.gnomeBleacherLeftEnd.position, 0.5f + (float)i * 0.25f);
			while (t < time + this.offset)
			{
				t += CupheadTime.Delta;
				this.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos, EaseUtils.EaseOutBounce(0f, 1f, Mathf.Clamp((t - this.offset) / time, 0f, 1f)));
				this.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos, EaseUtils.EaseOutBounce(0f, 1f, Mathf.Clamp(t / time, 0f, 1f)));
				yield return null;
			}
			rightStepStartPos = this.gnomeBleacherRight.transform.localPosition;
			leftStepStartPos = this.gnomeBleacherLeft.transform.localPosition;
			yield return CupheadTime.WaitForSeconds(this, this.enterStepPause);
		}
		while (this.level.InPhase2())
		{
			yield return null;
		}
		this.SFX_OMM_P2_End_BleacherPuppetsLower();
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0f, 1.5f);
		for (int j = 0; j < 3; j++)
		{
			float t = 0f;
			float time = this.exitStepTime;
			Vector3 rightEndPos2 = Vector3.Lerp(this.gnomeBleacherRightEnd.position, rightStartPos, (float)(j + 1) * 0.333f);
			Vector3 leftEndPos2 = Vector3.Lerp(this.gnomeBleacherLeftEnd.position, leftStartPos, (float)(j + 1) * 0.333f);
			while (t < time + this.offset)
			{
				t += CupheadTime.Delta;
				this.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos2, EaseUtils.EaseOutElastic(0f, 1f, Mathf.Clamp((t - this.offset) / time, 0f, 1f)));
				this.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos2, EaseUtils.EaseOutElastic(0f, 1f, Mathf.Clamp(t / time, 0f, 1f)));
				yield return null;
			}
			rightStepStartPos = this.gnomeBleacherRight.transform.localPosition;
			leftStepStartPos = this.gnomeBleacherLeft.transform.localPosition;
			yield return CupheadTime.WaitForSeconds(this, this.exitStepPause);
		}
		base.gameObject.SetActive(false);
		yield return null;
		yield break;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x0001B547 File Offset: 0x00019747
	public void SFX_OMM_P2_PuppetBleachersRaiseUp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_bleachersraiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_bleachersraiseup");
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x0001B563 File Offset: 0x00019763
	public void SFX_OMM_BleachersCrowdLoop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.001f, 0.001f);
		AudioManager.PlayLoop("sfx_dlc_omm_p2_bleacherscrowd_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_bleacherscrowd_loop");
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x0001B593 File Offset: 0x00019793
	public void SFX_OMM_P2_End_BleacherPuppetsLower()
	{
		AudioManager.Stop("sfx_dlc_omm_p2_bleacherscrowd_loop");
		AudioManager.Play("sfx_dlc_omm_p2_end_bleacherpuppetslower");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_bleacherpuppetslower");
	}

	// Token: 0x04001A31 RID: 6705
	[SerializeField]
	public GameObject gnomeBleacherRight;

	// Token: 0x04001A32 RID: 6706
	[SerializeField]
	public Transform gnomeBleacherRightEnd;

	// Token: 0x04001A33 RID: 6707
	[SerializeField]
	public GameObject gnomeBleacherLeft;

	// Token: 0x04001A34 RID: 6708
	[SerializeField]
	public Transform gnomeBleacherLeftEnd;

	// Token: 0x04001A35 RID: 6709
	[SerializeField]
	public OldManLevel level;

	// Token: 0x04001A36 RID: 6710
	[SerializeField]
	public float enterStepTime = 0.6f;

	// Token: 0x04001A37 RID: 6711
	[SerializeField]
	public float enterStepPause = 0.1f;

	// Token: 0x04001A38 RID: 6712
	[SerializeField]
	public float exitStepTime = 0.3f;

	// Token: 0x04001A39 RID: 6713
	[SerializeField]
	public float exitStepPause = 0.05f;

	// Token: 0x04001A3A RID: 6714
	[SerializeField]
	public float offset = 0.1f;
}
