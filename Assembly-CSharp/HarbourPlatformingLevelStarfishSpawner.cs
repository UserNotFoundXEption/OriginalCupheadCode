using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class HarbourPlatformingLevelStarfishSpawner : AbstractPausableComponent
{
	// Token: 0x06002E71 RID: 11889 RVA: 0x000DF978 File Offset: 0x000DDB78
	public void Start()
	{
		this.speedX = this.speedXString.Split(new char[]
		{
			','
		});
		this.speedXIndex = Random.Range(0, this.speedX.Length);
		this.typeIndex = Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x000DF9E8 File Offset: 0x000DDBE8
	public IEnumerator spawn_cr()
	{
		bool hashadSuccessfulSpawn = false;
		for (;;)
		{
			if (hashadSuccessfulSpawn)
			{
				yield return CupheadTime.WaitForSeconds(this, this.spawnTime.RandomFloat());
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.initialSpawnTime.RandomFloat());
			}
			Vector2 spawnPos = base.transform.position;
			spawnPos.y = base.transform.position.y;
			spawnPos.x += Random.Range(-this.xRange, this.xRange);
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				this.starfishPrefab.Spawn(spawnPos).Init(90f, Parser.FloatParse(this.speedX[this.speedXIndex]), this.speedYRange.RandomFloat(), this.loopSize, this.typeString.Split(new char[]
				{
					','
				})[this.typeIndex]);
				hashadSuccessfulSpawn = true;
				this.speedXIndex = (this.speedXIndex + 1) % this.speedX.Length;
				this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
				{
					','
				}).Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E73 RID: 11891 RVA: 0x000DFA04 File Offset: 0x000DDC04
	public override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04002687 RID: 9863
	[SerializeField]
	public MinMax initialSpawnTime;

	// Token: 0x04002688 RID: 9864
	[SerializeField]
	public MinMax spawnTime;

	// Token: 0x04002689 RID: 9865
	[SerializeField]
	public HarbourPlatformingLevelStarfish starfishPrefab;

	// Token: 0x0400268A RID: 9866
	[SerializeField]
	public string speedXString;

	// Token: 0x0400268B RID: 9867
	[SerializeField]
	public string typeString;

	// Token: 0x0400268C RID: 9868
	[SerializeField]
	public MinMax speedYRange;

	// Token: 0x0400268D RID: 9869
	[SerializeField]
	public float xRange;

	// Token: 0x0400268E RID: 9870
	[SerializeField]
	public float loopSize;

	// Token: 0x0400268F RID: 9871
	public int typeIndex;

	// Token: 0x04002690 RID: 9872
	public int speedXIndex;

	// Token: 0x04002691 RID: 9873
	public string[] speedX;
}
