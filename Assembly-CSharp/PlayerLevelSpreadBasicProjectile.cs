using System;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class PlayerLevelSpreadBasicProjectile : BasicProjectile
{
	// Token: 0x0600390B RID: 14603 RVA: 0x0002E798 File Offset: 0x0002C998
	public override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x0010A830 File Offset: 0x00108A30
	public override void Die()
	{
		base.Die();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
	}

	// Token: 0x0600390D RID: 14605 RVA: 0x0002E7BC File Offset: 0x0002C9BC
	public void _OnDieAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}
}
