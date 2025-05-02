using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class AirshipCrabLevelGems : ParrySwitch
{
	// Token: 0x06000F04 RID: 3844 RVA: 0x0008CA6C File Offset: 0x0008AC6C
	public void Init(LevelProperties.AirshipCrab.Gems properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.pink = base.GetComponent<SpriteRenderer>().color;
		this.startPos = base.transform.position;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-angle));
		this.velocity = -base.transform.right;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0000CB6F File Offset: 0x0000AD6F
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = AirshipCrabLevelGems.SideHit.None;
		this.parried = false;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0000CB90 File Offset: 0x0000AD90
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, CollisionPhase.Enter);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0008CAF0 File Offset: 0x0008ACF0
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != AirshipCrabLevelGems.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = AirshipCrabLevelGems.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0008CB74 File Offset: 0x0008AD74
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != AirshipCrabLevelGems.SideHit.Right)
				{
					Vector3 vector;
					vector..ctor((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = vector - position;
					this.lastSideHit = AirshipCrabLevelGems.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != AirshipCrabLevelGems.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 vector2;
				vector2..ctor((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = vector2 - position2;
				this.lastSideHit = AirshipCrabLevelGems.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0008CC88 File Offset: 0x0008AE88
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != AirshipCrabLevelGems.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = AirshipCrabLevelGems.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0008CD0C File Offset: 0x0008AF0C
	public void PickMovement()
	{
		if (!this.parried)
		{
			if (this.currentMovement != null)
			{
				base.StopCoroutine(this.currentMovement);
			}
			this.currentMovement = base.StartCoroutine(this.move_cr());
		}
		else
		{
			if (this.currentMovement != null)
			{
				base.StopCoroutine(this.currentMovement);
			}
			this.currentMovement = base.StartCoroutine(this.go_back_cr());
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0008CD7C File Offset: 0x0008AF7C
	public IEnumerator move_cr()
	{
		this.moving = true;
		this.parried = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().color = this.pink;
		while (this.moving)
		{
			base.transform.position += this.velocity * this.properties.bulletSpeed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0008CD98 File Offset: 0x0008AF98
	public IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0008CDBC File Offset: 0x0008AFBC
	public IEnumerator go_back_cr()
	{
		this.velocity = -base.transform.right;
		while (base.transform.position != this.startPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.bulletSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0000CBBF File Offset: 0x0000ADBF
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0008CDD8 File Offset: 0x0008AFD8
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.startTimer = true;
		this.parried = true;
		this.moving = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		this.PickMovement();
	}

	// Token: 0x04000C4A RID: 3146
	public bool parried;

	// Token: 0x04000C4B RID: 3147
	public bool startTimer;

	// Token: 0x04000C4C RID: 3148
	public bool moving;

	// Token: 0x04000C4D RID: 3149
	public AirshipCrabLevelGems.SideHit lastSideHit;

	// Token: 0x04000C4E RID: 3150
	public LevelProperties.AirshipCrab.Gems properties;

	// Token: 0x04000C4F RID: 3151
	public DamageDealer damageDealer;

	// Token: 0x04000C50 RID: 3152
	public Color pink;

	// Token: 0x04000C51 RID: 3153
	public Vector3 velocity;

	// Token: 0x04000C52 RID: 3154
	public Vector3 startPos;

	// Token: 0x04000C53 RID: 3155
	public Vector3 collisionPoint;

	// Token: 0x04000C54 RID: 3156
	public bool getCollisionPoint;

	// Token: 0x04000C55 RID: 3157
	public Coroutine currentMovement;

	// Token: 0x020009E5 RID: 2533
	public enum SideHit
	{
		// Token: 0x04004982 RID: 18818
		Top,
		// Token: 0x04004983 RID: 18819
		Bottom,
		// Token: 0x04004984 RID: 18820
		Left,
		// Token: 0x04004985 RID: 18821
		Right,
		// Token: 0x04004986 RID: 18822
		None
	}
}
