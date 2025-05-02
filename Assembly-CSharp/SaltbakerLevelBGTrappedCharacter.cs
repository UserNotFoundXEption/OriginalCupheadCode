using System;
using UnityEngine;

// Token: 0x0200036D RID: 877
public class SaltbakerLevelBGTrappedCharacter : MonoBehaviour
{
	// Token: 0x060026BC RID: 9916 RVA: 0x000C9AD8 File Offset: 0x000C7CD8
	public void Setup()
	{
		if (PlayerManager.Multiplayer)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice || PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
				{
					this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Cuphead : SaltbakerLevelBGTrappedCharacter.Character.Mugman);
				}
				else
				{
					this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Mugman : SaltbakerLevelBGTrappedCharacter.Character.Cuphead);
				}
			}
			else
			{
				this.charID = SaltbakerLevelBGTrappedCharacter.Character.Chalice;
			}
		}
		else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
		{
			this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Cuphead : SaltbakerLevelBGTrappedCharacter.Character.Mugman);
		}
		else
		{
			this.charID = ((PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm != Charm.charm_chalice) ? SaltbakerLevelBGTrappedCharacter.Character.Chalice : ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Mugman : SaltbakerLevelBGTrappedCharacter.Character.Cuphead));
		}
		for (int i = 0; i < 3; i++)
		{
			this.characters[i].SetActive(i == (int)this.charID);
		}
	}

	// Token: 0x04001FF4 RID: 8180
	[SerializeField]
	public GameObject[] characters;

	// Token: 0x04001FF5 RID: 8181
	public SaltbakerLevelBGTrappedCharacter.Character charID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x04001FF6 RID: 8182
	public SaltbakerLevelBGTrappedCharacter.Character pOneID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x04001FF7 RID: 8183
	public SaltbakerLevelBGTrappedCharacter.Character pTwoID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x02000F2D RID: 3885
	public enum Character
	{
		// Token: 0x04006D2C RID: 27948
		None = -1,
		// Token: 0x04006D2D RID: 27949
		Cuphead,
		// Token: 0x04006D2E RID: 27950
		Mugman,
		// Token: 0x04006D2F RID: 27951
		Chalice
	}
}
