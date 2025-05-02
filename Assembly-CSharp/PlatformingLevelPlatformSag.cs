using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class PlatformingLevelPlatformSag : LevelPlatform
{
	// Token: 0x06002FD0 RID: 12240 RVA: 0x000E2BC0 File Offset: 0x000E0DC0
	public void Start()
	{
		this.localPosY = base.transform.localPosition.y;
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x00027D38 File Offset: 0x00025F38
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.fall_cr());
		}
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x00027D5E File Offset: 0x00025F5E
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.go_up_cr());
		}
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x000E2BE8 File Offset: 0x000E0DE8
	public IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetLocalPosition(null, new float?(start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetLocalPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x000E2C20 File Offset: 0x000E0E20
	public IEnumerator fall_cr()
	{
		yield return base.StartCoroutine(this.goTo_cr(base.transform.localPosition.y, this.localPosY - this.sagAmount, 0.4f, EaseUtils.EaseType.easeOutBounce));
		yield break;
	}

	// Token: 0x06002FD5 RID: 12245 RVA: 0x000E2C3C File Offset: 0x000E0E3C
	public IEnumerator go_up_cr()
	{
		yield return base.StartCoroutine(this.goTo_cr(base.transform.localPosition.y, this.localPosY, 0.6f, EaseUtils.EaseType.easeOutBounce));
		yield break;
	}

	// Token: 0x04002798 RID: 10136
	[SerializeField]
	public float sagAmount = 30f;

	// Token: 0x04002799 RID: 10137
	public const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeOutBounce;

	// Token: 0x0400279A RID: 10138
	public const float FALL_TIME = 0.4f;

	// Token: 0x0400279B RID: 10139
	public const float RISE_TIME = 0.6f;

	// Token: 0x0400279C RID: 10140
	public float localPosY;
}
