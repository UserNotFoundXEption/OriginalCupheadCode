using System;
using UnityEngine;

// Token: 0x02000534 RID: 1332
public class WeaponAccuracyProjectile : BasicProjectile
{
	// Token: 0x0600382D RID: 14381 RVA: 0x0002DDEA File Offset: 0x0002BFEA
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.hitEnemy = true;
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x0002DDFB File Offset: 0x0002BFFB
	public override void OnDestroy()
	{
		if (this.EnemyDeath != null)
		{
			this.EnemyDeath(this.hitEnemy);
		}
		base.OnDestroy();
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x0002DE1F File Offset: 0x0002C01F
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x00106200 File Offset: 0x00104400
	public void SetSize(float size)
	{
		base.transform.SetScale(new float?(size), new float?(size), null);
	}

	// Token: 0x04002D31 RID: 11569
	public WeaponAccuracyProjectile.OnEnemyDeath EnemyDeath;

	// Token: 0x04002D32 RID: 11570
	public bool hitEnemy;

	// Token: 0x020011BD RID: 4541
	// (Invoke) Token: 0x06007ED3 RID: 32467
	public delegate void OnEnemyDeath(bool hitEnemy);
}
