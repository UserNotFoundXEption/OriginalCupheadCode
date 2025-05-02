using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class MapNPCBarbershop : AbstractMonoBehaviour
{
	// Token: 0x060030F0 RID: 12528 RVA: 0x00028B30 File Offset: 0x00026D30
	public void Start()
	{
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) > 0f)
		{
			this.NowFour();
			this.CleanUp();
		}
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x00028B53 File Offset: 0x00026D53
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawWireSphere(this.fourPosition + base.transform.parent.position, 1f);
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x00028B80 File Offset: 0x00026D80
	public void NowFour()
	{
		base.animator.runtimeAnimatorController = this.fourAnimatorController;
		base.transform.localPosition = this.fourPosition;
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x00028BA4 File Offset: 0x00026DA4
	public void CleanUp()
	{
		if (this.mapDialogueInteraction)
		{
			Object.Destroy(this.mapDialogueInteraction);
		}
		if (this.mapNPCDistanceAnimator)
		{
			Object.Destroy(this.mapNPCDistanceAnimator);
		}
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x00028BDC File Offset: 0x00026DDC
	public void SongLooped()
	{
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x00028BDE File Offset: 0x00026DDE
	public void Show()
	{
		base.animator.SetTrigger("show");
	}

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	public RuntimeAnimatorController fourAnimatorController;

	// Token: 0x04002869 RID: 10345
	[SerializeField]
	public Vector3 fourPosition;

	// Token: 0x0400286A RID: 10346
	[SerializeField]
	public MapNPCLostBarbershop mapNPCDistanceAnimator;

	// Token: 0x0400286B RID: 10347
	public MapDialogueInteraction mapDialogueInteraction;

	// Token: 0x0400286C RID: 10348
	[SerializeField]
	public int dialoguerVariableID = 10;
}
