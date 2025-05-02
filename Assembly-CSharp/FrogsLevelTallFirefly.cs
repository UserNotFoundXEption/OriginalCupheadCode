using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public class FrogsLevelTallFirefly : AbstractProjectile
{
	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06001E9A RID: 7834 RVA: 0x00019E21 File Offset: 0x00018021
	public override float DestroyLifetime
	{
		get
		{
			return 1E+07f;
		}
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000B3064 File Offset: 0x000B1264
	public FrogsLevelTallFirefly Create(Vector2 pos, Vector2 target, float speed, int hp, float followDelay, float followTime, float followDistance, float invincibleDuration, AbstractPlayerController player, int layer)
	{
		FrogsLevelTallFirefly frogsLevelTallFirefly = this.Create(pos) as FrogsLevelTallFirefly;
		frogsLevelTallFirefly.Health = hp;
		frogsLevelTallFirefly.Speed = speed;
		frogsLevelTallFirefly.DamagesType.OnlyPlayer();
		frogsLevelTallFirefly.CollisionDeath.OnlyPlayer();
		frogsLevelTallFirefly.CollisionDeath.PlayerProjectiles = true;
		frogsLevelTallFirefly.Init(pos, target, followDelay, followTime, followDistance, player, layer, invincibleDuration);
		frogsLevelTallFirefly.DestroyDistance = 1E+07f;
		return frogsLevelTallFirefly;
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00019E28 File Offset: 0x00018028
	public override void Awake()
	{
		base.Awake();
		if (Level.Current == null || !Level.Current.Started)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000B30D0 File Offset: 0x000B12D0
	public void Init(Vector2 pos, Vector2 target, float delay, float followTime, float followDistance, AbstractPlayerController player, int layer, float invincibleDuration)
	{
		base.transform.position = pos;
		this.target = target;
		this.followDelay = delay;
		this.followTime = followTime;
		this.followDistance = followDistance;
		this.currentHp = (float)this.Health;
		this.player = player;
		this.sprite.sortingOrder = layer;
		this.invincibleDuration = invincibleDuration;
		base.GetComponent<CircleCollider2D>().enabled = false;
		base.StartCoroutine(this.firefly_cr());
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000B3150 File Offset: 0x000B1350
	public override void Start()
	{
		base.Start();
		this.damageDealer.SetDamageFlags(true, false, false);
		this.damageDealer.SetDamage(1f);
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Enemy);
		this.damageDealer.SetRate(0.3f);
		DamageReceiver component = base.GetComponent<DamageReceiver>();
		component.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x00019E5A File Offset: 0x0001805A
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHp -= info.damage;
		this.Die();
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x00019E75 File Offset: 0x00018075
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.target, 20f);
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000B31B8 File Offset: 0x000B13B8
	public override void Die()
	{
		if (this.currentHp > 0f)
		{
			return;
		}
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_frogs_tall_firefly_death");
		this.emitAudioFromObject.Add("level_frogs_tall_firefly_death");
		base.Die();
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x00019E92 File Offset: 0x00018092
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.currentHp = 0f;
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x00019EB9 File Offset: 0x000180B9
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.currentHp = 0f;
		this.Die();
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x00019ECC File Offset: 0x000180CC
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		this.currentHp = 0f;
		this.Die();
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000B322C File Offset: 0x000B142C
	public void SetMovementPose()
	{
		Vector2 vector = this.target - this.aim.transform.position;
		if (vector.x > 0f)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
		if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.y))
		{
			if (vector.y < 0f)
			{
				base.animator.SetTrigger("OnMoveDown");
			}
			else
			{
				base.animator.SetTrigger("OnMoveForward");
			}
		}
		else
		{
			base.animator.SetTrigger("OnMoveForward");
		}
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000B3328 File Offset: 0x000B1528
	public IEnumerator firefly_cr()
	{
		yield return base.StartCoroutine(this.initialMove_cr());
		for (;;)
		{
			yield return base.StartCoroutine(this.follow_cr());
		}
		yield break;
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000B3344 File Offset: 0x000B1544
	public IEnumerator initialMove_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.SetScale(new float?(1f), null, null);
		float falloffDistance = 200f;
		int t = 0;
		int falloffFrames = (int)(falloffDistance * 2f / (this.Speed / 60f));
		Vector3 direction = this.target - this.aim.transform.position;
		direction.Normalize();
		float speed = this.Speed;
		this.SetMovementPose();
		while (Vector2.Distance(base.transform.position, this.target) > falloffDistance)
		{
			base.transform.position += direction * speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (t < falloffFrames)
		{
			if (PauseManager.state != PauseManager.State.Paused)
			{
				float value = (float)t / (float)falloffFrames;
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.Speed, 0f, value);
				base.transform.position += direction * speed * CupheadTime.FixedDelta;
				t++;
			}
			yield return wait;
		}
		base.animator.SetTrigger("OnIdle");
		yield break;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000B3360 File Offset: 0x000B1560
	public IEnumerator follow_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.animator.SetTrigger("OnIdle");
		yield return CupheadTime.WaitForSeconds(this, this.followDelay);
		Vector2 start = base.transform.position;
		this.aim.LookAt2D(this.player.center);
		this.target = base.transform.position + this.aim.right * this.followDistance;
		this.SetMovementPose();
		float t = 0f;
		while (t < this.followTime)
		{
			float val = t / this.followTime;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.x, this.target.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.y, this.target.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		base.transform.SetPosition(new float?(this.target.x), new float?(this.target.y), null);
		base.animator.SetTrigger("OnIdle");
		yield break;
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000B337C File Offset: 0x000B157C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.invincibleDuration > 0f)
		{
			this.invincibleDuration -= CupheadTime.FixedDelta;
			if (this.invincibleDuration <= 0f)
			{
				base.GetComponent<CircleCollider2D>().enabled = true;
			}
		}
	}

	// Token: 0x040018F0 RID: 6384
	[SerializeField]
	public Transform aim;

	// Token: 0x040018F1 RID: 6385
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x040018F2 RID: 6386
	public int Health;

	// Token: 0x040018F3 RID: 6387
	public float Speed;

	// Token: 0x040018F4 RID: 6388
	public float followDelay;

	// Token: 0x040018F5 RID: 6389
	public float followTime;

	// Token: 0x040018F6 RID: 6390
	public float followDistance;

	// Token: 0x040018F7 RID: 6391
	public Vector2 target;

	// Token: 0x040018F8 RID: 6392
	public float currentHp;

	// Token: 0x040018F9 RID: 6393
	public AbstractPlayerController player;

	// Token: 0x040018FA RID: 6394
	public float invincibleDuration;
}
