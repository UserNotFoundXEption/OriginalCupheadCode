using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class DicePalaceEightBallLevelEightBall : LevelProperties.DicePalaceEightBall.Entity
{
	// Token: 0x06001687 RID: 5767 RVA: 0x0009F294 File Offset: 0x0009D494
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		List<int> list = new List<int>(this.balls.Count);
		this.newList = new List<int>();
		for (int i = 0; i < this.balls.Count; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.balls.Count; j++)
		{
			int index = Random.Range(0, list.Count);
			this.newList.Add(list[index]);
			list.RemoveAt(index);
		}
		this.ballIndex = 0;
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x000132D3 File Offset: 0x000114D3
	public override void LevelInit(LevelProperties.DicePalaceEightBall properties)
	{
		base.LevelInit(properties);
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x000132FF File Offset: 0x000114FF
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0009F354 File Offset: 0x0009D554
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		AudioManager.Play("dice_palace_eight_ball_intro");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_intro");
		yield return base.animator.WaitForAnimationToStart(this, "Right_Idle", false);
		base.StartCoroutine(this.shoot_bullet_cr());
		base.StartCoroutine(this.spawn_balls_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x0009F370 File Offset: 0x0009D570
	public void LoopCounter()
	{
		if (this.currentLoops < base.properties.CurrentState.general.idleLoopAmount)
		{
			this.currentLoops++;
		}
		else
		{
			base.animator.SetTrigger("Continue");
			this.currentLoops = 0;
		}
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x00013312 File Offset: 0x00011512
	public void HitLeftIdle()
	{
		base.animator.SetBool("MovingLeft", false);
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x00013325 File Offset: 0x00011525
	public void HitRightIdle()
	{
		base.animator.SetBool("MovingLeft", true);
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x00013338 File Offset: 0x00011538
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.attackEffect = null;
		this.projectileEffect = null;
		this.projectile = null;
		this.pinkProjectile = null;
		this.balls = null;
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0009F3C8 File Offset: 0x0009D5C8
	public IEnumerator shoot_bullet_cr()
	{
		LevelProperties.DicePalaceEightBall.General p = base.properties.CurrentState.general;
		string[] projectileType = p.shootString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int projectileIndex = Random.Range(0, projectileType.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shootDelay);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack_Start", false);
			AudioManager.Play("dice_palace_eight_ball_attack_start");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_start");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			Effect effect = Object.Instantiate<Effect>(this.projectileEffect);
			effect.transform.position = this.root.transform.position;
			yield return effect.GetComponent<Animator>().WaitForAnimationToEnd(this, "Projectile", false, true);
			AbstractPlayerController player = PlayerManager.GetNext();
			Vector3 dir = player.transform.position - base.transform.position;
			AudioManager.Play("dice_palace_eight_ball_eight_attack_fire");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_eight_attack_fire");
			if (projectileType[projectileIndex][0] == 'R')
			{
				this.attackEffect.Create(this.root.transform.position);
				this.projectile.Create(this.root.transform.position, MathUtils.DirectionToAngle(dir), base.properties.CurrentState.general.shootSpeed);
			}
			else if (projectileType[projectileIndex][0] == 'P')
			{
				this.attackEffect.Create(this.root.transform.position);
				this.pinkProjectile.Create(this.root.transform.position, MathUtils.DirectionToAngle(dir), base.properties.CurrentState.general.shootSpeed);
			}
			projectileIndex = (projectileIndex + 1) % projectileType.Length;
			yield return CupheadTime.WaitForSeconds(this, p.attackDuration);
			base.animator.SetTrigger("OnEnd");
			AudioManager.Play("dice_palace_eight_ball_attack_end");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_end");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x00013363 File Offset: 0x00011563
	public void IntroSFX()
	{
		AudioManager.Play("dice_palace_eight_ball_eight_intro");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_eight_intro");
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0009F3E4 File Offset: 0x0009D5E4
	public IEnumerator spawn_balls_cr()
	{
		LevelProperties.DicePalaceEightBall.PoolBalls p = base.properties.CurrentState.poolBalls;
		string[] side = p.sideString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float offset = base.GetComponent<Renderer>().bounds.size.x / 2f;
		int sideIndex = Random.Range(0, side.Length);
		bool onLeft = false;
		Vector3 leftPos = new Vector3(-640f, 360f + offset, 0f);
		Vector3 rightPos = new Vector3(640f, 360f + offset, 0f);
		Vector3 pos = Vector3.zero;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			DicePalaceEightBallLevelPoolBall ballInstance = null;
			if (side[sideIndex][0] == 'L')
			{
				onLeft = true;
				pos = leftPos;
			}
			else if (side[sideIndex][0] == 'R')
			{
				onLeft = false;
				pos = rightPos;
			}
			else
			{
				Debug.LogError("sideString pattern is wrong", null);
			}
			int index = this.newList[this.ballIndex];
			while (index < 0 || index > this.balls.Count)
			{
				this.ballIndex = (this.ballIndex + 1) % this.balls.Count;
				index = this.newList[this.ballIndex];
				yield return null;
			}
			if (index == 0)
			{
				ballInstance = this.balls[index].Create(pos, p.oneJumpHorizontalSpeed, p.oneJumpVerticalSpeed, p.oneJumpGravity, p.oneGroundDelay, onLeft, this);
			}
			else if (index == 1)
			{
				ballInstance = this.balls[index].Create(pos, p.twoJumpHorizontalSpeed, p.twoJumpVerticalSpeed, p.twoJumpGravity, p.twoGroundDelay, onLeft, this);
			}
			else if (index == 2)
			{
				ballInstance = this.balls[index].Create(pos, p.threeJumpHorizontalSpeed, p.threeJumpVerticalSpeed, p.threeJumpGravity, p.threeGroundDelay, onLeft, this);
			}
			else if (index == 3)
			{
				ballInstance = this.balls[index].Create(pos, p.fourJumpHorizontalSpeed, p.fourJumpVerticalSpeed, p.fourJumpGravity, p.fourGroundDelay, onLeft, this);
			}
			else if (index == 4)
			{
				ballInstance = this.balls[index].Create(pos, p.fiveJumpHorizontalSpeed, p.fiveJumpVerticalSpeed, p.fiveJumpGravity, p.fiveGroundDelay, onLeft, this);
			}
			else
			{
				Debug.LogError("Invalid index", null);
			}
			if (ballInstance != null)
			{
				ballInstance.SetVariation(this.newList[this.ballIndex]);
			}
			this.ballIndex = (this.ballIndex + 1) % this.balls.Count;
			sideIndex = (sideIndex + 1) % side.Length;
		}
		yield break;
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0009F400 File Offset: 0x0009D600
	public void OnDeath()
	{
		if (this.OnEightBallDeath != null)
		{
			this.OnEightBallDeath();
		}
		this.StopAllCoroutines();
		AudioManager.PlayLoop("dice_palace_eight_ball_attack_death_loop");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_death_loop");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04001246 RID: 4678
	[SerializeField]
	public Effect attackEffect;

	// Token: 0x04001247 RID: 4679
	[SerializeField]
	public Effect projectileEffect;

	// Token: 0x04001248 RID: 4680
	[SerializeField]
	public Transform root;

	// Token: 0x04001249 RID: 4681
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x0400124A RID: 4682
	[SerializeField]
	public BasicProjectile pinkProjectile;

	// Token: 0x0400124B RID: 4683
	[SerializeField]
	public List<DicePalaceEightBallLevelPoolBall> balls;

	// Token: 0x0400124C RID: 4684
	public List<int> newList;

	// Token: 0x0400124D RID: 4685
	public DamageReceiver damageReceiver;

	// Token: 0x0400124E RID: 4686
	public int currentLoops;

	// Token: 0x0400124F RID: 4687
	public int ballIndex;

	// Token: 0x04001250 RID: 4688
	public Action OnEightBallDeath;
}
