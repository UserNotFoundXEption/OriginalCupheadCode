using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class PlayerSuperGhost : AbstractPlayerSuper
{
	// Token: 0x060037DF RID: 14303 RVA: 0x00105460 File Offset: 0x00103660
	public override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_ghost");
		if (!this.player.motor.Grounded)
		{
			this.cupheadBottom.enabled = false;
			this.mugmanBottom.enabled = false;
		}
		this.createHeart = true;
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x001054C0 File Offset: 0x001036C0
	public void FixedUpdate()
	{
		if (this.state != PlayerSuperGhost.State.Spinning)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		Quaternion localRotation = base.transform.localRotation;
		float num;
		if (this.t < WeaponProperties.LevelSuperGhost.initialSpeedTime)
		{
			num = Mathf.Clamp01(this.t / WeaponProperties.LevelSuperGhost.accelerationTime) * WeaponProperties.LevelSuperGhost.initialSpeed;
		}
		else
		{
			float num2 = Mathf.Clamp01((this.t - WeaponProperties.LevelSuperGhost.initialSpeedTime) / WeaponProperties.LevelSuperGhost.accelerationTime);
			num = Mathf.Lerp(WeaponProperties.LevelSuperGhost.initialSpeed, WeaponProperties.LevelSuperGhost.maxSpeed, num2);
		}
		Trilean2 lookDirection = new Trilean2(0, 0);
		if (this.player != null)
		{
			lookDirection = this.player.motor.LookDirection;
			if (this.player.motor.GravityReversed)
			{
				lookDirection.y *= -1;
			}
			if (this.player.IsDead || (lookDirection.x == 0 && lookDirection.y == 0))
			{
				lookDirection = this.lookDir;
			}
		}
		this.lookDir = lookDirection;
		Vector2 vector;
		vector..ctor(this.lookDir.x, this.lookDir.y);
		Vector2 normalized = vector.normalized;
		Vector2 vector2;
		vector2..ctor(normalized.x * num, normalized.y * num);
		this.velocity = Vector2.Lerp(this.velocity, vector2, CupheadTime.FixedDelta * WeaponProperties.LevelSuperGhost.turnaroundEaseMultiplier);
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		if (lookDirection.x > 0)
		{
			if (localRotation.z < 0.0349065848f)
			{
				localRotation.z += 0.01f;
			}
		}
		else if (localRotation.z > -0.0349065848f)
		{
			localRotation.z -= 0.01f;
		}
		base.transform.localRotation = localRotation;
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position + new Vector2(0f, 150f * base.transform.localScale.y), new Vector2(200f, 200f)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x0002D9A6 File Offset: 0x0002BBA6
	public void EndPlayerAnimation()
	{
		this.Fire();
		this.EndSuper(true);
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x00105754 File Offset: 0x00103954
	public IEnumerator super_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		AudioManager.Play("player_super_beam");
		this.state = PlayerSuperGhost.State.Spinning;
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperGhost.damage, WeaponProperties.LevelSuperGhost.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		this.lookDir = this.player.motor.TrueLookDirection;
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperGhost.initialSpeedTime);
		base.animator.SetTrigger("Continue");
		float t = 0f;
		float duration = (!this.createHeart) ? WeaponProperties.LevelSuperGhost.noHeartMaxSpeedTime : WeaponProperties.LevelSuperGhost.maxSpeedTime;
		while (t < duration && !this.interrupted)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = PlayerSuperGhost.State.Dying;
		base.animator.SetTrigger("Death");
		yield break;
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x0002D9B5 File Offset: 0x0002BBB5
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x00105770 File Offset: 0x00103970
	public void SpawnHeart()
	{
		if (this.player != null && this.createHeart)
		{
			this.heartPrefab.Create(this.heartRoot.position, this.player.motor.GravityReversalMultiplier);
		}
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x0002D9C2 File Offset: 0x0002BBC2
	public override void Interrupt()
	{
		this.createHeart = false;
		base.Interrupt();
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x0002D9D1 File Offset: 0x0002BBD1
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x0002D9D9 File Offset: 0x0002BBD9
	public void SoundSuperGhostVoice()
	{
		AudioManager.Play("player_super_ghost_voice");
		this.emitAudioFromObject.Add("player_super_ghost_voice");
	}

	// Token: 0x04002D06 RID: 11526
	[SerializeField]
	public PlayerSuperGhostHeart heartPrefab;

	// Token: 0x04002D07 RID: 11527
	[SerializeField]
	public Transform heartRoot;

	// Token: 0x04002D08 RID: 11528
	[SerializeField]
	public SpriteRenderer cupheadBottom;

	// Token: 0x04002D09 RID: 11529
	[SerializeField]
	public SpriteRenderer mugmanBottom;

	// Token: 0x04002D0A RID: 11530
	public PlayerSuperGhost.State state;

	// Token: 0x04002D0B RID: 11531
	public Vector2 velocity = Vector2.zero;

	// Token: 0x04002D0C RID: 11532
	public float t;

	// Token: 0x04002D0D RID: 11533
	public Trilean2 lookDir;

	// Token: 0x04002D0E RID: 11534
	public bool createHeart;

	// Token: 0x020011AF RID: 4527
	public enum State
	{
		// Token: 0x04007BC2 RID: 31682
		Intro,
		// Token: 0x04007BC3 RID: 31683
		Spinning,
		// Token: 0x04007BC4 RID: 31684
		Dying
	}
}
