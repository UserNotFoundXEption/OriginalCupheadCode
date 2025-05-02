using System;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class HarbourPlatformingLevelIcebergSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002E28 RID: 11816 RVA: 0x00026805 File Offset: 0x00024A05
	public override void Start()
	{
		base.Start();
		this.spawn = this.spawnDelayString.Split(new char[]
		{
			','
		});
		this.spawnIndex = Random.Range(0, this.spawn.Length);
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x000DEC34 File Offset: 0x000DCE34
	public override void Spawn()
	{
		this.spawnDelay.min = Parser.FloatParse(this.spawn[this.spawnIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.spawn[this.spawnIndex]);
		int num = Random.Range(0, this.icebergPrefabs.Length);
		float num2 = CupheadLevelCamera.Current.transform.position.x + CupheadLevelCamera.Current.Width / 2f + (this.icebergPrefabs[num].GetComponent<Renderer>().bounds.size.x + this.icebergPrefabs[num].GetComponent<Renderer>().bounds.size.x / 2f);
		float num3 = CupheadLevelCamera.Current.transform.position.y - 100f;
		this.icebergPrefabs[num].Spawn(new Vector3(num2, num3));
		base.Spawn();
		this.spawnIndex = (this.spawnIndex + 1) % this.spawn.Length;
	}

	// Token: 0x0400263E RID: 9790
	[SerializeField]
	public HarbourPlatformingLevelIceberg[] icebergPrefabs;

	// Token: 0x0400263F RID: 9791
	[SerializeField]
	public string spawnDelayString = "5.5,7.0";

	// Token: 0x04002640 RID: 9792
	public string[] spawn;

	// Token: 0x04002641 RID: 9793
	public int spawnIndex;
}
