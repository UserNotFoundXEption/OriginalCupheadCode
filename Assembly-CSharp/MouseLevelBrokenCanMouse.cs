using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class MouseLevelBrokenCanMouse : LevelProperties.Mouse.Entity
{
	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06001F4D RID: 8013 RVA: 0x0001A5ED File Offset: 0x000187ED
	// (set) Token: 0x06001F4E RID: 8014 RVA: 0x0001A5F5 File Offset: 0x000187F5
	public MouseLevelBrokenCanMouse.State state { get; set; }

	// Token: 0x06001F4F RID: 8015 RVA: 0x000B5BB4 File Offset: 0x000B3DB4
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		this.platformLocalPos = this.platform.localPosition;
		this.platform.transform.parent = null;
		this.setFlameCollidersEnabled(false);
		this.colliders = this.mouse.GetComponents<Collider2D>();
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this.colliders[i].enabled = false;
		}
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000B5C5C File Offset: 0x000B3E5C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.platform.position = new Vector2(base.transform.position.x, base.transform.position.y) + this.platformLocalPos;
		if (this.peeking && base.properties.CurrentHealth < base.properties.TotalHealth * this.catPeeking.Peek2Threshold)
		{
			this.catPeeking.StopPeeking();
			this.peeking = false;
		}
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x0001A5FE File Offset: 0x000187FE
	public void LateUpdate()
	{
		this.leftFlame.UpdateParentTransform(base.transform);
		this.rightFlame.UpdateParentTransform(base.transform);
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x0001A622 File Offset: 0x00018822
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001F53 RID: 8019 RVA: 0x0001A64B File Offset: 0x0001884B
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x0001A666 File Offset: 0x00018866
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x0001A679 File Offset: 0x00018879
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x0001A69A File Offset: 0x0001889A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x000B5D0C File Offset: 0x000B3F0C
	public void StartPattern(Transform transform)
	{
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this.colliders[i].enabled = true;
		}
		base.transform.position = transform.position;
		base.transform.localScale = transform.localScale;
		base.animator.SetTrigger("Continue");
		if (this.state != MouseLevelBrokenCanMouse.State.Dying)
		{
			this.state = MouseLevelBrokenCanMouse.State.Down;
			base.StartCoroutine("main_cr");
		}
		base.StartCoroutine(this.move_cr());
		this.direction = ((transform.localScale.x <= 0f) ? MouseLevelBrokenCanMouse.Direction.Right : MouseLevelBrokenCanMouse.Direction.Left);
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x000B5DC4 File Offset: 0x000B3FC4
	public IEnumerator main_cr()
	{
		LevelProperties.Mouse.State patternState = base.properties.CurrentState;
		string[] pattern = patternState.brokenCanFlame.attackString.RandomChoice<string>().Split(new char[]
		{
			','
		});
		while (!this.dead)
		{
			if (patternState != base.properties.CurrentState)
			{
				if (!patternState.brokenCanFlame.attackString.SequenceEqual(base.properties.CurrentState.brokenCanFlame.attackString))
				{
					pattern = base.properties.CurrentState.brokenCanFlame.attackString.RandomChoice<string>().Split(new char[]
					{
						','
					});
				}
				patternState = base.properties.CurrentState;
			}
			LevelProperties.Mouse.BrokenCanFlame p = base.properties.CurrentState.brokenCanFlame;
			foreach (string instruction in pattern)
			{
				this.state = ((this.state != MouseLevelBrokenCanMouse.State.Down) ? MouseLevelBrokenCanMouse.State.Down : MouseLevelBrokenCanMouse.State.Up);
				base.animator.SetTrigger((this.state != MouseLevelBrokenCanMouse.State.Down) ? "Up" : "Down");
				yield return base.animator.WaitForAnimationToEnd(this, (this.state != MouseLevelBrokenCanMouse.State.Down) ? "Going_Up" : "Going_Down", 1, false, true);
				base.animator.SetTrigger("Continue");
				yield return CupheadTime.WaitForSeconds(this, p.delayBeforeShot);
				if (instruction == "BF")
				{
					base.animator.SetTrigger("Shoot");
					BasicProjectile leftBullet = this.bulletPrefab.Create(this.leftBulletRoot.position, (float)((base.transform.localScale.x <= 0f) ? 0 : 180), p.shotSpeed);
					leftBullet.SetParryable(true);
					BasicProjectile rightBullet = this.bulletPrefab.Create(this.rightBulletRoot.position, (float)((base.transform.localScale.x <= 0f) ? 180 : 0), p.shotSpeed);
					rightBullet.SetParryable(true);
					yield return base.animator.WaitForAnimationToEnd(this, "Fire", 2, false, true);
					yield return CupheadTime.WaitForSeconds(this, p.delayAfterShot);
				}
				base.StartCoroutine(this.scale_flames_cr(true));
				base.animator.SetTrigger("Flame");
				yield return CupheadTime.WaitForSeconds(this, p.chargeTime);
				base.animator.SetTrigger("FlameContinue");
				this.setFlameCollidersEnabled(true);
				this.flameOn = true;
				yield return CupheadTime.WaitForSeconds(this, p.loopTime);
				this.setFlameCollidersEnabled(false);
				base.StartCoroutine(this.scale_flames_cr(false));
				this.flameOn = false;
				base.animator.SetTrigger("FlameContinue");
				yield return base.animator.WaitForAnimationToEnd(this, "End", 4, false, true);
			}
		}
		yield break;
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x000B5DE0 File Offset: 0x000B3FE0
	public IEnumerator scale_flames_cr(bool turningOn)
	{
		float t = 0f;
		float time = 1f;
		if (turningOn)
		{
			this.leftFlame.transform.SetScale(new float?(1f), new float?(1f), new float?(0f));
			this.rightFlame.transform.SetScale(new float?(1f), new float?(1f), new float?(0f));
			this.leftFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x), new float?(this.finalFlameScale.y), new float?(0f));
			this.rightFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x), new float?(this.finalFlameScale.y), new float?(0f));
			this.leftFlameSprite.enabled = true;
			this.rightFlameSprite.enabled = true;
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			if (turningOn)
			{
				this.leftFlame.transform.SetScale(new float?(-(t / time)), new float?(t / time), new float?(0f));
				this.rightFlame.transform.SetScale(new float?(t / time), new float?(t / time), new float?(0f));
				this.leftFlameSprite.transform.SetScale(new float?(t / time * this.finalFlameScale.x), new float?(t / time * this.finalFlameScale.y), new float?(0f));
				this.rightFlameSprite.transform.SetScale(new float?(t / time * this.finalFlameScale.x), new float?(t / time * this.finalFlameScale.y), new float?(0f));
			}
			else
			{
				this.leftFlame.transform.SetScale(new float?(-1f + t / time), new float?(1f - t / time), new float?(0f));
				this.rightFlame.transform.SetScale(new float?(1f - t / time), new float?(1f - t / time), new float?(0f));
				this.leftFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x - t / time * this.finalFlameScale.x), new float?(this.finalFlameScale.y - t / time * this.finalFlameScale.y), new float?(0f));
				this.rightFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x - t / time * this.finalFlameScale.x), new float?(this.finalFlameScale.y - t / time * this.finalFlameScale.y), new float?(0f));
			}
			yield return null;
		}
		if (!turningOn)
		{
			this.leftFlame.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.rightFlame.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.leftFlameSprite.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.rightFlameSprite.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.leftFlameSprite.enabled = false;
			this.rightFlameSprite.enabled = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x0001A6A9 File Offset: 0x000188A9
	public void setFlameCollidersEnabled(bool enabled)
	{
		this.leftFlame.SetColliderEnabled(enabled);
		this.rightFlame.SetColliderEnabled(enabled);
		if (enabled)
		{
			this.SoundMouseFlameThrower();
		}
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x000B5E04 File Offset: 0x000B4004
	public IEnumerator moveToX_cr(float x)
	{
		this.overrideMove = true;
		this.overrideMoveX = x;
		while (this.overrideMove)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000B5E28 File Offset: 0x000B4028
	public IEnumerator move_cr()
	{
		Vector2 startPos = base.transform.position;
		bool overridden;
		do
		{
			LevelProperties.Mouse.BrokenCanMove p = base.properties.CurrentState.brokenCanMove;
			Vector2 targetPos = startPos;
			overridden = false;
			if (this.overrideMove)
			{
				targetPos.x = this.overrideMoveX;
				overridden = true;
			}
			else if (this.direction == MouseLevelBrokenCanMouse.Direction.Left)
			{
				targetPos.x -= p.maxXPositionRange.RandomFloat();
			}
			else
			{
				targetPos.x += p.maxXPositionRange.RandomFloat();
			}
			float time = Mathf.Abs(targetPos.x - base.transform.position.x) / p.speed;
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, targetPos, EaseUtils.EaseType.easeInOutSine, time));
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			this.direction = ((this.direction != MouseLevelBrokenCanMouse.Direction.Left) ? MouseLevelBrokenCanMouse.Direction.Left : MouseLevelBrokenCanMouse.Direction.Right);
		}
		while (!this.overrideMove || !overridden);
		this.overrideMove = false;
		base.animator.Play("Idle", 0);
		yield break;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000B5E44 File Offset: 0x000B4044
	public IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta * this.hitPauseCoefficient();
			float wheelAnimProgress = -base.transform.localScale.x * base.transform.position.x / 100f;
			wheelAnimProgress %= 1f;
			if (wheelAnimProgress < 0f)
			{
				wheelAnimProgress += 1f;
			}
			base.animator.Play("Move", 0, wheelAnimProgress);
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000B5E84 File Offset: 0x000B4084
	public void OnBossDeath()
	{
		foreach (Collider2D collider2D in this.mouse.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.StopAllCoroutines();
		this.state = MouseLevelBrokenCanMouse.State.Dying;
		base.StartCoroutine(this.death_cr(false));
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x0001A6CF File Offset: 0x000188CF
	public void Transform()
	{
		this.state = MouseLevelBrokenCanMouse.State.Dying;
		this.SoundMouseScreamVoice();
		base.StartCoroutine(this.death_cr(true));
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x0001A703 File Offset: 0x00018903
	public void BeEaten()
	{
		Object.Destroy(this.leftFlame.gameObject);
		Object.Destroy(this.rightFlame.gameObject);
		Object.Destroy(this.platform.gameObject);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x000B5ED8 File Offset: 0x000B40D8
	public IEnumerator death_cr(bool transform)
	{
		base.animator.SetTrigger("Die");
		this.sawBlades.Leave();
		if (transform)
		{
			base.animator.SetTrigger("Down");
		}
		else
		{
			base.animator.SetBool("CrazyScissor", true);
		}
		if (this.flameOn)
		{
			base.animator.SetTrigger("FlameContinue");
		}
		else
		{
			base.animator.ResetTrigger("Flame");
			AudioManager.Play("level_mouse_scream_death_voice");
		}
		this.leftFlame.SetColliderEnabled(false);
		this.rightFlame.SetColliderEnabled(false);
		base.StopCoroutine("main_cr");
		if (transform)
		{
			yield return base.StartCoroutine(this.moveToX_cr(0f));
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Down", 1, false);
			this.cat.StartIntro();
		}
		yield break;
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x0001A740 File Offset: 0x00018940
	public void SoundMouseBrkCanScissorUp()
	{
		AudioManager.Play("level_mouse_broken_can_scissor_going_up");
		this.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_up");
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x0001A75C File Offset: 0x0001895C
	public void SoundMouseBrkCanScissorDown()
	{
		AudioManager.Play("level_mouse_broken_can_scissor_going_down");
		this.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_down");
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x0001A778 File Offset: 0x00018978
	public void SoundMouseBrkCanStartUp()
	{
		AudioManager.Play("level_mouse_broken_can_start_up");
		this.emitAudioFromObject.Add("level_mouse_broken_can_start_up");
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x0001A794 File Offset: 0x00018994
	public void SoundMouseFlameThrower()
	{
		AudioManager.Play("level_mouse_flamethrower");
		this.emitAudioFromObject.Add("level_mouse_flamethrower");
	}

	// Token: 0x06001F66 RID: 8038 RVA: 0x0001A7B0 File Offset: 0x000189B0
	public void SoundMouseSnarkyVoice()
	{
		AudioManager.Play("level_mouse_snarky_voice");
		this.emitAudioFromObject.Add("level_mouse_snarky_voice");
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x0001A7CC File Offset: 0x000189CC
	public void SoundMouseScreamVoice()
	{
		AudioManager.Play("level_mouse_scream_voice");
		this.emitAudioFromObject.Add("level_mouse_scream_voice");
	}

	// Token: 0x04001983 RID: 6531
	public const int CART_LAYER = 0;

	// Token: 0x04001984 RID: 6532
	public const int SCISSOR_LAYER = 1;

	// Token: 0x04001985 RID: 6533
	public const int CANNON_LAYER = 2;

	// Token: 0x04001986 RID: 6534
	public const int MOUSE_LAYER = 3;

	// Token: 0x04001987 RID: 6535
	public const int FLAME_LAYER = 4;

	// Token: 0x04001988 RID: 6536
	public const int CAN_LAYER = 5;

	// Token: 0x04001989 RID: 6537
	[SerializeField]
	public MouseLevelFlame leftFlame;

	// Token: 0x0400198A RID: 6538
	[SerializeField]
	public MouseLevelFlame rightFlame;

	// Token: 0x0400198B RID: 6539
	[SerializeField]
	public SpriteRenderer leftFlameSprite;

	// Token: 0x0400198C RID: 6540
	[SerializeField]
	public SpriteRenderer rightFlameSprite;

	// Token: 0x0400198D RID: 6541
	[SerializeField]
	public Vector2 finalFlameScale;

	// Token: 0x0400198E RID: 6542
	[SerializeField]
	public Transform leftBulletRoot;

	// Token: 0x0400198F RID: 6543
	[SerializeField]
	public Transform rightBulletRoot;

	// Token: 0x04001990 RID: 6544
	[SerializeField]
	public Transform mouse;

	// Token: 0x04001991 RID: 6545
	[SerializeField]
	public Transform platform;

	// Token: 0x04001992 RID: 6546
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x04001993 RID: 6547
	[SerializeField]
	public MouseLevelSawBladeManager sawBlades;

	// Token: 0x04001994 RID: 6548
	[SerializeField]
	public MouseLevelCat cat;

	// Token: 0x04001995 RID: 6549
	[SerializeField]
	public MouseLevelCatPeeking catPeeking;

	// Token: 0x04001997 RID: 6551
	public MouseLevelBrokenCanMouse.Direction direction;

	// Token: 0x04001998 RID: 6552
	public DamageReceiver damageReceiver;

	// Token: 0x04001999 RID: 6553
	public DamageDealer damageDealer;

	// Token: 0x0400199A RID: 6554
	public bool flameOn;

	// Token: 0x0400199B RID: 6555
	public Vector2 platformLocalPos;

	// Token: 0x0400199C RID: 6556
	public bool dead;

	// Token: 0x0400199D RID: 6557
	public bool peeking = true;

	// Token: 0x0400199E RID: 6558
	public Collider2D[] colliders;

	// Token: 0x0400199F RID: 6559
	public bool overrideMove;

	// Token: 0x040019A0 RID: 6560
	public float overrideMoveX;

	// Token: 0x040019A1 RID: 6561
	public const float WHEEL_MOVE_FACTOR = 100f;

	// Token: 0x02000DA1 RID: 3489
	public enum State
	{
		// Token: 0x04006291 RID: 25233
		Init,
		// Token: 0x04006292 RID: 25234
		Down,
		// Token: 0x04006293 RID: 25235
		Up,
		// Token: 0x04006294 RID: 25236
		Dying
	}

	// Token: 0x02000DA2 RID: 3490
	public enum Direction
	{
		// Token: 0x04006296 RID: 25238
		Left,
		// Token: 0x04006297 RID: 25239
		Right
	}
}
