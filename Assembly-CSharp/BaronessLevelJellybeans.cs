using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class BaronessLevelJellybeans : AbstractProjectile
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0000D4AF File Offset: 0x0000B6AF
	// (set) Token: 0x06000FAB RID: 4011 RVA: 0x0000D4B7 File Offset: 0x0000B6B7
	public BaronessLevelJellybeans.State state { get; set; }

	// Token: 0x06000FAC RID: 4012 RVA: 0x0008DFD4 File Offset: 0x0008C1D4
	public BaronessLevelJellybeans Create(LevelProperties.Baroness.Jellybeans properties, Vector3 pos, float speed, float health)
	{
		BaronessLevelJellybeans baronessLevelJellybeans = base.Create() as BaronessLevelJellybeans;
		baronessLevelJellybeans.properties = properties;
		baronessLevelJellybeans.speed = speed;
		baronessLevelJellybeans.health = health;
		baronessLevelJellybeans.transform.position = pos;
		return baronessLevelJellybeans;
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0008E010 File Offset: 0x0008C210
	public override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = BaronessLevelJellybeans.State.Run;
		AudioManager.Play("level_baroness_jellybean_spawn");
		this.emitAudioFromObject.Add("level_baroness_jellybean_spawn");
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.beginning_offset_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
	public void KillJelly()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.Die();
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0000D4F2 File Offset: 0x0000B6F2
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0008E0A8 File Offset: 0x0008C2A8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelJellybeans.State.Dead)
		{
			this.state = BaronessLevelJellybeans.State.Dead;
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play((!Rand.Bool()) ? "Jellybean_Death_B" : "Jellybean_Death_A");
		}
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0000D510 File Offset: 0x0000B710
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0008E11C File Offset: 0x0008C31C
	public IEnumerator fade_color_cr()
	{
		Color endColor = base.GetComponent<SpriteRenderer>().color;
		float fadeTime = 0.2f;
		float t = 0f;
		Color start = new Color(0f, 0f, 0f, 1f);
		while (t < fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = Color.Lerp(start, endColor, t / fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = endColor;
		yield return null;
		yield break;
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x0000D531 File Offset: 0x0000B731
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosion = null;
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x0008E138 File Offset: 0x0008C338
	public IEnumerator beginning_offset_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 pos = base.transform.position;
		Vector3 startPos = base.transform.position;
		this.velocity = this.properties.jumpSpeed;
		pos.y = base.transform.position.y;
		startPos.y = base.transform.position.y + 40f;
		this.originalPos = pos;
		base.transform.position = startPos;
		while (base.transform.position.y >= pos.y)
		{
			if (this.state == BaronessLevelJellybeans.State.Run)
			{
				base.transform.AddPosition(0f, -100f * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0008E154 File Offset: 0x0008C354
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.state = BaronessLevelJellybeans.State.Run;
		float offset = 200f;
		while (base.transform.position.x > -640f - offset)
		{
			if (this.state != BaronessLevelJellybeans.State.Jump)
			{
				Vector3 pos = base.transform.position;
				pos.x += -this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
				base.transform.position = pos;
			}
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0000D540 File Offset: 0x0000B740
	public void StartJump()
	{
		this.state = BaronessLevelJellybeans.State.Jump;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0008E170 File Offset: 0x0008C370
	public IEnumerator jump_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.velocity = this.properties.jumpSpeed;
		float decrement = 1f;
		Vector3 pos = base.transform.position;
		bool jumping = true;
		bool landing = false;
		base.animator.Play("Jellybean_Jump_Antic");
		yield return base.animator.WaitForAnimationToEnd(this, "Jellybean_Jump_Antic", false, true);
		while (jumping)
		{
			base.transform.AddPosition(0f, this.velocity * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			if (base.transform.position.y >= this.properties.heightDefault + this.properties.jumpHeight.RandomFloat())
			{
				this.velocity -= decrement;
				if (!landing)
				{
					this.velocity = -this.velocity;
					base.animator.SetTrigger("Land");
					landing = true;
				}
			}
			if (base.transform.position.y <= this.originalPos.y)
			{
				if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Jellybean_Jump_Land"))
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Jellybean_Jump_Land", false, true);
				}
				jumping = false;
			}
			yield return wait;
		}
		base.StartCoroutine(this.timer_cr());
		pos.y = this.originalPos.y;
		base.transform.position = pos;
		this.state = BaronessLevelJellybeans.State.Run;
		yield return null;
		yield break;
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x0008E18C File Offset: 0x0008C38C
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.afterJumpDuration);
		yield break;
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0000D556 File Offset: 0x0000B756
	public void DeathComplete()
	{
		this.explosion.Create(base.transform.position);
		AudioManager.Play("level_baroness_jellybean_death");
		this.emitAudioFromObject.Add("level_baroness_jellybean_death");
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x0000D594 File Offset: 0x0000B794
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04000CC8 RID: 3272
	[SerializeField]
	public Effect explosion;

	// Token: 0x04000CCA RID: 3274
	public float health;

	// Token: 0x04000CCB RID: 3275
	public float speed;

	// Token: 0x04000CCC RID: 3276
	public float velocity;

	// Token: 0x04000CCD RID: 3277
	public Vector3 originalPos;

	// Token: 0x04000CCE RID: 3278
	public LevelProperties.Baroness.Jellybeans properties;

	// Token: 0x04000CCF RID: 3279
	public DamageReceiver damageReceiver;

	// Token: 0x02000A09 RID: 2569
	public enum State
	{
		// Token: 0x04004A61 RID: 19041
		Dead,
		// Token: 0x04004A62 RID: 19042
		Run,
		// Token: 0x04004A63 RID: 19043
		Jump
	}
}
