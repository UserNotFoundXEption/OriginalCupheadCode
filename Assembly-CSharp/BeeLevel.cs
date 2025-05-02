using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class BeeLevel : Level
{
	// Token: 0x06000086 RID: 134 RVA: 0x0005EE20 File Offset: 0x0005D020
	public override void PartialInit()
	{
		this.properties = LevelProperties.Bee.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000087 RID: 135 RVA: 0x000036EE File Offset: 0x000018EE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Bee;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000088 RID: 136 RVA: 0x000036F5 File Offset: 0x000018F5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_bee;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000089 RID: 137 RVA: 0x000036F9 File Offset: 0x000018F9
	public float Speed
	{
		get
		{
			return this.speed * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00003707 File Offset: 0x00001907
	public int MissingPlatformCount
	{
		get
		{
			return this.missingPlatformCount;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600008B RID: 139 RVA: 0x0005EEB8 File Offset: 0x0005D0B8
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Bee.States.Main:
				return this._bossPortraitGuard;
			case LevelProperties.Bee.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Bee.States.Airplane:
				return this._bossPortraitAirplane;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600008C RID: 140 RVA: 0x0005EF34 File Offset: 0x0005D134
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Bee.States.Main:
				return this._bossQuoteGuard;
			case LevelProperties.Bee.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Bee.States.Airplane:
				return this._bossQuoteAirplane;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0005EFB0 File Offset: 0x0005D1B0
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.drip_cr());
		this.queen.LevelInit(this.properties);
		this.guard.LevelInit(this.properties);
		this.background.LevelInit(this.properties);
		this.airplane.LevelInit(this.properties);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x0000370F File Offset: 0x0000190F
	public override void Update()
	{
		base.Update();
		this.UpdateSpeed();
	}

	// Token: 0x0600008F RID: 143 RVA: 0x0005F014 File Offset: 0x0005D214
	public override void CreatePlayers()
	{
		base.CreatePlayers();
		if (PlayerManager.Multiplayer && this.allowMultiplayer && this.players[1].stats.isChalice)
		{
			this.players[1].transform.position = this.p2ChaliceSpawnPoint;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0005F070 File Offset: 0x0005D270
	public override void OnLevelStart()
	{
		this.missingPlatformCount = this.properties.CurrentState.movement.missingPlatforms;
		this.targetSpeed = -this.properties.CurrentState.movement.speed;
		base.StartCoroutine(this.beePattern_cr());
		this.CheckGrunts();
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0000371D File Offset: 0x0000191D
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Bee.States.Airplane)
		{
			base.StartCoroutine(this.airplane_cr());
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003748 File Offset: 0x00001948
	public void UpdateSpeed()
	{
		this.speed = Mathf.Lerp(this.speed, this.targetSpeed, 0.5f * CupheadTime.Delta);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00003771 File Offset: 0x00001971
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitAirplane = null;
		this._bossPortraitGuard = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0005F0C8 File Offset: 0x0005D2C8
	public void CheckGrunts()
	{
		if (this.gruntCoroutine != null)
		{
			base.StopCoroutine(this.gruntCoroutine);
		}
		if (this.properties.CurrentState.grunts.active)
		{
			this.gruntCoroutine = base.StartCoroutine(this.grunts_cr());
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0005F118 File Offset: 0x0005D318
	public IEnumerator beePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0005F134 File Offset: 0x0005D334
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Bee.Pattern.BlackHole:
			yield return base.StartCoroutine(this.blackHole_cr());
			break;
		case LevelProperties.Bee.Pattern.Chain:
			yield return base.StartCoroutine(this.chain_cr());
			break;
		case LevelProperties.Bee.Pattern.Triangle:
			yield return base.StartCoroutine(this.triangle_cr());
			break;
		case LevelProperties.Bee.Pattern.Follower:
			yield return base.StartCoroutine(this.follower_cr());
			break;
		case LevelProperties.Bee.Pattern.SecurityGuard:
			yield return base.StartCoroutine(this.security_cr());
			break;
		case LevelProperties.Bee.Pattern.Wing:
			yield return base.StartCoroutine(this.wing_cr());
			break;
		case LevelProperties.Bee.Pattern.Turbine:
			yield return base.StartCoroutine(this.turbine_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0005F150 File Offset: 0x0005D350
	public IEnumerator airplane_cr()
	{
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		this.queen.StartMorph();
		this.honeyDripping = false;
		this.targetSpeed = -this.properties.CurrentState.general.screenScrollSpeed;
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0005F16C File Offset: 0x0005D36C
	public IEnumerator turbine_cr()
	{
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		this.airplane.StartTurbine();
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0005F188 File Offset: 0x0005D388
	public IEnumerator wing_cr()
	{
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		this.airplane.StartWing();
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0005F1A4 File Offset: 0x0005D3A4
	public IEnumerator security_cr()
	{
		this.guard.StartSecurityGuard();
		while (this.guard.state != BeeLevelSecurityGuard.State.Ready)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0005F1C0 File Offset: 0x0005D3C0
	public IEnumerator blackHole_cr()
	{
		this.queen.StartBlackHole();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0005F1DC File Offset: 0x0005D3DC
	public IEnumerator triangle_cr()
	{
		this.queen.StartTriangle();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0005F1F8 File Offset: 0x0005D3F8
	public IEnumerator follower_cr()
	{
		this.queen.StartFollower();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0005F214 File Offset: 0x0005D414
	public IEnumerator chain_cr()
	{
		this.queen.StartChain();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0005F230 File Offset: 0x0005D430
	public IEnumerator drip_cr()
	{
		while (this.honeyDripping)
		{
			yield return CupheadTime.WaitForSeconds(this, (float)Random.Range(1, 3));
			this.prefabs.drip.Create();
		}
		yield break;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0005F24C File Offset: 0x0005D44C
	public IEnumerator grunts_cr()
	{
		string[] strings = this.properties.CurrentState.grunts.entrancePoints[Random.Range(0, this.properties.CurrentState.grunts.entrancePoints.Length)].Split(new char[]
		{
			','
		});
		int[] positions = new int[strings.Length];
		for (int i = 0; i < strings.Length; i++)
		{
			Parser.IntTryParse(strings[i], out positions[i]);
			positions[i] = Mathf.Clamp(positions[i], 0, this.gruntRoots.Length);
		}
		int index = Random.Range(0, positions.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.grunts.delay);
			int scale = (PlayerManager.Center.x <= 0f) ? 1 : -1;
			if (PlayerManager.Center.x > 0f)
			{
				scale = -1;
			}
			this.prefabs.grunt.Create(this.gruntRoots[positions[index]].position + new Vector3((float)(840 * scale), 0f, 0f), scale, this.properties.CurrentState.grunts.health, this.properties.CurrentState.grunts.speed);
			index = (int)Mathf.Repeat((float)(index + 1), (float)positions.Length);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000095 RID: 149
	public LevelProperties.Bee properties;

	// Token: 0x04000096 RID: 150
	public const float SPEED_TIME = 0.5f;

	// Token: 0x04000097 RID: 151
	[SerializeField]
	public Vector2 p2ChaliceSpawnPoint;

	// Token: 0x04000098 RID: 152
	[SerializeField]
	public BeeLevelAirplane airplane;

	// Token: 0x04000099 RID: 153
	[Space(10f)]
	[SerializeField]
	public BeeLevelQueen queen;

	// Token: 0x0400009A RID: 154
	[SerializeField]
	public BeeLevelSecurityGuard guard;

	// Token: 0x0400009B RID: 155
	[Space(10f)]
	[SerializeField]
	public Transform[] gruntRoots;

	// Token: 0x0400009C RID: 156
	[Space(10f)]
	[SerializeField]
	public BeeLevelBackground background;

	// Token: 0x0400009D RID: 157
	[Space(10f)]
	[SerializeField]
	public BeeLevel.Prefabs prefabs;

	// Token: 0x0400009E RID: 158
	public bool honeyDripping = true;

	// Token: 0x0400009F RID: 159
	public float speed;

	// Token: 0x040000A0 RID: 160
	public float targetSpeed;

	// Token: 0x040000A1 RID: 161
	public int missingPlatformCount;

	// Token: 0x040000A2 RID: 162
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitGuard;

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040000A4 RID: 164
	[SerializeField]
	public Sprite _bossPortraitAirplane;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	public string _bossQuoteGuard;

	// Token: 0x040000A6 RID: 166
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040000A7 RID: 167
	[SerializeField]
	public string _bossQuoteAirplane;

	// Token: 0x040000A8 RID: 168
	public Coroutine gruntCoroutine;

	// Token: 0x02000737 RID: 1847
	[Serializable]
	public class Prefabs
	{
		// Token: 0x04003B37 RID: 15159
		public BeeLevelGrunt grunt;

		// Token: 0x04003B38 RID: 15160
		public BeeLevelHoneyDrip drip;
	}
}
