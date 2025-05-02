using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D4 RID: 980
[RequireComponent(typeof(DamageReceiver))]
[RequireComponent(typeof(HitFlash))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractPlatformingLevelEnemy : AbstractLevelEntity
{
	// Token: 0x06002B31 RID: 11057 RVA: 0x0002444A File Offset: 0x0002264A
	public AbstractPlatformingLevelEnemy()
	{
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06002B32 RID: 11058 RVA: 0x00024479 File Offset: 0x00022679
	public EnemyID ID
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06002B33 RID: 11059 RVA: 0x00024481 File Offset: 0x00022681
	public float StartDelay
	{
		get
		{
			return this._startDelay;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06002B34 RID: 11060 RVA: 0x00024489 File Offset: 0x00022689
	public EnemyProperties Properties
	{
		get
		{
			if (this._properties == null)
			{
				this._properties = EnemyDatabase.GetProperties(this._id);
			}
			return this._properties;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06002B35 RID: 11061 RVA: 0x000244AD File Offset: 0x000226AD
	// (set) Token: 0x06002B36 RID: 11062 RVA: 0x000244B5 File Offset: 0x000226B5
	public float Health { get; set; }

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06002B37 RID: 11063 RVA: 0x000244BE File Offset: 0x000226BE
	// (set) Token: 0x06002B38 RID: 11064 RVA: 0x000244C6 File Offset: 0x000226C6
	public bool Dead { get; set; }

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06002B39 RID: 11065 RVA: 0x000244CF File Offset: 0x000226CF
	// (set) Token: 0x06002B3A RID: 11066 RVA: 0x000244D7 File Offset: 0x000226D7
	public DamageReceiver _damageReceiver { get; set; }

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000244E0 File Offset: 0x000226E0
	// (set) Token: 0x06002B3C RID: 11068 RVA: 0x000244E8 File Offset: 0x000226E8
	public DamageDealer _damageDealer { get; set; }

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06002B3D RID: 11069 RVA: 0x000244F1 File Offset: 0x000226F1
	// (set) Token: 0x06002B3E RID: 11070 RVA: 0x000244F9 File Offset: 0x000226F9
	public bool _started { get; set; }

	// Token: 0x06002B3F RID: 11071 RVA: 0x000D587C File Offset: 0x000D3A7C
	public override void Awake()
	{
		base.Awake();
		if (this.Properties == null)
		{
			this.Health = 10f;
			this._canParry = false;
		}
		else
		{
			this.Health = this.Properties.Health;
			this._canParry = this.Properties.CanParry;
		}
		this._damageReceiver = base.GetComponent<DamageReceiver>();
		this._damageDealer = DamageDealer.NewEnemy();
		this._damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x00024502 File Offset: 0x00022702
	public virtual void Start()
	{
		this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Instant);
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x00024521 File Offset: 0x00022721
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnLevelStartEvent -= this.OnLevelStart;
		}
		this.explosionPrefabs = null;
		this.parryEffectPrefab = null;
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x000D5904 File Offset: 0x000D3B04
	public virtual void Update()
	{
		if (this._startCondition == AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume && !this._started)
		{
			Rect rect = RectUtils.NewFromCenter(this._triggerPosition.x, this._triggerPosition.y, this._triggerSize.x, this._triggerSize.y);
			if (rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
			{
				this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume);
			}
		}
		if (this._damageDealer != null)
		{
			this._damageDealer.Update();
		}
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x0002455D File Offset: 0x0002275D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this._damageDealer != null && phase != CollisionPhase.Exit)
		{
			this._damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x00024586 File Offset: 0x00022786
	public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health <= 0f)
		{
			Level.ScoringData.pacifistRun = false;
			this.Die();
		}
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000245BC File Offset: 0x000227BC
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.Die();
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000245CA File Offset: 0x000227CA
	public virtual void Die()
	{
		this.IdleSounds = false;
		if (this.Dead)
		{
			return;
		}
		this.Dead = true;
		this.Explode();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000D59B8 File Offset: 0x000D3BB8
	public void Explode()
	{
		if (this.explosionPrefabs.Length > 0 && CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			this.explosionPrefabs.RandomChoice<PlatformingLevelGenericExplosion>().Create(base.GetComponent<Collider2D>().bounds.center);
		}
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000D5A1C File Offset: 0x000D3C1C
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		if (this.parryEffectPrefab != null)
		{
			this.parryEffectPrefab.Create(base.GetComponent<Collider2D>().bounds.center);
		}
		player.stats.OnParry(1f, true);
		this.Die();
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000D5A78 File Offset: 0x000D3C78
	public virtual IEnumerator idle_audio_delayer_cr(string key, float delayMin, float delayMax)
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
			{
				float delay = Random.Range(delayMin, delayMax);
				yield return CupheadTime.WaitForSeconds(this, delay);
				yield return null;
				if (this.IdleSounds)
				{
					AudioManager.Play(key);
					while (AudioManager.CheckIfPlaying(key))
					{
						yield return null;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000245F7 File Offset: 0x000227F7
	public void StartFromCustom()
	{
		if (!this._started)
		{
			this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Custom);
		}
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x0002460B File Offset: 0x0002280B
	public void ResetStartingCondition()
	{
		this._started = false;
	}

	// Token: 0x06002B4C RID: 11084
	public abstract void OnStart();

	// Token: 0x06002B4D RID: 11085 RVA: 0x00024614 File Offset: 0x00022814
	public void OnLevelStart()
	{
		this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.LevelStart);
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x0002461D File Offset: 0x0002281D
	public void StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition condition)
	{
		if (this.Dead || condition != this._startCondition || this._started)
		{
			return;
		}
		this._started = true;
		base.StartCoroutine(this.startWithCondition_cr());
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000D5AA8 File Offset: 0x000D3CA8
	public IEnumerator startWithCondition_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this._startDelay);
		this.OnStart();
		yield break;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00024656 File Offset: 0x00022856
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x00024669 File Offset: 0x00022869
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x000D5AC4 File Offset: 0x000D3CC4
	public void DrawGizmos(float a)
	{
		if (this._startCondition == AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume)
		{
			Gizmos.color = new Color(0f, 1f, 0f, a);
			Gizmos.DrawWireCube(this._triggerPosition, this._triggerSize);
		}
	}

	// Token: 0x040023DD RID: 9181
	public static readonly Vector2 CAMERA_DEATH_PADDING = new Vector2(100f, 100f);

	// Token: 0x040023DE RID: 9182
	[SerializeField]
	public EnemyID _id;

	// Token: 0x040023DF RID: 9183
	[SerializeField]
	public AbstractPlatformingLevelEnemy.StartCondition _startCondition;

	// Token: 0x040023E0 RID: 9184
	[SerializeField]
	public float _startDelay;

	// Token: 0x040023E1 RID: 9185
	[SerializeField]
	public Vector2 _triggerPosition = Vector2.zero;

	// Token: 0x040023E2 RID: 9186
	[SerializeField]
	public Vector2 _triggerSize = Vector2.one * 100f;

	// Token: 0x040023E3 RID: 9187
	[SerializeField]
	public PlatformingLevelGenericExplosion[] explosionPrefabs;

	// Token: 0x040023E4 RID: 9188
	[SerializeField]
	public Effect parryEffectPrefab;

	// Token: 0x040023E5 RID: 9189
	public EnemyProperties _properties;

	// Token: 0x040023EB RID: 9195
	public bool IdleSounds = true;

	// Token: 0x02000FF9 RID: 4089
	public enum StartCondition
	{
		// Token: 0x04007265 RID: 29285
		LevelStart,
		// Token: 0x04007266 RID: 29286
		TriggerVolume,
		// Token: 0x04007267 RID: 29287
		Instant,
		// Token: 0x04007268 RID: 29288
		Custom
	}
}
