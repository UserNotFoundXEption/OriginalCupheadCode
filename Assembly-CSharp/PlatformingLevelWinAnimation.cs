using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class PlatformingLevelWinAnimation : AbstractLevelHUDComponent
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06002FDE RID: 12254 RVA: 0x00027E17 File Offset: 0x00026017
	// (set) Token: 0x06002FDF RID: 12255 RVA: 0x00027E1F File Offset: 0x0002601F
	public PlatformingLevelWinAnimation.State CurrentState { get; set; }

	// Token: 0x06002FE0 RID: 12256 RVA: 0x00027E28 File Offset: 0x00026028
	public static PlatformingLevelWinAnimation Create()
	{
		return Object.Instantiate<PlatformingLevelWinAnimation>(Level.Current.LevelResources.platformingWin);
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x00027E3E File Offset: 0x0002603E
	public override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x00027E4D File Offset: 0x0002604D
	public void OnAnimComplete()
	{
		this.CurrentState = PlatformingLevelWinAnimation.State.Complete;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040027A0 RID: 10144
	public const float FRAME_DELAY = 5f;

	// Token: 0x020010DF RID: 4319
	public enum State
	{
		// Token: 0x04007789 RID: 30601
		Paused,
		// Token: 0x0400778A RID: 30602
		Unpaused,
		// Token: 0x0400778B RID: 30603
		Complete
	}
}
