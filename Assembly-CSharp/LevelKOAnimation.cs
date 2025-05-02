using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class LevelKOAnimation : AbstractLevelHUDComponent
{
	// Token: 0x06000D6E RID: 3438 RVA: 0x0000B7F5 File Offset: 0x000099F5
	public static LevelKOAnimation Create(bool isMaus)
	{
		LevelKOAnimation.isMausoleum = isMaus;
		return Object.Instantiate<LevelKOAnimation>(Level.Current.LevelResources.levelKO);
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0000B811 File Offset: 0x00009A11
	public override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0000B820 File Offset: 0x00009A20
	public void OnAnimComplete()
	{
		this.state = LevelKOAnimation.State.Complete;
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x00087254 File Offset: 0x00085454
	public IEnumerator anim_cr()
	{
		base.GetComponent<Animator>().SetTrigger(LevelKOAnimation.isMausoleum ? "StartMaus" : "Start");
		while (this.state == LevelKOAnimation.State.Animating)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000A78 RID: 2680
	public const float FRAME_DELAY = 5f;

	// Token: 0x04000A79 RID: 2681
	public LevelKOAnimation.State state;

	// Token: 0x04000A7A RID: 2682
	public static bool isMausoleum;

	// Token: 0x020009A0 RID: 2464
	public enum State
	{
		// Token: 0x040047B7 RID: 18359
		Animating,
		// Token: 0x040047B8 RID: 18360
		Complete
	}
}
