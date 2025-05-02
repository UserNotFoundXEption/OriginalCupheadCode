using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000265 RID: 613
public class FlyingGenieLevelBouncer : AbstractProjectile
{
	// Token: 0x06001BFC RID: 7164 RVA: 0x000AD174 File Offset: 0x000AB374
	public FlyingGenieLevelBouncer Init(Vector3 pos, LevelProperties.FlyingGenie.Obelisk properties, float angle)
	{
		base.transform.position = pos;
		this.properties = properties;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		return this;
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x00017B9F File Offset: 0x00015D9F
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x00017BB4 File Offset: 0x00015DB4
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x00017BD2 File Offset: 0x00015DD2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x000AD1E8 File Offset: 0x000AB3E8
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (Vector3.Dot(hit.transform.position, Vector3.right) > 0f)
			{
				Vector3 vector;
				vector..ctor((float)Level.Current.Left, base.transform.position.y, 0f);
				Vector3 position = base.transform.position;
				this.collisionPoint = vector - position;
				base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
			}
			else
			{
				Vector3 position2 = base.transform.position;
				Vector3 vector2;
				vector2..ctor((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = vector2 - position2;
				base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x000AD2D8 File Offset: 0x000AB4D8
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = vector - position;
			base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
		}
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x000AD34C File Offset: 0x000AB54C
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = vector - position;
			base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
		}
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x000AD3C0 File Offset: 0x000AB5C0
	public IEnumerator move_cr()
	{
		this.velocity = base.transform.up;
		this.speed = this.properties.bouncerSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.Delta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000AD3DC File Offset: 0x000AB5DC
	public IEnumerator change_dir_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000AD400 File Offset: 0x000AB600
	public void ChangeSpeed()
	{
		if (Vector3.Dot(this.velocity, Vector3.right) > 0f)
		{
			float num = this.properties.bouncerSpeed - this.properties.obeliskMovementSpeed;
			this.speed -= num;
		}
		else
		{
			this.speed = this.properties.bouncerSpeed;
		}
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x00017BF0 File Offset: 0x00015DF0
	public override void Die()
	{
		base.Die();
	}

	// Token: 0x040016B9 RID: 5817
	[SerializeField]
	public Transform sprite;

	// Token: 0x040016BA RID: 5818
	public LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x040016BB RID: 5819
	public Vector3 velocity;

	// Token: 0x040016BC RID: 5820
	public Vector3 average;

	// Token: 0x040016BD RID: 5821
	public Vector3 collisionPoint;

	// Token: 0x040016BE RID: 5822
	public float speed;
}
