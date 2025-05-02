using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class WeaponCrackshotProjectile : BasicProjectile
{
	// Token: 0x060038A0 RID: 14496 RVA: 0x00108614 File Offset: 0x00106814
	public override void Start()
	{
		base.Start();
		base.animator.Play(this.variant.ToString(), 0, Random.Range(0f, 1f));
		this.damageDealer.isDLCWeapon = true;
		AudioManager.Play("player_weapon_crackshot_shoot");
		this.emitAudioFromObject.Add("player_weapon_crackshot_shoot");
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x0002E298 File Offset: 0x0002C498
	public override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x0010867C File Offset: 0x0010687C
	public override void Die()
	{
		this.move = false;
		base.Die();
		this.coll.enabled = false;
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet"))
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactCometB" : "ImpactCometA");
		}
		else
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactSmallB" : "ImpactSmallA");
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x0002E2BC File Offset: 0x0002C4BC
	public void _OnDieAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x00108710 File Offset: 0x00106910
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.cracked && base.distance > WeaponProperties.LevelWeaponCrackshot.Basic.crackDistance && !base.dead)
		{
			this.cracked = true;
			this.crackFX.Create(base.transform.position);
			base.animator.SetBool("IsB", this.useBComet);
			base.animator.Play((!Rand.Bool()) ? "CometStartA" : "CometStartB");
			AudioManager.Play("player_weapon_crackshot_shootfast");
			this.emitAudioFromObject.Add("player_weapon_crackshot_shootfast");
			this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Basic.crackedDamage);
			this.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.crackedSpeed;
			this.FindTarget();
			if (this.target != null)
			{
				if (Vector3.Angle(base.transform.right, this.target.bounds.center - base.transform.position) > this.maxAngleRange)
				{
					if (Mathf.Abs(base.transform.right.y) < 0.05f)
					{
						base.transform.eulerAngles = new Vector3(0f, 0f, (base.transform.eulerAngles.z <= 90f) ? (MathUtils.DirectionToAngle(base.transform.right) + this.maxAngleRange) : (MathUtils.DirectionToAngle(base.transform.right) - this.maxAngleRange));
					}
					else
					{
						base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(base.transform.right) + this.maxAngleRange);
					}
				}
				else
				{
					base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position));
				}
			}
		}
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x0002E2C9 File Offset: 0x0002C4C9
	public void FindTarget()
	{
		this.target = this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x00108948 File Offset: 0x00106B48
	public Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		Collider2D collider2D = null;
		Collider2D collider2D2 = null;
		Vector2 vector = base.transform.position;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D3 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
					{
						float sqrMagnitude = (vector - collider2D3.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							num2 = sqrMagnitude;
							collider2D2 = collider2D3;
						}
						if (sqrMagnitude < num && Vector3.Angle(base.transform.right, collider2D3.bounds.center - vector) < this.maxAngleRange)
						{
							num = sqrMagnitude;
							collider2D = collider2D3;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D4 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D4.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D4.bounds.center, collider2D4.bounds.size / 2f))
						{
							float sqrMagnitude2 = (vector - collider2D4.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num2)
							{
								num2 = sqrMagnitude2;
								collider2D2 = collider2D4;
							}
							if (sqrMagnitude2 < num && Vector3.Angle(base.transform.right, collider2D4.bounds.center - vector) < this.maxAngleRange)
							{
								num = sqrMagnitude2;
								collider2D = collider2D4;
							}
						}
					}
				}
			}
		}
		return (!(collider2D == null)) ? collider2D : collider2D2;
	}

	// Token: 0x04002D80 RID: 11648
	public Collider2D target;

	// Token: 0x04002D81 RID: 11649
	public bool cracked;

	// Token: 0x04002D82 RID: 11650
	public float maxAngleRange;

	// Token: 0x04002D83 RID: 11651
	public int variant;

	// Token: 0x04002D84 RID: 11652
	public bool useBComet;

	// Token: 0x04002D85 RID: 11653
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04002D86 RID: 11654
	[SerializeField]
	public Effect crackFX;
}
