using System;

// Token: 0x020004EE RID: 1262
public class OnlineManager
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06003430 RID: 13360 RVA: 0x0002AE0A File Offset: 0x0002900A
	public static OnlineManager Instance
	{
		get
		{
			if (OnlineManager.instance == null)
			{
				OnlineManager.instance = new OnlineManager();
			}
			return OnlineManager.instance;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06003431 RID: 13361 RVA: 0x0002AE25 File Offset: 0x00029025
	// (set) Token: 0x06003432 RID: 13362 RVA: 0x0002AE2D File Offset: 0x0002902D
	public OnlineInterface Interface { get; set; }

	// Token: 0x06003433 RID: 13363 RVA: 0x0002AE36 File Offset: 0x00029036
	public void Init()
	{
		this.Interface = new OnlineInterfaceSteam();
		this.Interface.Init();
	}

	// Token: 0x04002AF7 RID: 10999
	public static OnlineManager instance;
}
