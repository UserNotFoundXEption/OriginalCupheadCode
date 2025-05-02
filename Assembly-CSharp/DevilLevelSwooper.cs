using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class DevilLevelSwooper : AbstractCollidableObject
{
	// Token: 0x060014F0 RID: 5360 RVA: 0x0009ADC8 File Offset: 0x00098FC8
	public DevilLevelSwooper Create(DevilLevelGiantHead parent, LevelProperties.Devil.Swoopers properties, Vector3 spawnPos, float xPos)
	{
		DevilLevelSwooper devilLevelSwooper = this.InstantiatePrefab<DevilLevelSwooper>();
		devilLevelSwooper.parent = parent;
		devilLevelSwooper.properties = properties;
		devilLevelSwooper.state = DevilLevelSwooper.State.Intro;
		devilLevelSwooper.transform.position = spawnPos;
		devilLevelSwooper.yPos = properties.yIdlePos.RandomFloat();
		devilLevelSwooper.StartCoroutine(devilLevelSwooper.spawn_cr(xPos));
		return devilLevelSwooper;
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x00011C79 File Offset: 0x0000FE79
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x00011CA3 File Offset: 0x0000FEA3
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x00011CBB File Offset: 0x0000FEBB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x00011CE4 File Offset: 0x0000FEE4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != DevilLevelSwooper.State.Dying)
		{
			this.Die();
		}
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x0009AE20 File Offset: 0x00099020
	public IEnumerator spawn_cr(float xPos)
	{
		this.hp = this.properties.hp;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		yield return base.animator.WaitForAnimationToEnd(this, "Spawn", false, true);
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50f)
		{
			base.transform.position += Vector3.up * 200f * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		float t = 0f;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector3(xPos, this.yPos);
		while (t < 2f)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / 2f);
			base.transform.position = Vector3.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(xPos), new float?(this.yPos), null);
		this.state = DevilLevelSwooper.State.Idle;
		yield break;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00011D1B File Offset: 0x0000FF1B
	public void Swoop()
	{
		this.state = DevilLevelSwooper.State.Swooping;
		base.StartCoroutine(this.swoop_cr());
		AudioManager.Play("mini_devil_attack");
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x0009AE44 File Offset: 0x00099044
	public IEnumerator swoop_cr()
	{
		float bestDistance = float.MaxValue;
		Vector2 bestVelocity = Vector2.zero;
		Vector3 target = PlayerManager.GetNext().center;
		Vector2 relativeTargetPos = target - base.transform.position;
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
		if (target.x > base.transform.position.x)
		{
			base.animator.SetTrigger("OnTurn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		base.animator.SetBool("Spinning", true);
		this.AttackSFX();
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			float angle = -this.properties.launchAngle.GetFloatAt(num);
			float floatAt = this.properties.launchSpeed.GetFloatAt(num);
			Vector2 vector = MathUtils.AngleToDirection(angle) * floatAt;
			float num2 = relativeTargetPos.x / vector.x;
			float num3 = vector.y * num2 + 0.5f * this.properties.gravity * num2 * num2;
			float num4 = Mathf.Abs(relativeTargetPos.y - num3);
			float num5 = vector.y + this.properties.gravity * num2;
			if (num5 >= 0f)
			{
				if (num4 < bestDistance)
				{
					bestDistance = num4;
					bestVelocity = vector;
				}
			}
		}
		if (target.x < base.transform.position.x)
		{
			bestVelocity.x *= -1f;
		}
		Vector2 velocity = bestVelocity;
		while (base.transform.position.y < (float)(Level.Current.Ceiling + 150))
		{
			velocity.y += this.properties.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.state = DevilLevelSwooper.State.Returning;
		float xPos = this.parent.PutSwooperInSlot(this);
		base.transform.SetPosition(new float?(xPos), null, null);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		float moveTime = 1.5f;
		float t = 0f;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, (float)(Level.Current.Ceiling + 150), this.yPos, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.state = DevilLevelSwooper.State.Idle;
		base.transform.SetPosition(null, new float?(this.yPos), null);
		base.animator.SetBool("Spinning", false);
		this.AttackSFXEnd();
		yield break;
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x0009AE60 File Offset: 0x00099060
	public void Die()
	{
		if (this.finalSwooping)
		{
			AudioManager.Stop("swooper_spin");
		}
		if (this.state == DevilLevelSwooper.State.Dying)
		{
			return;
		}
		this.state = DevilLevelSwooper.State.Dying;
		this.StopAllCoroutines();
		this.parent.OnSwooperDeath(this);
		base.StartCoroutine(this.death_cr());
		AudioManager.Play("mini_devil_die");
		this.emitAudioFromObject.Add("mini_devil_die");
		AudioManager.Stop("swooper_spin");
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0009AEDC File Offset: 0x000990DC
	public IEnumerator death_cr()
	{
		while (this.state == DevilLevelSwooper.State.Intro)
		{
			yield return null;
		}
		foreach (Effect effect in this.explosions)
		{
			effect.Create(base.transform.position);
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x0009AEF8 File Offset: 0x000990F8
	public void OnTurn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00011D3B File Offset: 0x0000FF3B
	public void AttackSFX()
	{
		AudioManager.PlayLoop("swooper_spin");
		this.emitAudioFromObject.Add("swooper_spin_end");
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00011D57 File Offset: 0x0000FF57
	public void AttackSFXEnd()
	{
		if (this.finalSwooping)
		{
			AudioManager.Stop("swooper_spin");
		}
		AudioManager.Play("swooper_spin_end");
		this.emitAudioFromObject.Add("swooper_spin_end");
		this.finalSwooping = false;
	}

	// Token: 0x04001127 RID: 4391
	[SerializeField]
	public Effect[] explosions;

	// Token: 0x04001128 RID: 4392
	public const float SPAWN_X_RATIO = 0.5f;

	// Token: 0x04001129 RID: 4393
	public DevilLevelSwooper.State state;

	// Token: 0x0400112A RID: 4394
	public DevilLevelGiantHead parent;

	// Token: 0x0400112B RID: 4395
	public LevelProperties.Devil.Swoopers properties;

	// Token: 0x0400112C RID: 4396
	public DamageDealer damageDealer;

	// Token: 0x0400112D RID: 4397
	public float hp;

	// Token: 0x0400112E RID: 4398
	public bool finalSwooping;

	// Token: 0x0400112F RID: 4399
	public float yPos;

	// Token: 0x02000B4F RID: 2895
	public enum State
	{
		// Token: 0x040052DE RID: 21214
		Intro,
		// Token: 0x040052DF RID: 21215
		Idle,
		// Token: 0x040052E0 RID: 21216
		Swooping,
		// Token: 0x040052E1 RID: 21217
		Returning,
		// Token: 0x040052E2 RID: 21218
		Dying
	}
}
