using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
public class TreePlatformingLevelDragonfly : PlatformingLevelBigEnemy
{
	// Token: 0x06002C67 RID: 11367 RVA: 0x000D9C40 File Offset: 0x000D7E40
	public override void Start()
	{
		base.Start();
		this.LockDistance = 1550f;
		this.startPos = base.transform.position;
		this.aimIndex = Random.Range(0, base.Properties.dragonFlyAimString.Split(new char[]
		{
			','
		}).Length);
		this.delayIndex = Random.Range(0, base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		}).Length);
		this.LockDistance -= base.Properties.dragonFlyLockDistOffset;
		this.mosquitos = new List<TreePlatformingLevelMosquito>(this.platforms.GetComponentsInChildren<TreePlatformingLevelMosquito>());
		this.currentMosquitos = this.randomizeList(this.mosquitos);
		base.StartCoroutine(this.enter_cr());
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x0002529C File Offset: 0x0002349C
	public override void Shoot()
	{
		if (!this.isShooting)
		{
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x000D9D10 File Offset: 0x000D7F10
	public IEnumerator enter_cr()
	{
		base.transform.position = new Vector3(this.startPos.x + 800f, this.startPos.y);
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		float t = 0f;
		float time = base.Properties.dragonFlyInitRiseTime;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(base.transform.position, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.startPos;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.sine_cr());
		yield break;
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x000D9D2C File Offset: 0x000D7F2C
	public IEnumerator shoot_cr()
	{
		float t = 0f;
		float t2 = 0f;
		float angle = 0f;
		bool pickDir = false;
		Vector3 direction = Vector3.zero;
		this.isShooting = true;
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.dragonFlyWarningDuration);
		base.animator.SetTrigger("Continue");
		while (t < base.Properties.dragonFlyAttackDuration)
		{
			pickDir = false;
			while (t2 < base.Properties.dragonFlyProjectileDelay)
			{
				t2 += CupheadTime.Delta;
				t += CupheadTime.Delta;
				yield return null;
			}
			t2 = 0f;
			if (base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			})[this.aimIndex][0] == 'R')
			{
				while (!pickDir)
				{
					if (this.currentMosquitos[this.cycleIndex].isActive)
					{
						direction = this.currentMosquitos[this.cycleIndex].transform.position - base.transform.position;
						this.currentMosquitos.RemoveAt(this.cycleIndex);
						if (this.currentMosquitos.Count > 0)
						{
							this.cycleIndex = (this.cycleIndex + 1) % this.currentMosquitos.Count;
						}
						else
						{
							this.currentMosquitos = this.randomizeList(this.mosquitos);
						}
						pickDir = true;
					}
					else
					{
						this.cycleIndex = (this.cycleIndex + 1) % this.currentMosquitos.Count;
						this.currentMosquitos = this.randomizeList(this.mosquitos);
					}
					yield return null;
				}
			}
			else if (base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			})[this.aimIndex][0] == 'P' && this._target.transform.position.x < base.transform.position.x)
			{
				direction = this._target.transform.position - base.transform.position;
			}
			angle = MathUtils.DirectionToAngle(direction);
			this.projectile.Create(this.projectileRoot.transform.position, angle + 5f, base.Properties.dragonFlyProjectileSpeed);
			this.aimIndex = (this.aimIndex + 1) % base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			}).Length;
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Attack_To_Idle", false, true);
		yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]));
		this.delayIndex = (this.delayIndex + 1) % base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		}).Length;
		this.isShooting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000D9D48 File Offset: 0x000D7F48
	public List<TreePlatformingLevelMosquito> randomizeList(List<TreePlatformingLevelMosquito> platforms)
	{
		List<TreePlatformingLevelMosquito> list = new List<TreePlatformingLevelMosquito>();
		List<TreePlatformingLevelMosquito> list2 = new List<TreePlatformingLevelMosquito>();
		list2.AddRange(platforms);
		for (int i = 0; i < platforms.Count; i++)
		{
			int index = Random.Range(0, list2.Count);
			list.Add(list2[index]);
			list2.RemoveAt(index);
		}
		this.cycleIndex = 0;
		return list;
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000D9DA8 File Offset: 0x000D7FA8
	public IEnumerator sine_cr()
	{
		float time = 0.5f;
		float t = 0f;
		float val = 1f;
		for (;;)
		{
			if (!this.isShooting && CupheadTime.Delta != 0f)
			{
				t += CupheadTime.Delta;
				float num = Mathf.Sin(t / time);
				base.transform.AddPosition(0f, num * val, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x000D9DC4 File Offset: 0x000D7FC4
	public override void Die()
	{
		if (!this.isDead)
		{
			this.StopAllCoroutines();
			this.isDead = true;
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play("Death");
			AudioManager.Play("level_platform_dragonfly_death");
			this.emitAudioFromObject.Add("level_platform_dragonfly_death");
			this.explosion.StartExplosion();
			base.StartCoroutine(this.fall_cr());
		}
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000D9E38 File Offset: 0x000D8038
	public IEnumerator fall_cr()
	{
		float velocity = 0f;
		float gravity = 1500f;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.explosion.StopExplosions();
		while (base.transform.position.y > -CupheadLevelCamera.Current.Height - 200f)
		{
			base.transform.AddPosition(0f, velocity * CupheadTime.Delta, 0f);
			velocity -= gravity * CupheadTime.Delta;
			yield return null;
		}
		this.<Die>__BaseCallProxy0();
		yield return null;
		yield break;
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000252B6 File Offset: 0x000234B6
	public void SoundDragonflyAttackWarning()
	{
		AudioManager.Play("level_platform_dragonfly_attack_warning");
		this.emitAudioFromObject.Add("level_platform_dragonfly_attack_warning");
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000252D2 File Offset: 0x000234D2
	public void SoundDragonflyAttackStart()
	{
		AudioManager.Play("level_platform_dragonfly_attack_start");
		this.emitAudioFromObject.Add("level_platform_dragonfly_attack_start");
	}

	// Token: 0x040024A2 RID: 9378
	[SerializeField]
	public LevelBossDeathExploder explosion;

	// Token: 0x040024A3 RID: 9379
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x040024A4 RID: 9380
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x040024A5 RID: 9381
	public GameObject platforms;

	// Token: 0x040024A6 RID: 9382
	public List<TreePlatformingLevelMosquito> mosquitos;

	// Token: 0x040024A7 RID: 9383
	public List<TreePlatformingLevelMosquito> currentMosquitos;

	// Token: 0x040024A8 RID: 9384
	public Vector3 startPos;

	// Token: 0x040024A9 RID: 9385
	public int delayIndex;

	// Token: 0x040024AA RID: 9386
	public int aimIndex;

	// Token: 0x040024AB RID: 9387
	public int cycleIndex;

	// Token: 0x040024AC RID: 9388
	public bool isShooting;
}
