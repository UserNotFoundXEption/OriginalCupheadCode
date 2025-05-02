using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class TrainLevelLollipopGhoulLightning : AbstractCollidableObject
{
	// Token: 0x06002A03 RID: 10755 RVA: 0x0002360B File Offset: 0x0002180B
	public void Start()
	{
		base.StartCoroutine(this.start_cr());
		this.damageDealer = DamageDealer.NewEnemy(0.2f);
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x0002362A File Offset: 0x0002182A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x00023642 File Offset: 0x00021842
	public void End()
	{
		base.StartCoroutine(this.end_cr());
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x00023651 File Offset: 0x00021851
	public void GoAway()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x00023664 File Offset: 0x00021864
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer == null)
		{
			return;
		}
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000D3180 File Offset: 0x000D1380
	public IEnumerator start_cr()
	{
		base.animator.SetTrigger("OnStart");
		yield return base.animator.WaitForAnimationToStart(this, "Loop", false);
		base.animator.SetBool("isFX", true);
		base.animator.SetTrigger("OnDustStart");
		yield break;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000D319C File Offset: 0x000D139C
	public IEnumerator end_cr()
	{
		base.animator.SetTrigger("OnEnd");
		base.animator.SetBool("isFX", false);
		yield return base.animator.WaitForAnimationToStart(this, "Init", false);
		base.animator.SetTrigger("OnDustEnd");
		yield return base.animator.WaitForAnimationToStart(this, "Init", 2, false);
		this.GoAway();
		yield break;
	}

	// Token: 0x0400231B RID: 8987
	[SerializeField]
	public Transform spark1;

	// Token: 0x0400231C RID: 8988
	[SerializeField]
	public Transform spark2;

	// Token: 0x0400231D RID: 8989
	public DamageDealer damageDealer;
}
