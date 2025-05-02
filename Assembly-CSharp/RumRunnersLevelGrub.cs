using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000343 RID: 835
public class RumRunnersLevelGrub : AbstractProjectile
{
	// Token: 0x17000303 RID: 771
	// (get) Token: 0x0600248B RID: 9355 RVA: 0x0001EE23 File Offset: 0x0001D023
	// (set) Token: 0x0600248C RID: 9356 RVA: 0x0001EE2B File Offset: 0x0001D02B
	public int x { get; set; }

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x0600248D RID: 9357 RVA: 0x0001EE34 File Offset: 0x0001D034
	// (set) Token: 0x0600248E RID: 9358 RVA: 0x0001EE3C File Offset: 0x0001D03C
	public int y { get; set; }

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x0600248F RID: 9359 RVA: 0x0001EE45 File Offset: 0x0001D045
	// (set) Token: 0x06002490 RID: 9360 RVA: 0x0001EE4D File Offset: 0x0001D04D
	public float speed { get; set; }

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06002491 RID: 9361 RVA: 0x0001EE56 File Offset: 0x0001D056
	// (set) Token: 0x06002492 RID: 9362 RVA: 0x0001EE5E File Offset: 0x0001D05E
	public bool moving { get; set; }

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06002493 RID: 9363 RVA: 0x0001EE67 File Offset: 0x0001D067
	// (set) Token: 0x06002494 RID: 9364 RVA: 0x0001EE6F File Offset: 0x0001D06F
	public bool startedEntering { get; set; }

