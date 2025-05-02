using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class MountainPlatformingLevelPickaxeProjectile : AbstractProjectile
{
	// Token: 0x06002F15 RID: 12053 RVA: 0x00027385 File Offset: 0x00025585
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.throw_pickaxe_cr());
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x0002739A File Offset: 0x0002559A
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000E0C5C File Offset: 0x000DEE5C
	public MountainPlatformingLevelPickaxeProjectile Create(Vector2 pos, float rotation, float speed, MountainPlatformingLevelMiner miner, Vector3 targetPos, Vector3 minerPos)
	{
		MountainPlatformingLevelPickaxeProjectile mountainPlatformingLevelPickaxeProjectile = base.Create(pos, rotation) as MountainPlatformingLevelPickaxeProjectile;
		mountainPlatformingLevelPickaxeProjectile.miner = miner;
		mountainPlatformingLevelPickaxeProjectile.speed = speed;
		mountainPlatformingLevelPickaxeProjectile.minerPosition = minerPos;
		mountainPlatformingLevelPickaxeProjectile.targetPos = targetPos;
		return mountainPlatformingLevelPickaxeProjectile;
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x000273B8 File Offset: 0x000255B8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000E0C98 File Offset: 0x000DEE98
	public IEnumerator throw_pickaxe_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = base.transform.position;
		float time = Vector3.Distance(base.transform.position, this.targetPos) / this.speed;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			base.transform.position = Vector3.Lerp(startPos, this.targetPos, val);
			yield return wait;
		}
		yield return wait;
		base.transform.position = this.targetPos;
		t = 0f;
		Vector3 dir = startPos - this.targetPos;
		for (;;)
		{
			if (this.miner != null)
			{
				if (t >= time)
				{
					break;
				}
				t += CupheadTime.FixedDelta;
				float num = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
				base.transform.position = Vector3.Lerp(this.targetPos, this.minerPosition, num);
			}
			else
			{
				base.transform.position += dir.normalized * this.speed * CupheadTime.FixedDelta;
			}
			yield return wait;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400270C RID: 9996
	public Vector3 minerPosition;

	// Token: 0x0400270D RID: 9997
	public Vector3 targetPos;

	// Token: 0x0400270E RID: 9998
	public MountainPlatformingLevelMiner miner;

	// Token: 0x0400270F RID: 9999
	public float speed;

	// Token: 0x04002710 RID: 10000
	public const float MAX_DIST = 5f;
}
