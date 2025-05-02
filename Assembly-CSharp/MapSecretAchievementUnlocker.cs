using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class MapSecretAchievementUnlocker : AbstractMonoBehaviour
{
	// Token: 0x06003171 RID: 12657 RVA: 0x000E9BBC File Offset: 0x000E7DBC
	public void OnTriggerEnter2D(Collider2D collider)
	{
		MapPlayerController component = collider.GetComponent<MapPlayerController>();
		OnlineManager.Instance.Interface.UnlockAchievement(component.id, "FoundSecretPassage");
		if (this.updateDialogue)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x040028B9 RID: 10425
	[SerializeField]
	public bool updateDialogue = true;

	// Token: 0x040028BA RID: 10426
	[SerializeField]
	public int dialoguerVariableID = 7;
}
