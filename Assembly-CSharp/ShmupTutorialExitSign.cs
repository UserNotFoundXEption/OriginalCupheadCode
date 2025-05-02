using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class ShmupTutorialExitSign : AbstractLevelInteractiveEntity
{
	// Token: 0x06002A5F RID: 10847 RVA: 0x00023AAA File Offset: 0x00021CAA
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x00023ACB File Offset: 0x00021CCB
	public override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000D3C78 File Offset: 0x000D1E78
	public IEnumerator go_cr()
	{
		this.activated = true;
		PlayerData.SaveCurrentFile();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0f);
		foreach (PlanePlayerController planePlayerController in Object.FindObjectsOfType<PlanePlayerController>())
		{
			planePlayerController.PauseAll();
		}
		foreach (PlaneSuperBomb planeSuperBomb in Object.FindObjectsOfType<PlaneSuperBomb>())
		{
			planeSuperBomb.Pause();
		}
		foreach (PlaneSuperChalice planeSuperChalice in Object.FindObjectsOfType<PlaneSuperChalice>())
		{
			planeSuperChalice.Pause();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x04002359 RID: 9049
	public bool activated;
}
