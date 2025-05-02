using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class FlyingCowboyLevelSausageLink : BasicProjectile
{
	// Token: 0x06001BD2 RID: 7122 RVA: 0x000ACDAC File Offset: 0x000AAFAC
	public void Initialize(FlyingCowboyLevelMeat.SausageType sausageType, Transform sausageLinkSqueezePoint, FlyingCowboyLevelSausageLink previousLink)
	{
		this.sausageType = sausageType;
		if (sausageType != FlyingCowboyLevelMeat.SausageType.U1 && sausageType != FlyingCowboyLevelMeat.SausageType.U2 && sausageType != FlyingCowboyLevelMeat.SausageType.U3)
		{
			base.StartCoroutine(this.squeeze_cr(sausageLinkSqueezePoint, previousLink));
		}
		if (sausageType == FlyingCowboyLevelMeat.SausageType.H1 || sausageType == FlyingCowboyLevelMeat.SausageType.H2 || sausageType == FlyingCowboyLevelMeat.SausageType.H3 || sausageType == FlyingCowboyLevelMeat.SausageType.H4 || sausageType == FlyingCowboyLevelMeat.SausageType.L5)
		{
			base.animator.SetFloat("Speed", (float)Rand.PosOrNeg());
		}
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000178C1 File Offset: 0x00015AC1
	public void Squeeze()
	{
		base.animator.Play("Squeeze" + this.sausageType.ToString());
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000ACE1C File Offset: 0x000AB01C
	public IEnumerator squeeze_cr(Transform sausageLinkSqueezePoint, FlyingCowboyLevelSausageLink previousLink)
	{
		while (base.transform.position.x > sausageLinkSqueezePoint.position.x)
		{
			yield return null;
		}
		this.Squeeze();
		if (previousLink != null)
		{
			previousLink.Squeeze();
		}
		yield break;
	}

	// Token: 0x040016A0 RID: 5792
	public FlyingCowboyLevelMeat.SausageType sausageType;
}
