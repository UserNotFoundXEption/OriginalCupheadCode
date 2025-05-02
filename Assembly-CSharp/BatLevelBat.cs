using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class BatLevelBat : LevelProperties.Bat.Entity
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06001085 RID: 4229 RVA: 0x0000DF30 File Offset: 0x0000C130
	// (set) Token: 0x06001086 RID: 4230 RVA: 0x0000DF38 File Offset: 0x0000C138
	public BatLevelBat.State state { get; set; }

	// Token: 0x06001087 RID: 4231 RVA: 0x000905B4 File Offset: 0x0008E7B4
	public override void LevelInit(LevelProperties.Bat properties)
	{
		base.LevelInit(properties);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.speed = properties.CurrentState.movement.movementSpeed;
		this.originalSpeed = this.speed;
		this.inMovingPhase = true;
		this.moving = true;
		this.startPosition = base.transform.position;
		this.startPosition.y = properties.CurrentState.movement.startPosY;
		base.transform.position = this.startPosition;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Left, base.transform);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x00090684 File Offset: 0x0008E884
	public IEnumerator intro_cr()
	{
		LevelProperties.Bat.State p = base.properties.CurrentState;
		this.anglePattern = p.batBouncer.bounceAngleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.angleIndex = Random.Range(0, this.anglePattern.Length);
		yield return CupheadTime.WaitForSeconds(this, 5f);
		base.animator.SetTrigger("OnIntro");
		base.StartCoroutine(this.bat_movement_cr());
		this.state = BatLevelBat.State.Idle;
		yield break;
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0000DF41 File Offset: 0x0000C141
	public void Die()
	{
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0000DF53 File Offset: 0x0000C153
	public void Update()
	{
		this.damageDealer.Update();
		if (this.state != BatLevelBat.State.Phase2)
		{
			this.VaryingSpeed();
		}
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x0000DF72 File Offset: 0x0000C172
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0000DF89 File Offset: 0x0000C189
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x000906A0 File Offset: 0x0008E8A0
	public void OnTurnAnimComplete()
	{
		base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x000906E8 File Offset: 0x0008E8E8
	public IEnumerator bat_movement_cr()
	{
		float offset = 200f;
		float stopDist = 100f;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			if (this.direction == BatLevelBat.Direction.Left)
			{
				while (base.transform.position.x > -640f + offset)
				{
					if (this.moving)
					{
						float num = -640f + offset - base.transform.position.x;
						num = Mathf.Abs(num);
						pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + offset, this.speed * CupheadTime.Delta);
						if (num < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				if (!this.inMovingPhase)
				{
					break;
				}
				this.direction = BatLevelBat.Direction.Right;
				yield return null;
			}
			else if (this.direction == BatLevelBat.Direction.Right)
			{
				while (base.transform.position.x < 640f - offset)
				{
					if (this.moving)
					{
						float num2 = 640f - offset - base.transform.position.x;
						num2 = Mathf.Abs(num2);
						pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, this.speed * CupheadTime.Delta);
						if (num2 < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				if (!this.inMovingPhase)
				{
					goto IL_336;
				}
				this.direction = BatLevelBat.Direction.Left;
				yield return null;
			}
			yield return null;
		}
		this.onRight = false;
		base.StartCoroutine(this.phase_2_handler_cr());
		goto IL_399;
		IL_336:
		this.onRight = true;
		base.StartCoroutine(this.phase_2_handler_cr());
		IL_399:
		yield break;
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x00090704 File Offset: 0x0008E904
	public void VaryingSpeed()
	{
		float num = 10f;
		if (this.slowDown)
		{
			if (this.speed <= 50f)
			{
				this.slowDown = false;
			}
			else
			{
				this.speed -= num;
			}
		}
		else if (this.speed < this.originalSpeed)
		{
			this.speed += num;
		}
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x0000DF9C File Offset: 0x0000C19C
	public void StartBouncer()
	{
		if (this.pattern != null)
		{
			base.StopCoroutine(this.pattern);
		}
		this.pattern = base.StartCoroutine(this.bouncer_cr());
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x00090770 File Offset: 0x0008E970
	public void SpawnBouncer()
	{
		float num = 0f;
		Parser.FloatTryParse(this.anglePattern[this.angleIndex], out num);
		num = ((this.direction != BatLevelBat.Direction.Right) ? num : (num + 90f));
		BatLevelBouncer batLevelBouncer = Object.Instantiate<BatLevelBouncer>(this.bouncerPrefab);
		batLevelBouncer.Init(base.properties.CurrentState.batBouncer, this.bouncerRoot.position, num);
		this.angleIndex = (this.angleIndex + 1) % this.anglePattern.Length;
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x000907FC File Offset: 0x0008E9FC
	public IEnumerator bouncer_cr()
	{
		LevelProperties.Bat.BatBouncer p = base.properties.CurrentState.batBouncer;
		this.state = BatLevelBat.State.Bouncer;
		this.moving = false;
		yield return CupheadTime.WaitForSeconds(this, p.stopDelay);
		this.SpawnBouncer();
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = BatLevelBat.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0000DFC7 File Offset: 0x0000C1C7
	public void StartGoblin()
	{
		base.StartCoroutine(this.goblin_cr());
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x00090818 File Offset: 0x0008EA18
	public IEnumerator goblin_cr()
	{
		LevelProperties.Bat.Goblins p = base.properties.CurrentState.goblins;
		string[] delayPattern = p.appearDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] entrancePattern = p.entranceString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayPattern.Length);
		int entranceIndex = Random.Range(0, entrancePattern.Length);
		int counter = 0;
		float delay = 0f;
		float startX = 0f;
		float pickShooter = (float)p.shooterOccuranceRange.RandomInt();
		float startY = (float)Level.Current.Ground + 100f;
		bool isShooter = false;
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (entrancePattern[entranceIndex][0] == 'R')
			{
				startX = 640f;
				Vector2 startPos = new Vector2(startX, startY);
				this.SpawnGoblin(false, startPos, isShooter);
			}
			else if (entrancePattern[entranceIndex][0] == 'L')
			{
				startX = -640f;
				Vector2 startPos = new Vector2(startX, startY);
				this.SpawnGoblin(true, startPos, isShooter);
			}
			isShooter = false;
			counter++;
			if ((float)counter == pickShooter)
			{
				isShooter = true;
				counter = 0;
			}
			entranceIndex = (entranceIndex + 1) % entrancePattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x00090834 File Offset: 0x0008EA34
	public void SpawnGoblin(bool leftSide, Vector2 startPos, bool isShooter)
	{
		LevelProperties.Bat.Goblins goblins = base.properties.CurrentState.goblins;
		BatLevelGoblin batLevelGoblin = Object.Instantiate<BatLevelGoblin>(this.goblinPrefab);
		batLevelGoblin.Init(goblins, startPos, leftSide, isShooter, goblins.HP);
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0000DFD6 File Offset: 0x0000C1D6
	public void StartLightning()
	{
		if (this.pattern != null)
		{
			base.StopCoroutine(this.pattern);
		}
		this.pattern = base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0000E001 File Offset: 0x0000C201
	public void SpawnCloud(Vector2 startPos)
	{
		this.lightning = Object.Instantiate<BatLevelLightning>(this.lightningPrefab);
		this.lightning.Init(base.properties.CurrentState.batLightning, startPos);
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00090870 File Offset: 0x0008EA70
	public IEnumerator lightning_cr()
	{
		this.state = BatLevelBat.State.Lightning;
		this.moving = false;
		LevelProperties.Bat.BatLightning p = base.properties.CurrentState.batLightning;
		string[] offsetString = p.centerOffset.GetRandom<string>().Split(new char[]
		{
			','
		});
		int offsetIndex = 0;
		float offset = 0f;
		Vector2 pos = Vector2.zero;
		pos.y = p.cloudHeight;
		int num = 0;
		while ((float)num < p.cloudCount)
		{
			Parser.FloatTryParse(offsetString[offsetIndex], out offset);
			pos.x = p.cloudDistance * (float)num + offset - (float)(Level.Current.Right / 2);
			this.SpawnCloud(pos);
			offsetIndex %= offsetString.Length;
			num++;
		}
		while (this.lightning != null)
		{
			yield return null;
		}
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = BatLevelBat.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0000E030 File Offset: 0x0000C230
	public void StartPhase2()
	{
		this.inMovingPhase = false;
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0009088C File Offset: 0x0008EA8C
	public IEnumerator phase_2_handler_cr()
	{
		float yPos = base.transform.position.y - 100f;
		this.state = BatLevelBat.State.Phase2;
		this.speed = this.originalSpeed;
		while (base.transform.position.y != yPos)
		{
			Vector3 pos = base.transform.position;
			pos.y = Mathf.MoveTowards(base.transform.position.y, yPos, this.speed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.StartMiniBats();
		this.StartPentagram();
		this.StartCross();
		yield return null;
		yield break;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0000E039 File Offset: 0x0000C239
	public void StartMiniBats()
	{
		base.StartCoroutine(this.mini_bats_cr());
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x000908A8 File Offset: 0x0008EAA8
	public void SpawnMiniBat(float angle)
	{
		LevelProperties.Bat.MiniBats miniBats = base.properties.CurrentState.miniBats;
		float rotation = (!this.onRight) ? angle : (-angle);
		float velocity = (!this.onRight) ? miniBats.speedX : (-miniBats.speedX);
		this.minibatPrefab.Create(this.coffinRoot.position, rotation, velocity, miniBats.speedY, miniBats.yMinMax, miniBats.HP);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x00090934 File Offset: 0x0008EB34
	public IEnumerator mini_bats_cr()
	{
		LevelProperties.Bat.MiniBats p = base.properties.CurrentState.miniBats;
		string[] angleString = p.batAngleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = Random.Range(0, angleString.Length);
		float angle = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			Parser.FloatTryParse(angleString[angleIndex], out angle);
			this.SpawnMiniBat(angle);
			yield return CupheadTime.WaitForSeconds(this, p.delay);
			angleIndex = (angleIndex + 1) % angleString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0000E048 File Offset: 0x0000C248
	public void StartPentagram()
	{
		base.StartCoroutine(this.pentagram_cr());
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x00090950 File Offset: 0x0008EB50
	public void SpawnPentagram()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 position = this.pentagramRoot.position;
		position.y = (float)Level.Current.Ground - 10f;
		BatLevelPentagram batLevelPentagram = Object.Instantiate<BatLevelPentagram>(this.pentagramPrefab);
		batLevelPentagram.Init(position, base.properties.CurrentState.pentagrams, next, this.onRight);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000909B8 File Offset: 0x0008EBB8
	public IEnumerator pentagram_cr()
	{
		LevelProperties.Bat.Pentagrams p = base.properties.CurrentState.pentagrams;
		string[] delayString = p.pentagramDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		float delay = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			this.SpawnPentagram();
			delayIndex %= delayString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0000E057 File Offset: 0x0000C257
	public void StartCross()
	{
		base.StartCoroutine(this.cross_cr());
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x000909D4 File Offset: 0x0008EBD4
	public void SpawnCross(int count)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.cross = Object.Instantiate<BatLevelCross>(this.crossPrefab);
		this.cross.Init(this.bouncerRoot.position, base.properties.CurrentState.crossToss, count, next);
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x00090A28 File Offset: 0x0008EC28
	public IEnumerator cross_cr()
	{
		LevelProperties.Bat.CrossToss p = base.properties.CurrentState.crossToss;
		string[] delayString = p.crossDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] countString = p.attackCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		int countIndex = Random.Range(0, countString.Length);
		float delay = 0f;
		int count = 0;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			if (this.cross == null)
			{
				Parser.FloatTryParse(delayString[delayIndex], out delay);
				Parser.IntTryParse(countString[countIndex], out count);
				yield return CupheadTime.WaitForSeconds(this, delay);
				this.SpawnCross(count);
			}
			delayIndex %= delayString.Length;
			count %= countString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0000E066 File Offset: 0x0000C266
	public void StartPhase3()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.soul_cr());
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00090A44 File Offset: 0x0008EC44
	public IEnumerator shoot_cr()
	{
		LevelProperties.Bat.WolfFire p = base.properties.CurrentState.wolfFire;
		AbstractPlayerController player = PlayerManager.GetNext();
		float counter = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Wolf)
		{
			yield return CupheadTime.WaitForSeconds(this, p.bulletDelay);
			this.ShootBullet(p.bulletSpeed, player);
			counter += 1f;
			if (counter >= p.bulletAimCount)
			{
				player = PlayerManager.GetNext();
				counter = 0f;
			}
		}
		yield break;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00090A60 File Offset: 0x0008EC60
	public void ShootBullet(float speed, AbstractPlayerController player)
	{
		float num = player.transform.position.x - this.bouncerRoot.position.x;
		float num2 = player.transform.position.y - this.bouncerRoot.position.y;
		float rotation = Mathf.Atan2(num2, num) * 57.29578f;
		this.wolfProjectile.Create(this.bouncerRoot.position, rotation, speed);
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x00090AF0 File Offset: 0x0008ECF0
	public void SpawnSoul()
	{
		LevelProperties.Bat.WolfSoul wolfSoul = base.properties.CurrentState.wolfSoul;
		AbstractPlayerController next = PlayerManager.GetNext();
		BatLevelHomingSoul batLevelHomingSoul = Object.Instantiate<BatLevelHomingSoul>(this.soulPrefab);
		batLevelHomingSoul.Init(this.bouncerRoot.position, next, wolfSoul);
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x00090B38 File Offset: 0x0008ED38
	public IEnumerator soul_cr()
	{
		this.SpawnSoul();
		yield return null;
		yield break;
	}

	// Token: 0x04000D76 RID: 3446
	[SerializeField]
	public Transform bouncerRoot;

	// Token: 0x04000D77 RID: 3447
	[SerializeField]
	public Transform coffinRoot;

	// Token: 0x04000D78 RID: 3448
	[SerializeField]
	public Transform pentagramRoot;

	// Token: 0x04000D79 RID: 3449
	[SerializeField]
	public BatLevelBouncer bouncerPrefab;

	// Token: 0x04000D7A RID: 3450
	[SerializeField]
	public BatLevelGoblin goblinPrefab;

	// Token: 0x04000D7B RID: 3451
	[SerializeField]
	public BatLevelMiniBat minibatPrefab;

	// Token: 0x04000D7C RID: 3452
	[SerializeField]
	public BatLevelLightning lightningPrefab;

	// Token: 0x04000D7D RID: 3453
	public BatLevelLightning lightning;

	// Token: 0x04000D7E RID: 3454
	[SerializeField]
	public BatLevelPentagram pentagramPrefab;

	// Token: 0x04000D7F RID: 3455
	[SerializeField]
	public BasicProjectile wolfProjectile;

	// Token: 0x04000D80 RID: 3456
	[SerializeField]
	public BatLevelCross crossPrefab;

	// Token: 0x04000D81 RID: 3457
	public BatLevelCross cross;

	// Token: 0x04000D82 RID: 3458
	[SerializeField]
	public BatLevelHomingSoul soulPrefab;

	// Token: 0x04000D83 RID: 3459
	public BatLevelBat.Direction direction = BatLevelBat.Direction.Left;

	// Token: 0x04000D85 RID: 3461
	public DamageDealer damageDealer;

	// Token: 0x04000D86 RID: 3462
	public bool slowDown;

	// Token: 0x04000D87 RID: 3463
	public bool onRight;

	// Token: 0x04000D88 RID: 3464
	public bool inMovingPhase;

	// Token: 0x04000D89 RID: 3465
	public bool moving;

	// Token: 0x04000D8A RID: 3466
	public string[] anglePattern;

	// Token: 0x04000D8B RID: 3467
	public int angleIndex;

	// Token: 0x04000D8C RID: 3468
	public float speed;

	// Token: 0x04000D8D RID: 3469
	public float originalSpeed;

	// Token: 0x04000D8E RID: 3470
	public Vector3 startPosition;

	// Token: 0x04000D8F RID: 3471
	public Coroutine pattern;

	// Token: 0x02000A49 RID: 2633
	public enum Direction
	{
		// Token: 0x04004BC5 RID: 19397
		Right,
		// Token: 0x04004BC6 RID: 19398
		Left
	}

	// Token: 0x02000A4A RID: 2634
	public enum State
	{
		// Token: 0x04004BC8 RID: 19400
		Idle,
		// Token: 0x04004BC9 RID: 19401
		Bouncer,
		// Token: 0x04004BCA RID: 19402
		Lightning,
		// Token: 0x04004BCB RID: 19403
		Phase2
	}
}
