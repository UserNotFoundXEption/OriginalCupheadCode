using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class ClownLevelClown : LevelProperties.Clown.Entity
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x0600137A RID: 4986 RVA: 0x00010567 File Offset: 0x0000E767
	// (set) Token: 0x0600137B RID: 4987 RVA: 0x0001056F File Offset: 0x0000E76F
	public ClownLevelClown.State state { get; set; }

	// Token: 0x0600137C RID: 4988 RVA: 0x00010578 File Offset: 0x0000E778
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x000105AE File Offset: 0x0000E7AE
	public void Start()
	{
		this.state = ClownLevelClown.State.BumperCar;
		this.notDashing = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x000105CB File Offset: 0x0000E7CB
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x000105D4 File Offset: 0x0000E7D4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x000105E7 File Offset: 0x0000E7E7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state != ClownLevelClown.State.Helium && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x00010611 File Offset: 0x0000E811
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x00010629 File Offset: 0x0000E829
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0001064A File Offset: 0x0000E84A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.regularDuck = null;
		this.pinkDuck = null;
		this.bombDuck = null;
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x00097F2C File Offset: 0x0009612C
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("clown_intro_continue");
		this.emitAudioFromObject.Add("clown_intro_continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		this.StartBumperCar();
		yield break;
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x00010667 File Offset: 0x0000E867
	public void StartBumperCar()
	{
		this.state = ClownLevelClown.State.BumperCar;
		base.animator.SetBool("BumperDeath", false);
		base.StartCoroutine(this.bumper_car_cr());
		base.StartCoroutine(this.ducks_cr());
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x0001069B File Offset: 0x0000E89B
	public void EndBumperCar()
	{
		base.animator.SetBool("BumperDeath", true);
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000106AE File Offset: 0x0000E8AE
	public void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		base.GetComponent<SpriteRenderer>().sortingOrder = 101;
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00097F48 File Offset: 0x00096148
	public IEnumerator end_bumper_car_cr()
	{
		AudioManager.Play("clown_bumper_death");
		this.emitAudioFromObject.Add("clown_bumper_death");
		while (base.transform.position.y > -660f)
		{
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position += new Vector2(-300f, this.fallAccumulatedGravity) * CupheadTime.Delta;
				this.fallAccumulatedGravity += -100f;
			}
			yield return null;
		}
		this.clownHelium.StartHeliumTank();
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x00097F64 File Offset: 0x00096164
	public IEnumerator dash_timer_cr(string[] delayPattern)
	{
		float waitTime;
		Parser.FloatTryParse(delayPattern[this.timerIndex], out waitTime);
		yield return CupheadTime.WaitForSeconds(this, waitTime);
		this.notDashing = false;
		this.timerIndex = (this.timerIndex + 1) % delayPattern.Length;
		yield return null;
		yield break;
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00097F88 File Offset: 0x00096188
	public IEnumerator bumper_car_cr()
	{
		this.notDashing = true;
		bool isFlipped = false;
		Vector3 bumperPos = base.transform.position;
		float offsetDash = 150f;
		float offsetMove = 250f;
		LevelProperties.Clown.BumperCar p = base.properties.CurrentState.bumperCar;
		string[] movementPattern = p.movementStrings.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] dashDelayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float t = 0f;
		float speed = p.movementSpeed;
		int movementIndex = Random.Range(0, movementPattern.Length);
		this.timerIndex = Random.Range(0, dashDelayPattern.Length);
		base.StartCoroutine(this.dash_timer_cr(dashDelayPattern));
		this.emitAudioFromObject.Add("clown_bumper_move");
		this.emitAudioFromObject.Add("clown_dash_start");
		this.emitAudioFromObject.Add("clown_dash_end");
		for (;;)
		{
			if (this.notDashing)
			{
				if (movementPattern[movementIndex][0] == 'F')
				{
					base.animator.SetTrigger("Move");
					while (t < p.movementDuration && this.notDashing && !this.stop)
					{
						speed = ((!isFlipped) ? (-p.movementSpeed) : p.movementSpeed);
						base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
						t += CupheadTime.Delta;
						yield return null;
					}
					AudioManager.Play("clown_bumper_move");
					if (this.notDashing)
					{
						yield return CupheadTime.WaitForSeconds(this, p.movementDelay);
					}
				}
				else if (movementPattern[movementIndex][0] == 'B')
				{
					base.animator.SetTrigger("Move");
					while (t < p.movementDuration && this.notDashing && !this.stop)
					{
						speed = ((!isFlipped) ? p.movementSpeed : (-p.movementSpeed));
						if (base.transform.position.x >= (float)Level.Current.Left + offsetDash && base.transform.position.x <= (float)Level.Current.Right - offsetDash)
						{
							base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
							t += CupheadTime.Delta;
							yield return null;
						}
						yield return null;
					}
					AudioManager.Play("clown_bumper_move");
					if (this.notDashing)
					{
						yield return CupheadTime.WaitForSeconds(this, p.movementDelay);
					}
				}
				this.stop = false;
				t = 0f;
				movementIndex = (movementIndex + 1) % movementPattern.Length;
				yield return null;
			}
			else
			{
				float dist = 640f - base.transform.position.x;
				if (dist < 50f)
				{
					base.animator.Play("Move_Forward");
					while (t < p.movementDuration)
					{
						speed = ((!isFlipped) ? (-p.movementSpeed) : p.movementSpeed);
						base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
						t += CupheadTime.Delta;
						yield return null;
					}
				}
				AudioManager.Play("clown_dash_start");
				base.animator.Play("Dash_Start");
				yield return CupheadTime.WaitForSeconds(this, p.bumperDashWarning);
				base.animator.SetTrigger("Continue");
				float endPos = isFlipped ? ((float)Level.Current.Right - offsetMove) : ((float)Level.Current.Left + offsetMove);
				while (base.transform.position.x != endPos)
				{
					bumperPos.x = Mathf.MoveTowards(base.transform.position.x, endPos, p.dashSpeed * CupheadTime.Delta * this.hitPauseCoefficient());
					base.transform.position = bumperPos;
					yield return null;
				}
				AudioManager.Play("clown_dash_end");
				base.animator.SetTrigger("End");
				isFlipped = !isFlipped;
				yield return base.animator.WaitForAnimationToEnd(this, "Dash_End", false, true);
				this.notDashing = true;
				t = 0f;
				base.StartCoroutine(this.dash_timer_cr(dashDelayPattern));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x00097FA4 File Offset: 0x000961A4
	public void FlipSprite()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00097FEC File Offset: 0x000961EC
	public void MoveAStop()
	{
		Vector2 vector = base.transform.position;
		this.stop = true;
		vector.y = this.forwardYPos.position.y;
		base.transform.position = vector;
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x000106CD File Offset: 0x0000E8CD
	public void MoveBStop()
	{
		this.stop = true;
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x0009803C File Offset: 0x0009623C
	public void AnimationOffsetUp()
	{
		Vector2 vector = base.transform.position;
		vector.y = this.forwardYPos.position.y;
		base.transform.position = vector;
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00098088 File Offset: 0x00096288
	public void SpawnDuck(ClownLevelDucks prefab, float startPercent)
	{
		if (prefab != null)
		{
			float num = 100f;
			LevelProperties.Clown.Duck duck = base.properties.CurrentState.duck;
			float maxYPos = duck.duckYHeightRange.RandomFloat();
			float num2 = startPercent / 100f * duck.duckYHeightRange.max;
			Vector2 pos = Vector3.zero;
			pos.y = 360f - num2;
			pos.x = 640f + num;
			ClownLevelDucks clownLevelDucks = Object.Instantiate<ClownLevelDucks>(prefab).Init(pos, base.properties.CurrentState.duck, maxYPos, duck.duckYMovementSpeed);
			clownLevelDucks.Init(pos, base.properties.CurrentState.duck, maxYPos, duck.duckYMovementSpeed);
		}
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00098148 File Offset: 0x00096348
	public IEnumerator ducks_cr()
	{
		LevelProperties.Clown.Duck p = base.properties.CurrentState.duck;
		string[] positionPattern = p.duckYStartPercentString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.duckTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int typeIndex = Random.Range(0, typePattern.Length);
		int posPercentIndex = Random.Range(0, positionPattern.Length);
		for (;;)
		{
			float spawnY = 0f;
			Parser.FloatTryParse(positionPattern[posPercentIndex], out spawnY);
			ClownLevelDucks toSpawn = null;
			if (typePattern[typeIndex][0] == 'R')
			{
				toSpawn = this.regularDuck;
			}
			else if (typePattern[typeIndex][0] == 'P')
			{
				toSpawn = this.pinkDuck;
			}
			else if (typePattern[typeIndex][0] == 'B')
			{
				toSpawn = this.bombDuck;
			}
			if (this.state != ClownLevelClown.State.Death)
			{
				this.SpawnDuck(toSpawn, spawnY);
			}
			yield return CupheadTime.WaitForSeconds(this, p.duckDelay);
			typeIndex = (typeIndex + 1) % typePattern.Length;
			posPercentIndex = (posPercentIndex + 1) % positionPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000FD1 RID: 4049
	public const float FALL_GRAVITY = -100f;

	// Token: 0x04000FD3 RID: 4051
	[SerializeField]
	public Transform forwardYPos;

	// Token: 0x04000FD4 RID: 4052
	[SerializeField]
	public ClownLevelDucks regularDuck;

	// Token: 0x04000FD5 RID: 4053
	[SerializeField]
	public ClownLevelDucks pinkDuck;

	// Token: 0x04000FD6 RID: 4054
	[SerializeField]
	public ClownLevelDucks bombDuck;

	// Token: 0x04000FD7 RID: 4055
	[SerializeField]
	public ClownLevelClownHelium clownHelium;

	// Token: 0x04000FD8 RID: 4056
	public bool notDashing;

	// Token: 0x04000FD9 RID: 4057
	public bool firstSelection;

	// Token: 0x04000FDA RID: 4058
	public bool stop;

	// Token: 0x04000FDB RID: 4059
	public float speed;

	// Token: 0x04000FDC RID: 4060
	public float fallAccumulatedGravity;

	// Token: 0x04000FDD RID: 4061
	public int timerIndex;

	// Token: 0x04000FDE RID: 4062
	public DamageDealer damageDealer;

	// Token: 0x04000FDF RID: 4063
	public DamageReceiver damageReceiver;

	// Token: 0x04000FE0 RID: 4064
	public DamageReceiver damageReceiverChild;

	// Token: 0x04000FE1 RID: 4065
	public Vector2 fallVelocity;

	// Token: 0x02000AF0 RID: 2800
	public enum State
	{
		// Token: 0x04005043 RID: 20547
		BumperCar,
		// Token: 0x04005044 RID: 20548
		Helium,
		// Token: 0x04005045 RID: 20549
		Death
	}
}
