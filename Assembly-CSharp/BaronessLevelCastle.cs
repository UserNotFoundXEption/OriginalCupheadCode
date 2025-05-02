using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class BaronessLevelCastle : LevelProperties.Baroness.Entity
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0000D1AC File Offset: 0x0000B3AC
	// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0000D1B3 File Offset: 0x0000B3B3
	public static BaronessLevelMiniBossBase CURRENT_MINI_BOSS { get; set; }

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0000D1BB File Offset: 0x0000B3BB
	// (set) Token: 0x06000F6D RID: 3949 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
	public BaronessLevelCastle.State state { get; set; }

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0000D1CC File Offset: 0x0000B3CC
	// (set) Token: 0x06000F6F RID: 3951 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
	public BaronessLevelCastle.TeethState teethState { get; set; }

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x06000F70 RID: 3952 RVA: 0x0008D6C0 File Offset: 0x0008B8C0
	// (remove) Token: 0x06000F71 RID: 3953 RVA: 0x0008D6F8 File Offset: 0x0008B8F8
	public event Action OnDeathEvent;

	// Token: 0x06000F72 RID: 3954 RVA: 0x0008D730 File Offset: 0x0008B930
	public override void Awake()
	{
		base.Awake();
		this.maxMiniBosses = false;
		this.continueTransition = false;
		this.originalEmergePos = this.emergePoint.position;
		this.originalBaronessPoint = this.baronessPhase1.transform.position;
		this.teethState = BaronessLevelCastle.TeethState.Unspawned;
		this.baronessPhase2.gameObject.SetActive(false);
		this.blink.enabled = false;
		this.blinkCounterMax = Random.Range(4, 7);
		this.damageReceiver = this.baronessPhase2.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 1f);
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0008D7E8 File Offset: 0x0008B9E8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BaronessLevelCastle.State.Dead)
		{
			this.state = BaronessLevelCastle.State.Dead;
			this.StartDeath();
		}
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0008D834 File Offset: 0x0008BA34
	public void Update()
	{
		this.damageDealer.Update();
		this.player = PlayerManager.GetNext();
		this.distToGround = this.player.transform.position.y - -360f;
		if (this.state == BaronessLevelCastle.State.Idle)
		{
			if (BaronessLevelCastle.CURRENT_MINI_BOSS == null)
			{
				if (!this.maxMiniBosses)
				{
					this.jellyChangeDelay = true;
					this.StartOpen();
				}
				else if (Level.Current.mode != Level.Mode.Easy)
				{
					this.state = BaronessLevelCastle.State.ChaseIntro;
					this.StartChase();
				}
			}
			else if (Level.Current.mode == Level.Mode.Easy && this.state != BaronessLevelCastle.State.EasyFinal && BaronessLevelCastle.CURRENT_MINI_BOSS.isDying && this.maxMiniBosses)
			{
				this.state = BaronessLevelCastle.State.EasyFinal;
				base.StartCoroutine(this.shoot_easy_cr());
			}
		}
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0000D1DD File Offset: 0x0000B3DD
	public override void LevelInit(LevelProperties.Baroness properties)
	{
		base.LevelInit(properties);
		this.baronessPhase1.getProperties(properties, (float)properties.CurrentState.baronessVonBonbon.HP, this);
		this.platform.getProperties(properties.CurrentState.platform);
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x0000D21A File Offset: 0x0000B41A
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x0008D920 File Offset: 0x0008BB20
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.6f);
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.Play("Castle_Open");
		AudioManager.Play("level_baroness_castle_gate_open");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Open", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.state = BaronessLevelCastle.State.Idle;
		yield break;
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0000D229 File Offset: 0x0000B429
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0000D247 File Offset: 0x0000B447
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.peppermintPrefab = null;
		this.cupcakePrefab = null;
		this.wafflePrefab = null;
		this.gumballPrefab = null;
		this.jawBreakerPrefab = null;
		this.candyCornPrefab = null;
		this.greenJellyPrefab = null;
		this.pinkJellyPrefab = null;
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0000D287 File Offset: 0x0000B487
	public void StartOpen()
	{
		this.state = BaronessLevelCastle.State.Open;
		base.StartCoroutine(this.open_cr());
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0000D29D File Offset: 0x0000B49D
	public void SetEyes()
	{
		base.animator.SetBool("ToCastleLoop", true);
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0008D93C File Offset: 0x0008BB3C
	public IEnumerator open_cr()
	{
		LevelProperties.Baroness.Open p = base.properties.CurrentState.open;
		this.castleOpen = true;
		if (this.baronessPoppedUp)
		{
			this.baronessPoppedUp = false;
			yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this, "Baroness_Leave", false, true);
		}
		if (this.bossIndex != 0)
		{
			AudioManager.Play("level_baroness_castle_gate_open");
			base.animator.Play("Castle_Open");
			yield return base.animator.WaitForAnimationToEnd(this, "Castle_Open", false, true);
			this.baronessPhase1.animator.Play("Baroness_Mad_Start");
			AudioManager.Play("level_baroness_stick_head_pop");
			while (this.baronessPhase1.popUpCounter < 4)
			{
				yield return null;
			}
			this.baronessPhase1.animator.SetTrigger("PopIn");
			AudioManager.Play("level_baroness_stick_head_pop");
			this.baronessPhase1.popUpCounter = 0;
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
		}
		switch ((BaronessLevelCastle.BossPossibility)Enum.Parse(typeof(BaronessLevelCastle.BossPossibility), BaronessLevel.PICKED_BOSSES[this.bossIndex]))
		{
		case BaronessLevelCastle.BossPossibility.Gumball:
			this.SpawnGumball();
			break;
		case BaronessLevelCastle.BossPossibility.Waffle:
			this.SpawnWaffle();
			break;
		case BaronessLevelCastle.BossPossibility.CandyCorn:
			this.SpawnCandyCorn();
			break;
		case BaronessLevelCastle.BossPossibility.Cupcake:
			this.SpawnCupcake();
			break;
		case BaronessLevelCastle.BossPossibility.Jawbreaker:
			this.SpawnJawbreaker();
			break;
		}
		yield return CupheadTime.WaitForSeconds(this, this.setWaitTime);
		base.animator.SetBool("ToCastleLoop", false);
		if (this.bossIndex < p.miniBossAmount)
		{
			this.bossIndex++;
		}
		if (base.properties.CurrentState.jellybeans.startingPoint == (float)this.bossIndex)
		{
			this.StartJellybeans();
		}
		if (this.bossIndex == p.miniBossAmount)
		{
			this.maxMiniBosses = true;
		}
		AudioManager.Play("level_baroness_castle_gate_close");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
		this.castleOpen = false;
		if (base.properties.CurrentState.baronessVonBonbon.miniBossStart == (float)this.bossIndex)
		{
			this.StartBaronessShoot();
		}
		this.state = BaronessLevelCastle.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0008D958 File Offset: 0x0008BB58
	public void Blink()
	{
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.blink.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.blink.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = Random.Range(4, 7);
		}
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0008D9B8 File Offset: 0x0008BBB8
	public void SpawnGumball()
	{
		Vector3 position = this.emergePoint.position;
		position.y = this.emergePoint.position.y + 100f;
		this.emergePoint.position = position;
		BaronessLevelGumball baronessLevelGumball = Object.Instantiate<BaronessLevelGumball>(this.gumballPrefab);
		LevelProperties.Baroness.Gumball gumball = base.properties.CurrentState.gumball;
		baronessLevelGumball.Init(gumball, this.emergePoint.position, (float)gumball.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelGumball;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Gumball;
		this.setWaitTime = 1f;
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0008DA68 File Offset: 0x0008BC68
	public void SpawnWaffle()
	{
		BaronessLevelWaffle baronessLevelWaffle = Object.Instantiate<BaronessLevelWaffle>(this.wafflePrefab);
		LevelProperties.Baroness.Waffle waffle = base.properties.CurrentState.waffle;
		baronessLevelWaffle.Init(waffle, this.emergePoint.position, this.pivotPoint, waffle.movementSpeed, (float)waffle.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelWaffle;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Waffle;
		this.setWaitTime = 1f;
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0008DAD8 File Offset: 0x0008BCD8
	public void SpawnCandyCorn()
	{
		BaronessLevelCandyCorn baronessLevelCandyCorn = Object.Instantiate<BaronessLevelCandyCorn>(this.candyCornPrefab);
		LevelProperties.Baroness.CandyCorn candyCorn = base.properties.CurrentState.candyCorn;
		baronessLevelCandyCorn.Init(candyCorn, new Vector3(this.emergePoint.position.x, this.emergePoint.position.y + 40f), candyCorn.movementSpeed, (float)candyCorn.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCandyCorn;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.CandyCorn;
		this.setWaitTime = 1f;
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0008DB68 File Offset: 0x0008BD68
	public void SpawnCupcake()
	{
		Vector3 position = this.emergePoint.position;
		position.y = this.emergePoint.position.y;
		position.x = this.emergePoint.position.x + 200f;
		this.emergePoint.position = position;
		BaronessLevelCupcake baronessLevelCupcake = Object.Instantiate<BaronessLevelCupcake>(this.cupcakePrefab);
		LevelProperties.Baroness.Cupcake cupcake = base.properties.CurrentState.cupcake;
		baronessLevelCupcake.Init(cupcake, this.emergePoint.position, (float)cupcake.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCupcake;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Cupcake;
		this.setWaitTime = 1f;
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x0008DC34 File Offset: 0x0008BE34
	public void SpawnJawbreaker()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		BaronessLevelJawbreaker baronessLevelJawbreaker = Object.Instantiate<BaronessLevelJawbreaker>(this.jawBreakerPrefab);
		LevelProperties.Baroness.Jawbreaker jawbreaker = base.properties.CurrentState.jawbreaker;
		baronessLevelJawbreaker.Init(jawbreaker, next, new Vector3(this.emergePoint.position.x, this.emergePoint.position.y + 10f), jawbreaker.jawbreakerHomingRotation, (float)jawbreaker.jawbreakerHomingHP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelJawbreaker;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Jawbreaker;
		this.setWaitTime = 3f;
		for (int i = 0; i < jawbreaker.jawbreakerMinis; i++)
		{
			this.setWaitTime += 0.5f;
		}
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
	public void StartBaronessShoot()
	{
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0008DCFC File Offset: 0x0008BEFC
	public IEnumerator shoot_cr()
	{
		this.state = BaronessLevelCastle.State.Idle;
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		string[] pattern = p.timeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.timeIndex = Random.Range(0, pattern.Length);
		Collider2D collider = this.baronessPhase1.shootPoint.GetComponent<Collider2D>();
		for (;;)
		{
			float timeShoot;
			Parser.FloatTryParse(pattern[this.timeIndex], out timeShoot);
			yield return CupheadTime.WaitForSeconds(this, timeShoot);
			this.baronessPhase1.shotEnough = false;
			if (this.castleOpen)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
			}
			this.baronessPoppedUp = true;
			AudioManager.Play("level_baroness_stick_head_open");
			this.baronessPhase1.animator.Play("Baroness_Pop_Up");
			while (this.baronessPoppedUp)
			{
				collider.enabled = true;
				if ((float)this.baronessPhase1.shootCounter >= p.attackCount.RandomFloat() || this.baronessPhase1.shotEnough)
				{
					break;
				}
				this.baronessPhase1.animator.SetTrigger("ToShoot");
				yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
				yield return null;
			}
			this.baronessPoppedUp = false;
			collider.enabled = false;
			this.baronessPhase1.shootCounter = 0;
			AudioManager.Play("level_baroness_stick_head_closed");
			this.baronessPhase1.animator.SetTrigger("Leave");
			if (this.timeIndex < pattern.Length - 1)
			{
				this.timeIndex++;
			}
			else
			{
				this.timeIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0008DD18 File Offset: 0x0008BF18
	public IEnumerator shoot_easy_cr()
	{
		float t = 0f;
		this.baronessPhase1.isEasyFinal = true;
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		Collider2D collider = this.baronessPhase1.shootPoint.GetComponent<Collider2D>();
		this.baronessPoppedUp = true;
		AudioManager.Play("level_baroness_stick_head_open");
		this.baronessPhase1.animator.Play("Baroness_Pop_Up");
		collider.enabled = true;
		if (this.castleOpen)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
		}
		while (this.baronessPhase1.isEasyFinal)
		{
			this.baronessPhase1.animator.SetTrigger("ToShoot");
			while (t < p.attackDelay && this.baronessPhase1.isEasyFinal)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		this.StartDeathEasy();
		yield break;
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0000D2BF File Offset: 0x0000B4BF
	public void StartJellybeans()
	{
		base.StartCoroutine(this.spawnJellybeans_cr());
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0008DD34 File Offset: 0x0008BF34
	public void SpawnJellyBeans(BaronessLevelJellybeans prefab)
	{
		Vector3 position = this.emergePoint.position;
		this.emergePoint.position = position;
		position.y = this.emergePoint.position.y - 20f;
		LevelProperties.Baroness.Jellybeans jellybeans = base.properties.CurrentState.jellybeans;
		prefab.Create(base.properties.CurrentState.jellybeans, position, jellybeans.movementSpeed, (float)jellybeans.HP);
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0008DDC0 File Offset: 0x0008BFC0
	public IEnumerator spawnJellybeans_cr()
	{
		LevelProperties.Baroness.Jellybeans p = base.properties.CurrentState.jellybeans;
		string[] typePattern = p.typeArray.GetRandom<string>().Split(new char[]
		{
			','
		});
		float change = 0f;
		while (this.state != BaronessLevelCastle.State.ChaseIntro)
		{
			for (int i = 0; i < typePattern.Length; i++)
			{
				BaronessLevelJellybeans toSpawn = null;
				float beanSpawnDelay = p.spawnDelay.RandomFloat();
				if (typePattern[i][0] == 'R')
				{
					toSpawn = this.greenJellyPrefab;
				}
				else if (typePattern[i][0] == 'P')
				{
					toSpawn = this.pinkJellyPrefab;
				}
				if ((BaronessLevelCastle.CURRENT_MINI_BOSS != null && this.state == BaronessLevelCastle.State.Idle) || this.state == BaronessLevelCastle.State.EasyFinal)
				{
					this.SpawnJellyBeans(toSpawn);
					yield return CupheadTime.WaitForSeconds(this, beanSpawnDelay - change);
				}
				else
				{
					yield return null;
				}
				if (this.jellyChangeDelay)
				{
					change += beanSpawnDelay - beanSpawnDelay * (1f - p.spawnDelayChangePercentage / 100f);
					this.jellyChangeDelay = false;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0000D2CE File Offset: 0x0000B4CE
	public void StartDeathEasy()
	{
		this.StopAllCoroutines();
		this.state = BaronessLevelCastle.State.Dead;
		base.StartCoroutine(this.death_easy_cr());
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0008DDDC File Offset: 0x0008BFDC
	public IEnumerator death_easy_cr()
	{
		float offset = 100f;
		float speed = 400f;
		if (!this.baronessPoppedUp)
		{
			Vector3 position = this.baronessPhase1.transform.position;
			position.y -= offset;
			this.baronessPhase1.transform.position = position;
		}
		this.baronessPhase1.animator.SetTrigger("Death");
		base.animator.SetTrigger("DeathEasy");
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.properties.WinInstantly();
		if (!this.baronessPoppedUp)
		{
			while (this.baronessPhase1.transform.position != this.originalBaronessPoint)
			{
				this.baronessPhase1.transform.position = Vector3.MoveTowards(this.baronessPhase1.transform.position, this.originalBaronessPoint, speed * CupheadTime.Delta);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0000D2EA File Offset: 0x0000B4EA
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0008DDF8 File Offset: 0x0008BFF8
	public IEnumerator death_cr()
	{
		this.pauseScrolling = true;
		this.teethState = BaronessLevelCastle.TeethState.Off;
		base.animator.SetTrigger("Death");
		Vector3 pos = this.castleWallFix.transform.position;
		pos.y = -45f;
		this.castleWallFix.transform.position = pos;
		this.castleWallFix.sortingLayerName = "Background";
		this.castleWallFix.sortingOrder = 15;
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				if (this.scrollForce != null)
				{
					levelPlayerController.motor.RemoveForce(this.scrollForce);
				}
			}
		}
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		base.animator.Play("Castle_Death");
		yield return null;
		yield break;
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x0000D2FF File Offset: 0x0000B4FF
	public void StartChase()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.chase_intro_cr());
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0008DE14 File Offset: 0x0008C014
	public IEnumerator chase_intro_cr()
	{
		this.baronessPhase1.transformCounter = 0;
		if (this.baronessPoppedUp)
		{
			this.baronessPhase1.animator.SetTrigger("Leave");
			this.baronessPhase1.animator.WaitForAnimationToEnd(this, "Baroness_Leave", false, true);
			this.baronessPoppedUp = false;
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		this.baronessPhase2.gameObject.SetActive(true);
		this.baronessPhase1.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		this.baronessPhase1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
		this.baronessPhase1.animator.Play("Baroness_To_Idle_1");
		while (this.baronessPhase1.transformCounter <= 2)
		{
			yield return null;
		}
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_1", false, true);
		this.baronessPhase1.transformCounter = 0;
		while (this.baronessPhase1.transformCounter <= 2 && this.continueTransition)
		{
			this.inAnimationLoop = true;
			yield return null;
		}
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_2_Loop", false, true);
		this.baronessPhase1.animator.SetTrigger("OnCandyCaneExit");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_3", false, true);
		base.animator.SetTrigger("StartPhase2");
		this.baronessPhase1.transformCounter = 0;
		AudioManager.Play("level_baroness_grab_castle");
		while (this.transitionCounter <= 4)
		{
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Transition_11", false, true);
		base.animator.SetTrigger("LoopArms");
		this.castleWallFix.sortingLayerName = "Default";
		this.baronessPhase2.gameObject.SetActive(true);
		this.castleCollidePhase2.SetActive(true);
		this.state = BaronessLevelCastle.State.Chase;
		base.StartCoroutine(this.handle_scroll_cr());
		base.StartCoroutine(this.peppermint_cr());
		base.StartCoroutine(this.final_shoot_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0000D314 File Offset: 0x0000B514
	public void PauseScroll()
	{
		this.pauseScrolling = !this.pauseScrolling;
		this.activateForce = !this.activateForce;
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x0000D334 File Offset: 0x0000B534
	public void ActivateTeeth()
	{
		base.animator.Play("Castle_Chase_Arms", 2);
		this.teethState = BaronessLevelCastle.TeethState.Idle;
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x0000D34E File Offset: 0x0000B54E
	public void HitCastleFrame()
	{
		if (this.inAnimationLoop)
		{
			this.continueTransition = true;
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x0000D362 File Offset: 0x0000B562
	public void TransitionCounter()
	{
		this.transitionCounter++;
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0008DE30 File Offset: 0x0008C030
	public void SwitchLayersToDefault()
	{
		this.baronessPhase2.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.baronessPhase2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		this.castlePhase2TopLayer.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.castlePhase2TopLayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0008DEF4 File Offset: 0x0008C0F4
	public IEnumerator handle_scroll_cr()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, 190f);
		for (;;)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
				if (!(levelPlayerController == null))
				{
					if (this.distToGround < 200f && this.activateForce)
					{
						levelPlayerController.motor.AddForce(this.scrollForce);
					}
					else
					{
						levelPlayerController.motor.RemoveForce(this.scrollForce);
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0000D372 File Offset: 0x0000B572
	public void HandsSound()
	{
		AudioManager.Play("level_baroness_castle_hands");
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x0000D37E File Offset: 0x0000B57E
	public void CastleRoar()
	{
		AudioManager.Play("level_baroness_castle_roar");
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0000D38A File Offset: 0x0000B58A
	public void PointSound()
	{
		AudioManager.Play("level_baroness_go_castle");
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x0008DF10 File Offset: 0x0008C110
	public IEnumerator peppermint_cr()
	{
		for (;;)
		{
			float seconds = base.properties.CurrentState.peppermint.peppermintSpawnDurationRange.RandomFloat();
			yield return CupheadTime.WaitForSeconds(this, seconds);
			this.teethState = BaronessLevelCastle.TeethState.StartOpen;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0000D396 File Offset: 0x0000B596
	public void OpenTeeth()
	{
		base.StartCoroutine(this.open_teeth_cr());
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x0008DF2C File Offset: 0x0008C12C
	public IEnumerator open_teeth_cr()
	{
		if (this.teethState == BaronessLevelCastle.TeethState.StartOpen)
		{
			this.teethState = BaronessLevelCastle.TeethState.Open;
			base.animator.SetBool("TeethOpen", true);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			BaronessLevelPeppermint peppermint = Object.Instantiate<BaronessLevelPeppermint>(this.peppermintPrefab);
			LevelProperties.Baroness.Peppermint p = base.properties.CurrentState.peppermint;
			peppermint.Init(this.emergePoint.position, p.peppermintSpeed);
			yield return CupheadTime.WaitForSeconds(this, 0.5f);
			this.teethState = BaronessLevelCastle.TeethState.Off;
		}
		yield break;
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x0000D3A5 File Offset: 0x0000B5A5
	public void CloseTeeth()
	{
		if (this.teethState == BaronessLevelCastle.TeethState.Off)
		{
			base.animator.SetBool("TeethOpen", false);
			this.teethState = BaronessLevelCastle.TeethState.Idle;
		}
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x0000D3CB File Offset: 0x0000B5CB
	public void HideTeeth()
	{
		this.teeth.enabled = false;
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x0000D3D9 File Offset: 0x0000B5D9
	public void ShowTeeth()
	{
		this.teeth.enabled = true;
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x0008DF48 File Offset: 0x0008C148
	public IEnumerator final_shoot_cr()
	{
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		string[] headString = p.finalProjectileHeadToss.Split(new char[]
		{
			','
		});
		int headIndex = Random.Range(0, headString.Length);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.baronessVonBonbon.finalProjectileInitialDelay);
		for (;;)
		{
			if (headString[headIndex][0] == 'H')
			{
				base.animator.SetBool("Toss", true);
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.baronessVonBonbon.finalProjectileAttackDelayRange.RandomFloat());
			}
			else
			{
				string[] delayString = headString[headIndex].Split(new char[]
				{
					':'
				});
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(delayString[1]));
			}
			headIndex = (headIndex + 1) % headString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x0000D3E7 File Offset: 0x0000B5E7
	public void FireHead()
	{
		this.baronessPhase1.FireFinalProjectile();
		base.animator.SetBool("Toss", false);
	}

	// Token: 0x04000C95 RID: 3221
	public bool pauseScrolling;

	// Token: 0x04000C99 RID: 3225
	public Transform[] homingRoots;

	// Token: 0x04000C9A RID: 3226
	[SerializeField]
	public SpriteRenderer teeth;

	// Token: 0x04000C9B RID: 3227
	[SerializeField]
	public SpriteRenderer blink;

	// Token: 0x04000C9C RID: 3228
	[SerializeField]
	public BaronessLevelPlatform platform;

	// Token: 0x04000C9D RID: 3229
	[SerializeField]
	public BaronessLevelBaroness baronessPhase1;

	// Token: 0x04000C9E RID: 3230
	[SerializeField]
	public Transform baronessPhase2;

	// Token: 0x04000C9F RID: 3231
	[SerializeField]
	public BaronessLevelPeppermint peppermintPrefab;

	// Token: 0x04000CA0 RID: 3232
	[SerializeField]
	public Transform blackCastleHole;

	// Token: 0x04000CA1 RID: 3233
	[SerializeField]
	public Transform castlePhase2TopLayer;

	// Token: 0x04000CA2 RID: 3234
	[SerializeField]
	public BaronessLevelCupcake cupcakePrefab;

	// Token: 0x04000CA3 RID: 3235
	[SerializeField]
	public BaronessLevelWaffle wafflePrefab;

	// Token: 0x04000CA4 RID: 3236
	[SerializeField]
	public BaronessLevelGumball gumballPrefab;

	// Token: 0x04000CA5 RID: 3237
	[SerializeField]
	public BaronessLevelJawbreaker jawBreakerPrefab;

	// Token: 0x04000CA6 RID: 3238
	[SerializeField]
	public BaronessLevelCandyCorn candyCornPrefab;

	// Token: 0x04000CA7 RID: 3239
	[SerializeField]
	public BaronessLevelJellybeans greenJellyPrefab;

	// Token: 0x04000CA8 RID: 3240
	[SerializeField]
	public BaronessLevelJellybeans pinkJellyPrefab;

	// Token: 0x04000CA9 RID: 3241
	[SerializeField]
	public Transform emergePoint;

	// Token: 0x04000CAA RID: 3242
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000CAB RID: 3243
	[SerializeField]
	public GameObject castleCollidePhase2;

	// Token: 0x04000CAC RID: 3244
	[SerializeField]
	public SpriteRenderer castleWallFix;

	// Token: 0x04000CAD RID: 3245
	public int bossIndex;

	// Token: 0x04000CAE RID: 3246
	public int timeIndex;

	// Token: 0x04000CAF RID: 3247
	public int transitionCounter;

	// Token: 0x04000CB0 RID: 3248
	public int blinkCounter;

	// Token: 0x04000CB1 RID: 3249
	public int blinkCounterMax;

	// Token: 0x04000CB2 RID: 3250
	public float setWaitTime;

	// Token: 0x04000CB3 RID: 3251
	public float distToGround;

	// Token: 0x04000CB4 RID: 3252
	public bool maxMiniBosses;

	// Token: 0x04000CB5 RID: 3253
	public bool castleOpen;

	// Token: 0x04000CB6 RID: 3254
	public bool baronessPoppedUp;

	// Token: 0x04000CB7 RID: 3255
	public bool continueTransition;

	// Token: 0x04000CB8 RID: 3256
	public bool inAnimationLoop;

	// Token: 0x04000CB9 RID: 3257
	public bool jellyChangeDelay;

	// Token: 0x04000CBA RID: 3258
	public bool openTeeth;

	// Token: 0x04000CBB RID: 3259
	public bool activateForce = true;

	// Token: 0x04000CBC RID: 3260
	public Vector3 originalEmergePos;

	// Token: 0x04000CBD RID: 3261
	public Vector3 originalBaronessPoint;

	// Token: 0x04000CBE RID: 3262
	public AbstractPlayerController player;

	// Token: 0x04000CBF RID: 3263
	public LevelPlayerMotor.VelocityManager.Force scrollForce;

	// Token: 0x04000CC0 RID: 3264
	public DamageReceiver damageReceiver;

	// Token: 0x04000CC1 RID: 3265
	public DamageDealer damageDealer;

	// Token: 0x020009F9 RID: 2553
	public enum State
	{
		// Token: 0x040049F8 RID: 18936
		Intro,
		// Token: 0x040049F9 RID: 18937
		Idle,
		// Token: 0x040049FA RID: 18938
		ChaseIntro,
		// Token: 0x040049FB RID: 18939
		Chase,
		// Token: 0x040049FC RID: 18940
		Open,
		// Token: 0x040049FD RID: 18941
		Dead,
		// Token: 0x040049FE RID: 18942
		EasyFinal
	}

	// Token: 0x020009FA RID: 2554
	public enum TeethState
	{
		// Token: 0x04004A00 RID: 18944
		Unspawned,
		// Token: 0x04004A01 RID: 18945
		Idle,
		// Token: 0x04004A02 RID: 18946
		Off,
		// Token: 0x04004A03 RID: 18947
		StartOpen,
		// Token: 0x04004A04 RID: 18948
		Open
	}

	// Token: 0x020009FB RID: 2555
	public enum BossPossibility
	{
		// Token: 0x04004A06 RID: 18950
		Gumball = 1,
		// Token: 0x04004A07 RID: 18951
		Waffle,
		// Token: 0x04004A08 RID: 18952
		CandyCorn,
		// Token: 0x04004A09 RID: 18953
		Cupcake,
		// Token: 0x04004A0A RID: 18954
		Jawbreaker
	}
}
