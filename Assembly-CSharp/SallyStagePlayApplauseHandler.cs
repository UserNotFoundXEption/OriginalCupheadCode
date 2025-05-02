using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000354 RID: 852
public class SallyStagePlayApplauseHandler : AbstractPausableComponent
{
	// Token: 0x06002598 RID: 9624 RVA: 0x000C71FC File Offset: 0x000C53FC
	public void Start()
	{
		this.handsStartPos = new Vector3[this.hands.Length];
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.hands[i].GetComponent<Animator>().Play((!Rand.Bool()) ? "B" : "A");
			this.handsStartPos[i] = this.hands[i].transform.position;
		}
		this.pinkPattern = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = Random.Range(0, this.pinkPattern.Length);
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x0001FA30 File Offset: 0x0001DC30
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(base.transform.position, this.endPos.transform.position);
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x000C72B4 File Offset: 0x000C54B4
	public void SlideApplause(bool slideIn)
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			base.StartCoroutine(this.slide_cr(this.hands[i], this.handsStartPos[i], slideIn, Random.Range(0.3f, 0.8f)));
			AudioManager.Play("sally_audience_applause");
		}
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x000C731C File Offset: 0x000C551C
	public IEnumerator slide_cr(Transform hand, Vector3 handStart, bool slideIn, float delay)
	{
		Vector3 start = (!slideIn) ? new Vector3(hand.transform.position.x, this.endPos.position.y) : handStart;
		Vector3 end = (!slideIn) ? handStart : new Vector3(hand.transform.position.x, this.endPos.position.y);
		float t = 0f;
		float frameTime = 0f;
		float time = 0.6f;
		yield return CupheadTime.WaitForSeconds(this, delay);
		while (t < time)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				frameTime -= 0.0416666679f;
				hand.transform.position = Vector3.Lerp(start, end, t / time);
			}
			yield return null;
		}
		hand.transform.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x0001FA58 File Offset: 0x0001DC58
	public void ThrowRose(Vector3 pos, LevelProperties.SallyStagePlay.Roses properties)
	{
		base.StartCoroutine(this.throw_rose_cr(pos, properties, this.roseHands[Random.Range(0, this.roseHands.Length)]));
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x000C7354 File Offset: 0x000C5554
	public IEnumerator throw_rose_cr(Vector3 pos, LevelProperties.SallyStagePlay.Roses properties, Transform arm)
	{
		string animationName;
		arm.GetComponent<Animator>().Play(animationName = ((!Rand.Bool()) ? "Rose_2_A" : "Rose_1_A"));
		arm.transform.SetPosition(new float?(pos.x), null, null);
		float speed = 900f;
		yield return arm.GetComponent<Animator>().WaitForAnimationToEnd(this, animationName, false, true);
		this.roseStill.transform.position = new Vector3(arm.transform.position.x, arm.transform.position.y + 50f);
		while (this.roseStill.transform.position.y < (float)Level.Current.Ceiling + 100f)
		{
			this.roseStill.transform.position += Vector3.up * speed * CupheadTime.Delta;
			yield return null;
		}
		SallyStagePlayLevelRose r = this.rose.Create(pos, properties);
		r.SetParryable(this.pinkPattern[this.pinkIndex][0] == 'P');
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		yield return null;
		yield break;
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x0001FA7E File Offset: 0x0001DC7E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.rose = null;
	}

	// Token: 0x04001F1E RID: 7966
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04001F1F RID: 7967
	[SerializeField]
	public SallyStagePlayLevelRose rose;

	// Token: 0x04001F20 RID: 7968
	[SerializeField]
	public Transform[] hands;

	// Token: 0x04001F21 RID: 7969
	public Vector3[] handsStartPos;

	// Token: 0x04001F22 RID: 7970
	[SerializeField]
	public Transform[] roseHands;

	// Token: 0x04001F23 RID: 7971
	[SerializeField]
	public Transform roseStill;

	// Token: 0x04001F24 RID: 7972
	[SerializeField]
	public Transform endPos;

	// Token: 0x04001F25 RID: 7973
	[SerializeField]
	public string pinkString;

	// Token: 0x04001F26 RID: 7974
	public string[] pinkPattern;

	// Token: 0x04001F27 RID: 7975
	public int pinkIndex;
}
