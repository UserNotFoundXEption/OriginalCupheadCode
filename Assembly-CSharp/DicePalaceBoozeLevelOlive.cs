using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class DicePalaceBoozeLevelOlive : AbstractCollidableObject
{
	// Token: 0x060015A7 RID: 5543 RVA: 0x0009C810 File Offset: 0x0009AA10
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.moving = false;
		this.shotCount = 0;
		this.moveCount = 0;
		this.nextPlayerTarget = PlayerId.PlayerOne;
		base.Awake();
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x00012697 File Offset: 0x00010897
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000126AF File Offset: 0x000108AF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x000126CD File Offset: 0x000108CD
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !this.isDead)
		{
			this.isDead = true;
			this.OnDeath();
		}
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0009C870 File Offset: 0x0009AA70
	public void InitOlive(LevelProperties.DicePalaceBooze properties, int maxShotCount, string yCoordinates, string xCoordinates)
	{
		this.properties = properties;
		this.shotCountMax = maxShotCount;
		this.health = (float)properties.CurrentState.martini.oliveHP;
		this.yCoordinates = yCoordinates;
		this.xCoordinates = xCoordinates;
		this.moveCountMaxIndex = Random.Range(0, properties.CurrentState.martini.moveString.Split(new char[]
		{
			','
		}).Length);
		this.moveCountMax = Parser.IntParse(properties.CurrentState.martini.moveString.Split(new char[]
		{
			','
		})[this.moveCountMaxIndex]);
		this.verticalCoordinateIndex = Random.Range(0, yCoordinates.Split(new char[]
		{
			','
		}).Length);
		this.horizontalCoordinateindex = Random.Range(0, xCoordinates.Split(new char[]
		{
			','
		}).Length);
		this.moveToTarget.y = (float)(Level.Current.Ground + Parser.IntParse(yCoordinates.Split(new char[]
		{
			','
		})[this.verticalCoordinateIndex]));
		this.moveToTarget.x = (float)(Level.Current.Left + 50 + Parser.IntParse(xCoordinates.Split(new char[]
		{
			','
		})[this.horizontalCoordinateindex]));
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0009C9E0 File Offset: 0x0009ABE0
	public void ResetOlive(int maxShotCount)
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.shotCountMax = maxShotCount;
		this.health = (float)this.properties.CurrentState.martini.oliveHP;
		base.StartCoroutine(this.attack_cr());
		this.isDead = false;
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0009CA30 File Offset: 0x0009AC30
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.moveCount < this.moveCountMax)
			{
				this.GetNextTarget();
				base.StartCoroutine(this.move_cr());
				this.moveCount++;
				while (this.moving)
				{
					yield return null;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.martini.oliveStopDuration);
			}
			else
			{
				AudioManager.Play("booze_olive_attack");
				this.emitAudioFromObject.Add("booze_olive_attack");
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.martini.oliveHesitateAfterShooting);
			}
		}
		yield break;
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0001270A File Offset: 0x0001090A
	public void Shoot()
	{
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0009CA4C File Offset: 0x0009AC4C
	public IEnumerator shoot_cr()
	{
		this.moveCount = 0;
		Vector3 target = PlayerManager.GetPlayer(this.nextPlayerTarget).center - base.transform.position;
		BasicProjectile proj = this.pimentoPrefab.Create(base.transform.position, 0f, this.properties.CurrentState.martini.bulletSpeed);
		proj.animator.SetBool("Reverse", Rand.Bool());
		proj.transform.right = target;
		IEnumerator enumerator = proj.GetComponentInChildren<Transform>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.shotCount++;
		if (this.shotCount > this.shotCountMax)
		{
			proj.SetParryable(true);
			this.shotCount = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0009CA68 File Offset: 0x0009AC68
	public IEnumerator move_cr()
	{
		this.moving = true;
		while (Vector3.Distance(base.transform.position, this.moveToTarget) > 5f)
		{
			Vector3 dir = (this.moveToTarget - base.transform.position).normalized;
			base.transform.position += dir * this.properties.CurrentState.martini.oliveSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.moving = false;
		yield break;
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x0009CA84 File Offset: 0x0009AC84
	public void GetNextTarget()
	{
		this.verticalCoordinateIndex++;
		if (this.verticalCoordinateIndex >= this.yCoordinates.Split(new char[]
		{
			','
		}).Length)
		{
			this.verticalCoordinateIndex = 0;
		}
		this.horizontalCoordinateindex++;
		if (this.horizontalCoordinateindex >= this.xCoordinates.Split(new char[]
		{
			','
		}).Length)
		{
			this.horizontalCoordinateindex = 0;
		}
		this.moveToTarget.y = (float)(Level.Current.Ground + Parser.IntParse(this.yCoordinates.Split(new char[]
		{
			','
		})[this.verticalCoordinateIndex]));
		this.moveToTarget.x = (float)(Level.Current.Left + 50 + Parser.IntParse(this.xCoordinates.Split(new char[]
		{
			','
		})[this.horizontalCoordinateindex]));
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x00012719 File Offset: 0x00010919
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.pimentoPrefab = null;
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0009CB78 File Offset: 0x0009AD78
	public void OnDeath()
	{
		AudioManager.Play("booze_olive_death");
		this.emitAudioFromObject.Add("booze_olive_death");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		if (base.gameObject.activeInHierarchy)
		{
			base.animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x0001272E File Offset: 0x0001092E
	public void Deactivate()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04001198 RID: 4504
	[SerializeField]
	public BasicProjectile pimentoPrefab;

	// Token: 0x04001199 RID: 4505
	public int verticalCoordinateIndex;

	// Token: 0x0400119A RID: 4506
	public int horizontalCoordinateindex;

	// Token: 0x0400119B RID: 4507
	public float health;

	// Token: 0x0400119C RID: 4508
	public int shotCount;

	// Token: 0x0400119D RID: 4509
	public int shotCountMax;

	// Token: 0x0400119E RID: 4510
	public int moveCount;

	// Token: 0x0400119F RID: 4511
	public int moveCountMaxIndex;

	// Token: 0x040011A0 RID: 4512
	public int moveCountMax;

	// Token: 0x040011A1 RID: 4513
	public bool isDead;

	// Token: 0x040011A2 RID: 4514
	public bool moving;

	// Token: 0x040011A3 RID: 4515
	public string yCoordinates;

	// Token: 0x040011A4 RID: 4516
	public string xCoordinates;

	// Token: 0x040011A5 RID: 4517
	public PlayerId nextPlayerTarget;

	// Token: 0x040011A6 RID: 4518
	public Vector3 moveToTarget;

	// Token: 0x040011A7 RID: 4519
	public LevelProperties.DicePalaceBooze properties;

	// Token: 0x040011A8 RID: 4520
	public DamageReceiver damageReceiver;

	// Token: 0x040011A9 RID: 4521
	public DamageDealer damageDealer;
}
