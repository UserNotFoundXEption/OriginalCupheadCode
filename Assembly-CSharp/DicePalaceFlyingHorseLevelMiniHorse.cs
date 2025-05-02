using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class DicePalaceFlyingHorseLevelMiniHorse : AbstractProjectile
{
	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00013569 File Offset: 0x00011769
	public override float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x0009F8EC File Offset: 0x0009DAEC
	public override void Awake()
	{
		base.Awake();
		this.jockey.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.damageReceiver = this.jockey.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.jockeyAnimator = this.jockey.GetComponent<Animator>();
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x00013570 File Offset: 0x00011770
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && !this.jockeyDead)
		{
			this.KillJockey();
			this.jockeyDead = true;
		}
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x000135AD File Offset: 0x000117AD
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x000135CB File Offset: 0x000117CB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x0009F958 File Offset: 0x0009DB58
	public void Init(Vector3 position, float hp, LevelProperties.DicePalaceFlyingHorse.MiniHorses properties, AbstractPlayerController player, DicePalaceFlyingHorseLevelHorse.MiniHorseType type, bool isPink, float threeProximity, int lane, Vector3 backgroundLane)
	{
		base.transform.position = position;
		this.hp = hp;
		this.properties = properties;
		this.isPink = isPink;
		this.player = player;
		this.threeProximity = threeProximity;
		this.backgroundLane = backgroundLane;
		base.animator.SetInteger("Horse", Random.Range(1, 3));
		if (type != DicePalaceFlyingHorseLevelHorse.MiniHorseType.One)
		{
			if (type != DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two)
			{
				if (type == DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three)
				{
					this.jockeyAnimator.SetInteger("Caddy", 4);
					this.horseCoroutine = base.StartCoroutine(this.horse_three_cr());
				}
			}
			else
			{
				this.jockeyAnimator.SetInteger("Caddy", Random.Range(1, 4));
				this.horseCoroutine = base.StartCoroutine(this.horse_two_cr());
			}
		}
		else
		{
			this.jockeyAnimator.SetInteger("Caddy", Random.Range(1, 4));
		}
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].sortingOrder = this.renderers.Length * lane + this.renderers[i].sortingOrder;
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x0009FA90 File Offset: 0x0009DC90
	public IEnumerator move_cr()
	{
		float speed = this.properties.miniSpeedRange.RandomFloat();
		while (base.transform.position.x > -740f)
		{
			base.transform.AddPosition(-speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		base.transform.position = this.backgroundLane;
		base.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
		SpriteRenderer horseRenderer = base.GetComponent<SpriteRenderer>();
		horseRenderer.color = ColorUtils.HexToColor("C5C5C5FF");
		horseRenderer.sortingLayerName = "Default";
		horseRenderer.sortingOrder -= 100;
		if (this.jockey != null)
		{
			SpriteRenderer component = this.jockey.GetComponent<SpriteRenderer>();
			component.material = horseRenderer.material;
			component.color = horseRenderer.color;
			component.sortingLayerName = "Default";
			component.sortingOrder -= 100;
			this.jockey.GetComponent<Collider2D>().enabled = false;
		}
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (base.transform.position.x < 740f)
		{
			base.transform.AddPosition(speed * CupheadTime.Delta * 0.5f, 0f, 0f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x0009FAAC File Offset: 0x0009DCAC
	public IEnumerator horse_two_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniTwoShotDelayRange.RandomFloat());
		if (!this.jockeyDead)
		{
			this.ShootBullet();
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x0009FAC8 File Offset: 0x0009DCC8
	public void ShootBullet()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 vector = this.player.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		if (this.isPink)
		{
			this.pinkBullet.Create(base.transform.position, rotation, this.properties.miniTwoBulletSpeed);
		}
		else
		{
			this.bullet.Create(base.transform.position, rotation, this.properties.miniTwoBulletSpeed);
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x0009FB90 File Offset: 0x0009DD90
	public IEnumerator horse_three_cr()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		float dist = base.transform.position.x - this.player.transform.position.x;
		while (dist > this.threeProximity)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			dist = base.transform.position.x - this.player.transform.position.x;
			yield return null;
		}
		if (!this.jockeyDead)
		{
			this.jockeyAnimator.SetTrigger("Attack");
			yield return this.jockeyAnimator.WaitForAnimationToStart(this, "CloakedAttack_End", false);
		}
		if (!this.jockeyDead)
		{
			this.jockey.transform.SetParent(null);
			this.jockey.transform.GetChild(0).SetParent(base.transform);
		}
		while (this.jockey.transform.position.y < 360f && !this.jockeyDead)
		{
			this.jockey.transform.AddPosition(0f, this.properties.miniThreeJockeySpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		if (!this.jockeyDead)
		{
			this.KillJockey();
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x000135E9 File Offset: 0x000117E9
	public override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x000135F7 File Offset: 0x000117F7
	public void KillJockey()
	{
		base.StopCoroutine(this.horseCoroutine);
		Object.Destroy(this.jockey);
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x00013610 File Offset: 0x00011810
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04001273 RID: 4723
	[SerializeField]
	public BasicProjectile bullet;

	// Token: 0x04001274 RID: 4724
	[SerializeField]
	public BasicProjectile pinkBullet;

	// Token: 0x04001275 RID: 4725
	[SerializeField]
	public GameObject jockey;

	// Token: 0x04001276 RID: 4726
	[SerializeField]
	public SpriteRenderer[] renderers;

	// Token: 0x04001277 RID: 4727
	public LevelProperties.DicePalaceFlyingHorse.MiniHorses properties;

	// Token: 0x04001278 RID: 4728
	public AbstractPlayerController player;

	// Token: 0x04001279 RID: 4729
	public DamageReceiver damageReceiver;

	// Token: 0x0400127A RID: 4730
	public Coroutine horseCoroutine;

	// Token: 0x0400127B RID: 4731
	public float hp;

	// Token: 0x0400127C RID: 4732
	public float threeProximity;

	// Token: 0x0400127D RID: 4733
	public bool isPink;

	// Token: 0x0400127E RID: 4734
	public bool jockeyDead;

	// Token: 0x0400127F RID: 4735
	public Vector3 backgroundLane;

	// Token: 0x04001280 RID: 4736
	public Animator jockeyAnimator;
}
