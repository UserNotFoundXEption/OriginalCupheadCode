using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class SallyStagePlayLevelFianceDeity : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x060025F8 RID: 9720 RVA: 0x0001FD85 File Offset: 0x0001DF85
	public void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x0001FDB6 File Offset: 0x0001DFB6
	public void Attack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x0001FDC5 File Offset: 0x0001DFC5
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (SallyStagePlayLevelAngel.extraHP > 0f)
		{
			SallyStagePlayLevelAngel.extraHP -= info.damage;
		}
		else
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000C80FC File Offset: 0x000C62FC
	public IEnumerator attack_cr()
	{
		LevelProperties.SallyStagePlay.Husband p = base.properties.CurrentState.husband;
		while (!this.isDead)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelayRange.RandomFloat());
			base.GetComponent<Animator>().SetBool("OnAttack", true);
			yield return base.GetComponent<Animator>().WaitForAnimationToEnd(this, "Puppet_Attack_Start", false, true);
			this.cherubProjectile.Create(this.husbandRoot.position, 0f, p.shotSpeed);
			base.GetComponent<Animator>().SetBool("OnAttack", false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x0001FDFD File Offset: 0x0001DFFD
	public void Dead()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		this.damageReceiver.enabled = false;
		base.GetComponent<Animator>().SetTrigger("OnDeath");
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x000C8118 File Offset: 0x000C6318
	public IEnumerator move_cr()
	{
		float t = 0f;
		float time = 3f;
		Vector3 endPos = new Vector3(-1140f, base.transform.position.y);
		Vector2 start = base.transform.position;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 0.8f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		base.GetComponent<Collider2D>().enabled = false;
		yield break;
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x0001FE35 File Offset: 0x0001E035
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.cherubProjectile = null;
	}

	// Token: 0x04001F77 RID: 8055
	[SerializeField]
	public SallyStagePlayLevelCherubProjectile cherubProjectile;

	// Token: 0x04001F78 RID: 8056
	[SerializeField]
	public Transform husbandRoot;

	// Token: 0x04001F79 RID: 8057
	public bool isDead;

	// Token: 0x04001F7A RID: 8058
	public float health;

	// Token: 0x04001F7B RID: 8059
	public DamageReceiver damageReceiver;
}
