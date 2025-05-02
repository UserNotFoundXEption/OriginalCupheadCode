using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class MapShmupTutorialBridgeActivator : MonoBehaviour
{
	// Token: 0x06003177 RID: 12663 RVA: 0x000E9D24 File Offset: 0x000E7F24
	public void Start()
	{
		if (!PlayerData.Data.IsFlyingTutorialCompleted && Level.PreviousLevel == Levels.ShmupTutorial)
		{
			PlayerData.Data.IsFlyingTutorialCompleted = true;
			this.blueprintObstacle.OnConditionNotMet();
			base.StartCoroutine(this.DoTransition());
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
		else if (!PlayerData.Data.IsFlyingTutorialCompleted)
		{
			this.blueprintObstacle.OnConditionNotMet();
		}
		else
		{
			this.blueprintObstacle.OnConditionAlreadyMet();
		}
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000E9DB8 File Offset: 0x000E7FB8
	public IEnumerator DoTransition()
	{
		yield return CupheadTime.WaitForSeconds(this, this.DoTransitionDelay);
		this.blueprintObstacle.DoTransition();
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		this.blueprintObstacle.OnConditionAlreadyMet();
		yield break;
	}

	// Token: 0x040028BD RID: 10429
	[SerializeField]
	public MapLevelDependentObstacle blueprintObstacle;

	// Token: 0x040028BE RID: 10430
	[SerializeField]
	public float DoTransitionDelay;

	// Token: 0x040028BF RID: 10431
	[SerializeField]
	public int dialoguerVariableID = 5;
}
