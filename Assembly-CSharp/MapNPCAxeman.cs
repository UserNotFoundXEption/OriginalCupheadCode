using System;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class MapNPCAxeman : MonoBehaviour
{
	// Token: 0x060030ED RID: 12525 RVA: 0x00028ACD File Offset: 0x00026CCD
	public void Start()
	{
		if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			base.transform.position = this.positionAfterWorld1;
		}
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x00028B04 File Offset: 0x00026D04
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.positionAfterWorld1, 0.5f);
	}

	// Token: 0x04002866 RID: 10342
	public Vector3 positionAfterWorld1;

	// Token: 0x04002867 RID: 10343
	[SerializeField]
	public int dialoguerVariableID = 3;
}
