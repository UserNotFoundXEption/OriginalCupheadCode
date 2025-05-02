using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class SnowCultLevelPeashot : BasicProjectile
{
	// Token: 0x06002870 RID: 10352 RVA: 0x00021F29 File Offset: 0x00020129
	public override void Start()
	{
		base.Start();
		this.SFX_SNOWCULT_TarotCardTravelLoop();
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000CE570 File Offset: 0x000CC770
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			int num = Random.Range(0, 2);
			if (num != 0)
			{
				if (num == 1)
				{
					base.animator.Play("SunPink", 0, 0.25f);
				}
			}
			else
			{
				base.animator.Play("SwordPink", 0, 0.25f);
			}
		}
		else
		{
			int num2 = Random.Range(0, 3);
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						base.animator.Play("Moon", 0, 0.25f);
					}
				}
				else
				{
					base.animator.Play("Sun", 0, 0.25f);
				}
			}
			else
			{
				base.animator.Play("Sword", 0, 0.25f);
			}
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x000CE664 File Offset: 0x000CC864
	public IEnumerator dead_cr()
	{
		this.Speed = 0f;
		this.move = false;
		this.boxCollider.enabled = false;
		this.SFX_SNOWCULT_TarotCardHitGround();
		switch ((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f * 10f))
		{
		case 0:
		case 5:
			base.animator.Play("Die");
			break;
		case 1:
		case 6:
			base.animator.Play("DieAngleB");
			base.GetComponent<SpriteRenderer>().flipX = true;
			break;
		case 2:
		case 7:
			base.animator.Play("DieAngleA");
			base.GetComponent<SpriteRenderer>().flipX = true;
			break;
		case 3:
		case 8:
			base.animator.Play("DieAngleA");
			break;
		case 4:
		case 9:
			base.animator.Play("DieAngleB");
			break;
		}
		base.animator.Update(0f);
		Vector3 impactPos = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
		{
			Vector3 offset = MathUtils.AngleToDirection((float)Random.Range(0, 360)) * base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 250f;
			offset.y *= 0.3f;
			this.sparkleEffect.Create(impactPos + offset);
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.005f, 0.01f));
		}
		this.Die();
		yield break;
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x000CE680 File Offset: 0x000CC880
	public override void Move()
	{
		base.transform.position += base.transform.up * -this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		if (base.transform.position.y <= (float)Level.Current.Ground + 15f || base.transform.position.x < (float)Level.Current.Left || base.transform.position.x > (float)Level.Current.Right)
		{
			base.StartCoroutine(this.dead_cr());
		}
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x00021F37 File Offset: 0x00020137
	public override void OnParryDie()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		base.OnParryDie();
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x00021F49 File Offset: 0x00020149
	public void SFX_SNOWCULT_TarotCardTravelLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x00021F65 File Offset: 0x00020165
	public void SFX_SNOWCULT_TarotCardHitGround()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground");
	}

	// Token: 0x040021A5 RID: 8613
	public const float GROUND_OFFSET = 15f;

	// Token: 0x040021A6 RID: 8614
	[SerializeField]
	public BoxCollider2D boxCollider;

	// Token: 0x040021A7 RID: 8615
	[SerializeField]
	public Effect sparkleEffect;
}
