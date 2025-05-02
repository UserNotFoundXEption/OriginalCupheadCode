using System;
using UnityEngine;

// Token: 0x020003BB RID: 955
public class TrainLevelSkeletonHand : AbstractTrainLevelSkeletonPart
{
	// Token: 0x06002A4D RID: 10829 RVA: 0x000239DD File Offset: 0x00021BDD
	public void PlaySlapFX()
	{
		this.effectPrefab.Create(this.effectRoot.position, base.transform.localScale);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x00023A16 File Offset: 0x00021C16
	public void Slap()
	{
		base.animator.Play("Slap");
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x00023A28 File Offset: 0x00021C28
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x04002347 RID: 9031
	[SerializeField]
	public Transform effectRoot;

	// Token: 0x04002348 RID: 9032
	[SerializeField]
	public Effect effectPrefab;
}
