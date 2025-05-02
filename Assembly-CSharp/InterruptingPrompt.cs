using System;

// Token: 0x020000EE RID: 238
public class InterruptingPrompt : AbstractMonoBehaviour
{
	// Token: 0x06000B30 RID: 2864 RVA: 0x0000A0B1 File Offset: 0x000082B1
	public static void SetCanInterrupt(bool canInterrupt)
	{
		if (ControllerDisconnectedPrompt.Instance != null)
		{
			ControllerDisconnectedPrompt.Instance.allowedToShow = canInterrupt;
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0000A0CE File Offset: 0x000082CE
	public static bool IsInterrupting()
	{
		return ControllerDisconnectedPrompt.Instance != null && ControllerDisconnectedPrompt.Instance.Visible;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0000A0ED File Offset: 0x000082ED
	public static bool CanInterrupt()
	{
		return ControllerDisconnectedPrompt.Instance != null && ControllerDisconnectedPrompt.Instance.allowedToShow;
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0000A10B File Offset: 0x0000830B
	public bool Visible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0000A118 File Offset: 0x00008318
	public override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0000A12C File Offset: 0x0000832C
	public void Show()
	{
		base.gameObject.SetActive(true);
		this.wasPausedBeforeInterrupt = (PauseManager.state == PauseManager.State.Paused);
		if (!this.wasPausedBeforeInterrupt)
		{
			PauseManager.Pause();
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0000A158 File Offset: 0x00008358
	public void Dismiss()
	{
		base.gameObject.SetActive(false);
		if (!this.wasPausedBeforeInterrupt)
		{
			PauseManager.Unpause();
		}
	}

	// Token: 0x040008DF RID: 2271
	public bool wasPausedBeforeInterrupt;
}
