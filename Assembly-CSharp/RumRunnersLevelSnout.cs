using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000350 RID: 848
public class RumRunnersLevelSnout : AbstractCollidableObject
{
	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06002523 RID: 9507 RVA: 0x0001F502 File Offset: 0x0001D702
	// (set) Token: 0x06002524 RID: 9508 RVA: 0x0001F50A File Offset: 0x0001D70A
	public bool isAttacking { get; set; }

	// Token: 0x06002525 RID: 9509 RVA: 0x000C5AF0 File Offset: 0x000C3CF0
	public void Start()
	{
		foreach (DamageReceiver damageReceiver in this.damageReceivers)
		{
			damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		this.snoutScale = base.transform.localScale;
		base.transform.position = RumRunnersLevelSnout.OffscreenCoord;
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x0001F513 File Offset: 0x0001D713
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.DoDamage(info.damage);
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x0001F526 File Offset: 0x0001D726
	public void Setup(LevelProperties.RumRunners properties)
	{
		this.properties = properties;
		this.copBallLaunchAnglePattern = new PatternString(properties.CurrentState.copBall.copBallLaunchAngleString, true, true);
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000C5B54 File Offset: 0x000C3D54
	public void Attack(Vector3 position, Vector2 shadowPosition, bool onLeft, RumRunnersLevelSnout.AttackType attackType)
	{
		Vector3 position2 = position;
		position2.x = (float)((!onLeft) ? Level.Current.Right : Level.Current.Left);
		Vector3 one = Vector3.one;
		one.x *= (float)((!onLeft) ? -1 : 1);
		this.dirtEffect.Create(position2, one);
		base.transform.position = position;
		this.shadowTransform.localPosition = shadowPosition;
		base.StartCoroutine(this.attack_cr(onLeft, attackType));
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000C5BE8 File Offset: 0x000C3DE8
	public IEnumerator attack_cr(bool onLeft, RumRunnersLevelSnout.AttackType attackType)
	{
		LevelProperties.RumRunners.AnteaterSnout p = this.properties.CurrentState.anteaterSnout;
		this.onLeft = onLeft;
		this.endNormal = (this.endTongue = false);
		base.transform.SetScale(new float?((!onLeft) ? (-this.snoutScale.x) : this.snoutScale.x), new float?(this.snoutScale.y), null);
		this.parent.SetEyeSide(onLeft);
		base.animator.SetBool("Fake", attackType == RumRunnersLevelSnout.AttackType.Fake);
		base.animator.SetBool("Tongue", attackType == RumRunnersLevelSnout.AttackType.Tongue);
		base.animator.SetTrigger("Attack");
		this.isAttacking = true;
		if (attackType == RumRunnersLevelSnout.AttackType.Fake || attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			float fullOutBoilDelay = p.snoutFullOutBoilDelay;
			if (fullOutBoilDelay > 0f)
			{
				yield return base.animator.WaitForAnimationToStart(this, "FullOutHold", false);
				yield return CupheadTime.WaitForSeconds(this, fullOutBoilDelay);
			}
			base.animator.SetTrigger("HoldComplete");
		}
		if (attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			yield return base.animator.WaitForAnimationToStart(this, "TongueHold", false);
			yield return CupheadTime.WaitForSeconds(this, p.tongueHoldDuration);
			base.animator.SetBool("Tongue", false);
			while (!this.endTongue)
			{
				yield return null;
			}
		}
		else
		{
			while (!this.endNormal)
			{
				yield return null;
			}
		}
		this.isAttacking = false;
		if (attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Off", false);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "QuickEnd", false, true);
		}
		base.transform.position = RumRunnersLevelSnout.OffscreenCoord;
		yield break;
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x0001F54C File Offset: 0x0001D74C
	public void animationEvent_EndNormalAttack()
	{
		this.endNormal = true;
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x0001F555 File Offset: 0x0001D755
	public void animationEvent_TriggerTongueEyes()
	{
		this.parent.TriggerEyesTurnaround();
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000C5C14 File Offset: 0x000C3E14
	public void animationEvent_EndFakeTongueAttack()
	{
		Effect effect = this.fakeTongueSpittleEffect.Create(this.tonguePokeFXTransform.position);
		if (!this.onLeft)
		{
			Vector3 localScale = effect.transform.localScale;
			localScale.x *= -1f;
			effect.transform.localScale = localScale;
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x0001F562 File Offset: 0x0001D762
	public void animationEvent_EndTongueAttack()
	{
		this.endTongue = true;
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000C5C70 File Offset: 0x000C3E70
	public void animationEvent_FlipIfNecessary()
	{
		float num = this.copBallLaunchAnglePattern.PopFloat();
		base.animator.SetBool("ThrowDown", num < 0f);
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000C5CA4 File Offset: 0x000C3EA4
	public void animationEvent_SpawnCopBall()
	{
		LevelProperties.RumRunners.CopBall copBall = this.properties.CurrentState.copBall;
		do
		{
			this.copBallList.RemoveAll((RumRunnersLevelCopBall b) => b == null || b.leaveScreen);
			if (this.copBallList.Count >= copBall.copBallMaxCount)
			{
				this.copBallList[0].leaveScreen = true;
			}
		}
		while (this.copBallList.Count >= copBall.copBallMaxCount);
		float @float = this.copBallLaunchAnglePattern.GetFloat();
		RumRunnersLevelCopBall rumRunnersLevelCopBall = this.copBallPrefab.Spawn<RumRunnersLevelCopBall>();
		float angle = (!this.onLeft) ? (180f - @float) : @float;
		rumRunnersLevelCopBall.Init(this.copballLaunchOrigin.position, MathUtils.AngleToDirection(angle), copBall.copBallSpeed, copBall.copBallHP, copBall, this.copballLaunchOrigin);
		this.copBallList.Add(rumRunnersLevelCopBall);
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000C5D98 File Offset: 0x000C3F98
	public void animationEvent_FireCopBall()
	{
		if (this.copBallList[this.copBallList.Count - 1] != null)
		{
			this.copBallList[this.copBallList.Count - 1].Launch();
		}
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000C5DE8 File Offset: 0x000C3FE8
	public void Death()
	{
		this.StopAllCoroutines();
		foreach (RumRunnersLevelCopBall rumRunnersLevelCopBall in this.copBallList)
		{
			if (rumRunnersLevelCopBall != null)
			{
				rumRunnersLevelCopBall.Death(false);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000C5E64 File Offset: 0x000C4064
	public void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Enter()
	{
		if (base.animator.GetBool("Tongue"))
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold");
		}
		else
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_enter");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_enter");
		}
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x0001F56B File Offset: 0x0001D76B
	public void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Tongue()
	{
		if (base.animator.GetBool("Tongue"))
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_attack");
		}
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x0001F591 File Offset: 0x0001D791
	public void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_ShortExit()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_exit");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_exit");
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x0001F5AD File Offset: 0x0001D7AD
	public void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_SpitBallCop()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop");
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x0001F5C9 File Offset: 0x0001D7C9
	public void AnimationEvent_SFX_RUMRUN_P3_BallCop_SpitVocalShouts()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts");
	}

	// Token: 0x04001EBC RID: 7868
	public static readonly Vector3 OffscreenCoord = new Vector3(0f, 1500f);

	// Token: 0x04001EBD RID: 7869
	[SerializeField]
	public Transform copballLaunchOrigin;

	// Token: 0x04001EBE RID: 7870
	[SerializeField]
	public RumRunnersLevelCopBall copBallPrefab;

	// Token: 0x04001EBF RID: 7871
	[SerializeField]
	public Effect dirtEffect;

	// Token: 0x04001EC0 RID: 7872
	[SerializeField]
	public RumRunnersLevelAnteater parent;

	// Token: 0x04001EC1 RID: 7873
	[SerializeField]
	public Transform shadowTransform;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	public DamageReceiver[] damageReceivers;

	// Token: 0x04001EC3 RID: 7875
	[SerializeField]
	public Transform tonguePokeFXTransform;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	public Effect fakeTongueSpittleEffect;

	// Token: 0x04001EC6 RID: 7878
	public LevelProperties.RumRunners properties;

	// Token: 0x04001EC7 RID: 7879
	public Vector2 snoutScale;

	// Token: 0x04001EC8 RID: 7880
	public List<RumRunnersLevelCopBall> copBallList = new List<RumRunnersLevelCopBall>();

	// Token: 0x04001EC9 RID: 7881
	public bool onLeft;

	// Token: 0x04001ECA RID: 7882
	public bool endNormal;

	// Token: 0x04001ECB RID: 7883
	public bool endTongue;

	// Token: 0x04001ECC RID: 7884
	public PatternString copBallLaunchAnglePattern;

	// Token: 0x02000EC3 RID: 3779
	public enum AttackType
	{
		// Token: 0x04006A24 RID: 27172
		Quick,
		// Token: 0x04006A25 RID: 27173
		Fake,
		// Token: 0x04006A26 RID: 27174
		Tongue
	}
}
