using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class FunhousePlatformingLevelDuckCarSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002D80 RID: 11648 RVA: 0x00025FA6 File Offset: 0x000241A6
	public override void Start()
	{
		base.Start();
		this.duckPinkIndex = Random.Range(0, this.duckPinkString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x00025FD2 File Offset: 0x000241D2
	public override void StartSpawning()
	{
		base.StartSpawning();
		base.StartCoroutine(this.start_spawning_cr());
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00025FE7 File Offset: 0x000241E7
	public override void EndSpawning()
	{
		base.StopCoroutine(this.start_spawning_cr());
		base.EndSpawning();
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x000DCF04 File Offset: 0x000DB104
	public IEnumerator start_spawning_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initalSpawnDelay.RandomFloat());
		for (;;)
		{
			if (this.firstTime)
			{
				this.ducksTop = false;
				this.firstTime = false;
			}
			else
			{
				this.ducksTop = !this.ducksTop;
			}
			base.StartCoroutine(this.spawn_ducks_cr());
			base.StartCoroutine(this.spawn_cars_cr());
			this.ducksSpawning = true;
			this.carsSpawning = true;
			while (this.ducksSpawning || this.carsSpawning)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x000DCF20 File Offset: 0x000DB120
	public IEnumerator spawn_ducks_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.duckDelay);
		float bigDuckSize = this.bigDuckPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		float smallDuckSize = this.smallDuckPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		FunhousePlatformingLevelDuck lastDuck = null;
		Vector2 startPos = Vector2.zero;
		startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + this.duckSpacing;
		startPos.y = ((!this.ducksTop) ? this.bottomSpawnRoot.position.y : this.topSpawnRoot.position.y);
		FunhousePlatformingLevelDuck bigDuck = this.bigDuckPrefab.Spawn(startPos);
		bigDuck.transform.SetScale(null, new float?((!this.ducksTop) ? bigDuck.transform.localScale.y : (-bigDuck.transform.localScale.y)), null);
		int num = 1;
		while ((float)num < this.duckCount)
		{
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + this.duckSpacing + (smallDuckSize + this.duckSpacing) * (float)num;
			startPos.y = ((!this.ducksTop) ? this.bottomSpawnRoot.position.y : this.topSpawnRoot.position.y);
			if (this.duckPinkString.Split(new char[]
			{
				','
			})[this.duckPinkIndex][0] == 'P')
			{
				FunhousePlatformingLevelDuck funhousePlatformingLevelDuck = this.smallDuckPrefab.Spawn(startPos);
				funhousePlatformingLevelDuck.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelDuck.transform.localScale.y : (-funhousePlatformingLevelDuck.transform.localScale.y)), null);
				funhousePlatformingLevelDuck.smallFirst = (num == 1);
				if ((float)num == this.duckCount - 1f)
				{
					funhousePlatformingLevelDuck.smallLast = true;
					lastDuck = funhousePlatformingLevelDuck;
				}
				else
				{
					funhousePlatformingLevelDuck.smallLast = false;
				}
			}
			else if (this.duckPinkString.Split(new char[]
			{
				','
			})[this.duckPinkIndex][0] == 'R')
			{
				FunhousePlatformingLevelDuck funhousePlatformingLevelDuck2 = this.smallDuckPinkPrefab.Spawn(startPos);
				funhousePlatformingLevelDuck2.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelDuck2.transform.localScale.y : (-funhousePlatformingLevelDuck2.transform.localScale.y)), null);
				funhousePlatformingLevelDuck2.smallFirst = (num == 1);
				if ((float)num == this.duckCount - 1f)
				{
					funhousePlatformingLevelDuck2.smallLast = true;
					lastDuck = funhousePlatformingLevelDuck2;
				}
				else
				{
					funhousePlatformingLevelDuck2.smallLast = false;
				}
			}
			this.duckPinkIndex = (this.duckPinkIndex + 1) % this.duckPinkString.Split(new char[]
			{
				','
			}).Length;
			num++;
		}
		while (lastDuck != null)
		{
			if (lastDuck.transform.position.x < CupheadLevelCamera.Current.transform.position.x)
			{
				break;
			}
			yield return null;
		}
		this.ducksSpawning = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x000DCF3C File Offset: 0x000DB13C
	public IEnumerator spawn_cars_cr()
	{
		this.SpawnHonk((!this.ducksTop) ? this.topSpawnRoot.position.y : this.bottomSpawnRoot.position.y, (float)((!this.ducksTop) ? -1 : 1), (!this.ducksTop) ? -100f : 100f);
		yield return CupheadTime.WaitForSeconds(this, this.carDelay);
		float carSize = this.carPrefabNormal.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		int index = 0;
		FunhousePlatformingLevelCar lastCar = null;
		Vector2 startPos = Vector2.zero;
		for (int i = 0; i < this.carCount; i++)
		{
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + (carSize + this.carSpacing * (float)i);
			startPos.y = ((!this.ducksTop) ? this.topSpawnRoot.position.y : this.bottomSpawnRoot.position.y);
			FunhousePlatformingLevelCar funhousePlatformingLevelCar = Object.Instantiate<FunhousePlatformingLevelCar>(this.carPrefabNormal);
			funhousePlatformingLevelCar.Init(startPos, 180f, this.carSpeed, index, i == 0, i == this.carCount - 1);
			funhousePlatformingLevelCar.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelCar.transform.localScale.y : (-funhousePlatformingLevelCar.transform.localScale.y)), null);
			if (i == this.carCount - 1)
			{
				lastCar = funhousePlatformingLevelCar;
			}
			index = (index + 1) % 4;
		}
		while (lastCar.transform.position.x > CupheadLevelCamera.Current.transform.position.x)
		{
			yield return null;
		}
		this.carsSpawning = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x000DCF58 File Offset: 0x000DB158
	public void SpawnHonk(float rootY, float yScale, float offset)
	{
		AudioManager.Play("funhouse_car_honk_sweet");
		Vector2 vector;
		vector..ctor(CupheadLevelCamera.Current.Bounds.xMax, rootY + offset);
		Effect effect = this.honkEffect.Create(vector);
		effect.transform.parent = CupheadLevelCamera.Current.transform;
		effect.transform.SetScale(null, new float?(yScale), null);
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x000DCFD8 File Offset: 0x000DB1D8
	public override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.topSpawnRoot.transform.position, 50f);
		Gizmos.color = new Color(1f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(this.bottomSpawnRoot.transform.position, 50f);
	}

	// Token: 0x040025AE RID: 9646
	[SerializeField]
	public Effect honkEffect;

	// Token: 0x040025AF RID: 9647
	[SerializeField]
	public Transform topSpawnRoot;

	// Token: 0x040025B0 RID: 9648
	[SerializeField]
	public Transform bottomSpawnRoot;

	// Token: 0x040025B1 RID: 9649
	[Header("Cars")]
	[SerializeField]
	public FunhousePlatformingLevelCar carPrefabNormal;

	// Token: 0x040025B2 RID: 9650
	[SerializeField]
	public float carSpeed;

	// Token: 0x040025B3 RID: 9651
	[SerializeField]
	public float carDelay;

	// Token: 0x040025B4 RID: 9652
	[SerializeField]
	public float carSpacing;

	// Token: 0x040025B5 RID: 9653
	[SerializeField]
	public int carCount;

	// Token: 0x040025B6 RID: 9654
	[Header("Ducks")]
	[SerializeField]
	public FunhousePlatformingLevelDuck bigDuckPrefab;

	// Token: 0x040025B7 RID: 9655
	[SerializeField]
	public FunhousePlatformingLevelDuck smallDuckPrefab;

	// Token: 0x040025B8 RID: 9656
	[SerializeField]
	public FunhousePlatformingLevelDuck smallDuckPinkPrefab;

	// Token: 0x040025B9 RID: 9657
	[SerializeField]
	public float duckDelay;

	// Token: 0x040025BA RID: 9658
	[SerializeField]
	public float duckCount;

	// Token: 0x040025BB RID: 9659
	[SerializeField]
	public float duckSpacing;

	// Token: 0x040025BC RID: 9660
	[SerializeField]
	public string duckPinkString;

	// Token: 0x040025BD RID: 9661
	public int duckPinkIndex;

	// Token: 0x040025BE RID: 9662
	public bool carsSpawning;

	// Token: 0x040025BF RID: 9663
	public bool ducksSpawning;

	// Token: 0x040025C0 RID: 9664
	public bool ducksTop;

	// Token: 0x040025C1 RID: 9665
	public bool firstTime = true;
}
