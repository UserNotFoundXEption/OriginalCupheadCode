using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032E RID: 814
public class RobotLevelRobotBodyPart : AbstractCollidableObject
{
	// Token: 0x06002379 RID: 9081 RVA: 0x000C0778 File Offset: 0x000BE978
	public virtual void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
		this.parent.OnPrimaryDeathEvent += this.OnPrimaryDeath;
		this.parent.OnSecondaryDeathEvent += this.OnSecondaryDeath;
		this.properties = properties;
		this.current = RobotLevelRobotBodyPart.state.primary;
		this.currentHealth[0] = (float)primaryHP;
		this.currentHealth[1] = (float)secondaryHP;
		this.attackDelayMinus = attackDelayMinus;
		base.StartCoroutine(this.checkCurrentState_cr());
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x0001E05C File Offset: 0x0001C25C
	public virtual void StartPrimary()
	{
		base.StartCoroutine(this.primaryAttack_cr());
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000C080C File Offset: 0x000BEA0C
	public virtual IEnumerator primaryAttack_cr()
	{
		while (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			yield return CupheadTime.WaitForSeconds(this, this.primaryAttackDelay);
			this.isAttacking = true;
			this.OnPrimaryAttack();
			while (this.isAttacking)
			{
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x0001E06B File Offset: 0x0001C26B
	public virtual void StartSecondary()
	{
		base.StartCoroutine(this.secondaryAttack_cr());
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000C0828 File Offset: 0x000BEA28
	public virtual IEnumerator secondaryAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		while (this.current == RobotLevelRobotBodyPart.state.secondary)
		{
			this.OnSecondaryAttack();
			yield return null;
			if (this.secondaryAttackDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, this.secondaryAttackDelay);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x000C0844 File Offset: 0x000BEA44
	public virtual void AttackDestroyed(bool isPrimary)
	{
		if (isPrimary)
		{
			AudioManager.Play("robot_vocals_angry");
			this.emitAudioFromObject.Add("robot_vocals_angry");
			this.parent.PrimaryDied();
			this.current = RobotLevelRobotBodyPart.state.secondary;
		}
		else
		{
			this.current = RobotLevelRobotBodyPart.state.none;
			base.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x0001E07A File Offset: 0x0001C27A
	public override void Awake()
	{
		this.currentHealth = new float[2];
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000C089C File Offset: 0x000BEA9C
	public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float num = this.currentHealth[(int)this.current];
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.currentHealth[(int)this.current] -= info.damage;
		}
		if (num > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.currentHealth[(int)this.current], 0f, num));
		}
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000C0914 File Offset: 0x000BEB14
	public IEnumerator checkCurrentState_cr()
	{
		while (this.current != RobotLevelRobotBodyPart.state.none)
		{
			RobotLevelRobotBodyPart.state state = this.current;
			if (state != RobotLevelRobotBodyPart.state.primary)
			{
				if (state == RobotLevelRobotBodyPart.state.secondary)
				{
					if (this.currentHealth[1] <= 0f)
					{
						this.AttackDestroyed(false);
					}
				}
			}
			else if (this.currentHealth[0] <= 0f)
			{
				this.AttackDestroyed(true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x0001E0B2 File Offset: 0x0001C2B2
	public virtual void OnPrimaryAttack()
	{
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x0001E0B4 File Offset: 0x0001C2B4
	public virtual void OnPrimaryDeath()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.primaryAttackDelay -= this.attackDelayMinus;
		}
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
	public virtual void OnSecondaryAttack()
	{
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x0001E0D6 File Offset: 0x0001C2D6
	public virtual void OnSecondaryDeath()
	{
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x0001E0D8 File Offset: 0x0001C2D8
	public virtual void ExitCurrentAttacks()
	{
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x0001E0DA File Offset: 0x0001C2DA
	public virtual void DeathEffect()
	{
		if (this.deathEffect != null)
		{
			base.StartCoroutine(this.death_effects_cr());
		}
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000C0930 File Offset: 0x000BEB30
	public IEnumerator death_effects_cr()
	{
		for (;;)
		{
			yield return null;
			this.deathEffect.Create(base.transform.position).GetComponent<Animator>().SetBool("IsA", Rand.Bool());
			yield return CupheadTime.WaitForSeconds(this, Random.Range(2f, 5f));
		}
		yield break;
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000C094C File Offset: 0x000BEB4C
	public virtual void Die()
	{
		this.StopAllCoroutines();
		this.ExitCurrentAttacks();
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
		Object.Destroy(base.gameObject, 15f);
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000C0990 File Offset: 0x000BEB90
	public override void OnDestroy()
	{
		if (this.parent != null)
		{
			this.parent.OnDeathEvent -= this.Die;
			this.parent.OnPrimaryDeathEvent -= this.OnPrimaryDeath;
			this.parent.OnSecondaryDeathEvent -= this.OnSecondaryDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x04001D6E RID: 7534
	public RobotLevelRobotBodyPart.state current;

	// Token: 0x04001D6F RID: 7535
	public LevelProperties.Robot properties;

	// Token: 0x04001D70 RID: 7536
	public RobotLevelRobot parent;

	// Token: 0x04001D71 RID: 7537
	public float decreaseAttackDelayAmount;

	// Token: 0x04001D72 RID: 7538
	public float[] currentHealth;

	// Token: 0x04001D73 RID: 7539
	public float primaryAttackDelay;

	// Token: 0x04001D74 RID: 7540
	public float secondaryAttackDelay;

	// Token: 0x04001D75 RID: 7541
	public float attackDelayMinus;

	// Token: 0x04001D76 RID: 7542
	public bool isAttacking;

	// Token: 0x04001D77 RID: 7543
	public DamageReceiver damageReceiver;

	// Token: 0x04001D78 RID: 7544
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x04001D79 RID: 7545
	[SerializeField]
	public GameObject primary;

	// Token: 0x04001D7A RID: 7546
	[SerializeField]
	public GameObject secondary;

	// Token: 0x02000E70 RID: 3696
	public enum state
	{
		// Token: 0x04006826 RID: 26662
		primary,
		// Token: 0x04006827 RID: 26663
		secondary,
		// Token: 0x04006828 RID: 26664
		none
	}
}
