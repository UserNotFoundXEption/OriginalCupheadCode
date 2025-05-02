using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
public class PlatformingLevelPitMoveTrigger : AbstractPausableComponent
{
	// Token: 0x06002BC4 RID: 11204 RVA: 0x000D796C File Offset: 0x000D5B6C
	public void Start()
	{
		Vector2 vector = base.transform.position;
		this.rect = RectUtils.NewFromCenter(this.trigger.Position.x + vector.x, this.trigger.Position.y + vector.y, this.trigger.Size.x, this.trigger.Size.y);
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x000D79E8 File Offset: 0x000D5BE8
	public void Update()
	{
		if (this.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnTriggerHit();
		}
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x00024B75 File Offset: 0x00022D75
	public void OnTriggerHit()
	{
		LevelPit.Instance.ExtraOffset = this.pitOffset;
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x00024B87 File Offset: 0x00022D87
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x00024B9A File Offset: 0x00022D9A
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000D7A44 File Offset: 0x000D5C44
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.trigger.Position, this.trigger.Size);
	}

	// Token: 0x04002440 RID: 9280
	[SerializeField]
	public float pitOffset;

	// Token: 0x04002441 RID: 9281
	[Header("Triggers")]
	public PlatformingLevelPitMoveTrigger.TriggerProperties trigger = new PlatformingLevelPitMoveTrigger.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x04002442 RID: 9282
	public Rect rect;

	// Token: 0x0200100D RID: 4109
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x0600772E RID: 30510 RVA: 0x000510C4 File Offset: 0x0004F2C4
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x040072BE RID: 29374
		public Vector2 Position = Vector2.zero;

		// Token: 0x040072BF RID: 29375
		public Vector2 Size = Vector2.one * 100f;
	}
}
