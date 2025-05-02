using System;

// Token: 0x020005B3 RID: 1459
public abstract class AbstractSwitch : AbstractCollidableObject
{
	// Token: 0x06003D34 RID: 15668 RVA: 0x00031638 File Offset: 0x0002F838
	public AbstractSwitch()
	{
	}

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x06003D35 RID: 15669 RVA: 0x00118788 File Offset: 0x00116988
	// (remove) Token: 0x06003D36 RID: 15670 RVA: 0x001187C0 File Offset: 0x001169C0
	public event Action OnActivate;

	// Token: 0x06003D37 RID: 15671 RVA: 0x00031640 File Offset: 0x0002F840
	public void DispatchEvent()
	{
		if (this.OnActivate != null)
		{
			this.OnActivate();
		}
	}
}
