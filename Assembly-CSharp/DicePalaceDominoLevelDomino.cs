using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class DicePalaceDominoLevelDomino : LevelProperties.DicePalaceDomino.Entity
{
	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06001644 RID: 5700 RVA: 0x00012EF5 File Offset: 0x000110F5
	// (set) Token: 0x06001645 RID: 5701 RVA: 0x00012EFD File Offset: 0x000110FD
	public DicePalaceDominoLevelDomino.State state { get; set; }

	// Token: 0x06001646 RID: 5702 RVA: 0x00012F06 File Offset: 0x00011106
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00012F3C File Offset: 0x0001113C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x00012F54 File Offset: 0x00011154
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x00012F67 File Offset: 0x00011167
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x0009ECC8 File Offset: 0x0009CEC8
	public override void LevelInit(LevelProperties.DicePalaceDomino properties)
	{
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.OnDeath;
		base.transform.parent.GetComponent<DicePalaceDominoLevelDominoSwing>().InitSwing(properties);
		this.happyAttackAngleIndex = Random.Range(0, properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		}).Length);
		this.happyAttackDirectionIndex = Random.Range(0, properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		}).Length);
		this.happyAttackBallTypePattern = properties.CurrentState.bouncyBall.projectileTypeString.Split(new char[]
		{
			','
		});
		this.happyAttackBallTypeIndex = Random.Range(0, this.happyAttackBallTypePattern.Length);
		this.happyAttackDelay = properties.CurrentState.bouncyBall.attackDelayRange.RandomFloat();
		this.sadAttackBoomerangTypeIndex = Random.Range(0, properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		}).Length);
		this.sadAttackDelay = properties.CurrentState.boomerang.attackDelayRange.RandomFloat();
		this.floor.InitFloor(properties);
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x00012F85 File Offset: 0x00011185
	public void OnIntroEnd()
	{
		base.animator.enabled = true;
		this.floor.StartSpawningTiles();
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x0009EE34 File Offset: 0x0009D034
	public IEnumerator intro_cr()
	{
		AudioManager.PlayLoop("dice_palace_domino_intro_start_loop");
		this.emitAudioFromObject.Add("dice_palace_domino_intro_start_loop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Intro", false);
		AudioManager.Stop("dice_palace_domino_intro_start_loop");
		AudioManager.Play("dice_palace_domino_intro");
		this.emitAudioFromObject.Add("dice_palace_domino_intro");
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x00012F9E File Offset: 0x0001119E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bouncyBallPrefab = null;
		this.boomerangPrefab = null;
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00012FB4 File Offset: 0x000111B4
	public void OnBouncyBall()
	{
		base.StartCoroutine(this.bouncyBall_cr());
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x0009EE50 File Offset: 0x0009D050
	public IEnumerator bouncyBall_cr()
	{
		this.state = DicePalaceDominoLevelDomino.State.BouncyBall;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bouncyBall.initialAttackDelay);
		base.animator.SetTrigger("OnProjectile");
		yield return base.animator.WaitForAnimationToStart(this, "Projectile_Attack", false);
		AudioManager.Play("dice_palace_domino_projectile_attack");
		this.emitAudioFromObject.Add("dice_palace_domino_projectile_attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.happyAttackDelay);
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x00012FC3 File Offset: 0x000111C3
	public void SpawnBall()
	{
		base.StartCoroutine(this.spawn_ball_cr());
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0009EE6C File Offset: 0x0009D06C
	public IEnumerator spawn_ball_cr()
	{
		float angle = (float)Parser.IntParse(base.properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		})[this.happyAttackAngleIndex]);
		if (base.properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		})[this.happyAttackDirectionIndex][0] == 'U')
		{
			angle = -angle;
		}
		Vector3 direction = Vector3.left;
		direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
		AbstractProjectile proj = this.bouncyBallPrefab.Create(this.bouncySpawnpoint.position);
		proj.SetParryable(this.happyAttackBallTypePattern[this.happyAttackBallTypeIndex][0] == 'P');
		proj.GetComponent<DicePalaceDominoLevelBouncyBall>().InitBouncyBall(base.properties.CurrentState.bouncyBall.bulletSpeed, direction);
		this.happyAttackAngleIndex++;
		if (this.happyAttackAngleIndex >= base.properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		}).Length)
		{
			this.happyAttackAngleIndex = 0;
		}
		this.happyAttackDirectionIndex++;
		if (this.happyAttackDirectionIndex >= base.properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		}).Length)
		{
			this.happyAttackDirectionIndex = 0;
		}
		this.happyAttackBallTypeIndex++;
		if (this.happyAttackBallTypeIndex >= this.happyAttackBallTypePattern.Length)
		{
			this.happyAttackBallTypeIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x00012FD2 File Offset: 0x000111D2
	public void OnBoomerang()
	{
		base.StartCoroutine(this.boomerang_cr());
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x0009EE88 File Offset: 0x0009D088
	public IEnumerator boomerang_cr()
	{
		this.state = DicePalaceDominoLevelDomino.State.Boomerang;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boomerang.initialAttackDelay);
		base.animator.SetTrigger("OnBird");
		yield return base.animator.WaitForAnimationToEnd(this, "Bird_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.sadAttackDelay);
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x00012FE1 File Offset: 0x000111E1
	public void SpawnBoomerang()
	{
		base.StartCoroutine(this.spawn_boomerang_cr());
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x0009EEA4 File Offset: 0x0009D0A4
	public IEnumerator spawn_boomerang_cr()
	{
		LevelProperties.DicePalaceDomino.Boomerang p = base.properties.CurrentState.boomerang;
		if (base.properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		})[this.sadAttackBoomerangTypeIndex][0] == 'R')
		{
			DicePalaceDominoLevelBoomerang proj = this.boomerangPrefab.Create(this.birdSpawnpoint.position, p.boomerangSpeed, p.health);
		}
		else
		{
			DicePalaceDominoLevelBoomerang proj = this.boomerangPrefab.Create(this.birdSpawnpoint.position, p.boomerangSpeed, p.health);
			proj.GetComponent<SpriteRenderer>().color = Color.magenta;
			proj.SetParryable(true);
		}
		this.sadAttackBoomerangTypeIndex++;
		if (this.sadAttackBoomerangTypeIndex >= base.properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		}).Length)
		{
			this.sadAttackBoomerangTypeIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x00012FF0 File Offset: 0x000111F0
	public void OnDeath()
	{
		AudioManager.PlayLoop("dice_palace_domino_death_start_loop");
		this.emitAudioFromObject.Add("dice_palace_domino_death_start_loop");
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x0001301C File Offset: 0x0001121C
	public void EndDeathLoop()
	{
		AudioManager.Stop("dice_palace_domino_death_start_loop");
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x00013028 File Offset: 0x00011228
	public void DeathSFX()
	{
		AudioManager.Play("dice_palace_domino_death");
		this.emitAudioFromObject.Add("dice_palace_domino_death");
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x00013044 File Offset: 0x00011244
	public void BirdAttackSFX()
	{
		AudioManager.Play("dice_palace_domino_bird_attack");
		this.emitAudioFromObject.Add("dice_palace_domino_bird_attack");
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x00013060 File Offset: 0x00011260
	public void SwingForwardSFX()
	{
		AudioManager.Play("swing_forward");
		this.emitAudioFromObject.Add("swing_forward");
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x0001307C File Offset: 0x0001127C
	public void SwingBackSFX()
	{
		AudioManager.Play("swing_back");
		this.emitAudioFromObject.Add("swing_back");
	}

	// Token: 0x0400121D RID: 4637
	[SerializeField]
	public Transform bouncySpawnpoint;

	// Token: 0x0400121E RID: 4638
	[SerializeField]
	public Transform birdSpawnpoint;

	// Token: 0x0400121F RID: 4639
	[SerializeField]
	public DicePalaceDominoLevelBouncyBall bouncyBallPrefab;

	// Token: 0x04001220 RID: 4640
	[SerializeField]
	public DicePalaceDominoLevelBoomerang boomerangPrefab;

	// Token: 0x04001221 RID: 4641
	[SerializeField]
	public DicePalaceDominoLevelFloor floor;

	// Token: 0x04001222 RID: 4642
	public int happyAttackAngleIndex;

	// Token: 0x04001223 RID: 4643
	public int happyAttackDirectionIndex;

	// Token: 0x04001224 RID: 4644
	public string[] happyAttackBallTypePattern;

	// Token: 0x04001225 RID: 4645
	public int happyAttackBallTypeIndex;

	// Token: 0x04001226 RID: 4646
	public float happyAttackDelay;

	// Token: 0x04001227 RID: 4647
	public int sadAttackBoomerangTypeIndex;

	// Token: 0x04001228 RID: 4648
	public float sadAttackDelay;

	// Token: 0x04001229 RID: 4649
	public DamageDealer damageDealer;

	// Token: 0x0400122A RID: 4650
	public DamageReceiver damageReceiver;

	// Token: 0x02000B89 RID: 2953
	public enum State
	{
		// Token: 0x0400544C RID: 21580
		Idle,
		// Token: 0x0400544D RID: 21581
		Boomerang,
		// Token: 0x0400544E RID: 21582
		BouncyBall
	}
}
