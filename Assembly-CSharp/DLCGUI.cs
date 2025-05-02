using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EC RID: 236
public class DLCGUI : AbstractMonoBehaviour
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00009FA6 File Offset: 0x000081A6
	// (set) Token: 0x06000B1D RID: 2845 RVA: 0x00009FAE File Offset: 0x000081AE
	public bool dlcMenuOpen { get; set; }

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00009FB7 File Offset: 0x000081B7
	// (set) Token: 0x06000B1F RID: 2847 RVA: 0x00009FBF File Offset: 0x000081BF
	public bool inputEnabled { get; set; }

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00009FC8 File Offset: 0x000081C8
	// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00009FD0 File Offset: 0x000081D0
	public bool justClosed { get; set; }

	// Token: 0x06000B22 RID: 2850 RVA: 0x00009FD9 File Offset: 0x000081D9
	public override void Awake()
	{
		base.Awake();
		this.dlcMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0000A004 File Offset: 0x00008204
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0007DF84 File Offset: 0x0007C184
	public void Update()
	{
		this.justClosed = false;
		this.timeSinceStart += Time.deltaTime;
		this.timeSinceConfirmPressed += Time.deltaTime;
		if (this.timeSinceStart < 0.25f)
		{
			return;
		}
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			base.StartCoroutine(this.hide_cr());
			return;
		}
		if (!this.dlcEnabled && this.timeSinceConfirmPressed >= 0.5f && DLCManager.CanRedirectToStore() && this.GetButtonDown(CupheadButton.Accept))
		{
			this.timeSinceConfirmPressed = 0f;
			DLCManager.LaunchStore();
			return;
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0007E038 File Offset: 0x0007C238
	public void ShowDLCMenu()
	{
		this.dlcEnabled = DLCManager.DLCEnabled();
		this.timeSinceStart = 0f;
		this.timeSinceConfirmPressed = 0f;
		this.dlcMenuOpen = true;
		this.canvasGroup.alpha = 1f;
		base.StartCoroutine(this.show_cr());
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0000A012 File Offset: 0x00008212
	public void hideDLCMenu()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.dlcMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0000A051 File Offset: 0x00008251
	public void interactable()
	{
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0007E08C File Offset: 0x0007C28C
	public IEnumerator show_cr()
	{
		Transform scaler;
		Text text;
		if (this.dlcEnabled)
		{
			scaler = this.installedScaler;
			this.notInstalled.SetActive(false);
			this.installed.SetActive(true);
			text = this.installedText;
		}
		else
		{
			scaler = this.notInstalledScaler;
			this.notInstalled.SetActive(true);
			this.installed.SetActive(false);
			text = this.notInstalledText;
		}
		this.fader.color = new Color(0f, 0f, 0f, DLCGUI.FaderAlpha);
		Image[] fadeImages = scaler.GetComponentsInChildren<Image>();
		foreach (Image image in fadeImages)
		{
			Color color2 = image.color;
			color2.a = 1f;
			image.color = color2;
		}
		float elapsedTime = 0f;
		while (elapsedTime < 0.4f)
		{
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = scaler.localScale;
			scale.x = (scale.y = EaseUtils.EaseOutCubic(2f, 1f, elapsedTime / 0.4f));
			scaler.localScale = scale;
			Color color = text.color;
			color.a = Mathf.Lerp(0f, 1f, elapsedTime / 0.4f);
			text.color = color;
			yield return null;
		}
		this.interactable();
		yield break;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0007E0A8 File Offset: 0x0007C2A8
	public IEnumerator hide_cr()
	{
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		Transform scaler;
		Text text;
		if (this.dlcEnabled)
		{
			scaler = this.installedScaler;
			this.notInstalled.SetActive(false);
			this.installed.SetActive(true);
			text = this.installedText;
		}
		else
		{
			scaler = this.notInstalledScaler;
			this.notInstalled.SetActive(true);
			this.installed.SetActive(false);
			text = this.notInstalledText;
		}
		Image[] fadeImages = scaler.GetComponentsInChildren<Image>();
		float elapsedTime = 0f;
		while (elapsedTime < 0.2f)
		{
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = scaler.localScale;
			scale.x = (scale.y = EaseUtils.EaseInCubic(1f, 2f, elapsedTime / 0.2f));
			scaler.localScale = scale;
			Color color = text.color;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.2f);
			text.color = color;
			foreach (Image image in fadeImages)
			{
				color = image.color;
				color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.2f);
				image.color = color;
			}
			color = this.fader.color;
			color.a = Mathf.Lerp(DLCGUI.FaderAlpha, 0f, elapsedTime / 0.2f);
			this.fader.color = color;
			yield return null;
		}
		this.hideDLCMenu();
		yield break;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0000A072 File Offset: 0x00008272
	public bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x040008CB RID: 2251
	public static readonly float FaderAlpha = 0.5f;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	public GameObject notInstalled;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	public GameObject installed;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	public Transform notInstalledScaler;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	public Transform installedScaler;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	public Image fader;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	public Text notInstalledText;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	public Text installedText;

	// Token: 0x040008D3 RID: 2259
	public CanvasGroup canvasGroup;

	// Token: 0x040008D4 RID: 2260
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040008D5 RID: 2261
	public float timeSinceStart;

	// Token: 0x040008D6 RID: 2262
	public float timeSinceConfirmPressed;

	// Token: 0x040008D7 RID: 2263
	public bool dlcEnabled;
}
