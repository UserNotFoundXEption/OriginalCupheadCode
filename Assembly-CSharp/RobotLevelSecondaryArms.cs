using System;
using UnityEngine;

// Token: 0x02000339 RID: 825
public class RobotLevelSecondaryArms : AbstractCollidableObject
{
	// Token: 0x170002FE RID: 766
	// (get) Token: 0x0600240C RID: 9228 RVA: 0x0001E6A7 File Offset: 0x0001C8A7
	// (set) Token: 0x0600240D RID: 9229 RVA: 0x0001E6AF File Offset: 0x0001C8AF
	public bool BossAlive { get; set; }

	// Token: 0x0600240E RID: 9230 RVA: 0x0001E6B8 File Offset: 0x0001C8B8
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.BossAlive = true;
		base.Awake();
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x0001E6D2 File Offset: 0x0001C8D2
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x0001E6EA File Offset: 0x0001C8EA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000C26F8 File Offset: 0x000C08F8
	public void InitHelper(LevelProperties.Robot properties)
	{
		this.spawnPoint = base.transform.GetChild(2).transform;
		this.ShootTwicePerCycle = properties.CurrentState.twistyArms.shootTwicePerCycle;
		this.bulletSpeed = properties.CurrentState.twistyArms.bulletSpeed;
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000C2748 File Offset: 0x000C0948
	public void OnTwistyArmsShoot()
	{
		if (this.twistyArmsProjectile != null)
		{
			AudioManager.Play("robot_arms_hand_shoot");
			this.emitAudioFromObject.Add("robot_arms_hand_shoot");
			this.twistyArmsProjectile.Create(this.spawnPoint.position + Vector3.up * 100f, 90f, this.bulletSpeed);
			this.twistyArmsProjectile.Create(this.spawnPoint.position + Vector3.down * 100f, -90f, this.bulletSpeed);
		}
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000C27F8 File Offset: 0x000C09F8
	public void SwapAnimations()
	{
		if (this.BossAlive)
		{
			float num = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
			if (this.ShootTwicePerCycle && num < 0.5f)
			{
				base.animator.Play("Shoot B");
			}
			else if (num > 0.5f)
			{
				base.animator.Play("Shoot A");
			}
		}
	}

	// Token: 0x04001DE0 RID: 7648
	public bool ShootTwicePerCycle;

	// Token: 0x04001DE1 RID: 7649
	public float bulletSpeed;

	// Token: 0x04001DE2 RID: 7650
	public Transform spawnPoint;

	// Token: 0x04001DE3 RID: 7651
	public DamageDealer damageDealer;

	// Token: 0x04001DE5 RID: 7653
	[SerializeField]
	public BasicProjectile twistyArmsProjectile;
}
