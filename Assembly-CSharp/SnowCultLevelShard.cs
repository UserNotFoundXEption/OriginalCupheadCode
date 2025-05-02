using System;
using UnityEngine;

// Token: 0x02000397 RID: 919
public class SnowCultLevelShard : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x06002892 RID: 10386 RVA: 0x000CED4C File Offset: 0x000CCF4C
	public virtual SnowCultLevelShard Init(Vector3 pivotPos, float angle, float loopSizeX, float loopSizeY, LevelProperties.SnowCult.ShardAttack properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.pivotPos = pivotPos;
		this.speed = properties.shardSpeed;
		this.Health = properties.shardHealth;
		base.GetComponent<Collider2D>().enabled = false;
		angle *= 0.0174532924f;
		base.transform.position = pivotPos + new Vector3(-Mathf.Sin(angle) * loopSizeX, Mathf.Cos(angle) * loopSizeY);
		base.transform.SetEulerAngles(null, null, new float?(90f + MathUtils.DirectionToAngle(pivotPos - base.transform.position)));
		this.basePos = base.transform.position;
		this.SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop();
		return this;
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x00022116 File Offset: 0x00020316
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f)
		{
			this.Recycle<SnowCultLevelShard>();
		}
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x00022141 File Offset: 0x00020341
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x0002215F File Offset: 0x0002035F
	public void Appear()
	{
		base.animator.SetTrigger("Appear");
		this.SFX_SNOWCULT_JackFrostIcecreamAppear();
		this.smoke.Create(base.transform.position);
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x0002219A File Offset: 0x0002039A
	public void LaunchProjectile()
	{
		base.animator.SetTrigger("StartMove");
		this.moving = true;
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000CEE24 File Offset: 0x000CD024
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.moving)
		{
			base.transform.position = this.basePos + Vector3.right * Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleX + Vector3.up * Mathf.Cos(this.wobbleTimer * 3f) * this.wobbleY;
			this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
		}
		else
		{
			base.transform.position += base.transform.up * -this.speed * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000221B3 File Offset: 0x000203B3
	public void SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000221CF File Offset: 0x000203CF
	public void SFX_SNOWCULT_JackFrostIcecreamAppear()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear");
	}

	// Token: 0x040021D0 RID: 8656
	public Vector3 basePos;

	// Token: 0x040021D1 RID: 8657
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x040021D2 RID: 8658
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x040021D3 RID: 8659
	[SerializeField]
	public float wobbleSpeed = 2f;

	// Token: 0x040021D4 RID: 8660
	public float wobbleTimer;

	// Token: 0x040021D5 RID: 8661
	public float speed;

	// Token: 0x040021D6 RID: 8662
	public float Health;

	// Token: 0x040021D7 RID: 8663
	public bool moving;

	// Token: 0x040021D8 RID: 8664
	public Vector2 pivotPos;

	// Token: 0x040021D9 RID: 8665
	public DamageReceiver damageReceiver;

	// Token: 0x040021DA RID: 8666
	[SerializeField]
	public Effect smoke;
}
