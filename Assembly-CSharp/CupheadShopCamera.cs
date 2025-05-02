using System;

// Token: 0x020000A0 RID: 160
public class CupheadShopCamera : AbstractCupheadGameCamera
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000079C3 File Offset: 0x00005BC3
	// (set) Token: 0x060007D7 RID: 2007 RVA: 0x000079CA File Offset: 0x00005BCA
	public static CupheadShopCamera Current { get; set; }

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000079D2 File Offset: 0x00005BD2
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x000079D9 File Offset: 0x00005BD9
	public override void Awake()
	{
		base.Awake();
		CupheadShopCamera.Current = this;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x000079E7 File Offset: 0x00005BE7
	public void OnDestroy()
	{
		if (CupheadShopCamera.Current == this)
		{
			CupheadShopCamera.Current = null;
		}
	}
}
