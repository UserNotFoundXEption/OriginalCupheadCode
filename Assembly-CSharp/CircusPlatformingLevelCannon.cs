using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000404 RID: 1028
public class CircusPlatformingLevelCannon : AbstractPausableComponent
{
	// Token: 0x06002CEF RID: 11503 RVA: 0x000DB6E4 File Offset: 0x000D98E4
	public void Start()
	{
		this.goingBackwards = Rand.Bool();
		this.shootIndex = Random.Range(0, this.shootRoots.Length);
		this.pinkSplits = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = Random.Range(0, this.pinkSplits.Length);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		foreach (DamageReceiver damageReceiver in this.cannons)
		{
			damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x000DB798 File Offset: 0x000D9998
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !this.isDead)
		{
			this.isDead = true;
			this.StopAllCoroutines();
			base.StartCoroutine(this.slide_off_cr());
		}
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x000DB7F0 File Offset: 0x000D99F0
	public IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			while (PlayerManager.GetNext().transform.position.x < this.startTrigger.transform.position.x)
			{
				yield return null;
			}
			base.animator.SetInteger("Cannon", this.shootIndex + 1);
			yield return CupheadTime.WaitForSeconds(this, this.projectileDelay);
			if (PlayerManager.GetNext().transform.position.x > this.endTrigger.position.x)
			{
				while (PlayerManager.GetNext().transform.position.x > this.endTrigger.position.x)
				{
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000DB80C File Offset: 0x000D9A0C
	public void Shoot()
	{
		CircusPlatformingLevelCannonProjectile circusPlatformingLevelCannonProjectile = this.projectile.Create(this.shootRoots[this.shootIndex].transform.position, 0f, -this.projectileSpeed) as CircusPlatformingLevelCannonProjectile;
		circusPlatformingLevelCannonProjectile.SetColor(this.pinkSplits[this.pinkIndex]);
		circusPlatformingLevelCannonProjectile.DestroyDistance = 0f;
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkSplits.Length;
		if (this.goingBackwards)
		{
			if (this.shootIndex > 0)
			{
				this.shootIndex--;
			}
			else
			{
				this.shootIndex = this.shootRoots.Length - 1;
			}
		}
		else
		{
			this.shootIndex = (this.shootIndex + 1) % this.shootRoots.Length;
		}
		base.animator.SetInteger("Cannon", 0);
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x000DB8F0 File Offset: 0x000D9AF0
	public IEnumerator slide_off_cr()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("Droop");
		float slideOffSpeed = 500f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < 1220f)
		{
			base.transform.AddPosition(0f, slideOffSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield break;
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x000DB90C File Offset: 0x000D9B0C
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.startTrigger.transform.position.x, this.startTrigger.transform.position.y - 1000f), new Vector2(this.startTrigger.transform.position.x, this.startTrigger.transform.position.y + 1000f));
		Gizmos.DrawLine(new Vector2(this.endTrigger.transform.position.x, this.endTrigger.transform.position.y - 1000f), new Vector2(this.endTrigger.transform.position.x, this.endTrigger.transform.position.y + 1000f));
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x0002589C File Offset: 0x00023A9C
	public void ShootSFX()
	{
		AudioManager.Play("circus_cannon_shoot");
		this.emitAudioFromObject.Add("circus_cannon_shoot");
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x000258B8 File Offset: 0x00023AB8
	public void DroopSFX()
	{
		AudioManager.Play("circus_cannon_droop");
		this.emitAudioFromObject.Add("circus_cannon_droop");
	}

	// Token: 0x04002526 RID: 9510
	public const string ShootParameterName = "Cannon";

	// Token: 0x04002527 RID: 9511
	[SerializeField]
	public float health;

	// Token: 0x04002528 RID: 9512
	[SerializeField]
	public DamageReceiver[] cannons;

	// Token: 0x04002529 RID: 9513
	[SerializeField]
	public Transform[] shootRoots;

	// Token: 0x0400252A RID: 9514
	[SerializeField]
	public CircusPlatformingLevelCannonProjectile projectile;

	// Token: 0x0400252B RID: 9515
	[SerializeField]
	public float projectileSpeed;

	// Token: 0x0400252C RID: 9516
	[SerializeField]
	public float projectileDelay;

	// Token: 0x0400252D RID: 9517
	[SerializeField]
	public Transform startTrigger;

	// Token: 0x0400252E RID: 9518
	[SerializeField]
	public Transform endTrigger;

	// Token: 0x0400252F RID: 9519
	[SerializeField]
	public string pinkString;

	// Token: 0x04002530 RID: 9520
	public int shootIndex;

	// Token: 0x04002531 RID: 9521
	public bool goingBackwards;

	// Token: 0x04002532 RID: 9522
	public bool isDead;

	// Token: 0x04002533 RID: 9523
	public string[] pinkSplits;

	// Token: 0x04002534 RID: 9524
	public int pinkIndex;
}
