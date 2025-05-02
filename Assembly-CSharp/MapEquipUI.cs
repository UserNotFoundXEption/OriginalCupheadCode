using System;

// Token: 0x020004BE RID: 1214
public class MapEquipUI : AbstractEquipUI
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06003272 RID: 12914 RVA: 0x000ECBD8 File Offset: 0x000EADD8
	public override bool CanPause
	{
		get
		{
			return Map.Current.CurrentState == Map.State.Ready && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x00029D2F File Offset: 0x00027F2F
	public override void OnPause()
	{
		base.OnPause();
		CupheadMapCamera.Current.StartBlur();
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x00029D41 File Offset: 0x00027F41
	public override void OnUnpause()
	{
		base.OnUnpause();
		CupheadMapCamera.Current.EndBlur();
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000ECC50 File Offset: 0x000EAE50
	public override void OnPauseAudio()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.EquipMenu.ToString(), 0.15f);
		AudioManager.PauseAllSFX();
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x00029D53 File Offset: 0x00027F53
	public override void OnUnpauseAudio()
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 0.1f);
	}
}
