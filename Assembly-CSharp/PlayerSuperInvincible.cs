using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class PlayerSuperInvincible : AbstractPlayerSuper
{
	// Token: 0x060037ED RID: 14317 RVA: 0x0010589C File Offset: 0x00103A9C
	public override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_invincibility");
		if (this.player.id == PlayerId.PlayerOne)
		{
			this.shadowMugman.SetActive(false);
			this.shadow = this.shadowCuphead;
		}
		else
		{
			this.shadowCuphead.SetActive(false);
			this.shadow = this.shadowMugman;
		}
		base.transform.position = this.player.transform.position;
		base.StartCoroutine(this.super_cr());
		if (!this.player.motor.Grounded)
		{
			this.shadow.SetActive(false);
			this.shadow.transform.position = this.player.GetComponent<LevelPlayerShadow>().ShadowPosition() + this.shadowOffset;
		}
		Level.ScoringData.superMeterUsed += 5;
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x00105984 File Offset: 0x00103B84
	public override void Interrupt()
	{
		this.StopAllCoroutines();
		AudioManager.ChangeBGMPitch(1f, 1.5f);
		if (this.player != null)
		{
			this.player.animationController.SetOldMaterial();
			this.player.stats.SetInvincible(false);
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x001059D8 File Offset: 0x00103BD8
	public IEnumerator super_cr()
	{
		if (this.player != null)
		{
			this.player.stats.SetInvincible(true);
		}
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperInvincibility.durationInvincible);
		if (this.player != null)
		{
			this.player.stats.SetInvincible(false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x001059F4 File Offset: 0x00103BF4
	public IEnumerator invincibility_fx_cr()
	{
		IEnumerator sparkleRoutine = this.sparkle_cr();
		base.StartCoroutine(sparkleRoutine);
		if (this.player != null)
		{
			this.player.animationController.SetMaterial(this.superMaterial);
		}
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperInvincibility.durationFX - 1.25f);
		AudioManager.ChangeBGMPitch(1.8f, 1.5f);
		for (int i = 0; i < 5; i++)
		{
			if (this.player != null)
			{
				this.player.animationController.SetOldMaterial();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
			if (this.player != null)
			{
				this.player.animationController.SetMaterial(this.superMaterial);
			}
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
		}
		AudioManager.ChangeBGMPitch(1f, 1.5f);
		if (this.player != null)
		{
			this.player.animationController.SetOldMaterial();
		}
		base.StopCoroutine(sparkleRoutine);
		yield return null;
		yield break;
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x00105A10 File Offset: 0x00103C10
	public IEnumerator sparkle_cr()
	{
		while (true && this.player != null)
		{
			float x = Random.Range(-this.player.colliderManager.Width, this.player.colliderManager.Width);
			float y = Random.Range(this.player.colliderManager.Height * -0.5f, this.player.colliderManager.Height * 1.5f);
			this.sparkle.Create(this.player.transform.position + new Vector3(x, y, 0f));
			yield return CupheadTime.WaitForSeconds(this, this.sparkleSpawnTime);
		}
		yield break;
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x00105A2C File Offset: 0x00103C2C
	public void EndPlayerAnimation()
	{
		this.Fire();
		this.EndSuper(false);
		base.StartCoroutine(this.invincibility_fx_cr());
		base.StartCoroutine(this.super_cr());
		if (this.player != null)
		{
			this.player.animationController.SetSpriteProperties(SpriteLayer.Effects, 3000);
		}
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x00105A88 File Offset: 0x00103C88
	public void BigCupAppears()
	{
		if (!this.player.motor.Grounded)
		{
			this.shadow.SetActive(true);
			float num = Mathf.Abs(this.player.transform.position.y - this.shadow.transform.position.y);
			float num2 = Mathf.Max(0f, 1f - num / 500f);
			this.shadow.transform.localScale = Vector3.one * num2;
		}
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x0002DA3B File Offset: 0x0002BC3B
	public void ResetSpriteOrder()
	{
		if (this.player != null)
		{
			this.player.animationController.ResetSpriteProperties();
		}
	}

	// Token: 0x04002D11 RID: 11537
	public const float maxShadowDistance = 500f;

	// Token: 0x04002D12 RID: 11538
	[SerializeField]
	public Material superMaterial;

	// Token: 0x04002D13 RID: 11539
	[SerializeField]
	public Effect sparkle;

	// Token: 0x04002D14 RID: 11540
	[SerializeField]
	public float sparkleSpawnTime;

	// Token: 0x04002D15 RID: 11541
	[SerializeField]
	public Vector3 shadowOffset;

	// Token: 0x04002D16 RID: 11542
	[SerializeField]
	public GameObject shadowCuphead;

	// Token: 0x04002D17 RID: 11543
	[SerializeField]
	public GameObject shadowMugman;

	// Token: 0x04002D18 RID: 11544
	public GameObject shadow;
}
