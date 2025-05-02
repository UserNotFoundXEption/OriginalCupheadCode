using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class DicePalaceDominoLevelBoomerang : AbstractProjectile
{
	// Token: 0x0600162D RID: 5677 RVA: 0x0009EBDC File Offset: 0x0009CDDC
	public DicePalaceDominoLevelBoomerang Create(Vector2 pos, float speed, float hp)
	{
		DicePalaceDominoLevelBoomerang dicePalaceDominoLevelBoomerang = base.Create(pos) as DicePalaceDominoLevelBoomerang;
		dicePalaceDominoLevelBoomerang.speed = speed;
		dicePalaceDominoLevelBoomerang.HP = hp;
		return dicePalaceDominoLevelBoomerang;
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x00012CCA File Offset: 0x00010ECA
	public override void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.move_cr());
		base.Start();
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00012D02 File Offset: 0x00010F02
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDead)
		{
			this.isDead = true;
			this.Killed();
		}
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00012D3F File Offset: 0x00010F3F
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x00012D5D File Offset: 0x00010F5D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x0009EC08 File Offset: 0x0009CE08
	public IEnumerator move_cr()
	{
		float dropPoint = (float)Level.Current.Ground + base.GetComponent<Collider2D>().bounds.size.y;
		float goToPos = -440f;
		while (base.transform.position.x > goToPos)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnDrop");
		yield return base.animator.WaitForAnimationToStart(this, "Fly_Drop_Start", false);
		AudioManager.Play("dice_palace_domino_bird_dive");
		this.emitAudioFromObject.Add("dice_palace_domino_bird_dive");
		while (base.transform.position.y > dropPoint)
		{
			base.transform.position += Vector3.down * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnStop");
		yield return null;
		yield break;
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00012D7B File Offset: 0x00010F7B
	public void ChangeDirection()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fly_right_cr());
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x0009EC24 File Offset: 0x0009CE24
	public IEnumerator fly_right_cr()
	{
		while (base.transform.position.x < 840f)
		{
			base.transform.position += Vector3.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x00012D90 File Offset: 0x00010F90
	public void WingFlapSFX()
	{
		AudioManager.Play("bird_bird_flap");
		this.emitAudioFromObject.Add("bird_bird_flap");
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x0009EC40 File Offset: 0x0009CE40
	public void Killed()
	{
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.Die();
		AudioManager.Play("dice_bird_die");
		this.emitAudioFromObject.Add("dice_bird_die");
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00012DAC File Offset: 0x00010FAC
	public void SpawnEffect()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.deathPoof.Create(base.transform.position);
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x00012DD1 File Offset: 0x00010FD1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.deathPoof = null;
	}

	// Token: 0x04001213 RID: 4627
	[SerializeField]
	public Effect deathPoof;

	// Token: 0x04001214 RID: 4628
	public DamageReceiver damageReceiver;

	// Token: 0x04001215 RID: 4629
	public float speed;

	// Token: 0x04001216 RID: 4630
	public float HP;

	// Token: 0x04001217 RID: 4631
	public bool isDead;
}
