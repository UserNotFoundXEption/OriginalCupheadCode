using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class FlyingBirdLevelSmallBird : LevelProperties.FlyingBird.Entity
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060019EC RID: 6636 RVA: 0x00016106 File Offset: 0x00014306
	// (set) Token: 0x060019ED RID: 6637 RVA: 0x0001610E File Offset: 0x0001430E
	public FlyingBirdLevelSmallBird.State state { get; set; }

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060019EE RID: 6638 RVA: 0x00016117 File Offset: 0x00014317
	// (set) Token: 0x060019EF RID: 6639 RVA: 0x0001611F File Offset: 0x0001431F
	public FlyingBirdLevelSmallBird.Direction direction { get; set; }

	// Token: 0x060019F0 RID: 6640 RVA: 0x000A76D0 File Offset: 0x000A58D0
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.sprite.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild = this.sprite.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnPlayerCollision;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(this.bulletRoot);
		this.aim.ResetLocalTransforms();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00016128 File Offset: 0x00014328
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.PositionEggs();
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x00016146 File Offset: 0x00014346
	public override void LevelInit(LevelProperties.FlyingBird properties)
	{
		base.LevelInit(properties);
		if (Level.Current.mode == Level.Mode.Easy)
		{
			properties.OnBossDeath += this.OnBossDeath;
		}
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000A7778 File Offset: 0x000A5978
	public void OnBossDeath()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.properties.OnBossDeath -= this.OnBossDeath;
		}
		this.sprite.GetComponent<Collider2D>().enabled = false;
		base.properties.OnStateChange -= this.OnBossDeath;
		this.StopAllCoroutines();
		this.sprite.transform.ResetLocalTransforms();
		base.animator.Play("Death");
		AudioManager.Play("level_flyingbird_small_bird_death_cry");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_death_cry");
		AudioManager.Stop("level_flyingbird_small_bird_rotating_eggs_loop");
		foreach (FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg in this.eggs)
		{
			flyingBirdLevelSmallBirdEgg.Explode();
		}
		if (Level.Current.mode != Level.Mode.Easy)
		{
			this.sprite.GetComponent<LevelBossDeathExploder>().StartExplosion();
			base.StartCoroutine(this.leave_cr());
		}
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00016170 File Offset: 0x00014370
	public void OnPlayerCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.state == FlyingBirdLevelSmallBird.State.Dead)
		{
			return;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00016197 File Offset: 0x00014397
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == FlyingBirdLevelSmallBird.State.Dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x000A7898 File Offset: 0x000A5A98
	public void StartPattern(Vector2 pos)
	{
		base.properties.OnStateChange += this.OnBossDeath;
		if (this.state != FlyingBirdLevelSmallBird.State.Init)
		{
			return;
		}
		this.state = FlyingBirdLevelSmallBird.State.Starting;
		base.transform.position = pos;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.float_cr());
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x000161B7 File Offset: 0x000143B7
	public void TurnComplete()
	{
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x000A7908 File Offset: 0x000A5B08
	public void PositionEggs()
	{
		if (this.eggs == null || this.eggs.Count < 1)
		{
			return;
		}
		foreach (FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg in this.eggs)
		{
			flyingBirdLevelSmallBirdEgg.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x000A798C File Offset: 0x000A5B8C
	public IEnumerator start_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.eggs_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.moveX_cr());
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.shooting_cr());
		yield break;
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000A79A8 File Offset: 0x000A5BA8
	public IEnumerator float_cr()
	{
		yield return this.sprite.TweenLocalPositionY(0f, 10f, 1f, EaseUtils.EaseType.easeOutSine);
		for (;;)
		{
			yield return this.sprite.TweenLocalPositionY(10f, -10f, 1f, EaseUtils.EaseType.easeInOutSine);
			yield return this.sprite.TweenLocalPositionY(-10f, 10f, 1f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x000A79C4 File Offset: 0x000A5BC4
	public IEnumerator leave_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SpriteRenderer renderer = this.sprite.GetComponent<SpriteRenderer>();
		float end = (float)((base.transform.position.x <= 0f) ? Level.Current.Left : Level.Current.Right);
		end += renderer.bounds.size.x / 2f * Mathf.Sign(base.transform.position.x);
		base.StartCoroutine(this.tweenX_cr(base.transform.position.x, end, base.properties.CurrentState.smallBird.leaveTime, EaseUtils.EaseType.easeInOutSine));
		this.sprite.GetComponent<Collider2D>().enabled = false;
		while (this.state != FlyingBirdLevelSmallBird.State.Dead)
		{
			yield return null;
		}
		Object.Destroy(this.sprite.GetComponent<LevelBossDeathExploder>());
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x000A79E0 File Offset: 0x000A5BE0
	public void ShootProjectile()
	{
		this.aim.LookAt2D(PlayerManager.Current.center);
		this.bulletPrefab.Create(this.bulletRoot.position, this.aim.eulerAngles.z + 180f, -base.properties.CurrentState.smallBird.shotSpeed).SetParryable(true);
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x000A7A54 File Offset: 0x000A5C54
	public IEnumerator shooting_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.smallBird.shotDelay);
			AbstractPlayerController target = PlayerManager.GetNext();
			if (this.direction == FlyingBirdLevelSmallBird.Direction.Left)
			{
				if (target.center.x > base.transform.position.x)
				{
					yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Right);
				}
			}
			else if (target.center.x < base.transform.position.x)
			{
				yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Left);
			}
			FlyingBirdLevelSmallBird.State lastState = this.state;
			this.state = FlyingBirdLevelSmallBird.State.Shooting;
			base.animator.SetTrigger("Shoot");
			base.animator.WaitForAnimationToEnd(this, "Shoot", false, true);
			this.state = lastState;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000161B9 File Offset: 0x000143B9
	public Coroutine Turn(FlyingBirdLevelSmallBird.Direction d)
	{
		return base.StartCoroutine(this.turn_cr(d));
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000A7A70 File Offset: 0x000A5C70
	public IEnumerator turn_cr(FlyingBirdLevelSmallBird.Direction d)
	{
		if (this.direction != d)
		{
			this.sprite.transform.SetScale(new float?((float)d), null, null);
			this.direction = d;
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		yield break;
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x000A7A94 File Offset: 0x000A5C94
	public IEnumerator eggs_cr()
	{
		int count = base.properties.CurrentState.smallBird.eggCount;
		this.eggs = new List<FlyingBirdLevelSmallBirdEgg>();
		this.eggContainer = new GameObject("Eggs").transform;
		this.eggContainer.SetParent(base.transform);
		this.eggContainer.ResetLocalTransforms();
		this.eggContainer.SetLocalPosition(null, new float?(-65f), null);
		for (int i = 0; i < count; i++)
		{
			float value = (float)i / (float)count * 360f;
			FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg = this.eggPrefab.InstantiatePrefab<FlyingBirdLevelSmallBirdEgg>();
			flyingBirdLevelSmallBirdEgg.SetParent(this.eggContainer, base.properties);
			flyingBirdLevelSmallBirdEgg.container.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
			this.eggs.Add(flyingBirdLevelSmallBirdEgg);
		}
		AudioManager.PlayLoop("level_flyingbird_small_bird_rotating_eggs_loop");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_rotating_eggs_loop");
		for (;;)
		{
			this.eggContainer.AddLocalEulerAngles(0f, 0f, base.properties.CurrentState.smallBird.eggRotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x000A7AB0 File Offset: 0x000A5CB0
	public IEnumerator moveX_cr()
	{
		this.direction = FlyingBirdLevelSmallBird.Direction.Left;
		for (;;)
		{
			float minX = base.properties.CurrentState.smallBird.minX;
			this.state = FlyingBirdLevelSmallBird.State.Left;
			yield return base.StartCoroutine(this.tweenX_cr(base.transform.position.x, minX, base.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine));
			yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Right);
			this.state = FlyingBirdLevelSmallBird.State.Right;
			yield return base.StartCoroutine(this.tweenX_cr(minX, 520f, base.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine));
			yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Left);
		}
		yield break;
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x000A7ACC File Offset: 0x000A5CCC
	public IEnumerator tweenX_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = FlyingBirdLevelSmallBird.State.Dead;
		base.transform.SetPosition(new float?(end), null, null);
		yield return null;
		yield break;
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x000A7B04 File Offset: 0x000A5D04
	public IEnumerator moveY_cr()
	{
		if (Rand.Bool())
		{
			yield return base.StartCoroutine(this.tweenY_cr(base.transform.position.y, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		else
		{
			float currentDist = -230f - base.transform.position.y;
			float normalDist = 490f;
			float time = base.properties.CurrentState.smallBird.timeY - currentDist / normalDist;
			yield return base.StartCoroutine(this.tweenY_cr(base.transform.position.y, -230f, time, EaseUtils.EaseType.easeInOutSine));
			yield return base.StartCoroutine(this.tweenY_cr(-230f, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.tweenY_cr(260f, -230f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
			yield return base.StartCoroutine(this.tweenY_cr(-230f, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		yield break;
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x000A7B20 File Offset: 0x000A5D20
	public IEnumerator tweenY_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield return null;
		yield break;
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x000161C8 File Offset: 0x000143C8
	public void SmallLaserShootSFX()
	{
		AudioManager.Play("level_flyingbird_small_bird_shoot");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_shoot");
	}

	// Token: 0x040014DA RID: 5338
	[SerializeField]
	public FlyingBirdLevelSmallBirdSprite sprite;

	// Token: 0x040014DB RID: 5339
	public CollisionChild collisionChild;

	// Token: 0x040014DC RID: 5340
	public DamageReceiver damageReceiver;

	// Token: 0x040014DD RID: 5341
	public DamageDealer damageDealer;

	// Token: 0x040014DE RID: 5342
	[Space(10f)]
	[SerializeField]
	public FlyingBirdLevelSmallBirdEgg eggPrefab;

	// Token: 0x040014DF RID: 5343
	[Space(10f)]
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x040014E0 RID: 5344
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x040014E3 RID: 5347
	public Transform aim;

	// Token: 0x040014E4 RID: 5348
	public Transform eggContainer;

	// Token: 0x040014E5 RID: 5349
	public List<FlyingBirdLevelSmallBirdEgg> eggs;

	// Token: 0x02000C59 RID: 3161
	public enum State
	{
		// Token: 0x04005968 RID: 22888
		Init,
		// Token: 0x04005969 RID: 22889
		Starting,
		// Token: 0x0400596A RID: 22890
		Right,
		// Token: 0x0400596B RID: 22891
		Left,
		// Token: 0x0400596C RID: 22892
		Shooting,
		// Token: 0x0400596D RID: 22893
		Dead
	}

	// Token: 0x02000C5A RID: 3162
	public enum Direction
	{
		// Token: 0x0400596F RID: 22895
		Right = -1,
		// Token: 0x04005970 RID: 22896
		Left = 1
	}
}
