using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class WeaponExploderProjectile : BasicProjectile
{
	// Token: 0x060038AD RID: 14509 RVA: 0x0002E301 File Offset: 0x0002C501
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x0002E309 File Offset: 0x0002C509
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.isEx)
		{
			this.UpdateDamageState();
		}
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x00108D14 File Offset: 0x00106F14
	public void UpdateDamageState()
	{
		if (base.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateTwo)
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.baseDamage;
			base.transform.SetScale(new float?(1f), new float?(1f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.baseExplosionRadius;
		}
		else if (base.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateThree)
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateTwo;
			base.transform.SetScale(new float?(1.5f), new float?(1.5f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateTwo;
		}
		else
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateThree;
			base.transform.SetScale(new float?(2.5f), new float?(2.5f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateThree;
		}
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x00108E08 File Offset: 0x00107008
	public override void Die()
	{
		base.Die();
		this.explosionPrefab.Create(base.transform.position, this.explodeRadius, this.Damage, base.DamageMultiplier, this.weapon, this.tracker);
		if (this.shrapnelPrefab != null)
		{
			BasicProjectile basicProjectile = this.shrapnelPrefab.Create(base.transform.position, base.transform.eulerAngles.z + 180f, WeaponProperties.LevelWeaponExploder.Ex.shrapnelSpeed);
			if (!WeaponProperties.LevelWeaponExploder.Ex.damageOn)
			{
				basicProjectile.DamagesType.Player = false;
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x0002E322 File Offset: 0x0002C522
	public override void AddToMeterScoreTracker(MeterScoreTracker tracker)
	{
		base.AddToMeterScoreTracker(tracker);
		this.tracker = tracker;
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x0002E332 File Offset: 0x0002C532
	public void EaseSpeed()
	{
		base.StartCoroutine(this.ease_speed_cr());
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x00108EC0 File Offset: 0x001070C0
	public IEnumerator ease_speed_cr()
	{
		float t = 0f;
		float time = this.easeTime;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.Speed = this.minMaxSpeed.GetFloatAt(t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002D88 RID: 11656
	[SerializeField]
	public WeaponExploderProjectileExplosion explosionPrefab;

	// Token: 0x04002D89 RID: 11657
	[SerializeField]
	public BasicProjectile shrapnelPrefab;

	// Token: 0x04002D8A RID: 11658
	[SerializeField]
	public bool isEx;

	// Token: 0x04002D8B RID: 11659
	public float explodeRadius;

	// Token: 0x04002D8C RID: 11660
	public float easeTime;

	// Token: 0x04002D8D RID: 11661
	public MinMax minMaxSpeed;

	// Token: 0x04002D8E RID: 11662
	public WeaponExploder weapon;

	// Token: 0x04002D8F RID: 11663
	public new MeterScoreTracker tracker;
}
