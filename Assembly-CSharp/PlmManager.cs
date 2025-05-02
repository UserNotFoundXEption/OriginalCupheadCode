using System;

// Token: 0x02000588 RID: 1416
public class PlmManager
{
	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x00030555 File Offset: 0x0002E755
	public static PlmManager Instance
	{
		get
		{
			if (PlmManager.instance == null)
			{
				PlmManager.instance = new PlmManager();
			}
			return PlmManager.instance;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x00030570 File Offset: 0x0002E770
	// (set) Token: 0x06003BA6 RID: 15270 RVA: 0x00030578 File Offset: 0x0002E778
	public PlmInterface Interface { get; set; }

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00113ECC File Offset: 0x001120CC
	public void Init()
	{
		this.Interface = new DummyPlmInterface();
		this.Interface.Init();
		this.Interface.OnSuspend += this.OnSuspend;
		this.Interface.OnResume += this.OnResume;
		this.Interface.OnConstrained += this.OnConstrained;
		this.Interface.OnUnconstrained += this.OnUnconstrained;
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x00030581 File Offset: 0x0002E781
	public void OnSuspend()
	{
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00030583 File Offset: 0x0002E783
	public void OnResume()
	{
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00030585 File Offset: 0x0002E785
	public void OnConstrained()
	{
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00030587 File Offset: 0x0002E787
	public void OnUnconstrained()
	{
	}

	// Token: 0x04002F79 RID: 12153
	public static PlmManager instance;
}
