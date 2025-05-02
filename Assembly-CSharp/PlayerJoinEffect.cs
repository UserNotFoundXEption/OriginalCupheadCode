using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class PlayerJoinEffect : AbstractMonoBehaviour
{
	// Token: 0x060035CB RID: 13771 RVA: 0x000FB0D8 File Offset: 0x000F92D8
	public static PlayerJoinEffect Create(PlayerId playerId, Vector2 pos, PlayerMode mode, bool isChalice)
	{
		PlayerJoinEffect playerJoinEffect = Object.Instantiate<PlayerJoinEffect>(Level.Current.LevelResources.joinEffect);
		playerJoinEffect.name = playerJoinEffect.name.Replace("(Clone)", string.Empty);
		playerJoinEffect.Init(playerId, pos, mode, isChalice);
		return playerJoinEffect;
	}

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x060035CC RID: 13772 RVA: 0x000FB120 File Offset: 0x000F9320
	// (remove) Token: 0x060035CD RID: 13773 RVA: 0x000FB158 File Offset: 0x000F9358
	public event AbstractPlayerController.OnReviveHandler OnPreReviveEvent;

	// Token: 0x1400008A RID: 138
	// (add) Token: 0x060035CE RID: 13774 RVA: 0x000FB190 File Offset: 0x000F9390
	// (remove) Token: 0x060035CF RID: 13775 RVA: 0x000FB1C8 File Offset: 0x000F93C8
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x060035D0 RID: 13776 RVA: 0x0002C17E File Offset: 0x0002A37E
	public override void Awake()
	{
		base.Awake();
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x000FB200 File Offset: 0x000F9400
	public void Init(PlayerId playerId, Vector2 pos, PlayerMode mode, bool isChalice)
	{
		this.playerId = playerId;
		this.playerMode = mode;
		base.animator.SetInteger("Mode", (int)this.playerMode);
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = this.cuphead;
		}
		else
		{
			this.spriteRenderer = this.mugman;
		}
		if (isChalice)
		{
			this.spriteRenderer = this.chalice;
		}
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		this.spriteRenderer.gameObject.SetActive(true);
		base.transform.position = pos;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x000FB2D0 File Offset: 0x000F94D0
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x0002C197 File Offset: 0x0002A397
	public void OnReviveStealAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x0002C1CC File Offset: 0x0002A3CC
	public void OnPlayerLeave(PlayerId id)
	{
		if (this.playerId == id)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x0002C1E5 File Offset: 0x0002A3E5
	public void OnDestroy()
	{
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
	}

	// Token: 0x04002BB0 RID: 11184
	public const string NAME = "Player_Join";

	// Token: 0x04002BB1 RID: 11185
	[SerializeField]
	public SpriteRenderer cuphead;

	// Token: 0x04002BB2 RID: 11186
	[SerializeField]
	public SpriteRenderer mugman;

	// Token: 0x04002BB3 RID: 11187
	[SerializeField]
	public SpriteRenderer chalice;

	// Token: 0x04002BB4 RID: 11188
	public PlayerId playerId;

	// Token: 0x04002BB5 RID: 11189
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002BB6 RID: 11190
	public PlayerMode playerMode;
}
