using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class FlyingMermaidLevelTurtle : AbstractCollidableObject
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x000193C9 File Offset: 0x000175C9
	// (set) Token: 0x06001DD8 RID: 7640 RVA: 0x000193D1 File Offset: 0x000175D1
	public FlyingMermaidLevelTurtle.State state { get; set; }

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000B1ED8 File Offset: 0x000B00D8
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 vector = base.transform.position;
		vector.y = this.spawnY;
		base.transform.position = vector;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000193DA File Offset: 0x000175DA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000B1F58 File Offset: 0x000B0158
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelTurtle.State.Dying)
		{
			this.state = FlyingMermaidLevelTurtle.State.Dying;
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000193F2 File Offset: 0x000175F2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x00019410 File Offset: 0x00017610
	public void Init(LevelProperties.FlyingMermaid.Turtle properties)
	{
		this.properties = properties;
		this.hp = properties.hp;
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B1FB0 File Offset: 0x000B01B0
	public IEnumerator die_cr()
	{
		AudioManager.Play("level_mermaid_turtle_flag");
		this.state = FlyingMermaidLevelTurtle.State.Dying;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.animator.SetTrigger("OnDeath");
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

	// Token: 0x06001DDF RID: 7647 RVA: 0x000B1FCC File Offset: 0x000B01CC
	public IEnumerator intro_cr()
	{
		AudioManager.Play("level_mermaid_turtle_enter");
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		float t = 0f;
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y += this.riseDistance / this.riseTime * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		Animator animator = base.GetComponent<Animator>();
		animator.SetTrigger("Continue");
		yield return animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.state = FlyingMermaidLevelTurtle.State.Idle;
		base.StartCoroutine(this.pattern_cr());
		base.StartCoroutine(this.move_cr());
		foreach (Collider2D collider2D2 in base.GetComponents<Collider2D>())
		{
			collider2D2.enabled = true;
		}
		yield break;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000B1FE8 File Offset: 0x000B01E8
	public IEnumerator move_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		for (;;)
		{
			if (this.moving)
			{
				Vector2 vector = base.transform.localPosition;
				vector.x -= this.properties.speed * CupheadTime.Delta;
				base.transform.localPosition = vector;
				if (vector.x < (float)Level.Current.Left - sprite.bounds.size.x / 2f)
				{
					Object.Destroy(base.gameObject);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B2004 File Offset: 0x000B0204
	public IEnumerator pattern_cr()
	{
		string[] pattern = this.properties.explodeSpreadshotString.GetRandom<string>().Split(new char[]
		{
			','
		});
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeUntilShoot.RandomFloat());
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				float waitTime = 0f;
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else if (!((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted)
			{
				this.currentExplodePattern = pattern[i];
				AudioManager.Play("level_mermaid_turtle_shell_pop");
				base.animator.SetTrigger("Shoot");
				yield return base.animator.WaitForAnimationToEnd(this, "Idle", false, true);
				this.moving = false;
				yield return base.animator.WaitForAnimationToEnd(this, "Shoot_B", false, true);
				this.moving = true;
				AudioManager.Play("level_mermaid_turtle_post_cannon");
			}
		}
		yield break;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000B2020 File Offset: 0x000B0220
	public void OnShootFX()
	{
		this.shootEffectPrefab.Create(this.shootEffectRoot.position);
		this.cannonBallPrefab.Create(this.cannonBallRoot.transform.position, this.currentExplodePattern, this.properties);
	}

	// Token: 0x04001878 RID: 6264
	[SerializeField]
	public float spawnY;

	// Token: 0x04001879 RID: 6265
	[SerializeField]
	public float riseTime;

	// Token: 0x0400187A RID: 6266
	[SerializeField]
	public float riseDistance;

	// Token: 0x0400187B RID: 6267
	[SerializeField]
	public float deathStayTime;

	// Token: 0x0400187C RID: 6268
	[SerializeField]
	public float deathMoveTime;

	// Token: 0x0400187D RID: 6269
	[SerializeField]
	public float deathMoveDistance;

	// Token: 0x0400187E RID: 6270
	[SerializeField]
	public FlyingMermaidLevelTurtleCannonBall cannonBallPrefab;

	// Token: 0x0400187F RID: 6271
	[SerializeField]
	public Transform cannonBallRoot;

	// Token: 0x04001880 RID: 6272
	[SerializeField]
	public Transform shootEffectRoot;

	// Token: 0x04001881 RID: 6273
	[SerializeField]
	public Effect shootEffectPrefab;

	// Token: 0x04001882 RID: 6274
	public DamageDealer damageDealer;

	// Token: 0x04001883 RID: 6275
	public DamageReceiver damageReceiver;

	// Token: 0x04001884 RID: 6276
	public LevelProperties.FlyingMermaid.Turtle properties;

	// Token: 0x04001885 RID: 6277
	public float hp;

	// Token: 0x04001886 RID: 6278
	public bool moving = true;

	// Token: 0x04001887 RID: 6279
	public string currentExplodePattern;

	// Token: 0x02000D56 RID: 3414
	public enum State
	{
		// Token: 0x0400609F RID: 24735
		Intro,
		// Token: 0x040060A0 RID: 24736
		Idle,
		// Token: 0x040060A1 RID: 24737
		Dying
	}
}
