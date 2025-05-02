using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000408 RID: 1032
public class CircusPlatformingLevelHotdog : AbstractPlatformingLevelEnemy
{
	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06002D08 RID: 11528 RVA: 0x000259A4 File Offset: 0x00023BA4
	// (set) Token: 0x06002D09 RID: 11529 RVA: 0x000DBBB8 File Offset: 0x000D9DB8
	public bool ProjectilesCanHit
	{
		get
		{
			return this.projectilesCanHit;
		}
		set
		{
			this.projectilesCanHit = value;
			for (int i = 0; i < this.projectileList.Count; i++)
			{
				this.projectileList[i].EnableCollider(this.projectilesCanHit);
			}
			base.animator.Play("Dance");
		}
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000259AC File Offset: 0x00023BAC
	public override void OnStart()
	{
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x000DBC10 File Offset: 0x000D9E10
	public override void Start()
	{
		base.Start();
		this.spawnPattern = this.spawnPatternString.Split(new char[]
		{
			','
		});
		this.condimentPattern = this.condimentPatternString.Split(new char[]
		{
			','
		});
		this.sidePattern = this.sidePatternString.Split(new char[]
		{
			','
		});
		this.shotDelayPattern = this.shotDelayPatternString.Split(new char[]
		{
			','
		});
		this.spawnIndex = Random.Range(0, this.spawnPattern.Length);
		this.condimentIndex = Random.Range(0, this.condimentPattern.Length);
		this.sideIndex = Random.Range(0, this.sidePattern.Length);
		this.shotDelayIndex = Random.Range(0, this.shotDelayPattern.Length);
		this.currentDelay = Parser.IntParse(this.shotDelayPattern[this.shotDelayIndex]);
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x000DBCFC File Offset: 0x000D9EFC
	public void ShootProjectile()
	{
		this.currentDelay--;
		if (this.currentDelay <= 0)
		{
			this.shotDelayIndex = (this.shotDelayIndex + 1) % this.shotDelayPattern.Length;
			this.currentDelay = Parser.IntParse(this.shotDelayPattern[this.shotDelayIndex]);
			string a = this.sidePattern[this.sideIndex];
			bool flag = a == "R";
			int num = Parser.IntParse(this.spawnPattern[this.spawnIndex]);
			if (flag)
			{
				num += this.projectilesSpawnPoints.Length / 2;
			}
			AudioManager.Play("circus_hotdog_projectile_shoot");
			this.emitAudioFromObject.Add("circus_hotdog_projectile_shoot");
			CircusPlatformingLevelHotdogProjectile circusPlatformingLevelHotdogProjectile = this.projectilePrefab.Create(this.projectilesSpawnPoints[num].position) as CircusPlatformingLevelHotdogProjectile;
			circusPlatformingLevelHotdogProjectile.Speed = -base.Properties.ProjectileSpeed;
			circusPlatformingLevelHotdogProjectile.SetCondiment(this.condimentPattern[this.condimentIndex]);
			circusPlatformingLevelHotdogProjectile.Side(flag);
			circusPlatformingLevelHotdogProjectile.DestroyDistance = this.projectileDistance;
			this.projectileList.Add(circusPlatformingLevelHotdogProjectile);
			circusPlatformingLevelHotdogProjectile.OnDestroyCallback += this.HotDogProjectileDie;
			circusPlatformingLevelHotdogProjectile.EnableCollider(this.projectilesCanHit);
			this.spawnIndex = (this.spawnIndex + 1) % this.spawnPattern.Length;
			this.condimentIndex = (this.condimentIndex + 1) % this.condimentPattern.Length;
			this.sideIndex = (this.sideIndex + 1) % this.sidePattern.Length;
		}
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x000259AE File Offset: 0x00023BAE
	public void HotDogProjectileDie(CircusPlatformingLevelHotdogProjectile obj)
	{
		obj.OnDestroyCallback -= this.HotDogProjectileDie;
		this.projectileList.Remove(obj);
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x000259CF File Offset: 0x00023BCF
	public override void Die()
	{
		base.animator.SetTrigger("Death");
		base.StartCoroutine(this.Explosion_cr());
		base.GetComponent<BoxCollider2D>().enabled = false;
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x000DBE78 File Offset: 0x000DA078
	public IEnumerator Explosion_cr()
	{
		this.exploder.StartExplosion();
		yield return new WaitForSeconds(2.5f);
		this.exploder.StopExplosions();
		yield break;
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x000259FA File Offset: 0x00023BFA
	public void DeathAnimationEnd()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x00025A07 File Offset: 0x00023C07
	public void HotDogDanceSFX()
	{
		AudioManager.Play("circus_hotdog_dance");
		this.emitAudioFromObject.Add("circus_hotdog_dance");
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x00025A23 File Offset: 0x00023C23
	public void HotDogDeathSFX()
	{
		AudioManager.Stop("circus_hotdog_dance");
		AudioManager.Play("circus_hotdog_death");
		this.emitAudioFromObject.Add("circus_hotdog_death");
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x00025A49 File Offset: 0x00023C49
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
	}

	// Token: 0x04002544 RID: 9540
	public const string DeathParameterName = "Death";

	// Token: 0x04002545 RID: 9541
	public const string Right = "R";

	// Token: 0x04002546 RID: 9542
	[SerializeField]
	public Transform[] projectilesSpawnPoints;

	// Token: 0x04002547 RID: 9543
	[SerializeField]
	public string spawnPatternString;

	// Token: 0x04002548 RID: 9544
	[SerializeField]
	public string condimentPatternString;

	// Token: 0x04002549 RID: 9545
	[SerializeField]
	public string sidePatternString;

	// Token: 0x0400254A RID: 9546
	[SerializeField]
	public string shotDelayPatternString;

	// Token: 0x0400254B RID: 9547
	[SerializeField]
	public float projectileDistance;

	// Token: 0x0400254C RID: 9548
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	public LevelBossDeathExploder exploder;

	// Token: 0x0400254E RID: 9550
	public string[] spawnPattern;

	// Token: 0x0400254F RID: 9551
	public string[] condimentPattern;

	// Token: 0x04002550 RID: 9552
	public string[] sidePattern;

	// Token: 0x04002551 RID: 9553
	public string[] shotDelayPattern;

	// Token: 0x04002552 RID: 9554
	public int spawnIndex;

	// Token: 0x04002553 RID: 9555
	public int condimentIndex;

	// Token: 0x04002554 RID: 9556
	public int sideIndex;

	// Token: 0x04002555 RID: 9557
	public int shotDelayIndex;

	// Token: 0x04002556 RID: 9558
	public int currentDelay;

	// Token: 0x04002557 RID: 9559
	public List<CircusPlatformingLevelHotdogProjectile> projectileList = new List<CircusPlatformingLevelHotdogProjectile>();

	// Token: 0x04002558 RID: 9560
	public bool projectilesCanHit;
}
