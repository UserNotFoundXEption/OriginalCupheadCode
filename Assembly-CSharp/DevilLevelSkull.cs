using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class DevilLevelSkull : AbstractProjectile
{
	// Token: 0x06001558 RID: 5464 RVA: 0x0009BFBC File Offset: 0x0009A1BC
	public DevilLevelSkull Create(Vector2 pos, LevelProperties.Devil.SkullEye properties)
	{
		DevilLevelSkull devilLevelSkull = this.InstantiatePrefab<DevilLevelSkull>();
		devilLevelSkull.properties = properties;
		devilLevelSkull.transform.position = pos;
		devilLevelSkull.StartCoroutine(devilLevelSkull.main_cr());
		return devilLevelSkull;
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x0001223A File Offset: 0x0001043A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0009BFF8 File Offset: 0x0009A1F8
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0009C044 File Offset: 0x0009A244
	public IEnumerator main_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector2 moveDir = (player.transform.position - base.transform.position).normalized;
		Vector2 velocity = moveDir * this.properties.initialMoveSpeed;
		float rotation = MathUtils.DirectionToAngle(player.transform.position - base.transform.position);
		float t = 0f;
		while (t < this.properties.initialMoveDuration)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		float rotationSpeed = (float)Rand.PosOrNeg() * this.properties.swirlRotationSpeed;
		t = 0f;
		Vector2 spiralOrigin = base.transform.position;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			rotation += rotationSpeed * CupheadTime.FixedDelta;
			base.transform.position = spiralOrigin + MathUtils.AngleToDirection(rotation) * this.properties.swirlMoveOutwardSpeed * t;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04001178 RID: 4472
	public LevelProperties.Devil.SkullEye properties;
}
