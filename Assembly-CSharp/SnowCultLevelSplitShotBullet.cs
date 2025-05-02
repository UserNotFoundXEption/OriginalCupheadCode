using System;
using UnityEngine;

// Token: 0x0200039B RID: 923
public class SnowCultLevelSplitShotBullet : AbstractProjectile
{
	// Token: 0x060028B3 RID: 10419 RVA: 0x000CF5C4 File Offset: 0x000CD7C4
	public virtual SnowCultLevelSplitShotBullet Init(Vector3 pos, float rotation, float speed, int numOfBullets, float spreadAngle, LevelProperties.SnowCult.SplitShot properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.basePos = pos;
		this.rotation = rotation;
		this.speed = speed;
		this.moving = false;
		this.numOfBullets = numOfBullets;
		this.spreadAngle = spreadAngle;
		this.coll = base.GetComponent<Collider2D>();
		this.wobbleTimer = Random.Range(0f, 6.28318548f);
		this.coll.enabled = false;
		this.spawnFX.Play("Spawn" + ((!base.CanParry) ? string.Empty : "Pink"));
		return this;
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000CF674 File Offset: 0x000CD874
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.moving)
		{
			base.transform.position += MathUtils.AngleToDirection(this.rotation) * this.speed * CupheadTime.FixedDelta;
			if (this.coll.enabled && Mathf.Abs(base.transform.position.x) > (float)Level.Current.Right)
			{
				this.middleAngle = ((base.transform.position.x >= 0f) ? 180f : 0f);
				base.transform.localScale = new Vector3(-Mathf.Sign(base.transform.position.x), 1f);
				base.transform.position = new Vector3((float)(Level.Current.Left - 65) * -Mathf.Sign(base.transform.position.x), base.transform.position.y);
				base.animator.Play("BucketExplode");
				this.SFX_SNOWCULT_JackFrostSplitshotBucketImpact();
				this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop();
				this.coll.enabled = false;
				this.SpawnProjectiles();
				this.speed = 0f;
			}
		}
		else
		{
			this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
			base.transform.position = this.basePos + new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Cos(this.wobbleTimer * 2f) * this.wobbleY);
		}
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000CF854 File Offset: 0x000CDA54
	public void Grow()
	{
		this.coll.enabled = true;
		this.startedGrowing = true;
		base.animator.Play("BucketStart" + ((!base.CanParry) ? string.Empty : "Pink"));
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x000CF8A4 File Offset: 0x000CDAA4
	public void Fire()
	{
		this.moving = true;
		base.animator.Play("BucketLoop" + ((!base.CanParry) ? string.Empty : "Pink"));
		this.shootFX.Create(base.transform.position, new Vector3(-Mathf.Sign(base.transform.position.x), 1f));
		this.spawnFX.Play("None");
		this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop();
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x0002231F File Offset: 0x0002051F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.SFX_SNOWCULT_JackFrostSplitshotBucketImpact();
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x00022343 File Offset: 0x00020543
	public void AniEvent_EndExplode()
	{
		this.Recycle<SnowCultLevelSplitShotBullet>();
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x000CF938 File Offset: 0x000CDB38
	public void SpawnProjectiles()
	{
		if (this.bulletsSpawned)
		{
			return;
		}
		this.bulletsSpawned = true;
		float num = this.spreadAngle / Mathf.Round((float)(this.numOfBullets / 2));
		float num2 = this.middleAngle - this.spreadAngle;
		for (int i = 0; i < this.numOfBullets; i++)
		{
			this.shatteredBullet.Create(base.transform.position, num2 + num * (float)i, this.speed);
		}
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000CF9BC File Offset: 0x000CDBBC
	public override void Update()
	{
		base.Update();
		if (this.main.dead != this.dead)
		{
			this.dead = true;
			this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop();
			if (!this.startedGrowing)
			{
				Object.Destroy(base.gameObject);
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("BucketStart" + ((!base.CanParry) ? string.Empty : "Pink")))
			{
				this.spawnFX.Play("None");
				base.animator.Play("BucketStartReverse" + ((!base.CanParry) ? string.Empty : "Pink"), 0, 1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			}
		}
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x0002234B File Offset: 0x0002054B
	public void SFX_SNOWCULT_JackFrostSplitshotBucketImpact()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact");
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x00022367 File Offset: 0x00020567
	public void SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x00022383 File Offset: 0x00020583
	public void SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
	}

	// Token: 0x040021F3 RID: 8691
	[SerializeField]
	public SnowCultLevelSplitShotBulletShattered shatteredBullet;

	// Token: 0x040021F4 RID: 8692
	[SerializeField]
	public Effect shootFX;

	// Token: 0x040021F5 RID: 8693
	[SerializeField]
	public Animator spawnFX;

	// Token: 0x040021F6 RID: 8694
	public float middleAngle;

	// Token: 0x040021F7 RID: 8695
	public float spreadAngle;

	// Token: 0x040021F8 RID: 8696
	public float rotation;

	// Token: 0x040021F9 RID: 8697
	public float speed;

	// Token: 0x040021FA RID: 8698
	public bool moving;

	// Token: 0x040021FB RID: 8699
	public int numOfBullets;

	// Token: 0x040021FC RID: 8700
	public bool bulletsSpawned;

	// Token: 0x040021FD RID: 8701
	public Collider2D coll;

	// Token: 0x040021FE RID: 8702
	public Vector3 basePos;

	// Token: 0x040021FF RID: 8703
	public float wobbleTimer;

	// Token: 0x04002200 RID: 8704
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x04002201 RID: 8705
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x04002202 RID: 8706
	[SerializeField]
	public float wobbleSpeed = 1f;

	// Token: 0x04002203 RID: 8707
	public SnowCultLevelJackFrost main;

	// Token: 0x04002204 RID: 8708
	public new bool dead;

	// Token: 0x04002205 RID: 8709
	public bool startedGrowing;
}
