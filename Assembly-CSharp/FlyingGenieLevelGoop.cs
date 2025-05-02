using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class FlyingGenieLevelGoop : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x06001C76 RID: 7286 RVA: 0x000181C0 File Offset: 0x000163C0
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000AE458 File Offset: 0x000AC658
	public void ActivateGoop()
	{
		base.animator.SetTrigger("OnStartGoop");
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000181EC File Offset: 0x000163EC
	public void DeactivateGoop()
	{
		this.moving = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.StopAllCoroutines();
		base.animator.Play("Off");
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x00018223 File Offset: 0x00016423
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000AE4A8 File Offset: 0x000AC6A8
	public IEnumerator move_cr()
	{
		LevelProperties.FlyingGenie.Coffin p = base.properties.CurrentState.coffin;
		bool goingUp = false;
		this.moving = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			if (this.moving)
			{
				if (goingUp)
				{
					while (base.transform.position.y < this.yMax)
					{
						base.transform.AddPosition(0f, p.heartMovement * CupheadTime.Delta, 0f);
						yield return null;
					}
					goingUp = !goingUp;
				}
				else
				{
					while (base.transform.position.y > this.yMin)
					{
						base.transform.AddPosition(0f, -p.heartMovement * CupheadTime.Delta, 0f);
						yield return null;
					}
					goingUp = !goingUp;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000AE4C4 File Offset: 0x000AC6C4
	public IEnumerator shoot_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			if (this.moving)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coffin.heartShotDelayRange.RandomFloat());
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000AE4E0 File Offset: 0x000AC6E0
	public void ShootProjectiles()
	{
		AudioManager.Play("genie_sarcophagus_eye_plop");
		this.emitAudioFromObject.Add("genie_sarcophagus_eye_plop");
		this.projectile.Create(this.topRoot.position, base.properties.CurrentState.coffin, true);
		this.projectile.Create(this.bottomRoot.position, base.properties.CurrentState.coffin, false);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00018236 File Offset: 0x00016436
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.death_cr());
		AudioManager.Play("genie_goop_voice_exit");
		this.emitAudioFromObject.Add("genie_goop_voice_exit");
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000AE558 File Offset: 0x000AC758
	public IEnumerator death_cr()
	{
		float moveSpeed = 50f;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (base.transform.localPosition.x < this.endRoot.localPosition.x)
		{
			base.transform.localPosition += base.transform.right * moveSpeed;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.DeactivateGoop();
		yield return null;
		yield break;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00018275 File Offset: 0x00016475
	public void SoundGenieGoopIntro()
	{
		AudioManager.Play("genie_goop_voice_enter");
		this.emitAudioFromObject.Add("genie_goop_voice_enter");
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00018291 File Offset: 0x00016491
	public void SoundGenieGoopAttackPre()
	{
		AudioManager.Play("genie_goop_attack_pre");
		this.emitAudioFromObject.Add("genie_goop_attack_pre");
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000182AD File Offset: 0x000164AD
	public void SoundGenieGoopAttack()
	{
		AudioManager.Play("gene_goop_voice_attack");
		this.emitAudioFromObject.Add("gene_goop_voice_attack");
	}

	// Token: 0x04001728 RID: 5928
	[SerializeField]
	public Transform topRoot;

	// Token: 0x04001729 RID: 5929
	[SerializeField]
	public Transform bottomRoot;

	// Token: 0x0400172A RID: 5930
	[SerializeField]
	public FlyingGenieLevelHelixProjectile projectile;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	public Transform endRoot;

	// Token: 0x0400172C RID: 5932
	public bool moving;

	// Token: 0x0400172D RID: 5933
	public float yMax = 60f;

	// Token: 0x0400172E RID: 5934
	public float yMin = -260f;

	// Token: 0x0400172F RID: 5935
	public DamageReceiver damageReceiver;
}
