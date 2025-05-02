using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class OldManLevelSpitProjectile : AbstractProjectile
{
	// Token: 0x0600213A RID: 8506 RVA: 0x000B9F68 File Offset: 0x000B8168
	public void Move(Vector3 position, float speedX, float speedY, float stopPosX, float gravity, float apexTime, int count)
	{
		base.transform.position = position;
		this.speed = new Vector3(speedX, speedY);
		this.stopPosX = stopPosX;
		this.gravity = gravity;
		this.apexTime = apexTime;
		this.smokeTimer = this.firstSmokeDelay;
		this.count = count;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.changeAnimations_cr());
		this.SFX_OMM_MouthCauldron_ProjLoop();
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x0001C5AE File Offset: 0x0001A7AE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x0001C5CC File Offset: 0x0001A7CC
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.GetComponent<SpriteRenderer>().color = ((!parryable) ? Color.white : Color.magenta);
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000B9FDC File Offset: 0x000B81DC
	public IEnumerator changeAnimations_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.apexTime + 0.229166672f);
		base.animator.SetTrigger("OnApex");
		yield return null;
		yield break;
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000B9FF8 File Offset: 0x000B81F8
	public IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.229166672f);
		while (base.transform.position.x > this.stopPosX)
		{
			this.speed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		float time = 0.6f;
		float t = 0f;
		while (base.transform.position.x > (float)Level.Current.Left - 100f)
		{
			if (base.transform.position.x <= (float)Level.Current.Left)
			{
				this.SFX_OMM_MouthCauldron_ProjLoopEnd();
			}
			if (this.speed.y > 0f)
			{
				this.speed.y = Mathf.Lerp(this.speed.y, 0f, t / time);
				t += CupheadTime.FixedDelta;
			}
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		this.SFX_OMM_MouthCauldron_ProjLoopEnd();
		this.Recycle<OldManLevelSpitProjectile>();
		yield break;
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000BA014 File Offset: 0x000B8214
	public override void Update()
	{
		base.Update();
		this.smokeTimer -= CupheadTime.Delta;
		if (this.smokeTimer <= 0f)
		{
			this.smokeTimer += this.smokeDelay;
			((OldManLevel)Level.Current).CreateFX(base.transform.position, false, base.CanParry);
		}
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x0001C5F5 File Offset: 0x0001A7F5
	public void AnimationEvent_SFX_OMM_MouthCauldron_ProjStart()
	{
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_loop_start");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_start");
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000BA084 File Offset: 0x000B8284
	public void SFX_OMM_MouthCauldron_ProjLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x0001C611 File Offset: 0x0001A811
	public void AnimationEvent_SFX_OMM_MouthCauldron_ProjHitPlayer()
	{
		this.SFX_OMM_MouthCauldron_ProjLoopEnd();
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_damageplayer");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_damageplayer");
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x0001C633 File Offset: 0x0001A833
	public void SFX_OMM_MouthCauldron_ProjLoopEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
	}

	// Token: 0x04001B60 RID: 7008
	public const float OFFSCREEN_OFFSET = 100f;

	// Token: 0x04001B61 RID: 7009
	public Vector3 speed;

	// Token: 0x04001B62 RID: 7010
	public float gravity;

	// Token: 0x04001B63 RID: 7011
	public float stopPosX;

	// Token: 0x04001B64 RID: 7012
	public float apexTime;

	// Token: 0x04001B65 RID: 7013
	[SerializeField]
	public float firstSmokeDelay = 1f;

	// Token: 0x04001B66 RID: 7014
	[SerializeField]
	public float smokeDelay = 0.05f;

	// Token: 0x04001B67 RID: 7015
	public float smokeTimer;

	// Token: 0x04001B68 RID: 7016
	public int count;
}
