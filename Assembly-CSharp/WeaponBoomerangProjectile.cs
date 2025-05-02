using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000539 RID: 1337
public class WeaponBoomerangProjectile : AbstractProjectile
{
	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06003853 RID: 14419 RVA: 0x0002DF86 File Offset: 0x0002C186
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x00106A8C File Offset: 0x00104C8C
	public override void Start()
	{
		base.Start();
		this.forwardDir = MathUtils.AngleToDirection(base.transform.rotation.eulerAngles.z);
		this.lateralDir = new Vector2(-this.forwardDir.y, this.forwardDir.x);
		this.lateralDir *= this.player.motor.TrueLookDirection.x;
		this.DestroyDistance = 0f;
		if (this.isEx)
		{
			this.trailPositions = new Vector2[6];
			for (int i = 0; i < this.trailPositions.Length; i++)
			{
				this.trailPositions[i] = base.transform.position;
			}
			base.StartCoroutine(this.ex_cr());
		}
		else
		{
			base.StartCoroutine(this.basic_cr());
		}
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00106B90 File Offset: 0x00104D90
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.wasCaught)
		{
			this.Die();
		}
		if (this.isEx && this.hasTurned && (base.transform.position - this.player.center).magnitude < this.player.colliderManager.Width / 2f + base.GetComponent<CircleCollider2D>().radius)
		{
			this.wasCaught = true;
		}
		bool flag = CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f));
		base.GetComponent<Collider2D>().enabled = flag;
		if (!flag && this.headedOffscreen)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.isEx)
		{
			this.updateTrails();
		}
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00106C90 File Offset: 0x00104E90
	public void updateTrails()
	{
		int num = this.currentPositionIndex - 2;
		if (num < 0)
		{
			num += this.trailPositions.Length;
		}
		int num2 = this.currentPositionIndex - 5;
		if (num2 < 0)
		{
			num2 += this.trailPositions.Length;
		}
		this.trail1.position = this.trailPositions[num];
		this.trail2.position = this.trailPositions[num2];
		this.currentPositionIndex = (this.currentPositionIndex + 1) % this.trailPositions.Length;
		this.trailPositions[this.currentPositionIndex] = base.transform.position;
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x00106D54 File Offset: 0x00104F54
	public override void Die()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.Die();
		this.StopAllCoroutines();
		this.SetInt(AbstractProjectile.Variant, this.variant);
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x00106DA4 File Offset: 0x00104FA4
	public IEnumerator basic_cr()
	{
		Vector2 startPos = base.transform.position;
		Vector2 turnPos = startPos + this.forwardDir * this.forwardDistance + this.lateralDir * this.lateralDistance * 0.5f;
		Vector2 returnPos = startPos + this.lateralDir * this.lateralDistance;
		float moveTime = this.forwardDistance / this.Speed * 1.57079637f;
		yield return base.StartCoroutine(this.move_cr(turnPos, EaseUtils.EaseType.easeOutSine, EaseUtils.EaseType.easeInSine, moveTime));
		this.hasTurned = true;
		yield return base.StartCoroutine(this.move_cr(returnPos, EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutSine, moveTime));
		Vector2 velocity = this.Speed * -this.forwardDir;
		this.headedOffscreen = true;
		for (;;)
		{
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x00106DC0 File Offset: 0x00104FC0
	public IEnumerator ex_cr()
	{
		Vector2 startPos = base.transform.position;
		Vector2 turnPos = startPos + this.forwardDir * this.forwardDistance + this.lateralDir * this.lateralDistance * 0.5f;
		for (;;)
		{
			EaseUtils.EaseType ease = (!this.hasTurned) ? EaseUtils.EaseType.easeOutSine : EaseUtils.EaseType.easeInOutSine;
			float moveTime = ((!this.hasTurned) ? this.forwardDistance : (this.forwardDistance * 2f)) / this.Speed;
			yield return base.StartCoroutine(this.move_cr(turnPos, ease, ease, moveTime));
			this.hasTurned = true;
			startPos = base.transform.position;
			Vector2 playerPos = this.player.transform.position;
			turnPos = playerPos + (playerPos - startPos).normalized * this.forwardDistance;
		}
		yield break;
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x00106DDC File Offset: 0x00104FDC
	public IEnumerator move_cr(Vector2 endPos, EaseUtils.EaseType forwardEaseType, EaseUtils.EaseType lateralEaseType, float time)
	{
		float t = 0f;
		Vector2 startPos = base.transform.localPosition;
		Vector2 relativeEndPos = endPos - startPos;
		float forwardMovement = Vector2.Dot(this.forwardDir, relativeEndPos);
		float lateralMovement = Vector2.Dot(this.lateralDir, relativeEndPos);
		while (t < time)
		{
			while (this.timeUntilUnfreeze > 0f)
			{
				this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
				yield return new WaitForFixedUpdate();
			}
			base.transform.position = startPos + this.forwardDir * EaseUtils.Ease(forwardEaseType, 0f, forwardMovement, t / time) + this.lateralDir * EaseUtils.Ease(lateralEaseType, 0f, lateralMovement, t / time);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.position = endPos;
		yield break;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x00106E14 File Offset: 0x00105014
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		if (this.isEx)
		{
			this.totalDamage += num;
			if (this.totalDamage > this.maxDamage)
			{
				this.Die();
			}
			if (num > 0f)
			{
				this.hitFXPrefab.Create(base.transform.position);
				this.timeUntilUnfreeze = this.hitFreezeTime;
				AudioManager.Play("player_ex_impact_hit");
				this.emitAudioFromObject.Add("player_ex_impact_hit");
			}
		}
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x0002DF8D File Offset: 0x0002C18D
	public void SetPink(bool pink)
	{
		if (pink)
		{
			this.SetParryable(true);
			this.variant = 2;
		}
		else
		{
			this.SetParryable(false);
			this.variant = Random.Range(0, 2);
		}
		this.SetInt(AbstractProjectile.Variant, this.variant);
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x00106EB0 File Offset: 0x001050B0
	public override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_peashot_miss");
			}
		}
	}

	// Token: 0x04002D3F RID: 11583
	public const float TurnTimeRatio = 0.4f;

	// Token: 0x04002D40 RID: 11584
	public float Speed;

	// Token: 0x04002D41 RID: 11585
	public float forwardDistance;

	// Token: 0x04002D42 RID: 11586
	public float lateralDistance;

	// Token: 0x04002D43 RID: 11587
	public float maxDamage;

	// Token: 0x04002D44 RID: 11588
	public float hitFreezeTime;

	// Token: 0x04002D45 RID: 11589
	public LevelPlayerController player;

	// Token: 0x04002D46 RID: 11590
	[SerializeField]
	public bool isEx;

	// Token: 0x04002D47 RID: 11591
	[SerializeField]
	public Transform trail1;

	// Token: 0x04002D48 RID: 11592
	[SerializeField]
	public Transform trail2;

	// Token: 0x04002D49 RID: 11593
	[SerializeField]
	public Effect hitFXPrefab;

	// Token: 0x04002D4A RID: 11594
	public Vector2[] trailPositions;

	// Token: 0x04002D4B RID: 11595
	public int currentPositionIndex;

	// Token: 0x04002D4C RID: 11596
	public const int trailFrameDelay = 3;

	// Token: 0x04002D4D RID: 11597
	public Vector2 forwardDir;

	// Token: 0x04002D4E RID: 11598
	public Vector2 lateralDir;

	// Token: 0x04002D4F RID: 11599
	public bool hasTurned;

	// Token: 0x04002D50 RID: 11600
	public bool wasCaught;

	// Token: 0x04002D51 RID: 11601
	public bool headedOffscreen;

	// Token: 0x04002D52 RID: 11602
	public float totalDamage;

	// Token: 0x04002D53 RID: 11603
	public int variant;

	// Token: 0x04002D54 RID: 11604
	public float timeUntilUnfreeze;
}
