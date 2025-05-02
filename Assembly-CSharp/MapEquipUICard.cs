using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004BF RID: 1215
public class MapEquipUICard : AbstractMonoBehaviour
{
	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06003278 RID: 12920 RVA: 0x00029D7B File Offset: 0x00027F7B
	// (set) Token: 0x06003279 RID: 12921 RVA: 0x00029D83 File Offset: 0x00027F83
	public bool ReadyAndWaiting { get; set; }

	// Token: 0x0600327A RID: 12922 RVA: 0x00029D8C File Offset: 0x00027F8C
	public void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x00029DB0 File Offset: 0x00027FB0
	public void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeft;
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x00029DD4 File Offset: 0x00027FD4
	public void Update()
	{
		this.HandleInput();
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x000ECC7C File Offset: 0x000EAE7C
	public void HandleInput()
	{
		if (this.playerInput == null || !this.inputEnabled || this.equipUI.CurrentState != AbstractEquipUI.ActiveState.Active || InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		MapEquipUICard.Side side = this.side;
		if (side != MapEquipUICard.Side.Front)
		{
			if (side == MapEquipUICard.Side.Back)
			{
				switch (this.back)
				{
				case MapEquipUICard.Back.Select:
					this.HandleInputBackSelect();
					break;
				case MapEquipUICard.Back.Ready:
					this.HandleInputBackReady();
					break;
				case MapEquipUICard.Back.Checklist:
					this.HandleInputChecklistReady();
					break;
				}
			}
		}
		else
		{
			this.HandleInputFront();
		}
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x000ECD30 File Offset: 0x000EAF30
	public void HandleInputFront()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.Close();
			return;
		}
		if (this.playerInput.GetButtonDown(18))
		{
			this.front.ChangeSelection(-1);
			return;
		}
		if (this.playerInput.GetButtonDown(20))
		{
			this.front.ChangeSelection(1);
			return;
		}
		if (this.front.checkListSelected)
		{
			if (this.playerInput.GetButtonDown(13))
			{
				int index = 0;
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
				{
					index = 1;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_3)
				{
					index = 2;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
				{
					index = 3;
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
				{
					index = 4;
				}
				this.checkList.SetCursorPosition(index, true);
				this.RotateToCheckList();
				return;
			}
		}
		else
		{
			if (this.playerInput.GetButtonDown(13))
			{
				this.RotateToBackSelect(this.front.Slot);
				return;
			}
			if (this.playerInput.GetButtonDown(15))
			{
				this.front.Unequip();
				return;
			}
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000ECE6C File Offset: 0x000EB06C
	public void HandleInputBackSelect()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.front.ChangeSelection(0);
			this.RotateToFront();
			return;
		}
		if (!this.backSelect.lockInput)
		{
			if (this.playerInput.GetButtonDown(15))
			{
				this.backSelect.Unequip();
				return;
			}
			if (this.playerInput.GetButtonDown(18))
			{
				this.backSelect.ChangeSelection(new Trilean2(-1, 0));
				return;
			}
			if (this.playerInput.GetButtonDown(20))
			{
				this.backSelect.ChangeSelection(new Trilean2(1, 0));
				return;
			}
			if (this.playerInput.GetButtonDown(16))
			{
				this.backSelect.ChangeSelection(new Trilean2(0, 1));
				return;
			}
			if (this.playerInput.GetButtonDown(19))
			{
				this.backSelect.ChangeSelection(new Trilean2(0, -1));
				return;
			}
			if (this.playerInput.GetButtonDown(11))
			{
				AudioManager.Play("menu_equipment_page");
				this.backSelect.ChangeSlot(1);
				return;
			}
			if (this.playerInput.GetButtonDown(12))
			{
				AudioManager.Play("menu_equipment_page");
				this.backSelect.ChangeSlot(-1);
				return;
			}
			if (this.playerInput.GetButtonDown(13))
			{
				this.backSelect.Accept();
				return;
			}
		}
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x00029DDC File Offset: 0x00027FDC
	public void HandleInputBackReady()
	{
		if (this.playerInput.GetButtonDown(13))
		{
			this.RotateToFront();
			return;
		}
		if (this.playerInput.GetButtonDown(15))
		{
			this.RotateToFront();
			return;
		}
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000ECFD0 File Offset: 0x000EB1D0
	public void HandleInputChecklistReady()
	{
		if (this.playerInput.GetButtonDown(14))
		{
			this.RotateToFront();
			return;
		}
		if (this.playerInput.GetButtonDown(18))
		{
			this.checkList.ChangeSelection(-1);
			return;
		}
		if (this.playerInput.GetButtonDown(20))
		{
			this.checkList.ChangeSelection(1);
			return;
		}
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000ED034 File Offset: 0x000EB234
	public void LateUpdate()
	{
		base.transform.localPosition = Vector2.Lerp(base.transform.localPosition, this.position, Time.deltaTime * 10f);
		base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(0f, 0f, this.roll), Time.deltaTime * 8f);
		if (this.rotation > 90f)
		{
			this.side = MapEquipUICard.Side.Back;
			this.SetBackActive(true);
			this.front.SetActive(false);
		}
		else
		{
			this.side = MapEquipUICard.Side.Front;
			this.SetBackActive(false);
			this.front.SetActive(true);
		}
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x00029E10 File Offset: 0x00028010
	public void Close()
	{
		if (!this.equipUI.Close())
		{
			this.RotateToBackReady();
		}
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000ED0FC File Offset: 0x000EB2FC
	public void Init(PlayerId id, AbstractEquipUI equipUI)
	{
		this.playerID = id;
		this.equipUI = equipUI;
		this.playerInput = PlayerManager.GetPlayerInput(id);
		this.backSelect.transform.SetScale(new float?(-1f), null, null);
		this.backReady.transform.SetScale(new float?(-1f), null, null);
		this.checkList.transform.SetScale(new float?(-1f), null, null);
		foreach (Image image in this.cupheadImages)
		{
			image.gameObject.SetActive(false);
		}
		foreach (Image image2 in this.mugmanImages)
		{
			image2.gameObject.SetActive(false);
		}
		this.cupheadChaos.SetActive(false);
		this.mugmanChaos.SetActive(false);
		PlayerId playerId = this.playerID;
		if (playerId != PlayerId.PlayerOne)
		{
			if (playerId != PlayerId.PlayerTwo)
			{
				if (playerId != PlayerId.Any && playerId != PlayerId.None)
				{
				}
			}
			else
			{
				Image[] array3 = (!PlayerManager.player1IsMugman) ? this.mugmanImages : this.cupheadImages;
				foreach (Image image3 in array3)
				{
					image3.gameObject.SetActive(true);
				}
				GameObject gameObject = (!PlayerManager.player1IsMugman) ? this.cupheadChaos : this.mugmanChaos;
				this.cuphead2POverlay.SetActive(PlayerManager.player1IsMugman);
				this.mugman1POverlay.SetActive(false);
				gameObject.SetActive(Localization.language != Localization.Languages.English);
			}
		}
		else
		{
			Image[] array5 = (!PlayerManager.player1IsMugman) ? this.cupheadImages : this.mugmanImages;
			foreach (Image image4 in array5)
			{
				image4.gameObject.SetActive(true);
			}
			GameObject gameObject2 = (!PlayerManager.player1IsMugman) ? this.cupheadChaos : this.mugmanChaos;
			this.mugman1POverlay.SetActive(PlayerManager.player1IsMugman);
			this.cuphead2POverlay.SetActive(false);
			gameObject2.SetActive(Localization.language != Localization.Languages.English);
		}
		this.front.Init(this.playerID);
		this.backSelect.Init(this.playerID);
		this.checkList.Init(this.playerID);
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x00029E28 File Offset: 0x00028028
	public void OnPlayerJoined(PlayerId playerId)
	{
		this.SetActive(true);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			this.SetMultiplayerOut(true);
		}
		this.SetMultiplayerIn(false);
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x00029E4B File Offset: 0x0002804B
	public void OnPlayerLeft(PlayerId playerId)
	{
		if (this.playerID == PlayerId.PlayerTwo)
		{
			this.SetMultiplayerOut(false);
			return;
		}
		this.SetSinglePlayerIn(false);
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x00029E68 File Offset: 0x00028068
	public void SetActive(bool active)
	{
		if (base.gameObject == null)
		{
			return;
		}
		base.gameObject.SetActive(active);
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000ED3CC File Offset: 0x000EB5CC
	public void SetBackActive(bool active)
	{
		this.backSelect.SetActive(false);
		this.backReady.SetActive(false);
		this.checkList.SetActive(false);
		if (!active)
		{
			return;
		}
		switch (this.back)
		{
		case MapEquipUICard.Back.Select:
			this.backSelect.SetActive(active);
			break;
		case MapEquipUICard.Back.Ready:
			this.backReady.SetActive(active);
			break;
		case MapEquipUICard.Back.Checklist:
			this.checkList.SetActive(active);
			break;
		}
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x00029E88 File Offset: 0x00028088
	public void SetSinglePlayerIn(bool instant = false)
	{
		this.ResetToFront();
		this.SetPosition(Vector2.zero, instant);
		this.SetRoll(Random.Range(-4f, 4f), instant);
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x00029EB2 File Offset: 0x000280B2
	public void SetSinglePlayerOut(bool instant = false)
	{
		this.SetPosition(new Vector2(0f, -720f), instant);
		this.SetRoll(0f, instant);
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x000ED45C File Offset: 0x000EB65C
	public void SetMultiplayerIn(bool instant = false)
	{
		this.ResetToFront();
		Vector2 pos;
		pos..ctor(-320f, 0f);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			pos.x *= -1f;
		}
		this.SetPosition(pos, instant);
		this.SetRoll(Random.Range(-4f, 4f), instant);
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x000ED4C0 File Offset: 0x000EB6C0
	public void SetMultiplayerOut(bool instant = false)
	{
		Vector2 pos;
		pos..ctor(-1280f, 0f);
		if (this.playerID == PlayerId.PlayerTwo)
		{
			pos.x *= -1f;
		}
		this.SetPosition(pos, instant);
		this.SetRoll(0f, instant);
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x00029ED6 File Offset: 0x000280D6
	public void SetPosition(Vector2 pos, bool instant)
	{
		this.position = pos;
		if (instant)
		{
			base.transform.localPosition = this.position;
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x00029EFB File Offset: 0x000280FB
	public void RotateToFront()
	{
		AudioManager.Play("menu_cardflip");
		this.front.Refresh();
		if (!this.CanRotate)
		{
			return;
		}
		this.ReadyAndWaiting = false;
		this.StartRotation(this.rotation, 0f);
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x00029F36 File Offset: 0x00028136
	public void ResetToFront()
	{
		this.StopRotation();
		this.SetRotation(0f);
		this.ReadyAndWaiting = false;
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x00029F50 File Offset: 0x00028150
	public void RotateToBackSelect(MapEquipUICard.Slot slot)
	{
		AudioManager.Play("menu_cardflip");
		this.backSelect.Setup(slot);
		if (!this.CanRotate)
		{
			return;
		}
		this.back = MapEquipUICard.Back.Select;
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x00029F8C File Offset: 0x0002818C
	public void RotateToBackReady()
	{
		if (!this.CanRotate)
		{
			return;
		}
		this.ReadyAndWaiting = true;
		this.back = MapEquipUICard.Back.Ready;
		AudioManager.Play("menu_ready");
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x00029FC3 File Offset: 0x000281C3
	public void RotateToCheckList()
	{
		AudioManager.Play("menu_cardflip");
		if (!this.CanRotate)
		{
			return;
		}
		this.back = MapEquipUICard.Back.Checklist;
		this.StartRotation(this.rotation, 180f);
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x00029FF3 File Offset: 0x000281F3
	public void StartRotation(float start, float end)
	{
		this.StopRotation();
		if (!this.CanRotate)
		{
			return;
		}
		this.rotationCoroutine = this.rotate_cr(start, end, 0.15f);
		base.StartCoroutine(this.rotationCoroutine);
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x0002A027 File Offset: 0x00028227
	public void StopRotation()
	{
		if (this.rotationCoroutine != null)
		{
			base.StopCoroutine(this.rotationCoroutine);
		}
		this.rotationCoroutine = null;
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000ED514 File Offset: 0x000EB714
	public void SetRotation(float r)
	{
		this.rotation = r;
		this.container.SetLocalEulerAngles(null, new float?(this.rotation), null);
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x000ED550 File Offset: 0x000EB750
	public IEnumerator rotate_cr(float start, float end, float time)
	{
		this.inputEnabled = false;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.SetRotation(EaseUtils.Ease(this.ROTATION_EASE, start, end, val));
			t += Time.deltaTime;
			yield return null;
		}
		this.SetRotation(end);
		this.inputEnabled = true;
		yield break;
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x000ED580 File Offset: 0x000EB780
	public void SetRoll(float r, bool instant)
	{
		this.roll = r;
		if (instant)
		{
			base.transform.SetLocalEulerAngles(null, null, new float?(r));
		}
	}

	// Token: 0x0400293D RID: 10557
	public const float POSITION_SPEED = 10f;

	// Token: 0x0400293E RID: 10558
	public const float ROLL_SPEED = 8f;

	// Token: 0x0400293F RID: 10559
	public RectTransform container;

	// Token: 0x04002940 RID: 10560
	[Header("Cards")]
	[SerializeField]
	public MapEquipUICardFront front;

	// Token: 0x04002941 RID: 10561
	[SerializeField]
	public MapEquipUICardBackSelect backSelect;

	// Token: 0x04002942 RID: 10562
	[SerializeField]
	public MapEquipUICardBackReady backReady;

	// Token: 0x04002943 RID: 10563
	[SerializeField]
	public MapEquipUIChecklist checkList;

	// Token: 0x04002944 RID: 10564
	[Header("Sprite Assets")]
	public Image[] cupheadImages;

	// Token: 0x04002945 RID: 10565
	public Image[] mugmanImages;

	// Token: 0x04002946 RID: 10566
	[SerializeField]
	public GameObject cupheadChaos;

	// Token: 0x04002947 RID: 10567
	[SerializeField]
	public GameObject mugmanChaos;

	// Token: 0x04002948 RID: 10568
	[SerializeField]
	public GameObject cuphead2POverlay;

	// Token: 0x04002949 RID: 10569
	[SerializeField]
	public GameObject mugman1POverlay;

	// Token: 0x0400294A RID: 10570
	[NonSerialized]
	public bool CanRotate;

	// Token: 0x0400294B RID: 10571
	public bool inputEnabled = true;

	// Token: 0x0400294C RID: 10572
	public AbstractEquipUI equipUI;

	// Token: 0x0400294D RID: 10573
	public MapEquipUICard.Side side;

	// Token: 0x0400294E RID: 10574
	public MapEquipUICard.Back back;

	// Token: 0x0400294F RID: 10575
	public Vector2 position;

	// Token: 0x04002950 RID: 10576
	public float rotation;

	// Token: 0x04002951 RID: 10577
	public float roll;

	// Token: 0x04002952 RID: 10578
	public PlayerId playerID;

	// Token: 0x04002953 RID: 10579
	public Player playerInput;

	// Token: 0x04002955 RID: 10581
	public const float ROTATION_FRONT = 0f;

	// Token: 0x04002956 RID: 10582
	public const float ROTATION_BACK = 180f;

	// Token: 0x04002957 RID: 10583
	public const float ROTATION_TIME = 0.15f;

	// Token: 0x04002958 RID: 10584
	public EaseUtils.EaseType ROTATION_EASE = EaseUtils.EaseType.easeOutBack;

	// Token: 0x04002959 RID: 10585
	public IEnumerator rotationCoroutine;

	// Token: 0x0400295A RID: 10586
	public const float ROLL_MIN = 1f;

	// Token: 0x0400295B RID: 10587
	public const float ROLL_MAX = 4f;

	// Token: 0x0200111D RID: 4381
	public enum Side
	{
		// Token: 0x040078D6 RID: 30934
		Front,
		// Token: 0x040078D7 RID: 30935
		Back
	}

	// Token: 0x0200111E RID: 4382
	public enum Slot
	{
		// Token: 0x040078D9 RID: 30937
		SHOT_A,
		// Token: 0x040078DA RID: 30938
		SHOT_B,
		// Token: 0x040078DB RID: 30939
		SUPER,
		// Token: 0x040078DC RID: 30940
		CHARM
	}

	// Token: 0x0200111F RID: 4383
	public enum Back
	{
		// Token: 0x040078DE RID: 30942
		Select,
		// Token: 0x040078DF RID: 30943
		Ready,
		// Token: 0x040078E0 RID: 30944
		Checklist
	}
}
