using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class MouseLevelBackgroundHopper : AbstractMonoBehaviour
{
	// Token: 0x06001F45 RID: 8005 RVA: 0x0001A54C File Offset: 0x0001874C
	public override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.localPosition;
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x0001A56A File Offset: 0x0001876A
	public void Start()
	{
		CupheadLevelCamera.Current.OnShakeEvent += this.OnShake;
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x0001A582 File Offset: 0x00018782
	public void OnDestroy()
	{
		if (CupheadLevelCamera.Current != null)
		{
			this.RemoveShake();
		}
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x0001A59A File Offset: 0x0001879A
	public void RemoveShake()
	{
		CupheadLevelCamera.Current.OnShakeEvent -= this.OnShake;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x0001A5B2 File Offset: 0x000187B2
	public void OnShake(float amount, float time)
	{
		if (this.hopCoroutine != null)
		{
			base.StopCoroutine(this.hopCoroutine);
		}
		this.hopCoroutine = base.StartCoroutine(this.hop_cr(amount));
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000B5B60 File Offset: 0x000B3D60
	public IEnumerator hop_cr(float amount)
	{
		for (int i = 0; i < this.hops.Length; i++)
		{
			float height = this.hops[i].height;
			float time = this.hops[i].time;
			Vector2 endPos = this.startPos + new Vector2(0f, height);
			float ht = time / 2f;
			yield return base.StartCoroutine(this.tween_cr(this.startPos.y, endPos.y, ht, EaseUtils.EaseType.easeOutSine));
			yield return base.StartCoroutine(this.tween_cr(endPos.y, this.startPos.y, ht, EaseUtils.EaseType.easeInSine));
			time /= 2f;
			height /= 2f;
		}
		yield break;
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000B5B7C File Offset: 0x000B3D7C
	public IEnumerator tween_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(start), new float?(0f));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(EaseUtils.Ease(ease, start, end, val)), new float?(0f));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(end), new float?(0f));
		yield break;
	}

	// Token: 0x04001980 RID: 6528
	public MouseLevelBackgroundHopper.Hop[] hops = new MouseLevelBackgroundHopper.Hop[1];

	// Token: 0x04001981 RID: 6529
	public Coroutine hopCoroutine;

	// Token: 0x04001982 RID: 6530
	public Vector2 startPos;

	// Token: 0x02000D9E RID: 3486
	[Serializable]
	public class Hop
	{
		// Token: 0x0400627B RID: 25211
		public float height = 50f;

		// Token: 0x0400627C RID: 25212
		public float time = 0.25f;
	}
}
