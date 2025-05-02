using System;
using UnityEngine;

// Token: 0x02000495 RID: 1173
public class MapNPCFishgirl : MonoBehaviour
{
	// Token: 0x0600312B RID: 12587 RVA: 0x00028EEB File Offset: 0x000270EB
	public void Start()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(new Levels[]
		{
			Levels.Mausoleum
		}, Level.Mode.Easy))
		{
			Dialoguer.SetGlobalFloat(12, 1f);
		}
	}
}
