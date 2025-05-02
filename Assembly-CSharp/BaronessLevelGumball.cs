using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class BaronessLevelGumball : BaronessLevelMiniBossBase
{
	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0000D906 File Offset: 0x0000BB06
	// (set) Token: 0x06000FFB RID: 4091 RVA: 0x0000D90E File Offset: 0x0000BB0E
	public BaronessLevelGumball.State state { get; set; }

	// Token: 0x06000FFC RID: 4092 RVA: 0x0008EC70 File Offset: 0x0008CE70
	public override void Awake()
	{
		base.Awake();
		this.fadeTime = 0.6f;
		this.isDying = false;
		this.movingLeft = true;
		base.RegisterCollisionChild(this.headCollider);
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiverChild = this.headCollider.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiverChild.OnDamageTaken += this.OnDamageTaken;
		this.headCollider.OnPlayerCollision += this.OnCollisionPlayer;
		this.headCollider.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0008ED30 File Offset: 0x0008CF30
	public override void Start()
	{
		base.Start();
		this.legs.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.legs.GetComponent<SpriteRenderer>().sortingOrder = 130;
		this.lid.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingOrder = 140;
		AudioManager.PlayLoop("level_baroness_gumball_feet_loop");
		this.emitAudioFromObject.Add("level_baroness_gumball_feet_loop");
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0008EDC8 File Offset: 0x0008CFC8
	public void Init(LevelProperties.Baroness.Gumball properties, Vector2 pos, float health)
	{
		this.properties = properties;
		this.health = health;
		base.transform.position = pos;
		this.offTime = properties.gumballAttackDurationOffRange;
		base.StartCoroutine(this.leaving_castle_cr());
		base.StartCoroutine(this.switch_child_cr());
		base.StartCoroutine(this.gumball_off_timer_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0008EE3C File Offset: 0x0008D03C
	public virtual IEnumerator leaving_castle_cr()
	{
		float t = 0f;
		float offTime = 0.22f;
		while (t < offTime)
		{
			this.lid.GetComponent<SpriteRenderer>().enabled = false;
			base.GetComponent<SpriteRenderer>().enabled = false;
			t += CupheadTime.Delta;
			yield return null;
		}
		this.lid.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0008EE58 File Offset: 0x0008D058
	public IEnumerator switch_child_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.legs.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingOrder = 252;
		this.legs.GetComponent<SpriteRenderer>().sortingOrder = 251;
		yield break;
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0000D917 File Offset: 0x0000BB17
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x0000D935 File Offset: 0x0000BB35
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x0008EE74 File Offset: 0x0008D074
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelGumball.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelGumball.State.Dying;
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x0000D94D File Offset: 0x0000BB4D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefabs = null;
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0008EF00 File Offset: 0x0008D100
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		bool endedLoop = false;
		bool movingRight = false;
		float time = this.properties.gumballMovementSpeed;
		float end = 0f;
		float t = 0f;
		for (;;)
		{
			float start = base.transform.position.x;
			if (movingRight)
			{
				end = 640f - this.properties.offsetX.max;
			}
			else
			{
				end = -640f + this.properties.offsetX.min;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				if (val > 0.8f && !endedLoop)
				{
					if (this.isDying && !movingRight)
					{
						break;
					}
					if (this.state == BaronessLevelGumball.State.On)
					{
						this.headSpark.SetActive(false);
					}
					base.animator.SetBool("Turn", true);
					base.animator.Play("Run_Legs");
					endedLoop = true;
				}
				t += CupheadTime.FixedDelta;
				yield return wait;
			}
			if (this.isDying)
			{
				break;
			}
			endedLoop = false;
			t = 0f;
			base.transform.SetPosition(new float?(end), null, null);
			movingRight = !movingRight;
			yield return wait;
		}
		while (base.transform.position.x > -940f)
		{
			base.transform.AddPosition(-this.properties.gumballDeathSpeed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		AudioManager.Stop("level_baroness_gumball_feet_loop");
		this.Die();
		yield break;
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0008EF1C File Offset: 0x0008D11C
	public void Switch()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		this.feetDust.SetScale(new float?(-this.feetDust.localScale.x), new float?(1f), new float?(1f));
		base.animator.SetBool("Turn", false);
		if (this.state == BaronessLevelGumball.State.On)
		{
			this.headSpark.SetActive(true);
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0008EFC4 File Offset: 0x0008D1C4
	public IEnumerator on_cr()
	{
		float rateTime = 0f;
		float attackTime = 0f;
		float attackDuration = this.properties.gumballAttackDurationOnRange.RandomFloat();
		base.animator.SetBool("Open", true);
		yield return base.animator.WaitForAnimationToStart(this, "Run_Open_Trans", false);
		AudioManager.PlayLoop("level_baroness_gumball_shoot_loop");
		this.emitAudioFromObject.Add("level_baroness_gumball_shoot_loop");
		this.headSpark.SetActive(true);
		this.state = BaronessLevelGumball.State.On;
		while (attackTime < attackDuration)
		{
			if (this.isDying)
			{
				break;
			}
			attackTime += CupheadTime.Delta;
			if (rateTime > this.properties.rateOfFire)
			{
				this.fireProjectiles();
				rateTime = 0f;
			}
			else
			{
				rateTime += CupheadTime.Delta;
			}
			yield return null;
		}
		base.animator.SetBool("Open", false);
		yield return new WaitForEndOfFrame();
		base.StartCoroutine(this.gumball_off_timer_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0008EFE0 File Offset: 0x0008D1E0
	public void fireProjectiles()
	{
		Vector2 zero = Vector2.zero;
		float num = (float)((!this.movingLeft) ? 200 : -200);
		zero.y = this.properties.velocityY.RandomFloat();
		zero.x = this.properties.velocityX.RandomFloat() + num;
		this.projectilePrefabs[Random.Range(0, this.projectilePrefabs.Length - 1)].Create(this.projectileRoot.position, zero, this.properties.gravity);
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0008F078 File Offset: 0x0008D278
	public IEnumerator gumball_off_timer_cr()
	{
		this.headSpark.SetActive(false);
		AudioManager.Stop("level_baroness_gumball_shoot_loop");
		this.state = BaronessLevelGumball.State.Off;
		this.offTime = this.properties.gumballAttackDurationOffRange.RandomFloat();
		yield return CupheadTime.WaitForSeconds(this, this.offTime);
		base.StartCoroutine(this.on_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0008F094 File Offset: 0x0008D294
	public IEnumerator death_cr()
	{
		this.StartExplosions();
		this.headCollider.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.isDying = true;
		base.animator.Play("Run_Death");
		base.animator.SetTrigger("Death");
		yield return null;
		yield break;
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0000D95C File Offset: 0x0000BB5C
	public void SoundGumballLidOpen()
	{
		AudioManager.Play("level_baroness_gumball_lid_open");
		this.emitAudioFromObject.Add("level_baroness_gumball_lid_open");
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0000D978 File Offset: 0x0000BB78
	public void SoundGumballLidClose()
	{
		AudioManager.Play("level_baroness_gumball_lid_close");
		this.emitAudioFromObject.Add("level_baroness_gumball_lid_close");
	}

	// Token: 0x04000CFE RID: 3326
	[SerializeField]
	public BaronessLevelGumballProjectile[] projectilePrefabs;

	// Token: 0x04000CFF RID: 3327
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04000D00 RID: 3328
	[SerializeField]
	public SpriteRenderer lid;

	// Token: 0x04000D01 RID: 3329
	[SerializeField]
	public SpriteRenderer legs;

	// Token: 0x04000D02 RID: 3330
	[SerializeField]
	public CollisionChild headCollider;

	// Token: 0x04000D03 RID: 3331
	[SerializeField]
	public GameObject headSpark;

	// Token: 0x04000D04 RID: 3332
	[SerializeField]
	public Transform feetDust;

	// Token: 0x04000D05 RID: 3333
	public LevelProperties.Baroness.Gumball properties;

	// Token: 0x04000D06 RID: 3334
	public DamageDealer damageDealer;

	// Token: 0x04000D07 RID: 3335
	public DamageReceiver damageReceiver;

	// Token: 0x04000D08 RID: 3336
	public DamageReceiver damageReceiverChild;

	// Token: 0x04000D09 RID: 3337
	public float health;

	// Token: 0x04000D0A RID: 3338
	public float offTime;

	// Token: 0x04000D0B RID: 3339
	public float onTime;

	// Token: 0x04000D0C RID: 3340
	public bool movingLeft;

	// Token: 0x04000D0D RID: 3341
	public bool slowDown;

	// Token: 0x02000A1B RID: 2587
	public enum State
	{
		// Token: 0x04004ACC RID: 19148
		On,
		// Token: 0x04004ACD RID: 19149
		Off,
		// Token: 0x04004ACE RID: 19150
		Dying
	}
}
