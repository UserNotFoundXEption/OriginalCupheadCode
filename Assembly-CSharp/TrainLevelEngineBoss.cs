using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class TrainLevelEngineBoss : LevelProperties.Train.Entity
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x0600298B RID: 10635 RVA: 0x000D245C File Offset: 0x000D065C
	// (remove) Token: 0x0600298C RID: 10636 RVA: 0x000D2494 File Offset: 0x000D0694
	public event TrainLevelEngineBoss.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x0600298D RID: 10637 RVA: 0x000D24CC File Offset: 0x000D06CC
	// (remove) Token: 0x0600298E RID: 10638 RVA: 0x000D2504 File Offset: 0x000D0704
	public event Action OnDeathEvent;

	// Token: 0x0600298F RID: 10639 RVA: 0x000D253C File Offset: 0x000D073C
	public override void Awake()
	{
		base.Awake();
		this.tailSwitch = TrainLevelEngineBossTail.Create(this.tailRoot);
		this.tailSwitch.OnActivate += this.OnTailParried;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.smokeRenderer = this.dropperRoot.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x00022EBB File Offset: 0x000210BB
	public void Start()
	{
		this.UpdateHeartDamageReceiver();
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x00022EC3 File Offset: 0x000210C3
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.engine.health;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000D25AC File Offset: 0x000D07AC
	public void StartBoss()
	{
		AudioManager.Play("train_engine_boss_run_start");
		this.TrainRunStep = true;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tailTimer_cr());
		base.StartCoroutine(this.fireProjectiles_cr());
		this.StartAttack();
		this.UpdateHeartDamageReceiver();
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x00022EE2 File Offset: 0x000210E2
	public void UpdateHeartDamageReceiver()
	{
		this.heartDamageReceiver.enabled = (this.doorState == TrainLevelEngineBoss.DoorState.Open || this.doorState == TrainLevelEngineBoss.DoorState.Closing);
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x000D2600 File Offset: 0x000D0800
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		base.animator.SetBool("Hit", true);
		base.CancelInvoke("StopHitAnim");
		base.Invoke("StopHitAnim", 0.25f);
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x00022F06 File Offset: 0x00021106
	public void Die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.damageReceiver.enabled = false;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x000D268C File Offset: 0x000D088C
	public IEnumerator die_cr()
	{
		AudioManager.Play("train_engine_boss_die");
		this.emitAudioFromObject.Add("train_engine_boss_die");
		base.animator.SetTrigger("OnDeath");
		this.door.SetActive(false);
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		yield return base.TweenPositionX(base.transform.position.x, -300f, 2.5f * Mathf.Abs(-300f - base.transform.position.x) / 400f, EaseUtils.EaseType.easeInOutSine);
		for (;;)
		{
			yield return base.TweenPositionX(base.transform.position.x, 100f, 2.5f, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenPositionX(base.transform.position.x, -300f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x00022F3A File Offset: 0x0002113A
	public void StopHitAnim()
	{
		base.animator.SetBool("Hit", false);
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x00022F4D File Offset: 0x0002114D
	public void SpawnDustOnFeet()
	{
		this.footDustPrefab.Create(this.footDustRoot.position, this.footDustRoot.localScale).Play();
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x00022F75 File Offset: 0x00021175
	public void StartAttack()
	{
		this.StopAttack();
		this.attackCoroutine = this.attack_cr();
		base.StartCoroutine(this.attackCoroutine);
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00022F96 File Offset: 0x00021196
	public void StopAttack()
	{
		if (this.attackCoroutine != null)
		{
			base.StopCoroutine(this.attackCoroutine);
		}
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x000D26A8 File Offset: 0x000D08A8
	public void OnAttackAnimComplete()
	{
		this.dropperPrefab.Create(this.dropperRoot.position, base.properties.CurrentState.engine.projectileUpSpeed, base.properties.CurrentState.engine.projectileXSpeed, base.properties.CurrentState.engine.projectileGravity);
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x00022FAF File Offset: 0x000211AF
	public void SmokeFX()
	{
		this.smokeRenderer.flipX = Rand.Bool();
		base.animator.SetTrigger("Smoke");
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x000D2710 File Offset: 0x000D0910
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.projectileDelay);
			base.animator.SetTrigger("OnAttack");
			AudioManager.Play("train_engine_boss_attack");
			this.emitAudioFromObject.Add("train_engine_boss_attack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		}
		yield break;
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x000D272C File Offset: 0x000D092C
	public IEnumerator fireProjectiles_cr()
	{
		for (;;)
		{
			if (this.doorState == TrainLevelEngineBoss.DoorState.Open)
			{
				base.animator.SetTrigger("FireAttack");
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.fireDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x000D2748 File Offset: 0x000D0948
	public void SpawnProjectile()
	{
		Vector2 zero = Vector2.zero;
		zero.y = base.properties.CurrentState.engine.fireVelocityY;
		zero.x = base.properties.CurrentState.engine.fireVelocityX;
		this.firePrefab.Create(this.fireRoot.position, zero, (float)base.properties.CurrentState.engine.fireGravity);
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060029A0 RID: 10656 RVA: 0x00022FD1 File Offset: 0x000211D1
	// (set) Token: 0x060029A1 RID: 10657 RVA: 0x00022FD9 File Offset: 0x000211D9
	public TrainLevelEngineBoss.DoorState doorState
	{
		get
		{
			return this._ds;
		}
		set
		{
			if (value == this._ds)
			{
				return;
			}
			this._ds = value;
			this.UpdateHeartDamageReceiver();
		}
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x00022FF5 File Offset: 0x000211F5
	public void DoorAnimOpenStarted()
	{
		if (this.desiredDoorState == TrainLevelEngineBoss.DoorState.Open && this.doorState == TrainLevelEngineBoss.DoorState.Closed)
		{
			this.doorState = TrainLevelEngineBoss.DoorState.Opening;
			base.animator.SetTrigger("Open");
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x0002302B File Offset: 0x0002122B
	public void DoorAnimCloseStarted()
	{
		if (this.desiredDoorState == TrainLevelEngineBoss.DoorState.Closed && this.doorState == TrainLevelEngineBoss.DoorState.Open)
		{
			this.doorState = TrainLevelEngineBoss.DoorState.Closing;
			base.animator.SetTrigger("Close");
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x00023061 File Offset: 0x00021261
	public void DoorOpenAnimComplete()
	{
		if (this.doorState == TrainLevelEngineBoss.DoorState.Opening)
		{
			AudioManager.Play("train_engine_boss_door");
			this.emitAudioFromObject.Add("train_engine_boss_door");
			this.doorState = TrainLevelEngineBoss.DoorState.Open;
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x00023096 File Offset: 0x00021296
	public void DoorCloseAnimComplete()
	{
		if (this.doorState == TrainLevelEngineBoss.DoorState.Closing)
		{
			AudioManager.Play("train_engine_boss_door_shut");
			this.emitAudioFromObject.Add("train_engine_boss_door_shut");
			this.doorState = TrainLevelEngineBoss.DoorState.Closed;
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000230CB File Offset: 0x000212CB
	public void IronStepSFX()
	{
		if (this.TrainRunStep)
		{
			AudioManager.Play("train_engine_step");
			this.emitAudioFromObject.Add("train_engine_step");
		}
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000230F2 File Offset: 0x000212F2
	public void UpdateDoorSprite()
	{
		this.doorSprites.DisableAll();
		this.doorSprites[this.doorState].enabled = true;
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000D27D0 File Offset: 0x000D09D0
	public IEnumerator doorTimer_cr()
	{
		this.desiredDoorState = TrainLevelEngineBoss.DoorState.Open;
		float time = base.properties.CurrentState.engine.doorTime.GetFloatAt(this.health / base.properties.CurrentState.engine.health);
		while (this.doorState != TrainLevelEngineBoss.DoorState.Open)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, time);
		this.desiredDoorState = TrainLevelEngineBoss.DoorState.Closed;
		while (this.doorState != TrainLevelEngineBoss.DoorState.Closed)
		{
			yield return null;
		}
		base.StartCoroutine(this.tailTimer_cr());
		yield break;
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060029A9 RID: 10665 RVA: 0x00023116 File Offset: 0x00021316
	// (set) Token: 0x060029AA RID: 10666 RVA: 0x0002311E File Offset: 0x0002131E
	public TrainLevelEngineBoss.TailState tailState
	{
		get
		{
			return this._tailState;
		}
		set
		{
			this.ChangeTail(value);
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000D27EC File Offset: 0x000D09EC
	public void ChangeTail(TrainLevelEngineBoss.TailState state)
	{
		if (state == this.tailState)
		{
			return;
		}
		this.tailSwitch.tailEnabled = (state == TrainLevelEngineBoss.TailState.On);
		this._tailState = state;
		this.tailSprites.DisableAll();
		this.tailSprites[state].enabled = true;
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x00023127 File Offset: 0x00021327
	public void OnTailParried()
	{
		this.tailState = TrainLevelEngineBoss.TailState.Off;
		base.StartCoroutine(this.doorTimer_cr());
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000D283C File Offset: 0x000D0A3C
	public IEnumerator tailTimer_cr()
	{
		this.tailState = TrainLevelEngineBoss.TailState.Off;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.tailDelay);
		this.tailState = TrainLevelEngineBoss.TailState.On;
		yield break;
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000D2858 File Offset: 0x000D0A58
	public IEnumerator move_cr()
	{
		float max_x = base.properties.CurrentState.engine.maxDist;
		float min_x = base.properties.CurrentState.engine.minDist;
		float forwardTime = base.properties.CurrentState.engine.forwardTime;
		float backTime = base.properties.CurrentState.engine.backTime;
		yield return base.TweenLocalPositionX(base.transform.position.x, min_x, 3f, EaseUtils.EaseType.easeOutSine);
		AudioManager.FadeSFXVolume("train_engine_boss_run_start", 0f, 3f);
		AudioManager.PlayLoop("train_engine_boss_run_loop");
		this.emitAudioFromObject.Add("train_engine_boss_run_loop");
		AudioManager.PlayLoop("train_engine_boss_fire_idle");
		this.emitAudioFromObject.Add("train_engine_boss_fire_idle");
		for (;;)
		{
			yield return base.TweenLocalPositionX(base.transform.position.x, max_x, forwardTime, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(base.transform.position.x, min_x, backTime, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x0002313D File Offset: 0x0002133D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.footDustPrefab = null;
		this.dropperPrefab = null;
		this.firePrefab = null;
	}

	// Token: 0x040022C9 RID: 8905
	public const string HitParameterName = "Hit";

	// Token: 0x040022CA RID: 8906
	public const string StopHitAnimName = "StopHitAnim";

	// Token: 0x040022CB RID: 8907
	public const float StopHitAnimTime = 0.25f;

	// Token: 0x040022CC RID: 8908
	[SerializeField]
	public DamageReceiverChild heartDamageReceiver;

	// Token: 0x040022CD RID: 8909
	[SerializeField]
	public Transform footDustRoot;

	// Token: 0x040022CE RID: 8910
	[SerializeField]
	public Effect footDustPrefab;

	// Token: 0x040022CF RID: 8911
	public DamageReceiver damageReceiver;

	// Token: 0x040022D0 RID: 8912
	public float health;

	// Token: 0x040022D1 RID: 8913
	public bool dead;

	// Token: 0x040022D2 RID: 8914
	public bool TrainRunStep;

	// Token: 0x040022D5 RID: 8917
	[Header("Dropper")]
	[SerializeField]
	public Transform dropperRoot;

	// Token: 0x040022D6 RID: 8918
	[SerializeField]
	public TrainLevelEngineBossDropperProjectile dropperPrefab;

	// Token: 0x040022D7 RID: 8919
	public IEnumerator attackCoroutine;

	// Token: 0x040022D8 RID: 8920
	public SpriteRenderer smokeRenderer;

	// Token: 0x040022D9 RID: 8921
	public const string FireAttackParameterName = "FireAttack";

	// Token: 0x040022DA RID: 8922
	[Header("Fire")]
	[SerializeField]
	public Transform fireRoot;

	// Token: 0x040022DB RID: 8923
	[SerializeField]
	public TrainLevelEngineBossFireProjectile firePrefab;

	// Token: 0x040022DC RID: 8924
	public const string OpenDoorParameterName = "Open";

	// Token: 0x040022DD RID: 8925
	public const string CloseDoorParameterName = "Close";

	// Token: 0x040022DE RID: 8926
	[Header("Door")]
	[SerializeField]
	public TrainLevelEngineBoss.DoorSprites doorSprites;

	// Token: 0x040022DF RID: 8927
	[SerializeField]
	public GameObject door;

	// Token: 0x040022E0 RID: 8928
	public TrainLevelEngineBoss.DoorState _ds = TrainLevelEngineBoss.DoorState.Closed;

	// Token: 0x040022E1 RID: 8929
	public TrainLevelEngineBoss.DoorState desiredDoorState = TrainLevelEngineBoss.DoorState.Closed;

	// Token: 0x040022E2 RID: 8930
	[Header("Tail")]
	[SerializeField]
	public TrainLevelEngineBoss.TailSprites tailSprites;

	// Token: 0x040022E3 RID: 8931
	[SerializeField]
	public Transform tailRoot;

	// Token: 0x040022E4 RID: 8932
	public TrainLevelEngineBoss.TailState _tailState = TrainLevelEngineBoss.TailState.Off;

	// Token: 0x040022E5 RID: 8933
	public TrainLevelEngineBossTail tailSwitch;

	// Token: 0x02000FA1 RID: 4001
	// (Invoke) Token: 0x06007559 RID: 30041
	public delegate void OnDamageTakenHandler(float damage);

	// Token: 0x02000FA2 RID: 4002
	public enum DoorState
	{
		// Token: 0x040070D5 RID: 28885
		Open,
		// Token: 0x040070D6 RID: 28886
		Closed,
		// Token: 0x040070D7 RID: 28887
		Opening,
		// Token: 0x040070D8 RID: 28888
		Closing
	}

	// Token: 0x02000FA3 RID: 4003
	[Serializable]
	public class DoorSprites
	{
		// Token: 0x17001550 RID: 5456
		public SpriteRenderer this[TrainLevelEngineBoss.DoorState state]
		{
			get
			{
				switch (state)
				{
				default:
					return this.open;
				case TrainLevelEngineBoss.DoorState.Closed:
					return this.closed;
				case TrainLevelEngineBoss.DoorState.Opening:
					return this.opening;
				case TrainLevelEngineBoss.DoorState.Closing:
					return this.closing;
				}
			}
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x000502D6 File Offset: 0x0004E4D6
		public void DisableAll()
		{
			this.open.enabled = false;
			this.closed.enabled = false;
			this.opening.enabled = false;
			this.closing.enabled = false;
		}

		// Token: 0x040070D9 RID: 28889
		public SpriteRenderer open;

		// Token: 0x040070DA RID: 28890
		public SpriteRenderer closed;

		// Token: 0x040070DB RID: 28891
		public SpriteRenderer opening;

		// Token: 0x040070DC RID: 28892
		public SpriteRenderer closing;
	}

	// Token: 0x02000FA4 RID: 4004
	public enum TailState
	{
		// Token: 0x040070DE RID: 28894
		On,
		// Token: 0x040070DF RID: 28895
		Off
	}

	// Token: 0x02000FA5 RID: 4005
	[Serializable]
	public class TailSprites
	{
		// Token: 0x17001551 RID: 5457
		public SpriteRenderer this[TrainLevelEngineBoss.TailState state]
		{
			get
			{
				if (state == TrainLevelEngineBoss.TailState.On || state != TrainLevelEngineBoss.TailState.Off)
				{
					return this.on;
				}
				return this.off;
			}
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x00050331 File Offset: 0x0004E531
		public void DisableAll()
		{
			this.on.enabled = false;
			this.off.enabled = false;
		}

		// Token: 0x040070E0 RID: 28896
		public SpriteRenderer on;

		// Token: 0x040070E1 RID: 28897
		public SpriteRenderer off;
	}
}
