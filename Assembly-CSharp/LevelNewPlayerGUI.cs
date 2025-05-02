using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000103 RID: 259
public class LevelNewPlayerGUI : AbstractMonoBehaviour
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0000A9CE File Offset: 0x00008BCE
	// (set) Token: 0x06000C0C RID: 3084 RVA: 0x0000A9D5 File Offset: 0x00008BD5
	public static LevelNewPlayerGUI Current { get; set; }

	// Token: 0x06000C0D RID: 3085 RVA: 0x00082358 File Offset: 0x00080558
	public override void Awake()
	{
		base.Awake();
		if (PlayerManager.player1IsMugman)
		{
			this.card.sprite = this.cupheadCard;
		}
		LevelNewPlayerGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x0000A9DD File Offset: 0x00008BDD
	public void OnDestroy()
	{
		if (LevelNewPlayerGUI.Current == this)
		{
			LevelNewPlayerGUI.Current = null;
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x000823B4 File Offset: 0x000805B4
	public void Init()
	{
		base.gameObject.SetActive(true);
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers && OnlineManager.Instance.Interface.GetUser(PlayerId.PlayerTwo) != null)
		{
			this.localizationHelper.ApplyTranslation(Localization.Find("PlayerTwoJoinedWithUser"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("USERNAME", OnlineManager.Instance.Interface.GetUser(PlayerId.PlayerTwo).Name, true)
			});
		}
		base.StartCoroutine(this.tweenIn_cr());
		base.StartCoroutine(this.text_cr());
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00082458 File Offset: 0x00080658
	public IEnumerator tweenIn_cr()
	{
		base.animator.Play("In");
		float t = 0f;
		AudioManager.Play("player_joined");
		PauseManager.Pause();
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		yield return new WaitForSeconds(2f);
		base.animator.Play("Out");
		base.StartCoroutine(this.tweenOut_cr());
		yield break;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00082474 File Offset: 0x00080674
	public IEnumerator tweenOut_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		while (InterruptingPrompt.IsInterrupting())
		{
			yield return null;
		}
		PauseManager.Unpause();
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00082490 File Offset: 0x00080690
	public IEnumerator text_cr()
	{
		for (;;)
		{
			this.text.color = Color.white;
			yield return new WaitForSeconds(0.0416666679f);
			this.text.color = ((!PlayerManager.player1IsMugman) ? this.mugmanColor : this.cupheadColor);
			yield return new WaitForSeconds(0.0416666679f);
		}
		yield break;
	}

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	public Image background;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	public Image card;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	public Sprite cupheadCard;

	// Token: 0x040009A9 RID: 2473
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x040009AA RID: 2474
	[SerializeField]
	public LocalizationHelper localizationHelper;

	// Token: 0x040009AB RID: 2475
	[SerializeField]
	public Color cupheadColor;

	// Token: 0x040009AC RID: 2476
	[SerializeField]
	public Color mugmanColor;

	// Token: 0x040009AD RID: 2477
	public CanvasGroup canvasGroup;
}
