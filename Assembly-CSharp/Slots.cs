using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200029C RID: 668
[Serializable]
public class Slots
{
	// Token: 0x06001E2A RID: 7722 RVA: 0x00019757 File Offset: 0x00017957
	public void Init(MonoBehaviour parent)
	{
		this.parent = parent;
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00019760 File Offset: 0x00017960
	public void Spin()
	{
		this.parent.StartCoroutine(this.spin_cr());
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x000B27AC File Offset: 0x000B09AC
	public IEnumerator spin_cr()
	{
		this.left.StartSpin();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.StartSpin();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.StartSpin();
		yield break;
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x00019774 File Offset: 0x00017974
	public void Stop(Slots.Mode mode)
	{
		this.parent.StartCoroutine(this.stop_cr(mode));
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000B27C8 File Offset: 0x000B09C8
	public IEnumerator stop_cr(Slots.Mode mode)
	{
		this.left.StopSpin(mode);
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.StopSpin(mode);
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.StopSpin(mode);
		yield break;
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x00019789 File Offset: 0x00017989
	public void StartFlash()
	{
		this.parent.StartCoroutine(this.startFlash_cr());
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000B27EC File Offset: 0x000B09EC
	public IEnumerator startFlash_cr()
	{
		this.left.Flash();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.mid.Flash();
		yield return CupheadTime.WaitForSeconds(this.parent, 0.2f);
		this.right.Flash();
		yield break;
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x0001979D File Offset: 0x0001799D
	public void OnDestroy()
	{
		this.left = null;
		this.mid = null;
		this.right = null;
	}

	// Token: 0x040018B3 RID: 6323
	public const float DELAY = 0.2f;

	// Token: 0x040018B4 RID: 6324
	[SerializeField]
	public FrogsLevelMorphedSlot left;

	// Token: 0x040018B5 RID: 6325
	[SerializeField]
	public FrogsLevelMorphedSlot mid;

	// Token: 0x040018B6 RID: 6326
	[SerializeField]
	public FrogsLevelMorphedSlot right;

	// Token: 0x040018B7 RID: 6327
	public MonoBehaviour parent;

	// Token: 0x02000D6D RID: 3437
	public enum Mode
	{
		// Token: 0x0400614F RID: 24911
		Snake,
		// Token: 0x04006150 RID: 24912
		Tiger,
		// Token: 0x04006151 RID: 24913
		Bison,
		// Token: 0x04006152 RID: 24914
		Oni
	}
}
