using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class MapDLC : Map
{
	// Token: 0x060031A9 RID: 12713 RVA: 0x00029472 File Offset: 0x00027672
	public override void SelectMusic()
	{
		this.currentMusic = -2;
		this.CheckMusic(false);
		this.CheckIfBossesCompleted();
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000EAB60 File Offset: 0x000E8D60
	public override void CheckMusic(bool isRecheck)
	{
		int num = this.currentMusic;
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout2 = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo);
		if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
		{
			if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)))
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 3 : 5);
			}
			else
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 2 : 4);
			}
		}
		else if (PlayerData.Data.pianoAudioEnabled)
		{
			num = 1;
		}
		else
		{
			num = ((!MapDLC.haveVisited) ? -1 : 0);
			MapDLC.haveVisited = true;
		}
		if ((this.currentMusic == -1 && num == 0) || (this.currentMusic == 0 && num == -1))
		{
			return;
		}
		if (num != this.currentMusic)
		{
			this.currentMusic = num;
			if (this.currentMusic == -1)
			{
				AudioManager.PlayBGM();
			}
			else
			{
				AudioManager.StartBGMAlternate(this.currentMusic);
			}
		}
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00029489 File Offset: 0x00027689
	public override void OnPlayerJoined(PlayerId playerId)
	{
		base.OnPlayerJoined(playerId);
		this.CheckMusic(true);
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00029499 File Offset: 0x00027699
	public override void OnPlayerLeave(PlayerId playerId)
	{
		base.OnPlayerLeave(playerId);
		this.CheckMusic(true);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x000294A9 File Offset: 0x000276A9
	public void CheckIfBossesCompleted()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
		{
			this.bakerySoundLoop.gameObject.SetActive(false);
		}
		else
		{
			this.bakerySoundLoop.Play();
		}
	}

	// Token: 0x040028DC RID: 10460
	public static bool haveVisited;

	// Token: 0x040028DD RID: 10461
	[SerializeField]
	public AudioSource bakerySoundLoop;
}
