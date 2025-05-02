using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class ClownLevelEnemy : AbstractProjectile
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600140A RID: 5130 RVA: 0x00010DC0 File Offset: 0x0000EFC0
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x0009958C File Offset: 0x0009778C
	public ClownLevelEnemy Create(Vector3 pos, float targetPosition, float HP, LevelProperties.Clown.Swing properties, ClownLevelClownSwing parent)
	{
		ClownLevelEnemy clownLevelEnemy = base.Create(pos) as ClownLevelEnemy;
		clownLevelEnemy.transform.position = pos;
		clownLevelEnemy.properties = properties;
		clownLevelEnemy.targetPosition = targetPosition;
		clownLevelEnemy.HP = HP;
		clownLevelEnemy.parent = parent;
		return clownLevelEnemy;
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000995D8 File Offset: 0x000977D8
	public override void Start()
	{
		base.Start();
		ClownLevelClownSwing clownLevelClownSwing = this.parent;
		clownLevelClownSwing.OnDeath = (Action)Delegate.Combine(clownLevelClownSwing.OnDeath, new Action(this.Die));
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		AudioManager.Play("clown_penguin_roll_start");
		this.emitAudioFromObject.Add("clown_penguin_roll_start");
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x00010DC7 File Offset: 0x0000EFC7
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x00010DE5 File Offset: 0x0000EFE5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x00010E03 File Offset: 0x0000F003
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDead)
		{
			this.isDead = true;
			this.Die();
		}
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x00010E40 File Offset: 0x0000F040
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.GetComponent<ClownLevelCoaster>())
		{
			this.Die();
		}
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x00010E60 File Offset: 0x0000F060
	public void StartMoving()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x0009965C File Offset: 0x0009785C
	public IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		float target = -640f + this.targetPosition;
		while (base.transform.position.x != target)
		{
			pos.x = Mathf.MoveTowards(base.transform.position.x, target, this.properties.movementSpeed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		AudioManager.Play("clown_penguin_roll_end");
		this.emitAudioFromObject.Add("clown_penguin_roll_end");
		this.damageReceiver.enabled = true;
		base.StartCoroutine(this.shoot_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x00099678 File Offset: 0x00097878
	public IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.initialAttackDelay);
		for (;;)
		{
			base.animator.SetTrigger("OnAttack");
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x00099694 File Offset: 0x00097894
	public void ShootBullet()
	{
		AudioManager.Play("clown_penguin_clap");
		this.emitAudioFromObject.Add("clown_penguin_clap");
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - base.transform.position;
		this.bullet.Create(this.root.transform.position, MathUtils.DirectionToAngle(vector), this.properties.bulletSpeed);
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x00099714 File Offset: 0x00097914
	public override void Die()
	{
		AudioManager.Play("clown_penguin_death");
		this.emitAudioFromObject.Add("clown_penguin_death");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		ClownLevelClownSwing clownLevelClownSwing = this.parent;
		clownLevelClownSwing.OnDeath = (Action)Delegate.Remove(clownLevelClownSwing.OnDeath, new Action(this.Die));
		base.Die();
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0009978C File Offset: 0x0009798C
	public void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x00010E6F File Offset: 0x0000F06F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bullet = null;
	}

	// Token: 0x04001051 RID: 4177
	[SerializeField]
	public BasicProjectile bullet;

	// Token: 0x04001052 RID: 4178
	[SerializeField]
	public Transform root;

	// Token: 0x04001053 RID: 4179
	public LevelProperties.Clown.Swing properties;

	// Token: 0x04001054 RID: 4180
	public ClownLevelClownSwing parent;

	// Token: 0x04001055 RID: 4181
	public ClownLevelCoasterHandler handler;

	// Token: 0x04001056 RID: 4182
	public DamageReceiver damageReceiver;

	// Token: 0x04001057 RID: 4183
	public float targetPosition;

	// Token: 0x04001058 RID: 4184
	public float HP;

	// Token: 0x04001059 RID: 4185
	public bool isDead;
}
