using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class WeaponPeashotExProjectile : AbstractProjectile
{
	// Token: 0x060038F0 RID: 14576 RVA: 0x0010A1B4 File Offset: 0x001083B4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.timeUntilUnfreeze > 0f)
		{
			this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
			this.currentSpeed = 0f;
		}
		else
		{
			this.currentSpeed = this.moveSpeed;
		}
		Vector2 vector = MathUtils.AngleToDirection(base.transform.eulerAngles.z) * this.currentSpeed;
		base.transform.AddPosition(vector.x * CupheadTime.FixedDelta, vector.y * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0010A260 File Offset: 0x00108460
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		this.totalDamage += num;
		if (this.totalDamage > this.maxDamage)
		{
			this.Die();
		}
		if (num > 0f)
		{
			this.hitFXPrefab.Create(this.hitFxRoot.position);
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
			this.timeUntilUnfreeze = this.hitFreezeTime;
		}
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x0002E671 File Offset: 0x0002C871
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x04002DCC RID: 11724
	[SerializeField]
	public Effect hitFXPrefab;

	// Token: 0x04002DCD RID: 11725
	[SerializeField]
	public Transform hitFxRoot;

	// Token: 0x04002DCE RID: 11726
	public float timeUntilUnfreeze;

	// Token: 0x04002DCF RID: 11727
	public float moveSpeed;

	// Token: 0x04002DD0 RID: 11728
	public float hitFreezeTime;

	// Token: 0x04002DD1 RID: 11729
	public float totalDamage;

	// Token: 0x04002DD2 RID: 11730
	public float currentSpeed;

	// Token: 0x04002DD3 RID: 11731
	public float maxDamage;
}
