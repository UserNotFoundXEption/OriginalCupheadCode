using System;

// Token: 0x020004BD RID: 1213
public class LevelEquipUI : AbstractEquipUI
{
	// Token: 0x0600326A RID: 12906 RVA: 0x00029CE9 File Offset: 0x00027EE9
	public new void Start()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x00029CF7 File Offset: 0x00027EF7
	public void Activate()
	{
		base.StartCoroutine(base.pause_cr());
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x00029D06 File Offset: 0x00027F06
	public override void Unpause()
	{
		LevelGameOverGUI.Current.ReactivateOnChangeEquipmentClosed();
		base.StartCoroutine(base.unpause_cr());
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x00029D1F File Offset: 0x00027F1F
	public override void OnPauseSound()
	{
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x00029D21 File Offset: 0x00027F21
	public override void OnUnpauseSound()
	{
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x00029D23 File Offset: 0x00027F23
	public override void PauseGameplay()
	{
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x00029D25 File Offset: 0x00027F25
	public override void UnpauseGameplay()
	{
	}
}
