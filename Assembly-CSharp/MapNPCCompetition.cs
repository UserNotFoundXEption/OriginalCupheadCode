using System;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class MapNPCCompetition : MonoBehaviour
{
	// Token: 0x06003127 RID: 12583 RVA: 0x000E8C6C File Offset: 0x000E6E6C
	public void Start()
	{
		int[] curseCharmPuzzleOrder = PlayerData.Data.curseCharmPuzzleOrder;
		foreach (int num in PlayerData.Data.curseCharmPuzzleOrder)
		{
		}
		for (int j = 0; j < curseCharmPuzzleOrder.Length; j++)
		{
			Dialoguer.SetGlobalFloat(this.dialogueVarIndices[j], (float)curseCharmPuzzleOrder[j]);
		}
	}

	// Token: 0x0400288F RID: 10383
	public int[] dialogueVarIndices = new int[]
	{
		26,
		27,
		28
	};
}
