using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class TrainLevelGhostCannonGhostSkull : AbstractProjectile
{
	// Token: 0x060029E1 RID: 10721 RVA: 0x000D2D3C File Offset: 0x000D0F3C
	public TrainLevelGhostCannonGhostSkull Create(Vector3 pos, float speed)
	{
		TrainLevelGhostCannonGhostSkull trainLevelGhostCannonGhostSkull = Object.Instantiate<TrainLevelGhostCannonGhostSkull>(this);
		trainLevelGhostCannonGhostSkull.transform.position = pos;
		trainLevelGhostCannonGhostSkull.maxSpeed = speed;
		return trainLevelGhostCannonGhostSkull;
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060029E2 RID: 10722 RVA: 0x00023447 File Offset: 0x00021647
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x0002344E File Offset: 0x0002164E
	public override void Start()
	{
		base.Start();
		this.SetParryable(true);
		base.StartCoroutine(this.speed_cr());
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000D2D64 File Offset: 0x000D0F64
	public override void Update()
	{
		base.Update();
		if (!base.dead && base.transform.position.y < -325f)
		{
			this.Die();
		}
		base.transform.AddPosition(0f, -this.speed * CupheadTime.Delta, 0f);
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000D2DCC File Offset: 0x000D0FCC
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (hit.tag == "ParrySwitch")
			{
				ParrySwitch component = hit.GetComponent<ParrySwitch>();
				if (component.name != "Right" && component.name != "Left")
				{
					return;
				}
				component.ActivateFromOtherSource();
				this.Die();
			}
			else if (hit.name == "HandCar")
			{
				this.Die();
			}
		}
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x0002346A File Offset: 0x0002166A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000D2E5C File Offset: 0x000D105C
	public IEnumerator speed_cr()
	{
		yield return base.TweenPositionY(base.transform.position.y, base.transform.position.y + 100f, 0.4f, EaseUtils.EaseType.easeOutCubic);
		yield return base.StartCoroutine(this.tweenSpeed_cr(0f, this.maxSpeed, 0.4f, EaseUtils.EaseType.linear));
		yield break;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x000D2E78 File Offset: 0x000D1078
	public IEnumerator tweenSpeed_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.speed = EaseUtils.Ease(ease, start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.speed = this.maxSpeed;
		yield break;
	}

	// Token: 0x0400230A RID: 8970
	public const float DEATH_Y = -325f;

	// Token: 0x0400230B RID: 8971
	public float maxSpeed;

	// Token: 0x0400230C RID: 8972
	public float speed;
}
