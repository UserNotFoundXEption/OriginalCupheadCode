using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class TrainLevelEngineBossDropperProjectile : AbstractProjectile
{
	// Token: 0x060029B1 RID: 10673 RVA: 0x000D2874 File Offset: 0x000D0A74
	public TrainLevelEngineBossDropperProjectile Create(Vector2 pos, float upSpeed, float xSpeed, float gravity)
	{
		TrainLevelEngineBossDropperProjectile trainLevelEngineBossDropperProjectile = this.InstantiatePrefab<TrainLevelEngineBossDropperProjectile>();
		trainLevelEngineBossDropperProjectile.Init(pos, upSpeed, xSpeed, gravity);
		return trainLevelEngineBossDropperProjectile;
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x000D2894 File Offset: 0x000D0A94
	public void Init(Vector2 pos, float upSpeed, float xSpeed, float gravity)
	{
		base.transform.position = pos;
		this.velocity.y = upSpeed;
		this.velocity.x = xSpeed;
		this.gravity = gravity;
		base.transform.localScale = Vector3.one * 0.5f;
		base.StartCoroutine(this.go_cr());
		base.StartCoroutine(this.scale_cr());
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000D2908 File Offset: 0x000D0B08
	public IEnumerator scale_cr()
	{
		float t = 0f;
		while (t < 0.4f)
		{
			base.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, t / 0.4f);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000D2924 File Offset: 0x000D0B24
	public IEnumerator go_cr()
	{
		AbstractPlayerController target = PlayerManager.GetNext();
		while (base.transform.position.y > target.center.y)
		{
			Vector3 vel = Vector3.zero;
			base.transform.AddPosition(0f, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
			if (target == null || target.IsDead)
			{
				target = PlayerManager.GetNext();
			}
		}
		int direction = (target.center.x <= base.transform.position.x) ? -1 : 1;
		base.transform.localScale = new Vector3((float)(-(float)direction), 1f, 1f);
		base.animator.SetTrigger("Horizontal");
		this.dustFX.Create(base.transform.position, new Vector3((float)direction, 1f, 1f)).Play();
		this.verticalCollider.enabled = false;
		this.horizontalCollider.enabled = true;
		for (;;)
		{
			base.transform.AddPosition((float)direction * this.velocity.x * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x00023162 File Offset: 0x00021362
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x00023186 File Offset: 0x00021386
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dustFX = null;
	}

	// Token: 0x040022E6 RID: 8934
	public const string HorizontalParameterName = "Horizontal";

	// Token: 0x040022E7 RID: 8935
	public const float ScaleTime = 0.4f;

	// Token: 0x040022E8 RID: 8936
	public const float StartScale = 0.5f;

	// Token: 0x040022E9 RID: 8937
	[SerializeField]
	public Effect dustFX;

	// Token: 0x040022EA RID: 8938
	[SerializeField]
	public CircleCollider2D verticalCollider;

	// Token: 0x040022EB RID: 8939
	[SerializeField]
	public BoxCollider2D horizontalCollider;

	// Token: 0x040022EC RID: 8940
	public Vector2 velocity;

	// Token: 0x040022ED RID: 8941
	public float gravity;
}
