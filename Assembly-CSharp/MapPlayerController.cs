using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class MapPlayerController : MapSprite
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x060031DE RID: 12766 RVA: 0x000EB9EC File Offset: 0x000E9BEC
	// (remove) Token: 0x060031DF RID: 12767 RVA: 0x000EBA20 File Offset: 0x000E9C20
	public static event Action OnEquipMenuOpenedEvent;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x060031E0 RID: 12768 RVA: 0x000EBA54 File Offset: 0x000E9C54
	// (remove) Token: 0x060031E1 RID: 12769 RVA: 0x000EBA88 File Offset: 0x000E9C88
	public static event Action OnEquipMenuClosedEvent;

	// Token: 0x060031E2 RID: 12770 RVA: 0x000EBABC File Offset: 0x000E9CBC
	public static bool CanMove()
	{
		return MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (!SceneLoader.Exists || (!SceneLoader.IsInIrisTransition && !SceneLoader.IsInBlurTransition)) && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard) && (!MapEventNotification.Current || !MapEventNotification.Current.showing);
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000296E2 File Offset: 0x000278E2
	// (set) Token: 0x060031E4 RID: 12772 RVA: 0x000296EA File Offset: 0x000278EA
	public MapPlayerController.State state { get; set; }

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060031E5 RID: 12773 RVA: 0x000296F3 File Offset: 0x000278F3
	// (set) Token: 0x060031E6 RID: 12774 RVA: 0x000296FB File Offset: 0x000278FB
	public PlayerId id { get; set; }

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00029704 File Offset: 0x00027904
	// (set) Token: 0x060031E8 RID: 12776 RVA: 0x0002970C File Offset: 0x0002790C
	public bool EquipMenuOpen { get; set; }

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060031E9 RID: 12777 RVA: 0x00029715 File Offset: 0x00027915
	// (set) Token: 0x060031EA RID: 12778 RVA: 0x0002971D File Offset: 0x0002791D
	public PlayerInput input { get; set; }

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060031EB RID: 12779 RVA: 0x00029726 File Offset: 0x00027926
	// (set) Token: 0x060031EC RID: 12780 RVA: 0x0002972E File Offset: 0x0002792E
	public MapPlayerMotor motor { get; set; }

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060031ED RID: 12781 RVA: 0x00029737 File Offset: 0x00027937
	// (set) Token: 0x060031EE RID: 12782 RVA: 0x0002973F File Offset: 0x0002793F
	public MapPlayerAnimationController animationController { get; set; }

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060031EF RID: 12783 RVA: 0x00029748 File Offset: 0x00027948
	// (set) Token: 0x060031F0 RID: 12784 RVA: 0x00029750 File Offset: 0x00027950
	public MapPlayerLadderManager ladderManager { get; set; }

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00029759 File Offset: 0x00027959
	public Vector2 Velocity
	{
		get
		{
			return this.motor.velocity;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060031F2 RID: 12786 RVA: 0x00029766 File Offset: 0x00027966
	public MapPlayerAnimationController.Direction Direction
	{
		get
		{
			return this.animationController.direction;
		}
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x000EBB58 File Offset: 0x000E9D58
	public override void Awake()
	{
		MapPlayerController[] array = Object.FindObjectsOfType<MapPlayerController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("PlayerTwo"))
			{
				Object.Destroy(array[i].gameObject);
			}
		}
		base.Awake();
		base.tag = "Player_Map";
		this.input = base.GetComponent<PlayerInput>();
		this.motor = base.GetComponent<MapPlayerMotor>();
		this.animationController = base.GetComponent<MapPlayerAnimationController>();
		this.ladderManager = base.GetComponent<MapPlayerLadderManager>();
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x00029773 File Offset: 0x00027973
	public void Start()
	{
		MapPlayerController.OnEquipMenuOpenedEvent += this.OnEquipMenuOpened;
		MapPlayerController.OnEquipMenuClosedEvent += this.OnEquipMenuClosed;
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x00029797 File Offset: 0x00027997
	public override void OnDestroy()
	{
		base.OnDestroy();
		MapPlayerController.OnEquipMenuOpenedEvent -= this.OnEquipMenuOpened;
		MapPlayerController.OnEquipMenuClosedEvent -= this.OnEquipMenuClosed;
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000EBBE4 File Offset: 0x000E9DE4
	public static MapPlayerController Create(PlayerId playerId, MapPlayerController.InitObject init)
	{
		MapPlayerController mapPlayerController = Object.Instantiate<MapPlayerController>(Map.Current.MapResources.mapPlayer);
		mapPlayerController.Init(playerId, init);
		return mapPlayerController;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000EBC10 File Offset: 0x000E9E10
	public void Init(PlayerId playerId, MapPlayerController.InitObject init)
	{
		base.gameObject.name = playerId.ToString();
		this.id = playerId;
		this.input.Init(this.id);
		this.animationController.Init(init.pose);
		base.transform.position = init.position;
		switch (init.pose)
		{
		case MapPlayerPose.Default:
			this.state = MapPlayerController.State.Walking;
			break;
		case MapPlayerPose.Joined:
			this.state = MapPlayerController.State.Stationary;
			base.StartCoroutine(this.joined_cr());
			break;
		case MapPlayerPose.Won:
			this.state = MapPlayerController.State.Stationary;
			break;
		}
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000297C1 File Offset: 0x000279C1
	public void Disable()
	{
		this.state = MapPlayerController.State.Stationary;
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x000297CA File Offset: 0x000279CA
	public void Enable()
	{
		this.state = MapPlayerController.State.Walking;
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x000297D3 File Offset: 0x000279D3
	public void OnLeave()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000297E0 File Offset: 0x000279E0
	public void OnChaliceJumpAnimationComplete()
	{
		base.transform.localScale = new Vector3(1f, 1f);
		this.OnJumpAnimationComplete();
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000EBCC8 File Offset: 0x000E9EC8
	public void OnJumpAnimationComplete()
	{
		if (this.joinedMidGame)
		{
			this.joinedMidGame = false;
			if (this.id == PlayerId.PlayerTwo)
			{
				MapPlayerController mapPlayerController = Map.Current.players[0];
				if (mapPlayerController.state != MapPlayerController.State.Stationary)
				{
					this.Enable();
				}
			}
			else
			{
				this.Enable();
			}
		}
		else
		{
			this.Enable();
		}
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x00029802 File Offset: 0x00027A02
	public void OnEquipMenuOpened()
	{
		this.EquipMenuOpen = true;
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x0002980B File Offset: 0x00027A0B
	public void OnEquipMenuClosed()
	{
		this.EquipMenuOpen = false;
	}

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x060031FF RID: 12799 RVA: 0x000EBD28 File Offset: 0x000E9F28
	// (remove) Token: 0x06003200 RID: 12800 RVA: 0x000EBD60 File Offset: 0x000E9F60
	public event MapPlayerController.LadderEnterEventHandler LadderEnterEvent;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06003201 RID: 12801 RVA: 0x000EBD98 File Offset: 0x000E9F98
	// (remove) Token: 0x06003202 RID: 12802 RVA: 0x000EBDD0 File Offset: 0x000E9FD0
	public event MapPlayerController.LadderExitEventHandler LadderExitEvent;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06003203 RID: 12803 RVA: 0x000EBE08 File Offset: 0x000EA008
	// (remove) Token: 0x06003204 RID: 12804 RVA: 0x000EBE40 File Offset: 0x000EA040
	public event Action LadderEnterCompleteEvent;

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06003205 RID: 12805 RVA: 0x000EBE78 File Offset: 0x000EA078
	// (remove) Token: 0x06003206 RID: 12806 RVA: 0x000EBEB0 File Offset: 0x000EA0B0
	public event Action LadderExitCompleteEvent;

	// Token: 0x06003207 RID: 12807 RVA: 0x00029814 File Offset: 0x00027A14
	public void LadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		this.state = MapPlayerController.State.LadderEnter;
		if (this.LadderEnterEvent != null)
		{
			this.LadderEnterEvent(point, ladder, location);
		}
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x00029836 File Offset: 0x00027A36
	public void LadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
		this.state = MapPlayerController.State.LadderExit;
		if (this.LadderExitEvent != null)
		{
			this.LadderExitEvent(point, exit, location);
		}
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x00029858 File Offset: 0x00027A58
	public void LadderEnterComplete()
	{
		this.state = MapPlayerController.State.Ladder;
		if (this.LadderEnterCompleteEvent != null)
		{
			this.LadderEnterCompleteEvent();
		}
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x00029877 File Offset: 0x00027A77
	public void LadderExitComplete()
	{
		this.state = MapPlayerController.State.Walking;
		if (this.LadderExitCompleteEvent != null)
		{
			this.LadderExitCompleteEvent();
		}
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x00029896 File Offset: 0x00027A96
	public void OnWinComplete()
	{
		this.animationController.CompleteJump();
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000EBEE8 File Offset: 0x000EA0E8
	public IEnumerator joined_cr()
	{
		yield return null;
		this.joinedMidGame = true;
		this.animationController.CompleteJump();
		yield break;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x000EBF04 File Offset: 0x000EA104
	public void SecretPathEnter(bool enter)
	{
		SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = ((!enter) ? 0 : -1000);
		}
		base.gameObject.layer = ((!enter) ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Map_Secret"));
		this.hideInteractionPrompts = enter;
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x000298A3 File Offset: 0x00027AA3
	public void TryActivateDjimmi()
	{
		if (!PlayerData.Data.TryActivateDjimmi())
		{
			AudioManager.Play("menu_locked");
		}
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x000298BE File Offset: 0x00027ABE
	public void JumpSFX()
	{
		AudioManager.Play("complete_bounce");
	}

	// Token: 0x04002905 RID: 10501
	public const string TAG = "Player_Map";

	// Token: 0x04002909 RID: 10505
	public bool joinedMidGame;

	// Token: 0x0400290E RID: 10510
	public bool hideInteractionPrompts;

	// Token: 0x02001110 RID: 4368
	public enum State
	{
		// Token: 0x04007895 RID: 30869
		Walking,
		// Token: 0x04007896 RID: 30870
		LadderEnter,
		// Token: 0x04007897 RID: 30871
		LadderExit,
		// Token: 0x04007898 RID: 30872
		Ladder,
		// Token: 0x04007899 RID: 30873
		Stationary
	}

	// Token: 0x02001111 RID: 4369
	[Serializable]
	public class InitObject
	{
		// Token: 0x06007C40 RID: 31808 RVA: 0x00053A03 File Offset: 0x00051C03
		public InitObject(Vector2 position, MapPlayerPose pose)
		{
			this.position = position;
			this.pose = pose;
		}

		// Token: 0x0400789A RID: 30874
		public Vector2 position;

		// Token: 0x0400789B RID: 30875
		public MapPlayerPose pose;
	}

	// Token: 0x02001112 RID: 4370
	// (Invoke) Token: 0x06007C42 RID: 31810
	public delegate void LadderEnterEventHandler(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location);

	// Token: 0x02001113 RID: 4371
	// (Invoke) Token: 0x06007C46 RID: 31814
	public delegate void LadderExitEventHandler(Vector2 point, Vector2 exit, MapLadder.Location location);
}
