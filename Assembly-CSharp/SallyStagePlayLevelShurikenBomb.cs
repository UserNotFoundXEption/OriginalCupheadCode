using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000364 RID: 868
public class SallyStagePlayLevelShurikenBomb : AbstractProjectile
{
	// Token: 0x06002679 RID: 9849 RVA: 0x000C9094 File Offset: 0x000C7294
	public void InitShuriken(LevelProperties.SallyStagePlay properties, int direction, AbstractPlayerController target)
	{
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.properties = properties;
		this.speed = properties.CurrentState.shuriken.InitialMovementSpeed;
		this.target = target;
		this.isActive = true;
		this.childSpawnCount = 0;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000C90EC File Offset: 0x000C72EC
	public void InitChildShuriken(int direction, int childSpawnCount, AbstractPlayerController target, LevelProperties.SallyStagePlay properties)
	{
		this.properties = properties;
		base.GetComponent<SpriteRenderer>().sprite = this.shuriken;
		if (childSpawnCount > 1)
		{
			this.currentYVelocity = properties.CurrentState.shuriken.ArcTwoVerticalVelocity;
			this.horizontalVelocity = properties.CurrentState.shuriken.ArcTwoHorizontalVelocity * Mathf.Sign((float)direction);
			this.gravity = properties.CurrentState.shuriken.ArcTwoGravity;
		}
		else
		{
			this.currentYVelocity = properties.CurrentState.shuriken.ArcOneVerticalVelocity;
			this.horizontalVelocity = properties.CurrentState.shuriken.ArcOneHorizontalVelocity * Mathf.Sign((float)direction);
			this.gravity = properties.CurrentState.shuriken.ArcOneGravity;
		}
		this.target = target;
		this.isActive = true;
		this.childSpawnCount = childSpawnCount;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000C91DC File Offset: 0x000C73DC
	public IEnumerator move_cr()
	{
		if (this.target == null || this.target.IsDead)
		{
			this.target = PlayerManager.GetNext();
		}
		Vector3 direction = (new Vector3(this.target.center.x, (float)Level.Current.Ground, 0f) - base.transform.position).normalized;
		for (;;)
		{
			if (this.boxCollider != null)
			{
				this.boxCollider.enabled = true;
			}
			if (this.childSpawnCount > 0)
			{
				base.transform.position += (Vector3.right * this.horizontalVelocity + Vector3.up * this.currentYVelocity) * CupheadTime.Delta;
				this.currentYVelocity -= this.gravity;
			}
			else
			{
				base.transform.position += direction * this.speed * CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x000C91F8 File Offset: 0x000C73F8
	public void Explode()
	{
		base.GetComponent<SpriteRenderer>().sprite = this.explosion;
		if (this.childSpawnCount < this.properties.CurrentState.shuriken.NumberOfChildSpawns)
		{
			this.childSpawnCount++;
			float x = base.GetComponent<SpriteRenderer>().bounds.size.x;
			for (int i = -1; i < 1; i++)
			{
				AbstractProjectile abstractProjectile = this.Create(base.transform.position + Vector3.right * x / 2f * Mathf.Sign((float)i) + Vector3.up * 50f);
				abstractProjectile.GetComponent<SallyStagePlayLevelShurikenBomb>().InitChildShuriken(i, this.childSpawnCount, this.target, this.properties);
			}
		}
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x000204D6 File Offset: 0x0001E6D6
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.isActive)
		{
			this.isActive = false;
			this.Explode();
			this.StopAllCoroutines();
			Object.Destroy(base.gameObject, 0.1f);
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x0002050E File Offset: 0x0001E70E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x04001FBB RID: 8123
	[SerializeField]
	public Sprite shuriken;

	// Token: 0x04001FBC RID: 8124
	[SerializeField]
	public Sprite explosion;

	// Token: 0x04001FBD RID: 8125
	public float currentYVelocity;

	// Token: 0x04001FBE RID: 8126
	public float horizontalVelocity;

	// Token: 0x04001FBF RID: 8127
	public float gravity;

	// Token: 0x04001FC0 RID: 8128
	public AbstractPlayerController target;

	// Token: 0x04001FC1 RID: 8129
	public float speed;

	// Token: 0x04001FC2 RID: 8130
	public int childSpawnCount;

	// Token: 0x04001FC3 RID: 8131
	public bool isActive;

	// Token: 0x04001FC4 RID: 8132
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001FC5 RID: 8133
	public BoxCollider2D boxCollider;
}
