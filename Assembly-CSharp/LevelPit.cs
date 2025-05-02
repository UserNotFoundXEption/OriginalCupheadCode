using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class LevelPit : AbstractCollidableObject
{
	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0000B83C File Offset: 0x00009A3C
	// (set) Token: 0x06000D74 RID: 3444 RVA: 0x0000B843 File Offset: 0x00009A43
	public static LevelPit Instance { get; set; }

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0000B84B File Offset: 0x00009A4B
	// (set) Token: 0x06000D76 RID: 3446 RVA: 0x0000B853 File Offset: 0x00009A53
	public float ExtraOffset
	{
		get
		{
			return this.extraOffset;
		}
		set
		{
			this.extraOffset = value;
		}
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x00087270 File Offset: 0x00085470
	public void Start()
	{
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			LevelPit.Instance = this;
			base.transform.SetParent(CupheadLevelCamera.Current.transform);
			base.transform.ResetLocalTransforms();
			base.transform.SetLocalPosition(new float?(0f), new float?(-500f), new float?(0f));
		}
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x0000B85C File Offset: 0x00009A5C
	public void FixedUpdate()
	{
		this.CheckPlayer(PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController);
		this.CheckPlayer(PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController);
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x000872DC File Offset: 0x000854DC
	public void CheckPlayer(LevelPlayerController player)
	{
		if (player == null || player.IsDead)
		{
			return;
		}
		float num = 1f;
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			num *= 1.3f;
		}
		num *= this.forceMultiplier;
		if (player.motor.GravityReversed && Level.Current.LevelType == Level.Type.Platforming)
		{
			float num2 = base.transform.parent.position.y - base.transform.localPosition.y;
			if (player.transform.position.y >= num2 - this.extraOffset)
			{
				player.OnPitKnockUp(num2 - this.extraOffset, num);
			}
		}
		else if (player.transform.position.y <= base.transform.position.y + this.extraOffset)
		{
			player.OnPitKnockUp(base.transform.position.y + this.extraOffset, num);
		}
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0000B880 File Offset: 0x00009A80
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.3f);
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0000B893 File Offset: 0x00009A93
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x00087400 File Offset: 0x00085600
	public void DrawGizmos(float a)
	{
		Rect rect;
		rect..ctor(base.baseTransform.position.x + -1000f, base.baseTransform.position.y, 2000f, 0f);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawLine(new Vector2(rect.xMin, rect.y), new Vector2(rect.xMax, rect.y));
		for (int i = 0; i < 20; i++)
		{
			float num = 100f;
			Rect rect2;
			rect2..ctor(rect.xMin + num * (float)i, rect.y, num, -20f);
			Gizmos.DrawLine(new Vector2(rect2.xMin, rect2.y), new Vector2(rect2.center.x, rect2.yMax));
			Gizmos.DrawLine(new Vector2(rect2.xMax, rect2.y), new Vector2(rect2.center.x, rect2.yMax));
		}
	}

	// Token: 0x04000A7C RID: 2684
	public const float PLATFORMING_LEVEL_CAMERA_OFFSET_Y = -500f;

	// Token: 0x04000A7D RID: 2685
	[SerializeField]
	public float extraOffset;

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	public float forceMultiplier = 1f;
}
