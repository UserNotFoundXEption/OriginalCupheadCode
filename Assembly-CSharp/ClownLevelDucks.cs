using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class ClownLevelDucks : AbstractProjectile
{
	// Token: 0x060013FA RID: 5114 RVA: 0x00099344 File Offset: 0x00097544
	public override void Awake()
	{
		base.Awake();
		this.bombDropped = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.body.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.collisionChild = this.body.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		if (this.isBombDuck)
		{
			this.bomb.GetComponent<Transform>();
			this.bomb.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00010C83 File Offset: 0x0000EE83
	public ClownLevelDucks Init(Vector2 pos, LevelProperties.Clown.Duck properties, float maxYPos, float speedY)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.maxYPos = maxYPos;
		this.speedY = speedY;
		this.originalSpeed = this.speedY;
		return this;
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x00010CB9 File Offset: 0x0000EEB9
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00010CCE File Offset: 0x0000EECE
	public override void Update()
	{
		base.Update();
		this.VaryingSpeed();
		if (this.isBombDuck)
		{
			this.BombCheck();
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x00010D03 File Offset: 0x0000EF03
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.StartCoroutine(this.spin_cr());
		if (this.isBombDuck && !this.bombDropped)
		{
			base.StartCoroutine(this.drop_bomb_cr());
		}
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x00010D35 File Offset: 0x0000EF35
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x00010D53 File Offset: 0x0000EF53
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.sparkPrefab = null;
		this.explosionPrefab = null;
		this.smokePrefab = null;
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x000993FC File Offset: 0x000975FC
	public IEnumerator move_cr()
	{
		bool goingUp = false;
		float stopDist = 100f;
		float endPos = 360f - this.maxYPos;
		this.slowDown = true;
		while (base.transform.position.x > -840f)
		{
			Vector3 pos = base.transform.position;
			while (base.transform.position.y != endPos)
			{
				float dist = base.transform.position.y - endPos;
				dist = Mathf.Abs(dist);
				if (dist < stopDist)
				{
					this.slowDown = true;
				}
				pos.x -= this.properties.duckXMovementSpeed * CupheadTime.Delta;
				pos.y = Mathf.MoveTowards(base.transform.position.y, endPos, this.speedY * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
			goingUp = !goingUp;
			endPos = ((!goingUp) ? (360f - this.maxYPos) : 360f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x00099418 File Offset: 0x00097618
	public void VaryingSpeed()
	{
		float num = 4f;
		if (this.slowDown)
		{
			if (this.speedY <= this.originalSpeed / 3f)
			{
				this.slowDown = false;
			}
			else
			{
				this.speedY -= num;
			}
		}
		else if (this.speedY < this.originalSpeed)
		{
			this.speedY += num;
		}
		else
		{
			this.speedY = this.originalSpeed;
		}
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0009949C File Offset: 0x0009769C
	public IEnumerator spin_cr()
	{
		AudioManager.Play("clown_regular_duck_spin");
		this.emitAudioFromObject.Add("clown_regular_duck_spin");
		Effect spark = Object.Instantiate<Effect>(this.sparkPrefab);
		spark.transform.position = base.transform.position;
		base.animator.SetBool("Spin", true);
		base.transform.GetComponent<Collider2D>().enabled = false;
		this.body.transform.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.spinDuration);
		base.animator.SetBool("Spin", false);
		base.transform.GetComponent<Collider2D>().enabled = true;
		this.body.transform.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x000994B8 File Offset: 0x000976B8
	public void BombCheck()
	{
		this.player = PlayerManager.GetNext();
		float num = 10f;
		float num2 = this.player.transform.position.x - base.transform.position.x;
		if (num2 > -num && num2 < num && !this.bombDropped)
		{
			base.StartCoroutine(this.drop_bomb_cr());
		}
	}

	// Token: 0x06001405 RID: 5125 RVA: 0x0009952C File Offset: 0x0009772C
	public IEnumerator drop_bomb_cr()
	{
		float vel = this.properties.bombSpeed;
		float acceleration = 5f;
		this.bombDropped = true;
		this.bomb.transform.parent = null;
		this.bomb.GetComponent<Animator>().SetBool("Fall", true);
		while (this.bomb.transform.position.y > (float)Level.Current.Ground)
		{
			this.bomb.transform.AddPosition(0f, -vel * CupheadTime.Delta, 0f);
			vel += acceleration;
			yield return null;
		}
		this.bomb.GetComponent<Animator>().SetBool("Fall", false);
		Effect explosion = Object.Instantiate<Effect>(this.explosionPrefab);
		explosion.transform.position = this.bomb.transform.position;
		int num = Random.Range(0, 3);
		if (num == 3)
		{
			Effect effect = Object.Instantiate<Effect>(this.smokePrefab);
			effect.transform.position = this.bomb.transform.position;
		}
		AudioManager.Play("clown_bulb_explosion");
		this.emitAudioFromObject.Add("clown_bulb_explosion");
		this.CreatePieces();
		Object.Destroy(this.bomb.gameObject);
		yield break;
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x00099548 File Offset: 0x00097748
	public void CreatePieces()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(this.bomb.transform.position);
		}
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x00010D70 File Offset: 0x0000EF70
	public override void OnParry(AbstractPlayerController player)
	{
		base.animator.SetBool("Spin", true);
		base.transform.GetComponent<Collider2D>().enabled = false;
		this.body.transform.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x00010DAA File Offset: 0x0000EFAA
	public override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x04001041 RID: 4161
	public bool isBombDuck;

	// Token: 0x04001042 RID: 4162
	[SerializeField]
	public Effect explosionPrefab;

	// Token: 0x04001043 RID: 4163
	[SerializeField]
	public Effect smokePrefab;

	// Token: 0x04001044 RID: 4164
	[SerializeField]
	public Effect sparkPrefab;

	// Token: 0x04001045 RID: 4165
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04001046 RID: 4166
	[SerializeField]
	public Transform bomb;

	// Token: 0x04001047 RID: 4167
	[SerializeField]
	public GameObject body;

	// Token: 0x04001048 RID: 4168
	public LevelProperties.Clown.Duck properties;

	// Token: 0x04001049 RID: 4169
	public AbstractPlayerController player;

	// Token: 0x0400104A RID: 4170
	public DamageReceiver damageReceiver;

	// Token: 0x0400104B RID: 4171
	public CollisionChild collisionChild;

	// Token: 0x0400104C RID: 4172
	public float maxYPos;

	// Token: 0x0400104D RID: 4173
	public float speedY;

	// Token: 0x0400104E RID: 4174
	public float originalSpeed;

	// Token: 0x0400104F RID: 4175
	public bool slowDown;

	// Token: 0x04001050 RID: 4176
	public bool bombDropped;
}
