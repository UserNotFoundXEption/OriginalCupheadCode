using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class TrainLevelEngineBossFireProjectile : AbstractProjectile
{
	// Token: 0x060029B8 RID: 10680 RVA: 0x0002319D File Offset: 0x0002139D
	public void Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		this.InstantiatePrefab<TrainLevelEngineBossFireProjectile>().Init(pos, velocity, gravity);
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000D2940 File Offset: 0x000D0B40
	public override void Start()
	{
		base.Start();
		base.animator.SetFloat("Direction", (float)((this.velocity.x <= 0f) ? 1 : -1));
		base.animator.Play("Idle", 0, Random.value);
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.collider2d = base.GetComponent<CircleCollider2D>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.spawnTrail_cr());
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000D29E4 File Offset: 0x000D0BE4
	public override void Update()
	{
		base.Update();
		if (this.state == TrainLevelEngineBossFireProjectile.State.Moving)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			if (base.transform.position.y < -300f)
			{
				this.Die();
			}
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000D2A88 File Offset: 0x000D0C88
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		int num = this.collider2d.OverlapCollider(TrainLevelEngineBossFireProjectile.filter, TrainLevelEngineBossFireProjectile.buffer);
		for (int i = 0; i < num; i++)
		{
			if (TrainLevelEngineBossFireProjectile.buffer[i].GetComponent<TrainLevelPlatform>())
			{
				this.Die();
				break;
			}
		}
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000231AD File Offset: 0x000213AD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000231D1 File Offset: 0x000213D1
	public override void Die()
	{
		base.Die();
		base.transform.rotation = Quaternion.identity;
		this.spriteRenderer.flipX = Rand.Bool();
		this.state = TrainLevelEngineBossFireProjectile.State.Dead;
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x00023200 File Offset: 0x00021400
	public void Init(Vector2 pos, Vector2 velocity, float gravity)
	{
		base.transform.position = pos;
		this.velocity = velocity;
		this.gravity = gravity;
		this.state = TrainLevelEngineBossFireProjectile.State.Moving;
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000D2AE4 File Offset: 0x000D0CE4
	public IEnumerator spawnTrail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.trailPrefab.Create(base.transform.position + TrainLevelEngineBossFireProjectile.TrailOffset).Play();
		}
		yield break;
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x00023228 File Offset: 0x00021428
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.trailPrefab = null;
	}

	// Token: 0x040022EE RID: 8942
	public const string IdleStateName = "Idle";

	// Token: 0x040022EF RID: 8943
	public const string DirectionParameterName = "Direction";

	// Token: 0x040022F0 RID: 8944
	public static readonly Vector3 TrailOffset = new Vector3(0f, 10f, 0f);

	// Token: 0x040022F1 RID: 8945
	public static ContactFilter2D filter = default(ContactFilter2D).NoFilter();

	// Token: 0x040022F2 RID: 8946
	public static Collider2D[] buffer = new Collider2D[10];

	// Token: 0x040022F3 RID: 8947
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x040022F4 RID: 8948
	public const float GROUND_Y = -300f;

	// Token: 0x040022F5 RID: 8949
	public TrainLevelEngineBossFireProjectile.State state;

	// Token: 0x040022F6 RID: 8950
	public Vector2 velocity;

	// Token: 0x040022F7 RID: 8951
	public float gravity;

	// Token: 0x040022F8 RID: 8952
	public CircleCollider2D collider2d;

	// Token: 0x040022F9 RID: 8953
	public SpriteRenderer spriteRenderer;

	// Token: 0x02000FAE RID: 4014
	public enum State
	{
		// Token: 0x0400710C RID: 28940
		Init,
		// Token: 0x0400710D RID: 28941
		Moving,
		// Token: 0x0400710E RID: 28942
		Dead
	}
}
