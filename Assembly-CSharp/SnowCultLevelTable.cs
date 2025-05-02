using System;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class SnowCultLevelTable : AbstractPausableComponent
{
	// Token: 0x060028C2 RID: 10434 RVA: 0x000CFB50 File Offset: 0x000CDD50
	public void Intro(Vector3 startVel)
	{
		base.transform.parent = null;
		base.transform.position = this.wiz.transform.position;
		this.vel = startVel / CupheadTime.FixedDelta;
		this.chase = true;
		base.animator.Play("Intro");
		this.SFX_SNOWCULT_WizardTableCrystalballLoop();
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000223D6 File Offset: 0x000205D6
	public void Outro()
	{
		this.chase = false;
		this.vel = (base.transform.position - this.lastPos) / CupheadTime.FixedDelta;
		this.outroTimer = 0.333333343f;
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000CFBB4 File Offset: 0x000CDDB4
	public void FixedUpdate()
	{
		if (!this.rend.enabled)
		{
			return;
		}
		this.lastPos = base.transform.position;
		base.transform.position += this.vel * CupheadTime.FixedDelta;
		if (this.chase)
		{
			this.vel += (this.wiz.transform.position - base.transform.position).normalized * this.accel * CupheadTime.FixedDelta;
			if (this.vel.magnitude > this.maxVel)
			{
				this.vel = this.vel.normalized * this.maxVel;
			}
			if (Vector2.Distance(this.wiz.transform.position, base.transform.position) > this.maxDistance)
			{
				base.transform.position = this.wiz.transform.position + (base.transform.position - this.wiz.transform.position).normalized * this.maxDistance;
			}
		}
		else
		{
			this.vel -= this.vel * this.decelOnDeactivate * CupheadTime.FixedDelta;
		}
		if (this.outroTimer > 0f)
		{
			this.outroTimer -= CupheadTime.FixedDelta;
			if (this.outroTimer <= 0f)
			{
				base.animator.Play("Outro");
				this.SFX_SNOWCULT_WizardTableDisappear();
			}
		}
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x00022410 File Offset: 0x00020610
	public void AnimationEvent_SFX_SNOWCULT_WizardTableAppear()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_appear");
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x0002242C File Offset: 0x0002062C
	public void SFX_SNOWCULT_WizardTableDisappear()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_disappear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_disappear");
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x00022452 File Offset: 0x00020652
	public void SFX_SNOWCULT_WizardTableCrystalballLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_snowcult_p1_wizard_crystalball_loop", 0.15f, 1f);
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
	}

	// Token: 0x04002209 RID: 8713
	[SerializeField]
	public SnowCultLevelWizard wiz;

	// Token: 0x0400220A RID: 8714
	public Vector3 vel;

	// Token: 0x0400220B RID: 8715
	public bool chase;

	// Token: 0x0400220C RID: 8716
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x0400220D RID: 8717
	[SerializeField]
	public float accel = 1f;

	// Token: 0x0400220E RID: 8718
	[SerializeField]
	public float decelOnDeactivate = 1f;

	// Token: 0x0400220F RID: 8719
	[SerializeField]
	public float maxVel = 200f;

	// Token: 0x04002210 RID: 8720
	[SerializeField]
	public float maxDistance = 20f;

	// Token: 0x04002211 RID: 8721
	public float outroTimer;

	// Token: 0x04002212 RID: 8722
	public Vector3 lastPos;
}
