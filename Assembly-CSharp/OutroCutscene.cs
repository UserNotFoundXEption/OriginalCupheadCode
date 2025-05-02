using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C5 RID: 197
public class OutroCutscene : Cutscene
{
	// Token: 0x060008F7 RID: 2295 RVA: 0x00076F7C File Offset: 0x0007517C
	public override void Start()
	{
		base.Start();
		this.book.SetActive(Localization.language == Localization.Languages.English);
		this.bookLocalized.SetActive(Localization.language != Localization.Languages.English);
		CreditsScreen.goodEnding = true;
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00076FF4 File Offset: 0x000751F4
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.33f);
		int numScreens = 6;
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.75f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
			if (i != 5)
			{
				this.NextPageSFX();
			}
			if (i == 0)
			{
				this.FireWhooshSFX();
			}
			if (i == 4)
			{
				this.Cheering();
			}
		}
		CreditsScreen.goodEnding = true;
		yield return CupheadTime.WaitForSeconds(this, 6.25f);
		AudioManager.FadeBGMVolume(0f, 3f, true);
		yield return CupheadTime.WaitForSeconds(this, 3f);
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
		yield break;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00077010 File Offset: 0x00075210
	public IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0007702C File Offset: 0x0007522C
	public void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency * 0.35f);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000087FA File Offset: 0x000069FA
	public void NextPageSFX()
	{
		AudioManager.Play("ui_confirm");
		AudioManager.Play("ui_pageturn");
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00008810 File Offset: 0x00006A10
	public void FireWhooshSFX()
	{
		AudioManager.Play("firewhoosh");
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0000881C File Offset: 0x00006A1C
	public void Cheering()
	{
		AudioManager.Play("cheering");
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00008828 File Offset: 0x00006A28
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00008837 File Offset: 0x00006A37
	public override void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Ending", true);
	}

	// Token: 0x040006CE RID: 1742
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040006CF RID: 1743
	[SerializeField]
	public Image arrow;

	// Token: 0x040006D0 RID: 1744
	[SerializeField]
	public GameObject book;

	// Token: 0x040006D1 RID: 1745
	[SerializeField]
	public GameObject bookLocalized;

	// Token: 0x040006D2 RID: 1746
	public float arrowTransparency;

	// Token: 0x040006D3 RID: 1747
	public bool arrowVisible;
}
