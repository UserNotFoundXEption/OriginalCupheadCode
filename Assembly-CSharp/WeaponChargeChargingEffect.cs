using System;
using UnityEngine;

// Token: 0x0200051F RID: 1311
public class WeaponChargeChargingEffect : AbstractMonoBehaviour
{
	// Token: 0x06003774 RID: 14196 RVA: 0x00103658 File Offset: 0x00101858
	public WeaponChargeChargingEffect Create(Vector2 pos)
	{
		WeaponChargeChargingEffect weaponChargeChargingEffect = this.InstantiatePrefab<WeaponChargeChargingEffect>();
		weaponChargeChargingEffect.transform.position = pos;
		return weaponChargeChargingEffect;
	}
}
