using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class BeeLevelHoneyDrip : AbstractMonoBehaviour
{
	// Token: 0x06001124 RID: 4388 RVA: 0x00091DF0 File Offset: 0x0008FFF0
	public BeeLevelHoneyDrip Create()
	{
		return Object.Instantiate<BeeLevelHoneyDrip>(this);
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x00091E08 File Offset: 0x00090008
	public BeeLevelHoneyDrip Create(int number)
	{
		BeeLevelHoneyDrip beeLevelHoneyDrip = this.Create();
		beeLevelHoneyDrip.i = number;
		return beeLevelHoneyDrip;
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x00091E24 File Offset: 0x00090024
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<Animator>().SetInteger("I", Random.Range(0, 6));
		base.transform.SetParent(Camera.main.transform);
		base.transform.SetLocalPosition(new float?((float)Random.Range(-540, 540)), new float?(415f), new float?(100f));
		base.transform.SetParent(null);
		AudioManager.Play("bee_honey_glug_sweet");
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x0000E7AF File Offset: 0x0000C9AF
	public void OnAnimationEnd()
	{
		if (this.i < 4 && Random.value < 0.5f)
		{
			this.Create(this.i + 1);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000DE6 RID: 3558
	public const float START_Y = 415f;

	// Token: 0x04000DE7 RID: 3559
	public const int MAX = 5;

	// Token: 0x04000DE8 RID: 3560
	public int i;
}
