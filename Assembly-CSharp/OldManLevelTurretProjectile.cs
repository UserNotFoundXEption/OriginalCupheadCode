using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class OldManLevelTurretProjectile : BasicProjectile
{
	// Token: 0x0600215D RID: 8541 RVA: 0x000BA5B4 File Offset: 0x000B87B4
	public override void Start()
	{
		base.Start();
		this.rend.flipX = Rand.Bool();
		base.animator.Play("Projectile", 0, Random.Range(0f, 1f));
		base.StartCoroutine(this.spawn_sparkles_cr());
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000BA604 File Offset: 0x000B8804
	public IEnumerator spawn_sparkles_cr()
	{
		this.sparkleAngle = (float)Random.Range(0, 360);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.sparkleSpawnDelay);
			((OldManLevel)Level.Current).CreateFX(base.transform.position + MathUtils.AngleToDirection(this.sparkleAngle) * this.sparkleDistanceRange.RandomFloat(), true, base.CanParry);
			this.sparkleAngle = (this.sparkleAngle + this.sparkleAngleShiftRange.RandomFloat()) % 360f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x000BA620 File Offset: 0x000B8820
	public override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.position += base.transform.up * this.Speed * CupheadTime.FixedDelta;
	}

	// Token: 0x04001B84 RID: 7044
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001B85 RID: 7045
	[SerializeField]
	public float sparkleSpawnDelay;

	// Token: 0x04001B86 RID: 7046
	[SerializeField]
	public MinMax sparkleAngleShiftRange;

	// Token: 0x04001B87 RID: 7047
	[SerializeField]
	public MinMax sparkleDistanceRange;

	// Token: 0x04001B88 RID: 7048
	public float sparkleAngle;
}
