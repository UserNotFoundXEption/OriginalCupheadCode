using System;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class TrainLevelPassengerCar : AbstractTrainLevelTrainCar
{
	// Token: 0x06002A1D RID: 10781 RVA: 0x000D3454 File Offset: 0x000D1654
	public void Explode(int i)
	{
		base.animator.SetInteger("State", i);
		base.animator.SetTrigger("OnDamaged");
		if (i != 0)
		{
			if (i == 1)
			{
				this.explosionEffects[1].Create(base.transform.position);
			}
		}
		else
		{
			this.explosionEffects[0].Create(base.transform.position);
		}
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x00023736 File Offset: 0x00021936
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionEffects = null;
	}

	// Token: 0x04002326 RID: 8998
	[SerializeField]
	public Effect[] explosionEffects;
}
