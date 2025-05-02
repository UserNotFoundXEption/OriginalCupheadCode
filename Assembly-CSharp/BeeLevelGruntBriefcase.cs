using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class BeeLevelGruntBriefcase : AbstractProjectile
{
	// Token: 0x0600111E RID: 4382 RVA: 0x00091D54 File Offset: 0x0008FF54
	public BeeLevelGruntBriefcase Create(int xScale, Vector2 pos)
	{
		BeeLevelGruntBriefcase beeLevelGruntBriefcase = Object.Instantiate<BeeLevelGruntBriefcase>(this);
		beeLevelGruntBriefcase.transform.position = pos;
		beeLevelGruntBriefcase.transform.SetScale(new float?((float)xScale), new float?(1f), new float?(1f));
		beeLevelGruntBriefcase.CollisionDeath.OnlyPlayer();
		beeLevelGruntBriefcase.DamagesType.OnlyPlayer();
		return beeLevelGruntBriefcase;
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0000E767 File Offset: 0x0000C967
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0000E789 File Offset: 0x0000C989
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x00091DB8 File Offset: 0x0008FFB8
	public IEnumerator x_cr()
	{
		for (;;)
		{
			if (base.transform.position.x < -1280f || base.transform.position.x > 1280f)
			{
				Object.Destroy(base.gameObject);
			}
			base.transform.AddPosition(200f * CupheadTime.Delta * -base.transform.localScale.x, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x00091DD4 File Offset: 0x0008FFD4
	public IEnumerator y_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			float val = t / time;
			float speed = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 1500f, 0f, val) * CupheadTime.Delta;
			base.transform.AddPosition(0f, speed, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < time)
		{
			float val2 = t / time;
			float speed2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, -1500f, val2) * CupheadTime.Delta;
			base.transform.AddPosition(0f, speed2, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (;;)
		{
			if (base.transform.position.y < -720f)
			{
				Object.Destroy(base.gameObject);
			}
			base.transform.AddPosition(0f, -1500f * CupheadTime.Delta, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000DE3 RID: 3555
	public const float Y_SPEED = 1500f;

	// Token: 0x04000DE4 RID: 3556
	public const float X_SPEED = 200f;

	// Token: 0x04000DE5 RID: 3557
	public const float TIME = 0.5f;
}
