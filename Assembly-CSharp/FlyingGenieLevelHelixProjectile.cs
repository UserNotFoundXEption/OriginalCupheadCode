using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class FlyingGenieLevelHelixProjectile : AbstractProjectile
{
	// Token: 0x06001C83 RID: 7299 RVA: 0x000AE574 File Offset: 0x000AC774
	public FlyingGenieLevelHelixProjectile Create(Vector3 pos, LevelProperties.FlyingGenie.Coffin properties, bool topOne)
	{
		FlyingGenieLevelHelixProjectile flyingGenieLevelHelixProjectile = base.Create() as FlyingGenieLevelHelixProjectile;
		flyingGenieLevelHelixProjectile.properties = properties;
		flyingGenieLevelHelixProjectile.transform.position = pos;
		flyingGenieLevelHelixProjectile.topOne = topOne;
		return flyingGenieLevelHelixProjectile;
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000182D1 File Offset: 0x000164D1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000182FA File Offset: 0x000164FA
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00018318 File Offset: 0x00016518
	public override void Start()
	{
		base.Start();
		base.animator.SetBool("OnTop", this.topOne);
		base.StartCoroutine(this.moveY_cr());
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000AE5A8 File Offset: 0x000AC7A8
	public IEnumerator moveY_cr()
	{
		float angle = 0f;
		float xSpeed = this.properties.heartShotXSpeed;
		float ySpeed = this.properties.heartShotYSpeed;
		Vector3 moveX = base.transform.position;
		while (base.transform.position.x != -640f)
		{
			float loopSize;
			if (this.topOne)
			{
				loopSize = this.properties.heartLoopYSize;
				ySpeed = this.properties.heartShotYSpeed;
			}
			else
			{
				loopSize = -this.properties.heartLoopYSize;
				ySpeed = -this.properties.heartShotYSpeed;
			}
			angle += ySpeed * CupheadTime.Delta;
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle + this.properties.heartLoopYSize) * CupheadTime.Delta * 60f * loopSize / 2f);
			moveX = -base.transform.right * xSpeed * CupheadTime.Delta;
			base.transform.position += moveX + moveY;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x04001730 RID: 5936
	public LevelProperties.FlyingGenie.Coffin properties;

	// Token: 0x04001731 RID: 5937
	public bool topOne;
}
