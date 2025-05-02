using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class SnowCultLevelIcePillar : AbstractProjectile
{
	// Token: 0x0600283E RID: 10302 RVA: 0x000CDFA8 File Offset: 0x000CC1A8
	public virtual SnowCultLevelIcePillar Init(Vector3 pos, LevelProperties.SnowCult.IcePillar properties, bool typeToPlay, float timeToDelay)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.typeString = ((!typeToPlay) ? "B" : "A");
		base.animator.Play("IceBlade_Start" + this.typeString);
		this.timeToDelay = timeToDelay;
		this.outTime = properties.outTime;
		this.Attack();
		return this;
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x00021C7C File Offset: 0x0001FE7C
	public void Attack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x000CE020 File Offset: 0x000CC220
	public IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.timeToDelay);
		base.animator.SetTrigger("popUp");
		this.SFX_SNOWCULT_BladeStabfromGround();
		yield break;
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x00021C8B File Offset: 0x0001FE8B
	public void WaitAndRetract()
	{
		base.StartCoroutine(this.waitandretract_cr());
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x000CE03C File Offset: 0x000CC23C
	public IEnumerator waitandretract_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.outTime);
		base.animator.SetTrigger("popDown");
		yield break;
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x00021C9A File Offset: 0x0001FE9A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x00021CB8 File Offset: 0x0001FEB8
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x00021CC0 File Offset: 0x0001FEC0
	public void WarningSmokeFX()
	{
		this.warningSmoke.Create(base.transform.position);
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x00021CD9 File Offset: 0x0001FED9
	public void SFX_SNOWCULT_BladeStabfromGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground");
	}

	// Token: 0x04002177 RID: 8567
	public const float Y_POS_START = -430f;

	// Token: 0x04002178 RID: 8568
	public const float Y_POS_END = -200f;

	// Token: 0x04002179 RID: 8569
	public string typeString;

	// Token: 0x0400217A RID: 8570
	public float timeToDelay;

	// Token: 0x0400217B RID: 8571
	public float outTime;

	// Token: 0x0400217C RID: 8572
	[SerializeField]
	public Effect warningSmoke;
}
