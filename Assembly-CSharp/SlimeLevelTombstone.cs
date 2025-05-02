using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class SlimeLevelTombstone : LevelProperties.Slime.Entity
{
	// Token: 0x17000329 RID: 809
	// (get) Token: 0x060027F9 RID: 10233 RVA: 0x00021889 File Offset: 0x0001FA89
	// (set) Token: 0x060027FA RID: 10234 RVA: 0x00021891 File Offset: 0x0001FA91
	public SlimeLevelTombstone.State state { get; set; }

	// Token: 0x060027FB RID: 10235 RVA: 0x000CD17C File Offset: 0x000CB37C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.GetComponent<LevelBossDeathExploder>().enabled = false;
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x0002189A File Offset: 0x0001FA9A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x000218B2 File Offset: 0x0001FAB2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.dealDamage && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000218DB File Offset: 0x0001FADB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000218EE File Offset: 0x0001FAEE
	public override void LevelInit(LevelProperties.Slime properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x000218F7 File Offset: 0x0001FAF7
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dustPrefab = null;
		this.smashDustBackPrefab = null;
		this.smashDustFrontPrefab = null;
		this.tinySlime = null;
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000CD1F0 File Offset: 0x000CB3F0
	public void StartIntro(float x)
	{
		this.state = SlimeLevelTombstone.State.Intro;
		base.transform.SetPosition(new float?(x), null, null);
		this.offsetIndex = Random.Range(0, base.properties.CurrentState.tombstone.attackOffsetString.Length);
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		base.properties.OnBossDeath += this.OnBossDeath;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000CD2A4 File Offset: 0x000CB4A4
	public IEnumerator intro_cr()
	{
		base.StartCoroutine(this.crush_slime_cr());
		AudioManager.Play("slime_tombstone_drop_onto_slime");
		this.emitAudioFromObject.Add("slime_tombstone_drop_onto_slime");
		yield return base.TweenPositionY(550f, -80f, 0.2f, EaseUtils.EaseType.linear);
		base.animator.SetTrigger("Continue");
		this.dustPrefab.Create(base.transform.position);
		this.StartMove();
		yield break;
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000CD2C0 File Offset: 0x000CB4C0
	public IEnumerator crush_slime_cr()
	{
		while (base.transform.position.y > 70f)
		{
			yield return null;
		}
		this.bigSlime.Explode();
		if (SlimeLevelSlime.TINIES)
		{
			SlimeLevelTinySlime slimeLevelTinySlime = Object.Instantiate<SlimeLevelTinySlime>(this.tinySlime);
			SlimeLevelTinySlime slimeLevelTinySlime2 = Object.Instantiate<SlimeLevelTinySlime>(this.tinySlime);
			slimeLevelTinySlime.Init(this.dust.transform.position, base.properties.CurrentState.tombstone, true, this);
			slimeLevelTinySlime2.Init(this.dust.transform.position, base.properties.CurrentState.tombstone, false, this);
		}
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield break;
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x0002191B File Offset: 0x0001FB1B
	public void StartMove()
	{
		this.state = SlimeLevelTombstone.State.Move;
		this.wantsToSmash = false;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.waitForSmash_cr());
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000CD2DC File Offset: 0x000CB4DC
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.direction = ((!MathUtils.RandomBool()) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
		string[] offsets = base.properties.CurrentState.tombstone.attackOffsetString.Split(new char[]
		{
			','
		});
		this.offsetIndex = (this.offsetIndex + 1) % offsets.Length;
		float offset = 0f;
		Parser.FloatTryParse(offsets[this.offsetIndex], out offset);
		bool justStarted = true;
		while (!this.wantsToSmash)
		{
			base.animator.SetTrigger((this.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
			yield return base.animator.WaitForAnimationToStart(this, (this.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
			base.animator.Play("Dirt");
			if (justStarted)
			{
				base.animator.Play("Dust_Start");
			}
			else
			{
				base.animator.Play("Dust_Start_End");
			}
			AudioManager.Play("slime_tombstone_slide");
			this.emitAudioFromObject.Add("slime_tombstone_slide");
			float startX = base.transform.position.x;
			float endX = (this.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
			float moveTime = Mathf.Abs(startX - endX) / base.properties.CurrentState.tombstone.moveSpeed;
			yield return base.TweenPositionX(startX, endX, moveTime, EaseUtils.EaseType.easeInOutSine);
			this.direction = ((this.direction != SlimeLevelTombstone.Direction.Right) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
			justStarted = false;
		}
		base.animator.SetTrigger((this.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
		yield return base.animator.WaitForAnimationToStart(this, (this.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
		base.animator.Play("Dust_Start_End");
		AudioManager.Play("slime_tombstone_slide");
		this.emitAudioFromObject.Add("slime_tombstone_slide");
		AbstractPlayerController player = PlayerManager.GetNext();
		float startX2 = base.transform.position.x;
		float endX2 = (this.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
		float moveTime2 = Mathf.Abs(startX2 - endX2) / base.properties.CurrentState.tombstone.moveSpeed;
		float targetX = 0f;
		float t = 0f;
		bool centeredOnPlayer = false;
		while (!centeredOnPlayer && t < moveTime2)
		{
			yield return wait;
			t += CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startX2, endX2, t / moveTime2)), null, null);
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			targetX = player.center.x + offset;
			if ((this.direction == SlimeLevelTombstone.Direction.Right && base.transform.position.x > targetX) || (this.direction == SlimeLevelTombstone.Direction.Left && base.transform.position.x < targetX))
			{
				centeredOnPlayer = true;
			}
		}
		base.transform.SetPosition(new float?(Mathf.Clamp(targetX, -500f, 500f)), null, null);
		base.animator.Play("Dust_End");
		base.animator.Play("Dirt_Off");
		this.StartSmash();
		yield break;
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000CD2F8 File Offset: 0x000CB4F8
	public void DustDirection()
	{
		this.dirt.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? -1 : 1)), null, null);
		this.dust.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? 1 : -1)), null, null);
		this.dust2.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? -1 : 1)), null, null);
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x00021945 File Offset: 0x0001FB45
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000CD3A8 File Offset: 0x000CB5A8
	public IEnumerator waitForSmash_cr()
	{
		float timeUntilAttack = base.properties.CurrentState.tombstone.attackDelay.RandomFloat();
		yield return CupheadTime.WaitForSeconds(this, timeUntilAttack);
		this.wantsToSmash = true;
		yield break;
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x00021966 File Offset: 0x0001FB66
	public void StartSmash()
	{
		this.state = SlimeLevelTombstone.State.Smash;
		base.StartCoroutine(this.smash_cr());
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x000CD3C4 File Offset: 0x000CB5C4
	public IEnumerator smash_cr()
	{
		base.animator.SetTrigger("StartSmash");
		yield return base.animator.WaitForAnimationToStart(this, "Smash_Pre_Hold", false);
		AudioManager.Play("slime_tombstone_splat");
		this.emitAudioFromObject.Add("slime_tombstone_splat");
		AudioManager.Play("slime_tombstone_splat_start");
		this.emitAudioFromObject.Add("slime_tombstone_splat_start");
		AudioManager.Stop("slime_tombstone_slide");
		this.emitAudioFromObject.Add("slime_tombstone_slide");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tombstone.anticipationHold);
		base.animator.SetTrigger("Continue");
		this.StartMove();
		yield break;
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x0002197C File Offset: 0x0001FB7C
	public void DisableDamageReceiver()
	{
		this.damageReceiver.enabled = false;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x0002198A File Offset: 0x0001FB8A
	public void EnableDamageReceiver()
	{
		this.damageReceiver.enabled = true;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x00021998 File Offset: 0x0001FB98
	public void EnableDamageDealer()
	{
		this.dealDamage = true;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000219A1 File Offset: 0x0001FBA1
	public void DisableDamageDealer()
	{
		this.dealDamage = false;
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000CD3E0 File Offset: 0x000CB5E0
	public void OnSmash()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		this.smashDustFrontPrefab.Create(base.transform.position);
		this.smashDustBackPrefab.Create(base.transform.position);
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x000219AA File Offset: 0x0001FBAA
	public void OnBossDeath()
	{
		if (this.onDeath != null)
		{
			this.onDeath();
		}
		this.StopAllCoroutines();
		base.animator.SetTrigger("Death");
		AudioManager.Play("slime_tombstone_death");
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x000219E2 File Offset: 0x0001FBE2
	public void TombstoneTauntsAudio()
	{
		AudioManager.Play("slime_tombstone_taunts");
	}

	// Token: 0x04002119 RID: 8473
	public const float startY = 550f;

	// Token: 0x0400211A RID: 8474
	public const float onGroundY = -80f;

	// Token: 0x0400211B RID: 8475
	public const float maxX = 500f;

	// Token: 0x0400211C RID: 8476
	public const float fallTime = 0.2f;

	// Token: 0x0400211D RID: 8477
	public const float crushSlimeY = 70f;

	// Token: 0x0400211E RID: 8478
	public int offsetIndex;

	// Token: 0x0400211F RID: 8479
	public bool dealDamage;

	// Token: 0x04002120 RID: 8480
	public DamageDealer damageDealer;

	// Token: 0x04002121 RID: 8481
	public DamageReceiver damageReceiver;

	// Token: 0x04002122 RID: 8482
	[SerializeField]
	public Transform dirt;

	// Token: 0x04002123 RID: 8483
	[SerializeField]
	public Transform dust;

	// Token: 0x04002124 RID: 8484
	[SerializeField]
	public Transform dust2;

	// Token: 0x04002125 RID: 8485
	[SerializeField]
	public Effect dustPrefab;

	// Token: 0x04002126 RID: 8486
	[SerializeField]
	public SlimeLevelSlime bigSlime;

	// Token: 0x04002127 RID: 8487
	[SerializeField]
	public SlimeLevelTinySlime tinySlime;

	// Token: 0x04002128 RID: 8488
	[SerializeField]
	public Effect smashDustBackPrefab;

	// Token: 0x04002129 RID: 8489
	[SerializeField]
	public Effect smashDustFrontPrefab;

	// Token: 0x0400212A RID: 8490
	public SlimeLevelTombstone.Direction direction;

	// Token: 0x0400212B RID: 8491
	public Action onDeath;

	// Token: 0x0400212C RID: 8492
	public bool wantsToSmash;

	// Token: 0x02000F5C RID: 3932
	public enum State
	{
		// Token: 0x04006ECB RID: 28363
		Init,
		// Token: 0x04006ECC RID: 28364
		Intro,
		// Token: 0x04006ECD RID: 28365
		Move,
		// Token: 0x04006ECE RID: 28366
		Smash
	}

	// Token: 0x02000F5D RID: 3933
	public enum Direction
	{
		// Token: 0x04006ED0 RID: 28368
		Left,
		// Token: 0x04006ED1 RID: 28369
		Right
	}
}
