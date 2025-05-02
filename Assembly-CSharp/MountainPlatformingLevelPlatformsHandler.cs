using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class MountainPlatformingLevelPlatformsHandler : AbstractPausableComponent
{
	// Token: 0x06002F1B RID: 12059 RVA: 0x000E0CB4 File Offset: 0x000DEEB4
	public void Start()
	{
		this.platforms = new Transform[this.platformHolder.GetComponentsInChildren<Transform>().Length];
		this.platforms = this.platformHolder.GetComponentsInChildren<Transform>();
		this.platformsStartPos = new Vector3[this.platformHolder.GetComponentsInChildren<Transform>().Length];
		for (int i = 0; i < this.platforms.Length; i++)
		{
			this.platformsStartPos[i] = this.platforms[i].position;
		}
		for (int j = 0; j < this.platforms.Length; j++)
		{
			this.OffScreen(this.platforms[j]);
		}
		this.parrySwitch.OnActivate += this.MovePlatforms;
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x000273E9 File Offset: 0x000255E9
	public void MovePlatforms()
	{
		if (!this.hasSwitched)
		{
			base.StartCoroutine(this.moving_cr());
			this.hasSwitched = true;
		}
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x000E0D78 File Offset: 0x000DEF78
	public IEnumerator moving_cr()
	{
		for (int i = 0; i < this.platforms.Length; i++)
		{
			base.StartCoroutine(this.move_platform_cr(this.platforms[i], this.platformsStartPos[i]));
			yield return CupheadTime.WaitForSeconds(this, this.platformAppearDelay);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x0002740A File Offset: 0x0002560A
	public void OffScreen(Transform platform)
	{
		platform.transform.position += Vector3.down * this.lowerAmount;
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x000E0D94 File Offset: 0x000DEF94
	public IEnumerator move_platform_cr(Transform platform, Vector3 startPos)
	{
		float t = 0f;
		float time = this.platformMoveTime;
		Vector2 start = platform.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			platform.transform.position = Vector2.Lerp(start, startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		platform.transform.position = startPos;
		yield return null;
		yield break;
	}

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	public Transform platformHolder;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	public ParrySwitch parrySwitch;

	// Token: 0x04002713 RID: 10003
	[SerializeField]
	public float platformMoveTime;

	// Token: 0x04002714 RID: 10004
	[SerializeField]
	public float platformAppearDelay;

	// Token: 0x04002715 RID: 10005
	public bool hasSwitched;

	// Token: 0x04002716 RID: 10006
	public Transform[] platforms;

	// Token: 0x04002717 RID: 10007
	public Vector3[] platformsStartPos;

	// Token: 0x04002718 RID: 10008
	public float lowerAmount = 1000f;
}
