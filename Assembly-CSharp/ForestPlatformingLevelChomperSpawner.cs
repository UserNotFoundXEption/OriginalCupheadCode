using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class ForestPlatformingLevelChomperSpawner : AbstractPausableComponent
{
	// Token: 0x06002C09 RID: 11273 RVA: 0x000D9058 File Offset: 0x000D7258
	public void Start()
	{
		this.started = false;
		Vector2 vector = base.transform.position;
		this.startRect = RectUtils.NewFromCenter(this.startTrigger.Position.x + vector.x, this.startTrigger.Position.y + vector.y, this.startTrigger.Size.x + Random.Range(-this.startTrigger.xVariation, this.startTrigger.xVariation), this.startTrigger.Size.y);
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x000D90F8 File Offset: 0x000D72F8
	public void Update()
	{
		if (this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStartTriggerHit();
		}
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x000D9154 File Offset: 0x000D7354
	public void OnStartTriggerHit()
	{
		if (this.started)
		{
			return;
		}
		this.started = true;
		foreach (ForestPlatformingLevelChomper forestPlatformingLevelChomper in this.chompers)
		{
			if (forestPlatformingLevelChomper != null)
			{
				forestPlatformingLevelChomper.StartAttacking();
			}
		}
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x00024E30 File Offset: 0x00023030
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x00024E43 File Offset: 0x00023043
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x000D91A8 File Offset: 0x000D73A8
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.startTrigger.Position, this.startTrigger.Size);
	}

	// Token: 0x0400247A RID: 9338
	[Header("Triggers")]
	public ForestPlatformingLevelChomperSpawner.TriggerProperties startTrigger = new ForestPlatformingLevelChomperSpawner.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x0400247B RID: 9339
	[Header("Chompers")]
	public ForestPlatformingLevelChomper[] chompers;

	// Token: 0x0400247C RID: 9340
	public bool started;

	// Token: 0x0400247D RID: 9341
	public Rect startRect;

	// Token: 0x0200101C RID: 4124
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x0600776D RID: 30573 RVA: 0x000512FC File Offset: 0x0004F4FC
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x0400730A RID: 29450
		public Vector2 Position = Vector2.zero;

		// Token: 0x0400730B RID: 29451
		public Vector2 Size = Vector2.one * 100f;

		// Token: 0x0400730C RID: 29452
		public float xVariation;
	}
}
