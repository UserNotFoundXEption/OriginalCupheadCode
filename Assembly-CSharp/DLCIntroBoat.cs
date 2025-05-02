using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class DLCIntroBoat : AbstractPausableComponent
{
	// Token: 0x060008CD RID: 2253 RVA: 0x00076718 File Offset: 0x00074918
	public void FixedUpdate()
	{
		this.curSpeed = Mathf.Lerp(this.speed.GetFloatAt(1f - this.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f), this.speed.GetFloatAt((1.1f - this.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f) % 1f), 0.5f);
		base.transform.position += Vector3.right * this.curSpeed * CupheadTime.FixedDelta;
	}

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	public Animator boatmanAnimator;

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	public MinMax speed;

	// Token: 0x040006A4 RID: 1700
	public float curSpeed;
}
