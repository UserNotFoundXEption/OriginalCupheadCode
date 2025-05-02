using System;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class LevelSelectScene : AbstractMonoBehaviour
{
	// Token: 0x0600335B RID: 13147 RVA: 0x0002A87D File Offset: 0x00028A7D
	public override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		CupheadEventSystem.Init();
		this.UpdatePlayers();
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x0002A896 File Offset: 0x00028A96
	public void Update()
	{
		if (Input.GetKeyDown(27))
		{
			Application.Quit();
		}
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x0002A8A9 File Offset: 0x00028AA9
	public void OnOnePlayerButtonPressed()
	{
		PlayerManager.Multiplayer = false;
		this.UpdatePlayers();
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x0002A8B7 File Offset: 0x00028AB7
	public void OnTwoPlayersButtonPressed()
	{
		PlayerManager.Multiplayer = true;
		this.UpdatePlayers();
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x000F3FD4 File Offset: 0x000F21D4
	public void UpdatePlayers()
	{
		float alpha = 0.3f;
		this.onePlayerButton.alpha = alpha;
		this.twoPlayersButton.alpha = alpha;
		if (PlayerManager.Multiplayer)
		{
			this.twoPlayersButton.alpha = 1f;
		}
		else
		{
			this.onePlayerButton.alpha = 1f;
		}
	}

	// Token: 0x04002A6A RID: 10858
	[SerializeField]
	public CanvasGroup onePlayerButton;

	// Token: 0x04002A6B RID: 10859
	[SerializeField]
	public CanvasGroup twoPlayersButton;
}
