using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000240 RID: 576
public class FlyingBlimpLevelEnemyProjectile : BasicProjectile
{
	// Token: 0x06001A84 RID: 6788 RVA: 0x000168FB File Offset: 0x00014AFB
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x00016910 File Offset: 0x00014B10
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.animator.SetTrigger("dead");
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0001692A File Offset: 0x00014B2A
	public void Destroy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000A8EFC File Offset: 0x000A70FC
	public IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.FX.Create(this.root.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x04001548 RID: 5448
	[SerializeField]
	public Effect FX;

	// Token: 0x04001549 RID: 5449
	[SerializeField]
	public Transform root;
}