	// Token: 0x06002495 RID: 9365 RVA: 0x000C4340 File Offset: 0x000C2540
	public RumRunnersLevelGrub Create(RumRunnersLevelGrubPath path, float rotation, float speed, float time, float hp, RumRunnersLevelSpider parent, int enterVariant, int variant, int spawnOrder, int x, int y)
	{
		RumRunnersLevelGrub rumRunnersLevelGrub = base.Create(path.start, rotation) as RumRunnersLevelGrub;
		rumRunnersLevelGrub.transform.localScale = new Vector3(0.3f * Mathf.Sign(path.transform.position.x - path.start.x), 0.3f);
		rumRunnersLevelGrub.path = path;
		rumRunnersLevelGrub.speed = speed;
		rumRunnersLevelGrub.time = time;
		rumRunnersLevelGrub.hp = hp;
		rumRunnersLevelGrub.parent = parent;
		rumRunnersLevelGrub.GetComponent<Collider2D>().enabled = true;
		rumRunnersLevelGrub.enterVariant = enterVariant;
		rumRunnersLevelGrub.variant = variant;
		rumRunnersLevelGrub.animator.SetInteger("Variant", enterVariant);
		rumRunnersLevelGrub.animator.SetInteger("BlinkLoops", Random.Range((int)RumRunnersLevelGrub.BlinkLoopsRange.minimum, (int)RumRunnersLevelGrub.BlinkLoopsRange.maximum + 1));
		rumRunnersLevelGrub.animator.Play("Start", 0, 0f);
		rumRunnersLevelGrub.spawnOrder = spawnOrder;
		rumRunnersLevelGrub.shadowDist = this.shadowTransform.localPosition.y;
		rumRunnersLevelGrub.x = x;
		rumRunnersLevelGrub.y = y;
		return rumRunnersLevelGrub;
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x000C4470 File Offset: 0x000C2670
	public override void Start()
	{
		base.Start();
		this.collider = base.GetComponent<Collider2D>();
		if (base.GetComponent<DamageReceiver>())
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.onDamageTaken;
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x0001EE78 File Offset: 0x0001D078
	public override void OnDieDistance()
	{
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x0001EE7A File Offset: 0x0001D07A
	public override void OnDieLifetime()
	{
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x000C44D0 File Offset: 0x000C26D0
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.moving)
		{
			this.horizontalSpeedEasingTime += CupheadTime.FixedDelta;
			this.basePos.x = this.basePos.x + Mathf.Lerp(0f, this.speed, this.horizontalSpeedEasingTime / 0.5f) * CupheadTime.FixedDelta;
			if (this.finishedEntering)
			{
				this.basePos.y = RumRunnersLevel.GroundWalkingPosY(this.basePos, null, this.yOffset, 200f);
			}
			if (this.basePos.x > 960f || this.basePos.x < -960f)
			{
				Object.Destroy(base.gameObject);
			}
			base.transform.position = this.basePos + this.wobblePos();
			if (this.finishedEntering)
			{
				this.shadowTransform.position = new Vector3(base.transform.position.x, this.basePos.y + this.shadowDist);
				this.wobbleTimer += this.wobbleSpeed * CupheadTime.FixedDelta;
			}
		}
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x0001EE7C File Offset: 0x0001D07C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600249B RID: 9371 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
	public void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.die(true);
		}
	}

	// Token: 0x0600249C RID: 9372 RVA: 0x0001EEDA File Offset: 0x0001D0DA
	public Vector3 wobblePos()
	{
		return new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY, 0f);
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x000C4610 File Offset: 0x000C2810
	public float GetTimeToMove()
	{
		if (this.moving)
		{
			return 0f;
		}
		if (!this.startedEntering)
		{
			return (15f + RumRunnersLevelGrub.flipEndClipLength[this.variant]) * 0.0416666679f;
		}
		int num = Animator.StringToHash(base.animator.GetLayerName(0) + ".Flip");
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == num)
		{
			return (15f * (1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) + RumRunnersLevelGrub.flipEndClipLength[this.variant]) * 1f / 24f;
		}
		return RumRunnersLevelGrub.flipEndClipLength[this.variant] * (1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 1f / 24f;
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x000C46F8 File Offset: 0x000C28F8
	public IEnumerator move_cr()
	{
		this.collider.enabled = false;
		float t = 0f;
		Vector3 destinationPoint = new Vector3(this.path.GetPoint(1f).x, RumRunnersLevel.GroundWalkingPosY(this.path.GetPoint(1f), null, this.yOffset, 200f) + 4f);
		float pathOffset = destinationPoint.y - this.path.GetPoint(1f).y;
		float orientation = Mathf.Sign(base.transform.position.x - destinationPoint.x);
		while (t <= 1f)
		{
			t += 0.0333333351f;
			this.setSortingOrder(75 + (int)(t * 10f));
			if (t > 0.9f)
			{
				base.animator.SetTrigger("EndFlyUp");
			}
			if (t + 0.0333333351f >= this.path.forceFGSet && t < this.path.forceFGSet)
			{
				this.setSortingLayer("Default");
			}
			base.transform.position = this.path.GetPoint(EaseUtils.EaseOutSine(0f, 1f, t)) + Vector2.up * pathOffset;
			base.transform.localScale = new Vector3(EaseUtils.EaseInCubic(0.3f, 0.8f, t) * orientation, EaseUtils.EaseInCubic(0.3f, 0.8f, t));
			yield return CupheadTime.WaitForSeconds(this, 0.0333333351f);
		}
		base.animator.SetTrigger("EndFlyUp");
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 0.8f, 0.8f);
		base.transform.position = destinationPoint;
		Vector3 vel = destinationPoint - (this.path.GetPoint(EaseUtils.EaseOutSine(0f, 1f, t - 0.0333333351f)) + Vector2.up * pathOffset);
		t = 0f;
		while (t < this.time || (this.parent && !this.parent.GrubCanEnter(base.transform.position, this.GetTimeToMove())))
		{
			base.transform.position = destinationPoint + Mathf.Sin(t * 10f) * vel * (Mathf.InverseLerp(1f, 0f, t) * 10f);
			vel = vel.magnitude * MathUtils.AngleToDirection(MathUtils.DirectionToAngle(vel) + 3f);
			yield return CupheadTime.WaitForSeconds(this, 0.0333333351f);
			t += 0.0333333351f;
		}
		base.transform.position = destinationPoint;
		Vector3 onGroundPoint = new Vector3(this.path.GetPoint(1f).x, RumRunnersLevel.GroundWalkingPosY(this.path.GetPoint(1f), null, this.yOffset, 200f));
		float timeToMove = this.GetTimeToMove();
		float flipLength = 0.625f;
		t = 0f;
		base.animator.SetTrigger("Enter");
		this.startedEntering = true;
		this.SFX_RUMRUN_Grub_VocalIntro();
		this.SFX_RUMRUN_Grub_FlyingLoop();
		while (t < timeToMove)
		{
			float moveTime = Mathf.Clamp(t / flipLength, 0f, 1f);
			float flipTime = Mathf.Clamp(t / flipLength, 0f, 1f);
			base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(destinationPoint.y, onGroundPoint.y, moveTime) + Mathf.Sin(flipTime * 3.14159274f) * 30f);
			base.transform.localScale = new Vector3(Mathf.Lerp(0.8f, 1f, moveTime) * Mathf.Sign(base.transform.localScale.x), Mathf.Lerp(0.8f, 1f, moveTime));
			this.basePos = base.transform.position;
			this.shadowTransform.position = new Vector3(base.transform.position.x, onGroundPoint.y + this.shadowDist);
			yield return CupheadTime.WaitForSeconds(this, 0.0333333351f);
			t += 0.0333333351f;
		}
		base.transform.position = new Vector3(base.transform.position.x, onGroundPoint.y);
		this.finishedEntering = true;
		yield break;
	}

	// Token: 0x0600249F RID: 9375 RVA: 0x0001EF16 File Offset: 0x0001D116
	public void die(bool playSound)
	{
		Object.Destroy(base.gameObject);
		this.deathEffect.Create(base.transform.position);
		this.SFX_RUMRUN_Grub_FlyingLoopStop();
		if (playSound)
		{
			this.SFX_RUMRUN_Grub_Lackey_DiePoof();
		}
	}

