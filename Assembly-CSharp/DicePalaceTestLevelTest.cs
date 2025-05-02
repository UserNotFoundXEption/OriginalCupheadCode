using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class DicePalaceTestLevelTest : LevelProperties.DicePalaceTest.Entity
{
	// Token: 0x060017F1 RID: 6129 RVA: 0x000A2AFC File Offset: 0x000A0CFC
	public IEnumerator start_it_cr()
	{
		base.StartCoroutine(this.shoot_right());
		base.StartCoroutine(this.shoot_right_2());
		base.StartCoroutine(this.shoot_left());
		yield return null;
		yield break;
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000A2B18 File Offset: 0x000A0D18
	public IEnumerator shoot_right()
	{
		for (;;)
		{
			this.basic.Create(this.shoot1.transform.position, 0f, this.speed);
			yield return CupheadTime.WaitForSeconds(this, 2f);
		}
		yield break;
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000A2B34 File Offset: 0x000A0D34
	public IEnumerator shoot_right_2()
	{
		for (;;)
		{
			this.basic.Create(this.shoot3.transform.position, 0f, this.speed);
			yield return CupheadTime.WaitForSeconds(this, 2f);
		}
		yield break;
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000A2B50 File Offset: 0x000A0D50
	public IEnumerator shoot_left()
	{
		for (;;)
		{
			this.basic.Create(this.shoot2.transform.position, 0f, -this.speed);
			yield return CupheadTime.WaitForSeconds(this, 2f);
		}
		yield break;
	}

	// Token: 0x04001364 RID: 4964
	[SerializeField]
	public BasicProjectile basic;

	// Token: 0x04001365 RID: 4965
	[SerializeField]
	public Transform shoot1;

	// Token: 0x04001366 RID: 4966
	[SerializeField]
	public Transform shoot2;

	// Token: 0x04001367 RID: 4967
	[SerializeField]
	public Transform shoot3;

	// Token: 0x04001368 RID: 4968
	public float speed = 200f;
}
