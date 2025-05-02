using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class AirplaneLevelBulldogCatAttack : LevelProperties.Airplane.Entity
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0000BA66 File Offset: 0x00009C66
	// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x0000BA6E File Offset: 0x00009C6E
	public bool isAttacking { get; set; }

	// Token: 0x06000DA9 RID: 3497 RVA: 0x0000BA77 File Offset: 0x00009C77
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0000BA84 File Offset: 0x00009C84
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0000BA8C File Offset: 0x00009C8C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0000BAA4 File Offset: 0x00009CA4
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x0000BAAD File Offset: 0x00009CAD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x00087B10 File Offset: 0x00085D10
	public void StartCat(Vector2 pos)
	{
		this.count = Random.Range(0, 3);
		this.isAttacking = true;
		base.transform.localScale = new Vector3(Mathf.Sign(pos.x), 1f);
		base.transform.position = pos;
		base.animator.SetBool("Exit", false);
		base.StartCoroutine(this.cat_cr());
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00087B84 File Offset: 0x00085D84
	public IEnumerator cat_cr()
	{
		LevelProperties.Airplane.Triple p = base.properties.CurrentState.triple;
		base.animator.Play("Intro");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 1E-05f, 1E-05f);
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whistle_in");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_whistle_in");
		yield return base.animator.WaitForAnimationToStart(this, "IntroLoop", false);
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		base.animator.SetTrigger("Continue");
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_catattack_hover");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_hover");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0.15f, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_enter");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_enter");
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		yield return CupheadTime.WaitForSeconds(this, p.shootWarning);
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootA");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootA", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delayAfterFirst.RandomFloat());
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootB");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootB", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delayAfterSecond.RandomFloat());
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootA");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootA", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.shootRecovery);
		base.animator.SetBool("Exit", true);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0f, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_leave");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_leave");
		yield return base.animator.WaitForAnimationToEnd(this, "Exit", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.returnDelay);
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x0000BACB File Offset: 0x00009CCB
	public void EarlyExit()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.early_exit_cr());
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00087BA0 File Offset: 0x00085DA0
	public IEnumerator early_exit_cr()
	{
		base.animator.SetBool("Exit", true);
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.triple.returnDelay);
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00087BBC File Offset: 0x00085DBC
	public void AniEvent_Shoot()
	{
		LevelProperties.Airplane.Triple triple = base.properties.CurrentState.triple;
		float num = triple.attackAngleRange.RandomFloat();
		float num2 = (base.transform.localScale.x >= 0f) ? 180f : 0f;
		BasicProjectile basicProjectile = this.projectile.Create(this.root.position, num2 + num, triple.bulletSpeed);
		basicProjectile.transform.localScale = new Vector3(1f, (float)((num2 <= 0f) ? 1 : -1));
		Animator component = basicProjectile.GetComponent<Animator>();
		component.Play((this.count % 3).ToString(), 0, Random.Range(0.375f, 0.75f));
		component.Update(0f);
		this.count++;
		base.animator.Play(Random.Range(0, 4).ToString(), 1, 0f);
		base.animator.Update(0f);
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0000BAE0 File Offset: 0x00009CE0
	public void OnDisable()
	{
		base.GetComponent<HitFlash>().StopAllCoroutinesWithoutSettingScale();
		base.GetComponent<SpriteRenderer>().color = Color.black;
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x0000BAFD File Offset: 0x00009CFD
	public void AniEvent_IntroFX()
	{
		base.animator.Play("IntroFX", 1);
		base.animator.Update(0f);
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0000BB20 File Offset: 0x00009D20
	public void AniEvent_FlashA()
	{
		base.animator.Play("FlashA", 2);
		base.animator.Update(0f);
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0000BB43 File Offset: 0x00009D43
	public void AniEvent_FlashB()
	{
		base.animator.Play("FlashB", 2);
		base.animator.Update(0f);
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0000BB66 File Offset: 0x00009D66
	public void AniEvent_EmberA()
	{
		base.animator.Play("EmberA", 3);
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0000BB79 File Offset: 0x00009D79
	public void AniEvent_EmberB()
	{
		base.animator.Play("EmberB", 3);
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0000BB8C File Offset: 0x00009D8C
	public void SFX_DOGFIGHT_Cat_Shoot()
	{
		AudioManager.Play("sfx_dlc_dogfight_catgun_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_catgun_shoot");
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0000BBA8 File Offset: 0x00009DA8
	public void SFX_DOGFIGHT_Cat_StartMeow()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_catgunmeow");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catgunmeow");
	}

	// Token: 0x04000ABB RID: 2747
	[SerializeField]
	public AirplaneLevelBulldogPlane main;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	public Transform root;

	// Token: 0x04000ABE RID: 2750
	public DamageDealer damageDealer;

	// Token: 0x04000ABF RID: 2751
	public int count;
}
