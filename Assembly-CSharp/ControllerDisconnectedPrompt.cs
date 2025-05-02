using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EA RID: 234
public class ControllerDisconnectedPrompt : InterruptingPrompt
{
	// Token: 0x06000B0A RID: 2826 RVA: 0x00009E7F File Offset: 0x0000807F
	public override void Awake()
	{
		base.Awake();
		ControllerDisconnectedPrompt.Instance = this;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00009E8D File Offset: 0x0000808D
	public void Show(PlayerId player)
	{
		this.currentPlayer = player;
		this.localizationHelper.currentID = Localization.Find((player != PlayerId.PlayerOne) ? "XboxPlayer2" : "XboxPlayer1").id;
		PlayerManager.OnDisconnectPromptDisplayed(player);
		base.Show();
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00009ECC File Offset: 0x000080CC
	public void Update()
	{
		if (base.Visible && !PlayerManager.IsControllerDisconnected(this.currentPlayer, true))
		{
			base.FrameDelayedCallback(new Action(base.Dismiss), 2);
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00009EFE File Offset: 0x000080FE
	public void OnDestroy()
	{
		ControllerDisconnectedPrompt.Instance = null;
	}

	// Token: 0x040008A2 RID: 2210
	public static ControllerDisconnectedPrompt Instance;

	// Token: 0x040008A3 RID: 2211
	public PlayerId currentPlayer;

	// Token: 0x040008A4 RID: 2212
	public bool allowedToShow;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	public Text playerText;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	public LocalizationHelper localizationHelper;
}
