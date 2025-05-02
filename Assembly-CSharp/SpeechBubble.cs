using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D0 RID: 208
public class SpeechBubble : AbstractPausableComponent
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x000090BC File Offset: 0x000072BC
	// (set) Token: 0x060009CC RID: 2508 RVA: 0x000090C4 File Offset: 0x000072C4
	public SpeechBubble.Mode mode { get; set; }

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x000090CD File Offset: 0x000072CD
	// (set) Token: 0x060009CE RID: 2510 RVA: 0x000090D5 File Offset: 0x000072D5
	public SpeechBubble.DisplayState displayState { get; set; }

	// Token: 0x060009CF RID: 2511 RVA: 0x00079990 File Offset: 0x00077B90
	public override void Awake()
	{
		if (SpeechBubble.Instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			SpeechBubble.Instance = this;
		}
		this.arrowAnchoredPosition = this.arrowBox.anchoredPosition;
		this.panPosition = base.transform.position;
		base.Awake();
		Dialoguer.Initialize();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x000799F8 File Offset: 0x00077BF8
	public void Start()
	{
		if (this.expandOnTheRight)
		{
			base.rectTransform.anchorMin = Vector2.zero;
			base.rectTransform.anchorMax = Vector2.zero;
			base.rectTransform.pivot = Vector2.zero;
		}
		else
		{
			base.rectTransform.anchorMin = new Vector2(1f, 0f);
			base.rectTransform.anchorMax = new Vector2(1f, 0f);
			base.rectTransform.pivot = Vector2.one;
		}
		this.canvasGroup.alpha = 0f;
		this.basePosition = base.rectTransform.position;
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.AddDialoguerEvents();
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00079AC4 File Offset: 0x00077CC4
	public int ProcessChoice(int playerSelection)
	{
		int num = 0;
		int i = 0;
		while (i <= playerSelection)
		{
			if (!this.OptionHidden(num))
			{
				i++;
			}
			num++;
		}
		return num - 1;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00079AF8 File Offset: 0x00077CF8
	public void Update()
	{
		if (MapEventNotification.Current == null || !MapEventNotification.Current.showing)
		{
			if (this.waiting)
			{
				return;
			}
			if (this.waitForFade)
			{
				return;
			}
			if (this.input.GetButtonUp(CupheadButton.Accept))
			{
				this.waitForRealease = false;
			}
			if (!this.waitForRealease && this.input.GetButtonDown(CupheadButton.Accept))
			{
				if (this.currentChoiceIndex >= 0)
				{
					if (this.displayState == SpeechBubble.DisplayState.WaitForSelection)
					{
						AudioManager.Play("level_menu_select");
					}
					Dialoguer.ContinueDialogue(this.ProcessChoice(this.currentChoiceIndex));
				}
				else
				{
					Dialoguer.ContinueDialogue();
				}
			}
			if (this.displayState == SpeechBubble.DisplayState.WaitForSelection && this.input.GetButtonDown(CupheadButton.Cancel))
			{
				Dialoguer.EndDialogue();
			}
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000090DE File Offset: 0x000072DE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
		SpeechBubble.Instance = null;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000090F2 File Offset: 0x000072F2
	public void OnLanguageChanged()
	{
		this.delayedShow = true;
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x000090FB File Offset: 0x000072FB
	public void OnEnable()
	{
		if (this.delayedShow)
		{
			this.delayedShow = false;
			this.Show(this.data.text);
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00079BD4 File Offset: 0x00077DD4
	public string ProcessTextPreShow(string text)
	{
		string normalizedText = this.GetNormalizedText(Localization.Translate(text).SanitizedText());
		TMP_FontAsset fontAsset = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
		TMP_FontAsset fontAsset2 = Localization.Instance.fonts[0][27].fontAsset;
		return (!(fontAsset == fontAsset2)) ? normalizedText : this.AdjustSpacingInFont(StringVariantGenerator.Instance.Generate(normalizedText));
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00079C5C File Offset: 0x00077E5C
	public void Show(string text)
	{
		if (this.showCoroutine != null)
		{
			base.StopCoroutine(this.showCoroutine);
		}
		string text2 = this.ProcessTextPreShow(text);
		int num = 8;
		if (Localization.language == Localization.Languages.Japanese || Localization.language == Localization.Languages.SimplifiedChinese)
		{
			num = 5;
		}
		else if (Localization.language == Localization.Languages.Korean)
		{
			num = 6;
		}
		this.showCoroutine = base.StartCoroutine(this.show_cr(SpeechBubble.Mode.Text, string.Concat(new object[]
		{
			text2,
			"<space=",
			num,
			"em> "
		}), null));
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00079CF4 File Offset: 0x00077EF4
	public void Show(string text, List<string> listItems)
	{
		if (this.showCoroutine != null)
		{
			base.StopCoroutine(this.showCoroutine);
		}
		string text2 = this.ProcessTextPreShow(text);
		this.showCoroutine = base.StartCoroutine(this.show_cr(SpeechBubble.Mode.ListChoice, text2, listItems));
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00009120 File Offset: 0x00007320
	public void Dismiss()
	{
		base.StartCoroutine(this.dismiss_cr(this.preventQuit));
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00079D38 File Offset: 0x00077F38
	public virtual string GetNormalizedText(string text)
	{
		string text2 = this.mainText.text;
		TMP_FontAsset font = this.mainText.font;
		text = text.Replace("{DEATHS}", "<size=15> </size><font=\"CupheadVogue-BoldSDF\"><b><size=36>" + PlayerData.Data.DeathCount(PlayerId.Any).ToStringInvariant() + "</size></b></font><size=15> </size>");
		text = text.Replace("{BOSSREF}", this.setBossRefText);
		string text3 = text;
		if (Localization.language != Localization.Languages.Japanese)
		{
			text3 = string.Empty;
			text = text.Replace("\n", " ");
			text = text.Replace(" ", "<space=11.19853> ");
			text = text.Replace("{BR}", "\n");
			this.mainText.text = text;
			this.mainText.font = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
			this.mainText.CalculateLayoutInputHorizontal();
			string text4 = string.Empty;
			int num = 10000;
			int num2 = 0;
			while (this.mainText.text.Length > 0 && num > 0)
			{
				num--;
				while (this.mainText.text.Length > 0 && this.mainText.preferredWidth > this.maxWidth && num > 0)
				{
					num--;
					string text5 = this.mainText.text.Substring(this.mainText.text.Length - 1, 1);
					if (text5.Equals(" "))
					{
						int num3 = this.mainText.text.LastIndexOf("<");
						text4 = this.mainText.text.Substring(num3, this.mainText.text.Length - num3) + text4;
						this.mainText.text = this.mainText.text.Substring(0, num3);
					}
					else
					{
						text4 = text5 + text4;
						this.mainText.text = this.mainText.text.Substring(0, this.mainText.text.Length - 1);
					}
					this.mainText.CalculateLayoutInputHorizontal();
				}
				int num4 = this.mainText.text.LastIndexOf(" ");
				if (num4 == -1 || string.IsNullOrEmpty(text4))
				{
					if (!string.IsNullOrEmpty(text4) && text4.Substring(0, 1).Equals("<"))
					{
						text3 = text3 + this.mainText.text + "\n";
					}
					else
					{
						text3 += this.mainText.text;
					}
				}
				else
				{
					text4 = this.mainText.text.Substring(num4 + 1) + text4;
					text3 = text3 + this.mainText.text.Substring(0, num4) + "\n";
				}
				this.mainText.text = text4;
				this.mainText.CalculateLayoutInputHorizontal();
				text4 = string.Empty;
				num2++;
			}
			if (num == 0)
			{
				Debug.LogError("THE WHILES ARE DEAD, BAD CODE !!!", null);
			}
			if (this.maxLines != -1 && num2 > this.maxLines)
			{
				text3 = text3.Replace("\n", " ");
				this.mainText.enableAutoSizing = true;
				this.textLayoutElement.enabled = true;
				this.layout.padding.left = 20;
				this.layout.padding.right = 20;
				this.layout.padding.bottom = 20;
				this.layout.padding.top = 20;
			}
			else
			{
				this.mainText.enableAutoSizing = false;
				this.textLayoutElement.enabled = false;
			}
		}
		else
		{
			this.mainText.enableAutoSizing = false;
			this.textLayoutElement.enabled = false;
		}
		this.mainText.text = text2;
		this.mainText.font = font;
		return text3;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0007A150 File Offset: 0x00078350
	public string AdjustSpacingInFont(string text)
	{
		string text2 = string.Empty;
		text2 = text.Replace("<space=11.19853>\n", "\n");
		text2 = text2.Replace("<space=11.19853>]", "<space=0.01244>]");
		return text2.Replace("<space=11.19853>}", "<space=-0.00622>}");
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0007A198 File Offset: 0x00078398
	public IEnumerator show_cr(SpeechBubble.Mode mode, string text, List<string> listItems)
	{
		this.waitForFade = true;
		if (this.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield return base.StartCoroutine(this.dismiss_cr(false));
		}
		if (this.expandOnTheRight)
		{
			this.box.GetComponent<RectTransform>().pivot = Vector2.zero;
		}
		else
		{
			this.box.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
		}
		this.layout.padding.left = 30;
		this.layout.padding.right = 30;
		this.layout.padding.bottom = 30;
		this.layout.padding.top = 30;
		this.layout.spacing = 0f;
		this.mainText.text = text;
		this.mainText.font = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
		this.choiceText.font = this.mainText.font;
		foreach (RectTransform rectTransform in this.bullets)
		{
			rectTransform.gameObject.SetActive(false);
		}
		this.currentChoiceIndex = -1;
		if (mode == SpeechBubble.Mode.ListChoice)
		{
			string choiceColumn = string.Empty;
			for (int i = 0; i < listItems.Count; i++)
			{
				if (i < listItems.Count - 1)
				{
					choiceColumn = choiceColumn + Localization.Translate(listItems[i]).SanitizedText() + "\n";
				}
				else
				{
					choiceColumn += Localization.Translate(listItems[i]).SanitizedText();
				}
			}
			if (Localization.language != Localization.Languages.Korean)
			{
				this.choiceText.text = StringVariantGenerator.Instance.Generate(choiceColumn);
			}
			else
			{
				this.choiceText.text = choiceColumn;
			}
			this.currentChoiceIndex = 0;
			this.layout.spacing = 30f;
			yield return null;
		}
		else
		{
			this.choiceText.text = null;
		}
		if (this.tailOnTheLeft)
		{
			this.tail.rectTransform.anchorMin = Vector2.zero;
			this.tail.rectTransform.anchorMax = Vector2.zero;
			this.tail.rectTransform.anchoredPosition = new Vector2(73f, this.tail.rectTransform.anchoredPosition.y);
		}
		else
		{
			this.tail.rectTransform.anchorMin = new Vector2(1f, 0f);
			this.tail.rectTransform.anchorMax = new Vector2(1f, 0f);
			this.tail.rectTransform.anchoredPosition = new Vector2(-73f, this.tail.rectTransform.anchoredPosition.y);
		}
		this.arrow.color = new Color(1f, 1f, 1f, 0f);
		float maxOffset = 0.05f;
		if (CupheadLevelCamera.Current != null)
		{
			maxOffset *= 100f;
		}
		base.rectTransform.position = this.basePosition + new Vector2(Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset)) * base.rectTransform.localScale.x;
		this.tail.sprite = this.tailVariants.RandomChoice<Sprite>();
		this.tail.enabled = !this.hideTail;
		this.arrow.sprite = this.arrowVariants.RandomChoice<Sprite>();
		base.animator.Play("Idle", 0, Random.Range(0f, 1f));
		base.animator.Play("Idle", 1, Random.Range(0f, 1f));
		this.displayState = SpeechBubble.DisplayState.FadeIn;
		yield return base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, 1f));
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.displayState = SpeechBubble.DisplayState.Showing;
		this.showCoroutine = null;
		Color colorHidden = new Color(1f, 1f, 1f, 0f);
		Color colorShown = new Color(1f, 1f, 1f, 1f);
		if (this.expandOnTheRight)
		{
			this.arrowBox.anchoredPosition = new Vector2(this.arrowAnchoredPosition.x + this.box.sizeDelta.x, this.arrowBox.anchoredPosition.y);
		}
		if (mode == SpeechBubble.Mode.Text)
		{
			this.arrow.color = ((!this.waiting) ? colorShown : colorHidden);
			this.cursor.color = colorHidden;
		}
		else
		{
			this.cursor.color = ((!this.waiting) ? colorShown : colorHidden);
			this.displayState = SpeechBubble.DisplayState.WaitForSelection;
			this.waitForFade = false;
			while (this.displayState == SpeechBubble.DisplayState.WaitForSelection)
			{
				if (this.waiting)
				{
					yield return null;
				}
				else
				{
					if (PauseManager.state != PauseManager.State.Paused)
					{
						if (this.input.GetButtonDown(CupheadButton.MenuDown) && this.currentChoiceIndex < listItems.Count - 1)
						{
							this.currentChoiceIndex++;
							base.animator.SetTrigger("MoveDown");
							AudioManager.Play("level_menu_move");
						}
						if (this.input.GetButtonDown(CupheadButton.MenuUp) && this.currentChoiceIndex > 0)
						{
							this.currentChoiceIndex--;
							base.animator.SetTrigger("MoveUp");
							AudioManager.Play("level_menu_move");
						}
					}
					this.cursorRoot.anchoredPosition = this.getCursorPos(this.currentChoiceIndex, listItems.Count);
					this.cursor.color = colorShown;
					yield return null;
				}
			}
		}
		this.waitForFade = false;
		this.cursor.color = colorHidden;
		yield break;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0007A1C8 File Offset: 0x000783C8
	public IEnumerator dismiss_cr(bool watchPreventQuit)
	{
		if (this.displayState == SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		while (this.displayState == SpeechBubble.DisplayState.FadeIn)
		{
			yield return null;
		}
		if (watchPreventQuit)
		{
			while (this.preventQuit)
			{
				yield return null;
			}
		}
		this.displayState = SpeechBubble.DisplayState.FadeOut;
		yield return base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, 0f));
		this.displayState = SpeechBubble.DisplayState.Hidden;
		yield break;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0007A1EC File Offset: 0x000783EC
	public IEnumerator fade_cr(float startOpacity, float endOpacity)
	{
		if (endOpacity == 0f)
		{
			this.canvasGroup.alpha = endOpacity;
			yield break;
		}
		yield return null;
		float t = 0f;
		while (t < 0.07f)
		{
			yield return null;
			t += CupheadTime.Delta;
			this.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / 0.07f);
		}
		this.canvasGroup.alpha = endOpacity;
		yield break;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0007A218 File Offset: 0x00078418
	public Vector2 getCursorPos(int choiceIndex, int choiceCount)
	{
		float num = this.choiceText.bounds.extents.y / (float)choiceCount * 2f;
		return new Vector2(this.choiceText.margin.x - 10f, 0f) + Vector2.up * (((float)choiceCount - 1f) / 2f) * num + Vector2.down * ((float)choiceIndex * num);
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00009135 File Offset: 0x00007335
	public void setOpacity(float opacity)
	{
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x0007A2A4 File Offset: 0x000784A4
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.OnDialogueStartedHandler;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.OnDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.OnDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0007A338 File Offset: 0x00078538
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onStarted -= this.OnDialogueStartedHandler;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase -= this.OnDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose -= this.OnDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0007A3CC File Offset: 0x000785CC
	public void OnDialogueStartedHandler()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Event;
		}
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.MoveToPosition(this.panPosition, 0.75f, 1f);
		}
		if (MapUIVignetteDialogue.Current != null)
		{
			MapUIVignetteDialogue.Current.FadeIn();
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00009137 File Offset: 0x00007337
	public void OnDialogueEndedHandler()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
		this.Dismiss();
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00009150 File Offset: 0x00007350
	public void OnDialogueInstantlyEndedHandler()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
		this.Dismiss();
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0007A44C File Offset: 0x0007864C
	public void OnDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this.data = data;
		if (data.choices == null)
		{
			this.Show(data.text);
		}
		else if (data.choices.Length > 0)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < data.choices.Length; i++)
			{
				if (!this.OptionHidden(i))
				{
					list.Add(data.choices[i]);
				}
			}
			this.Show(data.text, list);
		}
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00009169 File Offset: 0x00007369
	public void ClearHideOptionBitmask()
	{
		this.hideOptionBitmask = 0;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00009172 File Offset: 0x00007372
	public void HideOptionByIndex(int i)
	{
		this.hideOptionBitmask |= 1 << i;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00009187 File Offset: 0x00007387
	public bool OptionHidden(int i)
	{
		return (this.hideOptionBitmask & 1 << i) != 0;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0007A4D8 File Offset: 0x000786D8
	public void OnDialogueWindowCloseHandler()
	{
		this.Dismiss();
		this.ClearHideOptionBitmask();
		if (MapUIVignetteDialogue.Current != null)
		{
			MapUIVignetteDialogue.Current.FadeOut();
		}
		if (Map.Current != null && Map.Current.CurrentState != Map.State.Graveyard)
		{
			Map.Current.CurrentState = Map.State.Ready;
		}
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0000919C File Offset: 0x0000739C
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "Wait")
		{
			base.StartCoroutine(this.wait_cr(Parser.FloatParse(metadata)));
		}
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0007A538 File Offset: 0x00078738
	public IEnumerator wait_cr(float waitDuration)
	{
		this.waiting = true;
		this.arrow.color = new Color(1f, 1f, 1f, 0f);
		while (waitDuration > 0f)
		{
			yield return null;
			waitDuration -= CupheadTime.Delta;
		}
		this.waiting = false;
		this.arrow.color = new Color(1f, 1f, 1f, 1f);
		yield break;
	}

	// Token: 0x04000762 RID: 1890
	public const int REGULAR_ARROW_PADDING = 8;

	// Token: 0x04000763 RID: 1891
	public const int KOREAN_ARROW_PADDING = 6;

	// Token: 0x04000764 RID: 1892
	public const int JAP_CHI_ARROW_PADDING = 5;

	// Token: 0x04000765 RID: 1893
	public const float DEFAULT_TIME = 2f;

	// Token: 0x04000766 RID: 1894
	public const float FADE_TIME = 0.07f;

	// Token: 0x04000767 RID: 1895
	public const float END_TIME = 0.25f;

	// Token: 0x04000768 RID: 1896
	public const float ARROW_WAIT_TIME = 0.125f;

	// Token: 0x04000769 RID: 1897
	public const float MAX_RANDOM_OFFSET = 0.05f;

	// Token: 0x0400076A RID: 1898
	public const int MAX_CHOICES_PER_COLUMN = 4;

	// Token: 0x0400076B RID: 1899
	public const int COLUMN_PADDING = 55;

	// Token: 0x0400076C RID: 1900
	public const int COLUMN_SPACING = 45;

	// Token: 0x0400076D RID: 1901
	public const int DEFAULT_PADDING = 30;

	// Token: 0x0400076E RID: 1902
	public const int SMALL_PADDING = 20;

	// Token: 0x0400076F RID: 1903
	public const float TAIL_POSITION_X = -73f;

	// Token: 0x04000770 RID: 1904
	public const float CURSOR_OFFSET_H = 10f;

	// Token: 0x04000773 RID: 1907
	public static SpeechBubble Instance;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	public TextMeshProUGUI mainText;

	// Token: 0x04000775 RID: 1909
	[SerializeField]
	public TextMeshProUGUI choiceText;

	// Token: 0x04000776 RID: 1910
	[SerializeField]
	public VerticalLayoutGroup layout;

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	public Image tail;

	// Token: 0x04000778 RID: 1912
	[SerializeField]
	public List<Sprite> tailVariants;

	// Token: 0x04000779 RID: 1913
	[SerializeField]
	public RectTransform arrowBox;

	// Token: 0x0400077A RID: 1914
	[SerializeField]
	public Image arrow;

	// Token: 0x0400077B RID: 1915
	[SerializeField]
	public Image cursor;

	// Token: 0x0400077C RID: 1916
	[SerializeField]
	public RectTransform cursorRoot;

	// Token: 0x0400077D RID: 1917
	[SerializeField]
	public RectTransform box;

	// Token: 0x0400077E RID: 1918
	[SerializeField]
	public List<Sprite> arrowVariants;

	// Token: 0x0400077F RID: 1919
	[SerializeField]
	public CanvasGroup canvasGroup;

	// Token: 0x04000780 RID: 1920
	[SerializeField]
	public List<RectTransform> bullets;

	// Token: 0x04000781 RID: 1921
	public float maxWidth = 558f;

	// Token: 0x04000782 RID: 1922
	public Vector2 arrowAnchoredPosition;

	// Token: 0x04000783 RID: 1923
	public Vector2 basePosition;

	// Token: 0x04000784 RID: 1924
	public Vector2 panPosition;

	// Token: 0x04000785 RID: 1925
	public int currentChoiceIndex;

	// Token: 0x04000786 RID: 1926
	public int hideOptionBitmask;

	// Token: 0x04000787 RID: 1927
	public string setBossRefText = string.Empty;

	// Token: 0x04000788 RID: 1928
	public int maxLines = -1;

	// Token: 0x04000789 RID: 1929
	public bool tailOnTheLeft;

	// Token: 0x0400078A RID: 1930
	public bool expandOnTheRight;

	// Token: 0x0400078B RID: 1931
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x0400078C RID: 1932
	public bool waitForRealease;

	// Token: 0x0400078D RID: 1933
	public bool waitForFade;

	// Token: 0x0400078E RID: 1934
	public bool hideTail;

	// Token: 0x0400078F RID: 1935
	public Coroutine showCoroutine;

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	public LayoutElement textLayoutElement;

	// Token: 0x04000791 RID: 1937
	public bool waiting;

	// Token: 0x04000792 RID: 1938
	public bool delayedShow;

	// Token: 0x04000793 RID: 1939
	public DialoguerTextData data;

	// Token: 0x04000794 RID: 1940
	[HideInInspector]
	public bool preventQuit;

	// Token: 0x0200093E RID: 2366
	public enum Mode
	{
		// Token: 0x040045A7 RID: 17831
		Text,
		// Token: 0x040045A8 RID: 17832
		ListChoice
	}

	// Token: 0x0200093F RID: 2367
	public enum DisplayState
	{
		// Token: 0x040045AA RID: 17834
		Hidden,
		// Token: 0x040045AB RID: 17835
		FadeIn,
		// Token: 0x040045AC RID: 17836
		Showing,
		// Token: 0x040045AD RID: 17837
		WaitForSelection,
		// Token: 0x040045AE RID: 17838
		FadeOut
	}
}
