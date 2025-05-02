using System;
using UnityEngine;

// Token: 0x02000529 RID: 1321
public class PlayerSuperChaliceIIIMinion : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x060037B7 RID: 14263 RVA: 0x0002D792 File Offset: 0x0002B992
	public override void OnDieLifetime()
	{
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x0002D794 File Offset: 0x0002B994
	public override void OnDieDistance()
	{
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x00104C34 File Offset: 0x00102E34
	public override void Start()
	{
		base.Start();
		this.startY = base.transform.position.y;
		this.t = Random.Range(0f, 6.28318548f);
		this.wavelength = Random.Range(150f, 300f);
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x00104CAC File Offset: 0x00102EAC
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.impactFX.Create(Vector3.Lerp(base.transform.position, receiver.transform.position, Random.Range(0f, 1f)) + Random.insideUnitSphere * 25f);
		AudioManager.Play("player_super_chalice_barrage_impact");
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x00104D18 File Offset: 0x00102F18
	public override void Move()
	{
		base.transform.position += this.Direction * this.Speed * CupheadTime.FixedDelta;
		if (this.wave)
		{
			this.t += this.Speed * CupheadTime.FixedDelta;
			base.transform.position = new Vector3(base.transform.position.x, this.startY + Mathf.Sin(this.t / this.wavelength) * this.amplitude);
		}
	}

	// Token: 0x04002CE5 RID: 11493
	public int elementIndex;

	// Token: 0x04002CE6 RID: 11494
	public float wavelength = 180f;

	// Token: 0x04002CE7 RID: 11495
	public float amplitude = 20f;

	// Token: 0x04002CE8 RID: 11496
	public float t;

	// Token: 0x04002CE9 RID: 11497
	public float startY;

	// Token: 0x04002CEA RID: 11498
	public bool wave;

	// Token: 0x04002CEB RID: 11499
	[SerializeField]
	public Effect impactFX;
}
