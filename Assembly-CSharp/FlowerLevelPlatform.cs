using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class FlowerLevelPlatform : LevelPlatform
{
	// Token: 0x06001930 RID: 6448 RVA: 0x000A5C4C File Offset: 0x000A3E4C
	public void Start()
	{
		this.YPositionDown = this.YPositionUp - 30f;
		this.YFall = this.YPositionUp - 35f;
		if (this.shadow != null)
		{
			this.shadow.parent = null;
			Vector3 position = this.shadow.position;
			position.y = (float)Level.Current.Ground;
			this.shadow.position = position;
		}
		this.startPos = base.transform.position;
		this.startPos.y = this.YPositionUp;
		this.endPos = base.transform.position;
		this.endPos.y = this.YPositionDown;
		if (this.state == FlowerLevelPlatform.State.Down)
		{
			base.transform.SetPosition(null, new float?(this.YPositionUp), null);
			this.StartDown();
		}
		else
		{
			base.transform.SetPosition(null, new float?(this.YPositionDown), null);
			this.StartUp();
		}
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x00015804 File Offset: 0x00013A04
	public void StartDown()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.down_cr());
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00015819 File Offset: 0x00013A19
	public void StartUp()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.up_cr());
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0001582E File Offset: 0x00013A2E
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		this.StopAllCoroutines();
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0001584A File Offset: 0x00013A4A
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		this.StartUp();
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000A5D78 File Offset: 0x000A3F78
	public IEnumerator down_cr()
	{
		yield return new WaitForSeconds(0f);
		yield return base.StartCoroutine(this.goTo_cr(this.YPositionUp, this.YPositionDown, 3f, EaseUtils.EaseType.easeInOutSine));
		this.StartUp();
		yield break;
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000A5D94 File Offset: 0x000A3F94
	public IEnumerator up_cr()
	{
		yield return new WaitForSeconds(0f);
		yield return base.StartCoroutine(this.goTo_cr(this.YPositionDown, this.YPositionUp, 3f, EaseUtils.EaseType.easeInOutSine));
		this.StartDown();
		yield break;
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000A5DB0 File Offset: 0x000A3FB0
	public IEnumerator fall_cr()
	{
		float time = (1f - base.transform.position.y / this.YPositionDown) * 0.13f;
		yield return base.StartCoroutine(this.goTo_cr(base.transform.position.y, this.YFall, time, EaseUtils.EaseType.easeOutSine));
		yield return base.StartCoroutine(this.goTo_cr(this.YFall, this.YPositionDown, 0.12f, EaseUtils.EaseType.easeInOutSine));
		yield break;
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000A5DCC File Offset: 0x000A3FCC
	public IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x0400144D RID: 5197
	public float YPositionUp;

	// Token: 0x0400144E RID: 5198
	public const float TIME = 3f;

	// Token: 0x0400144F RID: 5199
	public const float FALL_TIME = 0.13f;

	// Token: 0x04001450 RID: 5200
	public const float FALL_BOUNCE_TIME = 0.12f;

	// Token: 0x04001451 RID: 5201
	public const float DELAY = 0f;

	// Token: 0x04001452 RID: 5202
	public const EaseUtils.EaseType FLOAT_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x04001453 RID: 5203
	public const EaseUtils.EaseType FALL_EASE = EaseUtils.EaseType.easeOutSine;

	// Token: 0x04001454 RID: 5204
	public const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x04001455 RID: 5205
	[SerializeField]
	public FlowerLevelPlatform.State state;

	// Token: 0x04001456 RID: 5206
	[SerializeField]
	public Transform shadow;

	// Token: 0x04001457 RID: 5207
	public Vector3 startPos;

	// Token: 0x04001458 RID: 5208
	public Vector3 endPos;

	// Token: 0x04001459 RID: 5209
	public float YPositionDown;

	// Token: 0x0400145A RID: 5210
	public float YFall;

	// Token: 0x02000C29 RID: 3113
	public enum State
	{
		// Token: 0x04005833 RID: 22579
		Up,
		// Token: 0x04005834 RID: 22580
		Down,
		// Token: 0x04005835 RID: 22581
		PlayerOn
	}
}
