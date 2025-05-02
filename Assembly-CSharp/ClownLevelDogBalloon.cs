using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class ClownLevelDogBalloon : AbstractProjectile
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060013EC RID: 5100 RVA: 0x00010BDE File Offset: 0x0000EDDE
	// (set) Token: 0x060013ED RID: 5101 RVA: 0x00010BE6 File Offset: 0x0000EDE6
	public ClownLevelDogBalloon.State state { get; set; }

	// Token: 0x060013EE RID: 5102 RVA: 0x00098F5C File Offset: 0x0009715C
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.Play("clown_dog_balloon_regular_intro");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_intro");
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00098FAC File Offset: 0x000971AC
	public void Init(float HP, Vector2 pos, float velocity, AbstractPlayerController player, LevelProperties.Clown.HeliumClown properties, bool flipped)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		this.velocity = velocity;
		this.health = HP;
		if (flipped)
		{
			base.transform.SetScale(new float?(1f), new float?(-base.transform.localScale.y), new float?(1f));
		}
		this.CalculateDirection();
		this.CalculateSin();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x00010BEF File Offset: 0x0000EDEF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00099040 File Offset: 0x00097240
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (this.properties.dogDieOnGround && phase == CollisionPhase.Enter && this.state != ClownLevelDogBalloon.State.Unspawned)
		{
			this.state = ClownLevelDogBalloon.State.Unspawned;
			this.StopAllCoroutines();
			base.animator.SetTrigger("Death");
			AudioManager.Play("clown_dog_balloon_regular_death");
			this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		}
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x000990B0 File Offset: 0x000972B0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != ClownLevelDogBalloon.State.Unspawned)
		{
			this.state = ClownLevelDogBalloon.State.Unspawned;
			this.StopAllCoroutines();
			base.animator.SetTrigger("Death");
			AudioManager.Play("clown_dog_balloon_regular_death");
			this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		}
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00010C0D File Offset: 0x0000EE0D
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00099124 File Offset: 0x00097324
	public void CalculateSin()
	{
		Vector2 zero = Vector2.zero;
		zero.x = (this.player.transform.position.x + base.transform.position.x) / 2f;
		zero.y = (this.player.transform.position.y + base.transform.position.y) / 2f;
		float num = -((this.player.transform.position.x - base.transform.position.x) / (this.player.transform.position.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00099278 File Offset: 0x00097478
	public void CalculateDirection()
	{
		float num = this.player.transform.position.x - base.transform.position.x;
		float num2 = this.player.transform.position.y - base.transform.position.y;
		float value = Mathf.Atan2(num2, num) * 57.29578f;
		this.pointAtPlayer = MathUtils.AngleToDirection(value);
		base.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00099328 File Offset: 0x00097528
	public IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		while (base.transform.position.y > -560f)
		{
			this.angle += 10f * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos += this.normalized * Mathf.Sin(this.angle) * 2f;
			}
			pos += this.pointAtPlayer * this.velocity * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00010C2B File Offset: 0x0000EE2B
	public void ChompSound()
	{
		AudioManager.Play("clown_dog_balloon_regular_chomp");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_chomp");
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00010C47 File Offset: 0x0000EE47
	public override void Die()
	{
		AudioManager.Play("clown_dog_balloon_regular_death");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04001038 RID: 4152
	public LevelProperties.Clown.HeliumClown properties;

	// Token: 0x04001039 RID: 4153
	public AbstractPlayerController player;

	// Token: 0x0400103A RID: 4154
	public Vector3 pointAtPlayer;

	// Token: 0x0400103B RID: 4155
	public Vector3 normalized;

	// Token: 0x0400103C RID: 4156
	public float health;

	// Token: 0x0400103D RID: 4157
	public float pointAt;

	// Token: 0x0400103E RID: 4158
	public float velocity;

	// Token: 0x0400103F RID: 4159
	public float angle;

	// Token: 0x04001040 RID: 4160
	public DamageReceiver damageReceiver;

	// Token: 0x02000B0E RID: 2830
	public enum State
	{
		// Token: 0x0400512D RID: 20781
		Spawned,
		// Token: 0x0400512E RID: 20782
		Unspawned
	}
}
