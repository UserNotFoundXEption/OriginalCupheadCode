using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
public class PlaneWeaponPeashotExProjectile : AbstractProjectile
{
	// Token: 0x06003AAA RID: 15018 RVA: 0x00110CD8 File Offset: 0x0010EED8
	public void Init()
	{
		this.Cuphead.enabled = ((this.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		this.Mugman.enabled = ((this.PlayerId == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman));
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x00110D58 File Offset: 0x0010EF58
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.chompFxPrefab.Create(this.chompFxRoot.position);
		this.state = PlaneWeaponPeashotExProjectile.State.Frozen;
		this.speed = 0f;
		this.timeSinceFrozen = 0f;
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x00110DBC File Offset: 0x0010EFBC
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		PlaneWeaponPeashotExProjectile.State state = this.state;
		if (state != PlaneWeaponPeashotExProjectile.State.Idle)
		{
			if (state == PlaneWeaponPeashotExProjectile.State.Frozen)
			{
				this.timeSinceFrozen += CupheadTime.FixedDelta;
				if (this.timeSinceFrozen > this.FreezeTime)
				{
					this.state = PlaneWeaponPeashotExProjectile.State.Idle;
					this.speed = this.MaxSpeed;
				}
			}
		}
		else
		{
			this.speed = Mathf.Min(this.MaxSpeed, this.speed + this.Acceleration * CupheadTime.FixedDelta);
		}
		base.transform.AddPosition(this.speed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x0002FB68 File Offset: 0x0002DD68
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x0002FB79 File Offset: 0x0002DD79
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x0002FB99 File Offset: 0x0002DD99
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x0002FBA8 File Offset: 0x0002DDA8
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04002F09 RID: 12041
	public float MaxSpeed;

	// Token: 0x04002F0A RID: 12042
	public float Acceleration;

	// Token: 0x04002F0B RID: 12043
	public float FreezeTime;

	// Token: 0x04002F0C RID: 12044
	[SerializeField]
	public Effect chompFxPrefab;

	// Token: 0x04002F0D RID: 12045
	[SerializeField]
	public Transform chompFxRoot;

	// Token: 0x04002F0E RID: 12046
	[SerializeField]
	public SpriteRenderer Cuphead;

	// Token: 0x04002F0F RID: 12047
	[SerializeField]
	public SpriteRenderer Mugman;

	// Token: 0x04002F10 RID: 12048
	public PlaneWeaponPeashotExProjectile.State state;

	// Token: 0x04002F11 RID: 12049
	public float timeSinceFrozen;

	// Token: 0x04002F12 RID: 12050
	public float speed;

	// Token: 0x020011F9 RID: 4601
	public enum State
	{
		// Token: 0x04007D1D RID: 32029
		Idle,
		// Token: 0x04007D1E RID: 32030
		Frozen
	}
}
