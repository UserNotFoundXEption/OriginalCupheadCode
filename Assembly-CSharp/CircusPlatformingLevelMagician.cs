using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class CircusPlatformingLevelMagician : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002D20 RID: 11552 RVA: 0x000DC04C File Offset: 0x000DA24C
	public override void Start()
	{
		base.Start();
		this.spawnPoints = new List<Transform>();
		this.spawnPoints.AddRange(this.spawnPointHolder.GetComponentsInChildren<Transform>());
		this.spawnPoints.RemoveAt(0);
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x00025AC8 File Offset: 0x00023CC8
	public override void OnStart()
	{
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x000DC09C File Offset: 0x000DA29C
	public IEnumerator check_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		while (player.transform.position.x < this.startPos.transform.position.x)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			yield return null;
		}
		base.StartCoroutine(this.appear_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x000DC0B8 File Offset: 0x000DA2B8
	public IEnumerator appear_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		yield return CupheadTime.WaitForSeconds(this, base.Properties.magicianAppearDelayRange.RandomFloat());
		for (;;)
		{
			while (player.transform.position.x < this.startPos.transform.position.x || player.transform.position.x > this.endPos.transform.position.x)
			{
				yield return null;
			}
			this.EnableMagician(true);
			while (!this.attackTrigger)
			{
				yield return null;
			}
			while (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
			{
				yield return null;
			}
			player = PlayerManager.GetFirst();
			Vector2 dir = player.transform.position - base.transform.position;
			this.projectileInstance = (this.projectile.Create(base.transform.position, MathUtils.DirectionToAngle(dir), base.Properties.ProjectileSpeed) as CircusPlatformingLevelMagicianBullet);
			this.projectileInstance.OnProjectileDeath += this.OnProjectileDeath;
			while (!this.disappearTrigger)
			{
				yield return null;
			}
			this.disappearTrigger = false;
			this.attackTrigger = false;
			this.EnableMagician(false);
			while (this.t < base.Properties.magicianAppearDelayRange.RandomFloat())
			{
				this.t += CupheadTime.Delta;
				yield return null;
			}
			this.t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x00025ACA File Offset: 0x00023CCA
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.projectileInstance != null)
		{
			this.projectileInstance.OnProjectileDeath -= this.OnProjectileDeath;
		}
		this.projectile = null;
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x00025B01 File Offset: 0x00023D01
	public void OnProjectileDeath()
	{
		base.animator.SetTrigger("EndAttack");
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x00025B13 File Offset: 0x00023D13
	public void Attack()
	{
		this.attackTrigger = true;
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x00025B1C File Offset: 0x00023D1C
	public void Disappear()
	{
		this.disappearTrigger = true;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x000DC0D4 File Offset: 0x000DA2D4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.white;
		List<Transform> list = new List<Transform>();
		list.AddRange(this.spawnPointHolder.GetComponentsInChildren<Transform>());
		list.RemoveAt(0);
		for (int i = 0; i < list.Count; i++)
		{
			Gizmos.DrawWireSphere(list[i].transform.position, 50f);
		}
		Gizmos.DrawLine(new Vector2(this.startPos.transform.position.x, this.startPos.transform.position.y + 1000f), new Vector2(this.startPos.transform.position.x, this.startPos.transform.position.y - 1000f));
		Gizmos.DrawLine(new Vector2(this.endPos.transform.position.x, this.endPos.transform.position.y + 1000f), new Vector2(this.endPos.transform.position.x, this.endPos.transform.position.y - 1000f));
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000DC254 File Offset: 0x000DA454
	public void EnableMagician(bool enabled)
	{
		base.GetComponent<Animator>().enabled = enabled;
		base.GetComponent<Collider2D>().enabled = enabled;
		base.GetComponent<SpriteRenderer>().enabled = enabled;
		if (enabled)
		{
			base.transform.position = this.spawnPoints[Random.Range(0, this.spawnPoints.Count)].transform.position;
		}
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x00025B25 File Offset: 0x00023D25
	public override void Die()
	{
		AudioManager.Play("circus_generic_death_big");
		this.emitAudioFromObject.Add("circus_generic_death_big");
		base.Die();
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x00025B47 File Offset: 0x00023D47
	public void AttackAppearSFX()
	{
		AudioManager.Play("circus_magician_appears");
		this.emitAudioFromObject.Add("circus_magician_appears");
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x00025B63 File Offset: 0x00023D63
	public void AttackIntroSFX()
	{
		AudioManager.Play("circus_magician_attack_intro");
		this.emitAudioFromObject.Add("circus_magician_attack_intro");
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x00025B7F File Offset: 0x00023D7F
	public void AttackOutroSFX()
	{
		AudioManager.Play("circus_magician_attack_outro");
		this.emitAudioFromObject.Add("circus_magician_attack_outro");
	}

	// Token: 0x04002564 RID: 9572
	public const string EndAttackParameterName = "EndAttack";

	// Token: 0x04002565 RID: 9573
	[SerializeField]
	public Transform startPos;

	// Token: 0x04002566 RID: 9574
	[SerializeField]
	public Transform endPos;

	// Token: 0x04002567 RID: 9575
	[SerializeField]
	public Transform spawnPointHolder;

	// Token: 0x04002568 RID: 9576
	[SerializeField]
	public CircusPlatformingLevelMagicianBullet projectile;

	// Token: 0x04002569 RID: 9577
	public List<Transform> spawnPoints;

	// Token: 0x0400256A RID: 9578
	public bool attackTrigger;

	// Token: 0x0400256B RID: 9579
	public bool disappearTrigger;

	// Token: 0x0400256C RID: 9580
	public float t;

	// Token: 0x0400256D RID: 9581
	public CircusPlatformingLevelMagicianBullet projectileInstance;
}
