using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000589 RID: 1417
public abstract class AbstractProjectile : AbstractCollidableObject
{
	// Token: 0x06003BAD RID: 15277 RVA: 0x00113F4C File Offset: 0x0011214C
	public AbstractProjectile()
	{
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06003BAE RID: 15278 RVA: 0x0003058B File Offset: 0x0002E78B
	public bool CanParry
	{
		get
		{
			return this._canParry;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06003BAF RID: 15279 RVA: 0x00030593 File Offset: 0x0002E793
	public bool CountParryTowardsScore
	{
		get
		{
			return this._countParryTowardsScore;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0003059B File Offset: 0x0002E79B
	// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x000305A3 File Offset: 0x0002E7A3
	public float distance { get; set; }

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x000305AC File Offset: 0x0002E7AC
	// (set) Token: 0x06003BB3 RID: 15283 RVA: 0x000305B4 File Offset: 0x0002E7B4
	public float lifetime { get; set; }

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x000305BD File Offset: 0x0002E7BD
	// (set) Token: 0x06003BB5 RID: 15285 RVA: 0x000305C5 File Offset: 0x0002E7C5
	public bool dead { get; set; }

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06003BB6 RID: 15286 RVA: 0x000305CE File Offset: 0x0002E7CE
	// (set) Token: 0x06003BB7 RID: 15287 RVA: 0x000305D6 File Offset: 0x0002E7D6
	public float StoneTime { get; set; }

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06003BB8 RID: 15288 RVA: 0x000305DF File Offset: 0x0002E7DF
	public virtual float ParryMeterMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x000305E6 File Offset: 0x0002E7E6
	// (set) Token: 0x06003BBA RID: 15290 RVA: 0x000305EE File Offset: 0x0002E7EE
	public DamageDealer.DamageSource DamageSource
	{
		get
		{
			return this.damageSource;
		}
		set
		{
			this.damageSource = value;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06003BBB RID: 15291 RVA: 0x00113FA0 File Offset: 0x001121A0
	public float DamageMultiplier
	{
		get
		{
			float num = PlayerManager.DamageMultiplier;
			if (base.tag == "PlayerProjectile")
			{
				if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_health_up_1)
				{
					num *= 1f - WeaponProperties.CharmHealthUpOne.weaponDebuff;
				}
				else if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_health_up_2)
				{
					num *= 1f - WeaponProperties.CharmHealthUpTwo.weaponDebuff;
				}
				else if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_EX && Level.Current.playerMode == PlayerMode.Plane && this is PlaneWeaponPeashotExProjectile)
				{
					num *= 1f - WeaponProperties.CharmEXCharm.planePeashotEXDebuff;
				}
			}
			return num;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06003BBC RID: 15292 RVA: 0x000305F7 File Offset: 0x0002E7F7
	public virtual float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06003BBD RID: 15293 RVA: 0x000305FE File Offset: 0x0002E7FE
	public virtual bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06003BBE RID: 15294 RVA: 0x00030601 File Offset: 0x0002E801
	public virtual float SafeTime
	{
		get
		{
			return 0.005f;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06003BBF RID: 15295 RVA: 0x00030608 File Offset: 0x0002E808
	public virtual float PlayerSafeTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06003BC0 RID: 15296 RVA: 0x0003060F File Offset: 0x0002E80F
	public virtual float EnemySafeTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x06003BC1 RID: 15297 RVA: 0x00114084 File Offset: 0x00112284
	// (remove) Token: 0x06003BC2 RID: 15298 RVA: 0x001140BC File Offset: 0x001122BC
	public event DamageDealer.OnDealDamageHandler OnDealDamageEvent;

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06003BC3 RID: 15299 RVA: 0x001140F4 File Offset: 0x001122F4
	// (remove) Token: 0x06003BC4 RID: 15300 RVA: 0x0011412C File Offset: 0x0011232C
	public event Action<AbstractProjectile> OnDie;

	// Token: 0x06003BC5 RID: 15301 RVA: 0x00114164 File Offset: 0x00112364
	public override void Awake()
	{
		base.Awake();
		this.distance = 0f;
		this.lifetime = 0f;
		this.StoneTime = -1f;
		if (base.CompareTag("PlayerProjectile") || !base.CompareTag("EnemyProjectile"))
		{
		}
		if (base.gameObject.layer != 12)
		{
			base.gameObject.layer = 12;
		}
		this.RandomizeVariant();
		if (Level.Current != null && Level.Current.CurrentScene == Scenes.scene_level_airplane)
		{
			this._setYPadding = -600f;
		}
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0011420C File Offset: 0x0011240C
	public virtual void Start()
	{
		this.damageDealer = new DamageDealer(this);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.SetStoneTime(this.StoneTime);
		this.damageDealer.PlayerId = this.PlayerId;
		if (this.tracker != null)
		{
			this.tracker.Add(this.damageDealer);
		}
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x0011427C File Offset: 0x0011247C
	public virtual void Update()
	{
		Vector3 position = base.transform.position;
		if (this.lifetime == 0f)
		{
			this.lastPosition = (this.startPosition = position);
		}
		if (this.DestroyDistance > 0f && Vector3.Distance(this.startPosition, position) >= this.DestroyDistance)
		{
			this.OnDieDistance();
		}
		this.distance += Vector3.Distance(this.lastPosition, position);
		this.lastPosition = position;
		if (this.DestroyLifetime > 0f && this.lifetime >= this.DestroyLifetime)
		{
			this.OnDieLifetime();
		}
		this.lifetime += Time.deltaTime;
		if (this.DestroyedAfterLeavingScreen)
		{
			bool flag = CupheadLevelCamera.Current.ContainsPoint(position, new Vector2(150f, this._setYPadding));
			if (this.hasBeenRendered && !flag)
			{
				Object.Destroy(base.gameObject);
			}
			if (!this.hasBeenRendered)
			{
				this.hasBeenRendered = flag;
			}
		}
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x00030616 File Offset: 0x0002E816
	public void ResetLifetime()
	{
		this.lifetime = 0f;
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x00030623 File Offset: 0x0002E823
	public void ResetDistance()
	{
		this.distance = 0f;
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x00030630 File Offset: 0x0002E830
	public override void checkCollision(Collider2D col, CollisionPhase phase)
	{
		if (this.lifetime < this.SafeTime)
		{
			return;
		}
		base.checkCollision(col, phase);
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06003BCB RID: 15307 RVA: 0x0003064C File Offset: 0x0002E84C
	public override bool allowCollisionPlayer
	{
		get
		{
			return this.lifetime > this.PlayerSafeTime;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06003BCC RID: 15308 RVA: 0x0003065C File Offset: 0x0002E85C
	public override bool allowCollisionEnemy
	{
		get
		{
			return this.lifetime > this.EnemySafeTime;
		}
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x00114398 File Offset: 0x00112598
	public virtual AbstractProjectile Create()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		return gameObject.GetComponent<AbstractProjectile>();
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x001143BC File Offset: 0x001125BC
	public virtual AbstractProjectile Create(Vector2 position)
	{
		AbstractProjectile abstractProjectile = this.Create();
		abstractProjectile.transform.position = position;
		return abstractProjectile;
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x001143E4 File Offset: 0x001125E4
	public virtual AbstractProjectile Create(Vector2 position, float rotation)
	{
		AbstractProjectile abstractProjectile = this.Create(position);
		abstractProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		return abstractProjectile;
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x00114420 File Offset: 0x00112620
	public virtual AbstractProjectile Create(Vector2 position, float rotation, Vector2 scale)
	{
		AbstractProjectile abstractProjectile = this.Create(position, rotation);
		abstractProjectile.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
		return abstractProjectile;
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x0003066C File Offset: 0x0002E86C
	public virtual void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		if (this.OnDealDamageEvent != null)
		{
			this.OnDealDamageEvent(damage, receiver, damageDealer);
		}
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x00030687 File Offset: 0x0002E887
	public bool GetDamagesType(DamageReceiver.Type type)
	{
		return this.DamagesType.GetType(type);
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x00030695 File Offset: 0x0002E895
	public virtual void SetParryable(bool parryable)
	{
		this._canParry = parryable;
		this.SetBool(AbstractProjectile.Parry, parryable);
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x000306AA File Offset: 0x0002E8AA
	public void SetStoneTime(float stoneTime)
	{
		this.StoneTime = stoneTime;
		if (this.damageDealer != null)
		{
			this.damageDealer.SetStoneTime(stoneTime);
		}
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x000306CA File Offset: 0x0002E8CA
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Walls)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x000306E4 File Offset: 0x0002E8E4
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Ceiling)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x000306FE File Offset: 0x0002E8FE
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Ground)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x00030718 File Offset: 0x0002E918
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.missed = false;
		if (this.CollisionDeath.Enemies)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x00030739 File Offset: 0x0002E939
	public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.EnemyProjectiles)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x00030753 File Offset: 0x0002E953
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Player)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x0003076D File Offset: 0x0002E96D
	public override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.PlayerProjectiles)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x00030787 File Offset: 0x0002E987
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Other)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x000307A1 File Offset: 0x0002E9A1
	public virtual void OnCollisionWideShotEX(GameObject hit, CollisionPhase phase)
	{
		this.OnCollisionDie(hit, phase);
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x000307AB File Offset: 0x0002E9AB
	public virtual void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		if (!this.dead)
		{
			this.Die();
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x000307BE File Offset: 0x0002E9BE
	public virtual void OnDieAnimationComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x000307CB File Offset: 0x0002E9CB
	public virtual void OnParry(AbstractPlayerController player)
	{
		if (this.CanParry)
		{
			this.OnParryDie();
		}
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x000307DE File Offset: 0x0002E9DE
	public virtual void OnParryDie()
	{
		Object.Destroy(base.gameObject);
		this.Die();
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x000307F1 File Offset: 0x0002E9F1
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.Die();
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00114464 File Offset: 0x00112664
	public virtual void Die()
	{
		this.dead = true;
		if (base.GetComponent<Collider2D>() != null)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		this.RandomizeVariant();
		this.SetTrigger(AbstractProjectile.OnDeathTrigger);
		if (this.OnDie != null)
		{
			this.OnDie(this);
		}
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x000307FF File Offset: 0x0002E9FF
	public virtual void OnDieDistance()
	{
		if (this.DestroyDistanceAnimated)
		{
			this.Die();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x00030822 File Offset: 0x0002EA22
	public virtual void OnDieLifetime()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x0003082F File Offset: 0x0002EA2F
	public virtual void SetTrigger(string trigger)
	{
		base.animator.SetTrigger(trigger);
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0003083D File Offset: 0x0002EA3D
	public virtual void SetInt(string integer, int i)
	{
		base.animator.SetInteger(integer, i);
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0003084C File Offset: 0x0002EA4C
	public virtual void SetBool(string boolean, bool b)
	{
		base.animator.SetBool(boolean, b);
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0003085B File Offset: 0x0002EA5B
	public virtual int GetVariants()
	{
		return base.animator.GetInteger(AbstractProjectile.MaxVariants);
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x001144C0 File Offset: 0x001126C0
	public virtual void RandomizeVariant()
	{
		int i = Random.Range(0, this.GetVariants());
		this.SetInt(AbstractProjectile.Variant, i);
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0003086D File Offset: 0x0002EA6D
	public void AddFiringHitbox(LevelPlayerWeaponFiringHitbox hitbox)
	{
		this.firingHitbox = hitbox;
		base.RegisterCollisionChild(hitbox);
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x001144E8 File Offset: 0x001126E8
	public virtual void FixedUpdate()
	{
		if (this.firingHitbox != null)
		{
			if (this.firstUpdate)
			{
				this.firstUpdate = false;
			}
			else
			{
				if (!this.dead)
				{
					base.GetComponent<Collider2D>().enabled = true;
				}
				Object.Destroy(this.firingHitbox.gameObject);
				this.firingHitbox = null;
			}
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x00030889 File Offset: 0x0002EA89
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.firingHitbox != null)
		{
			Object.Destroy(this.firingHitbox.gameObject);
		}
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x000308B2 File Offset: 0x0002EAB2
	public virtual void AddToMeterScoreTracker(MeterScoreTracker tracker)
	{
		this.tracker = tracker;
		if (this.damageDealer != null)
		{
			tracker.Add(this.damageDealer);
		}
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x00114564 File Offset: 0x00112764
	public static IEnumerable<DamageReceiver> FindOverlapScreenDamageReceivers()
	{
		AbstractProjectile.DamageReceiverComponentBuffer.Clear();
		AbstractProjectile.DamageReceiverSearchSet.Clear();
		Vector2 padding;
		padding..ctor(100f, 100f);
		Rect rect = CupheadLevelCamera.Current.CalculateContainsBounds(padding);
		int num = Physics2D.OverlapBoxNonAlloc(rect.center, rect.size, 0f, AbstractProjectile.ColliderBuffer);
		for (int i = 0; i < num; i++)
		{
			AbstractProjectile.DamageReceiverComponentBuffer.Clear();
			Collider2D collider2D = AbstractProjectile.ColliderBuffer[i];
			collider2D.GetComponentsInParent<DamageReceiver>(true, AbstractProjectile.DamageReceiverComponentBuffer);
			AbstractProjectile.DamageReceiverSearchSet.UnionWith(AbstractProjectile.DamageReceiverComponentBuffer);
		}
		return AbstractProjectile.DamageReceiverSearchSet;
	}

	// Token: 0x04002F7B RID: 12155
	public static string Variant = "Variant";

	// Token: 0x04002F7C RID: 12156
	public static string MaxVariants = "MaxVariants";

	// Token: 0x04002F7D RID: 12157
	public static string OnDeathTrigger = "OnDeath";

	// Token: 0x04002F7E RID: 12158
	public static string Parry = "Parry";

	// Token: 0x04002F7F RID: 12159
	public Vector3 startPosition;

	// Token: 0x04002F80 RID: 12160
	public Vector3 lastPosition;

	// Token: 0x04002F81 RID: 12161
	public MeterScoreTracker tracker;

	// Token: 0x04002F82 RID: 12162
	public bool hasBeenRendered;

	// Token: 0x04002F83 RID: 12163
	[SerializeField]
	public bool _canParry;

	// Token: 0x04002F84 RID: 12164
	public bool _countParryTowardsScore = true;

	// Token: 0x04002F88 RID: 12168
	public bool missed = true;

	// Token: 0x04002F89 RID: 12169
	public DamageDealer damageDealer;

	// Token: 0x04002F8B RID: 12171
	public float _setYPadding = 150f;

	// Token: 0x04002F8C RID: 12172
	public DamageDealer.DamageSource damageSource;

	// Token: 0x04002F8D RID: 12173
	public float Damage = 1f;

	// Token: 0x04002F8E RID: 12174
	public float DamageRate;

	// Token: 0x04002F8F RID: 12175
	public PlayerId PlayerId = PlayerId.None;

	// Token: 0x04002F90 RID: 12176
	public DamageDealer.DamageTypesManager DamagesType;

	// Token: 0x04002F91 RID: 12177
	public AbstractProjectile.CollisionProperties CollisionDeath;

	// Token: 0x04002F92 RID: 12178
	[NonSerialized]
	public float DestroyDistance = 3000f;

	// Token: 0x04002F93 RID: 12179
	[NonSerialized]
	public bool DestroyDistanceAnimated;

	// Token: 0x04002F96 RID: 12182
	public LevelPlayerWeaponFiringHitbox firingHitbox;

	// Token: 0x04002F97 RID: 12183
	public bool firstUpdate = true;

	// Token: 0x04002F98 RID: 12184
	public static readonly Collider2D[] ColliderBuffer = new Collider2D[500];

	// Token: 0x04002F99 RID: 12185
	public static HashSet<DamageReceiver> DamageReceiverSearchSet = new HashSet<DamageReceiver>();

	// Token: 0x04002F9A RID: 12186
	public static List<DamageReceiver> DamageReceiverComponentBuffer = new List<DamageReceiver>();

	// Token: 0x0200120A RID: 4618
	[Serializable]
	public class CollisionProperties
	{
		// Token: 0x06008008 RID: 32776 RVA: 0x00055A9C File Offset: 0x00053C9C
		public AbstractProjectile.CollisionProperties Copy()
		{
			return base.MemberwiseClone() as AbstractProjectile.CollisionProperties;
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x00055AA9 File Offset: 0x00053CA9
		public void SetAll(bool b)
		{
			this.Walls = b;
			this.Ceiling = b;
			this.Ground = b;
			this.Enemies = b;
			this.EnemyProjectiles = b;
			this.Player = b;
			this.PlayerProjectiles = b;
			this.Other = b;
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x00055AE3 File Offset: 0x00053CE3
		public void All()
		{
			this.SetAll(true);
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x00055AEC File Offset: 0x00053CEC
		public void None()
		{
			this.SetAll(false);
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x00055AF5 File Offset: 0x00053CF5
		public void OnlyPlayer()
		{
			this.SetAll(false);
			this.Player = true;
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x00055B05 File Offset: 0x00053D05
		public void OnlyEnemies()
		{
			this.SetAll(false);
			this.Player = true;
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x00055B15 File Offset: 0x00053D15
		public void OnlyBounds()
		{
			this.SetAll(false);
			this.SetBounds(true);
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x00055B25 File Offset: 0x00053D25
		public void SetBounds(bool b)
		{
			this.Walls = b;
			this.Ceiling = b;
			this.Ground = b;
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x00055B3C File Offset: 0x00053D3C
		public void PlayerProjectileDefault()
		{
			this.SetAll(false);
			this.SetBounds(true);
			this.Enemies = true;
			this.Other = true;
		}

		// Token: 0x04007D46 RID: 32070
		public bool Walls = true;

		// Token: 0x04007D47 RID: 32071
		public bool Ceiling = true;

		// Token: 0x04007D48 RID: 32072
		public bool Ground = true;

		// Token: 0x04007D49 RID: 32073
		public bool Enemies;

		// Token: 0x04007D4A RID: 32074
		public bool EnemyProjectiles;

		// Token: 0x04007D4B RID: 32075
		public bool Player;

		// Token: 0x04007D4C RID: 32076
		public bool PlayerProjectiles;

		// Token: 0x04007D4D RID: 32077
		public bool Other;
	}
}
