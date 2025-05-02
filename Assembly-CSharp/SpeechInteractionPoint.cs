using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class SpeechInteractionPoint : AbstractLevelInteractiveEntity
{
	// Token: 0x060009EF RID: 2543 RVA: 0x000091CB File Offset: 0x000073CB
	public override void Awake()
	{
		base.Awake();
		this.dialogueProperties.text = "Talk";
		this.isDisabledP1 = false;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0007A55C File Offset: 0x0007875C
	public override void Check()
	{
		base.Check();
		if (base.PlayerWithinDistance(PlayerId.PlayerOne))
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerMotor>().DisableJump();
			this.isDisabledP1 = true;
		}
		else if (base.PlayerWithinDistance(PlayerId.PlayerTwo))
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerMotor>().DisableJump();
			this.isDisabledP2 = true;
		}
		else if (this.isDisabledP1)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerMotor>().EnableJump();
			this.isDisabledP1 = false;
		}
		else if (this.isDisabledP2)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerMotor>().EnableJump();
			this.isDisabledP2 = false;
		}
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0007A608 File Offset: 0x00078808
	public override void Activate()
	{
		base.Activate();
		if (!this.activated)
		{
			if (base.PlayerWithinDistance(PlayerId.PlayerOne))
			{
				this.Show(PlayerId.PlayerOne);
			}
			if (base.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				this.Show(PlayerId.PlayerTwo);
			}
			this.activated = true;
		}
	}

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	public string[] allDialogue;

	// Token: 0x04000796 RID: 1942
	public bool activated;

	// Token: 0x04000797 RID: 1943
	public bool isDisabledP1;

	// Token: 0x04000798 RID: 1944
	public bool isDisabledP2;
}
