using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public abstract class PlatformingLevelEnemySpawner : AbstractPausableComponent
{
	// Token: 0x06002B5C RID: 11100 RVA: 0x000D5BA0 File Offset: 0x000D3DA0
	public PlatformingLevelEnemySpawner()
	{
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x000D5C18 File Offset: 0x000D3E18
	public virtual void Start()
	{
		this.started = false;
		this.ended = false;
		Vector2 vector = base.transform.position;
		this.startRect = RectUtils.NewFromCenter(this.startTrigger.Position.x + vector.x, this.startTrigger.Position.y + vector.y, this.startTrigger.Size.x, this.startTrigger.Size.y);
		this.stopRect = RectUtils.NewFromCenter(this.stopTrigger.Position.x + vector.x, this.stopTrigger.Position.y + vector.y, this.stopTrigger.Size.x, this.stopTrigger.Size.y);
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x000D5CFC File Offset: 0x000D3EFC
	public void Update()
	{
		if (this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStartTriggerHit();
		}
		if (this.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStopTriggerHit();
		}
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000246EE File Offset: 0x000228EE
	public void OnStartTriggerHit()
	{
		if (this.started)
		{
			return;
		}
		this.started = true;
		this.StartSpawning();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x00024716 File Offset: 0x00022916
	public void OnStopTriggerHit()
	{
		if (!this.started)
		{
			return;
		}
		if (this.ended)
		{
			return;
		}
		this.ended = true;
		this.EndSpawning();
		this.StopAllCoroutines();
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00024743 File Offset: 0x00022943
	public virtual void EndSpawning()
	{
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x00024745 File Offset: 0x00022945
	public virtual void StartSpawning()
	{
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x00024747 File Offset: 0x00022947
	public virtual void Spawn()
	{
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x000D5DA4 File Offset: 0x000D3FA4
	public IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initalSpawnDelay.RandomFloat());
		for (;;)
		{
			this.Spawn();
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x00024749 File Offset: 0x00022949
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x0002475C File Offset: 0x0002295C
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x000D5DC0 File Offset: 0x000D3FC0
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.startTrigger.Position, this.startTrigger.Size);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.stopTrigger.Position, this.stopTrigger.Size);
	}

	// Token: 0x040023F1 RID: 9201
	public bool destroyEnemyAfterLeavingScreen = true;

	// Token: 0x040023F2 RID: 9202
	[Header("Spawning Properties")]
	public MinMax spawnDelay = new MinMax(2f, 2f);

	// Token: 0x040023F3 RID: 9203
	public MinMax initalSpawnDelay = new MinMax(0f, 0f);

	// Token: 0x040023F4 RID: 9204
	[Header("Triggers")]
	public PlatformingLevelEnemySpawner.TriggerProperties startTrigger = new PlatformingLevelEnemySpawner.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x040023F5 RID: 9205
	public PlatformingLevelEnemySpawner.TriggerProperties stopTrigger = new PlatformingLevelEnemySpawner.TriggerProperties(new Vector2(200f, 0f));

	// Token: 0x040023F6 RID: 9206
	public bool started;

	// Token: 0x040023F7 RID: 9207
	public bool ended;

	// Token: 0x040023F8 RID: 9208
	public Rect startRect;

	// Token: 0x040023F9 RID: 9209
	public Rect stopRect;

	// Token: 0x02000FFD RID: 4093
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x060076EE RID: 30446 RVA: 0x00050E4F File Offset: 0x0004F04F
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x04007279 RID: 29305
		public Vector2 Position = Vector2.zero;

		// Token: 0x0400727A RID: 29306
		public Vector2 Size = Vector2.one * 100f;
	}
}
