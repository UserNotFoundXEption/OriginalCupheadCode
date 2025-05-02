using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class AirshipClamLevelBarnacleParryable : ParrySwitch
{
	// Token: 0x06000ED5 RID: 3797 RVA: 0x0000C923 File Offset: 0x0000AB23
	public override void Awake()
	{
		this.parried = false;
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0000C93D File Offset: 0x0000AB3D
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0008C4B4 File Offset: 0x0008A6B4
	public void InitBarnacle(int dir, LevelProperties.AirshipClam properties)
	{
		this.onPlayerCollisionDeath = true;
		this.properties = properties;
		base.GetComponent<SpriteRenderer>().sprite = this.parraybleBarnacleSprite;
		this.direction = dir;
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0008C500 File Offset: 0x0008A700
	public IEnumerator move_cr()
	{
		this.velocity = new Vector3(this.properties.CurrentState.barnacles.initialArcMovementX * (float)this.direction, this.properties.CurrentState.barnacles.initialArcMovementY, 0f);
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			if (!this.parried)
			{
				this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.initialGravity;
			}
			else
			{
				this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.parryGravity;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0000C94A File Offset: 0x0000AB4A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.onPlayerCollisionDeath)
		{
			this.damageDealer.DealDamage(hit);
			base.OnCollisionPlayer(hit, phase);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0000C977 File Offset: 0x0000AB77
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		this.velocity.x = 0f;
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0000C991 File Offset: 0x0000AB91
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0008C51C File Offset: 0x0008A71C
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.direction = ((base.transform.position.x <= player.center.x) ? -1 : 1);
		this.parried = true;
		this.velocity.y = this.properties.CurrentState.barnacles.parryArcMovementY;
		this.velocity.x = this.properties.CurrentState.barnacles.parryArcMovementX * (float)this.direction;
		base.OnParryPostPause(player);
		this.circleCollider.enabled = true;
		base.StartCoroutine(this.damageTypes_cr());
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0008C5CC File Offset: 0x0008A7CC
	public IEnumerator damageTypes_cr()
	{
		this.damageDealer.SetDamageFlags(false, false, false);
		this.onPlayerCollisionDeath = false;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.damageDealer.SetDamageFlags(true, false, false);
		this.onPlayerCollisionDeath = true;
		yield break;
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0000C9A6 File Offset: 0x0000ABA6
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04000C21 RID: 3105
	public bool parried;

	// Token: 0x04000C22 RID: 3106
	public bool onPlayerCollisionDeath;

	// Token: 0x04000C23 RID: 3107
	public int direction;

	// Token: 0x04000C24 RID: 3108
	public Vector3 velocity;

	// Token: 0x04000C25 RID: 3109
	public LevelProperties.AirshipClam properties;

	// Token: 0x04000C26 RID: 3110
	public CircleCollider2D circleCollider;

	// Token: 0x04000C27 RID: 3111
	public DamageDealer damageDealer;

	// Token: 0x04000C28 RID: 3112
	[SerializeField]
	public Sprite parraybleBarnacleSprite;
}
