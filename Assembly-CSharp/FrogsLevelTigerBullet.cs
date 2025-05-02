using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A3 RID: 675
public class FrogsLevelTigerBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x06001E56 RID: 7766 RVA: 0x00019928 File Offset: 0x00017B28
	public override void Start()
	{
		base.Start();
		this.bullet.OnPlayerCollision += base.DealDamage;
		base.StartCoroutine(this.bullet_cr());
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x000B2A94 File Offset: 0x000B0C94
	public IEnumerator bullet_cr()
	{
		float t = 0f;
		Transform trans = this.bullet.transform;
		float start = trans.localPosition.y;
		float end = start + 500f;
		for (;;)
		{
			t = 0f;
			AudioManager.Play("level_frogs_ball_platform_ball_launch");
			while (t < 0.5f)
			{
				float val = t / 0.5f;
				float y = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, end, val);
				trans.SetLocalPosition(null, new float?(y), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < 0.5f)
			{
				float val2 = t / 0.5f;
				float y2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, end, start, val2);
				trans.SetLocalPosition(null, new float?(y2), null);
				t += CupheadTime.Delta;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040018C6 RID: 6342
	public const float BULLET_TIME = 0.5f;

	// Token: 0x040018C7 RID: 6343
	public const float BULLET_HEIGHT = 500f;

	// Token: 0x040018C8 RID: 6344
	public CollisionChild bullet;
}
