using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class PlaneWeaponChaliceBombExProjectile : AbstractProjectile
{
	// Token: 0x06003A7E RID: 14974 RVA: 0x0010FA68 File Offset: 0x0010DC68
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.DamageRate += this.DamageRateIncrease;
		damageDealer.SetRate(this.DamageRate);
		this.chompFxPrefab.Create(this.chompFxRoot.position);
		this.state = PlaneWeaponChaliceBombExProjectile.State.Frozen;
		this.speed = 0f;
		this.timeSinceFrozen = 0f;
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x0010FAEC File Offset: 0x0010DCEC
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		PlaneWeaponChaliceBombExProjectile.State state = this.state;
		if (state != PlaneWeaponChaliceBombExProjectile.State.Idle)
		{
			if (state == PlaneWeaponChaliceBombExProjectile.State.Frozen)
			{
				this.timeSinceFrozen += CupheadTime.FixedDelta;
				if (this.timeSinceFrozen > this.FreezeTime)
				{
					this.state = PlaneWeaponChaliceBombExProjectile.State.Idle;
				}
			}
		}
		else
		{
			this.Velocity.y = this.Velocity.y - this.Gravity * CupheadTime.FixedDelta;
			base.transform.position += this.Velocity * CupheadTime.FixedDelta;
			base.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.Velocity));
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0002F9AC File Offset: 0x0002DBAC
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0002F9BD File Offset: 0x0002DBBD
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0002F9DD File Offset: 0x0002DBDD
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0002F9EC File Offset: 0x0002DBEC
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04002ED0 RID: 11984
	public float MaxSpeed;

	// Token: 0x04002ED1 RID: 11985
	public float Acceleration;

	// Token: 0x04002ED2 RID: 11986
	public float FreezeTime;

	// Token: 0x04002ED3 RID: 11987
	[SerializeField]
	public Effect chompFxPrefab;

	// Token: 0x04002ED4 RID: 11988
	[SerializeField]
	public Transform chompFxRoot;

	// Token: 0x04002ED5 RID: 11989
	public Vector3 Velocity;

	// Token: 0x04002ED6 RID: 11990
	public float Gravity;

	// Token: 0x04002ED7 RID: 11991
	public float DamageRateIncrease;

	// Token: 0x04002ED8 RID: 11992
	public PlaneWeaponChaliceBombExProjectile.State state;

	// Token: 0x04002ED9 RID: 11993
	public float timeSinceFrozen;

	// Token: 0x04002EDA RID: 11994
	public float speed;

	// Token: 0x020011F6 RID: 4598
	public enum State
	{
		// Token: 0x04007D0E RID: 32014
		Idle,
		// Token: 0x04007D0F RID: 32015
		Frozen
	}
}
