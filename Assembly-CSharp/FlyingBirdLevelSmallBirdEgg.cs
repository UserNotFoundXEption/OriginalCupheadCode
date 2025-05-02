using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class FlyingBirdLevelSmallBirdEgg : AbstractCollidableObject
{
	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06001A07 RID: 6663 RVA: 0x000161EC File Offset: 0x000143EC
	// (set) Token: 0x06001A08 RID: 6664 RVA: 0x000161F4 File Offset: 0x000143F4
	public Transform container { get; set; }

	// Token: 0x06001A09 RID: 6665 RVA: 0x000161FD File Offset: 0x000143FD
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.StartCoroutine(this.animSpeed_cr());
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x0001621D File Offset: 0x0001441D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.UpdateRotation();
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x0001623B File Offset: 0x0001443B
	public void UpdateRotation()
	{
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x0001623D File Offset: 0x0001443D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000A7B58 File Offset: 0x000A5D58
	public void SetParent(Transform parent, LevelProperties.FlyingBird properties)
	{
		this.properties = properties;
		this.container = new GameObject("Egg Container").transform;
		this.container.SetParent(parent);
		this.container.ResetLocalPosition();
		base.transform.SetParent(this.container);
		base.transform.ResetLocalTransforms();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x00016266 File Offset: 0x00014466
	public void Explode()
	{
		base.GetComponent<CircleCollider2D>().enabled = false;
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00016281 File Offset: 0x00014481
	public void OnDeathAnimComplete()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x000A7BC4 File Offset: 0x000A5DC4
	public IEnumerator move_cr()
	{
		yield return base.TweenLocalPositionX(0f, this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeOutCubic);
		for (;;)
		{
			yield return base.TweenLocalPositionX(this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggRange.min, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(this.properties.CurrentState.smallBird.eggRange.min, this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x000A7BE0 File Offset: 0x000A5DE0
	public IEnumerator explode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, (float)Random.Range(0, 1));
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		base.animator.Play("Explode");
		yield break;
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x000A7BFC File Offset: 0x000A5DFC
	public IEnumerator animSpeed_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x040014E7 RID: 5351
	public DamageDealer damageDealer;

	// Token: 0x040014E8 RID: 5352
	public LevelProperties.FlyingBird properties;
}
