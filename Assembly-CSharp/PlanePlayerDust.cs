using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000562 RID: 1378
public class PlanePlayerDust : AbstractMonoBehaviour
{
	// Token: 0x060039B8 RID: 14776 RVA: 0x0002F05B File Offset: 0x0002D25B
	public void Initialize(AbstractPlayerController playerController, float smallY, float bigY)
	{
		this.playerController = (PlanePlayerController)playerController;
		this.smallY = smallY;
		this.bigY = bigY;
		if (playerController != null)
		{
			base.StartCoroutine(this.setupSorting_cr());
		}
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x0010CFBC File Offset: 0x0010B1BC
	public IEnumerator setupSorting_cr()
	{
		while (this.playerController.animationController.spriteRenderer == null)
		{
			yield return null;
		}
		int playerOrder = this.playerController.animationController.spriteRenderer.sortingOrder;
		this.shadowRenderer.sortingOrder += playerOrder;
		this.backRenderer.sortingOrder += playerOrder;
		this.frontRenderer.sortingOrder += playerOrder;
		yield break;
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x0010CFD8 File Offset: 0x0010B1D8
	public void Update()
	{
		if (this.playerController == null)
		{
			return;
		}
		if (this.playerController.IsDead)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 0);
			base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, false);
			this.shadowRenderer.enabled = false;
			return;
		}
		float bottom = this.playerController.bottom;
		if (bottom < this.bigY)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 2);
			base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, true);
		}
		else if (bottom < this.smallY)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 1);
			this.setManualShadow(bottom);
		}
		else
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 0);
			this.setManualShadow(bottom);
		}
		base.transform.position = new Vector3(this.playerController.center.x, this.bigY) + PlanePlayerDust.PositionOffset;
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x0010D0EC File Offset: 0x0010B2EC
	public void setManualShadow(float bottom)
	{
		base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, false);
		if (bottom >= this.smallY)
		{
			this.shadowRenderer.enabled = false;
		}
		else
		{
			this.shadowRenderer.enabled = true;
			float num = MathUtilities.LerpMapping(bottom, this.smallY, this.bigY, 0f, (float)PlanePlayerDust.ManualShadowSpriteCount, true);
			base.animator.Play(PlanePlayerDust.ManualState, 2, num / (float)PlanePlayerDust.ManualShadowSpriteCount);
		}
	}

	// Token: 0x04002E54 RID: 11860
	public static readonly int SizeParameter = Animator.StringToHash("Size");

	// Token: 0x04002E55 RID: 11861
	public static readonly int ShadowLoopParameter = Animator.StringToHash("ShadowLoop");

	// Token: 0x04002E56 RID: 11862
	public static readonly int ManualState = Animator.StringToHash("Manual");

	// Token: 0x04002E57 RID: 11863
	public static readonly Vector3 PositionOffset = new Vector3(-70f, -20f);

	// Token: 0x04002E58 RID: 11864
	public static readonly int ManualShadowSpriteCount = 7;

	// Token: 0x04002E59 RID: 11865
	[SerializeField]
	public SpriteRenderer shadowRenderer;

	// Token: 0x04002E5A RID: 11866
	[SerializeField]
	public SpriteRenderer backRenderer;

	// Token: 0x04002E5B RID: 11867
	[SerializeField]
	public SpriteRenderer frontRenderer;

	// Token: 0x04002E5C RID: 11868
	public float smallY;

	// Token: 0x04002E5D RID: 11869
	public float bigY;

	// Token: 0x04002E5E RID: 11870
	public PlanePlayerController playerController;
}
