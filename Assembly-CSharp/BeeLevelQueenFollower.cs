using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class BeeLevelQueenFollower : AbstractProjectile
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x0000EB7C File Offset: 0x0000CD7C
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000924C8 File Offset: 0x000906C8
	public BeeLevelQueenFollower Create(Vector2 pos, BeeLevelQueenFollower.Properties properties)
	{
		BeeLevelQueenFollower beeLevelQueenFollower = base.Create() as BeeLevelQueenFollower;
		beeLevelQueenFollower.transform.position = pos;
		beeLevelQueenFollower.properties = properties;
		return beeLevelQueenFollower;
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001161 RID: 4449 RVA: 0x0000EB83 File Offset: 0x0000CD83
	public override float DestroyLifetime
	{
		get
		{
			return 300f;
		}
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x000924FC File Offset: 0x000906FC
	public override void Awake()
	{
		base.Awake();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("bee_queen_follower_loop");
		this.emitAudioFromObject.Add("bee_queen_follower_loop");
		base.StartCoroutine(this.check_pos_cr());
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x00092568 File Offset: 0x00090768
	public IEnumerator check_pos_cr()
	{
		float offset = 175f;
		while (base.transform.position.y < (float)Level.Current.Ceiling + offset && base.transform.position.y > (float)Level.Current.Ground - offset && base.transform.position.x > (float)Level.Current.Left - offset && base.transform.position.x < (float)Level.Current.Right + offset)
		{
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x0000EB8A File Offset: 0x0000CD8A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		this.childPrefab = null;
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x0000EBB0 File Offset: 0x0000CDB0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.attacking && this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.health -= info.damage;
		if (this.properties.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x0000EC19 File Offset: 0x0000CE19
	public override void Start()
	{
		base.Start();
		if (this.properties.parryable)
		{
			this.SetParryable(true);
		}
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00092584 File Offset: 0x00090784
	public override void Update()
	{
		base.Update();
		if (this.aim == null || this.properties.player == null || base.dead)
		{
			return;
		}
		base.transform.position += base.transform.right * (this.properties.speed * CupheadTime.Delta);
		this.aim.LookAt2D(this.properties.player.center);
		if (this.rotate)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.properties.rotationSpeed * CupheadTime.Delta);
		}
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x0000EC45 File Offset: 0x0000CE45
	public override void Die()
	{
		base.Die();
		AudioManager.Stop("bee_queen_follower_loop");
		this.circleCollider.enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00092668 File Offset: 0x00090868
	public IEnumerator go_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.introTime);
		base.animator.SetTrigger("Continue");
		this.attacking = true;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		float t = 0f;
		while (t < 2f)
		{
			float val = t / 2f;
			this.properties.speed = Mathf.Lerp(0f, this.properties.speedMax, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.properties.speed = this.properties.speedMax;
		yield return CupheadTime.WaitForSeconds(this, this.properties.homingTime);
		this.rotate = false;
		yield break;
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00092684 File Offset: 0x00090884
	public IEnumerator children_cr()
	{
		for (;;)
		{
			this.childPrefab.Create(base.transform.position, (float)Random.Range(0, 360), 0f, this.properties.childHealth);
			yield return CupheadTime.WaitForSeconds(this, this.properties.childDelay);
		}
		yield break;
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x0000EC69 File Offset: 0x0000CE69
	public override void OnParry(AbstractPlayerController player)
	{
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x0000EC78 File Offset: 0x0000CE78
	public override void OnParryDie()
	{
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x000926A0 File Offset: 0x000908A0
	public IEnumerator timer_cr()
	{
		this.SetParryable(false);
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetParryable(true);
		yield break;
	}

	// Token: 0x04000E0A RID: 3594
	public float coolDown = 0.4f;

	// Token: 0x04000E0B RID: 3595
	[SerializeField]
	public BasicDamagableProjectile childPrefab;

	// Token: 0x04000E0C RID: 3596
	public BeeLevelQueenFollower.Properties properties;

	// Token: 0x04000E0D RID: 3597
	public Transform aim;

	// Token: 0x04000E0E RID: 3598
	public CircleCollider2D circleCollider;

	// Token: 0x04000E0F RID: 3599
	public DamageReceiver damageReceiver;

	// Token: 0x04000E10 RID: 3600
	public bool attacking;

	// Token: 0x04000E11 RID: 3601
	public bool rotate = true;

	// Token: 0x02000A79 RID: 2681
	public class Properties
	{
		// Token: 0x06005A2E RID: 23086 RVA: 0x001DD3FC File Offset: 0x001DB5FC
		public Properties(AbstractPlayerController player, float introTime, float speed, float rotationSpeed, float homingTime, float health, float childDelay, float childHealth, bool parryable)
		{
			this.player = player;
			this.introTime = introTime;
			this.speedMax = speed;
			this.rotationSpeed = rotationSpeed;
			this.homingTime = homingTime;
			this.healthMax = health;
			this.childDelay = childDelay;
			this.childHealth = childHealth;
			this.speed = 0f;
			this.health = health;
			this.parryable = parryable;
		}

		// Token: 0x04004CFA RID: 19706
		public readonly AbstractPlayerController player;

		// Token: 0x04004CFB RID: 19707
		public readonly float introTime;

		// Token: 0x04004CFC RID: 19708
		public readonly float speedMax;

		// Token: 0x04004CFD RID: 19709
		public readonly float rotationSpeed;

		// Token: 0x04004CFE RID: 19710
		public readonly float homingTime;

		// Token: 0x04004CFF RID: 19711
		public readonly float healthMax;

		// Token: 0x04004D00 RID: 19712
		public readonly float childDelay;

		// Token: 0x04004D01 RID: 19713
		public readonly float childHealth;

		// Token: 0x04004D02 RID: 19714
		public readonly bool parryable;

		// Token: 0x04004D03 RID: 19715
		public float speed;

		// Token: 0x04004D04 RID: 19716
		public float health;
	}
}
