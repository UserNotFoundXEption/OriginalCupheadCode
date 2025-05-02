using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class PlaneWeaponLaser : AbstractPlaneWeapon
{
	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06003A9E RID: 15006 RVA: 0x0002FB08 File Offset: 0x0002DD08
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponLaser.Basic.rapidFire;
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06003A9F RID: 15007 RVA: 0x0002FB0F File Offset: 0x0002DD0F
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponLaser.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003AA0 RID: 15008 RVA: 0x00110940 File Offset: 0x0010EB40
	public override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Basic.speed;
		basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float num = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, num, 0f);
		if (this.player.Shrunk)
		{
			basicProjectile.Damage *= this.shrunkDamageMultiplier;
			basicProjectile.transform.AddPosition(0f, num * -0.5f, 0f);
			basicProjectile.DestroyDistance = (float)Random.Range(200, 350);
			basicProjectile.DestroyDistanceAnimated = true;
		}
		return basicProjectile;
	}

	// Token: 0x06003AA1 RID: 15009 RVA: 0x0002FB16 File Offset: 0x0002DD16
	public override AbstractProjectile fireEx()
	{
		base.StartCoroutine(this.ex_cr());
		return null;
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x00110A2C File Offset: 0x0010EC2C
	public IEnumerator ex_cr()
	{
		for (int wave = 0; wave < WeaponProperties.PlaneWeaponLaser.Ex.counts.Length; wave++)
		{
			int count = WeaponProperties.PlaneWeaponLaser.Ex.counts[wave];
			float angle = WeaponProperties.PlaneWeaponLaser.Ex.angles[wave];
			for (int i = 0; i < count; i++)
			{
				float value = Mathf.Lerp(0f, angle, (float)i / (float)count) - 90f;
				BasicProjectile basicProjectile = this.<fireEx>__BaseCallProxy0() as BasicProjectile;
				basicProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
				basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Ex.speed;
				basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Ex.damage;
				basicProjectile.PlayerId = this.player.id;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x04002F03 RID: 12035
	public const float Y_POS = 20f;

	// Token: 0x04002F04 RID: 12036
	public float[] yPositions = new float[]
	{
		20f,
		-20f
	};

	// Token: 0x04002F05 RID: 12037
	public int currentY;
}
