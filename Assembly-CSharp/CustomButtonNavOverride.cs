using System;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200063C RID: 1596
public class CustomButtonNavOverride : CustomButton
{
	// Token: 0x06004281 RID: 17025 RVA: 0x00035527 File Offset: 0x00033727
	public override Selectable FindSelectableOnUp()
	{
		if (!PlayerManager.Multiplayer)
		{
			return this.upOnSinglePlayer;
		}
		return (!this.upOnMultiPlayer) ? this.mapper.GetUnselectedPlayerButton() : this.upOnMultiPlayer;
	}

	// Token: 0x06004282 RID: 17026 RVA: 0x00136E54 File Offset: 0x00135054
	public override Selectable FindSelectableOnDown()
	{
		if (PlayerManager.Multiplayer)
		{
			return (!this.downOnMultiPlayer) ? this.mapper.GetUnselectedPlayerButton() : this.downOnMultiPlayer;
		}
		if (PlatformHelper.IsConsole)
		{
			return this;
		}
		return this.downOnSinglePlayer;
	}

	// Token: 0x04003448 RID: 13384
	[SerializeField]
	public Selectable upOnSinglePlayer;

	// Token: 0x04003449 RID: 13385
	[SerializeField]
	public Selectable downOnSinglePlayer;

	// Token: 0x0400344A RID: 13386
	[SerializeField]
	public Selectable upOnMultiPlayer;

	// Token: 0x0400344B RID: 13387
	[SerializeField]
	public Selectable downOnMultiPlayer;

	// Token: 0x0400344C RID: 13388
	[SerializeField]
	public ControlMapper mapper;
}
