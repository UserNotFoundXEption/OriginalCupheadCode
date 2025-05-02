using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200024A RID: 586
public class FlyingBlimpLevelStars : AbstractProjectile
{
	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00016C4D File Offset: 0x00014E4D
	// (set) Token: 0x06001ACD RID: 6861 RVA: 0x00016C55 File Offset: 0x00014E55
	public FlyingBlimpLevelStars.State state { get; set; }

	// Token: 0x06001ACE RID: 6862 RVA: 0x000A9A34 File Offset: 0x000A7C34
	public FlyingBlimpLevelStars Create(Vector2 pos, LevelProperties.FlyingBlimp.Stars properties)
	{
		FlyingBlimpLevelStars flyingBlimpLevelStars = base.Create() as FlyingBlimpLevelStars;
		flyingBlimpLevelStars.properties = properties;
		flyingBlimpLevelStars.transform.position = pos;
		return flyingBlimpLevelStars;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x00016C5E File Offset: 0x00014E5E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x00016C7C File Offset: 0x00014E7C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000A9A68 File Offset: 0x000A7C68
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		float num = (float)Random.Range(0, 2);
		this.starFx = Object.Instantiate<Transform>(this.starFXPrefab);
		this.starFx.transform.parent = base.transform;
		Vector3 position = base.transform.position;
		if (num == 0f)
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			this.starFx.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			position.x = base.transform.position.x + 70f;
		}
		else
		{
			position.x = base.transform.position.x - 10f;
			this.starFx.SetScale(new float?(1f), new float?(-1f), new float?(1f));
		}
		this.starFx.transform.position = position;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000A9BA0 File Offset: 0x000A7DA0
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.speedX.RandomFloat();
		float angle = 0f;
		while (base.transform.position.x > -840f)
		{
			angle += this.properties.speedY * CupheadTime.FixedDelta;
			if (CupheadTime.Delta != 0f)
			{
				Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * this.properties.sineSize);
				Vector3 moveX = base.transform.right * -speed * CupheadTime.FixedDelta;
				base.transform.position += moveX + moveY;
			}
			yield return wait;
		}
		this.Die();
		yield return wait;
		yield break;
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x00016C9A File Offset: 0x00014E9A
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x0400159D RID: 5533
	[SerializeField]
	public Transform starFXPrefab;

	// Token: 0x0400159E RID: 5534
	public Transform starFx;

	// Token: 0x0400159F RID: 5535
	public Vector3 spawnPoint;

	// Token: 0x040015A0 RID: 5536
	public LevelProperties.FlyingBlimp.Stars properties;

	// Token: 0x02000C9B RID: 3227
	public enum State
	{
		// Token: 0x04005B03 RID: 23299
		Unspawned,
		// Token: 0x04005B04 RID: 23300
		Spawned
	}
}
