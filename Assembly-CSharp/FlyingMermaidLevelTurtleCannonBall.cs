using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class FlyingMermaidLevelTurtleCannonBall : AbstractProjectile
{
	// Token: 0x06001DE4 RID: 7652 RVA: 0x000B2074 File Offset: 0x000B0274
	public FlyingMermaidLevelTurtleCannonBall Create(Vector2 pos, string explodePattern, LevelProperties.FlyingMermaid.Turtle properties)
	{
		FlyingMermaidLevelTurtleCannonBall flyingMermaidLevelTurtleCannonBall = base.Create() as FlyingMermaidLevelTurtleCannonBall;
		flyingMermaidLevelTurtleCannonBall.properties = properties;
		flyingMermaidLevelTurtleCannonBall.explodePattern = explodePattern;
		flyingMermaidLevelTurtleCannonBall.transform.position = pos;
		return flyingMermaidLevelTurtleCannonBall;
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x0001942D File Offset: 0x0001762D
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x00019442 File Offset: 0x00017642
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000B20B0 File Offset: 0x000B02B0
	public IEnumerator loop_cr()
	{
		AudioManager.Play("level_mermaid_turtle_cannon");
		float t = 0f;
		float bulletTime = this.properties.bulletTimeToExplode.RandomFloat();
		float targetDistance = this.properties.bulletSpeed * bulletTime;
		float apex = targetDistance + 50f;
		float launchSpeed = Mathf.Sqrt(4000f * apex);
		float timeToApex = launchSpeed / 2000f;
		float launchY = base.transform.position.y;
		while (t < timeToApex || base.transform.position.y > targetDistance + launchY)
		{
			t += CupheadTime.FixedDelta;
			float y = launchY + launchSpeed * t - 1000f * t * t;
			base.transform.SetPosition(null, new float?(y), null);
			yield return new WaitForFixedUpdate();
		}
		foreach (string s in this.explodePattern.Split(new char[]
		{
			'-'
		}))
		{
			float rotation = 0f;
			Parser.FloatTryParse(s, out rotation);
			this.spreadshotPrefab.Create(base.transform.position, rotation, this.properties.spreadshotBulletSpeed, this.properties.spiralRate);
		}
		this.explodeEffectPrefab.Create(base.transform.position);
		this.Die();
		yield break;
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x00019460 File Offset: 0x00017660
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001888 RID: 6280
	[SerializeField]
	public FlyingMermaidLevelTurtleSpiralProjectile spreadshotPrefab;

	// Token: 0x04001889 RID: 6281
	[SerializeField]
	public Effect explodeEffectPrefab;

	// Token: 0x0400188A RID: 6282
	public string explodePattern;

	// Token: 0x0400188B RID: 6283
	public LevelProperties.FlyingMermaid.Turtle properties;
}
