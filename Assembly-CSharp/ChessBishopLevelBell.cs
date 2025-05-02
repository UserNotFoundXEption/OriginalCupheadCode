using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class ChessBishopLevelBell : AbstractProjectile
{
	// Token: 0x17000247 RID: 583
	// (get) Token: 0x0600120E RID: 4622 RVA: 0x0000F45E File Offset: 0x0000D65E
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0000F461 File Offset: 0x0000D661
	public virtual ChessBishopLevelBell Init(Vector3 pos, AbstractPlayerController player, LevelProperties.ChessBishop.Bishop properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0000F497 File Offset: 0x0000D697
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x00093E6C File Offset: 0x0009206C
	public IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.projectileDelayRange.RandomFloat());
		base.animator.SetTrigger("Attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		Vector3 direction = (this.player.transform.position - base.transform.position).normalized;
		if (base.animator.GetInteger(AbstractProjectile.Variant) == 0)
		{
			base.animator.Play("A", 1);
			base.animator.Play("A", 2);
			base.animator.Play("IntroA", 3);
			foreach (Transform transform in this.smokeTransforms)
			{
				transform.rotation = Quaternion.Euler(0f, 0f, 45f);
			}
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(90f + MathUtils.DirectionToAngle(direction)));
		}
		else
		{
			base.animator.Play("B", 1);
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += direction * this.properties.projectileSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x04000E8F RID: 3727
	public const int AnimatorBaseLayer = 0;

	// Token: 0x04000E90 RID: 3728
	public const int AnimatorSmokeTopLayer = 1;

	// Token: 0x04000E91 RID: 3729
	public const int AnimatorSmokeMiddleLayer = 2;

	// Token: 0x04000E92 RID: 3730
	public const int AnimatorSmokeBottomLayer = 3;

	// Token: 0x04000E93 RID: 3731
	[SerializeField]
	public Transform[] smokeTransforms;

	// Token: 0x04000E94 RID: 3732
	public LevelProperties.ChessBishop.Bishop properties;

	// Token: 0x04000E95 RID: 3733
	public AbstractPlayerController player;
}
