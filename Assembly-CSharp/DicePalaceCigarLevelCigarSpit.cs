using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class DicePalaceCigarLevelCigarSpit : AbstractProjectile
{
	// Token: 0x0600161F RID: 5663 RVA: 0x0009EB18 File Offset: 0x0009CD18
	public void InitProjectile(LevelProperties.DicePalaceCigar properties, bool clockwise, bool onRight)
	{
		this.time = 0f;
		this.centerPoint = base.transform.position;
		this.onRight = onRight;
		if (!clockwise)
		{
			this.circleSpeed = -properties.CurrentState.spiralSmoke.circleSpeed;
		}
		else
		{
			this.circleSpeed = properties.CurrentState.spiralSmoke.circleSpeed;
		}
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.bullet_trail_cr());
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x0009EBA4 File Offset: 0x0009CDA4
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			this.centerPoint += -base.transform.right * this.properties.CurrentState.spiralSmoke.horizontalSpeed * CupheadTime.FixedDelta;
			Vector3 newPos = this.centerPoint;
			newPos.y = this.centerPoint.y + Mathf.Sin(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			if (this.onRight)
			{
				newPos.x = this.centerPoint.x + Mathf.Cos(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			}
			else
			{
				newPos.x = this.centerPoint.x + -Mathf.Cos(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			}
			base.transform.position = newPos;
			this.time += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x0009EBC0 File Offset: 0x0009CDC0
	public IEnumerator bullet_trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.16f, 0.2f));
			this.bulletFX.Create(base.transform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x00012C50 File Offset: 0x00010E50
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x04001209 RID: 4617
	[SerializeField]
	public Effect bulletFX;

	// Token: 0x0400120A RID: 4618
	public bool onRight;

	// Token: 0x0400120B RID: 4619
	public float time;

	// Token: 0x0400120C RID: 4620
	public float circleSpeed;

	// Token: 0x0400120D RID: 4621
	public Vector3 centerPoint;

	// Token: 0x0400120E RID: 4622
	public LevelProperties.DicePalaceCigar properties;
}
