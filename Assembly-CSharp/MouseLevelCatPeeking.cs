using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class MouseLevelCatPeeking : MonoBehaviour
{
	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0001ADC4 File Offset: 0x00018FC4
	public float Peek1Threshold
	{
		get
		{
			return this.peek1Threshold;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0001ADCC File Offset: 0x00018FCC
	public float Peek2Threshold
	{
		get
		{
			return this.peek2Threshold;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0001ADD4 File Offset: 0x00018FD4
	// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x0001ADDC File Offset: 0x00018FDC
	public bool IsPhase2
	{
		get
		{
			return this.isPhase2;
		}
		set
		{
			this.isPhase2 = value;
			this.catAnimator.SetBool("IsPhase2", value);
		}
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x0001ADF6 File Offset: 0x00018FF6
	public void StartPeeking()
	{
		this.peekRoutine = this.catPeeking_cr();
		base.StartCoroutine(this.peekRoutine);
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x0001AE11 File Offset: 0x00019011
	public void StopPeeking()
	{
		base.StopCoroutine(this.peekRoutine);
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000B699C File Offset: 0x000B4B9C
	public IEnumerator catPeeking_cr()
	{
		Transform catTransform = base.transform;
		for (;;)
		{
			bool isRight = Rand.Bool();
			this.catAnimator.SetBool("IsRight", isRight);
			catTransform.eulerAngles = Vector3.forward * this.catRotationRange.RandomFloat();
			this.catAnimator.SetTrigger("Peek");
			yield return null;
			yield return this.catAnimator.WaitForAnimationToEnd(this, true);
			yield return CupheadTime.WaitForSeconds(this, this.catDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x040019E0 RID: 6624
	public const string CatPeekParameterName = "Peek";

	// Token: 0x040019E1 RID: 6625
	public const string IsRightParameterName = "IsRight";

	// Token: 0x040019E2 RID: 6626
	[SerializeField]
	public Animator catAnimator;

	// Token: 0x040019E3 RID: 6627
	[SerializeField]
	public MinMax catDelay;

	// Token: 0x040019E4 RID: 6628
	[SerializeField]
	public MinMax catRotationRange;

	// Token: 0x040019E5 RID: 6629
	[Range(0f, 1f)]
	[SerializeField]
	public float peek1Threshold;

	// Token: 0x040019E6 RID: 6630
	[Range(0f, 1f)]
	[SerializeField]
	public float peek2Threshold;

	// Token: 0x040019E7 RID: 6631
	public bool isPhase2;

	// Token: 0x040019E8 RID: 6632
	public IEnumerator peekRoutine;
}
