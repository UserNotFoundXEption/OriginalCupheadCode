using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class PlanePlayerAnimationController : AbstractPlanePlayerComponent
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06003952 RID: 14674 RVA: 0x0010B934 File Offset: 0x00109B34
	public Transform activeTransform
	{
		get
		{
			if (base.player.stats.isChalice)
			{
				return this.chalice;
			}
			if (PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne)
			{
				return this.mugman;
			}
			return this.cuphead;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06003953 RID: 14675 RVA: 0x0002EB23 File Offset: 0x0002CD23
	// (set) Token: 0x06003954 RID: 14676 RVA: 0x0002EB2B File Offset: 0x0002CD2B
	public SpriteRenderer spriteRenderer { get; set; }

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06003955 RID: 14677 RVA: 0x0002EB34 File Offset: 0x0002CD34
	// (set) Token: 0x06003956 RID: 14678 RVA: 0x0002EB3C File Offset: 0x0002CD3C
	public PlanePlayerAnimationController.ShrinkStates ShrinkState { get; set; }

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06003957 RID: 14679 RVA: 0x0002EB45 File Offset: 0x0002CD45
	// (set) Token: 0x06003958 RID: 14680 RVA: 0x0002EB4D File Offset: 0x0002CD4D
	public bool Shrinking { get; set; }

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x06003959 RID: 14681 RVA: 0x0010B984 File Offset: 0x00109B84
	// (remove) Token: 0x0600395A RID: 14682 RVA: 0x0010B9BC File Offset: 0x00109BBC
	public event Action OnExFireAnimEvent;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x0600395B RID: 14683 RVA: 0x0010B9F4 File Offset: 0x00109BF4
	// (remove) Token: 0x0600395C RID: 14684 RVA: 0x0010BA2C File Offset: 0x00109C2C
	public event Action OnShrinkEvent;

	// Token: 0x0600395D RID: 14685 RVA: 0x0010BA64 File Offset: 0x00109C64
	public void Start()
	{
		base.player.weaponManager.OnExStartEvent += this.OnExStart;
		base.player.weaponManager.OnSuperStartEvent += this.OnSuperStart;
		base.player.parryController.OnParryStartEvent += this.OnParryStart;
		base.player.parryController.OnParrySuccessEvent += this.OnParrySuccess;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.stats.OnPlayerDeathEvent += this.OnDeath;
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnStoneShake += this.onStoneShake;
		base.player.stats.OnStoned += this.onStoned;
		if (this.spriteRenderer == null)
		{
			this.spriteRenderer = this.playerSprite.GetComponent<SpriteRenderer>();
		}
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chalice.GetComponent<SpriteRenderer>().sharedMaterial, SettingsData.Data.filter == BlurGamma.Filter.Chalice);
		if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.curseCharmLevel = CharmCurse.CalculateLevel(base.player.id);
		}
		if (this.curseCharmLevel > -1)
		{
			this.InitializeCurseFX();
		}
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x0002EB56 File Offset: 0x0002CD56
	public void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
		this.CheckActivateCurseFX();
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x0010BBF4 File Offset: 0x00109DF4
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

	// Token: 0x06003960 RID: 14688 RVA: 0x0002EB6B File Offset: 0x0002CD6B
	public void Update()
	{
		if (this.curseCharmLevel > -1)
		{
			this.HandleCurseFX();
		}
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x0010BC44 File Offset: 0x00109E44
	public void FixedUpdate()
	{
		this.HandleRotation();
		this.HandleShrunk();
		this.SetInteger("Y", base.player.motor.MoveDirection.y);
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x0010BC88 File Offset: 0x00109E88
	public void LevelInit()
	{
		PlayerId id = base.player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			this.playerSprite = ((!base.player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice);
		}
		else
		{
			this.playerSprite = ((!base.player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
		}
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		if (Level.Current.Started)
		{
			this.playerSprite.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x0010BD94 File Offset: 0x00109F94
	public void PlayIntro()
	{
		string str = ((base.player.id != PlayerId.PlayerOne || !PlayerManager.player1IsMugman) && (base.player.id != PlayerId.PlayerTwo || PlayerManager.player1IsMugman)) ? "Cuphead" : "Mugman";
		if (base.player.stats.isChalice)
		{
			base.animator.Play("Intro_Chalice_" + str + ((base.player.id != PlayerId.PlayerOne) ? "_P2" : string.Empty));
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_chalice && !base.player.stats.isChalice)
		{
			base.animator.Play("Intro_Chalice_" + str + "_Fail");
		}
		else
		{
			PlayerId id = base.player.id;
			if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
			{
				base.animator.Play("Intro");
			}
			else
			{
				base.animator.Play("Intro_Alt");
			}
		}
		this.spriteRenderer = this.playerSprite.GetComponent<SpriteRenderer>();
		this.playerSprite.gameObject.SetActive(true);
		if (base.gameObject.activeSelf)
		{
			if (!Level.Current.Started && base.player.id == PlayerId.PlayerTwo && base.player.stats.Loadout.charm != Charm.charm_chalice)
			{
				this.playerSprite.SetLocalPosition(new float?(this.introRoot.transform.localPosition.x), new float?(this.introRoot.transform.localPosition.y), new float?(0f));
			}
			base.StartCoroutine(this.done_intro_cr());
		}
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x0010BFA0 File Offset: 0x0010A1A0
	public IEnumerator done_intro_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, (base.player.id != PlayerId.PlayerOne) ? "Intro_Alt" : "Intro", false, true);
		base.StartCoroutine(this.puff_cr());
		this.CheckActivateCurseFX();
		yield return null;
		yield break;
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x0010BFBC File Offset: 0x0010A1BC
	public void CheckActivateCurseFX()
	{
		if (this.curseCharmLevel == 4 && this.paladinShadows != null && this.paladinShadowSprite.Length == 10)
		{
			if (this.paladinShadows[0] != null)
			{
				this.paladinShadows[0].enabled = true;
			}
			if (this.paladinShadows[1] != null)
			{
				this.paladinShadows[1].enabled = true;
			}
		}
		this.showCurseFX = true;
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x0010C038 File Offset: 0x0010A238
	public void ResetPosition()
	{
		this.playerSprite.SetLocalPosition(new float?(0f), new float?(0f), null);
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x0010C070 File Offset: 0x0010A270
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.Health <= 0 || info.damage <= 0f)
		{
			return;
		}
		this.hitSparkEffect.Create(base.player.center);
		this.hitDustEffect.Create(base.player.center);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x0002EB7F File Offset: 0x0002CD7F
	public void OnHealerCharm()
	{
		this.healerCharmEffect.Create(base.player.center, base.transform.localScale, base.player);
		AudioManager.Play("player_charmhealer_extraheart");
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x0002EBB2 File Offset: 0x0002CDB2
	public void SetOldMaterial()
	{
		this.spriteRenderer.material = this.tempMaterial;
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x0002EBC5 File Offset: 0x0002CDC5
	public void SetMaterial(Material m)
	{
		this.tempMaterial = this.spriteRenderer.material;
		this.spriteRenderer.material = m;
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
	public Material GetMaterial()
	{
		return this.spriteRenderer.material;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x0002EBF1 File Offset: 0x0002CDF1
	public SpriteRenderer GetSpriteRenderer()
	{
		return this.spriteRenderer;
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x0010C0E8 File Offset: 0x0010A2E8
	public void onStoned()
	{
		this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready;
		base.animator.SetLayerWeight(1, 0f);
		base.StopCoroutine(this.ex_cr());
		this.greenPrefab.Create(base.player.center);
		base.animator.Play("Stone_Idle");
		base.animator.ResetTrigger("Breakout");
		base.StartCoroutine(this.stone_animation_cr());
		base.StartCoroutine(this.create_poofs_cr());
		this.isStoned = true;
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x0010C174 File Offset: 0x0010A374
	public IEnumerator stone_animation_cr()
	{
		while (base.player.stats.StoneTime > 0f)
		{
			yield return null;
		}
		base.animator.SetTrigger("Breakout");
		AnimatorStateInfo animState = base.animator.GetCurrentAnimatorStateInfo(0);
		while (animState.IsName("Stone_Idle") || animState.IsName("Stone_Shake_A") || animState.IsName("Stone_Shake_B") || animState.IsName("Stone_Shake_C") || animState.IsName("Stone_Shake_C") || animState.IsName("Stone_Shake_D") || animState.IsName("Stone_Shake_E") || animState.IsName("Breakout"))
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x0002EBF9 File Offset: 0x0002CDF9
	public void Breakout()
	{
		this.isStoned = false;
		this.breakoutPrefab.Create(base.player.center).transform.parent = base.transform;
		base.StopCoroutine(this.create_poofs_cr());
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x0002EC34 File Offset: 0x0002CE34
	public void onStoneShake()
	{
		base.animator.SetTrigger("Shake");
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x0010C190 File Offset: 0x0010A390
	public IEnumerator create_poofs_cr()
	{
		float t = 0f;
		float time = 0.1f;
		while (base.player.stats.StoneTime > 0f)
		{
			if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Stone_Idle") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Breakout"))
			{
				string layerName = (!Rand.Bool()) ? SpriteLayer.Effects.ToString() : SpriteLayer.Enemies.ToString();
				Effect poof = Object.Instantiate<Effect>(this.poofPrefab);
				poof.transform.position = base.player.center;
				poof.animator.SetInteger("Poof", Random.Range(0, 3));
				poof.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
				while (t < time)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x0010C1AC File Offset: 0x0010A3AC
	public void HandleRotation()
	{
		float num = 0f;
		if (base.player.motor.MoveDirection.x < 0)
		{
			num = 9f;
		}
		else if (base.player.motor.MoveDirection.x > 0)
		{
			num = -9f;
		}
		if (base.player.Shrunk && !base.player.stats.isChalice)
		{
			num += 5f * (float)(-(float)base.player.motor.MoveDirection.x);
		}
		this.rotation = Mathf.Lerp(this.rotation, num, 7f * CupheadTime.FixedDelta);
		this.activeTransform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation));
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x0010C2AC File Offset: 0x0010A4AC
	public void HandleShrunk()
	{
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
		{
			if (this.shrinkCooldownTimeLeft <= 0f)
			{
				this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready;
			}
			this.shrinkCooldownTimeLeft -= CupheadTime.FixedDelta;
		}
		if (base.player.Parrying || base.player.WeaponBusy || base.player.stats.StoneTime > 0f || this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
		{
			return;
		}
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Ready && (base.player.input.actions.GetButtonDown(7) || base.player.input.actions.GetButtonDown(6)))
		{
			base.animator.SetLayerWeight(1, 1f);
			base.animator.Play("Shrink_In", 0);
			this.Shrinking = true;
			this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk;
			if (this.OnShrinkEvent != null)
			{
				this.OnShrinkEvent();
			}
			if (base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash)
			{
				this.smokeDashEffect.Create(base.player.center);
			}
			AudioManager.Play("player_plane_shrink");
		}
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Shrunk && !base.player.input.actions.GetButton(7) && !base.player.input.actions.GetButton(6))
		{
			this.Shrinking = false;
			base.animator.SetLayerWeight(1, 0f);
			base.animator.Play("Shrink_Out", 0);
			this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown;
			this.shrinkCooldownTimeLeft = 0.23300001f;
			AudioManager.Play("player_plane_expand");
		}
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x0010C498 File Offset: 0x0010A698
	public IEnumerator bomb_cr()
	{
		yield return null;
		base.animator.SetLayerWeight(2, 1f);
		float t = 0f;
		float[] slowShakeScales = new float[]
		{
			1f,
			1.184f,
			1.09f
		};
		float[] fastShakeScales = new float[]
		{
			1f,
			1.184f,
			1.09f,
			1.34f,
			1.09f,
			1.184f
		};
		while (base.player.weaponManager.states.super == PlanePlayerWeaponManager.States.Super.Intro)
		{
			yield return null;
		}
		while (base.player.weaponManager.states.super == PlanePlayerWeaponManager.States.Super.Countdown)
		{
			if (t < 0.4f * WeaponProperties.PlaneSuperBomb.countdownTime)
			{
				yield return null;
				t += CupheadTime.Delta;
			}
			else if (t < 0.7f * WeaponProperties.PlaneSuperBomb.countdownTime)
			{
				foreach (float scale in slowShakeScales)
				{
					base.transform.SetScale(new float?(scale), new float?(scale), null);
					yield return CupheadTime.WaitForSeconds(this, 0.0833333358f);
					t += 0.0833333358f;
				}
			}
			else
			{
				foreach (float scale2 in fastShakeScales)
				{
					base.transform.SetScale(new float?(scale2), new float?(scale2), null);
					yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
					t += 0.0416666679f;
				}
			}
		}
		base.animator.SetLayerWeight(2, 0f);
		base.transform.SetScale(new float?(1f), new float?(1f), null);
		yield break;
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x0002EC46 File Offset: 0x0002CE46
	public void SetSpriteVisible(bool visible)
	{
		this.playerSprite.gameObject.SetActive(visible);
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x0010C4B4 File Offset: 0x0010A6B4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.breakoutPrefab = null;
		this.poofPrefab = null;
		this.greenPrefab = null;
		this.puffPrefab = null;
		this.hitSparkEffect = null;
		this.hitDustEffect = null;
		this.smokeDashEffect = null;
		this.shrinkEffect = null;
		this.growEffect = null;
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x0010C508 File Offset: 0x0010A708
	public void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x0002EC59 File Offset: 0x0002CE59
	public void OnShrinkInComplete()
	{
		this.shrinkEffect.Create(base.player.center);
		this.Shrinking = false;
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x0002EC79 File Offset: 0x0002CE79
	public void OnShrinkOutComplete()
	{
		this.growEffect.Create(base.player.center);
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x0010C538 File Offset: 0x0010A738
	public void CreatePuff()
	{
		if (this.playerSprite == null)
		{
			return;
		}
		PlaneLevelEffect planeLevelEffect = this.puffPrefab.Create(this.playerSprite.position + PlanePlayerAnimationController.PUFF_OFFSET) as PlaneLevelEffect;
		if (base.player.motor.MoveDirection.x < 0)
		{
			planeLevelEffect.speed = 2f;
		}
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x0010C5B0 File Offset: 0x0010A7B0
	public IEnumerator puff_cr()
	{
		float delay = 0.17f;
		for (;;)
		{
			delay = 0.17f;
			if ((base.player.motor.MoveDirection.x != 0 || base.player.motor.MoveDirection.y != 0) && base.player.motor.MoveDirection.x >= 0)
			{
				delay = 0.07f;
			}
			if (base.player.motor.MoveDirection.x >= 0)
			{
				this.CreatePuff();
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x0010C5CC File Offset: 0x0010A7CC
	public void InitializeCurseFX()
	{
		this.curseEffectAngle = (float)Random.Range(0, 360);
		if (this.curseCharmLevel == 4)
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
			if (this.paladinShadows != null)
			{
				this.paladinShadows[0].transform.position = base.transform.position;
				this.paladinShadows[1].transform.position = base.transform.position;
				this.paladinShadows[0].sprite = this.spriteRenderer.sprite;
				this.paladinShadows[1].sprite = this.spriteRenderer.sprite;
				this.paladinShadows[0].transform.parent = null;
				this.paladinShadows[1].transform.parent = null;
			}
		}
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x0010C718 File Offset: 0x0010A918
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
			effect.transform.localScale = new Vector3(0.8f, 0.8f);
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

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x0002EC92 File Offset: 0x0002CE92
	public bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x0010CA88 File Offset: 0x0010AC88
	public IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
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

	// Token: 0x06003980 RID: 14720 RVA: 0x0002ECA7 File Offset: 0x0002CEA7
	public void OnExStart()
	{
		base.StartCoroutine(this.ex_cr());
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x0010CAA4 File Offset: 0x0010ACA4
	public IEnumerator ex_cr()
	{
		string dir = (base.player.motor.MoveDirection.y > 0) ? "Up" : "Down";
		base.animator.Play("Ex_" + dir);
		if (dir == "Up")
		{
			AudioManager.Play("player_plane_up_ex");
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Ex_" + dir, false, true);
		if (this.OnExFireAnimEvent != null)
		{
			this.OnExFireAnimEvent();
		}
		yield break;
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x0002ECB6 File Offset: 0x0002CEB6
	public void OnSuperStart()
	{
		base.StartCoroutine(this.bomb_cr());
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x0010CAC0 File Offset: 0x0010ACC0
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x0010CAF8 File Offset: 0x0010ACF8
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x0002ECC5 File Offset: 0x0002CEC5
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x0002ECE8 File Offset: 0x0002CEE8
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x0010CB3C File Offset: 0x0010AD3C
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

	// Token: 0x06003988 RID: 14728 RVA: 0x0010CB68 File Offset: 0x0010AD68
	public void OnParryStart()
	{
		if (this.isStoned)
		{
			this.Breakout();
		}
		base.animator.SetBool("ParrySuccess", false);
		base.animator.SetBool("ParryPlusCharm", base.player.stats.Loadout.charm == Charm.charm_parry_plus);
		if (base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone)
		{
			base.animator.Play("ParryAttack");
		}
		else
		{
			base.animator.Play("Parry");
		}
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x0002ED08 File Offset: 0x0002CF08
	public void OnParrySuccess()
	{
		base.animator.SetBool("ParrySuccess", true);
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x0010CC1C File Offset: 0x0010AE1C
	public void OnDeath(PlayerId playerId)
	{
		foreach (PlanePlayerDeathPart planePlayerDeathPart in this.deathPieces)
		{
			planePlayerDeathPart.CreatePart(base.player.id, base.transform.position);
		}
		this.deathEffect.Create(base.transform.position);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0002ED1B File Offset: 0x0002CF1B
	public void OnRevive(Vector3 pos)
	{
		this.SetAlpha(1f);
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x0002ED28 File Offset: 0x0002CF28
	public void SetInteger(string integer, int value)
	{
		base.animator.SetInteger(integer, value);
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x0002ED37 File Offset: 0x0002CF37
	public void SetTrigger(string trigger)
	{
		base.animator.SetTrigger(trigger);
	}

	// Token: 0x04002E12 RID: 11794
	public const int PALADIN_SHADOW_BUFFER_SIZE = 10;

	// Token: 0x04002E13 RID: 11795
	public const float ROTATION_MAX = 9f;

	// Token: 0x04002E14 RID: 11796
	public const float SHUNK_ROTATION_ADD = 5f;

	// Token: 0x04002E15 RID: 11797
	public const float ROTATION_SPEED = 7f;

	// Token: 0x04002E16 RID: 11798
	public const float INTRO_X = -150f;

	// Token: 0x04002E17 RID: 11799
	public const float PUFF_DELAY = 0.17f;

	// Token: 0x04002E18 RID: 11800
	public const float PUFF_DELAY_MOVING = 0.07f;

	// Token: 0x04002E19 RID: 11801
	public static readonly Vector2 PUFF_OFFSET = new Vector3(-50f, 0f);

	// Token: 0x04002E1A RID: 11802
	public const float SHRINK_COOLDOWN = 0.23300001f;

	// Token: 0x04002E1B RID: 11803
	[SerializeField]
	public Transform cuphead;

	// Token: 0x04002E1C RID: 11804
	[SerializeField]
	public Transform mugman;

	// Token: 0x04002E1D RID: 11805
	[SerializeField]
	public Transform chalice;

	// Token: 0x04002E1E RID: 11806
	[Space(10f)]
	[SerializeField]
	public Transform introRoot;

	// Token: 0x04002E1F RID: 11807
	[Space(10f)]
	[SerializeField]
	public Effect breakoutPrefab;

	// Token: 0x04002E20 RID: 11808
	[SerializeField]
	public Effect poofPrefab;

	// Token: 0x04002E21 RID: 11809
	[SerializeField]
	public Effect greenPrefab;

	// Token: 0x04002E22 RID: 11810
	[SerializeField]
	public PlaneLevelEffect puffPrefab;

	// Token: 0x04002E23 RID: 11811
	[Space(10f)]
	[SerializeField]
	public Effect hitSparkEffect;

	// Token: 0x04002E24 RID: 11812
	[SerializeField]
	public Effect hitDustEffect;

	// Token: 0x04002E25 RID: 11813
	[SerializeField]
	public Effect smokeDashEffect;

	// Token: 0x04002E26 RID: 11814
	[SerializeField]
	public HealerCharmSparkEffect healerCharmEffect;

	// Token: 0x04002E27 RID: 11815
	[SerializeField]
	public Effect curseEffect;

	// Token: 0x04002E28 RID: 11816
	[Space(10f)]
	[SerializeField]
	public Effect shrinkEffect;

	// Token: 0x04002E29 RID: 11817
	[SerializeField]
	public Effect growEffect;

	// Token: 0x04002E2A RID: 11818
	[Space(10f)]
	[SerializeField]
	public PlanePlayerDeathPart[] deathPieces;

	// Token: 0x04002E2B RID: 11819
	[SerializeField]
	public PlaneLevelEffect deathEffect;

	// Token: 0x04002E2C RID: 11820
	public Transform playerSprite;

	// Token: 0x04002E2E RID: 11822
	public float rotation;

	// Token: 0x04002E2F RID: 11823
	public float shrinkCooldownTimeLeft;

	// Token: 0x04002E30 RID: 11824
	public Material tempMaterial;

	// Token: 0x04002E33 RID: 11827
	public bool isStoned;

	// Token: 0x04002E34 RID: 11828
	[SerializeField]
	public float curseEffectDelay = 0.15f;

	// Token: 0x04002E35 RID: 11829
	[SerializeField]
	public MinMax curseAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x04002E36 RID: 11830
	[SerializeField]
	public MinMax curseDistanceRange = new MinMax(0f, 20f);

	// Token: 0x04002E37 RID: 11831
	public float curseEffectAngle;

	// Token: 0x04002E38 RID: 11832
	public float curseEffectTimer;

	// Token: 0x04002E39 RID: 11833
	public int curseCharmLevel = -1;

	// Token: 0x04002E3A RID: 11834
	public Vector3[] paladinShadowPosition;

	// Token: 0x04002E3B RID: 11835
	public Vector3[] paladinShadowScale;

	// Token: 0x04002E3C RID: 11836
	public Sprite[] paladinShadowSprite;

	// Token: 0x04002E3D RID: 11837
	[SerializeField]
	public SpriteRenderer[] paladinShadows;

	// Token: 0x04002E3E RID: 11838
	public bool showCurseFX;

	// Token: 0x04002E41 RID: 11841
	public IEnumerator colorCoroutine;

	// Token: 0x020011CE RID: 4558
	public enum ShrinkStates
	{
		// Token: 0x04007C5E RID: 31838
		Ready,
		// Token: 0x04007C5F RID: 31839
		Shrunk,
		// Token: 0x04007C60 RID: 31840
		Cooldown
	}

	// Token: 0x020011CF RID: 4559
	public enum AnimLayers
	{
		// Token: 0x04007C62 RID: 31842
		Base,
		// Token: 0x04007C63 RID: 31843
		Shrunk,
		// Token: 0x04007C64 RID: 31844
		Bomb
	}
}
