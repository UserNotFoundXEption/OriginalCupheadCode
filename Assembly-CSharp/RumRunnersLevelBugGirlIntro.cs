using System;
using UnityEngine;

// Token: 0x0200033E RID: 830
public class RumRunnersLevelBugGirlIntro : MonoBehaviour
{
	// Token: 0x06002460 RID: 9312 RVA: 0x0001EB99 File Offset: 0x0001CD99
	public void OnEnable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x0001EBB2 File Offset: 0x0001CDB2
	public void OnDisable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken -= this.OnDamageTaken;
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x0001EBCB File Offset: 0x0001CDCB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.introAnimation.bugGirlDamage += info.damage;
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x0001EBE5 File Offset: 0x0001CDE5
	public void animationEvent_BugWalkBegin()
	{
		this.introAnimation.StartBugWalk();
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x0001EBF2 File Offset: 0x0001CDF2
	public void animationEvent_BugTauntBegin()
	{
		this.introAnimation.StopBugWalk();
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x0001EBFF File Offset: 0x0001CDFF
	public void animationEvent_TauntBump()
	{
		this.introAnimation.BarrelExit();
	}

	// Token: 0x04001E1E RID: 7710
	[SerializeField]
	public RumRunnersLevelMobIntroAnimation introAnimation;
}
