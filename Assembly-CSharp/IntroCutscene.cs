using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C3 RID: 195
public class IntroCutscene : Cutscene
{
	// Token: 0x060008E6 RID: 2278 RVA: 0x00076D2C File Offset: 0x00074F2C
	public override void Start()
	{
		base.Start();
		this.book.SetActive(Localization.language == Localization.Languages.English);
		this.bookLocalized.SetActive(Localization.language != Localization.Languages.English);
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00076DA0 File Offset: 0x00074FA0
	public IEnumerator main_cr()
	{
		int numScreens = 11;
		yield return CupheadTime.WaitForSeconds(this, 6f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.75f);
			this.arrowVisible = true;
			while (this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
			if (i != numScreens - 1)
			{
				this.NextPageSFX();
			}
			if (i == 2)
			{
				this.DevilLaugh();
			}
			if (i == 4)
			{
				this.DiceRoll();
			}
			if (i == 5)
			{
				this.DevilSlam();
			}
			if (i == 8)
			{
				this.DevilKick();
			}
		}
		AudioManager.FadeBGMVolume(0f, 0.75f, true);
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00076DBC File Offset: 0x00074FBC
	public IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00076DD8 File Offset: 0x00074FD8
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

	// Token: 0x060008EA RID: 2282 RVA: 0x00008786 File Offset: 0x00006986
	public void NextPageSFX()
	{
		AudioManager.Play("ui_confirm");
		AudioManager.Play("ui_pageturn");
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0000879C File Offset: 0x0000699C
	public void DevilLaugh()
	{
		AudioManager.Play("devil_laugh");
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000087A8 File Offset: 0x000069A8
	public void DiceRoll()
	{
		AudioManager.Play("dice_roll");
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x000087B4 File Offset: 0x000069B4
	public void DevilSlam()
	{
		AudioManager.Play("devil_slam");
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x000087C0 File Offset: 0x000069C0
	public void DevilKick()
	{
		AudioManager.Play("devil_kick");
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x000087CC File Offset: 0x000069CC
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x040006C4 RID: 1732
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	public Image arrow;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	public GameObject book;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	public GameObject bookLocalized;

	// Token: 0x040006C8 RID: 1736
	public float arrowTransparency;

	// Token: 0x040006C9 RID: 1737
	public bool arrowVisible;
}
