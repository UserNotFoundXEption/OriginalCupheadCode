using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class DiceGateLevelKingDice : MonoBehaviour
{
	// Token: 0x0600156D RID: 5485 RVA: 0x0001236A File Offset: 0x0001056A
	public void SetDisappearBool()
	{
		PlayerData.Data.CurrentMapData.hasKingDiceDisappeared = true;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x00012381 File Offset: 0x00010581
	public void SoundKingDiceExitAnim()
	{
		AudioManager.Play("dicegate_kingdice_exit");
	}
}
