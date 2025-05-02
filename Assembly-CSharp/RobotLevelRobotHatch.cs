using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class RobotLevelRobotHatch : RobotLevelRobotBodyPart
{
	// Token: 0x060023A6 RID: 9126 RVA: 0x000C119C File Offset: 0x000BF39C
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.primaryAttackDelay = properties.CurrentState.shotBot.initialSpawnDelay.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.bombBot.bombDelay;
		primaryHP = properties.CurrentState.shotBot.hatchGateHealth;
		attackDelayMinus = properties.CurrentState.shotBot.shotbotSpawnDelayMinus;
		this.shotbotSpawnDelay = properties.CurrentState.shotBot.shotbotDelay;
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		base.animator.Play("Closed", 0, 0.75f);
		base.animator.Play("Loop", 1, 0.75f);
		base.animator.Play("Loop", 2, 0.75f);
		base.animator.Play("Loop", 3, 0.75f);
		this.StartPrimary();
		this.damageEffectRenderer = this.damageEffect.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
	public override void OnPrimaryAttack()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StartCoroutine(this.openHatch_cr());
			this.primaryAttackDelay = this.properties.CurrentState.shotBot.shotbotWaveDelay.RandomFloat();
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000C1290 File Offset: 0x000BF490
	public IEnumerator openHatch_cr()
	{
		float elapsedTime = base.animator.GetCurrentAnimatorStateInfo(2).length;
		float normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
		normalizedTime %= 1f;
		float delay = normalizedTime * 24f;
		int currentFrame = (int)(delay / 24f);
		if (currentFrame < 2)
		{
			delay = (float)((2 - currentFrame) / 24) * elapsedTime;
			this.nearestEventFrame = 2;
		}
		else if (currentFrame < 14)
		{
			delay = (float)((14 - currentFrame) / 24) * elapsedTime;
			this.nearestEventFrame = 14;
		}
		else
		{
			delay = (float)(24 - currentFrame) * elapsedTime;
			delay += (float)(2 - currentFrame) * elapsedTime;
			this.nearestEventFrame = 2;
		}
		yield return CupheadTime.WaitForSeconds(this, delay);
		yield return null;
		normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
		if (this.nearestEventFrame == 2)
		{
			base.animator.SetTrigger("IsOpenFrame2");
		}
		else if (this.nearestEventFrame == 14)
		{
			base.animator.SetTrigger("IsOpenFrame14");
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
		}
		yield break;
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000C12AC File Offset: 0x000BF4AC
	public void Open()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary)
		{
			base.GetComponent<SpriteRenderer>().enabled = true;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000C12FC File Offset: 0x000BF4FC
	public void Close()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary)
		{
			base.GetComponent<SpriteRenderer>().enabled = false;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000C134C File Offset: 0x000BF54C
	public IEnumerator closeHatch_cr()
	{
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.animator.SetTrigger("IsClosing");
		yield return base.animator.WaitForAnimationToEnd(this, true);
		yield return null;
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.isAttacking = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x0001E204 File Offset: 0x0001C404
	public void SpawnShotbotWave()
	{
		base.StartCoroutine(this.spawnShotbotWave_cr());
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000C1368 File Offset: 0x000BF568
	public IEnumerator spawnShotbotWave_cr()
	{
		for (int i = 0; i < this.properties.CurrentState.shotBot.shotbotCount; i++)
		{
			GameObject shotbot = Object.Instantiate<GameObject>(this.primary, base.transform.position + Vector3.right * 80f + Vector3.down * 20f, Quaternion.identity);
			shotbot.GetComponent<RobotLevelHatchShotbot>().InitShotbot(this.properties.CurrentState.shotBot.shotbotHealth, this.properties.CurrentState.shotBot.bulletSpeed, this.properties.CurrentState.shotBot.pinkBulletCount, this.properties.CurrentState.shotBot.shotbotShootDelay, this.properties.CurrentState.shotBot.shotbotFlightSpeed);
			yield return CupheadTime.WaitForSeconds(this, this.shotbotSpawnDelay);
			if (this.current != RobotLevelRobotBodyPart.state.primary)
			{
				break;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.StartCoroutine(this.closeHatch_cr());
		yield break;
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000C1384 File Offset: 0x000BF584
	public override void OnSecondaryAttack()
	{
		HomingProjectile homingProjectile = this.secondary.GetComponent<RobotLevelHatchBombBot>().Create(base.transform.position, 180f, (float)this.properties.CurrentState.bombBot.initialBombMovementSpeed, (float)this.properties.CurrentState.bombBot.bombHomingSpeed, this.properties.CurrentState.bombBot.bombRotationSpeed, (float)this.properties.CurrentState.bombBot.bombLifeTime, this.properties.CurrentState.bombBot.bombInitialMovementDuration.RandomFloat(), 4f, PlayerManager.GetNext());
		homingProjectile.GetComponent<RobotLevelHatchBombBot>().InitBombBot(this.properties.CurrentState.bombBot);
		homingProjectile.transform.right = Vector3.down;
		if (this.currentHealth[1] <= 0f)
		{
			base.gameObject.SetActive(false);
			this.StopAllCoroutines();
		}
		base.OnSecondaryAttack();
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000C1488 File Offset: 0x000BF688
	public override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			AudioManager.Play("robot_lower_chest_port_destroyed");
			this.emitAudioFromObject.Add("robot_lower_chest_port_destroyed");
			base.animator.Play("Off");
			base.GetComponent<BoxCollider2D>().enabled = false;
			this.StartSecondary();
			this.DeathEffect();
			base.StopCoroutine(this.openHatch_cr());
			base.StopCoroutine(this.closeHatch_cr());
			base.enabled = false;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
			foreach (GameObject gameObject in this.damagedHatches)
			{
				gameObject.SetActive(true);
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000C1580 File Offset: 0x000BF780
	public override void ExitCurrentAttacks()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StopCoroutine(this.openHatch_cr());
			base.StartCoroutine(this.closeHatch_cr());
		}
		if (this.current == RobotLevelRobotBodyPart.state.secondary)
		{
			base.StopCoroutine(this.secondaryAttack_cr());
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x0001E213 File Offset: 0x0001C413
	public void InitAnims()
	{
		base.animator.SetTrigger("OnRobotIntro");
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000C15D0 File Offset: 0x000BF7D0
	public override void Die()
	{
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffect.SetActive(false);
		foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.Die();
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x0001E225 File Offset: 0x0001C425
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.OnDamageTaken(info);
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffectRoutine = this.damageEffect_cr();
		base.StartCoroutine(this.damageEffectRoutine);
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000C1634 File Offset: 0x000BF834
	public IEnumerator damageEffect_cr()
	{
		for (int i = 0; i < 3; i++)
		{
			this.damageEffectRenderer.enabled = true;
			this.damageEffect.SetActive(true);
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
			this.damageEffect.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		}
		yield break;
	}

	// Token: 0x04001D92 RID: 7570
	public float shotbotSpawnDelay;

	// Token: 0x04001D93 RID: 7571
	public int nearestEventFrame;

	// Token: 0x04001D94 RID: 7572
	[SerializeField]
	public GameObject[] damagedHatches;

	// Token: 0x04001D95 RID: 7573
	[SerializeField]
	public GameObject damageEffect;

	// Token: 0x04001D96 RID: 7574
	public IEnumerator damageEffectRoutine;

	// Token: 0x04001D97 RID: 7575
	public SpriteRenderer damageEffectRenderer;
}
