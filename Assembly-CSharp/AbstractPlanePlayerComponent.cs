using System;

// Token: 0x0200055C RID: 1372
public abstract class AbstractPlanePlayerComponent : AbstractPlayerComponent
{
	// Token: 0x0600394D RID: 14669 RVA: 0x0002EAF6 File Offset: 0x0002CCF6
	public AbstractPlanePlayerComponent()
	{
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x0002EAFE File Offset: 0x0002CCFE
	// (set) Token: 0x0600394F RID: 14671 RVA: 0x0002EB06 File Offset: 0x0002CD06
	public PlanePlayerController player { get; set; }

	// Token: 0x06003950 RID: 14672 RVA: 0x0002EB0F File Offset: 0x0002CD0F
	public override void OnAwake()
	{
		base.OnAwake();
		this.player = base.GetComponent<PlanePlayerController>();
	}
}
