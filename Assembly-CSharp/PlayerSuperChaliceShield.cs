using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public class PlayerSuperChaliceShield : AbstractPlayerSuper
{
	// Token: 0x060037C9 RID: 14281 RVA: 0x0002D896 File Offset: 0x0002BA96
	public override void Awake()
	{
		base.Awake();
		base.tag = "Untagged";
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x00104E80 File Offset: 0x00103080
	public override void StartSuper()
	{
		this.player.weaponManager.OnSuperStart -= this.player.motor.StartSuper;
		if (this.player.motor.Grounded)
		{
			this.player.weaponManager.OnSuperEnd -= this.player.motor.OnSuperEnd;
		}
		base.StartSuper();
		AudioManager.Play("player_super_chalice_shield");
		base.StartCoroutine(this.super_cr());
		Level.ScoringData.superMeterUsed += 5;
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x00104F20 File Offset: 0x00103120
	public void CreateHeart()
	{
		this.shieldHeart = Object.Instantiate<GameObject>(this.shieldHeartPrefab);
		this.shieldHeart.transform.position = this.shieldHeartSpawnPos.position;
		this.shieldHeartScript = this.shieldHeart.GetComponent<PlayerSuperChaliceShieldHeart>();
		this.shieldHeartScript.player = this.player.transform;
		this.player.stats.SetChaliceShield(true);
		this.player.damageReceiver.Invulnerable(0.1f);
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0002D8A9 File Offset: 0x0002BAA9
	public void LetPlayerMove()
	{
		this.Fire();
		this.EndSuper(true);
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x00104FA8 File Offset: 0x001031A8
	public IEnumerator super_cr()
	{
		if (!this.player.motor.Grounded)
		{
			base.animator.Play("SuperAir");
		}
		while (this.player && !this.player.stats.ChaliceShieldOn)
		{
			yield return null;
		}
		while (this.player && this.player.stats.ChaliceShieldOn)
		{
			yield return null;
		}
		this.shieldHeartScript.Destroy();
		if (this.player)
		{
			this.player.damageReceiver.OnRevive(Vector3.zero);
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002CF5 RID: 11509
	[SerializeField]
	public Vector3 shadowOffset;

	// Token: 0x04002CF6 RID: 11510
	[SerializeField]
	public GameObject shieldHeartPrefab;

	// Token: 0x04002CF7 RID: 11511
	public GameObject shieldHeart;

	// Token: 0x04002CF8 RID: 11512
	[SerializeField]
	public Transform shieldHeartSpawnPos;

	// Token: 0x04002CF9 RID: 11513
	public PlayerSuperChaliceShieldHeart shieldHeartScript;
}
