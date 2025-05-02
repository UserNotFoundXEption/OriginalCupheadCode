using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000130 RID: 304
public class AirplaneLevelSecretLeader : LevelProperties.Airplane.Entity
{
	// Token: 0x06000E61 RID: 3681 RVA: 0x0000C330 File Offset: 0x0000A530
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.effectSide = Rand.Bool();
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0000C36B File Offset: 0x0000A56B
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x0008AA08 File Offset: 0x00088C08
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.rocketPositionString = new PatternString(properties.CurrentState.secretLeader.rocketHomingSpawnLocation, true, true);
		this.terrierProjectileParryableString = new PatternString(properties.CurrentState.secretTerriers.dogBulletParryString, true);
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0000C390 File Offset: 0x0000A590
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && !this.isDead)
		{
			this.Die();
		}
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0000C3C9 File Offset: 0x0000A5C9
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0000C3DF File Offset: 0x0000A5DF
	public bool TerrierProjectileParryable()
	{
		return this.terrierProjectileParryableString.PopLetter() == 'P';
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
	public void DieMain()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_main_cr());
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0008AA64 File Offset: 0x00088C64
	public IEnumerator die_main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.hiding = true;
		this.currentHole = 3;
		base.animator.Play("Death");
		this.isDead = true;
		base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetLeaderDeathPosition(this.currentHole);
		yield return base.animator.WaitForAnimationToStart(this, "DeathLoop", false);
		base.animator.Play("Tears", 1);
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death");
		yield break;
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0008AA80 File Offset: 0x00088C80
	public void Die()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		for (int i = 0; i < this.terriers.Length; i++)
		{
			this.terriers[i].Die(i);
		}
		this.level.leader.animator.Play("Off");
		this.level.leader.animator.Play("Copter_Death", this.level.leader.animator.GetLayerIndex("Death"));
		this.level.leader.animator.Play("Blades", base.animator.GetLayerIndex("DeathBlades"));
		base.animator.Play("DeathLoop");
		base.animator.Play("Tears", 1);
		base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetLeaderDeathPosition(this.currentHole);
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x0000C405 File Offset: 0x0000A605
	public void HideAnimationComplete()
	{
		this.moved = true;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0008ABC4 File Offset: 0x00088DC4
	public void AttackAnimationStart()
	{
		LevelProperties.Airplane.SecretLeader secretLeader = base.properties.CurrentState.secretLeader;
		Vector3 vector;
		vector..ctor((float)((!this.effectSide) ? 120 : -120), 120f);
		this.rocketBGPrefab.Create(Camera.main.transform.position + vector, MathUtils.DirectionToAngle(Vector3.up) + Random.Range(5f, 12f) * (float)((!this.effectSide) ? -1 : 1), new Vector3(2f, 2f), 600f);
		this.rocketBGEffect.Create(Camera.main.transform.position + vector);
		this.effectSide = !this.effectSide;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0008ACA8 File Offset: 0x00088EA8
	public void AttackAnimationComplete()
	{
		LevelProperties.Airplane.SecretLeader secretLeader = base.properties.CurrentState.secretLeader;
		this.rocketPrefab.Create(PlayerManager.GetNext(), Camera.main.transform.position + Vector3.up * 800f + this.rocketPositionString.PopFloat() * Vector3.right, secretLeader.rocketHomingSpeed, secretLeader.rocketHomingRotation, secretLeader.rocketHomingHP, secretLeader.rocketHomingTime);
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x0008AD34 File Offset: 0x00088F34
	public IEnumerator attack_cr()
	{
		this.level.OccupyHole(this.currentHole);
		for (;;)
		{
			base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
			base.transform.position = this.level.GetHolePosition(this.currentHole, true);
			this.rend.sortingOrder = this.currentHole % 3 + 50;
			this.backerRend.sortingOrder = this.currentHole % 3 + 13;
			bool lookingStraight = this.currentHole == 2 || this.currentHole == 5;
			base.animator.SetBool("EyesDown", !lookingStraight);
			this.hiding = false;
			if (!this.first)
			{
				base.animator.Play("Emerge");
			}
			this.first = false;
			this.boxCollider.enabled = true;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.leaderPreAttackDelay);
			base.animator.Play("AttackStart");
			yield return base.animator.WaitForAnimationToStart(this, "AttackPreHold", false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.attackAnticipationHold);
			base.animator.SetTrigger("ContinueAttack");
			yield return base.animator.WaitForAnimationToStart(this, "AttackPostHold", false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.attackRecoveryHold);
			base.animator.SetTrigger("ContinueAttack");
			yield return base.animator.WaitForAnimationToEnd(this, (!base.animator.GetBool("EyesDown")) ? "AttackEnd" : "AttackEndEyesDown", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.leaderPostAttackDelay);
			base.animator.Play((this.currentHole % 3 != 3) ? "Exit" : "ExitLow");
			while (!this.moved)
			{
				yield return null;
			}
			this.boxCollider.enabled = false;
			this.hiding = true;
			this.moved = false;
			int previousHole = this.currentHole;
			this.currentHole = -1;
			while (this.currentHole == -1)
			{
				this.currentHole = this.level.GetNextHole();
			}
			this.level.LeaveHole(previousHole);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.hideTime);
		}
		yield break;
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0000C40E File Offset: 0x0000A60E
	public void AnimationEvent_SFX_DOGFIGHT_PS_LeaderAttack()
	{
		AudioManager.Play("sfx_dlc_dogfight_ps_leader_batonattack");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_ps_leader_batonattack");
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_command");
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0008AD50 File Offset: 0x00088F50
	public void WORKAROUND_NullifyFields()
	{
		this.damageDealer = null;
		this.rocketBGPrefab = null;
		this.rocketPrefab = null;
		this.rocketBGEffect = null;
		this.level = null;
		this.terriers = null;
		this.rocketPositionString = null;
		this.terrierProjectileParryableString = null;
		this.boxCollider = null;
		this.rend = null;
		this.backerRend = null;
	}

	// Token: 0x04000B98 RID: 2968
	public bool isDead;

	// Token: 0x04000B99 RID: 2969
	public DamageDealer damageDealer;

	// Token: 0x04000B9A RID: 2970
	public DamageReceiver damageReceiver;

	// Token: 0x04000B9B RID: 2971
	[SerializeField]
	public BasicProjectile rocketBGPrefab;

	// Token: 0x04000B9C RID: 2972
	[SerializeField]
	public AirplaneLevelRocket rocketPrefab;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	public Effect rocketBGEffect;

	// Token: 0x04000B9E RID: 2974
	[SerializeField]
	public AirplaneLevel level;

	// Token: 0x04000B9F RID: 2975
	[SerializeField]
	public AirplaneLevelSecretTerrier[] terriers;

	// Token: 0x04000BA0 RID: 2976
	public PatternString rocketPositionString;

	// Token: 0x04000BA1 RID: 2977
	public PatternString terrierProjectileParryableString;

	// Token: 0x04000BA2 RID: 2978
	public bool attacked;

	// Token: 0x04000BA3 RID: 2979
	public bool moved;

	// Token: 0x04000BA4 RID: 2980
	public bool hiding;

	// Token: 0x04000BA5 RID: 2981
	public bool first = true;

	// Token: 0x04000BA6 RID: 2982
	[SerializeField]
	public int currentHole;

	// Token: 0x04000BA7 RID: 2983
	[SerializeField]
	public BoxCollider2D boxCollider;

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	public SpriteRenderer backerRend;

	// Token: 0x04000BAA RID: 2986
	public bool effectSide;
}
