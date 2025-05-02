using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class PlayerDeathEffect : AbstractMonoBehaviour
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x000FAA84 File Offset: 0x000F8C84
	public PlayerDeathEffect Create(PlayerId playerId, PlayerInput input, Vector2 pos, int deathCount, PlayerMode mode, bool canParry)
	{
		PlayerDeathEffect playerDeathEffect = Object.Instantiate<PlayerDeathEffect>(this);
		playerDeathEffect.name = playerDeathEffect.name.Replace("(Clone)", string.Empty);
		playerDeathEffect.Init(playerId, input, pos, deathCount, mode, canParry);
		return playerDeathEffect;
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x000FAAC4 File Offset: 0x000F8CC4
	public void CreateExplosionOnly(PlayerId playerId, Vector2 pos, PlayerMode mode)
	{
		if (mode != PlayerMode.Level)
		{
			if (mode == PlayerMode.Plane)
			{
				PlayerDeathEffect playerDeathEffect = Object.Instantiate<PlayerDeathEffect>(this);
				LevelPlayerDeathEffect levelPlayerDeathEffect = Object.Instantiate<LevelPlayerDeathEffect>(playerDeathEffect.explosionPrefab);
				levelPlayerDeathEffect.Init(pos);
				Object.Destroy(playerDeathEffect.gameObject);
			}
		}
		else
		{
			PlayerDeathEffect playerDeathEffect2 = Object.Instantiate<PlayerDeathEffect>(this);
			LevelPlayerDeathEffect levelPlayerDeathEffect2 = Object.Instantiate<LevelPlayerDeathEffect>(playerDeathEffect2.explosionPrefab);
			LevelPlayerController levelPlayerController = PlayerManager.GetPlayer(playerId) as LevelPlayerController;
			levelPlayerDeathEffect2.Init(pos, playerId, levelPlayerController.motor.Grounded);
			Object.Destroy(playerDeathEffect2.gameObject);
		}
	}

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x060035B4 RID: 13748 RVA: 0x000FAB54 File Offset: 0x000F8D54
	// (remove) Token: 0x060035B5 RID: 13749 RVA: 0x000FAB8C File Offset: 0x000F8D8C
	public event AbstractPlayerController.OnReviveHandler OnPreReviveEvent;

	// Token: 0x14000088 RID: 136
	// (add) Token: 0x060035B6 RID: 13750 RVA: 0x000FABC4 File Offset: 0x000F8DC4
	// (remove) Token: 0x060035B7 RID: 13751 RVA: 0x000FABFC File Offset: 0x000F8DFC
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x060035B8 RID: 13752 RVA: 0x0002C077 File Offset: 0x0002A277
	public override void Awake()
	{
		base.Awake();
		this.parrySwitch.OnActivate += this.OnParrySwitch;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x0002C0A8 File Offset: 0x0002A2A8
	public virtual void Start()
	{
		base.StartCoroutine(this.checkOutOfFrame_cr());
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x000FAC34 File Offset: 0x000F8E34
	public void Init(PlayerId playerId, PlayerInput input, Vector2 pos, int deathCount, PlayerMode mode, bool canParry)
	{
		this.playerInput = input;
		this.playerId = playerId;
		this.deathCount = deathCount;
		if (deathCount >= 10)
		{
			this.parrySwitch.gameObject.SetActive(false);
		}
		this.playerMode = mode;
		AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
		base.animator.SetInteger("Mode", (int)this.playerMode);
		base.animator.SetBool("CanParry", canParry);
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = ((!player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice);
		}
		else
		{
			this.spriteRenderer = ((!player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
		}
		this.effect.enabled = !PlayerManager.GetPlayer(playerId).stats.isChalice;
		this.chaliceEffect.enabled = PlayerManager.GetPlayer(playerId).stats.isChalice;
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		this.spriteRenderer.gameObject.SetActive(true);
		this.parrySwitch.gameObject.SetActive(canParry);
		base.transform.position = pos;
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x000FADDC File Offset: 0x000F8FDC
	public void OnAnimationComplete()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(this.playerId);
		LevelPlayerController levelPlayerController = (LevelPlayerController)player;
		LevelPlayerDeathEffect levelPlayerDeathEffect = Object.Instantiate<LevelPlayerDeathEffect>(this.explosionPrefab);
		levelPlayerDeathEffect.Init(base.transform.position, this.playerId, levelPlayerController.motor.Grounded);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x000FAE3C File Offset: 0x000F903C
	public void OnAnimationCompletePlane()
	{
		LevelPlayerDeathEffect levelPlayerDeathEffect = Object.Instantiate<LevelPlayerDeathEffect>(this.explosionPrefab);
		levelPlayerDeathEffect.Init(base.transform.position);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x000FAE78 File Offset: 0x000F9078
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x000FAEA8 File Offset: 0x000F90A8
	public virtual void OnParrySwitch()
	{
		if (this.exiting)
		{
			return;
		}
		this.exiting = true;
		this.StopAllCoroutines();
		this.parrySwitch.gameObject.SetActive(false);
		if (this.OnPreReviveEvent != null)
		{
			this.OnPreReviveEvent(base.transform.position);
		}
		AudioManager.Play("player_revive");
		AudioManager.Play((!PlayerManager.GetPlayer(this.playerId).stats.isChalice) ? "player_revive_thank_you" : "player_revive_thank_you_chalice");
		base.animator.SetTrigger("OnParry");
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x000FAF48 File Offset: 0x000F9148
	public virtual void OnReviveParryAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		AbstractPlayerController player = PlayerManager.GetPlayer(this.playerId);
		if (player.mode == PlayerMode.Level && player.stats.isChalice)
		{
			LevelPlayerController levelPlayerController = player as LevelPlayerController;
			levelPlayerController.motor.OnChaliceRevive();
		}
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x000FAFC8 File Offset: 0x000F91C8
	public void ReviveOutOfFrame()
	{
		if (this.exiting || !PlayerManager.Multiplayer)
		{
			return;
		}
		this.exiting = true;
		PlayerId id = (this.playerId != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo;
		AbstractPlayerController player = PlayerManager.GetPlayer(id);
		if (player == null || player.IsDead || !player.stats.PartnerCanSteal || Level.IsTowerOfPowerMain)
		{
			return;
		}
		player.stats.OnPartnerStealHealth();
		this.StopAllCoroutines();
		if (this.OnPreReviveEvent != null)
		{
			this.OnPreReviveEvent(player.center);
		}
		AudioManager.Play("player_revive");
		AudioManager.Play("player_revive_thank_you");
		base.animator.SetTrigger("OnSteal");
		base.transform.position = player.center;
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x0002C0B7 File Offset: 0x0002A2B7
	public void OnReviveStealAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x0002C0EC File Offset: 0x0002A2EC
	public void OnPlayerLeave(PlayerId id)
	{
		if (this.playerId == id)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x0002C105 File Offset: 0x0002A305
	public void OnDestroy()
	{
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.explosionPrefab = null;
		this.cuphead = null;
		this.mugman = null;
		this.parrySwitch = null;
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x0002C134 File Offset: 0x0002A334
	public void Clean()
	{
		this.OnDestroy();
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x000FB0A0 File Offset: 0x000F92A0
	public IEnumerator float_cr()
	{
		base.animator.SetTrigger("OnIdle");
		float floatSpeed = PlayerDeathEffect.FLOAT_SPEEDS[Mathf.Clamp(this.deathCount, 0, PlayerDeathEffect.FLOAT_SPEEDS.Length - 1)];
		while (true && !this.exiting)
		{
			base.transform.AddPosition(0f, floatSpeed * base.LocalDeltaTime, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x000FB0BC File Offset: 0x000F92BC
	public virtual IEnumerator checkOutOfFrame_cr()
	{
		for (;;)
		{
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 10f)) && this.playerInput.actions.GetButtonDown(8) && !this.exiting)
			{
				yield return new WaitForSeconds(0.1f);
				this.ReviveOutOfFrame();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002B9A RID: 11162
	public const string NAME = "Player_Death";

	// Token: 0x04002B9B RID: 11163
	public const string EFFECT_NAME = "Player_Death_Explosion";

	// Token: 0x04002B9C RID: 11164
	public const string PATH = "Player/Player_Death";

	// Token: 0x04002B9D RID: 11165
	public const float TIME_TO_SPEED = 1f;

	// Token: 0x04002B9E RID: 11166
	public static readonly float[] FLOAT_SPEEDS = new float[]
	{
		125f,
		200f,
		275f
	};

	// Token: 0x04002B9F RID: 11167
	public const int REVIVE_Y = 10;

	// Token: 0x04002BA0 RID: 11168
	public const int DEATH_COUNT_MAX = 10;

	// Token: 0x04002BA1 RID: 11169
	[SerializeField]
	public SpriteRenderer cuphead;

	// Token: 0x04002BA2 RID: 11170
	[SerializeField]
	public SpriteRenderer mugman;

	// Token: 0x04002BA3 RID: 11171
	[SerializeField]
	public SpriteRenderer chalice;

	// Token: 0x04002BA4 RID: 11172
	[SerializeField]
	public PlayerDeathParrySwitch parrySwitch;

	// Token: 0x04002BA5 RID: 11173
	[SerializeField]
	public LevelPlayerDeathEffect explosionPrefab;

	// Token: 0x04002BA6 RID: 11174
	[SerializeField]
	public SpriteRenderer effect;

	// Token: 0x04002BA7 RID: 11175
	[SerializeField]
	public SpriteRenderer chaliceEffect;

	// Token: 0x04002BA8 RID: 11176
	public PlayerId playerId;

	// Token: 0x04002BA9 RID: 11177
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002BAA RID: 11178
	public bool exiting;

	// Token: 0x04002BAB RID: 11179
	public PlayerInput playerInput;

	// Token: 0x04002BAC RID: 11180
	public int deathCount;

	// Token: 0x04002BAD RID: 11181
	public PlayerMode playerMode;
}
