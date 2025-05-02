using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public class TreePlatformingLevelBeetleSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002C4F RID: 11343 RVA: 0x000251DC File Offset: 0x000233DC
	public override void Awake()
	{
		base.Awake();
		this.beetles = new TreePlatformingLevelBeetle[base.GetComponentsInChildren<TreePlatformingLevelBeetle>().Length];
		this.beetles = base.GetComponentsInChildren<TreePlatformingLevelBeetle>();
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x00025203 File Offset: 0x00023403
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_play_sfx());
	}

	// Token: 0x06002C51 RID: 11345 RVA: 0x00025218 File Offset: 0x00023418
	public override void Spawn()
	{
		base.Spawn();
		this.Activate();
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x000D97D8 File Offset: 0x000D79D8
	public void Activate()
	{
		int num = Random.Range(0, this.beetles.Length);
		if (!this.beetles[num].isActivated)
		{
			this.beetles[num].Activate();
		}
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x000D9814 File Offset: 0x000D7A14
	public IEnumerator check_to_play_sfx()
	{
		for (;;)
		{
			foreach (TreePlatformingLevelBeetle treePlatformingLevelBeetle in this.beetles)
			{
				if (treePlatformingLevelBeetle.onCamera)
				{
					treePlatformingLevelBeetle.PlayIdleSFX();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400248C RID: 9356
	public TreePlatformingLevelBeetle[] beetles;
}
