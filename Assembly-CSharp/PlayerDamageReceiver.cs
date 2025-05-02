using System;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public class PlayerDamageReceiver : DamageReceiver
{
	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x0002FBBF File Offset: 0x0002DDBF
	// (set) Token: 0x06003AB8 RID: 15032 RVA: 0x0002FBC7 File Offset: 0x0002DDC7
	public PlayerDamageReceiver.State state { get; set; }

	// Token: 0x06003AB9 RID: 15033 RVA: 0x0002FBD0 File Offset: 0x0002DDD0
	public override void Awake()
	{
		base.Awake();
		if (this.type != DamageReceiver.Type.Player)
		{
		}
		this.type = DamageReceiver.Type.Player;
		this.player = base.GetComponent<AbstractPlayerController>();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00111090 File Offset: 0x0010F290
	public void Update()
	{
		if (this.state != PlayerDamageReceiver.State.Invulnerable)
		{
			return;
		}
		if (this.timer > 0f)
		{
			this.timer -= CupheadTime.Delta;
			if (this.timer <= 0f)
			{
				this.Vulnerable();
			}
		}
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x001110E8 File Offset: 0x0010F2E8
	public void HandleChaliceShmupSuper(DamageDealer.DamageInfo info)
	{
		if (this.player.stats.State == PlayerStatsManager.PlayerState.Super && this.player.stats.isChalice && this.player.stats.Loadout.super == Super.level_super_ghost)
		{
			base.TakeDamageBruteForce(info);
			return;
		}
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x00111148 File Offset: 0x0010F348
	public override void TakeDamage(DamageDealer.DamageInfo info)
	{
		if (this.player.stats.SuperInvincible)
		{
			return;
		}
		if (info.damage > 0f)
		{
			this.HandleChaliceShmupSuper(info);
			if (!base.enabled)
			{
				return;
			}
			if (info.damageSource == DamageDealer.DamageSource.Pit)
			{
				if (this.player.damageReceiver.state != PlayerDamageReceiver.State.Vulnerable)
				{
					return;
				}
			}
			else if (!this.player.CanTakeDamage)
			{
				return;
			}
			if (this.timer > 0f)
			{
				return;
			}
			float num = 1f;
			this.Invulnerable(2f * num);
			base.TakeDamage(info);
			if (this.player.stats.ChaliceShieldOn)
			{
				this.player.stats.SetChaliceShield(false);
			}
		}
		else if (info.stoneTime > 0f)
		{
			base.TakeDamage(info);
		}
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0002FC0E File Offset: 0x0002DE0E
	public void OnRevive(Vector3 pos)
	{
		this.Invulnerable(3f);
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x0002FC1B File Offset: 0x0002DE1B
	public void Invulnerable(float time)
	{
		this.state = PlayerDamageReceiver.State.Invulnerable;
		this.timer = time;
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x0002FC2B File Offset: 0x0002DE2B
	public void Vulnerable()
	{
		this.state = PlayerDamageReceiver.State.Vulnerable;
		this.timer = 0f;
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x0002FC3F File Offset: 0x0002DE3F
	public void OnDeath()
	{
		this.state = PlayerDamageReceiver.State.Other;
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x0002FC48 File Offset: 0x0002DE48
	public void OnWin()
	{
		this.state = PlayerDamageReceiver.State.Other;
	}

	// Token: 0x04002F17 RID: 12055
	public const float TIME_HIT = 2f;

	// Token: 0x04002F18 RID: 12056
	public const float TIME_REVIVED = 3f;

	// Token: 0x04002F19 RID: 12057
	public AbstractPlayerController player;

	// Token: 0x04002F1A RID: 12058
	public float timer;

	// Token: 0x020011FA RID: 4602
	public enum State
	{
		// Token: 0x04007D20 RID: 32032
		Vulnerable,
		// Token: 0x04007D21 RID: 32033
		Invulnerable,
		// Token: 0x04007D22 RID: 32034
		Other
	}
}
