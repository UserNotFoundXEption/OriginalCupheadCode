using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class ArcadePlayerAnimationController : AbstractArcadePlayerComponent
{
	// Token: 0x0600349B RID: 13467 RVA: 0x0002B27E File Offset: 0x0002947E
	public override void OnAwake()
	{
		base.OnAwake();
		this.SetSprites(base.player.id == PlayerId.PlayerOne);
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x000F6E28 File Offset: 0x000F5028
	public void Start()
	{
		base.basePlayer.OnPlayIntroEvent += this.PlayIntro;
		base.player.motor.OnParryEvent += this.OnParryStart;
		base.player.motor.OnGroundedEvent += this.OnGrounded;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.weaponManager.OnExStart += this.OnEx;
		base.player.weaponManager.OnSuperStart += this.OnSuper;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnWeaponFire += this.OnShotFired;
		LevelPauseGUI.OnPauseEvent += this.OnGuiPause;
		LevelPauseGUI.OnPauseEvent += this.OnGuiUnpause;
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0002B29A File Offset: 0x0002949A
	public void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x000F6F34 File Offset: 0x000F5134
	public void Update()
	{
		if (base.player.IsDead || !base.player.levelStarted)
		{
			return;
		}
		if (!this.hitAnimation && base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.x != this.GetInt(ArcadePlayerAnimationController.Integers.LookX))
		{
			this.SetBool(ArcadePlayerAnimationController.Booleans.Turning, true);
		}
		else
		{
			this.SetBool(ArcadePlayerAnimationController.Booleans.Turning, false);
		}
		this.SetBool(ArcadePlayerAnimationController.Booleans.Grounded, base.player.motor.Grounded);
		this.SetBool(ArcadePlayerAnimationController.Booleans.NearLanding, base.player.motor.GetTimeUntilLand() <= 0.15f && !base.player.motor.Parrying);
		this.SetInt(ArcadePlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		this.SetInt(ArcadePlayerAnimationController.Integers.MoveY, base.player.motor.MoveDirection.y);
		this.SetInt(ArcadePlayerAnimationController.Integers.LookX, base.player.motor.TrueLookDirection.x);
		this.SetInt(ArcadePlayerAnimationController.Integers.LookY, base.player.motor.TrueLookDirection.y);
		this.SetBool(ArcadePlayerAnimationController.Booleans.Shooting, base.player.weaponManager.IsShooting);
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(0);
		bool flag = currentAnimatorStateInfo.IsName("Idle") || currentAnimatorStateInfo.IsName("Run");
		if (this.shooting)
		{
			this.timeSinceStoppedShooting = 0f;
		}
		else
		{
			this.timeSinceStoppedShooting += CupheadTime.Delta;
		}
		bool flag2 = false;
		if (this.fired && flag)
		{
			this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnFire);
			this.SetInt(ArcadePlayerAnimationController.Integers.ArmVariant, (!Rand.Bool()) ? 1 : 0);
			flag2 = true;
		}
		this.fired = false;
		this.shooting = base.player.weaponManager.IsShooting;
		if (!this.shooting && !flag2)
		{
			this.ResetTrigger(ArcadePlayerAnimationController.Triggers.OnFire);
		}
		this.SetBool(ArcadePlayerAnimationController.Booleans.Dashing, base.player.motor.Dashing);
		this.SetBool(ArcadePlayerAnimationController.Booleans.NearDashEnd, base.player.motor.GetTimeUntilDashEnd() < ((!base.player.motor.Grounded) ? 0.108333334f : 0.15f));
		if (!base.player.motor.Dashing)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				base.transform.SetScale(new float?(base.player.motor.LookDirection.x), null, null);
			}
		}
		else
		{
			base.transform.SetScale(new float?((float)base.player.motor.DashDirection), null, null);
		}
		base.animator.Update(Time.deltaTime);
		for (int i = 0; i < 3; i++)
		{
			base.animator.Update(0f);
		}
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x000F72D4 File Offset: 0x000F54D4
	public void ChangeToRocket()
	{
		this.prong.SetActive(false);
		string animation = "Rocket";
		this.Play(animation);
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x000F72FC File Offset: 0x000F54FC
	public void ChangeToJetpack()
	{
		this.prong.SetActive(false);
		string animation = "Jetpack";
		this.Play(animation);
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0002B2A9 File Offset: 0x000294A9
	public override void OnPause()
	{
		base.OnPause();
		this.SetAlpha(1f);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0002B2BC File Offset: 0x000294BC
	public void OnGuiPause()
	{
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0002B2BE File Offset: 0x000294BE
	public void OnGuiUnpause()
	{
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0002B2C0 File Offset: 0x000294C0
	public void OnShotFired()
	{
		this.fired = true;
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0002B2C9 File Offset: 0x000294C9
	public void OnLevelWin()
	{
		base.player.damageReceiver.OnWin();
		this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnWin);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x000F7324 File Offset: 0x000F5524
	public void PlayIntro()
	{
		this.SetBool(ArcadePlayerAnimationController.Booleans.Intro, true);
		string text = (base.player.id != PlayerId.PlayerOne) ? "Mugman" : "Cuphead";
		this.Play("Intro_" + text);
		if (text == "Cuphead")
		{
			AudioManager.Play("player_intro_cuphead");
		}
		else
		{
			AudioManager.Play("player_intro_mugman");
		}
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x0002B2E2 File Offset: 0x000294E2
	public void LevelInit()
	{
		this.SetSprites(base.player.id == PlayerId.PlayerOne);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x000F7394 File Offset: 0x000F5594
	public void SetSprites(bool isCuphead)
	{
		this.cuphead.SetActive(isCuphead);
		this.mugman.SetActive(!isCuphead);
		if (isCuphead)
		{
			this.spriteRenderer = this.cuphead.GetComponent<SpriteRenderer>();
			this.armRenderer = this.cupheadArm.GetComponent<SpriteRenderer>();
		}
		else
		{
			this.spriteRenderer = this.mugman.GetComponent<SpriteRenderer>();
			this.armRenderer = this.mugmanArm.GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x000F740C File Offset: 0x000F560C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		AudioManager.Play("player_hit");
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal)
		{
			this.Play("Hit");
			this.hitAnimation = true;
		}
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x0002B2F8 File Offset: 0x000294F8
	public void OnRunDust()
	{
		this.runEffect.Create(this.runDustRoot.position);
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0002B311 File Offset: 0x00029511
	public void onHitAnimationComplete()
	{
		this.hitAnimation = false;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x0002B31A File Offset: 0x0002951A
	public void SetSpriteProperties(SpriteLayer layer, int order)
	{
		this.spriteRenderer.sortingLayerName = layer.ToString();
		this.spriteRenderer.sortingOrder = order;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x000F745C File Offset: 0x000F565C
	public void ResetSpriteProperties()
	{
		this.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString();
		this.spriteRenderer.sortingOrder = ((base.player.id != PlayerId.PlayerOne) ? -1 : 1);
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0002B340 File Offset: 0x00029540
	public void OnParryStart()
	{
		if (this.super)
		{
			return;
		}
		this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnParry);
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0002B355 File Offset: 0x00029555
	public void OnParrySuccess()
	{
		this.SetAlpha(1f);
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0002B362 File Offset: 0x00029562
	public void OnParryPause()
	{
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0002B364 File Offset: 0x00029564
	public void OnParryAnimEnd()
	{
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x0002B366 File Offset: 0x00029566
	public void ResumeNormanAnim()
	{
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0002B368 File Offset: 0x00029568
	public void OnGrounded()
	{
		if (!Level.Current.Started)
		{
			return;
		}
		AudioManager.Play("player_grounded");
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x000F74A8 File Offset: 0x000F56A8
	public void OnEx()
	{
		string text = "Forward";
		if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y > 0)
		{
			text = "Up";
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y > 0)
		{
			text = "Diagonal_Up";
		}
		else if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y < 0)
		{
			text = "Down";
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y < 0)
		{
			text = "Diagonal_Down";
		}
		if (text == "Forward")
		{
			AudioManager.Play("player_ex_forward_ground");
		}
		string text2 = "Ex." + text + "_";
		if (base.player.motor.Grounded)
		{
			text2 += "Ground";
		}
		else
		{
			text2 += "Air";
		}
		this.Play(text2);
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x000F7668 File Offset: 0x000F5868
	public void OnSuper()
	{
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		this.super = true;
		this.spriteRenderer.enabled = false;
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x0002B384 File Offset: 0x00029584
	public void OnSuperEnd()
	{
		this.super = false;
		this.spriteRenderer.enabled = true;
		this.ResetSpriteProperties();
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0002B39F File Offset: 0x0002959F
	public void _OnSuperAnimEnd()
	{
		base.player.UnpauseAll(false);
		base.player.motor.OnSuperEnd();
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0002B3BD File Offset: 0x000295BD
	public void Play(string animation)
	{
		base.animator.Play(animation, 0, 0f);
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x0002B3D1 File Offset: 0x000295D1
	public bool GetBool(ArcadePlayerAnimationController.Booleans b)
	{
		return base.animator.GetBool(b.ToString());
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x0002B3EB File Offset: 0x000295EB
	public void SetBool(ArcadePlayerAnimationController.Booleans b, bool value)
	{
		base.animator.SetBool(b.ToString(), value);
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x0002B406 File Offset: 0x00029606
	public int GetInt(ArcadePlayerAnimationController.Integers i)
	{
		return base.animator.GetInteger(i.ToString());
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x0002B420 File Offset: 0x00029620
	public void SetInt(ArcadePlayerAnimationController.Integers i, int value)
	{
		base.animator.SetInteger(i.ToString(), value);
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x0002B43B File Offset: 0x0002963B
	public void SetTrigger(ArcadePlayerAnimationController.Triggers t)
	{
		base.animator.SetTrigger(t.ToString());
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0002B455 File Offset: 0x00029655
	public void ResetTrigger(ArcadePlayerAnimationController.Triggers t)
	{
		base.animator.ResetTrigger(t.ToString());
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x000F76A8 File Offset: 0x000F58A8
	public void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
		this.armRenderer.color = color;
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000F76E4 File Offset: 0x000F58E4
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x000F771C File Offset: 0x000F591C
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x0002B46F File Offset: 0x0002966F
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0002B492 File Offset: 0x00029692
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000F7760 File Offset: 0x000F5960
	public IEnumerator setColor_cr(Color color, float time)
	{
		float t = 0f;
		Color startColor = this.spriteRenderer.color;
		while (t < time)
		{
			float val = t / time;
			this.SetColor(Color.Lerp(startColor, color, val));
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetColor(color);
		yield return null;
		yield break;
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060034C5 RID: 13509 RVA: 0x0002B4B2 File Offset: 0x000296B2
	public bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000F778C File Offset: 0x000F598C
	public IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
			while (!this.Flashing)
			{
				yield return true;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.5f);
			while (this.Flashing)
			{
				this.SetAlpha(0.3f);
				t = 0f;
				while (t < 0.05f)
				{
					if (!this.Flashing)
					{
						this.SetAlpha(1f);
						break;
					}
					t += base.LocalDeltaTime;
					yield return null;
				}
				if (!this.Flashing)
				{
					this.SetAlpha(1f);
					break;
				}
				this.SetAlpha(1f);
				t = 0f;
				while (t < 0.2f)
				{
					if (!this.Flashing)
					{
						this.SetAlpha(1f);
						break;
					}
					t += base.LocalDeltaTime;
					yield return null;
				}
				if (!this.Flashing)
				{
					this.SetAlpha(1f);
					break;
				}
			}
		}
		yield break;
	}

	// Token: 0x04002B2B RID: 11051
	[SerializeField]
	public GameObject prong;

	// Token: 0x04002B2C RID: 11052
	[SerializeField]
	public GameObject cuphead;

	// Token: 0x04002B2D RID: 11053
	[SerializeField]
	public GameObject mugman;

	// Token: 0x04002B2E RID: 11054
	[SerializeField]
	public GameObject cupheadArm;

	// Token: 0x04002B2F RID: 11055
	[SerializeField]
	public GameObject mugmanArm;

	// Token: 0x04002B30 RID: 11056
	[Space(10f)]
	[SerializeField]
	public Transform runDustRoot;

	// Token: 0x04002B31 RID: 11057
	[Space(10f)]
	[SerializeField]
	public Effect dashEffect;

	// Token: 0x04002B32 RID: 11058
	[SerializeField]
	public Effect groundedEffect;

	// Token: 0x04002B33 RID: 11059
	[SerializeField]
	public Effect hitEffect;

	// Token: 0x04002B34 RID: 11060
	[SerializeField]
	public Effect runEffect;

	// Token: 0x04002B35 RID: 11061
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002B36 RID: 11062
	public SpriteRenderer armRenderer;

	// Token: 0x04002B37 RID: 11063
	public bool hitAnimation;

	// Token: 0x04002B38 RID: 11064
	public bool super;

	// Token: 0x04002B39 RID: 11065
	public bool shooting;

	// Token: 0x04002B3A RID: 11066
	public bool fired;

	// Token: 0x04002B3B RID: 11067
	public float timeSinceStoppedShooting = 100f;

	// Token: 0x04002B3C RID: 11068
	public const float STOP_SHOOTING_DELAY = 0.0833f;

	// Token: 0x04002B3D RID: 11069
	public const float JUMP_END_ANIMATION_TIME = 0.15f;

	// Token: 0x04002B3E RID: 11070
	public const float DASH_END_ANIMATION_TIME = 0.15f;

	// Token: 0x04002B3F RID: 11071
	public const float DASH_END_AIR_ANIMATION_TIME = 0.108333334f;

	// Token: 0x04002B40 RID: 11072
	public IEnumerator colorCoroutine;

	// Token: 0x02001153 RID: 4435
	public enum Booleans
	{
		// Token: 0x040079DC RID: 31196
		Dashing,
		// Token: 0x040079DD RID: 31197
		Shooting,
		// Token: 0x040079DE RID: 31198
		Grounded,
		// Token: 0x040079DF RID: 31199
		Turning,
		// Token: 0x040079E0 RID: 31200
		Intro,
		// Token: 0x040079E1 RID: 31201
		Dead,
		// Token: 0x040079E2 RID: 31202
		NearLanding,
		// Token: 0x040079E3 RID: 31203
		NearDashEnd
	}

	// Token: 0x02001154 RID: 4436
	public enum Integers
	{
		// Token: 0x040079E5 RID: 31205
		MoveX,
		// Token: 0x040079E6 RID: 31206
		MoveY,
		// Token: 0x040079E7 RID: 31207
		LookX,
		// Token: 0x040079E8 RID: 31208
		LookY,
		// Token: 0x040079E9 RID: 31209
		ArmVariant
	}

	// Token: 0x02001155 RID: 4437
	public enum Triggers
	{
		// Token: 0x040079EB RID: 31211
		OnJump,
		// Token: 0x040079EC RID: 31212
		OnGround,
		// Token: 0x040079ED RID: 31213
		OnParry,
		// Token: 0x040079EE RID: 31214
		OnWin,
		// Token: 0x040079EF RID: 31215
		OnTurn,
		// Token: 0x040079F0 RID: 31216
		OnFire
	}
}
