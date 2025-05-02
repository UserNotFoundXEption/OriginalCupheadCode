using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class TrainLevelPumpkinProjectile : AbstractProjectile
{
	// Token: 0x06002A35 RID: 10805 RVA: 0x0002382C File Offset: 0x00021A2C
	public override void Start()
	{
		base.Start();
		this.SetParryable(true);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x06002A36 RID: 10806 RVA: 0x000D3898 File Offset: 0x000D1A98
	public override void Update()
	{
		base.Update();
		if (!this.hasDied && base.transform.position.y < -325f)
		{
			this.Die();
		}
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x000D38DC File Offset: 0x000D1ADC
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

	// Token: 0x06002A38 RID: 10808 RVA: 0x00023848 File Offset: 0x00021A48
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x00023871 File Offset: 0x00021A71
	public void Drop()
	{
		base.transform.SetParent(null);
		this.StopAllCoroutines();
		base.animator.Play("Fall");
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x000238A2 File Offset: 0x00021AA2
	public override void Die()
	{
		if (this.hasDied)
		{
			return;
		}
		this.hasDied = true;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x000D396C File Offset: 0x000D1B6C
	public IEnumerator float_cr()
	{
		float top = base.transform.localPosition.y;
		float bottom = top - 20f;
		float time = 0.4f;
		for (;;)
		{
			yield return base.TweenLocalPositionY(top, bottom, time, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionY(bottom, top, time, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x000D3988 File Offset: 0x000D1B88
	public IEnumerator drop_cr()
	{
		float top = base.transform.position.y;
		yield return base.TweenPositionY(top, -340f, this.fallTime, EaseUtils.EaseType.easeInSine);
		this.Die();
		yield break;
	}

	// Token: 0x0400233B RID: 9019
	public const float DEATH_Y = -325f;

	// Token: 0x0400233C RID: 9020
	public float fallTime;

	// Token: 0x0400233D RID: 9021
	public bool hasDied;
}
