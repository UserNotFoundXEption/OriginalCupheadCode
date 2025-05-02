using System;

// Token: 0x02000582 RID: 1410
public class DummyPlmInterface : PlmInterface
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06003B7F RID: 15231 RVA: 0x00113D0C File Offset: 0x00111F0C
	// (remove) Token: 0x06003B80 RID: 15232 RVA: 0x00113D44 File Offset: 0x00111F44
	public event OnSuspendHandler OnSuspend;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06003B81 RID: 15233 RVA: 0x00113D7C File Offset: 0x00111F7C
	// (remove) Token: 0x06003B82 RID: 15234 RVA: 0x00113DB4 File Offset: 0x00111FB4
	public event OnResumeHandler OnResume;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06003B83 RID: 15235 RVA: 0x00113DEC File Offset: 0x00111FEC
	// (remove) Token: 0x06003B84 RID: 15236 RVA: 0x00113E24 File Offset: 0x00112024
	public event OnConstrainedHandler OnConstrained;

	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x06003B85 RID: 15237 RVA: 0x00113E5C File Offset: 0x0011205C
	// (remove) Token: 0x06003B86 RID: 15238 RVA: 0x00113E94 File Offset: 0x00112094
	public event OnUnconstrainedHandler OnUnconstrained;

	// Token: 0x06003B87 RID: 15239 RVA: 0x00030548 File Offset: 0x0002E748
	public void Init()
	{
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0003054A File Offset: 0x0002E74A
	public bool IsConstrained()
	{
		return false;
	}
}
