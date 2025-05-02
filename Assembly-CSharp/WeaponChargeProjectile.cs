using System;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class WeaponChargeProjectile : BasicProjectile
{
	// Token: 0x06003882 RID: 14466 RVA: 0x00107A5C File Offset: 0x00105C5C
	public override void Die()
	{
		if (this.fullyCharged)
		{
			Vector2 vector = MathUtils.AngleToDirection(base.transform.eulerAngles.z) * 75f;
			base.transform.AddPosition(vector.x, vector.y, 0f);
		}
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.Die();
		if (this.fullyCharged)
		{
			AudioManager.Play("player_weapon_charge_full_impact");
			this.emitAudioFromObject.Add("player_weapon_charge_full_impact");
		}
	}

	// Token: 0x04002D6C RID: 11628
	public bool fullyCharged;
}
