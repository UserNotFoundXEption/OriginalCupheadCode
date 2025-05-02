using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class WeaponFirecrackerEXProjectile : BasicProjectile
{
	// Token: 0x060038C3 RID: 14531 RVA: 0x0002E443 File Offset: 0x0002C643
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x0002E44B File Offset: 0x0002C64B
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			base.StartCoroutine(this.explosion_cr());
		}
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x001091B0 File Offset: 0x001073B0
	public IEnumerator explosion_cr()
	{
		this.move = false;
		base.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002D97 RID: 11671
	public float bulletLife;

	// Token: 0x04002D98 RID: 11672
	public float explosionSize;

	// Token: 0x04002D99 RID: 11673
	public float explosionDuration;
}
