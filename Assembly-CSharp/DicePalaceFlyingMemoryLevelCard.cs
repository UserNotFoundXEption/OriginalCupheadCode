using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class DicePalaceFlyingMemoryLevelCard : ParrySwitch
{
	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060016DD RID: 5853 RVA: 0x00013779 File Offset: 0x00011979
	// (set) Token: 0x060016DE RID: 5854 RVA: 0x00013781 File Offset: 0x00011981
	public bool flippedUp { get; set; }

	// Token: 0x060016DF RID: 5855 RVA: 0x0001378A File Offset: 0x0001198A
	public override void Awake()
	{
		base.Awake();
		this.flippedUp = false;
		this.flippedDownCard = base.GetComponent<SpriteRenderer>().sprite;
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x000A01F8 File Offset: 0x0009E3F8
	public void FlipUp()
	{
		base.StartCoroutine(this.rotate_cr(0f, 360f, 0.6f));
		this.flippedUpCard = this.flippedUpCards[(int)this.card];
		base.GetComponent<SpriteRenderer>().sprite = this.flippedUpCard;
		this.flippedUp = true;
		this.pinkDot.enabled = false;
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x000A0258 File Offset: 0x0009E458
	public void EnableCards()
	{
		if (!this.permanentlyFlipped)
		{
			if (this.flippedUp)
			{
				base.StartCoroutine(this.rotate_cr(0f, 360f, 0.6f));
				base.GetComponent<SpriteRenderer>().sprite = this.flippedDownCard;
				this.flippedUp = false;
			}
			this.pinkDot.enabled = true;
			base.StartCoroutine(this.fade_pink_cr(false));
			base.GetComponent<Collider2D>().enabled = true;
		}
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000137AA File Offset: 0x000119AA
	public void DisableCard()
	{
		base.GetComponent<Collider2D>().enabled = false;
		if (!this.flippedUp || !this.permanentlyFlipped)
		{
			base.StartCoroutine(this.fade_pink_cr(true));
		}
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x000A02D8 File Offset: 0x0009E4D8
	public IEnumerator rotate_cr(float start, float end, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(new float?(0f), new float?(EaseUtils.Ease(this.ROTATION_EASE, start, end, val)), new float?(0f));
			t += Time.deltaTime;
			yield return null;
		}
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		yield return null;
		yield break;
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000137DC File Offset: 0x000119DC
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.FlipUp();
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x000A0308 File Offset: 0x0009E508
	public IEnumerator fade_pink_cr(bool fadingOut)
	{
		if (fadingOut)
		{
			float t = 0f;
			while (t < this.fadeTime)
			{
				this.pinkDot.color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.pinkDot.color = new Color(1f, 1f, 1f, 0f);
		}
		else
		{
			float t2 = 0f;
			while (t2 < this.fadeTime)
			{
				this.pinkDot.color = new Color(1f, 1f, 1f, t2 / this.fadeTime);
				t2 += CupheadTime.Delta;
				yield return null;
			}
			this.pinkDot.color = new Color(1f, 1f, 1f, 1f);
		}
		yield break;
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x000137EB File Offset: 0x000119EB
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.flippedUpCards = null;
		this.flippedUpCard = null;
		this.flippedDownCard = null;
	}

	// Token: 0x04001297 RID: 4759
	public bool permanentlyFlipped;

	// Token: 0x04001298 RID: 4760
	public const float ROTATION_TIME = 0.6f;

	// Token: 0x04001299 RID: 4761
	public const float ROTATION_BACK = 360f;

	// Token: 0x0400129A RID: 4762
	public EaseUtils.EaseType ROTATION_EASE = EaseUtils.EaseType.easeOutBack;

	// Token: 0x0400129B RID: 4763
	[SerializeField]
	public Sprite[] flippedUpCards;

	// Token: 0x0400129C RID: 4764
	[SerializeField]
	public SpriteRenderer pinkDot;

	// Token: 0x0400129D RID: 4765
	public Sprite flippedUpCard;

	// Token: 0x0400129E RID: 4766
	public Sprite flippedDownCard;

	// Token: 0x0400129F RID: 4767
	public Coroutine rotationCoroutine;

	// Token: 0x040012A0 RID: 4768
	public float fadeTime = 0.7f;

	// Token: 0x040012A1 RID: 4769
	public DicePalaceFlyingMemoryLevelCard.Card card;

	// Token: 0x02000BA7 RID: 2983
	public enum Card
	{
		// Token: 0x0400550C RID: 21772
		Cuphead,
		// Token: 0x0400550D RID: 21773
		Chips,
		// Token: 0x0400550E RID: 21774
		Flowers,
		// Token: 0x0400550F RID: 21775
		Shield,
		// Token: 0x04005510 RID: 21776
		Spindle,
		// Token: 0x04005511 RID: 21777
		Mugman
	}
}
