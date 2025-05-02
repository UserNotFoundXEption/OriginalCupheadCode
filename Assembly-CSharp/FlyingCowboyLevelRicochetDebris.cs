using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200025B RID: 603
public class FlyingCowboyLevelRicochetDebris : BasicUprightProjectile
{
	// Token: 0x06001BC2 RID: 7106 RVA: 0x000AC9D8 File Offset: 0x000AABD8
	public virtual BasicProjectile Create(Vector3 position, float speed, float bulletSpeed, FlyingCowboyLevelRicochetDebris.BulletType bulletType, bool bulletParryable)
	{
		FlyingCowboyLevelRicochetDebris flyingCowboyLevelRicochetDebris = this.Create(position, MathUtils.DirectionToAngle(Vector3.down), speed) as FlyingCowboyLevelRicochetDebris;
		flyingCowboyLevelRicochetDebris.bulletType = bulletType;
		flyingCowboyLevelRicochetDebris.bulletSpeed = bulletSpeed;
		flyingCowboyLevelRicochetDebris.bulletParryable = bulletParryable;
		return flyingCowboyLevelRicochetDebris;
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000ACA20 File Offset: 0x000AAC20
	public override void Start()
	{
		base.Start();
		base.animator.Update(0f);
		base.animator.Play(0, 0, Random.Range(0f, 1f));
		Vector3 localScale = base.transform.localScale;
		localScale.x *= (float)Rand.PosOrNeg();
		base.transform.localScale = localScale;
		if (base.animator.GetInteger(AbstractProjectile.Variant) == 0)
		{
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(FlyingCowboyLevelRicochetDebris.AllowedRotations.GetRandom<float>()));
		}
		base.StartCoroutine(this.fall_cr());
		base.StartCoroutine(this.shadowScale_cr());
		base.StartCoroutine(this.shadowPosition_cr());
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x0001785A File Offset: 0x00015A5A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer();
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x000ACAF8 File Offset: 0x000AACF8
	public IEnumerator fall_cr()
	{
		while (base.transform.position.y > -360f + FlyingCowboyLevelRicochetDebris.GroundOffset)
		{
			yield return null;
		}
		FlyingCowboyLevelRicochetDebris.BulletType bulletType = this.bulletType;
		if (bulletType != FlyingCowboyLevelRicochetDebris.BulletType.Nothing)
		{
			if (bulletType == FlyingCowboyLevelRicochetDebris.BulletType.Ricochet)
			{
				this.shootRicochetProjectile();
			}
		}
		this.SFX_COWGIRL_COWGIRL_P2_SafeDropImpact();
		this.Die();
		yield break;
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000ACB14 File Offset: 0x000AAD14
	public IEnumerator shadowScale_cr()
	{
		this.shadowTransform.rotation = Quaternion.identity;
		float ground = -360f + FlyingCowboyLevelRicochetDebris.GroundOffset;
		while (base.transform.position.y > ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset)
		{
			yield return null;
		}
		float startY = ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset;
		float endY = ground + FlyingCowboyLevelRicochetDebris.ShadowEndOffset;
		base.animator.Play("On", 1);
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		while (!base.dead)
		{
			float parentScale = base.transform.localScale.x;
			Vector3 scale = this.shadowTransform.localScale;
			scale.x = (scale.y = MathUtilities.LerpMapping(base.transform.position.y, startY, endY, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.min, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.max, true) / parentScale);
			this.shadowTransform.localScale = scale;
			yield return wait;
		}
		base.animator.Play("Off", 1);
		yield break;
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000ACB30 File Offset: 0x000AAD30
	public IEnumerator shadowPosition_cr()
	{
		float ground = -360f + FlyingCowboyLevelRicochetDebris.GroundOffset;
		while (!base.dead)
		{
			Vector3 position = this.shadowTransform.position;
			position.y = ground + FlyingCowboyLevelRicochetDebris.ShadowPositionOffset;
			this.shadowTransform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000ACB4C File Offset: 0x000AAD4C
	public void shootRicochetProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float rotation = MathUtils.DirectionToAngle(next.transform.position - base.transform.position);
		int num;
		BasicProjectile basicProjectile;
		if (this.bulletParryable)
		{
			num = Random.Range(0, this.parryableProjectiles.Length);
			basicProjectile = this.parryableProjectiles[num].Create(base.transform.position, rotation, this.bulletSpeed);
			num++;
		}
		else
		{
			num = Random.Range(0, this.regularProjectiles.Length);
			basicProjectile = this.regularProjectiles[num].Create(base.transform.position, rotation, this.bulletSpeed);
		}
		basicProjectile.SetParryable(this.bulletParryable);
		basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = num;
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000ACC1C File Offset: 0x000AAE1C
	public override void Die()
	{
		this.RandomizeVariant();
		base.Die();
		bool flag = Rand.Bool();
		int num;
		if (FlyingCowboyLevelRicochetDebris.LastBitsIndex == 0)
		{
			num = ((!flag) ? 2 : 1);
		}
		else if (FlyingCowboyLevelRicochetDebris.LastBitsIndex == 1)
		{
			num = ((!flag) ? 2 : 0);
		}
		else
		{
			num = ((!flag) ? 1 : 0);
		}
		FlyingCowboyLevelRicochetDebris.LastBitsIndex = num;
		for (int i = 0; i < this.deathBits.Length; i++)
		{
			this.deathBits[i].enabled = (i == num);
		}
		this.deathEffect.Create(new Vector3(base.transform.position.x, -360f + FlyingCowboyLevelRicochetDebris.GroundOffset - 10f));
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x0001786A File Offset: 0x00015A6A
	public void SFX_COWGIRL_COWGIRL_P2_SafeDropImpact()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_safedropimpact");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safedropimpact");
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x00017886 File Offset: 0x00015A86
	public void SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_safehitplayer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safehitplayer");
	}

	// Token: 0x04001691 RID: 5777
	public static readonly float GroundOffset = 100f;

	// Token: 0x04001692 RID: 5778
	public static readonly float ShadowPositionOffset = -50f;

	// Token: 0x04001693 RID: 5779
	public static readonly float ShadowStartOffset = 300f;

	// Token: 0x04001694 RID: 5780
	public static readonly float ShadowEndOffset = 50f;

	// Token: 0x04001695 RID: 5781
	public static readonly MinMax ShadowScaleRange = new MinMax(0.1f, 1f);

	// Token: 0x04001696 RID: 5782
	public static readonly float[] AllowedRotations = new float[]
	{
		-20f,
		-10f,
		0f,
		10f,
		20f
	};

	// Token: 0x04001697 RID: 5783
	public static int LastBitsIndex = 0;

	// Token: 0x04001698 RID: 5784
	[SerializeField]
	public SpriteRenderer[] deathBits;

	// Token: 0x04001699 RID: 5785
	[SerializeField]
	public BasicProjectile[] regularProjectiles;

	// Token: 0x0400169A RID: 5786
	[SerializeField]
	public BasicProjectile[] parryableProjectiles;

	// Token: 0x0400169B RID: 5787
	[SerializeField]
	public Transform shadowTransform;

	// Token: 0x0400169C RID: 5788
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x0400169D RID: 5789
	public FlyingCowboyLevelRicochetDebris.BulletType bulletType;

	// Token: 0x0400169E RID: 5790
	public float bulletSpeed;

	// Token: 0x0400169F RID: 5791
	public bool bulletParryable;

	// Token: 0x02000CDB RID: 3291
	public enum BulletType
	{
		// Token: 0x04005D27 RID: 23847
		Nothing,
		// Token: 0x04005D28 RID: 23848
		Ricochet
	}
}
