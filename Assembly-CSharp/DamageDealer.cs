using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class DamageDealer
{
	// Token: 0x06003C1F RID: 15391 RVA: 0x00030ABA File Offset: 0x0002ECBA
	public DamageDealer(float damage, float damageRate)
	{
		this.Setup(damage, damageRate);
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x00114D20 File Offset: 0x00112F20
	public DamageDealer(float damage, float damageRate, bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, damagesPlayer, damagesEnemy, damagesOther, 1f);
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x00114D74 File Offset: 0x00112F74
	public DamageDealer(float damage, float damageRate, DamageDealer.DamageSource damageSource, bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.Setup(damage, damageRate, damageSource, damagesPlayer, damagesEnemy, damagesOther, 1f);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x00114DC8 File Offset: 0x00112FC8
	public DamageDealer(AbstractProjectile projectile)
	{
		this.Setup(projectile.Damage, projectile.DamageRate, projectile.DamageSource, projectile.GetDamagesType(DamageReceiver.Type.Player), projectile.GetDamagesType(DamageReceiver.Type.Enemy), projectile.GetDamagesType(DamageReceiver.Type.Other), projectile.DamageMultiplier);
		this.SetDirection(DamageDealer.Direction.Neutral, projectile.transform);
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06003C23 RID: 15395 RVA: 0x00030AF6 File Offset: 0x0002ECF6
	// (set) Token: 0x06003C24 RID: 15396 RVA: 0x00030AFE File Offset: 0x0002ECFE
	public float DamageDealt { get; set; }

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06003C25 RID: 15397 RVA: 0x00030B07 File Offset: 0x0002ED07
	// (set) Token: 0x06003C26 RID: 15398 RVA: 0x00030B0F File Offset: 0x0002ED0F
	public float DamageMultiplier
	{
		get
		{
			return this.damageMultiplier;
		}
		set
		{
			this.damageMultiplier = value;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06003C27 RID: 15399 RVA: 0x00030B18 File Offset: 0x0002ED18
	// (set) Token: 0x06003C28 RID: 15400 RVA: 0x00030B20 File Offset: 0x0002ED20
	public PlayerId PlayerId
	{
		get
		{
			return this.playerId;
		}
		set
		{
			this.playerId = value;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06003C29 RID: 15401 RVA: 0x00030B29 File Offset: 0x0002ED29
	// (set) Token: 0x06003C2A RID: 15402 RVA: 0x00030B31 File Offset: 0x0002ED31
	public bool isDLCWeapon { get; set; }

	// Token: 0x140000BE RID: 190
	// (add) Token: 0x06003C2B RID: 15403 RVA: 0x00114E48 File Offset: 0x00113048
	// (remove) Token: 0x06003C2C RID: 15404 RVA: 0x00114E80 File Offset: 0x00113080
	public event DamageDealer.OnDealDamageHandler OnDealDamage;

	// Token: 0x06003C2D RID: 15405 RVA: 0x00030B3A File Offset: 0x0002ED3A
	public static DamageDealer NewEnemy()
	{
		return DamageDealer.NewEnemy(0.2f);
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x00030B46 File Offset: 0x0002ED46
	public static DamageDealer NewEnemy(float rate)
	{
		return new DamageDealer(1f, rate, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x00030B57 File Offset: 0x0002ED57
	public void Setup(float damage, float damageRate)
	{
		this.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, true, false, false, 1f);
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x00030B6A File Offset: 0x0002ED6A
	public void Setup(float damage, float damageRate, DamageDealer.DamageSource damageSource)
	{
		this.Setup(damage, damageRate, damageSource, true, false, false, 1f);
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x00114EB8 File Offset: 0x001130B8
	public void Setup(float damage, float damageRate, DamageDealer.DamageSource damageSource, bool damagesPlayer, bool damagesEnemy, bool damagesOther, float damageMultiplier = 1f)
	{
		this.damage = damage;
		this.damageRate = damageRate;
		this.damageMultiplier = damageMultiplier;
		this.damageTypes = new DamageDealer.DamageTypesManager();
		this.SetDamageFlags(damagesPlayer, damagesEnemy, damagesOther);
		this.SetDamageSource(damageSource);
		this.timers = new Dictionary<int, float>();
		this.timersList = new List<int>();
		this.StoneTime = -1f;
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x00030B7D File Offset: 0x0002ED7D
	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x00030B86 File Offset: 0x0002ED86
	public void SetRate(float rate)
	{
		this.damageRate = rate;
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x00030B8F File Offset: 0x0002ED8F
	public void SetDamageSource(DamageDealer.DamageSource source)
	{
		this.damageSource = source;
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x00030B98 File Offset: 0x0002ED98
	public void SetDamageFlags(bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.damageTypes.Player = damagesPlayer;
		this.damageTypes.Enemies = damagesEnemy;
		this.damageTypes.Other = damagesOther;
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x00030BBE File Offset: 0x0002EDBE
	public void SetDirection(DamageDealer.Direction direction, Transform origin)
	{
		this.direction = direction;
		this.origin = origin;
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x00114F1C File Offset: 0x0011311C
	public float DealDamage(GameObject hit)
	{
		DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
		if (damageReceiver == null)
		{
			DamageReceiverChild component = hit.GetComponent<DamageReceiverChild>();
			if (component != null && component.enabled)
			{
				damageReceiver = component.Receiver;
			}
		}
		if (!(damageReceiver != null) || !damageReceiver.enabled)
		{
			return 0f;
		}
		int instanceID = damageReceiver.GetInstanceID();
		if (!this.damageTypes.GetType(damageReceiver.type))
		{
			return 0f;
		}
		if (!this.timers.ContainsKey(instanceID))
		{
			this.timers.Add(instanceID, this.damageRate);
			this.timersList.Add(instanceID);
		}
		else if (this.damageRate == 0f)
		{
			return 0f;
		}
		if (this.timers[instanceID] < this.damageRate)
		{
			return 0f;
		}
		Vector2 vector = (!(this.origin != null)) ? Vector2.zero : this.origin.position;
		DamageDealer.DamageInfo damageInfo = new DamageDealer.DamageInfo(this.damage * this.damageMultiplier, this.direction, vector, this.damageSource);
		damageInfo.SetStoneTime(this.StoneTime);
		damageReceiver.TakeDamage(damageInfo);
		this.DamageDealt += this.damage * this.damageMultiplier;
		this.timers[damageReceiver.GetInstanceID()] = 0f;
		if (this.OnDealDamage != null)
		{
			this.OnDealDamage(this.damage * this.damageMultiplier, damageReceiver, this);
		}
		if (this.playerId != PlayerId.None && damageReceiver.type == DamageReceiver.Type.Enemy)
		{
			DamageDealer.lastPlayer = this.playerId;
			DamageDealer.lastPlayerDamageSource = this.damageSource;
			if (this.damageSource != DamageDealer.DamageSource.SmallPlane)
			{
				DamageDealer.didDamageWithNonSmallPlaneWeapon = true;
			}
			DamageDealer.lastDamageWasDLCWeapon = this.isDLCWeapon;
		}
		return this.damage;
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x00115114 File Offset: 0x00113314
	public void Update()
	{
		foreach (int num in this.timersList)
		{
			Dictionary<int, float> dictionary;
			int key;
			(dictionary = this.timers)[key = num] = dictionary[key] + CupheadTime.Delta;
		}
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x0011518C File Offset: 0x0011338C
	public void FixedUpdate()
	{
		foreach (int num in this.timersList)
		{
			Dictionary<int, float> dictionary;
			int key;
			(dictionary = this.timers)[key = num] = dictionary[key] + CupheadTime.FixedDelta;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06003C3A RID: 15418 RVA: 0x00030BCE File Offset: 0x0002EDCE
	// (set) Token: 0x06003C3B RID: 15419 RVA: 0x00030BD6 File Offset: 0x0002EDD6
	public float StoneTime { get; set; }

	// Token: 0x06003C3C RID: 15420 RVA: 0x00030BDF File Offset: 0x0002EDDF
	public void SetStoneTime(float stoneTime)
	{
		this.StoneTime = stoneTime;
	}

	// Token: 0x04002FB5 RID: 12213
	public static DamageDealer.DamageSource lastPlayerDamageSource;

	// Token: 0x04002FB6 RID: 12214
	public static PlayerId lastPlayer;

	// Token: 0x04002FB7 RID: 12215
	public static bool lastDamageWasDLCWeapon;

	// Token: 0x04002FB8 RID: 12216
	public static bool didDamageWithNonSmallPlaneWeapon;

	// Token: 0x04002FB9 RID: 12217
	public Dictionary<int, float> timers;

	// Token: 0x04002FBA RID: 12218
	public List<int> timersList;

	// Token: 0x04002FBB RID: 12219
	public float damage = 1f;

	// Token: 0x04002FBC RID: 12220
	public float damageRate = 1f;

	// Token: 0x04002FBD RID: 12221
	public float damageMultiplier = 1f;

	// Token: 0x04002FBE RID: 12222
	public DamageDealer.Direction direction;

	// Token: 0x04002FBF RID: 12223
	public Transform origin;

	// Token: 0x04002FC0 RID: 12224
	public DamageDealer.DamageSource damageSource;

	// Token: 0x04002FC1 RID: 12225
	public DamageDealer.DamageTypesManager damageTypes;

	// Token: 0x04002FC2 RID: 12226
	public PlayerId playerId = PlayerId.None;

	// Token: 0x0200120C RID: 4620
	public enum Direction
	{
		// Token: 0x04007D53 RID: 32083
		Neutral,
		// Token: 0x04007D54 RID: 32084
		Left,
		// Token: 0x04007D55 RID: 32085
		Right
	}

	// Token: 0x0200120D RID: 4621
	public enum DamageSource
	{
		// Token: 0x04007D57 RID: 32087
		Neutral,
		// Token: 0x04007D58 RID: 32088
		Enemy,
		// Token: 0x04007D59 RID: 32089
		Ex,
		// Token: 0x04007D5A RID: 32090
		SmallPlane,
		// Token: 0x04007D5B RID: 32091
		Super,
		// Token: 0x04007D5C RID: 32092
		Pit
	}

	// Token: 0x0200120E RID: 4622
	// (Invoke) Token: 0x06008018 RID: 32792
	public delegate void OnDealDamageHandler(float damage, DamageReceiver receiver, DamageDealer dealer);

	// Token: 0x0200120F RID: 4623
	public class DamageInfo
	{
		// Token: 0x0600801B RID: 32795 RVA: 0x00055B89 File Offset: 0x00053D89
		public DamageInfo(float damage, DamageDealer.Direction direction, Vector2 origin, DamageDealer.DamageSource source)
		{
			this.direction = direction;
			this.origin = origin;
			this.damageSource = source;
			this.damage = damage;
			this.stoneTime = -1f;
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600801C RID: 32796 RVA: 0x00055BB9 File Offset: 0x00053DB9
		// (set) Token: 0x0600801D RID: 32797 RVA: 0x00055BC1 File Offset: 0x00053DC1
		public float damage { get; set; }

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600801E RID: 32798 RVA: 0x00055BCA File Offset: 0x00053DCA
		// (set) Token: 0x0600801F RID: 32799 RVA: 0x00055BD2 File Offset: 0x00053DD2
		public DamageDealer.Direction direction { get; set; }

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06008020 RID: 32800 RVA: 0x00055BDB File Offset: 0x00053DDB
		// (set) Token: 0x06008021 RID: 32801 RVA: 0x00055BE3 File Offset: 0x00053DE3
		public Vector2 origin { get; set; }

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06008022 RID: 32802 RVA: 0x00055BEC File Offset: 0x00053DEC
		// (set) Token: 0x06008023 RID: 32803 RVA: 0x00055BF4 File Offset: 0x00053DF4
		public DamageDealer.DamageSource damageSource { get; set; }

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06008024 RID: 32804 RVA: 0x00055BFD File Offset: 0x00053DFD
		// (set) Token: 0x06008025 RID: 32805 RVA: 0x00055C05 File Offset: 0x00053E05
		public float stoneTime { get; set; }

		// Token: 0x06008026 RID: 32806 RVA: 0x00055C0E File Offset: 0x00053E0E
		public void SetStoneTime(float stoneTime)
		{
			this.stoneTime = stoneTime;
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x00055C17 File Offset: 0x00053E17
		public void SetEditorPlayer()
		{
			this.damage *= 10f;
		}
	}

	// Token: 0x02001210 RID: 4624
	[Serializable]
	public class DamageTypesManager
	{
		// Token: 0x06008029 RID: 32809 RVA: 0x00055C33 File Offset: 0x00053E33
		public DamageDealer.DamageTypesManager Copy()
		{
			return base.MemberwiseClone() as DamageDealer.DamageTypesManager;
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x00055C40 File Offset: 0x00053E40
		public void SetAll(bool b)
		{
			this.Player = b;
			this.Enemies = b;
			this.Other = b;
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x00055C57 File Offset: 0x00053E57
		public DamageDealer.DamageTypesManager OnlyPlayer()
		{
			this.SetAll(false);
			this.Player = true;
			return this;
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x00055C68 File Offset: 0x00053E68
		public DamageDealer.DamageTypesManager OnlyEnemies()
		{
			this.SetAll(false);
			this.Enemies = true;
			return this;
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x00055C79 File Offset: 0x00053E79
		public DamageDealer.DamageTypesManager PlayerProjectileDefault()
		{
			this.SetAll(false);
			this.Enemies = true;
			this.Other = true;
			return this;
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x00055C91 File Offset: 0x00053E91
		public bool GetType(DamageReceiver.Type type)
		{
			switch (type)
			{
			case DamageReceiver.Type.Enemy:
				return this.Enemies;
			case DamageReceiver.Type.Player:
				return this.Player;
			case DamageReceiver.Type.Other:
				return this.Other;
			default:
				return false;
			}
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x002945A8 File Offset: 0x002927A8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Player:",
				this.Player,
				", Enemies:",
				this.Enemies,
				", Other:",
				this.Other
			});
		}

		// Token: 0x04007D62 RID: 32098
		public bool Player;

		// Token: 0x04007D63 RID: 32099
		public bool Enemies;

		// Token: 0x04007D64 RID: 32100
		public bool Other;
	}
}
