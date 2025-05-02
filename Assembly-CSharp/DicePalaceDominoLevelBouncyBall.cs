using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class DicePalaceDominoLevelBouncyBall : AbstractProjectile
{
	// Token: 0x0600163A RID: 5690 RVA: 0x00012DE8 File Offset: 0x00010FE8
	public void InitBouncyBall(float speed, Vector3 direction)
	{
		this.deltaPosition = direction * speed;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.checkCollisions_cr());
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x00012E11 File Offset: 0x00011011
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			base.animator.SetInteger("Variation", 3);
		}
		else
		{
			base.animator.SetInteger("Variation", Random.Range(1, 3));
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x0009EC90 File Offset: 0x0009CE90
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += this.deltaPosition * CupheadTime.Delta;
			for (int i = 0; i < this.toRotate.Length; i++)
			{
				this.toRotate[i].Rotate(Vector3.forward, 180f * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x0009ECAC File Offset: 0x0009CEAC
	public IEnumerator checkCollisions_cr()
	{
		for (;;)
		{
			if (base.transform.position.y > (float)Level.Current.Ceiling)
			{
				this.deltaPosition.y = this.deltaPosition.y * -1f;
				this.BounceSFX();
				base.animator.SetTrigger("Bounce");
				this.hitEffectPrefab.Create(base.transform.position, new Vector3(1f, -1f, 1f));
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.y < (float)Level.Current.Ground)
			{
				this.deltaPosition.y = this.deltaPosition.y * -1f;
				this.BounceSFX();
				base.animator.SetTrigger("Bounce");
				this.hitEffectPrefab.Create(base.transform.position);
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.x > (float)Level.Current.Right)
			{
				this.deltaPosition.x = this.deltaPosition.x * -1f;
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.x < (float)Level.Current.Left)
			{
				this.deltaPosition.x = this.deltaPosition.x * -1f;
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00012E4D File Offset: 0x0001104D
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (hit.GetComponent<BasicDamageDealingObject>())
		{
			this.Die();
		}
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x00012E6D File Offset: 0x0001106D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00012E8B File Offset: 0x0001108B
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.hitEffectPrefab = null;
		this.explosion = null;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00012EA7 File Offset: 0x000110A7
	public override void Die()
	{
		this.BounceSFX();
		this.explosion.Create(base.transform.position);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x00012ED1 File Offset: 0x000110D1
	public void BounceSFX()
	{
		AudioManager.Play("dice_projectile_bounce");
		this.emitAudioFromObject.Add("dice_projectile_bounce");
	}

	// Token: 0x04001218 RID: 4632
	public const float RotationFactor = 180f;

	// Token: 0x04001219 RID: 4633
	[SerializeField]
	public Effect hitEffectPrefab;

	// Token: 0x0400121A RID: 4634
	[SerializeField]
	public Effect explosion;

	// Token: 0x0400121B RID: 4635
	[SerializeField]
	public Transform[] toRotate;

	// Token: 0x0400121C RID: 4636
	public Vector3 deltaPosition;

	// Token: 0x02000B86 RID: 2950
	public enum Colour
	{
		// Token: 0x0400543E RID: 21566
		blue,
		// Token: 0x0400543F RID: 21567
		green,
		// Token: 0x04005440 RID: 21568
		red,
		// Token: 0x04005441 RID: 21569
		yellow,
		// Token: 0x04005442 RID: 21570
		none
	}
}
