using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class TreePlatformingLevelButterflySpawner : AbstractPausableComponent
{
	// Token: 0x06002C62 RID: 11362 RVA: 0x00025272 File Offset: 0x00023472
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.instantiate_butterflies());
		base.StartCoroutine(this.spawner_cr());
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000D9B54 File Offset: 0x000D7D54
	public IEnumerator instantiate_butterflies()
	{
		this.butterflies = new List<TreePlatformingLevelButterfly>();
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		for (int i = 0; i < 5; i++)
		{
			TreePlatformingLevelButterfly treePlatformingLevelButterfly = Object.Instantiate<TreePlatformingLevelButterfly>(this.butterflySmall);
			treePlatformingLevelButterfly.transform.parent = base.transform;
			treePlatformingLevelButterfly.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 10000f);
			this.butterflies.Add(treePlatformingLevelButterfly);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000D9B70 File Offset: 0x000D7D70
	public IEnumerator spawner_cr()
	{
		bool keepChecking = true;
		TreePlatformingLevelButterfly spawn = null;
		yield return CupheadTime.WaitForSeconds(this, this.initalDelay);
		for (;;)
		{
			while (PlayerManager.GetNext().transform.position.x < this.startButterflies.position.x)
			{
				yield return null;
			}
			if (this.endButterflies != null)
			{
				while (PlayerManager.GetNext().transform.position.y > this.endButterflies.position.y)
				{
					yield return null;
				}
			}
			keepChecking = true;
			while (keepChecking)
			{
				foreach (TreePlatformingLevelButterfly treePlatformingLevelButterfly in this.butterflies)
				{
					if (!treePlatformingLevelButterfly.isActive)
					{
						spawn = treePlatformingLevelButterfly;
						keepChecking = false;
						break;
					}
				}
				yield return null;
			}
			bool onLeft = Rand.Bool();
			float y = Random.Range(CupheadLevelCamera.Current.Bounds.yMin + 50f, CupheadLevelCamera.Current.Bounds.yMax - 50f);
			float x = (!onLeft) ? (CupheadLevelCamera.Current.Bounds.xMax + 50f) : (CupheadLevelCamera.Current.Bounds.xMin - 50f);
			float scale = (float)((!onLeft) ? -1 : 1);
			spawn.transform.position = new Vector3(x, y);
			spawn.Init(new Vector2((!onLeft) ? (-this.velocityX.RandomFloat()) : this.velocityX.RandomFloat(), (float)((!Rand.Bool()) ? (-(float)this.velocityY) : this.velocityY)), scale, Random.Range(1, 5), this.velocityX);
			yield return CupheadTime.WaitForSeconds(this, this.delay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000D9B8C File Offset: 0x000D7D8C
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(this.startButterflies.position + new Vector3(0f, 1000f), this.startButterflies.position + new Vector3(0f, -1000f));
		Gizmos.color = Color.yellow;
		if (this.endButterflies != null)
		{
			Gizmos.DrawLine(this.endButterflies.position + new Vector3(1000f, 0f), this.endButterflies.position + new Vector3(-1000f, 0f));
		}
	}

	// Token: 0x04002498 RID: 9368
	public const int BUTTERFLIES = 5;

	// Token: 0x04002499 RID: 9369
	public const float OFFSET = 50f;

	// Token: 0x0400249A RID: 9370
	[SerializeField]
	public TreePlatformingLevelButterfly butterflySmall;

	// Token: 0x0400249B RID: 9371
	[SerializeField]
	public Transform startButterflies;

	// Token: 0x0400249C RID: 9372
	[SerializeField]
	public Transform endButterflies;

	// Token: 0x0400249D RID: 9373
	public List<TreePlatformingLevelButterfly> butterflies;

	// Token: 0x0400249E RID: 9374
	public MinMax delay = new MinMax(1.5f, 3f);

	// Token: 0x0400249F RID: 9375
	public MinMax velocityX = new MinMax(300f, 600f);

	// Token: 0x040024A0 RID: 9376
	public MinMax velocityY = new MinMax(100f, 200f);

	// Token: 0x040024A1 RID: 9377
	public float initalDelay = 6f;
}
