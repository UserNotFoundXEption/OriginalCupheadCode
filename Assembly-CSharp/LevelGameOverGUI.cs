using System;
using System.Collections;
using RektTransform;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
[RequireComponent(typeof(CanvasGroup))]
public class LevelGameOverGUI : AbstractMonoBehaviour
{
	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0000A831 File Offset: 0x00008A31
	// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x0000A838 File Offset: 0x00008A38
	public static Color COLOR_SELECTED { get; set; }

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0000A840 File Offset: 0x00008A40
	// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x0000A847 File Offset: 0x00008A47
	public static Color COLOR_INACTIVE { get; set; }

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0000A84F File Offset: 0x00008A4F
	// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x0000A856 File Offset: 0x00008A56
	public static Color COLOR_DESABLE { get; set; }

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00081B3C File Offset: 0x0007FD3C
	public override void Awake()
	{
		base.Awake();
		LevelGameOverGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		base.gameObject.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.cardCanvasGroup.alpha = 0f;
		this.helpCanvasGroup.alpha = 0f;
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
		LevelGameOverGUI.COLOR_SELECTED = this.menuItems[0].color;
		LevelGameOverGUI.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
		if (Level.IsTowerOfPower)
		{
			this.equipToolTip.SetActive(false);
			if (!TowerOfPowerLevelGameInfo.IsTokenLeft())
			{
				this.menuItems[0].gameObject.SetActive(false);
				this.selection = 1;
				this.UpdateSelection();
			}
			else
			{
				this.retryLocHelper.currentID = Localization.Find("OptionMenuRetryTowerBattle").id;
				this.retryLocHelper.ApplyTranslation();
			}
		}
		this.state = LevelGameOverGUI.State.Init;
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0000A85E File Offset: 0x00008A5E
	public void Start()
	{
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			this.updateRotateControlsToggleVisualValue();
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0000A88A File Offset: 0x00008A8A
	public void OnDestroy()
	{
		LevelGameOverGUI.Current = null;
		this.youDiedText = null;
		this.bossPortraitImage = null;
		this.timeline.cuphead = null;
		this.timeline.mugman = null;
		this.timeline = null;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00081C48 File Offset: 0x0007FE48
	public void Update()
	{
		if (this.state != LevelGameOverGUI.State.Ready)
		{
			return;
		}
		if (this.selection == 2 && Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane && (this.getButtonDown(CupheadButton.Accept) || this.getButtonDown(CupheadButton.MenuLeft) || this.getButtonDown(CupheadButton.MenuRight)))
		{
			AudioManager.Play("level_menu_card_down");
			this.toggleRotateControls();
			return;
		}
		int num = 0;
		if (this.getButtonDown(CupheadButton.Accept))
		{
			this.Select();
			AudioManager.Play("level_menu_select");
			this.state = LevelGameOverGUI.State.Exiting;
		}
		if (!Level.IsTowerOfPower && this.getButtonDown(CupheadButton.EquipMenu))
		{
			this.ChangeEquipment();
		}
		if (this.getButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			num++;
		}
		if (this.getButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			num--;
		}
		this.selection += num;
		this.selection = Mathf.Clamp(this.selection, 0, this.menuItems.Length - 1);
		if (!this.menuItems[this.selection].gameObject.activeSelf)
		{
			this.selection -= num;
			this.selection = Mathf.Clamp(this.selection, 0, this.menuItems.Length - 1);
		}
		this.UpdateSelection();
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0000A8BF File Offset: 0x00008ABF
	public bool getButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00081DBC File Offset: 0x0007FFBC
	public void UpdateSelection()
	{
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = LevelGameOverGUI.COLOR_SELECTED;
			}
			else
			{
				text.color = LevelGameOverGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00081E14 File Offset: 0x00080014
	public void Select()
	{
		if (!Level.IsGraveyard)
		{
			AudioManager.SnapshotReset(SceneLoader.SceneName, 2f);
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			SettingsData.Save();
			if (PlatformHelper.IsConsole)
			{
				SettingsData.SaveToCloud();
			}
		}
		switch (this.selection)
		{
		default:
			this.Retry();
			AudioManager.Play("level_menu_card_down");
			break;
		case 1:
			this.ExitToMap();
			AudioManager.Play("level_menu_card_down");
			break;
		case 2:
			this.QuitGame();
			AudioManager.Play("level_menu_card_down");
			break;
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0000A8CD File Offset: 0x00008ACD
	public void Retry()
	{
		if (Level.IsDicePalaceMain || Level.IsDicePalace)
		{
			DicePalaceMainLevelGameInfo.CleanUpRetry();
		}
		SceneLoader.ReloadLevel();
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0000A8ED File Offset: 0x00008AED
	public void ExitToMap()
	{
		SceneLoader.LoadLastMap();
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0000A8F4 File Offset: 0x00008AF4
	public void QuitGame()
	{
		Level.IsGraveyard = false;
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0000A90B File Offset: 0x00008B0B
	public void ChangeEquipment()
	{
		base.StartCoroutine(this.outforequip_cr());
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0000A91A File Offset: 0x00008B1A
	public void ReactivateOnChangeEquipmentClosed()
	{
		base.StartCoroutine(this.inforequip_cr());
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0000A929 File Offset: 0x00008B29
	public void SetAlpha(float value)
	{
		this.canvasGroup.alpha = value;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00081EE0 File Offset: 0x000800E0
	public void SetTextAlpha(float value)
	{
		Color color = this.youDiedText.color;
		color.a = value;
		this.youDiedText.color = color;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00081F10 File Offset: 0x00080110
	public void SetCardValue(float value)
	{
		this.cardCanvasGroup.alpha = value;
		this.helpCanvasGroup.alpha = value;
		this.cardCanvasGroup.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(30f, 4f, value)));
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00081F6C File Offset: 0x0008016C
	public void SetCardValueEquipSwap(float value)
	{
		this.cardCanvasGroup.alpha = value;
		this.helpCanvasGroup.alpha = value;
		this.cardCanvasGroup.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(30f, 4f, value)));
		this.cardCanvasGroup.transform.SetLocalPosition(null, new float?(Mathf.Lerp(-720f, 0f, value)), null);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00082000 File Offset: 0x00080200
	public void In(bool secretTriggered)
	{
		base.gameObject.SetActive(true);
		this.bossPortraitImage.sprite = Level.Current.BossPortrait;
		if (secretTriggered)
		{
			this.cardCanvasGroup.GetComponent<Image>().sprite = this.timelineSecret;
			this.timelineObj.SetActive(false);
		}
		if (this.bossQuoteLocalization == null)
		{
			this.bossQuoteText.text = "\"" + Level.Current.BossQuote + "\"";
		}
		else
		{
			this.bossQuoteLocalization.ApplyTranslation(Localization.Find(Level.Current.BossQuote), null);
			if (Localization.language == Localization.Languages.Korean)
			{
				this.bossQuoteLocalization.textMeshProComponent.fontStyle = FontStyles.Bold;
			}
		}
		if (this.bossPortraitImage.sprite != null)
		{
			this.bossPortraitImage.rectTransform.SetSize(this.bossPortraitImage.sprite.rect.width, this.bossPortraitImage.sprite.rect.height);
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0008212C File Offset: 0x0008032C
	public IEnumerator in_cr()
	{
		AudioManager.Play("level_menu_card_up");
		yield return base.TweenValue(0f, 1f, 0.05f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetAlpha));
		yield return new WaitForSeconds(1f);
		foreach (PlayerDeathEffect playerDeathEffect in Object.FindObjectsOfType<PlayerDeathEffect>())
		{
			playerDeathEffect.GameOverUnpause();
		}
		foreach (PlanePlayerDeathPart planePlayerDeathPart in Object.FindObjectsOfType<PlanePlayerDeathPart>())
		{
			planePlayerDeathPart.GameOverUnpause();
		}
		yield return base.TweenValue(1f, 0f, 0.25f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetTextAlpha));
		yield return new WaitForSeconds(0.3f);
		if (!Level.IsGraveyard && !Level.IsChessBoss)
		{
			AudioManager.Play("player_die_vinylscratch");
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4f);
			AudioManager.ChangeBGMPitch(0.7f, 6f);
		}
		CupheadLevelCamera.Current.StartBlur();
		this.timeline.Setup(this, Level.Current.timeline);
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValue));
		this.state = LevelGameOverGUI.State.Ready;
		yield return null;
		yield break;
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x00082148 File Offset: 0x00080348
	public IEnumerator outforequip_cr()
	{
		this.state = LevelGameOverGUI.State.Init;
		this.equipUI.gameObject.SetActive(true);
		this.equipUI.Activate();
		yield return base.TweenValue(1f, 0f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValueEquipSwap));
		yield break;
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00082164 File Offset: 0x00080364
	public IEnumerator inforequip_cr()
	{
		yield return base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValueEquipSwap));
		this.state = LevelGameOverGUI.State.Ready;
		yield break;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0000A937 File Offset: 0x00008B37
	public void toggleRotateControls()
	{
		SettingsData.Data.rotateControlsWithCamera = !SettingsData.Data.rotateControlsWithCamera;
		this.updateRotateControlsToggleVisualValue();
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00082180 File Offset: 0x00080380
	public void updateRotateControlsToggleVisualValue()
	{
		Text text = this.menuItems[2];
		text.GetComponent<LocalizationHelper>().ApplyTranslation(Localization.Find("CameraRotationControl"), null);
		text.text = string.Format(text.text, (!SettingsData.Data.rotateControlsWithCamera) ? "A" : "B");
	}

	// Token: 0x04000983 RID: 2435
	public static LevelGameOverGUI Current;

	// Token: 0x04000984 RID: 2436
	[SerializeField]
	public Image youDiedText;

	// Token: 0x04000985 RID: 2437
	[Space(10f)]
	[SerializeField]
	public CanvasGroup cardCanvasGroup;

	// Token: 0x04000986 RID: 2438
	[Space(10f)]
	[SerializeField]
	public CanvasGroup helpCanvasGroup;

	// Token: 0x04000987 RID: 2439
	[Space(10f)]
	[SerializeField]
	public Image bossPortraitImage;

	// Token: 0x04000988 RID: 2440
	[SerializeField]
	public Text bossQuoteText;

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	public LocalizationHelper bossQuoteLocalization;

	// Token: 0x0400098A RID: 2442
	[Space(10f)]
	[SerializeField]
	public Text[] menuItems;

	// Token: 0x0400098B RID: 2443
	[SerializeField]
	public LevelGameOverGUI.TimelineObjects timeline;

	// Token: 0x0400098C RID: 2444
	[SerializeField]
	public GameObject timelineObj;

	// Token: 0x0400098D RID: 2445
	[SerializeField]
	public Sprite timelineSecret;

	// Token: 0x0400098E RID: 2446
	[SerializeField]
	public LevelEquipUI equipUI;

	// Token: 0x0400098F RID: 2447
	[SerializeField]
	public GameObject equipToolTip;

	// Token: 0x04000990 RID: 2448
	public LevelGameOverGUI.State state;

	// Token: 0x04000991 RID: 2449
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000992 RID: 2450
	public CanvasGroup canvasGroup;

	// Token: 0x04000993 RID: 2451
	public int selection;

	// Token: 0x04000994 RID: 2452
	[SerializeField]
	public LocalizationHelper retryLocHelper;

	// Token: 0x02000979 RID: 2425
	public enum State
	{
		// Token: 0x040046E6 RID: 18150
		Init,
		// Token: 0x040046E7 RID: 18151
		Ready,
		// Token: 0x040046E8 RID: 18152
		Exiting
	}

	// Token: 0x0200097A RID: 2426
	[Serializable]
	public class TimelineObjects
	{
		// Token: 0x0600551E RID: 21790 RVA: 0x001C5A0C File Offset: 0x001C3C0C
		public void Setup(LevelGameOverGUI gui, Level.Timeline properties)
		{
			int num = 0;
			foreach (Level.Timeline.Event @event in properties.events)
			{
				RectTransform rectTransform = Object.Instantiate<RectTransform>(this.line);
				rectTransform.SetParent(this.line.parent, false);
				rectTransform.SetAsFirstSibling();
				rectTransform.name = "Line " + num++;
				Vector3 localPosition = Vector3.Lerp(this.end.localPosition, this.start.localPosition, @event.percentage);
				localPosition.y -= 7f;
				rectTransform.localPosition = localPosition;
			}
			this.line.gameObject.SetActive(false);
			Image image = (!PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice;
			float num2 = (!PlayerManager.player1IsMugman) ? properties.cuphead : properties.mugman;
			gui.StartCoroutine(this.timelineIcon_cr(image, num2 / properties.health));
			Image image2 = null;
			if (PlayerManager.Multiplayer)
			{
				image2 = ((!PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
				float num3 = (!PlayerManager.player1IsMugman) ? properties.mugman : properties.cuphead;
				gui.StartCoroutine(this.timelineIcon_cr(image2, num3 / properties.health));
			}
			this.cuphead.gameObject.SetActive(image == this.cuphead || image2 == this.cuphead);
			this.mugman.gameObject.SetActive(image == this.mugman || image2 == this.mugman);
			this.chalice.gameObject.SetActive(image == this.chalice || image2 == this.chalice);
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001C5C7C File Offset: 0x001C3E7C
		public IEnumerator timelineIcon_cr(Image icon, float percent)
		{
			Color startColor = new Color(1f, 1f, 1f, 0f);
			Color endColor = new Color(1f, 1f, 1f, 1f);
			float t = 0f;
			Vector3 endPosition = Vector3.Lerp(this.start.localPosition, this.end.localPosition, percent);
			icon.rectTransform.localPosition = this.start.localPosition;
			while (t < 2f)
			{
				float val = t / 2f;
				Vector3 newPosition = Vector3.Lerp(this.start.localPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
				icon.rectTransform.localPosition = newPosition;
				icon.color = Color.Lerp(startColor, endColor, val * 8f);
				t += Time.deltaTime;
				yield return null;
			}
			icon.rectTransform.localPosition = endPosition;
			yield break;
		}

		// Token: 0x040046E9 RID: 18153
		public RectTransform timeline;

		// Token: 0x040046EA RID: 18154
		public RectTransform line;

		// Token: 0x040046EB RID: 18155
		[Header("Players")]
		public Image cuphead;

		// Token: 0x040046EC RID: 18156
		public Image mugman;

		// Token: 0x040046ED RID: 18157
		public Image chalice;

		// Token: 0x040046EE RID: 18158
		[Header("Positions")]
		public Transform start;

		// Token: 0x040046EF RID: 18159
		public Transform end;

		// Token: 0x040046F0 RID: 18160
		public LevelGameOverGUI gui;
	}
}
