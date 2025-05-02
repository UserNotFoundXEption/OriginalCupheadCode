using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class FlyingCowboyLevelDebris : BasicProjectile
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06001B80 RID: 7040 RVA: 0x000175AF File Offset: 0x000157AF
	public bool isCurved
	{
		get
		{
			return this.gravity != 0f;
		}
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000ABAC4 File Offset: 0x000A9CC4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, 1000f), new Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, -1000f));
		Vector3 vector = base.transform.position;
		Vector3 vector2 = this.curveSpeed;
		for (int i = 0; i < 50; i++)
		{
			if (this.isCurved)
			{
				vector2 += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
				vector += vector2 * CupheadTime.FixedDelta;
			}
			else
			{
				vector += base.transform.right * this.Speed * CupheadTime.FixedDelta;
			}
			Gizmos.DrawWireSphere(vector, 10f);
		}
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000175C1 File Offset: 0x000157C1
	public void SetupLinearSpeed(MinMax speedRange, float speedUpDistance, Transform aimTransform)
	{
		base.StartCoroutine(this.speedUp_cr(speedRange, speedUpDistance, aimTransform));
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x000ABB94 File Offset: 0x000A9D94
	public IEnumerator speedUp_cr(MinMax speedRange, float distance, Transform aimTransform)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		float sqrDistance = distance * distance;
		while (Vector3.SqrMagnitude(base.transform.position - aimTransform.position) > sqrDistance)
		{
			yield return wait;
		}
		float duration = KinematicUtilities.CalculateTimeToChangeVelocity(speedRange.min, speedRange.max, distance);
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			this.Speed = Mathf.Lerp(speedRange.min, speedRange.max, elapsedTime / duration);
		}
		this.Speed = speedRange.max;
		yield break;
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x000175D3 File Offset: 0x000157D3
	public void SetupVacuum(Transform aimTransform, Transform destroyTransform)
	{
		base.StartCoroutine(this.vacuumAim_cr(aimTransform, destroyTransform));
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000ABBC4 File Offset: 0x000A9DC4
	public IEnumerator vacuumAim_cr(Transform aimTransform, Transform destroyTransform)
	{
		if (this.isCurved)
		{
			while (this.curveSpeed.x < 0f)
			{
				yield return null;
			}
		}
		else if (base.transform.position.x >= FlyingCowboyLevelDebris.OffsetAimXCutoff)
		{
			float distanceThreshold = (base.transform.position.y <= 0f) ? 105f : 80f;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(aimTransform.position + FlyingCowboyLevelDebris.OffsetAimAmount - base.transform.position)));
			if (base.transform.position.x > aimTransform.position.x + FlyingCowboyLevelDebris.OffsetAimAmount.x)
			{
				while (Mathf.Abs(base.transform.position.y - aimTransform.position.y) > distanceThreshold)
				{
					yield return null;
				}
			}
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtilities.DirectionToAngle(aimTransform.position - base.transform.position)));
		}
		while (base.transform.position.x < aimTransform.position.x)
		{
			yield return null;
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.vacuumSuckIn_cr(destroyTransform));
		yield break;
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000ABBF0 File Offset: 0x000A9DF0
	public IEnumerator vacuumSuckIn_cr(Transform destroyTransform)
	{
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtilities.DirectionToAngle(destroyTransform.position - base.transform.position)));
		this.move = true;
		if (this.isCurved)
		{
			this.Speed = this.curveSpeed.magnitude;
		}
		this.Speed *= 1.25f;
		base.StartCoroutine(this.squash_cr());
		while (base.transform.position.x < destroyTransform.position.x)
		{
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.Die();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000ABC14 File Offset: 0x000A9E14
	public IEnumerator squash_cr()
	{
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		float elapsedTime = 0f;
		while (elapsedTime < FlyingCowboyLevelDebris.SquashDuration)
		{
			yield return wait;
			elapsedTime += wait.frameTime + wait.accumulator;
			Vector3 scale = Vector3.Lerp(Vector2.one, FlyingCowboyLevelDebris.SquashAmount, elapsedTime / FlyingCowboyLevelDebris.SquashDuration);
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000175E4 File Offset: 0x000157E4
	public void ToCurve(Vector3 speed, float gravity)
	{
		this.curveSpeed = speed;
		this.gravity = gravity;
		base.StartCoroutine(this.gravity_cr());
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000ABC30 File Offset: 0x000A9E30
	public IEnumerator gravity_cr()
	{
		this.move = false;
		for (;;)
		{
			this.curveSpeed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.curveSpeed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04001645 RID: 5701
	public static readonly float OffsetAimXCutoff = 0f;

	// Token: 0x04001646 RID: 5702
	public static readonly Vector3 OffsetAimAmount = new Vector3(-50f, 0f);

	// Token: 0x04001647 RID: 5703
	public static readonly Vector3 SquashAmount = new Vector3(1.2f, 0.5f, 1f);

	// Token: 0x04001648 RID: 5704
	public static readonly float SquashDuration = 0.4f;

	// Token: 0x04001649 RID: 5705
	public Vector3 curveSpeed;

	// Token: 0x0400164A RID: 5706
	public float gravity;
}
