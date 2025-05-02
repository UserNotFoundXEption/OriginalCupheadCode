using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class PirateLevelBoatBeam : ParrySwitch
{
	// Token: 0x06002180 RID: 8576 RVA: 0x0001C94F File Offset: 0x0001AB4F
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0.1f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Left, base.transform);
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x0001C982 File Offset: 0x0001AB82
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000BABF8 File Offset: 0x000B8DF8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Exit)
		{
			return;
		}
		LevelPlayerController component = hit.GetComponent<LevelPlayerController>();
		if (component == null || component.Ducking)
		{
			return;
		}
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000BAC3C File Offset: 0x000B8E3C
	public PirateLevelBoatBeam Create(Transform parent)
	{
		PirateLevelBoatBeam pirateLevelBoatBeam = this.InstantiatePrefab<PirateLevelBoatBeam>();
		pirateLevelBoatBeam.Init(parent);
		return pirateLevelBoatBeam;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x0001C98F File Offset: 0x0001AB8F
	public void Init(Transform parent)
	{
		AudioManager.Play("level_pirate_ship_beam_fire");
		base.transform.SetParent(parent);
		base.transform.ResetLocalPosition();
		base.transform.ResetLocalRotation();
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x0001C9BD File Offset: 0x0001ABBD
	public void StartBeam()
	{
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x0001C9BF File Offset: 0x0001ABBF
	public void EndBeam()
	{
		base.animator.SetTrigger("OnEnd");
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x0001C9D1 File Offset: 0x0001ABD1
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x0001C9E5 File Offset: 0x0001ABE5
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.StartParryCooldown();
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x0001C9F4 File Offset: 0x0001ABF4
	public void OnEndAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001BA3 RID: 7075
	public DamageDealer damageDealer;
}
