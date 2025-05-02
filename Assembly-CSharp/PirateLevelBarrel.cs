using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class PirateLevelBarrel : LevelProperties.Pirate.Entity
{
	// Token: 0x06002161 RID: 8545 RVA: 0x000BA674 File Offset: 0x000B8874
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		Level.Current.OnStateChangedEvent += this.OnStateChanged;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		this.damageDealer = new DamageDealer(base.properties.CurrentState.barrel.damage, 1f);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
		this.state = PirateLevelBarrel.State.Move;
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x0001C7AB File Offset: 0x0001A9AB
	public void OnLevelStart()
	{
		this.moveCoroutine = this.move_cr();
		base.StartCoroutine(this.moveCoroutine);
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000BA6F4 File Offset: 0x000B88F4
	public void OnStateChanged()
	{
		this.damageDealer.SetDamage(base.properties.CurrentState.barrel.damage);
		base.StopCoroutine(this.moveCoroutine);
		this.moveCoroutine = this.move_cr();
		base.StartCoroutine(this.moveCoroutine);
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000BA748 File Offset: 0x000B8948
	public void Update()
	{
		AbstractPlayerController[] array = new AbstractPlayerController[]
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne),
			PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		};
		if (this.state == PirateLevelBarrel.State.Move)
		{
			float num = base.transform.position.x - 60f;
			float num2 = base.transform.position.x + 60f;
			foreach (AbstractPlayerController abstractPlayerController in array)
			{
				if (!(abstractPlayerController == null) && !(abstractPlayerController.transform == null) && !abstractPlayerController.IsDead)
				{
					if (abstractPlayerController.center.x > num && abstractPlayerController.center.x < num2)
					{
						this.PlayerFound();
						break;
					}
				}
			}
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x0001C7C6 File Offset: 0x0001A9C6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x0001C7E4 File Offset: 0x0001A9E4
	public void PlayerFound()
	{
		this.state = PirateLevelBarrel.State.Fall;
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000BA850 File Offset: 0x000B8A50
	public IEnumerator move_cr()
	{
		float time = base.properties.CurrentState.barrel.moveTime;
		float p = (base.transform.position.x - -570f) / 805f;
		if (this.direction == PirateLevelBarrel.Direction.Left)
		{
			p = 1f - p;
		}
		float t = time * p;
		for (;;)
		{
			if (this.direction == PirateLevelBarrel.Direction.Right)
			{
				while (t < time)
				{
					yield return base.StartCoroutine(this.waitForMove_cr());
					float val = t / time;
					float x = EaseUtils.Ease(EaseUtils.EaseType.linear, -570f, 235f, val);
					base.transform.SetPosition(new float?(x), null, null);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.direction = PirateLevelBarrel.Direction.Left;
			}
			if (this.direction == PirateLevelBarrel.Direction.Left)
			{
				while (t < time)
				{
					yield return base.StartCoroutine(this.waitForMove_cr());
					float val2 = t / time;
					float x2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 235f, -570f, val2);
					base.transform.SetPosition(new float?(x2), null, null);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.direction = PirateLevelBarrel.Direction.Right;
			}
		}
		yield break;
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000BA86C File Offset: 0x000B8A6C
	public IEnumerator waitForMove_cr()
	{
		while (this.state != PirateLevelBarrel.State.Move && this.state != PirateLevelBarrel.State.Safe)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000BA888 File Offset: 0x000B8A88
	public IEnumerator fall_cr()
	{
		AudioManager.Play("level_pirate_barrel_drop_attack");
		this.emitAudioFromObject.Add("level_pirate_barrel_drop_attack");
		base.animator.SetTrigger("OnFall");
		this.state = PirateLevelBarrel.State.Fall;
		base.GetComponent<Collider2D>().enabled = true;
		LevelProperties.Pirate.State properties = base.properties.CurrentState;
		float t = 0f;
		float time = properties.barrel.fallTime;
		while (t < time)
		{
			float val = t / time;
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInQuart, 250f, -225f, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(-225f), null);
		this.dustEffect.Create(base.transform.position);
		this.particlesEffect.Create(base.transform.position);
		base.animator.SetTrigger("OnSmash");
		this.state = PirateLevelBarrel.State.Hold;
		CupheadLevelCamera.Current.Shake(8f, 0.6f, false);
		yield return CupheadTime.WaitForSeconds(this, properties.barrel.groundHold);
		t = 0f;
		time = properties.barrel.riseTime;
		base.animator.SetTrigger("OnUp");
		this.state = PirateLevelBarrel.State.Up;
		while (t < time)
		{
			float val2 = t / time;
			float y2 = EaseUtils.Ease(EaseUtils.EaseType.linear, -225f, 250f, val2);
			base.transform.SetPosition(null, new float?(y2), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(250f), null);
		base.animator.SetTrigger("OnSafe");
		this.state = PirateLevelBarrel.State.Safe;
		yield return CupheadTime.WaitForSeconds(this, properties.barrel.safeTime);
		base.animator.SetTrigger("OnReady");
		this.state = PirateLevelBarrel.State.Move;
		yield break;
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x0001C7FA File Offset: 0x0001A9FA
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dustEffect = null;
		this.particlesEffect = null;
	}

	// Token: 0x04001B89 RID: 7049
	public const float MIN_X = -570f;

	// Token: 0x04001B8A RID: 7050
	public const float MAX_X = 235f;

	// Token: 0x04001B8B RID: 7051
	public const float UP_Y = 250f;

	// Token: 0x04001B8C RID: 7052
	public const float DOWN_Y = -225f;

	// Token: 0x04001B8D RID: 7053
	public const float RANGE = 120f;

	// Token: 0x04001B8E RID: 7054
	[SerializeField]
	public Effect particlesEffect;

	// Token: 0x04001B8F RID: 7055
	[SerializeField]
	public Effect dustEffect;

	// Token: 0x04001B90 RID: 7056
	public PirateLevelBarrel.State state;

	// Token: 0x04001B91 RID: 7057
	public PirateLevelBarrel.Direction direction = PirateLevelBarrel.Direction.Left;

	// Token: 0x04001B92 RID: 7058
	public IEnumerator moveCoroutine;

	// Token: 0x04001B93 RID: 7059
	public DamageDealer damageDealer;

	// Token: 0x02000E04 RID: 3588
	public enum State
	{
		// Token: 0x04006592 RID: 26002
		Init,
		// Token: 0x04006593 RID: 26003
		Move,
		// Token: 0x04006594 RID: 26004
		Fall,
		// Token: 0x04006595 RID: 26005
		Hold,
		// Token: 0x04006596 RID: 26006
		Up,
		// Token: 0x04006597 RID: 26007
		Safe
	}

	// Token: 0x02000E05 RID: 3589
	public enum Direction
	{
		// Token: 0x04006599 RID: 26009
		Right,
		// Token: 0x0400659A RID: 26010
		Left
	}
}
