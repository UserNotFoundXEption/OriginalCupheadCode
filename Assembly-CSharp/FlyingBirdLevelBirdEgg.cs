using System;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class FlyingBirdLevelBirdEgg : AbstractProjectile
{
	// Token: 0x060019A1 RID: 6561 RVA: 0x000A6BBC File Offset: 0x000A4DBC
	public virtual AbstractProjectile Create(float speed, Vector2 pos)
	{
		FlyingBirdLevelBirdEgg flyingBirdLevelBirdEgg = this.Create(pos, 0f) as FlyingBirdLevelBirdEgg;
		flyingBirdLevelBirdEgg.speed = -speed;
		flyingBirdLevelBirdEgg.CollisionDeath.OnlyPlayer();
		flyingBirdLevelBirdEgg.DamagesType.OnlyPlayer();
		return flyingBirdLevelBirdEgg;
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000A6BFC File Offset: 0x000A4DFC
	public override void Start()
	{
		base.Start();
		Level.Mode mode = Level.Current.mode;
		if (mode != Level.Mode.Easy)
		{
			if (mode != Level.Mode.Normal)
			{
				if (mode == Level.Mode.Hard)
				{
					this.maxProjectiles = 5;
				}
			}
			else
			{
				this.maxProjectiles = 3;
			}
		}
		else
		{
			this.maxProjectiles = 2;
		}
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000A6C58 File Offset: 0x000A4E58
	public override void Update()
	{
		base.Update();
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (this.state == FlyingBirdLevelBirdEgg.State.Idle && base.transform.position.x < -640f)
		{
			this.Explode();
			this.Die();
		}
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x00015E27 File Offset: 0x00014027
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000A6CDC File Offset: 0x000A4EDC
	public void Explode()
	{
		AudioManager.Play("level_flying_bird_egg_explode");
		this.emitAudioFromObject.Add("level_flying_bird_egg_explode");
		AudioManager.Play("level_flying_bird_egg_break");
		this.emitAudioFromObject.Add("level_flying_bird_egg_break");
		if (this.state != FlyingBirdLevelBirdEgg.State.Idle)
		{
			return;
		}
		this.state = FlyingBirdLevelBirdEgg.State.Exploded;
		this.effectPrefab.Create(base.transform.position);
		if (this.maxProjectiles == 0)
		{
			return;
		}
		Vector3 position = base.transform.position;
		position.x += 42f;
		if (this.maxProjectiles == 2)
		{
			this.childPrefab.Create(position, 90f, Vector2.one, -this.speed);
			this.childPrefab.Create(position, -90f, Vector2.one, -this.speed);
		}
		else
		{
			for (int i = 0; i < this.maxProjectiles; i++)
			{
				float rotation;
				switch (i)
				{
				default:
					rotation = 0f;
					break;
				case 1:
					rotation = -45f;
					break;
				case 2:
					rotation = 45f;
					break;
				case 3:
					rotation = 90f;
					break;
				case 4:
					rotation = -90f;
					break;
				}
				this.childPrefab.Create(position, rotation, Vector2.one, -this.speed);
			}
		}
	}

	// Token: 0x0400149D RID: 5277
	public const float ANGLE = 45f;

	// Token: 0x0400149E RID: 5278
	[SerializeField]
	public BasicProjectile childPrefab;

	// Token: 0x0400149F RID: 5279
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x040014A0 RID: 5280
	public float speed;

	// Token: 0x040014A1 RID: 5281
	public FlyingBirdLevelBirdEgg.State state;

	// Token: 0x040014A2 RID: 5282
	public int maxProjectiles;

	// Token: 0x02000C49 RID: 3145
	public enum State
	{
		// Token: 0x04005907 RID: 22791
		Idle,
		// Token: 0x04005908 RID: 22792
		Exploded
	}
}
