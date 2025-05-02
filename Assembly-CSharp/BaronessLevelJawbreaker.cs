using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class BaronessLevelJawbreaker : BaronessLevelMiniBossBase
{
	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x0000DA1F File Offset: 0x0000BC1F
	// (set) Token: 0x06001019 RID: 4121 RVA: 0x0000DA27 File Offset: 0x0000BC27
	public BaronessLevelJawbreaker.State state { get; set; }

	// Token: 0x0600101A RID: 4122 RVA: 0x0008F1C4 File Offset: 0x0008D3C4
	public override void Awake()
	{
		base.Awake();
		this.isDying = false;
		this.state = BaronessLevelJawbreaker.State.Spawned;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0008F214 File Offset: 0x0008D414
	public override void Start()
	{
		base.Start();
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.check_rotation_cr());
		base.StartCoroutine(this.switch_cr());
		base.StartCoroutine(this.reset_sprite_cr());
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0008F284 File Offset: 0x0008D484
	public IEnumerator switch_cr()
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 251;
		yield break;
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0000DA30 File Offset: 0x0000BC30
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0008F2A0 File Offset: 0x0008D4A0
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		if (this.aim == null || this.player == null || this.state == BaronessLevelJawbreaker.State.Unspawned)
		{
			return;
		}
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0008F320 File Offset: 0x0008D520
	public void FixedUpdate()
	{
		if (this.state == BaronessLevelJawbreaker.State.Spawned)
		{
			base.transform.position -= base.transform.right * this.properties.jawbreakerHomingSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			this.aim.LookAt2D(2f * base.transform.position - this.player.center);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.rotationSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		}
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0008F3E8 File Offset: 0x0008D5E8
	public IEnumerator check_rotation_cr()
	{
		for (;;)
		{
			if (((this.player.transform.position.x < base.transform.position.x && !this.lookingLeft) || (this.player.transform.position.x > base.transform.position.x && this.lookingLeft)) && !this.isTurning)
			{
				this.isTurning = true;
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
				this.lookingLeft = !this.lookingLeft;
				this.isTurning = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0008F404 File Offset: 0x0008D604
	public void Turn()
	{
		this.sprite.transform.SetScale(new float?(-this.sprite.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0008F454 File Offset: 0x0008D654
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state == BaronessLevelJawbreaker.State.Spawned)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			base.StartCoroutine(this.stopminis_cr());
			this.StartDeath();
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0008F4E0 File Offset: 0x0008D6E0
	public void Init(LevelProperties.Baroness.Jawbreaker properties, AbstractPlayerController player, Vector2 pos, float rotationSpeed, float health)
	{
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		this.properties = properties;
		this.player = player;
		this.rotationSpeed = rotationSpeed;
		this.health = health;
		base.transform.position = pos;
		this.spawnPos = base.transform.position;
		base.StartCoroutine(this.pickplayer_cr());
		this.minisRoutine = base.StartCoroutine(this.minis_cr());
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0008F580 File Offset: 0x0008D780
	public IEnumerator pickplayer_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.jawbreakerHomeDuration);
			this.player = PlayerManager.GetNext();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0000DA4E File Offset: 0x0000BC4E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.miniBluePrefab = null;
		this.miniRedPrefab = null;
		this.ghostPrefab = null;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0008F59C File Offset: 0x0008D79C
	public IEnumerator minis_cr()
	{
		this.targetPos = this.followPoint;
		Transform targetPos2 = this.targetPos;
		this.prefabsList = new List<BaronessLevelJawbreakerMini>();
		float spawnTime = this.properties.jawbreakerMiniSpace / this.properties.jawbreakerHomingSpeed;
		for (int i = 0; i < this.properties.jawbreakerMinis; i++)
		{
			if (i % 2 == 0)
			{
				yield return CupheadTime.WaitForSeconds(this, spawnTime);
				BaronessLevelJawbreakerMini blueminijawbreakers = Object.Instantiate<BaronessLevelJawbreakerMini>(this.miniBluePrefab);
				blueminijawbreakers.Init(this.properties, this.spawnPos, this.targetPos, this.rotationSpeed);
				targetPos2 = blueminijawbreakers.transform;
				this.prefabsList.Add(blueminijawbreakers);
			}
			else if (i % 2 == 1)
			{
				yield return CupheadTime.WaitForSeconds(this, spawnTime);
				BaronessLevelJawbreakerMini redminijawbreakers = Object.Instantiate<BaronessLevelJawbreakerMini>(this.miniRedPrefab);
				redminijawbreakers.Init(this.properties, this.spawnPos, targetPos2, this.rotationSpeed);
				this.targetPos = redminijawbreakers.transform;
				this.prefabsList.Add(redminijawbreakers);
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0008F5B8 File Offset: 0x0008D7B8
	public IEnumerator stopminis_cr()
	{
		base.StopCoroutine(this.minisRoutine);
		for (int i = 0; i < this.prefabsList.Count; i++)
		{
			this.prefabsList[i].Stop();
			yield return null;
		}
		base.StartCoroutine(this.killminis_cr());
		yield break;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0008F5D4 File Offset: 0x0008D7D4
	public IEnumerator killminis_cr()
	{
		this.prefabsList.Reverse();
		for (int i = 0; i < this.prefabsList.Count; i++)
		{
			this.prefabsList[i].StartDying();
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
		}
		yield break;
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0000DA6B File Offset: 0x0000BC6B
	public void StartDeath()
	{
		this.state = BaronessLevelJawbreaker.State.Explode;
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0008F5F0 File Offset: 0x0008D7F0
	public IEnumerator dying_cr()
	{
		this.StartExplosions();
		this.isDying = true;
		base.transform.rotation = Quaternion.identity;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Death", false, true);
		BaronessLevelJawbreakerGhost ghost = Object.Instantiate<BaronessLevelJawbreakerGhost>(this.ghostPrefab);
		ghost.transform.position = base.transform.position;
		this.Die();
		yield break;
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0008F60C File Offset: 0x0008D80C
	public IEnumerator reset_sprite_cr()
	{
		for (;;)
		{
			this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0000DA81 File Offset: 0x0000BC81
	public void SoundJawbreakerMouth()
	{
		AudioManager.Play("level_baroness_large_jawbreaker_mouth");
		this.emitAudioFromObject.Add("level_baroness_large_jawbreaker_mouth");
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0000DA9D File Offset: 0x0000BC9D
	public void SoundJawbreakerDeath()
	{
		AudioManager.Stop("level_baroness_large_jawbreaker_mouth");
		AudioManager.Play("level_baroness_large_jawbreaker_death");
	}

	// Token: 0x04000D12 RID: 3346
	public const float ROTATE_FRAME_TIME = 0.0833333358f;

	// Token: 0x04000D14 RID: 3348
	[SerializeField]
	public Transform sprite;

	// Token: 0x04000D15 RID: 3349
	[SerializeField]
	public BaronessLevelJawbreakerMini miniBluePrefab;

	// Token: 0x04000D16 RID: 3350
	[SerializeField]
	public BaronessLevelJawbreakerMini miniRedPrefab;

	// Token: 0x04000D17 RID: 3351
	[SerializeField]
	public Transform followPoint;

	// Token: 0x04000D18 RID: 3352
	[SerializeField]
	public BaronessLevelJawbreakerGhost ghostPrefab;

	// Token: 0x04000D19 RID: 3353
	public List<BaronessLevelJawbreakerMini> prefabsList;

	// Token: 0x04000D1A RID: 3354
	public LevelProperties.Baroness.Jawbreaker properties;

	// Token: 0x04000D1B RID: 3355
	public AbstractPlayerController player;

	// Token: 0x04000D1C RID: 3356
	public DamageDealer damageDealer;

	// Token: 0x04000D1D RID: 3357
	public DamageReceiver damageReceiver;

	// Token: 0x04000D1E RID: 3358
	public float health;

	// Token: 0x04000D1F RID: 3359
	public float rotationSpeed;

	// Token: 0x04000D20 RID: 3360
	public bool lookingLeft = true;

	// Token: 0x04000D21 RID: 3361
	public bool isTurning;

	// Token: 0x04000D22 RID: 3362
	public Transform aim;

	// Token: 0x04000D23 RID: 3363
	public Transform targetPos;

	// Token: 0x04000D24 RID: 3364
	public Vector3 spawnPos;

	// Token: 0x04000D25 RID: 3365
	public Vector3 deathPosition;

	// Token: 0x04000D26 RID: 3366
	public Coroutine minisRoutine;

	// Token: 0x02000A23 RID: 2595
	public enum State
	{
		// Token: 0x04004AF9 RID: 19193
		Unspawned,
		// Token: 0x04004AFA RID: 19194
		Spawned,
		// Token: 0x04004AFB RID: 19195
		Explode,
		// Token: 0x04004AFC RID: 19196
		Ghost
	}
}
