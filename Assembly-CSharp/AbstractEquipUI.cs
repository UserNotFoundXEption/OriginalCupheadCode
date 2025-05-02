using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public abstract class AbstractEquipUI : AbstractPauseGUI
{
	// Token: 0x0600323E RID: 12862 RVA: 0x00029B39 File Offset: 0x00027D39
	public AbstractEquipUI()
	{
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x0600323F RID: 12863 RVA: 0x00029B41 File Offset: 0x00027D41
	// (set) Token: 0x06003240 RID: 12864 RVA: 0x00029B48 File Offset: 0x00027D48
	public static AbstractEquipUI Current { get; set; }

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06003241 RID: 12865 RVA: 0x00029B50 File Offset: 0x00027D50
	// (set) Token: 0x06003242 RID: 12866 RVA: 0x00029B58 File Offset: 0x00027D58
	public AbstractEquipUI.ActiveState CurrentState { get; set; }

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06003243 RID: 12867 RVA: 0x00029B61 File Offset: 0x00027D61
	public override AbstractPauseGUI.InputActionSet CheckedActionSet
	{
		get
		{
			return AbstractPauseGUI.InputActionSet.UIInput;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06003244 RID: 12868 RVA: 0x00029B64 File Offset: 0x00027D64
	public override bool CanPause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06003245 RID: 12869 RVA: 0x00029B67 File Offset: 0x00027D67
	public override bool CanUnpause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x00029B6A File Offset: 0x00027D6A
	public override void InAnimation(float i)
	{
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x00029B6C File Offset: 0x00027D6C
	public override void OutAnimation(float i)
	{
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x000EC508 File Offset: 0x000EA708
	public override void Awake()
	{
		base.Awake();
		AbstractEquipUI.Current = this;
		this.playerTwo = Object.Instantiate<MapEquipUICard>(this.playerOne);
		this.playerTwo.transform.SetParent(this.playerOne.transform.parent, false);
		this.playerTwo.Init(PlayerId.PlayerTwo, this);
		this.playerTwo.name = "PlayerTwo";
		this.playerOne.transform.SetSiblingIndex(this.playerTwo.transform.GetSiblingIndex());
		this.playerOne.Init(PlayerId.PlayerOne, this);
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x000EC5A0 File Offset: 0x000EA7A0
	public void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
		if (PlayerManager.Multiplayer)
		{
			Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
			this.playerOne.container.anchoredPosition = anchoredPosition;
			this.playerTwo.container.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x00029B6E File Offset: 0x00027D6E
	public void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeft;
		if (AbstractEquipUI.Current == this)
		{
			AbstractEquipUI.Current = null;
		}
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x000EC60C File Offset: 0x000EA80C
	public void OnPlayerJoined(PlayerId playerId)
	{
		Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
		anchoredPosition.y += 10f;
		this.playerOne.container.anchoredPosition = anchoredPosition;
		this.playerTwo.container.anchoredPosition = anchoredPosition;
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x000EC660 File Offset: 0x000EA860
	public void OnPlayerLeft(PlayerId playerId)
	{
		Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
		anchoredPosition.y -= 10f;
		this.playerOne.container.anchoredPosition = anchoredPosition;
		this.playerTwo.container.anchoredPosition = anchoredPosition;
	}

	// Token: 0x0600324D RID: 12877 RVA: 0x000EC6B4 File Offset: 0x000EA8B4
	public override void OnPause()
	{
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
		this.OnPauseAudio();
		base.FrameDelayedCallback(new Action(this.SetStateActive), 1);
		this.playerOne.CanRotate = false;
		this.playerTwo.CanRotate = false;
		AudioManager.Play("menu_cardup");
		if (PlayerManager.Multiplayer)
		{
			this.playerOne.SetActive(true);
			this.playerTwo.SetActive(true);
			this.playerOne.SetMultiplayerOut(true);
			this.playerTwo.SetMultiplayerOut(true);
			this.playerOne.SetMultiplayerIn(false);
			this.playerTwo.SetMultiplayerIn(false);
		}
		else
		{
			this.playerOne.SetActive(true);
			this.playerTwo.SetActive(false);
			this.playerOne.SetSinglePlayerOut(true);
			this.playerOne.SetSinglePlayerIn(false);
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerData.Data.ResetHasNewPurchase(PlayerId.Any);
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x00029BA8 File Offset: 0x00027DA8
	public void SetStateActive()
	{
		this.CurrentState = AbstractEquipUI.ActiveState.Active;
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x00029BB1 File Offset: 0x00027DB1
	public override void OnPauseComplete()
	{
		base.OnPauseComplete();
		this.playerOne.CanRotate = true;
		this.playerTwo.CanRotate = true;
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x000EC7AC File Offset: 0x000EA9AC
	public override void OnUnpause()
	{
		this.OnUnpauseAudio();
		base.OnUnpause();
		this.playerOne.CanRotate = false;
		this.playerTwo.CanRotate = false;
		if (PlayerManager.Multiplayer)
		{
			this.playerOne.SetMultiplayerOut(false);
			this.playerTwo.SetMultiplayerOut(false);
		}
		else
		{
			this.playerOne.SetSinglePlayerOut(false);
		}
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x00029BD1 File Offset: 0x00027DD1
	public virtual void OnPauseAudio()
	{
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x00029BD3 File Offset: 0x00027DD3
	public virtual void OnUnpauseAudio()
	{
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x00029BD5 File Offset: 0x00027DD5
	public override void OnUnpauseComplete()
	{
		base.OnUnpauseComplete();
		this.CurrentState = AbstractEquipUI.ActiveState.Inactive;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x000EC810 File Offset: 0x000EAA10
	public bool Close()
	{
		if (PlayerManager.Multiplayer && !this.playerOne.ReadyAndWaiting && !this.playerTwo.ReadyAndWaiting)
		{
			return false;
		}
		if (Map.Current != null)
		{
			Map.Current.OnCloseEquipMenu();
		}
		AudioManager.Play("menu_carddown");
		this.Unpause();
		return true;
	}

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	public MapEquipUICard playerOne;

	// Token: 0x04002930 RID: 10544
	public MapEquipUICard playerTwo;

	// Token: 0x0200111B RID: 4379
	public enum ActiveState
	{
		// Token: 0x040078CD RID: 30925
		Inactive,
		// Token: 0x040078CE RID: 30926
		Active
	}
}
