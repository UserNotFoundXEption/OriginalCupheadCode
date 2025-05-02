using System;

// Token: 0x0200055B RID: 1371
public class ParryAttackSpark : Effect
{
	// Token: 0x1700047F RID: 1151
	// (set) Token: 0x0600394C RID: 14668 RVA: 0x0002EAE3 File Offset: 0x0002CCE3
	public bool IsCuphead
	{
		set
		{
			base.animator.SetBool("IsCuphead", value);
		}
	}
}
