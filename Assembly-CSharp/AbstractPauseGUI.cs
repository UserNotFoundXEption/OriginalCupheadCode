using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E4 RID: 228
[RequireComponent(typeof(CanvasGroup))]
public abstract class AbstractPauseGUI : AbstractMonoBehaviour
{
	// Token: 0x06000AAE RID: 2734 RVA: 0x00009A4B File Offset: 0x00007C4B
	public AbstractPauseGUI()
	{
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00009A53 File Offset: 0x00007C53
	// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x00009A5B File Offset: 0x00007C5B
	public AbstractPauseGUI.State state { get; set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00009A64 File Offset: 0x00007C64
	public virtual CupheadButton LevelInputButton
	{
		get
		{
			return CupheadButton.Pause;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00009A67 File Offset: 0x00007C67
	public virtual CupheadButton UIInputButton
	{
		get
		{
			return CupheadButton.EquipMenu;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00009A6B File Offset: 0x00007C6B
	public virtual AbstractPauseGUI.InputActionSet CheckedActionSet
	{
		get
		{
			return AbstractPauseGUI.InputActionSet.LevelInput;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000AB4 RID: 2740
	public abstract bool CanPause { get; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00009A6E File Offset: 0x00007C6E
	public virtual bool CanUnpause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00009A71 File Offset: 0x00007C71
	public virtual bool RespondToDeadPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00009A74 File Offset: 0x00007C74
	public override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.HideImmediate();
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x00009A8E File Offset: 0x00007C8E
	public virtual void Update()
	{
		this.UpdateInput();
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x0007C14C File Offset: 0x0007A34C
	public void UpdateInput()
	{
		if (!this.CanPause)
		{
			return;
		}
		bool flag = (this.CheckedActionSet != AbstractPauseGUI.InputActionSet.LevelInput) ? this.GetButtonDown(this.UIInputButton) : this.GetButtonDown(this.LevelInputButton);
		if (flag)
		{
			base.StartCoroutine(this.ShowPauseMenu());
		}
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0007C1A4 File Offset: 0x0007A3A4
	public IEnumerator ShowPauseMenu()
	{
		if (MapEventNotification.Current != null)
		{
			while (MapEventNotification.Current.showing)
			{
				yield return null;
			}
		}
		if (this.state == AbstractPauseGUI.State.Unpaused && PauseManager.state == PauseManager.State.Unpaused)
		{
			this.Pause();
		}
		else if (this.state == AbstractPauseGUI.State.Paused && this.CanUnpause)
		{
			this.Unpause();
		}
		yield break;
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00009A96 File Offset: 0x00007C96
	public virtual void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements, RestartTowerConfirmGUI restartTowerConfirmGUI)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00009AA4 File Offset: 0x00007CA4
	public virtual void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00009AB2 File Offset: 0x00007CB2
	public virtual void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00009AC0 File Offset: 0x00007CC0
	public virtual void Pause()
	{
		if (this.state == AbstractPauseGUI.State.Unpaused && PauseManager.state == PauseManager.State.Unpaused)
		{
			base.StartCoroutine(this.pause_cr());
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00009AE4 File Offset: 0x00007CE4
	public virtual void Unpause()
	{
		if (this.state == AbstractPauseGUI.State.Paused)
		{
			base.StartCoroutine(this.unpause_cr());
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00009AFF File Offset: 0x00007CFF
	public virtual void OnPause()
	{
		this.OnPauseSound();
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00009B16 File Offset: 0x00007D16
	public virtual void OnPauseComplete()
	{
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00009B18 File Offset: 0x00007D18
	public virtual void OnUnpause()
	{
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
		this.OnUnpauseSound();
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00009B2F File Offset: 0x00007D2F
	public virtual void OnUnpauseComplete()
	{
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0007C1C0 File Offset: 0x0007A3C0
	public virtual void OnPauseSound()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.15f);
		AudioManager.PauseAllSFX();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0007C1EC File Offset: 0x0007A3EC
	public virtual void OnUnpauseSound()
	{
		AudioManager.SnapshotReset((!this.isWorldMap) ? Level.Current.CurrentScene.ToString() : PlayerData.Data.CurrentMap.ToString(), 0.1f);
		AudioManager.UnpauseAllSFX();
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00009B31 File Offset: 0x00007D31
	public virtual void HideImmediate()
	{
		this.canvasGroup.alpha = 0f;
		this.SetInteractable(false);
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00009B4A File Offset: 0x00007D4A
	public virtual void ShowImmediate()
	{
		this.canvasGroup.alpha = 1f;
		this.SetInteractable(true);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00009B63 File Offset: 0x00007D63
	public void SetInteractable(bool interactable)
	{
		this.canvasGroup.interactable = interactable;
		this.canvasGroup.blocksRaycasts = interactable;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0007C248 File Offset: 0x0007A448
	public IEnumerator pause_cr()
	{
		Vibrator.StopVibrating(PlayerId.PlayerOne);
		Vibrator.StopVibrating(PlayerId.PlayerTwo);
		this.OnPause();
		this.PauseGameplay();
		this.SetInteractable(true);
		yield return base.StartCoroutine(this.animate_cr(this.InTime, new AbstractPauseGUI.AnimationDelegate(this.InAnimation), 0f, 1f));
		this.state = AbstractPauseGUI.State.Paused;
		this.OnPauseComplete();
		yield break;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0007C264 File Offset: 0x0007A464
	public IEnumerator unpause_cr()
	{
		this.OnUnpause();
		this.SetInteractable(true);
		this.UnpauseGameplay();
		yield return base.StartCoroutine(this.animate_cr(this.OutTime, new AbstractPauseGUI.AnimationDelegate(this.OutAnimation), 1f, 0f));
		this.state = AbstractPauseGUI.State.Unpaused;
		this.SetInteractable(false);
		this.OnUnpauseComplete();
		yield break;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00009B7D File Offset: 0x00007D7D
	public virtual void PauseGameplay()
	{
		PauseManager.Pause();
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00009B84 File Offset: 0x00007D84
	public virtual void UnpauseGameplay()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x0007C280 File Offset: 0x0007A480
	public IEnumerator animate_cr(float time, AbstractPauseGUI.AnimationDelegate anim, float start, float end)
	{
		anim(0f);
		this.state = AbstractPauseGUI.State.Animating;
		this.canvasGroup.alpha = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.canvasGroup.alpha = Mathf.Lerp(start, end, val);
			anim(val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = end;
		anim(1f);
		yield break;
	}

	// Token: 0x06000ACE RID: 2766
	public abstract void InAnimation(float i);

	// Token: 0x06000ACF RID: 2767
	public abstract void OutAnimation(float i);

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00009B8B File Offset: 0x00007D8B
	public virtual float InTime
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00009B92 File Offset: 0x00007D92
	public virtual float OutTime
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00009B99 File Offset: 0x00007D99
	public bool GetButtonDown(CupheadButton button)
	{
		return (!(AbstractEquipUI.Current != null) || AbstractEquipUI.Current.CurrentState != AbstractEquipUI.ActiveState.Active || button != CupheadButton.EquipMenu) && this.input.GetButtonDown(button);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00009BD9 File Offset: 0x00007DD9
	public void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00009BE5 File Offset: 0x00007DE5
	public void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00009BF1 File Offset: 0x00007DF1
	public bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x04000859 RID: 2137
	[SerializeField]
	public bool isWorldMap;

	// Token: 0x0400085B RID: 2139
	public CanvasGroup canvasGroup;

	// Token: 0x0400085C RID: 2140
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x02000956 RID: 2390
	public enum State
	{
		// Token: 0x0400462E RID: 17966
		Unpaused,
		// Token: 0x0400462F RID: 17967
		Paused,
		// Token: 0x04004630 RID: 17968
		Animating
	}

	// Token: 0x02000957 RID: 2391
	public enum InputActionSet
	{
		// Token: 0x04004632 RID: 17970
		LevelInput,
		// Token: 0x04004633 RID: 17971
		UIInput
	}

	// Token: 0x02000958 RID: 2392
	// (Invoke) Token: 0x060054B7 RID: 21687
	public delegate void AnimationDelegate(float i);
}
