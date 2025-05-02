using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class TestLevelShootableTimer : AbstractCollidableObject
{
	// Token: 0x06000D8D RID: 3469 RVA: 0x00087760 File Offset: 0x00085960
	public void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.child.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0000B927 File Offset: 0x00009B27
	public void Update()
	{
		if (Input.GetKeyDown(116))
		{
			this.timerStarted = true;
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0000B93C File Offset: 0x00009B3C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.timerStarted)
		{
			this.damageTaken += info.damage;
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x000877B4 File Offset: 0x000859B4
	public IEnumerator timer_cr()
	{
		for (;;)
		{
			float t = 0f;
			if (this.timerStarted)
			{
				while (t < this.maxTime)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				yield return null;
				this.damageTaken = 0f;
				this.timerStarted = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	public float maxTime = 3f;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	public DamageReceiver child;

	// Token: 0x04000A94 RID: 2708
	public DamageReceiver damageReceiver;

	// Token: 0x04000A95 RID: 2709
	public float damageTaken;

	// Token: 0x04000A96 RID: 2710
	public bool timerStarted;
}
