using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class RumRunnersLevelMobIntroAnimation : MonoBehaviour
{
	// Token: 0x1700030C RID: 780
	// (get) Token: 0x060024EB RID: 9451 RVA: 0x0001F27F File Offset: 0x0001D47F
	// (set) Token: 0x060024EC RID: 9452 RVA: 0x0001F287 File Offset: 0x0001D487
	public float bugGirlDamage { get; set; }

	// Token: 0x060024ED RID: 9453 RVA: 0x0001F290 File Offset: 0x0001D490
	public void Start()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.grub.SetActive(false);
		}
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x000C5584 File Offset: 0x000C3784
	public IEnumerator bugWalk()
	{
		float walkSpeed = this.bugGirlWalkDistance / this.bugGirlWalkDuration;
		for (;;)
		{
			yield return null;
			this.bugGirlTransform.position = this.bugGirlTransform.position + new Vector3(walkSpeed * CupheadTime.Delta, 0f);
		}
		yield break;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000C55A0 File Offset: 0x000C37A0
	public IEnumerator timeout_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevelMobIntroAnimation.IntroTimeoutDuration);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x0001F2AD File Offset: 0x0001D4AD
	public void StartBugWalk()
	{
		this.bugWalkCoroutine = base.StartCoroutine(this.bugWalk());
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x0001F2C1 File Offset: 0x0001D4C1
	public void StopBugWalk()
	{
		base.StopCoroutine(this.bugWalkCoroutine);
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x000C55BC File Offset: 0x000C37BC
	public void BarrelExit()
	{
		this.barrelAnimator.SetTrigger("Exit");
		SpriteRenderer component = this.barrelAnimator.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Foreground";
		component.sortingOrder = 100;
		base.StartCoroutine(this.timeout_cr());
	}

	// Token: 0x04001E91 RID: 7825
	public static readonly float IntroTimeoutDuration = 2f;

	// Token: 0x04001E92 RID: 7826
	[SerializeField]
	public Transform bugGirlTransform;

	// Token: 0x04001E93 RID: 7827
	[SerializeField]
	public float bugGirlWalkDistance;

	// Token: 0x04001E94 RID: 7828
	[SerializeField]
	public float bugGirlWalkDuration;

	// Token: 0x04001E95 RID: 7829
	[SerializeField]
	public Animator barrelAnimator;

	// Token: 0x04001E96 RID: 7830
	[SerializeField]
	public GameObject grub;

	// Token: 0x04001E98 RID: 7832
	public Coroutine bugWalkCoroutine;
}
