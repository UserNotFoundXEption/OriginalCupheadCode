using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B5 RID: 181
public class DevilCutscene : Cutscene
{
	// Token: 0x06000866 RID: 2150 RVA: 0x00008100 File Offset: 0x00006300
	public override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00075EB4 File Offset: 0x000740B4
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.optionSelector.Show();
		yield break;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00008131 File Offset: 0x00006331
	public void RefuseDevil()
	{
		this.ConfirmSFX();
		base.StartCoroutine(this.refuse_devil_cr());
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00008146 File Offset: 0x00006346
	public void JoinDevil()
	{
		this.ConfirmSFX();
		base.StartCoroutine(this.join_devil_cr());
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00075ED0 File Offset: 0x000740D0
	public IEnumerator join_devil_cr()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.PlayBGMPlaylistManually(false);
		this.evilVersionsBaseGame.SetActive(true);
		this.evilVersionsDLC.SetActive(false);
		if (DLCManager.DLCEnabled() && (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice || (PlayerManager.Multiplayer && PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)))
		{
			this.evilVersionsBaseGame.SetActive(false);
			this.evilVersionsDLC.SetActive(true);
		}
		base.animator.SetTrigger("joinedDevil");
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		base.animator.SetTrigger("fadeOut");
		yield return base.animator.WaitForAnimationToEnd(this, "Fade_Out", 1, false, true);
		base.animator.SetTrigger("Continue");
		base.animator.SetTrigger("fadeIn");
		this.DevilEvilSFX();
		base.StartCoroutine(this.blink_cr());
		yield return CupheadTime.WaitForSeconds(this, 10f);
		this.KillSFX();
		CreditsScreen.goodEnding = false;
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
		yield break;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00075EEC File Offset: 0x000740EC
	public IEnumerator blink_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(3f, 5f));
			base.animator.SetTrigger("Blink");
		}
		yield break;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00075F08 File Offset: 0x00074108
	public IEnumerator refuse_devil_cr()
	{
		base.animator.SetTrigger("refusedDevil");
		this.DevilAngrySFX();
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		this.KillSFX();
		SceneLoader.LoadLevel(Levels.Devil, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00075F24 File Offset: 0x00074124
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
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0000815B File Offset: 0x0000635B
	public void ConfirmSFX()
	{
		AudioManager.Play("ui_confirm");
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00008167 File Offset: 0x00006367
	public void DevilEvilSFX()
	{
		AudioManager.PlayLoop("sfx_hell_fire");
		AudioManager.Play("devil_laugh");
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0000817D File Offset: 0x0000637D
	public void DevilAngrySFX()
	{
		AudioManager.PlayLoop("sfx_hell_fire");
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00008189 File Offset: 0x00006389
	public void KillSFX()
	{
		AudioManager.FadeSFXVolume("sfx_hell_fire", 0f, 4f);
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0000819F File Offset: 0x0000639F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x04000664 RID: 1636
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	public Image arrow;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	public DevilCutsceneOptionSelector optionSelector;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	public GameObject evilVersionsBaseGame;

	// Token: 0x04000668 RID: 1640
	[SerializeField]
	public GameObject evilVersionsDLC;

	// Token: 0x04000669 RID: 1641
	public float arrowTransparency;

	// Token: 0x0400066A RID: 1642
	public bool arrowVisible;
}
