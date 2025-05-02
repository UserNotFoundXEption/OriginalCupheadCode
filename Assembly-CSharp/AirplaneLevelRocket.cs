using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class AirplaneLevelRocket : HomingProjectile
{
	// Token: 0x06000E56 RID: 3670 RVA: 0x0008A7BC File Offset: 0x000889BC
	public AirplaneLevelRocket Create(AbstractPlayerController player, Vector2 pos, float speed, float rotationSpeed, float health, float homingTime)
	{
		AirplaneLevelRocket airplaneLevelRocket = base.Create(pos, -90f, speed, speed, rotationSpeed, this.DestroyLifetime, 0f, player) as AirplaneLevelRocket;
		airplaneLevelRocket.DamagesType.OnlyPlayer();
		airplaneLevelRocket.Init(health);
		airplaneLevelRocket.homingTimer = homingTime;
		return airplaneLevelRocket;
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0000C2CB File Offset: 0x0000A4CB
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.sfx_rocket_spawn_cr());
		base.StartCoroutine(this.spawn_effect_cr());
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0000C2ED File Offset: 0x0000A4ED
	public void Init(float health)
	{
		this.health = health;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0000C30D File Offset: 0x0000A50D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Impact");
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x0008A808 File Offset: 0x00088A08
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0008A85C File Offset: 0x00088A5C
	public IEnumerator continue_without_homing_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x0008A878 File Offset: 0x00088A78
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		this.sprite.GetComponent<SpriteRenderer>().enabled = false;
		GameObject gameObject = GameObject.Find("BullDogPlane");
		if (gameObject && Mathf.Abs(gameObject.transform.position.x - base.transform.position.x) < 800f && Mathf.Abs(gameObject.transform.position.y - base.transform.position.y) < 175f)
		{
			this.deathOnPlaneFX.Create(base.transform.position);
		}
		else
		{
			this.deathFX.Create(base.transform.position);
		}
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_DeathExplode");
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x0008A970 File Offset: 0x00088B70
	public IEnumerator spawn_effect_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.fxSpawnRate.RandomFloat());
			this.effectFX.Create(this.effectRoot.position);
			AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Chuff");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x0008A98C File Offset: 0x00088B8C
	public override void Update()
	{
		base.Update();
		if (this.homingTimer > 0f)
		{
			this.homingTimer -= CupheadTime.Delta;
			if (this.homingTimer <= 0f)
			{
				this.StopAllCoroutines();
				base.StartCoroutine(this.continue_without_homing_cr());
			}
		}
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x0008A9EC File Offset: 0x00088BEC
	public IEnumerator sfx_rocket_spawn_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Entrance");
		yield break;
	}

	// Token: 0x04000B90 RID: 2960
	public float homingTimer;

	// Token: 0x04000B91 RID: 2961
	public float health;

	// Token: 0x04000B92 RID: 2962
	[SerializeField]
	public Transform effectRoot;

	// Token: 0x04000B93 RID: 2963
	[SerializeField]
	public Effect effectFX;

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	public Effect deathFX;

	// Token: 0x04000B95 RID: 2965
	[SerializeField]
	public Effect deathOnPlaneFX;

	// Token: 0x04000B96 RID: 2966
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04000B97 RID: 2967
	[SerializeField]
	public MinMax fxSpawnRate;
}
