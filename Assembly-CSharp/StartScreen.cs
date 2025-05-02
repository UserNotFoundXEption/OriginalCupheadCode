using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004DE RID: 1246
public class StartScreen : AbstractMonoBehaviour
{
	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06003399 RID: 13209 RVA: 0x0002AB28 File Offset: 0x00028D28
	// (set) Token: 0x0600339A RID: 13210 RVA: 0x0002AB30 File Offset: 0x00028D30
	public StartScreen.State state { get; set; }

	// Token: 0x0600339B RID: 13211 RVA: 0x0002AB39 File Offset: 0x00028D39
	public override void Awake()
	{
		base.Awake();
		Debug.Log("Build version " + Application.version);
		Cuphead.Init(false);
		CupheadTime.Reset();
		PauseManager.Reset();
		this.shouldLoadSlotSelect = false;
		PlayerData.inGame = false;
		PlayerManager.ResetPlayers();
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x000F58A8 File Offset: 0x000F3AA8
	public void Start()
	{
		if (PlatformHelper.PreloadSettingsData)
		{
			SettingsData.ApplySettingsOnStartup();
		}
		if (AudioNoiseHandler.Instance != null)
		{
			AudioNoiseHandler.Instance.OpticalSound();
		}
		if (StartScreenAudio.Instance == null)
		{
			StartScreenAudio startScreenAudio = Object.Instantiate(Resources.Load("Audio/TitleScreenAudio")) as StartScreenAudio;
			startScreenAudio.name = "StartScreenAudio";
		}
		SettingsData.ApplySettingsOnStartup();
		base.FrameDelayedCallback(new Action(this.StartFrontendSnapshot), 1);
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x000F5938 File Offset: 0x000F3B38
	public void Update()
	{
		StartScreen.State state = this.state;
		if (state != StartScreen.State.MDHR_Splash)
		{
			if (state == StartScreen.State.Title)
			{
				this.UpdateTitleScreen();
			}
		}
		else
		{
			this.UpdateSplashMDHR();
		}
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0002AB77 File Offset: 0x00028D77
	public void UpdateSplashMDHR()
	{
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0002AB79 File Offset: 0x00028D79
	public void UpdateTitleScreen()
	{
		if (this.shouldLoadSlotSelect)
		{
			AudioManager.Play("ui_playerconfirm");
			AudioManager.Play("level_select");
			SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x0002ABAC File Offset: 0x00028DAC
	public void onPlayerJoined(PlayerId playerId)
	{
		this.shouldLoadSlotSelect = true;
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x000F5978 File Offset: 0x000F3B78
	public IEnumerator loop_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("mdhr_logo_sting");
		yield return base.StartCoroutine(this.tweenRenderer_cr(this.fader, 1f));
		this.mdhrSplash.Play("Logo");
		yield return this.mdhrSplash.WaitForAnimationToEnd(this, "Logo", false, true);
		AudioManager.SnapshotReset(Scenes.scene_title.ToString(), 0.3f);
		if (!CreditsScreen.goodEnding)
		{
			AudioManager.PlayBGM();
		}
		else if (DLCManager.DLCEnabled() && !this.forceOriginalTitleScreen())
		{
			AudioManager.StartBGMAlternate(0);
			this.titleAnimation.SetActive(false);
			this.titleAnimationDLC.SetActive(true);
		}
		else
		{
			AudioManager.PlayBGMPlaylistManually(true);
		}
		StartScreen.initialLoadData = null;
		CreditsScreen.goodEnding = true;
		SettingsData.Data.hasBootedUpGame = true;
		yield return base.StartCoroutine(this.tweenRenderer_cr(this.mdhrSplash.GetComponent<SpriteRenderer>(), 0.4f));
		this.state = StartScreen.State.Title;
		PlayerManager.OnPlayerJoinedEvent += this.onPlayerJoined;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerOne, true, false);
		yield break;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x0002ABB5 File Offset: 0x00028DB5
	public void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.onPlayerJoined;
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x000F5994 File Offset: 0x000F3B94
	public IEnumerator tweenRenderer_cr(SpriteRenderer renderer, float time)
	{
		float t = 0f;
		Color c = renderer.color;
		c.a = 1f;
		yield return null;
		while (t < time)
		{
			c.a = 1f - t / time;
			renderer.color = c;
			t += Time.deltaTime;
			yield return null;
		}
		c.a = 0f;
		renderer.color = c;
		yield return null;
		yield break;
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x0002ABC8 File Offset: 0x00028DC8
	public bool forceOriginalTitleScreen()
	{
		if (StartScreen.initialLoadData != null)
		{
			return StartScreen.initialLoadData.forceOriginalTitleScreen;
		}
		return SettingsData.Data.forceOriginalTitleScreen;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x000F59B8 File Offset: 0x000F3BB8
	public virtual void StartFrontendSnapshot()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.FrontEnd.ToString(), 0.15f);
	}

	// Token: 0x04002ADB RID: 10971
	public static StartScreen.InitialLoadData initialLoadData;

	// Token: 0x04002ADD RID: 10973
	public AudioClip[] SelectSound;

	// Token: 0x04002ADE RID: 10974
	[SerializeField]
	public Animator mdhrSplash;

	// Token: 0x04002ADF RID: 10975
	[SerializeField]
	public SpriteRenderer fader;

	// Token: 0x04002AE0 RID: 10976
	[SerializeField]
	public GameObject titleAnimation;

	// Token: 0x04002AE1 RID: 10977
	[SerializeField]
	public GameObject titleAnimationDLC;

	// Token: 0x04002AE2 RID: 10978
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04002AE3 RID: 10979
	public bool shouldLoadSlotSelect;

	// Token: 0x04002AE4 RID: 10980
	public const string PATH = "Audio/TitleScreenAudio";

	// Token: 0x02001145 RID: 4421
	public class InitialLoadData
	{
		// Token: 0x04007999 RID: 31129
		public bool forceOriginalTitleScreen;
	}

	// Token: 0x02001146 RID: 4422
	public enum State
	{
		// Token: 0x0400799B RID: 31131
		Animating,
		// Token: 0x0400799C RID: 31132
		MDHR_Splash,
		// Token: 0x0400799D RID: 31133
		Title
	}
}
