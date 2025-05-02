using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class FlyingGenieLevelSpawner : AbstractProjectile
{
	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001CCE RID: 7374 RVA: 0x00018694 File Offset: 0x00016894
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000AF300 File Offset: 0x000AD500
	public FlyingGenieLevelSpawner Create(Vector2 pos, AbstractPlayerController player, LevelProperties.FlyingGenie.Bullets properties)
	{
		FlyingGenieLevelSpawner flyingGenieLevelSpawner = base.Create(pos) as FlyingGenieLevelSpawner;
		flyingGenieLevelSpawner.properties = properties;
		flyingGenieLevelSpawner.player = player;
		return flyingGenieLevelSpawner;
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x0001869B File Offset: 0x0001689B
	public override void Start()
	{
		base.Start();
		this.SetUpSpawnPoints();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000AF32C File Offset: 0x000AD52C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i].transform.Rotate(0f, 0f, this.properties.spawnerRotateSpeed * CupheadTime.Delta);
		}
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000186B6 File Offset: 0x000168B6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000AF3A0 File Offset: 0x000AD5A0
	public void SetUpSpawnPoints()
	{
		float num = Random.Range(0f, 6.28318548f);
		float rotation = 0f;
		float x = base.transform.position.x;
		float y = base.transform.position.y;
		this.points = new FlyingGenieLevelSpawnerPoint[this.properties.spawnerCount];
		for (int i = 0; i < this.properties.spawnerCount; i++)
		{
			if (i == 0)
			{
				rotation = num * 57.29578f + 90f;
			}
			else if (i == 1)
			{
				rotation = num * 57.29578f - 90f;
			}
			else if (i == 2)
			{
				rotation = num * 57.29578f + 360f;
			}
			else if (i == 3)
			{
				rotation = num * 57.29578f - 180f;
			}
			this.points[i] = this.pointPrefab.Create(new Vector3(x, y), rotation, this.properties);
			this.points[i].transform.parent = base.transform;
		}
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000AF4CC File Offset: 0x000AD6CC
	public IEnumerator move_cr()
	{
		float offset = 200f;
		float size = base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
		int count = 0;
		int maxCount = this.properties.spawnerMoveCountRange.RandomInt();
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startDir = Vector3.zero - base.transform.position;
		while (base.transform.position.y > 0f)
		{
			base.transform.position += startDir.normalized * this.properties.spawnerSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		for (;;)
		{
			Vector3 start = base.transform.position;
			Vector3 dir = this.player.transform.position - base.transform.position;
			Vector3 endDist = start + dir.normalized * this.properties.spawnerDistance;
			if (this.isDead)
			{
				for (;;)
				{
					base.transform.position += dir.normalized * this.properties.spawnerSpeed * CupheadTime.FixedDelta;
					if (base.transform.position.x < -640f - offset || base.transform.position.x > 640f + offset || base.transform.position.y > 360f + offset || base.transform.position.y < -360f - offset)
					{
						break;
					}
					yield return wait;
				}
				this.Kill();
				this.StopAllCoroutines();
			}
			while (base.transform.position != endDist)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, endDist, this.properties.spawnerSpeed * CupheadTime.FixedDelta);
				if (base.transform.position.x < -640f + size || base.transform.position.x > 640f - size || base.transform.position.y > 360f - size || base.transform.position.y < -360f + size)
				{
					break;
				}
				yield return wait;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerMoveDelay);
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			count++;
			if (count >= maxCount)
			{
				while (this.attackCount < this.properties.spawnerShotCount)
				{
					base.animator.SetTrigger("Attack");
					yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerShotDelay);
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerHesitate);
				count = 0;
				this.attackCount = 0;
				maxCount = this.properties.spawnerMoveCountRange.RandomInt();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000AF4E8 File Offset: 0x000AD6E8
	public void Kill()
	{
		foreach (FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint in this.points)
		{
			flyingGenieLevelSpawnerPoint.Dead();
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000AF528 File Offset: 0x000AD728
	public void Attack()
	{
		foreach (FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint in this.points)
		{
			flyingGenieLevelSpawnerPoint.Shoot();
		}
		this.attackCount++;
		if (this.attackCount >= this.properties.spawnerShotCount)
		{
			base.animator.SetTrigger("End");
		}
	}

	// Token: 0x04001768 RID: 5992
	public const string AttackParameterName = "Attack";

	// Token: 0x04001769 RID: 5993
	public const string EndAttackParameterName = "End";

	// Token: 0x0400176A RID: 5994
	[SerializeField]
	public FlyingGenieLevelSpawnerPoint pointPrefab;

	// Token: 0x0400176B RID: 5995
	public FlyingGenieLevelSpawnerPoint[] points;

	// Token: 0x0400176C RID: 5996
	public LevelProperties.FlyingGenie.Bullets properties;

	// Token: 0x0400176D RID: 5997
	public AbstractPlayerController player;

	// Token: 0x0400176E RID: 5998
	public float speed;

	// Token: 0x0400176F RID: 5999
	public int attackCount;

	// Token: 0x04001770 RID: 6000
	public bool isDead;
}
