using System;

// Token: 0x0200057A RID: 1402
public abstract class PlayerEvent<T> : GameEvent where T : PlayerEvent<T>, new()
{
	// Token: 0x06003AC7 RID: 15047 RVA: 0x0002FC80 File Offset: 0x0002DE80
	public PlayerEvent()
	{
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x0002FC88 File Offset: 0x0002DE88
	// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x0002FC90 File Offset: 0x0002DE90
	public PlayerId playerId { get; set; }

	// Token: 0x06003ACA RID: 15050 RVA: 0x0002FC99 File Offset: 0x0002DE99
	public static T Shared(PlayerId playerId)
	{
		if (PlayerEvent<T>._instance == null)
		{
			PlayerEvent<T>._instance = Activator.CreateInstance<T>();
		}
		PlayerEvent<T>._instance.playerId = playerId;
		return PlayerEvent<T>._instance;
	}

	// Token: 0x04002F1D RID: 12061
	public static T _instance;
}
