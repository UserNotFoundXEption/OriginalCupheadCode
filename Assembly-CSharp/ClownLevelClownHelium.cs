using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class ClownLevelClownHelium : LevelProperties.Clown.Entity
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001392 RID: 5010 RVA: 0x000106E9 File Offset: 0x0000E8E9
	// (set) Token: 0x06001393 RID: 5011 RVA: 0x000106F1 File Offset: 0x0000E8F1
	public ClownLevelClownHelium.State state { get; set; }

	// Token: 0x06001394 RID: 5012 RVA: 0x00098164 File Offset: 0x00096364
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = this.head.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.head.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x000106FA File Offset: 0x0000E8FA
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.pivotPoint.transform.position = this.heliumStopPos.transform.position;
		this.headMoving = true;
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x0001072A File Offset: 0x0000E92A
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0001073D File Offset: 0x0000E93D
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0001075E File Offset: 0x0000E95E
	public void StartHeliumTank()
	{
		this.StopAllCoroutines();
		this.state = ClownLevelClownHelium.State.Helium;
		base.StartCoroutine(this.helium_tank_intro_cr());
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x000981B0 File Offset: 0x000963B0
	public IEnumerator helium_tank_intro_cr()
	{
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		float t = 0f;
		float time = 5f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.heliumStopPos.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.heliumStopPos.position;
		base.StartCoroutine(this.helium_tank_cr());
		base.StartCoroutine(this.tank_effects_cr());
		base.StartCoroutine(this.pipe_puffs_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x000981CC File Offset: 0x000963CC
	public void SpawnBalloonDogs(ClownLevelDogBalloon dogPrefab, Vector3 startPos, bool isFlipped)
	{
		LevelProperties.Clown.HeliumClown heliumClown = base.properties.CurrentState.heliumClown;
		AbstractPlayerController next = PlayerManager.GetNext();
		if (dogPrefab != null)
		{
			ClownLevelDogBalloon clownLevelDogBalloon = Object.Instantiate<ClownLevelDogBalloon>(dogPrefab);
			clownLevelDogBalloon.Init(heliumClown.dogHP, startPos, heliumClown.dogSpeed, next, heliumClown, isFlipped);
		}
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x0001077A File Offset: 0x0000E97A
	public void HeliumTankSFX()
	{
		AudioManager.Play("clown_helium_tanks");
		this.emitAudioFromObject.Add("clown_helium_tanks");
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x00098220 File Offset: 0x00096420
	public IEnumerator helium_tank_cr()
	{
		this.emitAudioFromObject.Add("clown_helium_tanks");
		AudioManager.Play("clown_helium_intro_continue");
		this.emitAudioFromObject.Add("clown_helium_intro_continue");
		AudioManager.Play("clown_helium_extend_pipes");
		this.emitAudioFromObject.Add("clown_helium_extend_pipes");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Helium_Intro_End", 3, false, true);
		LevelProperties.Clown.HeliumClown p = base.properties.CurrentState.heliumClown;
		string[] spawnPattern = p.dogSpawnOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.dogDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.dogTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 pickedPipePos = Vector3.zero;
		float waitTime = 0f;
		bool isFlipped = false;
		int spawnIndex = Random.Range(0, spawnPattern.Length);
		int delayIndex = Random.Range(0, delayPattern.Length);
		int typeIndex = Random.Range(0, typePattern.Length);
		for (;;)
		{
			ClownLevelDogBalloon toSpawn = null;
			string[] nextPos = spawnPattern[spawnIndex].Split(new char[]
			{
				'-'
			});
			foreach (string s in nextPos)
			{
				int pipeSelection;
				Parser.IntTryParse(s, out pipeSelection);
				foreach (ClownLevelClownHelium.PipePositions pipePositions in this.pipePositions)
				{
					if (pipePositions.orderNum == pipeSelection)
					{
						pickedPipePos = pipePositions.pipeEntrance.position;
						isFlipped = (pipeSelection > 3);
					}
				}
				if (typePattern[typeIndex][0] == 'R')
				{
					toSpawn = this.regularDog;
				}
				else if (typePattern[typeIndex][0] == 'P')
				{
					toSpawn = this.pinkDog;
				}
				this.SpawnBalloonDogs(toSpawn, pickedPipePos, isFlipped);
				typeIndex = (typeIndex + 1) % typePattern.Length;
			}
			Parser.FloatTryParse(delayPattern[delayIndex], out waitTime);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			spawnIndex = (spawnIndex + 1) % spawnPattern.Length;
			delayIndex = (delayIndex + 1) % delayPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x0009823C File Offset: 0x0009643C
	public IEnumerator pipe_puffs_cr()
	{
		string order = "0,5,1,4,2,3,5,1,2,3";
		int orderIndex = Random.Range(0, order.Split(new char[]
		{
			','
		}).Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.16f, 0.65f));
			this.pipePositions[Parser.IntParse(order.Split(new char[]
			{
				','
			})[orderIndex])].pipeEntrance.GetComponent<Animator>().SetInteger("Type", Random.Range(0, 3));
			this.pipePositions[Parser.IntParse(order.Split(new char[]
			{
				','
			})[orderIndex])].pipeEntrance.GetComponent<Animator>().SetTrigger("OnPuff");
			orderIndex = (orderIndex + 1) % order.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00098258 File Offset: 0x00096458
	public IEnumerator tank_effects_cr()
	{
		bool isRight = Rand.Bool();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.16f, 0.85f));
			this.tankEffects.SetBool("isLeft", isRight);
			this.tankEffects.SetTrigger("OnPuff");
			isRight = !isRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00010796 File Offset: 0x0000E996
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.regularDog = null;
		this.pinkDog = null;
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x00098274 File Offset: 0x00096474
	public void SetHead()
	{
		this.pivotPoint.transform.position = this.head.transform.position;
		this.head.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Head");
		base.StartCoroutine(this.head_moving_cr());
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000107AC File Offset: 0x0000E9AC
	public void SetBody()
	{
		base.animator.Play("Helium_Idle");
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000982D0 File Offset: 0x000964D0
	public IEnumerator head_moving_cr()
	{
		for (;;)
		{
			if (this.headMoving)
			{
				this.PathMovement();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x000982EC File Offset: 0x000964EC
	public void PathMovement()
	{
		this.angle += 1.8f * CupheadTime.Delta * this.hitPauseCoefficient();
		Vector3 vector;
		vector..ctor(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 vector2;
		vector2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		this.head.transform.position = this.pivotPoint.position;
		this.head.transform.position += vector + vector2;
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000107BE File Offset: 0x0000E9BE
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000983A4 File Offset: 0x000965A4
	public IEnumerator death_cr()
	{
		this.head.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.head_moving_cr());
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.head.transform.parent = null;
		base.StartCoroutine(this.head_death_cr());
		float moveSpeed = base.properties.CurrentState.heliumClown.heliumMoveSpeed;
		float acceleration = base.properties.CurrentState.heliumClown.heliumAcceleration;
		float endPos = -860f;
		while (base.transform.position.y > endPos)
		{
			moveSpeed += acceleration;
			base.transform.AddPosition(0f, -moveSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x000983C0 File Offset: 0x000965C0
	public IEnumerator head_death_cr()
	{
		this.StartExplosions();
		float moveSpeed = base.properties.CurrentState.heliumClown.heliumMoveSpeed;
		float acceleration = base.properties.CurrentState.heliumClown.heliumAcceleration;
		float endPos = 1060f;
		float t = 0f;
		float time = 1f;
		Vector2 start = this.head.transform.position;
		Vector2 end = new Vector3(this.head.transform.position.x, this.heliumStopPos.transform.position.y - 50f, 0f);
		this.headMoving = false;
		base.animator.SetTrigger("Dead");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.head.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		while (this.head.transform.position.y < endPos)
		{
			if (CupheadTime.Delta != 0f)
			{
				moveSpeed += acceleration;
				this.head.transform.AddPosition(0f, moveSpeed * CupheadTime.Delta, 0f);
			}
			yield return null;
		}
		this.EndExplosions();
		this.clownHorse.StartCarouselHorse();
		Object.Destroy(this.head.gameObject);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x000107D3 File Offset: 0x0000E9D3
	public void StartExplosions()
	{
		this.head.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x000107E5 File Offset: 0x0000E9E5
	public void EndExplosions()
	{
		this.head.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x04000FE3 RID: 4067
	[SerializeField]
	public Animator tankEffects;

	// Token: 0x04000FE4 RID: 4068
	[SerializeField]
	public ClownLevelClownHorse clownHorse;

	// Token: 0x04000FE5 RID: 4069
	[SerializeField]
	public GameObject head;

	// Token: 0x04000FE6 RID: 4070
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000FE7 RID: 4071
	[SerializeField]
	public Transform heliumStopPos;

	// Token: 0x04000FE8 RID: 4072
	[SerializeField]
	public ClownLevelClownHelium.PipePositions[] pipePositions;

	// Token: 0x04000FE9 RID: 4073
	[SerializeField]
	public ClownLevelDogBalloon regularDog;

	// Token: 0x04000FEA RID: 4074
	[SerializeField]
	public ClownLevelDogBalloon pinkDog;

	// Token: 0x04000FEB RID: 4075
	public DamageReceiver damageReceiver;

	// Token: 0x04000FEC RID: 4076
	public bool headMoving;

	// Token: 0x04000FED RID: 4077
	public float angle;

	// Token: 0x04000FEE RID: 4078
	public float loopSize = 10f;

	// Token: 0x02000AF6 RID: 2806
	public enum State
	{
		// Token: 0x04005070 RID: 20592
		BumperCar,
		// Token: 0x04005071 RID: 20593
		Helium,
		// Token: 0x04005072 RID: 20594
		Death
	}

	// Token: 0x02000AF7 RID: 2807
	[Serializable]
	public class PipePositions
	{
		// Token: 0x04005073 RID: 20595
		public Transform pipeEntrance;

		// Token: 0x04005074 RID: 20596
		public int orderNum;
	}
}
