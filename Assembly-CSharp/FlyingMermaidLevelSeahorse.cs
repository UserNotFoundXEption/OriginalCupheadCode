using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class FlyingMermaidLevelSeahorse : AbstractCollidableObject
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x00019288 File Offset: 0x00017488
	// (set) Token: 0x06001DB5 RID: 7605 RVA: 0x00019290 File Offset: 0x00017490
	public FlyingMermaidLevelSeahorse.State state { get; set; }

	// Token: 0x06001DB6 RID: 7606 RVA: 0x000B16E0 File Offset: 0x000AF8E0
	public override void Awake()
	{
		base.Awake();
		this.spray.enabled = false;
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 vector = base.transform.position;
		vector.y = this.spawnY;
		base.transform.position = vector;
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x00019299 File Offset: 0x00017499
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x000B176C File Offset: 0x000AF96C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelSeahorse.State.Dying)
		{
			AudioManager.Play("level_mermaid_seahorse_death");
			this.state = FlyingMermaidLevelSeahorse.State.Dying;
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x000192B1 File Offset: 0x000174B1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x000B17CC File Offset: 0x000AF9CC
	public void Init(LevelProperties.FlyingMermaid.Seahorse properties)
	{
		this.properties = properties;
		GroundHomingMovement component = base.GetComponent<GroundHomingMovement>();
		component.acceleration = properties.acceleration;
		component.maxSpeed = properties.maxSpeed;
		component.bounceRatio = properties.bounceRatio;
		this.hp = properties.hp;
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x000B1818 File Offset: 0x000AFA18
	public IEnumerator die_cr()
	{
		this.state = FlyingMermaidLevelSeahorse.State.Dying;
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = false;
		collider.enabled = false;
		base.animator.SetTrigger("SprayDeath");
		this.spray.End();
		AudioManager.Play("level_mermaid_seahorse_death");
		base.animator.SetTrigger("OnDeath");
		Transform deathFx = Object.Instantiate<Transform>(this.deathFxPrefab);
		deathFx.SetParent(this.deathFxRoot);
		deathFx.ResetLocalTransforms();
		yield return CupheadTime.WaitForSeconds(this, this.deathStayTime);
		float t = 0f;
		while (t < this.deathMoveTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y -= this.deathMoveDistance * CupheadTime.Delta / this.deathMoveTime;
			base.transform.localPosition = position;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x000B1834 File Offset: 0x000AFA34
	public IEnumerator intro_cr()
	{
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = false;
		collider.enabled = false;
		float t = 0f;
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y += this.riseDistance / this.riseTime * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		AudioManager.Play("level_mermaid_seahorse_intro");
		Animator animator = base.GetComponent<Animator>();
		animator.SetTrigger("Continue");
		yield return animator.WaitForAnimationToStart(this, "Spit_Start", false);
		this.spray.enabled = true;
		this.spray.Init(this.properties);
		yield return animator.WaitForAnimationToEnd(this, "Spit_Start", false, true);
		this.state = FlyingMermaidLevelSeahorse.State.Spit;
		base.StartCoroutine(this.spit_cr());
		yield break;
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x000B1850 File Offset: 0x000AFA50
	public IEnumerator spit_cr()
	{
		AudioManager.Play("level_mermaid_seahorse_spit");
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = true;
		collider.enabled = true;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			if (t > this.properties.homingDuration || ((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted)
			{
				break;
			}
			yield return null;
		}
		homer.EnableHoming = false;
		homer.bounceEnabled = false;
		base.animator.SetTrigger("SprayDeath");
		this.spray.End();
		yield break;
	}

	// Token: 0x04001856 RID: 6230
	[SerializeField]
	public float spawnY;

	// Token: 0x04001857 RID: 6231
	[SerializeField]
	public float riseTime;

	// Token: 0x04001858 RID: 6232
	[SerializeField]
	public float riseDistance;

	// Token: 0x04001859 RID: 6233
	[SerializeField]
	public float deathStayTime;

	// Token: 0x0400185A RID: 6234
	[SerializeField]
	public float deathMoveTime;

	// Token: 0x0400185B RID: 6235
	[SerializeField]
	public float deathMoveDistance;

	// Token: 0x0400185C RID: 6236
	[SerializeField]
	public FlyingMermaidLevelSeahorseSpray spray;

	// Token: 0x0400185D RID: 6237
	[SerializeField]
	public Transform deathFxRoot;

	// Token: 0x0400185E RID: 6238
	[SerializeField]
	public Transform deathFxPrefab;

	// Token: 0x0400185F RID: 6239
	public DamageDealer damageDealer;

	// Token: 0x04001860 RID: 6240
	public DamageReceiver damageReceiver;

	// Token: 0x04001861 RID: 6241
	public LevelProperties.FlyingMermaid.Seahorse properties;

	// Token: 0x04001862 RID: 6242
	public float hp;

	// Token: 0x02000D4F RID: 3407
	public enum State
	{
		// Token: 0x04006075 RID: 24693
		Intro,
		// Token: 0x04006076 RID: 24694
		Spit,
		// Token: 0x04006077 RID: 24695
		Dying
	}
}
