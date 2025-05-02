using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class GraveyardLevelSplitDevil : LevelProperties.Graveyard.Entity
{
	// Token: 0x06001EBC RID: 7868 RVA: 0x00019F8A File Offset: 0x0001818A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x00019FA2 File Offset: 0x000181A2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000B3730 File Offset: 0x000B1930
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += base.GetComponentInParent<GraveyardLevelSplitDevil>().OnDamageTaken;
		if (base.transform.localScale.x > 0f)
		{
			this.SetIsAngel(true);
			this.id = 0;
		}
		this.sideString = ((base.transform.localScale.x >= 0f) ? "left" : "right");
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x000B37D4 File Offset: 0x000B19D4
	public override void LevelInit(LevelProperties.Graveyard properties)
	{
		base.LevelInit(properties);
		this.numProjectiles = new PatternString(properties.CurrentState.splitDevilProjectiles.numProjectiles[this.id], true);
		this.projectileAngleOffset = new PatternString(properties.CurrentState.splitDevilProjectiles.angleOffsetString, true);
		this.projectilePinkString = new PatternString(properties.CurrentState.splitDevilProjectiles.pinkString, true);
		this.level = (Level.Current as GraveyardLevel);
		base.StartCoroutine(this.fade_in_cr());
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000B3860 File Offset: 0x000B1A60
	public IEnumerator fade_in_cr()
	{
		this.mainRend.color = new Color(0f, 0f, 0f, 0f);
		this.haloRend.color = new Color(0f, 0f, 0f, 0f);
		float t = 0f;
		while (t < 4f)
		{
			this.mainRend.color = new Color(0f, 0f, 0f, t / 4f);
			this.haloRend.color = new Color(0f, 0f, 0f, t / 4f);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		this.mainRend.color = new Color(0f, 0f, 0f, 1f);
		this.haloRend.color = new Color(0f, 0f, 0f, 1f);
		yield break;
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000B387C File Offset: 0x000B1A7C
	public void SetIsAngel(bool value)
	{
		if (this.isAngel != value && !value)
		{
			AudioManager.Play("sfx_dlc_graveyard_changedirectionbad");
			this.emitAudioFromObject.Add("sfx_dlc_graveyard_changedirectionbad");
		}
		base.animator.SetBool("isAngel", value);
		this.coll.enabled = !value;
		this.isAngel = value;
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + this.sideString, (!this.isAngel) ? 1E-05f : 0.4f, 0.4f);
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + this.sideString, this.isAngel ? 1E-05f : 0.4f, 0.4f);
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000B3944 File Offset: 0x000B1B44
	public void LateUpdate()
	{
		if (this.dead)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null && !levelPlayerController.IsDead)
			{
				num2++;
				num += (int)Mathf.Sign(levelPlayerController.transform.localScale.x);
			}
		}
		if (Mathf.Abs(num) == num2)
		{
			if (this.isAngel != (Mathf.Sign((float)num) == Mathf.Sign(base.transform.localScale.x)) && this.headLooping)
			{
				this.ResyncHead();
			}
			this.SetIsAngel(Mathf.Sign((float)num) == Mathf.Sign(base.transform.localScale.x));
		}
		this.headlessRend.enabled = (this.headRend.sprite != null);
		this.mainRend.enabled = !this.headlessRend.enabled;
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x00019FCB File Offset: 0x000181CB
	public void NextPattern()
	{
		base.StartCoroutine((!this.level.CheckForBeamAttack()) ? this.projectile_cr() : this.roar_cr());
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000B3A8C File Offset: 0x000B1C8C
	public IEnumerator roar_cr()
	{
		LevelProperties.Graveyard.SplitDevilBeam p = base.properties.CurrentState.splitDevilBeam;
		base.animator.SetBool("isSinging", true);
		int targetA = Animator.StringToHash(base.animator.GetLayerName(0) + ".SingStartAngel");
		int targetB = Animator.StringToHash(base.animator.GetLayerName(0) + ".SingStartDevil");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetA && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetB)
		{
			yield return null;
		}
		this.beamPrefab.Create(new Vector3(Mathf.Sign(base.transform.position.x) * (float)(Level.Current.Right - 50), 80f), p.speed.RandomFloat() * -Mathf.Sign(base.transform.position.x), p.warning, this);
		yield return new WaitForSeconds(1f);
		base.animator.SetBool("isSinging", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack.RandomFloat());
		this.NextPattern();
		yield break;
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x000B3AA8 File Offset: 0x000B1CA8
	public void ResyncHead()
	{
		float num = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 0.25f;
		float num2 = base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime % 0.25f;
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleAngel"))
		{
			base.animator.Play("ShootLoopAngel", 1, num2 + num);
			base.animator.Update(0f);
		}
		else if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleDevil"))
		{
			base.animator.Play("ShootLoopDevil", 1, num2 + num);
			base.animator.Update(0f);
		}
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x000B3BB8 File Offset: 0x000B1DB8
	public IEnumerator projectile_cr()
	{
		this.triggerShoot = true;
		while (this.triggerShoot)
		{
			yield return null;
		}
		base.animator.Play("Charge", 2, 0f);
		yield return CupheadTime.WaitForSeconds(this, 0.166666672f);
		this.headLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Charge", 2, false, false);
		LevelProperties.Graveyard.SplitDevilProjectiles p = base.properties.CurrentState.splitDevilProjectiles;
		AbstractPlayerController player = PlayerManager.GetNext();
		float delayBetweenProjectiles = p.delayBetweenProjectiles.RandomFloat();
		LevelPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		int projectileCount = this.numProjectiles.PopInt();
		for (int i = 0; i < projectileCount; i++)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			float rotation = MathUtils.DirectionToAngle(player.center - this.projectileRoot.transform.position);
			rotation += this.projectileAngleOffset.PopFloat();
			bool isPink = this.projectilePinkString.PopLetter() == 'P';
			if (isPink)
			{
				this.projectilePinkPrefab.Create(this.projectileRoot.transform.position, rotation, p.projectileSpeed, this);
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.transform.position, rotation, p.projectileSpeed, this);
			}
			base.animator.Play((!this.isAngel) ? ((!Rand.Bool()) ? "FireB" : "FireA") : "Light", (!isPink) ? 3 : 4, 0f);
			this.shootFXRend[0].flipX = (this.isAngel && Rand.Bool());
			this.shootFXRend[1].flipX = (this.isAngel && Rand.Bool());
			this.shootFXRend[0].flipY = (this.isAngel && Rand.Bool());
			this.shootFXRend[1].flipY = (this.isAngel && Rand.Bool());
			this.SFX_SplitDevil_Shoot();
			if (i < projectileCount - 1)
			{
				yield return CupheadTime.WaitForSeconds(this, Mathf.Clamp(delayBetweenProjectiles - 0.458333343f, 0f, float.MaxValue));
				base.animator.Play("Charge", 2, 0f);
				yield return base.animator.WaitForAnimationToEnd(this, "Charge", 2, false, true);
				yield return CupheadTime.WaitForSeconds(this, 0.125f);
			}
		}
		this.headLooping = false;
		base.animator.SetBool("isShooting", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack.RandomFloat());
		this.NextPattern();
		yield break;
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000B3BD4 File Offset: 0x000B1DD4
	public void AniEvent_CanStartShoot()
	{
		if (this.triggerShoot)
		{
			bool flag = base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleAngel");
			if (flag != this.isAngel)
			{
				base.animator.Play((!flag) ? "ShootStartDevilToAngel" : "ShootStartAngelToDevil", 1, 0f);
			}
			else
			{
				base.animator.Play((!this.isAngel) ? "ShootStartDevil" : "ShootStartAngel", 1, 0f);
			}
			base.animator.SetBool("isShooting", true);
			this.triggerShoot = false;
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x00019FF5 File Offset: 0x000181F5
	public void OnDisable()
	{
		if (!base.animator.GetBool("isShooting"))
		{
			this.headlessRend.enabled = false;
			this.mainRend.enabled = true;
		}
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x0001A024 File Offset: 0x00018224
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000B3C98 File Offset: 0x000B1E98
	public void Die()
	{
		this.dead = true;
		this.headRend.enabled = false;
		this.headlessRend.enabled = false;
		this.mainRend.enabled = true;
		this.triggerShoot = false;
		this.StopAllCoroutines();
		base.animator.Play((!this.isAngel) ? "DeathDevilLoop" : "DeathAngelLoop");
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000B3D10 File Offset: 0x000B1F10
	public IEnumerator death_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		base.animator.SetTrigger("DeathContinue");
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield break;
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x0001A037 File Offset: 0x00018237
	public void ActivateBGSkellyMask()
	{
		this.bgSkellyMask.SetActive(true);
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x000B3D2C File Offset: 0x000B1F2C
	public void SFXSingRoar()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + this.sideString, (!this.isAngel) ? 1E-05f : 0.4f, 0.01f);
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + this.sideString, this.isAngel ? 1E-05f : 0.4f, 0.01f);
		AudioManager.Play("sfx_dlc_graveyard_angelsing_" + this.sideString);
		this.emitAudioFromObject.Add("sfx_dlc_graveyard_angelsing_" + this.sideString);
		AudioManager.Play("sfx_dlc_graveyard_devilangryrage_" + this.sideString);
		this.emitAudioFromObject.Add("sfx_dlc_graveyard_devilangryrage_" + this.sideString);
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x0001A045 File Offset: 0x00018245
	public void AnimationEvent_SFX_SplitDevil_AngelSing()
	{
		this.SFXSingRoar();
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x0001A04D File Offset: 0x0001824D
	public void AnimationEvent_SFX_SplitDevil_DevilRage()
	{
		this.SFXSingRoar();
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x000B3E04 File Offset: 0x000B2004
	public void SFX_SplitDevil_Shoot()
	{
		AudioManager.Play((!this.isAngel) ? "sfx_dlc_graveyard_devil_shoot" : "sfx_DLC_Graveyard_Angel_Shoot");
		this.emitAudioFromObject.Add((!this.isAngel) ? "sfx_dlc_graveyard_devil_shoot" : "sfx_DLC_Graveyard_Angel_Shoot");
	}

	// Token: 0x04001909 RID: 6409
	public const float SING_ROAR_MAX_VOLUME = 0.4f;

	// Token: 0x0400190A RID: 6410
	[SerializeField]
	public GraveyardLevelSplitDevilProjectile projectilePrefab;

	// Token: 0x0400190B RID: 6411
	[SerializeField]
	public GraveyardLevelSplitDevilProjectile projectilePinkPrefab;

	// Token: 0x0400190C RID: 6412
	[SerializeField]
	public GraveyardLevelSplitDevilBeam beamPrefab;

	// Token: 0x0400190D RID: 6413
	public DamageDealer damageDealer;

	// Token: 0x0400190E RID: 6414
	public DamageReceiver damageReceiver;

	// Token: 0x0400190F RID: 6415
	[SerializeField]
	public GameObject bgSkellyMask;

	// Token: 0x04001910 RID: 6416
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04001911 RID: 6417
	[SerializeField]
	public SpriteRenderer headRend;

	// Token: 0x04001912 RID: 6418
	[SerializeField]
	public SpriteRenderer mainRend;

	// Token: 0x04001913 RID: 6419
	[SerializeField]
	public SpriteRenderer headlessRend;

	// Token: 0x04001914 RID: 6420
	[SerializeField]
	public SpriteRenderer haloRend;

	// Token: 0x04001915 RID: 6421
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001916 RID: 6422
	public PatternString numProjectiles;

	// Token: 0x04001917 RID: 6423
	public PatternString projectileAngleOffset;

	// Token: 0x04001918 RID: 6424
	public bool triggerShoot;

	// Token: 0x04001919 RID: 6425
	public bool isAngel;

	// Token: 0x0400191A RID: 6426
	public bool dead;

	// Token: 0x0400191B RID: 6427
	public bool headLooping;

	// Token: 0x0400191C RID: 6428
	[SerializeField]
	public SpriteRenderer[] shootFXRend;

	// Token: 0x0400191D RID: 6429
	public int id = 1;

	// Token: 0x0400191E RID: 6430
	public GraveyardLevel level;

	// Token: 0x0400191F RID: 6431
	public PatternString projectilePinkString;

	// Token: 0x04001920 RID: 6432
	public string sideString;
}
