using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class SnowCultLevelQuadShot : AbstractProjectile
{
	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06002881 RID: 10369 RVA: 0x00022002 File Offset: 0x00020202
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000CE994 File Offset: 0x000CCB94
	public virtual SnowCultLevelQuadShot Init(Vector3 startPos, Vector3 destPos, float speed, string hazardDirectionInstruction, LevelProperties.SnowCult.QuadShot properties, int rowPosition, float delay, float distanceBetween, AbstractPlayerController targetPlayer)
	{
		base.ResetLifetime();
		base.ResetDistance();
		((SnowCultLevel)Level.Current).OnYetiHitGround += this.WhaleDeath;
		this.id = ((rowPosition % 2 != 0) ? 'B' : 'A');
		base.animator.Play("Emerge" + this.id);
		base.transform.position = startPos;
		this.startPos = startPos;
		this.destPos = destPos;
		this.properties = properties;
		this.speed = speed;
		this.hazardDirectionInstruction = hazardDirectionInstruction;
		this.rowPosition = rowPosition;
		this.distanceBetween = distanceBetween;
		this.targetPlayer = targetPlayer;
		this.delay = delay;
		base.transform.localScale = new Vector3((float)((this.rowPosition <= 1) ? 1 : -1), 1f);
		base.StartCoroutine(this.move_to_launch_pos_cr());
		base.tag = "EnemyProjectile";
		return this;
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x00022009 File Offset: 0x00020209
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000CEA94 File Offset: 0x000CCC94
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.grounded)
		{
			return;
		}
		base.OnCollisionEnemy(hit, phase);
		if (phase != CollisionPhase.Exit && (hit.GetComponent<SnowCultLevelWhaleCollision>() || hit.GetComponent<SnowCultLevelQuadShot>()) && phase == CollisionPhase.Enter)
		{
			base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x - hit.gameObject.transform.position.x), 1f);
			this.WhaleDeath();
		}
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000CEB30 File Offset: 0x000CCD30
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.grounded)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health < 0f)
		{
			if (this.running)
			{
				if (!base.dead)
				{
					Level.Current.RegisterMinionKilled();
					base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), 1f);
					this.rend.flipX = Rand.Bool();
					this.Dead();
				}
			}
			else
			{
				this.running = true;
				this.health = this.properties.hazardHealth;
			}
		}
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000CEBDC File Offset: 0x000CCDDC
	public IEnumerator move_to_launch_pos_cr()
	{
		float t = 0f;
		while (t < 0.333333343f)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
			t += 0.0416666679f;
			base.transform.position = Vector3.Lerp(this.startPos, this.destPos, Mathf.InverseLerp(0f, 0.333333343f, t));
		}
		yield break;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000CEBF8 File Offset: 0x000CCDF8
	public void Shoot(float angle)
	{
		this.angle = angle;
		base.transform.localScale = new Vector3((float)((angle >= -90f) ? -1 : 1), 1f);
		base.animator.Play("Launch" + this.id);
		this.sparkEffect.Create(base.transform.position);
		if (this.rowPosition % 2 == 0)
		{
			this.rend.sortingOrder = 3;
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000CEC94 File Offset: 0x000CCE94
	public IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		while (base.transform.position.y > -240f)
		{
			base.transform.position += MathUtils.AngleToDirection(this.angle) * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.position = new Vector3(base.transform.position.x, -240f);
		base.StartCoroutine(this.run_away_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000CECB0 File Offset: 0x000CCEB0
	public IEnumerator run_away_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.tag = "Enemy";
		this.grounded = true;
		this.health = this.properties.groundHealth;
		base.animator.Play("HitGround" + this.id);
		this.SFX_SNOWCULT_QuadshotMinionStuckInGround();
		this.snowLandEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
		float t = (this.properties.hazardMoveDelay - this.delay) * this.popOutWarningTimeNormalized;
		while (t > 0f && !this.running)
		{
			t -= CupheadTime.Delta;
			yield return null;
		}
		animHelper.Speed = 1.5f;
		t = (this.properties.hazardMoveDelay - this.delay) * (1f - this.popOutWarningTimeNormalized);
		while (t > 0f && !this.running)
		{
			t -= CupheadTime.Delta;
			yield return null;
		}
		animHelper.Speed = 1f;
		this.running = true;
		this.health = this.properties.hazardHealth;
		float direction = 0f;
		string text = this.hazardDirectionInstruction;
		if (text != null)
		{
			if (!(text == "L"))
			{
				if (!(text == "R"))
				{
					if (!(text == "F"))
					{
						if (!(text == "G"))
						{
							if (text == "P")
							{
								direction = (float)((base.transform.position.x - this.targetPlayer.transform.position.x <= 0f) ? 1 : -1);
							}
						}
						else
						{
							float num = base.transform.position.x + ((float)(2 - this.rowPosition) - 0.5f) * this.distanceBetween;
							direction = (float)((Mathf.Abs(num - (float)Level.Current.Left) <= Mathf.Abs(num - (float)Level.Current.Right)) ? 1 : -1);
						}
					}
					else
					{
						direction = (float)((Mathf.Abs(base.transform.position.x - (float)Level.Current.Left) <= Mathf.Abs(base.transform.position.x - (float)Level.Current.Right)) ? 1 : -1);
					}
				}
				else
				{
					direction = 1f;
				}
			}
			else
			{
				direction = -1f;
			}
		}
		base.transform.localScale = new Vector3(-direction, 1f);
		base.animator.Play("PopOut" + this.id);
		this.SFX_SNOWCULT_QuadshotMinionFlipUp();
		this.snowPopOutEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.166666672f)
		{
			yield return null;
		}
		this.health = this.properties.hazardHealth;
		this.rend.sortingOrder = 1;
		t = 0f;
		while (base.transform.position.x > (float)(Level.Current.Left - 200) && base.transform.position.x < (float)(Level.Current.Right + 200))
		{
			t = Mathf.Clamp(t + CupheadTime.FixedDelta * 2f, 0f, 1f);
			base.transform.position += Vector3.right * direction * this.properties.hazardSpeed * CupheadTime.FixedDelta * t;
			yield return wait;
		}
		this.Recycle<SnowCultLevelQuadShot>();
		yield break;
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x00022027 File Offset: 0x00020227
	public void WhaleDeath()
	{
		this.Dead();
		this.rend.sortingLayerName = "Foreground";
		base.animator.Play("WhaleDeath" + this.id);
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000CECCC File Offset: 0x000CCECC
	public void Dead()
	{
		this.StopAllCoroutines();
		this.deathPuff.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.deathPuff.flipX = Rand.Bool();
		this.deathPuff.flipY = Rand.Bool();
		base.animator.Play("Death");
		this.SFX_SNOWCULT_QuadshotMinionDie();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x0002205F File Offset: 0x0002025F
	public void aniEvent_Dead()
	{
		this.Recycle<SnowCultLevelQuadShot>();
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x00022067 File Offset: 0x00020267
	public override void OnDestroy()
	{
		if (Level.Current)
		{
			((SnowCultLevel)Level.Current).OnYetiHitGround -= this.WhaleDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x00022099 File Offset: 0x00020299
	public void SFX_SNOWCULT_QuadshotMinionStuckInGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_stuckinground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_stuckinground");
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000220B5 File Offset: 0x000202B5
	public void SFX_SNOWCULT_QuadshotMinionFlipUp()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_flipup");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_flipup");
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000220D1 File Offset: 0x000202D1
	public void SFX_SNOWCULT_QuadshotMinionDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_death_explode");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_death_explode");
	}

	// Token: 0x040021B9 RID: 8633
	public const float GROUND_Y = -240f;

	// Token: 0x040021BA RID: 8634
	[SerializeField]
	public float popOutWarningTimeNormalized = 0.8f;

	// Token: 0x040021BB RID: 8635
	[SerializeField]
	public Effect sparkEffect;

	// Token: 0x040021BC RID: 8636
	[SerializeField]
	public Effect snowLandEffect;

	// Token: 0x040021BD RID: 8637
	[SerializeField]
	public Effect snowPopOutEffect;

	// Token: 0x040021BE RID: 8638
	[SerializeField]
	public SpriteRenderer deathPuff;

	// Token: 0x040021BF RID: 8639
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x040021C0 RID: 8640
	public LevelProperties.SnowCult.QuadShot properties;

	// Token: 0x040021C1 RID: 8641
	public DamageReceiver damageReceiver;

	// Token: 0x040021C2 RID: 8642
	public float speed;

	// Token: 0x040021C3 RID: 8643
	public float delay;

	// Token: 0x040021C4 RID: 8644
	public float angle;

	// Token: 0x040021C5 RID: 8645
	public string hazardDirectionInstruction;

	// Token: 0x040021C6 RID: 8646
	public int rowPosition;

	// Token: 0x040021C7 RID: 8647
	public float distanceBetween;

	// Token: 0x040021C8 RID: 8648
	public AbstractPlayerController targetPlayer;

	// Token: 0x040021C9 RID: 8649
	public Vector3 startPos;

	// Token: 0x040021CA RID: 8650
	public Vector3 destPos;

	// Token: 0x040021CB RID: 8651
	public float health;

	// Token: 0x040021CC RID: 8652
	public bool isDead;

	// Token: 0x040021CD RID: 8653
	public bool grounded;

	// Token: 0x040021CE RID: 8654
	public bool running;

	// Token: 0x040021CF RID: 8655
	public char id;
}
