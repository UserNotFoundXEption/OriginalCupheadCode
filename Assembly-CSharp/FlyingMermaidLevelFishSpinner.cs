using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class FlyingMermaidLevelFishSpinner : AbstractProjectile
{
	// Token: 0x06001D27 RID: 7463 RVA: 0x000B0270 File Offset: 0x000AE470
	public FlyingMermaidLevelFishSpinner Create(Vector2 pos, Vector2 direction, LevelProperties.FlyingMermaid.SpinnerFish properties)
	{
		FlyingMermaidLevelFishSpinner flyingMermaidLevelFishSpinner = base.Create() as FlyingMermaidLevelFishSpinner;
		flyingMermaidLevelFishSpinner.properties = properties;
		flyingMermaidLevelFishSpinner.direction = direction;
		flyingMermaidLevelFishSpinner.transform.position = pos;
		return flyingMermaidLevelFishSpinner;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x00018B50 File Offset: 0x00016D50
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tails_cr());
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x00018B72 File Offset: 0x00016D72
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000B02AC File Offset: 0x000AE4AC
	public IEnumerator tails_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeBeforeTails);
		base.animator.SetTrigger("StartTails");
		yield break;
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000B02C8 File Offset: 0x000AE4C8
	public IEnumerator move_cr()
	{
		base.transform.SetEulerAngles(null, null, new float?((float)Random.Range(0, 360)));
		Vector2 velocity = this.direction * this.properties.bulletSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			base.transform.Rotate(0f, 0f, this.properties.rotationSpeed * CupheadTime.FixedDelta);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x00018B90 File Offset: 0x00016D90
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040017C9 RID: 6089
	public LevelProperties.FlyingMermaid.SpinnerFish properties;

	// Token: 0x040017CA RID: 6090
	public Vector2 direction;
}
