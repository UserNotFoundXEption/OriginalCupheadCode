using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class FlyingBlimpLevelMoonLady : LevelProperties.FlyingBlimp.Entity
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x00016A8E File Offset: 0x00014C8E
	// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x00016A96 File Offset: 0x00014C96
	public FlyingBlimpLevelMoonLady.State state { get; set; }

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000A9444 File Offset: 0x000A7644
	public override void Awake()
	{
		base.Awake();
		this.childColliders = base.gameObject.GetComponentsInChildren<CollisionChild>();
		this.changeStarted = false;
		Vector3 vector;
		vector..ctor(1f, 1f, 1f);
		if (Rand.Bool())
		{
			vector.y = -vector.y;
			this.smoke.transform.position = this.smokeFlippedPos.transform.position;
		}
		this.smoke.transform.SetScale(new float?(1f), new float?(vector.y), new float?(1f));
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
			collisionChild.GetComponent<Collider2D>().enabled = false;
		}
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000A9568 File Offset: 0x000A7768
	public void StartIntro()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.transform.position = this.transformSpawnPoint.position;
		base.animator.SetTrigger("To A");
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x00016A9F File Offset: 0x00014C9F
	public override void LevelInit(LevelProperties.FlyingBlimp properties)
	{
		base.LevelInit(properties);
		this.state = FlyingBlimpLevelMoonLady.State.Unspawned;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000A95B4 File Offset: 0x000A77B4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state != FlyingBlimpLevelMoonLady.State.Morph)
		{
			if (base.properties.CurrentState.uFO.invincibility)
			{
				if (info.damage < base.properties.CurrentHealth)
				{
					base.properties.DealDamage(info.damage);
				}
				else if (this.state == FlyingBlimpLevelMoonLady.State.Idle)
				{
					base.properties.DealDamage(info.damage);
				}
			}
			else
			{
				base.properties.DealDamage(info.damage);
			}
		}
		if (base.properties.CurrentHealth <= 0f && this.state != FlyingBlimpLevelMoonLady.State.Death)
		{
			this.StartDeath();
		}
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000A9670 File Offset: 0x000A7870
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (PauseManager.state == PauseManager.State.Paused)
		{
			this.pedal.Pause();
			this.gears.Pause();
		}
		else
		{
			this.pedal.UnPause();
			this.gears.UnPause();
		}
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x00016AAF File Offset: 0x00014CAF
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.pedal.Stop();
		this.gears.Stop();
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x00016ACD File Offset: 0x00014CCD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000A96D0 File Offset: 0x000A78D0
	public IEnumerator intro_cr()
	{
		AudioManager.Play("level_flying_blimp_transform_moon");
		this.state = FlyingBlimpLevelMoonLady.State.Morph;
		LevelProperties.FlyingBlimp.Morph p = base.properties.CurrentState.morph;
		PlanePlayerController playerOne = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerOne);
		PlanePlayerController playerTwo = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerTwo);
		if (playerOne != null && playerOne.isActiveAndEnabled)
		{
			playerOne.animationController.SetColorOverTime(this.dimColor.GetComponent<SpriteRenderer>().color, 15f);
		}
		if (playerTwo != null && playerTwo.isActiveAndEnabled)
		{
			playerTwo.animationController.SetColorOverTime(this.dimColor.GetComponent<SpriteRenderer>().color, 15f);
		}
		yield return null;
		while (base.transform.position != this.transformMorphEndPoint.position)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.transformMorphEndPoint.position, 300f * CupheadTime.Delta);
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield return CupheadTime.WaitForSeconds(this, p.crazyAHold);
		this.pedal.Stop();
		base.animator.SetTrigger("To B");
		yield return CupheadTime.WaitForSeconds(this, p.crazyBHold);
		base.animator.SetTrigger("End");
		base.StartCoroutine(this.stars_cr());
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_End", false, true);
		this.state = FlyingBlimpLevelMoonLady.State.Idle;
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.GetComponent<Collider2D>().enabled = true;
		}
		Level.Current.SetBounds(null, new int?(Level.Current.Right - 250), null, null);
		base.StartCoroutine(this.ufo_attack_handler_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000A96EC File Offset: 0x000A78EC
	public void SpawnStar(FlyingBlimpLevelStars prefab, Vector2 startPoint)
	{
		if (prefab != null)
		{
			Vector2 pos = prefab.transform.position;
			pos.y = 360f - startPoint.y;
			pos.x = 640f;
			prefab.Create(pos, base.properties.CurrentState.stars);
		}
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000A9750 File Offset: 0x000A7950
	public IEnumerator stars_cr()
	{
		LevelProperties.FlyingBlimp.Stars p = base.properties.CurrentState.stars;
		string[] positionPattern = p.positionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.typeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = Random.Range(0, typePattern.Length);
		float t = 0f;
		int place = Random.Range(0, positionPattern.Length);
		float waitTime = 0f;
		Vector2 spawnPos = Vector2.zero;
		for (;;)
		{
			for (int j = place; j < positionPattern.Length; j++)
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				if (positionPattern[j][0] == 'D')
				{
					Parser.FloatTryParse(positionPattern[j].Substring(1), out waitTime);
				}
				else
				{
					string[] array = positionPattern[j].Split(new char[]
					{
						'-'
					});
					foreach (string s in array)
					{
						float y = 0f;
						Parser.FloatTryParse(s, out y);
						FlyingBlimpLevelStars prefab = null;
						if (typePattern[i][0] == 'A')
						{
							prefab = this.starPrefabA;
						}
						else if (typePattern[i][0] == 'B')
						{
							prefab = this.starPrefabB;
						}
						else if (typePattern[i][0] == 'C')
						{
							prefab = this.starPrefabC;
						}
						else if (typePattern[i][0] == 'P')
						{
							prefab = this.starPrefabPink;
						}
						Parser.FloatTryParse(positionPattern[j].Substring(1), out waitTime);
						spawnPos.y = y;
						if (this.state != FlyingBlimpLevelMoonLady.State.Death)
						{
							this.SpawnStar(prefab, spawnPos);
						}
						i = (i + 1) % typePattern.Length;
					}
					waitTime = p.delay;
				}
				t += waitTime;
				j %= positionPattern.Length;
				place = 0;
			}
		}
		yield break;
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000A976C File Offset: 0x000A796C
	public IEnumerator ufo_attack_handler_cr()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Idle;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.uFO.moonWaitForNextATK);
		base.StartCoroutine(this.ufo_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x00016AEB File Offset: 0x00014CEB
	public void SmokeEffect()
	{
		base.animator.Play("Moon_Smoke");
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000A9788 File Offset: 0x000A7988
	public void SpawnUFO(FlyingBlimpLevelUFO prefab)
	{
		LevelProperties.FlyingBlimp.UFO uFO = base.properties.CurrentState.uFO;
		FlyingBlimpLevelUFO flyingBlimpLevelUFO = Object.Instantiate<FlyingBlimpLevelUFO>(prefab);
		flyingBlimpLevelUFO.Init(this.ufoStartPoint.position, this.ufoMidPoint.position, this.ufoStopPoint.position, uFO.UFOSpeed, uFO.UFOHP, uFO);
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000A97F0 File Offset: 0x000A79F0
	public IEnumerator ufo_cr()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Attack;
		float volume = 0.1f;
		LevelProperties.FlyingBlimp.UFO p = base.properties.CurrentState.uFO;
		string[] typePattern = p.UFOString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = Random.Range(0, typePattern.Length);
		AudioManager.Play("level_flying_blimp_moon_anticipation");
		base.animator.SetTrigger("To ATK");
		yield return CupheadTime.WaitForSeconds(this, p.moonATKAnticipation);
		this.gears.Play();
		this.gears.volume = volume;
		AudioManager.Play("level_flying_blimp_moon_face_extend");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Moon_Attack", false, true);
		this.time = 0f;
		this.startTimer = true;
		base.StartCoroutine(this.timer_cr());
		while (this.time < p.moonATKDuration)
		{
			this.pedal.volume = volume;
			if (volume < 1f)
			{
				volume += 0.1f;
			}
			if (this.state != FlyingBlimpLevelMoonLady.State.Death)
			{
				if (typePattern[index][0] == 'A')
				{
					this.SpawnUFO(this.ufoPrefabA);
				}
				else if (typePattern[index][0] == 'B')
				{
					this.SpawnUFO(this.ufoPrefabB);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, p.UFODelay);
			if (index < typePattern.Length - 1)
			{
				index++;
			}
			else
			{
				index = 0;
			}
		}
		this.pedal.volume = 1f;
		base.animator.SetTrigger("End");
		this.startTimer = false;
		this.gears.Stop();
		AudioManager.Play("level_flying_blimp_moon_gears_idle");
		yield return base.animator.WaitForAnimationToEnd(this, "Moon_Attack_To_Idle", false, true);
		base.StartCoroutine(this.ufo_attack_handler_cr());
		yield break;
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000A980C File Offset: 0x000A7A0C
	public IEnumerator timer_cr()
	{
		while (this.startTimer)
		{
			this.time += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x00016AFD File Offset: 0x00014CFD
	public void StartDeath()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Death;
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000A9828 File Offset: 0x000A7A28
	public IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x0400156E RID: 5486
	public bool changeStarted;

	// Token: 0x04001570 RID: 5488
	[SerializeField]
	public GameObject smoke;

	// Token: 0x04001571 RID: 5489
	[SerializeField]
	public Transform smokeFlippedPos;

	// Token: 0x04001572 RID: 5490
	[SerializeField]
	public AudioSource pedal;

	// Token: 0x04001573 RID: 5491
	[SerializeField]
	public AudioSource gears;

	// Token: 0x04001574 RID: 5492
	[SerializeField]
	public FlyingBlimpLevelUFO ufoPrefabA;

	// Token: 0x04001575 RID: 5493
	[SerializeField]
	public FlyingBlimpLevelUFO ufoPrefabB;

	// Token: 0x04001576 RID: 5494
	[SerializeField]
	public Transform ufoStartPoint;

	// Token: 0x04001577 RID: 5495
	[SerializeField]
	public Transform ufoMidPoint;

	// Token: 0x04001578 RID: 5496
	[SerializeField]
	public Transform ufoStopPoint;

	// Token: 0x04001579 RID: 5497
	[SerializeField]
	public Transform dimColor;

	// Token: 0x0400157A RID: 5498
	[SerializeField]
	public Transform transformSpawnPoint;

	// Token: 0x0400157B RID: 5499
	[SerializeField]
	public Transform transformMorphEndPoint;

	// Token: 0x0400157C RID: 5500
	[SerializeField]
	public FlyingBlimpLevelStars starPrefabA;

	// Token: 0x0400157D RID: 5501
	[SerializeField]
	public FlyingBlimpLevelStars starPrefabB;

	// Token: 0x0400157E RID: 5502
	[SerializeField]
	public FlyingBlimpLevelStars starPrefabC;

	// Token: 0x0400157F RID: 5503
	[SerializeField]
	public FlyingBlimpLevelStars starPrefabPink;

	// Token: 0x04001580 RID: 5504
	public DamageDealer damageDealer;

	// Token: 0x04001581 RID: 5505
	public DamageReceiver damageReceiver;

	// Token: 0x04001582 RID: 5506
	public CollisionChild[] childColliders;

	// Token: 0x04001583 RID: 5507
	public float time;

	// Token: 0x04001584 RID: 5508
	public bool startTimer;

	// Token: 0x02000C8F RID: 3215
	public enum State
	{
		// Token: 0x04005AAC RID: 23212
		Unspawned,
		// Token: 0x04005AAD RID: 23213
		Morph,
		// Token: 0x04005AAE RID: 23214
		Idle,
		// Token: 0x04005AAF RID: 23215
		Attack,
		// Token: 0x04005AB0 RID: 23216
		Death
	}
}
