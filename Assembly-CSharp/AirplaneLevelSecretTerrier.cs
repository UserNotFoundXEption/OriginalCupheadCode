using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class AirplaneLevelSecretTerrier : LevelProperties.Airplane.Entity
{
	// Token: 0x06000E71 RID: 3697 RVA: 0x0008ADAC File Offset: 0x00088FAC
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.MoveToHolePosition();
		base.animator.Play("Intro_" + this.introNum[this.currentHole]);
		base.animator.Update(0f);
		this.firstAttack = true;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0000C463 File Offset: 0x0000A663
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0008AE2C File Offset: 0x0008902C
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.hp = properties.CurrentState.secretTerriers.dogRetreatDamage;
		this.level.OccupyHole(this.currentHole);
		base.transform.localScale = new Vector3(-Mathf.Sign(this.level.GetHolePosition(this.currentHole, false).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetHolePosition(this.currentHole, false);
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0000C488 File Offset: 0x0000A688
	public void AniEvent_StartTerriers()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x0008AED4 File Offset: 0x000890D4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.hp > 0f)
		{
			this.hp -= info.damage;
			if (this.hp <= 0f)
			{
				Level.Current.RegisterMinionKilled();
				this.StopAllCoroutines();
				this.coll.enabled = false;
				base.StartCoroutine(this.timeout_cr());
			}
		}
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0000C497 File Offset: 0x0000A697
	public int CurrentHole()
	{
		return this.currentHole;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0000C49F File Offset: 0x0000A69F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0008AF40 File Offset: 0x00089140
	public void MoveToHolePosition()
	{
		this.rend.sortingOrder = this.currentHole % 3 + 50;
		this.backerRend.sortingOrder = this.currentHole % 3 + 13;
		base.transform.localScale = new Vector3(-Mathf.Sign(this.level.GetHolePosition(this.currentHole, false).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetHolePosition(this.currentHole, false);
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0008AFE4 File Offset: 0x000891E4
	public void Die(int index)
	{
		this.StopAllCoroutines();
		while (this.currentHole == -1)
		{
			this.currentHole = this.level.GetNextHole();
			this.MoveToHolePosition();
		}
		string text = "Death_" + (index + 1).ToString();
		base.animator.Play(text);
		if (index + 1 == 1 || index + 1 == 4)
		{
			base.animator.Play("Stars", 1);
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0000C4B5 File Offset: 0x0000A6B5
	public void HideAnimationComplete()
	{
		this.moved = true;
		this.coll.enabled = false;
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0008B06C File Offset: 0x0008926C
	public void AttackAnimationComplete()
	{
		LevelProperties.Airplane.SecretTerriers secretTerriers = base.properties.CurrentState.secretTerriers;
		if (this.nextAttackPink)
		{
			this.bulletPrefabPink.Create(this.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, base.transform.localScale);
		}
		else
		{
			this.bulletPrefab.Create(this.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, base.transform.localScale);
		}
		this.attacked = true;
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplethrow");
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0008B110 File Offset: 0x00089310
	public void TryStartAttack()
	{
		this.nextAttackPink = this.leader.TerrierProjectileParryable();
		if (this.canAttack)
		{
			base.animator.SetTrigger((!this.nextAttackPink) ? "Attack" : "AttackPink");
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0008B160 File Offset: 0x00089360
	public IEnumerator attack_cr()
	{
		this.level.OccupyHole(this.currentHole);
		for (;;)
		{
			this.MoveToHolePosition();
			if (!this.firstAttack)
			{
				base.animator.Play("Emerge");
			}
			this.firstAttack = false;
			this.canAttack = true;
			this.coll.enabled = true;
			this.attacked = false;
			this.moved = false;
			while (!this.attacked)
			{
				yield return null;
			}
			this.canAttack = false;
			this.attacked = false;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretTerriers.dogPostAttackDelay);
			base.animator.SetTrigger("OnMove");
			while (!this.moved)
			{
				yield return null;
			}
			this.moved = false;
			int previousHole = this.currentHole;
			this.currentHole = -1;
			while (this.currentHole == -1)
			{
				this.currentHole = this.level.GetNextHole();
			}
			this.level.LeaveHole(previousHole);
		}
		yield break;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0008B17C File Offset: 0x0008937C
	public IEnumerator timeout_cr()
	{
		base.animator.ResetTrigger("Attack");
		base.animator.ResetTrigger("OnMove");
		base.animator.Play("Move");
		this.canAttack = false;
		this.level.LeaveHole(this.currentHole);
		this.currentHole = -1;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretTerriers.dogTimeOut);
		this.hp = base.properties.CurrentState.secretTerriers.dogRetreatDamage;
		while (this.currentHole == -1)
		{
			this.currentHole = this.level.GetNextHole();
			yield return null;
		}
		base.StartCoroutine(this.attack_cr());
		yield break;
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x0000C4CA File Offset: 0x0000A6CA
	public void AniEvent_PullGrenadePin()
	{
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplepinclink");
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0008B198 File Offset: 0x00089398
	public void WORKAROUND_NullifyFields()
	{
		this.damageDealer = null;
		this.bulletRoot = null;
		this.bulletPrefab = null;
		this.bulletPrefabPink = null;
		this.level = null;
		this.introNum = null;
		this.coll = null;
		this.leader = null;
		this.rend = null;
		this.backerRend = null;
	}

	// Token: 0x04000BAB RID: 2987
	public bool isDead;

	// Token: 0x04000BAC RID: 2988
	public DamageDealer damageDealer;

	// Token: 0x04000BAD RID: 2989
	public DamageReceiver damageReceiver;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x04000BAF RID: 2991
	[SerializeField]
	public AirplaneLevelSecretTerrierBullet bulletPrefab;

	// Token: 0x04000BB0 RID: 2992
	[SerializeField]
	public AirplaneLevelSecretTerrierBullet bulletPrefabPink;

	// Token: 0x04000BB1 RID: 2993
	[SerializeField]
	public AirplaneLevel level;

	// Token: 0x04000BB2 RID: 2994
	public bool attacked;

	// Token: 0x04000BB3 RID: 2995
	public bool moved;

	// Token: 0x04000BB4 RID: 2996
	public bool canAttack;

	// Token: 0x04000BB5 RID: 2997
	public float hp;

	// Token: 0x04000BB6 RID: 2998
	[SerializeField]
	public int currentHole;

	// Token: 0x04000BB7 RID: 2999
	public int[] introNum = new int[]
	{
		1,
		3,
		2,
		0,
		4
	};

	// Token: 0x04000BB8 RID: 3000
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04000BB9 RID: 3001
	[SerializeField]
	public AirplaneLevelSecretLeader leader;

	// Token: 0x04000BBA RID: 3002
	public bool firstAttack;

	// Token: 0x04000BBB RID: 3003
	public bool nextAttackPink;

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000BBD RID: 3005
	[SerializeField]
	public SpriteRenderer backerRend;
}
