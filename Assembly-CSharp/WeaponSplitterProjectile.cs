using System;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class WeaponSplitterProjectile : BasicProjectile
{
	// Token: 0x06003904 RID: 14596 RVA: 0x0002E729 File Offset: 0x0002C929
	public override void Start()
	{
		base.Start();
		if (this.splitDamage > -1f)
		{
			this.damageDealer.SetDamage(this.splitDamage);
		}
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0002E752 File Offset: 0x0002C952
	public override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x0002E776 File Offset: 0x0002C976
	public override void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x0002E783 File Offset: 0x0002C983
	public void _OnDieAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x0010A5F0 File Offset: 0x001087F0
	public void Split()
	{
		this.baseAngle = base.transform.eulerAngles.z;
		if (this.isMain)
		{
			this.damageDealer.SetDamage((this.nextDistance != WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB) ? WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageA : WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageB);
			WeaponSplitterProjectile weaponSplitterProjectile = Object.Instantiate<WeaponSplitterProjectile>(this, base.transform.position, Quaternion.identity);
			weaponSplitterProjectile.isMain = false;
			weaponSplitterProjectile.splitAngle = -WeaponProperties.LevelWeaponSplitter.Basic.splitAngle;
			weaponSplitterProjectile.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
			weaponSplitterProjectile.dist = this.dist;
			weaponSplitterProjectile.splitDamage = this.Damage;
			weaponSplitterProjectile = Object.Instantiate<WeaponSplitterProjectile>(this, base.transform.position, Quaternion.identity);
			weaponSplitterProjectile.isMain = false;
			weaponSplitterProjectile.splitAngle = WeaponProperties.LevelWeaponSplitter.Basic.splitAngle;
			weaponSplitterProjectile.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
			weaponSplitterProjectile.dist = this.dist;
			weaponSplitterProjectile.splitDamage = this.Damage;
		}
		else
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			this.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
		}
		if (this.nextDistance == WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB)
		{
			this.nextDistance = float.MaxValue;
		}
		else
		{
			this.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB;
		}
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0010A794 File Offset: 0x00108994
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.dist += this.Speed * CupheadTime.FixedDelta;
		if (this.dist > this.nextDistance)
		{
			this.Split();
		}
		if (this.distancePastSplit > 0f)
		{
			this.distancePastSplit -= this.Speed * CupheadTime.FixedDelta;
			if (this.distancePastSplit <= 0f)
			{
				base.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle);
			}
		}
	}

	// Token: 0x04002DE4 RID: 11748
	public bool isMain;

	// Token: 0x04002DE5 RID: 11749
	public float nextDistance;

	// Token: 0x04002DE6 RID: 11750
	public float baseAngle;

	// Token: 0x04002DE7 RID: 11751
	public float distancePastSplit;

	// Token: 0x04002DE8 RID: 11752
	public float splitAngle;

	// Token: 0x04002DE9 RID: 11753
	public float dist;

	// Token: 0x04002DEA RID: 11754
	public float splitDamage = -1f;
}
