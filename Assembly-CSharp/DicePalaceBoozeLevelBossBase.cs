using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class DicePalaceBoozeLevelBossBase : LevelProperties.DicePalaceBooze.Entity
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001580 RID: 5504 RVA: 0x000124C5 File Offset: 0x000106C5
	// (set) Token: 0x06001581 RID: 5505 RVA: 0x000124CD File Offset: 0x000106CD
	public bool isDead { get; set; }

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001582 RID: 5506 RVA: 0x000124D6 File Offset: 0x000106D6
	// (set) Token: 0x06001583 RID: 5507 RVA: 0x000124DD File Offset: 0x000106DD
	public static int DEATH_COUNTER { get; set; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x000124E5 File Offset: 0x000106E5
	// (set) Token: 0x06001585 RID: 5509 RVA: 0x000124EC File Offset: 0x000106EC
	public static float ATTACK_DELAY { get; set; }

	// Token: 0x06001586 RID: 5510 RVA: 0x000124F4 File Offset: 0x000106F4
	public void Start()
	{
		this.isDead = false;
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER = 0;
		DicePalaceBoozeLevelBossBase.ATTACK_DELAY = 0f;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x0001250D File Offset: 0x0001070D
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x0009C244 File Offset: 0x0009A444
	public virtual void StartDying()
	{
		this.isDead = true;
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER++;
		if (DicePalaceBoozeLevelBossBase.DEATH_COUNTER >= 3)
		{
			this.AllDead();
		}
		else
		{
			DicePalaceBoozeLevelBossBase.ATTACK_DELAY += base.properties.CurrentState.main.delaySubstractAmount;
			this.Dying();
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00012516 File Offset: 0x00010716
	public void Dying()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x0009C2A0 File Offset: 0x0009A4A0
	public IEnumerator dying_cr()
	{
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<DamageReceiver>().enabled = false;
		Object.Destroy(base.GetComponent<Rigidbody2D>());
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield return null;
		yield break;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x0001252B File Offset: 0x0001072B
	public void AllDead()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0001254E File Offset: 0x0001074E
	public virtual void HandleDead()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04001183 RID: 4483
	public float health;
}
