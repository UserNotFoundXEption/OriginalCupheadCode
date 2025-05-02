using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public abstract class AbstractPlayerController : AbstractPausableComponent
{
	// Token: 0x06003468 RID: 13416 RVA: 0x0002B01E File Offset: 0x0002921E
	public AbstractPlayerController()
	{
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x000F6924 File Offset: 0x000F4B24
	public static AbstractPlayerController Create(PlayerId id, Vector2 pos, PlayerMode mode)
	{
		AbstractPlayerController abstractPlayerController;
		switch (mode)
		{
		default:
			abstractPlayerController = Object.Instantiate<LevelPlayerController>(Level.Current.LevelResources.levelPlayer);
			break;
		case PlayerMode.Plane:
			abstractPlayerController = Object.Instantiate<PlanePlayerController>(Level.Current.LevelResources.planePlayer);
			break;
		case PlayerMode.Arcade:
			abstractPlayerController = Object.Instantiate<ArcadePlayerController>(((RetroArcadeLevel)Level.Current).playerPrefab);
			break;
		}
		abstractPlayerController.transform.position = pos;
		abstractPlayerController.mode = mode;
		abstractPlayerController.LevelInit(id);
		return abstractPlayerController;
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600346A RID: 13418 RVA: 0x0002B026 File Offset: 0x00029226
	public PlayerInput input
	{
		get
		{
			if (this._input == null)
			{
				this._input = base.GetComponent<PlayerInput>();
			}
			return this._input;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600346B RID: 13419 RVA: 0x0002B04B File Offset: 0x0002924B
	public PlayerStatsManager stats
	{
		get
		{
			if (this._stats == null)
			{
				this._stats = base.GetComponent<PlayerStatsManager>();
			}
			return this._stats;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600346C RID: 13420 RVA: 0x0002B070 File Offset: 0x00029270
	public PlayerDamageReceiver damageReceiver
	{
		get
		{
			if (this._damageReceiver == null)
			{
				this._damageReceiver = base.GetComponent<PlayerDamageReceiver>();
			}
			return this._damageReceiver;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600346D RID: 13421 RVA: 0x0002B095 File Offset: 0x00029295
	public PlayerCameraController cameraController
	{
		get
		{
			if (this._cameraController == null)
			{
				this._cameraController = base.GetComponent<PlayerCameraController>();
			}
			return this._cameraController;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600346E RID: 13422 RVA: 0x0002B0BA File Offset: 0x000292BA
	// (set) Token: 0x0600346F RID: 13423 RVA: 0x0002B0C2 File Offset: 0x000292C2
	public PlayerId id { get; set; }

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06003470 RID: 13424 RVA: 0x0002B0CB File Offset: 0x000292CB
	// (set) Token: 0x06003471 RID: 13425 RVA: 0x0002B0D3 File Offset: 0x000292D3
	public PlayerMode mode { get; set; }

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06003472 RID: 13426 RVA: 0x0002B0DC File Offset: 0x000292DC
	public bool IsDead
	{
		get
		{
			return !this._isReviving && this.stats.Health <= 0;
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06003473 RID: 13427 RVA: 0x0002B0FC File Offset: 0x000292FC
	// (set) Token: 0x06003474 RID: 13428 RVA: 0x0002B104 File Offset: 0x00029304
	public bool levelStarted { get; set; }

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06003475 RID: 13429 RVA: 0x0002B10D File Offset: 0x0002930D
	// (set) Token: 0x06003476 RID: 13430 RVA: 0x0002B115 File Offset: 0x00029315
	public bool levelEnded { get; set; }

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06003477 RID: 13431 RVA: 0x0002B11E File Offset: 0x0002931E
	public BoxCollider2D collider
	{
		get
		{
			if (this._collider == null)
			{
				this._collider = base.GetComponent<BoxCollider2D>();
			}
			return this._collider;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06003478 RID: 13432 RVA: 0x0002B143 File Offset: 0x00029343
	public BoxCollider2D collider2D
	{
		get
		{
			return this.collider;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06003479 RID: 13433 RVA: 0x0002B14B File Offset: 0x0002934B
	public virtual Vector3 center
	{
		get
		{
			if (base.transform == null)
			{
				return Vector3.zero;
			}
			return base.transform.position + this.collider.offset;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x0600347A RID: 13434 RVA: 0x0002B184 File Offset: 0x00029384
	public virtual Vector3 CameraCenter
	{
		get
		{
			return this.cameraController.center;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x0600347B RID: 13435 RVA: 0x000F69B8 File Offset: 0x000F4BB8
	public float left
	{
		get
		{
			return this.center.x - this.collider.size.x * 0.5f;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x0600347C RID: 13436 RVA: 0x000F69F0 File Offset: 0x000F4BF0
	public float right
	{
		get
		{
			return this.center.x + this.collider.size.x * 0.5f;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x0600347D RID: 13437 RVA: 0x000F6A28 File Offset: 0x000F4C28
	public float top
	{
		get
		{
			return this.center.y + this.collider.size.y * 0.5f;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x0600347E RID: 13438 RVA: 0x000F6A60 File Offset: 0x000F4C60
	public float bottom
	{
		get
		{
			return this.center.y - this.collider.size.y * 0.5f;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x0600347F RID: 13439 RVA: 0x0002B196 File Offset: 0x00029396
	public float width
	{
		get
		{
			return this.right - this.left;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06003480 RID: 13440 RVA: 0x0002B1A5 File Offset: 0x000293A5
	public float height
	{
		get
		{
			return this.top - this.bottom;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06003481 RID: 13441
	public abstract bool CanTakeDamage { get; }

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06003482 RID: 13442 RVA: 0x000F6A98 File Offset: 0x000F4C98
	// (remove) Token: 0x06003483 RID: 13443 RVA: 0x000F6AD0 File Offset: 0x000F4CD0
	public event Action OnPlayIntroEvent;

	// Token: 0x06003484 RID: 13444 RVA: 0x0002B1B4 File Offset: 0x000293B4
	public virtual void PlayIntro()
	{
		if (this.OnPlayIntroEvent != null)
		{
			this.OnPlayIntroEvent();
		}
	}

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06003485 RID: 13445 RVA: 0x000F6B08 File Offset: 0x000F4D08
	// (remove) Token: 0x06003486 RID: 13446 RVA: 0x000F6B40 File Offset: 0x000F4D40
	public event Action OnPlatformingLevelAwakeEvent;

	// Token: 0x06003487 RID: 13447 RVA: 0x0002B1CC File Offset: 0x000293CC
	public virtual void OnPlatformingLevelAwake()
	{
		if (this.OnPlatformingLevelAwakeEvent != null)
		{
			this.OnPlatformingLevelAwakeEvent();
		}
	}

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06003488 RID: 13448 RVA: 0x000F6B78 File Offset: 0x000F4D78
	// (remove) Token: 0x06003489 RID: 13449 RVA: 0x000F6BB0 File Offset: 0x000F4DB0
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x0600348A RID: 13450 RVA: 0x000F6BE8 File Offset: 0x000F4DE8
	public override void Awake()
	{
		AbstractPlayerController[] array = Object.FindObjectsOfType<AbstractPlayerController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("PlayerTwo"))
			{
				Object.Destroy(array[i].gameObject);
			}
		}
		base.Awake();
		if (Level.Current == null || !Level.Current.PlayersCreated)
		{
			if (base.gameObject != null)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x000F6C78 File Offset: 0x000F4E78
	public virtual void LevelInit(PlayerId id)
	{
		this.id = id;
		base.name = id.ToString();
		PlayerManager.SetPlayer(id, this);
		this.input.Init(this.id);
		this.cameraController.LevelInit();
		this.stats.LevelInit();
		this.stats.OnPlayerDeathEvent += this.OnDeath;
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0002B1E4 File Offset: 0x000293E4
	public virtual void OnLevelWin()
	{
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x000F6CE8 File Offset: 0x000F4EE8
	public virtual void LevelStart()
	{
		foreach (AbstractPlayerComponent abstractPlayerComponent in base.GetComponentsInChildren<AbstractPlayerComponent>())
		{
			abstractPlayerComponent.OnLevelStart();
		}
		this.levelStarted = true;
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0002B1E6 File Offset: 0x000293E6
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.levelEnded = true;
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0002B1F5 File Offset: 0x000293F5
	public virtual void OnDeath(PlayerId playerId)
	{
		this._isReviving = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0002B20A File Offset: 0x0002940A
	public virtual void OnLeave(PlayerId playerId)
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0002B217 File Offset: 0x00029417
	public virtual void OnPreRevive(Vector3 pos)
	{
		this.stats.OnPreRevive();
		this._isReviving = true;
		base.transform.position = pos;
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000F6D24 File Offset: 0x000F4F24
	public virtual void LevelJoin(Vector3 pos)
	{
		this.LevelStart();
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		PlayerJoinEffect playerJoinEffect = PlayerJoinEffect.Create(this.id, base.transform.position, this.mode, this.stats.isChalice);
		playerJoinEffect.OnPreReviveEvent += this.OnPreRevive;
		playerJoinEffect.OnReviveEvent += this.OnRevive;
		this.OnPreRevive(pos);
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x0002B237 File Offset: 0x00029437
	public virtual void BufferInputs()
	{
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x000F6DAC File Offset: 0x000F4FAC
	public virtual void OnRevive(Vector3 pos)
	{
		this.reviveHelper = new GameObjectHelper("Revive Helper");
		this.reviveHelper.events.StartCoroutine(this.reviveDelay_cr(1, pos));
		this.stats.OnRevive();
		base.transform.position = pos;
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x000F6DFC File Offset: 0x000F4FFC
	public IEnumerator reviveDelay_cr(int frameDelay, Vector3 pos)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		this._isReviving = false;
		base.gameObject.SetActive(true);
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(pos);
		}
		yield break;
	}

	// Token: 0x04002B1C RID: 11036
	public PlayerInput _input;

	// Token: 0x04002B1D RID: 11037
	public PlayerStatsManager _stats;

	// Token: 0x04002B1E RID: 11038
	public PlayerDamageReceiver _damageReceiver;

	// Token: 0x04002B1F RID: 11039
	public PlayerCameraController _cameraController;

	// Token: 0x04002B22 RID: 11042
	public bool _isReviving;

	// Token: 0x04002B25 RID: 11045
	public BoxCollider2D _collider;

	// Token: 0x04002B29 RID: 11049
	public GameObjectHelper reviveHelper;

	// Token: 0x02001151 RID: 4433
	// (Invoke) Token: 0x06007D4A RID: 32074
	public delegate void OnReviveHandler(Vector3 pos);
}
