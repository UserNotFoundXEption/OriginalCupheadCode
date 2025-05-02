using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class BaronessLevelCandyCorn : BaronessLevelMiniBossBase
{
	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0000D5C9 File Offset: 0x0000B7C9
	// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x0000D5D1 File Offset: 0x0000B7D1
	public BaronessLevelCandyCorn.State state { get; set; }

	// Token: 0x06000FC3 RID: 4035 RVA: 0x0008E270 File Offset: 0x0008C470
	public override void Awake()
	{
		base.Awake();
		this.firstTime = true;
		this.isDying = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = BaronessLevelCandyCorn.State.Move;
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x0000D5DA File Offset: 0x0000B7DA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x0008E2C8 File Offset: 0x0008C4C8
	public void Init(LevelProperties.Baroness.CandyCorn properties, Vector2 pos, float speed, float health)
	{
		this.properties = properties;
		this.speed = speed;
		this.health = health;
		base.transform.position = pos;
		this.bottomPoint = pos.y;
		this.isTop = false;
		this.movingLeft = true;
		if (this.properties.spawnMinis)
		{
			base.StartCoroutine(this.spawnMinis_cr());
		}
		base.StartCoroutine(this.switchLayer_cr());
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0000D5F8 File Offset: 0x0000B7F8
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x0000D610 File Offset: 0x0000B810
	public void FixedUpdate()
	{
		if (this.state == BaronessLevelCandyCorn.State.Move)
		{
			if (this.moveY)
			{
				this.MoveAlongY();
			}
			else
			{
				this.MoveAlongX();
			}
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0008E344 File Offset: 0x0008C544
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health <= 0f && this.state != BaronessLevelCandyCorn.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelCandyCorn.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0008E3CC File Offset: 0x0008C5CC
	public IEnumerator switchLayer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield break;
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0000D639 File Offset: 0x0000B839
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.miniCandyPrefab = null;
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0008E3E8 File Offset: 0x0008C5E8
	public IEnumerator spawnMinis_cr()
	{
		this.targetPos = base.transform;
		Transform targetPos2 = this.targetPos;
		SpriteRenderer thisRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_A") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_B"))
			{
				BaronessLevelCandyCornMini miniCandyCorn = Object.Instantiate<BaronessLevelCandyCornMini>(this.miniCandyPrefab);
				miniCandyCorn.Init(base.transform.position, this.properties.miniCornMovementSpeed, (float)this.properties.miniCornHP);
				targetPos2 = miniCandyCorn.transform;
				SpriteRenderer r = miniCandyCorn.GetComponent<SpriteRenderer>();
				r.sortingLayerName = thisRenderer.sortingLayerName;
				r.sortingOrder = thisRenderer.sortingOrder - 1;
				yield return CupheadTime.WaitForSeconds(this, this.properties.miniCornSpawnDelay);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0008E404 File Offset: 0x0008C604
	public void MoveAlongX()
	{
		float num = 50f;
		float num2 = 10f;
		Vector3 vector;
		vector..ctor(this.properties.centerPosition, 0f, 0f);
		Vector3 vector2 = vector - base.transform.position;
		if (this.movingLeft)
		{
			if (base.transform.position.x > -640f + num)
			{
				base.transform.position -= base.transform.right * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
				if (vector2.x < num2 && vector2.x > -num2 && !this.justSwitchedMiddle)
				{
					this.checkIfSwitch();
				}
			}
			else
			{
				this.moveY = true;
				this.justSwitchedMiddle = false;
				this.movingLeft = false;
			}
		}
		else if (!this.movingLeft)
		{
			if (base.transform.position.x < (float)Level.Current.Right - num)
			{
				base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
				if (vector2.x < num2 && vector2.x > -num2 && !this.justSwitchedMiddle)
				{
					this.checkIfSwitch();
				}
			}
			else
			{
				this.moveY = true;
				this.justSwitchedMiddle = false;
				this.movingLeft = true;
			}
		}
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0008E5AC File Offset: 0x0008C7AC
	public void MoveAlongY()
	{
		float num = 125f;
		if (!this.isTop)
		{
			if (base.transform.position.y < 360f - num)
			{
				base.transform.position += base.transform.up * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			}
			else
			{
				this.isTop = true;
				this.moveY = false;
			}
		}
		else if (base.transform.position.y > this.bottomPoint)
		{
			base.transform.position -= base.transform.up * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		}
		else
		{
			this.isTop = false;
			this.moveY = false;
		}
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x0000D648 File Offset: 0x0000B848
	public void checkIfSwitch()
	{
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x0008E6A4 File Offset: 0x0008C8A4
	public IEnumerator switch_cr()
	{
		string[] pattern = this.properties.changeLevelString.GetRandom<string>().Split(new char[]
		{
			','
		});
		if (this.firstTime)
		{
			this.firstIndex = Random.Range(0, pattern.Length);
			this.firstTime = false;
		}
		if (pattern[this.firstIndex][0] == 'Y')
		{
			this.moveY = true;
			this.justSwitchedMiddle = true;
		}
		else if (pattern[this.firstIndex][0] == 'N')
		{
			this.moveY = false;
			this.justSwitchedMiddle = true;
		}
		if (this.firstIndex < pattern.Length - 1)
		{
			this.firstIndex++;
		}
		else
		{
			this.firstIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x0000D657 File Offset: 0x0000B857
	public void StartDeath()
	{
		this.state = BaronessLevelCandyCorn.State.Dying;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x0008E6C0 File Offset: 0x0008C8C0
	public IEnumerator death_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.deathMoveSpeed;
		this.StartExplosions();
		this.isDying = true;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (base.transform.position.y < 560f)
		{
			base.transform.position += Vector3.up * speed * CupheadTime.FixedDelta;
			speed += this.properties.deathAcceleration;
			yield return wait;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x0000D673 File Offset: 0x0000B873
	public void SoundCandyCornBite()
	{
		AudioManager.Play("level_baroness_candycorn_bite");
		this.emitAudioFromObject.Add("level_baroness_candycorn_bite");
	}

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	public BaronessLevelCandyCornMini miniCandyPrefab;

	// Token: 0x04000CD7 RID: 3287
	public LevelProperties.Baroness.CandyCorn properties;

	// Token: 0x04000CD8 RID: 3288
	public Transform targetPos;

	// Token: 0x04000CD9 RID: 3289
	public DamageDealer damageDealer;

	// Token: 0x04000CDA RID: 3290
	public DamageReceiver damageReceiver;

	// Token: 0x04000CDB RID: 3291
	public float health;

	// Token: 0x04000CDC RID: 3292
	public float speed;

	// Token: 0x04000CDD RID: 3293
	public float bottomPoint;

	// Token: 0x04000CDE RID: 3294
	public int firstIndex;

	// Token: 0x04000CDF RID: 3295
	public bool isTop;

	// Token: 0x04000CE0 RID: 3296
	public bool moveY;

	// Token: 0x04000CE1 RID: 3297
	public bool firstTime;

	// Token: 0x04000CE2 RID: 3298
	public bool justSwitchedMiddle;

	// Token: 0x04000CE3 RID: 3299
	public bool movingLeft;

	// Token: 0x02000A10 RID: 2576
	public enum State
	{
		// Token: 0x04004A8E RID: 19086
		Move,
		// Token: 0x04004A8F RID: 19087
		Dying
	}
}
