using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class RobotLevelRobotHead : RobotLevelRobotBodyPart
{
	// Token: 0x060023B6 RID: 9142 RVA: 0x000C1650 File Offset: 0x000BF850
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 1, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		base.GetComponent<BoxCollider2D>().enabled = true;
		this.currentPlayer = PlayerManager.GetNext();
		this.primaryAttackDelay = properties.CurrentState.hose.attackDelayRange.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.cannon.attackDelay;
		this.attackStringGroup = Random.Range(0, properties.CurrentState.cannon.shootString.Length);
		this.attackStringIndex = Random.Range(0, properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		}).Length);
		parent.OnDeathEvent += this.OnPrimaryDeath;
		primaryHP = properties.CurrentState.hose.health;
		attackDelayMinus = properties.CurrentState.hose.delayMinus;
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		this.StartPrimary();
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000C1744 File Offset: 0x000BF944
	public override void OnPrimaryAttack()
	{
		if (this.currentPlayer == null || this.currentPlayer.IsDead)
		{
			this.currentPlayer = PlayerManager.GetNext();
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			if (this.currentPlayer.id == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
				}
			}
			else
			{
				this.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			}
			base.StartCoroutine(this.warningLaser_cr());
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000C17DC File Offset: 0x000BF9DC
	public IEnumerator warningLaser_cr()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.hose.warningDuration);
			if (this.current == RobotLevelRobotBodyPart.state.primary)
			{
				if (this.currentPlayer == null || this.currentPlayer.IsDead)
				{
					this.currentPlayer = PlayerManager.GetNext();
				}
				Vector3 dir = (this.currentPlayer.center - base.transform.position).normalized;
				this.angle = Vector3.Angle(Vector3.up, dir);
				if (this.angle < 0f)
				{
					this.angle *= -1f;
				}
				this.angle = Mathf.Clamp(this.angle, this.properties.CurrentState.hose.aimAngleParameter.min, this.properties.CurrentState.hose.aimAngleParameter.max);
				yield return null;
				this.laser = this.primary.GetComponent<RobotLevelHoseLaser>().Create(base.transform.position, this.angle - 90f, this);
				this.laser.animator.SetTrigger("OnWarning");
				AudioManager.Play("robot_raygun_charge");
				this.emitAudioFromObject.Add("robot_raygun_charge");
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.hose.warningDuration);
			if (this.current == RobotLevelRobotBodyPart.state.primary)
			{
				this.laser.animator.SetTrigger("OnAttack");
				AudioManager.Play("robot_raygun_shoot");
				this.emitAudioFromObject.Add("robot_raygun_shoot");
				yield return null;
			}
			else
			{
				AudioManager.Stop("robot_raygun_charge");
			}
			yield return null;
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StartCoroutine(this.attackLaser_cr());
		}
		yield break;
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000C17F8 File Offset: 0x000BF9F8
	public IEnumerator attackLaser_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, (float)this.properties.CurrentState.hose.beamDuration);
		AudioManager.Stop("robot_raygun_charge");
		if (this.laser != null)
		{
			Object.Destroy(this.laser.gameObject);
			this.isAttacking = false;
		}
		if ((float)Random.Range(0, 100) <= 25f && !AudioManager.CheckIfPlaying("robot_vocals_laugh"))
		{
			AudioManager.Play("robot_vocals_laugh");
			this.emitAudioFromObject.Add("robot_vocals_laugh");
		}
		yield break;
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000C1814 File Offset: 0x000BFA14
	public override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			this.parent.animator.SetBool("HeadStageTwoTransition", true);
			base.StartCoroutine(this.endLasers_cr());
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000C1868 File Offset: 0x000BFA68
	public IEnumerator endLasers_cr()
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this.parent, "Idle", 2, true, true);
		AudioManager.Play("robot_head_antennae_destroyed");
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.StopCoroutine(this.warningLaser_cr());
		base.StopCoroutine(this.attackLaser_cr());
		this.ExitCurrentAttacks();
		this.StartSecondary();
		this.deathEffect.Create(base.transform.position);
		AudioManager.Play("robot_upper_chest_port_destroyed");
		this.emitAudioFromObject.Add("robot_upper_chest_port_destroyed");
		yield break;
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000C1884 File Offset: 0x000BFA84
	public IEnumerator startSecondary_cr()
	{
		this.StartSecondary();
		yield return null;
		yield break;
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000C18A0 File Offset: 0x000BFAA0
	public override void OnSecondaryAttack()
	{
		this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelay;
		string attackString = this.properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		})[this.attackStringIndex];
		this.attackStringIndex++;
		if (this.attackStringIndex >= this.properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		}).Length - 1)
		{
			this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelay;
			this.attackStringIndex = 0;
			this.attackStringGroup++;
			if (this.attackStringGroup >= this.properties.CurrentState.cannon.shootString.Length - 1)
			{
				this.attackStringGroup = 0;
				this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelayRange.RandomFloat();
			}
		}
		this.parent.animator.SetTrigger("HeadAttack");
		base.StartCoroutine(this.spreadShot_cr(attackString));
		base.OnSecondaryAttack();
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000C19EC File Offset: 0x000BFBEC
	public IEnumerator spreadShot_cr(string attackString)
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this, "Stage Two Idle", 2, true, true);
		this.cannonSpreadShot(attackString);
		yield break;
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000C1A10 File Offset: 0x000BFC10
	public void cannonSpreadShot(string attackString)
	{
		int num = 0;
		Parser.IntTryParse(attackString.Substring(1), out num);
		num--;
		string[] array = this.properties.CurrentState.cannon.spreadVariableGroups[num].Split(new char[]
		{
			','
		});
		float speed = 0f;
		int num2 = 0;
		MinMax minMax = new MinMax(0f, 0f);
		foreach (string text in array)
		{
			if (text[0] == 'S')
			{
				Parser.FloatTryParse(text.Substring(1), out speed);
			}
			else if (text[0] == 'N')
			{
				Parser.IntTryParse(text.Substring(1), out num2);
			}
			else
			{
				string[] array3 = text.Split(new char[]
				{
					'-'
				});
				Parser.FloatTryParse(array3[0], out minMax.min);
				Parser.FloatTryParse(array3[1], out minMax.max);
			}
		}
		AudioManager.Play("robot_head_shoot");
		this.emitAudioFromObject.Add("robot_head_shoot");
		for (int j = 0; j < num2; j++)
		{
			float floatAt = minMax.GetFloatAt((float)j / ((float)num2 - 1f));
			if (j % 2 == 0)
			{
				BasicProjectile component = this.secondary.GetComponent<BasicProjectile>();
				component.Create(base.transform.position, floatAt, speed);
			}
			else
			{
				this.nutProjectile.Create(base.transform.position, floatAt, speed);
			}
		}
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x0001E266 File Offset: 0x0001C466
	public override void ExitCurrentAttacks()
	{
		if (this.laser != null)
		{
			Object.Destroy(this.laser.gameObject);
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x04001D98 RID: 7576
	public RobotLevelHoseLaser laser;

	// Token: 0x04001D99 RID: 7577
	public AbstractPlayerController currentPlayer;

	// Token: 0x04001D9A RID: 7578
	public float angle;

	// Token: 0x04001D9B RID: 7579
	public int attackStringGroup;

	// Token: 0x04001D9C RID: 7580
	public int attackStringIndex;

	// Token: 0x04001D9D RID: 7581
	[SerializeField]
	public BasicProjectile nutProjectile;
}
