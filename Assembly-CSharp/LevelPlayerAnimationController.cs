using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class LevelPlayerAnimationController : AbstractLevelPlayerComponent
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060035EC RID: 13804 RVA: 0x0002C2C7 File Offset: 0x0002A4C7
	// (set) Token: 0x060035ED RID: 13805 RVA: 0x0002C2CF File Offset: 0x0002A4CF
	public SpriteRenderer spriteRenderer { get; set; }

	// Token: 0x060035EE RID: 13806 RVA: 0x000FB960 File Offset: 0x000F9B60
	public void Start()
	{
		if (!this.chaliceActivated)
		{
			base.animator.SetLayerWeight(3, 0f);
			base.animator.SetLayerWeight(4, 0f);
		}
		base.basePlayer.OnPlayIntroEvent += this.PlayIntro;
		base.basePlayer.OnPlatformingLevelAwakeEvent += this.CheckIfChaliceAndActivate;
		base.player.motor.OnParryEvent += this.OnParryStart;
		base.player.motor.OnGroundedEvent += this.OnGrounded;
		base.player.motor.OnDashStartEvent += this.OnDashStart;
		base.player.motor.OnDashEndEvent += this.OnDashEnd;
		base.player.motor.OnDoubleJumpEvent += this.ChaliceDoubleJumpFX;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.weaponManager.OnExStart += this.OnEx;
		base.player.weaponManager.OnSuperStart += this.OnSuper;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnWeaponFire += this.OnShotFired;
		LevelPauseGUI.OnPauseEvent += this.OnGuiPause;
		LevelPauseGUI.OnPauseEvent += this.OnGuiUnpause;
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
		this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss);
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chalice.GetComponent<SpriteRenderer>().sharedMaterial, SettingsData.Data.filter == BlurGamma.Filter.Chalice);
		if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.curseCharmLevel = CharmCurse.CalculateLevel(base.player.id);
		}
		if (Level.Current.LevelType != Level.Type.Platforming)
		{
			if (base.player.stats.isChalice)
			{
				if ((Level.IsDicePalace && !DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) || SceneLoader.CurrentLevel == Levels.Kitchen || SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
				{
					this.CheckIfChaliceAndActivate();
					base.basePlayer.OnPlayIntroEvent -= this.PlayIntro;
				}
				else if (SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker)
				{
					this.StartChaliceIntroHold(false);
				}
			}
			else if (base.player.stats.Loadout.charm == Charm.charm_chalice && SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker && SceneLoader.CurrentLevel != Levels.Kitchen && SceneLoader.CurrentLevel != Levels.ChaliceTutorial && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
			{
				this.StartChaliceIntroHold(true);
			}
		}
		if (SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
		{
			this.spriteRenderer.gameObject.layer = 31;
			foreach (SpriteRenderer spriteRenderer in this.chaliceSprites)
			{
				spriteRenderer.gameObject.layer = 31;
			}
			foreach (SpriteRenderer spriteRenderer2 in this.chaliceJumpShootRenderers)
			{
				spriteRenderer2.gameObject.layer = 31;
			}
		}
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x0002C2D8 File Offset: 0x0002A4D8
	public void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x000FBD44 File Offset: 0x000F9F44
	public void OnDisable()
	{
		if (this.paladinShadows[0])
		{
			this.paladinShadows[0].enabled = false;
		}
		if (this.paladinShadows[1])
		{
			this.paladinShadows[1].enabled = false;
		}
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x000FBD94 File Offset: 0x000F9F94
	public void Update()
	{
		if (base.player.IsDead || !base.player.levelStarted)
		{
			return;
		}
		if (this.curseCharmLevel > -1 && !this.showCurseFX && !Level.IsChessBoss)
		{
			this.InitializeCurseFX();
			this.showCurseFX = true;
		}
		if (base.player.stats.isChalice && this.chaliceActivated)
		{
			this.ChaliceAimSpriteHandling();
			this.ChaliceJumpHandling();
			this.ChaliceJumpShootHandling();
			if (!base.player.motor.Dashing)
			{
				base.animator.SetLayerWeight(3, 1f);
				if (this.chaliceInvincibleSparklesCoroutine != null)
				{
					base.StopCoroutine(this.chaliceInvincibleSparklesCoroutine);
					this.chaliceInvincibleSparklesCoroutine = null;
				}
			}
		}
		if (this.curseCharmLevel > -1)
		{
			this.HandleCurseFX();
		}
		if (!this.hitAnimation && base.player.motor.LookDirection.x != 0 && this.lastTrueLookDir.x != base.player.motor.TrueLookDirection.x)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.Turning, true);
		}
		else
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.Turning, false);
		}
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
		this.SetBool(LevelPlayerAnimationController.Booleans.Grounded, base.player.motor.Grounded);
		this.SetBool(LevelPlayerAnimationController.Booleans.Locked, base.player.motor.Locked);
		if (base.player.motor.Locked)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		}
		if (base.player.motor.Ducking || base.player.motor.IsUsingSuperOrEx)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveY, 0);
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, true);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveY, base.player.motor.MoveDirection.y);
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, false);
		}
		this.SetInt(LevelPlayerAnimationController.Integers.LookX, base.player.motor.LookDirection.x);
		this.SetInt(LevelPlayerAnimationController.Integers.LookY, base.player.motor.LookDirection.y);
		this.SetBool(LevelPlayerAnimationController.Booleans.Shooting, base.player.weaponManager.IsShooting);
		float num = (!base.player.weaponManager.IsShooting && this.timeSinceStoppedShooting >= 0.0833f) ? 0f : 1f;
		if (!base.player.stats.isChalice)
		{
			base.animator.SetLayerWeight(1, num);
			base.animator.SetLayerWeight(2, (base.player.motor.LookDirection.y <= 0) ? 0f : num);
		}
		else
		{
			if (!base.player.motor.Grounded && base.animator.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX))
			{
				num = 0f;
			}
			if (!this.ExitingChaliceSuper())
			{
				base.animator.SetLayerWeight(4, 1f - num);
			}
			else
			{
				base.animator.SetLayerWeight(4, 0f);
			}
			base.animator.SetLayerWeight(5, num);
			base.animator.SetLayerWeight(6, (base.player.motor.LookDirection.y <= 0) ? 0f : num);
			if (base.player.motor.ChaliceDuckDashed && !base.player.motor.Grounded)
			{
				this.chaliceFellFromDuckDash = true;
			}
			if (base.player.motor.Grounded)
			{
				this.chaliceFellFromDuckDash = false;
			}
		}
		if (this.shooting)
		{
			this.timeSinceStoppedShooting = 0f;
		}
		else
		{
			this.timeSinceStoppedShooting += CupheadTime.Delta;
		}
		bool flag = false;
		if (this.fired && ((base.player.motor.Grounded && (base.player.motor.LookDirection.x == 0 || base.player.motor.Locked || base.player.motor.LookDirection.y < 0)) || (base.player.stats.isChalice && !base.player.motor.ChaliceDoubleJumped)))
		{
			this.SetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
			flag = true;
		}
		this.fired = false;
		this.shooting = base.player.weaponManager.IsShooting;
		if (!this.shooting && !flag)
		{
			this.ResetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
		}
		if (base.player.motor.Dashing && this.GetBool(LevelPlayerAnimationController.Booleans.Dashing) != base.player.motor.Dashing)
		{
			if (base.player.stats.isChalice)
			{
				base.animator.SetLayerWeight(3, 0f);
			}
			if (base.player.stats.isChalice && base.player.motor.Ducking)
			{
				this.ChaliceDuckDashHandling();
			}
			else
			{
				this.Play("Dash.Air");
				if (base.player.stats.Loadout.charm != Charm.charm_smoke_dash || !base.player.stats.CurseSmokeDash || Level.IsChessBoss || (base.player.stats.isChalice && !base.player.motor.Ducking))
				{
					this.dashEffect.Create(base.transform.position, base.transform.localScale);
				}
				if (base.player.stats.isChalice)
				{
					this.chaliceDashEffectActive = this.chaliceDashEffect.Create(base.transform.position, base.transform.localScale);
					this.chaliceDashEffectActive.transform.parent = base.transform;
				}
			}
		}
		this.SetBool(LevelPlayerAnimationController.Booleans.Dashing, base.player.motor.Dashing);
		if (!base.player.motor.Dashing)
		{
			if (base.player.motor.LookDirection.x != 0 && !this.ExitingChaliceSuper())
			{
				base.transform.SetScale(new float?(base.player.motor.LookDirection.x), null, null);
			}
		}
		else
		{
			base.transform.SetScale(new float?((float)base.player.motor.DashDirection), null, null);
		}
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x0002C2E7 File Offset: 0x0002A4E7
	public void ResetMoveX()
	{
		this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		this.inScaredIntro = false;
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x000FC584 File Offset: 0x000FA784
	public void ChaliceDoubleJumpFX()
	{
		float value = 0f;
		if (base.player.input.GetAxis(PlayerInput.Axis.X) > 0f || (base.player.input.GetAxis(PlayerInput.Axis.X) > 0f && base.player.input.GetAxis(PlayerInput.Axis.Y) > 0f))
		{
			value = -35f;
		}
		else if (base.player.input.GetAxis(PlayerInput.Axis.X) < 0f || (base.player.input.GetAxis(PlayerInput.Axis.X) < 0f && base.player.input.GetAxis(PlayerInput.Axis.Y) > 0f))
		{
			value = 35f;
		}
		Effect effect = this.chaliceDoubleJumpEffect.Create(base.transform.position);
		effect.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x000FC684 File Offset: 0x000FA884
	public void ChaliceIncrementJumpDescendLoopCounter()
	{
		if (base.player.motor.MoveDirection.y < 0)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, this.GetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter) + 1);
		}
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x0002C2FC File Offset: 0x0002A4FC
	public void ChaliceResetJumpDescendLoopCounter()
	{
		this.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, 0);
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000FC6CC File Offset: 0x000FA8CC
	public void ChaliceDuckDashHandling()
	{
		this.Play("Duck.Duck_Dash");
		AudioManager.Play("chalice_roll");
		if (this.chaliceInvincibleSparklesCoroutine != null)
		{
			base.StopCoroutine(this.chaliceInvincibleSparklesCoroutine);
			this.chaliceInvincibleSparklesCoroutine = null;
		}
		this.chaliceInvincibleSparklesCoroutine = base.StartCoroutine(this.chaliceInvincibleSparkle_cr());
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x000FC720 File Offset: 0x000FA920
	public IEnumerator chaliceInvincibleSparkle_cr()
	{
		for (;;)
		{
			float x = Random.Range(-base.player.colliderManager.Width, base.player.colliderManager.Width);
			float y = Random.Range(base.player.colliderManager.Height * -0.5f, base.player.colliderManager.Height * 1.5f);
			this.chaliceDuckDashSparkles.Create(base.player.transform.position + new Vector3(x, y, 0f));
			yield return CupheadTime.WaitForSeconds(this, 0.05f);
		}
		yield break;
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x0002C30A File Offset: 0x0002A50A
	public void ChaliceJumpHandling()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.DoubleJump, base.player.motor.ChaliceDoubleJumped);
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x000FC73C File Offset: 0x000FA93C
	public void ChaliceJumpShootHandling()
	{
		bool enabled = (base.player.weaponManager.IsShooting || this.timeSinceStoppedShooting < 0.0833f) && !base.player.motor.Grounded && !base.player.motor.Dashing && !base.player.motor.ChaliceDoubleJumped && !this.chaliceFellFromDuckDash && !this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) && !this.hitAnimation && !this.super;
		this.chaliceJumpShootRenderers[0].enabled = enabled;
		this.chaliceJumpShootRenderers[1].enabled = enabled;
		if (!base.player.motor.Grounded)
		{
			this.spriteRenderer.enabled = (!base.player.weaponManager.IsShooting && this.timeSinceStoppedShooting >= 0.0833f);
			if (base.player.motor.ChaliceDoubleJumped || this.chaliceFellFromDuckDash || base.player.motor.Dashing || this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) || this.hitAnimation)
			{
				this.spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x000FC8A0 File Offset: 0x000FAAA0
	public void ChaliceAimSpriteHandling()
	{
		if (base.player.motor.Locked)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		}
		if (base.player.weaponManager.IsShooting || this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle) || (this.GetInt(LevelPlayerAnimationController.Integers.MoveX) != 0 && !base.player.motor.Dashing) || base.player.motor.Dashing || base.player.motor.DashState == LevelPlayerMotor.DashManager.State.End || !base.player.motor.Grounded || this.inScaredIntro)
		{
			this.SwitchChaliceAim(-1);
			this.spriteRenderer.enabled = true;
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			this.SwitchChaliceAim(2);
			this.spriteRenderer.enabled = false;
			if (base.player.motor.LookDirection.y > 0)
			{
				this.SwitchChaliceAim(1);
				this.spriteRenderer.enabled = false;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				this.SwitchChaliceAim(3);
				this.spriteRenderer.enabled = false;
			}
		}
		else if (base.player.motor.LookDirection.y > 0)
		{
			this.SwitchChaliceAim(0);
			this.spriteRenderer.enabled = false;
		}
		else if (base.player.motor.LookDirection.y < 0)
		{
			this.SwitchChaliceAim(4);
			this.spriteRenderer.enabled = false;
		}
		else
		{
			this.SwitchChaliceAim(-1);
			this.spriteRenderer.enabled = true;
		}
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x000FCAE0 File Offset: 0x000FACE0
	public void SwitchChaliceAim(int spriteToEnable)
	{
		for (int i = 0; i < this.chaliceSprites.Length; i++)
		{
			this.chaliceSprites[i].enabled = (i == spriteToEnable);
		}
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x000FCB18 File Offset: 0x000FAD18
	public void ChaliceEndAirEX()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, false);
		if (base.player.stats.isChalice && !base.player.motor.Grounded)
		{
			string text = this.exDirection;
			if (text != null)
			{
				if (!(text == "Forward"))
				{
					if (!(text == "Up") && !(text == "Down") && !(text == "Diagonal_Down"))
					{
						if (text == "Diagonal_Up")
						{
							base.animator.Play(this.ChaliceAirEXRecovery, 3, 0f);
						}
					}
					else
					{
						base.animator.Play(this.ChaliceAirEXRecovery, 3, 0.0416666679f);
					}
				}
				else
				{
					base.animator.Play(this.ChaliceAirEXRecovery, 3, 0.0833333358f);
				}
			}
		}
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x000FCC14 File Offset: 0x000FAE14
	public void IsIntroB()
	{
		if (!base.player.stats.isChalice)
		{
			this.isIntroB = true;
			if ((base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman))
			{
				this.Play("Boil_Mugman");
			}
		}
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x000FCC80 File Offset: 0x000FAE80
	public void CookieFail()
	{
		if (Level.Current.CurrentLevel == Levels.Bee && base.player.id == PlayerId.PlayerTwo)
		{
			base.transform.position += Vector3.left * 32f;
		}
		string str = ((base.player.id != PlayerId.PlayerOne || !PlayerManager.player1IsMugman) && (base.player.id != PlayerId.PlayerTwo || PlayerManager.player1IsMugman)) ? "Cuphead" : "Mugman";
		this.Play("Intro_Chalice_" + str + "_Fail");
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x000FCD34 File Offset: 0x000FAF34
	public void ScaredChalice(bool showPortal)
	{
		this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		this.inScaredIntro = true;
		this.ActivateChaliceAnimationLayers();
		base.animator.Play("Intro_Chalice_Scared", 3);
		if (!showPortal)
		{
			return;
		}
		bool flag = (base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman);
		string text = (!flag) ? "Cuphead" : "Mugman";
		this.chaliceIntroAnimation.Create(base.transform.position, flag, true);
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x0002C327 File Offset: 0x0002A527
	public void ForceDirection()
	{
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x000FCDDC File Offset: 0x000FAFDC
	public void InitializeCurseFX()
	{
		this.curseEffectAngle = (float)Random.Range(0, 360);
		if (this.curseCharmLevel == 4 && this.paladinShadows != null)
		{
			this.paladinShadowPosition = new Vector3[10];
			this.paladinShadowScale = new Vector3[10];
			this.paladinShadowSprite = new Sprite[10];
			for (int i = 0; i < 10; i++)
			{
				this.paladinShadowPosition[i] = base.transform.position;
				this.paladinShadowSprite[i] = this.spriteRenderer.sprite;
				this.paladinShadowScale[i] = base.transform.localScale;
			}
			this.paladinShadows[0].transform.position = base.transform.position;
			this.paladinShadows[1].transform.position = base.transform.position;
			this.paladinShadows[0].sprite = this.spriteRenderer.sprite;
			this.paladinShadows[1].sprite = this.spriteRenderer.sprite;
			this.paladinShadows[0].enabled = true;
			this.paladinShadows[1].enabled = true;
			this.paladinShadows[0].transform.parent = null;
			this.paladinShadows[1].transform.parent = null;
		}
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x000FCF44 File Offset: 0x000FB144
	public void HandleCurseFX()
	{
		if (PauseManager.state == PauseManager.State.Paused || !this.showCurseFX)
		{
			return;
		}
		this.curseEffectTimer += CupheadTime.Delta;
		while (this.curseEffectTimer >= this.curseEffectDelay)
		{
			Effect effect = this.curseEffect.Create(base.player.center + MathUtils.AngleToDirection(this.curseEffectAngle) * this.curseDistanceRange.RandomFloat());
			string text = null;
			if (this.curseCharmLevel < 2)
			{
				text = ((!Rand.Bool()) ? "Flames" : "Cloud") + Random.Range(0, 3).ToString();
			}
			if (this.curseCharmLevel == 2)
			{
				text = ((!Rand.Bool()) ? ("Dizzy" + Random.Range(0, 4).ToString()) : ("Cloud" + Random.Range(0, 3).ToString()));
			}
			if (this.curseCharmLevel == 3)
			{
				text = "Dizzy" + Random.Range(0, 4).ToString();
			}
			if (this.curseCharmLevel == 4)
			{
				text = "Sparkle" + Random.Range(0, 3).ToString();
			}
			effect.animator.Play(text);
			this.curseEffectAngle = (this.curseEffectAngle + this.curseAngleShiftRange.RandomFloat()) % 360f;
			this.curseEffectTimer -= this.curseEffectDelay;
		}
		if (this.curseCharmLevel == 4 && this.paladinShadows != null)
		{
			this.paladinShadows[0].enabled = !base.player.motor.Dashing;
			this.paladinShadows[1].enabled = !base.player.motor.Dashing;
			for (int i = 9; i > 0; i--)
			{
				this.paladinShadowPosition[i] = this.paladinShadowPosition[i - 1];
				this.paladinShadowScale[i] = this.paladinShadowScale[i - 1];
				this.paladinShadowSprite[i] = this.paladinShadowSprite[i - 1];
			}
			this.paladinShadowPosition[0] = base.transform.position;
			this.paladinShadowScale[0] = base.transform.localScale;
			this.paladinShadowSprite[0] = this.spriteRenderer.sprite;
			this.paladinShadows[0].transform.position = this.paladinShadowPosition[5];
			this.paladinShadows[1].transform.position = this.paladinShadowPosition[9];
			this.paladinShadows[0].transform.localScale = this.paladinShadowScale[5];
			this.paladinShadows[1].transform.localScale = this.paladinShadowScale[9];
			this.paladinShadows[0].sprite = this.paladinShadowSprite[5];
			this.paladinShadows[1].sprite = this.paladinShadowSprite[9];
		}
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x0002C33F File Offset: 0x0002A53F
	public void UpdateAnimator()
	{
		this.Update();
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x0002C347 File Offset: 0x0002A547
	public override void OnPause()
	{
		base.OnPause();
		this.SetAlpha(1f);
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x0002C35A File Offset: 0x0002A55A
	public void OnGuiPause()
	{
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x0002C35C File Offset: 0x0002A55C
	public void OnGuiUnpause()
	{
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x0002C35E File Offset: 0x0002A55E
	public void OnShotFired()
	{
		this.fired = true;
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x0002C367 File Offset: 0x0002A567
	public void OnRevive(Vector3 pos)
	{
		base.animator.Play("Jump");
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x000FD2D8 File Offset: 0x000FB4D8
	public void OnGravityReversed()
	{
		base.transform.SetScale(null, new float?(base.player.motor.GravityReversalMultiplier), null);
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x0002C379 File Offset: 0x0002A579
	public override void OnLevelStart()
	{
		this.CheckIfChaliceAndActivate();
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x0002C381 File Offset: 0x0002A581
	public void OnLevelWin()
	{
		base.player.damageReceiver.OnWin();
		this.SetTrigger(LevelPlayerAnimationController.Triggers.OnWin);
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x0002C39E File Offset: 0x0002A59E
	public void ActivateChaliceAnimationLayers()
	{
		base.animator.SetLayerWeight(3, 1f);
		base.animator.SetLayerWeight(4, 1f);
		this.SetChaliceSprites();
		this.chaliceActivated = true;
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0002C3CF File Offset: 0x0002A5CF
	public void CheckIfChaliceAndActivate()
	{
		if (base.player.stats.isChalice)
		{
			this.ActivateChaliceAnimationLayers();
		}
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x000FD318 File Offset: 0x000FB518
	public void StartChaliceIntroHold(bool fail)
	{
		if (Level.Current.Started || Level.Current.blockChalice)
		{
			return;
		}
		bool flag = (!PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne) || (PlayerManager.player1IsMugman && base.player.id != PlayerId.PlayerOne);
		if (fail)
		{
			base.animator.Play((!flag) ? "Intro_Chalice_Mugman_Fail_Start" : "Intro_Chalice_Cuphead_Fail_Start");
		}
		else
		{
			base.animator.Play("Intro_Chalice_Hold");
			this.chaliceIntroCurrent = this.chaliceIntroAnimation.Create(base.transform.position + Vector3.down * base.player.motor.DistanceToGround(), !flag, false);
			this.SetChaliceSprites();
		}
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x000FD404 File Offset: 0x000FB604
	public void PlayIntro()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.Intro, true);
		bool flag = (base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman);
		string str = (!flag) ? "Cuphead" : "Mugman";
		if (SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker)
		{
			if (base.player.stats.isChalice)
			{
				base.animator.Play("Idle", 0);
				base.animator.Play("Intro_Chalice_" + str, 3);
				if (this.chaliceIntroCurrent)
				{
					this.chaliceIntroCurrent.EndHold();
				}
				this.ActivateChaliceAnimationLayers();
			}
			else if (base.player.stats.Loadout.charm != Charm.charm_chalice || Level.Current.blockChalice)
			{
				string str2 = string.Empty;
				str2 = ((!this.isIntroB) ? "Intro_" : "Intro_B_");
				this.Play(str2 + str);
			}
		}
		else if (!base.player.stats.isChalice)
		{
			if (base.player.id == PlayerId.PlayerOne)
			{
				AudioManager.Play("player_scared_intro");
			}
			this.inScaredIntro = true;
			this.Play("Intro_Scared");
		}
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x000FD590 File Offset: 0x000FB790
	public void ScaredSprite(bool facingLeft)
	{
		base.animator.enabled = false;
		base.enabled = false;
		base.player.motor.enabled = false;
		if (base.player.id == PlayerId.PlayerOne)
		{
			this.cuphead.GetComponent<SpriteRenderer>().sprite = this.cupheadScaredSprite;
			this.cuphead.GetComponent<SpriteRenderer>().flipX = facingLeft;
		}
		else
		{
			this.mugman.GetComponent<SpriteRenderer>().sprite = this.mugmanScaredSprite;
			this.mugman.GetComponent<SpriteRenderer>().flipX = facingLeft;
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x000FD624 File Offset: 0x000FB824
	public void LevelInit()
	{
		bool sprites = (!PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne) || (PlayerManager.player1IsMugman && base.player.id != PlayerId.PlayerOne);
		this.SetSprites(sprites);
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x000FD674 File Offset: 0x000FB874
	public void SetSprites(bool isCuphead)
	{
		this.cuphead.SetActive(isCuphead);
		this.mugman.SetActive(!isCuphead);
		this.chalice.SetActive(false);
		if (isCuphead)
		{
			this.spriteRenderer = this.cuphead.GetComponent<SpriteRenderer>();
		}
		else
		{
			this.spriteRenderer = this.mugman.GetComponent<SpriteRenderer>();
		}
		this.tempMaterial = this.spriteRenderer.material;
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x0002C3EC File Offset: 0x0002A5EC
	public void SetChaliceSprites()
	{
		this.cuphead.SetActive(false);
		this.mugman.SetActive(false);
		this.chalice.SetActive(true);
		this.spriteRenderer = this.chalice.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x0002C423 File Offset: 0x0002A623
	public void EnableSpriteRenderer()
	{
		this.spriteRenderer.enabled = true;
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x000FD6E8 File Offset: 0x000FB8E8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.SuperInvincible)
		{
			return;
		}
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		if (base.player.stats.Health == 4)
		{
			AudioManager.Play("player_damage_crack_level1");
		}
		else if (base.player.stats.Health == 3)
		{
			AudioManager.Play("player_damage_crack_level2");
		}
		else if (base.player.stats.Health == 2)
		{
			AudioManager.Play("player_damage_crack_level3");
		}
		else if (base.player.stats.Health == 1)
		{
			AudioManager.Play("player_damage_crack_level4");
		}
		AudioManager.Play("player_hit");
		bool grounded = base.player.motor.Grounded;
		if (grounded)
		{
			this.Play("Hit.Hit_Ground");
		}
		else
		{
			this.Play("Hit.Hit_Air");
		}
		this.hitAnimation = true;
		this.hitEffect.Create(base.player.center, base.transform.localScale);
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x0002C431 File Offset: 0x0002A631
	public void OnHealerCharm()
	{
		this.healerCharmEffect.Create(base.player.center, base.transform.localScale, base.player);
		AudioManager.Play("sfx_player_charmhealer_extraheart");
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x000FD820 File Offset: 0x000FBA20
	public void OnDashStart()
	{
		this.hitAnimation = false;
		if ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
		{
			this.spriteRenderer.enabled = false;
			this.smokeDashEffect.Create(base.player.center);
		}
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x000FD898 File Offset: 0x000FBA98
	public void OnDashEnd()
	{
		if ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
		{
			this.spriteRenderer.enabled = true;
			this.smokeDashEffect.Create(base.player.center);
		}
		if (!base.player.motor.Grounded && base.player.stats.isChalice)
		{
			base.animator.Play((!base.player.motor.ChaliceDoubleJumped) ? this.ChaliceJumpDescend : this.ChaliceJumpBall, 3, 0f);
		}
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x0002C464 File Offset: 0x0002A664
	public void OnRunDust()
	{
		if (base.enabled)
		{
			this.runEffect.Create(this.runDustRoot.position);
		}
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x0002C488 File Offset: 0x0002A688
	public void OnChaliceDashSparkle()
	{
		if (base.enabled && base.player.stats.isChalice)
		{
			this.chaliceDashSparkle.Create(this.sparkleRoot.position);
		}
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x0002C4C1 File Offset: 0x0002A6C1
	public void OnBurst()
	{
		this.powerUpBurstEffect.Create(base.player.center);
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x0002C4DA File Offset: 0x0002A6DA
	public void onHitAnimationComplete()
	{
		this.hitAnimation = false;
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x0002C4E3 File Offset: 0x0002A6E3
	public void SetSpriteProperties(SpriteLayer layer, int order)
	{
		this.spriteRenderer.sortingLayerName = layer.ToString();
		this.spriteRenderer.sortingOrder = order;
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x000FD968 File Offset: 0x000FBB68
	public void ResetSpriteProperties()
	{
		this.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString();
		this.spriteRenderer.sortingOrder = ((base.player.id != PlayerId.PlayerOne) ? -1 : 1);
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x000FD9B4 File Offset: 0x000FBBB4
	public void OnParryStart()
	{
		if (this.super)
		{
			return;
		}
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, true);
		}
		if ((base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone) && !base.GetComponent<IParryAttack>().AttackParryUsed && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, true);
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
		}
		this.SetTrigger(LevelPlayerAnimationController.Triggers.OnParry);
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x000FDA98 File Offset: 0x000FBC98
	public void OnParrySuccess()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, false);
		}
		if ((base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone) && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
		}
		this.SetAlpha(1f);
		if (base.player.stats.isChalice)
		{
			if (this.chaliceDashEffectActive != null)
			{
				Object.Destroy(this.chaliceDashEffectActive.gameObject);
			}
			base.animator.Play("Jump_Launch", 3, 0f);
		}
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x0002C509 File Offset: 0x0002A709
	public void OnParryPause()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.animator.enabled = false;
			this.spriteRenderer.GetComponent<LevelPlayerParryAnimator>().StartSet();
		}
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x0002C537 File Offset: 0x0002A737
	public void OnParryAnimEnd()
	{
		this.ResumeNormanAnim();
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x0002C53F File Offset: 0x0002A73F
	public void _ChaliceStartOnIdle4()
	{
		if (base.player.stats.isChalice)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, false);
			base.animator.Play("IdleFromFour", 3);
		}
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x0002C573 File Offset: 0x0002A773
	public void ResumeNormanAnim()
	{
		this.spriteRenderer.GetComponent<LevelPlayerParryAnimator>().StopSet();
		base.animator.enabled = true;
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x0002C591 File Offset: 0x0002A791
	public void OnGrounded()
	{
		if (!Level.Current.Started)
		{
			return;
		}
		AudioManager.Play("player_grounded");
		this.groundedEffect.Create(base.transform.position, base.transform.localScale);
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x000FDB7C File Offset: 0x000FBD7C
	public void OnEx()
	{
		if (base.player.stats.isChalice)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, true);
		}
		this.exDirection = "Forward";
		if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y > 0)
		{
			this.exDirection = "Up";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y > 0)
		{
			this.exDirection = "Diagonal_Up";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y < 0)
		{
			this.exDirection = "Down";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y < 0)
		{
			this.exDirection = "Diagonal_Down";
			AudioManager.Play("player_ex_forward_ground");
		}
		if (this.exDirection == "Forward")
		{
			AudioManager.Play("player_ex_forward_ground");
		}
		string text = "Ex." + this.exDirection + "_";
		if (base.player.motor.Grounded)
		{
			text += "Ground";
		}
		else
		{
			text += "Air";
		}
		this.Play(text);
		this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, !base.player.motor.Grounded);
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x000FDDC4 File Offset: 0x000FBFC4
	public void OnSuper()
	{
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		this.super = true;
		if (base.player.stats.isChalice)
		{
			this.shooting = false;
			this.ChaliceJumpShootHandling();
		}
		this.spriteRenderer.enabled = false;
		this.SwitchChaliceAim(-1);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x000FDE30 File Offset: 0x000FC030
	public void OnSuperEnd()
	{
		this.super = false;
		this.spriteRenderer.enabled = true;
		this.ResetSpriteProperties();
		if (base.player.stats.isChalice)
		{
			this.timeSinceStoppedShooting = 1f;
			if (base.player.stats.Loadout.super == Super.level_super_chalice_shield)
			{
				base.StartCoroutine(this.end_chalice_super_cr((!base.player.motor.Grounded) ? this.ChaliceSuper2ReturnAir : this.ChaliceSuper2Return));
			}
			if (base.player.stats.Loadout.super == Super.level_super_chalice_vert_beam)
			{
				base.StartCoroutine(this.end_chalice_super_cr(this.ChaliceSuper1Return));
			}
		}
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x000FDEFC File Offset: 0x000FC0FC
	public bool ExitingChaliceSuper()
	{
		int shortNameHash = base.animator.GetCurrentAnimatorStateInfo(3).shortNameHash;
		return shortNameHash == this.ChaliceSuper1Return || shortNameHash == this.ChaliceSuper2Return || shortNameHash == this.ChaliceSuper2ReturnAir;
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x000FDF44 File Offset: 0x000FC144
	public IEnumerator end_chalice_super_cr(int animState)
	{
		base.animator.Play(animState, 3, 0f);
		base.animator.Update(0f);
		if (base.player.weaponManager.allowInput)
		{
			base.player.weaponManager.DisableInput();
			while (base.animator.GetCurrentAnimatorStateInfo(3).shortNameHash == animState)
			{
				yield return null;
			}
			base.player.weaponManager.EnableInput();
		}
		yield break;
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x0002C5CF File Offset: 0x0002A7CF
	public void _OnSuperAnimEnd()
	{
		base.player.UnpauseAll(false);
		base.player.motor.OnSuperEnd();
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x0002C5ED File Offset: 0x0002A7ED
	public void SetOldMaterial()
	{
		this.spriteRenderer.material = this.tempMaterial;
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x0002C600 File Offset: 0x0002A800
	public void SetMaterial(Material m)
	{
		this.tempMaterial = this.spriteRenderer.material;
		this.spriteRenderer.material = m;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x0002C61F File Offset: 0x0002A81F
	public Material GetMaterial()
	{
		return this.spriteRenderer.material;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x0002C62C File Offset: 0x0002A82C
	public SpriteRenderer GetSpriteRenderer()
	{
		return this.spriteRenderer;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x0002C634 File Offset: 0x0002A834
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dashEffect = null;
		this.groundedEffect = null;
		this.hitEffect = null;
		this.runEffect = null;
		this.smokeDashEffect = null;
		this.powerUpBurstEffect = null;
		this.cupheadScaredSprite = null;
		this.mugmanScaredSprite = null;
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x0002C674 File Offset: 0x0002A874
	public void Play(string animation)
	{
		base.animator.Play(animation, 0, 0f);
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x0002C688 File Offset: 0x0002A888
	public bool GetBool(int b)
	{
		return base.animator.GetBool(b);
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x0002C696 File Offset: 0x0002A896
	public void SetBool(int b, bool value)
	{
		base.animator.SetBool(b, value);
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x0002C6A5 File Offset: 0x0002A8A5
	public int GetInt(int i)
	{
		return base.animator.GetInteger(i);
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x0002C6B3 File Offset: 0x0002A8B3
	public void SetInt(int i, int value)
	{
		base.animator.SetInteger(i, value);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x0002C6C2 File Offset: 0x0002A8C2
	public void SetTrigger(int t)
	{
		base.animator.SetTrigger(t);
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x0002C6D0 File Offset: 0x0002A8D0
	public void ResetTrigger(int t)
	{
		base.animator.ResetTrigger(t);
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x000FDF68 File Offset: 0x000FC168
	public void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x000FDF98 File Offset: 0x000FC198
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x000FDFD0 File Offset: 0x000FC1D0
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x0002C6DE File Offset: 0x0002A8DE
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x0002C701 File Offset: 0x0002A901
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x000FE014 File Offset: 0x000FC214
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

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x0600363E RID: 13886 RVA: 0x0002C721 File Offset: 0x0002A921
	public bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x000FE040 File Offset: 0x000FC240
	public IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
			while (!this.Flashing)
			{
				yield return true;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.417f);
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
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x0002C736 File Offset: 0x0002A936
	public void SoundIntroPowerup()
	{
		if (!this.intropowerupactive)
		{
			AudioManager.Play("player_powerup");
			this.emitAudioFromObject.Add("player_powerup");
			this.intropowerupactive = true;
		}
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x0002C764 File Offset: 0x0002A964
	public void SoundParryAxe()
	{
		AudioManager.Play("player_parry_axe");
		this.emitAudioFromObject.Add("player_parry_axe");
	}

	// Token: 0x04002BD2 RID: 11218
	public const int PALADIN_SHADOW_BUFFER_SIZE = 10;

	// Token: 0x04002BD3 RID: 11219
	public int ChaliceSuper1Return = Animator.StringToHash("Chalice_Super_1_Return");

	// Token: 0x04002BD4 RID: 11220
	public int ChaliceSuper2Return = Animator.StringToHash("Chalice_Super_2_Return");

	// Token: 0x04002BD5 RID: 11221
	public int ChaliceSuper2ReturnAir = Animator.StringToHash("Chalice_Super_2_Return_Air");

	// Token: 0x04002BD6 RID: 11222
	public int ChaliceAirEXRecovery = Animator.StringToHash("Air_EX_Recovery");

	// Token: 0x04002BD7 RID: 11223
	public int ChaliceJumpBall = Animator.StringToHash("Jump_Ball");

	// Token: 0x04002BD8 RID: 11224
	public int ChaliceJumpDescend = Animator.StringToHash("Jump_Descend");

	// Token: 0x04002BD9 RID: 11225
	[SerializeField]
	public GameObject cuphead;

	// Token: 0x04002BDA RID: 11226
	[SerializeField]
	public GameObject mugman;

	// Token: 0x04002BDB RID: 11227
	[SerializeField]
	public GameObject chalice;

	// Token: 0x04002BDC RID: 11228
	[SerializeField]
	public SpriteRenderer[] chaliceSprites;

	// Token: 0x04002BDD RID: 11229
	[Space(10f)]
	[SerializeField]
	public Transform runDustRoot;

	// Token: 0x04002BDE RID: 11230
	[SerializeField]
	public Transform sparkleRoot;

	// Token: 0x04002BDF RID: 11231
	[Space(10f)]
	[SerializeField]
	public Effect dashEffect;

	// Token: 0x04002BE0 RID: 11232
	[SerializeField]
	public Effect groundedEffect;

	// Token: 0x04002BE1 RID: 11233
	[SerializeField]
	public Effect hitEffect;

	// Token: 0x04002BE2 RID: 11234
	[SerializeField]
	public Effect runEffect;

	// Token: 0x04002BE3 RID: 11235
	[SerializeField]
	public Effect curseEffect;

	// Token: 0x04002BE4 RID: 11236
	[SerializeField]
	public Effect smokeDashEffect;

	// Token: 0x04002BE5 RID: 11237
	[SerializeField]
	public HealerCharmSparkEffect healerCharmEffect;

	// Token: 0x04002BE6 RID: 11238
	[SerializeField]
	public Effect powerUpBurstEffect;

	// Token: 0x04002BE7 RID: 11239
	[SerializeField]
	public Effect chaliceDoubleJumpEffect;

	// Token: 0x04002BE8 RID: 11240
	[SerializeField]
	public Effect chaliceDashEffect;

	// Token: 0x04002BE9 RID: 11241
	public Effect chaliceDashEffectActive;

	// Token: 0x04002BEA RID: 11242
	[SerializeField]
	public Effect chaliceDashSparkle;

	// Token: 0x04002BEB RID: 11243
	[SerializeField]
	public SpriteRenderer[] chaliceJumpShootRenderers;

	// Token: 0x04002BEC RID: 11244
	[SerializeField]
	public Material chaliceDuckDashMaterial;

	// Token: 0x04002BED RID: 11245
	[SerializeField]
	public Effect chaliceDuckDashSparkles;

	// Token: 0x04002BEE RID: 11246
	public Coroutine chaliceInvincibleSparklesCoroutine;

	// Token: 0x04002BEF RID: 11247
	public bool chaliceFellFromDuckDash;

	// Token: 0x04002BF0 RID: 11248
	[SerializeField]
	public LevelPlayerChaliceIntroAnimation chaliceIntroAnimation;

	// Token: 0x04002BF1 RID: 11249
	public LevelPlayerChaliceIntroAnimation chaliceIntroCurrent;

	// Token: 0x04002BF2 RID: 11250
	[SerializeField]
	public Sprite cupheadScaredSprite;

	// Token: 0x04002BF3 RID: 11251
	[SerializeField]
	public Sprite mugmanScaredSprite;

	// Token: 0x04002BF5 RID: 11253
	public bool hitAnimation;

	// Token: 0x04002BF6 RID: 11254
	public bool super;

	// Token: 0x04002BF7 RID: 11255
	public bool shooting;

	// Token: 0x04002BF8 RID: 11256
	public bool fired;

	// Token: 0x04002BF9 RID: 11257
	public bool intropowerupactive;

	// Token: 0x04002BFA RID: 11258
	public string exDirection;

	// Token: 0x04002BFB RID: 11259
	public Trilean2 lastTrueLookDir = new Trilean2(1, 0);

	// Token: 0x04002BFC RID: 11260
	public float timeSinceStoppedShooting = 100f;

	// Token: 0x04002BFD RID: 11261
	public Material tempMaterial;

	// Token: 0x04002BFE RID: 11262
	public const float STOP_SHOOTING_DELAY = 0.0833f;

	// Token: 0x04002BFF RID: 11263
	public bool isIntroB;

	// Token: 0x04002C00 RID: 11264
	public bool chaliceActivated;

	// Token: 0x04002C01 RID: 11265
	public bool inScaredIntro;

	// Token: 0x04002C02 RID: 11266
	[SerializeField]
	public float curseEffectDelay = 0.15f;

	// Token: 0x04002C03 RID: 11267
	[SerializeField]
	public MinMax curseAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x04002C04 RID: 11268
	[SerializeField]
	public MinMax curseDistanceRange = new MinMax(0f, 20f);

	// Token: 0x04002C05 RID: 11269
	public float curseEffectAngle;

	// Token: 0x04002C06 RID: 11270
	public float curseEffectTimer;

	// Token: 0x04002C07 RID: 11271
	public int curseCharmLevel = -1;

	// Token: 0x04002C08 RID: 11272
	public Vector3[] paladinShadowPosition;

	// Token: 0x04002C09 RID: 11273
	public Vector3[] paladinShadowScale;

	// Token: 0x04002C0A RID: 11274
	public Sprite[] paladinShadowSprite;

	// Token: 0x04002C0B RID: 11275
	[SerializeField]
	public SpriteRenderer[] paladinShadows;

	// Token: 0x04002C0C RID: 11276
	public bool showCurseFX;

	// Token: 0x04002C0D RID: 11277
	public IEnumerator colorCoroutine;

	// Token: 0x0200117D RID: 4477
	public static class Booleans
	{
		// Token: 0x04007AA2 RID: 31394
		public static readonly int Dashing = Animator.StringToHash("Dashing");

		// Token: 0x04007AA3 RID: 31395
		public static readonly int Locked = Animator.StringToHash("Locked");

		// Token: 0x04007AA4 RID: 31396
		public static readonly int Shooting = Animator.StringToHash("Shooting");

		// Token: 0x04007AA5 RID: 31397
		public static readonly int Grounded = Animator.StringToHash("Grounded");

		// Token: 0x04007AA6 RID: 31398
		public static readonly int Turning = Animator.StringToHash("Turning");

		// Token: 0x04007AA7 RID: 31399
		public static readonly int Intro = Animator.StringToHash("Intro");

		// Token: 0x04007AA8 RID: 31400
		public static readonly int Dead = Animator.StringToHash("Dead");

		// Token: 0x04007AA9 RID: 31401
		public static readonly int HasParryCharm = Animator.StringToHash("HasParryCharm");

		// Token: 0x04007AAA RID: 31402
		public static readonly int HasParryAttack = Animator.StringToHash("HasParryAttack");

		// Token: 0x04007AAB RID: 31403
		public static readonly int ChaliceOffIdle = Animator.StringToHash("ChaliceOffIdle");

		// Token: 0x04007AAC RID: 31404
		public static readonly int DoubleJump = Animator.StringToHash("DoubleJump");

		// Token: 0x04007AAD RID: 31405
		public static readonly int ChaliceAirEX = Animator.StringToHash("ChaliceAirEX");
	}

	// Token: 0x0200117E RID: 4478
	public static class Integers
	{
		// Token: 0x04007AAE RID: 31406
		public static readonly int MoveX = Animator.StringToHash("MoveX");

		// Token: 0x04007AAF RID: 31407
		public static readonly int MoveY = Animator.StringToHash("MoveY");

		// Token: 0x04007AB0 RID: 31408
		public static readonly int LookX = Animator.StringToHash("LookX");

		// Token: 0x04007AB1 RID: 31409
		public static readonly int LookY = Animator.StringToHash("LookY");

		// Token: 0x04007AB2 RID: 31410
		public static readonly int ChaliceJumpDescendLoopCounter = Animator.StringToHash("ChaliceJumpDescendLoopCounter");
	}

	// Token: 0x0200117F RID: 4479
	public static class Triggers
	{
		// Token: 0x04007AB3 RID: 31411
		public static readonly int OnJump = Animator.StringToHash("OnJump");

		// Token: 0x04007AB4 RID: 31412
		public static readonly int OnGround = Animator.StringToHash("OnGround");

		// Token: 0x04007AB5 RID: 31413
		public static readonly int OnParry = Animator.StringToHash("OnParry");

		// Token: 0x04007AB6 RID: 31414
		public static readonly int OnWin = Animator.StringToHash("OnWin");

		// Token: 0x04007AB7 RID: 31415
		public static readonly int OnTurn = Animator.StringToHash("OnTurn");

		// Token: 0x04007AB8 RID: 31416
		public static readonly int OnFire = Animator.StringToHash("OnFire");
	}

	// Token: 0x02001180 RID: 4480
	public enum AnimLayers
	{
		// Token: 0x04007ABA RID: 31418
		Base,
		// Token: 0x04007ABB RID: 31419
		ShootRun,
		// Token: 0x04007ABC RID: 31420
		ShootRunDiag,
		// Token: 0x04007ABD RID: 31421
		ChaliceSpecial,
		// Token: 0x04007ABE RID: 31422
		ChaliceSync,
		// Token: 0x04007ABF RID: 31423
		ChaliceShootRun,
		// Token: 0x04007AC0 RID: 31424
		ChaliceShootRunDiag
	}

	// Token: 0x02001181 RID: 4481
	public enum ChaliceAim
	{
		// Token: 0x04007AC2 RID: 31426
		UpAim,
		// Token: 0x04007AC3 RID: 31427
		DiagUpAim,
		// Token: 0x04007AC4 RID: 31428
		ForwardAim,
		// Token: 0x04007AC5 RID: 31429
		DiagDownAim,
		// Token: 0x04007AC6 RID: 31430
		DownAim
	}
}
