using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class FrogsLevelShortClapBullet : AbstractProjectile
{
	// Token: 0x06001E77 RID: 7799 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
	public FrogsLevelShortClapBullet Create(FrogsLevelShort.Direction frogDir, FrogsLevelShortClapBullet.Direction dir, Vector2 pos, Vector2 velocity)
	{
		FrogsLevelShortClapBullet frogsLevelShortClapBullet = base.Create(pos) as FrogsLevelShortClapBullet;
		frogsLevelShortClapBullet.CollisionDeath.OnlyPlayer();
		frogsLevelShortClapBullet.DamagesType.OnlyPlayer();
		frogsLevelShortClapBullet.Init(frogDir, dir, pos, velocity);
		return frogsLevelShortClapBullet;
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x00019BA7 File Offset: 0x00017DA7
	public void Init(FrogsLevelShort.Direction frogDir, FrogsLevelShortClapBullet.Direction dir, Vector2 pos, Vector2 velocity)
	{
		this.frogDirection = frogDir;
		this.velocity = velocity;
		this.direction = dir;
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x00019BCC File Offset: 0x00017DCC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x00019BE2 File Offset: 0x00017DE2
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000B2D34 File Offset: 0x000B0F34
	public IEnumerator go_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float upY = this.velocity.y;
		float downY = -this.velocity.y;
		float x = (this.frogDirection != FrogsLevelShort.Direction.Right) ? (-this.velocity.x) : this.velocity.x;
		float y = (this.direction != FrogsLevelShortClapBullet.Direction.Up) ? downY : upY;
		if (this.direction == FrogsLevelShortClapBullet.Direction.Up)
		{
			base.transform.LookAt2D(base.transform.position + new Vector2(x, upY));
		}
		else
		{
			base.transform.LookAt2D(base.transform.position + new Vector2(x, downY));
		}
		for (;;)
		{
			if (this.direction == FrogsLevelShortClapBullet.Direction.Up)
			{
				if (base.transform.position.y >= 360f)
				{
					AudioManager.Play("level_frogs_short_clap_bounce");
					this.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
					this.direction = FrogsLevelShortClapBullet.Direction.Down;
					this.bounceEffect.Create(base.transform.position, new Vector3(1f, -1f, 1f));
					y = downY;
					base.transform.LookAt2D(base.transform.position + new Vector2(x, y));
				}
			}
			else if (base.transform.position.y <= (float)Level.Current.Ground)
			{
				AudioManager.Play("level_frogs_short_clap_bounce");
				this.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
				this.direction = FrogsLevelShortClapBullet.Direction.Up;
				this.bounceEffect.Create(base.transform.position);
				y = upY;
				base.transform.LookAt2D(base.transform.position + new Vector2(x, y));
			}
			if (base.transform.position.x > 640f + base.GetComponent<SpriteRenderer>().bounds.size.x / 2f)
			{
				break;
			}
			base.transform.AddPosition(x * CupheadTime.FixedDelta, y * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x040018D8 RID: 6360
	public const float MAX_Y = 360f;

	// Token: 0x040018D9 RID: 6361
	[SerializeField]
	public Effect bounceEffect;

	// Token: 0x040018DA RID: 6362
	public Vector2 velocity;

	// Token: 0x040018DB RID: 6363
	public FrogsLevelShort.Direction frogDirection;

	// Token: 0x040018DC RID: 6364
	public FrogsLevelShortClapBullet.Direction direction;

	// Token: 0x02000D7F RID: 3455
	public enum Direction
	{
		// Token: 0x040061BC RID: 25020
		Up,
		// Token: 0x040061BD RID: 25021
		Down
	}
}
