using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class RumRunnersLevelBarrel : LevelProperties.RumRunners.Entity
{
	// Token: 0x06002441 RID: 9281 RVA: 0x000C3040 File Offset: 0x000C1240
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.coll = base.GetComponent<Collider2D>();
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x0001E9AB File Offset: 0x0001CBAB
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		((RumRunnersLevel)Level.Current).OnUpperBridgeDestroy += this.onUpperBridgeDestroy;
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x000C3090 File Offset: 0x000C1290
	public void Initialize(float dir, Vector3 spawnPos, RumRunnersLevelWorm parent, bool parryable, bool isCop)
	{
		this.isCop = isCop;
		this.facingDirection = dir;
		base.transform.position = spawnPos;
		base.transform.localScale = new Vector3(dir, 1f);
		this.parent = parent;
		this.runSpeed = base.properties.CurrentState.barrels.barrelSpeed;
		this.HP = (float)base.properties.CurrentState.barrels.barrelHP;
		this._canParry = parryable;
		if (isCop)
		{
			base.animator.Play("Cop");
		}
		else if (Rand.Bool())
		{
			base.animator.Play((!base.canParry) ? "DanceA" : "DanceAParry");
		}
		else
		{
			base.animator.Play((!base.canParry) ? "DanceB" : "DanceBParry");
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x0001E9CF File Offset: 0x0001CBCF
	public override void OnParry(AbstractPlayerController player)
	{
		player.stats.OnParry(1f, true);
		this.Die(false, false);
		this._canParry = false;
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x0001E9F1 File Offset: 0x0001CBF1
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x0001EA09 File Offset: 0x0001CC09
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die(false, true);
		}
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x0001EA40 File Offset: 0x0001CC40
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000C3198 File Offset: 0x000C1398
	public IEnumerator move_cr()
	{
		while (base.transform.position.x * this.facingDirection < 960f)
		{
			base.transform.position += Vector3.right * this.facingDirection * this.runSpeed * CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.coll, this.verticalOffset, 200f)), null);
			if (Level.Current.mode == Level.Mode.Easy && this.parent.isDead)
			{
				this.Die(false, true);
				this._canParry = false;
			}
			yield return new WaitForFixedUpdate();
		}
		this.Die(true, true);
		yield break;
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x000C31B4 File Offset: 0x000C13B4
	public void Die(bool immediate, bool spawnShrapnel = true)
	{
		((RumRunnersLevel)Level.Current).OnUpperBridgeDestroy -= this.onUpperBridgeDestroy;
		this.StopAllCoroutines();
		if (immediate)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.isCop)
		{
			base.StartCoroutine(this.copDeath_cr());
		}
		else
		{
			if (base.transform.position.x * this.facingDirection < 960f)
			{
				Effect effect = this.deathPoof.Create(base.transform.position);
				if (!spawnShrapnel)
				{
					effect.GetComponent<Animator>().Play("Poof", 0, 0.0833333358f);
				}
				this.SFX_RUMRUN_BarrelExplode();
				if (spawnShrapnel)
				{
					float num = Random.Range(0f, 6.28318548f);
					for (int i = 0; i < 2; i++)
					{
						for (int j = 0; j < 4; j++)
						{
							float num2 = num + 6.28318548f * (float)j / 4f;
							Vector3 vector;
							vector..ctor(Mathf.Cos(num2) * 50f, Mathf.Sin(num2) * 50f);
							Effect effect2 = this.deathShrapnel.Create(base.transform.position + vector);
							effect2.animator.SetInteger("Effect", j);
							effect2.animator.SetBool("Parry", this._canParry);
							if (i > 0)
							{
								SpriteRenderer component = effect2.GetComponent<SpriteRenderer>();
								component.sortingLayerName = "Background";
								component.sortingOrder = 95;
								component.color = new Color(0.7f, 0.7f, 0.7f, 1f);
								effect2.transform.SetScale(new float?(0.75f), new float?(0.75f), null);
							}
							SpriteDeathParts component2 = effect2.GetComponent<SpriteDeathParts>();
							if (vector.x > 0f)
							{
								component2.SetVelocityX(0f, component2.VelocityXMax);
							}
							else
							{
								component2.SetVelocityX(component2.VelocityXMin, 0f);
							}
						}
					}
				}
			}
			if (!spawnShrapnel)
			{
				base.GetComponent<Collider2D>().enabled = false;
				this.runSpeed = 0f;
				base.StartCoroutine(this.destroy_with_delay_cr());
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x000C3418 File Offset: 0x000C1618
	public IEnumerator destroy_with_delay_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x000C3434 File Offset: 0x000C1634
	public IEnumerator copDeath_cr()
	{
		this.SFX_RUMRUN_Police_DiePoof();
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.SetTrigger("CopDeath");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "CopDeath", 0, false, false, true);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x000C3450 File Offset: 0x000C1650
	public void onUpperBridgeDestroy(Rangef effectRange)
	{
		if (base.transform.position.y < 0f)
		{
			return;
		}
		if (effectRange.ContainsInclusive(base.transform.position.x))
		{
			this.Die(false, true);
			this._canParry = false;
		}
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x0001EA57 File Offset: 0x0001CC57
	public void SFX_RUMRUN_BarrelExplode()
	{
		AudioManager.Play("sfx_dlc_rumrun_barrel_explode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_barrel_explode");
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x0001EA73 File Offset: 0x0001CC73
	public void SFX_RUMRUN_Police_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
		AudioManager.Stop("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x04001DFF RID: 7679
	[SerializeField]
	public Effect deathPoof;

	// Token: 0x04001E00 RID: 7680
	[SerializeField]
	public Effect deathShrapnel;

	// Token: 0x04001E01 RID: 7681
	[SerializeField]
	public float verticalOffset;

	// Token: 0x04001E02 RID: 7682
	public DamageDealer damageDealer;

	// Token: 0x04001E03 RID: 7683
	public DamageReceiver damageReceiver;

	// Token: 0x04001E04 RID: 7684
	public float runSpeed;

	// Token: 0x04001E05 RID: 7685
	public float HP;

	// Token: 0x04001E06 RID: 7686
	public float facingDirection;

	// Token: 0x04001E07 RID: 7687
	public Collider2D coll;

	// Token: 0x04001E08 RID: 7688
	public RumRunnersLevelWorm parent;

	// Token: 0x04001E09 RID: 7689
	public bool isCop;
}
