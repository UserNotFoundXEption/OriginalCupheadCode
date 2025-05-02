using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class MountainPlatformingLevelWallProjectile : AbstractProjectile
{
	// Token: 0x06002F4F RID: 12111 RVA: 0x000E15C4 File Offset: 0x000DF7C4
	public MountainPlatformingLevelWallProjectile Create(Vector2 pos, float rotation, Vector2 velocity, float gravity, float yGround)
	{
		MountainPlatformingLevelWallProjectile mountainPlatformingLevelWallProjectile = base.Create() as MountainPlatformingLevelWallProjectile;
		mountainPlatformingLevelWallProjectile.transform.position = pos;
		mountainPlatformingLevelWallProjectile.velocity = velocity;
		mountainPlatformingLevelWallProjectile.startVelocity = velocity;
		mountainPlatformingLevelWallProjectile.gravity = gravity;
		mountainPlatformingLevelWallProjectile.yGround = yGround;
		mountainPlatformingLevelWallProjectile.transform.SetEulerAngles(null, null, new float?(rotation));
		return mountainPlatformingLevelWallProjectile;
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x000E1630 File Offset: 0x000DF830
	public override void Start()
	{
		base.Start();
		this.timeToApex = Mathf.Sqrt(2f * this.velocity.y / this.gravity);
		this.startVelocity.y = this.timeToApex * this.gravity;
		base.StartCoroutine(this.check_to_kill_cr());
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x000E168C File Offset: 0x000DF88C
	public IEnumerator handle_hit_ground_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Hit", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.onGround = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x00027673 File Offset: 0x00025873
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x000E16A8 File Offset: 0x000DF8A8
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.onGround)
		{
			return;
		}
		if (base.transform.position.y <= this.yGround)
		{
			this.onGround = true;
			this.HandleHitGround();
		}
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x00027691 File Offset: 0x00025891
	public void HandleHitGround()
	{
		this.velocity.y = this.startVelocity.y;
		base.animator.SetTrigger("OnHitGround");
		base.StartCoroutine(this.handle_hit_ground_cr());
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x000E1708 File Offset: 0x000DF908
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.SetEulerAngles(null, null, new float?(Mathf.Atan2(-this.velocity.y, -this.velocity.x) * 57.29578f));
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x000E17D0 File Offset: 0x000DF9D0
	public void ChangeRootEnd()
	{
		base.transform.position = this.root.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(Mathf.Atan2(-this.velocity.y, -this.velocity.x) * 57.29578f));
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x000E1840 File Offset: 0x000DFA40
	public void ChangeRootBeginning()
	{
		AudioManager.Play("castle_mountain_wall_oil_bounce");
		this.emitAudioFromObject.Add("castle_mountain_wall_oil_bounce");
		base.transform.position = this.root1.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x000E18AC File Offset: 0x000DFAAC
	public IEnumerator check_to_kill_cr()
	{
		while (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400273E RID: 10046
	[SerializeField]
	public Transform root;

	// Token: 0x0400273F RID: 10047
	[SerializeField]
	public Transform root1;

	// Token: 0x04002740 RID: 10048
	public Vector2 velocity;

	// Token: 0x04002741 RID: 10049
	public Vector2 startVelocity;

	// Token: 0x04002742 RID: 10050
	public float gravity;

	// Token: 0x04002743 RID: 10051
	public float timeToApex;

	// Token: 0x04002744 RID: 10052
	public float yGround;

	// Token: 0x04002745 RID: 10053
	public bool onGround;
}