	// Token: 0x060024A0 RID: 9376 RVA: 0x0001EF4C File Offset: 0x0001D14C
	public void AniEvent_EnableCollision()
	{
		this.collider.enabled = true;
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x0001EF5A File Offset: 0x0001D15A
	public void AniEvent_StartMoving()
	{
		this.moving = true;
	}

	// Token: 0x060024A2 RID: 9378 RVA: 0x000C4714 File Offset: 0x000C2914
	public void AniEvent_OnFlip()
	{
		this.setSortingLayer("Enemies");
		this.setSortingOrder(this.spawnOrder + 1);
		base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x) * Mathf.Sign(base.transform.position.x - PlayerManager.GetNext().transform.position.x), base.transform.localScale.y);
		this.speed *= -base.transform.localScale.x;
		base.animator.SetInteger("Variant", this.variant);
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000C47E0 File Offset: 0x000C29E0
	public void animationEvent_BlinkCompleted()
	{
		base.animator.SetInteger("BlinkLoops", Random.Range((int)RumRunnersLevelGrub.BlinkLoopsRange.minimum, (int)RumRunnersLevelGrub.BlinkLoopsRange.maximum + 1));
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x0001EF63 File Offset: 0x0001D163
	public void setSortingLayer(string layerName)
	{
		this.mainRenderer.sortingLayerName = layerName;
		this.blinkRenderer.sortingLayerName = layerName;
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x0001EF7D File Offset: 0x0001D17D
	public void setSortingOrder(int order)
	{
		this.mainRenderer.sortingOrder = order;
		this.blinkRenderer.sortingOrder = order + 1;
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x0001EF99 File Offset: 0x0001D199
	public void SFX_RUMRUN_Grub_Lackey_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x0001EFB5 File Offset: 0x0001D1B5
	public void SFX_RUMRUN_Grub_VocalIntro()
	{
	}

	// Token: 0x060024A8 RID: 9384 RVA: 0x0001EFB7 File Offset: 0x0001D1B7
	public void SFX_RUMRUN_Grub_FlyingLoop()
	{
		AudioManager.Play("sfx_dlc_rumrun_grub_flying_loop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_grub_flying_loop");
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x0001EFD3 File Offset: 0x0001D1D3
	public void SFX_RUMRUN_Grub_FlyingLoopStop()
	{
		AudioManager.Stop("sfx_dlc_rumrun_grub_flying_loop");
	}

	// Token: 0x04001E3A RID: 7738
	public static readonly float[] enterEndClipLength = new float[]
	{
		10f,
		11f,
		16f
	};

	// Token: 0x04001E3B RID: 7739
	public const float flipClipLength = 15f;

	// Token: 0x04001E3C RID: 7740
	public static readonly float[] flipEndClipLength = new float[]
	{
		17f,
		12f,
		9f,
		19f
	};

	// Token: 0x04001E3D RID: 7741
	public const float START_SIZE = 0.3f;

	// Token: 0x04001E3E RID: 7742
	public const float WAIT_SIZE = 0.8f;

	// Token: 0x04001E3F RID: 7743
	public const float END_FLY_UP_TRIGGER = 0.9f;

	// Token: 0x04001E40 RID: 7744
	public const float OVERSHOOT = 10f;

	// Token: 0x04001E41 RID: 7745
	public const float FLIP_HEIGHT = 30f;

	// Token: 0x04001E42 RID: 7746
	public const float TIME_TO_FULL_X_SPEED = 0.5f;

	// Token: 0x04001E43 RID: 7747
	public static readonly Rangef BlinkLoopsRange = new Rangef(2f, 3f);

	// Token: 0x04001E44 RID: 7748
	public const float EnterYOffset = 4f;

	// Token: 0x04001E45 RID: 7749
	[SerializeField]
	public float yOffset;

	// Token: 0x04001E46 RID: 7750
	[SerializeField]
	public SpriteRenderer mainRenderer;

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	public SpriteRenderer blinkRenderer;

	// Token: 0x04001E48 RID: 7752
	[SerializeField]
	public Transform shadowTransform;

	// Token: 0x04001E49 RID: 7753
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x04001E4A RID: 7754
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x04001E4B RID: 7755
	[SerializeField]
	public float wobbleSpeed = 1f;

	// Token: 0x04001E4C RID: 7756
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x04001E52 RID: 7762
	public float time;

	// Token: 0x04001E53 RID: 7763
	public float hp;

	// Token: 0x04001E54 RID: 7764
	public DamageReceiver damageReceiver;

	// Token: 0x04001E55 RID: 7765
	public RumRunnersLevelSpider parent;

	// Token: 0x04001E56 RID: 7766
	public Collider2D collider;

	// Token: 0x04001E57 RID: 7767
	public bool finishedEntering;

	// Token: 0x04001E58 RID: 7768
	public RumRunnersLevelGrubPath path;

	// Token: 0x04001E59 RID: 7769
	public int enterVariant;

	// Token: 0x04001E5A RID: 7770
	public int variant;

	// Token: 0x04001E5B RID: 7771
	public int spawnOrder;

	// Token: 0x04001E5C RID: 7772
	public float wobbleTimer;

	// Token: 0x04001E5D RID: 7773
	public Vector3 basePos;

	// Token: 0x04001E5E RID: 7774
	public float shadowDist;

	// Token: 0x04001E5F RID: 7775
	public float horizontalSpeedEasingTime;
}
